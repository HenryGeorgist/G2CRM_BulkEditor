'Public Class HeaderNumCastExprNode
'    Inherits NumExprNode
'    Private _expr As HeaderContainer
'    Public Sub New(ByVal expr As HeaderContainer)
'        _expr = expr
'        '_expr.overrideexpectedtype = TypeEnum.Num
'    End Sub
'    Public ReadOnly Property GetHeaderContainer As HeaderContainer
'        Get
'            Return _expr
'        End Get
'    End Property
'    Public Overrides Function ToString() As String
'        Return _expr.Tostring()
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
'        Return TokenEnum.HEADER
'    End Function
'    Public Overrides Function containsVariable() As Boolean
'        Return True
'    End Function
'    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
'        _expr.SetColNums(uniqueheaderNames)
'    End Sub
'    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
'        Return _expr.GetHeaderNames
'    End Function
'End Class
