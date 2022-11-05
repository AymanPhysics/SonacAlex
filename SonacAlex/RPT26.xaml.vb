Imports System.Data

Public Class RPT26

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@FromId", "@ToId", "Header"}
        rpt.paravalue = New String() {FromAccNo.SelectedValue, ToAccNo.SelectedValue, CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "Chart.rpt"
            Case 2
                rpt.Rpt = "Analysis.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        If Flag = 2 Then
            bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis where SubType=1 union all select 0 Id,'-' Name order by Id", FromAccNo)
            bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Analysis where SubType=1 union all select 0 Id,'-' Name order by Id", ToAccNo)
        Else
            bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id", FromAccNo)
            bm.FillCombo("select Id,cast(Id as nvarchar(100))+' - '+Name Name from Chart where SubType=1 union all select 0 Id,'-' Name order by Id", ToAccNo)
        End If
    End Sub

    Dim lop As Boolean = False
    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")

    End Sub

End Class