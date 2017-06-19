Public MustInherit Class BinaryHeaderExprNode
    Inherits ParseTreeNode
    Protected leftNode As ParseTreeNode
    Protected rightNode As ParseTreeNode
    Protected opName As String
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        Me.leftNode = left
        Me.rightNode = right
        Select Case rightNode.GetType
            Case GetType(HeaderBoolCastExprNode)
                Select Case leftNode.GetType
                    Case GetType(HeaderBoolCastExprNode)
                        leftNode = leftNode
                    Case GetType(HeaderNumCastExprNode)
                        leftNode = New HeaderBoolCastExprNode(CType(leftNode, HeaderNumCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderStringCastExprNode)
                        leftNode = New HeaderBoolCastExprNode(CType(leftNode, HeaderStringCastExprNode).GetHeaderContainer)
                    Case GetType(BinaryHeaderExprNode)
                End Select
            Case GetType(HeaderNumCastExprNode)
                Select Case leftNode.GetType
                    Case GetType(HeaderBoolCastExprNode)
                        leftNode = New HeaderNumCastExprNode(CType(leftNode, HeaderBoolCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderNumCastExprNode)
                        leftNode = leftNode
                    Case GetType(HeaderStringCastExprNode)
                        leftNode = New HeaderNumCastExprNode(CType(leftNode, HeaderStringCastExprNode).GetHeaderContainer)
                    Case GetType(BinaryHeaderExprNode)
                End Select
            Case GetType(HeaderStringCastExprNode)
                Select Case leftNode.GetType
                    Case GetType(HeaderBoolCastExprNode)
                        leftNode = New HeaderStringCastExprNode(CType(leftNode, HeaderBoolCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderNumCastExprNode)
                        leftNode = New HeaderStringCastExprNode(CType(leftNode, HeaderNumCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderStringCastExprNode)
                        leftNode = leftNode
                    Case GetType(BinaryHeaderExprNode)
                End Select
            Case GetType(BinaryHeaderExprNode)
            Case GetType(NumExprNode)
                Select Case leftNode.GetType
                    Case GetType(HeaderBoolCastExprNode)
                        leftNode = New HeaderNumCastExprNode(CType(leftNode, HeaderBoolCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderNumCastExprNode)
                        leftNode = leftNode
                    Case GetType(HeaderStringCastExprNode)
                        leftNode = New HeaderNumCastExprNode(CType(leftNode, HeaderStringCastExprNode).GetHeaderContainer)
                    Case GetType(BinaryHeaderExprNode)
                End Select
            Case GetType(StringExprNode)
                Select Case leftNode.GetType
                    Case GetType(HeaderBoolCastExprNode)
                        leftNode = New HeaderStringCastExprNode(CType(leftNode, HeaderBoolCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderNumCastExprNode)
                        leftNode = New HeaderStringCastExprNode(CType(leftNode, HeaderNumCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderStringCastExprNode)
                        leftNode = leftNode
                    Case GetType(BinaryHeaderExprNode)
                End Select
            Case GetType(BoolExprNode)
                Select Case leftNode.GetType
                    Case GetType(HeaderBoolCastExprNode)
                        leftNode = leftNode
                    Case GetType(HeaderNumCastExprNode)
                        leftNode = New HeaderBoolCastExprNode(CType(leftNode, HeaderNumCastExprNode).GetHeaderContainer)
                    Case GetType(HeaderStringCastExprNode)
                        leftNode = New HeaderBoolCastExprNode(CType(leftNode, HeaderStringCastExprNode).GetHeaderContainer)
                    Case GetType(BinaryHeaderExprNode)
                End Select
        End Select
    End Sub

    Public Overrides Function containsVariable() As Boolean
        If leftNode.containsVariable Then Return True
        If rightNode.containsVariable Then Return True
        Return False
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(leftNode.GetHeaderNames)
        ret.AddRange(rightNode.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function Tostring() As String
        Return "(" & leftNode.Tostring() & " " & opName & " " & rightNode.Tostring() & ")"
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        leftNode.SetColNums(uniqueheaderNames)
        rightNode.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Sub SetColTypes(uniqueColTypes As System.Collections.Generic.List(Of System.Type))
        leftNode.SetColTypes(uniqueColTypes)
        rightNode.SetColTypes(uniqueColTypes)
    End Sub
    Public Overrides Sub Update(ByRef row() As Object, ByVal RoworCellNum As Integer)
        leftNode.Update(row, RoworCellNum)
        rightNode.Update(row, RoworCellNum)
    End Sub
End Class
