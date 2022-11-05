Imports System.Data

Public Class RPT03
    Dim bm As New BasicMethods
    Public Flag As Integer = 0

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If ComboBox1.SelectedIndex < 0 Then ComboBox1.SelectedIndex = 0

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Flag", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(ComboBox1.SelectedValue), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "ProductionItemCollectionMotion2.rpt"
        End Select

        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        bm.FillCombo("ProductionItemCollectionMotionFlags", ComboBox1, " where Id>=11", , True)
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub


End Class
