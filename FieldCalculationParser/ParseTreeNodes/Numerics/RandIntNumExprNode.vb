Public Class RandIntNumExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _RAND As Random
    Private _seed As ParseTreeNode
    Private _isheader As Boolean = False
    Sub New()
        _type = TypeEnum.Int
        _RAND = New Random
        _seed = Nothing
    End Sub
    Sub New(ByVal seed As ParseTreeNode)
        _type = TypeEnum.Int
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
        If _ParseError <> "" Then _ParseErrors.Add(_ParseError)
        _seed = seed
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(_RAND.Next, TypeEnum.Int)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Return New ParseTreeNodeResult(CSng(_RAND.Next))
    End Function
    Public Overrides Function ExpressionToString() As String
        If IsNothing(_seed) Then
            Return "RANDINT()"
        Else
            Return "RANDINT(" & _seed.ExpressionToString & ")"
        End If

    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        If IsNothing(_seed) Then Exit Sub
        If _seed.Token = TokenEnum.HEADER Then
            _seed.Update(row)
            Dim s As Integer
            Integer.TryParse(_seed.Evaluate.GetResult.ToString, s)
            _RAND = New Random(s)
        End If
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        If IsNothing(_seed) Then Exit Sub
        If _seed.Token = TokenEnum.HEADER Then
            _seed.UpdateSingle(row)
            Dim s As Integer
            Integer.TryParse(_seed.Evaluate.GetSingleResult.ToString, s)
            _RAND = New Random(s)
        End If
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.RANDINT
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        If IsNothing(_seed) Then Exit Sub
        _seed.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        If IsNothing(_seed) Then
        Else
            ret.AddRange(_seed.GetHeaderNames)
        End If
        Return ret
    End Function
    Public Overrides Function Geterrormessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If IsNothing(_seed) Then
        Else
            ret.AddRange(_seed.GetErrorMessages)
        End If
        Return ret
    End Function
    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "RANDINT"
        End Get
    End Property
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Statistics
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "RANDINT()"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Statistics.xml"
        End Get
    End Property
End Class
