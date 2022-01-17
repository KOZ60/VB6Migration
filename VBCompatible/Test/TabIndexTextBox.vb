Public Class TabIndexTextBox
    Inherits VBCompatible.VBTextBox

    Public Sub New()
        Text = TabIndex
    End Sub

    Protected Overrides Sub OnTabIndexChanged(e As EventArgs)
        MyBase.OnTabIndexChanged(e)
        Text = TabIndex
    End Sub

    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)
        Text = TabIndex
    End Sub

End Class
