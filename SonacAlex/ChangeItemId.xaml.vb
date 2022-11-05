Public Class ChangeItemId
    Dim bm As New BasicMethods
    Public AllowChange As Boolean = False
    Public txtItemId As New TextBox
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        If ItemId.Text = "" Then
            ItemId.Focus()
            Return
        End If
        If NewItemId.Text = "" Then
            NewItemId.Focus()
            Return
        End If

        bm.ExecuteNonQuery("ChangeItemId", {"OldId", "NewId"}, {ItemId.Text, NewItemId.Text})
        txtItemId.Text = NewItemId.Text

        ItemId.Clear()
        NewItemId.Clear()
        ItemId.Focus()
        bm.ShowMSG("ItemId Changed Successfully.")
        AllowChange = True
        Try
            CType(Parent, Window).Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        lblItemId.SetResourceReference(Label.ContentProperty, "ItemId")
        lblNewItemId.SetResourceReference(Label.ContentProperty, "New ItemId")
    End Sub

    Public MyItemId As String = ""
    Private Sub ChangePassword_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        ItemId.Text = MyItemId
    End Sub
End Class
