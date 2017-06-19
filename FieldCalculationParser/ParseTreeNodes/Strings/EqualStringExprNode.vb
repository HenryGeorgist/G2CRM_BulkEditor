Public Class EqualStringExprNode
    Inherits BinaryStringExprNode
    Sub New(ByVal Left As StringExprNode, ByVal right As StringExprNode)
        MyBase.New(Left, right)
        opName = "="
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        If leftNode.Evaluate.GetResult = rightNode.Evaluate.GetResult Then
            Return New ParseTreeNodeResult(True, TypeEnum.Bool)
        Else
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function Type() As TypeEnum
        Return TypeEnum.Bool
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.EQ
    End Function
End Class
