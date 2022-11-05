Imports System.Data

Public Class Contacts
    Dim bm As New BasicMethods
    Public Statement As String = "select CallerId,CallerId2,CallerId3,CallerName,ContactGroupId,dbo.GetContactGroupName(ContactGroupId)ContactGroupName,ContactTypeId,dbo.GetContactTypeName(ContactGroupId,ContactTypeId)ContactTypeName,Address1,Address2,Email,Line from Contacts"
    Dim MyTextBoxes() As Control = {}

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = ""
    Public LinkFile As Integer
    Public Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {})
        LoadResource()
        btnNew.IsEnabled = False

        btnbtnEdit.IsEnabled = Md.Manager
        btnDelete.IsEnabled = Md.Manager

        Banner1.StopTimer = True
        Banner1.Header = Header
        bm.FillCombo("ContactGroups", ContactGroupId, "")
        txt_TextChanged(ContactGroupId, Nothing)
        dt = bm.ExecuteAdapter(Statement)
        dt.TableName = "tbl"
        DataGridView1.Foreground = System.Windows.Media.Brushes.Black
        DataGridView1.MinColumnWidth = 100
        dv.Table = dt
        If Not DataGridView1.ItemsSource Is Nothing Then
            DataGridView1.ItemsSource = Nothing
        End If
        DataGridView1.IsReadOnly = True
        DataGridView1.ItemsSource = dv

        Try
            AddBinding(CallerId, 0)
            AddBinding(CallerId2, 1)
            AddBinding(CallerId3, 2)
            AddBinding(CallerName, 3)
            AddBinding(ContactGroupId, 5)
            AddBinding(ContactTypeId, 7)
            AddBinding(Address1, 8)
            AddBinding(Address2, 9)
            AddBinding(Email, 10)
            DataGridView1.Columns(4).Visibility = Windows.Visibility.Hidden
            DataGridView1.Columns(6).Visibility = Windows.Visibility.Hidden
            DataGridView1.Columns(11).Visibility = Windows.Visibility.Hidden
            
            DataGridView1.Columns(0).Header = "التليفون 1"
            DataGridView1.Columns(1).Header = "التليفون 2"
            DataGridView1.Columns(2).Header = "التليفون 3"
            DataGridView1.Columns(3).Header = "الاسم"
            DataGridView1.Columns(5).Header = "المجموعة"
            DataGridView1.Columns(7).Header = "النوع"
            DataGridView1.Columns(8).Header = "عنوان 1"
            DataGridView1.Columns(9).Header = "عنوان 2"
            DataGridView1.Columns(10).Header = "البريد الاليكتروني"
            DataGridView1.SelectedIndex = 0
        Catch
        End Try

        DataGridView1.SelectedIndex = dv.Table.Rows.Count - 1
        txt_TextChanged(Nothing, Nothing)
    End Sub

    Sub AddBinding(txt As TextBox, i As Integer)
        Dim binding As New Binding("ActualWidth")
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        binding.Source = DataGridView1.Columns(i)
        txt.SetBinding(TextBox.WidthProperty, binding)
    End Sub

    Sub AddBinding(txt As ComboBox, i As Integer)
        Dim binding As New Binding("ActualWidth")
        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        binding.Source = DataGridView1.Columns(i)
        txt.SetBinding(ComboBox.WidthProperty, binding)
    End Sub

    Private Sub txt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CallerId.GotFocus, CallerName.GotFocus, ContactGroupId.GotFocus, ContactTypeId.GotFocus, Address1.GotFocus, Address2.GotFocus, Email.GotFocus
        Try
            dv.Sort = DataGridView1.Columns(sender.Tag).Header
        Catch
        End Try
    End Sub

    Private Sub txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CallerId.TextChanged, CallerId2.TextChanged, CallerId3.TextChanged, CallerName.TextChanged, ContactGroupId.LostFocus, ContactTypeId.LostFocus, Address1.TextChanged, Address2.TextChanged, Email.TextChanged
        Try
            If sender Is ContactGroupId Then
                bm.FillCombo("ContactTypes", ContactTypeId, " where ContactGroupId=" & Val(ContactGroupId.SelectedValue))
            End If

            dv.RowFilter = " (CallerId like '%" & CallerId.Text & "%' or CallerId2 like '%" & CallerId.Text & "%' or CallerId3 like '%" & CallerId.Text & "%' ) "
            dv.RowFilter &= " and (CallerId like '%" & CallerId2.Text & "%' or CallerId2 like '%" & CallerId2.Text & "%' or CallerId3 like '%" & CallerId2.Text & "%' ) "
            dv.RowFilter &= " and (CallerId like '%" & CallerId3.Text & "%' or CallerId2 like '%" & CallerId3.Text & "%' or CallerId3 like '%" & CallerId3.Text & "%' ) "
            dv.RowFilter &= " and CallerName like '%" & CallerName.Text & "%' "
            If ContactGroupId.SelectedIndex > 0 Then dv.RowFilter &= " and ContactGroupId =" & Val(ContactGroupId.SelectedValue)
            If ContactTypeId.SelectedIndex > 0 Then dv.RowFilter &= " and ContactTypeId =" & Val(ContactTypeId.SelectedValue)
            dv.RowFilter &= " and Address1 like '%" & Address1.Text & "%' "
            dv.RowFilter &= " and Address2 like '%" & Address2.Text & "%' "
            dv.RowFilter &= " and Email like '%" & Email.Text & "%' "

        Catch
        End Try
    End Sub

    Public SelectedId As String = 0
    Public SelectedName As String = ""
    Public SelectedRow As System.Data.DataRowView
    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = System.Windows.Input.Key.Enter Then
                SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
                SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
                SelectedRow = DataGridView1.Items(DataGridView1.SelectedIndex)
                'Close()
            ElseIf e.Key = Input.Key.Escape Then
                'Close()
            ElseIf e.Key = Input.Key.Up Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex - 1
                DataGridView1.ScrollIntoView(DataGridView1.SelectedItem)
            ElseIf e.Key = Input.Key.Down Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex + 1
                DataGridView1.ScrollIntoView(DataGridView1.SelectedItem)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DataGridView1_ColumnHeaderDragCompleted(sender As Object, e As Primitives.DragCompletedEventArgs) Handles DataGridView1.ColumnHeaderDragCompleted
        MessageBox.Show(6)
    End Sub


    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            SelectedId = DataGridView1.Items(DataGridView1.SelectedIndex)(0).ToString
            SelectedName = DataGridView1.Items(DataGridView1.SelectedIndex)(1).ToString
            SelectedRow = DataGridView1.Items(DataGridView1.SelectedIndex)
            ShowCallCenter()
        Catch ex As Exception
        End Try
    End Sub

    Sub ShowCallCenter()
        Dim MyContent As New CallCenter With {.MyCallerId = SelectedId}
        Dim frm As New Window With {.Title = SelectedId, .Content = MyContent, .WindowState = WindowState.Maximized}
        frm.Topmost = True
        frm.Show()
    End Sub


    Private Sub LoadResource()
    End Sub

    Private Sub t_Tick(sender As Object, e As EventArgs)
        If Not sender Is Nothing Then CType(sender, System.Windows.Threading.DispatcherTimer).Stop()

        'Dim CurrentActualWidth As Integer = 0

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        Try
            If CallerName.Text.Trim = "" Then Return
            If CurrentLine = 0 AndAlso DataGridView1.Items.Count > 0 Then Return
            CallerName.Text = CallerName.Text.Trim

            CallerId.Text = CallerId.Text.Replace("'", "''")
            CallerId2.Text = CallerId2.Text.Replace("'", "''")
            CallerId3.Text = CallerId3.Text.Replace("'", "''")
            CallerName.Text = CallerName.Text.Replace("'", "''")
            Address1.Text = Address1.Text.Replace("'", "''")
            Address2.Text = Address2.Text.Replace("'", "''")
            Email.Text = Email.Text.Replace("'", "''")

            Dim str As String = ""
            If CurrentLine = 0 Then
                str = "insert Contacts(CallerId,CallerId2,CallerId3,CallerName,ContactGroupId,ContactTypeId,Address1,Address2,Email,UserName,MyGetDate) values('" & CallerId.Text.Trim & "','" & CallerId2.Text.Trim & "','" & CallerId3.Text.Trim & "','" & CallerName.Text.Trim & "','" & Val(ContactGroupId.SelectedValue) & "','" & Val(ContactTypeId.SelectedValue) & "','" & Address1.Text.Trim & "','" & Address2.Text.Trim & "','" & Email.Text.Trim & "'," & Md.UserName & ",GETDATE())"
            Else
                str = "update Contacts set CallerId='" & CallerId.Text.Trim & "',CallerId2='" & CallerId2.Text.Trim & "',CallerId3='" & CallerId3.Text.Trim & "',CallerName='" & CallerName.Text.Trim & "',ContactGroupId='" & ContactGroupId.SelectedValue & "',ContactTypeId='" & ContactTypeId.SelectedValue & "',Address1='" & Address1.Text.Trim & "',Address2='" & Address2.Text.Trim & "',Email='" & Email.Text.Trim & "',UserName=" & Md.UserName & ",MyGetDate=GetDate() where Line=" & CurrentLine
            End If

            If Not bm.ExecuteNonQuery(Str) Then Return
            btnNew_Click(Nothing, Nothing)
        Catch
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Try
            If bm.ShowDeleteMSG Then
                bm.ExecuteNonQuery("delete Contacts where Line=" & DataGridView1.Items(DataGridView1.SelectedIndex)(11))
                Window_Loaded(Nothing, Nothing)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Dim CurrentLine As Integer = 0
    Private Sub btnbtnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnbtnEdit.Click
        btnbtnEdit.IsEnabled = False
        btnNew.IsEnabled = True

        Try
            CurrentLine = DataGridView1.Items(DataGridView1.SelectedIndex)(11)
            CallerId.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
            CallerId2.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
            CallerId3.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(2)
            CallerName.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(3)
            ContactGroupId.SelectedValue = DataGridView1.Items(DataGridView1.SelectedIndex)(4)
            txt_TextChanged(ContactGroupId, Nothing)
            ContactTypeId.SelectedValue = DataGridView1.Items(DataGridView1.SelectedIndex)(6)
            Address1.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(8)
            Address2.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(9)
            Email.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(10)

        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As RoutedEventArgs) Handles btnNew.Click
        btnbtnEdit.IsEnabled = True
        btnNew.IsEnabled = False

        CurrentLine = 0
        CallerId.Clear()
        CallerId2.Clear()
        CallerId3.Clear()
        CallerName.Clear()
        ContactGroupId.SelectedIndex = 0
        ContactTypeId.SelectedIndex = 0
        Address1.Clear()
        Address2.Clear()
        Email.Clear()
        Window_Loaded(Nothing, Nothing)
    End Sub
End Class