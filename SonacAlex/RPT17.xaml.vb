Imports System.Data

Public Class RPT17

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@TestId", "@SubTestId", "@Id", "@CaseId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(TestId.Text), Val(SubTestId.Text), Val(ItemId.Text), Val(CaseId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "LabTestsAll.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({TestId, SubTestId, ItemId, CaseId})
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Dim lop As Boolean = False


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")

        lblPatient.SetResourceReference(Label.ContentProperty, "Patient")

    End Sub

    Private Sub TestId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TestId.KeyUp
        bm.ShowHelp("LaboratoryTestTypes", TestId, TestName, e, "select cast(Id as varchar(100)) Id,Name from LaboratoryTestTypes", "LaboratoryTestTypes")
    End Sub

    Private Sub SubTestId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubTestId.KeyUp
        bm.ShowHelp("", SubTestId, SubTestName, e, "select cast(Id as varchar(100)) Id,Name from LaboratoryTests where TestId=" & TestId.Text.Trim)
    End Sub

    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        bm.ShowHelp("LabTestItems", ItemId, ItemName, e, "select cast(Id as varchar(100)) Id,Name from LabTestItems where TestId=" & TestId.Text.Trim & " and SubTestId=" & SubTestId.Text)
    End Sub

    Private Sub TestId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TestId.LostFocus
        bm.LostFocus(TestId, TestName, "select Name from LaboratoryTestTypes where Id=" & TestId.Text.Trim())
        SubTestId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub SubTestId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubTestId.LostFocus
        bm.LostFocus(SubTestId, SubTestName, "select Name from LaboratoryTests where TestId=" & TestId.Text.Trim & " and Id=" & SubTestId.Text.Trim())
        ItemId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from LabTestItems where TestId=" & TestId.Text.Trim & " and SubTestId=" & SubTestId.Text.Trim & " and Id=" & ItemId.Text.Trim())
    End Sub



    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,Gender,DateOfBirth from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
    End Sub

End Class