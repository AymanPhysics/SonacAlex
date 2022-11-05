Imports System.Data

Public Class CurrencyExchangeByDate
    Public TableName As String = "CurrencyExchangeByDate"
    Public SubId As String = "DayDate"

    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G1 As New MyGrid

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()

        LoadWFH1()

        bm.Fields = New String() {SubId}
        bm.control = New Control() {DayDate}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(Nothing, Nothing)
    End Sub

    Structure GC1
        Shared CurrencyId As String = "CurrencyId"
        Shared Exchange As String = "Exchange"
    End Structure

    Private Sub LoadWFH1()
        WFH1.Child = G1

        G1.Columns.Clear()
        G1.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCCurrencyId As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId.HeaderText = "العملة"
        GCCurrencyId.Name = GC1.CurrencyId
        bm.FillCombo("select Id,Name from Currencies where Id<>1 union all select 0 Id,'-' Name order by Id", GCCurrencyId)
        G1.Columns.Add(GCCurrencyId)

        G1.Columns.Add(GC1.Exchange, "سعر الصرف")

        G1.Columns(GC1.CurrencyId).FillWeight = 300 
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)


        dt = bm.ExecuteAdapter("select * from " & TableName & " Where " & SubId & "='" & bm.ToStrDate(DayDate.SelectedDate) & "'")
        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add()
            G1.Rows(i).Cells(GC1.CurrencyId).Value = dt.Rows(i)("CurrencyId").ToString
            G1.Rows(i).Cells(GC1.Exchange).Value = dt.Rows(i)("Exchange").ToString
        Next
        G1.CurrentCell = G1.Rows(G1.Rows.Count - 1).Cells(0)
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {bm.ToStrDate(DayDate.SelectedDate)}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If DayDate.SelectedDate Is Nothing Then
            DayDate.Focus()
            Return
        End If
        bm.DefineValues()

        G1.EndEdit()

        If Not bm.SaveGrid(G1, TableName, New String() {SubId}, New String() {bm.ToStrDate(DayDate.SelectedDate)}, New String() {"CurrencyId", "Exchange"}, New String() {GC1.CurrencyId, GC1.Exchange}, New VariantType() {VariantType.Integer, VariantType.Decimal}, New String() {}) Then Return

        btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        DayDate.SelectedDate = bm.MyGetDate
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Sub ClearControls()
        G1.Rows.Clear()
        G1.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("MsgDelete") Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & bm.ToStrDate(DayDate.SelectedDate) & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {bm.ToStrDate(DayDate.SelectedDate)}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
     

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DayDate.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {bm.ToStrDate(DayDate.SelectedDate)}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        G1.Focus()
    End Sub
     
    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last") 
         
    End Sub
     
End Class
