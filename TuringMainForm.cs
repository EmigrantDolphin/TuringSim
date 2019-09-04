using System.Windows.Forms;
using System.Drawing;
using System;

class TuringMainForm : Form{
    public static readonly int FormHeight = 720;
    public static readonly int FormWidth = 1280;

    Simulator simulator;

    public TuringMainForm(){
        FormInit();

        simulator = new Simulator("lala");
        simulator.Point = new Point (10, 10);
        simulator.Size = new Size (FormWidth - 50, 100);
    }

    private void FormInit(){
        this.SetBounds(10, 10, FormWidth, FormHeight);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;       
        this.Paint += new PaintEventHandler(this.OnDraw);
        this.MouseClick += new MouseEventHandler(this.OnClick);
    }

    private void OnDraw(object sender, PaintEventArgs e){
        simulator.Draw(sender, e);

    }

    private void OnClick(object sender, MouseEventArgs e){
        Console.WriteLine("ASS");
    }

}