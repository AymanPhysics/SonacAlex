Imports System.Data

Public Class BankCash_G2Types
    Public TableName As String = "BankCash_G2Types"
    Public SubId As String = "Flag"
    Public SubId2 As String = "Id"

    Public SubName As String = "Name"

    Public TableDetailsName As String = "EmpBankCash_G2Types"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()

        If Flag = 3 Then
            TableName = "Entry2Types"
            TableDetailsName = "EmpEntry2Types"
        ElseIf Flag = 4 Then
            TableName = "EntryTypes"
            TableDetailsName = "EmpEntryTypes"
        End If

        bm.Fields = {SubId, SubId2, SubName, "FromInvoiceNo", "ToInvoiceNo"}
        bm.control = {txtFlag, txtID, txtName, FromInvoiceNo, ToInvoiceNo}
        bm.KeyFields = {SubId, SubId2}
        bm.Table_Name = TableName


        LoadWFH()
        btnNew_Click(sender, e)
    End Sub

    Structure GC
        Shared Id As String = "Id"
        Shared Name As String = "Name"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Id, "كود الموظف")
        G.Columns.Add(GC.Name, "اسم الموظف")

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Name).FillWeight = 300

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index Then
                G.Rows(e.RowIndex).Cells(GC.Name).Value = bm.ExecuteScalar("select dbo.GetEmpArName(" & G.Rows(e.RowIndex).Cells(GC.Id).Value & ")")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index Then
                If bm.ShowHelpGrid("Employees", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "select Id,Name from Employees") Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Name)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetEmpArName(EmpId)EmpName from " & TableDetailsName & " where TypeId" & "=" & txtID.Text.Trim & " and Flag=" & txtFlag.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("EmpId").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("EmpName").ToString
        Next
        G.RefreshEdit()

    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        G.EndEdit()

        FromInvoiceNo.Text = Val(FromInvoiceNo.Text)
        ToInvoiceNo.Text = Val(ToInvoiceNo.Text)

        bm.DefineValues()
        If Not bm.Save(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text.Trim}) Then Return

        bm.SaveGrid(G, TableDetailsName, New String() {"Flag", "TypeId"}, New String() {txtFlag.Text, txtID.Text}, New String() {"EmpId"}, New String() {GC.Id}, New VariantType() {VariantType.Integer}, New String() {GC.Id})

        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls(False)
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"

        FromInvoiceNo.Text = 1
        ToInvoiceNo.Text = 1000000

        G.Rows.Clear()
        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG("MsgDelete") Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "='" & txtID.Text.Trim & "'    delete from " & TableDetailsName & " where " & SubId & "='" & txtFlag.Text.Trim & "' and " & "TypeId" & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName & " where " & SubId & "='" & txtFlag.Text.Trim & "'")
    End Sub

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
            txtName.Focus()
            lv = False
            Return
        End If
        FillControls()
        lv = False
        txtName.SelectAll()
        txtName.Focus()
        txtName.SelectAll()
        txtName.Focus()
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, FromInvoiceNo.KeyDown, ToInvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    
    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        lblId.SetResourceReference(Label.ContentProperty, "Id")
        LblName.SetResourceReference(Label.ContentProperty, "Name")


    End Sub


End Class
