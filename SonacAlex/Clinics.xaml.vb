Imports System.Data
Public Class Clinics
    Public TableName As String = "Clinics"
    Public SubId As String = "Id"



    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public ShowInpatientDepartmentId As Boolean = False

    WithEvents G As New MyGrid

    Private Sub Clinics_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        LoadWFH()

        bm.FillCombo("RoomTypes", RoomTypeId, "")
        bm.FillCombo("InpatientDepartments", InpatientDepartmentId, "")

        If Not ShowInpatientDepartmentId Then
            lblInpatientDepartmentId.Visibility = Windows.Visibility.Hidden
            InpatientDepartmentId.Visibility = Windows.Visibility.Hidden
        End If

        bm.Fields = New String() {SubId, "Name", "Duration", "Cnt", "hh", "hh2", "hh3", "hh4", "hh5", "hh6", "hh7", "mm", "mm2", "mm3", "mm4", "mm5", "mm6", "mm7", "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "ConsultationPrice", "DetectionPrice", "Living", "Supervision", "Care", "MainAccNo", "SubAccNo", "RoomTypeId", "InpatientDepartmentId"}
        bm.control = New Control() {txtID, txtName, Duration, Cnt, hh, hh2, hh3, hh4, hh5, hh6, hh7, mm, mm2, mm3, mm4, mm5, mm6, mm7, Saturday, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, ConsultationPrice, DetectionPrice, Living, Supervision, Care, MainAccNo, SubAccNo, RoomTypeId, InpatientDepartmentId}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        If Flag = 2 OrElse Flag = 3 OrElse Flag = 4 OrElse Flag = 5 Then
            lblDetectionPrice.Visibility = Windows.Visibility.Hidden
            DetectionPrice.Visibility = Windows.Visibility.Hidden
            lblLE1.Visibility = Windows.Visibility.Hidden
            lblConsultationPrice.Visibility = Windows.Visibility.Hidden
            ConsultationPrice.Visibility = Windows.Visibility.Hidden
            lblLE2.Visibility = Windows.Visibility.Hidden
        End If

        If Flag = 3 OrElse Flag = 4 OrElse Flag = 5 Then
            lblDuration.Visibility = Windows.Visibility.Hidden
            Duration.Visibility = Windows.Visibility.Hidden
            lblMinutes.Visibility = Windows.Visibility.Hidden
            SubGrid.Visibility = Windows.Visibility.Hidden
            lblCnt.SetResourceReference(Label.ContentProperty, "Max count")
        End If

        If Flag = 5 Then
            lblCnt.Visibility = Windows.Visibility.Hidden
            Cnt.Visibility = Windows.Visibility.Hidden
            TabControl1.Visibility = Windows.Visibility.Hidden
        End If

        If Flag <> 5 Then
            lblLiving.Visibility = Windows.Visibility.Hidden
            Living.Visibility = Windows.Visibility.Hidden
            lblSupervision.Visibility = Windows.Visibility.Hidden
            Supervision.Visibility = Windows.Visibility.Hidden
            lblCare.Visibility = Windows.Visibility.Hidden
            Care.Visibility = Windows.Visibility.Hidden
            lblLE4.Visibility = Windows.Visibility.Hidden
            lblLE5.Visibility = Windows.Visibility.Hidden
            lblLE6.Visibility = Windows.Visibility.Hidden

            lblMainAccNo.Visibility = Windows.Visibility.Hidden
            MainAccNo.Visibility = Windows.Visibility.Hidden
            MainAccName.Visibility = Windows.Visibility.Hidden
            lblSubAccNo.Visibility = Windows.Visibility.Hidden
            SubAccNo.Visibility = Windows.Visibility.Hidden
            SubAccName.Visibility = Windows.Visibility.Hidden
        End If

        If Flag <> 3 Then

            lblRoomTypeId.Visibility = Windows.Visibility.Hidden
            RoomTypeId.Visibility = Windows.Visibility.Hidden
        End If

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
        G.Columns.Add(GC.Id, "مسلسل")
        G.Columns.Add(GC.Name, "الاسم")

        G.Columns(GC.Id).ReadOnly = True
        G.Columns(GC.Name).FillWeight = 300

        G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        G.TabStop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
        G.Rows.Clear()
        Cnt_LostFocus(Nothing, Nothing)
        dt = bm.ExecuteAdapter("Select * from " & TableName & "Names where MainId='" & txtID.Text.Trim & "'")
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("Id").ToString
            G.Rows(i).Cells(GC.Name).Value = dt.Rows(i)("Name").ToString
        Next
        If G.Rows.Count > 0 Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(0)

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        If MainAccNo.Visibility = Windows.Visibility.Visible AndAlso Val(MainAccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo.Focus()
            Return
        End If
        If Val(SubAccNo.Text) = 0 AndAlso SubAccNo.IsEnabled AndAlso Val(MainAccNo.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo.Focus()
            Return
        End If

        If Val(RoomTypeId.SelectedValue) < 1 AndAlso RoomTypeId.Visibility = Windows.Visibility.Visible Then
            bm.ShowMSG("برجاء تحديد النوع")
            RoomTypeId.Focus()
            Return
        End If

        ConsultationPrice.Text = Val(ConsultationPrice.Text)
        DetectionPrice.Text = Val(DetectionPrice.Text)
        
        bm.DefineValues()
        G.EndEdit()

        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        If Not bm.SaveGrid(G, TableName & "Names", New String() {"MainId"}, New String() {txtID.Text}, New String() {"Id", "Name"}, New String() {GC.Id, GC.Name}, New VariantType() {VariantType.Integer, VariantType.String}, New String() {}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True

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
        bm.ClearControls()
        G.Rows.Clear()

        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)

        Saturday.IsChecked = True
        Sunday.IsChecked = True
        Monday.IsChecked = True
        Tuesday.IsChecked = True
        Wednesday.IsChecked = True
        Thursday.IsChecked = True
        Friday.IsChecked = True

        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'   delete from " & TableName & "Names where MainId='" & txtID.Text.Trim & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False


    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(TableName, txtID, txtName, e, "Select cast(Id as varchar(10))Id," & Resources.Item("CboName") & " Name from " & TableName) Then
            txtID_LostFocus(sender, Nothing)
        End If
    End Sub


    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {txtID.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
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


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, Duration.KeyDown, Cnt.KeyDown, hh.KeyDown, hh2.KeyDown, hh3.KeyDown, hh4.KeyDown, hh5.KeyDown, hh6.KeyDown, hh7.KeyDown, mm.KeyDown, mm2.KeyDown, mm3.KeyDown, mm4.KeyDown, mm5.KeyDown, mm6.KeyDown, mm7.KeyDown, MainAccNo.KeyDown, SubAccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
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

        Saturday.SetResourceReference(CheckBox.ContentProperty, "Saturday")
        Sunday.SetResourceReference(CheckBox.ContentProperty, "Sunday")
        Monday.SetResourceReference(CheckBox.ContentProperty, "Monday")
        Tuesday.SetResourceReference(CheckBox.ContentProperty, "Tuesday")
        Wednesday.SetResourceReference(CheckBox.ContentProperty, "Wednesday")
        Thursday.SetResourceReference(CheckBox.ContentProperty, "Thursday")
        Friday.SetResourceReference(CheckBox.ContentProperty, "Friday")

        lblId.SetResourceReference(Label.ContentProperty, "Id")
        lblArName.SetResourceReference(Label.ContentProperty, "Name")

        lblMinutes.SetResourceReference(Label.ContentProperty, "Minutes")
        lblCnt.SetResourceReference(Label.ContentProperty, "Max Res. count")
        lblConsultationPrice.SetResourceReference(Label.ContentProperty, "ConsultationPrice")
        lblLE1.SetResourceReference(Label.ContentProperty, "L.E.")
        lblDetectionPrice.SetResourceReference(Label.ContentProperty, "DetectionPrice")
        lblLE2.SetResourceReference(Label.ContentProperty, "L.E.")
        lblDuration.SetResourceReference(Label.ContentProperty, "Duration")
        lblhh.SetResourceReference(Label.ContentProperty, "hh")
        lblmm.SetResourceReference(Label.ContentProperty, "mm")
        lblhh.SetResourceReference(Label.ContentProperty, "hh")
        lblmm.SetResourceReference(Label.ContentProperty, "mm")
        
    End Sub

    Dim lop As Boolean = False
    

    Private Sub Saturday_Checked(sender As Object, e As RoutedEventArgs) Handles Saturday.Checked
        hh.IsEnabled = True
        mm.IsEnabled = True
    End Sub
    Private Sub Saturday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Saturday.Unchecked
        hh.Clear()
        mm.Clear()
        hh.IsEnabled = False
        mm.IsEnabled = False
    End Sub

    Private Sub Sunday_Checked(sender As Object, e As RoutedEventArgs) Handles Sunday.Checked
        hh2.IsEnabled = True
        mm2.IsEnabled = True
    End Sub
    Private Sub Sunday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Sunday.Unchecked
        hh2.Clear()
        mm2.Clear()
        hh2.IsEnabled = False
        mm2.IsEnabled = False
    End Sub

    Private Sub Monday_Checked(sender As Object, e As RoutedEventArgs) Handles Monday.Checked
        hh3.IsEnabled = True
        mm3.IsEnabled = True
    End Sub
    Private Sub Monday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Monday.Unchecked
        hh3.Clear()
        mm3.Clear()
        hh3.IsEnabled = False
        mm3.IsEnabled = False
    End Sub

    Private Sub Tuesday_Checked(sender As Object, e As RoutedEventArgs) Handles Tuesday.Checked
        hh4.IsEnabled = True
        mm4.IsEnabled = True
    End Sub
    Private Sub Tuesday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Tuesday.Unchecked
        hh4.Clear()
        mm4.Clear()
        hh4.IsEnabled = False
        mm4.IsEnabled = False
    End Sub

    Private Sub Wednesday_Checked(sender As Object, e As RoutedEventArgs) Handles Wednesday.Checked
        hh5.IsEnabled = True
        mm5.IsEnabled = True
    End Sub
    Private Sub Wednesday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Wednesday.Unchecked
        hh5.Clear()
        mm5.Clear()
        hh5.IsEnabled = False
        mm5.IsEnabled = False
    End Sub

    Private Sub Thursday_Checked(sender As Object, e As RoutedEventArgs) Handles Thursday.Checked
        hh6.IsEnabled = True
        mm6.IsEnabled = True
    End Sub
    Private Sub Thursday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Thursday.Unchecked
        hh6.Clear()
        mm6.Clear()
        hh6.IsEnabled = False
        mm6.IsEnabled = False
    End Sub

    Private Sub Friday_Checked(sender As Object, e As RoutedEventArgs) Handles Friday.Checked
        hh7.IsEnabled = True
        mm7.IsEnabled = True
    End Sub
    Private Sub Friday_UnChecked(sender As Object, e As RoutedEventArgs) Handles Friday.Unchecked
        hh7.Clear()
        mm7.Clear()
        hh7.IsEnabled = False
        mm7.IsEnabled = False
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
        End If
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        If bm.AccNoShowHelp(MainAccNo, MainAccName, e, , , ) Then
            MainAccNo_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub Cnt_LostFocus(sender As Object, e As RoutedEventArgs) Handles Cnt.LostFocus
        While Val(Cnt.Text) > G.Rows.Count
            G.Rows.Add({G.Rows.Count + 1, ""})
        End While
        While Val(Cnt.Text) < G.Rows.Count
            G.Rows.RemoveAt(G.Rows.Count - 1)
        End While
    End Sub


End Class
