Imports System.Data

Public Class RPT18

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@ItemId", "@ColorId", "@SizeId", "@Count", "Header"}
        rpt.paravalue = New String() {Val(ItemId.Text), Val(ColorId.Text), Val(SizeId.Text), Val(Count.Text), CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                    rpt.Rpt = "PrintBarcode.rpt"

        End Select
        'rpt.Show()
        rpt.Print(".", Md.BarcodePrinter, 1)
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({ItemId, ColorId, SizeId})
        Button2.Content = "طباعة"
        
        lblColorId.Visibility = Windows.Visibility.Hidden
        ColorId.Visibility = Windows.Visibility.Hidden
        ColorName.Visibility = Windows.Visibility.Hidden

        lblSizeId.Visibility = Windows.Visibility.Hidden
        SizeId.Visibility = Windows.Visibility.Hidden
        SizeName.Visibility = Windows.Visibility.Hidden



    End Sub

    Dim lop As Boolean = False



    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")

    End Sub

    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Id as varchar(100)) Id,Name from Items") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
        ColorId_LostFocus(Nothing, Nothing)
        SizeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub ColorId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ColorId.LostFocus
        bm.LostFocus(ColorId, ColorName, "select Name from ColorsDetails where ColorId=(select It.ColorId from Items It where It.Id=" & ItemId.Text.Trim() & ") and Id=" & ColorId.Text.Trim())
    End Sub

    Private Sub ColorId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ColorId.KeyUp
        bm.ShowHelp("ColorsDetails", ColorId, ColorName, e, "select cast(Id as varchar(100)) Id,Name from ColorsDetails where ColorId=(select It.ColorId from Items It where It.Id=" & ItemId.Text.Trim() & ")")
    End Sub

    Private Sub SizeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SizeId.LostFocus
        bm.LostFocus(SizeId, SizeName, "select Name from SizesDetails where SizeId=(select It.SizeId from Items It where It.Id=" & ItemId.Text.Trim() & ") and Id=" & SizeId.Text.Trim())
    End Sub

    Private Sub SizeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SizeId.KeyUp
        bm.ShowHelp("SizesDetails", SizeId, SizeName, e, "select cast(Id as varchar(100)) Id,Name from SizesDetails where SizeId=(select It.SizeId from Items It where It.Id=" & ItemId.Text.Trim() & ")")
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyDown, ColorId.KeyDown, SizeId.KeyDown, Count.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

End Class