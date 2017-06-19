Public Class XML_Scanner
    Private _xmlReader As XElement
    Sub New(ByVal path As String)
        _xmlReader = XElement.Load(path)
    End Sub
    Sub New(ByVal stream As System.IO.Stream)
        _xmlReader = XElement.Load(stream)
    End Sub
    Public Function GetTitle() As String
        Dim titlestring As String = "Untitled"
        For Each att As XAttribute In _xmlReader.Attributes
            If att.Name = "title" Then titlestring = att.Value
        Next
        Return titlestring
        'Return _xmlReader.Attribute("title").Value
    End Function
    Public Function GetWidth() As Double
        Dim containswidth As Boolean = False
        Dim widthstring As String = ""
        For Each att As XAttribute In _xmlReader.Attributes
            If att.Name = "width" Then containswidth = True : widthstring = att.Value
        Next
        If containswidth Then
            Dim Width As Double
            If Double.TryParse(widthstring, Width) = True Then
                Return Width
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function
    Public Function GetHeight() As Double
        Dim containsheight As Boolean = False
        Dim heightstring As String = ""
        For Each att As XAttribute In _xmlReader.Attributes
            If att.Name = "height" Then containsheight = True : heightstring = att.Value
        Next
        If containsheight Then
            Dim height As Double
            If Double.TryParse(heightstring, height) = True Then
                Return height
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function
    Public Function GetDescription() As String
        Dim descriptionstring As String = ""
        For Each att As XAttribute In _xmlReader.Attributes
            If att.Name = "description" Then descriptionstring = att.Value
        Next
        Return descriptionstring
    End Function
    Public Function CreateListOfTopics() As List(Of HelpTopic)
        Dim lst As New List(Of HelpTopic)
        For Each ele As XElement In _xmlReader.Elements
            If ele.Name = "Topic" Then
                lst.Add(New HelpTopic(ele))
            End If
        Next
        Return lst
    End Function
End Class
