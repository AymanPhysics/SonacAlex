Imports System.Data

Public Class Entry2
    Public TableName As String = "Entry2"
    Public SubId As String = "Entry2TypeId"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 3
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {btnPrint})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId1)
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId2)
        bm.FillCombo("CostTypes", CostTypeId, "")

        LoadResource()
        bm.Fields = New String() {SubId, SubId2, "DayDate", "Value", "Value1", "MainAccNo1", "SubAccNo1", "CurrencyId1", "Value2", "MainAccNo2", "SubAccNo2", "CurrencyId2", "Notes", "Canceled", "CostTypeId", "PurchaseAccNo", "ImportMessageId", "StoreId", "StoreInvoiceNo", "IsCost1", "IsCost2"}
        bm.control = New Control() {Entry2TypeId, txtID, DayDate, Value, Value1, MainAccNo1, SubAccNo1, CurrencyId1, Value2, MainAccNo2, SubAccNo2, CurrencyId2, Notes, Canceled, CostTypeId, PurchaseAccNo, ImportMessageId, StoreId, StoreInvoiceNo, IsCost1, IsCost2}
        bm.KeyFields = New String() {SubId, SubId2}
        bm.Table_Name = TableName

        bm.FillCombo("GetEmpEntry2Types @Flag=" & Flag & ",@EmpId=" & Md.UserName & "", Entry2TypeId)

        'Value1.Visibility = Windows.Visibility.Hidden
        'lblValue1.Visibility = Windows.Visibility.Hidden
        'CurrencyId1.Visibility = Windows.Visibility.Hidden
        'Value2.Visibility = Windows.Visibility.Hidden
        'lblValue2.Visibility = Windows.Visibility.Hidden
        'CurrencyId2.Visibility = Windows.Visibility.Hidden
        'lblValue.Content = "المبلغ"


        GroupBoxCostType.Visibility = Windows.Visibility.Hidden

        btnNew_Click(sender, e)
        DayDate.Focus()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        
        lv = True
        bm.FillControls(Me)
        CostTypeId_SelectionChanged(Nothing, Nothing)

        MainAccNo1_LostFocus(Nothing, Nothing)
        SubAccNo1_LostFocus(Nothing, Nothing)
        MainAccNo2_LostFocus(Nothing, Nothing)
        SubAccNo2_LostFocus(Nothing, Nothing)

        PurchaseAccNo_LostFocus(Nothing, Nothing)
        ImportMessageId_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)
        StoreInvoiceNo_LostFocus(Nothing, Nothing)

        DayDate.Focus()
        lv = False

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {Val(Entry2TypeId.SelectedValue), txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not btnSave.IsEnabled Then Return

        If Entry2TypeId.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد اليومية")
            Entry2TypeId.Focus()
            Return
        End If
        If Val(MainAccNo1.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo1.Focus()
            Return
        End If
        If Val(SubAccNo1.Text) = 0 AndAlso SubAccNo1.IsEnabled AndAlso Val(MainAccNo1.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo1.Focus()
            Return
        End If
        If Val(MainAccNo2.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo2.Focus()
            Return
        End If
        If Val(SubAccNo2.Text) = 0 AndAlso SubAccNo2.IsEnabled AndAlso Val(MainAccNo2.Text) <> 0 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo2.Focus()
            Return
        End If

        If CostTypeId.SelectedIndex = 0 AndAlso Val(PurchaseAccNo.Text) > 0 AndAlso Val(ImportMessageId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الرسالة")
            ImportMessageId.Focus()
            Return
        End If
        If CostTypeId.SelectedIndex = 0 AndAlso Val(StoreId.Text) > 0 AndAlso Val(StoreInvoiceNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد مسلسل الفاتورة")
            StoreInvoiceNo.Focus()
            Return
        End If


        Value.Text = Val(Value.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId, SubId2}, New String() {Val(Entry2TypeId.SelectedValue), txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Dim lop As Boolean = False
    Sub ClearControls()
        If lop OrElse lv Then Return

        bm.ClearControls()

        IsCost1.IsChecked = True

        MainAccName1.Clear()
        SubAccName1.Clear()
        MainAccName2.Clear()
        SubAccName2.Clear()

        PurchaseAccName.Clear()
        ImportMessageName.Clear()
        StoreName.Clear()

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & SubId & "=" & Val(Entry2TypeId.SelectedValue))
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        'txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & Val(Entry2TypeId.SelectedValue) & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {Val(Entry2TypeId.SelectedValue), txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {SubId, SubId2}, New String() {Val(Entry2TypeId.SelectedValue), txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()

            dt = bm.ExecuteAdapter("select FromInvoiceNo,ToInvoiceNo from Entry2Types where  Id=" & Entry2TypeId.SelectedValue.ToString)
            If dt.Rows.Count > 0 Then
                If Val(txtID.Text) < dt.Rows(0)("FromInvoiceNo") OrElse Val(txtID.Text) > dt.Rows(0)("ToInvoiceNo") Then txtID.Text = ""
            End If

            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, MainAccNo1.KeyDown, SubAccNo1.KeyDown, MainAccNo2.KeyDown, SubAccNo2.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub SubAccNo1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo1.LostFocus
        If Val(MainAccNo1.Text) = 0 Or Not SubAccNo1.IsEnabled Then
            SubAccNo1.Clear()
            SubAccName1.Clear()
            CurrencyId1.SelectedValue = 1
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo1.Text & "')")
        bm.LostFocus(SubAccNo1, SubAccName1, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo1.Text.Trim() & " and AccNo='" & MainAccNo1.Text & "'")

        If lop OrElse lv Then Return
        Dim x As Integer = Val(bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo1.Text.Trim() & " and AccNo='" & MainAccNo1.Text & "'"))
        If x < 1 Then x = 1
        CurrencyId1.SelectedValue = x
    End Sub

    Private Sub SubAccNo1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubAccNo1.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo1.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo1, SubAccName1, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & MainAccNo1.Text & "'") Then
            SubAccNo1_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo1_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo1.LostFocus
        bm.AccNoLostFocus(MainAccNo1, MainAccName1, , , )

        SubAccNo1.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo1.Text & "')").Rows.Count > 0
        SubAccNo1_LostFocus(Nothing, Nothing)
        If SubAccNo1.IsEnabled Then
            SubAccNo1.Focus()
        Else
            Value.Focus()
        End If
    End Sub

    Private Sub MainAccNo1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo1.KeyUp
        If bm.AccNoShowHelp(MainAccNo1, MainAccName1, e, , , ) Then
            MainAccNo1_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub SubAccNo2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo2.LostFocus
        If Val(MainAccNo2.Text) = 0 Or Not SubAccNo2.IsEnabled Then
            SubAccNo2.Clear()
            SubAccName2.Clear()
            CurrencyId2.SelectedValue = 1
            CurrencyId2_SelectionChanged(Nothing, Nothing)
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo2.Text & "')")
        bm.LostFocus(SubAccNo2, SubAccName2, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo2.Text.Trim() & " and AccNo='" & MainAccNo2.Text & "'")

        If lop OrElse lv Then Return
        Dim x As Integer = Val(bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo2.Text.Trim() & " and AccNo='" & MainAccNo2.Text & "'"))
        If x < 1 Then x = 1
        CurrencyId2.SelectedValue = x
        CurrencyId2_SelectionChanged(Nothing, Nothing)
    End Sub

    Private Sub SubAccNo2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubAccNo2.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo2.Text & "')")
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo2, SubAccName2, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & MainAccNo2.Text & "'") Then
            SubAccNo2_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo2.LostFocus
        bm.AccNoLostFocus(MainAccNo2, MainAccName2, , , )

        SubAccNo2.IsEnabled = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo2.Text & "')").Rows.Count > 0
        SubAccNo2_LostFocus(Nothing, Nothing)
        If SubAccNo2.IsEnabled Then
            SubAccNo2.Focus()
        Else
            Notes.Focus()
        End If
    End Sub

    Private Sub MainAccNo2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo2.KeyUp
        If bm.AccNoShowHelp(MainAccNo2, MainAccName2, e, , , ) Then
            MainAccNo2_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Value.Text = ""
        Value.IsEnabled = False
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked
        Value.IsEnabled = True
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        lblID.SetResourceReference(Label.ContentProperty, "Id")
        lblDayDate.SetResourceReference(Label.ContentProperty, "DayDate")
        lblValue.SetResourceReference(Label.ContentProperty, "Value")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
        lblMainAccNo1.SetResourceReference(Label.ContentProperty, "MainAccNo")
        lblSubAccNo1.SetResourceReference(Label.ContentProperty, "SubAccNo")
        lblMainAccNo2.SetResourceReference(Label.ContentProperty, "MainAccNo")
        lblSubAccNo2.SetResourceReference(Label.ContentProperty, "SubAccNo")
    End Sub


    Private Sub Value1_LostFocus(sender As Object, e As RoutedEventArgs) Handles Value1.LostFocus, Value2.LostFocus
        Try
            If Val(Value1.Text) = 0 Then
                Value.Text = Val(Value2.Text) * Val(bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(SubAccNo2.Text) & ",'" & (bm.ExecuteScalar("select Id from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo2.Text & "')")) & "'," & CurrencyId2.SelectedValue.ToString & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')"))
            Else
                Value.Text = Val(Value1.Text) * Val(bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(SubAccNo1.Text) & ",'" & (bm.ExecuteScalar("select Id from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo1.Text & "')")) & "'," & CurrencyId1.SelectedValue.ToString & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')"))
            End If
        Catch ex As Exception
        End Try
        Value_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CurrencyId1_SelectionChanged(sender As Object, e As RoutedEventArgs) Handles CurrencyId1.SelectionChanged
        If lop OrElse lv Then Return
        If Val(Value1.Text) > 0 Then Return
        Try
            Value1.Text = Val(Value.Text) / Val(bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(SubAccNo1.Text) & ",'" & (bm.ExecuteScalar("select Id from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo1.Text & "')")) & "'," & CurrencyId1.SelectedValue.ToString & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')"))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CurrencyId2_SelectionChanged(sender As Object, e As RoutedEventArgs) Handles CurrencyId2.SelectionChanged
        If lop OrElse lv Then Return
        Try
            Value2.Text = Val(Value.Text) / Val(bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(SubAccNo2.Text) & ",'" & (bm.ExecuteScalar("select Id from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & MainAccNo2.Text & "')")) & "'," & CurrencyId2.SelectedValue.ToString & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')"))
        Catch ex As Exception
        End Try
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
        If bm.ShowHelpMultiColumns("الفواتير", StoreInvoiceNo, StoreInvoiceNo, e, "select cast(InvoiceNo as varchar(100)) 'الفاتورة',dbo.GetSupplierName(ToId) 'المورد',DocNo 'رقم عقد المورد',cast(TotalAfterDiscount as nvarchar(100)) 'إجمالي الفاتورة',cast(OrderTypeId as nvarchar(100)) 'مسلسل الطلبية',dbo.GetOrderTypes(OrderTypeId) 'اسم الطلبية' from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and ToId in('" & SubAccNo1.Text & "','" & SubAccNo2.Text & "')") Then
        End If
    End Sub

    Private Sub StoreInvoiceNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles StoreInvoiceNo.LostFocus
        If Val(StoreInvoiceNo.Text) = 0 Then Return
        If Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & StoreInvoiceNo.Text) Then
            bm.ShowMSG("هذا الرقم غير صحيح")
            StoreInvoiceNo.Clear()
        End If
    End Sub

    Dim DontClear As Boolean = False
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        DontClear = True
        btnSave_Click(Nothing, Nothing)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Rpt = "Entry2One.rpt"
        rpt.paraname = New String() {"@Entry2TypeId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(Entry2TypeId.SelectedValue), Val(txtID.Text), CType(Parent, Page).Title}
        rpt.Show()
    End Sub

    Private Sub IsCost1_Checked(sender As Object, e As RoutedEventArgs) Handles IsCost1.Checked, IsCost1.Unchecked
        IsCost2.IsChecked = Not IsCost1.IsChecked
    End Sub

    Private Sub IsCost2_Checked(sender As Object, e As RoutedEventArgs) Handles IsCost2.Checked, IsCost2.Unchecked
        IsCost1.IsChecked = Not IsCost2.IsChecked
    End Sub

    Private Sub Entry2TypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Entry2TypeId.SelectionChanged
        btnNew_Click(Nothing, Nothing)
        txtID_LostFocus(Nothing, Nothing)
    End Sub


    Private Sub Value_LostFocus(sender As Object, e As RoutedEventArgs) Handles Value.LostFocus
        CurrencyId1_SelectionChanged(Nothing, Nothing)
        CurrencyId2_SelectionChanged(Nothing, Nothing)
    End Sub
End Class
