Imports System.Windows.Controls

Public Class HelpButton
    Private _xmlpath As String
    Private _AssemblyName As String = ""
    Private _RootNamespace As String = ""
    Private _PathIsResource As Boolean = False
    Private _TopicNames As String = ""
    Public Property TheButton As Button
        Get
            Return Button
        End Get
        Set(value As Button)
            Button = value
        End Set
    End Property

    Public Property TopicNames As String
        Get
            Return _TopicNames
        End Get
        Set(value As String)
            _TopicNames = value
        End Set
    End Property
    Public Property xmlPATH()
        Get
            Return _xmlpath
        End Get
        Set(value)
            _xmlpath = value
        End Set
    End Property
    Public Property PathIsEmbeddedResource As Boolean
        Get
            Return _PathIsResource
        End Get
        Set(value As Boolean)
            _PathIsResource = value
        End Set
    End Property
    Public Property AssemblyName As String
        Get
            Return _AssemblyName
        End Get
        Set(value As String)
            _AssemblyName = value
        End Set
    End Property
    Public Property RootNamespace As String
        Get
            Return _RootNamespace
        End Get
        Set(value As String)
            _RootNamespace = value
        End Set
    End Property
    Public Sub LaunchHelpDialog()
        If _xmlpath = "" Then MsgBox("No help Path was defined") : Exit Sub
        AddHandler HelpDialog.HelpNotFound, AddressOf CloseHD
        Dim hd As HelpDialog
        If _TopicNames = "" Then
            hd = New HelpDialog(_xmlpath, _AssemblyName, _RootNamespace)
        Else
            Dim Topics As List(Of String) = csvtolist(_TopicNames)
            hd = New HelpDialog(_xmlpath, Topics, _AssemblyName, _RootNamespace)
        End If
        hd.Owner = Windows.Window.GetWindow(Me)
        Try
            hd.Show()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub CloseHD(ByRef sender As HelpDialog)
        sender.Close()
    End Sub
    Private Sub Button_Click(sender As Object, e As Windows.RoutedEventArgs)
        LaunchHelpDialog()
    End Sub
    Private Sub HelpButton_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles HelpButton.Loaded
        If IsNothing(Me.ToolTip) OrElse Me.ToolTip = "" Then
            Me.ToolTip = "Get Help on " & Windows.Window.GetWindow(Me).Title
        End If
    End Sub
    Private Function csvtolist(ByVal str As String) As List(Of String)
        Dim CSVList As New List(Of String)
        Dim strings() As String = Split(str, ",")
        For i = 0 To strings.Count - 1
            CSVList.Add(strings(i))
        Next
        Return CSVList
    End Function
End Class
