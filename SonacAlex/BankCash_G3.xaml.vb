Imports System.Data

Public Class BankCash_G3
    Public TableName As String = "BankCash_G3"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G As New MyGrid
    WithEvents G2 As New MyGrid
    Public Flag As Integer = 0
    'Public MyLinkFile As Integer = 0
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})

        lblNotes.Visibility = Windows.Visibility.Hidden
        Notes.Visibility = Windows.Visibility.Hidden

        bm.Fields = New String() {SubId, SubId2, "DayDate", "Canceled"}
        bm.control = New Control() {txtFlag, txtID, DayDate, Canceled}
        bm.KeyFields = New String() {SubId, SubId2}
        bm.Table_Name = TableName

        LoadResource()
        LoadWFH()
        LoadWFH2()
        btnNew_Click(sender, e)
        DayDate.Focus()
    End Sub



    Structure GC
        Shared DocNo As String = "DocNo"
        Shared MainValue As String = "MainValue"
        Shared Value As String = "Value"
        Shared FromLinkFile As String = "FromLinkFile"
        Shared FromSubAccNo As String = "FromSubAccNo"
        Shared FromSubAccName As String = "FromSubAccName"
        Shared ToLinkFile As String = "ToLinkFile"
        Shared ToSubAccNo As String = "ToSubAccNo"
        Shared ToSubAccName As String = "ToSubAccName"
        Shared Notes As String = "Notes"
        Shared CurrencyId As String = "CurrencyId"
        Shared Exchange As String = "Exchange"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.DocNo, "رقم المستند")

        Dim GCFromLinkFile As New Forms.DataGridViewComboBoxColumn
        GCFromLinkFile.HeaderText = "الجهة"
        GCFromLinkFile.Name = GC.FromLinkFile
        bm.FillCombo("select Id,Name from LinkFile union all select 0 Id,'-' Name order by Id", GCFromLinkFile)
        G.Columns.Add(GCFromLinkFile)

        G.Columns.Add(GC.FromSubAccNo, "كود الحساب")
        G.Columns.Add(GC.FromSubAccName, "اسم الحساب")
        
        G.Columns.Add(GC.MainValue, "القيمة")

        Dim GCCurrencyId As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId.HeaderText = "العملة"
        GCCurrencyId.Name = GC.CurrencyId
        bm.FillCombo("select Id,Name from Currencies order by Id", GCCurrencyId)
        G.Columns.Add(GCCurrencyId)

        G.Columns.Add(GC.Exchange, "المعامل")
        G.Columns.Add(GC.Value, "المعادل")

        Dim GCToLinkFile As New Forms.DataGridViewComboBoxColumn
        GCToLinkFile.HeaderText = "الجهة"
        GCToLinkFile.Name = GC.ToLinkFile
        bm.FillCombo("select Id,Name from LinkFile union all select 0 Id,'-' Name order by Id", GCToLinkFile)
        G.Columns.Add(GCToLinkFile)

        G.Columns.Add(GC.ToSubAccNo, "كود الحساب المحول إليه")
        G.Columns.Add(GC.ToSubAccName, "اسم الحساب المحول إليه")

        G.Columns.Add(GC.Notes, "البيان")

        G.Columns(GC.FromSubAccName).ReadOnly = True
        G.Columns(GC.ToSubAccName).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True

        G.Columns(GC.Exchange).Visible = False
        G.Columns(GC.CurrencyId).ReadOnly = True
        G.Columns(GC.Exchange).ReadOnly = True

        G.Columns(GC.FromSubAccName).FillWeight = 200
        G.Columns(GC.ToSubAccName).FillWeight = 200
        G.Columns(GC.Notes).FillWeight = 200
        G.Columns(GC.DocNo).FillWeight = 100


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
        AddHandler G.RowsAdded, AddressOf G_RowsAdded
    End Sub

    Structure GC2
        Shared MainValue As String = "MainValue"
        Shared CurrencyId As String = "CurrencyId"
    End Structure


    Private Sub LoadWFH2()
        WFH2.Child = G2

        G2.Columns.Clear()
        G2.ForeColor = System.Drawing.Color.DarkBlue

        G2.Columns.Add(GC2.MainValue, "الإجمالي")

        Dim GCCurrencyId As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId.HeaderText = "العملة"
        GCCurrencyId.Name = GC2.CurrencyId
        bm.FillCombo("select Id,Name from Currencies order by Id", GCCurrencyId)
        G2.Columns.Add(GCCurrencyId)

        G2.ReadOnly = True
        G2.AllowUserToAddRows = False
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        If lop Then Return
        lop = True
        bm.FillControls(Me)

        Dim dt As DataTable = bm.ExecuteAdapter("select * from " & TableName & " where " & SubId & "=" & txtFlag.Text.Trim & " and " & SubId2 & "=" & txtID.Text)

        G.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G.Rows.Add()
            G.Rows(i).Cells(GC.DocNo).Value = dt.Rows(i)("DocNo").ToString
            G.Rows(i).Cells(GC.MainValue).Value = dt.Rows(i)("MainValue").ToString
            G.Rows(i).Cells(GC.Value).Value = dt.Rows(i)("Value").ToString
            G.Rows(i).Cells(GC.FromLinkFile).Value = dt.Rows(i)("FromLinkFile").ToString
            G.Rows(i).Cells(GC.FromSubAccNo).Value = dt.Rows(i)("FromSubAccNo").ToString
            G.Rows(i).Cells(GC.ToLinkFile).Value = dt.Rows(i)("ToLinkFile").ToString
            G.Rows(i).Cells(GC.ToSubAccNo).Value = dt.Rows(i)("ToSubAccNo").ToString
            G.Rows(i).Cells(GC.Notes).Value = dt.Rows(i)("Notes").ToString
            G.Rows(i).Cells(GC.CurrencyId).Value = dt.Rows(i)("CurrencyId").ToString
            G.Rows(i).Cells(GC.Exchange).Value = dt.Rows(i)("Exchange").ToString
            lop = False
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.FromSubAccNo).Index, i))
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ToSubAccNo).Index, i))
            lop = True
        Next
        DayDate.Focus()
        G.RefreshEdit()
        lop = False
        CalcTotal()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(txtID.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد المسلسل")
            txtID.Focus()
            Return
        End If

        G.EndEdit()

        For i As Integer = 0 To G.Rows.Count - 1
            If Val(G.Rows(i).Cells(GC.MainValue).Value) = 0 Then
                Continue For
            End If
            If Val(G.Rows(i).Cells(GC.FromSubAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد البنك بالسطر " & (i + 1).ToString)
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.FromSubAccNo)
                Return
            End If
            If Val(G.Rows(i).Cells(GC.ToSubAccNo).Value) = 0 Then
                bm.ShowMSG("برجاء تحديد البنك بالسطر " & (i + 1).ToString)
                G.Focus()
                G.CurrentCell = G.Rows(i).Cells(GC.ToSubAccNo)
                Return
            End If
        Next

        If Not IsDate(DayDate.SelectedDate) Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            DayDate.Focus()
            Return
        End If


        bm.DefineValues()

        If Not bm.SaveGrid(G, TableName, New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text}, New String() {"DocNo", "MainValue", "Value", "FromLinkFile", "FromSubAccNo", "ToLinkFile", "ToSubAccNo", "Notes", "CurrencyId", "Exchange"}, New String() {GC.DocNo, GC.MainValue, GC.Value, GC.FromLinkFile, GC.FromSubAccNo, GC.ToLinkFile, GC.ToSubAccNo, GC.Notes, GC.CurrencyId, GC.Exchange}, New VariantType() {VariantType.String, VariantType.Decimal, VariantType.Decimal, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.Integer, VariantType.String, VariantType.Integer, VariantType.Decimal}, New String() {GC.MainValue, GC.FromSubAccNo, GC.ToSubAccNo}) Then Return

        If Not bm.Save(New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text}) Then Return

        btnNew_Click(sender, e)
    End Sub

    Dim lop As Boolean = False

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.DocNo).Value = Nothing
        G.Rows(i).Cells(GC.MainValue).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.FromLinkFile).Value = Nothing
        G.Rows(i).Cells(GC.FromSubAccNo).Value = Nothing
        G.Rows(i).Cells(GC.FromSubAccName).Value = Nothing
        G.Rows(i).Cells(GC.ToLinkFile).Value = Nothing
        G.Rows(i).Cells(GC.ToSubAccNo).Value = Nothing
        G.Rows(i).Cells(GC.ToSubAccName).Value = Nothing
        G.Rows(i).Cells(GC.Notes).Value = Nothing
        G.Rows(i).Cells(GC.CurrencyId).Value = Nothing
        G.Rows(i).Cells(GC.Exchange).Value = Nothing
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If lop Then Return
        Try
            If G.Columns(e.ColumnIndex).Name = GC.MainValue OrElse G.Columns(e.ColumnIndex).Name = GC.Exchange Then
                G.Rows(e.RowIndex).Cells(GC.MainValue).Value = Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value)
                G.Rows(e.RowIndex).Cells(GC.Exchange).Value = Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value)
                G.Rows(e.RowIndex).Cells(GC.Value).Value = Math.Round(Val(G.Rows(e.RowIndex).Cells(GC.Exchange).Value) * Val(G.Rows(e.RowIndex).Cells(GC.MainValue).Value), 2, MidpointRounding.AwayFromZero)
            ElseIf G.Columns(e.ColumnIndex).Name = GC.FromLinkFile OrElse G.Columns(e.ColumnIndex).Name = GC.FromSubAccNo Then
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.FromLinkFile).Value))

                If dt.Rows.Count > 0 Then
                    bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.FromSubAccNo), G.Rows(e.RowIndex).Cells(GC.FromSubAccName), "select Name from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.FromSubAccNo).Value))

                    G.Rows(e.RowIndex).Cells(GC.CurrencyId).Value = bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.FromSubAccNo).Value))
                    G.Rows(e.RowIndex).Cells(GC.Exchange).Value = bm.ExecuteScalar("select dbo.GetCurrencyExchange(Id," & Val(G.Rows(e.RowIndex).Cells(GC.FromLinkFile).Value) & ",CurrencyId,0,'" & bm.ToStrDate(DayDate.SelectedDate) & "') from " & dt.Rows(0)("TableName") & " where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.FromSubAccNo).Value))
                Else
                    G.Rows(e.RowIndex).Cells(GC.FromSubAccNo).Value = ""
                    G.Rows(e.RowIndex).Cells(GC.FromSubAccName).Value = ""
                End If
            ElseIf G.Columns(e.ColumnIndex).Name = GC.ToLinkFile OrElse G.Columns(e.ColumnIndex).Name = GC.ToSubAccNo Then
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & Val(G.Rows(e.RowIndex).Cells(GC.ToLinkFile).Value))

                If dt.Rows.Count > 0 Then
                    bm.LostFocusGrid(G.Rows(e.RowIndex).Cells(GC.ToSubAccNo), G.Rows(e.RowIndex).Cells(GC.ToSubAccName), "select Name from " & dt.Rows(0)("TableName") & " where CurrencyId=" & Val(G.Rows(e.RowIndex).Cells(GC.CurrencyId).Value) & " and Id=" & Val(G.Rows(e.RowIndex).Cells(GC.ToSubAccNo).Value))
                Else
                    G.Rows(e.RowIndex).Cells(GC.ToSubAccNo).Value = ""
                    G.Rows(e.RowIndex).Cells(GC.ToSubAccName).Value = ""
                End If

            End If
            
            CalcTotal()
            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        Catch ex As Exception
        End Try
    End Sub
    Dim loplop As Boolean = False

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls(False)
        ClearControls()
    End Sub

    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True

        DayDate.SelectedDate = bm.MyGetDate()
        G.Rows.Clear()
        CalcTotal()
        
        Value.Clear()
        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & SubId & "=" & txtFlag.Text)
        If txtID.Text = "" Then txtID.Text = "1"
        txtID.Focus()
        txtID.SelectAll()
        lop = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId, SubId2}, New String() {txtFlag.Text, txtID.Text}, "Back", dt)
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
        bm.RetrieveAll(New String() {SubId, SubId2}, New String() {txtFlag.Text.Trim, txtID.Text}, dt)
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
            If Not G.CurrentRow.ReadOnly AndAlso bm.ShowDeleteMSG("MsgDeleteRow") Then
                G.Rows.Remove(G.CurrentRow)
                CalcTotal()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Dim LopCalc As Boolean = False
    Private Sub CalcTotal()
        'Or lop 
        If LopCalc Then Return
        LopCalc = True
        Try
            Value.Text = Math.Round(0, 2)
            G2.Rows.Clear()

            For i As Integer = 0 To G.Rows.Count - 1
                Value.Text += Val(G.Rows(i).Cells(GC.Value).Value)
                For x As Integer = 0 To G2.Rows.Count - 1
                    If Val(G2.Rows(x).Cells(GC2.CurrencyId).Value) = Val(G.Rows(i).Cells(GC.CurrencyId).Value) Then
                        G2.Rows(x).Cells(GC2.MainValue).Value += Val(G.Rows(i).Cells(GC.MainValue).Value)
                        GoTo A
                    End If
                Next
                If Val(G.Rows(i).Cells(GC.CurrencyId).Value) > 0 Then
                    G2.Rows.Add({Val(G.Rows(i).Cells(GC.MainValue).Value), G.Rows(i).Cells(GC.CurrencyId).Value})
                End If
A:
            Next
        Catch ex As Exception
        End Try
        LopCalc = False
    End Sub


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        'e.Handled = True
        If G.CurrentCell Is Nothing OrElse G.CurrentCell.ReadOnly Then Return
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.FromSubAccNo).Index Then
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.FromLinkFile).Value))

                If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.FromSubAccNo), G.Rows(G.CurrentCell.RowIndex).Cells(GC.FromSubAccName), e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.MainValue)
                End If
            ElseIf G.CurrentCell.ColumnIndex = G.Columns(GC.ToSubAccNo).Index Then
                dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.ToLinkFile).Value))

                If dt.Rows.Count > 0 AndAlso bm.ShowHelpGrid(dt.Rows(0)("TableName"), G.Rows(G.CurrentCell.RowIndex).Cells(GC.ToSubAccNo), G.Rows(G.CurrentCell.RowIndex).Cells(GC.ToSubAccName), e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName") & " where CurrencyId=" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.CurrencyId).Value)) Then
                    G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Notes)
                End If
            End If
        Catch ex As Exception
        End Try
        G.CommitEdit(Forms.DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub G_RowsAdded(sender As Object, e As Forms.DataGridViewRowsAddedEventArgs)
        If e.RowIndex < e.RowCount - 1 Then
            G.Rows(e.RowIndex).Cells(GC.DocNo).Value = e.RowIndex + 1
        End If
    End Sub

End Class
