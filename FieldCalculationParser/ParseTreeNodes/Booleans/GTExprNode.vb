Public Class GTExprNode
    Inherits BinaryBoolExprNode
    Private Const _OpName As String = ">"
    Private Const _OpString As String = "Greater Than"
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)
    End Sub
    Public Overrides Function ExpressionToString() As String
        Return "(" & leftNode.ExpressionToString & Me.OpName & rightNode.ExpressionToString & ")"
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim l As Double
        Double.TryParse(leftNode.Evaluate.GetResult.ToString, l)
        Dim r As Double
        Double.TryParse(rightNode.Evaluate.GetResult.ToString, r)
        If l > r Then
            Return New ParseTreeNodeResult(True, TypeEnum.Bool)
        Else
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim l As Double
        Double.TryParse(leftNode.Evaluate.GetResult.ToString, l)
        Dim r As Double
        Double.TryParse(rightNode.Evaluate.GetResult.ToString, r)
        If l > r Then
            Return New ParseTreeNodeResult(True, TypeEnum.Bool)
        Else
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.GT
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
