using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{

    public partial class Form1 : Form
    {
        private Game game=new Game(gridSize,toWin);
        private static int cellSize = 25,//размер одной ячейки (cellSize x cellSize) (для 30х30 размер = 30)
            gridSize =30,//размер сетки (gridSize x gridSize) (по условию 30)
            toWin = 5,//фигур для победы (по условию 5)
            indentX = 30,//отступ от правого верхнего края по Х
            indentY = 150;//отступ от правого верхнего края по Y
        private bool xTurn = true;
        private void Form1_Click(object sender, EventArgs e)
        {
            if(game.field[((Cursor.Position.X - this.Location.X) - indentX - 8) / cellSize, ((Cursor.Position.Y - this.Location.Y) - indentY - 31) / cellSize]==0)
            if (xTurn)//первые-крестики
            {
                //защита от клика вне поля
                if (((Cursor.Position.X - this.Location.X) - indentX - 8) / cellSize < 30 && ((Cursor.Position.Y - this.Location.Y) - indentY - 31) / cellSize < 30 && ((Cursor.Position.Y - this.Location.Y) - indentY - 31) > -1 && ((Cursor.Position.X - this.Location.X) - indentX - 8) > -1)
                
                {
                    game.field[((Cursor.Position.X - this.Location.X) - indentX - 8) / cellSize, ((Cursor.Position.Y - this.Location.Y) - indentY - 31) / cellSize] = 1;
                    xTurn = !xTurn;//смена хода
                }
            }
            else
            {
                //защита от клика вне поля
                if (((Cursor.Position.X - this.Location.X) - indentX - 8) / cellSize < 30 && ((Cursor.Position.Y - this.Location.Y) - indentY - 31) / cellSize < 30 && ((Cursor.Position.Y - this.Location.Y) - indentY - 31) > -1 && ((Cursor.Position.X - this.Location.X) - indentX - 8) > -1)
                {
                    game.field[((Cursor.Position.X - this.Location.X) - indentX - 8) / cellSize, ((Cursor.Position.Y - this.Location.Y) - indentY - 31) / cellSize] = 2;
                    xTurn = !xTurn;//смена хода
                }
            }

            Refresh();
        }
public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(30+ indentX + cellSize * gridSize, 30+ indentY + cellSize * gridSize);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            Pen xPen = new Pen(Color.Blue);
            xPen.Width = 4;
            Pen oPen = new Pen(Color.Red);
            oPen.Width = 3;
            Pen finishPen = new Pen(Color.DeepPink);
            finishPen.Width = 5;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    switch(game.field[i, j])
                    {
                        case 1:
                            g.DrawLine(xPen, i * cellSize+ indentX + 5, j * cellSize + indentY + 5, (i+1) * cellSize + indentX -5 , (j+1) * cellSize + indentY - 5);//отрисовка крестика
                            g.DrawLine(xPen, (i + 1) * cellSize + indentX - 5 , j * cellSize + indentY + 5, i * cellSize + indentX + 5, (j + 1) * cellSize + indentY - 5);//отрисовка крестика
                            break;
                        case 2:
                            g.DrawEllipse(oPen, i * cellSize + indentX + 4, j * cellSize + indentY + 4, cellSize - 8, cellSize - 8);//отрисовка нолика
                            break;
                        case 0:
                            break;
                    }
                        
                }
            }
            
            for (int i = indentY; i <= cellSize * gridSize + indentY; i += cellSize)//отрисовка поля
            {
                g.DrawLine(pen, indentX, i, cellSize * gridSize + indentX, i);
            }
            for (int i = indentX; i <= cellSize * gridSize + indentX; i += cellSize)//отрисовка поля
            {
                g.DrawLine(pen, i, indentY, i, cellSize * gridSize + indentY);
            }
            int[] winLine = game.CheckVinner();
            if (winLine.Length == 4)//если кто-то выиграл
            {
                g.DrawLine(finishPen,(int)(winLine[0]*cellSize+indentX+0.5*cellSize), (int)(winLine[1] * cellSize + indentY + 0.5 * cellSize), (int)(winLine[2] * cellSize + indentX + 0.5 * cellSize), (int)(winLine[3] * cellSize + indentY + 0.5 * cellSize));

                ShowMessage(game.field[winLine[0], winLine[1]]);
            }


        }
        private void ShowMessage(int x)
        {
            if (x == 1)
            {

                MessageBox.Show("Победили крестики");

            }
            else
            {

                MessageBox.Show("Победили нолики");

            }

        }




    }
    public class Game
    {
        public int[,] field { get; set; }
        public int toWin { get; set; }
        public Game(int gridSize,int toWin)
        {
            field = new int[gridSize, gridSize];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(0); j++)
                {
                    field[i, j] = 0;
                }
            }
            this.toWin = toWin;
        }
        public int[] CheckVinner()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(0); j++)
                {
                    if (i <= field.GetLength(0) - toWin)
                    {
                        if (CheckLine(i, j, 0))
                        {

                            return new int[4] { i, j, i + toWin-1, j };
                        }//if

                        if (j >= toWin - 1)
                        {
                            if (CheckLine(i, j, 1))
                            {

                                return new int[4] { i, j, i + toWin - 1, j - toWin + 1 };
                            }//if
                        }//if

                    }//if





                    if (j >= toWin - 1)
                    {
                        if (CheckLine(i, j, 2))
                        {

                            return new int[4] { i, j, i, j - toWin+1 };
                        }//if
                        if (i >= toWin-1)
                        {
                            if (CheckLine(i, j, 3))
                            {

                                return new int[4] { i, j, i - toWin + 1, j - toWin + 1 };
                            }//if

                        }//if
                    }//if



                }//for
            }//for
            return new int[1] { 0 };
        }//checkVinner

        private bool CheckLine(int i1,int j1,int k)//k-параметр, от которого зависит наклон линии, которую будут проверять(0=0градусов, 1=45, 2=90, 3=135)
        {
            int count=0;
            switch(k){
                case 0:
                    while (field[i1,j1]!=0&&field[i1++,j1]==field[i1,j1])
                    {
                        if (++count == toWin-1)
                            return true;
                    }
                    
                    break;
                case 1:
                    while (field[i1, j1] != 0 && field[i1++, j1--] == field[i1, j1])
                    {
                        if (++count == toWin - 1)
                            return true;
                    }
                    break;
                case 2:
                    while (field[i1, j1] != 0 && field[i1, j1--] == field[i1, j1])
                    {
                        if (++count == toWin - 1)
                            return true;
                    }
                    break;
                case 3:
                    while (field[i1, j1] != 0 && field[i1--, j1--] == field[i1, j1])
                    {
                        if (++count == toWin - 1)
                            return true;
                    }
                    break;
            }
            return false;
        }


    }
}
