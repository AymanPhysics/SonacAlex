Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management
Imports System.ComponentModel
Imports System.IO

Public Class Entry3

    Public TableName As String = "Entry3"
    Public TableDetailsName As String = "EntryDt3"
    Public SubId  As String = "InvoiceNo"

    Dim dv As New DataView
    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    
    Dim WithEvents BackgroundWorker1 As New BackgroundWorker

    Sub NewId()
        'InvoiceNo.Clear()
        InvoiceNo.IsEnabled = Md.Manager
    End Sub

    Sub UndoNewId()
        'InvoiceNo.IsEnabled = True
    End Sub

    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {btnPrint})
        LoadResource()

        MyAttach.Flag = MyFlag.Entry3

        bm.Fields = New String() {SubId}
        bm.control = New Control() {InvoiceNo}
        bm.KeyFields = New String() {SubId}
        bm.Table_Name = TableName

        LoadWFH()
        btnNew_Click(Nothing, Nothing)
        
        btnLast_Click(Nothing, Nothing)

        btnPrint.Visibility = Windows.Visibility.Hidden
    End Sub


    Structure GC
        Shared DayDate As String = "DayDate"
        Shared DocTypeId As String = "DocTypeId"
        Shared DocNo As String = "DocNo"
        Shared SeasonId As String = "SeasonId"
        Shared ItemId As String = "ItemId"
        Shared Qty As String = "Qty"
        Shared Price As String = "Price"
        Shared Debit As String = "Debit"
        Shared Credit As String = "Credit"
        Shared DedValue As String = "DedValue"
        Shared DedTax As String = "DedTax"
        Shared AnalysisId As String = "AnalysisId"
        Shared Notes As String = "Notes"
        Shared CheckNo As String = "CheckNo"
        Shared BankId As String = "BankId"
        Shared CheckDate As String = "CheckDate"
        Shared AccNo As String = "AccNo"
        Shared FarmId As String = "FarmId"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G
        G.RowHeadersVisible = False
        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.DayDate, "التاريخ")
        bm.MakeGridCombo(G, "نوع المستند", GC.DocTypeId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from DocTypes union all select 0 Id,'-' Name order by Id", 200)
        G.Columns.Add(GC.DocNo, "رقم المستند")
        
        bm.MakeGridCombo(G, "الموسم", GC.SeasonId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Seasons union all select 0 Id,'-' Name order by Id", 200)

        bm.MakeGridCombo(G, "الصنف", GC.ItemId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from AgricultureItems union all select 0 Id,'-' Name order by Id")
        G.Columns.Add(GC.Qty, "الكمية")
        G.Columns.Add(GC.Price, "السعر")

        G.Columns.Add(GC.Debit, "مدين")
        G.Columns.Add(GC.Credit, "دائن")
        G.Columns.Add(GC.DedValue, "قيمة الخصم والإضافة")
        G.Columns.Add(GC.DedTax, "ضريبة القيمة المضافة")

        bm.MakeGridCombo(G, "الشركة", GC.AnalysisId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis where SubType=1 union all select 0 Id,'-' Name order by Id")

        G.Columns.Add(GC.Notes, "البيان")
        G.Columns.Add(GC.CheckNo, "رقم الشيك")

        bm.MakeGridCombo(G, "البنك المسحوب عليه", GC.BankId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Banks union all select 0 Id,'-' Name order by Id")

        G.Columns.Add(GC.CheckDate, "تاريخ الاستحقاق")

        bm.MakeGridCombo(G, "الحساب", GC.AccNo, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id")

        bm.MakeGridCombo(G, "المزرعة", GC.FarmId, "select Id,cast(Id as nvarchar(100))+' - '+Name Name from Farms union all select 0 Id,'-' Name order by Id")
        G.Columns.Add(GC.Line, "Line")

        G.Columns(GC.DayDate).FillWeight = 150 
        G.Columns(GC.CheckNo).FillWeight = 150
        G.Columns(GC.CheckDate).FillWeight = 150
        G.Columns(GC.Notes).FillWeight = 300 
        G.Columns(GC.DedValue).Visible = False
        G.Columns(GC.Line).Visible = False

        G.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        For i As Integer = 0 To G.ColumnCount - 1
            G.Columns(i).MinimumWidth = G.Columns(i).FillWeight
        Next


        bm.AddThousandSeparator(G.Columns(GC.Price))
        bm.AddThousandSeparator(G.Columns(GC.Debit))
        bm.AddThousandSeparator(G.Columns(GC.Credit))
        bm.AddThousandSeparator(G.Columns(GC.DedValue))
        bm.AddThousandSeparator(G.Columns(GC.DedTax))

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.CellBeginEdit, AddressOf G_CellBeginEdit
        AddHandler G.RowsAdded, AddressOf G_RowsAdded
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        For j As Integer = 0 To G.Columns.Count - 1
            G.Rows(i).Cells(j).Value = Nothing
        Next
    End Sub

    Function Rnd(str As Object) As Decimal
        Try
            Return Decimal.Parse(str)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.DayDate Then
                If e.RowIndex > 0 AndAlso G.Rows(e.RowIndex).Cells(GC.DayDate).Value = "" Then
                    G.Rows(e.RowIndex).Cells(GC.DayDate).Value = G.Rows(e.RowIndex - 1).Cells(GC.DayDate).Value
                End If
                G.Rows(e.RowIndex).Cells(GC.DayDate).Value = bm.ToStrDate(G.Rows(e.RowIndex).Cells(GC.DayDate).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.CheckDate Then
                G.Rows(e.RowIndex).Cells(GC.CheckDate).Value = bm.ToStrDate(G.Rows(e.RowIndex).Cells(GC.CheckDate).Value)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Qty OrElse G.Columns(e.ColumnIndex).Name = GC.Price Then
                G.Rows(e.RowIndex).Cells(GC.Debit).Value = Rnd(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Rnd(G.Rows(e.RowIndex).Cells(GC.Price).Value)
                G.Rows(e.RowIndex).Cells(GC.Credit).Value = 0
            ElseIf G.Columns(e.ColumnIndex).Name = GC.Debit Then
                If Rnd(G.Rows(e.RowIndex).Cells(GC.Debit).Value) = 0 Then
                    G.Rows(e.RowIndex).Cells(GC.Credit).Value = Rnd(G.Rows(e.RowIndex).Cells(GC.Qty).Value) * Rnd(G.Rows(e.RowIndex).Cells(GC.Price).Value)
                End If
            End If
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
        If G.Columns(e.ColumnIndex).Name = GC.Debit OrElse G.Columns(e.ColumnIndex).Name = GC.Credit Then
            CalcTotal()
        End If
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True

        btnSave.IsEnabled = True
        btnDelete.IsEnabled = True

        UndoNewId()
        bm.FillControls(Me)

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableDetailsName & " where " & SubId & "=" & InvoiceNo.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).HeaderCell.Value = (i + 1).ToString
            G.Rows(i).Cells(GC.DayDate).Value = bm.ToStrDateForGrid(dt.Rows(i)("DayDate"))
            G.Rows(i).Cells(GC.DocTypeId).Value = dt.Rows(i)("DocTypeId").ToString
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.SeasonId).Value = dt.Rows(i)("SeasonId").ToString
            G.Rows(i).Cells(GC.ItemId).Value = dt.Rows(i)("ItemId")
            G.Rows(i).Cells(GC.Qty).Value = dt.Rows(i)("Qty")
            G.Rows(i).Cells(GC.Price).Value = dt.Rows(i)("Price")
            G.Rows(i).Cells(GC.Debit).Value = dt.Rows(i)("Debit")
            G.Rows(i).Cells(GC.Credit).Value = dt.Rows(i)("Credit")
            G.Rows(i).Cells(GC.DedValue).Value = dt.Rows(i)("DedValue")
            G.Rows(i).Cells(GC.DedTax).Value = dt.Rows(i)("DedTax")
            G.Rows(i).Cells(GC.AnalysisId).Value = dt.Rows(i)("AnalysisId").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes")
            G.Rows(i).Cells(GC.CheckNo).Value = dt.Rows(i)("CheckNo")
            G.Rows(i).Cells(GC.BankId).Value = dt.Rows(i)("BankId").ToString
            Try
                G.Rows(i).Cells(GC.CheckDate).Value = bm.ToStrDateForGrid(dt.Rows(i)("CheckDate"))
            Catch
            End Try
            G.Rows(i).Cells(GC.AccNo).Value = dt.Rows(i)("AccNo").ToString
            G.Rows(i).Cells(GC.FarmId).Value = dt.Rows(i)("FarmId").ToString
            G.Rows(i).Cells(GC.Line).Value = dt.Rows(i)("Line")
        Next
        'DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()


        MyAttach.Key1 = Val(InvoiceNo.Text)
        MyAttach.Key2 = 0
        MyAttach.LoadTree()


        G.ReadOnly = True
        If IsPosted.IsChecked Then
            btnSave.IsEnabled = False
            btnDelete.IsEnabled = False
        End If
        G.Focus()
        G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(0)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPrint.Click
        DontClear = True
        btnSave_Click(sender, e)
        DontClear = False

        PrintPone(sender)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not btnSave.IsEnabled Then Return

        If Not CType(sender, Button).IsEnabled Then Return

        G.EndEdit()
        If Not G.CurrentCell Is Nothing Then
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.CurrentCell.ColumnIndex, G.CurrentCell.RowIndex))
        End If

        CalcTotal()

        If Rnd(Diff.Text) <> 0 Then
            bm.ShowMSG("المدين لا يساوى الدائن")
            Return
        End If

        'For i As Integer = 0 To G.Rows.Count - 1
        '    If Rnd(G.Rows(i).Cells(GC.Debit).Value) = 0 AndAlso Rnd(G.Rows(i).Cells(GC.Credit).Value) = 0 Then
        '        Continue For
        '    End If
        '    If Rnd(G.Rows(i).Cells(GC.FarmId).Value) = 0 Then
        '        bm.ShowMSG("برجاء تحديد المزرعة بالسطر " & (i + 1).ToString)
        '        G.Focus()
        '        G.CurrentCell = G.Rows(i).Cells(GC.FarmId)
        '        Return
        '    End If
        'Next

        Dim State As BasicMethods.SaveState = BasicMethods.SaveState.All
        If InvoiceNo.Text.Trim = "" Then
            InvoiceNo.Text = bm.ExecuteScalar("select max(" & SubId & ")+1 from " & TableName)
            If InvoiceNo.Text = "" Then InvoiceNo.Text = "1"
            State = BasicMethods.SaveState.Insert
        End If

        If Md.MyProjectType = ProjectType.SonacAlex Then
            State = BasicMethods.SaveState.All
        End If

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, State) Then
            If State = BasicMethods.SaveState.Insert Then
                InvoiceNo.Text = ""
            End If
            Return
        End If

        If Not bm.SaveGrid(G, TableDetailsName, New String() {SubId}, New String() {InvoiceNo.Text}, New String() {"DayDate", "DocTypeId", "DocNo", "SeasonId", "ItemId", "Qty", "Price", "Debit", "Credit", "DedValue", "DedTax", "AnalysisId", "Notes", "CheckNo", "BankId", "CheckDate", "AccNo", "FarmId"}, New String() {GC.DayDate, GC.DocTypeId, GC.DocNo, GC.SeasonId, GC.ItemId, GC.Qty, GC.Price, GC.Debit, GC.Credit, GC.DedValue, GC.DedTax, GC.AnalysisId, GC.Notes, GC.CheckNo, GC.BankId, GC.CheckDate, GC.AccNo, GC.FarmId}, New VariantType() {VariantType.Date, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Integer, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.String, VariantType.String, VariantType.Integer, VariantType.Date, VariantType.Integer, VariantType.Integer}, New String() {}, "Line", "Line") Then Return
        
        If Not DontClear Then
            If Md.MyProjectType = ProjectType.SonacAlex Then
                bm.ShowMSG("تم الحفظ", True)
            Else
                btnNew_Click(sender, e)
            End If
        End If

        G.ReadOnly = True
        
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
        G.Focus()
        Try
            G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(0)
        Catch
        End Try
    End Sub

    Sub ClearControls()
        Try
            NewId()

            G.ReadOnly = False
           
            bm.ClearControls(False)

            InvoiceNo.Clear()
            
            G.Rows.Clear()
            CalcTotal()


            MyAttach.Key1 = Val(InvoiceNo.Text)
            MyAttach.Key2 = 0
            MyAttach.LoadTree()

        Catch
        End Try

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'")

            bm.ExecuteNonQuery("delete from " & TableDetailsName & " where " & SubId & "='" & InvoiceNo.Text.Trim & "'")
            btnPrevios_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {InvoiceNo.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False
    Private Sub txtID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InvoiceNo.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {SubId}, New String() {InvoiceNo.Text.Trim}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles InvoiceNo.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
        bm.MyKeyPress(sender, e, True)
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

    Private Sub PrintPone(ByVal sender As System.Object)
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EntryTypeId", "@InvoiceNo", "Header"}
        rpt.paravalue = New String() {InvoiceNo.Text, CType(Parent, Page).Title}
        'rpt.Rpt = "EntryOneMain.rpt"
        rpt.Show()
    End Sub


    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        If LopCalc Or lop Then Return
        LopCalc = True
        Try
            Debit.Text = Math.Round(0, 2)
            Credit.Text = Math.Round(0, 2)
            Diff.Text = Math.Round(0, 2)
            For i As Integer = 0 To G.Rows.Count - 1
                Debit.Text += Rnd(G.Rows(i).Cells(GC.Debit).Value)
                Credit.Text += Rnd(G.Rows(i).Cells(GC.Credit).Value)
            Next
        Catch ex As Exception
        End Try
        Diff.Text = Rnd(Debit.Text) - Rnd(Credit.Text)

        bm.AddThousandSeparator(Debit)
        bm.AddThousandSeparator(Credit)
        bm.AddThousandSeparator(Diff)
        LopCalc = False
    End Sub

    Dim DontClear As Boolean = False

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)


    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")

        btnFirst.SetResourceReference(Button.ContentProperty, "First")
        btnNext.SetResourceReference(Button.ContentProperty, "Next")
        btnPrevios.SetResourceReference(Button.ContentProperty, "Previous")
        btnLast.SetResourceReference(Button.ContentProperty, "Last")
    End Sub

    Private Sub G_CellBeginEdit(sender As Object, e As Forms.DataGridViewCellCancelEventArgs)
        Try
            If Rnd(G.Rows(G.CurrentRow.Index).Cells(GC.Debit).Value) + Rnd(G.Rows(G.CurrentRow.Index).Cells(GC.Credit).Value) <> 0 AndAlso G.CurrentRow.Index > 0 Then
                If G.Rows(G.CurrentRow.Index).Cells(GC.DocNo).Value Is Nothing OrElse G.Rows(G.CurrentRow.Index).Cells(GC.DocNo).Value = "" Then
                    G.Rows(G.CurrentRow.Index).Cells(GC.DocNo).Value = G.Rows(G.CurrentRow.Index - 1).Cells(GC.DocNo).Value
                End If
                If G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value Is Nothing OrElse G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value = "" Then
                    G.Rows(G.CurrentRow.Index).Cells(GC.Notes).Value = G.Rows(G.CurrentRow.Index - 1).Cells(GC.Notes).Value
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        G.ReadOnly = False
    End Sub

    Private Sub G_RowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        If lop Then Return
        Try
            Dim i As Integer = e.RowIndex
            If i < 2 Then Return
            For j As Integer = 0 To G.Columns.Count - 1
                G.Rows(i - 1).Cells(j).Value = G.Rows(i - 2).Cells(j).Value
            Next
            G.Rows(i - 1).Cells(GC.DayDate).Value = bm.ToStrDate(G.Rows(i - 1).Cells(GC.DayDate).Value)
        Catch ex As Exception
        End Try
    End Sub

End Class
