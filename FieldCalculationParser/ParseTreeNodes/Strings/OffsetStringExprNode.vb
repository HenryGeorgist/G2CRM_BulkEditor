Public Class OffsetStringExprNode
    Inherits StringExprNode
    Private _expr As HeaderContainer
    Private _offset As Integer
    Private _data As List(Of String)
    Sub New(ByVal expr As HeaderContainer, ByVal offset As NegationNode)
        _expr = expr
        If (_expr.Type And TypeEnum.Str) > 0 Then
            _type = TypeEnum.Str
        Else
            _ParseError = "The field selected produces a non text value, but is being evaluated in a text expression"
            _ParseErrors.Add(_ParseError)
            _type = TypeEnum.ERR
        End If
        _offset = CInt(offset.Evaluate.GetResult)
        If _offset > 0 Then
            'Throw New ArgumentException("offset cannot be more than zero (it cannot look forward, only backward)")
            _ParseErrors.Add("offset cannot be more than zero (it cannot look forward, only backward)")
            _offset = 0
        End If

        _offset = Math.Abs(_offset) 'shhh dont tell anyone.
        _data = New List(Of String)
        For i = 0 To _offset - 1
            _data.Add("") 'not sure how this will work
        Next
    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim ret As New ParseTreeNodeResult(_data(0), TypeEnum.Str)
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
        Return "Offset(" & _expr.ToString & ", -" & _offset.ToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _expr.Update(row)
        _data.Add(_expr.Evaluate.GetResult)
    End Sub
End Class
