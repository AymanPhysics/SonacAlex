Imports System.Data
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar

Public Class ProForma
    Public TableName As String = "ProFormaMaster"
    Public TableDetailsName As String = "ProFormaDetails"
    Public MainId As String = "CustomerId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return

        MyAttach.Flag = Flag

        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        bm.FillCombo("ShippedPers", ShippedPerId, "")
        bm.FillCombo("Banks", BankId, "")
        bm.FillCombo("Ports", FromPortId, "")
        bm.FillCombo("Ports2", ToPortId, "")

        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {MainId, SubId, SubId2, "DayDate", "Canceled", "CurrencyId", "IsPosted", "ShippedPerId", "FromPortId", "ToPortId", "Advance", "Remaining", "Total", "AdvanceDate", "RemainingDate", "BankId", "Freight", "PaymentMothod", "Notes", "SubCustomerId"}
        bm.control = New Control() {CustomerId, txtFlag, txtID, DayDate, Canceled, CurrencyId, IsPosted, ShippedPerId, FromPortId, ToPortId, Advance, Remaining, Total, AdvanceDate, RemainingDate, BankId, Freight, PaymentMothod, Notes, SubCustomerId}
        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName

        LoadWFH()

        btnNew_Click(sender, e)
        CustomerId.Focus()
    End Sub



    Structure GC
        Shared ItemsTitleId As String = "ItemsTitleId"
        Shared Id As String = "Id"
        Shared GradeId As String = "GradeId"
        Shared Mark As String = "Mark"
        Shared ContainerTypeId As String = "ContainerTypeId"
        Shared UnitQty As String = "UnitQty"
        Shared UnitsWeightId As String = "UnitsWeightId"
        Shared UnitsWeightQty As String = "UnitsWeightQty"
        Shared PreQty As String = "PreQty"
        Shared Qty As String = "Qty"
        Shared QtyTon As String = "QtyTon"
        Shared Price As String = "Price"
        Shared PriceTypeId As String = "PriceTypeId"
        Shared Value As String = "Value"
        Shared TypeOfPriceId As String = "TypeOfPriceId"
        Shared Line As String = "Line"
        Shared Sizes As String = "Sizes"
        Shared SD_Notes As String = "SD_Notes"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCItemsTitleId As New Forms.DataGridViewComboBoxColumn
        GCItemsTitleId.HeaderText = "Title"
        GCItemsTitleId.Name = GC.ItemsTitleId
        bm.FillCombo("select 0 Id,'' Name union select Id,Name From ItemTitles", GCItemsTitleId)
        G.Columns.Add(GCItemsTitleId)

        Dim GCId As New Forms.DataGridViewComboBoxColumn
        GCId.HeaderText = "Item"
        GCId.Name = GC.Id
        bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+EnName From Items where IsStopped=0 and IsExports=1", GCId)
        G.Columns.Add(GCId)

        Dim GCGradeId As New Forms.DataGridViewComboBoxColumn
        GCGradeId.HeaderText = "Grade"
        GCGradeId.Name = GC.GradeId
        bm.FillCombo("select 0 Id,'' Name union select Id,Name From Grades", GCGradeId)
        G.Columns.Add(GCGradeId)

        'Dim GCMarkId As New Forms.DataGridViewComboBoxColumn
        'GCMarkId.HeaderText = "Mark"
        'GCMarkId.Name = GC.MarkId
        'bm.FillCombo("select 0 Id,'' Name union select Id,cast(Id as nvarchar(100))+' - '+Name From Marks", GCMarkId)
        'G.Columns.Add(GCMarkId)

        G.Columns.Add(GC.Mark, "Mark")

        Dim GCContainerTypeId As New Forms.DataGridViewComboBoxColumn
        GCContainerTypeId.HeaderText = "Package Type"
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
        G.Columns.Add(GC.Qty, "Net Weight /Kg")
        G.Columns.Add(GC.QtyTon, "Net Weight /Ton")

        G.Columns.Add(GC.Price, "Price")

        Dim GCPriceTypeId As New Forms.DataGridViewComboBoxColumn
        GCPriceTypeId.HeaderText = "Type"
        GCPriceTypeId.Name = GC.PriceTypeId
        bm.FillCombo("select 0 Id,'' Name union all select Id,Name from PriceTypes", GCPriceTypeId)
        G.Columns.Add(GCPriceTypeId)

        G.Columns.Add(GC.Value, "Value")

        Dim GCTypeOfPriceId As New Forms.DataGridViewComboBoxColumn
        GCTypeOfPriceId.HeaderText = "Type of Price"
        GCTypeOfPriceId.Name = GC.TypeOfPriceId
        bm.FillCombo("select 0 Id,'' Name union all select Id,Name from TypeOfPrices", GCTypeOfPriceId)
        G.Columns.Add(GCTypeOfPriceId)

        G.Columns.Add(GC.Line, "Line")


        G.Columns.Add(GC.Sizes, "Sizes")
        G.Columns.Add(GC.SD_Notes, "Notes")


        G.Columns(GC.ItemsTitleId).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 300 '110
        G.Columns(GC.GradeId).FillWeight = 150
        G.Columns(GC.Mark).FillWeight = 200
        G.Columns(GC.ContainerTypeId).FillWeight = 200

        G.Columns(GC.UnitsWeightId).FillWeight = 150

        G.Columns(GC.PriceTypeId).FillWeight = 150

        G.Columns(GC.Sizes).FillWeight = 200
        G.Columns(GC.SD_Notes).FillWeight = 280

        G.Columns(GC.UnitQty).ReadOnly = True

        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitsWeightQty).Visible = False

        G.Columns(GC.Line).Visible = False
        G.Columns(GC.Qty).ReadOnly = True
        G.Columns(GC.QtyTon).ReadOnly = True

        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill

        If Flag = MyFlag.ISO Then
            G.Columns(GC.Price).Visible = False
            G.Columns(GC.PriceTypeId).Visible = False
            G.Columns(GC.TypeOfPriceId).Visible = False
            G.Columns(GC.Value).Visible = False
        End If

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
        SubCustomerId_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableDetailsName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "=" & txtFlag.Text.Trim & " and " & SubId2 & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.ItemsTitleId).Value = dt.Rows(i)("ItemsTitleId").ToString
            G.Rows(i).Cells(GC.Id).Value = dt.Rows(i)("ItemId").ToString
            G.Rows(i).Cells(GC.GradeId).Value = dt.Rows(i)("GradeId").ToString
            G.Rows(i).Cells(GC.Mark).Value = dt.Rows(i)("Mark").ToString
            G.Rows(i).Cells(GC.ContainerTypeId).Value = dt.Rows(i)("ContainerTypeId").ToString
            G.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            G.Rows(i).Cells(GC.QtyTon).Value = dt.Rows(i)("QtyTon").ToString
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            G.Rows(i).Cells(GC.PriceTypeId).Value = dt.Rows(i)("PriceTypeId").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
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
        MyAttach.Flag = Flag
        MyAttach.LoadTree()

        DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()

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

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Value).Value) = 0 Then
                Continue For
            End If

        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If txtID.Text.Trim = "" Then
            txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "=" & txtFlag.Text)
            If txtID.Text = "" Then txtID.Text = "1"
            lblLastEntry.Text = txtID.Text
            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()

        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                txtID.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, TableDetailsName, New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text}, New String() {"ItemsTitleId", "ItemId", "GradeId", "Mark", "ContainerTypeId", "Qty", "QtyTon", "Price", "PriceTypeId", "Value", "SD_Notes", "UnitsWeightId", "UnitsWeightQty", "PreQty", "TypeOfPriceId", "Sizes"}, New String() {GC.ItemsTitleId, GC.Id, GC.GradeId, GC.Mark, GC.ContainerTypeId, GC.Qty, GC.QtyTon, GC.Price, GC.PriceTypeId, GC.Value, GC.SD_Notes, GC.UnitsWeightId, GC.UnitsWeightQty, GC.PreQty, GC.TypeOfPriceId, GC.Sizes}, New VariantType() {VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Decimal, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String}, New String() {}) Then Return


        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {CustomerId.Text, txtFlag.Text.Trim, txtID.Text},, TableDetailsName) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.ItemsTitleId).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.GradeId).Value = Nothing
        G.Rows(i).Cells(GC.Mark).Value = Nothing
        G.Rows(i).Cells(GC.ContainerTypeId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.QtyTon).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.PriceTypeId).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
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
            If G.Columns(e.ColumnIndex).Name = GC.Value Then
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value)
            End If

            If G.Columns(e.ColumnIndex).Name = GC.UnitsWeightId Then
                G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value = Val(bm.ExecuteScalar("select Weights from UnitsWeights where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightId).Value)))
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = bm.Round(Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.PreQty OrElse G.Columns(e.ColumnIndex).Name = GC.UnitsWeightQty Then
                G.Rows(e.RowIndex).Cells(GC.Qty).Value = bm.Round(Val(G.Rows(e.RowIndex).Cells(GC.UnitsWeightQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value))
            End If
            G.Rows(e.RowIndex).Cells(GC.QtyTon).Value = bm.Round(Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value) / 1000)
            If Val(G.Rows(e.RowIndex).Cells(GC.PriceTypeId).Value) = 1 Then
                G.Rows(e.RowIndex).Cells(GC.Value).Value = bm.Round(Val(G.Rows(e.RowIndex).Cells(GC.PreQty).Value) * Val(G.Rows(e.RowIndex).Cells(GC.Price).Value))
            ElseIf Val(G.Rows(e.RowIndex).Cells(GC.PriceTypeId).Value) = 2 Then
                G.Rows(e.RowIndex).Cells(GC.Value).Value = bm.Round(Val(G.Rows(e.RowIndex).Cells(GC.QtyTon).Value) * Val(G.Rows(e.RowIndex).Cells(GC.Price).Value))
            Else
                G.Rows(e.RowIndex).Cells(GC.Value).Value = 0
            End If


            CalcTotal()
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
        If lop Then Return
        lop = True

        DayDate.SelectedDate = bm.MyGetDate()
        G.Rows.Clear()
        CalcTotal()

        CustomerId_LostFocus(Nothing, Nothing)
        SubCustomerId_LostFocus(Nothing, Nothing)
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag

        txtID.Clear()

        MyAttach.Key1 = Val(CustomerId.Text)
        MyAttach.Key2 = Val(txtID.Text)
        MyAttach.LoadTree()

        DayDate.Focus()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & MainId & "=" & CustomerId.Text & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
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

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Total.KeyDown
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

    Private Sub SubCustomerId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubCustomerId.LostFocus
        If Val(SubCustomerId.Text.Trim) = 0 Then
            SubCustomerId.Clear()
            SubCustomerName.Clear()
            Return
        End If
        bm.LostFocus(SubCustomerId, SubCustomerName, "select Name from SubCustomers where Id=" & SubCustomerId.Text.Trim())
    End Sub
    Private Sub SubCustomerId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubCustomerId.KeyUp
        If bm.ShowHelp("SubCustomers", SubCustomerId, SubCustomerName, e, "select cast(Id as varchar(100)) Id,Name from SubCustomers", "SubCustomers") Then
            SubCustomerId_LostFocus(Nothing, Nothing)
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
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal() Handles Advance.TextChanged, Freight.TextChanged
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            Total.Text = Math.Round(0, 2)
            For i As Integer = 0 To G.Rows.Count - 1
                Total.Text += Val(G.Rows(i).Cells(GC.Value).Value)
            Next
            Total.Text += Val(Freight.Text)
            Remaining.Text = Val(Total.Text) - Val(Advance.Text)
            LopCalc = False
        Catch ex As Exception
        End Try
    End Sub

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
        rpt.paraname = New String() {"@Flag", "@CustomerId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Flag, Val(CustomerId.Text), txtID.Text, CType(Parent, Page).Title}
        Select Case Flag
            Case MyFlag.ProForma
                rpt.Rpt = "ProFormaOne.rpt"
            Case MyFlag.ISO
                rpt.Rpt = "ISOOne.rpt"
            Case MyFlag.CustomsInvoice
                rpt.Rpt = "CustomsInvoice.rpt"
            Case MyFlag.RealInvoice
                rpt.Rpt = "RealInvoice.rpt"
        End Select
        rpt.Show()
    End Sub
End Class
