Imports System.Data
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Threading

Public Class Cashier

    Dim dt As New DataTable
    Dim dv As New DataView
    Dim bm As New BasicMethods
    Dim MyTextBoxes() As TextBox = {}
    Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 5)}

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.Addcontrol_MouseDoubleClick({SaveId, StoreId})
        DayDate.SelectedDate = bm.MyGetDate()
        SaveId.Text = Md.DefaultSave
        SaveId_LostFocus(Nothing, Nothing)
        StoreId.Text = Md.DefaultStore
        StoreId_LostFocus(Nothing, Nothing)
        AddHandler t.Tick, AddressOf FillGrid
        t_Tick()
    End Sub


    Private Sub FillGrid()
        Try
            If Val(StoreId.Text) = 0 Then Return
            If DayDate.SelectedDate Is Nothing Then Return
            dt = bm.ExecuteAdapter("select distinct cast(M.InvoiceNo as nvarchar(100))'رقم الفاتورة',cast(M.ToId as nvarchar(100))'كود العميل',dbo.GetCustomerName(M.ToId)'اسم العميل',dbo.GetEmpName(M.Cashier)'البائع',cast(M.TotalAfterDiscount as nvarchar(100))'قيمة الفاتورة' from SalesMaster M left join SalesDeliveryHistory H on(M.StoreId=H.StoreId and cast(M.InvoiceNo as nvarchar(100))=H.SalesDetailsInvoiceNo) where M.StoreId=" & Val(StoreId.Text) & " and M.Flag=13 and M.DayDate='" & bm.ToStrDate(DayDate.SelectedDate) & "' and M.PaymentType=4 and H.SalesDetailsInvoiceNo is null")
            dv.Table = dt
            DG.ItemsSource = dv

            DG.SelectionUnit = DataGridSelectionUnit.FullRow
            DG.IsReadOnly = True
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try

        Dim x As Decimal = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            x += Val(dt.Rows(i)(4))
        Next
        Total.Text = x

        txt_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.Sort = DG.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.RowFilter = " 1=1"
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).ColumnName = "IsSelected" Then Continue For
                dv.RowFilter &= " and [" & dt.Columns(i).ColumnName & "] like '%" & MyTextBoxes(i).Text & "%' "
            Next
        Catch
        End Try
    End Sub

    Private Sub t_Tick()
        Try
            SC.Children.Clear()
            For i As Integer = 0 To dt.Columns.Count - 1
                Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 10)}
                ReDim Preserve MyTextBoxes(MyTextBoxes.Length + 1)
                MyTextBoxes(i) = txt
                Dim binding As New Binding("ActualWidth")
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                binding.Source = DG.Columns(i)
                txt.SetBinding(TextBox.WidthProperty, binding)
                txt.Tag = i
                txt.TabIndex = i
                txt.HorizontalAlignment = Windows.HorizontalAlignment.Left
                txt.VerticalAlignment = Windows.VerticalAlignment.Top
                SC.Children.Add(txt)
                AddHandler txt.GotFocus, AddressOf txt_Enter
                AddHandler txt.TextChanged, AddressOf txt_TextChanged
            Next
            DG.SelectedIndex = 0
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub




    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        Dim str As String = " where 1=1 "
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")" & str) Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Public Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        Dim str As String = ""
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim() & str)

        FillGrid()
    End Sub

    Private Sub DG_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles DG.MouseDoubleClick
        t.Stop()
        If DG.SelectedIndex < 0 Then
            t.Start()
            Return
        End If
        Dim Value As String = DG.SelectedItems(0)(4)
        If bm.ShowMSGNo("هل أنت متأكد من سداد الفاتورة رقم " & DG.SelectedItems(0)(0) & " بمبلغ " & Value, Value) Then
            If Value = DG.SelectedItems(0)(4) Then
                If bm.ExecuteNonQuery("update SalesMaster set PaymentType=1,SaveId='" & Val(SaveId.Text) & "' where StoreId='" & Val(StoreId.Text) & "' and Flag=13 and InvoiceNo='" & DG.SelectedItems(0)(0) & "'") Then
                    PrintPone(DG.SelectedItems(0)(0), Value)
                    'bm.ShowMSG("تمت العملية بنجاح")
                    FillGrid()
                End If
            Else
                If bm.ExecuteNonQuery("update SalesMaster set PaymentType=5,SaveId='" & Val(SaveId.Text) & "',CashValue='" & Value & "' where StoreId='" & Val(StoreId.Text) & "' and Flag=13 and InvoiceNo='" & DG.SelectedItems(0)(0) & "'") Then
                    PrintPone(DG.SelectedItems(0)(0), Value)
                    'bm.ShowMSG("تمت العملية بنجاح")
                    FillGrid()
                End If
            End If
        End If
        t.Start()
    End Sub

    Private Sub PrintPone(InvoiceNo As Integer, Payed As String)
        Dim rpt As New ReportViewer
        rpt.Rpt = "SalesPone2.rpt"
        rpt.paraname = New String() {"@FromDate", "@ToDate", "@Shift", "@Flag", "@StoreId", "@FromInvoiceNo", "@ToInvoiceNo", "@InvoiceNo", "@NewItemsOnly", "@RPTFlag1", "@RPTFlag2", "@PrintingGroupId", "Header", "Remaining", "Payed"}
        rpt.paravalue = New String() {DayDate.SelectedDate, DayDate.SelectedDate, 0, 13, StoreId.Text, InvoiceNo, InvoiceNo, InvoiceNo, 0, 0, 0, 0, CType(Parent, Page).Title, 0, Val(Payed)}
        rpt.Print(".", Md.PonePrinter, 1)
    End Sub


    Private Sub SaveId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.LostFocus
        bm.LostFocus(SaveId, SaveName, "select Name from Fn_EmpPermissions(5," & Md.UserName & ") where Id=" & SaveId.Text.Trim(), True)
    End Sub

    Private Sub SaveId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SaveId.KeyUp
        If bm.ShowHelp("Saves", SaveId, SaveName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(5," & Md.UserName & ")") Then
            SaveId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DayDate.SelectedDateChanged
        FillGrid()
    End Sub
End Class
