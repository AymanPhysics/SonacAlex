Imports System.Data

Public Class PhoneIndex

    Dim bm As New BasicMethods
    Public FirstColumn As String = "Tel", SecondColumn As String = "Mobile 1", ThirdColumn As String = "Mobile 2", FourthColumn As String = "Name", TableName As String = "PhoneIndex"

    Dim dt As New DataTable
    Dim dv As New DataView
    Public Header As String = "Contacts"

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {}, {})
        LoadResource()
        Try
            dt = bm.ExecuteAdapter("select Phone1,Phone2,Phone3,Name,Id from PhoneIndex")
            dt.TableName = "tbl"
            dt.Columns(0).ColumnName = FirstColumn
            dt.Columns(1).ColumnName = SecondColumn
            dt.Columns(2).ColumnName = ThirdColumn
            dt.Columns(3).ColumnName = FourthColumn

            
            dv.Table = dt

            DataGridView1.ItemsSource = dv
            DataGridView1.Columns(0).Width = 125
            DataGridView1.Columns(1).Width = 125
            DataGridView1.Columns(2).Width = 125
            DataGridView1.Columns(3).Width = ActualWidth - 500
            DataGridView1.Columns(4).Visibility = Windows.Visibility.Hidden
            DataGridView1.SelectedIndex = 0

            DataGridView1.Columns(0).Header = Resources.Item(FirstColumn)
            DataGridView1.Columns(1).Header = Resources.Item(SecondColumn)
            DataGridView1.Columns(2).Header = Resources.Item(ThirdColumn)
            DataGridView1.Columns(3).Header = Resources.Item(FourthColumn)

        Catch
        End Try
        txtID.Focus()
    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtId2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID2.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtId3_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID3.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = FourthColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtID2.TextChanged, txtID3.TextChanged, txtName.TextChanged

        Dim s As String = ""

        If txtID.Text.Trim <> "" Then s &= " ([" & FirstColumn & "] like '%" & txtID.Text.Replace("'", "''") & "%' or [" & SecondColumn & "] like '%" & txtID.Text.Replace("'", "''") & "%' or [" & ThirdColumn & "] like '%" & txtID.Text.Replace("'", "''") & "%') and "

        If txtID2.Text.Trim <> "" Then s &= " ([" & FirstColumn & "] like '%" & txtID2.Text.Replace("'", "''") & "%' or [" & SecondColumn & "] like '%" & txtID2.Text.Replace("'", "''") & "%' or [" & ThirdColumn & "] like '%" & txtID2.Text.Replace("'", "''") & "%') and "

        If txtID3.Text.Trim <> "" Then s &= " ([" & FirstColumn & "] like '%" & txtID3.Text.Replace("'", "''") & "%' or [" & SecondColumn & "] like '%" & txtID3.Text.Replace("'", "''") & "%' or [" & ThirdColumn & "] like '%" & txtID3.Text.Replace("'", "''") & "%') and "

        s &= " [" & FourthColumn & "] like '%" & txtName.Text.Replace("'", "''") & "%'"

        dv.RowFilter = s

    End Sub

    Public SelectedId As Integer = 0
    Public SelectedName As String = ""

    Private Sub DataGridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                DataGridView1.SelectedIndex = DataGridView1.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        If txtIDx.Text.Trim = "" AndAlso txtID2x.Text.Trim = "" AndAlso txtID3x.Text.Trim = "" Then
            bm.ShowMSG("Please, Insert Tel")
            Return
        End If
        If txtNamex.Text.Trim = "" Then
            bm.ShowMSG("Please, Insert Name")
            Return
        End If

        If Not bm.ExecuteNonQuery("Insert PhoneIndex(Phone1,Phone2,Phone3,Name) select '" & txtIDx.Text.Replace("'", "''") & "','" & txtID2x.Text.Replace("'", "''") & "','" & txtID3x.Text.Replace("'", "''") & "','" & txtNamex.Text.Replace("'", "''") & "'") Then Return

        btnNew_Click(Nothing, Nothing)
        Dim x As Integer = 0
        For ii As Integer = 0 To DataGridView1.Items.Count - 1
            If DataGridView1.Items(ii)(4) >= x Then x = DataGridView1.Items(ii)(4)
        Next

        For ii As Integer = 0 To DataGridView1.Items.Count - 1
            If DataGridView1.Items(ii)(4) = x Then
                DataGridView1.SelectedItem = DataGridView1.Items(ii)
                Return
            End If
        Next
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        txtIDx.Clear()
        txtID2x.Clear()
        txtID3x.Clear()
        txtNamex.Clear()

        Window_Loaded(Nothing, Nothing)
        DataGridView1.SelectedIndex = dv.Table.Rows.Count - 1
        txtId_TextChanged(Nothing, Nothing)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedIndex < 0 Then
            bm.ShowMSG("Please, Select a Record to be Deleted")
            Return
        End If
        If Not bm.ShowDeleteMSG("Are you Sure you want to Delete [" & DataGridView1.Items(DataGridView1.SelectedIndex)(3) & "]?") Then Return

        bm.ExecuteNonQuery("Delete PhoneIndex where Id=" & DataGridView1.Items(DataGridView1.SelectedIndex)(4))
        btnNew_Click(Nothing, Nothing)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click

        Dim i As Integer = -1
        Try
            i = DataGridView1.SelectedIndex
        Catch ex As Exception
        End Try
        If i = -1 Then
            bm.ShowMSG("Please, Select a Record to be Updated")
            Return
        End If

        If txtIDx.Text.Trim = "" AndAlso txtID2x.Text.Trim = "" AndAlso txtID3x.Text.Trim = "" Then
            bm.ShowMSG("Please, Insert Tel")
            Return
        End If
        If txtNamex.Text.Trim = "" Then
            bm.ShowMSG("Please, Insert Name")
            Return
        End If


        Dim x As Integer = DataGridView1.Items(i)(4)
        If Not bm.ExecuteNonQuery("Update PhoneIndex set Phone1='" & txtIDx.Text.Replace("'", "''") & "',Phone2='" & txtID2x.Text.Replace("'", "''") & "',Phone3='" & txtID3x.Text.Replace("'", "''") & "',Name='" & txtNamex.Text.Replace("'", "''") & "' where Id=" & DataGridView1.Items(i)(4)) Then Return


        btnNew_Click(Nothing, Nothing)

        For ii As Integer = 0 To DataGridView1.Items.Count - 1
            If DataGridView1.Items(ii)(4) = x Then
                DataGridView1.SelectedItem = DataGridView1.Items(ii)
                Return
            End If
        Next
    End Sub


    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles DataGridView1.MouseDoubleClick
        Try
            txtIDx.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(0)
            txtID2x.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(1)
            txtID3x.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(2)
            txtNamex.Text = DataGridView1.Items(DataGridView1.SelectedIndex)(3)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        btnDelete.SetResourceReference(Button.ContentProperty, "Delete")
        btnNew.SetResourceReference(Button.ContentProperty, "New")
        Button1.SetResourceReference(Button.ContentProperty, "UpdateSelected")

        
    End Sub

End Class