Public Class StringNode
    Inherits StringExprNode
    Private _Str As String
    Sub New(ByVal str As String)
        _type = TypeEnum.Str
        _Str = str
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_Str, TypeEnum.Str)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_Str, TypeEnum.Str)
    End Function
    Public Overrides Function ExpressionToString() As String
        Return Chr(34) & _Str & Chr(34)
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.StringLIT
    End Function
    Public Overrides Function Simplify() As ParseTreeNode
        Return Me
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return False
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Return New List(Of String)
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        Return ret
    End Function
End Class
