Public Class Parser
    Private _scanner As Scanner
    Private tok As Token
    Private _variablenames As List(Of String)
    Private _variabletypes As List(Of Type)
    'Public Event RaiseError(ByVal Message As String)
    Public Event TokenFound(ByVal idpos As Integer, ByVal tok As TokenEnum, ByVal text As String, ByVal helpdoc As String)
    Public Sub New(scanner As Scanner)
        _scanner = scanner
        AddHandler _scanner.TokenFound, AddressOf RaiseTokenFound
        AddHandler _scanner.ErrorFound, AddressOf AddScanErrors
    End Sub
    Private Sub AddScanErrors(ByVal message As String)
        Dim r As New ErrorNode(TypeEnum.ERR, message, "")
        r.AddParseError(message)
    End Sub
    Public Function Parse(ByVal AvailableVariableNames As List(Of String), ByVal availableVariableTypes As List(Of Type), ByVal expectedoutputType As TypeEnum) As ParseTreeNode
        ParseTreeNode.Initialize()
        Dim ret As ParseTreeNode
        Scan()
        _variablenames = AvailableVariableNames
        _variabletypes = availableVariableTypes
        ret = ParseATreeNode()
        If expectedoutputType = TypeEnum.UnDeclared Then
            Return ret
        Else
            If ret Is Nothing Then
                ret = New ErrorNode(expectedoutputType, "The Parsed statement yeilded a result of nothing", "")
                ret.AddParseError("The Parsed statement yeilded a result of nothing")
                Return ret
            End If
            If (ret.Type And expectedoutputType) = 0 Then
                If ret.Type = TypeEnum.Int AndAlso (expectedoutputType And TypeEnum.Deciml) > 0 Then
                    'ok
                    Return ret
                End If
                If ret.Type = TypeEnum.Int AndAlso expectedoutputType = TypeEnum.Shrt Then
                    'also ok
                    Return ret
                End If
                If (ret.Type And TypeEnum.Deciml) > 0 AndAlso (expectedoutputType And TypeEnum.integral) > 0 Then
                    'not ok, roudning
                    ret.AddParseError("The expression could produce a non integer result, and the output destination is expected to be a whole number, it is recommended you change your expression to Round(expression)")
                    Return ret
                End If
                ret.AddParseError("The parsed statement returns a value of type " & ret.Type.ToString & ", the output of the equation needs to be of type " & expectedoutputType.ToString)
                ret = New ErrorNode(expectedoutputType, "The parsed statement returns a value of type " & ret.Type.ToString & ", the output of the equation needs to be of type " & expectedoutputType.ToString, "Error")
                Return ret
            Else
                Return ret
            End If
        End If
    End Function
    Private Sub RaiseTokenFound(ByVal idpos As Integer, ByVal tok As TokenEnum, ByVal text As String, ByVal helpdoc As String)
        RaiseEvent TokenFound(idpos, tok, text, helpdoc)
    End Sub
    Private Function ParseATreeNode(ByVal lefthandside As ParseTreeNode) As ParseTreeNode
        Dim exitloop As Boolean = False
        Select Case tok.tok
            Case TokenEnum.ConvertToBoolean
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                lefthandside = New ConvertToBoolean(parsetreenode)
            Case TokenEnum.ConvertToByte
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                lefthandside = New ConvertToByte(parsetreenode)
            Case TokenEnum.ConvertToDouble
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                lefthandside = New ConvertToDouble(parsetreenode)
            Case TokenEnum.ConvertToInteger
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                lefthandside = New ConvertToInteger(parsetreenode)
            Case TokenEnum.ConvertToShort
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                lefthandside = New ConvertToShort(parsetreenode)
            Case TokenEnum.ConvertToString
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                lefthandside = New ConvertToString(parsetreenode)
            Case TokenEnum.CRBRACKET
                lefthandside = lefthandside
                Scan()
                exitloop = True
            Case TokenEnum.INDEX
                lefthandside = New IndexNumNode(tok.str)
                Scan()
                'lefthandside = NumFactor(lefthandside)
            Case TokenEnum.KIF
                lefthandside = FactorIf()
            Case TokenEnum.KOR
                Scan()
                lefthandside = FactorOR()
            Case TokenEnum.BOOLLIT
                lefthandside = New BoolNode(tok.str)
                Scan()
                'exitloop = True
            Case TokenEnum.KAND
                Scan()
                lefthandside = FactorAnd()
            Case TokenEnum.StringLIT
                lefthandside = stringparse(New StringNode(tok.str))
            Case TokenEnum.INSTRING
                lefthandside = factorInstring()
            Case TokenEnum.SUBSTRING
                lefthandside = FactorSubstring()
            Case TokenEnum.RIGHT
                lefthandside = FactorStringRight()
            Case TokenEnum.LEFT
                lefthandside = FactorStringLeft()
            Case TokenEnum.LEN
                lefthandside = factorLEN()
            Case TokenEnum.LE
                lefthandside = BoolExpr(lefthandside)
            Case TokenEnum.EQ
                lefthandside = BoolExpr(lefthandside)
            Case TokenEnum.GE
                lefthandside = BoolExpr(lefthandside)
            Case TokenEnum.GT
                lefthandside = BoolExpr(lefthandside)
            Case TokenEnum.LT
                lefthandside = BoolExpr(lefthandside)
            Case TokenEnum.NEQ
                lefthandside = BoolExpr(lefthandside)
            Case TokenEnum.CONTAINS
                lefthandside = FactorContains()
            Case TokenEnum.RPAREN
                lefthandside = lefthandside
                exitloop = True
            Case TokenEnum.PLUS
                lefthandside = NumExpr(lefthandside)
            Case TokenEnum.MINUS
                lefthandside = NumExpr(lefthandside)
            Case TokenEnum.TIMES
                lefthandside = NumTerm(lefthandside)
            Case TokenEnum.EXPONENT
                lefthandside = NumTerm(lefthandside)
            Case TokenEnum.DIVIDE
                lefthandside = NumTerm(lefthandside)
            Case TokenEnum.NUMLIT
                lefthandside = New NumNode(Double.Parse(tok.str))
                Scan()
            Case TokenEnum.NUMLITINT
                lefthandside = New NumIntNode(Integer.Parse(tok.str))
                Scan()
            Case TokenEnum.LPAREN
                Scan()
                lefthandside = ParenteticalStatement(lefthandside)
            Case TokenEnum.HEADER
                If _variablenames.Contains(tok.str) Then
                    lefthandside = headerparse(_variabletypes(_variablenames.IndexOf(tok.str)))
                Else
                    If IsNothing(lefthandside) Then Return New ErrorNode(TypeEnum.ERR, "The text " & Me.tok.str & " was found inside of brackets, but the avaialble headers do not match case or spelling.", Me.tok.str)
                    lefthandside.AddParseError("The text " & Me.tok.str & " was found inside of brackets, but the avaialble headers do not match case or spelling." & Me.tok.str)
                    Scan()
                    Return lefthandside
                End If

            Case TokenEnum.OFFSET
                lefthandside = offsetparse()
            Case TokenEnum.COMMA
                Return lefthandside
            Case TokenEnum.RAND
                lefthandside = FactorRand()
            Case TokenEnum.RANDBETWEEN
                lefthandside = FactorRandBetween()
            Case TokenEnum.RANDINT
                lefthandside = FactorRandINT()
            Case TokenEnum.RANDINTBETWEEN
                lefthandside = FactorRandINTBetween()
            Case TokenEnum.ROUND
                lefthandside = FactorRound()
            Case TokenEnum.ROUNDDOWN
                lefthandside = FactorRoundDown()
            Case TokenEnum.ROUNDUP
                lefthandside = FactorRoundUp()
            Case TokenEnum.NORMINV
                lefthandside = FactorNormINV()
            Case TokenEnum.TRIINV
                lefthandside = FactorTriINV()
            Case TokenEnum.ANDPERSTAND
                Scan()
                lefthandside = New ConcatenateExprNode(lefthandside, StringFactor())
            Case TokenEnum.EOF
                Return lefthandside
            Case TokenEnum.ERR
                If Me.tok.str = "The scanner was called after the end of the file was found, some expression must not be complete" Then
                    exitloop = True
                    Return lefthandside
                Else
                    Return New ErrorNode(TypeEnum.ERR, Me.tok.str, Me.tok.str)
                End If
            Case Else
                exitloop = True
                If IsNothing(lefthandside) Then Return New ErrorNode(TypeEnum.ERR, "encountered a token that isnt recognized, and there was no lefthand side of the expression.", Me.tok.str)
                lefthandside.AddParseError("encountered a token that isnt recognized from parseatreenode(byval treenode) " & Me.tok.str)
                Return lefthandside
        End Select
        If exitloop = True Then
            Return lefthandside
        Else
            Return ParseATreeNode(lefthandside)
        End If
    End Function
    Private Function ParseATreeNode() As ParseTreeNode
        Scan()
        Dim ret As ParseTreeNode = Nothing
        Return ParseATreeNode(ret)
    End Function
    Private Function ParenteticalStatement(term As ParseTreeNode) As ParseTreeNode
        Do Until IsTok(TokenEnum.RPAREN)
            term = ParseATreeNode(term)
            If IsTok(TokenEnum.COMMA) Then
                term.AddParseError("Found a comma in a parenthetical statment, this shouldnt happen")
                Return New ErrorNode(TypeEnum.UnDeclared, "Found a comma in a parenthetical statment, this shouldnt happen", term.ExpressionToString)
            End If
            If IsTok(TokenEnum.EOF) Then
                Expected("Expected parenthetical expression but got ")
                If IsNothing(term) Then
                    Return New ErrorNode(TypeEnum.UnDeclared, "Expected a right parenthesis, but encountered the end of the expression", "")
                Else
                    Return New ErrorNode(TypeEnum.UnDeclared, "Expected a right parenthesis, but encountered a " & tok.ToString, term.ExpressionToString)
                End If

            End If
        Loop
        Scan()
        Return term
    End Function
    Private Function FactorNormINV() As ParseTreeNode
        Scan() 'remove token.enum.norminv
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim rand As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'move on from the comma
        Dim mean As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'move on from the comma
        Dim stdev As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the rparen..
        Return New NormalDistExprNode(rand, mean, stdev)
    End Function
    Private Function FactorTriINV() As ParseTreeNode
        Scan() 'remove token.enum.triinv
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim rand As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'move on from the comma
        Dim min As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'move on from the comma
        Dim mostlikely As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'move on from the comma..
        Dim max As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the rparen..
        Return New TriangularDistExprNode(rand, min, mostlikely, max)
    End Function
    Private Function FactorRand() As ParseTreeNode
        Scan() 'remove rand
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove left paren
        If IsTok(TokenEnum.RPAREN) Then
            Scan()
            Return New RandNumExprNode()
        Else
            Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
            MustBe(TokenEnum.RPAREN)
            Scan() 'i think i need to do this... what happens?
            Return New RandNumExprNode(ret)
        End If
    End Function
    Private Function FactorRandBetween() As ParseTreeNode
        Scan() 'remove rand
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove left paren
        Dim Min As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'remove comma
        Dim max As ParseTreeNode = ParseATreeNode(Nothing)
        If IsTok(TokenEnum.RPAREN) Then
            Scan()
            Return New RandBetweenNumExprNode(Min, max)
        Else
            MustBe(TokenEnum.COMMA)
            Scan() 'remove comma
            Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
            MustBe(TokenEnum.RPAREN)
            Scan() 'remove rparen
            Return New RandBetweenNumExprNode(Min, max, ret)
        End If
    End Function
    Private Function FactorRandINT() As ParseTreeNode
        Scan() 'remove rand
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove left paren
        If IsTok(TokenEnum.RPAREN) Then
            Scan()
            Return New RandIntNumExprNode()
        Else
            Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
            MustBe(TokenEnum.RPAREN)
            Scan()
            Return New RandIntNumExprNode(ret)
        End If
    End Function
    Private Function FactorRandINTBetween() As ParseTreeNode
        Scan() 'remove rand
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove left paren
        Dim Min As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'remove comma
        Dim max As ParseTreeNode = ParseATreeNode(Nothing)
        If IsTok(TokenEnum.RPAREN) Then
            Scan()
            Return New RandIntBetweenNumExprNode(Min, max)
        Else
            MustBe(TokenEnum.COMMA)
            Scan() 'remove comma
            Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
            MustBe(TokenEnum.RPAREN)
            Scan() 'remove rparen
            Return New RandIntBetweenNumExprNode(Min, max, ret)
        End If
    End Function
    Private Function FactorRound() As ParseTreeNode
        Scan() 'remove round
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove parentisis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        If IsTok(TokenEnum.COMMA) Then
            Scan()
            Dim digits As ParseTreeNode = ParseATreeNode(Nothing)
            MustBe(TokenEnum.RPAREN)
            Scan()
            Return New RoundNumExprNode(ret, digits)
        Else
            MustBe(TokenEnum.RPAREN)
            Scan()
            Return New RoundNumExprNode(ret)
        End If
    End Function
    Private Function FactorRoundDown() As ParseTreeNode
        Scan() 'remove round
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove parentisis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan()
        Return New RoundDownNumExprNode(ret)
    End Function
    Private Function FactorRoundUp() As ParseTreeNode
        Scan() 'remove round
        MustBe(TokenEnum.LPAREN)
        Scan() 'remove parentisis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan()
        Return New RoundUpNumExprNode(ret)
    End Function
    Private Function FactorStringLeft() As ParseTreeNode
        Scan() 'remove token.enum.left
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        If IsTok(TokenEnum.COMMA) Then
            Scan() 'move on from the comma
        Else
            Scan()
            MustBe(TokenEnum.COMMA)
            Scan()
        End If
        Dim retnum As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the rparen 
        Return New LeftExprNode(ret, retnum)
    End Function
    Private Function FactorContains() As ParseTreeNode
        Scan() 'remove token.enum.contains
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan() 'move on from the comma
        Dim ret2 As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the number..
        Return New ContainsExprNode(ret, ret2)
    End Function
    Private Function factorLEN() As ParseTreeNode
        Scan() 'len
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parentisis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the right paren
        Return New LenEXPRNode(ret)
    End Function
    Private Function FactorInstring() As ParseTreeNode
        Scan() 'remove token.enum.left
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan()
        Dim retSearch As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan()

        Return New InstringExprNode(ret, retSearch)
    End Function
    Private Function FactorSubstring() As ParseTreeNode
        Scan() 'remove token.enum.left
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan()
        Dim retnumStart As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan()
        Dim retnumLength As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the right paren..
        Return New SubStringExprNode(ret, retnumStart, retnumLength)
    End Function
    Private Function FactorStringRight() As ParseTreeNode
        Scan() 'remove token.enum.left
        MustBe(TokenEnum.LPAREN)
        Scan() 'left parenthesis
        Dim ret As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.COMMA)
        Scan()
        Dim retnum As ParseTreeNode = ParseATreeNode(Nothing)
        MustBe(TokenEnum.RPAREN)
        Scan() 'move on from the number..
        Return New RightExprNode(ret, retnum)
    End Function
    Private Function FactorIf() As IfExprNode
        Scan() 'removes the if part of the statement
        MustBe(TokenEnum.LPAREN)
        Dim ret As New IfExprNode(ParseATreeNode())
        MustBe(TokenEnum.COMMA)
        ret.SetThen = ParseATreeNode()
        MustBe(TokenEnum.COMMA)
        ret.SetElse = ParseATreeNode()
        MustBe(TokenEnum.RPAREN)
        Scan() 'removes the ending right parentisis during parse a tree node.
        Return ret
    End Function
    Private Function FactorOR() As ParseTreeNode
        MustBe(TokenEnum.LPAREN)
        Dim Leftnode As ParseTreeNode = ParseATreeNode()
        Do Until IsTok(TokenEnum.RPAREN)
            MustBe(TokenEnum.COMMA)
            Dim rightnode As ParseTreeNode = ParseATreeNode()
            Leftnode = New OrExprNode(Leftnode, rightnode)
            If IsTok(TokenEnum.EOF) Or IsTok(TokenEnum.ERR) Then
                Return New ErrorNode(TypeEnum.Bool, "Encountered the end of an or expression before the final right parenthesis, or encountered an error " & tok.ToString, Leftnode.ExpressionToString)
            End If
        Loop
        Scan() ' for the ending right parenthesis of the or statement
        Return Leftnode
    End Function
    Private Function FactorAnd() As ParseTreeNode
        MustBe(TokenEnum.LPAREN)
        Dim Leftnode As ParseTreeNode = ParseATreeNode()
        Do Until IsTok(TokenEnum.RPAREN)
            MustBe(TokenEnum.COMMA)
            Dim rightnode As ParseTreeNode = ParseATreeNode()
            Leftnode = New AndExprNode(Leftnode, rightnode)
            If IsTok(TokenEnum.EOF) Or IsTok(TokenEnum.ERR) Then
                Return New ErrorNode(TypeEnum.Bool, "the end of an and expression before the final right parenthesis, or encountered an error " & tok.ToString, Leftnode.ExpressionToString)
            End If
        Loop
        Scan() ' for the ending right parentesis of the and statement
        Return Leftnode
    End Function
    Private Function BoolExpr(ByVal expr As ParseTreeNode) As ParseTreeNode
        If IsNothing(expr) Then
            Dim err As New ErrorNode(TypeEnum.ERR, "the parser found an error, the lefhandside of the token " & Me.tok.str & " was nothing", "")
            Scan()
            Return err
        End If

        Dim ret As BoolExprNode
        Select Case Me.tok.tok
            Case TokenEnum.LT
                ret = New LTExprNode(expr, ParseATreeNode)
            Case TokenEnum.LE
                ret = New LEExprNode(expr, ParseATreeNode)
            Case TokenEnum.GT
                ret = New GTExprNode(expr, ParseATreeNode)
            Case TokenEnum.GE
                ret = New GEExprNode(expr, ParseATreeNode)
            Case TokenEnum.EQ
                ret = New EQExprNode(expr, ParseATreeNode) 'New StringBoolCastExprNode(New EqualStringExprNode(expr, ParseATreeNode))
            Case TokenEnum.NEQ
                ret = New NEQExprNode(expr, ParseATreeNode)
            Case Else
        Expected(">, <, >=, <=, =, or !=")
        Return Nothing
        End Select
        Return ret
    End Function
    Private Function NumBoolExpr() As ParseTreeNode
        Return BoolExpr(NumTerm())
    End Function
    Private Function NumExpr() As ParseTreeNode
        Dim negate As Boolean = False
        If IsTok(TokenEnum.PLUS) Then
            Scan()
        ElseIf IsTok(TokenEnum.MINUS) Then
            Scan()
            negate = True
        End If

        Dim ret = NumTerm()
        If negate Then
            ret = New NegationNode(ret)
        End If

        Return NumExpr(ret)
    End Function
    Private Function numexpr(leftnode As ParseTreeNode) As ParseTreeNode
        If IsNothing(leftnode) Then
        Else
            Select Case leftnode.Token
                Case TokenEnum.KIF
                    If (leftnode.Type And TypeEnum.Num) > 0 Then
                        leftnode = leftnode
                    Else
                        leftnode.AddParseError("Encountered an if statement that doesn't return a double in a larger numerical expression")
                        Return New ErrorNode(TypeEnum.Num, "Encountered an if statement that doesn't return a double in a larger numerical expression", leftnode.ExpressionToString)
                        'Throw New Exception("Encountered an if statement that doesn't return a double in a larger numerical expression")
                    End If
                Case Else
                    leftnode = leftnode
            End Select
        End If

        While IsTok(TokenEnum.PLUS) Or IsTok(TokenEnum.MINUS) 'this keeps going until it is a token other than plus or minus.
            If IsTok(TokenEnum.PLUS) Then
                Scan()
                leftnode = New PlusExprNode(leftnode, NumTerm())
            ElseIf IsTok(TokenEnum.MINUS) Then
                Scan()
                If IsNothing(leftnode) Then
                    leftnode = New NegationNode(NumTerm())
                Else
                    leftnode = New MinusExprNode(leftnode, NumTerm())
                End If

            End If
        End While
        If IsTok(TokenEnum.LE) Or IsTok(TokenEnum.EQ) Or IsTok(TokenEnum.GE) Or IsTok(TokenEnum.GT) Or IsTok(TokenEnum.LT) Or IsTok(TokenEnum.NEQ) Then
            leftnode = BoolExpr(leftnode)
        Else
            leftnode = leftnode
        End If
        Return leftnode
    End Function

    Private Function NumFactor() As ParseTreeNode

        'lefthandside never gets used.
        Dim ret As ParseTreeNode = Nothing 'ParseATreeNode(Nothing)
        'If IsNothing(ret) Then Return New ErrorNode(TypeEnum.Num, "parser could not find the righthand side of the equation", "")
        'If (ret.Type And TypeEnum.Num) > 0 Then
        '    Return ret
        'Else
        '    Return New ErrorNode(TypeEnum.Num, "the right hand side was not numeric.", ret.Tostring)
        'End If
        Select Case tok.tok
            Case TokenEnum.HEADER
                If _variablenames.Contains(tok.str) Then
                    ret = headerparse(_variabletypes(_variablenames.IndexOf(tok.str)))
                Else
                    ret = New ErrorNode(TypeEnum.ERR, "The text " & Me.tok.str & " was found inside of brackets, but the avaialble headers do not match case or spelling.", Me.tok.str)
                    Return ret
                End If
                'ret = headerparse(_variabletypes(_variablenames.IndexOf(tok.str)))
            Case TokenEnum.NUMLIT
                ret = New NumNode(Double.Parse(tok.str))
                Scan()
            Case TokenEnum.NUMLITINT
                ret = New NumIntNode(Integer.Parse(tok.str))
                Scan()
            Case TokenEnum.INDEX
                ret = New IndexNumNode(tok.str)
                'do i need to scan?
                Scan()
            Case TokenEnum.LPAREN
                Scan()
                ret = ParenteticalStatement(NumExpr())
            Case TokenEnum.NORMINV
                ret = FactorNormINV()
            Case TokenEnum.TRIINV
                ret = FactorTriINV()
            Case TokenEnum.RAND
                ret = FactorRand()
            Case TokenEnum.LEN
                ret = factorLEN()
            Case TokenEnum.ROUND
                ret = FactorRound()
            Case TokenEnum.ROUNDDOWN
                ret = FactorRoundDown()
            Case TokenEnum.ROUNDUP
                ret = FactorRoundUp()
            Case TokenEnum.RANDBETWEEN
                ret = FactorRandBetween()
            Case TokenEnum.KIF
                Dim temp As IfExprNode = FactorIf()
                If (temp.Type And TypeEnum.Num) > 0 Then
                    ret = temp
                Else
                    Return New ErrorNode(TypeEnum.Num, "Encountered an if statement that doesn't return a double in a larger numerical expression", Me.tok.str)
                End If
            Case Else

                Return New ErrorNode(TypeEnum.Num, "Expected a number or (numeric expression) or a [header container] which produced a number, but got " & tok.str & "at position " & tok.pos, "")


        End Select
        Return ret
    End Function
    Private Function NumTerm() As ParseTreeNode
        Return NumTerm(NumFactor())
    End Function
    Private Function NumTerm(leftnode As ParseTreeNode) As ParseTreeNode
        Dim ret As ParseTreeNode = Nothing
        If IsNothing(leftnode) Then
            Dim err As New ErrorNode(TypeEnum.ERR, "Was asked to evaluate " & Me.tok.str & " and found no left hand side of the operation", "")
            Scan()
            Return err
        End If
        Select Case leftnode.Token
            Case TokenEnum.KIF
                If (leftnode.Type And TypeEnum.Num) > 0 Then
                    ret = leftnode
                Else
                    leftnode.AddParseError("Encountered an if statement that doesn't return a double in a larger numerical expression")
                    ret = New ErrorNode(TypeEnum.Num, "Encountered an if statement that doesn't return a double in a larger numerical expression", leftnode.ExpressionToString)
                    'Throw New Exception("Encountered an if statement that doesn't return a double in a larger numerical expression")
                End If
            Case Else
                ret = leftnode
        End Select
        While IsTok(TokenEnum.TIMES) Or IsTok(TokenEnum.DIVIDE) Or IsTok(TokenEnum.EXPONENT)
            Select Case Me.tok.tok
                Case TokenEnum.TIMES
                    Scan()
                    ret = New MultiplyExprNode(ret, NumFactor())
                Case TokenEnum.DIVIDE
                    Scan()
                    ret = New DivideExprNode(ret, NumFactor())
                Case TokenEnum.EXPONENT
                    Scan()
                    ret = New ExponentExprNode(ret, NumFactor())
            End Select
        End While
        Return ret

    End Function
    Private Function MustBe(tok As TokenEnum) As Boolean
        If Not IsTok(tok) Then
            Expected(GetName(tok))
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub Expected(str As String)
        Dim tmp As New ErrorNode(TypeEnum.ERR, "Expected " & str & ", got " & GetName(Me.tok.tok) & Environment.NewLine & "At: " & Me.tok.linenumber & ":" & Me.tok.pos, str)
        tmp.AddParseError("Expected " & str & ", got " & GetName(Me.tok.tok) & Environment.NewLine & "At: " & Me.tok.linenumber & ":" & Me.tok.pos)
        'Return New ParseException("Expected " & str & ", got " & GetName(Me.tok.tok) & Environment.NewLine & "At: " & Me.tok.linenumber & ":" & Me.tok.pos)
    End Sub
    Private Function GetName(k As TokenEnum) As String
        Return [Enum].GetName(GetType(TokenEnum), k)
    End Function
    Private Function IsTok(tok As TokenEnum) As Boolean
        Return tok = Me.tok.tok
    End Function
    ''' <summary>
    ''' Advances to the next fully validated token.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Scan()
        tok = Me._scanner.Scan()
    End Sub
    Private Function StringFactor() As ParseTreeNode
        Dim ret As ParseTreeNode = Nothing
        Select Case tok.tok
            Case TokenEnum.HEADER
                If _variablenames.Contains(tok.str) Then
                    Return headerparse(_variabletypes(_variablenames.IndexOf(tok.str)))
                Else
                    ret = New ErrorNode(TypeEnum.ERR, "The text " & Me.tok.str & " was found inside of brackets, but the avaialble headers do not match case or spelling.", Me.tok.str)
                    Return ret
                End If
                If (ret.Type And TypeEnum.Str) > 0 Then
                    Return ret
                Else
                    Return New ErrorNode(TypeEnum.Str, "Encountered an if statement that doesn't return a string in a larger string expression", Me.tok.str)
                End If
            Case TokenEnum.INDEX
                Scan()
                Return New ConvertToString(New IndexNumNode(tok.str))
            Case TokenEnum.ConvertToString
                Scan()
                MustBe(TokenEnum.LPAREN)
                Scan()
                Dim parsetreenode As ParseTreeNode = ParseATreeNode(Nothing)
                MustBe(TokenEnum.RPAREN)
                Scan()
                Return New ConvertToString(parsetreenode)
            Case TokenEnum.KIF
                Dim temp As IfExprNode = FactorIf()
                If (temp.Type And TypeEnum.Str) > 0 Then
                    Return temp
                Else
                    Return New ErrorNode(TypeEnum.Str, "Encountered an if statement that doesn't return a string in a larger string expression", Me.tok.str)
                End If
            Case TokenEnum.LEFT
                Return FactorStringLeft()
            Case TokenEnum.RIGHT
                Return FactorStringRight()
            Case TokenEnum.StringLIT
                ret = New StringNode(tok.str)
                Scan()
                Return ret
            Case TokenEnum.SUBSTRING
                Return FactorSubstring()
            Case Else

                Return New ErrorNode(TypeEnum.Num, "Expected a string or expression which produced a string, but got " & tok.tok.ToString & " at position " & tok.pos, "")
                'error?
        End Select
    End Function
    Private Function stringparse(left As ParseTreeNode) As ParseTreeNode
        Scan() 'get rid of the string
        Select Case tok.tok
            Case TokenEnum.ANDPERSTAND
                Scan()
                Return New ConcatenateExprNode(left, StringFactor())
            Case TokenEnum.StringLIT 'how could this happen? a string proceeded by a string immediately?
                Expected("any string token other than a string literal")
                Return New ErrorNode(TypeEnum.Str, "any string token other than a string literal", left.ExpressionToString)

            Case TokenEnum.EQ
                Scan()
                Return New EQExprNode(left, ParseATreeNode(Nothing))
            Case TokenEnum.NEQ
                Scan()
                Return New NEQExprNode(left, ParseATreeNode(Nothing))
            Case TokenEnum.LEFT
                Return FactorStringLeft()
            Case TokenEnum.RIGHT
                Return FactorStringRight()
            Case Else
                Return left
        End Select
    End Function

    Private Function headerparse(ByVal type As Type) As ParseTreeNode
        Dim hdr = New HeaderContainer(tok.str, type)
        Scan()
        Return hdr
    End Function
    Private Function offsetparse() As ParseTreeNode
        Scan() 'remove the offset?
        Scan() 'remove the parenthesis
        MustBe(TokenEnum.HEADER)
        Dim hdr As ParseTreeNode = ParseATreeNode(Nothing)
        If IsNothing(hdr) Then
            If IsTok(TokenEnum.COMMA) Then
                Scan() 'remove the comma
                If IsTok(TokenEnum.RPAREN) Then
                    Scan() 'remove the parentesis
                    Return New OffsetExprNode(Nothing, Nothing)
                Else
                    'they may have put in the number. or they havent put in the right paren.
                    Dim tmp As New ErrorNode(TypeEnum.Valid, "Improper syntax on the offset expression, expected a header and then a comma", "offset(," & tok.str)
                    Return New OffsetExprNode(Nothing, Nothing)
                End If
            Else
                'offset left paren, but no comma?
                Dim tmp As New ErrorNode(TypeEnum.Valid, "Improper syntax on the offset expression, expected a header and then a comma", "offset(")
                Return New OffsetExprNode(Nothing, Nothing)
            End If
        Else
            Scan() 'remove the comma
            Dim num As ParseTreeNode = ParseATreeNode(Nothing)
            If IsNothing(num) Then
                Return New OffsetExprNode(hdr, Nothing)
            Else
                If num.containsVariable Then
                    num.AddParseError("the offset must be a number less than or equal to zero that does not change")
                    Return New ErrorNode(TypeEnum.UnDeclared, "the offset must be a number less than or equal to zero that does not change", num.ExpressionToString)
                    'Throw New ArgumentException("the offset must be a number less than or equal to zero that does not change")
                End If
                If num.GetType <> GetType(NegationNode) Then
                    num.AddParseError("The numerical value must be a negative number in the offset expression")
                    Return New ErrorNode(TypeEnum.UnDeclared, "the offset must be a number less than or equal to zero", num.ExpressionToString)
                End If
                MustBe(TokenEnum.RPAREN)
                Scan() 'remove the parenthesis
                Select Case hdr.Type
                    Case TypeEnum.Bool
                        Return New OffsetExprNode(hdr, num)
                    Case TypeEnum.Str
                        Return New OffsetExprNode(hdr, num)
                    Case TypeEnum.Int
                        Return New OffsetExprNode(hdr, num)
                    Case TypeEnum.Shrt
                        Return New OffsetExprNode(hdr, num)
                    Case TypeEnum.Deciml
                        Return New OffsetExprNode(hdr, num)
                    Case TypeEnum.Dub
                        Return New OffsetExprNode(hdr, num)
                    Case Else
                        hdr.AddParseError("the header doesnt have an appropriate type")
                        Return New ErrorNode(TypeEnum.UnDeclared, "the header doesnt have an appropriate type", hdr.ExpressionToString)
                End Select
            End If
        End If
    End Function
End Class
