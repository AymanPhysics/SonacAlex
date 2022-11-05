Imports System.Data

Public Class BankCash_G
    Public TableName As String = "BankCash_G"
    Public MainId As String = "BankId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {btnPrint, btnPrint2, btnPrint3})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {MainId, SubId, SubId2, "DayDate", "Canceled", "CurrencyId", "IsPosted"}
        bm.control = New Control() {BankId, txtFlag, txtID, DayDate, Canceled, CurrencyId, IsPosted}
        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName

        LoadWFH()

        If MyLinkFile = 5 Then
            BankId.Text = Md.DefaultSave
            BankId_LostFocus(Nothing, Nothing)
        ElseIf MyLinkFile = 6 Then
            BankId.Text = Md.DefaultBank
            BankId_LostFocus(Nothing, Nothing)
        Else
        End If
        btnNew_Click(sender, e)
        BankId.Focus()
    End Sub



    Structure GC
        Shared Value As String = "Value"
        Shared LinkFile As String = "LinkFile"
        Shared MainAccNo As String = "MainAccNo"
        Shared MainAccName As String = "MainAccName"
        Shared SubAccNo As String = "SubAccNo"
        Shared SubAccName As String = "SubAccName"
        Shared AnalysisId As String = "AnalysisId"
        Shared AnalysisName As String = "AnalysisName"
        Shared CostCenterId As String = "CostCenterId"
        Shared CostCenterName As String = "CostCenterName"
        Shared CostCenterSubId As String = "CostCenterSubId"
        Shared CostCenterSubName As String = "CostCenterSubName"
        Shared CostTypeId As String = "CostTypeId"
        Shared PurchaseAccNo As String = "PurchaseAccNo"
        Shared ImportMessageId As String = "ImportMessageId"
        Shared StoreId As String = "StoreId"
        Shared StoreInvoiceNo As String = "StoreInvoiceNo"
        Shared Notes As String = "Notes"
        Shared DocNo As String = "DocNo"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue
        G.Columns.Add(GC.Value, "القيمة")

        Dim GCLinkFile As New Forms.DataGridViewComboBoxColumn
        GCLinkFile.HeaderText = "الجهة"
        GCLinkFile.Name = GC.LinkFile
        bm.FillCombo("select Id,Name from LinkFile union all select 0 Id,'-' Name order by Id", GCLinkFile)
        G.Columns.Add(GCLinkFile)

        G.Columns.Add(GC.MainAccNo, "الحساب")
        G.Columns.Add(GC.MainAccName, "اسم الحساب")
        G.Columns.Add(GC.SubAccNo, "الفرعى")
        G.Columns.Add(GC.SubAccName, "اسم الفرعى")
        G.Columns.Add(GC.AnalysisId, "التحليلي")
        G.Columns.Add(GC.AnalysisName, "اسم التحليلي")
        G.Columns.Add(GC.CostCenterId, "م. التكلفة")
        G.Columns.Add(GC.CostCenterName, "اسم م. التكلفة")
        G.Columns.Add(GC.CostCenterSubId, "عنصر التكلفة")
        G.Columns.Add(GC.CostCenterSubName, "اسم عنصر التكلفة")

        Dim GCCostTypeId As New Forms.DataGridViewComboBoxColumn
        GCCostTypeId.HeaderText = "نوع التكلفة"
        GCCostTypeId.Name = GC.CostTypeId
        bm.FillCombo("select Id,Name from CostTypes union all select 0 Id,'-' Name order by Id", GCCostTypeId)
        G.Columns.Add(GCCostTypeId)

        G.Columns.Add(GC.PurchaseAccNo, "حساب المشتريات")
        G.Columns.Add(GC.ImportMessageId, "الرسالة")
        G.Columns.Add(GC.StoreId, "المخزن")
        G.Columns.Add(GC.StoreInvoiceNo, "مسلسل الفاتورة")

        G.Columns.Add(GC.Notes, "البيان")
        G.Columns.Add(GC.DocNo, "رقم المستند")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.Value).FillWeight = 100
        G.Columns(GC.MainAccNo).FillWeight = 80
        G.Columns(GC.MainAccName).FillWeight = 200
        G.Columns(GC.SubAccNo).FillWeight = 80
        G.Columns(GC.SubAccName).FillWeight = 200
        G.Columns(GC.AnalysisId).FillWeight = 80
        G.Columns(GC.AnalysisName).FillWeight = 200
        G.Columns(GC.CostCenterId).FillWeight = 80
        G.Columns(GC.CostCenterName).FillWeight = 200
        G.Columns(GC.CostCenterSubId).FillWeight = 80
        G.Columns(GC.CostCenterSubName).FillWeight = 200

        G.Columns(GC.CostTypeId).FillWeight = 120
        G.Columns(GC.Notes).FillWeight = 200
        G.Columns(GC.DocNo).FillWeight = 100

        G.Columns(GC.MainAccName).ReadOnly = True
        G.Columns(GC.SubAccName).ReadOnly = True
        G.Columns(GC.AnalysisName).ReadOnly = True
        G.Columns(GC.CostCenterName).ReadOnly = True
        G.Columns(GC.CostCenterSubName).ReadOnly = True

        If Md.ShowBankCash_GAccNo_Not_LinkFile Then
            G.Columns(GC.LinkFile).Visible = False
            G.Columns(GC.MainAccNo).Visible = True
            If Md.ShowGridAccNames Then
                G.Columns(GC.MainAccName).Visible = True
                MainAccName.Visibility = Windows.Visibility.Hidden
            Else
                G.Columns(GC.MainAccName).Visible = False
                MainAccName.Visibility = Windows.Visibility.Visible
            End If
        Else
            G.Columns(GC.LinkFile).Visible = True
            G.Columns(GC.MainAccNo).Visible = False
            G.Columns(GC.MainAccName).Visible = False
            MainAccName.Visibility = Windows.Visibility.Hidden
        End If

        If Md.ShowGridAccNames Then
            G.Columns(GC.SubAccName).Visible = True
            SubAccName.Visibility = Windows.Visibility.Hidden
        Else
            G.Columns(GC.SubAccName).Visible = False
            SubAccName.Visibility = Windows.Visibility.Visible
        End If

        If Md.ShowAnalysis Then
            G.Columns(GC.AnalysisId).Visible = True
            G.Columns(GC.AnalysisName).Visible = Md.ShowGridAccNames
            AnalysisName.Visibility = IIf(Md.ShowGridAccNames, Windows.Visibility.Hidden, Windows.Visibility.Visible)
        Else
            G.Columns(GC.AnalysisId).Visible = False
            G.Columns(GC.AnalysisName).Visible = False
            AnalysisName.Visibility = Windows.Visibility.Hidden
        End If

        If Md.ShowCostCenter Then
            G.Columns(GC.CostCenterId).Visible = True
            G.Columns(GC.CostCenterName).Visible = Md.ShowGridAccNames
            CostCenterName.Visibility = IIf(Md.ShowGridAccNames, Windows.Visibility.Hidden, Windows.Visibility.Visible)
        Else
            G.Columns(GC.CostCenterId).Visible = False
            G.Columns(GC.CostCenterName).Visible = False
            CostCenterName.Visibility = Windows.Visibility.Hidden
        End If

        If Md.ShowCostCenterSub Then
            G.Columns(GC.CostCenterSubId).Visible = True
            G.Columns(GC.CostCenterSubName).Visible = Md.ShowGridAccNames
            CostCenterSubName.Visibility = IIf(Md.ShowGridAccNames, Windows.Visibility.Hidden, Windows.Visibility.Visible)
        Else
            G.Columns(GC.CostCenterSubId).Visible = False
            G.Columns(GC.CostCenterSubName).Visible = False
            CostCenterSubName.Visibility = Windows.Visibility.Hidden
        End If

        
        G.Columns(GC.CostTypeId).Visible = False
        G.Columns(GC.PurchaseAccNo).Visible = False
        G.Columns(GC.ImportMessageId).Visible = False
        G.Columns(GC.StoreId).Visible = False
        G.Columns(GC.StoreInvoiceNo).Visible = False
        'PurchaseAccName.Visibility = Windows.Visibility.Hidden
        'ImportMessageName.Visibility = Windows.Visibility.Hidden
        'StoreName.Visibility = Windows.Visibility.Hidden


        G.Columns(GC.Line).Visible = False

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.CellBeginEdit, AddressOf G_CellBeginEdit
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True


        bm.FillControls(Me)
        BankId_LostFocus(Nothing, Nothing)

        Dim dt As DataTable = bm.ExecuteAdapter("select *,dbo.GetAccName(MainAccNo)MainAccName,dbo.GetAnalysisName(Analysisid)AnalysisName,dbo.GetCostCenterName(CostCenterId)CostCenterName,dbo.GetCostCenterSubName(CostCenterSubId)CostCenterSubName from " & TableName & " where " & MainId & "=" & BankId.Text & " and " & SubId & "=" & txtFlag.Text.Trim & " and " & SubId2 & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.LinkFile).Value = dt.Rows(i)("LinkFile").ToString
            G.Rows(i).Cells(GC.MainAccNo).Value = dt.Rows(i)("MainAccNo").ToString
            G.Rows(i).Cells(GC.MainAccName).Value = dt.Rows(i)("MainAccName").ToString
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainAccNo).Index, i))
            G.Rows(i).Cells(GC.SubAccNo).Value = dt.Rows(i)("SubAccNo").ToString
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, i))
            G.Rows(i).Cells(GC.AnalysisId).Value = dt.Rows(i)("AnalysisId").ToString
            G.Rows(i).Cells(GC.AnalysisName).Value = dt.Rows(i)("AnalysisName").ToString
            G.Rows(i).Cells(GC.CostCenterId).Value = dt.Rows(i)("CostCenterId").ToString
            G.Rows(i).Cells(GC.CostCenterName).Value = dt.Rows(i)("CostCenterName").ToString
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.CostCenterId).Index, i))
            G.Rows(i).Cells(GC.CostCenterSubId).Value = dt.Rows(i)("CostCenterSubId").ToString
            G.Rows(i).Cells(GC.CostCenterSubName).Value = dt.Rows(i)("CostCenterSubName").ToString
            G.Rows(i).Cells(GC.CostTypeId).Value = dt.Rows(i)("CostTypeId").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
        Next
        DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AllowSave = False
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If
        If Not bm.TestDateValidity(DayDate) Then Return

        G.EndEdit()

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.Value).Value) = 0 Then
                Continue For
            End If

            If Md.ShowBankCash_GAccNo_Not_LinkFile AndAlso Val(G.Rows(i).Cells(GC.MainAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الحساب بالسطر " & (i + 1).ToString)
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.MainAccNo)
                Return
            ElseIf Not G.Rows(i).Cells(GC.SubAccNo).ReadOnly AndAlso Val(G.Rows(i).Cells(GC.SubAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الفرعى بالسطر " & (i + 1).ToString)
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.SubAccNo)
                Return
            End If
        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If


        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}, New String() {"Value", "LinkFile", "MainAccNo", "SubAccNo", "AnalysisId", "CostCenterId", "CostCenterSubId", "CostTypeId", "Notes", "DocNo"}, New String() {GC.Value, GC.LinkFile, GC.MainAccNo, GC.SubAccNo, GC.AnalysisId, GC.CostCenterId, GC.CostCenterSubId, GC.CostTypeId, GC.Notes, GC.DocNo}, New VariantType() {VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.String}, New String() {}) Then Return

        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}) Then Return

        If Not DontClear Then btnNew_Click(sender, e)
        AllowSave = True
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.LinkFile).Value = Nothing
        G.Rows(i).Cells(GC.MainAccNo).Value = Nothing
        G.Rows(i).Cells(GC.MainAccName).Value = Nothing
        G.Rows(i).Cells(GC.SubAccNo).Value = Nothing
        G.Rows(i).Cells(GC.SubAccName).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisId).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisName).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterName).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubName).Value = Nothing
        G.Rows(i).Cells(GC.CostTypeId).Value = Nothing
        G.Rows(i).Cells(GC.Notes).Value = Nothing
        G.Rows(i).Cells(GC.DocNo).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Value Then
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainAccNo Then
                'bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MainAccNo), MainAccName)
                bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MainAccNo), G.Rows(e.RowIndex).Cells(GC.MainAccName))
                MainAccName.Content = G.Rows(e.RowIndex).Cells(GC.MainAccName).Value
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "')")
                If dt.Rows.Count = 0 Then
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = True
                Else
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                End If
                GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, e.RowIndex))
                'G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.SubAccNo Then
                If Md.ShowBankCash_GAccNo_Not_LinkFile Then
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "')")
                    If dt.Rows.Count > 0 Then
                        'bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & " and AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "'")
                        bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.SubAccName), "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value) & " and AccNo='" & G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value & "'")
                        SubAccName.Content = G.Rows(e.RowIndex).Cells(GC.SubAccName).Value
                        'G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                    Else
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.SubAccName).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = True
                        SubAccName.Content = ""
                    End If
                Else
                    G.Rows(e.RowIndex).Cells(GC.SubAccNo).ReadOnly = False
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & G.Rows(e.RowIndex).Cells(GC.LinkFile).Value)
                    If dt.Rows.Count > 0 Then
                        bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.SubAccNo), G.Rows(e.RowIndex).Cells(GC.SubAccName), "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value))
                        SubAccName.Content = G.Rows(e.RowIndex).Cells(GC.SubAccName).Value
                    Else
                        G.Rows(e.RowIndex).Cells(GC.SubAccNo).Value = ""
                        G.Rows(e.RowIndex).Cells(GC.SubAccName).Value = ""
                        SubAccName.Content = ""
                    End If
                End If
                CalcBalSub(e.RowIndex)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.AnalysisId Then
                'bm.AnalysisIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.AnalysisId), AnalysisName)
                bm.AnalysisIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.AnalysisId), G.Rows(e.RowIndex).Cells(GC.AnalysisName))
                AnalysisName.Content = G.Rows(e.RowIndex).Cells(GC.AnalysisName).Value
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CostCenterId Then
                'bm.CostCenterIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterId), CostCenterName)
                bm.CostCenterIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterId), G.Rows(e.RowIndex).Cells(GC.CostCenterName))
                CostCenterName.Content = G.Rows(e.RowIndex).Cells(GC.CostCenterName).Value
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CostCenterSubId Then
                'bm.CostCenterSubIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterSubId), CostCenterSubName)
                bm.CostCenterSubIdLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.CostCenterSubId), G.Rows(e.RowIndex).Cells(GC.CostCenterSubName))
                CostCenterSubName.Content = G.Rows(e.RowIndex).Cells(GC.CostCenterSubName).Value
            ElseIf G.Columns(e.ColumnIndex).Name = GC.PurchaseAccNo Then
                'bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo), PurchaseAccName)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.PurchaseAccNo Then
                'bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo), PurchaseAccName)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ImportMessageId Then
                'bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.ImportMessageId), ImportMessageName, "select dbo.GetAccName(AccNo) from ImportMessages where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.ImportMessageId).Value))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.StoreId Then
                'bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.StoreId), StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.StoreId).Value))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.StoreInvoiceNo Then
                If Not G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = Nothing AndAlso Not bm.IF_Exists("select InvoiceNo from SalesMaster where StoreId=" & G.CurrentRow.Cells(GC.StoreId).Value & " and Flag=" & Sales.FlagState.الاستيراد & " and InvoiceNo=" & G.CurrentRow.Cells(GC.StoreInvoiceNo).Value) Then
                    bm.ShowMSG("هذا الرقم غير صحيح")
                    G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = Nothing
                    Exit Sub
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CostTypeId Then
                Select Case G.Rows(e.RowIndex).Cells(GC.CostTypeId).Value
                    Case 1
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                    Case 2
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                    Case 3
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                    Case 4
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = False
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = False
                    Case Else
                        G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly = True
                        G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly = True
                End Select

                If G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.PurchaseAccNo).Value = ""
                If G.Rows(e.RowIndex).Cells(GC.ImportMessageId).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.ImportMessageId).Value = ""
                If G.Rows(e.RowIndex).Cells(GC.StoreId).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.StoreId).Value = ""
                If G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).ReadOnly Then G.Rows(e.RowIndex).Cells(GC.StoreInvoiceNo).Value = ""

            End If


            CalcTotal()
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
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

    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True


        DayDate.SelectedDate = bm.MyGetDate()
        G.Rows.Clear()
        CalcTotal()

        CurrentBal.Content = 0

        MainAccName.Content = ""
        SubAccName.Content = ""
        AnalysisName.Content = ""
        CostCenterName.Content = ""
        CostCenterSubName.Content = ""

        'ImportMessageName.Content = ""
        'StoreName.Content = ""
        Value.Clear()
        BankId_LostFocus(Nothing, Nothing)
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

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
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
        bm.LostFocus(BankId, BankName, "select Name from Fn_EmpPermissions(" & MyLinkFile & "," & Md.UserName & ") where Id=" & BankId.Text.Trim())
        Try
            If dt.Rows.Count > 0 Then
                CurrencyId.SelectedValue = Val(bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim()))
            Else
                CurrencyId.SelectedValue = 1
            End If
        Catch
        End Try

        CurrentBalMain.Content = ""
        If Val(BankId.Text) = 0 Then
            CurrentBalMain.Content = "0"
        Else
            If Not lop Then
                'CurrentBalMain.Content = bm.ExecuteScalar("select dbo.Bal0Link('" & MyLinkFile & "','" & Val(BankId.Text) & "','" & bm.ToStrDate(DayDate.SelectedDate) & "',0)")
            End If
        End If
        

        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MyLinkFile & "," & Md.UserName & ")") Then
            BankId_LostFocus(Nothing, Nothing)
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

        lblBank.SetResourceReference(Label.ContentProperty, "Bank")
        If MyLinkFile = 5 Then lblBank.SetResourceReference(Label.ContentProperty, "Safe")

        lblDayDate.SetResourceReference(Label.ContentProperty, "DayDate")
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            Value.Text = Math.Round(0, 2)
            For i As Integer = 0 To G.Rows.Count - 1
                Value.Text += Val(G.Rows(i).Cells(GC.Value).Value)
            Next

            LopCalc = False
        Catch ex As Exception
        End Try
    End Sub


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        'e.Handled = True
        If G.CurrentCell Is Nothing Then Return
        If G.CurrentCell.ReadOnly Then Return
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.MainAccNo).Index Then
                If bm.AccNoShowHelpGrid(G.CurrentRow.Cells(GC.MainAccNo), G.CurrentRow.Cells(GC.MainAccName), e, 1) Then
                    MainAccName.Content = G.CurrentRow.Cells(GC.MainAccNo).Value
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.SubAccNo).Index Then
                If Md.ShowBankCash_GAccNo_Not_LinkFile Then
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=(select C.LinkFile from Chart C where C.Id='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainAccNo).Value & "')")
                    If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName), e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where AccNo='" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainAccNo).Value & "'") Then
                        SubAccName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName).Value
                        'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentCell.RowIndex))
                        If G.Columns(GC.AnalysisId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId)
                        ElseIf G.Columns(GC.CostCenterId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                        Else
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                        End If
                    End If
                Else
                    dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & G.Rows(G.CurrentCell.RowIndex).Cells(GC.LinkFile).Value)

                    If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccNo), SubAccName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
                        SubAccName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.SubAccName).Value
                        'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentCell.RowIndex))
                        If G.Columns(GC.AnalysisId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId)
                        ElseIf G.Columns(GC.CostCenterId).Visible Then
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                        Else
                            G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                        End If
                    End If
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.AnalysisId).Index Then
                If bm.ShowHelpGrid("Analysis", G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId), G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisName), e, "select cast(Id as varchar(100)) Id,Name from Analysis") Then
                    AnalysisName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisName).Value
                    If G.Columns(GC.CostCenterId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId)
                    Else
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                    End If
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.CostCenterId).Index Then
                If bm.ShowHelpGrid("CostCenters", G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterId), CostCenterName, e, "select cast(Id as varchar(100)) Id,Name from CostCenters where SubType=1") Then
                    If G.Columns(GC.CostCenterSubId).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubId)
                    Else
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                    End If
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.CostCenterSubId).Index Then
                If bm.ShowHelpGrid("CostCenterSubs", G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubId), G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubName), e, "select cast(Id as varchar(100)) Id,Name from CostCenterSubs") Then
                    CostCenterSubName.Content = G.Rows(G.CurrentCell.RowIndex).Cells(GC.CostCenterSubName).Value
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.PurchaseAccNo).Index Then
                'If bm.AccNoShowHelpGrid(G.CurrentRow.Cells(GC.PurchaseAccNo), PurchaseAccName, e, 1, , True) Then
                '    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.ImportMessageId)
                'End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.ImportMessageId).Index Then
                'If bm.ShowHelpGrid("ImportMessages", G.Rows(G.CurrentCell.RowIndex).Cells(GC.ImportMessageId), ImportMessageName, e, "select cast(Id as varchar(100)) Id,dbo.GetAccName(AccNo) Name from ImportMessages") Then
                '    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreId)
                'End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.StoreId).Index Then
                'If bm.ShowHelpGrid("Stores", G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreId), StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
                '    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.StoreInvoiceNo)
                'End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.StoreInvoiceNo).Index Then
                If bm.ShowHelpGrid("الفواتير", G.CurrentRow.Cells(GC.StoreInvoiceNo), G.CurrentRow.Cells(GC.StoreInvoiceNo), e, "select cast(InvoiceNo as varchar(100)) Id,dbo.GetSupplierName(ToId) Name from SalesMaster where StoreId=" & G.CurrentRow.Cells(GC.StoreId).Value & " and Flag=" & Sales.FlagState.الاستيراد, , "الفاتورة", "المورد") Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                End If
            End If
        Catch ex As Exception
        End Try
        G.CommitEdit(Forms.DataGridViewDataErrorContexts.Commit)
    End Sub


    Private Sub G_CellBeginEdit(sender As Object, e As Forms.DataGridViewCellCancelEventArgs)
        If CType(G.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If
        If CType(G.Rows(e.RowIndex).Cells(GC.CostTypeId), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G.Rows(e.RowIndex).Cells(GC.CostTypeId), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        If lop Then Return

        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.LinkFile).Index, G.CurrentRow.Index))

        'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.SubAccNo).Index, G.CurrentRow.Index))
        'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.CostCenterId).Index, G.CurrentRow.Index))

        MainAccName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.MainAccName).Value
        SubAccName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.SubAccName).Value
        AnalysisName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.AnalysisName).Value
        CostCenterName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.CostCenterName).Value
        CostCenterSubName.Content = G.Rows(G.CurrentRow.Index).Cells(GC.CostCenterSubName).Value

        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.PurchaseAccNo).Index, G.CurrentRow.Index))
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ImportMessageId).Index, G.CurrentRow.Index))
        GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.StoreId).Index, G.CurrentRow.Index))
        CalcBalSub(G.CurrentRow.Index)
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click, btnPrint2.Click, btnPrint3.Click
        DontClear = True
        If btnSave.IsEnabled AndAlso btnSave.Visibility = Windows.Visibility.Visible Then
            btnSave_Click(sender, e)
        Else
            AllowSave = True
        End If
        DontClear = False
        If Not AllowSave Then Return

        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@BankId", "@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {Val(BankId.Text), Flag, txtID.Text, CType(Parent, Page).Title}
        If sender Is btnPrint Then
            rpt.Rpt = "BankCash_G1.rpt"
            rpt.Show()
        ElseIf sender Is btnPrint2 Then
            rpt.Rpt = "BankCash_G12.rpt"
            rpt.Print()
        ElseIf sender Is btnPrint3 Then
            rpt.Rpt = "BankCash_G13.rpt"
            If Flag = 2 Then rpt.Rpt = "BankCash_G14.rpt"
            rpt.Show()
        End If
    End Sub

    Private Sub CalcBalSub(i As Integer)
        If Not lop AndAlso Val(G.Rows(i).Cells(GC.SubAccNo).Value) > 0 Then
            Select Case G.Rows(i).Cells(GC.LinkFile).Value
                Case 1, 13
                    CurrentBal.Content = bm.ExecuteScalar("select dbo.Bal0Link('" & G.Rows(i).Cells(GC.LinkFile).Value & "','" & G.Rows(i).Cells(GC.SubAccNo).Value & "','" & bm.ToStrDate(DayDate.SelectedDate) & "',0)")
                Case Else
                    CurrentBal.Content = bm.ExecuteScalar("select dbo.Bal0Link('" & G.Rows(i).Cells(GC.LinkFile).Value & "','" & G.Rows(i).Cells(GC.SubAccNo).Value & "','" & bm.ToStrDate(DayDate.SelectedDate) & "',0) from Fn_EmpPermissions(" & G.Rows(i).Cells(GC.LinkFile).Value & "," & Md.UserName & ") WHERE Id='" & G.Rows(i).Cells(GC.SubAccNo).Value & "'")
            End Select
        Else
            CurrentBal.Content = ""
        End If
    End Sub

End Class
