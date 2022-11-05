Imports System.Data

Public Class RPT39
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@MachineId", "@FromDate", "@ToDate", "Header"}
        rpt.paravalue = New String() {Val(MachineId.Text), FromDate.SelectedDate, ToDate.SelectedDate, CType(Parent, Page).Title}


        Select Case Flag
            Case 1
                rpt.Rpt = "MachinesMotion.rpt"
            Case 2
                rpt.Rpt = "MachinesMotion2.rpt"
          
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({MachineId})

        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)


    End Sub

    Private Sub MachineId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles MachineId.KeyUp
        If bm.ShowHelp("Machines", MachineId, MachineName, e, "select cast(Id as nvarchar(100)),Name from Machines") Then
            MachineId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub MachineId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MachineId.LostFocus
        bm.LostFocus(MachineId, MachineName, "select Name from Machines where Id=" & MachineId.Text.Trim())
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")

    End Sub

End Class