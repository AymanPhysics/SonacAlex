Imports System.Data

Public Class RPT37
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@CaseId", "@OperationType", "@SurgeonId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), Val(OperationType.SelectedValue), Val(SurgeonId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "OperationsRoomsAll2.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId, SurgeonId, FromDate, ToDate})
         
        bm.FillCombo("OperationTypes", OperationType, " where IsStopped=0")

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

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CaseId.KeyUp
        
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select Name from Cases where Id=" & CaseId.Text.Trim())
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblPatient.SetResourceReference(Label.ContentProperty, "Patient")
    End Sub

    Private Sub SurgeonId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SurgeonId.KeyUp
        If bm.ShowHelp("Doctors", SurgeonId, SurgeonName, e, "select cast(Id as varchar(100)) Id,Name from Employees where Doctor=1") Then
            SurgeonId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub SurgeonId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SurgeonId.LostFocus
        bm.LostFocus(SurgeonId, SurgeonName, "select Name from Employees where Doctor=1 and Id=" & SurgeonId.Text.Trim())
    End Sub

End Class