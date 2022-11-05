Imports System.Data

Public Class RPT24
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        bm.Addcontrol_MouseDoubleClick({CaseId})

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@Id", "Header"}
        rpt.paravalue = New String() {Val(CaseId.Text), CType(Parent, Page).Title}


        Select Case Flag
            Case 1
                rpt.Rpt = "Cases2.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

    End Sub


    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select Name from Cases2 where Id=" & CaseId.Text.Trim())
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblPatient.SetResourceReference(Label.ContentProperty, "Patient")
    
    End Sub

End Class