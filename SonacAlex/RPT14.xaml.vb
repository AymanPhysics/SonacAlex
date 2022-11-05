Imports System.Data

Public Class RPT14
    Public MyLinkFile As Integer = 0
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Val(CostCenterId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد مركز التكلفة")
            CostCenterId.Focus()
            Return
        End If
        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@CostCenterId", "CostCenterName", "@OutComeTypeId", "OutComeTypeName", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CostCenterId.Text), CostCenterName.Text, Val(OutComeTypeId.Text), OutComeTypeName.Text, FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "BankCash2.rpt"
            Case 2
                rpt.Rpt = "BankCash22.rpt"
            Case 3
                rpt.Rpt = "CostCenterMotion.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CostCenterId, OutComeTypeId})
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

        If Flag = 3 Then
            lblOutComeTypeId.Visibility = Windows.Visibility.Hidden
            OutComeTypeId.Visibility = Windows.Visibility.Hidden
            OutComeTypeName.Visibility = Windows.Visibility.Hidden
        End If
    End Sub

    Dim lop As Boolean = False
    

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
    End Sub

    Private Sub CostCenterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CostCenterId.LostFocus
        bm.CostCenterIdLostFocus(CostCenterId, CostCenterName, )
    End Sub

    Private Sub CostCenterId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CostCenterId.KeyUp
        bm.CostCenterIdShowHelp(CostCenterId, CostCenterName, e, )
    End Sub

    Private Sub OutComeTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles OutComeTypeId.KeyUp
        bm.ShowHelp("OutComeTypes", OutComeTypeId, OutComeTypeName, e, "select cast(Id as varchar(100)) Id,Name from OutComeTypes", "Countries")
    End Sub

    Private Sub OutComeTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OutComeTypeId.LostFocus
        bm.LostFocus(OutComeTypeId, OutComeTypeName, "select Name from OutComeTypes where Id=" & OutComeTypeId.Text.Trim())
    End Sub

End Class