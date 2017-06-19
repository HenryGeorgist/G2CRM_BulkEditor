Public Class IfExprNode
    Inherits ParseTreeNode
    Implements IDisplayToTreeNode
    Private _thentype As TypeEnum
    Private _thenexpr As ParseTreeNode
    Private _elseexpr As ParseTreeNode
    Private _expr As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal expr As ParseTreeNode)
        _expr = expr
        _type = TypeEnum.Bool
        If IsNothing(_expr) Then
            _type = TypeEnum.ERR
            _ParseError = "the [Condition] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is nothing"
            _ParseErrors.Add(_ParseError)
            _expr = New ErrorNode(TypeEnum.Bool, "Empty", "")
        Else
            If _expr.Type <> TypeEnum.Bool Then
                _type = TypeEnum.ERR
                _ParseError = "the [Condition] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is not a logical statement"
                _ParseErrors.Add(_ParseError)
            End If
        End If
    End Sub
    Public WriteOnly Property SetThen As ParseTreeNode
        Set(value As ParseTreeNode)
            _thenexpr = value
            If IsNothing(value) Then
                _ParseError = "the [Then_Expression] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is nothing"
                If IsNothing(_expr) Then _ParseError = "the [Condition] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is nothing"
                _type = TypeEnum.ERR
                _ParseErrors.Add(_ParseError)
                _thenexpr = New ErrorNode(TypeEnum.Bool, "Empty", "")
            Else
                _thentype = value.Type
            End If

        End Set
    End Property
    Public WriteOnly Property SetElse As ParseTreeNode
        Set(value As ParseTreeNode)
            _elseexpr = value
            If IsNothing(value) Then
                _ParseError = "the [Else_Expression] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is nothing"
                If IsNothing(_thenexpr) Then _ParseError = "the [Then_Expression] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is nothing"
                If IsNothing(_expr) Then _ParseError = "the [Condition] of the if expression if([Condition],[Then_Expression],[Else_Expression]) is nothing"
                _type = TypeEnum.ERR
                _ParseErrors.Add(_ParseError)
            Else
                If value.Type <> _thentype Then
                    If (value.Type And TypeEnum.Num) > 0 And (_thentype And TypeEnum.Num) > 0 Then
                        'they are both numerical
                        If (value.Type And TypeEnum.integral) > 0 And (_thentype And TypeEnum.integral) > 0 Then
                            _type = TypeEnum.integral
                        Else
                            _type = TypeEnum.Deciml 'if they are both decimal, decimal, if one is decimal, decimal.
                        End If
                    Else
                        'they are not both numerical
                        _ParseError = "The else statement is of type " & value.Type.ToString & ", the then statement is of type " & _thentype.ToString & ". This Expression is invalid"
                        _ParseErrors.Add(_ParseError)
                        'Throw New Exception()
                        _type = TypeEnum.ERR
                    End If

                Else
                    _type = value.Type
                End If
            End If

        End Set
    End Property
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim con As Boolean
        Boolean.TryParse(_expr.Evaluate.GetResult.ToString, con)
        If con Then
            Return _thenexpr.Evaluate 'may need to determine the type of treenode and evaluate the correct expression to return.
        Else
            Return _elseexpr.Evaluate
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim con As Boolean
        Boolean.TryParse(_expr.Evaluate.GetResult.ToString, con)
        If con Then
            Return _thenexpr.EvaluateSingle 'may need to determine the type of treenode and evaluate the correct expression to return.
        Else
            Return _elseexpr.EvaluateSingle
        End If
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "IF(" & _expr.ExpressionToString() & "," & _thenexpr.ExpressionToString & "," & _elseexpr.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _expr.Update(row)
        _thenexpr.Update(row)
        _elseexpr.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _expr.UpdateSingle(row)
        _thenexpr.UpdateSingle(row)
        _elseexpr.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.KIF
    End Function
    Public Overrides Function containsVariable() As Boolean
        If _expr.containsVariable Then
            Return True
        ElseIf _thenexpr.containsVariable Then
            Return True
        ElseIf _elseexpr.containsVariable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Function Simplify() As ParseTreeNode
        Dim ifnode As IfExprNode
        If _expr.containsVariable Then
            ifnode = New IfExprNode(_expr)
        Else
            ifnode = New IfExprNode(_expr.Simplify)
        End If
        If _thenexpr.containsVariable Then
            ifnode.SetThen = _thenexpr
        Else
            ifnode.SetThen = _thenexpr.Simplify
        End If
        If _elseexpr.containsVariable Then
            ifnode.SetElse = _elseexpr
        Else
            ifnode.SetElse = _elseexpr.Simplify
        End If
        Return ifnode
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _expr.SetColNums(uniqueheaderNames)
        _elseexpr.SetColNums(uniqueheaderNames)
        _thenexpr.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_expr.GetHeaderNames)
        ret.AddRange(_elseexpr.GetHeaderNames)
        ret.AddRange(_thenexpr.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If Not IsNothing(_expr) Then ret.AddRange(_expr.GetErrorMessages)
        If Not IsNothing(_thenexpr) Then ret.AddRange(_thenexpr.GetErrorMessages)
        If Not IsNothing(_elseexpr) Then ret.AddRange(_elseexpr.GetErrorMessages)
        Return ret
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "IF"
        End Get
    End Property
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Booleans
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "IF(,,)"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Booleans.xml"
        End Get
    End Property
End Class
