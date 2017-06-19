Public MustInherit Class BinaryErrorExprNode
    Inherits ParseTreeNode
    Protected leftNode As ParseTreeNode
    Protected rightNode As ParseTreeNode
    Private _tok As TokenEnum = TokenEnum.ERR
    'Protected opName As String
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        If (left.Type And right.Type And TypeEnum.Num) > 0 Then
            _type = TypeEnum.Num
        Else
            _type = TypeEnum.ERR
            _ParseError = "cannot " & OpString & " type " & left.Type.ToString & " and type " & right.Type.ToString
            _ParseErrors.Add(_ParseError)

        End If

        Me.leftNode = left
        Me.rightNode = right
    End Sub
    Public Overrides Function ExpressionToString() As String
        Return "(" & leftNode.ExpressionToString() & " " & OpName & " " & rightNode.ExpressionToString() & ")"
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
    Public MustOverride ReadOnly Property OpName As String
    Public MustOverride ReadOnly Property OpString As String
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        l.AddRange(leftNode.GetErrorMessages)
        l.AddRange(rightNode.GetErrorMessages)
        Return l
    End Function

    Public Overrides Function Token() As TokenEnum
        Return _tok
    End Function
End Class
