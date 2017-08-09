Public Class HeaderContainer
    Inherits ParseTreeNode
    Private _name As String
    Private _Basetype As Type
    'Private _type As TypeEnum
    Private _colnum As Int32
    Private _result As Object
    Private _resultSingle As Single
    Sub New(ByVal Name As String, type As Type)
        _name = Name
        _Basetype = type
        Select Case _Basetype
            Case GetType(Boolean)
                _type = TypeEnum.Bool
            Case GetType(Double)
                _type = TypeEnum.Dub
            Case GetType(Single)
                _type = TypeEnum.Sng
            Case GetType(Long)
                _type = TypeEnum.Int
            Case GetType(Int64)
                _type = TypeEnum.Int
            Case GetType(Int32)
                _type = TypeEnum.Int
            Case GetType(Int16)
                _type = TypeEnum.Int
            Case GetType(Byte)
                _type = TypeEnum.byt
            Case GetType(String)
                _type = TypeEnum.Str
            Case Else
                'Throw New ArgumentException("this Type is not supported " & _type.ToString)
                _ParseError = "this Type is not supported " & _type.ToString
                _type = TypeEnum.ERR
        End Select
        _colnum = Nothing
    End Sub
    Public Overrides Function Evaluate() As ParseTreeNodeResult
        If IsNothing(_result) Then
            Select Case _type
                Case TypeEnum.Bool
                    Return New ParseTreeNodeResult(False, TypeEnum.ERR)
                Case TypeEnum.Num
                    Return New ParseTreeNodeResult(0, TypeEnum.ERR)
                Case TypeEnum.Str
                    Return New ParseTreeNodeResult("", TypeEnum.ERR)
                Case Else
                    Return New ParseTreeNodeResult("error", TypeEnum.ERR)
            End Select
        Else
            Return New ParseTreeNodeResult(_result, _type)
        End If
    End Function
    Public Overrides Function EvaluateSingle() As ParseTreeNodeResult
        If IsNothing(_result) Then
            Return New ParseTreeNodeResult("error", TypeEnum.ERR)
        Else
            Return New ParseTreeNodeResult(_resultSingle)
        End If
    End Function
    Public Overrides Function Token() As TokenEnum
        Return TokenEnum.HEADER
    End Function
    Public Overrides Function ExpressionToString() As String
        Return "[" & _name + "]"
    End Function
    Public Overrides Function containsVariable() As Boolean
        Return True
    End Function
    Public Overrides Sub Update(ByRef row() As Object)
        If IsNothing(_colnum) Then
            _ComputeErrors.Add("The header " & _name & " did not yeild a valid column number, no value can be set for this variable.")
            _result = Nothing
        Else
            _result = row(_colnum) 'was cstr()
        End If
    End Sub
    Public Overrides Sub UpdateSingle(ByRef row() As Single)
        If IsNothing(_colnum) Then
            _ComputeErrors.Add("The header " & _name & " did not yeild a valid column number, no value can be set for this variable.")
            _result = Nothing
        Else
            _result = row(_colnum) 'was cstr()
        End If
    End Sub
    Public Overrides Function Simplify() As ParseTreeNode
        Return Me
    End Function
    ''' <summary>
    ''' this must be called before set type
    ''' </summary>
    ''' <param name="uniqueheaderNames"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SetColNums(uniqueheaderNames As System.Collections.Generic.List(Of String))
        If uniqueheaderNames.Contains(_name) Then
            _colnum = uniqueheaderNames.IndexOf(_name)
        Else
            _colnum = Nothing
            _ParseError = "The header " & _name & " was not found in the list of headers, please check spelling and capitalization"
            AddParseError(_ParseError)
        End If
    End Sub
    Public Overrides Function GetHeaderNames() As System.Collections.Generic.List(Of String)
        Dim ret As New List(Of String)
        ret.Add(_name)
        Return ret
    End Function
    Public Overrides Function GetErrorMessages() As List(Of String)
        Dim l As New List(Of String)
        If _ParseError <> "" Then l.Add(_ParseError)
        Return l
    End Function
End Class
