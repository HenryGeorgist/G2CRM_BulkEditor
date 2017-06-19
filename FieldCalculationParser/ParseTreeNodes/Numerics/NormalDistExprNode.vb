Public Class NormalDistExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _RAND As ParseTreeNode
    Private _mean As ParseTreeNode
    Private _stdev As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal randparsenode As ParseTreeNode, ByVal mean As ParseTreeNode, ByVal stdev As ParseTreeNode)
        _type = TypeEnum.Num
        _RAND = randparsenode
        _mean = mean
        _stdev = stdev
        If IsNothing(_stdev) Then
            _type = TypeEnum.ERR
            _ParseError = "The stdev argument of the NormINV function was not specified"
            _stdev = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            If (_stdev.Type And TypeEnum.Num) = 0 Then
                _type = TypeEnum.ERR
                _ParseError = "The stdev argument of the NormINV function does not produce a numerical value"
                _ParseErrors.Add(_ParseError)
            End If
        End If
        If IsNothing(_mean) Then
            _type = TypeEnum.ERR
            _mean = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseError = "The mean argument of the NormINV function was not specified"
            _ParseErrors.Add(_ParseError)
        Else
            If (_mean.Type And TypeEnum.Num) = 0 Then
                _type = TypeEnum.ERR
                _ParseError = "The mean argument of the NormINV function does not produce a numerical value"
                _ParseErrors.Add(_ParseError)
            End If
        End If
        If IsNothing(_RAND) Then
            _type = TypeEnum.ERR
            _RAND = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseError = "The rand argument of the NormINV function was not specified"
            _ParseErrors.Add(_ParseError)
        Else
            If (_RAND.Type And TypeEnum.Num) = 0 Then
                _type = TypeEnum.ERR
                _ParseError = "The rand argument of the NormINV function does not produce a numerical value"
                _ParseErrors.Add(_ParseError)
            End If
        End If
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim mean As Double
        Double.TryParse(_mean.Evaluate.GetResult.ToString, mean)
        Dim stdev As Double
        Double.TryParse(_stdev.Evaluate.GetResult.ToString, stdev)
        Dim r As Double
        Double.TryParse(_RAND.Evaluate.GetResult.ToString, r)
        Dim dist As New Statistics.Normal(mean, stdev)
        Return New ParseTreeNodeResult(dist.getDistributedVariable(r), TypeEnum.Num)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim mean As Double
        Double.TryParse(_mean.Evaluate.GetResult.ToString, mean)
        Dim stdev As Double
        Double.TryParse(_stdev.Evaluate.GetResult.ToString, stdev)
        Dim r As Double
        Double.TryParse(_RAND.Evaluate.GetResult.ToString, r)
        Dim dist As New Statistics.Normal(mean, stdev)
        Return New ParseTreeNodeResult(CSng(dist.getDistributedVariable(r)))
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "NORMINV(" & _RAND.ExpressionToString & "," & _mean.ExpressionToString & "," & _stdev.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _RAND.Update(row)
        _mean.Update(row)
        _stdev.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _RAND.UpdateSingle(row)
        _mean.UpdateSingle(row)
        _stdev.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.NORMINV
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If Not IsNothing(_RAND) Then ret.AddRange(_RAND.GetErrorMessages)
        If Not IsNothing(_mean) Then ret.AddRange(_mean.GetErrorMessages)
        If Not IsNothing(_stdev) Then ret.AddRange(_stdev.GetErrorMessages)
        Return ret
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _RAND.SetColNums(uniqueheaderNames)
        _mean.SetColNums(uniqueheaderNames)
        _stdev.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_mean.GetHeaderNames)
        ret.AddRange(_stdev.GetHeaderNames)
        ret.AddRange(_RAND.GetHeaderNames)
        Return ret
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "NORMINV"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Statistics
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "NORMINV(,,)"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Statistics.xml"
        End Get
    End Property
End Class
