Public Class NegationNode
    Inherits NumExprNode
    Private _expr As ParseTreeNode
    Sub New(ByVal Expr As ParseTreeNode)
        If (Expr.Type And TypeEnum.Num) > 0 Then
            _expr = Expr
        Else
            If Expr.Type = TypeEnum.ERR Then
                '_type = TypeEnum.ERR
                _expr = Expr
            Else
                _expr = New ErrorNode(TypeEnum.Num, "the expression " & Expr.ExpressionToString & " does not produce a numerical value", Expr.ExpressionToString)
            End If

        End If
        _type = _expr.Type
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        If _type = TypeEnum.Int Then
            Dim expr As Integer
            Integer.TryParse(_expr.Evaluate.GetResult.ToString, expr)
            Return New ParseTreeNodeResult(-expr, _type)
        Else
            Dim expr As Double
            Double.TryParse(_expr.Evaluate.GetResult.ToString, expr)
            Return New ParseTreeNodeResult(-expr, _type)
        End If

    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim l As Single = _expr.Evaluate.GetSingleResult
        Return New ParseTreeNodeResult(-l)
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "-(" & _expr.ExpressionToString() & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _expr.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _expr.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.MINUS
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return False
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _expr.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_expr.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_expr.GetErrorMessages)
        Return ret
    End Function
End Class
