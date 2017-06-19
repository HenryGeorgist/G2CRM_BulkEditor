Public Class AndExprNode
    Inherits BinaryBoolExprNode
    Implements IDisplayToTreeNode
    Private Const _OpName As String = "AND"
    Private Const _OpString As String = "And"
    Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)

    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim l As Boolean
        Boolean.TryParse(leftNode.Evaluate.GetResult.ToString, l)
        If l Then
            Return rightNode.Evaluate 'if it is true, it is true, if it is false, the expression returns false.
        Else
            Return New ParseTreeNodeResult(l, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim l As Boolean
        Boolean.TryParse(leftNode.Evaluate.GetResult.ToString, l)
        If l Then
            Return rightNode.Evaluate 'if it is true, it is true, if it is false, the expression returns false.
        Else
            Return New ParseTreeNodeResult(l, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.KAND
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

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return _OpName
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Booleans
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return _OpName & "(,)"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Booleans.xml"
        End Get
    End Property
End Class
