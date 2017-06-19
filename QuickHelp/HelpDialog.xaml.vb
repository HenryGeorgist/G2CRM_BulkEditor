Class HelpDialog
    Private _document As HelpDocument
    Public Shared Event HelpNotFound(ByRef sender As HelpDialog)
    Public Sub New(ByVal xmlHelppath As String, ByVal AssemblyName As String, ByVal RootNamespace As String)
        InitializeComponent()
        Dim assmb As System.Reflection.Assembly = System.Reflection.Assembly.Load(AssemblyName)
        If IsNothing(assmb) Then
            MsgBox("The referenced assembly name: " & AssemblyName & " could not be found")
            RaiseEvent HelpNotFound(Me)
            Exit Sub
        End If

        Dim strm As System.IO.Stream = assmb.GetManifestResourceStream(RootNamespace & "." & xmlHelppath)
        If IsNothing(strm) Then
            MsgBox("The embedded resource file named: " & xmlHelppath & " could not be found")
            RaiseEvent HelpNotFound(Me)
            Exit Sub
        End If
        _document = New HelpDocument(strm)
        If _document.Title <> "" Then Me.Title = _document.Title
        'DocumentDescriptionRow.Height = New Windows.GridLength(0)

        If _document.Width > 0 Then Me.Width = _document.Width
        If _document.Height > 0 Then Me.Height = _document.Height
        For i = 0 To _document.GetTopics.Count - 1
            TopicList.Items.Add(_document.GetTopics(i).GetTopicName)
        Next
        If TopicList.Items.Count <= 1 Then
            'FilterText.Visibility = Windows.Visibility.Collapsed
            'FilterButton.Visibility = Windows.Visibility.Collapsed
            FilterStackPanel.Visibility = Windows.Visibility.Collapsed
            TopicList.Visibility = Windows.Visibility.Collapsed
            'TopicListRow.Height = New Windows.GridLength(1, Windows.GridUnitType.Auto)
        End If


    End Sub
    Public Sub New(ByVal xmlHelppath As String, ByVal TopicNames As List(Of String), ByVal AssemblyName As String, ByVal RootNamespace As String)
        InitializeComponent()
        Dim assmb As System.Reflection.Assembly = System.Reflection.Assembly.Load(AssemblyName)
        If IsNothing(assmb) Then
            MsgBox("The referenced assembly name: " & AssemblyName & " could not be found")
            RaiseEvent HelpNotFound(Me)
            Exit Sub
        End If
        Dim strm As System.IO.Stream = assmb.GetManifestResourceStream(RootNamespace & "." & xmlHelppath)
        If IsNothing(strm) Then
            MsgBox("The embedded resource file named: " & xmlHelppath & " could not be found")
            RaiseEvent HelpNotFound(Me)
            Exit Sub
        End If
        _document = New HelpDocument(strm)
        If _document.Title <> "" Then Me.Title = _document.Title
        'DocumentDescriptionRow.Height = New Windows.GridLength(0)
        If _document.Width > 0 Then Me.Width = _document.Width
        If _document.Height > 0 Then Me.Height = _document.Height
        For i As Int32 = 0 To TopicNames.Count - 1
            TopicList.Items.Add(_document.GetTopicsByName(TopicNames(i)).GetTopicName)
        Next
        If TopicList.Items.Count <= 1 Then
            'FilterText.Visibility = Windows.Visibility.Collapsed
            'FilterButton.Visibility = Windows.Visibility.Collapsed
            FilterStackPanel.Visibility = Windows.Visibility.Collapsed
            TopicList.Visibility = Windows.Visibility.Collapsed
            'TopicListRow.Height = New Windows.GridLength(1, Windows.GridUnitType.Auto)
        End If

    End Sub
    Public Sub New(ByVal xmlHelppath As String, TopicName As String, ByVal AssemblyName As String, ByVal RootNamespace As String)
        InitializeComponent()
        'FilterText.Visibility = Windows.Visibility.Collapsed
        'FilterButton.Visibility = Windows.Visibility.Collapsed
        FilterStackPanel.Visibility = Windows.Visibility.Collapsed
        TopicList.Visibility = Windows.Visibility.Collapsed
        'mygrid.RowDefinitions(0).Height = New Windows.GridLength(1, Windows.GridUnitType.Auto)
        'mygrid.RowDefinitions(1).Height = New Windows.GridLength(1, Windows.GridUnitType.Auto)
        Dim assmb As System.Reflection.Assembly = System.Reflection.Assembly.Load(AssemblyName)
        If IsNothing(assmb) Then
            MsgBox("The referenced assembly name: " & AssemblyName & " could not be found")
            RaiseEvent HelpNotFound(Me)
            Exit Sub
        End If
        Dim strm As System.IO.Stream = assmb.GetManifestResourceStream(RootNamespace & "." & xmlHelppath)
        If IsNothing(strm) Then
            MsgBox("The embedded resource file named: " & xmlHelppath & " could not be found")
            RaiseEvent HelpNotFound(Me)
            Exit Sub
        End If
        _document = New HelpDocument(strm)
        If _document.Title <> "" Then Me.Title = _document.Title
        'DocumentDescriptionRow.Height = New Windows.GridLength(0)
        If _document.Width > 0 Then Me.Width = _document.Width
        If _document.Height > 0 Then Me.Height = _document.Height
        For i = 0 To _document.GetTopics.Count - 1
            TopicList.Items.Add(_document.GetTopics(i).GetTopicName)
        Next
        If _document.GetTopicIndexByName(TopicName) = -1 Then
            MsgBox("Topic name not found")
            RaiseEvent HelpNotFound(Me)
            TopicList.SelectedIndex = 0
        Else
            TopicList.SelectedIndex = _document.GetTopicIndexByName(TopicName)
        End If
        'Me.SizeToContent = Windows.SizeToContent.Height
    End Sub
    Private Sub TopicList_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles TopicList.SelectionChanged
        If TopicList.SelectedIndex = -1 Then Exit Sub
        DisplayGrid.Children.Clear()
        DisplayGrid.Children.Add(_document.GetTopicsByName(TopicList.SelectedItem).GetWebControl)
    End Sub

    Private Sub FilterButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles FilterButton.Click
        If FilterText.Text = "" Or FilterText.Text = "Filter" Then
            TopicList.Items.Clear()
            For i = 0 To _document.GetTopics.Count - 1
                TopicList.Items.Add(_document.GetTopics(i).GetTopicName)
            Next
        End If
        Dim l As List(Of HelpTopic) = _document.GetTopicsByKeyword(FilterText.Text)
        If l.Count = 0 Then
            'result returned no matches
            TopicList.Items.Clear()
            For i = 0 To _document.GetTopics.Count - 1
                TopicList.Items.Add(_document.GetTopics(i).GetTopicName)
            Next
        Else
            TopicList.Items.Clear()
            For i = 0 To l.Count - 1
                TopicList.Items.Add(l(i).GetTopicName)
            Next
            TopicList.SelectedIndex = 0
        End If
    End Sub


    Private Sub HelpDialog_Loaded(sender As Object, e As Windows.RoutedEventArgs) Handles Me.Loaded
        If TopicList.Items.Count >= 1 AndAlso TopicList.SelectedIndex = -1 Then TopicList.SelectedIndex = 0
    End Sub
End Class