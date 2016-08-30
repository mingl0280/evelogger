Imports System.Windows.Forms

Public Class ProxyDialog



    Public Structure myResult

        Private m_dResult As DialogResult
        Private m_IPAddr As String
        Private m_Username As String
        Private m_Password As String
        Private m_portid As Integer
        Public Property BaseResult As DialogResult
            Get
                Return m_dResult
            End Get
            Set(value As DialogResult)
                m_dResult = value
            End Set
        End Property

        Public ReadOnly Property ProxyData As ProxyInfo
            Get
                Return New ProxyInfo(m_Username, m_Password, m_IPAddr, m_portid)
            End Get
        End Property

        Public Sub New(r As DialogResult, u As String, p As String, i As String, Optional pt As Integer = 8000)
            m_dResult = r
            m_IPAddr = i
            m_Username = u
            m_Password = p
            m_portid = pt
        End Sub
    End Structure

    Shared m_rslt As myResult

    Public Shadows Function ShowDialog() As myResult
        MyBase.ShowDialog()
        Return m_rslt
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text = "" Then
            m_rslt = New myResult(Windows.Forms.DialogResult.Cancel, "", "", "", "")
        Else
            m_rslt = New myResult(Windows.Forms.DialogResult.OK, TextBox2.Text, TextBox3.Text, TextBox1.Text, TextBox4.Text)
            WriteRegistry("IP", TextBox1.Text)
            WriteRegistry("ProxyUser", TextBox2.Text)
            WriteRegistry("ProxyPass", TextBox3.Text)
            WriteRegistry("ProxyPort", TextBox4.Text)
        End If
        Me.Close()
    End Sub



    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        m_rslt = New myResult(Windows.Forms.DialogResult.Cancel, "", "", "")
        Me.Close()
    End Sub

    Private Sub ProxyDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        TextBox1.Text = ReadRegistry("IP")
        TextBox2.Text = ReadRegistry("ProxyUser")
        TextBox3.Text = ReadRegistry("ProxyPass")
        TextBox4.Text = ReadRegistry("ProxyPort")
    End Sub
End Class
