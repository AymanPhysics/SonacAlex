Imports System.Data

Public Class Chart

    Public TableName As String = "Chart"
    Public TableDetailedName As String = "ChartBal0"
    Public TableDetailedName2 As String = "ChartBal1"
    Public SubId As String = "Id"

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    WithEvents G1 As New MyGrid
    WithEvents G2 As New MyGrid
    Public Flag As Integer = 0

    Private Sub Chart_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        LoadResource()
        bm.FillCombo("LinkFile", LinkFile, "", , True)
        Dim v() As String = {SubId, "Name", "MainAccId", "MotionType", "SubType", "LinkFile", "Level", "EndType", "Id2", "HideFromAccountEnd", "TaxType", "TaxSubType"}
        bm.Fields = v
        Dim c() As Control = {txtID, txtName, AccNo, MotionType, SubType, LinkFile, Level, EndType, Id2, HideFromAccountEnd, TaxType, TaxSubType}
        bm.control = c

        Dim k() As String = {SubId}
        bm.KeyFields = k

        bm.Table_Name = TableName

        'If Md.MyProjectType = ProjectType.SonacAlex Then
        '    PanelGroups.Visibility = Windows.Visibility.Hidden
        'End If
        LoadWFH(WFH1, G1)
        LoadWFH(WFH2, G2)

        LoadTree()
        btnNew_Click(sender, e)
    End Sub


    Structure GC
        Shared CurrencyId As String = "CurrencyId"
        Shared MainBal0 As String = "MainBal0"
        Shared Bal0 As String = "Bal0"
    End Structure

    Private Sub LoadWFH(WHF As Forms.Integration.WindowsFormsHost, G As MyGrid)
        WHF.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        Dim GCCurrencyId As New Forms.DataGridViewComboBoxColumn
        GCCurrencyId.HeaderText = "العملة"
        GCCurrencyId.Name = GC.CurrencyId
        bm.FillCombo("select Id,Name from Currencies union all select 0 Id,'-' Name order by Id", GCCurrencyId)
        G.Columns.Add(GCCurrencyId)

        G.Columns.Add(GC.MainBal0, "العملة الأجنبية")
        G.Columns.Add(GC.Bal0, "القيمة بالجنيه المصري")


        bm.AddThousandSeparator(G.Columns(GC.MainBal0))
        bm.AddThousandSeparator(G.Columns(GC.Bal0))

    End Sub

    Sub LoadTree()
        'If Md.MyProjectType = ProjectType.SonacAlex Then Return

        TreeView1.Items.Clear()
        Dim dt As DataTable = bm.ExecuteAdapter("Select * from Chart order by Id")
        TreeView1.Items.Add(New TreeViewItem With {.Header = "دليل الحسابات"})
        TreeView1.Items(0).Tag = ""
        TreeView1.Items(0).FontSize = 20
        Dim dr() As DataRow = dt.Select("MainAccId=''")
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn As New TreeViewItem
                nn.Foreground = Brushes.DarkRed
                nn.FontSize = 18
                nn.Tag = dr(i)("Id")
                nn.Header = dr(i)("Id") & "          " & dr(i)("Name")
                TreeView1.Items(0).Items.Add(nn)
                loadNode(dt, nn)
            Catch
            End Try
        Next
        CType(TreeView1.Items(0), TreeViewItem).IsExpanded = True
    End Sub

    Sub loadNode(ByVal dt As DataTable, ByVal nn As TreeViewItem)
        Dim dr() As DataRow = dt.Select("MainAccId='" & nn.Tag & "'")
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn2 As New TreeViewItem
                nn2.Foreground = Brushes.DarkBlue
                nn2.FontSize = nn.FontSize - 1
                'nn2.Name = "Node_" & dr(i)("Id")
                nn2.Tag = dr(i)("Id")
                nn2.Header = dr(i)("Id") & "          " & dr(i)("Name")
                nn.Items.Add(nn2)
                loadNode(dt, nn2)
            Catch
            End Try
        Next
        nn.IsExpanded = True
    End Sub


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CType(Application.Current.MainWindow, MainWindow).TabControl1.Items.Remove(Me.Parent)
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {SubId}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        AccNo_LostFocus(Nothing, Nothing)
        Id2_LostFocus(Nothing, Nothing)
        TaxType_LostFocus(Nothing, Nothing)
        TaxSubType_LostFocus(Nothing, Nothing)
        bm.FillControls(Me)

        dt = bm.ExecuteAdapter("select * from " & TableDetailedName & " Where " & SubId & "='" & txtID.Text & "'")
        G1.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G1.Rows.Add()
            G1.Rows(i).Cells(GC.CurrencyId).Value = dt.Rows(i)("CurrencyId").ToString
            G1.Rows(i).Cells(GC.MainBal0).Value = dt.Rows(i)("MainBal0")
            G1.Rows(i).Cells(GC.Bal0).Value = dt.Rows(i)("Bal0")
        Next
        G1.CurrentCell = G1.Rows(G1.Rows.Count - 1).Cells(0)

        dt = bm.ExecuteAdapter("select * from " & TableDetailedName2 & " Where " & SubId & "='" & txtID.Text & "'")
        G2.Rows.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            G2.Rows.Add()
            G2.Rows(i).Cells(GC.CurrencyId).Value = dt.Rows(i)("CurrencyId").ToString
            G2.Rows(i).Cells(GC.MainBal0).Value = dt.Rows(i)("MainBal0")
            G2.Rows(i).Cells(GC.Bal0).Value = dt.Rows(i)("Bal0")
        Next
        G2.CurrentCell = G2.Rows(G2.Rows.Count - 1).Cells(0)

        txtName.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtName.Text.Trim = "" Then
            txtName.Focus()
            Return
        End If
        G1.EndEdit()
        G2.EndEdit()

        bm.DefineValues()
        If Not bm.Save(New String() {SubId}, New String() {txtID.Text.Trim}) Then Return

        If Not TreeView1.SelectedItem Is Nothing AndAlso Not TreeView1.SelectedItem.Tag Is Nothing AndAlso txtID.Text = TreeView1.SelectedItem.Tag Then
            CType(TreeView1.SelectedItem, TreeViewItem).Header = txtID.Text & "          " & txtName.Text
        End If

        If Not bm.SaveGrid(G1, TableDetailedName, New String() {SubId}, New String() {txtID.Text}, New String() {"CurrencyId", "MainBal0", "Bal0"}, New String() {GC.CurrencyId, GC.MainBal0, GC.Bal0}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal}, New String() {GC.CurrencyId}) Then Return
        If Not bm.SaveGrid(G2, TableDetailedName2, New String() {SubId}, New String() {txtID.Text}, New String() {"CurrencyId", "MainBal0", "Bal0"}, New String() {GC.CurrencyId, GC.MainBal0, GC.Bal0}, New VariantType() {VariantType.Integer, VariantType.Decimal, VariantType.Decimal}, New String() {GC.CurrencyId}) Then Return

        UpdateMotionType(txtID.Text, Val(Level.Text))
        bm.ExecuteNonQuery("exec UpdateChartBal0")

        btnNew_Click(sender, e)

    End Sub

    Sub UpdateMotionType(ByVal id As String, ByVal lvl As Integer)
        Dim dt As DataTable = bm.ExecuteAdapter("select Id from Chart where MainAccId='" & id & "'")
        For i As Integer = 0 To dt.Rows.Count - 1
            bm.ExecuteNonQuery("update Chart set MotionType=" & MotionType.SelectedIndex & ",EndType=" & EndType.SelectedIndex & ",Level=" & lvl + 1 & " where Id='" & dt.Rows(i)(0) & "'")
            UpdateMotionType(dt.Rows(i)(0), lvl + 1)
        Next
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click

        bm.FirstLast(New String() {SubId}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Sub ClearControls()
        txtID.Clear()
        Dim s As String = ""
        If Md.MyProjectType = ProjectType.SonacAlex Then
            s = AccNo.Text
        End If
        bm.ClearControls()
        G1.Rows.Clear()
        G2.Rows.Clear()
        SubType.SelectedIndex = 1
        AccNo.Text = s
        AccNo_LostFocus(Nothing, Nothing)
        Id2_LostFocus(Nothing, Nothing)
        TaxType_LostFocus(Nothing, Nothing)
        TaxSubType_LostFocus(Nothing, Nothing)
        txtID.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.IF_Exists("select top 1 InvoiceNo from EntryDt where MainAccNo=" & txtID.Text) _
                   OrElse bm.IF_Exists("select top 1 InvoiceNo from BankCash_G2 where MainAccNo=" & txtID.Text) Then
            bm.ShowMSG("لا يمكن المسح")
            Return
        End If
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from " & TableDetailedName & " where " & SubId & "='" & txtID.Text.Trim & "'")
            bm.ExecuteNonQuery("delete from " & TableDetailedName2 & " where " & SubId & "='" & txtID.Text.Trim & "'")

            If txtID.Text = TreeView1.SelectedItem.Tag Then
                Try
                    CType(CType(TreeView1.SelectedItem, TreeViewItem).Parent, TreeViewItem).Items.Remove(CType(TreeView1.SelectedItem, TreeViewItem))
                Catch ex As Exception
                End Try
            End If

            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {SubId}, New String() {txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_KeyUp(sender As Object, e As KeyEventArgs) Handles txtID.KeyUp
        If bm.ShowHelp("Chart", txtID, txtName, e, "select cast(Id as varchar(100)) Id,Name from Chart") Then
            txtName.Focus()
        End If
    End Sub

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
        txtName.Focus()
        lv = False
        'txtName.Text = dt(0)("Name")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub AccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles AccNo.LostFocus

        AccName.Clear()
        MotionType.IsEnabled = True
        EndType.IsEnabled = True

        Level.Text = 1
        MotionType.SelectedIndex = 0
        EndType.SelectedIndex = 0

        If Val(AccNo.Text) = 0 Then
            Return
        End If

        bm.AccNoLostFocus(AccNo, AccName, 0, , )
        If AccNo.Text = "" Then Return

        If AccNo.Text = txtID.Text Then
            bm.ShowMSG("Main Account couldn't be Equal to Sub Account")
            AccNo.Clear()
            AccName.Clear()
        End If

        MotionType.IsEnabled = False
        EndType.IsEnabled = False

        Dim dt As DataTable = bm.ExecuteAdapter("select MotionType,EndType,Level from Chart where Id='" & AccNo.Text & "'")
        If dt.Rows.Count > 0 Then
            MotionType.SelectedIndex = Val(dt.Rows(0)("MotionType"))
            EndType.SelectedIndex = Val(dt.Rows(0)("EndType"))
            Level.Text = 1 + Val(dt.Rows(0)("Level"))
        End If

    End Sub
    Private Sub Id2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Id2.LostFocus
        Name2.Clear()
        If Val(Id2.Text) = 0 Then
            Return
        End If
        bm.AccNoLostFocus(Id2, Name2, -1, , , False)
    End Sub
    Private Sub AccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles AccNo.KeyUp
        bm.AccNoShowHelp(AccNo, AccName, e, 0, , )
    End Sub

    Private Sub Id2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Id2.KeyUp
        bm.AccNoShowHelp(Id2, Name2, e, 1, , , False)
    End Sub


    Private Sub SubType_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles SubType.SelectionChanged
        If SubType.SelectedIndex = 1 Then
            LinkFile.IsEnabled = True
            Id2.IsEnabled = True
        Else
            LinkFile.SelectedIndex = 0
            LinkFile.IsEnabled = False
            Id2.Clear()
            Id2.IsEnabled = False
        End If

    End Sub

    Private Sub TreeView1_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Object)) Handles TreeView1.SelectedItemChanged
        If TreeView1.SelectedItem Is Nothing Then Return
        txtID.Text = TreeView1.SelectedItem.Tag
        txtID_LostFocus(Nothing, Nothing)
        TreeView1.Focus()
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
        lblAccNo.SetResourceReference(Label.ContentProperty, "AccNo")
        lblLinkFile.SetResourceReference(Label.ContentProperty, "LinkFile")
        lblMotionType.SetResourceReference(Label.ContentProperty, "MotionType")
        lblName.SetResourceReference(Label.ContentProperty, "Name")
        lblSubType.SetResourceReference(Label.ContentProperty, "SubType")


        bm.ResetComboboxContent(SubType)
        bm.ResetComboboxContent(MotionType)
    End Sub

    Private Sub btnSave_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy.Click
        LoadTree()
    End Sub


    Private Sub TaxType_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TaxType.LostFocus
        TaxTypeName.Text = bm.ExecuteScalar("select DESCRIPTION Name from TaxApi_TaxTypes where CODE='" & TaxType.Text.Trim & "'")
        If TaxTypeName.Text.Trim = "" Then
            TaxType.Clear()
        End If
        TaxSubType_LostFocus(Nothing, Nothing)
    End Sub
    Private Sub TaxSubType_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TaxSubType.LostFocus
        TaxSubTypeName.Text = bm.ExecuteScalar("select DESCRIPTION Name from TaxApi_TaxSubTypes where CODE='" & TaxSubType.Text.Trim & "' and TypeCode='" & TaxType.Text.Trim & "'")
        If TaxSubTypeName.Text.Trim = "" Then
            TaxSubType.Clear()
        End If
    End Sub

    Private Sub TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TaxType.KeyUp
        If bm.ShowHelp("TaxTypes", TaxType, TaxTypeName, e, "select Code Id,DESCRIPTION Name from TaxApi_TaxTypes") Then
            TaxType_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub TaxSubType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TaxSubType.KeyUp
        If bm.ShowHelp("TaxSubTypes", TaxSubType, TaxSubTypeName, e, "select Code Id,DESCRIPTION Name from TaxApi_TaxSubTypes where TypeCODE='" & TaxType.Text.Trim & "'") Then
            TaxSubType_LostFocus(Nothing, Nothing)
        End If
    End Sub


End Class

