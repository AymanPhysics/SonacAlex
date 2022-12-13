' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Threading
Imports System.Data

Namespace EmployeeTracker
    ''' <summary>
    ''' Interaction logic for Banner.xaml
    ''' </summary>
    Partial Public Class Banner
        Inherits UserControl

        Dim bm As New BasicMethods
        Dim dt As DataTable
        Dim t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 0, 100)}
        Dim t2 As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 0, 5)}
        Dim t3 As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 10)}
        Dim t4 As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 10)}
        Dim t5 As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 10)}

        Public Header As String = ""
        Public StopTimer As Boolean = False
        Public Sub New()
            InitializeComponent()
            AddHandler t.Tick, AddressOf Tick
            AddHandler t2.Tick, AddressOf Tick2
            AddHandler t3.Tick, AddressOf Tick3
            AddHandler t4.Tick, AddressOf Tick4
            AddHandler t5.Tick, AddressOf Tick5
            t3.Stop()
        End Sub

        Public Sub Tick()
            If IsLogedIn AndAlso StopTimer Then
                't.Stop()
                t2.Stop()
                'lblMain.Text = Header
                'Return
            End If
            If IsLogedIn AndAlso Not Md.IsExports Then
                t4.Stop()
            End If
            If IsLogedIn AndAlso Not Md.IsExports2 Then
                t5.Stop()
            End If
            Try
                '"                " &
                lblMain.Text = Md.CompanyName.Split(vbCrLf)(0) & " " & Md.UdlName.Replace("Connect", "")
                If ShowShifts AndAlso Not ShowShiftForEveryStore AndAlso IsLogedIn Then
                    lblMain.Text &= "   |   " & Md.CurrentDate.Date.ToShortDateString & "  " & Md.CurrentShiftName & " "
                End If
                If Not IsNothing(Md.ArName) Then
                    lblMain.Text &= "   |   " & Md.Currentpage.Trim & " |  " & Resources.Item("Username") & ": " & IIf(Application.Current.MainWindow.FlowDirection = Windows.FlowDirection.LeftToRight, Md.EnName, Md.ArName)
                End If
                Header = lblMain.Text
                lblMain.FlowDirection = Application.Current.MainWindow.FlowDirection
                lblMain.HorizontalAlignment = Windows.HorizontalAlignment.Center
            Catch ex As Exception
            End Try
        End Sub

        Private Sub Tick2(sender As Object, e As EventArgs)
            If Not Md.IsLogedIn Then Return
            bm.GetCurrent()
            If Not ShowShifts Then
                StopTimer = True
            End If
        End Sub

        Private Sub Tick3(sender As Object, e As EventArgs)
            If Val(Md.UserName) = 0 Then Return
            dt = bm.ExecuteAdapter("GetMsgs", {"UserName"}, {Md.UserName})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim str As String = "تم عمل "
                Dim str2 As String = ""
                Dim id As Integer = 0
                If Md.UserCanRecieve1 AndAlso dt.Rows(i)("Msg").ToString.Split(";")(2) = 14 Then
                    str &= "مردودات المبيعات رقم "
                    str2 = "مردودات المبيعات"
                    id = 549
                ElseIf Md.UserCanRecieve2 AndAlso dt.Rows(i)("Msg").ToString.Split(";")(2) = 8 Then
                    str &= "تحويل إلى مخزن رقم "
                    str2 = "تحويل إلى مخزن"
                    id = 521
                Else
                    Continue For
                End If
                str &= dt.Rows(i)("Msg").ToString.Split(";")(3)
                If bm.ShowDeleteMSG(str, "Open", "Exit") Then
                    Dim c As New Sales With {.Flag = Val(dt.Rows(i)("Msg").ToString.Split(";")(2))}
                    Dim w As New MyWindow With {.Content = c, .Title = str2}

                    w.MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowEdit") = 1
                    w.MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowDelete") = 1
                    w.MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowNavigate") = 1
                    w.MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowPrint") = 1

                    w.Show()
                    c.Sales_Loaded(Nothing, Nothing)
                    w.WindowState = WindowState.Maximized
                    c.StoreId.Text = dt.Rows(i)("Msg").ToString.Split(";")(1)
                    c.StoreId_LostFocus(Nothing, Nothing)
                    c.InvoiceNo.Text = dt.Rows(i)("Msg").ToString.Split(";")(3)
                    c.InvoiceNo_Leave(Nothing, Nothing)
                End If
                bm.ExecuteNonQuery("insert MsgTbl(UserName,Msg) select " & Md.UserName & ",'" & dt.Rows(i)("Msg") & "'")
            Next
        End Sub

        Private Sub Tick4(sender As Object, e As EventArgs)
            If Val(Md.UserName) = 0 Then Return
            Dim id As Integer = 204
            dt = bm.ExecuteAdapter("GetProFormaPaymentsMSG", {"UserName"}, {Md.UserName})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim Str As String = "New Notification for [" & dt.Rows(i)("C_EnName") & " / " & dt.Rows(i)("SubC_Name") & "], Country:[" & dt.Rows(i)("CN_Name") & "], Transfers:[" & dt.Rows(i)("Transfers") & "], Deductions:[" & dt.Rows(i)("Deductions") & "],Description:[" & dt.Rows(i)("Description") & "], Date:[" & dt.Rows(i)("MyGetDate") & "]"
                If bm.ShowDeleteMSG(Str, "Open", "Exit") Then
                    Dim frm As New ProForma With {.Flag = dt.Rows(i)("Flag")}
                    Dim w As New MyWindow With {.Content = frm, .Title = dt.Rows(i)("FlagName")}

                    w.MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowEdit") = 1
                    w.MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowDelete") = 1
                    w.MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowNavigate") = 1
                    w.MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowPrint") = 1
                    w.Show()
                    frm.BasicForm_Loaded(Nothing, Nothing)
                    w.WindowState = WindowState.Maximized
                    frm.CustomerId.Text = dt.Rows(i)("CustomerId")
                    frm.CustomerId_LostFocus(Nothing, Nothing)
                    frm.Flag = dt.Rows(i)("Flag")
                    frm.txtID.Text = dt.Rows(i)("InvoiceNo")
                    frm.txtID_LostFocus(Nothing, Nothing)
                End If
                bm.ExecuteNonQuery("update Employees set ExportsLastLine=" & dt.Rows(i)("Line") & " where Id=" & Md.UserName)
            Next
        End Sub

        Private Sub Tick5(sender As Object, e As EventArgs)
            If Val(Md.UserName) = 0 Then Return
            Dim id As Integer = 204
            dt = bm.ExecuteAdapter("GetProFormaMSG", {"UserName"}, {Md.UserName})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim Str As String = "New Notification for [" & dt.Rows(i)("C_EnName") & " / " & dt.Rows(i)("SubC_Name") & "], Country:[" & dt.Rows(i)("CN_Name") & "], " & dt.Rows(i)("FlagName") & ", Date:[" & dt.Rows(i)("MyGetDate") & "]"
                If bm.ShowDeleteMSG(Str, "Open", "Exit") Then
                    Dim frm As New ProForma With {.Flag = dt.Rows(i)("Flag"), .IsPayments = True}
                    Dim w As New MyWindow With {.Content = frm, .Title = dt.Rows(i)("FlagName")}

                    w.MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowEdit") = 1
                    w.MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowDelete") = 1
                    w.MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowNavigate") = 1
                    w.MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & id).ToList(0)("AllowPrint") = 1
                    w.Show()
                    frm.BasicForm_Loaded(Nothing, Nothing)
                    w.WindowState = WindowState.Maximized
                    frm.CustomerId.Text = dt.Rows(i)("CustomerId")
                    frm.CustomerId_LostFocus(Nothing, Nothing)
                    frm.Flag = dt.Rows(i)("Flag")
                    frm.txtID.Text = dt.Rows(i)("InvoiceNo")
                    frm.txtID_LostFocus(Nothing, Nothing)
                End If
                bm.ExecuteNonQuery("update Employees set Exports2LastLine=" & dt.Rows(i)("Line") & " where Id=" & Md.UserName)
            Next
        End Sub

    End Class
End Namespace
