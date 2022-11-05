Imports System.Data

Public Class RPT13

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@GroupId", "@TypeId", "@FromId", "@ToId", "Header"}
        rpt.paravalue = New String() {Val(GroupId.Text), Val(TypeId.Text), Val(ItemId.Text), Val(ItemId2.Text), CType(Parent, Page).Title}
        Select Case Flag
            Case 1
                rpt.Rpt = "Items.rpt"
            Case 2
                rpt.Rpt = "ItemsWithImages.rpt"
            Case 3
                rpt.Rpt = "ItemsQty2.rpt"
            Case 4
                rpt.Rpt = "ItemsProfit.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({GroupId, TypeId, ItemId, ItemId2})
    End Sub

    Dim lop As Boolean = False



    Private Sub LoadResource()
        Button2.SetResourceReference(Button.ContentProperty, "View Report")
        
    End Sub

    Private Sub ItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyUp
        If bm.ShowHelp("Items", ItemId, ItemName, e, "select cast(Id as varchar(100)) Id,Name from Items where (GroupId='" & Val(GroupId.Text) & "' or '" & Val(GroupId.Text) & "'=0) and (TypeId='" & Val(TypeId.Text) & "' or '" & Val(TypeId.Text) & "'=0)") Then
            ItemId_LostFocus(ItemId, Nothing)
        End If
    End Sub

    Private Sub ItemId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId.LostFocus
        bm.LostFocus(ItemId, ItemName, "select Name from Items where Id=" & ItemId.Text.Trim())
        ItemId2.Text = ItemId.Text
        ItemName2.Text = ItemName.Text
    End Sub

    Private Sub ItemId2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId2.KeyUp
        If bm.ShowHelp("Items", ItemId2, ItemName2, e, "select cast(Id as varchar(100)) Id,Name from Items where (GroupId='" & Val(GroupId.Text) & "' or '" & Val(GroupId.Text) & "'=0) and (TypeId='" & Val(TypeId.Text) & "' or '" & Val(TypeId.Text) & "'=0)") Then
            ItemId2_LostFocus(ItemId2, Nothing)
        End If
    End Sub

    Private Sub ItemId2_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles ItemId2.LostFocus
        bm.LostFocus(ItemId2, ItemName2, "select Name from Items where Id=" & ItemId2.Text.Trim())
    End Sub

    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles ItemId.KeyDown, ItemId2.KeyDown, GroupId.KeyDown, TypeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

End Class