Public Class NumNode
    Inherits NumExprNode
    Private _val As Double
    Sub New(ByVal val As Double)
        _type = TypeEnum.Dub
        _val = val
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_val, TypeEnum.Dub)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(CSng(_val))
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
