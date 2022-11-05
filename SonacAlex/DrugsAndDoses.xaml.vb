Imports System.Data

Public Class DrugsAndDoses

    Public EmpId As Integer
    Public DatePicker1 As DateTime
    Public ReservId As Integer
    Public CaseId As Integer
    Public CaseName As String

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    WithEvents G3 As New MyGrid

    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        LoadWFH3()

        LoadDoses()
        LoadDrugs()

        dt = bm.ExecuteAdapter("select * from ReservationCbo3 where EmpId='" & EmpId & "' and DayDate='" & bm.ToStrDate(DatePicker1) & "' and ReservId='" & ReservId & "'and CaseId='" & CaseId & "'")
        G3.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add()
            G3.Rows(i).Cells(GC3.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G3.Rows(i).Cells(GC3.Notes2).Value = dt.Rows(i)("Notes2").ToString
        Next
        G3.RefreshEdit()

    End Sub

    Structure GC3
        Shared Notes1 As String = "Notes1"
        Shared Notes2 As String = "Notes2"
    End Structure

    Private Sub LoadWFH3()
        WFH3.Child = G3

        G3.Columns.Clear()
        G3.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G3.ForeColor = System.Drawing.Color.DarkBlue
        G3.ColumnHeadersVisible = False

        G3.Columns.Add(GC3.Notes1, "")
        G3.Columns.Add(GC3.Notes2, "")
        G3.Columns(GC3.Notes1).FillWeight = 100
        G3.Columns(GC3.Notes2).FillWeight = 200
        G3.Columns(GC3.Notes2).CellTemplate.Style.Alignment = Forms.DataGridViewContentAlignment.MiddleRight

        G3.AllowUserToDeleteRows = True
        G3.EditMode = Forms.DataGridViewEditMode.EditOnEnter
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnPrint.Click
        
        bm.SaveGrid(G3, "ReservationCbo3", New String() {"EmpId", "DayDate", "ReservId", "CaseId"}, New String() {EmpId, bm.ToStrDate(DatePicker1), ReservId, CaseId}, New String() {"Notes1", "Notes2"}, New String() {GC3.Notes1, GC3.Notes2}, New VariantType() {VariantType.String, VariantType.String}, New String() {GC3.Notes1})


        If sender Is btnSave Then CType(Parent, Window).Close()
        If sender Is btnPrint Then
            Dim rpt As New ReportViewer
            rpt.Header = CType(Parent, Window).Title
            rpt.paraname = New String() {"@EmpId", "@CaseId", "CaseName", "@FromDate", "@ToDate", "@FromId", "@ToId", "@All", "Header", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
            rpt.paravalue = New String() {Val(EmpId), Val(CaseId), CaseName, bm.ToStrDate(DatePicker1), bm.ToStrDate(DatePicker1), ReservId, ReservId, 0, "Patient History", 1, 1, 1, 1, 1, 1, 1, 1, 1}
            rpt.Rpt = "cbo3.rpt"
            'rpt.Show()
            rpt.Print()
        End If
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")

    End Sub


    Private Sub Add3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add3.Click
        If cbo31.Text.Trim = "" Then Return
        G3.Rows.Add(cbo31.Text, cbo32.Text)
        bm.AddItemToTable("Drugs", cbo31.Text)
        bm.AddItemToTable("Doses", cbo32.Text)
        cbo31.Text = ""
        cbo32.Text = ""
        'LoadDrugs()
        'LoadDoses()
        cbo31.Focus()
    End Sub

    Private Sub cbo31_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles cbo31.PreviewKeyDown
        If e.Key = Key.Enter Then cbo32.Focus()
    End Sub

    Private Sub cbo32_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles cbo32.PreviewKeyDown
        If e.Key = Key.Enter Then Add3_Click(Nothing, Nothing)
    End Sub


    Private Sub LoadDoses()
        bm.FillCombo("Doses", cbo32, "", "")
    End Sub

    Private Sub LoadDrugs()
        bm.FillCombo("Drugs", cbo31, "", "")
    End Sub


    Private Sub btnSave_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy.Click

        Dim s As String = bm.ExecuteScalar("select top 1 dbo.ToStrDate(DayDate) from ReservationCbo3 where EmpId='" & EmpId & "' and DayDate<'" & bm.ToStrDate(DatePicker1) & "' /*and ReservId='" & ReservId & "'*/ and CaseId='" & CaseId & "' order by DayDate desc")

        dt = bm.ExecuteAdapter("select * from ReservationCbo3 where EmpId='" & EmpId & "' and DayDate='" & s & "' /*and ReservId='" & ReservId & "'*/ and CaseId='" & CaseId & "'")
        G3.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G3.Rows.Add()
            G3.Rows(i).Cells(GC3.Notes1).Value = dt.Rows(i)("Notes1").ToString
            G3.Rows(i).Cells(GC3.Notes2).Value = dt.Rows(i)("Notes2").ToString
        Next
        G3.RefreshEdit()

    End Sub

End Class
