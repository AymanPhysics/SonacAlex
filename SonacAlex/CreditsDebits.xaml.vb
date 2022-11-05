Imports System.Data

Public Class CreditsDebits
    Public TableName As String = ""
    Public SubId As String = "Id"
    Public SubName As String = "Name"



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
        bm.Fields = New String() {SubId, SubName, "AccNo", "Address", "Mobile", "Tel", "ApplyCurrencyCycle", "CurrencyId", "Exchange", "MainBal0", "Bal0", "Notes"}
        bm.control = New Control() {txtID, txtName, AccNo, Address, Mobile, Tel, ApplyCurrencyCycle, CurrencyId, Exchange, MainBal0, Bal0, Notes}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        If MyLinkFile = 3 OrElse MyLinkFile = 4 OrElse MyLinkFile = 5 OrElse MyLinkFile = 6 Then
            Address.Visibility = Windows.Visibility.Hidden
            lblAddress.Visibility = Windows.Visibility.Hidden
            Mobile.Visibility = Windows.Visibility.Hidden
            lblMobile.Visibility = Windows.Visibility.Hidden
            Tel.Visibility = Windows.Visibility.Hidden
            lblTel.Visibility = Windows.Visibility.Hidden
        ElseIf MyLinkFile = 9 Or MyLinkFile = 10 Then
            Address.Visibility = Windows.Visibility.Hidden
            lblAddress.Visibility = Windows.Visibility.Hidden
            Mobile.Visibility = Windows.Visibility.Hidden
            lblMobile.Visibility = Windows.Visibility.Hidden
            Tel.Visibility = Windows.Visibility.Hidden
            lblTel.Visibility = Windows.Visibility.Hidden

            bm.Fields = New String() {SubId, SubName, "AccNo", "CurrencyId", "ApplyCurrencyCycle", "Exchange", "MainBal0", "Bal0", "Notes"}
            bm.control = New Control() {txtID, txtName, AccNo, CurrencyId, ApplyCurrencyCycle, Exchange, MainBal0, Bal0, Notes}
        ElseIf MyLinkFile = 11 Then
            Address.Visibility = Windows.Visibility.Hidden
            lblAddress.Visibility = Windows.Visibility.Hidden
            Mobile.Visibility = Windows.Visibility.Hidden
            lblMobile.Visibility = Windows.Visibility.Hidden
            Tel.Visibility = Windows.Visibility.Hidden
            lblTel.Visibility = Windows.Visibility.Hidden
            bm.Fields = New String() {SubId, SubName, "AccNo", "CurrencyId", "ApplyCurrencyCycle", "Exchange", "MainBal0", "Bal0", "Notes"}
            bm.control = New Control() {txtID, txtName, AccNo, CurrencyId, ApplyCurrencyCycle, Exchange, MainBal0, Bal0, Notes}
        End If

        If MyLinkFile = 6 Then
            bm.Fields = New String() {SubId, SubName, "AccNo", "Address", "Mobile", "Tel", "ApplyCurrencyCycle", "CurrencyId", "Exchange", "MainBal0", "Bal0", "Limit", "Notes"}
            bm.control = New Control() {txtID, txtName, AccNo, Address, Mobile, Tel, ApplyCurrencyCycle, CurrencyId, Exchange, MainBal0, Bal0, Limit, Notes}
            lblLimit.Visibility = Windows.Visibility.Visible
            Limit.Visibility = Windows.Visibility.Visible
        Else
            lblLimit.Visibility = Windows.Visibility.Hidden
            Limit.Visibility = Windows.Visibility.Hidden
        End If

        If Not Md.ShowCurrency Then
            CurrencyId.Visibility = Windows.Visibility.Hidden
            lblExchange.Visibility = Windows.Visibility.Hidden
            Exchange.Visibility = Windows.Visibility.Hidden
            lblBal0.Visibility = Windows.Visibility.Hidden
            Bal0.Visibility = Windows.Visibility.Hidden
            ApplyCurrencyCycle.Visibility = Windows.Visibility.Hidden
        End If

        btnNew_Click(sender, e)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        AccNo_LostFocus(Nothing, Nothing)
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
        If Val(AccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo.Focus()
            Return
        End If
        Bal0.Text = Val(Bal0.Text.Trim)
        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return
        btnNew_Click(sender, e)
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Sub ClearControls()
        bm.ClearControls()
        CurrencyId_LostFocus(Nothing, Nothing)

        AccName.Clear()
        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        txtName.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetSubAccUsingCount(" & MyLinkFile & ",'" & txtID.Text.Trim & "')")) > 0 Then
                bm.ShowMSG("غير مسموح بمسح حساب عليه حركات")
                Exit Sub
            End If
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
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
            Dim s As String = txtID.Text
            ClearControls()
            txtID.Text = s
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, AccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp(CType(Parent, Page).Title, txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from " & TableName, TableName) Then
            txtName.Focus()
        End If
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Bal0.KeyDown, MainBal0.KeyDown, Exchange.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub


    
    'Private Sub MyBase_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    If Not btnSave.Enabled Then Exit Sub
    '    Select Case bm.RequestDelete
    '        Case BasicMethods.CloseState.Yes
    '            
    '            btnSave_Click(Nothing, Nothing)
    '            If Not AllowClose Then e.Cancel = True
    '        Case BasicMethods.CloseState.No

    '        Case BasicMethods.CloseState.Cancel
    '            e.Cancel = True
    '    End Select
    'End Sub


    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , MyLinkFile, )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , MyLinkFile, )
    End Sub


    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")

        lblId.SetResourceReference(Label.ContentProperty, "Id")
        lblName.SetResourceReference(Label.ContentProperty, "Name")
        lblAccNo.SetResourceReference(Label.ContentProperty, "AccNo")

        lblAddress.SetResourceReference(Label.ContentProperty, "Address")
        lblMobile.SetResourceReference(Label.ContentProperty, "Mobile")
        lblTel.SetResourceReference(Label.ContentProperty, "Tel")

    End Sub

    Private Sub CurrencyId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CurrencyId.LostFocus
        Try
            Exchange.Text = bm.ExecuteScalar("select dbo.GetCurrencyExchange(0,0," & CurrencyId.SelectedValue.ToString & ",0,getdate())")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MainBal0_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MainBal0.TextChanged, Exchange.TextChanged
        Bal0.Text = Val(MainBal0.Text) * Val(Exchange.Text)
    End Sub
End Class