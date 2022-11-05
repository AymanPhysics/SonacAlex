Imports System.Data

Public Class RPT10
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@CustId", "@CompanyId", "Header"}
        rpt.paravalue = New String() {Val(CustomerId.Text), Val(CompanyId.Text), CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "CustomerCompanies.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({CustomerId, CompanyId})
        LoadResource()
        Dim MyNow As DateTime = bm.MyGetDate()
    End Sub




    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustomerId.KeyDown, CompanyId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub CustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus
        bm.LostFocus(CustomerId, CustomerName, "select Name from Customers where Id=" & CustomerId.Text.Trim())
        Dim s As String = ""
        Dim dt As DataTable = bm.ExecuteAdapter("GetCustomerData", New String() {"Id"}, New String() {Val(CustomerId.Text)})
        CustomerId.ToolTip = ""
        CustomerName.ToolTip = ""
        If dt.Rows.Count = 0 Then Return
        For i As Integer = 0 To dt.Columns.Count - 2
            s &= dt.Rows(0)(i).ToString & IIf(i = dt.Columns.Count - 1, "", vbCrLf)
        Next
        CustomerId.ToolTip = s
        CustomerName.ToolTip = s
    End Sub

    Private Sub CustomerId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustomerId.KeyUp
        'bm.ShowHelp("Customers", CustomerId, CustomerName, e, "select cast(Id as varchar(100)) Id,Name from Customers")
        bm.ShowHelpCustomers(CustomerId, CustomerName, e)
    End Sub

    Private Sub CompanyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CompanyId.LostFocus
        If Val(CompanyId.Text.Trim) = 0 Then
            CompanyId.Clear()
            CompanyName.Clear()
            Return
        End If

        bm.LostFocus(CompanyId, CompanyName, "select Name from CustomerCompanies where Id=" & CompanyId.Text.Trim())
    End Sub

    Private Sub CompanyId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CompanyId.KeyUp
        If bm.ShowHelp("CustomerCompanies", CompanyId, CompanyName, e, "select cast(Id as varchar(100)) Id,Name from CustomerCompanies") Then
            CompanyId_LostFocus(Nothing, Nothing)
        End If
    End Sub


End Class