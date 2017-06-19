Public MustInherit Class BinaryBoolExprNode
    Inherits BoolExprNode
    Protected leftNode As ParseTreeNode
    Protected rightNode As ParseTreeNode
    'Private _ParseError As String
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        _type = TypeEnum.Bool
        Me.leftNode = left
        Me.rightNode = right
        If IsNothing(left) Then
            _ParseError = "There is no left side to the operation " & OpString
            _type = TypeEnum.ERR
            _ParseErrors.Add(_ParseError)
            leftNode = New ErrorNode(TypeEnum.Bool, "Empty", "")
        Else
            If IsNothing(rightNode) Then
                _ParseError = "There is no right side to the operation " & OpString & " with lefthandside of: " & leftNode.ExpressionToString
                _type = TypeEnum.ERR
                _ParseErrors.Add(_ParseError)
                rightNode = New ErrorNode(TypeEnum.Bool, "Empty", "")
            Else
                If (leftNode.Type And TypeEnum.Bool) > 0 And (rightNode.Type And TypeEnum.Bool) > 0 Then
                ElseIf OpName = "=" Then
                ElseIf OpName = "!=" Then
                ElseIf (leftNode.Type And TypeEnum.Num) > 0 And (rightNode.Type And TypeEnum.Num) > 0 Then
                    'ElseIf (left.Type And right.Type And TypeEnum.Str) > 0 Then
                Else
                    If leftNode.Type = TypeEnum.ERR Then
                    ElseIf right.Type = TypeEnum.ERR Then
                    Else
                        _ParseError = "cannot evaluate operation " & OpString & " with type " & leftNode.Type.ToString & " and " & rightNode.Type.ToString
                        _type = TypeEnum.ERR
                        _ParseErrors.Add(_ParseError)
                    End If
                    _type = TypeEnum.ERR
                End If
            End If
        End If
    End Sub
    Public Overrides Function containsVariable() As Boolean
        If leftNode.containsVariable Or rightNode.containsVariable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Function ExpressionToString() As String
        If IsNothing(leftNode) Then
            Return Me.OpName & "(,)"
        Else
            If IsNothing(rightNode) Then
                Return Me.OpName & "(" & leftNode.ExpressionToString & ",)"
            Else
                Return Me.OpName & "(" & leftNode.ExpressionToString & "," & rightNode.ExpressionToString & ")"
            End If
        End If
        Return Me.OpName & "(" & leftNode.ExpressionToString & "," & rightNode.ExpressionToString & ")"
    End Function
    Public Overrides Function Simplify() As ParseTreeNode
        'this needs work.
        If Me.containsVariable Then
            Return Me
        Else
            Return New BoolNode(Me.Evaluate.GetResult.ToString)
        End If
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        leftNode.SetColNums(uniqueheaderNames)
        rightNode.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Sub Update(ByRef row() As Object)
        leftNode.Update(row)
        rightNode.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        leftNode.UpdateSingle(row)
        rightNode.UpdateSingle(row)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(leftNode.GetHeaderNames)
        ret.AddRange(rightNode.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        If Not _ParseError = "" Then ret.Add(_ParseError)
        If Not IsNothing(leftNode) Then ret.AddRange(leftNode.GetErrorMessages)
        If Not IsNothing(rightNode) Then ret.AddRange(rightNode.GetErrorMessages)
        Return ret
    End Function
    Public MustOverride ReadOnly Property OpName As String
    Public MustOverride ReadOnly Property OpString As String
End Class
