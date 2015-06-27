Imports System.Windows.Forms
Imports System.Net
Imports System.IO
Imports System


Public Class CaptchaDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text <> "" Then
            captcha = TextBox1.Text
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            Dim cbox = MessageBox.Show("你确定要不输入验证码吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If cbox = Windows.Forms.DialogResult.No Then
                Exit Sub
            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CaptchaDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCaptcha()
    End Sub

    Private Sub LoadCaptcha()
        Dim imageDownloader As WebClient = New WebClient
        Dim bbyte As Byte()
        GUID = getGUID()
        bbyte = imageDownloader.DownloadData(captchaImageURL + GUID + "&fid=100")
        Dim ms As MemoryStream = New MemoryStream()
        ms.Write(bbyte, 0, bbyte.Length)
        ms.Flush()
        PictureBox1.Image = Image.FromStream(ms)
        imageDownloader.Dispose()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        LoadCaptcha()
    End Sub
End Class
