<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.客户端设定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.填表ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.清空ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.不退出重新登录ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.打开官方登陆器ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.退出ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.客户端设定ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.打开客户端设定ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.验证客户端版本ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.设置自动保存用户名和密码ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.帮助ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.关于ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.显示登陆器ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.快速登录ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.退出ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.OvalShape1 = New Microsoft.VisualBasic.PowerPacks.OvalShape()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.客户端设定ToolStripMenuItem, Me.客户端设定ToolStripMenuItem1, Me.帮助ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(391, 25)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '客户端设定ToolStripMenuItem
        '
        Me.客户端设定ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.填表ToolStripMenuItem, Me.清空ToolStripMenuItem, Me.不退出重新登录ToolStripMenuItem, Me.打开官方登陆器ToolStripMenuItem, Me.退出ToolStripMenuItem})
        Me.客户端设定ToolStripMenuItem.Name = "客户端设定ToolStripMenuItem"
        Me.客户端设定ToolStripMenuItem.Size = New System.Drawing.Size(68, 21)
        Me.客户端设定ToolStripMenuItem.Text = "登录操作"
        '
        '填表ToolStripMenuItem
        '
        Me.填表ToolStripMenuItem.Name = "填表ToolStripMenuItem"
        Me.填表ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.填表ToolStripMenuItem.Text = "登录"
        '
        '清空ToolStripMenuItem
        '
        Me.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem"
        Me.清空ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.清空ToolStripMenuItem.Text = "清空"
        '
        '不退出重新登录ToolStripMenuItem
        '
        Me.不退出重新登录ToolStripMenuItem.Name = "不退出重新登录ToolStripMenuItem"
        Me.不退出重新登录ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.不退出重新登录ToolStripMenuItem.Text = "不退出重新登录"
        '
        '打开官方登陆器ToolStripMenuItem
        '
        Me.打开官方登陆器ToolStripMenuItem.Name = "打开官方登陆器ToolStripMenuItem"
        Me.打开官方登陆器ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.打开官方登陆器ToolStripMenuItem.Text = "打开官方登陆器"
        '
        '退出ToolStripMenuItem
        '
        Me.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem"
        Me.退出ToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.退出ToolStripMenuItem.Text = "退出"
        '
        '客户端设定ToolStripMenuItem1
        '
        Me.客户端设定ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.打开客户端设定ToolStripMenuItem, Me.验证客户端版本ToolStripMenuItem, Me.设置自动保存用户名和密码ToolStripMenuItem})
        Me.客户端设定ToolStripMenuItem1.Name = "客户端设定ToolStripMenuItem1"
        Me.客户端设定ToolStripMenuItem1.Size = New System.Drawing.Size(80, 21)
        Me.客户端设定ToolStripMenuItem1.Text = "客户端设定"
        '
        '打开客户端设定ToolStripMenuItem
        '
        Me.打开客户端设定ToolStripMenuItem.Name = "打开客户端设定ToolStripMenuItem"
        Me.打开客户端设定ToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.打开客户端设定ToolStripMenuItem.Text = "修复客户端"
        '
        '验证客户端版本ToolStripMenuItem
        '
        Me.验证客户端版本ToolStripMenuItem.Name = "验证客户端版本ToolStripMenuItem"
        Me.验证客户端版本ToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.验证客户端版本ToolStripMenuItem.Text = "验证客户端版本"
        '
        '设置自动保存用户名和密码ToolStripMenuItem
        '
        Me.设置自动保存用户名和密码ToolStripMenuItem.Name = "设置自动保存用户名和密码ToolStripMenuItem"
        Me.设置自动保存用户名和密码ToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.设置自动保存用户名和密码ToolStripMenuItem.Text = "设置自动保存用户名和密码"
        '
        '帮助ToolStripMenuItem
        '
        Me.帮助ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.关于ToolStripMenuItem})
        Me.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem"
        Me.帮助ToolStripMenuItem.Size = New System.Drawing.Size(44, 21)
        Me.帮助ToolStripMenuItem.Text = "帮助"
        '
        '关于ToolStripMenuItem
        '
        Me.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem"
        Me.关于ToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.关于ToolStripMenuItem.Text = "关于"
        '
        'Timer1
        '
        '
        'Timer2
        '
        Me.Timer2.Interval = 1800000
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "EVE简易登陆器"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.显示登陆器ToolStripMenuItem, Me.快速登录ToolStripMenuItem, Me.退出ToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(137, 70)
        '
        '显示登陆器ToolStripMenuItem
        '
        Me.显示登陆器ToolStripMenuItem.Name = "显示登陆器ToolStripMenuItem"
        Me.显示登陆器ToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.显示登陆器ToolStripMenuItem.Text = "显示登陆器"
        '
        '快速登录ToolStripMenuItem
        '
        Me.快速登录ToolStripMenuItem.Name = "快速登录ToolStripMenuItem"
        Me.快速登录ToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.快速登录ToolStripMenuItem.Text = "快速登录"
        '
        '退出ToolStripMenuItem1
        '
        Me.退出ToolStripMenuItem1.Name = "退出ToolStripMenuItem1"
        Me.退出ToolStripMenuItem1.Size = New System.Drawing.Size(136, 22)
        Me.退出ToolStripMenuItem1.Text = "退出"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "用户名"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 12)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "密码"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(85, 35)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(178, 20)
        Me.ComboBox1.TabIndex = 5
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(85, 68)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(178, 21)
        Me.TextBox1.TabIndex = 6
        Me.TextBox1.UseSystemPasswordChar = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(287, 66)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "登录"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(287, 33)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 8
        Me.Button2.Text = "重置"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 12)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "正在测试服务器Ping..."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 126)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(227, 12)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "服务器状态：正常开服 在线人数：123456"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(157, 102)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(137, 12)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "当前客户端已是最新版！"
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.OvalShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(391, 151)
        Me.ShapeContainer1.TabIndex = 13
        Me.ShapeContainer1.TabStop = False
        '
        'OvalShape1
        '
        Me.OvalShape1.BackColor = System.Drawing.Color.Red
        Me.OvalShape1.BackStyle = Microsoft.VisualBasic.PowerPacks.BackStyle.Opaque
        Me.OvalShape1.BorderColor = System.Drawing.Color.Red
        Me.OvalShape1.Location = New System.Drawing.Point(8, 127)
        Me.OvalShape1.Name = "OvalShape1"
        Me.OvalShape1.Size = New System.Drawing.Size(10, 10)
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(247, 126)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(0, 12)
        Me.Label6.TabIndex = 14
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 151)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "EVE Online 简易登陆器"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents 客户端设定ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 打开官方登陆器ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 客户端设定ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 打开客户端设定ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 帮助ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 关于ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 退出ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 验证客户端版本ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 不退出重新登录ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents 设置自动保存用户名和密码ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 填表ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 清空ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 退出ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 显示登陆器ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents OvalShape1 As Microsoft.VisualBasic.PowerPacks.OvalShape
    Friend WithEvents 快速登录ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label6 As System.Windows.Forms.Label

End Class
