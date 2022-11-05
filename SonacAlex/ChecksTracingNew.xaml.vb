Imports System.Data

Public Class ChecksTracingNew

    Dim MyTextBoxes() As TextBox = {}
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Dim dt2 As New DataTable
    Dim dt3 As New DataTable
    Public Flag As Integer = 0
    Dim dv As New DataView
    Dim dv2 As New DataView
    Dim dv3 As New DataView

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        'bm.FillCombo("LinkFile", MainLinkFile, "", , True)
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({BankId, BankId2})

        bm.FillCombo("CheckTypes", CheckTypeId, "", , True)
        bm.FillCombo("LinkFile", MainLinkFile, "", , True)
        bm.FillCombo("LinkFile", MainLinkFile2, "", , True)

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

        LoadGrid()

    End Sub

    Sub LoadGrid()
        dt = bm.ExecuteAdapter("GetCheckMotion", {"IsRPT", "TypeId", "CheckTypeName2", "CheckNo", "MainLinkFile", "MainLinkFile2", "BankId", "BankId2", "Rdo", "FromDate", "ToDate"}, {0, 0, 0, 0, 0, 0, 0, 0, 0, "1900-1-1", bm.ToStrDate(ToDate.SelectedDate)})
        dt.TableName = "tbl"
        dv.Table = dt
        DataGridView1.ItemsSource = dv
        DataGridView1.IsReadOnly = True
        DataGridView1.Columns(dt.Columns("Flag").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ROWNUMBER").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("LinkFile").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("CheckTypeName").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ROWNUMBER2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("LinkFile2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("CheckNo2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("MainValue2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("CurrencyName2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("MyDate").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("MyDateName").Ordinal).Visibility = Windows.Visibility.Hidden

        DataGridView1.Columns(dt.Columns("CheckNo").Ordinal).Header = "رقم الشيك"
        DataGridView1.Columns(dt.Columns("LinkFileName").Ordinal).Header = "الجهة - أصل الشيك"
        DataGridView1.Columns(dt.Columns("AccNo").Ordinal).Header = "الفرعي"
        DataGridView1.Columns(dt.Columns("AccName").Ordinal).Header = "اسم الفرعي"
        DataGridView1.Columns(dt.Columns("MainValue").Ordinal).Header = "المبلغ"
        DataGridView1.Columns(dt.Columns("CurrencyName").Ordinal).Header = "العملة"
        DataGridView1.Columns(dt.Columns("LinkFileName2").Ordinal).Header = "الجهة - المكان الحالي"
        DataGridView1.Columns(dt.Columns("AccNo2").Ordinal).Header = "الفرعي"
        DataGridView1.Columns(dt.Columns("AccName2").Ordinal).Header = "اسم الفرعي"
        DataGridView1.Columns(dt.Columns("CheckTypeName2").Ordinal).Header = "حالة الشيك"

    End Sub

    Sub GridFilter() Handles TypeId.SelectionChanged, CheckTypeId.SelectionChanged, CheckNo.TextChanged, MainLinkFile.SelectionChanged, MainLinkFile2.SelectionChanged, BankId.TextChanged, BankId2.TextChanged, Rdo1.Checked, Rdo2.Checked, Rdo3.Checked, FromDate.SelectedDateChanged
        Try
            If DataGridView1.ItemsSource Is Nothing Then Return
            DataGridView2.ItemsSource = Nothing
            dv.RowFilter = "1=1"
            If TypeId.SelectedIndex > 0 Then dv.RowFilter &= " and Flag='" & TypeId.SelectedIndex & "'"
            If CheckTypeId.SelectedIndex > 0 Then dv.RowFilter &= " and CheckTypeName2='" & CheckTypeId.Items(CheckTypeId.SelectedIndex)(1) & "'"
            If CheckNo.Text.Trim <> "" Then dv.RowFilter &= " and CheckNo like '%" & CheckNo.Text & "%'"
            If MainLinkFile.SelectedIndex > 0 Then dv.RowFilter &= " and LinkFile='" & MainLinkFile.SelectedValue & "'"
            If MainLinkFile2.SelectedIndex > 0 Then dv.RowFilter &= " and LinkFile2='" & MainLinkFile2.SelectedValue & "'"
            If BankId.Text.Trim <> "" Then dv.RowFilter &= " and AccNo like '%" & BankId.Text & "%'"
            If BankId2.Text.Trim <> "" Then dv.RowFilter &= " and AccNo2 like '%" & BankId2.Text & "%'"
            If Rdo1.IsChecked Then
                dv.RowFilter &= " and [تاريخ التسجيل]>='" & bm.ToStrDate(FromDate.SelectedDate) & "' and [تاريخ التسجيل]<='" & bm.ToStrDate(ToDate.SelectedDate) & "'"
                'DataGridView1.CanUserSortColumns
            ElseIf Rdo2.IsChecked Then
                dv.RowFilter &= " and [تاريخ الاستحقاق]>='" & bm.ToStrDate(FromDate.SelectedDate) & "' and [تاريخ الاستحقاق]<='" & bm.ToStrDate(ToDate.SelectedDate) & "'"
            Else
                dv.RowFilter &= " and [تاريخ آخر حركة]>='" & bm.ToStrDate(FromDate.SelectedDate) & "' and [تاريخ آخر حركة]<='" & bm.ToStrDate(ToDate.SelectedDate) & "' and CheckTypeName2='محصل'"
            End If
        Catch ex As Exception
            Dim s As String = ex.Message
        End Try

        Try
            If CheckTypeId.Items(CheckTypeId.SelectedIndex)(1) = "محصل" Then
                DataGridView1.Columns(dt.Columns("تاريخ آخر حركة").Ordinal).Header = "تاريخ التحصيل"
            ElseIf CheckTypeId.Items(CheckTypeId.SelectedIndex)(1) = "دفعة من حساب شيك" Then
                DataGridView1.Columns(dt.Columns("تاريخ آخر حركة").Ordinal).Header = "تاريخ الدفعة"
            ElseIf CheckTypeId.Items(CheckTypeId.SelectedIndex)(1) = "مرتد" Then
                DataGridView1.Columns(dt.Columns("تاريخ آخر حركة").Ordinal).Header = "تاريخ الارتداد"
            ElseIf CheckTypeId.Items(CheckTypeId.SelectedIndex)(1) = "تم التسليم" Then
                DataGridView1.Columns(dt.Columns("تاريخ آخر حركة").Ordinal).Header = "تاريخ التسليم"
            Else
                DataGridView1.Columns(dt.Columns("تاريخ آخر حركة").Ordinal).Header = "تاريخ آخر حركة"
            End If
        Catch ex As Exception
        End Try

        Try
            If Rdo1.IsChecked Then
                dv.Sort = "[تاريخ التسجيل]"
            ElseIf Rdo2.IsChecked Then
                dv.Sort = "[تاريخ الاستحقاق]"
            Else
                dv.Sort = "[تاريخ آخر حركة]"
            End If
        Catch ex As Exception

        End Try
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
        Catch
        End Try
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile.SelectedValue & "," & Md.UserName & ")") Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub MainLinkFile_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile.LostFocus
        BankId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub BankId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId2.LostFocus
        Try
            If Val(BankId2.Text.Trim) = 0 Then
                BankId2.Clear()
                BankName2.Clear()
                Return
            End If

            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile2.SelectedValue)
            bm.LostFocus(BankId2, BankName2, "select Name from Fn_EmpPermissions(" & MainLinkFile2.SelectedValue & "," & Md.UserName & ") where Id=" & BankId2.Text.Trim())
        Catch
        End Try
    End Sub
    Private Sub BankId2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId2.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MainLinkFile2.SelectedValue)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId2, BankName2, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MainLinkFile2.SelectedValue & "," & Md.UserName & ")") Then
            BankId2_LostFocus(Nothing, Nothing)
        End If
    End Sub

    Private Sub MainLinkFile2_LostFocus(sender As Object, e As RoutedEventArgs) Handles MainLinkFile2.LostFocus
        BankId2_LostFocus(Nothing, Nothing)
    End Sub

    Dim lop As Boolean = False

    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.Sort = dt.Columns(sender.Tag).ColumnName
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            dv.RowFilter = " 1=1"
            Dim i As Integer = 0
            For x As Integer = 0 To dt.Columns.Count - 1
                Select Case dt.Columns(x).ColumnName
                    Case "ROWNUMBER", "LinkFile", "CheckTypeName", "ROWNUMBER2", "LinkFile2", "CheckNo2", "MainValue2", "CurrencyName2"
                        Continue For
                End Select
                dv.RowFilter &= " and [" & dt.Columns(x).ColumnName & "] like '%" & MyTextBoxes(i).Text & "%' "
                i += 1
            Next
        Catch
        End Try
    End Sub


    Private Sub LoadResource()

    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGridView1.SelectionChanged
        Try

            dt2 = bm.ExecuteAdapter("GetCheckMotionSub", New String() {"CheckNo"}, {DataGridView1.CurrentItem("CheckNo")})
            dt2.TableName = "tbl"

            dv2.Table = dt2
            DataGridView2.ItemsSource = dv2
            DataGridView2.IsReadOnly = True

            'DataGridView2.Columns(dt2.Columns("StoreId").Ordinal).Visibility = Windows.Visibility.Hidden
            'DataGridView2.Columns(dt2.Columns("Flag").Ordinal).Visibility = Windows.Visibility.Hidden
            'DataGridView2.Columns(dt2.Columns("ToId").Ordinal).Visibility = Windows.Visibility.Hidden
            'DataGridView2.Columns(dt2.Columns("Title").Ordinal).Visibility = Windows.Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("MainLinkFile").Ordinal).Visibility = Windows.Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("LinkFile").Ordinal).Visibility = Windows.Visibility.Hidden
            DataGridView2.Columns(dt2.Columns("ROWNUMBER").Ordinal).Visibility = Windows.Visibility.Hidden
            'DataGridView2.Columns(dt2.Columns("SF_Name").Ordinal).Header = "نوع الحركة"
            'DataGridView2.Columns(dt2.Columns("StoreName").Ordinal).Header = "المخزن"
            'DataGridView2.Columns(dt2.Columns("InvoiceNo").Ordinal).Header = "المسلسل"
            'Try : DataGridView2.Columns(dt2.Columns("ToName").Ordinal).Header = dt2.Rows(0)("Title") : Catch : End Try
            'DataGridView2.Columns(dt2.Columns("ToName").Ordinal).Header = "الجهة"
        Catch
        End Try
    End Sub


    Private Sub btnPrint1_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint1.Click, btnPrint11.Click
        Dim rpt As New ReportViewer
        rpt.Header = CType(Parent, Page).Title
        Dim Rdo As Integer = 0
        If Rdo1.IsChecked Then
            Rdo = 1
        ElseIf Rdo2.IsChecked Then
            Rdo = 2
        Else
            Rdo = 3
        End If

        rpt.paraname = New String() {"@IsRPT", "@TypeId", "@CheckTypeName2", "@CheckNo", "@MainLinkFile", "@MainLinkFile2", "@BankId", "@BankId2", "@Rdo", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {1, TypeId.SelectedIndex, CheckTypeId.Items(CheckTypeId.SelectedIndex)(1), CheckNo.Text, MainLinkFile.SelectedValue, MainLinkFile2.SelectedValue, Val(BankId.Text), Val(BankId2.Text), Rdo, bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate), CType(Parent, Page).Title}
        rpt.Rpt = "CheckMotion.rpt"
        If sender Is btnPrint11 Then rpt.Rpt = "CheckMotion2.rpt"
        rpt.Show()
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        Try
            Dim rpt As New ReportViewer
            rpt.Header = CType(Parent, Page).Title
            rpt.paraname = New String() {"@CheckNo", "Header"}
            rpt.paravalue = New String() {DataGridView1.SelectedItem("CheckNo"), CType(Parent, Page).Title}
            rpt.Rpt = "CheckMotionSub.rpt"
            rpt.Show()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ToDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles ToDate.SelectedDateChanged
        LoadGrid()
        GridFilter()
    End Sub

End Class