using System.Drawing;
using System.Windows.Forms;

public class Head : IDrawable, IContainer{
    public Point Point {
        get {return point;}
        set {point = value;
            fillRectangle = new Rectangle(point.X, point.Y, size.Width, size.Height);}
    }
    public Size Size{
        get {return size;}
        set {size = value;
            fillRectangle = new Rectangle(point.X, point.Y, size.Width, size.Height);}
    }
    private Point point;
    private Size size;

    Rectangle fillRectangle;
    SolidBrush brush = new SolidBrush(Color.Red);

    public Head(){
        Size = new Size(20,20);
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.FillRectangle(brush, fillRectangle);
    }

}