Public Class StringBoolCastExprNode
    Inherits BoolExprNode
    Private _expr As ParseTreeNode
    Public Sub New(ByVal expr As StringExprNode)
        _expr = expr
        If expr.Type <> TypeEnum.Bool Then
            _ParseError.Add("This expression is of type " & expr.Type.ToString & vbNewLine & "It cannot be converted into a string cast as boolean")
            _expr = New ErrorNode(TypeEnum.Bool)
            'Throw New Exception("This expression is of type " & expr.Type.ToString & vbNewLine & "It cannot be converted into a string cast as boolean")
        End If
    End Sub
    Public Overrides Function ToString() As String
        Return _expr.Tostring()
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return _expr.Evaluate
    End Function
    Public Overrides Sub Update(ByRef row() As Object, ByVal RoworColNum As Integer)
        _expr.Update(row, RoworColNum)
    End Sub
    Public Overrides Function Type() As TypeEnum
        Return TypeEnum.Bool
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.StringLIT
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return _expr.containsVariable
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _expr.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_expr.GetHeaderNames)
        Return ret
    End Function
End Class
