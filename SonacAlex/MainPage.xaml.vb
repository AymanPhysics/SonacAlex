' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
Imports System.Data
Imports System.Xml
Imports System.IO.Ports
Imports System.Threading

Partial Public Class MainPage
    Inherits Page
    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}

    Private sampleGridOpacityAnimation As DoubleAnimation
    Private sampleGridTranslateTransformAnimation As DoubleAnimation
    Private borderTranslateDoubleAnimation As DoubleAnimation

    Public Sub New()
        InitializeComponent()

        Dim widthBinding As New Binding("ActualWidth")
        widthBinding.Source = Me

        sampleGridOpacityAnimation = New DoubleAnimation()
        sampleGridOpacityAnimation.To = 0
        sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        sampleGridTranslateTransformAnimation = New DoubleAnimation()
        sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(0.15)
        sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        borderTranslateDoubleAnimation = New DoubleAnimation()
        borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0)

    End Sub
    Private Shared _packUri As New Uri("pack://application:,,,/")

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        borderTranslateDoubleAnimation.From = 0
        borderTranslateDoubleAnimation.To = -ActualWidth
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
        GridSampleViewer_Loaded(Nothing, Nothing)
        Md.Currentpage = ""
    End Sub

    Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If TypeOf args.Source Is RadioButton Then
            Dim theButton As RadioButton = CType(args.Source, RadioButton)

            Dim theFrame
            If TypeOf theButton.Tag Is MyPage Then
                theFrame = CType(theButton.Tag, MyPage)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyPage).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            ElseIf TypeOf theButton.Tag Is Window Then
                theFrame = CType(theButton.Tag, MyWindow)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            End If

            theButton.IsTabStop = False
            CType(args.Source, RadioButton).IsChecked = False

            If TypeOf theButton.Tag Is Window Then
                CType(theFrame, Window).Show()
                CType(theFrame, Window).WindowState = WindowState.Minimized
                CType(theFrame, Window).WindowState = WindowState.Maximized
            ElseIf m.layoutSwitcher.SelectedIndex = 1 Then
                Dim frm As New MyWindow
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    frm.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    frm.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                frm.Content = theButton.Tag
                frm.WindowState = WindowState.Maximized
                frm.Show()
                frm.WindowState = WindowState.Minimized
                frm.WindowState = WindowState.Maximized
            Else
                SampleDisplayFrame.Content = theButton.Tag
                SampleDisplayBorder.Visibility = Visibility.Visible
                Try
                    theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Catch ex As Exception
                End Try
                sampleDisplayFrameLoaded(theFrame, args)
            End If
        End If

    End Sub

    Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)
        If TypeOf sender Is MyWindow Then
            Try
                If Resources.Item(CType(sender, MyWindow).Tag) = "" Then
                    CType(sender, MyWindow).Title = CType(sender, MyWindow).Tag
                Else
                    CType(sender, MyWindow).Title = Resources.Item(CType(sender, MyWindow).Tag)
                End If
                Md.Currentpage = CType(sender, MyWindow).Title
            Catch ex As Exception
            End Try
        ElseIf TypeOf sender Is Page Then
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        ElseIf Not sender Is Nothing AndAlso TypeOf CType(sender, Frame).Content Is Page Then
            Try
                If Resources.Item(CType(CType(sender, Frame).Content, Page).Tag) = "" Then
                    CType(CType(sender, Frame).Content, Page).Title = CType(CType(sender, Frame).Content, Page).Tag
                Else
                    CType(CType(sender, Frame).Content, Page).Title = Resources.Item(CType(CType(sender, Frame).Content, Page).Tag)
                End If
                Md.Currentpage = CType(CType(sender, Frame).Content, Page).Title
            Catch ex As Exception
            End Try
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        End If

        sampleGridTranslateTransformAnimation.To = -ActualWidth
        borderTranslateDoubleAnimation.From = -ActualWidth
        borderTranslateDoubleAnimation.To = 0

        SampleDisplayBorder.Visibility = Visibility.Visible
        SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
        SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
    End Sub

    Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If bm.TestIsLoaded(Me, True) Then Return
        tab.Margin = New Thickness(0)
        tab.HorizontalAlignment = HorizontalAlignment.Stretch
        tab.VerticalAlignment = VerticalAlignment.Stretch
        'tab.Style = FindResource("TabControlLeftStyle")
        'tab.Style = FindResource("OutlookTabControlStyle")

        Load()

        SampleDisplayBorderTranslateTransform.X = -ActualWidth
        SampleDisplayBorder.Visibility = Visibility.Hidden
    End Sub

    Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
        SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
    End Sub

    Dim DynamicMenuitem As Integer = 0
    Dim DtCurrentMenuitem As New DataTable With {.TableName = "T"}
    Sub TestCurrentMenuitem(CurrentMenuitem As Integer)
        If DtCurrentMenuitem.Columns.Count = 0 Then DtCurrentMenuitem.Columns.Add("C")
        If DtCurrentMenuitem.Select("C=" & CurrentMenuitem).Length > 0 Then MessageBox.Show(CurrentMenuitem)
        DtCurrentMenuitem.Rows.Add(CurrentMenuitem)
    End Sub
    Sub LoadLabel(CurrentMenuitem As Integer, ByVal G As WrapPanel, Ttl As String)
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim lbl0 As New Label With {.Height = ActualHeight, .Margin = New Windows.Thickness(24, 0, 0, 0)}
        G.Children.Add(lbl0)

        Dim lbl As New Label With {.Name = "menuitem" & CurrentMenuitem, .FontFamily = New System.Windows.Media.FontFamily("khalaad al-arabeh 2"), .FontSize = 30, .HorizontalContentAlignment = Windows.HorizontalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromArgb(255, 9, 103, 168)), .FontWeight = FontWeight.FromOpenTypeWeight(1), .Height = 90}
        lbl.SetResourceReference(Label.ContentProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            lbl.Content = Ttl
        End If

        G.Children.Add(lbl)
        
        If Ttl = "" Then lbl.Height = 0


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Visibility = Windows.Visibility.Collapsed
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)

            Dim it2 As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it2.IsEnabled = False
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it2)
        End If
    End Sub

    Function LoadRadio(CurrentMenuitem As Integer, ByVal G As WrapPanel, ByVal Ttl As String) As RadioButton
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim RName As String = "menuitem" & CurrentMenuitem
        Dim r As New RadioButton With {.Name = RName, .Style = Application.Current.FindResource("GlassRadioButtonStyle"), .Width = 120, .Height = 70}
        'r.Tag = New Page With {.Content = frm}


        Dim t As New TranslateTextAnimationExample
        t.RealText.Tag = Ttl
        t.RealText.SetResourceReference(TextBlock.TextProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            t.RealText.Text = Ttl
        End If

        r.SetResourceReference(RadioButton.BackgroundProperty, "SC")
        t.SetResourceReference(RadioButton.BackgroundProperty, "SC")

        r.Content = t
        G.Children.Add(r)

        r.SetResourceReference(RadioButton.ToolTipProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            r.ToolTip = Ttl
        End If


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = Ttl, .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Tag = r
            it.SetResourceReference(MenuItem.HeaderProperty, Ttl)
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)
            AddHandler it.Click, AddressOf it_Click
        End If
        Return r
    End Function

    Private Sub it_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As RadioButton = CType(sender.Tag, RadioButton)
            x.RaiseEvent(New RoutedEventArgs(RadioButton.CheckedEvent))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridSampleViewer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
    End Sub

    Private Sub ResizeHeader(G As WrapPanel)
        If Lvl Then Return
        Dim Ttl As String = CType(CType(G.Parent, ScrollViewer).Parent, TabItem).Header
        Try
            While Md.DictionaryCurrent.Item(Ttl).Length < 16
                Md.DictionaryCurrent.Item(Ttl) = " " & Md.DictionaryCurrent.Item(Ttl) & " "
            End While
        Catch
        End Try
    End Sub


    Public Lvl As Boolean = False
    Public Sub Load()

        If MyProjectType = ProjectType.PCs Then
            LoadGPCs(0)
            Return
        End If

        LoadTabs()

        If Not Lvl Then
            dtLevelsMenuitems = bm.ExecuteAdapter("select * from LevelsMenuitems where LevelId=" & Md.LevelId)
            Dim dtLevelsTabs As DataTable = bm.ExecuteAdapter("select * from LevelsTabs where LevelId=" & Md.LevelId)
            If dtLevelsMenuitems.Rows.Count = 0 Then Application.Current.Shutdown()

            For i As Integer = 0 To tab.Items.Count - 1
                Dim item As TabItem = CType(tab.Items(i), TabItem)

                If dtLevelsTabs.Select("Id=" & tab.Items(i).Name.ToString.Replace("tab", "")).Length = 0 Then
                    item.Visibility = Windows.Visibility.Collapsed
                    CType(m.MyMenu.Items(i), MenuItem).Visibility = Windows.Visibility.Collapsed
                End If
                item.Content.Visibility = item.Visibility

                For x As Integer = 0 To CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children.Count - 1
                    If CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(RadioButton) Then
                        Dim t As RadioButton = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), RadioButton)
                        If dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Windows.Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Windows.Visibility.Collapsed
                        End If
                    ElseIf CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(Label) Then
                        Dim t As Label = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), Label)
                        If t.Name = "" Then
                            t.Visibility = Windows.Visibility.Visible
                        ElseIf dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Windows.Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Windows.Visibility.Collapsed
                        End If
                    End If
                Next
            Next

            For i As Integer = 0 To tab.Items.Count - 1
                If CType(tab.Items(i), TabItem).Visibility = Windows.Visibility.Visible Then
                    CType(tab.Items(i), TabItem).IsSelected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Function MakePanel(CurrentTab As Integer, MyHeader As String, ImagePath As String) As WrapPanel
        Dim SV As New MyScrollViewer
        bm.SetImage(SV.Img, ImagePath)
        Dim t As New TabItem With {.Content = SV, .Name = "tab" & CurrentTab, .Header = MyHeader, .Tag = MyHeader}

        'Template.ControlTemplate().Grid().Border().TextBlock()
        'FontFamily="khalaad al-arabeh 2" FontSize="12

        t.Style = FindResource("MyTabItem")
        't.Style = FindResource("OutlookTabItemStyle")
        't.Background = FindResource("OutlookButtonBackground")
        't.Foreground = FindResource("OutlookButtonForeground")

        tab.Items.Add(t)
        Dim G As WrapPanel = SV.MyWrapPanel
        G.Name = "MyWrapPanel" & CurrentTab
        G.AddHandler(System.Windows.Controls.Primitives.ToggleButton.CheckedEvent, New System.Windows.RoutedEventHandler(AddressOf Me.selectedSampleChanged))

        ResizeHeader(G)
        t.SetResourceReference(TabItem.HeaderProperty, t.Header)

        If t.Header Is Nothing Then
            t.Header = MyHeader
        End If

        If Not Lvl Then
            Dim it As New MenuItem With {.Header = MyHeader, .MaxWidth = 150, .Name = "NewMenuItem" & CurrentTab}
            it.Tag = t
            it.SetResourceReference(MenuItem.HeaderProperty, MyHeader)
            m.MyMenu.Items.Add(it)
            AddHandler it.MouseEnter, AddressOf itm_Click
        End If

        Return G
    End Function

    Private Sub itm_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As TabItem = CType(sender.Tag, TabItem)
            x.Focus()
            x.IsSelected = True
            x.BringIntoView()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadGPCs(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "File", "Omega.jpg")

        AddHandler LoadRadio(0, G, "PCs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       Dim frm As New BasicForm With {.TableName = "PCs"}
                                                       bm.SetImage(CType(frm, BasicForm).Img, "password.jpg")
                                                       frm.txtName.MaxLength = 1000
                                                       m.TabControl1.Items.Clear()
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

    End Sub

    Private Sub LoadGFile(CurrentTab As Integer)
        Dim s As String = "buttonscreen.jpg"
        s = "MainOMEGA.jpg"


        Dim G As WrapPanel = MakePanel(CurrentTab, "File", s)
        Dim frm As UserControl

        AddHandler LoadRadio(101, G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               sender.Tag = New MyPage With {.Content = New Employees}
                                                           End Sub


        AddHandler LoadRadio(103, G, "Countries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Countries"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(104, G, "Cities").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm2 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .TableName = "Cities", .MainId = "CountryId", .SubId = "Id", .SubName = "Name"}

                                                                bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub


            AddHandler LoadRadio(105, G, "Areas").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm3 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .Main2TableName = "Cities", .Main2MainId = "CountryId", .Main2SubId = "Id", .Main2SubName = "Name", .lblMain2_Content = "City", .TableName = "Areas", .MainId = "CountryId", .MainId2 = "CityId", .SubId = "Id", .SubName = "Name"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub



        
        
        AddHandler LoadRadio(115, G, "Departments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "Departments"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


        AddHandler LoadRadio(116, G, "Attachment Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "AttachmentTypes"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub


        AddHandler LoadRadio(130, G, "Statics").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Statics
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub




    End Sub

    Private Sub LoadGExports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Exports", s)
        Dim frm As UserControl


        LoadLabel(214, G, "File")


        AddHandler LoadRadio(241, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Customers With {.IsExport = True}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(236, G, "SubCustomers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "SubCustomers"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(242, G, "Items Title").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "ItemTitles"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(243, G, "Grades").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New BasicForm With {.TableName = "Grades"}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(215, G, "Marks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New BasicForm With {.TableName = "Marks"}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(234, G, "Container Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "ContainerTypes"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        'شروط الدفع
        AddHandler LoadRadio(216, G, "Payment Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "PaymentTypes"}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(217, G, "Ports of Loading").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "Ports"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(218, G, "Discharge Ports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Ports2"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        AddHandler LoadRadio(219, G, "Shipping Lines").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm1_2 With {.TableName = "ShippingLines", .lblName2_text = "Transit Time", .SubName2 = "TransitTime"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(220, G, "Shipping Methods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "ShippingMethods"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(221, G, "Couriers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "Couriers"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(222, G, "Currencies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Currencies"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        'AddHandler LoadRadio(115, G, "Freight Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                           frm = New BasicForm With {.TableName = "FreightTypes"}
        '                                                           sender.Tag = New MyPage With {.Content = frm}
        '                                                       End Sub

        AddHandler LoadRadio(223, G, "Invoices Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "OutcomeTypes"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(224, G, "Forwarders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Forwarders"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(225, G, "Truckers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "Truckers"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(226, G, "Loading Locations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "LoadingLocations"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(227, G, "Varieties").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Varieties"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(228, G, "Forwarder Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ForwarderPrices
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub


        'AddHandler LoadRadio(229, G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                   frm = New BasicForm With {.TableName = "Items"}
        '                                                   sender.Tag = New MyPage With {.Content = frm}
        '                                               End Sub

        AddHandler LoadRadio(230, G, "Units Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm1_2 With {.TableName = "UnitsWeights", .lblName2_text = "Weight", .SubName2 = "Weights"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(231, G, "Banks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New BasicForm With {.TableName = "Banks", .IsMultiLine = True}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub


        AddHandler LoadRadio(232, G, "Stations").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "Stations"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(233, G, "Shipped Per").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "ShippedPers"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub





        LoadLabel(203, G, "Motion")

        AddHandler LoadRadio(204, G, "Proforma Invoice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ProForma With {.Flag = MyFlag.ProForma}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(235, G, "Booking").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Booking
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        'AddHandler LoadRadio(205, G, "ISO").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                 frm = New ISO
        '                                                 sender.Tag = New MyPage With {.Content = frm}
        '                                             End Sub

        AddHandler LoadRadio(237, G, "Iso").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                         frm = New ProForma With {.Flag = MyFlag.ISO}
                                                         sender.Tag = New MyPage With {.Content = frm}
                                                     End Sub

        AddHandler LoadRadio(238, G, "CustomsInvoice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New ProForma With {.Flag = MyFlag.CustomsInvoice}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(239, G, "Invoice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New ProForma With {.Flag = MyFlag.RealInvoice}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub



        LoadLabel(240, G, "")

        ''''''''''''''''''''''''''''''

        AddHandler LoadRadio(206, G, "Files").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Files
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(207, G, "Bills").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Bills
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(208, G, "Payments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New Outcomes
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        LoadLabel(213, G, "Loadings Sheet")

        AddHandler LoadRadio(209, G, "Loadings Sheet 1").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(210, G, "Loadings Sheet 2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 2}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(211, G, "Loadings Sheet 3").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 3}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(212, G, "Loadings Sheet 4").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New LoadingSheet With {.Flag = 4}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        '243
    End Sub

    Private Sub LoadGStores(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Stores", s)
        Dim frm As UserControl

        AddHandler LoadRadio(501, G, "Groups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Groups
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub


        AddHandler LoadRadio(502, G, "Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           'frm = New BasicForm2 With {.MainTableName = "Groups", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Group", .TableName = "Types", .MainId = "GroupId", .SubId = "Id", .SubName = "Name"}
                                                           frm = New Types
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowPriceLists Then
            AddHandler LoadRadio(503, G, "PriceLists").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "PriceLists"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Not Md.ShowQtySub Then
                AddHandler LoadRadio(563, G, "Itemunits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "Itemunits"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If

        If Md.MyProjectType = ProjectType.SonacAlex Then
            AddHandler LoadRadio(563, G, "UnitsWeights").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm1_2 With {.TableName = "UnitsWeights", .lblName2_text = "الوزن", .SubName2 = "Weights"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If



            AddHandler LoadRadio(510, G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Items
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            If Md.ShowBarcode Then
                AddHandler LoadRadio(511, G, "Print Barcode").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT18 With {.Flag = 1}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If


        AddHandler LoadRadio(514, G, "Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Stores
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(504, G, "SalesTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "SalesTypes"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        LoadLabel(515, G, "Stores Motion")

        AddHandler LoadRadio(516, G, "Starting balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.أرصدة_افتتاحية}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        Dim str As String = "Adding"
        AddHandler LoadRadio(517, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Sales With {.Flag = Sales.FlagState.إضافة}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        str = "Exchange"
        AddHandler LoadRadio(518, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Sales With {.Flag = Sales.FlagState.صرف}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub


        AddHandler LoadRadio(544, G, "مرتجع صرف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.تسوية_إضافة}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(519, G, "Gifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Sales With {.Flag = Sales.FlagState.هدايا}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(520, G, "Loses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Sales With {.Flag = Sales.FlagState.هالك}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub


        AddHandler LoadRadio(521, G, "Transfer to a Store").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Sales With {.Flag = Sales.FlagState.تحويل_إلى_مخزن}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub


        AddHandler LoadRadio(524, G, "Inventory").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Inventory
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub



        LoadLabel(535, G, "Purchases")

        AddHandler LoadRadio(536, G, "Purchases").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.مشتريات}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(537, G, "Purchase Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New Sales With {.Flag = Sales.FlagState.مردودات_مشتريات}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub


        LoadLabel(539, G, "Sales")


        AddHandler LoadRadio(549, G, "محلي").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New Sales With {.Flag = Sales.FlagState.المبيعات}
                                                          sender.Tag = New MyPage With {.Content = frm}
                                                      End Sub

        AddHandler LoadRadio(550, G, "مردودات محلي").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Sales With {.Flag = Sales.FlagState.مردودات_المبيعات}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub



        Dim f As String = "Exports"
        Dim f2 As String = "Exports Returns"
        LoadLabel(551, G, f)

        AddHandler LoadRadio(552, G, f).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                     frm = New Sales With {.Flag = Sales.FlagState.التصدير}
                                                     sender.Tag = New MyPage With {.Content = frm}
                                                 End Sub

        AddHandler LoadRadio(553, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                      frm = New Sales With {.Flag = Sales.FlagState.مردودات_التصدير}
                                                      sender.Tag = New MyPage With {.Content = frm}
                                                  End Sub

        LoadLabel(554, G, "ETA")

        AddHandler LoadRadio(555, G, "TokenPin").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New BasicForm With {.TableName = "TokenPins"}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub


        AddHandler LoadRadio(556, G, "رفع المبيعات لمصلحة الضرائب").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New SalesUploadToETA
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub


        '554
    End Sub

    Private Sub LoadGAccountants(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts", s)
        Dim frm As UserControl

        AddHandler LoadRadio(801, G, "Chart").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Chart
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowAnalysis Then
            AddHandler LoadRadio(802, G, "Analysis").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Companies
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.ShowCostCenter Then
            AddHandler LoadRadio(803, G, "CostCenters").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New CostCenters
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        If Md.ShowCostCenterSub Then
            AddHandler LoadRadio(804, G, "CostCenterSubs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New CostCenters With {.TableName = "CostCenterSubs", .MyHeader = "عناصر التكلفة"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.ShowCurrency Then
            AddHandler LoadRadio(805, G, "Currencies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm1_2 With {.TableName = "Currencies", .lblName2_text = "الرمز", .SubName2 = "Sign"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If


        AddHandler LoadRadio(847, G, "Status").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New BasicForm With {.TableName = "Status"}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(806, G, "CheckBanks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "CheckBanks"}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(807, G, "Income Daily Motion Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New BankCash_G2Types With {.Flag = 1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

        AddHandler LoadRadio(808, G, "Outcome Daily Motion Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New BankCash_G2Types With {.Flag = 2}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub




        
        AddHandler LoadRadio(810, G, "Entry Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BankCash_G2Types With {.Flag = 4}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


        If Md.MyProjectType = ProjectType.SonacAlex Then
            AddHandler LoadRadio(811, G, "FinalReportsMain").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "FinalReportsMain"}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(812, G, "FinalReportsSub").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm2 With {.MainTableName = "FinalReportsMain", .lblMain_Content = "FinalReportsMain", .TableName = "FinalReportsSub", .MainId = "FinalReportsMainId", .Flag = 2}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(813, G, "FinalReportsSub2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New FinalReportsSubDetails
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If


        If Md.ShowCurrency Then
            AddHandler LoadRadio(857, G, "CurrencyExchangeByDate").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New CurrencyExchangeByDate
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        LoadLabel(814, G, "File")

        AddHandler LoadRadio(815, G, "Assets").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Assets
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub


        AddHandler LoadRadio(816, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Customers
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub


        AddHandler LoadRadio(817, G, "Suppliers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Suppliers
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(818, G, "Debits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New CreditsDebits With {.TableName = "Debits", .MyLinkFile = 3}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(819, G, "Credits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New CreditsDebits With {.TableName = "Credits", .MyLinkFile = 4}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(820, G, "Saves").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New CreditsDebits With {.TableName = "Saves", .MyLinkFile = 5}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowBanks Then
            AddHandler LoadRadio(821, G, "Banks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New CreditsDebits With {.TableName = "Banks", .MyLinkFile = 6}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
        End If


        AddHandler LoadRadio(822, G, "Sellers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New CreditsDebits With {.TableName = "Sellers", .MyLinkFile = 7}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub



        AddHandler LoadRadio(824, G, "OutComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New CreditsDebits With {.TableName = "OutComeTypes", .MyLinkFile = 9}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(825, G, "InComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CreditsDebits With {.TableName = "InComeTypes", .MyLinkFile = 10}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        LoadLabel(828, G, "Daily Motion")


        Dim str As String = "Entry"
        If Md.MyProjectType = ProjectType.SonacAlex Then
            str = "التسويات"
        End If
        AddHandler LoadRadio(833, G, Str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Entry
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        LoadLabel(834, G, "المقبوضات")


        AddHandler LoadRadio(835, G, "مقبوضات صندوق اسكندرية جنيه مصرى").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New BankCash_G2 With {.Flag = 1, .FlagSub = 1}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub

        AddHandler LoadRadio(836, G, "مقبوضات صندوق دمنهور جنيه مصرى").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New BankCash_G2 With {.Flag = 1, .FlagSub = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub

        AddHandler LoadRadio(837, G, "مقبوضات صندوق اسكندرية دولار").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New BankCash_G2 With {.Flag = 1, .FlagSub = 3}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        AddHandler LoadRadio(838, G, "مقبوضات صندوق اسكندرية يورو").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New BankCash_G2 With {.Flag = 1, .FlagSub = 4}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        AddHandler LoadRadio(839, G, "مقبوضات شيكات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BankCash_G2 With {.Flag = 1, .FlagSub = 5}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        LoadLabel(840, G, "المدفوعات")

        AddHandler LoadRadio(841, G, "مدفوعات صندوق اسكندرية جنيه مصرى").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New BankCash_G2 With {.Flag = 2, .FlagSub = 1}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub

        AddHandler LoadRadio(842, G, "مدفوعات صندوق دمنهور جنيه مصرى").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New BankCash_G2 With {.Flag = 2, .FlagSub = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub

        AddHandler LoadRadio(843, G, "مدفوعات صندوق اسكندرية دولار").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New BankCash_G2 With {.Flag = 2, .FlagSub = 3}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        AddHandler LoadRadio(844, G, "مدفوعات صندوق اسكندرية يورو").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New BankCash_G2 With {.Flag = 2, .FlagSub = 4}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        AddHandler LoadRadio(845, G, "مدفوعات شيكات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BankCash_G2 With {.Flag = 2, .FlagSub = 5}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub



        'If Md.ShowBanks Then
        '    AddHandler LoadRadio(837, G, "Bank Transfer").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                               frm = New BankCash_G3
        '                                                               sender.Tag = New MyPage With {.Content = frm}
        '                                                           End Sub
        'End If



        'AddHandler LoadRadio(846, G, "Checks Tracing").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                            frm = New ChecksTracingNew
        '                                                            sender.Tag = New MyPage With {.Content = frm}
        '                                                        End Sub





        LoadLabel(848, G, "خطابات الضمان")

        AddHandler LoadRadio(849, G, "أنواع خطابات الضمان").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm With {.TableName = "GuaranteeTypes"}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(850, G, "الأصناف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New BasicForm With {.TableName = "GuaranteeItems"}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(851, G, "خطابات الضمان").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Guarantees
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub



    End Sub

    Private Sub LoadGFarms(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "الزراعات", s)
        Dim frm As UserControl

        LoadLabel(900, G, "البيانات الأساسية")
        AddHandler LoadRadio(901, G, "أنواع المستندات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "DocTypes"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(902, G, "الأصناف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New BasicForm With {.TableName = "AgricultureItems"}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(903, G, "المزارع").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New BasicForm With {.TableName = "Farms"}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(904, G, "المواسم").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New BasicForm With {.TableName = "Seasons"}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        LoadLabel(905, G, "الحركة")

        AddHandler LoadRadio(906, G, "زراعات قائمة بالتكلفة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New Entry3
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        '908

    End Sub

    Private Sub LoadGSecurity(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Options", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1101, G, "Change Password").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ChangePassword
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1102, G, "Levels").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Levels
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(1103, G, "Attachement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Attachments
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        If Md.ShowShifts Then
            AddHandler LoadRadio(1104, G, "Close Shift").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New CalcSalary With {.Flag = 6}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        AddHandler LoadRadio(1105, G, "فتح سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New CalcSalary With {.Flag = 11}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub


        AddHandler LoadRadio(1106, G, "بدء سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New CalcSalary With {.Flag = 12}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(1107, G, "استيراد الأرصدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New CalcSalary With {.Flag = 13}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

    End Sub

    Private Sub LoadGExportsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Exports Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1201, G, "Destination Prices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT01 With {.Flag = 1}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        'AddHandler LoadRadio(1202, G, "Loading Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                            frm = New RPT02 With {.Flag = 1}
        '                                                            sender.Tag = New MyPage With {.Content = frm}
        '                                                        End Sub

        AddHandler LoadRadio(1203, G, "UnPayed Files").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT02 With {.Flag = 2}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub


        AddHandler LoadRadio(1204, G, "Loading Sheet All").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT01 With {.Flag = 2}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(1205, G, "Loading Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT01 With {.Flag = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1206, G, "Invoices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT03 With {.Flag = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

    End Sub
    Private Sub LoadGStoresReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Stores Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1501, G, "Items Printing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT13 With {.Flag = 1}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(1502, G, "Items Printing With Images").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT13 With {.Flag = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        LoadLabel(1503, G, "Stores Motion")

        AddHandler LoadRadio(1504, G, "Stores Motions Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 1, .Detail = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(1569, G, "حركات المخازن مجمع").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT6 With {.Flag = 1, .Detail = 20}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(1505, G, "Stores Motions Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT6 With {.Flag = 1, .Detail = 0}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        If Md.ShowStoresMotionsEditing Then
            AddHandler LoadRadio(1506, G, "Stores Motions Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 1, .Detail = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
        End If

        
        AddHandler LoadRadio(1509, G, "Purchase Invoices Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 2, .Detail = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        AddHandler LoadRadio(1510, G, "Purchase Invoices Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 2, .Detail = 0}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        If Md.ShowStoresMotionsEditing Then
            AddHandler LoadRadio(1511, G, "Purchase Invoices Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New RPT6 With {.Flag = 2, .Detail = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
        End If

        
        AddHandler LoadRadio(1521, G, "Sales Invoices Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 3, .Detail = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1522, G, "Sales Invoices Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT6 With {.Flag = 3, .Detail = 0}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1523, G, "Stores Sales Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 9}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1524, G, "Sales Invoices Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT6 With {.Flag = 3, .Detail = 2}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If




        Dim f1 As String = "Exports Detailed"
        Dim f2 As String = "Exports Total"
        Dim f3 As String = "Exports Editing"
        
        If Md.MyProjectType = ProjectType.SonacAlex Then
            AddHandler LoadRadio(1525, G, f1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 8, .Detail = 1}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            AddHandler LoadRadio(1526, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 8, .Detail = 0}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1527, G, f3).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT6 With {.Flag = 8, .Detail = 2}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
            End If

        End If



        LoadLabel(1536, G, "Items Motion")

        AddHandler LoadRadio(1540, G, "Sales Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT6 With {.Flag = 30, .Detail = 4}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub


        AddHandler LoadRadio(1541, G, "Items Sales Price Avg").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 3, .Detail = 10}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        AddHandler LoadRadio(1542, G, "Item Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT6 With {.Flag = 3, .Detail = 11}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(1543, G, "Most Sales Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT6 With {.Flag = 3, .Detail = 12}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        AddHandler LoadRadio(1544, G, "Most Profit Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 3, .Detail = 13}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        AddHandler LoadRadio(1545, G, "Items Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT6 With {.Flag = 3, .Detail = 3}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(1546, G, "Items Sales Summarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 15}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1547, G, "Best-Selling Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT6 With {.Flag = 3, .Detail = 14}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        AddHandler LoadRadio(1548, G, "Items Sales All Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 6}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        

        AddHandler LoadRadio(1551, G, "ItemCard").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT12 With {.Flag = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(1552, G, "Item Motion With Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 5, .Detail = 7}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        LoadLabel(1558, G, "Items Balances")

        AddHandler LoadRadio(1559, G, "Items Balance In Store").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT12 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1561, G, "Store Balance with Purchase Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                        frm = New RPT12 With {.Flag = 5}
                                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                                    End Sub

        AddHandler LoadRadio(1562, G, "Store Balance with Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT12 With {.Flag = 51, .ReportFlag = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(1563, G, "Store Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT12 With {.Flag = 52, .ReportFlag = 2}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

        AddHandler LoadRadio(1564, G, "Items Balance In All Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT12 With {.Flag = 3}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        AddHandler LoadRadio(1565, G, "All Stores Balance with Purchase Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                             frm = New RPT12 With {.Flag = 6}
                                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                                         End Sub

        AddHandler LoadRadio(1566, G, "All Stores Balance with Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT12 With {.Flag = 61, .ReportFlag = 1}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        AddHandler LoadRadio(1567, G, "All Stores Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New RPT12 With {.Flag = 62, .ReportFlag = 2}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub

        AddHandler LoadRadio(1568, G, "Items exceeded limit demand").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT12 With {.Flag = 4}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
        '1569
    End Sub

    Private Sub LoadGAccountsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1801, G, "Chart").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT26 With {.Flag = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(1884, G, "الشركات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New RPT26 With {.Flag = 2}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(1802, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT38 With {.Flag = 1}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        Dim str As String = "Account Motion"
        If Md.MyProjectType = ProjectType.SonacAlex Then
            str = "حساب الأستاذ"
        End If
        AddHandler LoadRadio(1803, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New RPT2 With {.Flag = 1, .MyLinkFile = -1}
                                                        sender.Tag = New MyPage With {.Content = frm}
                                                    End Sub


        AddHandler LoadRadio(1881, G, "حساب الأستاذ حسب المؤيد والغير مؤيد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New RPT2 With {.Flag = 4, .MyLinkFile = -1}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub

        AddHandler LoadRadio(1882, G, "حساب الأستاذ حسب الحالة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 5, .MyLinkFile = -1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub


        AddHandler LoadRadio(1883, G, "حساب الأستاذ حسب الشركات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 6, .MyLinkFile = -1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub


            AddHandler LoadRadio(1804, G, "EntryView").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT36 With {.Flag = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            If Md.ShowBankCash_G Then
                AddHandler LoadRadio(1805, G, "Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT21 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

                If Md.ShowStoresMotionsEditing Then
                    AddHandler LoadRadio(1806, G, "Income Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT21 With {.Flag = 1, .Detailed = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
                End If

                AddHandler LoadRadio(1807, G, "Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT21 With {.Flag = 2}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

                If Md.ShowStoresMotionsEditing Then
                    AddHandler LoadRadio(1808, G, "Outcome Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT21 With {.Flag = 2, .Detailed = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
                End If

        
            If Md.ShowCostCenter Then
                'AddHandler LoadRadio(1813,G, "CostCenterOutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                          frm = New RPT14 With {.Flag = 1}
                '                                                          sender.Tag = New MyPage With {.Content = frm}
                '                                                      End Sub

                'AddHandler LoadRadio(1814,G, "CostCenterOutComeToTal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                               frm = New RPT14 With {.Flag = 2}
                '                                                               sender.Tag = New MyPage With {.Content = frm}
                '                                                           End Sub

                AddHandler LoadRadio(1815, G, "CostCenterAccountMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT14 With {.Flag = 3}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If
        End If

        AddHandler LoadRadio(1816, G, "Save Daily Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT2 With {.Flag = 2, .MyLinkFile = 5}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub


        If Md.ShowCurrency Then
            AddHandler LoadRadio(1817, G, "Currency Basket").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT28 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End If


        AddHandler LoadRadio(1818, G, "AllEntries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New RPT25 With {.Flag = 22}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


        AddHandler LoadRadio(1822, G, "Statement of account").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 1}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        AddHandler LoadRadio(1823, G, "Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT11 With {.Flag = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(1824, G, "Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT2 With {.Flag = 3}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1887, G, "أكواد العملاء").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT2 With {.Flag = 3, .MyLinkFile = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1888, G, "أكواد العملاء مختصر").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT2 With {.Flag = 3, .MyLinkFile = 1, .Flag2 = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub


        AddHandler LoadRadio(1871, G, "CalcAssetsDepreciation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New CalcSalary With {.Flag = 9}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1872, G, "Calc Store Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT25 With {.Flag = 8}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1873, G, "Account Balance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT20 With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1885, G, "ميزان المراجعة عملة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT20 With {.Flag = 2}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(1874, G, "Operating").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT27 With {.Flag = 1, .MyEndType = 0}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1875, G, "Trading").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New RPT27 With {.Flag = 1, .MyEndType = 1}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(1876, G, "Gains and losses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT27 With {.Flag = 1, .MyEndType = 2}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1877, G, "Balance Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT27 With {.Flag = 1, .MyEndType = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1878, G, "Income Statement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT27 With {.Flag = 2, .MyEndType = 2, .IsIncomeStatement = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1879, G, "Financial Position").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT27 With {.Flag = 3, .MyEndType = 3}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub



        If Md.MyProjectType = ProjectType.SonacAlex Then
            AddHandler LoadRadio(1880, G, "FinalReports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT40 With {.Flag = 1, .MainTableName = "FinalReportsMain", .lblMain_Content = "FinalReportsMain", .Main2TableName = "FinalReportsSub", .Main2MainId = "FinalReportsMainId", .lblMain2_Content = "FinalReportsSub"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If



        AddHandler LoadRadio(1886, G, "خطابات الضمان").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT42 With {.Flag = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub



        '1888
    End Sub

    Private Sub LoadAbout(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "About", s)
        Dim wb As New WebBrowser With {.Margin = New Thickness(0)}
        wb.Navigate("http://omegaapp.blogspot.com.eg/")
        G.Children.Add(wb)
        wb.Width = tab.ActualWidth - 20
        wb.Height = tab.ActualHeight - 60
    End Sub


    Private Sub LoadGFarmsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "تقارير الزراعات", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1901, G, "حساب أستاذ حسب المزرعة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT43 With {.Flag = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1902, G, "حساب أستاذ حسب الحساب").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT43 With {.Flag = 2}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        AddHandler LoadRadio(1903, G, "حساب أستاذ حسب الشركة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT43 With {.Flag = 3}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

    End Sub


    Private Sub LoadTabs()
        LoadGFile(1)

        LoadGExports(2)

        LoadGStores(5)

        LoadGAccountants(8)

        LoadGFarms(9)

        LoadGSecurity(11)

        LoadGExportsReports(12)

        LoadGStoresReports(15)

        LoadGAccountsReports(18)

        LoadGFarmsReports(19)

    End Sub



End Class

