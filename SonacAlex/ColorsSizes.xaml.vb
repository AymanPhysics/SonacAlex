Imports System.Data

Public Class ColorsSizes

    Public TableName As String = "Colors"
    Public SubTableName As String = "ColorsDetails"
    Public SubTableNameField As String = "ColorId"
    Public SubId As String = "Id"
    Public SubName As String = "Name"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents G As New MyGrid


    Private Sub LabTestItems_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        LoadWFH()
        bm.Fields = New String() {SubId, SubName}
        bm.control = New Control() {txtID, txtName}
        bm.KeyFields = New String() {SubId}

        bm.Table_Name = TableName
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
        G.Columns.Add(GC.Id, "الكود")
        G.Columns.Add(GC.Name, "الاسم")

        G.Columns(GC.Id).ReadOnly = True

        G.Columns(GC.Id).FillWeight = 100
        G.Columns(GC.Name).FillWeight = 300

        G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G.AllowUserToAddRows = True
        G.AllowUserToDeleteRows = True
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G.TabStop = False
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            Return
        End If
        G.EndEdit()
        DelivaryCost.Text = Val(DelivaryCost.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not bm.ExecuteNonQuery("delete " & SubTableName & " where " & SubTableNameField & "='" & Val(txtID.Text) & "'") Then Return

        Dim str As String = "Insert " & SubTableName & "(" & SubTableNameField & ",Id,Name) values "
        For i As Integer = 0 To G.Rows.Count - 1
            Try
                If G.Rows(i).Cells(GC.Name).Value.ToString.Trim = "" Then Continue For
                str &= "('" & Val(txtID.Text) & "','" & G.Rows(i).Cells(GC.Id).Value.ToString & "','" & G.Rows(i).Cells(GC.Name).Value.ToString & "'),"
            Catch ex As Exception
            End Try
        Next
        str = str.Substring(0, str.Length - 1)
        bm.ExecuteNonQuery(str)


        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        lop = True
        Try
            G.Rows.Clear()
            txtName.Clear()
            DelivaryCost.Clear()

            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If txtID.Text = "" Then txtID.Text = "1"

            txtName.Focus()
        Catch
        End Try
        lop = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'    delete " & SubTableName & " where " & SubTableNameField & "='" & Val(txtID.Text) & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        Try
            bm.ShowHelp(TableName, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName)
            txtName.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub DelivaryCost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles DelivaryCost.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        LblId.SetResourceReference(Label.ContentProperty, "Id")
        LblName.SetResourceReference(Label.ContentProperty, "Name")
        'TabItem0.SetResourceReference(TabItem.HeaderProperty, "Normal Value")

    End Sub

    Dim lop As Boolean = False
    Private Sub FillControls()
        bm.FillControls(Me)
        lop = True
        bm.FillControls(Me)


        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & SubTableName & " where " & SubTableNameField & "='" & Val(txtID.Text) & "'")
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
        Next
        G.RefreshEdit()

        lop = False
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        Try
            G.CurrentRow.Cells(GC.Id).Value = G.CurrentRow.Index + 1
        Catch ex As Exception
        End Try
    End Sub

End Class
