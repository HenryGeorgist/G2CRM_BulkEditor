Imports System.Windows.Controls
Imports System.IO

Public Class HelpTopic
    Private _TopicName As String
    Private _Keywords As List(Of String)
    Private _WebControl As WebBrowser
    Public ReadOnly Property GetKeyWords As List(Of String)
        Get
            Return _Keywords
        End Get
    End Property
    Public ReadOnly Property GetTopicName As String
        Get
            Return _TopicName
        End Get
    End Property
    Public ReadOnly Property GetWebControl As WebBrowser
        Get
            Return _WebControl
        End Get
    End Property
    Sub New(ByVal xel As XElement)
        _TopicName = xel.Attribute("name").Value
        _Keywords = csvtolist(xel.Attribute("keywords").Value, True)
        Dim AssemblyName As String = xel.Attribute("AssemblyName").Value
        Dim RootNamespace As String = xel.Attribute("RootNamespace").Value
        Dim TempPath As String = System.IO.Path.GetTempPath()
        If System.IO.Directory.Exists(TempPath & "HEC\" & AssemblyName) = False Then System.IO.Directory.CreateDirectory(TempPath & "HEC\" & AssemblyName)

        Dim HTMLFile As String = TempPath & "HEC\" & AssemblyName & "\" & xel.Value
        Dim assmb As System.Reflection.Assembly = System.Reflection.Assembly.Load(AssemblyName)
        If IsNothing(assmb) Then
            MsgBox("The referenced assembly name: " & AssemblyName & " could not be found")
            Exit Sub
        End If
        '
        If File.Exists(HTMLFile) = False Then
            Dim strm As System.IO.Stream = assmb.GetManifestResourceStream(RootNamespace & "." & xel.Value.ToString)
            If IsNothing(strm) Then
                Throw New ArgumentException(RootNamespace & "." & xel.Value.ToString & " could not be found")
            Else
                Using OutputFile As Stream = File.Create(HTMLFile)
                    strm.CopyTo(OutputFile)
                End Using
                strm.Close() ' shouldnt using close on its own?
            End If

        End If
        '
        Dim containsimages As Boolean = False
        For Each att As XAttribute In xel.Attributes
            If att.Name = "Images" Then containsimages = True
        Next
        Dim strings As String() = assmb.GetManifestResourceNames

        If containsimages Then
            Dim Images As List(Of String) = csvtolist(xel.Attribute("Images").Value)
            For Each Image As String In Images
                If Image = "" Then Continue For
                If File.Exists(TempPath & "HEC\" & AssemblyName & "\" & Image) = False Then
                    Using imgstrm As Stream = assmb.GetManifestResourceStream(RootNamespace & "." & Image)
                        Using OutputFile As Stream = File.Create(TempPath & "HEC\" & AssemblyName & "\" & Image)
                            imgstrm.CopyTo(OutputFile)
                        End Using
                    End Using
                End If
            Next
        End If
        '
        _WebControl = New WebBrowser
        _WebControl.Navigate(New Uri(HTMLFile))
        With _WebControl
            .Width = Double.NaN
            .Height = Double.NaN
            .VerticalAlignment = Windows.VerticalAlignment.Stretch
            .HorizontalAlignment = Windows.HorizontalAlignment.Stretch
            AddHandler .LoadCompleted, AddressOf BrowserLoaded
        End With
    End Sub
    Private Sub BrowserLoaded(sender As Object, e As System.Windows.Navigation.NavigationEventArgs)
        Dim doc As mshtml.IHTMLDocument2 = _WebControl.Document
    End Sub
    Private Function csvtolist(ByVal str As String, Optional ignorecase As Boolean = False) As List(Of String)
        Dim CSVList As New List(Of String)
        Dim strings() As String = Split(str, ",")
        For i = 0 To strings.Count - 1
            If ignorecase Then
                CSVList.Add(strings(i).ToLower)
            Else
                CSVList.Add(strings(i))
            End If
        Next
        Return CSVList
    End Function
End Class
