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

            For c As Integer = 0 To 20
                .Columns.Add(String.Format("Column{0}", c), 100, HorizontalAlignment.Left)
            Next

            For r As Integer = 0 To 1000
                Dim item As ListViewItem = .Items.Add(String.Format("row{0}", r))
                For c As Integer = 1 To .Columns.Count - 1
                    item.SubItems.Add(String.Format("{0},{1}", r, c))
                Next
            Next
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

    Private Sub mnuFileForm2_Click(sender As Object, e As EventArgs) Handles mnuFileForm2.Click
        Dim f As New Form2()
        f.Show()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        VbPanel4.Enabled = CheckBox4.Checked
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        VbTreeView1.Enabled = CheckBox5.Checked
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        VbPanel5.Enabled = CheckBox6.Checked
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        Me.VbListView1.Enabled = CheckBox7.Checked
    End Sub


End Class

