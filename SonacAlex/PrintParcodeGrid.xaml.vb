Imports System.Data

Public Class PrintParcodeGrid

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    WithEvents G As New MyGrid

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        
        For i As Integer = 0 To G.Rows.Count - 1
            Try
                Dim rpt As New ReportViewer
                rpt.Rpt = "PrintBarcodeSizeColor.rpt"
                If Val(InvoiceNo.Text) > 0 Then rpt.Rpt = "PrintBarcodeSizeColorOccasion.rpt"
                rpt.paraname = New String() {"@InvoiceNo", "@ItemId", "@ColorId", "@SizeId", "@Count", "Header"}
                If Val(G.Rows(i).Cells(GC.Id).Value) = 0 Then Continue For
                rpt.paravalue = New String() {Val(InvoiceNo.Text), Val(G.Rows(i).Cells(GC.Id).Value), Val(G.Rows(i).Cells(GC.Color).Value), Val(G.Rows(i).Cells(GC.Size).Value), Val(G.Rows(i).Cells(GC.Qty).Value), CType(Parent, Page).Title}
                rpt.Print(".", Md.BarcodePrinter, 1)
            Catch ex As Exception
            End Try
        Next
        G.Rows.Clear()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadWFH()
        LoadResource()
    End Sub

    Dim lop As Boolean = False



    Private Sub LoadResource()


    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Color As String = "Color"
        Shared Size As String = "Size"
        Shared Qty As String = "Qty"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

        Dim GCColor As New Forms.DataGridViewComboBoxColumn
        GCColor.HeaderText = "اللون"
        GCColor.Name = GC.Color
        bm.FillCombo("select 0 Id,'' Name", GCColor)
        G.Columns.Add(GCColor)

        Dim GCSize As New Forms.DataGridViewComboBoxColumn
        GCSize.HeaderText = "المقاس"
        GCSize.Name = GC.Size
        bm.FillCombo("select 0 Id,'' Name", GCSize)
        G.Columns.Add(GCSize)
        G.Columns.Add(GC.Qty, "العدد")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Color).FillWeight = 100
        G.Columns(GC.Size).FillWeight = 100
        G.Columns(GC.Qty).FillWeight = 100
        
        G.Columns(GC.Name).ReadOnly = True
        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        
        
        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If G.Columns(G.CurrentCell.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(G.CurrentCell.RowIndex).Cells(GC.Barcode).Value Is Nothing Then
            Dim BC As String = Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Barcode).Value.ToString)
            If BC.Length > 12 AndAlso Val(BC.Substring(0, 1)) > 0 Then BC = BC.Substring(0, 12)
            If Not G.Rows(G.CurrentCell.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and left(Barcode,12)='" & Val(BC) & "'")
                AddItem(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value, G.CurrentCell.RowIndex, 0)
                Exit Sub
            End If
        ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        End If
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        CalcRow(e.RowIndex)
    End Sub

    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If


            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "select cast(Id as varchar(100)) Id,Name from Items where IsStopped=0") Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If

                If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.Id).Value)) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items  where IsStopped=0 and Id='" & Id & "'")
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)

            bm.FillCombo("select 0 Id,'-' Name union select Id,Name from ColorsDetails where ColorId=(select It.ColorId from Items It where It.Id='" & Id & "'" & ") order by Id", G.Rows(i).Cells(GC.Color))

            bm.FillCombo("select 0 Id,'-' Name union select Id,Name from SizesDetails where SizeId=(select It.SizeId from Items It where It.Id='" & Id & "'" & ") order by Id", G.Rows(i).Cells(GC.Size))

            If G.Rows(i).Cells(GC.Color).Value Is Nothing Then G.Rows(i).Cells(GC.Color).Value = 0
            If G.Rows(i).Cells(GC.Size).Value Is Nothing Then G.Rows(i).Cells(GC.Size).Value = 0
            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)
            
            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Name)
                G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.Color).Value = Nothing
        G.Rows(i).Cells(GC.Size).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
    End Sub


    Private Sub btnItemsSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnItemsSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F1))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnBalSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnBalSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F12))
        Catch ex As Exception
        End Try
    End Sub

End Class