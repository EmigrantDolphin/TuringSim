using System.Windows.Forms;
using System.Drawing;

class Simbol : IContainer, IDrawable{
    public Point Point {get; set;} = new Point(13, 13);
    public Size Size {get; set;} = new Size (20, 20);

    public string Value {get; set;}
    Font font;
    readonly float fontSize = Settings.TapeFontSize;
    SolidBrush brush;
    public Simbol(string value){
        Value = value;
        font = new Font(new FontFamily("Arial"), fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        Size = new Size((int)fontSize, (int)fontSize);
        brush = new SolidBrush(Color.Black);
    }

    public void Draw(object sender, PaintEventArgs e){
        
        e.Graphics.DrawString(Value, font, brush, this.Point);
    }

}