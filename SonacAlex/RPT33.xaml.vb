Imports System.Data

Public Class RPT33
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}


        Select Case Flag
            Case 1
                rpt.Rpt = "KidneysWashMotion0.rpt"
            Case 2
                rpt.Rpt = "KidneysWashMotionItems.rpt"
            Case 3
                If Val(bm.ExecuteScalar("select dbo.GetCaseStatus(" & Val(CaseId.Text) & ")")) = 1 Then
                    bm.ShowMSG("هذا المريض موجود بالفعل")
                    Return
                End If
                bm.ExecuteNonQuery("SetCaseStatus", {"CaseId", "UserName", "InOut"}, {Val(CaseId.Text), Md.UserName, 1})
                bm.ShowMSG("تم تسجيل دخول المريض بنجاح")
                Return
            Case 4
                If Val(bm.ExecuteScalar("select dbo.GetCaseStatus(" & Val(CaseId.Text) & ")")) <> 1 Then
                    bm.ShowMSG("هذا المريض غير موجود بالمستشفى")
                    Return
                End If
                bm.ExecuteNonQuery("SetCaseStatus", {"CaseId", "UserName", "InOut"}, {Val(CaseId.Text), Md.UserName, 2})
                bm.ShowMSG("تم تسجيل خروج المريض بنجاح")
                Return
            Case 5
                bm.ExecuteNonQuery("UpdateDocNoAll", {"FromDate", "ToDate"}, {bm.ToStrDate(FromDate.SelectedDate), bm.ToStrDate(ToDate.SelectedDate)})
                bm.ShowMSG("تمت العملية بنجاح")
                Return
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate


        If Flag = 3 OrElse Flag = 4 Then
            lblFromDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden
            lblToDate.Visibility = Windows.Visibility.Hidden
            ToDate.Visibility = Windows.Visibility.Hidden
        ElseIf Flag = 5 Then
            FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
            lblPatient.Visibility = Windows.Visibility.Hidden
            CaseId.Visibility = Windows.Visibility.Hidden
            CaseName.Visibility = Windows.Visibility.Hidden
        End If

        If Flag = 3 Then
            Button2.Content = "دخول المريض"
        ElseIf Flag = 4 Then
            Button2.Content = "خروج المريض"
        ElseIf Flag = 5 Then
            Button2.Content = "احتساب"
        End If
    End Sub



    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        Select Case Flag
            Case 1, 2
                bm.LostFocus(CaseId, CaseName, "select Name from Cases2 where Id=" & CaseId.Text.Trim())
            Case Else
                bm.LostFocus(CaseId, CaseName, "select Name from Cases where Id=" & CaseId.Text.Trim())
        End Select
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblPatient.SetResourceReference(Label.ContentProperty, "Patient")

    End Sub

End Class