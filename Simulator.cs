using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

class Simulator : IContainer, IDrawable {
    enum Direction { R, L, Stay }
    struct Command{
        public string curState {get; internal set;}
        public string curSimbol {get; internal set;}
        public string newSimbol {get; internal set;}
        public Direction dir {get; internal set;}
        public string newState {get; internal set;}

        public Command(string curState, string curSimbol, string newSimbol, Direction dir, string newState){
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
             Reposition();}
    }
    public Size Size {
        get{return size;}
        set{size = value;
            Reposition();}
    }
    private Point point = new Point(10, 10);
    private Size size = new Size(50, 50);
    private Rectangle outlineRectangle;

    private readonly List<Simbol> tape = new List<Simbol>();
    private readonly int simbolXOffset = 1;
    private readonly int simbolYOffset = 5;
    private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();
    private int headPos;
    private readonly List<Button> buttons = new List<Button>();

    private DateTime savedTime = DateTime.Now;
    private float headMoveInterval = 0.3f;
    private Simbol selectedSimbol;
    private Command selectedCommand;
    private readonly Head head;

    private Thread thread;

    private readonly Action invalidate;
    private readonly Action<Simulator> exit;

    Text text;

    public Simulator(Action invalidate, Action<Simulator> exit){
        this.invalidate = invalidate;
        this.exit = exit;
 
        GatherData();

        selectedSimbol = tape[headPos];
        selectedCommand = new Command("","","",Direction.Stay,"0"); // first command only new state matters.
        head = new Head();
        text = new Text();
        InitButtons();
        Reposition();
        invalidate();
    }

    private void Reposition(){
        for (int i = 0; i < tape.Count; i++)
            tape[i].Point = new Point(i * (tape[i].Size.Width + simbolXOffset) + this.Point.X + simbolXOffset, this.Point.Y + simbolYOffset);
        head.Point = new Point(selectedSimbol.Point.X, selectedSimbol.Point.Y + selectedSimbol.Size.Height + 2);
        outlineRectangle = new Rectangle(point.X, point.Y, size.Width, size.Height);
        text.Point = new Point(point.X + 3, head.Point.Y + head.Size.Height + 3);

        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Point = new Point(point.X + 10 + (buttons[i].Size.Width + 10) * i, 
                                         point.Y + size.Height - buttons[i].Size.Height);

    }

    private void InitButtons(){
        var startButton = new Button(() => {
            if (thread == null)
                thread = new Thread(new ThreadStart(Loop));
            if (!thread.IsAlive)
                thread.Start();   
        });
        var stopButton = new Button(() => {
            if (thread != null){
                thread.Abort();
                thread = null;   
            }
        });
        var stepButton = new Button(() => ProcessSelectedSimbol());
        var exitButton = new Button(() => {
            if (thread != null)
                thread.Abort();
            exit(this);
        });

        startButton.Text = "Start";
        stopButton.Text = "Stop";
        stepButton.Text = "Step";
        exitButton.Text = "Exit";

        buttons.Add(startButton);
        buttons.Add(stopButton);
        buttons.Add(stepButton);
        buttons.Add(exitButton);
    }

    private void GatherData(){
        string filePath = "";
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text Files|*.txt";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
            filePath = openFileDialog.FileName;
        else
            throw new Exception("File Path was not selected");

        string[] lines = File.ReadAllLines(filePath);
        
        if (lines.Length < 3)
            throw new Exception("File requires 3 lines minimum. Head position, tape and a command");
        
        if (!int.TryParse(lines[0], out headPos))
            throw new Exception("Head position is not a number");

        for (int i = 0; i < lines[1].Length; i++)
            tape.Add(new Simbol(lines[1][i].ToString()));
        if (headPos < 0 || headPos >= tape.Count)
            throw new Exception("Defined head position is out of bounds from tape");

        for (int i = 2; i < lines.Length; i++){
            string[] commandData = lines[i].Split(" ");
            if (commandData.Length < 5){
                // skipping empty lines
                continue;
            }
            string curState = commandData[0];
            string curSim = commandData[1];
            if (curSim == "_")
                curSim = " ";
            string newSim = commandData[2];
            if (newSim == "_")
                newSim = " ";

            commandData[3] = commandData[3].ToLower();
            Direction dir = Direction.Stay;
            if (commandData[3] == "l")
                dir = Direction.L;
            else if (commandData[3] == "r")
                dir = Direction.R;
            else if (commandData[3] == "*")
                dir = Direction.Stay;
            else
                throw new Exception("unknown direction: " + commandData[3]);

            string newState = commandData[4];
            Command comms = new Command(curState, curSim, newSim, dir, newState);
            try{
                commands.Add(comms.curState+comms.curSimbol, comms);
            }catch (ArgumentException){
                throw new Exception("Program contains duplicate commands");
            }
        }

        //Console.WriteLine("Data Gathered. Command Lines: "+ commands.Count);

    }

    public void Loop(){
        while(true)
            if ((DateTime.Now - savedTime).Seconds > headMoveInterval){
                ProcessSelectedSimbol();
                savedTime = DateTime.Now;
            }
    }

    private void ProcessSelectedSimbol(){
        try{
            try{
                selectedCommand = commands[selectedCommand.newState+selectedSimbol.Value];
            }catch(Exception){
                throw new Exception("new state doesn't contain this simbol");
            }
        }catch(Exception){
            try{
                selectedCommand = commands[selectedCommand.newState+"*"];
            }catch(Exception){
                if (selectedCommand.newState.ToLower().Contains("halt"))
                    text.Message = "Halted";
                else
                    text.Message = "No command found for state: " + selectedCommand.newState + ", simbol: " + selectedSimbol.Value;
                invalidate();
                if (thread != null)
                    thread.Abort();
                return;
            }
        }


        if (selectedCommand.newSimbol != "*")
            selectedSimbol.Value = selectedCommand.newSimbol;

        if (selectedCommand.dir == Direction.L){
            headPos--;
            if (headPos < 0){
                tape.Insert(0, new Simbol(" "));
                headPos++;
                Reposition();
            }
        }
        if (selectedCommand.dir == Direction.R){
            headPos++;
            if (headPos >= tape.Count){
                tape.Add(new Simbol(" "));
                Reposition();
            }
        }
        selectedSimbol = tape[headPos];
        head.Point = new Point(selectedSimbol.Point.X, head.Point.Y);
        invalidate();
    }

    public void Draw(object sender, PaintEventArgs e){
        e.Graphics.DrawRectangle(new Pen(Color.Red), outlineRectangle);
        text.Draw(sender, e);
        head.Draw(sender, e);
        foreach (var simbol in tape)
            simbol.Draw(sender, e);
        foreach (var button in buttons)
            button.Draw(sender, e);
    }

    public void Click(object sender, MouseEventArgs e){
        foreach(var button in buttons)
            button.OnClick(sender, e);
    }
    public void OnMouseMove(object sender, MouseEventArgs e){
        foreach(var button in buttons)
            button.OnMouseMove(sender, e);
    }

    public void OnClosing(object sender, CancelEventArgs e){
        if (thread != null)
            thread.Abort();
    }
    
}