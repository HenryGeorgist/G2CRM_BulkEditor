Public Class SubStringExprNode
    Inherits StringExprNode
    Implements IDisplayToTreeNode
    Private _Str As ParseTreeNode
    Private _startIndex As ParseTreeNode
    Private _length As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal str As ParseTreeNode, ByVal Lcharacters As ParseTreeNode, ByVal length As ParseTreeNode)
        _type = TypeEnum.Str
        _Str = str
        _startIndex = Lcharacters
        _length = length
        If IsNothing(_Str) Then
            _type = TypeEnum.ERR
            _ParseError = "the text in the Substring expression was not specified"
            _ParseErrors.Add(_ParseError)
            _Str = New ErrorNode(TypeEnum.Str, "Empty", "")
        Else
            If IsNothing(_startIndex) Then
                _type = TypeEnum.ERR
                _ParseError = "the start index in the Substring expression was not specified"
                _ParseErrors.Add(_ParseError)
                _startIndex = New ErrorNode(TypeEnum.Int, "Empty", "")
            Else
                If IsNothing(_length) Then
                    _type = TypeEnum.ERR
                    _ParseError = "the length in the Substring expression was not specified"
                    _ParseErrors.Add(_ParseError)
                    _length = New ErrorNode(TypeEnum.Str, "Empty", "")
                Else
                    If (_Str.Type And TypeEnum.Str) > 0 Then
                    Else
                        _type = TypeEnum.ERR
                        _ParseError = "the text in the Substring expression was not text"
                        _ParseErrors.Add(_ParseError)
                    End If
                    If (_startIndex.Type And TypeEnum.Num) > 0 Then
                    Else
                        _type = TypeEnum.ERR
                        _ParseError = "the start index in the Substring expression was not numerical"
                        _ParseErrors.Add(_ParseError)
                    End If
                    If (_length.Type And TypeEnum.Num) > 0 Then
                    Else
                        _type = TypeEnum.ERR
                        _ParseError = "the length in the Substring expression was not numerical"
                        _ParseErrors.Add(_ParseError)
                    End If
                End If
            End If
        End If
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim str As String = CType(_Str.Evaluate.GetResult, String)
        Dim start As Integer = CInt(_startIndex.Evaluate.GetResult)
        Dim len As Integer = CInt(_length.Evaluate.GetResult)
        If str.Length < start + len Then
            If IsNothing(_RowOrCellNum) Then
                _ComputeErrors.Add("The substring function had a start index and length that was larger than the string expression")
            Else
                _ComputeErrors.Add("The substring function had a start index and length that was larger than the string expression on row " & _RowOrCellNum)
            End If
            Return New ParseTreeNodeResult("", TypeEnum.ERR)
        Else

            Return New ParseTreeNodeResult(str.Substring(start, len), TypeEnum.Str)
        End If

    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim str As String = CType(_Str.Evaluate.GetResult, String)
        Dim start As Integer = CInt(_startIndex.Evaluate.GetResult)
        Dim len As Integer = CInt(_length.Evaluate.GetResult)
        If str.Length < start + len Then
            If IsNothing(_RowOrCellNum) Then
                _ComputeErrors.Add("The substring function had a start index and length that was larger than the string expression")
            Else
                _ComputeErrors.Add("The substring function had a start index and length that was larger than the string expression on row " & _RowOrCellNum)
            End If
            Return New ParseTreeNodeResult("", TypeEnum.ERR)
        Else

            Return New ParseTreeNodeResult(str.Substring(start, len), TypeEnum.Str)
        End If

    End Function
    Public Overrides Function ExpressionToString() As String
        Return "SUBSTRING (" & Chr(34) & _Str.ExpressionToString & Chr(34) & "," & _startIndex.ExpressionToString & ", " & _length.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _Str.Update(row)
        _startIndex.Update(row)
        _length.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _Str.UpdateSingle(row)
        _startIndex.UpdateSingle(row)
        _length.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.SUBSTRING
    End Function
    Public Overrides Function containsVariable() As Boolean
        If _Str.containsVariable Or _startIndex.containsVariable Or _length.containsVariable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _Str.SetColNums(uniqueheaderNames)
        _startIndex.SetColNums(uniqueheaderNames)
        _length.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_Str.GetHeaderNames)
        ret.AddRange(_startIndex.GetHeaderNames)
        ret.AddRange(_length.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If Not IsNothing(_Str) Then ret.AddRange(_Str.GetErrorMessages)
        If Not IsNothing(_startIndex) Then ret.AddRange(_startIndex.GetErrorMessages)
        If Not IsNothing(_length) Then ret.AddRange(_length.GetErrorMessages)
        Return ret
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "SUBSTRING"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Strings
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "SUBSTRING(,,)"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Strings.xml"
        End Get
    End Property
End Class
