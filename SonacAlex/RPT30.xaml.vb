Imports System.Data

Public Class RPT30
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If StoreId.Visibility = Windows.Visibility.Visible AndAlso Val(StoreId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblStoreId.Content)
            StoreId.Focus()
            Return
        End If
        If StoreInvoiceNo.Visibility = Windows.Visibility.Visible AndAlso Val(StoreInvoiceNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblStoreInvoiceNo.Content)
            StoreInvoiceNo.Focus()
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@StoreId", "StoreName", "@InvoiceNo", "Header", "Detailed", "@Id", "@SerialNo"}
        rpt.paravalue = New String() {Val(StoreId.Text), StoreName.Text, Val(StoreInvoiceNo.Text), CType(Parent, Page).Title, IIf(Detailed.IsChecked, 1, 0), 3, Val(StoreInvoiceNo.Text)}

        Select Case Flag
            Case 1
                rpt.Rpt = "ImportInvoiceCost.rpt"
            Case 2
                rpt.Rpt = "ImportMessageCostHistoryVal3.rpt"
            Case 3
                rpt.Rpt = "SalesPone32_N.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({StoreId, StoreInvoiceNo})
        LoadResource()
        If Flag = 2 OrElse Flag = 3 Then Detailed.Visibility = Windows.Visibility.Hidden
        If Flag = 3 Then lblStoreInvoiceNo.Content = "رقم إذن الصرف"
    End Sub

    Private Sub LoadResource()
        Detailed.SetResourceReference(CheckBox.ContentProperty, "Detailed")
        Button2.SetResourceReference(Button.ContentProperty, "View Report")

    End Sub


    Private Sub StoreId_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
        End If
    End Sub

    Private Sub StoreId_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text)
    End Sub

    Private Sub StoreInvoiceNo_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreInvoiceNo.KeyUp
        If Flag = 3 Then Return
        If bm.ShowHelpMultiColumns("الفواتير", StoreInvoiceNo, StoreInvoiceNo, e, "select cast(InvoiceNo as varchar(100)) 'الفاتورة',dbo.GetSupplierName(ToId) 'المورد',DocNo 'رقم عقد المورد',cast(TotalAfterDiscount as nvarchar(100)) 'إجمالي الفاتورة',cast(OrderTypeId as nvarchar(100)) 'مسلسل الطلبية',dbo.GetOrderTypes(OrderTypeId) 'اسم الطلبية' from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد) Then
        End If
    End Sub

    Private Sub StoreInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreInvoiceNo.LostFocus
        If Val(StoreInvoiceNo.Text) = 0 OrElse Flag = 3 Then Return
        If Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & StoreInvoiceNo.Text) Then
            bm.ShowMSG("هذا الرقم غير صحيح")
            StoreInvoiceNo.Clear()
        End If
    End Sub

End Class