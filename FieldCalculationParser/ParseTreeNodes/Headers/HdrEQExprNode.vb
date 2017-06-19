Public Class HdrEQExprNode
    Inherits BinaryHeaderExprNode
    Private _typesWork As Boolean = Nothing
    Private _type As TypeEnum
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)
        opName = "="
    End Sub
    Public Overrides Function ToString() As String
        Return "(" & leftNode.Tostring & Me.opName & rightNode.Tostring & ")"
    End Function
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim l As ParseTreeNodeResult = leftNode.Evaluate
        Dim r As ParseTreeNodeResult = rightNode.Evaluate
        If _typesWork = Nothing Then
            If l.Type = r.Type Then
                _typesWork = True
                Select Case l.Type
                    Case TypeEnum.Bool
                        _type = TypeEnum.Bool
                    Case TypeEnum.Doub
                        _type = TypeEnum.Doub
                    Case TypeEnum.Int
                        _type = TypeEnum.Int
                    Case TypeEnum.Str
                        _type = TypeEnum.Str
                    Case Else
                        _type = TypeEnum.Str
                End Select
            Else
                Throw New ArgumentException("The left side of the equation " & MyBase.Tostring & " did not produce the same type as the right side")
            End If
        Else
            'do nothing
        End If
        Select Case _type
            Case TypeEnum.Bool
                If l.GetBool = r.GetBool Then
                    Return New ParseTreeNodeResult(True)
                Else
                    Return New ParseTreeNodeResult(False)
                End If
            Case TypeEnum.Doub
                If l.GetNum = r.GetNum Then
                    Return New ParseTreeNodeResult(True)
                Else
                    Return New ParseTreeNodeResult(False)
                End If
            Case TypeEnum.Int
                If l.GetNum = r.GetNum Then
                    Return New ParseTreeNodeResult(True)
                Else
                    Return New ParseTreeNodeResult(False)
                End If
            Case TypeEnum.Str
                If l.GetStr = r.GetStr Then
                    Return New ParseTreeNodeResult(True)
                Else
                    Return New ParseTreeNodeResult(False)
                End If
            Case Else
                If l.GetStr = r.GetStr Then
                    Return New ParseTreeNodeResult(True)
                Else
                    Return New ParseTreeNodeResult(False)
                End If
        End Select


    End Function
    Public Overrides Function Type() As TypeEnum
        Return TypeEnum.Bool
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.EQ
    End Function
End Class
