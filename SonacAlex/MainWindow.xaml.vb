Imports System.Drawing
Imports System.Data
Imports System.IO
Imports System.Windows.Controls.Primitives
Imports System.Xml

Class MainWindow
    Dim bm As New BasicMethods
    Public Nlvl As Boolean = False
    Dim bol As Boolean = False
    Dim Copy As Boolean = False

    Public WMP As New WMPLib.WindowsMediaPlayer
    Private Sub MainWindow_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        'Application.Current.MainWindow.SizeToContent = Windows.SizeToContent.WidthAndHeight
        'Application.Current.MainWindow.SizeToContent = Windows.SizeToContent.WidthAndHeight
        'Topmost = True
        BringIntoView()
        'Dim s As String = bm.Decrypt("otJ8kXBQS1NqkmfDT1lDNQ==")
    End Sub

    Private Sub MainWindow_Deactivated(sender As Object, e As EventArgs) Handles Me.Deactivated
        'Topmost = False
    End Sub

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        SaveSetting("SonacAlex", "Lang", "LayoutType", layoutSwitcher.SelectedIndex)
        If Nlvl Or bol Then Return
        If Copy = True Then
            bol = True
            Application.Current.Shutdown()
            Exit Sub
        End If
        bm.ClearTemp()
        If bm.ShowDeleteMSG("MsgExit") Then
            bol = True
            Md.FourceExit = True
            Application.Current.Shutdown()
        Else
            e.Cancel = True
            'Me.BringIntoView()
        End If
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try

            ''TestingApi

            ''Dim taxapi As New TaxApi
            ''1. Login as Taxpayer System
            'taxapi.Login()
            ''2. Get Document Types
            'taxapi.GetDocumentTypes()
            ''3. Get Document Type
            'taxapi.GetDocumentTypes(1)
            ''4. Get Document Type Version
            'taxapi.GetDocumentTypeVersion(1, 1)
            ''5.1 Submit Documents (JSON)
            'taxapi.SubmitDocuments()
            ''6. Cancel Document
            'taxapi.CancelDocument("WP4S67BQTSS88054ZPYQAD4F10", "Wrong buyer details or Wrong invoice details")
            ''7. Reject Document
            'taxapi.RejectDocument("WP4S67BQTSS88054ZPYQAD4F10", "Wrong buyer details or Wrong invoice details")
            ''8. Get Recent Documents
            'taxapi.GetRecentDocuments()
            ''10. Get Package Requests
            'taxapi.GetPackageRequests()
            ''11. Get Document Package
            'taxapi.GetDocumentPackage("DocumentPackageUniqueID As String")
            ''12. Get Document
            'taxapi.GetDocument("WP4S67BQTSS88054ZPYQAD4F10")
            ''13. Get Submission
            'taxapi.GetSubmission("SubmissionUniqueID As String")
            ''15. Get Document Printout (PDF)
            'taxapi.GetDocumentPrintout("WP4S67BQTSS88054ZPYQAD4F10")
            ''18. Get Notifications
            'taxapi.GetNotifications()
            ''20. Get Document Details
            'taxapi.GetNotifications("DocumentUniqueID As String")
            ''21. Decline Cancel Document
            'taxapi.DeclineCancelDocument("DocumentUniqueID As String")
            ''22. Decline Rejection Document
            'taxapi.DeclineRejectionDocument("DocumentUniqueID As String")
            ''23. Create EGS Code Usage
            'taxapi.CreateEGSCodeUsage("EGS", "EG-940570:  10000006", "EG-267222203-5598542", "Test Code", "كود تجريبي", "activeFrom:  2021-05-21T00:00:00.000", "activeTo: 2021-10-22T00:00:00.000", "description: Description of code", "descriptionAr: وصف الكود بالعربي", "requestReason: Request reason text")
            ''24. Search my EGS code usage requests
            'taxapi.SearchEGSCodeUsageRequests()


            ''25. Request Code Reuse
            'taxapi.RequestCodeReuse("EGS", "EG-267222203-11010102", "Request reason text")


            ''26. Search Published Codes

            ''27. Get Code Details By Item Code

            ''27. Get Code Details By Item Code
            'taxapi.GetCodeDetailsByItemCode("EG-267222203-11010102", "ItemCode As String")
            ''28. Update EGS Code Usage
            'taxapi.GetCodeDetailsByItemCode("EG-267222203-11010101")
            ''29. Update Code
            'taxapi.UpdateCode("EG-267222203-11010101", "ItemCode As String")


            'Application.Current.Shutdown()

            ''''''''''''''''''''''''''''''''''''''''''''''''




            LoadResource()

            If Not Md.MyProjectType = ProjectType.SonacAlex Then
                MyMenu.Visibility = Windows.Visibility.Collapsed
            End If
            MyMenu.Visibility = Windows.Visibility.Collapsed


            Try
                Dim frm As New Window With {.WindowState = WindowState.Minimized, .ShowInTaskbar = False}
                frm.Show()
                frm.Content = WMP
                frm.Hide()
            Catch ex As Exception

            End Try

            layoutSwitcher.SelectedIndex = Val(GetSetting("SonacAlex", "Lang", "LayoutType"))
            langSwitcher.SelectedIndex = Val(GetSetting("SonacAlex", "Lang", "En"))
            'SaveSetting("SonacAlex", "Lang", "En", GetSetting("SonacAlex", "Lang", "En"))
            'layoutSwitcher.Visibility = Windows.Visibility.Hidden


            Select Case Md.MyProjectType
                Case ProjectType.SonacAlex
                    Md.AllowMultiCurrencyPerSubAcc = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.SonacAlex
                    Md.ShowBankCash_G = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.PCs
                    Md.ShowGridAccNames = False
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.SonacAlex
                    Md.ShowBankCash_GAccNo_Not_LinkFile = True
            End Select

            Select Case Md.MyProjectType
                Case ProjectType.SonacAlex
                    Md.HideSubAccNo = True
            End Select

            If Not LoadConnection() Then Return
            bm = New BasicMethods()
            LoadBarcodePrinter()
            LoadPonePrinter()

            If Not MyProjectType = ProjectType.PCs Then
                If Md.Demo Then
                    bm.TestDemo()
                Else
                    bm.TestProtection()
                End If
            End If

            Dim v As Integer = Val(bm.ExecuteScalar("select LastVersion from LastVersion"))
            If v > Md.LastVersion Or v = 0 Then
                bm.ShowMSG("MsgLastVersion")
                Application.Current.Shutdown()
            End If

            If Md.LastVersion > v Then
                bm.ExecuteNonQuery("delete from LastVersion insert into LastVersion (LastVersion) select " & Md.LastVersion)
            End If

            bm.ClearTemp()
            Md.CompanyName = bm.ExecuteScalar("select CompanyName from Statics")
            Md.CompanyTel = bm.ExecuteScalar("select CompanyTel from Statics")
            btnChangeLanguage.Visibility = Windows.Visibility.Hidden
            'langSwitcher.Visibility = Windows.Visibility.Hidden
            Dim L As New Login

            For i As Integer = 0 To langSwitcher.Items.Count - 1
                Try
                    Md.MyDictionaries.Items.Add(New ResourceDictionary With {.Source = New Uri(TryCast(TryCast(langSwitcher.Items(i), XmlElement).Attributes("Dic"), XmlAttribute).Value, UriKind.Relative)})
                Catch ex As Exception
                End Try
            Next
            langSwitcher_SelectionChanged(Nothing, Nothing)


            bm.SetImage(L.Img, "Login.jpg")

            LoadTabs(L)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub LoadTabs(G As Object)
        Try
            MainGrid.Children.Clear()
            MainGrid.Children.Add(New Frame With {.Content = G})
        Catch ex As Exception
        End Try
    End Sub

    Public Sub AddTabOLD(ByVal M As MenuItem, ByVal L As UserControl)
        Dim Tab As New TabItem
        Tab.Header = M.Header
        Tab.Name = "Tab" & M.Name
        Tab.Content = L
        For Each it As TabItem In TabControl1.Items
            If it.Name = Tab.Name Then
                Tab = it
                TabControl1.SelectedItem = Tab
                Return
            End If
        Next
        TabControl1.Items.Add(Tab)
        TabControl1.SelectedItem = Tab
    End Sub

    'Add new tab --> mahmoud
    Public Sub AddTAB(ByVal M As MenuItem, ByVal UserCtrl As UserControl, Optional ByVal HaveClose As Boolean = True)
        Dim TabName As String = M.Name
        Dim TabHeader As String = M.Header
        Dim MW As MainWindow = Application.Current.MainWindow
        Dim TI As TabItem
        For I As Integer = 0 To MW.TabControl1.Items.Count - 1
            TI = MW.TabControl1.Items(I)
            If TI.Name = TabName Then
                TI.Focus()
                Exit Sub
            End If
        Next
        TI = New TabItem
        If HaveClose Then
            TI.Header = New TabsHeader With {.MyTabHeader = TabHeader, .MyTabName = TabName, .WithClose = Visibility.Visible}
        Else
            TI.Header = New TabsHeader With {.MyTabHeader = TabHeader, .MyTabName = TabName, .WithClose = Visibility.Hidden}
        End If
        Try
            CType(TI.Header, TabsHeader).Grid1.Children.Add(M.Icon)
        Catch ex As Exception
        End Try
        TI.Name = TabName
        TI.Content = UserCtrl
        MW.TabControl1.Items.Add(TI)
        TI.Focus()
    End Sub

    Function LoadConnectionOLD() As Boolean
        If con.State = ConnectionState.Open Then Return True
        Dim st As New StreamReader(Md.UdlName & ".udl")
        Dim s As String = ""
        st.ReadLine()
        st.ReadLine()
        s += st.ReadLine
        con.ConnectionString = s.Substring(20)
        Dim cb As New SqlClient.SqlConnectionStringBuilder(con.ConnectionString)
        Dim f As New Form1
        'con.ConnectionString = "Data Source=" & cb.DataSource & ";Initial Catalog=" & cb.InitialCatalog & ";Persist Security Info=True;User ID=" & cb.UserID & ";Password=" & cb.Password 'f.Password 
        con.ConnectionString = cb.ConnectionString
        Try
            con.Open()
        Catch ex As Exception
            bm.ShowMSG("Connection failed")
            bol = True
            Md.FourceExit = True
            Application.Current.Shutdown()
            Return False
        End Try
        Return True
    End Function


    Function LoadConnection() As Boolean
        If con.State = ConnectionState.Open Then Return True

        If Md.UdlName = "" Then
            For Each myfile As String In Directory.GetFiles(System.Windows.Forms.Application.StartupPath)
                If myfile.ToLower.EndsWith(".udldll") Then
                    Md.UdlName = myfile.ToLower.Replace(".udldll", "").Split("\").Last
                End If
            Next
            If Md.UdlName = "" Then
                Dim frm As New EditConnection
                frm.Show()
                frm.Hide()
                frm.AccYear.Text = Md.UdlName

                frm.ServerName.Text = con.DataSource
                frm.Database.Text = con.Database
                frm.ShowDialog()
                If Not frm.Ok Then
                    Application.Current.Shutdown()
                    Return False
                End If
            End If
        End If

        Dim st As New StreamReader(Md.UdlName & ".udldll")
        Dim s As String = st.ReadLine
        st.Close()
        st.Dispose()
        If con.State = ConnectionState.Open Then con.Close()
        con.ConnectionString = bm.Decrypt(s)
        Dim cb As New SqlClient.SqlConnectionStringBuilder(con.ConnectionString)
        'Dim f As New Form1
        'con.ConnectionString = "Data Source=" & cb.DataSource & ";Initial Catalog=" & cb.InitialCatalog & ";Persist Security Info=True;User ID=" & cb.UserID & ";Password=" & cb.Password 'f.Password 
        con.ConnectionString = cb.ConnectionString
        con = New SqlClient.SqlConnection(cb.ConnectionString)

        Try
            con.Open()
            con.Close()
        Catch ex As Exception
            bm.ShowMSG("Connection failed")
            bol = True
            'Md.FourceExit = True
            'Application.Current.Shutdown()

            Dim frm As New EditConnection
            frm.Show()
            frm.Hide()
            frm.AccYear.Text = Md.UdlName
            frm.ServerName.Text = cb.DataSource
            frm.Database.Text = cb.InitialCatalog

            frm.ShowDialog()

            Return False
        End Try
        Return True
    End Function


    Sub LoadBarcodePrinter()
        Md.BarcodePrinter = GetSetting("SonacAlex", "Settings", "BarcodePrinter")
        If Md.BarcodePrinter <> "" Then Return
        Try
            Dim st As New StreamReader("BarcodePrinter.dll")
            Md.BarcodePrinter = st.ReadLine
            st.Close()
        Catch ex As Exception
        End Try
    End Sub

    Sub LoadPonePrinter()
        Md.PonePrinter = GetSetting("SonacAlex", "Settings", "PonePrinter")
        If Md.PonePrinter <> "" Then Return
        Try
            Dim st As New StreamReader("PonePrinter.dll")
            Md.PonePrinter = st.ReadLine
            st.Close()
        Catch ex As Exception
        End Try
    End Sub
    Public LogedIn As Boolean = False
    Public Flag As Integer = 1


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnExit.Click
        Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogout.Click
        Try
            If Not bm.ShowDeleteMSG("MsgExit") Then Return
            Forms.Application.Restart()
            Application.Current.Shutdown()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnChangeLanguage_Click(sender As Object, e As RoutedEventArgs) Handles btnChangeLanguage.Click
        'If GetSetting("SonacAlex", "Lang", "En", True) = False Then
        '    FlowDirection = Windows.FlowDirection.LeftToRight
        '    Md.DictionaryCurrent = Md.DictionaryEn
        '    SaveSetting("SonacAlex", "Lang", "En", True)
        'Else
        '    FlowDirection = Windows.FlowDirection.RightToLeft
        '    Md.DictionaryCurrent = Md.DictionaryAr
        '    SaveSetting("SonacAlex", "Lang", "En", False)
        'End If
        Resources = Md.DictionaryCurrent
        Banner1.Resources = Md.DictionaryCurrent
        Banner2.Resources = Md.DictionaryCurrent
        If MainGrid.Children(0).GetType.ToString = "System.Windows.Controls.Frame" Then CType(MainGrid.Children(0), Frame).Refresh()

    End Sub

    Private Sub LoadResource()
        'Md.DictionaryAr.Source = New Uri("Dic_Ar.xaml", UriKind.Relative)
        'Md.DictionaryEn.Source = New Uri("Dic_En.xaml", UriKind.Relative)

        btnChangeLanguage.SetResourceReference(Button.ContentProperty, "Ar-En")
        btnLogout.SetResourceReference(Button.ContentProperty, "Logout")
        btnExit.SetResourceReference(Button.ContentProperty, "ExitApp")
    End Sub


    Private Sub MainWindow_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.Key = System.Windows.Input.Key.Enter Then
                'e.Handled = True
                If FocusManager.GetFocusedElement(Me).GetType = GetType(Button) Then Return
                If FocusManager.GetFocusedElement(Me).GetType = GetType(Forms.Integration.WindowsFormsHost) Then Return
                If FocusManager.GetFocusedElement(Me).GetType = GetType(TextBox) Then
                    If CType(FocusManager.GetFocusedElement(Me), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then Return
                End If
                Dim c As Control = FocusManager.GetFocusedElement(Me)
                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) With {.RoutedEvent = Keyboard.KeyDownEvent})
                e.Handled = True
                'c.Focus()

                InputManager.Current.ProcessInput(New KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Enter) With {.RoutedEvent = Keyboard.KeyDownEvent})
                'If FocusManager.GetFocusedElement(Me).GetType = GetType(TextBox) AndAlso Not CType(FocusManager.GetFocusedElement(Me), TextBox).VerticalScrollBarVisibility = ScrollBarVisibility.Visible Then CType(FocusManager.GetFocusedElement(Me), TextBox).SelectAll()
            End If
        Catch
        End Try
    End Sub

    Private Sub btnMinimize_Click(sender As Object, e As RoutedEventArgs) Handles btnMinimize.Click
        WindowState = Windows.WindowState.Minimized
    End Sub

    Private Sub langSwitcher_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles langSwitcher.SelectionChanged
        Try
            Dim e1 As XmlElement = TryCast(langSwitcher.SelectedItem, XmlElement)
            If e1 IsNot Nothing Then
                Md.DictionaryCurrent = Md.MyDictionaries.Items(langSwitcher.SelectedIndex)
                FlowDirection = TryCast(e1.Attributes("FlowDirection"), XmlAttribute).Value

                Resources = Md.DictionaryCurrent
                Banner1.Resources = Md.DictionaryCurrent
                Banner2.Resources = Md.DictionaryCurrent
                If MainGrid.Children(0).GetType.ToString = "System.Windows.Controls.Frame" Then CType(MainGrid.Children(0), Frame).Refresh()
                SaveSetting("SonacAlex", "Lang", "En", langSwitcher.SelectedIndex)
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub layoutSwitcher_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles layoutSwitcher.SelectionChanged
        If layoutSwitcher.SelectedIndex = 1 Then
            WindowStyle = Windows.WindowStyle.ThreeDBorderWindow
        Else
            WindowStyle = Windows.WindowStyle.None
            WindowState = Windows.WindowState.Minimized
            WindowState = Windows.WindowState.Maximized
        End If
    End Sub
End Class
