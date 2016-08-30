Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Text.Encoding
Imports System.Net
Imports System.Web
Imports System.Windows.Forms
Imports System.Net.NetworkInformation
Imports System.Diagnostics
Imports System.Xml
Imports System.Security.Cryptography
Imports Microsoft.VisualBasic.Devices
Imports System.Runtime.Remoting.Messaging
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.ComponentModel
Imports System.Management


Public Class Form1

#Const NOBRO = True

#Region "全局变量及委托声明"
    ''' <summary>
    ''' 程序退出指示
    ''' </summary>
    ''' <remarks></remarks>
    Dim stopping As Integer
    '---------------------------------------
    Dim firstrun As Integer
    Private sso As String
    Dim pt, pt2 As Thread
    Dim FWDStatus As String = ""
    Dim Overtime As Boolean = False
    Private isSupportDx11 As Boolean = False

    Private Delegate Sub voidDelegate(ByRef i As String)
    Private Delegate Sub FormDelegate(ByRef k As Boolean, ByRef obj As Object)
    Private Delegate Function AsyncMethodCaller(s As String) As String
#End Region

#Region "窗体事件响应"

    ''' <summary>
    ''' 启动时执行的操作，包括检查版本号，设定菜单项，打开登陆页等。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ApplicationTitle As String
        Dim valsuc As Integer

        useProxy = PromoteProxy()

        ValidClientVersion(valsuc)                        '验证版本号

        不退出重新登录ToolStripMenuItem.Enabled = False

        '设定标题栏
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        Me.Text = ApplicationTitle

        '设定验证版本号的提示
        Select Case valsuc
            Case 1
                Label5.Text = "当前客户端已经是最新版！"
            Case 2
                Label5.Text = "正在更新当前客户端..."
            Case -1
                exitprog()
        End Select
        aAccounts = FillAccountsInfo()   '获得所有账户信息
        flushMenuItems()                 '刷新列表框和菜单
        pingserver()                     '启动对服务器的ping进程
        Timer2.Start()

        OvalCircle1.New2(New SolidBrush(Color.Red), OvalCircle1.Width, OvalCircle1.Height)
#Const A = 0
#If A = 1 Then
        Dim g = Graphics.Factory.Create() 'this graphics stands for DX->Graphics
        For Each it In g.Adapters
            Dim r = it.Description.Description
            If r.StartsWith("NVIDIA") Then
                If r.IndexOf("Titan") Then
                    isSupportDx11 = True
                    Exit For
                Else
                    If Val(Regex.Replace(r, "[a-z]", "", RegexOptions.IgnoreCase).Trim().Replace(" ", "")) >= 400 Then
                        isSupportDx11 = True
                        Exit For
                    End If
                End If
            Else
                If r.IndexOf("Radeon") >= 0 Then
                    If Val(Regex.Replace(r, "[a-z]", "", RegexOptions.IgnoreCase).Trim().Replace(" ", "")) >= 5000 Then
                        isSupportDx11 = True
                        Exit For
                    End If
                End If
            End If
        Next
#Else
        Try
            Dim MGO = New ManagementObjectSearcher("select * from Win32_VideoController")
            For Each elem As ManagementObject In MGO.Get()
                Try
                    Dim r = elem.GetPropertyValue("Caption")
                    If r.StartsWith("NVIDIA") Then
                        If r.IndexOf("Titan") Then
                            isSupportDx11 = True
                            Exit For
                        Else
                            If Val(Regex.Replace(r, "[a-z]", "", RegexOptions.IgnoreCase).Trim().Replace(" ", "")) >= 400 Then
                                isSupportDx11 = True
                                Exit For
                            End If
                        End If
                    Else
                        If r.IndexOf("Radeon") >= 0 Then
                            If Val(Regex.Replace(r, "[a-z]", "", RegexOptions.IgnoreCase).Trim().Replace(" ", "")) >= 5000 Then
                                isSupportDx11 = True
                                Exit For
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try
            Next

        Catch ex As Exception

        End Try
#End If
        Dim OSV As New Version(My.Computer.Info.OSVersion)
        If OSV.Major < 6 Then isSupportDx11 = False

        If isSupportDx11 Then
            Label7.Text = "DirectX 11"
        Else
            Label7.Text = "DirectX 9"
        End If

    End Sub

    ''' <summary>
    ''' 终止所有线程
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        Environment.Exit(0)
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Normal Then
            stopping = 0
            firstrun = firstrun + 1
            If firstrun > 1 Then
                pingserver()
            End If
        Else
            pt.Abort()
            pt2.Abort()
            stopping = 1
        End If
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
        End If
    End Sub

    ''' <summary>
    ''' 当用户名输入框失去焦点时（通常为tab）触发验证码验证
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox1_LostFocus(sender As Object, e As EventArgs) Handles ComboBox1.LostFocus
        If isRequestCaptcha(ComboBox1.Text) Then
            CaptchaDialog.ShowDialog()
        End If
    End Sub

    ''' <summary>
    ''' 自动填充存储的密码
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        On Error GoTo -1
        If ComboBox1.Text <> "" And ComboBox1.SelectedIndex >= 0 Then
            TextBox1.Text = decode(aAccounts(ComboBox1.SelectedIndex + 1).password_encrypted)
        End If
        'Debug.WriteLine(ComboBox1.Text)
    End Sub

    ''' <summary>
    ''' 触发登录事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim th As Thread = New Thread(AddressOf StartLogin)
        th.Start(New String() {ComboBox1.Text, TextBox1.Text})
    End Sub

    ''' <summary>
    ''' 用户名变更时清除密码或自动填充
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        For i As Integer = 1 To UBound(aAccounts)
            If ComboBox1.Text = aAccounts(i).user Then
                ComboBox1.SelectedItem = ComboBox1.Text
                Exit For
            Else
                TextBox1.Text = ""
            End If
        Next
    End Sub

#End Region

#Region "托盘菜单事件响应"

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Show()
            Me.WindowState = FormWindowState.Normal
            Me.BringToFront()
        ElseIf Me.WindowState = FormWindowState.Normal Then
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

    Private Sub 退出ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem1.Click
        Application.Exit()
    End Sub

    Private Sub 显示登陆器ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 显示登陆器ToolStripMenuItem.Click
        Me.Show()
    End Sub

#End Region

#Region "主界面菜单事件响应"

    ''' <summary>
    ''' 自动填表的菜单项响应事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub menuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuStrip1.Click, ContextMenuStrip1.Click
        Dim item As String = sender.name
        For i As Integer = 1 To UBound(aAccounts)
            If aAccounts(i).user = item Then
                TextBox1.Text = decode(aAccounts(i).password_encrypted)
                ComboBox1.Text = aAccounts(i).user
                Button1_Click(Nothing, Nothing)
            End If
        Next
    End Sub

    Private Sub 清空ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 清空ToolStripMenuItem.Click, Button2.Click

        ComboBox1.Text = ""
        ComboBox1.SelectedItem = Nothing

    End Sub

    Private Sub 打开官方登陆器ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 打开官方登陆器ToolStripMenuItem.Click
        Try
            Shell("eve.exe")
        Catch ex As Exception
            MsgBox("无法打开eve.exe,请确定本程序已经放置于EVE Online根目录下!!!")
            exitprog()
        End Try
    End Sub

    Private Sub 打开客户端设定ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 打开客户端设定ToolStripMenuItem.Click
        Try
            Shell("repair.exe")
        Catch ex As Exception
            MsgBox("无法打开repair.exe,请确定本程序已经放置于EVE Online根目录下!!!")
            exitprog()
        End Try
    End Sub

    Private Sub 关于ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 关于ToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub

    Private Sub 退出ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 退出ToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub 验证客户端版本ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 验证客户端版本ToolStripMenuItem.Click
        Dim valsuc As Integer
        Label5.Text = "验证中，请稍等..."
        ValidClientVersion(valsuc)
        Select Case valsuc
            Case 1
                MsgBox("您使用的客户端已经是最新版！")
                Label5.Text = "当前客户端已经是最新版！"
            Case -1
                MsgBox("验证过程出错，请稍后再试。")
                Label5.Text = "验证过程出错，请稍后再试。"
            Case Else
                Label5.Text = "正在更新中..."
        End Select
    End Sub

    Private Sub 不退出重新登录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 不退出重新登录ToolStripMenuItem.Click
        If sso = "" Then MsgBox("你还没有登录过！")
        StartEVE()
    End Sub

    Private Sub 设置自动保存用户名和密码ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 设置自动保存用户名和密码ToolStripMenuItem.Click
        Dialog1.Show()
    End Sub

#End Region

#Region "状态事件响应"

    Private Sub Label3_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label3.MouseHover
        ToolTip1.Show("已发送：" + PingSent.ToString + " 已接收：" + PingRecv.ToString, Label3, 5000)
    End Sub

    Private Sub Form1_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseHover
        ToolTip1.Hide(Label3)
    End Sub
#End Region

#Region "计时器事件响应"

    ''' <summary>
    ''' 重置ping数据
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        PingSent = 0
        PingRecv = 0
        Timer2.Start()
    End Sub

    ''' <summary>
    ''' 超时计时器
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Overtime = True
        sso = "ALREADYSTARTED"
    End Sub

#End Region

#Region "功能函数"

    ''' <summary>
    ''' 使用国服API获取服务器状态信息
    ''' </summary>
    ''' <param name="sr">程序是否已经退出</param>
    ''' <remarks></remarks>
    Private Sub getserverstatus(ByVal sr As Integer)
        Dim node(2) As XmlNode
        Dim datastr As String()
        Dim open As String
        Dim xmltable As XmlDocument
        ReDim datastr(2)
        xmltable = New XmlDocument()
        While stopping = 0
            Try
                xmltable.Load("https://api.eve-online.com.cn/server/serverstatus.xml.aspx")
                node(0) = xmltable.DocumentElement.SelectSingleNode("result").SelectSingleNode("serverOpen")
                node(1) = xmltable.DocumentElement.SelectSingleNode("result").SelectSingleNode("onlinePlayers")
                If Me.IsDisposed = True Then Exit Sub
                If node(0).InnerText = "True" Then
                    open = "正常开服"
                Else
                    open = "已关闭"
                End If
                Dim strback = "服务器状况:" + open + " 当前在线：" + node(1).InnerText
                Me.Invoke(New voidDelegate(AddressOf updateUI), strback)
                Thread.Sleep(30 * 1000)
            Catch ex As Exception
            End Try
        End While
    End Sub

    ''' <summary>
    ''' 对EVE服务器进行Ping
    ''' </summary>
    ''' <remarks></remarks>
    Sub pingserver()
        If stopping = 1 Then
            pt.Abort()
            pt2.Abort()
        End If
        pt = New Thread(AddressOf getserverstatus, 0)
        pt2 = New Thread(AddressOf ping, 0)
        pt.Start(stopping)
        pt2.Start(stopping)
    End Sub

    ''' <summary>
    ''' 多线程ping服务器操作
    ''' </summary>
    ''' <param name="s">程序是否已经退出</param>
    ''' <remarks></remarks>
    Sub ping(ByVal s As Integer)
        Dim PingReq As New Ping
        Dim PingRet As PingReply
        Dim PingSet As New PingOptions
        Dim host As String = "211.144.214.68"
        Dim data As String = "a"
        Dim bindata As Byte() = ASCII.GetBytes(data)
        PingSet.Ttl = 64
        PingSet.DontFragment = True
        readIPaddr(host)
        While stopping = 0
            Try
                Thread.Sleep(5000)
                PingRet = PingReq.Send(host, 2000, bindata, PingSet)
                PingSent = PingSent + 1
                Try
                    If PingRet.Status = IPStatus.Success Then
                        Me.Invoke(New voidDelegate(AddressOf UpdateUI3), "服务器当前Ping: " + PingRet.RoundtripTime.ToString())
                        PingRecv = PingRecv + 1
                    Else
                        Me.Invoke(New voidDelegate(AddressOf UpdateUI3), "无法连接到服务器")
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End While
        PingReq.Dispose()
    End Sub

    ''' <summary>
    ''' 启动登录过程
    ''' </summary>
    ''' <param name="userinfo">用户信息</param>
    ''' <remarks></remarks>
    Sub StartLogin(ByVal userinfo() As String)
        Dim objlist As Object() = {Me.ComboBox1, Me.TextBox1, Me.Button1}
        'DisableOrEnableObject(objlist, False)
        Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "获取登录信息……")
tag:
        Try
            GetASPNetSessionIDAndLoginAddr()
        Catch ex As Exception
            GoTo tag
        End Try
        Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "检查验证码……")
        If isRequestCaptcha(userinfo(0)) Then
            Dim Cresult = CaptchaDialog.ShowDialog()
            If Cresult = Windows.Forms.DialogResult.Cancel Then
                Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "验证码获取失败，取消登录")
                'DisableOrEnableObject(objlist, True)
                Exit Sub
            End If
        End If
        Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "验证码通过，正在登录……")
        Dim Linfo() As String
        While 1
            Linfo = TryLogin(userinfo(0), userinfo(1))
            If Linfo(0) <> "3" Then
                Exit While
            Else
                Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "侦测到EULA，重置登录过程")
                Exit Sub
            End If
        End While
        Dim LinfoLen = UBound(Linfo)
        Debug.WriteLine(LinfoLen)
        If Linfo(0) = "0" Then
            MessageBox.Show(Linfo(1), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "登录失败，错误.")
            'DisableOrEnableObject(objlist, True)
        ElseIf Linfo(0) = "1" Then
            StartEVE(Linfo(1))
            Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "登录成功，游戏已启动.")
            MessageBox.Show("游戏已启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DisableOrEnableObject(objlist, True)
        ElseIf Linfo(0) = "2" Then
            StartEVE(Linfo(2))
            Me.Invoke(New voidDelegate(AddressOf UpdateUI4), "登录成功，游戏已启动.")
            MessageBox.Show("游戏已启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ''' <summary>
    ''' 批量处理控件的启用和禁用
    ''' </summary>
    ''' <param name="o">控件列表</param>
    ''' <param name="i">参数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisableOrEnableObject(ByRef o As Object(), ByVal i As Boolean)
        Try
            For Each item As Object In o
                Me.Invoke(New FormDelegate(AddressOf changeItemStatus), i, o(item))
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 启动EVE
    ''' </summary>
    ''' <param name="ss">超时参数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function StartEVE(Optional ByVal ss As String = "")
        Try
            If ss = "" Then ss = sso
            If ss = "ALREADYSTARTED" Then MsgBox("您的登录已超时，请重新登录")
            Process.Start(Application.StartupPath + "\bin\exefile.exe", "/noconsole /ssoToken=" + ss)

        Catch ex As Exception
            MsgBox("打开ExeFile.exe出错，请检查程序是否放置于EVE根目录下！")
            exitprog()
            stopping = 1
            Return False
            'pingserver()
            'Me.Close()
            'Exit Function
        End Try
        If ss <> "" Then
            Timer1.Interval = 300 * 1000
            Timer1.Start()
            不退出重新登录ToolStripMenuItem.Enabled = True
            Overtime = False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 刷新填表菜单
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub flushMenuItems()
        Dim f As ToolStripMenuItem = MenuStrip1.Items(0)
        Dim doitem As ToolStripMenuItem = f.DropDownItems(0)
        Dim CMStripitem As ToolStripMenuItem = ContextMenuStrip1.Items(1)
        'Dim doitem2 As ToolStripMenuItem = CMStripitem.DropDownItems(0)
        doitem.DropDownItems.Clear()
        CMStripitem.DropDownItems.Clear()
        ComboBox1.Items.Clear()
        For i As Integer = 1 To UBound(aAccounts)
            doitem.DropDownItems.Add(aAccounts(i).user, Nothing, AddressOf menuItem_Click).Name = aAccounts(i).user
            ComboBox1.Items.Add(aAccounts(i).user)
            CMStripitem.DropDownItems.Add(aAccounts(i).user, Nothing, AddressOf menuItem_Click).Name = aAccounts(i).user
        Next
    End Sub

    ''' <summary>
    ''' 更新服务器状态标签并提示服务器开服信息
    ''' </summary>
    ''' <param name="i"></param>
    ''' <remarks></remarks>
    Sub updateUI(ByRef i As String)
        Me.Label4.Text = i
        If i.IndexOf("正常开服") > 0 Then
            'OvalShape1.BorderColor = Color.Lime
            OvalCircle1.Color = New SolidBrush(Color.Lime)
            'OvalShape1.BackColor = Color.Lime
        Else
            'OvalShape1.BorderColor = Color.Red
            OvalCircle1.Color = New SolidBrush(Color.Red)
            'OvalShape1.BackColor = Color.Red
        End If
        Dim KSUB As String = i.Substring(0, 10)
        If KSUB <> FWDStatus Then
            With NotifyIcon1
                .BalloonTipIcon = ToolTipIcon.Info
                .BalloonTipText = i
                .BalloonTipTitle = "提示"
                .ShowBalloonTip(5000)
                Dim x As Audio = New Audio '播放提示音
                x.PlaySystemSound(Media.SystemSounds.Exclamation)
            End With
            FWDStatus = KSUB
        Else
            FWDStatus = KSUB
        End If
    End Sub

    ''' <summary>
    ''' 更新状态标签3
    ''' </summary>
    ''' <param name="str"></param>
    ''' <remarks></remarks>
    Sub UpdateUI2(ByRef str As String)
        'Me.Label1.Text = str
        Me.Label5.Text = str
    End Sub

    ''' <summary>
    ''' 更新状态标签4
    ''' </summary>
    ''' <param name="i"></param>
    ''' <remarks></remarks>
    Sub UpdateUI3(ByRef i As String)
        Me.Label3.Text = i
    End Sub

    ''' <summary>
    ''' 更改某项的显示
    ''' </summary>
    ''' <param name="k"></param>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Sub changeItemStatus(ByRef k As Boolean, ByRef obj As Object)
        obj.enabled = k
    End Sub

    ''' <summary>
    ''' 更新登录状态标签
    ''' </summary>
    ''' <param name="i"></param>
    ''' <remarks></remarks>
    Sub UpdateUI4(ByRef i As String)
        Me.Label4.Text = i
    End Sub

    Sub readInfo(ByRef s As String)
        s = ComboBox1.Text
    End Sub
#End Region

End Class
