Public Class OffsetExprNode
    Inherits ParseTreeNode
    Implements IDisplayToTreeNode
    Private _expr As ParseTreeNode
    Private _offset As Integer
    Private _data As List(Of ParseTreeNodeResult) ' probably should be a stack.
    Sub New()

    End Sub
    Sub New(ByVal expr As ParseTreeNode, ByVal offset As ParseTreeNode)
        _expr = expr
        If IsNothing(_expr) Then
            _ParseError = "There is no header expression"
            _type = TypeEnum.ERR
            _expr = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        End If
        If IsNothing(offset) Then
            _ParseError = "There is no offset integer"
            _type = TypeEnum.ERR
            _offset = 0 'New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            _offset = CInt(offset.Evaluate.GetResult)
            If _offset > 0 Then
                _ParseErrors.Add("offset cannot be more than zero (it cannot look forward, only backward)")
                _offset = 0
            End If
            _offset = Math.Abs(_offset) 'shh dont tell anyone
        End If

        _data = New List(Of ParseTreeNodeResult)
        If (_expr.Type And TypeEnum.Bool) = 0 Then
            'If (_expr.Type = 0 AndAlso TypeEnum.Bool = 0) Then MsgBox("here")
            If (_expr.Type And TypeEnum.Num) = 0 Then
                If (_expr.Type And TypeEnum.Str) = 0 Then
                    _expr.AddParseError("the header doesnt have an appropriate type")
                    _type = TypeEnum.ERR
                Else
                    _type = _expr.Type
                    For i = 0 To _offset - 1
                        _data.Add(New ParseTreeNodeResult("", _expr.Type))
                    Next
                End If
            Else
                _type = _expr.Type
                For i = 0 To _offset - 1
                    _data.Add(New ParseTreeNodeResult(0, _expr.Type))
                Next
            End If
        Else
            _type = _expr.Type
            For i = 0 To _offset - 1
                _data.Add(New ParseTreeNodeResult(False, _expr.Type))
            Next
        End If
    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim ret As New ParseTreeNodeResult(_data(0).GetResult, _expr.Type)
        _data.RemoveAt(0)
        Return ret
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim ret As New ParseTreeNodeResult(_data(0).GetSingleResult)
        _data.RemoveAt(0)
        Return ret
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret = _expr.GetHeaderNames
        Return ret
    End Function
    Public Overrides Function Geterrormessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret = _expr.GetErrorMessages
        Return ret
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _expr.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.OFFSET
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "Offset(" & _expr.ExpressionToString & ", -" & _offset.ToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _expr.Update(row)
        _data.Add(_expr.Evaluate)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _expr.UpdateSingle(row)
        _data.Add(_expr.EvaluateSingle)
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
