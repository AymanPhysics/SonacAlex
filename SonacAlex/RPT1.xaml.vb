Imports System.Data

Public Class RPT1
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        'If Flag = 5 AndAlso Val(CaseId.Text) = 0 Then
        'bm.ShowMSG("Please, Select a Patient")
        'CaseId.Focus()
        'Return
        'End If

        bm.Addcontrol_MouseDoubleClick({CaseId})

        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If VisitingType.SelectedValue Is Nothing Then VisitingType.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then UserId.SelectedIndex = 0
        If SerialType.SelectedValue Is Nothing Then SerialType.SelectedIndex = 0


        Dim rpt As New ReportViewer
        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        rpt.paraname = New String() {"@EmpId", "@CaseId", "@CsId", "@VisitingType", "@FromDate", "@ToDate", "@FromId", "@ToId", "Header", "@UserId", "@FromSerialId", "@ToSerialId", "@Canceled", "@SerialType", "@All", "@IsReservations", "@IsServices", "@ServiceGroupId", "@ServiceTypeId", "@Returned", "@CompanyId", "@DepartmentId", "@FromHH", "@FromMM", "@ToHH", "@ToMM", "@ShowZeroValues"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Val(CaseId.Text), Val(UserId.SelectedValue.ToString()), Val(VisitingType.SelectedValue.ToString), FromDate.SelectedDate, ToDate.SelectedDate, Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, Val(UserId.SelectedValue), Val(FromSerialId.Text), Val(ToSerialId.Text), Canceled.SelectedIndex.ToString, SerialType.SelectedValue.ToString, 0, 1, 1, 0, 0, Returned.SelectedIndex.ToString, 0, 0, 0, 0, 0, 0, 0}


        Select Case Flag
            Case 1
                rpt.Rpt = "DailyReservations.rpt"
            Case 2
                rpt.Rpt = "DailyReservationsDetailed.rpt"
            Case 3
                rpt.Rpt = "ReservationsDepartments.rpt"
            Case 4
                rpt.Rpt = "ReservationsDoctors.rpt"
            Case 5
                rpt.Rpt = "ReservationsDepartments2.rpt"
            Case 6
                rpt.Rpt = "ReservationsDepartments2S1.rpt"
            Case 7
                rpt.Rpt = "ReservationsDepartments2S2.rpt"
            Case 8
                rpt.Rpt = "ReservationsDepartments2S3.rpt"
            Case 9
                rpt.Rpt = "ReservationsDepartments2S4.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        If Flag = 1 Then
            Canceled.Visibility = Windows.Visibility.Hidden
            lblCanceled.Visibility = Windows.Visibility.Hidden
            Returned.Visibility = Windows.Visibility.Hidden
            lblReturned.Visibility = Windows.Visibility.Hidden
        ElseIf Flag = 2 Then
            lblUsername.Visibility = Windows.Visibility.Hidden
            UserId.Visibility = Windows.Visibility.Hidden
            
            lblFromSerialId.Visibility = Windows.Visibility.Hidden
            FromSerialId.Visibility = Windows.Visibility.Hidden
            lblToSerialId.Visibility = Windows.Visibility.Hidden
            ToSerialId.Visibility = Windows.Visibility.Hidden

            lblSerialType.Visibility = Windows.Visibility.Hidden
            SerialType.Visibility = Windows.Visibility.Hidden
            lblVisitingType.Visibility = Windows.Visibility.Hidden
            VisitingType.Visibility = Windows.Visibility.Hidden

            Canceled.Visibility = Windows.Visibility.Hidden
            lblCanceled.Visibility = Windows.Visibility.Hidden
            Returned.Visibility = Windows.Visibility.Hidden
            lblReturned.Visibility = Windows.Visibility.Hidden
        ElseIf Flag = 3 OrElse Flag = 4 OrElse Flag = 6 OrElse Flag = 7 OrElse Flag = 8 OrElse Flag = 9 Then
            lblDoctor.Visibility = Windows.Visibility.Hidden
            EmpId.Visibility = Windows.Visibility.Hidden
            lblPatient.Visibility = Windows.Visibility.Hidden
            CaseId.Visibility = Windows.Visibility.Hidden
            CaseName.Visibility = Windows.Visibility.Hidden

            lblUsername.Visibility = Windows.Visibility.Hidden
            UserId.Visibility = Windows.Visibility.Hidden
            lblFromResId.Visibility = Windows.Visibility.Hidden
            FromInvoice.Visibility = Windows.Visibility.Hidden
            lblToResId.Visibility = Windows.Visibility.Hidden
            ToInvoice.Visibility = Windows.Visibility.Hidden

            lblFromSerialId.Visibility = Windows.Visibility.Hidden
            FromSerialId.Visibility = Windows.Visibility.Hidden
            lblToSerialId.Visibility = Windows.Visibility.Hidden
            ToSerialId.Visibility = Windows.Visibility.Hidden

            lblSerialType.Visibility = Windows.Visibility.Hidden
            SerialType.Visibility = Windows.Visibility.Hidden
            lblVisitingType.Visibility = Windows.Visibility.Hidden
            VisitingType.Visibility = Windows.Visibility.Hidden
        End If

        bm.FillCombo("SerialTypes", SerialType, "")

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where SystemUser='1' and Stopped='0' union select 0 Id,'-' Name order by Name", UserId)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' union select 0 Id,'-' Name order by Name", EmpId)
        EmpId.SelectedValue = Md.UserName
        If EmpId.SelectedIndex < 0 Then EmpId.SelectedIndex = 0
        Canceled.SelectedIndex = 2
        Returned.SelectedIndex = 2
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub

    Private Sub SerialType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles SerialType.SelectionChanged
        Try
            bm.FillCombo("VisitingTypes", VisitingType, "where (SerialId=" & SerialType.SelectedValue.ToString & " or " & SerialType.SelectedValue.ToString & "=0)")
        Catch ex As Exception
        End Try
    End Sub


    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select EnName Name from Cases where Id=" & CaseId.Text.Trim())
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblVisitingType.SetResourceReference(Label.ContentProperty, "VisitingType")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
        lblDoctor.SetResourceReference(Label.ContentProperty, "Doctor")
        lblFromResId.SetResourceReference(Label.ContentProperty, "From Res. Id")
        lblToResId.SetResourceReference(Label.ContentProperty, "To Res. Id")
        lblPatient.SetResourceReference(Label.ContentProperty, "Patient")
        lblUsername.SetResourceReference(Label.ContentProperty, "Username")
        lblFromSerialId.SetResourceReference(Label.ContentProperty, "From Serial Id")
        lblToSerialId.SetResourceReference(Label.ContentProperty, "To Serial Id")
        lblCanceled.SetResourceReference(Label.ContentProperty, "Canceled")
        lblReturned.SetResourceReference(Label.ContentProperty, "Returned")
        lblSerialType.SetResourceReference(Label.ContentProperty, "Serial Type")

        bm.ResetComboboxContent(Canceled)
        bm.ResetComboboxContent(Returned)
    End Sub

End Class