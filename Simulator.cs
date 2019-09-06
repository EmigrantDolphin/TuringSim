using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System;
using System.Collections.Generic;


class Simulator : IContainer, IDrawable {
    enum Direction { R, L, Stay }
    struct Commands{
        public string curState {get; internal set;}
        public string curSimbol {get; internal set;}
        public string newSimbol {get; internal set;}
        public Direction dir {get; internal set;}
        public string newState {get; internal set;}

        public Commands(string curState, string curSimbol, string newSimbol, Direction dir, string newState){
            this.curState = curState;
            this.curSimbol = curSimbol;
            this.newSimbol = newSimbol;
            this.dir = dir;
            this.newState = newState;
        }
    }

    public Point Point {
        get{ return point;}
        set{ point = value;
             outlineRectangle = new Rectangle(value.X, value.Y, size.Width, size.Height);}
    }
    public Size Size {
        get{return size;}
        set{size = value;
            outlineRectangle = new Rectangle(point.X, point.Y, value.Width, value.Height);}
    }
    private Point point = new Point(10, 10);
    private Size size = new Size(50, 50);
    private Rectangle outlineRectangle;

    private List<string> tape = new List<string>();
    private List<Simbol> simbol = new List<Simbol>();
    private int simbolXOffset = 1;
    private int simbolYOffset = 5;
    private Dictionary<string, Commands> commands = new Dictionary<string, Commands>();
    private int headPos;
    private bool isDataGathered = false;

    private Button button;



    public Simulator(){
        button = new Button(() => {Console.WriteLine("SS");});
        GatherData();
        outlineRectangle = new Rectangle(point.X, point.Y, size.Width, size.Height);
    }

    private void GatherData(){
        string filePath = "";
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text Files|*.txt";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
            filePath = openFileDialog.FileName;

        
        if (filePath == ""){
            //error no path selected;
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        
        if (lines.Length < 3){
            //error not enough lines
            return;
        }
        
        if (!int.TryParse(lines[0], out headPos)){
            //error head pos is not a number;
            return;
        }

            for (int i = 0; i < lines[1].Length; i++){
                simbol.Add(new Simbol(lines[1][i].ToString()));
                simbol[i].Point = new Point(i * (simbol[i].Size.Width + simbolXOffset) + this.Point.X + simbolXOffset, this.Point.Y + simbolYOffset);
            }


        for (int i = 2; i < lines.Length; i++){
            string[] commandData = lines[i].Split(" ");
            if (commandData.Length != 5){
                //error command has to have 5 columns
                Console.WriteLine("Need to have 5 commands. Commands found: "+ commandData.Length.ToString());
                continue;
            }
            string curState = commandData[0];
            string curSim = commandData[1];
            string newSim = commandData[2];
            commandData[3] = commandData[3].ToLower();
            Direction dir = Direction.Stay;
            if (commandData[3] == "l")
                dir = Direction.L;
            else if (commandData[3] == "r")
                dir = Direction.R;
            else if (commandData[3] == "*")
                dir = Direction.Stay;
            else{
                //error unknown direction on line i
                Console.WriteLine("unknown dir: "+ commandData[3]);
                return;
            }
            string newState = commandData[4];
            Commands comms = new Commands(curState, curSim, newSim, dir, newState);
            try{
                commands.Add(comms.curState+comms.curSimbol, comms);
            }catch (ArgumentException){
                Console.WriteLine("Program contains duplicate commands");
            }
        }

        isDataGathered = true;
        Console.WriteLine("Data Gathered. Command Lines: "+ commands.Count);

    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.DrawRectangle(new Pen(Color.Red), outlineRectangle);
        button.Draw(sender, e);
        foreach (var sim in simbol)
            sim.Draw(sender, e);
    }

    public void Click(object sender, MouseEventArgs e){
        Console.WriteLine(e.Location);
        button.OnClick(sender, e);
    }
    
}