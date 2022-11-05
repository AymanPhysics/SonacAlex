Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class ItemCollectionMotion

    Public MainTableName As String = "Stores"
    Public MainSubId As String = "Id"
    Public MainSubName As String = "Name"

    Public TableName As String = "ItemCollectionMotionMaster"
    Public TableDetailsName As String = "ItemCollectionMotionDetailsFrom"
    Public TableDetailsName2 As String = "ItemCollectionMotionDetailsTo"

    Public MainId As String = "StoreId"
    Public SubId As String = "InvoiceNo"

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Dim StaticsDt As New DataTable
    WithEvents GFrom As New MyGrid
    WithEvents GTo As New MyGrid
    WithEvents GCurrent As New MyGrid
    WithEvents MyTimer As New Threading.DispatcherTimer
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Sub NewId()
        InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {btnPrint})
        'btnPrint.Visibility = Windows.Visibility.Hidden
        LoadResource()
        DayDate.SelectedDate = Nothing
        DayDate.SelectedDate = bm.MyGetDate() 

        LoadWFH(WFHFrom, GFrom)
        LoadWFH(WFHTo, GTo)
        bm.FillCombo("Employees", SystemUser, "", , True)

         

        RdoGrouping_Checked(Nothing, Nothing)
        Done_Checked(Nothing, Nothing)

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

        bm.Fields = New String() {MainId, SubId, "DayDate", "DayDate2", "Done", "Notes", "DocNo", "MotionTypeId", "SystemUser", "Temp"}
        bm.control = New Control() {StoreId, InvoiceNo, DayDate, DayDate2, Done, Notes, DocNo, MotionTypeId, SystemUser, Temp}
        bm.KeyFields = New String() {MainId, SubId}

        bm.Table_Name = TableName

        LoadGroups()
        LoadAllItems()

        StoreId.Text = Md.DefaultStore
        StoreId_LostFocus(Nothing, Nothing)
         
        btnNew_Click(Nothing, Nothing)
    End Sub


    Structure GC
        Shared MainItemId As String = "MainItemId"
        Shared Barcode As String = "Barcode"
        Shared ItemId As String = "ItemId"
        Shared ItemName As String = "ItemName"
        Shared UnitId As String = "UnitId"
        Shared UnitQty As String = "UnitQty"
        Shared CurrentBal As String = "CurrentBal"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared Value As String = "Value"
        Shared TotalQty As String = "TotalQty"
        Shared SerialNo As String = "SerialNo"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH(WFH As Forms.Integration.WindowsFormsHost, G As MyGrid)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.MainItemId, "الصنف الرئيسي")
        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.ItemId, "كود الصنف")
        G.Columns.Add(GC.ItemName, "اسم الصنف")

        Dim GCUnitId As New Forms.DataGridViewComboBoxColumn
        GCUnitId.HeaderText = "الوحدة"
        GCUnitId.Name = GC.UnitId
        bm.FillCombo("select 0 Id,'' Name", GCUnitId)
        G.Columns.Add(GCUnitId)

        G.Columns.Add(GC.UnitQty, "عدد الفرعى")

        G.Columns.Add(GC.CurrentBal, "الرصيد")

        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Price, "السعر")
        G.Columns.Add(GC.Value, "القيمة")
        G.Columns.Add(GC.TotalQty, "إجمالى الكمية")

        G.Columns.Add(GC.SerialNo, "رقم الإذن")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.ItemId).FillWeight = 110
        G.Columns(GC.ItemName).FillWeight = 300
        
        If G Is GFrom Then
            G.Columns(GC.MainItemId).Visible = False
        End If
        G.Columns(GC.ItemName).ReadOnly = True
        G.Columns(GC.UnitQty).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.TotalQty).ReadOnly = True

        G.Columns(GC.CurrentBal).ReadOnly = True

        G.Columns(GC.CurrentBal).Visible = False

        G.Columns(GC.SerialNo).Visible = False
        G.Columns(GC.UnitQty).Visible = False
        G.Columns(GC.UnitId).Visible = Md.ShowQtySub
        G.Columns(GC.TotalQty).Visible = False

        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
            btnPrint.Visibility = Windows.Visibility.Hidden
        End If

        G.Columns(GC.Line).Visible = False

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.GotFocus, AddressOf GridGotFocus
        AddHandler G.RowsAdded, AddressOf GridRowsAdded
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub


    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadGroups2", New String() {"Form"}, New String() {0})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub
     

    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            WTypes.Children.Clear()
            WItems.Children.Clear()

            TabTypes.Header = Tp & " - " & xx.Content.ToString
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadTypes2", New String() {"GroupId", "Form"}, New String() {xx.Tag.ToString, 0})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & xx.Tag.ToString & "_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub


    Sub LoadAllItems()
        Try
            HelpDt = bm.ExecuteAdapter("Select cast(Id as nvarchar(100))Id,Name,SalesPrice Price From Items  where IsStopped=0 " & ItemWhere())
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75

            

            HelpGD.SelectedIndex = 0
        Catch
        End Try

    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrice.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged, txtPrice.TextChanged
        Try
            dv.RowFilter = " [" & FirstColumn & "] like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
        Catch
        End Try
    End Sub


    Private Sub HelpGD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.PreviewKeyDown, txtName.PreviewKeyDown, txtPrice.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub HelpGD_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelpGD.MouseDoubleClick
        Try
            AddItem(HelpGD.Items(HelpGD.SelectedIndex)(0))
        Catch ex As Exception
        End Try
    End Sub



    Function ItemWhere() As String
        Dim st As String = ""
        st = " and ItemType in(0,1,2,3) "
        Return st
    End Function
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()

            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items  where IsStopped=0 " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 370)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WItems.Children.Add(x)
                AddHandler x.Click, AddressOf TabItem
            Next
        Catch
        End Try
    End Sub

    Private Sub TabItem(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        AddItem(x.Tag)
    End Sub

    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            Dim G As MyGrid = GCurrent
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                For x As Integer = 0 To G.Rows.Count - 1
                    If Not G.Rows(x).Cells(GC.ItemId).Value Is Nothing AndAlso G.Rows(x).Cells(GC.ItemId).Value.ToString = Id.ToString AndAlso Not G.Rows(x).ReadOnly Then
                        i = x
                        Exists = True
                        GoTo Br
                    End If
                Next
                i = G.Rows.Add()
                G.CurrentCell = G.Rows(i).Cells(GC.ItemName)
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select  dbo.GetStoreItemBal('" & StoreId.Text & "','" & Id & "',0,'0','" & bm.ToStrDate(DayDate.SelectedDate) & "')Bal,dbo.GetItemLastInPrice(Id,'" & bm.ToStrDate(DayDate.SelectedDate) & "')ItemLastInPrice,* From Items_View  where /*IsStopped=0 and*/ Id='" & Id & "' " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.ItemId).Value Is Nothing Or G.Rows(i).Cells(GC.ItemId).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)

                Return
            End If
            G.Rows(i).Cells(GC.ItemId).Value = dr(0)("Id")
            G.Rows(i).Cells(GC.ItemName).Value = dr(0)("Name")
            G.Rows(i).Cells(GC.CurrentBal).Value = dr(0)("Bal")
            If Val(G.Rows(i).Cells(GC.Price).Value) = 0 Then
                G.Rows(i).Cells(GC.Price).Value = dr(0)("ItemLastInPrice") 'PurchasePrice
            End If
            G_SelectionChanged(Nothing, Nothing)

            'G.Rows(i).Cells(GC.Unit).Value = dr(0)(GC.Unit)
            LoadItemUint(i)

            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)


            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Qty).Index, i))
            Try
                G.CurrentCell = G.Rows(i).Cells(GC.ItemName)
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
            Catch ex As Exception
            End Try


            LoadItemPrice(i)
            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Dim lop As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        If lop Then Return
        Try
            Dim G As MyGrid = GCurrent
            If G.Rows(i).Cells(GC.ItemId).Value Is Nothing OrElse G.Rows(i).Cells(GC.ItemId).Value.ToString = "" Then
                ClearRow(i)
                Return
            ElseIf G.Rows(i).Cells(GC.ItemId).Value Is Nothing OrElse G.Rows(i).Cells(GC.ItemId).Value.ToString = "" Then

                'If Not lop AndAlso Val(InvoiceNo.Text) = 0 AndAlso Val(G.Rows(i).Cells(GC.Qty).Value) > Val(G.Rows(i).Cells(GC.CurrentBal).Value) Then
                '    If Md.MyProject = Client.NawarGroup Then
                '        bm.ShowMSG("رصيد الصنف لا يسمح")
                '        G.Rows(i).Cells(GC.Qty).Value = 0
                '    End If
                'End If
            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)
            If Not lop AndAlso Val(InvoiceNo.Text) = 0 AndAlso Val(G.Rows(i).Cells(GC.Qty).Value) > Val(G.Rows(i).Cells(GC.CurrentBal).Value) AndAlso G Is GFrom Then
                
            End If
            G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.Qty).Value) * Val(G.Rows(i).Cells(GC.Price).Value)
            ItemBal.Text = G.CurrentRow.Cells(GC.CurrentBal).Value

            If G Is GFrom AndAlso MotionTypeId.SelectedIndex = 1 Then
                For x As Integer = 0 To GTo.Rows.Count - 1
                    If GTo.Rows(x).Cells(GC.Qty).Value = 0 Then Continue For
                    Dim MyVal As Decimal = 0
                    For y As Integer = 0 To GFrom.Rows.Count - 1
                        If GFrom.Rows(y).Cells(GC.MainItemId).Value = GTo.Rows(x).Cells(GC.ItemId).Value Then
                            MyVal += Val(GFrom.Rows(y).Cells(GC.Value).Value)
                        End If
                    Next
                    GTo.Rows(x).Cells(GC.Value).Value = MyVal
                    GTo.Rows(x).Cells(GC.Price).Value = GTo.Rows(x).Cells(GC.Value).Value / GTo.Rows(x).Cells(GC.Qty).Value
                Next
            End If
        Catch ex As Exception
            ClearRow(i)
        End Try

    End Sub

    Sub ClearRow(ByVal i As Integer)
        Dim G As MyGrid = GCurrent
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.ItemId).Value = Nothing
        G.Rows(i).Cells(GC.ItemName).Value = Nothing
        G.Rows(i).Cells(GC.UnitId).Value = Nothing
        G.Rows(i).Cells(GC.UnitQty).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Price).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.TotalQty).Value = Nothing
        G.Rows(i).Cells(GC.SerialNo).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            Dim G As MyGrid = sender
            If G.Columns(e.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value Is Nothing Then
                Dim BC As String = G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString
                If BC.Length > 12 AndAlso Val(BC.Substring(0, 1)) > 0 Then BC = BC.Substring(0, 12)
                If Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    'G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and left(Barcode,12)='" & Val(BC) & "'")
                    G.Rows(e.RowIndex).Cells(GC.ItemId).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and Barcode='" & BC & "'")
                    AddItem(G.Rows(e.RowIndex).Cells(GC.ItemId).Value, e.RowIndex, 0)
                    Exit Sub
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ItemId Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.ItemId).Value, e.RowIndex, 0)
                G_SelectionChanged(Nothing, Nothing)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.UnitId Then
                LoadItemPrice(e.RowIndex)
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try
        'G.Rows(e.RowIndex).Cells(GC.TotalQty).Value = Val(MainQty.Text) * Val(G.Rows(e.RowIndex).Cells(GC.Qty).Value)

    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        Dim str As String = " where 1=1 "
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")" & str) Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        Dim str As String = ""
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim() & str)
        ClearControls()
    End Sub


    Sub FillControls()
        If lop Then Return
        lop = True
        UndoNewId()
        bm.FillControls(Me)

        GCurrent = GFrom
        Dim dt As DataTable = bm.ExecuteAdapter("select SD.* from " & TableDetailsName & " SD where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text)
        GCurrent.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            GCurrent.Rows.Add()
            GCurrent.Rows(i).Cells(GC.MainItemId).Value = dt.Rows(i)("MainItemId").ToString
            GCurrent.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString
            GCurrent.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId").ToString
            GCurrent.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName").ToString
            LoadItemUint(i)
            GCurrent.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            GCurrent.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            GCurrent.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            GCurrent.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            GCurrent.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            GCurrent.Rows(i).Cells(GC.TotalQty).Value = dt.Rows(i)("TotalQty").ToString
            GCurrent.Rows(i).Cells(GC.SerialNo).Value = dt.Rows(i)("SerialNo").ToString
            GCurrent.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            CalcRow(i)
        Next
        GCurrent.RefreshEdit()

        GCurrent = GTo
        dt = bm.ExecuteAdapter("select SD.* from " & TableDetailsName2 & " SD where SD.StoreId=" & StoreId.Text & " and SD.InvoiceNo=" & InvoiceNo.Text)
        GCurrent.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            GCurrent.Rows.Add()
            GCurrent.Rows(i).Cells(GC.MainItemId).Value = dt.Rows(i)("MainItemId").ToString
            GCurrent.Rows(i).Cells(GC.Barcode).Value = dt.Rows(i)("Barcode").ToString
            GCurrent.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId").ToString
            GCurrent.Rows(i).Cells(GC.ItemName).Value = dt.Rows(i)("ItemName").ToString
            LoadItemUint(i)
            GCurrent.Rows(i).Cells(GC.UnitId).Value = dt.Rows(i)("UnitId")
            GCurrent.Rows(i).Cells(GC.UnitQty).Value = dt.Rows(i)("UnitQty").ToString
            GCurrent.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty").ToString
            GCurrent.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price").ToString
            GCurrent.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            GCurrent.Rows(i).Cells(GC.TotalQty).Value = dt.Rows(i)("TotalQty").ToString
            GCurrent.Rows(i).Cells(GC.SerialNo).Value = dt.Rows(i)("SerialNo").ToString
            GCurrent.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            CalcRow(i)
        Next
        GCurrent.RefreshEdit()

        Notes.Focus()
        lop = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If StoreId.Text.Trim = "" Then Return
        If Not CType(sender, Button).IsEnabled Then Return

        If DayDate.SelectedDate = Nothing Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If

        If Done.IsChecked Then
            If DayDate2.SelectedDate = Nothing Then
                bm.ShowMSG("برجاء تحديد تاريخ التنفيذ")
                DayDate2.Focus()
                Return
            End If
            If DayDate2.SelectedDate <= DayDate.SelectedDate Then
                bm.ShowMSG("برجاء تحديد تاريخ تنفيذ صحيح")
                DayDate2.Focus()
                Return
            End If
        End If

        GFrom.EndEdit()
        GTo.EndEdit()
        Try
            CalcRow(GFrom.CurrentRow.Index)
        Catch ex As Exception
        End Try
        Try
            CalcRow(GTo.CurrentRow.Index)
        Catch ex As Exception
        End Try


        Dim MyG As MyGrid = GFrom
        If MotionTypeId.SelectedIndex = 1 Then
            MyG = GFrom
        Else
            MyG = GTo
        End If
        For i As Integer = 0 To MyG.Rows.Count - 1
            If Val(MyG.Rows(i).Cells(GC.ItemId).Value) <> 0 AndAlso Val(MyG.Rows(i).Cells(GC.MainItemId).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الصنف الرئيسي بالسطر رقم " & (i + 1))
                Exit Sub
            End If
        Next

        If Not TestPrices() Then Return

        Dim ZeroExists As Boolean = False
        For i As Integer = 0 To GFrom.Rows.Count - 1
            If Val(GFrom.Rows(i).Cells(GC.ItemId).Value) <> 0 AndAlso Val(GFrom.Rows(i).Cells(GC.Value).Value) = 0 Then
                ZeroExists = True
                Exit For
            End If
        Next
        For i As Integer = 0 To GTo.Rows.Count - 1
            If Val(GTo.Rows(i).Cells(GC.ItemId).Value) <> 0 AndAlso Val(GTo.Rows(i).Cells(GC.Value).Value) = 0 Then
                ZeroExists = True
                Exit For
            End If
        Next
        If ZeroExists AndAlso Not bm.ShowDeleteMSG("يوجد أصناف بدون أسعار، هل تريد الاستمرار؟") Then Return

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName & " where " & MainId & "='" & StoreId.Text & "'")
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            lblLastEntry.Text = InvoiceNo.Text



            State = BasicMethods.SaveState.Insert
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
                lblLastEntry.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(GFrom, TableDetailsName, New String() {"StoreId", "InvoiceNo"}, New String() {StoreId.Text, InvoiceNo.Text}, New String() {"MainItemId", "Barcode", "ItemId", "ItemName", "UnitId", "UnitQty", "Qty", "Price", "Value", "TotalQty", "SerialNo"}, New String() {GC.MainItemId, GC.Barcode, GC.ItemId, GC.ItemName, GC.UnitId, GC.UnitQty, GC.Qty, GC.Price, GC.Value, GC.TotalQty, GC.SerialNo}, New VariantType() {VariantType.String, VariantType.String, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer}, New String() {GC.ItemId}, "Line", "Line") Then Return

        If Not bm.SaveGrid(GTo, TableDetailsName2, New String() {"StoreId", "InvoiceNo"}, New String() {StoreId.Text, InvoiceNo.Text}, New String() {"MainItemId", "Barcode", "ItemId", "ItemName", "UnitId", "UnitQty", "Qty", "Price", "Value", "TotalQty", "SerialNo"}, New String() {GC.MainItemId, GC.Barcode, GC.ItemId, GC.ItemName, GC.UnitId, GC.UnitQty, GC.Qty, GC.Price, GC.Value, GC.TotalQty, GC.SerialNo}, New VariantType() {VariantType.String, VariantType.String, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer}, New String() {GC.ItemId}, "Line", "Line") Then Return



        Select Case CType(sender, Button).Name
            Case btnPrint.Name
                State = BasicMethods.SaveState.Print
        End Select

        TraceInvoice(State.ToString)

        If sender Is btnPrint Then
            PrintPone(sender, 0)
            'txtID_Leave(Nothing, Nothing)
            '
            Return
        End If
        If Not DontClear Then btnNew_Click(sender, e)

    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteItemCollectionMotion", New String() {MainId, SubId, "UserDelete", "State"}, New String() {StoreId.Text, InvoiceNo.Text, Md.UserName, State})
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Dim SalesSerialNoCount As Integer = 0
    Sub ClearControls()
        Try

            NewId()
            Dim d As DateTime = Nothing
            Try
                If d.Year = 1 Then d = bm.MyGetDate
                d = DayDate.SelectedDate
            Catch ex As Exception
            End Try

            Dim st As String = StoreId.Text

            bm.ClearControls(False)

            SystemUser.SelectedValue = Md.UserName

            SalesSerialNoCount = Val(bm.ExecuteScalar("Select top 1 SalesSerialNoCount from Statics"))

            DayDate.SelectedDate = bm.MyGetDate()

            StoreId.Text = st

            GFrom.Rows.Clear()
            GTo.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")

            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'")

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'")

            bm.ExecuteNonQuery("delete from " & TableDetailsName2 & " where " & SubId & "='" & InvoiceNo.Text.Trim & "' and " & MainId & " ='" & StoreId.Text & "'")

            btnNew_Click(sender, e)
        End If
    End Sub


    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId}, New String() {StoreId.Text, InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyDown, InvoiceNo.KeyDown, txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtPrice.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            Dim G As MyGrid = GCurrent
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)

            End If
        Catch ex As Exception
        End Try
    End Sub
    Function UnitCount(dt As DataTable, i As Integer) As String
        Select Case i
            Case 1
                Return dt.Rows(0)("UnitCount")
            Case 2
                Return dt.Rows(0)("UnitCount2")
            Case Else
                Return 1
        End Select
    End Function

    Private Sub PrintPone(ByVal sender As System.Object, ByVal NewItemsOnly As Integer)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@StoreId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {StoreId.Text, InvoiceNo.Text, CType(Parent, Page).Title}
        rpt.Rpt = "ItemCollectionMotion.rpt"
        rpt.Show()

    End Sub


    Private Sub RdoGrouping_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoGrouping.Checked, RdoSearch.Checked
        Try
            If RdoGrouping.IsChecked Then
                txtID.Visibility = Visibility.Hidden
                txtName.Visibility = Visibility.Hidden
                txtPrice.Visibility = Visibility.Hidden
                HelpGD.Visibility = Visibility.Hidden
                PanelGroups.Visibility = Visibility.Visible
                PanelTypes.Visibility = Visibility.Visible
                PanelItems.Visibility = Visibility.Visible
            ElseIf RdoSearch.IsChecked Then
                txtID.Visibility = Visibility.Visible
                txtName.Visibility = Visibility.Visible
                txtPrice.Visibility = Visibility.Visible
                HelpGD.Visibility = Visibility.Visible
                PanelGroups.Visibility = Visibility.Hidden
                PanelTypes.Visibility = Visibility.Hidden
                PanelItems.Visibility = Visibility.Hidden
            End If
            
        Catch
        End Try
    End Sub


    Dim DontClear As Boolean = False


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Dim G As MyGrid = sender
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.ItemId).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.ItemName).Index Then
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.ItemId), G.CurrentRow.Cells(GC.ItemName), e, "select cast(Id as varchar(100)) Id,Name,SalesPrice 'السعر' from Items where IsStopped=0 " & ItemWhere()) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemId).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If


            If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.ItemId), G.CurrentRow.Cells(GC.ItemName), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.ItemId).Value)) Then
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemId).Index, G.CurrentCell.RowIndex))
                If G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.UnitId)
                ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                End If
            End If
        Catch ex As Exception
        End Try
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

    Private Sub LoadItemUint(i As Integer)
        Dim G As MyGrid = GCurrent
        Dim Id As Integer = Val(G.Rows(i).Cells(GC.ItemId).Value)
        'Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & Id & "' and " & ItemWhere() & "")

        bm.FillCombo("select 0 Id,Unit Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 1 Id,UnitSub Name From Items where Id='" & Id & "' " & ItemWhere() & " union select 2 Id,UnitSub2 Name From Items where Id='" & Id & "' " & ItemWhere() & "", G.Rows(i).Cells(GC.UnitId))
         

        If G.Rows(i).Cells(GC.UnitId).Value Is Nothing Then G.Rows(i).Cells(GC.UnitId).Value = 0  

    End Sub

    Private Sub LoadItemPrice(i As Integer)
        Dim G As MyGrid = GCurrent
        Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items where Id='" & G.Rows(i).Cells(GC.ItemId).Value & "' " & ItemWhere())
        If dt.Rows.Count = 0 Then Return 
        G.Rows(i).Cells(GC.UnitQty).Value = UnitCount(dt, G.Rows(i).Cells(GC.UnitId).Value) 
    End Sub

    Private Sub GridGotFocus(sender As Object, e As EventArgs)
        GCurrent = sender
    End Sub

    Private Sub GridRowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        Try
            If lop Then Return
            If CType(sender, MyGrid).Columns(GC.MainItemId).Visible Then
                If sender Is GFrom Then
                    GFrom.Rows(GFrom.CurrentRow.Index).Cells(GC.MainItemId).Value = GTo.Rows(GTo.CurrentRow.Index).Cells(GC.ItemId).Value
                    'GFrom.RefreshEdit()
                ElseIf sender Is GTo Then
                    GTo.Rows(GTo.CurrentRow.Index).Cells(GC.MainItemId).Value = GFrom.Rows(GFrom.CurrentRow.Index).Cells(GC.ItemId).Value
                    'GTo.RefreshEdit()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MotionTypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles MotionTypeId.SelectionChanged
        If MotionTypeId.SelectedIndex = 1 Then
            GFrom.Columns(GC.MainItemId).Visible = True
            GTo.Columns(GC.MainItemId).Visible = False
        Else
            GFrom.Columns(GC.MainItemId).Visible = False
            GTo.Columns(GC.MainItemId).Visible = True
        End If
    End Sub


    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        Try
            ItemBal.Text = GFrom.CurrentRow.Cells(GC.CurrentBal).Value
        Catch ex As Exception
            ItemBal.Clear()
        End Try
    End Sub

    Private Function TestPrices() As Boolean
        Dim G1, G2 As MyGrid
        If MotionTypeId.SelectedIndex = 0 Then
            G1 = GFrom
            G2 = GTo
        Else
            G1 = GTo
            G2 = GFrom
        End If

        For i As Integer = 0 To G1.Rows.Count - 1
            Dim Value As Decimal = 0
            If Val(G1.Rows(i).Cells(GC.ItemId).Value) = 0 Then Continue For
            'If Val(G1.Rows(i).Cells(GC.Value).Value) = 0 Then
            '    bm.ShowMSG("برجاء تحديد سعر الصنف رقم " & G1.Rows(i).Cells(GC.ItemId).Value)
            '    G1.Focus()
            '    G1.BeginEdit(True)
            '    G1.CurrentCell = G1.Rows(i).Cells(GC.Price)
            '    Return False
            'End If
            For x As Integer = 0 To G2.Rows.Count - 1
                If Val(G1.Rows(i).Cells(GC.ItemId).Value) <> Val(G2.Rows(x).Cells(GC.MainItemId).Value) OrElse Val(G2.Rows(x).Cells(GC.ItemId).Value) = 0 Then Continue For
                Value += Val(G2.Rows(x).Cells(GC.Value).Value)
            Next
            If Math.Round(Value, 6, MidpointRounding.AwayFromZero) <> Math.Round(Val(G1.Rows(i).Cells(GC.Value).Value), 6, MidpointRounding.AwayFromZero) Then
                bm.ShowMSG("برجاء مراجعة أسعار مكونات الصنف رقم " & G1.Rows(i).Cells(GC.ItemId).Value)
                G1.Focus()
                G1.BeginEdit(True)
                G1.CurrentCell = G1.Rows(i).Cells(GC.ItemId)
                Return False
            End If
        Next

        Return True
    End Function

    Private Sub Done_Checked(sender As Object, e As RoutedEventArgs) Handles Done.Checked, Done.Unchecked
        If Done.IsChecked Then
            DayDate2.Visibility = Windows.Visibility.Visible
            If DayDate2.SelectedDate Is Nothing Then DayDate2.SelectedDate = bm.MyGetDate.Date
        Else
            DayDate2.Visibility = Windows.Visibility.Hidden
        End If
    End Sub
End Class
