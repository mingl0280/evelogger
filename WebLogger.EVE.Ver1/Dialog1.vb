Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Encoding
Imports System.IO
Imports System.IO.StreamWriter


Public Class Dialog1

    Private star As String = ""

    Private Sub Dialog1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For i As Integer = 1 To UBound(aAccounts)
            ComboBox1.Items.Add(aAccounts(i).user)
        Next
        readini2("starp", star)
        writeini("starp", star)
        If star = "C" Then
            CheckBox1.Checked = True
            TextBox2.UseSystemPasswordChar = True
        Else
            CheckBox1.Checked = False
            TextBox2.UseSystemPasswordChar = False
        End If
    End Sub

    ''' <summary>
    ''' 增加按钮点击事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If UBound(aAccounts) = 0 Then   '如果aAccounts里面没有数据
            ReDim aAccounts(1)
            aAccounts(1).user = ComboBox1.Text
            aAccounts(1).password_encrypted = encode(TextBox2.Text)
            ComboBox1.Items.Add(ComboBox1.Text)
        Else
            ReDim Preserve aAccounts(aAccounts.Length)
            ComboBox1.Items.Add(ComboBox1.Text)
            aAccounts(UBound(aAccounts)).user = ComboBox1.Text
            aAccounts(UBound(aAccounts)).password_encrypted = encode(TextBox2.Text)
        End If
        MessageBox.Show("添加成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Dim RSLString1, RSLString2 As String
        RSLString1 = ""
        RSLString2 = ""
        For i As Integer = 1 To UBound(aAccounts)
            RSLString1 = RSLString1 + "|" + aAccounts(i).user
            RSLString2 = RSLString2 + "|" + aAccounts(i).password_encrypted
        Next
        RSLString1 = RSLString1.Remove(0, 1)
        RSLString2 = RSLString2.Remove(0, 1)
        writeini("Account_Names", RSLString1)
        writeini("Passwords", RSLString2)
        writeini("starp", star)
        Form1.flushMenuItems()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            star = "C"
        Else
            star = ""
        End If
        If star <> "" Then
            TextBox2.UseSystemPasswordChar = True
        Else
            TextBox2.UseSystemPasswordChar = False
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        TextBox2.Text = decode(aAccounts(ComboBox1.SelectedIndex + 1).password_encrypted)
    End Sub

End Class
