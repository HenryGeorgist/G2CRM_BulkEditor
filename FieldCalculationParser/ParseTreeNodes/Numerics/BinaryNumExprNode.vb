Public MustInherit Class BinaryNumExprNode
    Inherits NumExprNode
    Protected leftNode As ParseTreeNode
    Protected rightNode As ParseTreeNode
    'Protected opName As String
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        Me.leftNode = left
        Me.rightNode = right
        If IsNothing(leftNode) Then
            _ParseError = "There is no left side to the operation " & OpString
            _type = TypeEnum.ERR
            leftNode = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            If IsNothing(rightNode) Then
                _ParseError = "There is no right side to the operation " & OpString & " with lefthandside of: " & leftNode.ExpressionToString
                _type = TypeEnum.ERR
                rightNode = New ErrorNode(TypeEnum.Num, "Empty", "")
                _ParseErrors.Add(_ParseError)
            Else
                If (left.Type And TypeEnum.Num) > 0 And (right.Type And TypeEnum.Num) > 0 Then
                    If (left.Type And TypeEnum.integral) > 0 And (right.Type And TypeEnum.integral) > 0 Then
                        If OpString = "Divide" Then
                            _type = TypeEnum.Dub
                        Else
                            _type = TypeEnum.Int
                        End If

                    Else
                        _type = TypeEnum.Dub
                    End If
                Else
                    _ParseError = "cannot " & OpString & " type " & left.Type.ToString & " and type " & right.Type.ToString
                    _ParseErrors.Add(_ParseError)
                    _type = TypeEnum.ERR
                End If
            End If
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
    Public Overrides Function Simplify() As ParseTreeNode
        If Me.containsVariable Then
            If leftNode.containsVariable Then
            Else
                leftNode = leftNode.Simplify()
            End If
            If rightNode.containsVariable Then
            Else
                rightNode = rightNode.Simplify()
            End If
            Return Me
        Else
            Dim d As Double
            Double.TryParse(Me.Evaluate.GetResult.ToString, d)
            Return New NumNode(d)
        End If
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(leftNode.GetHeaderNames)
        ret.AddRange(rightNode.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        If _type = TypeEnum.ERR Then l.Add(_ParseError)
        If Not IsNothing(leftNode) Then l.AddRange(leftNode.GetErrorMessages)
        If Not IsNothing(rightNode) Then l.AddRange(rightNode.GetErrorMessages)
        Return l
    End Function
    Public MustOverride ReadOnly Property OpName As String
    Public MustOverride ReadOnly Property OpString As String
End Class
