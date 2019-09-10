using System.Windows.Forms;
using System.Drawing;

class Simbol : IContainer, IDrawable{
    public Point Point {
        get {return simbol.Point;}
        set {simbol.Point = value;}
    }
    public Size Size {get; set;}

    public string Value {
        get{return simbol.Message;}
        set{simbol.Message = value;}
    }
    private readonly float fontSize = 20;
    private readonly Text simbol;

    public Simbol(string value){
        simbol = new Text(fontSize);
        simbol.Message = value;

        Size = new Size((int)fontSize, (int)fontSize);
    }

    public void Draw(object sender, PaintEventArgs e){      
        simbol.Draw(sender, e);
    }

}