Public Class ConcatenateExprNode
    Inherits BinaryStringExprNode
    Implements IDisplayToTreeNode
    Private Const _opname As String = "&"
    Private Const _opstring As String = "Concatenate With"
    Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub
    Sub New(ByVal Left As ParseTreeNode, ByVal right As ParseTreeNode)
        MyBase.New(Left, right)
        _type = TypeEnum.Str
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(CType(leftNode.Evaluate.GetResult, String) & CType(rightNode.Evaluate.GetResult, String), TypeEnum.Str)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(CType(leftNode.Evaluate.GetResult, String) & CType(rightNode.Evaluate.GetResult, String), TypeEnum.Str)
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.ANDPERSTAND
    End Function
    Public Overrides ReadOnly Property OpName As String
        Get
            Return _opname
        End Get
    End Property

    Public Overrides ReadOnly Property OpString As String
        Get
            Return _opstring
        End Get
    End Property

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "Concatenate"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Strings
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "&"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Strings.xml"
        End Get
    End Property
End Class
