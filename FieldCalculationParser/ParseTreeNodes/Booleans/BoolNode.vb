Public Class BoolNode
    Inherits BoolExprNode
    Private _bool As Boolean
    Sub New(ByVal Bo As String)
        If Boolean.TryParse(Bo, _bool) Then
            _type = TypeEnum.Bool
        Else
            _type = TypeEnum.ERR
            _ParseError = "The string " & Bo & " cannot be converted to boolean"
            _ParseErrors.Add(_ParseError)
        End If

    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return False
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_bool, TypeEnum.Bool)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_bool, TypeEnum.Bool)
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.BOOLLIT
    End Function
    Public Overrides Function ExpressionToString() As String
        Return _bool.ToString
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
    End Sub
    Public Overrides Function Simplify() As ParseTreeNode
        Return Me
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        If _type = TypeEnum.ERR Then l.Add(_ParseError)
        Return l
    End Function
End Class
