Public Class RightExprNode
    Inherits StringExprNode
    Implements IDisplayToTreeNode
    Private _Str As ParseTreeNode
    Private _char As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal str As ParseTreeNode, ByVal characters As ParseTreeNode)
        _type = TypeEnum.Str
        _Str = str
        _char = characters
        If IsNothing(_Str) Then
            _type = TypeEnum.ERR
            _ParseError = "The text in the Right function was not specified"
            _ParseErrors.Add(_ParseError)
            _Str = New ErrorNode(TypeEnum.Str, "Empty", "")
        Else
            If IsNothing(_char) Then
                _type = TypeEnum.ERR
                _ParseError = "The number of characters in the Right function was not specified"
                _ParseErrors.Add(_ParseError)
                _char = New ErrorNode(TypeEnum.Str, "Empty", "")
            Else
                If (_char.Type And TypeEnum.Num) > 0 Then
                    If (_Str.Type And TypeEnum.Str) > 0 Then
                    ElseIf _Str.Type = TypeEnum.ERR Then
                        _type = TypeEnum.ERR
                        _ParseError = "The text portion of the right expression had some error"
                        _ParseErrors.Add(_ParseError)
                    Else
                        _ParseError = "The text portion of the right expression did not yeild a text result, it has been internally converted"
                        _Str = New ConvertToString(str)
                        _ParseErrors.Add(_ParseError)
                    End If
                Else
                    _type = TypeEnum.ERR
                    _ParseError = "The character portion of the right expression was not numerical, it must be an integer"
                    _ParseErrors.Add(_ParseError)
                End If
            End If
        End If
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult

        Return New ParseTreeNodeResult(Right(_Str.Evaluate.GetResult.ToString, CInt(_char.Evaluate.GetResult)), TypeEnum.Str)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult

        Return New ParseTreeNodeResult(Right(_Str.Evaluate.GetResult.ToString, CInt(_char.Evaluate.GetResult)), TypeEnum.Str)
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "RIGHT(" & Chr(34) & _Str.ExpressionToString & Chr(34) & "," & _char.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _Str.Update(row)
        _char.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _Str.UpdateSingle(row)
        _char.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.RIGHT
    End Function
    Public Overrides Function containsVariable() As Boolean
        If _Str.containsVariable Or _char.containsVariable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _Str.SetColNums(uniqueheaderNames)
        _char.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_Str.GetHeaderNames)
        ret.AddRange(_char.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If Not IsNothing(_Str) Then ret.AddRange(_Str.GetErrorMessages)
        If Not IsNothing(_char) Then ret.AddRange(_char.GetErrorMessages)
        Return ret
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "RIGHT"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Strings
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "Right(,)"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Strings.xml"
        End Get
    End Property
End Class
