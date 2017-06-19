Public Class NumIntNode
    Inherits NumExprNode
    Private _val As Integer
    Sub New(ByVal val As Integer)
        _type = TypeEnum.Int
        _val = val
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_val, TypeEnum.Int)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_val)
    End Function
    Public Overrides Function ExpressionToString() As String
        Return _val.ToString()
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.NUMLIT
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return False
    End Function
    Public Overrides Function Simplify() As ParseTreeNode
        Return Me
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
    End Sub
    Public Overrides Sub Update(ByRef row() As Object)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Return New List(Of String)
    End Function
End Class
