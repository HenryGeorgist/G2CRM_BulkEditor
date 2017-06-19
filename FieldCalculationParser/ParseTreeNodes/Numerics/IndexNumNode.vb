Public Class IndexNumNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _string As String
    Sub New()

    End Sub
    Sub New(ByVal str As String)
        _type = TypeEnum.Int
        If IsNothing(str) Then
            _ParseError = "There is no string in the index operator, please add any string value"
            _type = TypeEnum.ERR
            _string = ""
            _ParseErrors.Add(_ParseError)
        Else
            _string = str
        End If

    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_RowOrCellNum, TypeEnum.Num)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_RowOrCellNum)
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        Return ret
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.INDEX
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "{" & _string & "}"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        ' _value = RoworCellNum
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        ' _value = RoworCellNum
    End Sub
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "Index"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Numerics
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "{i}"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Numerics.xml"
        End Get
    End Property
End Class
