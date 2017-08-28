Public Class Scanner
    Dim sr As System.IO.StreamReader
    Dim putback As Boolean = True
    Dim gotEOF As Boolean = False
    Dim c As Char = Chr(13)
    Dim linenumber As Int32 = 1
    Dim pos As Int32 = 0
    Public Event TokenFound(ByVal pos As Int32, ByVal token As TokenEnum, ByVal text As String, ByVal helpdocpath As String)
    Public Event ErrorFound(ByVal message As String)
    Public Sub New(stream As System.IO.Stream)
        sr = New System.IO.StreamReader(stream)
    End Sub
    Public Function Scan() As Token
        If gotEOF Then
            System.Diagnostics.Debug.WriteLine("scan called after EOF")
            Return New Token(TokenEnum.ERR, "The scanner was called after the end of the file was found, some expression must not be complete", linenumber, pos)
        End If
        If putback = True Then
            putback = False

        Else
            c = GetCharacter(sr)

        End If
        If Char.IsLetter(c) Or c = "_" Then
            Dim idPos = pos
            Dim id As String = BuildString()
            Return New Token(KeywordLookup(id), id, linenumber, idPos)
        ElseIf Char.IsDigit(c) Or c = "."c Then
            Dim numPos = pos
            Dim num As NumberResult = BuildNum()
            If num.IsNumerical Then
                If num.HasDecimal Then
                    RaiseEvent TokenFound(pos, TokenEnum.NUMLIT, num.Input, "")
                    Return New Token(TokenEnum.NUMLIT, num.Input, linenumber, numPos)
                Else
                    RaiseEvent TokenFound(pos, TokenEnum.NUMLITINT, num.Input, "")
                    Return New Token(TokenEnum.NUMLITINT, num.Input, linenumber, numPos)
                End If

            Else
                RaiseEvent TokenFound(pos, TokenEnum.StringLIT, num.Input, "")
                Return New Token(TokenEnum.StringLIT, num.Input, linenumber, numPos)
            End If

        Else
            Select Case c
                Case "("c
                    RaiseEvent TokenFound(pos, TokenEnum.LPAREN, "(", "")
                    Return New Token(TokenEnum.LPAREN, c, linenumber, pos)
                Case ")"c
                    RaiseEvent TokenFound(pos, TokenEnum.RPAREN, ")", "")
                    Return New Token(TokenEnum.RPAREN, c, linenumber, pos)
                Case "{"c
                    c = GetCharacter(sr)
                    RaiseEvent TokenFound(pos, TokenEnum.INDEX, "{", "")
                    Dim idpos = pos
                    Dim id = BuildString("}"c)
                    Return New Token(TokenEnum.INDEX, id, linenumber, idpos)
                Case "["c
                    c = GetCharacter(sr)
                    Dim idpos = pos
                    RaiseEvent TokenFound(pos, TokenEnum.LBRACKET, "[", "")
                    Dim id = BuildString("]"c)
                    Return New Token(TokenEnum.HEADER, id, linenumber, idpos)
                Case "+"c
                    RaiseEvent TokenFound(pos, TokenEnum.PLUS, "+", "FieldCalculationParser.Numerics.xml")
                    Return BuildOperator()
                Case "-"c
                    RaiseEvent TokenFound(pos, TokenEnum.MINUS, "-", "FieldCalculationParser.Numerics.xml")
                    Return BuildOperator()
                Case "*"c
                    RaiseEvent TokenFound(pos, TokenEnum.TIMES, "*", "FieldCalculationParser.Numerics.xml")
                    Return BuildOperator()
                Case "/"c
                    RaiseEvent TokenFound(pos, TokenEnum.DIVIDE, "/", "FieldCalculationParser.Numerics.xml")
                    Return BuildOperator()
                Case "^"c
                    RaiseEvent TokenFound(pos, TokenEnum.EXPONENT, "^", "FieldCalculationParser.Numerics.xml")
                    Return BuildOperator()
                Case "="c
                    RaiseEvent TokenFound(pos, TokenEnum.EQ, "=", "FieldCalculationParser.Booleans.xml")
                    Return BuildOperator()
                Case "&"c
                    Return BuildOperator()
                Case "!"c
                    Return BuildOperator()
                Case "<"c
                    Return BuildOperator()
                Case ">"c
                    Return BuildOperator()
                Case ","c
                    Return BuildOperator()
                Case "'"c
                    c = GetCharacter(sr)
                    Dim idpos = pos
                    RaiseEvent TokenFound(pos, TokenEnum.StringLIT, "'", "")
                    Dim id = BuildString("'"c)
                    Return New Token(TokenEnum.StringLIT, id, linenumber, idpos)
                Case Chr(34)
                    c = GetCharacter(sr)
                    Dim idpos = pos
                    RaiseEvent TokenFound(pos, TokenEnum.StringLIT, Chr(34), "")
                    Dim id = BuildString(Chr(34))
                    Return New Token(TokenEnum.StringLIT, id, linenumber, idpos)
                Case Chr(32) 'space
                    c = GetCharacter(sr)
                    Return BuildOperator()
                Case Chr(0) ' null, we treat it as EOF
                    gotEOF = True
                    Return New Token(TokenEnum.EOF, "EOF", linenumber, pos)
                Case Else
                    Return New Token(TokenEnum.ERR, "bad character " & New String(c, 1), linenumber, pos)
            End Select
        End If
    End Function
    Public Function GetCharacter(sr As System.IO.StreamReader) As Char
        Dim buf(1) As Char
        Dim read As Integer
        'Try
        read = sr.Read(buf, 0, 1)
        If read <> 1 Then
            gotEOF = True
            Return Chr(0)
        Else
            pos = pos + 1
            Return buf(0)
        End If
    End Function
    Private Function BuildOperator() As Token
        Select Case c
            Case "="c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.EQ, "=", linenumber, pos)
            Case "&"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.ANDPERSTAND, "&", linenumber, pos)
            Case "+"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.PLUS, "+", linenumber, pos)
            Case "-"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.MINUS, "-", linenumber, pos)
            Case "/"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.DIVIDE, "/", linenumber, pos)
            Case "\"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.DIVIDE, "\", linenumber, pos)
            Case "*"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.TIMES, "*", linenumber, pos)
            Case "^"c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.EXPONENT, "*", linenumber, pos)
            Case ","c
                c = GetCharacter(sr)
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.COMMA, ",", linenumber, pos)
            Case ">"c
                c = GetCharacter(sr)
                If c.Equals("="c) Then
                    If Not c.Equals(" "c) Then putback = True
                    Return New Token(TokenEnum.GE, ">=", linenumber, pos)
                End If
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.GT, ">", linenumber, pos)
            Case "<"c
                c = GetCharacter(sr)
                If c.Equals("="c) Then
                    If Not c.Equals(" "c) Then putback = True
                    Return New Token(TokenEnum.LE, "<=", linenumber, pos)
                End If
                If Not c.Equals(" "c) Then putback = True
                Return New Token(TokenEnum.LT, "<", linenumber, pos)
            Case "!"c
                c = GetCharacter(sr)
                If c.Equals("="c) Then
                    If Not c.Equals(" "c) Then putback = True
                    Return New Token(TokenEnum.NEQ, "!=", linenumber, pos)
                End If
                If Not c.Equals(" "c) Then putback = True
                RaiseEvent ErrorFound("Found an ! operator without an equals following.")
                Return New Token(TokenEnum.ERR, "! not followed by =", linenumber, pos)
            Case Else
                putback = True
                RaiseEvent ErrorFound("Found an invalid operator after a space.")
                Return New Token(TokenEnum.ERR, "Found a space followed by " + c + " unable to dicipher the intent of the user", linenumber, pos)
        End Select
    End Function
    Private Function BuildString() As String
        Dim s As String = ""
        Do
            s &= c
            c = GetCharacter(sr)
        Loop While Char.IsLetterOrDigit(c) Or Char.IsWhiteSpace(c) Or c = "_"c
        putback = True
        Return s
    End Function
    Private Function BuildString(terminator As Char) As String
        Dim s As String = ""
        Do
            s &= c
            c = GetCharacter(sr)
        Loop While Char.IsLetterOrDigit(c) OrElse Char.IsWhiteSpace(c) OrElse c = "_"c And Not c.Equals(terminator) And Not c.Equals(Chr(0))
        Return s
    End Function
    Private Function BuildNum() As NumberResult
        Dim s As NumberResult = New NumberResult("", False, True)
        Do
            If (c.Equals("."c)) Then
                If (s.HasDecimal) Then
                    s.IsNumerical = False
                Else
                    s.IsNumerical = True
                End If
                s.HasDecimal = True
            End If
            s.Input += c
            c = GetCharacter(sr)
        Loop While Char.IsDigit(c) Or c = "."c
        putback = True
        Return s
    End Function
    Public Function KeywordLookup(id As String) As TokenEnum
        Dim tmp As String = id.ToUpper
        Select Case tmp
            Case "IF"
                RaiseEvent TokenFound(pos, TokenEnum.KIF, "IF", "FieldCalculationParser.Booleans.xml")
                Return TokenEnum.KIF
            Case "AND"
                RaiseEvent TokenFound(pos, TokenEnum.KAND, "AND", "FieldCalculationParser.Booleans.xml")
                Return TokenEnum.KAND
            Case "OR"
                RaiseEvent TokenFound(pos, TokenEnum.KOR, "OR", "FieldCalculationParser.Booleans.xml")
                Return TokenEnum.KOR
            Case ","
                Return TokenEnum.COMMA
                'Case "T"
                '    Return TokenEnum.BOOLLIT
                'Case "F"
                '    Return TokenEnum.BOOLLIT
            Case "TRUE"
                RaiseEvent TokenFound(pos, TokenEnum.BOOLLIT, "True", "")
                Return TokenEnum.BOOLLIT

            Case "FALSE"
                RaiseEvent TokenFound(pos, TokenEnum.BOOLLIT, "False", "")
                Return TokenEnum.BOOLLIT
            Case "RIGHT"
                RaiseEvent TokenFound(pos, TokenEnum.RIGHT, "RIGHT", "FieldCalculationParser.Strings.xml")
                Return TokenEnum.RIGHT
            Case "LEFT"
                RaiseEvent TokenFound(pos, TokenEnum.LEFT, "LEFT", "FieldCalculationParser.Strings.xml")
                Return TokenEnum.LEFT
            Case "LEN"
                RaiseEvent TokenFound(pos, TokenEnum.LEN, "LEN", "FieldCalculationParser.Strigns.xml")
                Return TokenEnum.LEN
            Case "RAND"
                RaiseEvent TokenFound(pos, TokenEnum.RAND, "RAND", "FieldCalculationParser.Statistics.xml")
                Return TokenEnum.RAND
            Case "RANDBETWEEN"
                RaiseEvent TokenFound(pos, TokenEnum.RANDBETWEEN, "RANDBETWEEN", "FieldCalculationParser.Statistics.xml")
                Return TokenEnum.RANDBETWEEN
            Case "RANDINT"
                RaiseEvent TokenFound(pos, TokenEnum.RANDINT, "RANDINT", "FieldCalculationParser.Statistics.xml")
                Return TokenEnum.RANDINT
            Case "RANDINTBETWEEN"
                RaiseEvent TokenFound(pos, TokenEnum.RANDINTBETWEEN, "RANDINTBETWEEN", "FieldCalculationParser.Statistics.xml")
                Return TokenEnum.RANDINTBETWEEN
            Case "ROUND"
                RaiseEvent TokenFound(pos, TokenEnum.ROUND, "ROUND", "FieldCalculationParser.Numerics.xml")
                Return TokenEnum.ROUND
            Case "ROUNDDOWN"
                RaiseEvent TokenFound(pos, TokenEnum.ROUNDDOWN, "ROUNDDOWN", "FieldCalculationParser.Numerics.xml")
                Return TokenEnum.ROUNDDOWN
            Case "ROUNDUP"
                RaiseEvent TokenFound(pos, TokenEnum.ROUNDUP, "ROUNDUP", "FieldCalculationParser.Numerics.xml")
                Return TokenEnum.ROUNDUP
            Case "NORMINV"
                RaiseEvent TokenFound(pos, TokenEnum.NORMINV, "NORMINV", "FieldCalculationParser.Statistics.xml")
                Return TokenEnum.NORMINV
            Case "TRIINV"
                RaiseEvent TokenFound(pos, TokenEnum.TRIINV, "TRIINV", "FieldCalculationParser.Statistics.xml")
                Return TokenEnum.TRIINV
            Case "INSTRING"
                RaiseEvent TokenFound(pos, TokenEnum.INSTRING, "INSTRING", "FieldCalculatonParser.Strings.xml")
                Return TokenEnum.INSTRING
            Case "SUBSTRING"
                RaiseEvent TokenFound(pos, TokenEnum.SUBSTRING, "SUBSTRING", "FieldCalculationParser.Strings.xml")
                Return TokenEnum.SUBSTRING
            Case "CONTAINS"
                RaiseEvent TokenFound(pos, TokenEnum.CONTAINS, "CONTAINS", "FieldCalculationParser.Booleans.xml")
                Return TokenEnum.CONTAINS
            Case "OFFSET"
                'RaiseEvent TokenFound(pos, TokenEnum.OFFSET, "OFFSET", "FieldCalculationParser.Numerics.xml")
                Return TokenEnum.OFFSET
            Case "DBL"
                RaiseEvent TokenFound(pos, TokenEnum.ConvertToDouble, "DBL", "FieldCalculationParser.Converters.xml")
                Return TokenEnum.ConvertToDouble
            Case "STR"
                RaiseEvent TokenFound(pos, TokenEnum.ConvertToString, "STR", "FieldCalculationParser.Converters.xml")
                Return TokenEnum.ConvertToString
            Case "INT"
                RaiseEvent TokenFound(pos, TokenEnum.ConvertToInteger, "INT", "FieldCalculationParser.Converters.xml")
                Return TokenEnum.ConvertToInteger
            Case "SHORT"
                RaiseEvent TokenFound(pos, TokenEnum.ConvertToShort, "SHORT", "FieldCalculationParser.Converters.xml")
                Return TokenEnum.ConvertToShort
            Case "BOOL"
                RaiseEvent TokenFound(pos, TokenEnum.ConvertToBoolean, "BOOL", "FieldCalculationParser.Converters.xml")
                Return TokenEnum.ConvertToBoolean
            Case "BYTE"
                RaiseEvent TokenFound(pos, TokenEnum.ConvertToByte, "BYTE", "FieldCalculationParser.Converters.xml")
                Return TokenEnum.ConvertToByte
            Case Else
                RaiseEvent TokenFound(pos, TokenEnum.StringLIT, id, "")
                Return TokenEnum.StringLIT
        End Select
    End Function
    Friend Class NumberResult
        Friend Input As String
        Friend HasDecimal As Boolean = False
        Friend IsNumerical As Boolean = True
        Friend Sub New(str As String, hasd As Boolean, isnum As Boolean)
            Input = str
            HasDecimal = hasd
            IsNumerical = isnum
        End Sub
    End Class
End Class
