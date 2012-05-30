using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

using Utilities;
using ProjectCommon;
using Logging;

namespace DirectShowPlayer
{
    public abstract class Shape
    {
        protected static int m_Width;
        protected static int m_Height;

        protected static float m_WidthRatio;
        protected static float m_HeightRatio;

        protected static float m_WiimoteLine1YRatio;
        protected static float m_WiimoteLine2YRatio;

        public static Font FontOverlay;

        private const int FLASH_COUNT = 5;

        private static float currHLocationRatio;
        private static float m_Speed;
        private static List<Shape> m_StaticShapes;
        private static List<Shape> m_DynamicShapes;

        private static int m_FlashCounter;

        protected float m_InitialXRatio;
        protected float m_InitialYRatio;

        protected float m_CurrentXRatio;
        protected float m_CurrentYRatio;

        protected Pen m_Pen;


        public static Shape getShape(string []row)
        {
            string foot = row[ProjectConstants.SHAPE_FOOT_INDEX];
            string message = row[ProjectConstants.SHAPE_STEP_NAME_INDEX];
            float time = (float)Convert.ToDouble(row[ProjectConstants.SHAPE_TIME_INDEX]);

//            Console.Out.WriteLine("foot = " + foot + " : message = " + message + " : tine = " + time);
            message = message.Replace(" ", "\r\n");

            float yRatio;

            ImageShape shape = new ImageShape();
            if (foot.CompareTo(ProjectCommon.ProjectConstants.SHAPE_LEFT_FOOT) == 0)
                yRatio = m_WiimoteLine2YRatio - m_HeightRatio / 2;
            else
                yRatio = m_WiimoteLine1YRatio - m_HeightRatio / 2;

            shape.createShape(System.Drawing.Pens.Blue, time / ProjectConstants.TOTAL_FRAME_TIME * -1 , yRatio, message, ProjectConstants.TAP_SHOE_IMAGE_PATH);

            return (Shape) shape;
        }

        public void createShape(Pen p_Pen, float xRatio, float yRatio) 
        {
            m_Pen = p_Pen;
            m_InitialXRatio = xRatio;
            m_InitialYRatio = yRatio;

            m_CurrentXRatio = xRatio;
            m_CurrentYRatio = yRatio;
        }

        public void incrementXPositionRatio(float x) 
        { 
            m_CurrentXRatio = m_CurrentXRatio + x;
            if (m_CurrentXRatio > 5.0F)
                m_CurrentXRatio = 0;
        }

        public void incrementYPositionRatio(float y) 
        { 
            m_CurrentYRatio = m_CurrentYRatio + y;
            if (m_CurrentYRatio > 1)
                m_CurrentYRatio = 0;
        }

        public float getXPositionRatio() { return m_CurrentXRatio; }

        public float getYPositionRatio() { return m_CurrentYRatio; }

        public abstract void draw(Graphics g, int width, int height);

        public static void updateGraphics(Graphics g,int width, int height)
        {
            foreach (Shape shape in m_StaticShapes)
                shape.draw(g, width, height);


            foreach (Shape2D shape in m_DynamicShapes)
            {
                shape.incrementXPositionRatio(m_Speed);
                if (shape.getXPositionRatio() < 0.485 || shape.getXPositionRatio() > .505)
                {
                    shape.setBorder(false);
                    shape.draw(g, width, height);
                }
                else
                {
                    shape.setBorder(true);
                    m_FlashCounter++;
                    if (m_FlashCounter != FLASH_COUNT)
                        shape.draw(g, width, height);
                    else
                        m_FlashCounter = -1;
                }
            }
        }



        #region Wiimote Draw Code

        public static void initializeShapes(int width, int height)
        {
            currHLocationRatio = 0;
            m_Speed = .004F;

            m_FlashCounter = -1;

            m_StaticShapes = new List<Shape>();
            m_DynamicShapes = new List<Shape>();


            float wiimoteLine1YRatio = 3F / 10.0F;
            float wiimoteLine2YRatio = 8F/ 10.0F;

            float shapeRatio = 1.0F/3.0F;

            float shapeXRatio = 1.0F / 32.0F;
            float shapeYRatio = 1.0F / 4.0F;

            int fSize;
            // scale the font size in some portion to the video image
            fSize = 4 * (width / 128);

            FontOverlay = new Font("Times New Roman", fSize, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point);

            m_Width = width;
            m_Height = height;
            m_WidthRatio = shapeXRatio;
            m_HeightRatio = shapeYRatio;
            m_WiimoteLine1YRatio = wiimoteLine1YRatio;
            m_WiimoteLine2YRatio = wiimoteLine2YRatio;

            LineShape wiimoteLine1 = new LineShape();
            wiimoteLine1.createShape(System.Drawing.Pens.DarkOrange, 0, wiimoteLine1YRatio - 0.01F, 1, wiimoteLine1YRatio - 0.01F);
            m_StaticShapes.Add(wiimoteLine1);

            LineShape wiimoteLine1b = new LineShape();
            wiimoteLine1b.createShape(System.Drawing.Pens.DarkOrange, 0, wiimoteLine1YRatio + 0.01F, 1, wiimoteLine1YRatio + 0.01F);
            m_StaticShapes.Add(wiimoteLine1b);

            LineShape wiimoteLine2 = new LineShape();
            wiimoteLine2.createShape(System.Drawing.Pens.DarkOrange, 0, wiimoteLine2YRatio - 0.01F, 1, wiimoteLine2YRatio - 0.01F);
            m_StaticShapes.Add(wiimoteLine2);

            LineShape wiimoteLine2b = new LineShape();
            wiimoteLine2b.createShape(System.Drawing.Pens.DarkOrange, 0, wiimoteLine2YRatio + 0.01F, 1, wiimoteLine2YRatio + 0.01F);
            m_StaticShapes.Add(wiimoteLine2b);

            RectangleShape wiimoteLine1Rect = new RectangleShape();
            wiimoteLine1Rect.createShape(System.Drawing.Pens.Red, 0.49F, wiimoteLine1YRatio - 0.12F);
            m_StaticShapes.Add(wiimoteLine1Rect);

            RectangleShape wiimoteLine2Rect = new RectangleShape();
            wiimoteLine2Rect.createShape(System.Drawing.Pens.Red, 0.49F, wiimoteLine2YRatio - 0.12F);
            m_StaticShapes.Add(wiimoteLine2Rect);

            // scale the font size in some portion to the video image
            fSize = 4 * (m_Width / 64);

            Font LabelFontOverlay = new Font("Times New Roman", fSize, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point);

            StringShape wiimote1Label = new StringShape();
            wiimote1Label.createShape(System.Drawing.Pens.Red, 0.925F, wiimoteLine1YRatio- 0.25F, "L", LabelFontOverlay);
            m_StaticShapes.Add(wiimote1Label);

            StringShape wiimote1Label2 = new StringShape();
            wiimote1Label2.createShape(System.Drawing.Pens.Red, 0.925F, wiimoteLine2YRatio - 0.25F, "R", LabelFontOverlay);
            m_StaticShapes.Add(wiimote1Label2);

            ShapeCSVParser parser = new ShapeCSVParser();
            parser.loadShapeData(m_DynamicShapes);
        }


        #endregion

    }

    public class LineShape : Shape
    {
        protected float m_X2Ratio;
        protected float m_Y2Ratio;

        Point m_P1;
        Point m_P2;

        public LineShape()
        {
            m_P1 = new Point();
            m_P2 = new Point();
        }

        public void createShape(Pen p_Pen, float x1Ratio, float y1Ratio, float x2Ratio, float y2Ratio)
        {
            base.createShape(p_Pen, x1Ratio, y1Ratio);
            m_X2Ratio = x2Ratio;
            m_Y2Ratio = y2Ratio;

        }

        public override void draw(Graphics g, int width, int height)
        {
            m_P1.X = (int)(m_CurrentXRatio * width);
            m_P1.Y = (int)(m_CurrentYRatio * height);

            m_P2.X = (int)(m_X2Ratio * width);
            m_P2.Y = (int)(m_Y2Ratio * height);

            g.DrawLine(m_Pen, m_P1, m_P2);

        }
    }

    public abstract class Shape2D : Shape
    {
        protected String m_Message;
        protected Rectangle m_Rectangle;

        protected bool m_BorderOption;

        public void createShape(Pen p_Pen, float xRatio, float yRatio,string message)
        {
            base.createShape(p_Pen, xRatio, yRatio);
            m_Message = message;
//            m_Message = m_Message.Replace(" ", "\r\n");
            m_Rectangle = new Rectangle(0, 0, 0, 0);
            m_BorderOption = false;
        }

        public void setBorder(bool option)
        {
            m_BorderOption = option;
        }

        protected void setRectangle(int width, int height)
        {
            m_Rectangle.Width = (int)(width * m_WidthRatio);
            m_Rectangle.Height = (int)(height * m_HeightRatio);
            m_Rectangle.X = (int)((width * m_CurrentXRatio));
            m_Rectangle.Y = (int)(height * m_CurrentYRatio);
        }

        public override void draw(Graphics g, int width, int height)
        {
            g.DrawString(m_Message, FontOverlay, System.Drawing.Brushes.Red,
            m_Rectangle.X, m_Rectangle.Y - 0.15F * height, System.Drawing.StringFormat.GenericTypographic);

            if (m_BorderOption)
                g.DrawRectangle(System.Drawing.Pens.Red, m_Rectangle);

        }
    }



    public class RectangleShape : Shape2D
    {

        public override void draw(Graphics g,int width,int height)
        {

            setRectangle(width, height);

            if (m_Rectangle.X > width * 0.4 && m_Rectangle.X < width * 0.6)
                g.DrawRectangle(System.Drawing.Pens.Red, m_Rectangle);
            else
                g.DrawRectangle(m_Pen, m_Rectangle);
        }
    }

    public class StringShape : Shape2D
    {
        private Font m_StringFont;

        public void createShape(Pen p_Pen, float xRatio, float yRatio, string message, Font p_FontOverlay)
        {
            base.createShape(p_Pen, xRatio, yRatio, message);
            m_StringFont = p_FontOverlay;
        }

        public override void draw(Graphics g, int width, int height)
        {
            setRectangle(width, height);
            g.DrawString(m_Message, m_StringFont, System.Drawing.Brushes.Red,
            m_Rectangle.X, m_Rectangle.Y, System.Drawing.StringFormat.GenericTypographic);

        }
    }

    public class CircleShape : Shape2D
    {

        public override void draw(Graphics g, int width, int height)
        {
            setRectangle(width, height);

            if (m_Rectangle.X > width * 0.4 && m_Rectangle.X < width * 0.6)
                g.DrawEllipse(System.Drawing.Pens.Red, m_Rectangle);
            else
                g.DrawEllipse(m_Pen, m_Rectangle);
        }
    }

    public class ImageShape : Shape2D
    {
        private Image m_Image;

        public void createShape(Pen p_Pen, float xRatio, float yRatio, string message,string imageFile)
        {
            base.createShape(p_Pen, xRatio, yRatio,message);

            // Create image.
            m_Image = Image.FromFile(imageFile);
        }

        public override void draw(Graphics g, int width, int height)
        {
            setRectangle(width, height);
            g.DrawImage(m_Image, m_Rectangle);
            base.draw(g, width, height);

        }
    }

    public class ShapeCSVParser
    {
        private CSVFileParser m_Parser;

        public void loadShapeData(List<Shape> p_DynamicShapes)
        {
            m_Parser = new CSVFileParser();
            m_Parser.startParsingCSVData(ProjectConstants.SHAPE_LIST_PATH, 0, 0);

            string[] row;

            while((row = m_Parser.getNextRow(0)) != null)
            {
                Shape l_Shape = Shape.getShape(row);
                p_DynamicShapes.Add(l_Shape);
            }

        }
    }
}
