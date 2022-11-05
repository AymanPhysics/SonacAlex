Imports System.Data
Imports System.ComponentModel

Public Class RPT41
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public Detailed As Integer = 1
    Dim dt As New DataTable

    Public MainTableName As String = ""
    Public Main2TableName As String = ""
    Public Main2MainId As String = "Id"

    Public lblMain_Content As String
    Public lblMain2_Content As String

    Dim WithEvents BackgroundWorker1 As New BackgroundWorker

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If ToDate.SelectedDate Is Nothing Then
            bm.ShowMSG("برجاء تحديد التاريخ")
            Return
        End If

        If Val(EntryTypeId.SelectedValue) = 0 Then
            bm.ShowMSG("برجاء تحديد اليومية")
            Return
        End If

        If Val(Amount.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد الرصيد")
            Return
        End If

        Button2.IsEnabled = False
        __DayDate = bm.ToStrDate(ToDate.SelectedDate)
        __EntryTypeId = Val(EntryTypeId.SelectedValue)
        __Amount = Val(Amount.Text)
        BackgroundWorker1.RunWorkerAsync()

    End Sub


    Dim __DayDate, __EntryTypeId, __Amount As String

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        bm.ExecuteNonQuery("CloseCases", {"DayDate", "EntryTypeId", "Amount"}, {__DayDate, __EntryTypeId, __Amount})
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        bm.ShowMSG("تمت العملية بنجاح")
        Button2.IsEnabled = True
    End Sub


    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        bm.FillCombo("GetEmpEntryTypes @Flag=" & 4 & ",@EmpId=" & Md.UserName & "", EntryTypeId)

        bm.Addcontrol_MouseDoubleClick({})

        Dim MyNow As DateTime = bm.MyGetDate()
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
    End Sub


    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        lblToDate.SetResourceReference(Label.ContentProperty, "To Date")


    End Sub


End Class