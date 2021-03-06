Public Class MultiplyExprNode
    Inherits BinaryNumExprNode
    Implements IDisplayToTreeNode
    Private Const _OpName As String = "*"
    Private Const _OpString As String = "Multiply"
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)
    End Sub
    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        If _type = TypeEnum.Int Then
            Dim l As Integer
            Integer.TryParse(leftNode.Evaluate.GetResult.ToString, l)
            Dim r As Integer
            Integer.TryParse(rightNode.Evaluate.GetResult.ToString, r)
            Return New ParseTreeNodeResult(l * r, _type)
        Else
            Dim l As Double
            Double.TryParse(leftNode.Evaluate.GetResult.ToString, l)
            Dim r As Double
            Double.TryParse(rightNode.Evaluate.GetResult.ToString, r)
            Return New ParseTreeNodeResult(l * r, _type)
        End If

    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim l As Single = leftNode.Evaluate.GetSingleResult
        Dim r As Single = rightNode.Evaluate.GetSingleResult
        Return New ParseTreeNodeResult(l * r)
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.TIMES
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
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Operators
        End Get
    End Property

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return _OpName
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return _OpName
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Numerics.xml"
        End Get
    End Property
End Class
