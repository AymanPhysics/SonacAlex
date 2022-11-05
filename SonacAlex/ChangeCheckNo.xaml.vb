Public Class ChangeCheckNo
    Dim bm As New BasicMethods
    Public AllowChange As Boolean = False
    Public txtCheck As New TextBox
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        If CheckNo.Text = "" Then
            CheckNo.Focus()
            Return
        End If
        If NewCheckNo.Text = "" Then
            NewCheckNo.Focus()
            Return
        End If 

        bm.ExecuteNonQuery("update BankCash_G2 set CheckNo='" & NewCheckNo.Text & "' where CheckNo='" & CheckNo.Text & "'")
        txtCheck.Text = NewCheckNo.Text
         
        CheckNo.Clear()
        NewCheckNo.Clear()
        CheckNo.Focus()
        bm.ShowMSG("CheckNo Changed Successfully.")
        AllowChange = True
        Try
            CType(Parent, Window).Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, "Save")
        lblCheckNo.SetResourceReference(Label.ContentProperty, "CheckNo")
        lblNewCheckNo.SetResourceReference(Label.ContentProperty, "New CheckNo")
    End Sub

    Public MyCheckNo As String = ""
    Private Sub ChangePassword_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        CheckNo.Text = MyCheckNo
    End Sub
End Class
