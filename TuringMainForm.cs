using System.Windows.Forms;
using System.Drawing;
using System;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;

class TuringMainForm : Form{
    public static readonly int FormHeight = 720;
    public static readonly int FormWidth = 1280;

    List<Simulator> simulators = new List<Simulator>();
    Queue<Simulator> simulatorsToBeRemoved = new Queue<Simulator>();

    private Button buttonAdd;

    Font font;
    float fontSize = 16;
    SolidBrush textBrush = new SolidBrush(Color.Black);
    SolidBrush rectBrush = new SolidBrush(Color.Green);
    FontFamily fontFamily = new FontFamily("Arial");
    Point messagePoint;
    string message;
    public TuringMainForm(){
        FormInit();

        buttonAdd = new Button(() => CreateSimulator());
        buttonAdd.Point = new Point(FormWidth/2, 0); 
        buttonAdd.Text = "Add";
        font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        messagePoint = new Point(50, buttonAdd.Size.Height + 3);

    }

    private void QueueForSimulatorRemoval(Simulator simulator){
        simulatorsToBeRemoved.Enqueue(simulator);
    }

    private void CreateSimulator(){
        try{
            simulators.Add(new Simulator(this.Invalidate, QueueForSimulatorRemoval));
            Reposition();
        }catch (Exception e){
            message = e.Message;
            this.Invalidate();
            return;
        }
    }

    private void Reposition(){
        for(int i = 0; i < simulators.Count; i++){
            simulators[i].Size = new Size (FormWidth - 50, 100);
            simulators[i].Point = new Point (10, 50 + i * (simulators[i].Size.Height + 10));
        }
    }

    private void FormInit(){
        this.SetBounds(10, 10, FormWidth, FormHeight);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;       
        this.DoubleBuffered = true;
        this.Paint += new PaintEventHandler(this.OnDraw);
        this.MouseClick += new MouseEventHandler(this.OnClick);
        this.Closing += new CancelEventHandler(this.OnClosing);
    }

    private void OnDraw(object sender, PaintEventArgs e){
        foreach(var simulator in simulators)
            simulator.Draw(sender, e);
        buttonAdd.Draw(sender, e);
        e.Graphics.DrawString(message, font, textBrush, messagePoint);

    }

    private void OnClick(object sender, MouseEventArgs e){
        foreach(var simulator in simulators)
            simulator.Click(sender, e);
        buttonAdd.OnClick(sender, e);

        for (int i = 0; i < simulatorsToBeRemoved.Count; i++){
            simulators.Remove(simulatorsToBeRemoved.Dequeue());
            Reposition();
            this.Invalidate();
        }
    }

    private void OnClosing(object sender, CancelEventArgs e){
        
    }

}