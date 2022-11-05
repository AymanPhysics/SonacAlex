Imports System.Data

Public Class RPT11
    Public MyLinkFile As Integer = 0
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If MainLinkFile.IsEnabled AndAlso MainLinkFile.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMainLinkFile.Content)
            MainLinkFile.Focus()
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@DayDate", "@ToDate", "@LinkFile", "Header", "IsGroupped", "@P", "@Z", "@N"}
        rpt.paravalue = New String() {MainAccNo.Text, MainAccName.Text, ToDate.SelectedDate, ToDate.SelectedDate, MyLinkFile, CType(Parent, Page).Title.Trim & IIf(MainLinkFile.IsVisible, " " & MainLinkFile.Text, ""), IIf(IsGroupped.IsChecked, 1, 0), IIf(P.IsChecked, 1, 0), IIf(Z.IsChecked, 1, 0), IIf(N.IsChecked, 1, 0)}
        Select Case Flag
            Case 1
                If Md.ShowCurrency Then
                    rpt.Rpt = "AllBal.rpt"
                Else
                    rpt.Rpt = "AllBalTel.rpt"
                End If
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({MainAccNo})
        bm.FillCombo("LinkFile", MainLinkFile, "", , True)

        If MyLinkFile > 0 Then
            MainLinkFile.SelectedValue = MyLinkFile
            lblMainLinkFile.Visibility = Windows.Visibility.Hidden
            MainLinkFile.Visibility = Windows.Visibility.Hidden
        End If
        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Dim lop As Boolean = False



    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")

    End Sub

    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , MyLinkFile, )
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , MyLinkFile, )
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        MyLinkFile = MainLinkFile.SelectedValue
        MainAccNo_LostFocus(Nothing, Nothing) 
    End Sub

End Class