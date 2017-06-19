Public Class EQExprNode
    Inherits BinaryBoolExprNode
    Private Const _OpName As String = "="
    Private Const _OpString As String = "Equals"
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)
    End Sub
    Public Overrides Function ExpressionToString() As String
        Return "(" & leftNode.ExpressionToString & Me.OpName & rightNode.ExpressionToString & ")"
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim l As ParseTreeNodeResult = leftNode.Evaluate
        Dim r As ParseTreeNodeResult = rightNode.Evaluate
        If (l.Type And TypeEnum.Num) > 0 And (r.Type And TypeEnum.Num) > 0 Then
            Dim lnum As Double
            Double.TryParse(l.GetResult.ToString, lnum)
            Dim rnum As Double
            Double.TryParse(r.GetResult.ToString, rnum)
            Return New ParseTreeNodeResult(lnum = rnum, TypeEnum.Bool)
        ElseIf (l.Type And TypeEnum.Bool) > 0 And (r.Type And TypeEnum.Bool) > 0 Then
            Dim lnum As Boolean
            Boolean.TryParse(l.GetResult.ToString, lnum)
            Dim rnum As Boolean
            Boolean.TryParse(r.GetResult.ToString, rnum)
            Return New ParseTreeNodeResult(lnum = rnum, TypeEnum.Bool)
        ElseIf (l.Type And TypeEnum.Str) > 0 And (r.Type And TypeEnum.Str) > 0 Then
            If _IsCaseSensitive Then
                If l.GetResult.ToString = r.GetResult.ToString Then
                    Return New ParseTreeNodeResult(True, TypeEnum.Bool)
                Else
                    Return New ParseTreeNodeResult(False, TypeEnum.Bool)
                End If
            Else
                If String.Equals(l.GetResult.ToString, r.GetResult.ToString, StringComparison.CurrentCultureIgnoreCase) Then
                    Return New ParseTreeNodeResult(True, TypeEnum.Bool)
                Else
                    Return New ParseTreeNodeResult(False, TypeEnum.Bool)
                End If
            End If
        Else
            'unknown?
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If

    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim l As ParseTreeNodeResult = leftNode.Evaluate
        Dim r As ParseTreeNodeResult = rightNode.Evaluate
        If (l.Type And TypeEnum.Num) > 0 And (r.Type And TypeEnum.Num) > 0 Then
            Dim lnum As Double
            Double.TryParse(l.GetResult.ToString, lnum)
            Dim rnum As Double
            Double.TryParse(r.GetResult.ToString, rnum)
            Return New ParseTreeNodeResult(lnum = rnum, TypeEnum.Bool)
        ElseIf (l.Type And TypeEnum.Bool) > 0 And (r.Type And TypeEnum.Bool) > 0 Then
            Dim lnum As Boolean
            Boolean.TryParse(l.GetResult.ToString, lnum)
            Dim rnum As Boolean
            Boolean.TryParse(r.GetResult.ToString, rnum)
            Return New ParseTreeNodeResult(lnum = rnum, TypeEnum.Bool)
        ElseIf (l.Type And TypeEnum.Str) > 0 And (r.Type And TypeEnum.Str) > 0 Then
            If _IsCaseSensitive Then
                If l.GetResult.ToString = r.GetResult.ToString Then
                    Return New ParseTreeNodeResult(True, TypeEnum.Bool)
                Else
                    Return New ParseTreeNodeResult(False, TypeEnum.Bool)
                End If
            Else
                If String.Equals(l.GetResult.ToString, r.GetResult.ToString, StringComparison.CurrentCultureIgnoreCase) Then
                    Return New ParseTreeNodeResult(True, TypeEnum.Bool)
                Else
                    Return New ParseTreeNodeResult(False, TypeEnum.Bool)
                End If
            End If
        Else
            'unknown?
            Return New ParseTreeNodeResult(False, TypeEnum.Bool)
        End If

    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.EQ
    End Function

    Public Overrides ReadOnly Property OpName As String
        Get
            Return _OpName
        End Get
    End Property

    Public Overrides ReadOnly Property OpString As String
        Get
            Return _OpString
        End Get
    End Property
End Class
