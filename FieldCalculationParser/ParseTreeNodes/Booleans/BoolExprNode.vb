Public MustInherit Class BoolExprNode
    Inherits ParseTreeNode
    Public MustOverride Overrides Function Evaluate() As ParseTreeNodeResult
    Public MustOverride Overrides Function ExpressionToString() As String
End Class
