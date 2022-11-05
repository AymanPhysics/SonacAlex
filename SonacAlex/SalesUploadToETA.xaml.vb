Imports System.Data

Public Class SalesUploadToETA

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    'Public uuidUrl As String = "https://preprod.invoicing.eta.gov.eg/documents/"
    'Public printUrl As String = "https://preprod.invoicing.eta.gov.eg/print/documents/"

    Public uuidUrl As String = "https://invoicing.eta.gov.eg/documents/"
    Public printUrl As String = "https://invoicing.eta.gov.eg/print/documents/"

    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        lblNotes.Visibility = Visibility.Hidden
        Notes.Visibility = Visibility.Hidden
        btnDelete.Visibility = Visibility.Hidden
        btnDelete_Copy.Visibility = Visibility.Hidden
        LoadWFH()
        bm.FillCombo("select Name Id,cast(Id as nvarchar(10))+' - '+Name Name from TokenPins union select 0,'-' order by Name", TokenPin)
        TokenPin.SelectedValue = Val(GetSetting("OMEGA", "TokenPin", "TokenPin"))
        btnNew_Click(Nothing, Nothing)
    End Sub

    Structure GC
        Shared IsSelected As String = "IsSelected"
        Shared Flag As String = "Flag"
        Shared FlagName As String = "FlagName"
        Shared StoreId As String = "StoreId"
        Shared StoreName As String = "StoreName"
        Shared InvoiceNo As String = "InvoiceNo"
        Shared DocNo As String = "DocNo"
        Shared DayDate As String = "DayDate"
        Shared ToId As String = "ToId"
        Shared ToName As String = "ToName"
        Shared Qty As String = "Qty"
        Shared Total As String = "Total"
        Shared DiscountValue As String = "DiscountValue"
        Shared TaxAmount As String = "TaxAmount"
        Shared TotalAfterDiscount As String = "TotalAfterDiscount"
        Shared CurrencyName As String = "CurrencyName"
        Shared Notes As String = "Notes"
        Shared DocumentDetails As String = "DocumentDetails"
        Shared uuid As String = "uuid"
        Shared longId As String = "longId"
        Shared PDF As String = "PDF"
        Shared NeedReSubmit As String = "NeedReSubmit"
    End Structure

    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCIsSelected As New Forms.DataGridViewCheckBoxColumn
        GCIsSelected.HeaderText = "تحديد"
        GCIsSelected.Name = GC.IsSelected
        G.Columns.Add(GCIsSelected)

        G.Columns.Add(GC.Flag, "Flag")
        G.Columns.Add(GC.FlagName, "النوع")
        G.Columns.Add(GC.StoreId, "كود المخزن")
        G.Columns.Add(GC.StoreName, "اسم المخزن")
        G.Columns.Add(GC.InvoiceNo, "المسلسل")
        G.Columns.Add(GC.DocNo, "رقم الفاتورة")
        G.Columns.Add(GC.DayDate, "التاريخ")
        G.Columns.Add(GC.ToId, "كود العميل")
        G.Columns.Add(GC.ToName, "اسم العميل")
        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Total, "القيمة")
        G.Columns.Add(GC.DiscountValue, "ض.أ.ت.ص")
        G.Columns.Add(GC.TaxAmount, "ض.ق.م")
        G.Columns.Add(GC.TotalAfterDiscount, "صافى الفاتورة")
        G.Columns.Add(GC.CurrencyName, "العملة")
        G.Columns.Add(GC.Notes, "ملاحظات الضرائب")
        G.Columns.Add(GC.DocumentDetails, "التفاصيل")

        bm.AddThousandSeparator(G.Columns(GC.Qty), 3)
        bm.AddThousandSeparator(G.Columns(GC.Total))
        bm.AddThousandSeparator(G.Columns(GC.DiscountValue))
        bm.AddThousandSeparator(G.Columns(GC.TaxAmount))
        bm.AddThousandSeparator(G.Columns(GC.TotalAfterDiscount))

        Dim GCuuid As New Forms.DataGridViewLinkColumn
        GCuuid.HeaderText = "uuid"
        GCuuid.Name = GC.uuid
        G.Columns.Add(GCuuid)

        Dim GClongId As New Forms.DataGridViewLinkColumn
        GClongId.HeaderText = "Share"
        GClongId.Name = GC.longId
        G.Columns.Add(GClongId)

        Dim GCPDF As New Forms.DataGridViewLinkColumn
        GCPDF.HeaderText = "PDF"
        GCPDF.Name = GC.PDF
        G.Columns.Add(GCPDF)

        G.Columns.Add(GC.NeedReSubmit, "تحتاج إعادة رفع")

        G.Columns(GC.IsSelected).FillWeight = 80
        G.Columns(GC.StoreName).FillWeight = 150
        G.Columns(GC.DayDate).FillWeight = 150
        G.Columns(GC.ToName).FillWeight = 200
        G.Columns(GC.Notes).FillWeight = 200
        G.Columns(GC.DocumentDetails).FillWeight = 200
        G.Columns(GC.uuid).FillWeight = 300
        G.Columns(GC.longId).FillWeight = 300

        G.Columns(GC.Flag).Visible = False
        G.Columns(GC.StoreId).Visible = False
        G.Columns(GC.StoreName).Visible = False
        G.Columns(GC.InvoiceNo).Visible = False



        'G.ReadOnly = True
        For index = 1 To G.Columns.Count - 1
            G.Columns(index).ReadOnly = True
        Next
        G.AllowUserToAddRows = False
        G.AllowUserToDeleteRows = False

        RemoveHandler G.CellContentClick, AddressOf G_CellContentClick
        AddHandler G.CellContentClick, AddressOf G_CellContentClick
    End Sub

    Function TestTokenPin() As Boolean
        If TokenPin.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد التوكين")
            TokenPin.Focus()
            Return False
        End If
        Return True
    End Function
    Private Sub G_CellContentClick(sender As Object, e As Forms.DataGridViewCellEventArgs)
        If Not TestTokenPin() Then Return
        Dim tx As New TaxApi(TokenPin.SelectedValue)
        tx.Login()
        If G.CurrentCell.ColumnIndex = G.Columns(GC.uuid).Index AndAlso G.Rows(e.RowIndex).Cells(GC.uuid).Value.ToString() <> "" Then
            System.Diagnostics.Process.Start(uuidUrl & G(G.Columns(GC.uuid).Index, e.RowIndex).Value.ToString())
        ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.longId).Index AndAlso G.Rows(e.RowIndex).Cells(GC.uuid).Value.ToString() <> "" Then
            System.Diagnostics.Process.Start(printUrl & G(G.Columns(GC.uuid).Index, e.RowIndex).Value.ToString() & "/share/" & G(G.Columns(GC.longId).Index, e.RowIndex).Value.ToString())
        ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.PDF).Index AndAlso G.Rows(e.RowIndex).Cells(GC.uuid).Value.ToString() <> "" Then
            tx.GetDocumentPrintout(G(G.Columns(GC.uuid).Index, e.RowIndex).Value.ToString())
        Else
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")
    End Sub

    Private Sub btnNew_Click(sender As Object, e As RoutedEventArgs) Handles btnNew.Click
        FromDate.SelectedDate = bm.MyGetDate
        ToDate.SelectedDate = FromDate.SelectedDate
    End Sub

    Private Sub btnGet_Click(sender As Object, e As RoutedEventArgs) Handles btnGet.Click

        G.Rows.Clear()
        Dim dt As DataTable = bm.ExecuteAdapter("getSalesMaster", {"FromDate", "ToDate"}, {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate)})

        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.IsSelected).Value = False
            G.Rows(i).Cells(GC.Flag).Value = dt.Rows(i)("Flag").ToString
            G.Rows(i).Cells(GC.FlagName).Value = dt.Rows(i)("FlagName").ToString
            G.Rows(i).Cells(GC.StoreId).Value = dt.Rows(i)("StoreId").ToString
            G.Rows(i).Cells(GC.StoreName).Value = dt.Rows(i)("StoreName").ToString
            G.Rows(i).Cells(GC.InvoiceNo).Value = dt.Rows(i)("InvoiceNo").ToString
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.DayDate).Value = dt.Rows(i)("DayDate").ToString
            G.Rows(i).Cells(GC.ToId).Value = dt.Rows(i)("ToId").ToString
            G.Rows(i).Cells(GC.ToName).Value = dt.Rows(i)("ToName").ToString
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty")
            G.Rows(i).Cells(GC.Total).Value = dt.Rows(i)("Total")
            G.Rows(i).Cells(GC.DiscountValue).Value = dt.Rows(i)("DiscountValue")
            G.Rows(i).Cells(GC.TaxAmount).Value = dt.Rows(i)("TaxAmount")
            G.Rows(i).Cells(GC.TotalAfterDiscount).Value = dt.Rows(i)("TotalAfterDiscount")
            G.Rows(i).Cells(GC.CurrencyName).Value = dt.Rows(i)("CurrencyName").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.DocumentDetails).Value = dt.Rows(i)("DocumentDetails").ToString
            G.Rows(i).Cells(GC.uuid).Value = dt.Rows(i)("uuid").ToString
            G.Rows(i).Cells(GC.longId).Value = dt.Rows(i)("longId").ToString
            G.Rows(i).Cells(GC.PDF).Value = "PDF"
            G.Rows(i).Cells(GC.NeedReSubmit).Value = dt.Rows(i)("NeedReSubmit").ToString
        Next

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click


        If Not TestTokenPin() Then Return
        SaveSetting("OMEGA", "TokenPin", "TokenPin", TokenPin.SelectedValue)
        Dim tx As New TaxApi(TokenPin.SelectedValue)
        tx.Login()
        G.EndEdit()
        For i As Integer = 0 To G.Rows.Count - 1

            If G.Rows(i).Cells(GC.IsSelected).Value = False Then
                Continue For
            End If

            If G.Rows(i).Cells(GC.uuid).Value.ToString.Length > 0 AndAlso Val(G.Rows(i).Cells(GC.NeedReSubmit).Value.ToString) = 0 Then
                Continue For
            ElseIf Val(G.Rows(i).Cells(GC.NeedReSubmit).Value.ToString) = 1 Then
                tx.CancelDocument(G.Rows(i).Cells(GC.uuid).Value, "سيتم إعادة رفعها بعد تحديثها")
            End If

            tx.SubmitDocuments(G.Rows(i).Cells(GC.Flag).Value, G.Rows(i).Cells(GC.StoreId).Value, G.Rows(i).Cells(GC.InvoiceNo).Value)

        Next
        btnGet_Click(Nothing, Nothing)

        Threading.Thread.Sleep(3000)

        btnGet2_Click(Nothing, Nothing)
    End Sub

    Private Sub btnGet2_Click(sender As Object, e As RoutedEventArgs) Handles btnGet2.Click
        If Not TestTokenPin() Then Return
        Dim tx As New TaxApi(TokenPin.SelectedValue)
        tx.Login()
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.uuid).Value.ToString.Length > 0 Then
                tx.GetDocumentDetails(G.Rows(i).Cells(GC.uuid).Value)
            End If
        Next
        btnGet_Click(Nothing, Nothing)
        bm.ShowMSG("تمت العملية بنجاح")

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        If G.CurrentRow Is Nothing Then Return
        If Not TestTokenPin() Then Return
        If Notes.Text.Trim = "" Then
            bm.ShowMSG("يجب تحديد سبب الإلغاء")
            Return
        End If
        If Not bm.ShowDeleteMSG("هل أنت متأكد من الإلغاء؟") Then Return
        Dim tx As New TaxApi(TokenPin.SelectedValue)
        tx.Login()
        tx.CancelDocument(G.CurrentRow.Cells(GC.uuid).Value, Notes.Text)
        btnGet_Click(Nothing, Nothing)
        bm.ShowMSG("تمت العملية بنجاح")
    End Sub

    Private Sub btnDelete_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete_Copy.Click
        If G.CurrentRow Is Nothing Then Return
        If Not TestTokenPin() Then Return
        If Notes.Text.Trim = "" Then
            bm.ShowMSG("يجب تحديد سبب الإلغاء")
            Return
        End If
        If Not bm.ShowDeleteMSG("سيتم إلغاء الفاتورة وإعادة الرفع .. استمرار؟") Then Return
        Dim tx As New TaxApi(TokenPin.SelectedValue)
        tx.Login()
        tx.CancelDocument(G.CurrentRow.Cells(GC.uuid).Value, Notes.Text)
        tx.SubmitDocuments(G.CurrentRow.Cells(GC.Flag).Value, G.CurrentRow.Cells(GC.StoreId).Value, G.CurrentRow.Cells(GC.InvoiceNo).Value)
        btnGet_Click(Nothing, Nothing)
        bm.ShowMSG("تمت العملية بنجاح")
    End Sub


    Dim IsSelected As Boolean = False
    Private Sub BtnIsSelected_Click(sender As Object, e As RoutedEventArgs) Handles btnIsSelected.Click
        G.EndEdit()
        IsSelected = Not IsSelected
        For i As Integer = 0 To G.Rows.Count - 1
            G.Rows(i).Cells(GC.IsSelected).Value = IsSelected
        Next
        If IsSelected Then
            btnIsSelected.Content = "إلغاء تحديد الكل"
        Else
            btnIsSelected.Content = "تحديد الكل"
        End If
    End Sub
End Class
