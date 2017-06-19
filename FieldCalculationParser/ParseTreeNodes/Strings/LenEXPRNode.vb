Public Class LenEXPRNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _Str As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal str As ParseTreeNode)
        _type = TypeEnum.Int
        _Str = str
        If IsNothing(_Str) Then
            _type = TypeEnum.ERR
            _ParseError = "the text in the len expression was not specified"
            _ParseErrors.Add(_ParseError)
            _Str = New ErrorNode(TypeEnum.Str, "Empty", "")
        Else
            If (_Str.Type And TypeEnum.Str) > 0 Then
                'good
            ElseIf _Str.Type = TypeEnum.ERR Then
                _type = TypeEnum.ERR
                _ParseError = "the string in the len expression contained some error"
                _ParseErrors.Add(_ParseError)
            Else
                _Str = New ConvertToString(str)
                _ParseError = "the string in the len expression was not recognized as a string, it has been converted to a string."
                _ParseErrors.Add(_ParseError)
            End If
        End If
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(Len(_Str.Evaluate.GetResult), TypeEnum.Int)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(Len(_Str.Evaluate.GetResult))
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "LEN(" & _Str.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _Str.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _Str.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.LEN
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return _Str.containsVariable
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _Str.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_Str.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function Geterrormessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.Add(_ParseError)
        If Not IsNothing(_Str) Then ret.AddRange(_Str.GetErrorMessages)
        Return ret
    End Function
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "LEN"
        End Get
    End Property
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Strings
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "LEN()"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Strings.xml"
        End Get
    End Property
End Class
