Imports VBCompatible

Public Class Form2
    Inherits VBForm

    Private loadIndex As Integer
    Private cmdIndex As Integer = 0

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmdIndex = cmd.UBound
        loadIndex = cmd.UBound
    End Sub

    Private Sub Cmd01_Click(sender As Object, e As EventArgs) Handles cmd01.Click
        cmdIndex += 1
        Dim newCmd = cmd.Load(cmdIndex)
        newCmd.Visible = True
        newCmd.Top = (cmd.Count - loadIndex - 1) * cmd01.Height + cmd01.Top
        newCmd.Text = "cmd" & VB6.Format(cmdIndex, "00")
    End Sub

    Private Sub cmd_Click(sender As Object, e As EventArgs) Handles cmd.Click
        Dim index = cmd.GetIndex(sender)
        VbLabel1.Text = String.Format("cmd({0}) がクリックされました。", index)
        Debug.Print("cmd({0}) がクリックされました。", index)
        Debug.Print(VB6.Format(12345678901234, "#,##0"))
        Debug.Print(VB6.Format(Now, "yyyy/mm/dd hh:mm:ss"))
    End Sub

    Private Sub VbCommandButton1_Click(sender As Object, e As EventArgs)
        Using dlg = New VB6.EventEnumForm()
            dlg.ShowDialog(Me)
        End Using
    End Sub

    Private Sub VbCommandButton2_Click(sender As Object, e As EventArgs) Handles VbCommandButton2.Click
        Try
            cmd.Unload(cmd.UBound)
        Catch ex As Exception
            MessageBox.Show(ex.Message, ex.Source)
        End Try
    End Sub

End Class