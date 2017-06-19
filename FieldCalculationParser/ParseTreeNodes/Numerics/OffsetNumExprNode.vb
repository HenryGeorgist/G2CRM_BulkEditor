Public Class OffsetNumExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _expr As HeaderContainer
    Private _offset As Integer
    Private _data As List(Of Double)
    Sub New()

    End Sub
    Sub New(ByVal expr As HeaderContainer, ByVal offset As NegationNode)
        _expr = expr
        If (_expr.Type And TypeEnum.Num) > 0 Then
            'all is well
            _type = _expr.Type
        Else
            _ParseError = "The field selected produces a non numerical value, but is being evaluated in a numerical expression"
            _ParseErrors.Add(_ParseError)
            _type = TypeEnum.ERR
        End If
        _offset = CInt(offset.Evaluate.GetResult)
        If _offset > 0 Then
            'Throw New ArgumentException("offset cannot be more than zero (it cannot look forward, only backward)")
            _ParseError = "offset cannot be more than zero (it cannot look forward, only backward)"
            _ParseErrors.Add(_ParseError)
            _offset = 0
            _type = TypeEnum.ERR
        End If

        _offset = Math.Abs(_offset) 'shhh dont tell anyone.
        _data = New List(Of Double)
        For i = 0 To _offset - 1
            _data.Add(0) 'not sure how this will do
        Next
    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim ret As New ParseTreeNodeResult(_data(0), TypeEnum.Num)
        _data.RemoveAt(0)
        Return ret
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret = _expr.GetHeaderNames
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        ret = _expr.GetErrorMessages
        If _ParseError <> "" Then ret.Add(_ParseError)
        Return ret
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _expr.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.OFFSET
    End Function
    Public Overrides Function Tostring() As String
        Return "Offset(" & _expr.Tostring & ", -" & _offset.ToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _expr.Update(row)
        _data.Add(_expr.Evaluate.GetResult)
    End Sub

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "OFFSET"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Numerics
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "Offset(,)"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Numerics.xml"
        End Get
    End Property
End Class
