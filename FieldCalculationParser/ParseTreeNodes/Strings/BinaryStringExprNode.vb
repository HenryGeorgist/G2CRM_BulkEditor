Public MustInherit Class BinaryStringExprNode
    Inherits StringExprNode
    Protected leftNode As ParseTreeNode
    Protected rightNode As ParseTreeNode
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        'If left.Type <> right.Type Then Throw New Exception("cannot combine type: " & left.Type.ToString & " type " & right.Type.ToString) 'what if it is a string and a number?
        Me.leftNode = left
        Me.rightNode = right
        If IsNothing(leftNode) Then
            _ParseError = "There is no left side to the operation " & OpName
            _type = TypeEnum.ERR
            _ParseErrors.Add(_ParseError)
            leftNode = New ErrorNode(TypeEnum.Bool, "Empty", "")
        End If
        If IsNothing(rightNode) Then
            _ParseError = "There is no right side to the operation " & OpName & " with lefthandside of: " & leftNode.ExpressionToString
            _type = TypeEnum.ERR
            _ParseErrors.Add(_ParseError)
            rightNode = New ErrorNode(TypeEnum.Bool, "Empty", "")
        Else
            If leftNode.ContainsError Then _type = TypeEnum.ERR
            If rightNode.ContainsError Then _type = TypeEnum.ERR
        End If
    End Sub
    Public Overrides Function ExpressionToString() As String
        Return "(" & leftNode.ExpressionToString() & " " & opName & " " & rightNode.ExpressionToString() & ")"
    End Function
    Public Overrides Function containsVariable() As Boolean
        If leftNode.containsVariable Or rightNode.containsVariable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        leftNode.Update(row)
        rightNode.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        leftNode.UpdateSingle(row)
        rightNode.UpdateSingle(row)
    End Sub
    Public Overrides Function Simplify() As ParseTreeNode
        If Me.containsVariable Then
            Return Me
        Else
            Return New StringNode(Me.Evaluate.GetResult.ToString)
        End If
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        leftNode.SetColNums(uniqueheaderNames)
        rightNode.SetColNums(uniqueheaderNames)
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
        ret = leftNode.GetErrorMessages
        If Not IsNothing(rightNode) Then ret.AddRange(rightNode.GetErrorMessages)
        Return ret
    End Function
    Public MustOverride ReadOnly Property OpName As String
    Public MustOverride ReadOnly Property OpString As String
End Class
