Imports System.Data

Public Class BankCash_G2
    Public TableName As String = "BankCash_G2"
    Public TableName2 As String = "BankCash_G22"
    Public MainId As String = "BankCash_G2TypeId"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Dim bm2 As New BasicMethods

    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid
    Public Flag As Integer = 0
    Public FlagSub As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {btnPrint, btnPrint2})
        GroupBoxPaymentType.Visibility = Windows.Visibility.Hidden
        If Not Md.Manager Then
            'btnDelete.Visibility = Windows.Visibility.Hidden
            'btnFirst.Visibility = Windows.Visibility.Hidden
            'btnPrevios.Visibility = Windows.Visibility.Hidden
            'btnNext.Visibility = Windows.Visibility.Hidden
            'btnLast.Visibility = Windows.Visibility.Hidden

            'btnPrint.Visibility = Windows.Visibility.Hidden
            'btnPrint2.Visibility = Windows.Visibility.Hidden
            DayDate.IsEnabled = False
        End If

        lblNotes.Visibility = Windows.Visibility.Hidden
        Notes.Visibility = Windows.Visibility.Hidden
        GroupBoxDed.Visibility = Windows.Visibility.Hidden
        btnPrint2.Visibility = Windows.Visibility.Hidden
        bm.Addcontrol_MouseDoubleClick({CheckNo, CheckBankId})

        btnChangeCheckNo.Visibility = Windows.Visibility.Hidden

        bm.Fields = New String() {MainId, SubId, SubId2, "MainLinkFile", "BankId", "DayDate", "Canceled", "CurrencyId", "SystemUser", "IsPosted", "DocNo"}
        bm.control = New Control() {BankCash_G2TypeId, txtFlag, txtID, MainLinkFile, BankId, DayDate, Canceled, CurrencyId, SystemUser, IsPosted, DocNo}
        bm.KeyFields = New String() {MainId, SubId, SubId2}
        bm.Table_Name = TableName

        txtFlag.Text = Flag

        bm2.Fields = bm.Fields
        bm2.control = bm.control
        bm2.KeyFields = bm.KeyFields
        bm2.Table_Name = TableName2

        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        LoadResource()
        LoadWFH(WFH1, G1)
        LoadWFH(WFH2, G2)

        Dim x As Integer = Val(GetSetting("SonacAlex", "BankCash_G2TypeId", Flag))
        bm.FillCombo("GetEmpBankCash_G2Types @Flag=" & Flag & ",@EmpId=" & Md.UserName & "", BankCash_G2TypeId)
        BankCash_G2TypeId.SelectedValue = x

        bm.FillCombo("CheckTypes", CheckTypeId, "", , True, True)
        bm.FillCombo("LinkFile", MainLinkFile, "", , True)
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        bm.FillCombo("Employees", SystemUser, "", , True)

        If Md.MyProjectType = ProjectType.SonacAlex Then
            WFH1.Margin = New Thickness(WFH1.Margin.Left, WFH1.Margin.Top, WFH1.Margin.Right, WFH1.Margin.Bottom + WFH2.Height)
        Else
            WFH2.Visibility = Windows.Visibility.Hidden
        End If

        SaveSetting("SonacAlex", "BankCash_G2TypeId", Flag, x)
        btnNew_Click(sender, e)

        If Md.MyProjectType = ProjectType.SonacAlex Then
            CurrencyId.IsEnabled = True
            btnLast_Click(sender, e)
        Else
            btnEdit.Visibility = Windows.Visibility.Hidden
        End If


        If Flag = 2 Then
            lblCheckBankId.Visibility = Windows.Visibility.Hidden
            CheckBankId.Visibility = Windows.Visibility.Hidden
            CheckBankName.Visibility = Windows.Visibility.Hidden
        End If

        Select Case FlagSub
            Case 1
                BankCash_G2TypeId.SelectedValue = 1
                MainLinkFile.SelectedValue = 5
                BankId.Text = 1
            Case 2
                BankCash_G2TypeId.SelectedValue = 3
                MainLinkFile.SelectedValue = 5
                BankId.Text = 2
            Case 3
                BankCash_G2TypeId.SelectedValue = 1
                MainLinkFile.SelectedValue = 5
                BankId.Text = 3
            Case 4
                BankCash_G2TypeId.SelectedValue = 1
                MainLinkFile.SelectedValue = 5
                BankId.Text = 4
            Case 5
                BankCash_G2TypeId.SelectedValue = 2
                MainLinkFile.SelectedValue = 6
                BankId.Text = 0
        End Select
        BankId_LostFocus(Nothing, Nothing)
        BankId.Focus()
    End Sub



    Structure GC
        Shared DocNo As String = "DocNo"
        Shared MainValue As String = "MainValue"
        Shared Exchange As String = "Exchange"
        Shared Value As String = "Value"
        Shared LinkFile As String = "LinkFile"
        Shared MainAccNo As String = "MainAccNo"
        Shared MainAccName As String = "MainAccName"

        Shared CurrencyId2 As String = "CurrencyId2"
        Shared MainValue2 As String = "MainValue2"
        Shared Exchange2 As String = "Exchange2"

        Shared AnalysisId As String = "AnalysisId"
        Shared CostCenterId As String = "CostCenterId"
        Shared CostCenterSubId As String = "CostCenterSubId"
        
        Shared Notes As String = "Notes"
        Shared CheckTypeId As String = "CheckTypeId"
        Shared CheckNo As String = "CheckNo"
        Shared CheckDate As String = "CheckDate"
        Shared CheckBankId As String = "CheckBankId"
        Shared CheckNotes As String = "CheckNotes"

        Shared Line As String = "Line"
        Shared StatusId As String = "StatusId"
        Shared StatusValue As String = "StatusValue"
        Shared IsDocumented As String = "IsDocumented"
        Shared IsNotDocumented As String = "IsNotDocumented"
        Shared SalesDocNo As String = "SalesDocNo"
        Shared SalesTypeId As String = "SalesTypeId"
    End Structure


    Private Sub LoadWFH(WFH As Forms.Integration.WindowsFormsHost, G As MyGrid)
        WFH.Child = G

        G.RowHeadersWidth = 4
        G.RowHeadersWidthSizeMode = Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing

        G.ColumnHeadersHeightSizeMode = Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        
        
        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.DocNo, "رقم المستند")
        G.Columns.Add(GC.MainValue, "القيمة")
        G.Columns.Add(GC.Exchange, "سعر الصرف")
        G.Columns.Add(GC.Value, "المعادل بالعملة المحلية")

        Dim GCLinkFile As New Forms.DataGridViewComboBoxColumn
        GCLinkFile.HeaderText = "الجهة"
        GCLinkFile.Name = GC.LinkFile
        bm.FillCombo("select Id,Name from LinkFile union all select 0 Id,'-' Name order by Id", GCLinkFile)
        G.Columns.Add(GCLinkFile)

        If Md.MyProjectType = ProjectType.SonacAlex Then
            G.RowHeadersWidth = 4
            G.RowHeadersWidthSizeMode = Forms.DataGridViewRowHeadersWidthSizeMode.EnableResizing
        End If
        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None


        Dim GCSalesTypeId As New Forms.DataGridViewComboBoxColumn
        GCSalesTypeId.HeaderText = "الصنف"
        GCSalesTypeId.Name = GC.SalesTypeId
        bm.FillCombo("select Id,Name from SalesTypes union all select 0 Id,'-' Name order by Id", GCSalesTypeId)
        G.Columns.Add(GCSalesTypeId)
        G.Columns.Add(GC.SalesDocNo, "رقم الفاتورة")

        G.Columns.Add(GC.MainAccNo, "الحساب")
        'bm.MakeGridCombo(G, "الحساب", GC.MainAccNo, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id")
        G.Columns.Add(GC.MainAccName, "اسم الحساب")


        Dim GCCurrencyId2 As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId2.HeaderText = "عملة الفرعي"
        GCCurrencyId2.Name = GC.CurrencyId2
        bm.FillCombo("select Id,Name from Currencies order by Id", GCCurrencyId2)
        G.Columns.Add(GCCurrencyId2)
        G.Columns.Add(GC.MainValue2, "القيمة لدى الفرعي")
        G.Columns.Add(GC.Exchange2, "سعر الصرف بالعملة المحلية")

        bm.MakeGridCombo(G, "اسم الشركة", GC.AnalysisId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis where SubType=1 union all select 0 Id,'-' Name order by Id")
        
        bm.MakeGridCombo(G, "م. التكلفة", GC.CostCenterId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenters where SubType=1 union all select 0 Id,'-' Name order by Id")
        
        bm.MakeGridCombo(G, "عنصر التكلفة", GC.CostCenterSubId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenterSubs where SubType=1 union all select 0 Id,'-' Name order by Id")
        
        G.Columns.Add(GC.Notes, "البيان")

        bm.MakeGridCombo(G, "حالة الشيك", GC.CheckTypeId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from CheckTypes union all select 0 Id,'-' Name order by Id")
        G.Columns.Add(GC.CheckNo, IIf(FlagSub = 5, "رقم الشيك", "رقم الربط"))
        G.Columns.Add(GC.CheckDate, "تاريخ الاستحقاق")
        bm.MakeGridCombo(G, "البنك", GC.CheckBankId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from CheckBanks union all select 0 Id,'-' Name order by Id")
        G.Columns.Add(GC.CheckNotes, "المستفيد")
        G.Columns.Add(GC.Line, "Line")

        bm.MakeGridCombo(G, "الحالة", GC.StatusId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Status union all select 0 Id,'-' Name order by Id")

        G.Columns.Add(GC.StatusValue, "قيمة الحالة")

        G.Columns.Add(GC.IsDocumented, "مؤيد")
        G.Columns.Add(GC.IsNotDocumented, "غير مؤيد")

        G.Columns(GC.DocNo).Visible = False

        'G.Columns(GC.CheckTypeId).Visible = False
        'G.Columns(GC.CheckNo).Visible = False
        'G.Columns(GC.CheckDate).Visible = False
        'G.Columns(GC.CheckBankId).Visible = False
        'G.Columns(GC.CheckNotes).Visible = False

        G.Columns(GC.Value).ReadOnly = True

        If Flag = 2 Then
            G.Columns(GC.SalesTypeId).Visible = False
            G.Columns(GC.SalesDocNo).Visible = False
        End If

        If Md.MyProjectType = ProjectType.SonacAlex Then
            G.Columns(GC.Value).ReadOnly = False
        Else
            G.Columns(GC.IsDocumented).Visible = False
            G.Columns(GC.IsNotDocumented).Visible = False
        End If

        SetGCExchange(G)

        If Not Md.MyProjectType = ProjectType.SonacAlex Then
            G.Columns(GC.CurrencyId2).ReadOnly = True
            G.Columns(GC.Exchange2).ReadOnly = True
        End If

        G.Columns(GC.CurrencyId2).FillWeight = 100

        G.Columns(GC.MainAccNo).MinimumWidth = 80
        G.Columns(GC.MainAccName).MinimumWidth = 200
        'G.Columns(GC.Notes).FillWeight = 80
        G.Columns(GC.Exchange).MinimumWidth = 40
        G.Columns(GC.MainValue).MinimumWidth = 80
        G.Columns(GC.Value).MinimumWidth = 80
        G.Columns(GC.SalesTypeId).MinimumWidth = 40
        G.Columns(GC.SalesDocNo).MinimumWidth = 40
        G.Columns(GC.CheckNotes).MinimumWidth = 40
        G.Columns(GC.StatusValue).MinimumWidth = 40
        G.Columns(GC.IsDocumented).MinimumWidth = 40
        G.Columns(GC.IsNotDocumented).MinimumWidth = 50
        G.Columns(GC.Notes).MinimumWidth = 200
        'G.Columns(GC.DocNo).FillWeight = 60
        G.Columns(GC.AnalysisId).MinimumWidth = 150
        G.Columns(GC.CostCenterId).MinimumWidth = 150
        G.Columns(GC.CostCenterSubId).MinimumWidth = 150
        G.Columns(GC.CheckNo).MinimumWidth = 70
        G.Columns(GC.CheckNotes).MinimumWidth = 150
        G.Columns(GC.CheckDate).MinimumWidth = 70


        G.Columns(GC.CheckBankId).FillWeight = 150
        G.Columns(GC.CheckNotes).FillWeight = 150
        G.Columns(GC.Exchange).FillWeight = 40

        If Md.MyProjectType = ProjectType.SonacAlex Then
            G.Columns(GC.CurrencyId2).Visible = False
            G.Columns(GC.MainValue2).Visible = False
            G.Columns(GC.Exchange2).Visible = False
        End If

        If Md.ShowBankCash_GAccNo_Not_LinkFile Then
            G.Columns(GC.LinkFile).Visible = False
            G.Columns(GC.MainAccNo).Visible = True
        Else
            G.Columns(GC.LinkFile).Visible = True
            G.Columns(GC.MainAccNo).Visible = False
        End If

        If Not Md.ShowCurrency Then
            G.Columns(GC.CurrencyId2).Visible = False
            G.Columns(GC.Exchange).Visible = False
            G.Columns(GC.Exchange2).Visible = False
            G.Columns(GC.Value).Visible = False
            G.Columns(GC.MainValue2).Visible = False
        End If

        If Md.ShowAnalysis Then
            G.Columns(GC.AnalysisId).Visible = True
        Else
            G.Columns(GC.AnalysisId).Visible = False
        End If

        If Md.ShowCostCenter Then
            G.Columns(GC.CostCenterId).Visible = True
        Else
            G.Columns(GC.CostCenterId).Visible = False
        End If

        If Md.ShowCostCenterSub Then
            G.Columns(GC.CostCenterSubId).Visible = True
        Else
            G.Columns(GC.CostCenterSubId).Visible = False
        End If

        G.Columns(GC.MainAccName).ReadOnly = True


        G.Columns(GC.CheckTypeId).Visible = False

        G.Columns(GC.Line).Visible = False
        If FlagSub <> 5 Then
            G.Columns(GC.CheckDate).Visible = False
            G.Columns(GC.CheckBankId).Visible = False
        End If

        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        For i As Integer = 0 To G.ColumnCount - 1
            G.Columns(i).MinimumWidth = G.Columns(i).FillWeight
        Next

        bm.AddThousandSeparator(G.Columns(GC.MainValue))
        bm.AddThousandSeparator(G.Columns(GC.Value))
        bm.AddThousandSeparator(G.Columns(GC.StatusValue))
        bm.AddThousandSeparator(G.Columns(GC.IsDocumented))
        bm.AddThousandSeparator(G.Columns(GC.IsNotDocumented))

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.RowsAdded, AddressOf G_RowsAdded
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.CellBeginEdit, AddressOf G_CellBeginEdit
        AddHandler G.SelectionChanged, AddressOf G_SelectionChanged
        AddHandler G.GotFocus, AddressOf G_GotFocus
    End Sub

    Dim CurrentGrid As MyGrid
    Private Sub G_GotFocus(sender As Object, e As EventArgs)
        CurrentGrid = sender
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim IsNew As Boolean = False
    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True

        bm.FillControls(Me)
        BankId_LostFocus(Nothing, Nothing)

        FillGrid(G1, TableName)
        FillGrid(G2, TableName2)

        DayDate.Focus()
        lop = False
        CalcTotal()

        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If

        DisableControls()

        G_SelectionChanged(G1, Nothing)
    End Sub

    Sub FillGrid(G As MyGrid, MyTableName As String)

        Dim dt As DataTable = bm.ExecuteAdapter("select T.*,/*dbo.GetAccName(MainAccNo)*/ Ch.Name MainAccName/*,dbo.GetAnalysisName(Analysisid)AnalysisName,dbo.GetCostCenterName(CostCenterId)CostCenterName,dbo.GetCostCenterSubName(CostCenterSubId)CostCenterSubName*/ from " & MyTableName & " T left join Chart Ch on(Ch.Id=T.MainAccNo) where T." & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and T." & SubId & "=" & txtFlag.Text.Trim & " and T." & SubId2 & "=" & txtID.Text & " order by T.Line")

        IsNew = True
        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            IsNew = False
            G.Rows.Add()
            G.Rows(i).Cells(GC.MainValue).Value = dt.Rows(i)("MainValue")
            G.Rows(i).Cells(GC.Exchange).Value = dt.Rows(i)("Exchange").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value")
            G.Rows(i).Cells(GC.LinkFile).Value = dt.Rows(i)("LinkFile").ToString
            G.Rows(i).Cells(GC.MainAccNo).Value = dt.Rows(i)("MainAccNo").ToString
            lop = False
            'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainAccNo).Index, i))
            G.Rows(i).Cells(GC.MainAccNo).Value = dt.Rows(i)("MainAccNo").ToString
            G.Rows(i).Cells(GC.MainAccName).Value = dt.Rows(i)("MainAccName").ToString
            lop = True
            G.Rows(i).Cells(GC.AnalysisId).Value = dt.Rows(i)("AnalysisId").ToString
            G.Rows(i).Cells(GC.CostCenterId).Value = dt.Rows(i)("CostCenterId").ToString
            G.Rows(i).Cells(GC.CostCenterSubId).Value = dt.Rows(i)("CostCenterSubId").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.SalesTypeId).Value = dt.Rows(i)("SalesTypeId").ToString
            G.Rows(i).Cells(GC.CheckTypeId).Value = dt.Rows(i)("CheckTypeId").ToString
            G.Rows(i).Cells(GC.CheckNo).Value = dt.Rows(i)("CheckNo").ToString
            G.Rows(i).Cells(GC.CheckDate).Value = bm.ToStrDate(dt.Rows(i)("CheckDate").ToString)
            G.Rows(i).Cells(GC.CheckBankId).Value = dt.Rows(i)("CheckBankId").ToString
            G.Rows(i).Cells(GC.CheckNotes).Value = dt.Rows(i)("CheckNotes").ToString

            G.Rows(i).Cells(GC.CurrencyId2).Value = dt.Rows(i)("CurrencyId2").ToString
            G.Rows(i).Cells(GC.MainValue2).Value = dt.Rows(i)("MainValue2").ToString
            G.Rows(i).Cells(GC.Exchange2).Value = dt.Rows(i)("Exchange2").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line").ToString
            G.Rows(i).Cells(GC.StatusId).Value = dt.Rows(i)("StatusId").ToString
            G.Rows(i).Cells(GC.StatusValue).Value = dt.Rows(i)("StatusValue")
            G.Rows(i).Cells(GC.IsDocumented).Value = dt.Rows(i)("IsDocumented").ToString
            G.Rows(i).Cells(GC.IsNotDocumented).Value = dt.Rows(i)("IsNotDocumented").ToString
            G.Rows(i).Cells(GC.SalesTypeId).Value = dt.Rows(i)("SalesTypeId").ToString
            G.Rows(i).Cells(GC.SalesDocNo).Value = dt.Rows(i)("SalesDocNo").ToString


        Next
        Try
            If G.Rows(dt.Rows.Count - 1).Cells(GC.MainAccNo).Visible Then
                G.CurrentCell = G.Rows(dt.Rows.Count - 1).Cells(GC.MainAccNo)
                G.RefreshEdit()
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not btnSave.IsEnabled Then Return

        AllowSave = False
        If BankCash_G2TypeId.SelectedValue Is Nothing OrElse BankCash_G2TypeId.SelectedValue < 1 Then
            bm.ShowMSG("برجاء تحديد اليومية")
            BankCash_G2TypeId.Focus()
            Return
        End If
        If Val(txtID.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المسلسل")
            txtID.Focus()
            Return
        End If
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If

        If DocNo.Text.Trim = "" Then
            bm.ShowMSG("برجاء تحديد رقم المستند")
            DocNo.Focus()
            Return
        End If

        G1.EndEdit()
        G2.EndEdit()

        For i As Integer = 0 To G1.Rows.Count - 2
            'If Val(G1.Rows(i).Cells(GC.MainValue).Value) = 0 Then
            '    Continue For
            'End If

            If Val(G1.Rows(i).Cells(GC.Value).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد القيمة بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.Value)
                Return
            End If

            If Md.ShowBankCash_GAccNo_Not_LinkFile AndAlso Val(G1.Rows(i).Cells(GC.MainAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الحساب بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.MainAccNo)
                Return
            End If
            If G1.Rows(i).Cells(GC.Notes).Value Is Nothing OrElse G1.Rows(i).Cells(GC.Notes).Value.ToString.Trim = "" Then
                bm.ShowMSG("برجاء تحديد البيان بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.Notes)
                Return
            End If
            If Not G1.Rows(i).Cells(GC.CheckNo).Value Is Nothing AndAlso Not TestCheck(i) Then
                bm.ShowMSG("مبلغ الشيك " & G1.Rows(i).Cells(GC.CheckNo).Value.ToString & " غير صحيح")
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.CheckNo)
                Return
            End If
            If Val(G1.Rows(i).Cells(GC.CheckNo).Value) = 0 AndAlso Val(G1.Rows(i).Cells(GC.CheckTypeId).Value) <> 3 AndAlso Val(G1.Rows(i).Cells(GC.CheckTypeId).Value) > 1 Then
                bm.ShowMSG("برجاء تحديد رقم الشيك بالسطر " & (i + 1).ToString)
                G1.Focus()
                G1.CurrentCell = G1.Rows(i).Cells(GC.Notes)
                Try
                    G1.CurrentCell = G1.Rows(i).Cells(GC.DocNo)
                Catch ex As Exception
                End Try
                Return
            End If
        Next


        For i As Integer = 0 To G2.Rows.Count - 2
            'If Val(G2.Rows(i).Cells(GC.MainValue).Value) = 0 Then
            '    Continue For
            'End If

            If Md.ShowBankCash_GAccNo_Not_LinkFile AndAlso Val(G2.Rows(i).Cells(GC.MainAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد الحساب بالسطر " & (i + 1).ToString)
                G2.Focus()
                G2.CurrentCell = G2.Rows(i).Cells(GC.MainAccNo)
                Return
            End If
            If G2.Rows(i).Cells(GC.Notes).Value Is Nothing OrElse G2.Rows(i).Cells(GC.Notes).Value.ToString.Trim = "" Then
                bm.ShowMSG("برجاء تحديد البيان بالسطر " & (i + 1).ToString)
                G2.Focus()
                G2.CurrentCell = G2.Rows(i).Cells(GC.Notes)
                Return
            End If

            If G2.Rows(i).Cells(GC.CheckNo).Value <> "" Then
                For i2 As Integer = 0 To G1.Rows.Count - 1
                    If G2.Rows(i).Cells(GC.CheckNo).Value = G1.Rows(i2).Cells(GC.CheckNo).Value Then
                        GoTo A
                    End If
                Next
                bm.ShowMSG("برجاء تحديد رقم شيك صحيح بالسطر " & (i + 1).ToString)
                G2.Focus()
                G2.CurrentCell = G2.Rows(i).Cells(GC.CheckNo)
                Return
A:
            End If
        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If



        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.Update
        'If IsNew Then State = BasicMethods.SaveState.Insert

        SystemUser.SelectedValue = Md.UserName
        bm.DefineValues()
        bm2.DefineValues()

        If Not bm.SaveGrid(G1, TableName, New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, New String() {"MainValue", "Exchange", "Value", "LinkFile", "MainAccNo", "AnalysisId", "CostCenterId", "CostCenterSubId", "Notes", "DocNo", "CheckTypeId", "CheckNo", "CheckNotes", "CheckDate", "CheckBankId", "CurrencyId2", "MainValue2", "Exchange2", "IsDocumented", "IsNotDocumented", "SalesTypeId", "SalesDocNo", "StatusId", "StatusValue"}, New String() {GC.MainValue, GC.Exchange, GC.Value, GC.LinkFile, GC.MainAccNo, GC.AnalysisId, GC.CostCenterId, GC.CostCenterSubId, GC.Notes, GC.DocNo, GC.CheckTypeId, GC.CheckNo, GC.CheckNotes, GC.CheckDate, GC.CheckBankId, GC.CurrencyId2, GC.MainValue2, GC.Exchange2, GC.IsDocumented, GC.IsNotDocumented, GC.SalesTypeId, GC.SalesDocNo, GC.StatusId, GC.StatusValue}, New VariantType() {VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Date, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal}, New String() {}, "Line", "Line") Then Return

        If Not bm.SaveGrid(G2, TableName2, New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, New String() {"MainValue", "Exchange", "Value", "LinkFile", "MainAccNo", "AnalysisId", "CostCenterId", "CostCenterSubId", "Notes", "DocNo", "CheckTypeId", "CheckNo", "CheckNotes", "CheckDate", "CheckBankId", "CurrencyId2", "MainValue2", "Exchange2", "IsDocumented", "IsNotDocumented", "SalesTypeId", "SalesDocNo", "StatusId", "StatusValue"}, New String() {GC.MainValue, GC.Exchange, GC.Value, GC.LinkFile, GC.MainAccNo, GC.AnalysisId, GC.CostCenterId, GC.CostCenterSubId, GC.Notes, GC.DocNo, GC.CheckTypeId, GC.CheckNo, GC.CheckNotes, GC.CheckDate, GC.CheckBankId, GC.CurrencyId2, GC.MainValue2, GC.Exchange2, GC.IsDocumented, GC.IsNotDocumented, GC.SalesTypeId, GC.SalesDocNo, GC.StatusId, GC.StatusValue}, New VariantType() {VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Date, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal}, New String() {}, "Line", "Line") Then Return

        If Not bm.Save(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, State) Then Return

        If Not bm2.Save(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, State) Then Return

        Select Case CType(sender, Button).Name
            Case btnPrint.Name, btnPrint2.Name
                State = BasicMethods.SaveState.Print
        End Select

        TraceInvoice(State.ToString)

        If Not DontClear Then
            If Md.MyProjectType = ProjectType.SonacAlex Then
                bm.ShowMSG("تم الحفظ", True)
            Else
                btnNew_Click(sender, e)
            End If
        End If

        DisableControls()

        AllowSave = True
        IsEditing = False
        SaveSetting("SonacAlex", "BankCash_G2TypeId", Flag, BankCash_G2TypeId.SelectedValue)
    End Sub

    Sub EnableControls()
        G1.ReadOnly = False
        G2.ReadOnly = False

        G1.Columns(GC.MainAccName).ReadOnly = True
        G2.Columns(GC.MainAccName).ReadOnly = True

        CheckTypeId.IsEnabled = True
        MainLinkFile.IsEnabled = True
        BankId.IsEnabled = True
        CurrencyId.IsEnabled = True
        DocNo.IsEnabled = True
    End Sub

    Sub DisableControls()
        G1.ReadOnly = True
        G2.ReadOnly = True
        CheckTypeId.IsEnabled = False
        MainLinkFile.IsEnabled = False
        BankId.IsEnabled = False
        CurrencyId.IsEnabled = False
        DocNo.IsEnabled = False
    End Sub

    Sub TraceInvoice(ByVal State As String)
        bm.ExecuteNonQuery("BeforeDeleteBankCash_G2", New String() {"Flag", "BankCash_G2TypeId", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, BankCash_G2TypeId.SelectedValue, txtID.Text, Md.UserName, State})
        bm.ExecuteNonQuery("BeforeDeleteBankCash_G22", New String() {"Flag", "BankCash_G2TypeId", "InvoiceNo", "UserDelete", "State"}, New String() {Flag, BankCash_G2TypeId.SelectedValue, txtID.Text, Md.UserName, State})
    End Sub


    Dim lop As Boolean = False

    Sub ClearRow(G As MyGrid, ByVal i As Integer)
        G.Rows(i).Cells(GC.MainValue).Value = Nothing
        G.Rows(i).Cells(GC.Exchange).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.LinkFile).Value = Nothing
        G.Rows(i).Cells(GC.MainAccNo).Value = Nothing
        G.Rows(i).Cells(GC.MainAccName).Value = Nothing
        G.Rows(i).Cells(GC.AnalysisId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterId).Value = Nothing
        G.Rows(i).Cells(GC.CostCenterSubId).Value = Nothing
        G.Rows(i).Cells(GC.Notes).Value = Nothing
        G.Rows(i).Cells(GC.DocNo).Value = Nothing
        G.Rows(i).Cells(GC.SalesTypeId).Value = Nothing
        G.Rows(i).Cells(GC.CheckTypeId).Value = "1"
        G.Rows(i).Cells(GC.CheckNo).Value = Nothing
        G.Rows(i).Cells(GC.CheckDate).Value = Nothing
        G.Rows(i).Cells(GC.CheckBankId).Value = Nothing
        G.Rows(i).Cells(GC.CheckNotes).Value = Nothing

        G.Rows(i).Cells(GC.CurrencyId2).Value = Nothing
        G.Rows(i).Cells(GC.MainValue2).Value = Nothing
        G.Rows(i).Cells(GC.Exchange2).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
        G.Rows(i).Cells(GC.IsDocumented).Value = Nothing
        G.Rows(i).Cells(GC.IsNotDocumented).Value = Nothing
        G.Rows(i).Cells(GC.SalesTypeId).Value = Nothing
        G.Rows(i).Cells(GC.SalesDocNo).Value = Nothing

    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Dim G As MyGrid = sender

        If lop Then Return
        Try
            If G.Columns(e.ColumnIndex).Name = GC.MainValue OrElse G.Columns(e.ColumnIndex).Name = GC.Exchange Then

                If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then
                    G.Rows(e.RowIndex).Cells(GC.Exchange).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchangeMain(" & CurrencyId.SelectedValue.ToString & ",'" & bm.ToStrDate(DayDate.SelectedDate) & "')")
                    If Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) = 0 Then
                        G.Rows(e.RowIndex).Cells(GC.Exchange).Value = 1
                    End If
                    If Val(G.Rows(e.RowIndex).Cells(GC.Exchange2).Value) = 0 Then
                        G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = 1
                    End If
                End If

                G.Rows(e.RowIndex).Cells(GC.MainValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value)
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value), 4, MidpointRounding.AwayFromZero)

                'GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.MainValue2).Index, G.CurrentRow.Index))
                
                G.Rows(e.RowIndex).Cells(GC.MainValue2).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value)
                G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value = CurrencyId.SelectedValue

            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainValue2 Then
                If Val(G.Rows(e.RowIndex).Cells(GC.LinkFile).Value) = 8 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId2).Value) = 1 Then
                    G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue2).Value)
                    G.Rows(e.RowIndex).Cells(GC.Exchange).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) / Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value)
                End If
                G.Rows(e.RowIndex).Cells(GC.Exchange2).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) / Val(G.Rows(e.RowIndex).Cells(GC.MainValue2).Value)

            ElseIf G.Columns(e.ColumnIndex).Name = GC.Value Then
                'GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.IsDocumented).Index, e.RowIndex))
            ElseIf G.Columns(e.ColumnIndex).Name = GC.StatusValue AndAlso G.Rows(e.RowIndex).Cells(GC.StatusValue).Value <> "" Then
                If G.Rows(e.RowIndex).Cells(GC.StatusValue).Value > Rnd(G.Rows(e.RowIndex).Cells(GC.Value).Value) Then
                    bm.ShowMSG("يوجد خطأ في قيمة الحالة")
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.StatusId Then
                If Val(G.Rows(e.RowIndex).Cells(GC.StatusId).Value) <> 0 AndAlso Val(G.Rows(e.RowIndex).Cells(GC.StatusId).Value) <> 4 Then
                    G.Rows(e.RowIndex).Cells(GC.StatusValue).Value = Rnd(G.Rows(e.RowIndex).Cells(GC.Value).Value)
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.IsDocumented Then 'AndAlso G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value <> 0 
                If Rnd(G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value) > Rnd(G.Rows(e.RowIndex).Cells(GC.Value).Value) Then
                    bm.ShowMSG("يوجد خطأ في قيمة المؤيد")
                    G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value = 0
                    G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value = 0
                ElseIf G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value = "1" Then
                    G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value = Rnd(G.Rows(e.RowIndex).Cells(GC.Value).Value)
                    G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value = 0
                Else
                    G.Rows(e.RowIndex).Cells(GC.IsNotDocumented).Value = Val(G.Rows(e.RowIndex).Cells(GC.Value).Value) - Val(G.Rows(e.RowIndex).Cells(GC.IsDocumented).Value)
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.SalesDocNo AndAlso Val(G.Rows(e.RowIndex).Cells(GC.SalesDocNo).Value) > 0 Then
                G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value = bm.ExecuteScalar("getSalesCustMainAccNo", {"SalesTypeId", "SalesDocNo"}, {Val(G.Rows(e.RowIndex).Cells(GC.SalesTypeId).Value), Val(G.Rows(e.RowIndex).Cells(GC.SalesDocNo).Value)})
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CheckDate AndAlso G.Rows(e.RowIndex).Cells(GC.CheckDate).Value.ToString <> "" Then
                G.Rows(e.RowIndex).Cells(GC.CheckDate).Value = bm.ToStrDate(G.Rows(e.RowIndex).Cells(GC.CheckDate).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.MainAccNo Then
                'bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MainAccNo), MainAccName)
                bm.AccNoLostFocusGrid(G.Rows(e.RowIndex).Cells(GC.MainAccNo), G.Rows(e.RowIndex).Cells(GC.MainAccName))
                G.Rows(e.RowIndex).Cells(GC.CheckNotes).Value = G.Rows(e.RowIndex).Cells(GC.MainAccName).Value
                If G.Rows(e.RowIndex).Cells(GC.MainAccNo).Value.ToString.Trim = "" Then
                    G.ReverseCurrentCell = True
                End If
            End If

            Try
                If G.Rows(G.CurrentRow.Index).Cells(GC.SalesTypeId).Value Is Nothing OrElse G.Rows(G.CurrentRow.Index).Cells(GC.SalesTypeId).Value = "" Then
                    G.Rows(G.CurrentRow.Index).Cells(GC.SalesTypeId).Value = G.Rows(G.CurrentRow.Index - 1).Cells(GC.SalesTypeId).Value
                End If
            Catch ex As Exception
            End Try

            Try
                If G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value Is Nothing OrElse G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value = "" Then
                    G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value = G.Rows(G.CurrentRow.Index - 1).Cells(GC.Notes).Value
                End If
            Catch ex As Exception
            End Try

            loplop = True
            Try
                If Val(G.Rows(e.RowIndex).Cells(GC.CheckTypeId).Value) < 1 Then G.Rows(e.RowIndex).Cells(GC.CheckTypeId).Value = "1"
                CheckTypeId.SelectedValue = G.Rows(e.RowIndex).Cells(GC.CheckTypeId).Value
                CheckNo.Text = G.Rows(e.RowIndex).Cells(GC.CheckNo).Value
                CheckNotes.Text = G.Rows(e.RowIndex).Cells(GC.CheckNotes).Value

                CheckBankId.Text = G.Rows(e.RowIndex).Cells(GC.CheckBankId).Value
                CheckBankId_LostFocus(Nothing, Nothing)
                CheckDate.SelectedDate = Nothing
                If G.Rows(e.RowIndex).Cells(GC.CheckDate).Value Is Nothing Then
                    G.Rows(e.RowIndex).Cells(GC.CheckDate).Value = Nothing
                Else
                    CheckDate.SelectedDate = DateTime.Parse(G.Rows(e.RowIndex).Cells(GC.CheckDate).Value)
                End If
            Catch ex As Exception
            End Try
            loplop = False
            TestEnable()

            CalcTotal()
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub
    Dim loplop As Boolean = False

    Function Rnd(str As Object) As Decimal
        Try
            Return Decimal.Parse(str)
        Catch ex As Exception
            Return 0
        End Try
    End Function



    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {MainId, SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Dim LopNew As Boolean = False
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        LopNew = True
        ClearControls()
        IsEditing = False
        LopNew = False
    End Sub

    Sub ClearControls()
        If lop Then Return
        lop = True

        EnableControls()

        cboSearch.SelectedIndex = 0

        IsNew = True


        Dim d As Date = bm.MyGetDate()
        If Md.MyProjectType = ProjectType.SonacAlex AndAlso Not DayDate.SelectedDate Is Nothing Then
            d = DayDate.SelectedDate
        End If

        Dim s1 = MainLinkFile.SelectedValue
        Dim s2 = BankId.Text
        Dim s3 = CurrencyId.SelectedValue

        bm.ClearControls(False)
        
        MainLinkFile.SelectedValue = s1
        BankId.Text = s2
        CurrencyId.SelectedValue = s3

        DayDate.SelectedDate = d

        G1.Rows.Clear()
        G2.Rows.Clear()
        CalcTotal()

        CheckTypeId.SelectedValue = 1
        CheckNo.Clear()
        CheckDate.SelectedDate = Nothing
        CheckBankId.Clear()
        CheckBankName.Clear()
        CheckNotes.Clear()
        TestEnable()

        Value.Clear()
        MainValue.Clear()
        BankId_LostFocus(Nothing, Nothing)

        txtFlag.Text = Flag
        SystemUser.SelectedValue = Md.UserName

        If BankCash_G2TypeId.SelectedIndex < 1 Then
            lop = False
            Return
        End If
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and " & SubId & "=" & txtFlag.Text)
        dt = bm.ExecuteAdapter("select FromInvoiceNo,ToInvoiceNo from BankCash_G2Types where Flag=" & Flag & " and Id=" & BankCash_G2TypeId.SelectedValue.ToString)
        If dt.Rows.Count = 0 Then
            lop = False
            Return
        End If
        If txtID.Text = "" Then txtID.Text = dt.Rows(0)("FromInvoiceNo")
        If Val(txtID.Text) > dt.Rows(0)("ToInvoiceNo") Then txtID.Text = dt.Rows(0)("ToInvoiceNo")
        'DayDate.Focus()
        'txtID.Focus()
        'txtID.SelectAll()
        DocNo.Focus()
        DocNo.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            TraceInvoice("Deleted")
            bm.ExecuteNonQuery("delete from " & TableName & " where " & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            bm.ExecuteNonQuery("delete from " & TableName2 & " where " & MainId & "=" & BankCash_G2TypeId.SelectedValue.ToString & " and " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnPrevios_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv OrElse BankCash_G2TypeId.SelectedValue Is Nothing Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {MainId, SubId, SubId2}, New String() {BankCash_G2TypeId.SelectedValue.ToString, txtFlag.Text.Trim, txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()

            dt = bm.ExecuteAdapter("select FromInvoiceNo,ToInvoiceNo from BankCash_G2Types where Flag=" & Flag & " and Id=" & BankCash_G2TypeId.SelectedValue.ToString)
            If dt.Rows.Count > 0 Then
                If Val(txtID.Text) < dt.Rows(0)("FromInvoiceNo") OrElse Val(txtID.Text) > dt.Rows(0)("ToInvoiceNo") Then txtID.Text = ""
            End If

            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, DocNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus
        Try
            If Val(BankId.Text.Trim) = 0 Then
                BankId.Clear()
                BankName.Clear()
                Return
            End If

            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
            bm.LostFocus(BankId, BankName, "select Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ") where Id=" & BankId.Text.Trim())
            CurrencyId.SelectedValue = bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
            CurrencyId_SelectionChanged(Nothing, Nothing)
        Catch
        End Try
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ")") Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub


    Private Sub CheckBankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CheckBankId.LostFocus
        If Val(CheckBankId.Text.Trim) = 0 Then
            CheckBankId.Clear()
            CheckBankName.Clear()
            Return
        End If
        bm.LostFocus(CheckBankId, CheckBankName, "select Name from CheckBanks where Id=" & CheckBankId.Text.Trim())
    End Sub
    Private Sub CheckBankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CheckBankId.KeyUp
        If bm.ShowHelp("Banks", CheckBankId, CheckBankName, e, "select cast(Id as varchar(100)) Id,Name from CheckBanks", "CheckBanks") Then
            CheckBankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub CheckNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CheckNo.KeyUp
        Dim str As String = "GetCheckStates "
        If Flag = 2 Then
            str &= " @LinkFile=" & Val(MainLinkFile.SelectedValue) & ",@AccNo=" & Val(BankId.Text) & ",@LinkFile2=" & Val(G1.CurrentRow.Cells(GC.LinkFile).Value) & ",@AccNo2=" & 0
        Else
            str &= " @LinkFile2=" & Val(MainLinkFile.SelectedValue) & ",@AccNo2=" & Val(BankId.Text) & ",@LinkFile=" & Val(G1.CurrentRow.Cells(GC.LinkFile).Value) & ",@AccNo=" & 0
        End If

        If bm.ShowHelpMultiColumns("الشيكات", CheckNo, CheckNo, e, str) Then
            'CheckNo.Text = bm.SelectedRow(0)
            CheckNo_LostFocus(Nothing, Nothing)
            CheckBankId_LostFocus(Nothing, Nothing)

            Try
                G1.CurrentRow.Cells(GC.MainValue).Value = bm.SelectedRow("المتبقي")
                GridCalcRow(G1, New System.Windows.Forms.DataGridViewCellEventArgs(G1.Columns(GC.MainValue).Index, G1.CurrentRow.Index))
                G1.CurrentRow.Cells(GC.MainValue2).Value = 0
            Catch
            End Try

        End If

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
        lblNotes.SetResourceReference(Label.ContentProperty, "Notes")
    End Sub

    Private Sub btnDeleteRow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDeleteRow.Click
        Try
            If CurrentGrid Is Nothing Then Return
            If Not CurrentGrid.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                CurrentGrid.Rows.Remove(CurrentGrid.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        LopCalc = True
        Try
            Value.Text = Math.Round(0, 4)
            MainValue.Text = Math.Round(0, 4)
            For i As Integer = 0 To G1.Rows.Count - 1
                Value.Text += Val(G1.Rows(i).Cells(GC.Value).Value)
                MainValue.Text += Val(G1.Rows(i).Cells(GC.MainValue).Value)
            Next
            For i As Integer = 0 To G2.Rows.Count - 1
                Value.Text -= Val(G2.Rows(i).Cells(GC.Value).Value)
                MainValue.Text -= Val(G2.Rows(i).Cells(GC.MainValue).Value)
            Next
        Catch ex As Exception
        End Try

        LopCalc = False
    End Sub


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        Dim G As MyGrid = sender
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.MainAccNo).Index Then
                If bm.AccNoShowHelpGrid(G.CurrentRow.Cells(GC.MainAccNo), G.CurrentRow.Cells(GC.MainAccName), e, 1) Then
                    'MainAccName.Content = G.CurrentRow.Cells(GC.MainAccNo).Value
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.AnalysisId)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub G_CellBeginEdit(sender As Object, e As Forms.DataGridViewCellCancelEventArgs)
        Dim G As MyGrid = sender
        If CType(G.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If
        Try
            If BankCash_G2TypeId.SelectedValue = 2 Then
                CheckTypeId.SelectedValue = 2
                CheckTypeId_LostFocus(Nothing, Nothing)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub G_SelectionChanged(sender As Object, e As EventArgs)
        Dim G As MyGrid = sender
        If G.CurrentRow Is Nothing Then Return
        Try
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.LinkFile).Index, G.CurrentRow.Index))
        Catch
        End Try
    End Sub

    Private Sub BankCash_G2TypeId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles BankCash_G2TypeId.SelectionChanged
        btnNew_Click(Nothing, Nothing)
        txtID_LostFocus(Nothing, Nothing)
        'SaveSetting("SonacAlex", "BankCash_G2TypeId", Flag, BankCash_G2TypeId.SelectedValue)
    End Sub

    Private Sub Exchange_TextChanged(sender As Object, e As TextChangedEventArgs) 'Handles Exchange.TextChanged
        For i As Integer = 0 To G1.Rows.Count - 1
            If Val(G1.Rows(i).Cells(GC.MainValue).Value) <> 0 Then
                GridCalcRow(G1, New Forms.DataGridViewCellEventArgs(G1.Columns(GC.MainValue).Index, i))
            End If
        Next
    End Sub

    Private Sub CheckTypeId_LostFocus(sender As Object, e As RoutedEventArgs) Handles CheckTypeId.LostFocus
        If loplop Then Return
        If G1.CurrentRow Is Nothing Then
            G1.CurrentCell = G1.Rows(G1.Rows.Add()).Cells(GC.MainValue)
        End If

        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckTypeId).Value = CheckTypeId.SelectedValue.ToString
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckTypeId).Value = "1"
        End Try
        TestEnable()
    End Sub

    Private Sub CheckNo_LostFocus(sender As Object, e As RoutedEventArgs) Handles CheckNo.LostFocus
        Try
            dt = bm.ExecuteAdapter("select top 1 dbo.ToStrDate(CheckDate),CheckBankId from BankCash_G2 where CheckNo='" & CheckNo.Text & "' Order by Daydate")
            If dt.Rows.Count > 0 Then
                CheckDate.SelectedDate = CType(dt.Rows(0)(0), DateTime)
                CheckBankId.Text = dt.Rows(0)(1)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CheckNo_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CheckNo.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        If G1.CurrentRow.Index = G1.NewRowIndex Then
            Dim i As Integer = G1.Rows.Add
            G1.CurrentCell = G1.Rows(i).Cells(G1.CurrentCell.ColumnIndex)
        End If
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckNo).Value = CheckNo.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckNo).Value = ""
        End Try
    End Sub

    Private Sub CheckNotes_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CheckNotes.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        If G1.CurrentRow.Index = G1.NewRowIndex Then
            Dim i As Integer = G1.Rows.Add
            G1.CurrentCell = G1.Rows(i).Cells(G1.CurrentCell.ColumnIndex)
        End If
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckNotes).Value = CheckNotes.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckNotes).Value = ""
        End Try
    End Sub

    Private Sub CheckDate_TextChanged(sender As Object, e As SelectionChangedEventArgs) Handles CheckDate.SelectedDateChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckDate).Value = CheckDate.SelectedDate.Value.Date
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckDate).Value = Nothing
        End Try
    End Sub

    Private Sub CheckBankId_TextChanged(sender As Object, e As TextChangedEventArgs) Handles CheckBankId.TextChanged
        If loplop OrElse G1.CurrentRow Is Nothing Then Return
        Try
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckBankId).Value = CheckBankId.Text
        Catch ex As Exception
            G1.Rows(G1.CurrentRow.Index).Cells(GC.CheckBankId).Value = ""
        End Try
        CheckBankId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TestEnable()
        Return
        If CheckTypeId.SelectedValue = 1 Then
            CheckNo.IsReadOnly = False
            CheckNo.IsEnabled = False
            CheckDate.IsEnabled = False
            CheckBankId.IsEnabled = False
            'CheckNotes.IsEnabled = False
            CheckNo.Clear()
            CheckDate.SelectedDate = Nothing
            CheckBankId.Clear()
            CheckBankName.Clear()
            'CheckNotes.Clear()
        ElseIf CheckTypeId.SelectedValue = 2 OrElse CheckTypeId.SelectedValue = 3 OrElse CheckTypeId.SelectedValue = 11 Then
            CheckNo.IsReadOnly = False
            CheckNo.IsEnabled = True
            CheckDate.IsEnabled = True
            CheckBankId.IsEnabled = True
            CheckNotes.IsEnabled = True
            If CheckNo.Text.Trim = "" AndAlso CheckTypeId.SelectedValue = 3 Then
                lop = True
                CheckNo.Text = Flag & "-" & BankCash_G2TypeId.SelectedValue & "-" & txtID.Text & "-" & (G1.CurrentRow.Index + 1)
                lop = False
            End If
        Else
            CheckNo.IsReadOnly = True
            CheckNo.IsEnabled = True
            CheckDate.IsEnabled = False
            CheckBankId.IsEnabled = False
            'CheckNotes.IsEnabled = False
        End If

    End Sub

    Private Sub CurrencyId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CurrencyId.SelectionChanged
        SetGCExchange(G1)
        SetGCExchange(G2)
    End Sub

    Dim AllowSave As Boolean = False
    Dim DontClear As Boolean = False
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@BankCash_G2TypeId", "@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {BankCash_G2TypeId.SelectedValue.ToString, Flag, txtID.Text, CType(Parent, Page).Title}
        rpt.Rpt = "BankCash_G21.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        rpt.paraname = New String() {"@BankCash_G2TypeId", "@Flag", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {BankCash_G2TypeId.SelectedValue.ToString, Flag, txtID.Text, ""}
        rpt.Rpt = "BankCash_G22.rpt"
        rpt.Show()
    End Sub

    Private Sub btnChangeCheckNo_Click(sender As Object, e As RoutedEventArgs) Handles btnChangeCheckNo.Click
        Dim frm As New Window 'With {.SizeToContent = True}
        frm.Content = New ChangeCheckNo With {.MyCheckNo = CheckNo.Text, .txtCheck = CheckNo}
        frm.ShowDialog()
        If CType(frm.Content, ChangeCheckNo).AllowChange Then
            'CheckNo.Text = CType(frm.Content, ChangeCheckNo).MyCheckNo
            CheckNo_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        BankId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub SetGCExchange(G As MyGrid)
        If Md.MyProjectType = ProjectType.SonacAlex Then Return
        Try
            If Flag = 2 Then
                G.Columns(GC.Exchange).ReadOnly = True
            ElseIf Val(CurrencyId.SelectedValue) = 1 Then
                G.Columns(GC.Exchange).ReadOnly = True
            Else
                G.Columns(GC.Exchange).ReadOnly = False
            End If
        Catch
        End Try
    End Sub
     
    Private Sub G_CellBeginEdit2(sender As Object, e As Forms.DataGridViewCellCancelEventArgs)
        If CType(G2.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value Is Nothing Then
            CType(G2.Rows(e.RowIndex).Cells(GC.LinkFile), System.Windows.Forms.DataGridViewComboBoxCell).Value = "0"
        End If
    End Sub
     

    Dim SearchLop As Boolean = False
    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnSearch.Click
        SearchLop = True
        bm.DefineValues()
        bm.SearchTable({MainId, SubId, SubId2}, {BankCash_G2TypeId.SelectedValue, Flag, txtID.Text}, cboSearch, "DocNo", {DayDate}, , True)
        SearchLop = False
    End Sub

    Private Sub cboSearch_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cboSearch.SelectionChanged
        If SearchLop Or lv Then Return
        If cboSearch.SelectedValue = 0 Then
            btnNew_Click(Nothing, Nothing)
            Return
        End If
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
        cboSearch.Focus()
    End Sub

    Dim IsEditing As Boolean = False
    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        IsEditing = True
        EnableControls()
    End Sub
    Private Sub DayDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles DayDate.SelectedDateChanged
        If IsEditing OrElse LopNew Then Return
        btnSearch_Click(Nothing, Nothing)
        txtID.Text = cboSearch.SelectedValue.ToString
        txtID_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub G_RowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        'If G1.CurrentRow.Cells(GC.CheckTypeId).Value Is Nothing Then
        '    G1.CurrentRow.Cells(GC.CheckTypeId).Value = 1
        'End If
        If Not sender Is G2 Then Return
        Try
            G2.CurrentRow.Cells(GC.CheckTypeId).Value = G1.Rows(0).Cells(GC.CheckTypeId).Value
            G2.CurrentRow.Cells(GC.CheckBankId).Value = G1.Rows(0).Cells(GC.CheckBankId).Value
            G2.CurrentRow.Cells(GC.CheckDate).Value = G1.Rows(0).Cells(GC.CheckDate).Value
            G2.CurrentRow.Cells(GC.CheckNo).Value = G1.Rows(0).Cells(GC.CheckNo).Value
            G2.CurrentRow.Cells(GC.CheckNotes).Value = G1.Rows(0).Cells(GC.CheckNotes).Value
        Catch ex As Exception
        End Try
    End Sub

    Function TestCheck(x As Integer) As Boolean
        Dim v As Decimal = 0
        For i As Integer = 0 To G1.Rows.Count - 1
            If G1.Rows(i).Cells(GC.CheckNo).Value = G1.Rows(x).Cells(GC.CheckNo).Value Then
                v += G1.Rows(i).Cells(GC.Value).Value
            End If
        Next
        For i As Integer = 0 To G2.Rows.Count - 1
            If G2.Rows(i).Cells(GC.CheckNo).Value = G1.Rows(x).Cells(GC.CheckNo).Value Then
                v -= G2.Rows(i).Cells(GC.Value).Value
            End If
        Next
        If v < 0 Then Return False
        Return True
    End Function
End Class
