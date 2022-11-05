Imports System.Data

Public Class RPT27

    Public MyEndType As Integer = 0
    Public IsIncomeStatement As Integer = 0

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Level.SelectedIndex < 1 Then
            bm.ShowMSG("برجاء تحديد المستوى")
            Level.Focus()
            Return
        End If

        Dim MyId As Integer = 0
        If IsCostCenter.IsChecked Then
            MyId = Val(bm.ExecuteScalar("select isnull((select max(Id) from CostCenterIds),0)+1"))
            If MyId = 0 Then
                bm.ShowMSG("Network Error, Please Try Agian ...")
                Return
            End If
            Dim ss As String = "declare @Id bigint=" & MyId.ToString
            Dim Chk As CheckBox
            For Each nn As TreeViewItem In TreeView1.Items(0).Items
                If nn.Name = "" Then Continue For
                Chk = nn.Header
                If Chk.IsChecked Then
                    ss &= " insert CostCenterIds(Id,CostCenterId) select @Id," & nn.Tag
                End If
                GetSubNode(nn, ss)
            Next

            bm.ExecuteNonQuery(ss)
        End If
        Dim rpt As New ReportViewer

        rpt.paraname = New String() {"@EndType", "@Level", "@IsIncomeStatement", "@FromDate", "@ToDate", "@IsCostCenter", "@MyId", "Header"}
        rpt.paravalue = New String() {MyEndType, Level.SelectedIndex, IsIncomeStatement, FromDate.SelectedDate, ToDate.SelectedDate, IIf(IsCostCenter.IsChecked, 1, 0), MyId, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "AccountEnd.rpt"
            Case 2, 3
                rpt.Rpt = "AccountEnd2.rpt"
        End Select
        rpt.Show()
    End Sub


    Sub GetSubNode(ByVal nn As TreeViewItem, ByRef ss As String)
        Dim Chk As CheckBox
        For Each nn2 As TreeViewItem In nn.Items
            Chk = nn2.Header
            If Chk.IsChecked Then
                ss &= " insert CostCenterIds(Id,CostCenterId) select @Id," & nn2.Tag
            End If
            GetSubNode(nn2, ss)
        Next
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        FillLevel()

        LoadTree()
        IsCostCenter_Unchecked(Nothing, Nothing)

        If Not Md.ShowCostCenter Then IsCostCenter.Visibility = Windows.Visibility.Hidden

        Select Case Flag
            Case 2, 3
                'lblLevel.Visibility = Windows.Visibility.Hidden
                'Level.Visibility = Windows.Visibility.Hidden
        End Select
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblToDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
    End Sub

    Private Sub FillLevel()
        Try
            Level.Items.Clear()
            Dim x As Integer = Val(bm.ExecuteScalar("select min(Level) from chart where SubType=1"))
            Dim y As Integer = Val(bm.ExecuteScalar("select max(Level) from chart where SubType=1"))
            Level.Items.Add("-")
            For i As Integer = 1 To x
                Level.Items.Add(New ComboBoxItem With {.Content = i})
            Next
            For i As Integer = x + 1 To y
                Level.Items.Add(New ComboBoxItem With {.Content = i, .Background = System.Windows.Media.Brushes.Red})
            Next
            Level.SelectedIndex = x
        Catch
        End Try
    End Sub


    Sub LoadTree()
        TreeView1.Items.Clear()
        Dim dt As DataTable = bm.ExecuteAdapter("Select * from CostCenters order by Id")
        'TreeView1.Items.Add(New TreeViewItem With {.Header = "مراكز التكلفة"})
        TreeView1.Items.Add(New TreeViewItem With {.Header = New CheckBox With {.Content = "مراكز التكلفة"}})
        AddHandler CType(TreeView1.Items(0).Header, CheckBox).Checked, AddressOf CheckedChanged
        AddHandler CType(TreeView1.Items(0).Header, CheckBox).Unchecked, AddressOf CheckedChanged


        TreeView1.Items(0).Tag = ""
        TreeView1.Items(0).FontSize = 20
        Dim dr() As DataRow = dt.Select("MainCostCenterId=0")
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn As New TreeViewItem
                nn.Foreground = Brushes.DarkRed
                nn.FontSize = 18
                nn.Name = "Node_" & dr(i)("Id")
                nn.Tag = dr(i)("Id")
                nn.Header = dr(i)("Id") & "          " & dr(i)("Name")
                nn.Header = New CheckBox With {.Content = nn.Header}
                TreeView1.Items(0).Items.Add(nn)
                loadNode(dt, nn)
                AddHandler CType(nn.Header, CheckBox).Checked, AddressOf CheckedChanged
                AddHandler CType(nn.Header, CheckBox).Unchecked, AddressOf CheckedChanged
            Catch
            End Try
        Next
        CType(TreeView1.Items(0), TreeViewItem).IsExpanded = True
    End Sub

    Sub loadNode(ByVal dt As DataTable, ByVal nn As TreeViewItem)
        Dim dr() As DataRow = dt.Select("MainCostCenterId=" & nn.Tag)
        For i As Integer = 0 To dr.Length - 1
            Try
                Dim nn2 As New TreeViewItem
                nn2.Foreground = Brushes.DarkBlue
                nn2.FontSize = nn.FontSize - 1
                nn2.Name = "Node_" & dr(i)("Id")
                nn2.Tag = dr(i)("Id")
                nn2.Header = dr(i)("Id") & "          " & dr(i)("Name")
                nn2.Header = New CheckBox With {.Content = nn2.Header}
                nn.Items.Add(nn2)
                loadNode(dt, nn2)
                AddHandler CType(nn2.Header, CheckBox).Checked, AddressOf CheckedChanged
                AddHandler CType(nn2.Header, CheckBox).Unchecked, AddressOf CheckedChanged
            Catch
            End Try
        Next
        nn.IsExpanded = True
    End Sub

    Dim lop As Boolean = False
    Private Sub CheckedChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim ch As CheckBox = sender
        Dim p As TreeViewItem = ch.Parent

        If Not lop Then
            For Each n As TreeViewItem In p.Items
                CType(n.Header, CheckBox).IsChecked = ch.IsChecked
            Next
        End If

        If p.Parent.GetType.ToString = "System.Windows.Controls.TreeViewItem" Then
            lop = True
            Dim PP As TreeViewItem = p.Parent
            If ch.IsChecked Then CType(PP.Header, CheckBox).IsChecked = True
            lop = False
        End If
    End Sub


    Private Sub IsCostCenter_Checked(sender As Object, e As RoutedEventArgs) Handles IsCostCenter.Checked
        PanelGroups.Visibility = Windows.Visibility.Visible
    End Sub

    Private Sub IsCostCenter_Unchecked(sender As Object, e As RoutedEventArgs) Handles IsCostCenter.Unchecked
        PanelGroups.Visibility = Windows.Visibility.Hidden
    End Sub
End Class