using System.Drawing;
using System;
using System.Windows.Forms;

public class Button : IDrawable, IClickable, IContainer{
    public Point Point {
        get{ return point;}
        set{ point = value;
             fillRectangle = new Rectangle(value.X, value.Y, size.Width, size.Height);
             text.Point = value;}
    }
    public Size Size {
        get{return size;}
        set{size = value;
            fillRectangle = new Rectangle(point.X, point.Y, value.Width, value.Height);}
    }

    public string Text {
        get {return text.Message;}
        set {text.Message = value;}
    }

    
    private Point point;
    private Size size;
    private Rectangle fillRectangle;
    private readonly Action action;

    private readonly Text text = new Text();
    SolidBrush rectBrush;
    private readonly SolidBrush mouseOverBrush = new SolidBrush(Color.FromArgb(0,0,255));
    private readonly SolidBrush defaultBrush = new SolidBrush(Color.FromArgb(0,255,255));


    public Button (Action action){
        Text = "Button";
        rectBrush = defaultBrush;
        this.action = action;
        Size = new Size(50,20);
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.FillRectangle(rectBrush, fillRectangle);
        text.Draw(sender, e);
    }
    public void OnClick(object sender, MouseEventArgs e){
        if (e.Location.X >= Point.X && e.Location.X <= Point.X + Size.Width &&
            e.Location.Y >= Point.Y && e.Location.Y <= Point.Y + Size.Height)
            action();
    }
    public void OnMouseMove(object sender, MouseEventArgs e){
        if (fillRectangle.Contains(e.Location))
            rectBrush = mouseOverBrush;
        else
            rectBrush = defaultBrush;
    }
    
}