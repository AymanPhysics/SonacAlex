Imports System.Data
Imports System.ComponentModel

Public Class RPT43
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
        rpt.paraname = New String() {"@FromDocTypeId", "@ToDocTypeId", "@FromSeasonId", "@ToSeasonId", "@FromItemId", "@ToItemId", "@FromAnalysisId", "@ToAnalysisId", "@FromBankId", "@ToBankId", "@FromAccNo", "@ToAccNo", "@FromFarmId", "@ToFarmId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(FromDocTypeId.SelectedValue), Val(ToDocTypeId.SelectedValue), Val(FromSeasonId.SelectedValue), Val(ToSeasonId.SelectedValue), Val(FromItemId.SelectedValue), Val(ToItemId.SelectedValue), Val(FromAnalysisId.SelectedValue), Val(ToAnalysisId.SelectedValue), Val(FromBankId.SelectedValue), Val(ToBankId.SelectedValue), Val(FromAccNo.SelectedValue), Val(ToAccNo.SelectedValue), Val(FromFarmId.SelectedValue), Val(ToFarmId.SelectedValue), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "EntryDt31.rpt"
            Case 2
                rpt.Rpt = "EntryDt32.rpt"
            Case 3
                rpt.Rpt = "EntryDt33.rpt"
        End Select
        rpt.Show()
    End Sub


    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        bm.FillCombo("DocTypes", FromDocTypeId, "")
        bm.FillCombo("Seasons", FromSeasonId, "")
        bm.FillCombo("AgricultureItems", FromItemId, "")
        bm.FillCombo("Analysis", FromAnalysisId, "where SubType=1")
        'bm.FillCombo("Chart", FromAccNo, "where SubType=1 ")
        bm.FillCombo("Banks", FromBankId, "")
        bm.FillCombo("Farms", FromFarmId, "")

        bm.FillCombo("DocTypes", ToDocTypeId, "")
        bm.FillCombo("Seasons", ToSeasonId, "")
        bm.FillCombo("AgricultureItems", ToItemId, "")
        bm.FillCombo("Analysis", ToAnalysisId, "where SubType=1")
        'bm.FillCombo("Chart", ToAccNo, "where SubType=1 ")
        bm.FillCombo("Banks", ToBankId, "")
        bm.FillCombo("Farms", ToFarmId, "")


        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id", FromAccNo)
        bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id", ToAccNo)


        bm.Addcontrol_MouseDoubleClick({})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")


    End Sub


End Class