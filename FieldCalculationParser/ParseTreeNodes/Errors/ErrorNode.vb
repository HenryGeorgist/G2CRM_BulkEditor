Public Class ErrorNode
    Inherits ParseTreeNode
    Private _OutputType As TypeEnum
    Private _ErrorMessage As String
    Private _UserInput As String
    Sub New(ByVal expectedoutputtype As TypeEnum, ByVal ErrorMessage As String, ByVal userinput As String)
        _OutputType = expectedoutputtype
        _ErrorMessage = ErrorMessage
        _ParseError = ErrorMessage
        _ParseErrors.Add(_ParseError)
        _UserInput = userinput
        _type = TypeEnum.ERR
    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return False
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Select Case _OutputType
            Case TypeEnum.Bool
                Return New ParseTreeNodeResult(False, TypeEnum.ERR)
            Case TypeEnum.Num
                Return New ParseTreeNodeResult(0, TypeEnum.ERR)
            Case TypeEnum.Str
                Return New ParseTreeNodeResult("", TypeEnum.ERR)
            Case Else
                Return New ParseTreeNodeResult("error", TypeEnum.ERR)
        End Select
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(0.0F, TypeEnum.ERR)
    End Function
    Public Overrides Function GetHeaderNames() As List(Of String)
        Return Nothing
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As List(Of String))
        'do nothing
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.ERR
    End Function
    Public Overrides Function ExpressionToString() As String
        Return _UserInput
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        'do nothing
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        'do nothing
    End Sub
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        l.Add(_ErrorMessage)
        Return l
    End Function
End Class
