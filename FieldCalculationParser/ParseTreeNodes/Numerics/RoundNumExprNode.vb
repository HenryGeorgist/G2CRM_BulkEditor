Public Class RoundNumExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _val As ParseTreeNode
    Private _digits As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal val As ParseTreeNode, ByVal numdigits As ParseTreeNode)
        If IsNothing(val) Then
            _ParseError = "the round expression was found, but the value argument has not been specified."
            _type = TypeEnum.ERR
            _val = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            If (val.Type And TypeEnum.Num) = 0 Then _ParseError = "The value argument of round was not numerical" : _ParseErrors.Add(_ParseError)
            If IsNothing(numdigits) Then
                _ParseError = "the round expression was found, but the number of digits argument has not been specified."
                _type = TypeEnum.ERR
                _val = New ErrorNode(TypeEnum.Int, "Empty", "")
                _ParseErrors.Add(_ParseError)
            Else
                _val = val
                _digits = numdigits
                If _val.ContainsError Then _type = TypeEnum.ERR
                If _digits.ContainsError Then _type = TypeEnum.ERR
                If _digits.containsVariable Then
                    _type = TypeEnum.Dub
                Else
                    If CInt(_digits.Evaluate.GetResult) = 0 Then
                        _type = TypeEnum.Int
                    Else
                        _type = TypeEnum.Dub
                    End If
                End If

            End If
        End If

    End Sub
    Sub New(ByVal val As ParseTreeNode)
        If IsNothing(val) Then
            _ParseError = "the round expression was found, but the value argument has not been specified."
            _type = TypeEnum.ERR
            _ParseErrors.Add(_ParseError)
        Else
            If (val.Type And TypeEnum.Num) = 0 Then _ParseError = "The value argument of round was not numerical" : _ParseErrors.Add(_ParseError)
            _val = val
            _digits = New NumNode(0)
            _type = TypeEnum.Int
        End If

    End Sub
    Public Overrides Function containsVariable() As Boolean
        If _val.containsVariable Then Return True
        If _digits.containsVariable Then Return True
        Return False
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim d As Decimal
        If Not Decimal.TryParse(_val.Evaluate.GetResult.ToString, d) Then _ComputeErrors.Add("value could not be converted to a decimal at " & RowOrCellNum)
        Dim i As Integer
        If Not Integer.TryParse(_digits.Evaluate.GetResult.ToString, i) Then _ComputeErrors.Add("digits could not be converted to an integer at " & RowOrCellNum)
        If i < 0 Or i > 15 Then
            _ComputeErrors.Add("the digit argument must be between 0 and 15 inclusive at " & RowOrCellNum)
            Return New ParseTreeNodeResult(Math.Round(d, 0), TypeEnum.Int)
        Else
            If d.ToString.Length - Left(d.ToString, InStr(d.ToString, ".")).Length - 1 < i Then
                _ComputeErrors.Add("the digit argument must have more decimal places than the numberof digits argument at " & RowOrCellNum)
                Return New ParseTreeNodeResult(d, TypeEnum.Num)
            End If
            Return New ParseTreeNodeResult(Math.Round(d, i), TypeEnum.Num)
        End If


    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim d As Decimal
        If Not Decimal.TryParse(_val.Evaluate.GetResult.ToString, d) Then _ComputeErrors.Add("value could not be converted to a decimal at " & RowOrCellNum)
        Dim i As Integer
        If Not Integer.TryParse(_digits.Evaluate.GetResult.ToString, i) Then _ComputeErrors.Add("digits could not be converted to an integer at " & RowOrCellNum)
        If i < 0 Or i > 15 Then
            _ComputeErrors.Add("the digit argument must be between 0 and 15 inclusive at " & RowOrCellNum)
            Return New ParseTreeNodeResult(CSng(Math.Round(d, 0)))
        Else
            If d.ToString.Length - Left(d.ToString, InStr(d.ToString, ".")).Length - 1 < i Then
                _ComputeErrors.Add("the digit argument must have more decimal places than the numberof digits argument at " & RowOrCellNum)
                Return New ParseTreeNodeResult(CSng(d))
            End If
            Return New ParseTreeNodeResult(CSng(Math.Round(d, i)))
        End If


    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret = _val.GetHeaderNames
        ret.AddRange(_digits.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        ret.Add(_ParseError)
        If Not IsNothing(_val) Then ret.AddRange(_val.GetErrorMessages)
        If Not IsNothing(_digits) Then ret.AddRange(_digits.GetErrorMessages)
        Return ret
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _val.SetColNums(uniqueheaderNames)
        _digits.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.ROUND
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "Round(" & _val.ExpressionToString & "," & _digits.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _val.Update(row)
        _digits.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _val.UpdateSingle(row)
        _digits.UpdateSingle(row)
    End Sub
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "ROUND"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Numerics
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "ROUND(,)"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Numerics.xml"
        End Get
    End Property
End Class
