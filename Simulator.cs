using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System;
using System.Collections.Generic;


class Simulator : IContainer {
    enum Direction { R, L }
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
        get{return Size;}
        set{size = value;
            outlineRectangle = new Rectangle(point.X, point.Y, value.Width, value.Height);}
    }
    private Point point = new Point(10, 10);
    private Size size = new Size(50, 50);
    private Rectangle outlineRectangle;

    private List<string> tape;
    private Dictionary<string, Commands> commands;
    private int headPos;


    public Simulator(string fileName){
        GatherData();
        outlineRectangle = new Rectangle(point.X, point.Y, size.Width, size.Height);
    }

    private void GatherData(){
        string filePath = "";
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text Files|*.txt";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
            filePath = openFileDialog.FileName;

        
        if (filePath != ""){
            //error no path selected;
            return;
        }

        string[] lines = File.ReadAllLines(@filePath);
        
        if (lines.Length < 3){
            //error not enough lines
            return;
        }

        if (!int.TryParse(lines[0], out headPos)){
            //error head pos is not a number;
            return;
        }
        
        foreach (string simbol in lines[1].Split(null))
            tape.Add(simbol);

        for (int i = 2; i < lines.Length; i++){
            string[] commandData = lines[i].Split(null);
            if (commandData.Length != 4){
                //error command has to have 5 columns
                return;
            }
            string curState = commandData[0];
            string curSim = commandData[1];
            string newSim = commandData[2];
            commandData[3] = commandData[3].ToLower();
            Direction dir = Direction.L;
            if (commandData[3] == "l")
                dir = Direction.L;
            else if (commandData[3] == "r")
                dir = Direction.R;
            else{
                //error unknown direction on line i
                return;
            }
            string newState = commandData[4];
            Commands comms = new Commands(curState, curSim, newSim, dir, newState);
            commands.Add(comms.curState+comms.curSimbol, comms);

        }


        
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.DrawRectangle(new Pen(Color.Red), outlineRectangle);
    }
    
}