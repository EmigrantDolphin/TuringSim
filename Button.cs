using System.Drawing;
using System;
using System.Windows.Forms;

public class Button : IDrawable, IClickable, IContainer{
    public Point Point {
        get{ return point;}
        set{ point = value;
             fillRectangle = new Rectangle(value.X, value.Y, size.Width, size.Height);}
    }
    public Size Size {
        get{return size;}
        set{size = value;
            fillRectangle = new Rectangle(point.X, point.Y, value.Width, value.Height);}
    }

    public string Text {get; set;} = "Button";

    
    private Point point;
    private Size size;
    private Rectangle fillRectangle;
    private Action action;

    Font font;
    float fontSize = 16;
    SolidBrush textBrush = new SolidBrush(Color.Black);
    SolidBrush rectBrush = new SolidBrush(Color.Green);
    FontFamily fontFamily = new FontFamily("Arial");

    public Button (Action action){
        this.action = action;
        font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        Size = new Size(50,20);
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.FillRectangle(rectBrush, fillRectangle);
        e.Graphics.DrawString(Text, font, textBrush, point);

    }
    public void OnClick(object sender, MouseEventArgs e){
        if (e.Location.X >= Point.X && e.Location.X <= Point.X + Size.Width &&
            e.Location.Y >= Point.Y && e.Location.Y <= Point.Y + Size.Height)
            action();
            Console.WriteLine(e.Clicks);
    }
    
}