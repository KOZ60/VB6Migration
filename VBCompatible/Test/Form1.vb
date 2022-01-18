Imports System.Reflection
Imports VBCompatible

Public Class Form1
    Inherits VBCompatible.VBForm

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        VbPanel1.Enabled = CheckBox1.Checked
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        For Each con As Control In VbPanel1.Controls
            con.Enabled = CheckBox2.Checked
        Next
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        For Each con As Control In VbPanel1.Controls
            Dim pi As PropertyInfo = con.GetType().GetProperty("ReadOnly")
            If pi IsNot Nothing Then
                pi.SetValue(con, CheckBox3.Checked)
            End If
        Next
    End Sub

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Right Then
            PopupMenu(mnuFile)
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With VbListView1
            .View = View.Details
            .FullRowSelect = True
            If .Items.Count < 2 Then
                .Columns.Add(" 名　前 ", 100, HorizontalAlignment.Left)
                .Columns.Add("郵便番号", 100, HorizontalAlignment.Left)
                .Columns.Add(" 住　所 ", 150, HorizontalAlignment.Left)
            End If

            Dim i As Integer = .Items.Count

            .Items.Add("草名木　強", i)
            .Items(i).SubItems.Add("123-4567")
            .Items(i).SubItems.Add("東京都 新宿区")

            .Items.Add("木邨　卓也", i + 1)
            .Items(i + 1).SubItems.Add("111-2222")
            .Items(i + 1).SubItems.Add("東京都 千代田区")

            .Items.Add("仲居　正弘", i + 2)
            .Items(i + 2).SubItems.Add("222-3333")
            .Items(i + 2).SubItems.Add("東京都 港区")

            .Items.Add("蚊取　新語", i + 3)
            .Items(i + 3).SubItems.Add("333-3333")
            .Items(i + 3).SubItems.Add("東京都 足立区")

            .Items.Add("稲墻　五老", i + 4)
            .Items(i + 4).SubItems.Add("444-4444")
            .Items(i + 4).SubItems.Add("東京都 中央区")

        End With
    End Sub

    Private Sub EventEnumFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EventEnumFormToolStripMenuItem.Click
        Dim f As New VBCompatible.ControlArray.EventEnumForm()
        f.Show()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        VBSSTab1.TabVisible(0) = ToolStripMenuItem2.Checked
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        VBSSTab1.TabVisible(1) = ToolStripMenuItem3.Checked
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        VBSSTab1.TabVisible(2) = ToolStripMenuItem4.Checked
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        VBSSTab1.TabVisible(3) = ToolStripMenuItem5.Checked
    End Sub

End Class

