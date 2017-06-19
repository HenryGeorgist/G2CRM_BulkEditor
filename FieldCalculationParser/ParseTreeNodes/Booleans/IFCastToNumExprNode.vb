'Public Class IFCastToNumExprNode
'    Inherits NumExprNode
'    Private _expr As IfExprNode
'    Public Sub New(ByVal expr As IfExprNode)
'        _expr = expr
'        If expr.Type And TypeEnum.Num > 0 Then
'        Else
'            _ParseErrors.Add("This if expression is of type " & expr.Type.ToString & vbNewLine & "It cannot be converted to a number")
'        End If
'    End Sub
'    Public Overrides Function ToString() As String
'        Return _expr.ToString()
'    End Function
'    Public Overrides Function Evaluate() As ParseTreeNodeResult
'        Return _expr.Evaluate
'    End Function
'    Public Overrides Sub Update(ByRef row() As Object, ByVal RoworCellNum As Integer)
'        _expr.Update(row, RoworCellNum)
'    End Sub
'    Public Overrides Function Type() As TypeEnum
'        Return TypeEnum.Num
'    End Function
'    Public Overrides Function Token() As TokenEnum
'        Return TokenEnum.KIF
'    End Function
'    Public Overrides Function containsVariable() As Boolean
'        Return _expr.containsVariable
'    End Function
'    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
'        _expr.SetColNums(uniqueheaderNames)
'    End Sub
'    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
'        Dim ret As New List(Of String)
'        ret.AddRange(_expr.GetHeaderNames)
'        Return ret
'    End Function
'End Class
