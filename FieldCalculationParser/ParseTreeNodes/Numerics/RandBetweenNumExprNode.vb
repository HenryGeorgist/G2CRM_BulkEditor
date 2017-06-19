Public Class RandBetweenNumExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _RAND As Random
    Private _seed As ParseTreeNode
    Private _max As ParseTreeNode
    Private _min As ParseTreeNode
    Private _isheader As Boolean = False
    Sub New()

    End Sub
    Sub New(ByVal min As ParseTreeNode, ByVal max As ParseTreeNode, Optional ByVal seed As ParseTreeNode = Nothing)
        _type = TypeEnum.Num
        If IsNothing(seed) Then
            _RAND = New Random
        Else
            If (seed.Type And TypeEnum.Num) > 0 Then
                If seed.Token = TokenEnum.HEADER Then
                    _isheader = True
                Else
                    Dim s As Integer
                    Integer.TryParse(seed.Evaluate.GetResult.ToString, s)
                    _RAND = New Random(s)
                End If
            Else
                _type = TypeEnum.ERR
                _ParseError = "The seed supplied does not produce numerical values"
                _RAND = New Random
            End If
        End If
        _seed = seed
        _min = min
        If IsNothing(_min) Then
            _type = TypeEnum.ERR : _ParseError = "The min value in the randbetween function was not defined"
            _min = New ErrorNode(TypeEnum.Num, "Empty", "")
        Else
            If (_min.Type And TypeEnum.Num) = 0 Then _type = TypeEnum.ERR : _ParseError = "The min value in the randbetween function did not produce a numerical output"
        End If
        _max = max
        If IsNothing(_max) Then
            _type = TypeEnum.ERR : _ParseError = "The max value in the randbetween function was not defined"
            _max = New ErrorNode(TypeEnum.Num, "Empty", "")
        Else
            If (_max.Type And TypeEnum.Num) = 0 Then _type = TypeEnum.ERR : _ParseError = "The max value in the randbetween function did not produce a numerical output"
        End If

        If _ParseError <> "" Then _ParseErrors.Add(_ParseError)
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim min As Double
        Double.TryParse(_min.Evaluate.GetResult.ToString, min)
        Dim max As Double
        Double.TryParse(_max.Evaluate.GetResult.ToString, max)
        Return New ParseTreeNodeResult(min + ((max - min) * _RAND.NextDouble), TypeEnum.Num)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim min As Single = _min.Evaluate.GetSingleResult
        Dim max As Single = _max.Evaluate.GetSingleResult
        Return New ParseTreeNodeResult(CSng(min + ((max - min) * _RAND.NextDouble)))
    End Function
    Public Overrides Function ExpressionToString() As String
        If IsNothing(_seed) Then
            Return "RANDBETWEEN(" & _min.ExpressionToString & "," & _max.ExpressionToString & ")"
        Else
            Return "RANDBETWEEN(" & _min.ExpressionToString & "," & _max.ExpressionToString & "," & _seed.ExpressionToString & ")"
        End If

    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        If _isheader Then
            _seed.Update(row)
            Dim s As Integer
            Integer.TryParse(_seed.Evaluate.GetResult.ToString, s)
            _RAND = New Random(s)
        End If
        _min.Update(row)
        _max.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        If _isheader Then
            _seed.UpdateSingle(row)
            Dim s As Integer
            Integer.TryParse(_seed.Evaluate.GetResult.ToString, s)
            _RAND = New Random(s)
        End If
        _min.UpdateSingle(row)
        _max.UpdateSingle(row)
    End Sub
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        If Not IsNothing(_seed) Then _seed.SetColNums(uniqueheaderNames)
        _min.SetColNums(uniqueheaderNames)
        _max.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.RANDBETWEEN
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_min.GetHeaderNames)
        ret.AddRange(_max.GetHeaderNames)
        If Not IsNothing(_seed) Then ret.AddRange(_seed.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        ret.AddRange(_min.GetErrorMessages)
        ret.AddRange(_max.GetErrorMessages)
        If Not IsNothing(_seed) Then ret.AddRange(_seed.GetErrorMessages)
        Return ret
    End Function
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "RANDBETWEEN"
        End Get
    End Property
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Statistics
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "RANDBETWEEN(,,)"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Statistics.xml"
        End Get
    End Property
End Class
