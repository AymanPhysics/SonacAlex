Imports System.Data

Public Class FinalReportsSubDetails
    Public MainTableName As String = "FinalReportsMain"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"


    Public Main2TableName As String = "FinalReportsSub"
    Public Main2MainId As String = "FinalReportsMainId"
    Public Main2SubId As String = "Id"
    Public Main2SubName As String = "Name"


    Public TableName As String = "FinalReportsSub2"
    Public MainId As String = "FinalReportsMainId"
    Public MainId2 As String = "FinalReportsSubId"
    Public SubId As String = "Id"
    Public SubName As String = "Name"

    Public TableNameDetails As String = "FinalReportsSubDetails"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0

    Private Sub BasicForm3_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {btnPrintAll})
        LoadResource()
        bm.Fields = New String() {MainId, MainId2, SubId, SubName}
        bm.control = New Control() {CboMain, CboMain2, txtID, txtName}
        If Flag = 1 Then
            bm.Fields = New String() {MainId, MainId2, SubId, SubName, "DelivaryCost"}
            bm.control = New Control() {CboMain, CboMain2, txtID, txtName, DelivaryCost}
            lblDelivaryCost.Visibility = Visibility.Visible
            DelivaryCost.Visibility = Visibility.Visible
            lblLE.Visibility = Visibility.Visible
        End If
        bm.KeyFields = New String() {MainId, MainId2, SubId}
        btnPrintAll.Visibility = Windows.Visibility.Hidden

        LoadWFH()

        bm.FillCombo(MainTableName, CboMain, "")
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub


    Structure GC
        Shared AccNo As String = "AccNo"
        Shared AccName As String = "AccName"
        Shared MotionType As String = "MotionType"
        Shared Perc As String = "Perc"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.AccNo, "كود الحساب")
        G.Columns.Add(GC.AccName, "اسم الحساب")

        Dim GCMotionType As New Forms.DataGridViewComboBoxColumn
        GCMotionType.HeaderText = "النوع"
        GCMotionType.Name = GC.MotionType
        bm.FillCombo("select 0 Id,'مدين' Name union all select 1 Id,'دائن' Name order by Id", GCMotionType)
        G.Columns.Add(GCMotionType)

        G.Columns.Add(GC.Perc, "النسبة")
        
        G.Columns(GC.AccName).FillWeight = 300
        G.Columns(GC.MotionType).FillWeight = 150

        G.Columns(GC.AccName).ReadOnly = True


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown

    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.AccNo).Value = Nothing
        G.Rows(i).Cells(GC.AccName).Value = Nothing
        G.Rows(i).Cells(GC.MotionType).Value = Nothing
        G.Rows(i).Cells(GC.Perc).Value = Nothing
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, MainId2, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Or CboMain.SelectedValue.ToString = 0 Or CboMain2.SelectedValue.ToString = 0 Then
            Return
        End If
        DelivaryCost.Text = Val(DelivaryCost.Text)

        G.EndEdit()

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}) Then Return

        If Not bm.SaveGrid(G, TableNameDetails, New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue, CboMain2.SelectedValue, txtID.Text}, New String() {"AccNo", "MotionType", "Perc"}, New String() {GC.AccNo, GC.MotionType, GC.Perc}, New VariantType() {VariantType.String, VariantType.Integer, VariantType.Decimal}, New String() {GC.AccNo}) Then Return

        btnNew_Click(sender, e)

    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {MainId, MainId2, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        lop = True
        Try
            bm.ClearControls(False)
            G.Rows.Clear()
            DelivaryCost.Clear()
            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & CboMain2.SelectedValue.ToString & "'")
            If txtID.Text = "" Then txtID.Text = "1"

            txtName.Focus()
        Catch
        End Try
        lop = False
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & " ='" & CboMain2.SelectedValue.ToString & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, MainId2, SubId}, New String() {CboMain.SelectedValue.ToString, CboMain2.SelectedValue.ToString, txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
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

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        If lop Then Return
        Dim s As String = ""
        Try
            s = CboMain.SelectedValue.ToString
        Catch ex As Exception
        End Try
        bm.FillCombo(Main2TableName, CboMain2, " where " & Main2MainId & "='" & s & "'")
        ClearControls()
    End Sub

    Private Sub CboMain2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain2.SelectionChanged
        ClearControls()
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
        btnPrintAll.SetResourceReference(Button.ContentProperty, "Print All")

        lblMain.SetResourceReference(Label.ContentProperty, "FinalReportsMain")
        lblMain2.SetResourceReference(Label.ContentProperty, "FinalReportsSub")
        LblId.SetResourceReference(Label.ContentProperty, "Id")
        LblName.SetResourceReference(Label.ContentProperty, "Name")

    End Sub

    Dim lop As Boolean = False
    Private Sub FillControls()
        bm.FillControls(Me)
        CboMain_SelectedIndexChanged(Nothing, Nothing)
        lop = True
        bm.FillControls(Me)

        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetAccName(AccNo)AccName from " & TableNameDetails & " where " & MainId & "=" & CboMain.SelectedValue & " and " & MainId2 & "=" & CboMain2.SelectedValue & " and " & SubId & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.AccNo).Value = dt.Rows(i)("AccNo").ToString
            G.Rows(i).Cells(GC.AccName).Value = dt.Rows(i)("AccName").ToString
            G.Rows(i).Cells(GC.MotionType).Value = dt.Rows(i)("MotionType").ToString
            G.Rows(i).Cells(GC.Perc).Value = dt.Rows(i)("Perc").ToString
        Next
        txtName.Focus()
        G.RefreshEdit()

        lop = False
    End Sub

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(TableName, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName & " where " & MainId & "=" & CboMain.SelectedValue.ToString & " and " & MainId2 & "=" & CboMain2.SelectedValue.ToString) Then
            txtName.Focus()
        End If
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As RoutedEventArgs) Handles btnPrintAll.Click
        bm.PrintTbl(CType(Parent, Page).Title, TableName)
    End Sub

    Private Sub GridCalcRow(sender As Object, e As Forms.DataGridViewCellEventArgs)
        For i = 0 To G.Rows.Count - 1
            If i = e.RowIndex Then Continue For
            If G.Rows(i).Cells(GC.AccNo).Value Is Nothing Then Continue For
            If G.Rows(i).Cells(GC.AccNo).Value = "" Then Continue For
            If G.Rows(i).Cells(GC.AccNo).Value = G.Rows(e.RowIndex).Cells(GC.AccNo).Value Then
                bm.ShowMSG("تم تكرار الحساب")
                ClearRow(e.RowIndex)
            End If
        Next

        If G.Columns(e.ColumnIndex).Name = GC.AccNo Then
            bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.AccNo), G.Rows(e.RowIndex).Cells(GC.AccName))
        End If
    End Sub

    Private Sub GridKeyDown(sender As Object, e As Forms.KeyEventArgs)
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.AccNo).Index Then
                If bm.AccNoShowHelpGrid(G.CurrentRow.Cells(GC.AccNo), G.CurrentRow.Cells(GC.AccName), e, 1) Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.MotionType)
                End If
            End If
        Catch
        End Try
    End Sub

End Class
