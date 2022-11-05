Imports System.Data

Public Class Complaint

    Public TableName As String = "CasesComplaint"
    Public MainId As String = "CaseId"
    Public MainId2 As String = "Mykey"
    Public MainId3 As String = "MyFlag"
    Public SubId As String = "Id"
    Public SubName As String = "Name"
    Public lblMain_Content As String


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False
    Public ReLoadMenue As Boolean = False

    Public MyCase As Integer
    Public MyFlag As String
    Public MyKey As Integer

    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        If WithImage Then
            btnSetImage.Visibility = Visibility.Visible
            btnSetNoImage.Visibility = Visibility.Visible
            Image1.Visibility = Visibility.Visible
        End If


        bm.FillCombo("select Id,Name from Cases union select 0 Id,'-' Name order by Name", CboMain)

        Dim v() As String = {MainId, MainId2, MainId3, SubId, SubName, "DayDate"}
        bm.Fields = v

        Dim c() As Control = {CboMain, txtMyKey, txtMyFlag, txtID, txtName, DayDate}
        bm.control = c

        Dim k() As String = {MainId, MainId2, MainId3, SubId}
        bm.KeyFields = k

        bm.Table_Name = TableName
        btnNew_Click(sender, e)
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        If WithImage Then bm.GetImage(TableName, New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}, "Image", Image1)

    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, MainId2, MainId3, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Or CboMain.SelectedValue.ToString = 0 Then
            Return
        End If
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}) Then Return
        If WithImage Then bm.SaveImage(TableName, New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}, "Image", Image1)


        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {MainId, MainId2, MainId3, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        Try
            CboMain.SelectedValue = MyCase
            txtMyKey.Text = MyKey
            txtMyFlag.Text = MyFlag
            bm.SetNoImage(Image1)
            txtName.Clear()

            txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & txtMyKey.Text & "' and " & MainId3 & "='" & txtMyFlag.Text & "'")
            If txtID.Text = "" Then txtID.Text = "1"
            DayDate.SelectedDate = Now.Date
            txtName.Focus()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "' and " & MainId & " ='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & txtMyKey.Text & "' and " & MainId3 & "='" & txtMyFlag.Text & "'  delete CasesComplaintDt where " & MainId & "='" & CboMain.SelectedValue.ToString & "' and " & MainId2 & "='" & txtMyKey.Text & "' and " & MainId3 & "='" & txtMyFlag.Text & "' and " & SubId & "='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {MainId, MainId2, MainId3, SubId}, New String() {CboMain.SelectedValue.ToString, txtMyKey.Text, txtMyFlag.Text, txtID.Text.Trim}, dt)
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

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
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

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
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
