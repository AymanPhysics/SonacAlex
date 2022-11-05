Imports System.Data

Public Class RPT28


    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If BankId.IsEnabled AndAlso Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If
        If MainLinkFile.IsEnabled AndAlso MainLinkFile.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMainLinkFile.Content)
            MainLinkFile.Focus()
            Return
        End If
        If CurrencyId.IsEnabled AndAlso CurrencyId.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblCurrencyId.Content)
            CurrencyId.Focus()
            Return
        End If


        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Id", "@LinkFile", "@CurrencyId", "CurrencyName", "@ApplyCurrencyCycle", "@DayDate", "Header"}
        rpt.paravalue = New String() {Val(BankId.Text), MainLinkFile.SelectedValue, CurrencyId.SelectedValue, IIf(CurrencyId.SelectedIndex > 0, CurrencyId.Text, BankName.Text), IIf(ApplyCurrencyCycle.IsChecked, 2, 1), ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "CurrencyExchange.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({BankId})
        bm.FillCombo("select 0 Id,'-' Name union all select Id,Name from Currencies where Id<>1 order by Id", CurrencyId)
        bm.FillCombo("LinkFile", MainLinkFile, "", , True)

        ApplyCurrencyCycle_UnChecked(Nothing, Nothing)

        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
    End Sub
     


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub
     

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        Try
            If Val(BankId.Text.Trim) = 0 Then
                BankId.Clear()
                BankName.Clear()
                Return
            End If

            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
            bm.LostFocus(BankId, BankName, "select Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ") where ApplyCurrencyCycle=2 and Id=" & BankId.Text.Trim())
        Catch
        End Try
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ") where ApplyCurrencyCycle=2") Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        BankId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub ApplyCurrencyCycle_Checked(sender As Object, e As RoutedEventArgs) Handles ApplyCurrencyCycle.Checked
        CurrencyId.SelectedIndex = 0
        CurrencyId.IsEnabled = False
        MainLinkFile.IsEnabled = True
        BankId.IsEnabled = True
    End Sub

    Private Sub ApplyCurrencyCycle_UnChecked(sender As Object, e As RoutedEventArgs) Handles ApplyCurrencyCycle.Unchecked
        CurrencyId.IsEnabled = True
        MainLinkFile.SelectedIndex = 0
        BankId.Clear()
        BankName.Clear()
        MainLinkFile.IsEnabled = False
        BankId.IsEnabled = False
    End Sub

End Class