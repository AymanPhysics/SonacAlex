Imports System.Data

Public Class Degrees
    Public TableName As String = "Degrees"
    Public SubId As String = "Id"
    Public SubName As String = "Name"


    Public TableDetailsName As String = "VisitingTypeDegrees"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid 
    Dim m As MainWindow = Application.Current.MainWindow
    Public Flag As Integer = 0
    Public WithImage As Boolean = False

    Public Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        LoadWFH() 
        If WithImage Then
            btnSetImage.Visibility = Visibility.Visible
            btnSetNoImage.Visibility = Visibility.Visible
            Image1.Visibility = Visibility.Visible
        End If
        bm.Fields = New String() {SubId, SubName}
        bm.control = New Control() {txtID, txtName}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)

        bm.SetImage(Img, "attchmentfile.jpg")

    End Sub

    Structure GC
        Shared VisitingTypeId As String = "VisitingTypeId"
        Shared VisitingTypeName As String = "VisitingTypeName"
        Shared Price As String = "Price"
    End Structure

    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.VisitingTypeId, "VisitingTypeId")
        G.Columns.Add(GC.VisitingTypeName, "النوع")

        G.Columns.Add(GC.Price, "السعر")


        G.Columns(GC.VisitingTypeName).FillWeight = 200 

        G.Columns(GC.VisitingTypeId).ReadOnly = True
        G.Columns(GC.VisitingTypeName).ReadOnly = True 
        G.Columns(GC.VisitingTypeId).Visible = False

        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False
         
    End Sub
     

    Sub FillControls()
        bm.FillControls(Me)
        If WithImage Then bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        FillGrid() 

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
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
            txtName.Focus()
            Return
        End If

        G.EndEdit()

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        If WithImage Then bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)

        If Not bm.SaveGrid(G, TableDetailsName, New String() {"DegreeId"}, New String() {txtID.Text}, New String() {"VisitingTypeId", "Price"}, New String() {GC.VisitingTypeId, GC.Price}, New VariantType() {VariantType.Integer, VariantType.Decimal}, New String() {GC.VisitingTypeId, GC.VisitingTypeId}) Then Return

        bm.ExecuteNonQuery("UpdateDoctorPrices", {"Id"}, {Val(txtID.Text)})
       
        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub


    Sub ClearControls()
        If lv Then Return
        bm.SetNoImage(Image1)
        bm.ClearControls(False)
        CheckBox2.IsChecked = True
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"
        FillGrid() 

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            DeleteDetails()
            btnNew_Click(sender, e)
        End If
    End Sub

    Sub DeleteDetails()
        bm.ExecuteNonQuery("delete from " & TableDetailsName & " where DegreeId='" & txtID.Text.Trim & "'")

    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName, TableName) Then
            txtName.Focus()
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub


    

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

        lblId.SetResourceReference(Label.ContentProperty, "Id")
        lblName.SetResourceReference(Label.ContentProperty, "Name")
    End Sub

    Private Sub FillGrid()
        dt = bm.ExecuteAdapter("GetVisitingTypeDegrees", {"DegreeId"}, {Val(txtID.Text)})
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add({dt.Rows(i)("VisitingTypeId"), dt.Rows(i)("VisitingTypeName"), dt.Rows(i)("Price")})
        Next
    End Sub
      
    Private Sub G_PreviewKeyDown(sender As Object, e As Forms.PreviewKeyDownEventArgs)
        If e.KeyCode = Forms.Keys.Delete AndAlso G.CurrentRow.Index >= 0 AndAlso bm.ShowDeleteMSG() Then
            G.Rows.Remove(G.CurrentRow)
        End If
    End Sub
     

End Class
