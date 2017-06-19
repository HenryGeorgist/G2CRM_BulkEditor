Public Class InstringExprNode
    Inherits NumExprNode
    Implements IDisplayToTreeNode
    Private _Str As ParseTreeNode
    Private _searchCharacter As ParseTreeNode
    Sub New()

    End Sub
    Sub New(ByVal str As ParseTreeNode, ByVal character As ParseTreeNode)
        _type = TypeEnum.Int
        _Str = str
        _searchCharacter = character
        If IsNothing(_Str) Then
            _type = TypeEnum.ERR
            _ParseError = "the text in the instring expression was not specified"
            _ParseErrors.Add(_ParseError)
            _Str = New ErrorNode(TypeEnum.Str, "Empty", "")
        Else
            If IsNothing(_searchCharacter) Then
                _type = TypeEnum.ERR
                _ParseError = "the character to search in the Instring expression was not specified"
                _ParseErrors.Add(_ParseError)
                _searchCharacter = New ErrorNode(TypeEnum.Str, "Empty", "")
            Else
                If (_Str.Type And TypeEnum.Str) > 0 Then
                Else
                    _type = TypeEnum.ERR
                    _ParseError = "the text in the Substring expression was not text"
                    _ParseErrors.Add(_ParseError)
                End If

            End If
        End If
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        Dim str As String = CType(_Str.Evaluate.GetResult, String)
        Dim instr As String = CStr(_searchCharacter.Evaluate.GetResult)
        If str.Length < instr.Length Then
            If IsNothing(_RowOrCellNum) Then
                _ComputeErrors.Add("The instring function had a searchstring longer than the string expression")
            Else
                _ComputeErrors.Add("The instring function had a searchstring longer than the string expression on row " & _RowOrCellNum)
            End If
            Return New ParseTreeNodeResult(-1, TypeEnum.Int)
        Else

            Return New ParseTreeNodeResult(str.IndexOf(instr), TypeEnum.Int)
        End If

    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        Dim str As String = CType(_Str.Evaluate.GetResult, String)
        Dim instr As String = CStr(_searchCharacter.Evaluate.GetResult)
        If str.Length < instr.Length Then
            If IsNothing(_RowOrCellNum) Then
                _ComputeErrors.Add("The instring function had a searchstring longer than the string expression")
            Else
                _ComputeErrors.Add("The instring function had a searchstring longer than the string expression on row " & _RowOrCellNum)
            End If
            Return New ParseTreeNodeResult(-1.0F)
        Else

            Return New ParseTreeNodeResult(str.IndexOf(instr))
        End If

    End Function
    Public Overrides Function ExpressionToString() As String
        Return "INSTRING (" & Chr(34) & _Str.ExpressionToString & Chr(34) & "," & _searchCharacter.ExpressionToString & ")"
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        _Str.Update(row)
        _searchCharacter.Update(row)
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        _Str.UpdateSingle(row)
        _searchCharacter.UpdateSingle(row)
    End Sub
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.INSTRING
    End Function
    Public Overrides Function containsVariable() As Boolean
        If _Str.containsVariable Or _searchCharacter.containsVariable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        _Str.SetColNums(uniqueheaderNames)
        _searchCharacter.SetColNums(uniqueheaderNames)
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.AddRange(_Str.GetHeaderNames)
        ret.AddRange(_searchCharacter.GetHeaderNames)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim ret As New List(Of String)
        If _ParseError <> "" Then ret.Add(_ParseError)
        If Not IsNothing(_Str) Then ret.AddRange(_Str.GetErrorMessages)
        If Not IsNothing(_searchCharacter) Then ret.AddRange(_searchCharacter.GetErrorMessages)
        Return ret
    End Function

    Public ReadOnly Property DisplayName As String Implements IDisplayToTreeNode.DisplayName
        Get
            Return "INSTRING"
        End Get
    End Property

    Public ReadOnly Property DisplayType As DisplayTypes Implements IDisplayToTreeNode.DisplayType
        Get
            Return DisplayTypes.Strings
        End Get
    End Property

    Public ReadOnly Property FunctionSyntax As String Implements IDisplayToTreeNode.FunctionSyntax
        Get
            Return "INSTRING(,)"
        End Get
    End Property

    Public ReadOnly Property HelpFile As String Implements IDisplayToTreeNode.HelpFile
        Get
            Return "Strings.xml"
        End Get
    End Property
End Class
