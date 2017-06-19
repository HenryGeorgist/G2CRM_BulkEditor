Public Class ConvertToBoolean
    Inherits ParseTreeNode
    Implements IDisplayToTreeNode
    Private _node As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal ParseTreeNode As ParseTreeNode)
        If IsNothing(ParseTreeNode) Then
            _ParseError = "There is no inner expression"
            _type = TypeEnum.ERR
            _node = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            _type = TypeEnum.Bool
            _node = ParseTreeNode
            If _node.Type = TypeEnum.ERR Then _type = TypeEnum.ERR
        End If

    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return _node.containsVariable
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim ret As Boolean
        If Boolean.TryParse(_node.Evaluate.GetResult.ToString, ret) Then
            Return New ParseTreeNodeResult(ret, TypeEnum.Bool)
        Else
            _ComputeErrors.Add("The value: " & _node.Evaluate.GetResult.ToString & ", cannot be converted to Boolean, so the value False was used on row " & RowOrCellNum)
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim ret As Boolean
        If Boolean.TryParse(_node.Evaluate.GetResult.ToString, ret) Then
            Return New ParseTreeNodeResult(ret, TypeEnum.Bool)
        Else
            _ComputeErrors.Add("The value: " & _node.Evaluate.GetResult.ToString & ", cannot be converted to Boolean, so the value False was used on row " & RowOrCellNum)
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Return _node.GetHeaderNames
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _node.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.ConvertToBoolean
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "Bool(" & _node.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _node.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _node.UpdateSingle(row)
    End Sub
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        l = _node.GetErrorMessages
        Return l
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "ConvertToBoolean"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Converters
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "Bool()"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Converters.xml"
        End Get
    End Property
End Class
