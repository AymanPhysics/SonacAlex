Imports System.Data

Public Class RPT19

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public MyLinkFile As Integer = 0
    Public Flag As Integer = 0
    Public RptFlag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@Flag", "@BankId", "@MainAccNo", "@SubAccNo", "@CostCenterId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {RptFlag, Val(BankId.Text), Val(MainAccNo.Text), Val(SubAccNo.Text), Val(CostCenterId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = ".rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({BankId, MainAccNo, SubAccNo, CostCenterId})
        If Not Md.ShowCostCenter Then
            lblCostCenterId.Visibility = Windows.Visibility.Hidden
            CostCenterId.Visibility = Windows.Visibility.Hidden
            CostCenterName.Visibility = Windows.Visibility.Hidden
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1,1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Dim lop As Boolean = False


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
        lblBank.SetResourceReference(Label.ContentProperty, "Bank")
        If MyLinkFile = 5 Then lblBank.SetResourceReference(Label.ContentProperty, "Safe")
        lblMainAccNo.SetResourceReference(Label.ContentProperty, "MainAccNo")
        lblSubAccNo.SetResourceReference(Label.ContentProperty, "SubAccNo")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyDown, SubAccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        If Val(BankId.Text.Trim) = 0 Then
            BankId.Clear()
            BankName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        bm.LostFocus(BankId, BankName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If Val(MainAccNo.Text) = 0 Or Not SubAccNo.IsEnabled Then
            SubAccNo.Clear()
            SubAccName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
        bm.LostFocus(SubAccNo, SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim() & " and AccNo='" & MainAccNo.Text & "'")
    End Sub

    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubAccNo.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & MainAccNo.Text & "'") Then
            SubAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , , )

        SubAccNo.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')").Rows.Count > 0
        SubAccNo_LostFocus(Nothing, Nothing)

        If SubAccNo.IsEnabled Then
            SubAccNo.Focus()
        Else
            CostCenterId.Focus()
        End If
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , , )
    End Sub

    Private Sub CostCenterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CostCenterId.LostFocus
        bm.CostCenterIdLostFocus(CostCenterId, CostCenterName, )
    End Sub

    Private Sub CostCenterId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CostCenterId.KeyUp
        bm.CostCenterIdShowHelp(CostCenterId, CostCenterName, e, )
    End Sub

End Class