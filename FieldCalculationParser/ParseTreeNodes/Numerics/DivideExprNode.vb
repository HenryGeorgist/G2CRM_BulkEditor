Public Class DivideExprNode
    Inherits BinaryNumExprNode
    Private Const _OpName As String = "/"
    Private Const _OpString As String = "Divide"
    Public Sub New(left As ParseTreeNode, right As ParseTreeNode)
        MyBase.New(left, right)

    End Sub

    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim top As Double
        Double.TryParse(leftNode.Evaluate.GetResult.ToString, top)
        Dim bottom As Double
        Double.TryParse(rightNode.Evaluate.GetResult.ToString, bottom)
        If top = 0 Then Return New ParseTreeNodeResult(0, TypeEnum.Dub)
        If bottom = 0 Then
            If IsNothing(_RowOrCellNum) Then
                _ComputeErrors.Add("divide by zero")
            Else
                _ComputeErrors.Add("divide by zero on row " & _RowOrCellNum)
            End If
            'shouldnt i know which row this is on? where is that handled?
            Return New ParseTreeNodeResult(Double.NaN, TypeEnum.Dub)
        End If
        'Throw New ArithmeticException("Divide by Zero")
        Return New ParseTreeNodeResult(top / bottom, TypeEnum.Dub)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim top As Single = leftNode.Evaluate.GetSingleResult
        Dim bottom As Single = rightNode.Evaluate.GetSingleResult
        If top = 0 Then Return New ParseTreeNodeResult(0.0F)
        If bottom = 0 Then
            If IsNothing(_RowOrCellNum) Then
                _ComputeErrors.Add("divide by zero")
            Else
                _ComputeErrors.Add("divide by zero on row " & _RowOrCellNum)
            End If
            'shouldnt i know which row this is on? where is that handled?
            Return New ParseTreeNodeResult(Single.NaN)
        End If
        'Throw New ArithmeticException("Divide by Zero")
        Return New ParseTreeNodeResult(top / bottom)
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.DIVIDE
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
