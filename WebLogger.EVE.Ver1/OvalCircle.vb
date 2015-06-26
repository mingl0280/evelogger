Public Class OvalCircle
    Dim r As System.Drawing.Rectangle
    Dim g As Graphics
    Dim mw, mh As Integer
    Public Sub New2(ByVal Color As SolidBrush, ByVal wid As Integer, hei As Integer)
        'InitializeComponent()
        Me.Width = wid
        Me.Height = hei
        g = Me.CreateGraphics()
        r = New Rectangle(New Point(0, 0), New Size(Me.Width - 2, Me.Height - 2))
        g.DrawEllipse(New Pen(Brushes.Transparent, 0), r)
        g.FillEllipse(Color, r)
    End Sub
    Public WriteOnly Property Color As SolidBrush
        Set(value As SolidBrush)
            g.FillEllipse(value, r)
        End Set
    End Property
End Class
