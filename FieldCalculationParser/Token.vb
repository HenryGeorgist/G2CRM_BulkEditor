Public Class Token
    Public tok As TokenEnum
    Public str As String
    Public linenumber As Int32
    Public pos As Int32

    Public Sub New(tok As TokenEnum, str As String, linenumber As Int32, pos As Int32)
        Me.tok = tok
        Me.str = str
        Me.linenumber = linenumber
        Me.pos = pos
    End Sub

    Public Overrides Function ToString() As String
        Return "Token(" & tok.ToString & ", " & str & ")"
    End Function
End Class

