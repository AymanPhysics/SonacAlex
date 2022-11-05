Imports System.Data

Public Class RPT40
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detailed As Integer = 1
    Dim dt As New DataTable

    Public MainTableName As String = ""
    Public Main2TableName As String = ""
    Public Main2MainId As String = "Id"

    Public lblMain_Content As String
    Public lblMain2_Content As String

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@T0_Id", "@T_ID", "@FromDate", "@ToDate", "Header", "@WindowId", "@CostCenterId", "@CostCenterSubId"}
        rpt.paravalue = New String() {CboMain.SelectedValue, CboMain2.SelectedValue, FromDate.SelectedDate, ToDate.SelectedDate, IIf(CboMain2.SelectedIndex < 1, CboMain.Text, CboMain2.Text), Val(WindowId.SelectedValue), Val(CostCenterId.SelectedValue), Val(CostCenterSubId.SelectedValue)}
        Select Case Flag
            Case 1
                rpt.Rpt = "AccountEnd3.rpt"
                If Md.IsMyVeryFruits Then
                    If CboMain.SelectedValue = 3 Then
                        rpt.Rpt = "AccountEnd3_V.rpt"
                    Else
                        rpt.Rpt = "AccountEnd31_V.rpt"
                    End If
                End If
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        bm.FillCombo(MainTableName, CboMain, "")

        bm.FillCombo("Fn_AllWindows()", WindowId, "", , True)
        bm.FillCombo("CostCenters", CostCenterId, "WHERE SubType=1", , True)
        bm.FillCombo("CostCenterSubs", CostCenterSubId, "WHERE SubType=1", , True)

        bm.Addcontrol_MouseDoubleClick({})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblFromDate.SetResourceReference(Label.ContentProperty, "From Date")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")

        lblMain.SetResourceReference(Label.ContentProperty, lblMain_Content)
        lblMain2.SetResourceReference(Label.ContentProperty, lblMain2_Content)

    End Sub

    Private Sub CboMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboMain.SelectionChanged
        Dim s As String = ""
        Try
            s = CboMain.SelectedValue.ToString
        Catch ex As Exception
        End Try
        bm.FillCombo(Main2TableName, CboMain2, " where " & Main2MainId & "='" & s & "'")

    End Sub
End Class