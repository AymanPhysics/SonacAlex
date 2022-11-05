Imports System.Data

Public Class ChecksTracing

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
        bm.Addcontrol_MouseDoubleClick({})
        dt = bm.ExecuteAdapter("GetCheckMotion")
        dt.TableName = "tbl"
        dv.Table = dt
        DataGridView1.ItemsSource = dv
        DataGridView1.IsReadOnly = True
        DataGridView1.Columns(dt.Columns("ROWNUMBER").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("LinkFile").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("CheckTypeName").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("ROWNUMBER2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("LinkFile2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("CheckNo2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("MainValue2").Ordinal).Visibility = Windows.Visibility.Hidden
        DataGridView1.Columns(dt.Columns("CurrencyName2").Ordinal).Visibility = Windows.Visibility.Hidden

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

        Dim t As New System.Windows.Threading.DispatcherTimer With {.IsEnabled = True, .Interval = New System.TimeSpan(0, 0, 10)}
        AddHandler t.Tick, AddressOf t_Tick
    End Sub

    Public Sub LoadChecks()
        Dim i As Integer = 0
        Dim CurrentActualWidth As Integer = 0
        For x As Integer = 0 To dt.Columns.Count - 1
            Select Case dt.Columns(x).ColumnName
                Case "ROWNUMBER", "LinkFile", "CheckTypeName", "ROWNUMBER2", "LinkFile2", "CheckNo2", "MainValue2", "CurrencyName2"
                    Continue For
            End Select
            Dim txt As New TextBox With {.Height = 30, .Margin = New Thickness(0, 0, 0, 0)}
            ReDim Preserve MyTextBoxes(MyTextBoxes.Length + 1)
            MyTextBoxes(i) = txt
            Dim d = DataGridView1.Columns(x).ActualWidth
            txt.Width = d

            Dim binding As New Binding("ActualWidth")
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            binding.Source = DataGridView1.Columns(x)
            txt.SetBinding(TextBox.WidthProperty, binding)

            CurrentActualWidth += DataGridView1.Columns(x).ActualWidth
            txt.Tag = i
            txt.TabIndex = i
            txt.HorizontalAlignment = Windows.HorizontalAlignment.Left
            txt.VerticalAlignment = Windows.VerticalAlignment.Top
            SC.Children.Add(txt)
            AddHandler txt.GotFocus, AddressOf txt_Enter
            AddHandler txt.TextChanged, AddressOf txt_TextChanged
            i += 1
        Next

        For x As Integer = 0 To DataGridView1.Columns.Count - 1
            DataGridView1.Columns(x).IsReadOnly = True
        Next

        txt_TextChanged(Nothing, Nothing)
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

    Private Sub t_Tick(sender As Object, e As EventArgs)
        CType(sender, System.Windows.Threading.DispatcherTimer).Stop()
        LoadChecks()
    End Sub

End Class