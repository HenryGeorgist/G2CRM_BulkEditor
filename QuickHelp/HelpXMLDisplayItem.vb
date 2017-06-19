Public MustInherit Class HelpXMLDisplayItem
    Private _Header As System.Windows.Controls.TextBlock
    Protected _Control As System.Windows.UIElement
    Sub New(ByVal HeaderContent As String)
        _Header = New System.Windows.Controls.TextBlock
        _Header.Text = HeaderContent
        _Header.Margin = New System.Windows.Thickness(3, 10, 3, 0)
        _Header.FontWeight = System.Windows.FontWeights.Bold
        If HeaderContent = "" Then _Header.Visibility = Windows.Visibility.Collapsed
    End Sub
    Public ReadOnly Property GetHeader As System.Windows.Controls.TextBlock
        Get
            Return _Header
        End Get
    End Property
    Public ReadOnly Property GetDisplaymember As System.Windows.UIElement
        Get
            Return _Control
        End Get
    End Property
    'Public MustOverride Function GetDisplayMember() As System.Windows.UIElement
    Public MustOverride Function GetMinHeight() As Integer
End Class
