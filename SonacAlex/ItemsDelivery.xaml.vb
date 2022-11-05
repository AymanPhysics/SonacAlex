Imports System.Data

Public Class ItemsDelivery

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Dim dt2 As New DataTable
    Dim dt3 As New DataTable
    Public Flag As Integer = 0
    Dim dv As New DataView
    Dim dv2 As New DataView
    Dim dv3 As New DataView

    Dim MyTextBoxes2() As TextBox = {}
    Dim MyTextBoxes3() As TextBox = {}

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.Addcontrol_MouseDoubleClick({StoreId, SaveId, BankId, WaiterId})

        btnPrint.Visibility = Windows.Visibility.Collapsed

        StoreId.Text = Md.DefaultStore
        'StoreId.IsEnabled = Md.Manager
        StoreId_LostFocus(Nothing, Nothing)

        SaveId.Text = Md.DefaultSave
        BankId.Text = Md.DefaultBank
        'SaveId.IsEnabled = Md.Manager
        SaveId_LostFocus(Nothing, Nothing)
        BankId.IsEnabled = Md.Manager
        BankId_LostFocus(Nothing, Nothing)


    End Sub

    Dim lop As Boolean = False

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyDown, SaveId.KeyDown, WaiterId.KeyDown, BankId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub LoadG2()
        Try
            dt2 = bm.ExecuteAdapter("GetDeliveryCustomers", {"StoreId", "SalesDeliveryHistoryInvoiceNo"}, {Val(StoreId.Text), Val(SalesDeliveryHistoryInvoiceNo.Text)})
            dt2.TableName = "tbl"

            dv2.Table = dt2
            DataGridView2.ItemsSource = dv2
            'DataGridView2.IsReadOnly = True
            For index = 0 To DataGridView2.Columns.Count - 1
                DataGridView2.Columns(index).IsReadOnly = True
            Next
            DataGridView2.Columns(dt2.Columns("IsPayed").Ordinal).Visibility = Windows.Visibility.Collapsed

            DataGridView2.Columns(dt2.Columns("Payed").Ordinal).IsReadOnly = False
            DataGridView2.Columns(dt2.Columns("PayedVisa").Ordinal).IsReadOnly = False
            DataGridView2.Columns(dt2.Columns("IsSelected").Ordinal).IsReadOnly = False

            DataGridView2.Columns(dt2.Columns("ToId").Ordinal).Header = "كود العميل"
            DataGridView2.Columns(dt2.Columns("ToName").Ordinal).Header = "اسم العميل"
            DataGridView2.Columns(dt2.Columns("Tel").Ordinal).Header = "التليفون"
            DataGridView2.Columns(dt2.Columns("Address").Ordinal).Header = "العنوان"
            DataGridView2.Columns(dt2.Columns("TotalValue").Ordinal).Header = "القيمة"
            DataGridView2.Columns(dt2.Columns("Bal0").Ordinal).Header = "الرصيد"
            DataGridView2.Columns(dt2.Columns("Payed").Ordinal).Header = "سداد نقدي"
            DataGridView2.Columns(dt2.Columns("PayedVisa").Ordinal).Header = "سداد فيزا"
            DataGridView2.Columns(dt2.Columns("InvoiceNo").Ordinal).Header = "الفاتورة"
            DataGridView2.Columns(dt2.Columns("IsSelected").Ordinal).Header = "تحديد"

        Catch
        End Try

        CalcGrid2()
        SearchGrid2()

        AddHandler DataGridView2.CellEditEnding, AddressOf DataGridView2_CellEditEnding
    End Sub


    Private Sub LoadG3()
        Try
            dt3 = bm.ExecuteAdapter("GetDeliveryCustomers2", {"StoreId", "SalesDeliveryHistoryInvoiceNo"}, {Val(StoreId.Text), Val(SalesDeliveryHistoryInvoiceNo.Text)})
            dt3.TableName = "tbl"

            dv3.Table = dt3
            DataGridView3.ItemsSource = dv3
            'DataGridView3.IsReadOnly = True
            For index = 0 To DataGridView3.Columns.Count - 1
                DataGridView3.Columns(index).IsReadOnly = True
            Next
            DataGridView3.Columns(dt3.Columns("IsDelivered").Ordinal).IsReadOnly = False
            DataGridView3.Columns(dt3.Columns("IsPayed").Ordinal).Visibility = Windows.Visibility.Collapsed

            
            DataGridView3.Columns(dt3.Columns("StoreName").Ordinal).Header = "الفرع"
            DataGridView3.Columns(dt3.Columns("SalesDeliveryHistoryInvoiceNo").Ordinal).Header = "الكشف"
            DataGridView3.Columns(dt3.Columns("InvoiceNo").Ordinal).Header = "الفاتورة"
            DataGridView3.Columns(dt3.Columns("DayDate").Ordinal).Header = "التاريخ"
            DataGridView3.Columns(dt3.Columns("SalesManName").Ordinal).Header = "المندوب"
            DataGridView3.Columns(dt3.Columns("ToId").Ordinal).Header = "كود العميل"
            DataGridView3.Columns(dt3.Columns("ToName").Ordinal).Header = "اسم العميل"
            DataGridView3.Columns(dt3.Columns("SalesTypeName").Ordinal).Header = "النوع"
            DataGridView3.Columns(dt3.Columns("ItemId").Ordinal).Header = "كود الصنف"
            DataGridView3.Columns(dt3.Columns("ItemName").Ordinal).Header = "اسم الصنف"
            DataGridView3.Columns(dt3.Columns("Qty").Ordinal).Header = "الكمية"
            DataGridView3.Columns(dt3.Columns("Price").Ordinal).Header = "السعر"
            DataGridView3.Columns(dt3.Columns("TotalValue").Ordinal).Header = "القيمة"
            DataGridView3.Columns(dt3.Columns("IsDelivered").Ordinal).Header = "تحديد"

            DataGridView3.Columns(dt3.Columns("StoreId").Ordinal).Visibility = Windows.Visibility.Collapsed
            DataGridView3.Columns(dt3.Columns("SalesManId").Ordinal).Visibility = Windows.Visibility.Collapsed
            DataGridView3.Columns(dt3.Columns("Line").Ordinal).Visibility = Windows.Visibility.Collapsed
            DataGridView3.Columns(dt3.Columns("Bal0").Ordinal).Visibility = Windows.Visibility.Collapsed
            DataGridView3.Columns(dt3.Columns("D_IsDelivered").Ordinal).Visibility = Windows.Visibility.Collapsed

        Catch
        End Try
        AddHandler DataGridView3.PreparingCellForEdit, AddressOf DataGridView3_PreparingCellForEdit
        AddHandler DataGridView3.BeginningEdit, AddressOf DataGridView3_BeginningEdit


        btnSave1.Visibility = Windows.Visibility.Visible
        If Val(SalesDeliveryHistoryInvoiceNo.Text) > 0 Then
            For index = 0 To DataGridView2.Items.Count - 1
                If DataGridView2.Items(index)("IsPayed") = 1 Then
                    btnSave1.Visibility = Windows.Visibility.Hidden
                    Exit For
                End If
            Next
            For index = 0 To DataGridView3.Items.Count - 1
                If DataGridView3.Items(index)("IsPayed") = 1 Then
                    btnSave1.Visibility = Windows.Visibility.Hidden
                    Exit For
                End If
            Next

        End If

        CalcGrid3()
        SearchGrid3()
    End Sub

    Private Sub btnPay_Click(sender As Object, e As RoutedEventArgs) Handles btnPay.Click
        For index = 0 To DataGridView2.Items.Count - 1
            DataGridView2.Items(index)("Payed") = DataGridView2.Items(index)("Bal0")
            DataGridView2.Items(index)("PayedVisa") = 0
        Next
    End Sub

    Private Sub btnIsDelivered_Click(sender As Object, e As RoutedEventArgs) Handles btnIsDelivered.Click
        If btnIsDelivered.Tag Is Nothing Then btnIsDelivered.Tag = False
        btnIsDelivered.Tag = Not btnIsDelivered.Tag
        For index = 0 To DataGridView3.Items.Count - 1
            DataGridView3.Items(index)("IsDelivered") = btnIsDelivered.Tag
        Next
    End Sub

    Private Sub btnSave1_Click(sender As Object, e As RoutedEventArgs) Handles btnSave1.Click
        If Val(StoreId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد مخزن")
            StoreId.Focus()
            Return
        End If
        If Val(SaveId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد خزنة")
            SaveId.Focus()
            Return
        End If
        If Not bm.ShowDeleteMSG("هل أنت متأكد من السداد؟") Then Return
        
        Dim MyInvoiceNo As Integer = Val(bm.ExecuteScalar("select isnull(max(InvoiceNo),0)+1 from BankCash_G where BankId=" & Val(SaveId.Text) & " and Flag=1"))
        Dim MyInvoiceNoVisa As Integer = Val(bm.ExecuteScalar("select isnull(max(InvoiceNo),0)+1 from BankCash_G where BankId=" & Val(BankId.Text) & " and Flag=3"))
        If MyInvoiceNo = 0 OrElse MyInvoiceNoVisa = 0 Then
            bm.ShowMSG("برجاء إعادة المحاولة")
            Return
        End If

        Dim MyDate As String = bm.ToStrDate(bm.MyGetDate)

        Dim str As String = ""
        For index = 0 To DataGridView2.Items.Count - 1
            If DataGridView2.Items(index)("Payed") > 0 Then
                str &= " INSERT INTO BankCash_G(InvoiceNo,Flag,DayDate,BankId,CostTypeId,LinkFile,SubAccNo,Value,Canceled,Notes,UserName,MyGetDate,CostCenterId,CurrencyId,DocNo,PurchaseAccNo,ImportMessageId,StoreId,StoreInvoiceNo)VALUES(" & MyInvoiceNo & ",1,'" & MyDate & "'," & Val(SaveId.Text) & ",0,1," & DataGridView2.Items(index)("ToId") & "," & DataGridView2.Items(index)("Payed") & ",0,'سداد أوتوماتيكي - " & StoreName.Text & " - مسلسل " & SalesDeliveryHistoryInvoiceNo.Text & "'," & Md.UserName & ",GetDate(),0,1,'','',0,0,0)"
                str &= " update SalesDeliveryHistory set Payed='" & DataGridView2.Items(index)("Payed") & "' where StoreId='" & Val(StoreId.Text) & "' and InvoiceNo='" & Val(SalesDeliveryHistoryInvoiceNo.Text) & "' and CustomerId='" & DataGridView2.Items(index)("ToId") & "'"
            End If
            If DataGridView2.Items(index)("PayedVisa") > 0 Then
                str &= " INSERT INTO BankCash_G(InvoiceNo,Flag,DayDate,BankId,CostTypeId,LinkFile,SubAccNo,Value,Canceled,Notes,UserName,MyGetDate,CostCenterId,CurrencyId,DocNo,PurchaseAccNo,ImportMessageId,StoreId,StoreInvoiceNo)VALUES(" & MyInvoiceNoVisa & ",3,'" & MyDate & "'," & Val(BankId.Text) & ",0,1," & DataGridView2.Items(index)("ToId") & "," & DataGridView2.Items(index)("PayedVisa") & ",0,'سداد أوتوماتيكي - " & StoreName.Text & " - مسلسل " & SalesDeliveryHistoryInvoiceNo.Text & "'," & Md.UserName & ",GetDate(),0,1,'','',0,0,0)"
                str &= " update SalesDeliveryHistory set PayedVisa='" & DataGridView2.Items(index)("PayedVisa") & "' where StoreId='" & Val(StoreId.Text) & "' and InvoiceNo='" & Val(SalesDeliveryHistoryInvoiceNo.Text) & "' and CustomerId='" & DataGridView2.Items(index)("ToId") & "'"

                If Val(BankId.Text) = 0 Then
                    bm.ShowMSG("برجاء تحديد بنك الفيزا")
                    Return
                End If
            End If

            str &= " exec updateCustomersTempBal0 " & DataGridView2.Items(index)("ToId") & " "
        Next

        str &= " update SalesDeliveryHistory set IsPayed=1 where StoreId='" & Val(StoreId.Text) & "' and InvoiceNo='" & Val(SalesDeliveryHistoryInvoiceNo.Text) & "'"
        bm.ExecuteNonQuery(str)
        SalesDeliveryHistoryInvoiceNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub btnSave2_Click(sender As Object, e As RoutedEventArgs) Handles btnSave2.Click
        If Not bm.ShowDeleteMSG("هل أنت متأكد من التسليم للعميل؟") Then Return
        Dim Str As String = ""
        For index = 0 To DataGridView3.Items.Count - 1
            If DataGridView3.Items(index)("IsDelivered") Then
                Str &= " update SalesDetails set IsDelivered=1 where Line=" & DataGridView3.Items(index)("Line")
            End If
        Next
        bm.ExecuteNonQuery(Str)

        SalesDeliveryHistoryInvoiceNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub SaveId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(5," & Md.UserName & ")") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Fn_EmpPermissions(5," & Md.UserName & ") where Id=" & SaveId.Text.Trim(), True)
    End Sub

    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        bm.ShowHelp("Banks", BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Banks", "Banks")
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        bm.LostFocus(BankId, BankName, "select Name from Banks where Id=" & BankId.Text.Trim())
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
        'If sender Is Nothing Then Return

        SalesDeliveryHistoryInvoiceNo_LostFocus(Nothing, Nothing)
    End Sub

    Sub SearchGrid2()
        Try
            If SC2.Children.Count > 0 Then Return
            SC2.Children.Clear()
            For i As Integer = 0 To dt2.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes2(MyTextBoxes2.Length + 1)
                MyTextBoxes2(i) = txt
                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DataGridView2.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)

                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = Windows.HorizontalAlignment.Left
                txt.VerticalAlignment = Windows.VerticalAlignment.Top
                txt.Visibility = DataGridView2.Columns(i).Visibility
                SC2.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter2
                AddHandler txt.TextChanged, AddressOf txt_TextChanged2
            Next
            DataGridView2.SelectedIndex = 0
        Catch
        End Try
    End Sub

    Sub SearchGrid3()
        Try
            If SC3.Children.Count > 0 Then Return
            SC3.Children.Clear()
            For i As Integer = 0 To dt3.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes3(MyTextBoxes3.Length + 1)
                MyTextBoxes3(i) = txt
                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DataGridView3.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)

                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = Windows.HorizontalAlignment.Left
                txt.VerticalAlignment = Windows.VerticalAlignment.Top
                txt.Visibility = DataGridView3.Columns(i).Visibility
                SC3.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter3
                AddHandler txt.TextChanged, AddressOf txt_TextChanged3
            Next
            DataGridView3.SelectedIndex = 0
        Catch
        End Try
    End Sub


    Private Sub txt_Enter2(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv2.Sort = DataGridView2.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged2(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dv2.RowFilter = " 1=1"
        For i As Integer = 0 To dt2.Columns.Count - 1
            Try
                dv2.RowFilter &= " and [" & dt2.Columns(i).ColumnName & "] like '%" & MyTextBoxes2(i).Text & "%' "
            Catch
            End Try
        Next
        CalcGrid2()
    End Sub

    Private Sub txt_Enter3(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv3.Sort = DataGridView3.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged3(ByVal sender As System.Object, ByVal e As System.EventArgs)
        dv3.RowFilter = " 1=1"
        For i As Integer = 0 To dt3.Columns.Count - 1
            Try
                dv3.RowFilter &= " and [" & dt3.Columns(i).ColumnName & "] like '%" & MyTextBoxes3(i).Text & "%' "
            Catch
            End Try
        Next
        CalcGrid3()
    End Sub

    Private Sub WaiterId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles WaiterId.KeyUp
        bm.ShowHelp("المندوبين", WaiterId, WaiterName, e, "select cast(Id as varchar(100)) Id," & Resources.Item("CboName") & " Name from Employees where Waiter=1 and Stopped=0")
    End Sub

    Private Sub WaiterId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles WaiterId.LostFocus
        bm.LostFocus(WaiterId, WaiterName, "select " & Resources.Item("CboName") & " Name from Employees where Waiter=1 and Id=" & WaiterId.Text.Trim() & " and Stopped=0")
    End Sub

    Private Sub btnSave3_Click(sender As Object, e As RoutedEventArgs) Handles btnSave3.Click
        If Val(WaiterId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المندوب")
            Return
        End If
        If Not bm.ShowDeleteMSG("هل أنت متأكد من التسليم للمندوب؟") Then Return

        Dim str As String = ""
        Dim NewInvoiceNo As Integer = Val(bm.ExecuteScalar("select isnull(MAX(cast(InvoiceNo as bigint)),0)+1 from SalesDeliveryHistory where StoreId=" & Val(StoreId.Text)))
        If NewInvoiceNo = 0 Then
            bm.ShowMSG("برجاء إعادة المحاولة")
            Return
        End If

        For index = 0 To DataGridView3.Items.Count - 1
            If DataGridView3.Items(index)("IsDelivered") Then
                str &= vbCrLf & " update SalesDetails set SalesManId=" & Val(WaiterId.Text) & " where Line=" & DataGridView3.Items(index)("Line")
                str &= vbCrLf & " insert SalesDeliveryHistory(StoreId,InvoiceNo,DayDate,CustomerId,Bal0,SalesDetailsInvoiceNo,SalesDetailsLine,UserName,SalesManId) select " & DataGridView3.Items(index)("StoreId") & "," & NewInvoiceNo & ",cast(GetDate() as date)," & DataGridView3.Items(index)("ToId") & "," & DataGridView3.Items(index)("Bal0") & ",'" & DataGridView3.Items(index)("InvoiceNo") & "'," & DataGridView3.Items(index)("Line") & "," & Md.UserName & "," & Val(WaiterId.Text) & ""
            End If
        Next


        For index = 0 To DataGridView2.Items.Count - 1
            If DataGridView2.Items(index)("IsSelected") Then
                str &= vbCrLf & " if not exists (select StoreId from SalesDeliveryHistory where StoreId='" & Val(StoreId.Text) & "' and InvoiceNo='" & NewInvoiceNo & "' and CustomerId='" & DataGridView2.Items(index)("ToId") & "') insert SalesDeliveryHistory(StoreId,InvoiceNo,DayDate,CustomerId,Bal0,SalesDetailsInvoiceNo,SalesDetailsLine,UserName,SalesManId) select '" & Val(StoreId.Text) & "'," & NewInvoiceNo & ",cast(GetDate() as date)," & DataGridView2.Items(index)("ToId") & "," & DataGridView2.Items(index)("Bal0") & ",'" & DataGridView2.Items(index)("InvoiceNo") & "',0," & Md.UserName & "," & Val(WaiterId.Text) & ""
            End If
        Next

        bm.ExecuteNonQuery(str)
        WaiterId.Clear()
        WaiterId_LostFocus(Nothing, Nothing)
        SalesDeliveryHistoryInvoiceNo.Text = NewInvoiceNo
        SalesDeliveryHistoryInvoiceNo_LostFocus(Nothing, Nothing)
        btnPrint_Click(Nothing, Nothing)
    End Sub

    Private Sub CalcGrid2()
        Total1.Text = "0"
        Total11.Text = "0"
        For index = 0 To DataGridView2.Items.Count - 1
            If Val(DataGridView2.Items(index)("Bal0")) > 0 Then
                Total1.Text += Val(DataGridView2.Items(index)("Bal0"))
            Else
                Total11.Text += Val(DataGridView2.Items(index)("Bal0"))
            End If
        Next
    End Sub

    Private Sub CalcGrid3()
        Total2.Text = "0"
        Total3.Text = "0"
        For index = 0 To DataGridView3.Items.Count - 1
            Total2.Text += Val(DataGridView3.Items(index)("Qty"))
            Total3.Text += Val(DataGridView3.Items(index)("TotalValue"))

            DataGridView3.BeginEdit()
        Next
    End Sub

    Private Sub SalesDeliveryHistoryInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles SalesDeliveryHistoryInvoiceNo.LostFocus
        LoadG2()
        LoadG3()
        If Val(SalesDeliveryHistoryInvoiceNo.Text) = 0 Then
            btnSave3.Visibility = Windows.Visibility.Visible
            btnPrint.Visibility = Windows.Visibility.Hidden
            btnSelectAll.Visibility = Windows.Visibility.Hidden
            btnIsDelivered.Visibility = Windows.Visibility.Hidden
            WaiterId.IsEnabled = True
        Else
            btnSave3.Visibility = Windows.Visibility.Hidden
            btnPrint.Visibility = Windows.Visibility.Visible
            btnSelectAll.Visibility = Windows.Visibility.Visible
            btnIsDelivered.Visibility = Windows.Visibility.Visible
            WaiterId.IsEnabled = False
            If DataGridView3.Items.Count > 0 Then
                WaiterId.Text = DataGridView3.Items(0)("SalesManId")
                WaiterId_LostFocus(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@StoreId", "@SalesDeliveryHistoryInvoiceNo", "Header", "StoreName", "WaiterName"}
        rpt.paravalue = New String() {Val(StoreId.Text), Val(SalesDeliveryHistoryInvoiceNo.Text), CType(Parent, Page).Title, StoreName.Text, WaiterName.Text}
        rpt.Rpt = "DeliveryCustomers.rpt"
        rpt.Show()
    End Sub

    Private Sub DataGridView3_PreparingCellForEdit(sender As Object, e As DataGridPreparingCellForEditEventArgs)
        'e.EditingElement.IsEnabled = Not DataGridView3.Items(e.Row.GetIndex)("D_IsDelivered")

        If DataGridView3.Items(e.Row.GetIndex)("D_IsDelivered") Then
            'e.EditingElement.IsEnabled = False
            DataGridView3.CancelEdit()
        End If
    End Sub

    Private Sub DataGridView3_BeginningEdit(sender As Object, e As DataGridBeginningEditEventArgs)
        'If DataGridView3.Items(e.Row.GetIndex)("D_IsDelivered") Then e.Cancel = True
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As RoutedEventArgs) Handles btnRefresh.Click
        btnRefresh.IsEnabled = False
        SalesDeliveryHistoryInvoiceNo.Clear()
        SalesDeliveryHistoryInvoiceNo_LostFocus(Nothing, Nothing)
        btnRefresh.IsEnabled = True
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectAll.Click
        If btnSelectAll.Tag Is Nothing Then btnSelectAll.Tag = False
        btnSelectAll.Tag = Not btnSelectAll.Tag
        For index = 0 To DataGridView2.Items.Count - 1
            DataGridView2.Items(index)("IsSelected") = btnSelectAll.Tag
            For i = 0 To DataGridView3.Items.Count - 1
                DataGridView3.Items(i)("IsDelivered") = btnSelectAll.Tag
            Next
        Next
    End Sub

    Dim Lop2 As Boolean = False
    Private Sub DataGridView2_CellEditEnding(sender As Object, e As DataGridCellEditEndingEventArgs)
        If Lop2 OrElse e.Column.DisplayIndex <> dt2.Columns("IsSelected").Ordinal Then Return

        Lop2 = True
        DataGridView2.CommitEdit()

        Dim b As Boolean = DataGridView2.Items(e.Row.GetIndex)("IsSelected")
        For index = 0 To DataGridView3.Items.Count - 1
            If DataGridView2.Items(e.Row.GetIndex)("ToId") = DataGridView3.Items(index)("ToId") Then
                DataGridView3.Items(index)("IsDelivered") = b
            End If
        Next

        Lop2 = False
    End Sub


End Class