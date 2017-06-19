Public Class ConvertToInteger
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
            'ElseIf (ParseTreeNode.Type And TypeEnum.Num) = 0 Then
            '    _ParseError = "The expression inside the funtion int() does not produuce a numerical value"
            '    _type = TypeEnum.ERR
            '    _node = New ErrorNode(TypeEnum.Int, "Cannot convert to number", ParseTreeNode.Tostring)
        Else
            _type = TypeEnum.Int
            _node = ParseTreeNode
            If _node.Type = TypeEnum.ERR Then _type = TypeEnum.ERR
        End If
    End Sub
    Public Overrides Function containsVariable() As Boolean
        Return _node.containsVariable
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim ret As Integer = 0
        Dim str As String = _node.Evaluate.GetResult.ToString
        If Integer.TryParse(str, ret) Then
            Return New ParseTreeNodeResult(ret, _type)
        ElseIf IsNumeric(str) Then
            Try
                Dim d As Double = 0
                Double.TryParse(str, d)
                ret = CInt(Math.Floor(d))
                Return New ParseTreeNodeResult(ret, _type)
            Catch ex As Exception
                _ComputeErrors.Add("the result " & _node.Evaluate.GetResult.ToString & " was not properly converted to integer, the result 0 was reported for row " & RowOrCellNum)
                Return New ParseTreeNodeResult(0, _type)
            End Try
        Else
            _ComputeErrors.Add("the result " & _node.Evaluate.GetResult.ToString & " was not properly converted to integer, the result 0 was reported for row " & RowOrCellNum)
            Return New ParseTreeNodeResult(ret, _type)
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim ret As Integer = 0
        Dim str As String = _node.Evaluate.GetResult.ToString
        If Integer.TryParse(str, ret) Then
            Return New ParseTreeNodeResult(ret, _type)
        ElseIf IsNumeric(str) Then
            Try
                Dim d As Double = 0
                Double.TryParse(str, d)
                ret = CInt(Math.Floor(d))
                Return New ParseTreeNodeResult(ret, _type)
            Catch ex As Exception
                _ComputeErrors.Add("the result " & _node.Evaluate.GetResult.ToString & " was not properly converted to integer, the result 0 was reported for row " & RowOrCellNum)
                Return New ParseTreeNodeResult(0, _type)
            End Try
        Else
            _ComputeErrors.Add("the result " & _node.Evaluate.GetResult.ToString & " was not properly converted to integer, the result 0 was reported for row " & RowOrCellNum)
            Return New ParseTreeNodeResult(ret, _type)
        End If
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Return _node.GetHeaderNames
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Return _node.GetErrorMessages
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _node.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.ConvertToInteger
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "Int(" & _node.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _node.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _node.UpdateSingle(row)
    End Sub
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "ConvertToInteger"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Converters
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "Int()"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Converters.xml"
        End Get
    End Property
End Class
