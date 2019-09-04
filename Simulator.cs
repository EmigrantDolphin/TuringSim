using System.Drawing;
using System.Windows.Forms;


class Simulator : IContainer {
    public Point Point {
        get{ return point;}
        set{ point = value;
             outlineRectangle = new Rectangle(value.X, value.Y, size.Width, size.Height);}
    }
    public Size Size {
        get{return Size;}
        set{size = value;
            outlineRectangle = new Rectangle(point.X, point.Y, value.Width, value.Height);}
    }
    private Point point = new Point(10, 10);
    private Size size = new Size(50, 50);

    private Rectangle outlineRectangle;


    public Simulator(string fileName){

        outlineRectangle = new Rectangle(point.X, point.Y, size.Width, size.Height);
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.DrawRectangle(new Pen(Color.Red), outlineRectangle);
    }
    
}