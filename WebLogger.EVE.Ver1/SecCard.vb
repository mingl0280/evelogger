Public Class SecCard
    Private Sub SecCard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub New(ByVal Loc1, ByVal Loc2, ByVal Loc3)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Label1.Text = Loc1
        Label2.Text = Loc2
        Label3.Text = Loc3
    End Sub


    ''' <summary>
    ''' PassCard Value 1
    ''' </summary>
    ''' <returns>Value box 1</returns>
    Public ReadOnly Property Value1 As String
        Get
            Return TextBox1.Text
        End Get
    End Property

    ''' <summary>
    ''' PassCard Value 2
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Value2 As String
        Get
            Return TextBox2.Text
        End Get
    End Property

    ''' <summary>
    ''' Passcard Value 3
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Value3 As String
        Get
            Return TextBox3.Text
        End Get
    End Property

    Public Sub SetLocation(ByVal Loc1 As String, ByVal loc2 As String, ByVal loc3 As String)
        Label1.Text = Loc1
        Label2.Text = loc2
        Label3.Text = loc3
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class