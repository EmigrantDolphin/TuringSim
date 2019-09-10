using System.Windows.Forms;
using System.Drawing;

public class Text : IDrawable{

    public string Message {get; set;}

    public Point Point {get; set;}

    private readonly Font font;
    private float fontSize = 16;
    private readonly SolidBrush textBrush = new SolidBrush(Color.Black);
    private readonly FontFamily fontFamily = new FontFamily("Arial");

    public Text(){
        font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
    }
    public Text(float fontSize){
        font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.DrawString(Message, font, textBrush, Point);
    }
}