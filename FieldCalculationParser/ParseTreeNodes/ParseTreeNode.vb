Public MustInherit Class ParseTreeNode
    Protected Shared _ParseErrors As List(Of String)
    Protected Shared _ComputeErrors As List(Of String)
    Protected Shared _IsCaseSensitive As Boolean = False
    Protected Shared _RowOrCellNum As Integer = Nothing
    Protected _ParseError As String
    Protected _type As TypeEnum
    Public Shared Sub Initialize()
        _ParseErrors = New List(Of String)
        _ComputeErrors = New List(Of String)
    End Sub
    Public Shared Property IsCaseSensitive As Boolean
        Get
            Return _IsCaseSensitive
        End Get
        Set(value As Boolean)
            _IsCaseSensitive = value
        End Set
    End Property
    Public Shared Property RowOrCellNum As Integer
        Get
            Return _RowOrCellNum
        End Get
        Set(value As Integer)
            _RowOrCellNum = value
        End Set
    End Property
    Public ReadOnly Property GetParseErrors As List(Of String)
        Get
            Return _ParseErrors
        End Get
    End Property
    Public ReadOnly Property GetComputeErrors As List(Of String)
        Get
            Return _ComputeErrors
        End Get
    End Property
    Friend Sub AddParseError(message As String)
        _ParseErrors.Add(message)
    End Sub
    Public Overridable Function Simplify() As ParseTreeNode
        Return Me.Simplify
    End Function
    Public MustOverride Function containsVariable() As Boolean
    Public Function ContainsError() As Boolean
        If _type = TypeEnum.ERR Then Return True
        Return False
    End Function
    Public MustOverride Function GetErrorMessages() As List(Of String)
    Public MustOverride Function Evaluate() As ParseTreeNodeResult
    Public MustOverride Function EvaluateSingle() As ParseTreeNodeResult
    Public MustOverride Sub UpdateSingle(ByRef row As Single())
    Public MustOverride Sub Update(ByRef row As Object()) 'how does this work with offset parameters?
    Public MustOverride Sub SetColNums(ByVal uniqueheaderNames As List(Of String))
    'Public MustOverride Sub SetColTypes(ByVal uniqueColTypes As List(Of Type)) 'must be ordered the same as the unique header names are.
    Public MustOverride Function ExpressionToString() As String
    Public Function Type() As TypeEnum
        Return _type
    End Function
    Public MustOverride Function Token() As TokenEnum
    Public MustOverride Function GetHeaderNames() As List(Of String)
End Class
