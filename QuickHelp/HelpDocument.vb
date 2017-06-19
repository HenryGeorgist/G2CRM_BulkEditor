Public Class HelpDocument
    Private _Topics As List(Of HelpTopic)
    Private _Title As String
    Private _Description As String
    Private _Width As Double
    Private _Height As Double
    Public ReadOnly Property Title As String
        Get
            Return _Title
        End Get
    End Property
    Public ReadOnly Property Description As String
        Get
            Return _Description
        End Get
    End Property
    Public ReadOnly Property Width As Double
        Get
            Return _Width
        End Get
    End Property
    Public ReadOnly Property Height As Double
        Get
            Return _Height
        End Get
    End Property
    Public ReadOnly Property GetTopics As List(Of HelpTopic)
        Get
            Return _Topics
        End Get
    End Property
    Sub New(ByVal XMLPath As String)
        Dim s As New XML_Scanner(XMLPath)
        _Topics = s.CreateListOfTopics
        _Title = s.GetTitle
        _Description = s.GetDescription
        _Width = s.GetWidth
        _Height = s.GetHeight
    End Sub
    Sub New(ByVal stream As System.IO.Stream)
        Dim s As New XML_Scanner(stream)
        _Topics = s.CreateListOfTopics
        _Title = s.GetTitle
        _Description = s.GetDescription
        _Width = s.GetWidth
        _Height = s.GetHeight
    End Sub
    Public Function GetTopicsByName(ByVal name As String) As HelpTopic
        For i = 0 To _Topics.Count - 1
            If _Topics(i).GetTopicName.ToLower = name.ToLower Then Return _Topics(i)
        Next
        Return Nothing
    End Function
    Public Function GetTopicIndexByName(ByVal name As String) As Integer
        For i = 0 To _Topics.Count - 1
            If _Topics(i).GetTopicName.ToLower = name.ToLower Then Return i
        Next
        Return -1
    End Function
    Public Function GetTopicsByKeyword(ByVal keyword As String) As List(Of HelpTopic)
        Dim ret As New List(Of HelpTopic)
        For i = 0 To _Topics.Count - 1
            If _Topics(i).GetKeyWords.Contains(keyword.ToLower) Then
                ret.Add(_Topics(i))
            End If
        Next
        Return ret
    End Function
End Class
