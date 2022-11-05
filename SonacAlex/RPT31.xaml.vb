Imports System.Data

Public Class RPT31
    Public MyLinkFile As Integer = 0
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        'If MainLinkFile.Visibility = Windows.Visibility.Visible AndAlso MainLinkFile.IsEnabled AndAlso MainLinkFile.SelectedIndex = 0 Then
        '    bm.ShowMSG("برجاء تحديد " & lblMainLinkFile.Content)
        '    MainLinkFile.Focus()
        '    Return
        'End If
        'If MyLinkFile = 0 AndAlso Val(MainAccNo.Text) = 0 Then
        '    bm.ShowMSG("برجاء تحديد الحساب العام")
        '    MainAccNo.Focus()
        '    Return
        'End If
        'If MyLinkFile = 0 AndAlso Val(SubAccNo.Text) = 0 AndAlso SubAccNo.IsEnabled Then
        '    bm.ShowMSG("برجاء تحديد الحساب الفرعى")
        '    SubAccNo.Focus()
        '    Return
        'End If
        
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@SubAccNo", "SubAccName", "@FromDate", "@ToDate", "Header", "@LinkFile", "@ToId", "@CostTypeId", "@PurchaseAccNo", "@ImportMessageId", "@StoreId", "@StoreInvoiceNo"}
        rpt.paravalue = New String() {MainAccNo.Text, MainAccName.Text, Val(SubAccNo.Text), SubAccName.Text, FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title.Trim & " " & MainLinkFile.Text, MyLinkFile, Val(SubAccNo.Text), Val(CostTypeId.SelectedValue), Val(PurchaseAccNo.Text), Val(ImportMessageId.Text), Val(StoreId.Text), Val(StoreInvoiceNo.Text)}
        Select Case Flag
            Case 1
                rpt.Rpt = "OutcomeDetailed.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({MainAccNo, SubAccNo, PurchaseAccNo, ImportMessageId, StoreId, StoreInvoiceNo})
        bm.FillCombo("LinkFile", MainLinkFile, "", , True)
        MainLinkFile.SelectedValue = 9
        MainLinkFile_LostFocus(Nothing, Nothing)
        bm.FillCombo("CostTypes", CostTypeId, "")

        If MyLinkFile = -1 Then
            MyLinkFile = 0
            MainLinkFile.SelectedIndex = 0
            MainLinkFile.Visibility = Windows.Visibility.Hidden
            lblMainLinkFile.Visibility = Windows.Visibility.Hidden
        ElseIf MyLinkFile = 0 Then
            lblMainAcc.Visibility = Windows.Visibility.Hidden
            MainAccNo.Visibility = Windows.Visibility.Hidden
            MainAccName.Visibility = Windows.Visibility.Hidden
        End If

        If Flag = 2 Then
            MainLinkFile.Visibility = Windows.Visibility.Hidden
            lblMainLinkFile.Visibility = Windows.Visibility.Hidden
        End If

        If MyLinkFile > 0 And Flag <> 3 Then
            lblMainAcc.Visibility = Windows.Visibility.Collapsed
            MainAccNo.Visibility = Windows.Visibility.Collapsed
            MainAccName.Visibility = Windows.Visibility.Collapsed
        End If
        If Flag = 3 Then
            lblMainAcc.Visibility = Windows.Visibility.Visible
            MainAccNo.Visibility = Windows.Visibility.Visible
            MainAccName.Visibility = Windows.Visibility.Visible
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

    End Sub

    Dim lop As Boolean = False
    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If lop Then Return
        If MyLinkFile = 0 Then
            If Val(MainAccNo.Text) = 0 Or Not SubAccNo.IsEnabled Then
                SubAccNo.Clear()
                SubAccName.Clear()
                Return
            End If
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
            bm.LostFocus(SubAccNo, SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim() & " and AccNo='" & MainAccNo.Text & "'")
        Else
            If Val(SubAccNo.Text) = 0 Then
                SubAccNo.Clear()
                SubAccName.Clear()
                Return
            End If
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
            bm.LostFocus(SubAccNo, SubAccName, "select Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ") where Id=" & SubAccNo.Text.Trim())
            If MyLinkFile > 0 Then
                bm.LostFocus(SubAccNo, MainAccNo, "select AccNo from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim())
                lop = True
                MainAccNo_LostFocus(Nothing, Nothing)
                lop = False
            End If

        End If
    End Sub
    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubAccNo.KeyUp
        If MyLinkFile = 0 Then
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')")
            If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & dt.Rows(0)("Id") & "," & Md.UserName & ") where AccNo='" & MainAccNo.Text & "'") Then
                SubAccNo_LostFocus(Nothing, Nothing)
            End If
        Else
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
            If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ")") Then
                SubAccNo_LostFocus(Nothing, Nothing)
            End If
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , MyLinkFile, )

        SubAccNo.IsEnabled = MainAccNo.Visibility <> Windows.Visibility.Visible OrElse MyLinkFile > 0 OrElse bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo.Text & "')").Rows.Count > 0
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , MyLinkFile, )
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
        lblMainAcc.SetResourceReference(Label.ContentProperty, "Main AccNo")
        lblSubAcc.SetResourceReference(Label.ContentProperty, "Sub AccNo")
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        MyLinkFile = MainLinkFile.SelectedValue
        MainAccNo_LostFocus(Nothing, Nothing)
        SubAccNo_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CostTypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CostTypeId.SelectionChanged
        Select Case CostTypeId.SelectedValue
            Case 1
                PurchaseAccNo.IsEnabled = False
                ImportMessageId.IsEnabled = False
                StoreId.IsEnabled = False
                StoreInvoiceNo.IsEnabled = False
            Case 2
                PurchaseAccNo.IsEnabled = True
                ImportMessageId.IsEnabled = False
                StoreId.IsEnabled = False
                StoreInvoiceNo.IsEnabled = False
            Case 3
                PurchaseAccNo.IsEnabled = True
                ImportMessageId.IsEnabled = True
                StoreId.IsEnabled = False
                StoreInvoiceNo.IsEnabled = False
            Case 4
                PurchaseAccNo.IsEnabled = False
                ImportMessageId.IsEnabled = False
                StoreId.IsEnabled = True
                StoreInvoiceNo.IsEnabled = True
            Case Else
                PurchaseAccNo.IsEnabled = True
                ImportMessageId.IsEnabled = True
                StoreId.IsEnabled = True
                StoreInvoiceNo.IsEnabled = True
        End Select

        If Not PurchaseAccNo.IsEnabled Then PurchaseAccNo.Clear() : PurchaseAccName.Clear()
        If Not ImportMessageId.IsEnabled Then ImportMessageId.Clear() : ImportMessageName.Clear()
        If Not StoreId.IsEnabled Then StoreId.Clear() : StoreName.Clear()
        If Not StoreInvoiceNo.IsEnabled Then StoreInvoiceNo.Clear()

    End Sub


    Private Sub PurchaseAccNo_KeyUp(sender As Object, e As KeyEventArgs) Handles PurchaseAccNo.KeyUp
        If bm.ShowHelp("OrderTypes", PurchaseAccNo, PurchaseAccName, e, "select cast(Id as varchar(100)) Id,Name from OrderTypes") Then
        End If
    End Sub

    Private Sub PurchaseAccNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles PurchaseAccNo.LostFocus
        bm.LostFocus(PurchaseAccNo, PurchaseAccName, "select Name from OrderTypes where Id=" & Val(PurchaseAccNo.Text))
    End Sub

    Private Sub ImportMessageId_KeyUp(sender As Object, e As KeyEventArgs) Handles ImportMessageId.KeyUp
        If bm.ShowHelp("ImportMessages", ImportMessageId, ImportMessageName, e, "select cast(Id as varchar(100)) 'رقم الرسالة',dbo.GetShipperName(ShipperId) الشاحن from ImportMessages where OrderTypeId='" & PurchaseAccNo.Text & "'") Then
        End If
    End Sub

    Private Sub ImportMessageId_LostFocus(sender As Object, e As RoutedEventArgs) Handles ImportMessageId.LostFocus
        bm.LostFocus(ImportMessageId, ImportMessageName, "select dbo.GetShipperName(ShipperId) from ImportMessages  where OrderTypeId='" & PurchaseAccNo.Text & "' and Id=" & ImportMessageId.Text)
    End Sub

    Private Sub StoreId_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
        End If
    End Sub

    Private Sub StoreId_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text)
    End Sub

    Private Sub StoreInvoiceNo_KeyUp(sender As Object, e As KeyEventArgs) Handles StoreInvoiceNo.KeyUp
        If bm.ShowHelpMultiColumns("الفواتير", StoreInvoiceNo, StoreInvoiceNo, e, "select cast(InvoiceNo as varchar(100)) 'الفاتورة',dbo.GetSupplierName(ToId) 'المورد',DocNo 'رقم عقد المورد',cast(TotalAfterDiscount as nvarchar(100)) 'إجمالي الفاتورة',cast(OrderTypeId as nvarchar(100)) 'مسلسل الطلبية',dbo.GetOrderTypes(OrderTypeId) 'اسم الطلبية' from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد) Then
        End If
    End Sub

    Private Sub StoreInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreInvoiceNo.LostFocus
        If Val(StoreInvoiceNo.Text) = 0 Then Return
        If Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & StoreInvoiceNo.Text) Then
            bm.ShowMSG("هذا الرقم غير صحيح")
            StoreInvoiceNo.Clear()
        End If
    End Sub

End Class