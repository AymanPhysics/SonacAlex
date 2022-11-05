Imports System.Data

Public Class RPT32
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Dim IsCalculated = False
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Flag <> 6 AndAlso Flag <> 7 Then
            If OrderTypeId.Visibility = Windows.Visibility.Visible AndAlso Val(OrderTypeId.Text) = 0 Then
                bm.ShowMSG("برجاء تحديد " & lblOrderTypeId.Content)
                OrderTypeId.Focus()
                Return
            End If
            If ImportMessageId.Visibility = Windows.Visibility.Visible AndAlso Val(ImportMessageId.Text) = 0 Then
                bm.ShowMSG("برجاء تحديد " & lblImportMessageId.Content)
                ImportMessageId.Focus()
                Return
            End If
        End If


        If Not IsCalculated AndAlso BtnCalc.Visibility = Windows.Visibility.Visible AndAlso bm.ShowDeleteMSG("هل تريد احتساب تكلفة الأصناف؟") Then
            BtnCalc_Click(BtnCalc, Nothing)
            IsCalculated = True
            Return
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromDate", "@ToDate", "Header", "@OrderTypeId", "@ImportMessageId", "@Id", "@StoreId", "@InvoiceNo", "@ItemId", "ItemName", "OrderTypeName", "ImportMessageName", "StoreName", "@SerialNo"}
        rpt.paravalue = New String() {FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title, Val(OrderTypeId.Text), Val(ImportMessageId.Text), Val(ImportMessageId.Text), Val(StoreId.Text), Val(StoreInvoiceNo.Text), Val(ItemId.Text), ItemName.Text, OrderTypeName.Text, ImportMessageName.Text, StoreName.Text, 0}

        Select Case Flag
            Case 1
                rpt.Rpt = "ImportMessagesAll.rpt"
            Case 2
                rpt.Rpt = "ImportMessagesAll2.rpt"
            Case 3
                rpt.Rpt = "ImportMessages2.rpt"
            Case 4
                rpt.Rpt = "ImportMessages2Images.rpt"
            Case 5
                'rpt.Rpt = "ImportMessages23.rpt"
                'rpt.Rpt = "ItemMotion2_N.rpt"
                'rpt.Rpt = "ItemMotion22_N.rpt"
                rpt.Rpt = "ItemMotion2Fifo_N.rpt"
            Case 6
                rpt.Rpt = "ImportMessageCostHistoryVal4.rpt"
            Case 7
                rpt.Rpt = "ImportMessageCostHistoryVal4.rpt"
            Case 8
                rpt.Rpt = "ItemMotion2Fifo_N2.rpt"
            Case 9
                rpt.Rpt = "SalesSerialDetails.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.Addcontrol_MouseDoubleClick({OrderTypeId, ImportMessageId, StoreId, StoreInvoiceNo, ItemId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate


        If Flag <> 5 AndAlso Flag <> 8 Then
            lblStoreId.Visibility = Windows.Visibility.Hidden
            StoreId.Visibility = Windows.Visibility.Hidden
            StoreName.Visibility = Windows.Visibility.Hidden

            lblStoreInvoiceNo.Visibility = Windows.Visibility.Hidden
            StoreInvoiceNo.Visibility = Windows.Visibility.Hidden

            lblItemId.Visibility = Windows.Visibility.Hidden
            ItemId.Visibility = Windows.Visibility.Hidden
            ItemName.Visibility = Windows.Visibility.Hidden
        End If


        If Flag = 9 Then
            lblStoreId.Visibility = Windows.Visibility.Visible
            StoreId.Visibility = Windows.Visibility.Visible
            StoreName.Visibility = Windows.Visibility.Visible
            
            lblOrderTypeId.Visibility = Windows.Visibility.Hidden
            OrderTypeId.Visibility = Windows.Visibility.Hidden
            OrderTypeName.Visibility = Windows.Visibility.Hidden

            lblImportMessageId.Visibility = Windows.Visibility.Hidden
            ImportMessageId.Visibility = Windows.Visibility.Hidden
            ImportMessageName.Visibility = Windows.Visibility.Hidden
        End If

        If Flag <> 6 AndAlso Flag <> 7 Then
            lblFromDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden

            lblToDate.Visibility = Windows.Visibility.Hidden
            ToDate.Visibility = Windows.Visibility.Hidden
        End If

        If Not (Flag = 5 OrElse Flag = 8) Then
            BtnCalc.Visibility = Windows.Visibility.Hidden
        End If

        LoadResource()

    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        
    End Sub

    Private Sub OrderTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OrderTypeId.LostFocus
        bm.LostFocus(OrderTypeId, OrderTypeName, "select Name from OrderTypes where Id=" & OrderTypeId.Text.Trim(), True)
    End Sub

    Private Sub OrderTypeId_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OrderTypeId.KeyUp
        If bm.ShowHelp("OrderTypes", OrderTypeId, OrderTypeName, e, "select cast(Id as varchar(100)) Id,Name from OrderTypes") Then
            OrderTypeId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ImportMessageId_KeyUp(sender As Object, e As KeyEventArgs) Handles ImportMessageId.KeyUp
        If bm.ShowHelp("ImportMessages", ImportMessageId, ImportMessageName, e, "select cast(Id as varchar(100)) 'رقم الرسالة',dbo.GetShipperName(ShipperId) الشاحن from ImportMessages where OrderTypeId='" & OrderTypeId.Text & "'") Then
            ImportMessageId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub ImportMessageId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ImportMessageId.LostFocus
        bm.LostFocus(ImportMessageId, ImportMessageName, "select dbo.GetShipperName(ShipperId) from ImportMessages  where OrderTypeId='" & OrderTypeId.Text & "' and Id=" & ImportMessageId.Text)
        StoreId.Text = bm.ExecuteScalar("select StoreId from ImportMessages  where OrderTypeId='" & OrderTypeId.Text & "' and Id=" & ImportMessageId.Text)
        StoreId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub StoreId_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
        End If
    End Sub

    Private Sub StoreId_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text)
    End Sub

    Private Sub StoreInvoiceNo_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreInvoiceNo.KeyUp
        If bm.ShowHelpMultiColumns("الفواتير", StoreInvoiceNo, StoreInvoiceNo, e, "select cast(T.InvoiceNo as varchar(100)) 'الفاتورة',dbo.GetSupplierName(T.ToId) 'المورد',T.DocNo 'رقم عقد المورد',cast(T.TotalAfterDiscount as nvarchar(100)) 'إجمالي الفاتورة' from SalesMaster T left join ImportMessagesDetails TT on(T.OrderTypeId=TT.OrderTypeId AND T.InvoiceNo=TT.InvoiceNo) where T.StoreId=" & StoreId.Text & " and T.Flag=" & Sales.FlagState.الاستيراد & " and T.OrderTypeId=" & Val(OrderTypeId.Text) & " and TT.Id=" & Val(ImportMessageId.Text)) Then
        End If
    End Sub

    Private Sub StoreInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreInvoiceNo.LostFocus
        If Val(StoreInvoiceNo.Text) = 0 Then Return
        If Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & StoreInvoiceNo.Text) Then
            bm.ShowMSG("هذا الرقم غير صحيح")
            StoreInvoiceNo.Clear()
        End If
    End Sub

    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select distinct cast(It.Id as varchar(100)) Id,It.Name from Items It left join ImportMessagesDetailsSub T on(It.Id=T.ItemId)  where (T.OrderTypeId=" & Val(OrderTypeId.Text) & " or " & Val(OrderTypeId.Text) & "=0) and (T.Id=" & Val(ImportMessageId.Text) & " or " & Val(ImportMessageId.Text) & "=0) and (T.StoreId=" & Val(StoreId.Text) & " or " & Val(StoreId.Text) & "=0) AND (T.InvoiceNo=" & Val(StoreInvoiceNo.Text) & " or " & Val(StoreInvoiceNo.Text) & "=0)") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
    End Sub

    Private Sub BtnCalc_Click(sender As Object, e As RoutedEventArgs) Handles BtnCalc.Click
        Dim f As New RPT25 With {.Flag = 20}
        f.UserControl_Loaded(Nothing, Nothing)
        f.FromDate.SelectedDate = FromDate.SelectedDate
        f.ToDate.SelectedDate = ToDate.SelectedDate
        f.Button2_Click(BtnCalc, Nothing)
    End Sub
End Class