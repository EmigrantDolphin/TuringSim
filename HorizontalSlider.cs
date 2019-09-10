using System.Drawing;
using System.Windows.Forms;
using System;

public class HorizontalSlider : IContainer, IDrawable{
    
    public Point Point{
        get { return point; }
        set { point = value;
              sliderRectangle = new Rectangle(point, size);}
    }
    public Size Size{
        get { return size; }
        set { size = value;
              size.Width = value.Width == 0 ? 1 : value.Width; 
              sliderRectangle = new Rectangle(point, size);}
    }
    public int TotalItemSize{
        get { return totalItemSize; }
        set { totalItemSize = value;
              CalculateSliderSize();}
    }

    private int totalItemSize = 1;
    private Point point = new Point(0,0);
    private Size size = new Size(1,1);
    private Rectangle sliderRectangle = new Rectangle(new Point(0, 0), new Size(1, 1));
    private float sliderRatio;
    private bool isHolding = false;
    private bool isEnabled = false;
    private int savedMouseX = 0;

    private readonly SolidBrush defaultBrush = new SolidBrush(Color.FromArgb(100,100,100));
    private readonly SolidBrush holdingBrush = new SolidBrush(Color.FromArgb(150,150,150));
    private SolidBrush sliderBrush;
    
    public HorizontalSlider(){
        sliderBrush = defaultBrush;
        CalculateSliderSize();
    }

    private void CalculateSliderSize(){
        sliderRatio = (totalItemSize / (float)size.Width);

        if (sliderRatio == 0)
            sliderRatio = 1;

        int sliderSize = (int)(size.Width / sliderRatio);
        sliderRectangle.Width = sliderSize;
        if (sliderRatio > 1)
            isEnabled = true;
        else
            isEnabled = false;
    }

    public void OnMouseClick(object sender, MouseEventArgs e){
        if (!isEnabled)
            return;
        if (sliderRectangle.Contains(e.Location)){
            isHolding = true;
            savedMouseX = e.Location.X;
            sliderBrush = holdingBrush;
        }
    }
    public void OnMouseUp(object sender, MouseEventArgs e){
        if (!isEnabled)
            return;
        isHolding = false;
        sliderBrush = defaultBrush;
    }
    public void OnMouseMove(object sender, MouseEventArgs e, ref int xOffset){
        if (!isEnabled)
            return;

        if (isHolding){
            int deltaMouseX = e.Location.X - savedMouseX;
            if (sliderRectangle.X + deltaMouseX >= point.X && 
                sliderRectangle.X + sliderRectangle.Width + deltaMouseX < point.X + size.Width){

                sliderRectangle.X += e.Location.X - savedMouseX;
                
                xOffset -= (int)((e.Location.X - savedMouseX) * sliderRatio);
                savedMouseX = e.Location.X;
            }
            if (sliderRectangle.X + deltaMouseX < 0){
                xOffset = 0;
                sliderRectangle.X = point.X;
            }
            if (sliderRectangle.X + sliderRectangle.Width + deltaMouseX > point.X + size.Width){
                sliderRectangle.X = point.X + size.Width - sliderRectangle.Width;
                xOffset = -(int)(sliderRectangle.X * sliderRatio);
            }
        }

    }

    public void Draw(object sender, PaintEventArgs e){
        if (!isEnabled)
            return;
        e.Graphics.FillRectangle(sliderBrush, sliderRectangle);
    }
}