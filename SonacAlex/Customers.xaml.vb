Imports System.Data
Imports System.IO
Imports Newtonsoft.Json

Public Class Customers
    Public TableName As String = "Customers"
    Public SubId As String = "Id"
    Public SubName As String = "Name"



    Public MyId As Integer = 0
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public IsExport As Boolean = False

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        bm.FillCombo("PriceLists", PriceListId, "")
        PriceListId.Visibility = Windows.Visibility.Hidden
        If Md.ShowPriceLists Then
            PriceListId.Visibility = Windows.Visibility.Visible
        End If

        receiver_address_country.ItemsSource = JsonConvert.DeserializeObject(Of List(Of Country))(File.ReadAllText("Json/CountryCodes.json"))
        receiver_address_country.SelectedValuePath = "code"
        receiver_address_country.DisplayMemberPath = "Desc_ar"


        bm.FillCombo("select Code Id,Name from TaxApi_TeceiverTypes order by Id desc", receiver_type)


        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {SubId, SubName, "EnName", "AccNo", "CountryId", "CityId", "AreaId", "Address", "Floor", "Appartment", "Tel", "Mobile", "CurrencyId", "Exchange", "MainBal0", "Bal0", "DescPerc", "Type", "ApplyCurrencyCycle", "Ban", "PriceListId", "receiver_address_branchID", "receiver_address_country", "receiver_address_governate", "receiver_address_regionCity", "receiver_address_street", "receiver_address_buildingNumber", "receiver_address_postalCode", "receiver_address_floor", "receiver_address_room", "receiver_address_landmark", "receiver_address_additionalInformation", "receiver_type", "receiver_id", "receiver_name", "Notes"}
        bm.control = New Control() {txtID, txtName, EnName, AccNo, CountryId, CityId, AreaId, Address, Floor, Appartment, Tel, Mobile, CurrencyId, Exchange, MainBal0, Bal0, DescPerc, Type, ApplyCurrencyCycle, Ban, PriceListId, receiver_address_branchID, receiver_address_country, receiver_address_governate, receiver_address_regionCity, receiver_address_street, receiver_address_buildingNumber, receiver_address_postalCode, receiver_address_floor, receiver_address_room, receiver_address_landmark, receiver_address_additionalInformation, receiver_type, receiver_id, receiver_name, Notes}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName
        If Not Md.ShowCurrency Then
            CurrencyId.Visibility = Windows.Visibility.Hidden
            lblExchange.Visibility = Windows.Visibility.Hidden
            Exchange.Visibility = Windows.Visibility.Hidden
            lblBal0.Visibility = Windows.Visibility.Hidden
            Bal0.Visibility = Windows.Visibility.Hidden
            ApplyCurrencyCycle.Visibility = Windows.Visibility.Hidden
        End If
        btnNew_Click(sender, e)
        If MyId > 0 Then
            txtID.Text = MyId
            txtID_LostFocus(Nothing, Nothing)
            If Not Md.Manager Then
                btnDelete.IsEnabled = False
                Bal0.IsEnabled = False
                MainBal0.IsEnabled = False
            End If
        End If

        If IsExport Then
            btnDelete.Visibility = Visibility.Hidden
            AccNo.IsEnabled = False
            txtName.IsEnabled = False
            TabControl2.IsEnabled = False
            CountryId.IsEnabled = False
            CityId.IsEnabled = False
            AreaId.IsEnabled = False
            Floor.IsEnabled = False
            Appartment.IsEnabled = False
            Tel.IsEnabled = False
            Mobile.IsEnabled = False
            CurrencyId.IsEnabled = False
            Exchange.IsEnabled = False
            MainBal0.IsEnabled = False
            Bal0.IsEnabled = False
            DescPerc.IsEnabled = False
            Type.IsEnabled = False
            ApplyCurrencyCycle.IsEnabled = False
            Ban.IsEnabled = False
            PriceListId.IsEnabled = False
        End If
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        AccNo_LostFocus(Nothing, Nothing)
        CountryId_LostFocus(Nothing, Nothing)
        CityId_LostFocus(Nothing, Nothing)
        AreaId_LostFocus(Nothing, Nothing)
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsExport AndAlso txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If

        If Not IsExport AndAlso Val(AccNo.Text) = 0 Then
            bm.ShowMSG("Please, Select Main AccNo")
            AccNo.Focus()
            Return
        End If
        If IsExport AndAlso EnName.Text.Trim = "" Then
            EnName.Focus()
            Return
        End If
        If IsExport AndAlso txtName.Text.Trim = "" Then
            txtName.Text = EnName.Text
        End If

        Bal0.Text = Val(Bal0.Text.Trim)
        DescPerc.Text = Val(DescPerc.Text.Trim)
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


        bm.ClearControls(False)
        receiver_address_country.SelectedValue = "EG"
        CurrencyId_LostFocus(Nothing, Nothing)

        AccName.Clear()
        CountryName.Clear()
        CityName.Clear()
        AreaName.Clear()


        receiver_address_country.SelectedValue = "EG"
        receiver_address_governate.Text = "الإسكندرية"
        receiver_address_regionCity.Text = "الإسكندرية"


        txtName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
        If txtID.Text = "" Then txtID.Text = "1"

        'txtName.Focus()
        If IsExport Then
            EnName.Focus()
        Else
            txtName.Focus()
        End If
        'txtID.Focus()
        'txtID.SelectAll()

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            If Val(bm.ExecuteScalar("select dbo.GetSubAccUsingCount(" & 1 & ",'" & txtID.Text.Trim & "')")) > 0 Then
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

    Private Sub CountryId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CountryId.KeyUp
        bm.ShowHelp("Countries", CountryId, CountryName, e, "select cast(Id as varchar(100)) Id,Name from Countries", "Countries")
    End Sub

    Private Sub CityId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CityId.KeyUp
        bm.ShowHelp("Cities", CityId, CityName, e, "select cast(Id as varchar(100)) Id,Name from Cities where CountryId=" & CountryId.Text.Trim, "Cities", {"CountryId"}, {Val(CountryId.Text)})
    End Sub

    Private Sub AreaId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AreaId.KeyUp
        bm.ShowHelp("Areas", AreaId, AreaName, e, "select cast(Id as varchar(100)) Id,Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text, "Areas", {"CountryId", "CityId"}, {Val(CountryId.Text), Val(CityId.Text)})
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, CountryId.KeyDown, CityId.KeyDown, AreaId.KeyDown, AccNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub


    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Bal0.KeyDown, DescPerc.KeyDown, MainBal0.KeyDown, Exchange.KeyDown
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



    Private Sub CountryId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CountryId.LostFocus
        bm.LostFocus(CountryId, CountryName, "select Name from Countries where Id=" & CountryId.Text.Trim())
        CityId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub CityId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CityId.LostFocus
        bm.LostFocus(CityId, CityName, "select Name from Cities where CountryId=" & CountryId.Text.Trim & " and Id=" & CityId.Text.Trim())
        AreaId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub AreaId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AreaId.LostFocus
        bm.LostFocus(AreaId, AreaName, "select Name from Areas where CountryId=" & CountryId.Text.Trim & " and CityId=" & CityId.Text.Trim & " and Id=" & AreaId.Text.Trim())
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus
        bm.AccNoLostFocus(AccNo, AccName, , 1, )
    End Sub

    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, , 1, )
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
        lblAppartment.SetResourceReference(Label.ContentProperty, "Appartment")
        lblBal0.SetResourceReference(Label.ContentProperty, "Bal0")
        lblCountryId.SetResourceReference(Label.ContentProperty, "CountryId")
        lblCityId.SetResourceReference(Label.ContentProperty, "CityId")
        lblAreaId.SetResourceReference(Label.ContentProperty, "AreaId")
        lblDescPerc.SetResourceReference(Label.ContentProperty, "DescPerc")
        lblFloor.SetResourceReference(Label.ContentProperty, "Floor")
        lblMobile.SetResourceReference(Label.ContentProperty, "Mobile")
        lblTel.SetResourceReference(Label.ContentProperty, "Tel")
    End Sub

    Private Sub txtID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelpCustomers(txtID, txtName, e) Then
            txtID_LostFocus(sender, Nothing)
        End If
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

    Private Sub txtName_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtName.TextChanged
        receiver_name.Text = txtName.Text
    End Sub

    Public Sub Ban_Checked(sender As Object, e As RoutedEventArgs) Handles Ban.Checked, Ban.Unchecked
        If sender.IsChecked Then
            sender.Foreground = System.Windows.Media.Brushes.Red
            sender.FontWeight = FontWeights.ExtraBold
        Else
            sender.Foreground = System.Windows.Media.Brushes.Black
            sender.FontWeight = FontWeights.Normal
        End If
    End Sub
End Class
