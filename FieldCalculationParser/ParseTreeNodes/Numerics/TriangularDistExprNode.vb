Public Class TriangularDistExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _RAND As ParseTreeNode
    Private _min As ParseTreeNode
    Private _mostlikely As ParseTreeNode
    Private _max As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal randparsenode As ParseTreeNode, ByVal min As ParseTreeNode, ByVal Mostlikely As ParseTreeNode, ByVal max As ParseTreeNode)
        _type = TypeEnum.Dub
        If IsNothing(randparsenode) Then
            _type = TypeEnum.ERR
            _ParseError = "The random number argument in the TriInv expression was nothing"
            _RAND = New ErrorNode(TypeEnum.Num, "Empty", "")
            _ParseErrors.Add(_ParseError)
        Else
            If IsNothing(min) Then
                _type = TypeEnum.ERR
                _ParseError = "The min argument in the TriInv expression was nothing"
                _min = New ErrorNode(TypeEnum.Num, "Empty", "")
                _ParseErrors.Add(_ParseError)
            Else
                If IsNothing(Mostlikely) Then
                    _type = TypeEnum.ERR
                    _mostlikely = New ErrorNode(TypeEnum.Num, "Empty", "")
                    _ParseError = "The most likely argument in the TriInv expression was nothing"
                    _ParseErrors.Add(_ParseError)
                Else
                    If IsNothing(max) Then
                        _type = TypeEnum.ERR
                        _max = New ErrorNode(TypeEnum.Num, "Empty", "")
                        _ParseError = "The max argument in the TriInv expression was nothing"
                        _ParseErrors.Add(_ParseError)
                    Else
                        If (randparsenode.Type And TypeEnum.Deciml) > 0 Then
                            _RAND = randparsenode
                            If _RAND.GetType = GetType(RandNumExprNode) Then
                            Else
                                If _RAND.containsVariable Then
                                    'all hell breaks loose.
                                Else
                                    'it is a constant and needs to be between zero and 1.
                                    Dim r As Double
                                    Double.TryParse(_RAND.Evaluate.GetResult.ToString, r)
                                    If r > 1 Then
                                        _type = TypeEnum.ERR
                                        _ParseError = "The random number evaluated to greater than 1"
                                        _ParseErrors.Add(_ParseError)
                                        _RAND = New NumNode(0.999999999999)
                                    End If

                                    If r < 0 Then
                                        _type = TypeEnum.ERR
                                        _ParseError = "The random number evaluated to less than than 0"
                                        _ParseErrors.Add(_ParseError)
                                        _RAND = New NumNode(0.00000000000001)
                                    End If
                                End If
                            End If
                        Else
                            _type = TypeEnum.ERR
                            _ParseError = "The random argument of this function does not produce a decimal"
                            _ParseErrors.Add(_ParseError)
                            _RAND = New NumNode(0.5)
                        End If

                        If (min.Type And TypeEnum.Num) > 0 Then
                            _min = min
                        Else
                            _type = TypeEnum.ERR
                            _ParseError = "The Min argument of this function is not a numerical value"
                            _ParseErrors.Add(_ParseError)
                        End If
                        If (Mostlikely.Type And TypeEnum.Num) > 0 Then
                            _mostlikely = Mostlikely
                        Else
                            _type = TypeEnum.ERR
                            _ParseError = "The mostlikely argument of this function is not a numerical value"
                            _ParseErrors.Add(_ParseError)
                        End If
                        If (max.Type And TypeEnum.Num) > 0 Then
                            _max = max
                        Else
                            _type = TypeEnum.ERR
                            _ParseError = "The Max argument of this funciton is not a numerical value"
                            _ParseErrors.Add(_ParseError)
                        End If
                        'headers cause problems, but i should check if min is less than the mostlikely and the max is greater than the most likely

                        If _max.containsVariable Or _min.containsVariable Or _mostlikely.containsVariable Then
                            'check at runtime?
                        Else
                            If _type = TypeEnum.ERR Then
                            Else
                                Dim ma As Double
                                Dim mi As Double
                                Dim ml As Double
                                Double.TryParse(_max.Evaluate.GetResult.ToString, ma)
                                Double.TryParse(_min.Evaluate.GetResult.ToString, mi)
                                Double.TryParse(_mostlikely.Evaluate.GetResult.ToString, ml)
                                If mi <= ml Then
                                Else
                                    _type = TypeEnum.ERR
                                    _ParseError = "The min argument is not less than or equal to the most likely argument"
                                    _ParseErrors.Add(_ParseError)
                                End If
                                If ma >= ml Then
                                Else
                                    _type = TypeEnum.ERR
                                    _ParseError = "The max argument is not greater than or equal to the most likely argument"
                                    _ParseErrors.Add(_ParseError)
                                End If
                            End If

                        End If
                    End If
                End If
            End If
        End If



    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim max As Double
        Dim min As Double
        Dim ml As Double
        Double.TryParse(_max.Evaluate.GetResult.ToString, max)
        Double.TryParse(_min.Evaluate.GetResult.ToString, min)
        Double.TryParse(_mostlikely.Evaluate.GetResult.ToString, ml)
        Dim r As Double
        Double.TryParse(_RAND.Evaluate.GetResult.ToString, r)
        Dim dist As New Statistics.Triangular(min, max, ml)
        Return New ParseTreeNodeResult(dist.getDistributedVariable(r), TypeEnum.Dub)
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim max As Double
        Dim min As Double
        Dim ml As Double
        Double.TryParse(_max.Evaluate.GetResult.ToString, max)
        Double.TryParse(_min.Evaluate.GetResult.ToString, min)
        Double.TryParse(_mostlikely.Evaluate.GetResult.ToString, ml)
        Dim r As Double
        Double.TryParse(_RAND.Evaluate.GetResult.ToString, r)
        Dim dist As New Statistics.Triangular(min, max, ml)
        Return New ParseTreeNodeResult(CSng(dist.getDistributedVariable(r)))
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "TRIINV(" & _RAND.ExpressionToString & "," & _min.ExpressionToString & "," & _mostlikely.ExpressionToString & "," & _max.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _RAND.Update(row)
        _min.Update(row)
        _max.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _RAND.UpdateSingle(row)
        _min.UpdateSingle(row)
        _max.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.TRIINV
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _RAND.SetColNums(uniqueheaderNames)
        _min.SetColNums(uniqueheaderNames)
        _max.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_min.GetHeaderNames)
        ret.AddRange(_max.GetHeaderNames)
        ret.AddRange(_mostlikely.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If Not IsNothing(_min) Then ret.AddRange(_min.GetErrorMessages)
        If Not IsNothing(_max) Then ret.AddRange(_max.GetErrorMessages)
        If Not IsNothing(_mostlikely) Then ret.AddRange(_mostlikely.GetErrorMessages)
        Return ret
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "TRIINV"
        End Get
    End Property
    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Statistics
        End Get
    End Property
    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "TriInv(,,,)"
        End Get
    End Property
    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Statistics.xml"
        End Get
    End Property
End Class
