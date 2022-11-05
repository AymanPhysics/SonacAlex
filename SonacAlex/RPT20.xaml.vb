Imports System.Data

Public Class RPT20

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Level.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد المستوى")
            Level.Focus()
            Return
        End If

        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@Level", "@ActiveOnly", "@HasBalOnly", "@FromDate", "@ToDate", "Header", "@FromMainAccNo", "@ToMainAccNo"}
        rpt.paravalue = New String() {Level.SelectedIndex, IIf(Detailed.IsChecked, 1, 0), IIf(DetailedInvoice.IsChecked, 1, 0), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, MainAccNo.Text, ToMainAccNo.Text}
        Select Case Flag
            Case 1
                rpt.Rpt = "AccountBalance.rpt"
            Case 2
                rpt.Rpt = "AccountBalanceCurrency.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({MainAccNo, ToMainAccNo})
        FillLevel()

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

    End Sub



    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
    End Sub

    Private Sub FillLevel()
        Try
            Level.Items.Clear()
            Dim x As Integer = Val(bm.ExecuteScalar("select min(Level) from chart where SubType=1"))
            Dim y As Integer = Val(bm.ExecuteScalar("select max(Level) from chart where SubType=1"))
            Level.Items.Add("-")
            For i As Integer = 1 To x
                Level.Items.Add(New ComboBoxItem With {.Content = i})
            Next
            For i As Integer = x + 1 To y
                Level.Items.Add(New ComboBoxItem With {.Content = i, .Background = System.Windows.Media.Brushes.Red})
            Next
            Level.SelectedIndex = x
        Catch
        End Try

        Level.SelectedIndex = Level.Items.Count - 1
    End Sub

    Dim MyLinkFile As Integer = 0
    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, 1, MyLinkFile, )
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, 1, MyLinkFile, )
    End Sub


    Private Sub ToMainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ToMainAccNo.LostFocus
        bm.AccNoLostFocus(ToMainAccNo, ToMainAccName, 1, MyLinkFile, )
    End Sub

    Private Sub ToMainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ToMainAccNo.KeyUp
        bm.AccNoShowHelp(ToMainAccNo, ToMainAccName, e, 1, MyLinkFile, )
    End Sub

End Class