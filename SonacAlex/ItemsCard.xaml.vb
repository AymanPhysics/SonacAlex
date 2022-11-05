Imports System.Data

Public Class ItemsCard

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Dim dt2 As New DataTable
    Dim dt3 As New DataTable
    Public Flag As Integer = 0
    Dim dv As New DataView
    Dim dv2 As New DataView
    Dim dv3 As New DataView

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.SelectionChanged, FromDate.SelectedDateChanged, ToDate.SelectedDateChanged
        If StoreId.SelectedIndex < 1 OrElse FromDate.SelectedDate Is Nothing OrElse ToDate.SelectedDate Is Nothing Then
            Return
        End If

        dt = bm.ExecuteAdapter("GetItemCardWindow", New String() {"StoreId", "FromDate", "ToDate"}, {Val(StoreId.SelectedValue), bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate)})
        dt.TableName = "tbl"

        dv.Table = dt
        DataGridView1.ItemsSource = dv

        DataGridView1.IsReadOnly = True
        DataGridView1.RowHeaderWidth = 0

        DataGridView1.MinColumnWidth = 0
        DataGridView1.Columns(dt.Columns("GroupId").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("TypeId").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ItemType").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("GroupName").Ordinal).Header = "المجموعة"
        DataGridView1.Columns(dt.Columns("TypeName").Ordinal).Header = "النوع"
        DataGridView1.Columns(dt.Columns("Bal0").Ordinal).Header = "رصيد أول المدة"
        DataGridView1.Columns(dt.Columns("Id").Ordinal).Header = "كود الصنف"
        DataGridView1.Columns(dt.Columns("Name").Ordinal).Header = "اسم الصنف"
        DataGridView1.Columns(dt.Columns("Unit").Ordinal).Header = "الوحدة"
        DataGridView1.Columns(dt.Columns("Bal").Ordinal).Header = "الرصيد"
        txtId_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.FillCombo("Stores", StoreId, "")
        bm.FillCombo("Groups", GroupId, "")
        bm.FillCombo("select Id,Name from SalesFlags where Id not in(20,26) and Id in(select distinct Flag from SalesMaster)union all select 0 Id,'-' Name union all select -1 Id,'فك وتركيب' Name order by Name", CboFlag)
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({ItemId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1,1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Dim lop As Boolean = False

    Private Sub LoadResource()
        'Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")

    End Sub


    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelpMultiColumns("Items", ItemId, ItemName, e, "select cast(Id as varchar(100)) 'كود الصنف',Name 'اسم الصنف',dbo.GetGroupName(GroupId)'المجموعة',dbo.GetTypeName(GroupId,TypeId)'النوع' from Items where ItemType<>3 and (GroupId='" & Val(GroupId.SelectedValue) & "' or '" & Val(GroupId.SelectedValue) & "'=0) and (TypeId='" & Val(TypeId.SelectedValue) & "' or '" & Val(TypeId.SelectedValue) & "'=0)") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
    End Sub


    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub GroupId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles GroupId.SelectionChanged
        Try
            bm.FillCombo("Types", TypeId, " where GroupId=" & GroupId.SelectedValue.ToString)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles P.Checked, P.Unchecked, Z.Checked, Z.Unchecked, N.Checked, N.Unchecked, GroupId.SelectionChanged, TypeId.SelectionChanged, ItemId.TextChanged, ItemName.TextChanged
        Try
            dv.RowFilter = " [Id] like '%" & ItemId.Text.Trim & "%'"
            dv.RowFilter &= " and [Name] like '%" & ItemName.Text.Trim & "%'"
            dv.RowFilter &= " and ([GroupId] ='" & GroupId.SelectedValue & "' or '" & GroupId.SelectedValue & "'=0)"
            dv.RowFilter &= " and ([TypeId] ='" & TypeId.SelectedValue & "' or '" & TypeId.SelectedValue & "'=0)"
            If Not P.IsChecked Then dv.RowFilter &= " and Not [Bal]>0"
            If Not Z.IsChecked Then dv.RowFilter &= " and Not [Bal]=0"
            If Not N.IsChecked Then dv.RowFilter &= " and Not [Bal]<0"
        Catch
        End Try
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView1.SelectionChanged
        Try
            'bm.SetNoImage(Image1)
            bm.GetImage("Items", New String() {"Id"}, New String() {DataGridView1.CurrentItem("Id")}, "Image", Image1)

            dt2 = bm.ExecuteAdapter("GetItemCardWindowSub", New String() {"StoreId", "ItemId", "FromDate", "ToDate"}, {Val(StoreId.SelectedValue), DataGridView1.CurrentItem("Id"), bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate)})
            dt2.TableName = "tbl"

            dv2.Table = dt2
            DataGridView2.ItemsSource = dv2
            DataGridView2.IsReadOnly = True

            DataGridView2.Columns(dt2.Columns("StoreId").Ordinal).Visibility = Windows.Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("Flag").Ordinal).Visibility = Windows.Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("ToId").Ordinal).Visibility = Windows.Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("Title").Ordinal).Visibility = Windows.Visibility.Hidden
            'DataGridView2.Columns(dt2.Columns("SF_Name").Ordinal).Header = "نوع الحركة"
            'DataGridView2.Columns(dt2.Columns("StoreName").Ordinal).Header = "المخزن"
            'DataGridView2.Columns(dt2.Columns("InvoiceNo").Ordinal).Header = "المسلسل"
            'Try : DataGridView2.Columns(dt2.Columns("ToName").Ordinal).Header = dt2.Rows(0)("Title") : Catch : End Try
            'DataGridView2.Columns(dt2.Columns("ToName").Ordinal).Header = "الجهة"
            CboFlag_SelectionChanged(Nothing, Nothing)
            DataGridView2_SelectionChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Private Sub CboFlag_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CboFlag.SelectionChanged
        Try
            dv2.RowFilter = " (Flag ='" & CboFlag.SelectedValue & "' or '" & CboFlag.SelectedValue & "'=0)"
        Catch
        End Try
    End Sub

    Private Sub DataGridView2_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView2.SelectionChanged
        Try

            DataGridView3.ItemsSource = Nothing
            dt3 = bm.ExecuteAdapter("GetSalesSpecific", New String() {"StoreId", "Flag", "InvoiceNo", "EmpId"}, {DataGridView2.CurrentItem("StoreId"), DataGridView2.CurrentItem("Flag"), DataGridView2.CurrentItem("المسلسل"), Md.UserName})
            dt3.TableName = "tbl"

            dv3.Table = dt3
            DataGridView3.ItemsSource = dv3
            DataGridView3.IsReadOnly = True

            DataGridView3.Columns(dt3.Columns("ItemId").Ordinal).Header = "كود الصنف"
            DataGridView3.Columns(dt3.Columns("ItemName").Ordinal).Header = "اسم الصنف"
            DataGridView3.Columns(dt3.Columns("Qty").Ordinal).Header = "الكمية"
            DataGridView3.Columns(dt3.Columns("Qty2").Ordinal).Header = "العدد/عبوة"
            DataGridView3.Columns(dt3.Columns("Qty3").Ordinal).Header = "عدد العبوات"
            DataGridView3.Columns(dt3.Columns("Price").Ordinal).Header = "السعر"
            DataGridView3.Columns(dt3.Columns("Value").Ordinal).Header = "القيمة"
        Catch
        End Try
    End Sub


End Class