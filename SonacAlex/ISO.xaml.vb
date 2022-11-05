Imports System.Data
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar

Public Class ISO
    Public TableName As String = "ISO"
    Public MainId As String = "CustomerId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        MyAttach.Flag = MyFlag.ISO

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        bm.FillCombo("ShippedPers", ShippedPerId, "")
        bm.FillCombo("Banks", BankId, "")
        bm.FillCombo("Ports", FromPortId, "")
        bm.FillCombo("Ports2", ToPortId, "")

        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {MainId, SubId, SubId2, "DayDate", "Canceled", "CurrencyId", "IsPosted", "ShippedPerId", "FromPortId", "ToPortId", "BankId", "PaymentMothod", "Notes"}
        bm.control = New Control() {CustomerId, txtFlag, txtID, DayDate, Canceled, CurrencyId, IsPosted, ShippedPerId, FromPortId, ToPortId, BankId, PaymentMothod, Notes}
        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName

        LoadWFH()

        btnNew_Click(sender, e)
        CustomerId.Focus()
    End Sub



    Structure GC
        Shared Id As String = "Id"
        Shared Mark As String = "Mark"
        Shared ContainerTypeId As String = "ContainerTypeId"
        Shared UnitQty As String = "UnitQty"
        Shared UnitsWeightId As String = "UnitsWeightId"
        Shared UnitsWeightQty As String = "UnitsWeightQty"
        Shared PreQty As String = "PreQty"
        Shared Qty As String = "Qty"
        Shared TypeOfPriceId As String = "TypeOfPriceId"
        Shared Line As String = "Line"
        Shared Sizes As String = "Sizes"
        Shared SD_Notes As String = "SD_Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCId As New Forms.DataGridViewComboBoxColumn
        GCId.HeaderText = "Item"
        GCId.Name = GC.Id
        bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+EnName From Items where IsStopped=0 and IsExports=1", GCId)
        G.Columns.Add(GCId)

        'Dim GCMarkId As New Forms.DataGridViewComboBoxColumn
        'GCMarkId.HeaderText = "Mark"
        'GCMarkId.Name = GC.MarkId
        'bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+Name From Marks", GCMarkId)
        'G.Columns.Add(GCMarkId)

        G.Columns.Add(GC.Mark, "Mark")

        Dim GCContainerTypeId As New Forms.DataGridViewComboBoxColumn
        GCContainerTypeId.HeaderText = "Container Type"
        GCContainerTypeId.Name = GC.ContainerTypeId
        bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+Name From ContainerTypes", GCContainerTypeId)
        G.Columns.Add(GCContainerTypeId)

        G.Columns.Add(GC.UnitQty, "عدد الفرعى")

        Dim GCUnitsWeightId As New Forms.DataGridViewComboBoxColumn
        GCUnitsWeightId.HeaderText = "Unit"
        GCUnitsWeightId.Name = GC.UnitsWeightId
        bm.FillCombo("select 0 Id,'' Name union all select Id,Name from UnitsWeights where Id>0", GCUnitsWeightId)
        G.Columns.Add(GCUnitsWeightId)

        G.Columns.Add(GC.UnitsWeightQty, "Weight")


        G.Columns.Add(GC.PreQty, "Qty")
        G.Columns.Add(GC.Qty, "Net Weight")


        Dim GCTypeOfPriceId As New Forms.DataGridViewComboBoxColumn
        GCTypeOfPriceId.HeaderText = "Type of Price"
        GCTypeOfPriceId.Name = GC.TypeOfPriceId
        bm.FillCombo("select 0 Id,'' Name union all select Id,Name from TypeOfPrices", GCTypeOfPriceId)
        G.Columns.Add(GCTypeOfPriceId)

        G.Columns.Add(GC.Line, "Line")


        G.Columns.Add(GC.Sizes, "Sizes")
        G.Columns.Add(GC.SD_Notes, "Notes")


        G.Columns(GC.Id).FillWeight = 300 '110
        G.Columns(GC.Mark).FillWeight = 200
        G.Columns(GC.ContainerTypeId).FillWeight = 200

        G.Columns(GC.UnitsWeightId).FillWeight = 150

        G.Columns(GC.Sizes).FillWeight = 200
        G.Columns(GC.SD_Notes).FillWeight = 280

        G.Columns(GC.UnitQty).ReadOnly = True

        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitsWeightQty).Visible = False

        G.Columns(GC.Line).Visible = False
        G.Columns(GC.Qty).ReadOnly = True

        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        bm.FillControls(Me)
        CustomerId_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "=" & txtFlag.Text.Trim & " and " & SubId2 & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.Mark).Value = dt.Rows(i)("Mark").ToString
            G.Rows(i).Cells(GC.ContainerTypeId).Value = dt.Rows(i)("ContainerTypeId").ToString
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.TypeOfPriceId).Value = dt.Rows(i)("TypeOfPriceId").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.Sizes).Value = dt.Rows(i)("Sizes").ToString
            G.Rows(i).Cells(GC.SD_Notes).Value = dt.Rows(i)("SD_Notes").ToString

            G.Rows(i).Cells(GC.UnitsWeightId).Value = dt.Rows(i)("UnitsWeightId").ToString
            G.Rows(i).Cells(GC.UnitsWeightQty).Value = dt.Rows(i)("UnitsWeightQty").ToString
            G.Rows(i).Cells(GC.PreQty).Value = dt.Rows(i)("PreQty").ToString
        Next


        MyAttach.Key1 = Val(CustomerId.Text)
        MyAttach.Key2 = Val(txtID.Text)
        MyAttach.LoadTree()

        DayDate.Focus()
        G.RefreshEdit()
        lop = False

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If Val(CustomerId.Text) = 0 Then
            bm.ShowMSG("Please, Select a customer")
            CustomerId.Focus()
            Return
        End If
        If Not bm.TestDateValidity(DayDate) Then Return

        G.EndEdit()

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If


        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}, New String() {"ItemId", "Mark", "ContainerTypeId", "Qty", "SD_Notes", "UnitsWeightId", "UnitsWeightQty", "PreQty", "TypeOfPriceId", "Sizes"}, New String() {GC.Id, GC.Mark, GC.ContainerTypeId, GC.Qty, GC.SD_Notes, GC.UnitsWeightId, GC.UnitsWeightQty, GC.PreQty, GC.TypeOfPriceId, GC.Sizes}, New VariantType() {VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String}, New String() {}) Then Return


        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Mark).Value = Nothing
        G.Rows(i).Cells(GC.ContainerTypeId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.TypeOfPriceId).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.Sizes).Value = Nothing
        G.Rows(i).Cells(GC.SD_Notes).Value = Nothing

        G.Rows(i).Cells(GC.UnitsWeightId).Value = Nothing
        G.Rows(i).Cells(GC.UnitsWeightQty).Value = Nothing
        G.Rows(i).Cells(GC.PreQty).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.UnitsWeightId Then
                G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value = Val(bm.ExecuteScalar("select Weights from UnitsWeights where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightId).Value)))
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.PreQty OrElse G.Columns(e.ColumnIndex).Name = GC.UnitsWeightQty Then
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value)
            End If

            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub



    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True

        DayDate.SelectedDate = bm.MyGetDate()
        G.Rows.Clear()

        CustomerId_LostFocus(Nothing, Nothing)
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"

        MyAttach.Key1 = Val(CustomerId.Text)
        MyAttach.Key2 = Val(txtID.Text)
        MyAttach.LoadTree()

        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}, dt)
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

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub CustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CustomerId.LostFocus

        If Val(CustomerId.Text.Trim) = 0 Then
            CustomerId.Clear()
            CustomerName.Clear()
            Return
        End If
        bm.LostFocus(CustomerId, CustomerName, "select EnName from Customers where Id=" & CustomerId.Text.Trim())
        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub CustomerId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CustomerId.KeyUp
        If bm.ShowHelp("Customers", CustomerId, CustomerName, e, "select cast(Id as varchar(100)) Id,EnName from Customers") Then
            CustomerId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        lblDayDate.SetResourceReference(Label.ContentProperty, "DayDate")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        'e.Handled = True
        If G.CurrentCell Is Nothing Then Return
        If G.CurrentCell.ReadOnly Then Return
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If

        Catch ex As Exception
        End Try
        G.CommitEdit(Forms.DataGridViewDataErrorContexts.Commit)
    End Sub



    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        DontClear = True
        If btnSave.IsEnabled AndAlso btnSave.Visibility = Windows.Visibility.Visible Then
            btnSave_Click(sender, e)
        Else
            AllowSave = True
        End If
        DontClear = False
        If Not AllowSave Then Return
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@CustomerId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(CustomerId.Text), txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "ISOOne.rpt"
        rpt.Show()
    End Sub
End Class
