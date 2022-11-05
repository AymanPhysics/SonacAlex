Imports System.Data

Public Class ItemsClothes
    Public TableName As String = "Items"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Dim dt As New DataTable
    Dim bm As New BasicMethods
    WithEvents GColors As New MyGrid
    WithEvents GSizes As New MyGrid

    Public Flag As Integer = 0

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()

        LoadWFHColors()
        LoadWFHSizes()

        lblStoreId.Visibility = Visibility.Hidden
        StoreId.Visibility = Visibility.Hidden
        StoreName.Visibility = Visibility.Hidden
        lblPrintingGroupId.Visibility = Visibility.Hidden
        PrintingGroupId.Visibility = Visibility.Hidden
        PrintingGroupName.Visibility = Visibility.Hidden

        If Not Md.ShowQtySub Then
            lblUnitSub.Visibility = Visibility.Hidden
            lblPurchasePriceSub.Visibility = Visibility.Hidden
            lblSalesPriceSub.Visibility = Visibility.Hidden
            lblUnitCount.Visibility = Visibility.Hidden

            UnitSub.Visibility = Visibility.Hidden
            PurchasePriceSub.Visibility = Visibility.Hidden
            SalesPriceSub.Visibility = Visibility.Hidden
            UnitCount.Visibility = Visibility.Hidden

            UnitSub2.Visibility = Visibility.Hidden
            PurchasePriceSub2.Visibility = Visibility.Hidden
            SalesPriceSub2.Visibility = Visibility.Hidden
            UnitCount2.Visibility = Visibility.Hidden
        End If

        bm.Fields = New String() {SubId, SubName, "GroupId", "TypeId", "PrintingGroupId", "StoreId", "PurchasePrice", "PurchasePriceSub", "PurchasePriceSub2", "SalesPrice", "SalesPriceSub", "SalesPriceSub2", "ItemType", "Unit", "UnitSub", "UnitSub2", "UnitCount", "UnitCount2", "Adding", "IsTables", "IsTakeAway", "IsDelivary", "Limit", "ColorId", "SizeId", "IsStopped"}
        bm.control = New Control() {txtID, txtName, GroupId, TypeId, PrintingGroupId, StoreId, PurchasePrice, PurchasePriceSub, PurchasePriceSub2, SalesPrice, SalesPriceSub, SalesPriceSub2, ItemType, Unit, UnitSub, UnitSub2, UnitCount, UnitCount2, Adding, IsTables, IsTakeAway, IsDelivary, Limit, ColorId, SizeId, IsStopped}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)
        ItemType.SelectedIndex = 0
    End Sub

    Structure GCColors
        Shared Id As String = "Id"
        Shared Name As String = "Name"
    End Structure
    Structure GCSizes
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared PurchasePrice As String = "PurchasePrice"
        Shared SalesPrice As String = "SalesPrice"
        Shared Limit As String = "Limit"
    End Structure

    Private Sub LoadWFHColors()
        WFHColors.Child = GColors
        GColors.Columns.Clear()
        GColors.ForeColor = System.Drawing.Color.DarkBlue
        GColors.Columns.Add(GCColors.Id, "الكود")
        GColors.Columns.Add(GCColors.Name, "الاسم")

        GColors.Columns(GCColors.Id).ReadOnly = True
        GColors.Columns(GCColors.Name).ReadOnly = True

        GColors.Columns(GCColors.Id).FillWeight = 100
        GColors.Columns(GCColors.Name).FillWeight = 300

        GColors.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        GColors.AllowUserToDeleteRows = False
        GColors.AllowUserToAddRows = False

        GColors.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        GColors.TabStop = False
    End Sub

    Private Sub LoadWFHSizes()
        WFHSizes.Child = GSizes
        GSizes.Columns.Clear()
        GSizes.ForeColor = System.Drawing.Color.DarkBlue
        GSizes.Columns.Add(GCSizes.Id, "الكود")
        GSizes.Columns.Add(GCSizes.Name, "الاسم")
        GSizes.Columns.Add(GCSizes.PurchasePrice, "سعر الشراء")
        GSizes.Columns.Add(GCSizes.SalesPrice, "سعر البيع")
        GSizes.Columns.Add(GCSizes.Limit, "حد الطلب")

        GSizes.Columns(GCSizes.Id).ReadOnly = True
        GSizes.Columns(GCSizes.Name).ReadOnly = True

        GSizes.Columns(GCSizes.Id).FillWeight = 100
        GSizes.Columns(GCSizes.Name).FillWeight = 300
        GSizes.Columns(GCSizes.PurchasePrice).FillWeight = 100
        GSizes.Columns(GCSizes.SalesPrice).FillWeight = 100
        GSizes.Columns(GCSizes.Limit).FillWeight = 100

        GSizes.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
        GSizes.AllowUserToDeleteRows = False
        GSizes.AllowUserToAddRows = False
        GSizes.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        GSizes.TabStop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        bm.GetImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
        GroupId_LostFocus(Nothing, Nothing)
        TypeId_LostFocus(Nothing, Nothing)
        PrintingGroupId_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)

        ColorId_LostFocus(Nothing, Nothing)
        SizeId_LostFocus(Nothing, Nothing)

        CurrentColor = 0
        CurrentSize = 0
        ColorId_LostFocus(Nothing, Nothing)
        SizeId_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select D.Id,D.Name,Z.PurchasePrice,Z.SalesPrice,Z.Limit from ItemSizes Z left join SizesDetails D on (Z.Id=D.Id and SizeId='" & Val(SizeId.Text) & "') where Z.ItemId='" & Val(txtID.Text) & "'")
        For i As Integer = 0 To dt.Rows.Count - 1
            If GSizes.Rows.Count <= i Then Exit For
            GSizes.Rows(i).Cells(GCSizes.Id).Value = dt.Rows(i)("Id").ToString
            GSizes.Rows(i).Cells(GCSizes.Name).Value = dt.Rows(i)("Name").ToString
            GSizes.Rows(i).Cells(GCSizes.PurchasePrice).Value = dt.Rows(i)("PurchasePrice").ToString
            GSizes.Rows(i).Cells(GCSizes.SalesPrice).Value = dt.Rows(i)("SalesPrice").ToString
            GSizes.Rows(i).Cells(GCSizes.Limit).Value = dt.Rows(i)("Limit").ToString
        Next
        GSizes.RefreshEdit()

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

        GColors.EndEdit()
        GSizes.EndEdit()

        UnitCount.Text = Val(UnitCount.Text)
        PurchasePrice.Text = Val(PurchasePrice.Text)
        PurchasePriceSub.Text = Val(PurchasePriceSub.Text)
        SalesPrice.Text = Val(SalesPrice.Text)
        SalesPriceSub.Text = Val(SalesPriceSub.Text)

        UnitCount2.Text = Val(UnitCount2.Text)
        PurchasePriceSub2.Text = Val(PurchasePriceSub2.Text)
        SalesPriceSub2.Text = Val(SalesPriceSub2.Text)
        Limit.Text = Val(Limit.Text)

        ColorId.Text = Val(ColorId.Text)
        SizeId.Text = Val(SizeId.Text)



        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        bm.ExecuteNonQuery("delete ItemSizes where ItemId='" & Val(txtID.Text) & "'")

        Dim str As String = "Insert ItemSizes(ItemId,Id,PurchasePrice,SalesPrice,Limit) values "
        For i As Integer = 0 To GSizes.Rows.Count - 1
            Try
                str &= "('" & Val(txtID.Text) & "','" & GSizes.Rows(i).Cells(GCSizes.Id).Value.ToString & "','" & Val(GSizes.Rows(i).Cells(GCSizes.PurchasePrice).Value) & "','" & Val(GSizes.Rows(i).Cells(GCSizes.SalesPrice).Value) & "','" & Val(GSizes.Rows(i).Cells(GCSizes.Limit).Value) & "'),"
            Catch ex As Exception
            End Try
        Next
        str = str.Substring(0, str.Length - 1)
        bm.ExecuteNonQuery(str)

        bm.SaveImage(TableName, New String() {SubId}, New String() {txtID.Text.Trim}, "Image", Image1)
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
        bm.ClearControls()
        ItemType.SelectedIndex = 2

        bm.SetNoImage(Image1)

        GColors.Rows.Clear()
        GSizes.Rows.Clear()

        GroupName.Clear()
        TypeName.Clear()
        PrintingGroupName.Clear()
        StoreName.Clear()

        ColorName.Clear()
        SizeName.Clear()

        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete ItemSizes where ItemId='" & Val(txtID.Text) & "'")
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
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

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")")
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub SizeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SizeId.KeyUp
        bm.ShowHelp("Sizes", SizeId, SizeName, e, "select cast(Id as varchar(100)) Id,Name from Sizes")
    End Sub

    Dim CurrentColor As String
    Private Sub ColorId_GotFocus(sender As Object, e As RoutedEventArgs) Handles ColorId.GotFocus
        CurrentColor = ColorId.Text
    End Sub

    Dim CurrentSize As String
    Private Sub SizeId_GotFocus(sender As Object, e As RoutedEventArgs) Handles SizeId.GotFocus
        CurrentSize = SizeId.Text
    End Sub

    Private Sub ColorId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ColorId.KeyUp
        bm.ShowHelp("Colors", ColorId, ColorName, e, "select cast(Id as varchar(100)) Id,Name from Colors")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, GroupId.KeyDown, TypeId.KeyDown, PrintingGroupId.KeyDown, StoreId.KeyDown, ItemType.KeyDown, Limit.KeyDown, ColorId.KeyDown, SizeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        Try
            If bm.ShowHelp("Items", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from Items") Then txtName.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub PrintingGroupId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PrintingGroupId.KeyUp
        bm.ShowHelp("PrintingGroups", PrintingGroupId, PrintingGroupName, e, "select cast(Id as varchar(100)) Id,Name from PrintingGroups")
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles PurchasePrice.KeyDown, PurchasePriceSub.KeyDown, SalesPrice.KeyDown, SalesPriceSub.KeyDown, PurchasePriceSub2.KeyDown, SalesPriceSub2.KeyDown, UnitCount.KeyDown, UnitCount2.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub PrintingGroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles PrintingGroupId.LostFocus
        bm.LostFocus(PrintingGroupId, PrintingGroupName, "select Name from PrintingGroups where Id=" & PrintingGroupId.Text.Trim())
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

    Private Sub ColorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ColorId.LostFocus
        bm.LostFocus(ColorId, ColorName, "select Name from Colors where Id=" & ColorId.Text.Trim())
        If Val(ColorId.Text) <> Val(CurrentColor) Then
            GColors.Rows.Clear()
            dt = bm.ExecuteAdapter("select * from ColorsDetails where ColorId=" & Val(ColorId.Text))
            For i As Integer = 0 To dt.Rows.Count - 1
                GColors.Rows.Add()
                GColors.Rows(i).Cells(GCColors.Id).Value = dt.Rows(i)("Id")
                GColors.Rows(i).Cells(GCColors.Name).Value = dt.Rows(i)("Name")
            Next
        End If
    End Sub

    Private Sub SizeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SizeId.LostFocus
        bm.LostFocus(SizeId, SizeName, "select Name from Sizes where Id=" & SizeId.Text.Trim())
        If Val(SizeId.Text) <> Val(CurrentSize) Then
            GSizes.Rows.Clear()
            dt = bm.ExecuteAdapter("select * from SizesDetails where SizeId=" & Val(SizeId.Text))
            For i As Integer = 0 To dt.Rows.Count - 1
                GSizes.Rows.Add()
                GSizes.Rows(i).Cells(GCSizes.Id).Value = dt.Rows(i)("Id")
                GSizes.Rows(i).Cells(GCSizes.Name).Value = dt.Rows(i)("Name")
                GSizes.Rows(i).Cells(GCSizes.PurchasePrice).Value = PurchasePrice.Text
                GSizes.Rows(i).Cells(GCSizes.SalesPrice).Value = SalesPrice.Text
                GSizes.Rows(i).Cells(GCSizes.Limit).Value = Limit.Text
            Next
        End If
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub btnSetImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetImage.Click
        bm.SetImage(Image1)
    End Sub

    Private Sub btnSetNoImage_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSetNoImage.Click
        bm.SetNoImage(Image1, False, True)
    End Sub

    Private Sub SalesUnitCount_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                SalesPriceSub.Text = 0
            Else
                SalesPriceSub.Text = Val(SalesPrice.Text) * Val(UnitCount.Text)
            End If
        Catch ex As Exception
            SalesPriceSub.Text = 0
        End Try
    End Sub

    Private Sub SalesUnitCount2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount2.LostFocus
        Try
            If Val(UnitCount2.Text) = 0 Then
                SalesPriceSub2.Text = 0
            Else
                SalesPriceSub2.Text = Val(SalesPrice.Text) * Val(UnitCount2.Text)
            End If
        Catch ex As Exception
            SalesPriceSub2.Text = 0
        End Try
    End Sub

    Private Sub PurchaseUnitCount_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount.LostFocus
        Try
            If Val(UnitCount.Text) = 0 Then
                PurchasePriceSub.Text = 0
            Else
                PurchasePriceSub.Text = Val(PurchasePrice.Text) * Val(UnitCount.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub.Text = 0
        End Try
    End Sub

    Private Sub PurchaseUnitCount2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles UnitCount2.LostFocus
        Try
            If Val(UnitCount2.Text) = 0 Then
                PurchasePriceSub2.Text = 0
            Else
                PurchasePriceSub2.Text = Val(PurchasePrice.Text) * Val(UnitCount2.Text)
            End If
        Catch ex As Exception
            PurchasePriceSub2.Text = 0
        End Try
    End Sub

    Private Sub ItemType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles ItemType.SelectionChanged
        Select Case ItemType.SelectedIndex
            Case 0, 1
                IsDelivary.IsChecked = False
                IsTables.IsChecked = False
                IsTakeAway.IsChecked = False
            Case 2, 3
                IsDelivary.IsChecked = True
                IsTables.IsChecked = True
                IsTakeAway.IsChecked = True
        End Select
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
