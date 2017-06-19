Public Class RoundUpNumExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _val As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal val As ParseTreeNode)
        _val = val
        If IsNothing(_val) Then
            _type = TypeEnum.ERR
            _ParseError = "The value expression in the rounddown function was not specified"
            _val = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            If (_val.Type And TypeEnum.Num) > 0 Then
                _type = TypeEnum.Int
            Else
                _type = TypeEnum.ERR
                _ParseError = "The value expression in the rounddown function does not produce a numerical result"
                _ParseErrors.Add(_ParseError)
            End If
        End If
    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return _val.containsVariable
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(Math.Ceiling(CDbl(_val.Evaluate.GetResult)), TypeEnum.Int)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(CSng(Math.Ceiling(CDbl(_val.Evaluate.GetResult))))
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret = _val.GetHeaderNames
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.Add(_ParseError)
        If Not IsNothing(_val) Then ret.AddRange(_val.GetErrorMessages)
        Return ret
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _val.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.ROUNDUP
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "RoundUp(" & _val.ExpressionToString & ")"
    End Function

    Public Overrides Sub Update(ByRef row() As Object)
        _val.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _val.UpdateSingle(row)
    End Sub
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "ROUNDUP"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Numerics
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "ROUNDUP()"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Numerics.xml"
        End Get
    End Property
End Class
