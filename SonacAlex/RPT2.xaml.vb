Imports System.Data

Public Class RPT2
    Public MyLinkFile As Integer = 0
    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Flag2 As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Flag <> 3 AndAlso MainLinkFile.Visibility = Windows.Visibility.Visible AndAlso MainLinkFile.IsEnabled AndAlso MainLinkFile.SelectedIndex = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblMainLinkFile.Content)
            MainLinkFile.Focus()
            Return
        End If


        If SubAccNo.Visibility = Windows.Visibility.Visible AndAlso MyLinkFile = 0 AndAlso Val(SubAccNo.Text) = 0 AndAlso SubAccNo.IsEnabled AndAlso Flag <> 3 Then
            bm.ShowMSG("برجاء تحديد الحساب الفرعى")
            SubAccNo.Focus()
            Return
        End If

        If MyLinkFile > 0 AndAlso Val(SubAccNo.Text) = 0 AndAlso SubAccNo.Visibility = Windows.Visibility.Visible AndAlso Flag <> 3 Then
            bm.ShowMSG("برجاء تحديد الكود")
            SubAccNo.Focus()
            Return
        End If

        MyLinkFile = MainLinkFile.SelectedValue

        Dim rpt As New ReportViewer
        Dim RPTFlag1 As Integer = 2
        RPTFlag1 = IIf(MyLinkFile = 1, 3, RPTFlag1)
        RPTFlag1 = IIf(MyLinkFile = 13, 4, RPTFlag1)

        rpt.paraname = New String() {"@MainAccNo", "MainAccName", "@SubAccNo", "SubAccName", "@FromDate", "@ToDate", "Header", "@Detailed", "@DetailedInvoice", "@LinkFile", "@ToId", "@RPTFlag1", "@RPTFlag2", "@ActiveOnly", "@HasBalOnly", "@WindowId", "@CostCenterId", "@CostCenterSubId", "@FromMainAccNo", "@ToMainAccNo", "@FromAnalysisId", "@ToAnalysisId", "@FromStatusId", "@ToStatusId"}
        rpt.paravalue = New String() {FromAccNo.SelectedValue, FromAccNo.Text, Val(SubAccNo.Text), SubAccName.Text, FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title.Trim & " " & IIf(MainLinkFile.SelectedIndex > 0, MainLinkFile.Text, ""), 0, 0, MyLinkFile, Val(SubAccNo.Text), RPTFlag1, 0, IIf(Flag2 = 1, 1, 0), 0, Val(WindowId.SelectedValue), Val(CostCenterId.SelectedValue), Val(CostCenterSubId.SelectedValue), FromAccNo.SelectedValue, ToAccNo.SelectedValue, Val(FromAnalysisId.SelectedValue), Val(ToAnalysisId.SelectedValue), Val(FromStatusId.SelectedValue), Val(ToStatusId.SelectedValue)}
        Select Case Flag
            Case 1
                rpt.Rpt = "AccountMotionCurrency.rpt"
                If Md.IsMyVeryFruits Then
                    rpt.Rpt = "AccountMotionCurrency_V.rpt"
                End If
            Case 2
                rpt.Rpt = "AccountMotionBanks2.rpt"
            Case 3
                rpt.Rpt = "Assistant.rpt"
            Case 4
                rpt.Rpt = "AccountMotionCurrency2.rpt"
            Case 5
                rpt.Rpt = "AccountMotionCurrency3.rpt"
            Case 6
                rpt.Rpt = "AccountMotionCurrency4.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({SubAccNo})

        bm.FillCombo("LinkFile", MainLinkFile, "", , True)

        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis where SubType=1 union all select 0 Id,'-' Name order by Id", FromAnalysisId)
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis where SubType=1 union all select 0 Id,'-' Name order by Id", ToAnalysisId)


        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Status union all select 0 Id,'-' Name order by Id", FromStatusId)
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Status union all select 0 Id,'-' Name order by Id", ToStatusId)
         
        bm.FillCombo("Fn_AllWindows()", WindowId, "", , True)

        'bm.FillCombo("CostCenters", CostCenterId, "WHERE SubType=1", , True)
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from CostCenters where SubType=1 union all select 0 Id,'-' Name order by Id", CostCenterId)

        bm.FillCombo("CostCenterSubs", CostCenterSubId, "WHERE SubType=1", , True)

        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id", FromAccNo)
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id", ToAccNo)

        If MyLinkFile = -1 Then
            lblSubAcc.Visibility = Windows.Visibility.Hidden
            SubAccNo.Visibility = Windows.Visibility.Hidden
            SubAccName.Visibility = Windows.Visibility.Hidden

            MyLinkFile = 0
            MainLinkFile.SelectedIndex = 0
            MainLinkFile.Visibility = Windows.Visibility.Hidden
            lblMainLinkFile.Visibility = Windows.Visibility.Hidden
        ElseIf MyLinkFile = 0 Then
            lblFromAccNo.Visibility = Windows.Visibility.Hidden
            FromAccNo.Visibility = Windows.Visibility.Hidden
            lblToAccNo.Visibility = Windows.Visibility.Hidden
            ToAccNo.Visibility = Windows.Visibility.Hidden

            MainLinkFile.SelectedIndex = 0
            MainLinkFile.Visibility = Windows.Visibility.Visible
            lblMainLinkFile.Visibility = Windows.Visibility.Visible
        Else
            MainLinkFile.SelectedValue = MyLinkFile
            MainLinkFile.Visibility = Windows.Visibility.Hidden
            lblMainLinkFile.Visibility = Windows.Visibility.Hidden
        End If


        If Flag = 1 OrElse Flag = 4 OrElse Flag = 5 OrElse Flag = 6 Then
            lblFromAccNo.Content = "من حساب"
        Else

            lblFromAnalysisId.Visibility = Windows.Visibility.Hidden
            FromAnalysisId.Visibility = Windows.Visibility.Hidden
            lblToAnalysisId.Visibility = Windows.Visibility.Hidden
            ToAnalysisId.Visibility = Windows.Visibility.Hidden

            lblStatusId.Visibility = Windows.Visibility.Hidden
            FromStatusId.Visibility = Windows.Visibility.Hidden

            lblStatusId_Copy.Visibility = Windows.Visibility.Hidden
            ToStatusId.Visibility = Windows.Visibility.Hidden

            lblToAccNo.Visibility = Windows.Visibility.Hidden
            ToAccNo.Visibility = Windows.Visibility.Hidden

            WindowId.Visibility = Windows.Visibility.Hidden
            lblWindowId.Visibility = Windows.Visibility.Hidden
            CostCenterId.Visibility = Windows.Visibility.Hidden
            lblCostCenterId.Visibility = Windows.Visibility.Hidden
            CostCenterSubId.Visibility = Windows.Visibility.Hidden
            lblCostCenterSubId.Visibility = Windows.Visibility.Hidden
        End If

        If Flag = 3 Then
        End If
        If MyLinkFile > 0 Then
            lblFromAccNo.Visibility = Windows.Visibility.Hidden
            FromAccNo.Visibility = Windows.Visibility.Hidden
            lblToAccNo.Visibility = Windows.Visibility.Hidden
            ToAccNo.Visibility = Windows.Visibility.Hidden
            lblSubAcc.Visibility = Windows.Visibility.Hidden
            SubAccNo.Visibility = Windows.Visibility.Hidden
            SubAccName.Visibility = Windows.Visibility.Hidden
        End If

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

    End Sub

    Dim lop As Boolean = False


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")
        'lblMainAcc.SetResourceReference(Label.ContentProperty, "Main AccNo")
    End Sub

    Private Sub SubAccNo_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SubAccNo.LostFocus
        If lop OrElse SubAccNo.Visibility <> Windows.Visibility.Visible Then Return
        If MyLinkFile = 0 Then

            dt = bm.ExecuteAdapter("select * from LinkFile where Id='" & MainLinkFile.SelectedValue & "'")
            If dt.Rows.Count = 0 Then Return
            bm.LostFocus(SubAccNo, SubAccName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim())
            bm.LostFocus(SubAccNo, FromAccNo, "select AccNo from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim())
            ToAccNo.SelectedValue = FromAccNo.SelectedValue
        Else
            If Val(SubAccNo.Text) = 0 Then
                SubAccNo.Clear()
                SubAccName.Clear()
                Return
            End If
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
            bm.LostFocus(SubAccNo, SubAccName, "select Name from Fn_EmpPermissions(" & MyLinkFile & "," & Md.UserName & ") where Id=" & SubAccNo.Text.Trim(), , True)
            If MyLinkFile > 0 Then
                'bm.LostFocus(SubAccNo, FromAccNo, "select AccNo from " & dt.Rows(0)("TableName") & " where Id=" & SubAccNo.Text.Trim())
            End If

        End If
    End Sub
    Private Sub SubAccNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SubAccNo.KeyUp
        If MyLinkFile = 0 Then
            dt = bm.ExecuteAdapter("select * from LinkFile where Id='" & MainLinkFile.SelectedValue & "'")
            If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & dt.Rows(0)("Id") & "," & Md.UserName & ")") Then
                SubAccNo_LostFocus(Nothing, Nothing)
            End If
        Else
            dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
            If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), SubAccNo, SubAccName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpPermissions(" & MyLinkFile & "," & Md.UserName & ")") Then
                SubAccNo_LostFocus(Nothing, Nothing)
            End If
        End If
    End Sub


    Private Sub FromAccNo_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles FromAccNo.MouseDoubleClick
        bm.AccNoShowHelpCombo(FromAccNo, e, 1, MyLinkFile, )
    End Sub

    Private Sub ToAccNo_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles ToAccNo.MouseDoubleClick
        bm.AccNoShowHelpCombo(ToAccNo, e, 1, MyLinkFile, )
    End Sub
End Class