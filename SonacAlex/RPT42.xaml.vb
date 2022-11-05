Imports System.Data
Imports System.ComponentModel

Public Class RPT42
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
        rpt.paraname = New String() {"@GuaranteeTypeId", "@ItemId", "@BankId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(GuaranteeTypeId.SelectedValue), Val(ItemId.SelectedValue), Val(BankId.SelectedValue), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}

        Select Case Flag
            Case 1
                rpt.Rpt = "Guarantees.rpt"
        End Select
        rpt.Show()
    End Sub
     

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        bm.FillCombo("GuaranteeTypes", GuaranteeTypeId, "")
        bm.FillCombo("GuaranteeItems", ItemId, "")
        bm.FillCombo("Banks", BankId, "")
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