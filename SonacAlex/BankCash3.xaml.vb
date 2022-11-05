Imports System.Data

Public Class BankCash3
    Public TableName As String = "BankCash3"
    Public MainId As String = "BankId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        bm.Fields = New String() {MainId, SubId, SubId2, "DayDate", "Value", "MainAccNo", "Notes", "Canceled", "Cash", "ImportMessageId", "StoreId", "StoreInvoiceNo", "CurrencyId", "OutComeTypeId"}
        bm.control = New Control() {BankId, txtFlag, txtID, DayDate, Value, MainAccNo, Notes, Canceled, Cash, ImportMessageId, StoreId, StoreInvoiceNo, CurrencyId, OutComeTypeId}

        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName
        RdoCash_Checked(Nothing, Nothing)
        btnNew_Click(sender, e)
        BankId.Focus()
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        'BankId_LostFocus(Nothing, Nothing)
        MainAccNo_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)
        ImportMessageId_LostFocus(Nothing, Nothing)
        OutComeTypeId_LostFocus(Nothing, Nothing)
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If
        If Val(MainAccNo.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الحساب العام")
            MainAccNo.Focus()
            Return
        End If
        If Val(ImportMessageId.Text) = 0 And ImportMessageId.Visibility = Windows.Visibility.Visible Then
            bm.ShowMSG("برجاء تحديد الرسالة")
            ImportMessageId.Focus()
            Return
        End If
        If Val(StoreId.Text) = 0 And StoreId.Visibility = Windows.Visibility.Visible Then
            bm.ShowMSG("برجاء تحديد المخزن")
            StoreId.Focus()
            Return
        End If
        If Val(StoreInvoiceNo.Text) = 0 And StoreInvoiceNo.Visibility = Windows.Visibility.Visible Then
            bm.ShowMSG("برجاء تحديد الفاتورة")
            StoreInvoiceNo.Focus()
            Return
        End If
        Value.Text = Val(Value.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}) Then Return
        btnNew_Click(sender, e)
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

    Dim lop As Boolean = False
    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True

        bm.ClearControls()
        BankId_LostFocus(Nothing, Nothing)

        'BankName.Clear()
        MainAccName.Clear()
        ImportMessageName.Clear()
        StoreName.Clear()
        OutComeTypeName.Clear()
        Rdo1.IsChecked = True
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & BankId.Text & " and " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & BankId.Text & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, MainAccNo.KeyDown, ImportMessageId.KeyDown, StoreId.KeyDown, StoreInvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus

        If Val(BankId.Text.Trim) = 0 Then
            BankId.Clear()
            BankName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        bm.LostFocus(BankId, BankName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
        CurrencyId.SelectedValue = bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub MainAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MainAccNo.LostFocus
        bm.AccNoLostFocus(MainAccNo, MainAccName, , , True)

        StoreInvoiceNo_LostFocus(Nothing, Nothing)
        ImportMessageId_LostFocus(Nothing, Nothing)

    End Sub

    Private Sub MainAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MainAccNo.KeyUp
        bm.AccNoShowHelp(MainAccNo, MainAccName, e, , , True)
    End Sub

    Private Sub RdoCash_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Rdo1.Checked, Rdo2.Checked, Rdo3.Checked, Rdo4.Checked
        Try
            Cash.Text = 0
            If Rdo1.IsChecked Then
                Cash.Text = 1
            ElseIf Rdo2.IsChecked Then
                Cash.Text = 2
            ElseIf Rdo3.IsChecked Then
                Cash.Text = 3
            ElseIf Rdo4.IsChecked Then
                Cash.Text = 4
            End If
        Catch ex As Exception
        End Try

        Try
            ImportMessageId.Clear()
            ImportMessageName.Clear()
            StoreId.Clear()
            StoreName.Clear()
            StoreInvoiceNo.Clear()

            If Rdo1.IsChecked Then
                lblImportMessageId.Visibility = Windows.Visibility.Hidden
                ImportMessageId.Visibility = Windows.Visibility.Hidden
                ImportMessageName.Visibility = Windows.Visibility.Hidden
                lblStoreId.Visibility = Windows.Visibility.Hidden
                StoreId.Visibility = Windows.Visibility.Hidden
                StoreName.Visibility = Windows.Visibility.Hidden
                lblStoreInvoiceNo.Visibility = Windows.Visibility.Hidden
                StoreInvoiceNo.Visibility = Windows.Visibility.Hidden
            ElseIf Rdo2.IsChecked Then
                lblImportMessageId.Visibility = Windows.Visibility.Hidden
                ImportMessageId.Visibility = Windows.Visibility.Hidden
                ImportMessageName.Visibility = Windows.Visibility.Hidden
                lblStoreId.Visibility = Windows.Visibility.Hidden
                StoreId.Visibility = Windows.Visibility.Hidden
                StoreName.Visibility = Windows.Visibility.Hidden
                lblStoreInvoiceNo.Visibility = Windows.Visibility.Hidden
                StoreInvoiceNo.Visibility = Windows.Visibility.Hidden
            ElseIf Rdo3.IsChecked Then
                lblImportMessageId.Visibility = Windows.Visibility.Visible
                ImportMessageId.Visibility = Windows.Visibility.Visible
                ImportMessageName.Visibility = Windows.Visibility.Visible
                lblStoreId.Visibility = Windows.Visibility.Hidden
                StoreId.Visibility = Windows.Visibility.Hidden
                StoreName.Visibility = Windows.Visibility.Hidden
                lblStoreInvoiceNo.Visibility = Windows.Visibility.Hidden
                StoreInvoiceNo.Visibility = Windows.Visibility.Hidden
            ElseIf Rdo4.IsChecked Then
                lblImportMessageId.Visibility = Windows.Visibility.Hidden
                ImportMessageId.Visibility = Windows.Visibility.Hidden
                ImportMessageName.Visibility = Windows.Visibility.Hidden
                lblStoreId.Visibility = Windows.Visibility.Visible
                StoreId.Visibility = Windows.Visibility.Visible
                StoreName.Visibility = Windows.Visibility.Visible
                lblStoreInvoiceNo.Visibility = Windows.Visibility.Visible
                StoreInvoiceNo.Visibility = Windows.Visibility.Visible
            Else
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub Cash_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Cash.TextChanged
        Try
            If Cash.Text = 1 Then
                Rdo1.IsChecked = True
            ElseIf Cash.Text = 2 Then
                Rdo2.IsChecked = True
            ElseIf Cash.Text = 3 Then
                Rdo3.IsChecked = True
            ElseIf Cash.Text = 4 Then
                Rdo4.IsChecked = True
            End If
        Catch ex As Exception
        End Try
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

        lblBank.SetResourceReference(Label.ContentProperty, "Bank")
        If MyLinkFile = 5 Then lblBank.SetResourceReference(Label.ContentProperty, "Safe")

        lblDayDate.SetResourceReference(Label.ContentProperty, "DayDate")
        lblValue.SetResourceReference(Label.ContentProperty, "Value")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
        lblMainAccNo.SetResourceReference(Label.ContentProperty, "MainAccNo")
    End Sub




    Private Sub ImportMessageId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ImportMessageId.KeyUp
        If bm.ShowHelp("الرسائل", ImportMessageId, ImportMessageName, e, "select cast(Id as varchar(100)) Id,Name from ImportMessages where AccNo=" & MainAccNo.Text) Then
            ImportMessageId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub ImportMessageId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ImportMessageId.LostFocus
        bm.LostFocus(ImportMessageId, ImportMessageName, "select Name from ImportMessages where Id=" & ImportMessageId.Text.Trim() & " and AccNo=" & MainAccNo.Text)
    End Sub



    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub


    Private Sub StoreInvoiceNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreInvoiceNo.KeyUp
        If bm.ShowHelp("الفواتير", StoreInvoiceNo, StoreInvoiceNo, e, "select cast(InvoiceNo as varchar(100)) Id,dbo.GetSupplierName(ToId) Name from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and AccNo=" & MainAccNo.Text, , , , "الفاتورة", "المورد") Then
            StoreInvoiceNo_LostFocus(StoreInvoiceNo, Nothing)
        End If
    End Sub

    Private Sub StoreInvoiceNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreInvoiceNo.LostFocus
        If Val(StoreInvoiceNo.Text) = 0 Then Return
        If Not StoreInvoiceNo.Text.Trim = "" AndAlso Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & StoreId.Text & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & StoreInvoiceNo.Text & " and AccNo=" & MainAccNo.Text) Then
            bm.ShowMSG("هذا الرقم غير صحيح")
            StoreInvoiceNo.Clear()
            StoreInvoiceNo.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub OutComeTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles OutComeTypeId.KeyUp
        bm.ShowHelp("OutComeTypes", OutComeTypeId, OutComeTypeName, e, "select cast(Id as varchar(100)) Id,Name from OutComeTypes")
    End Sub

    Private Sub OutComeTypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles OutComeTypeId.LostFocus
        bm.LostFocus(OutComeTypeId, OutComeTypeName, "select Name from OutComeTypes where Id=" & OutComeTypeId.Text.Trim())
    End Sub


End Class
