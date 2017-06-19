Public Class ContainsExprNode
    Inherits BoolExprNode
    Implements IDisplayToTreeNode
    Private _expr1 As ParseTreeNode
    Private _expr2 As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal item1 As ParseTreeNode, ByVal item2 As ParseTreeNode)
        _type = TypeEnum.Bool
        _expr1 = item1
        _expr2 = item2
        If IsNothing(_expr1) Then
            _type = TypeEnum.ERR
            _ParseError = "The Contains expression does not contain an input value for text1"
            _ParseErrors.Add(_ParseError)
            _expr1 = New ErrorNode(TypeEnum.Str, "Empty", "")
        Else
            If IsNothing(_expr2) Then
                _type = TypeEnum.ERR
                _ParseError = "The Contains expression does not contain an input value for text2"
                _ParseErrors.Add(_ParseError)
                _expr2 = New ErrorNode(TypeEnum.Str, "Empty", "")
            Else
                If _expr1.Type = TypeEnum.ERR Then _type = TypeEnum.ERR
                If _expr2.Type = TypeEnum.ERR Then _type = TypeEnum.ERR
            End If

        End If

    End Sub
    Public Overrides Function containsVariable() As Boolean
        If _expr1.containsVariable Then Return True
        If _expr2.containsVariable Then Return True
        Return False
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        If _expr1.Evaluate.GetResult.ToString.Length < _expr2.Evaluate.GetResult.ToString.Length Then Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        If _IsCaseSensitive Then
            Return New ParseTreeNodeResult(CType(_expr1.Evaluate.GetResult, String).Contains(_expr2.Evaluate.GetResult.ToString), TypeEnum.Bool)
        Else
            Return New ParseTreeNodeResult(CType(_expr1.Evaluate.GetResult.ToString.ToUpper, String).Contains(_expr2.Evaluate.GetResult.ToString.ToUpper), TypeEnum.Bool)
        End If

    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        If _expr1.Evaluate.GetResult.ToString.Length < _expr2.Evaluate.GetResult.ToString.Length Then Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        If _IsCaseSensitive Then
            Return New ParseTreeNodeResult(CType(_expr1.Evaluate.GetResult, String).Contains(_expr2.Evaluate.GetResult.ToString), TypeEnum.Bool)
        Else
            Return New ParseTreeNodeResult(CType(_expr1.Evaluate.GetResult.ToString.ToUpper, String).Contains(_expr2.Evaluate.GetResult.ToString.ToUpper), TypeEnum.Bool)
        End If

    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_expr1.GetHeaderNames)
        ret.AddRange(_expr2.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        l.Add(_ParseError)
        If Not IsNothing(_expr1) Then l.AddRange(_expr1.GetErrorMessages)
        If Not IsNothing(_expr2) Then l.AddRange(_expr2.GetErrorMessages)
        Return l
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _expr1.SetColNums(uniqueheaderNames)
        _expr2.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.CONTAINS
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "Contains(" & _expr1.ExpressionToString & "," & _expr2.ExpressionToString & ")"
    End Function
    Public Overloads Overrides Sub Update(ByRef row() As Object)
        _expr1.Update(row)
        _expr2.Update(row)
    End Sub
    Public Overloads Overrides Sub UpdateSingle(ByRef row() As Single)
        _expr1.UpdateSingle(row)
        _expr2.UpdateSingle(row)
    End Sub
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "Contains"
        End Get
    End Property
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Booleans
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "Contains(,)"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Booleans.xml"
        End Get
    End Property
End Class
