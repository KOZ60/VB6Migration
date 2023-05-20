Imports VBCompatible

Public Class Form2
    Inherits VBForm
    Private cmdIndex As Integer = 0

    Private Sub Cmd01_Click(sender As Object, e As EventArgs) Handles cmd01.Click
        cmdIndex += 1
        Dim newCmd = cmd.Load(cmdIndex)
        newCmd.Visible = True
        newCmd.Top = cmdIndex * cmd01.Height + cmd01.Top
    End Sub

    Private Sub cmd_Click(sender As Object, e As EventArgs) Handles cmd.Click
        Dim index = cmd.GetIndex(sender)
        Debug.Print("cmd({0}) がクリックされました。", index)
        Debug.Print(VB6.Format(12345678901234, "#,##0"))
        Debug.Print(VB6.Format(Now, "yyyy/mm/dd hh:mm:ss"))
    End Sub

    Private Sub VbCommandButton1_Click(sender As Object, e As EventArgs) Handles VbCommandButton1.Click
        Using dlg = New ControlArray.EventEnumForm()
            dlg.ShowDialog(Me)
        End Using
    End Sub

End Class