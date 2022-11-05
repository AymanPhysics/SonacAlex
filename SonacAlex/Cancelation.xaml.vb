Imports System.Data

Public Class Cancelation
    Dim dt As New DataTable
    Dim bm As New BasicMethods
    Public Flag As Integer = 1

    Dim m As MainWindow = Application.Current.MainWindow
    
    Public Sub Cancelation_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        txtYear.Text = bm.MyGetDate().Year
        bm.FillCombo("SerialTypes", CboMain, "")
        bm.FillCombo("VisitingTypes", VisitingType, "")
        bm.FillCombo("Companies", CompanyId, "")
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees union select 0 Id,'-' Name order by Name", EmpIdReservation)

        CaseId.IsEnabled = False
        If Flag = 3 Then
            CboMain.Visibility = Windows.Visibility.Hidden
            lblId.Visibility = Windows.Visibility.Hidden
        Else
            TabControl2.Visibility = Windows.Visibility.Hidden
        End If
    End Sub



    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtID.Text.Trim = "" Then
            txtID.Focus()
            Return
        End If
        If CboMain.Visibility = Windows.Visibility.Visible AndAlso CboMain.SelectedIndex = 0 Then
            CboMain.Focus()
            Return
        End If

        Dim dt As DataTable = bm.ExecuteAdapter("GetCancleSerialId", {"Year", "SerialType", "SerialId"}, {Val(txtYear.Text), CboMain.SelectedValue.ToString, Val(txtID.Text)})
        If dt.Rows.Count = 0 Then
            bm.ShowMSG("Invalid Id")
            Return
        ElseIf dt.Rows(0)(0) = 1 Then
            bm.ShowMSG("Already Canceled")
            Return
        End If

        dt.Clear()
        dt = bm.ExecuteAdapter("GetRefundSerialId", {"Year", "SerialType", "SerialId"}, {Val(txtYear.Text), CboMain.SelectedValue.ToString, Val(txtID.Text)})
        If dt.Rows.Count = 0 Then
            bm.ShowMSG("Invalid Id")
            Return
        ElseIf dt.Rows(0)(0) = 1 Then
            bm.ShowMSG("Already Refund")
            Return
        End If

        If Flag = 3 Then
            dt = bm.ExecuteAdapter("CreateNewReservations", {"YEAR", "SerialId", "UserName", "Value", "Payed", "Remaining", "Notes"}, {Val(txtYear.Text), Val(txtID.Text), Md.UserName, Val(Value.Text), Val(Payed.Text), Val(Remaining.Text), MyNotes.Text.Trim})
            If dt.Rows.Count > 0 Then
                bm.ShowMSG("تم تسجيل مسلسل " & dt.Rows(0)("ReservId").ToString & " لنفس التاريخ بسريال رقم " & dt.Rows(0)("SerialId").ToString)
            End If
        End If

        bm.ExecuteNonQuery(IIf(Flag = 1 Or Flag = 3, "CancleSerialId", "RefundSerialId"), {"Year", "SerialType", "SerialId", "EmpId"}, {Val(txtYear.Text), CboMain.SelectedValue.ToString, Val(txtID.Text), Md.UserName})
        If Flag <> 3 Then bm.ShowMSG("Saved Successfuly")
        txtID.Clear()
        Clear()
    End Sub

    Sub ClearControls()
        bm.SetNoImage(Image1)
        txtID.Clear()
        CboMain.Focus()
    End Sub

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If Flag = 3 Then
            Clear()
            Dim dt As DataTable = bm.ExecuteAdapter("select * from Reservations where Year(DayDate)='" & Val(txtYear.Text) & "' and SerialId='" & txtID.Text & "'")
            EmpIdReservation.SelectedValue = Md.UserName
            If dt.Rows.Count = 1 Then

                lblReservId.Content = dt.Rows(0)("ReservId").ToString
                DayDate.Content = dt.Rows(0)("DayDate").ToString

                lblTime.Content = dt.Rows(0)("Time").ToString
                CaseId.Text = dt.Rows(0)("CaseId").ToString
                CaseName.Text = dt.Rows(0)("CaseName").ToString
                CaseID_LostFocus(Nothing, Nothing)
                MyNotes.Text = dt.Rows(0)("MyNotes").ToString

                VisitingType.SelectedValue = dt.Rows(0)("VisitingType").ToString
                Value.Text = dt.Rows(0)("Value").ToString
                Payed.Text = dt.Rows(0)("Payed").ToString
                Remaining.Text = dt.Rows(0)("Remaining").ToString

                VisitingType.IsEnabled = False
                
                EmpIdReservation.SelectedValue = dt.Rows(0)("EmpIdReservation").ToString

            End If
            CaseId.Focus()

        End If
    End Sub

    Sub Clear()
        DayDate.Content = ""
        lblReservId.Content = ""
        lblTime.Content = ""

        CaseId.Clear()
        CaseName.Clear()
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        CaseID_LostFocus(Nothing, Nothing)

        CompanyId.SelectedIndex = 0
        MyNotes.Clear()

        VisitingType.SelectedIndex = 0
        Value.Clear()
        Payed.Clear()
        Remaining.Clear()
        EmpIdReservation.SelectedIndex = 0

        CaseId.IsEnabled = True
        VisitingType.IsEnabled = True
        Value.IsEnabled = True
        Payed.IsEnabled = True

    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, CboMain.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(Button.ContentProperty, IIf(Flag = 1, "Invoice Cancelation", "Invoice Refund"))

        lblId.SetResourceReference(Label.ContentProperty, "Serial Type")
        lblName.SetResourceReference(Label.ContentProperty, "Serial")
        lblYear.SetResourceReference(Label.ContentProperty, "Year")

        TabItemCurrentReservation.SetResourceReference(TabItem.HeaderProperty, "Current Reservation")

        lblDate.SetResourceReference(Label.ContentProperty, "Date")
        lblReserv.SetResourceReference(Label.ContentProperty, "Reserve")
        lblTim.SetResourceReference(Label.ContentProperty, "Time")
        lblPatient.SetResourceReference(Label.ContentProperty, "Patient")
        lblVisitingType.SetResourceReference(Label.ContentProperty, "VisitingType")
        lblValue.SetResourceReference(Label.ContentProperty, "Value")
        lblPayed.SetResourceReference(Label.ContentProperty, "Payed")
        lblRemaining.SetResourceReference(Label.ContentProperty, "Remaining")

    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown, Payed.KeyDown, Remaining.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub Value_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Value.TextChanged, Payed.TextChanged
        Remaining.Text = Val(Value.Text) - Val(Payed.Text)
    End Sub


    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select " & Resources.Item("CboName") & " Name from Cases where Id=" & CaseId.Text.Trim())
        CaseId.ToolTip = ""
        CaseName.ToolTip = ""
        CompanyId.SelectedIndex = 0
        Dim dt As DataTable = bm.ExecuteAdapter("select HomePhone,Mobile,CompanyId from Cases where Id=" & CaseId.Text.Trim())
        If dt.Rows.Count > 0 Then
            CompanyId.SelectedValue = Val(dt.Rows(0)("CompanyId").ToString)
            CaseId.ToolTip = Resources.Item("Id") & ": " & CaseId.Text & vbCrLf & Resources.Item("Name") & ": " & CaseName.Text & vbCrLf & Resources.Item("HomePhone") & ": " & dt.Rows(0)("HomePhone").ToString & vbCrLf & Resources.Item("Mobile") & ": " & dt.Rows(0)("Mobile").ToString
            CaseName.ToolTip = CaseId.ToolTip
        End If
    End Sub


End Class
