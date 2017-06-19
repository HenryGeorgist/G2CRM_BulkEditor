Public Class ExponentExprNode
    Inherits BinaryNumExprNode
    Private Const _OpName As String = "^"
    Private Const _OpString As String = "raise"
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        If _type = TypeEnum.Int Then
            Dim l As Integer
            Integer.TryParse(leftNode.Evaluate.GetResult.ToString, l)
            Dim r As Integer
            Integer.TryParse(rightNode.Evaluate.GetResult.ToString, r)
            Return New ParseTreeNodeResult(l ^ r, TypeEnum.Int)
        Else
            Dim l As Double
            Double.TryParse(leftNode.Evaluate.GetResult.ToString, l)
            Dim r As Double
            Double.TryParse(rightNode.Evaluate.GetResult.ToString, r)
            Return New ParseTreeNodeResult(l ^ r, TypeEnum.Dub)
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        If _type = TypeEnum.Int Then
            Dim l As Integer
            Integer.TryParse(leftNode.Evaluate.GetResult.ToString, l)
            Dim r As Integer
            Integer.TryParse(rightNode.Evaluate.GetResult.ToString, r)
            Return New ParseTreeNodeResult(CSng(l ^ r))
        Else
            Dim l As Double
            Double.TryParse(leftNode.Evaluate.GetResult.ToString, l)
            Dim r As Double
            Double.TryParse(rightNode.Evaluate.GetResult.ToString, r)
            Return New ParseTreeNodeResult(CSng(l ^ r))
        End If
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.EXPONENT
    End Function
    Public Overrides ReadOnly Property OpName As String
        Get
            Return _OpName
        End Get
    End Property

    Public Overrides ReadOnly Property OpString As String
        Get
            Return _OpString
        End Get
    End Property
End Class
