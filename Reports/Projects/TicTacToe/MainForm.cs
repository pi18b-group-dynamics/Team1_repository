using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    class MainForm: Form
    {
        private int[,] cells = new int[30, 30];
        private Side cur;
        public MainForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Size = new Size(700, 750);
            this.Text = "TicTacToe";
            cur = Side.X;//
            Panel game = new Panel()
            {
                ClientSize = new Size(602,602),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point((this.ClientSize.Width - 602) / 2, 50)
            };
            int w = game.ClientSize.Width / 30,
                height = game.ClientSize.Height,
                width = game.ClientSize.Width;
            game.Paint += (a, b) =>
            {
                using (var p = new Pen(Color.Black, 1))
                {
                    using (var g = game.CreateGraphics())
                    {
                        for (int i = 1; i < 30; i++)
                        {
                            g.DrawLine(p, new Point(i * w, 0), new Point(i * w, height));
                            g.DrawLine(p, new Point(0, i * w), new Point(width, i * w));
                        }
                    }
                }
            };
            game.MouseClick += (o, ea) =>
            {
                using (var p = new Pen(Color.Green, 3))
                {
                    using (var g = game.CreateGraphics())
                    {
                        int i = ea.Y / 20,
                            j = ea.X / 20;
                        if (cells[i, j] < 0)
                            return;
                        //MessageBox.Show($"{i},{j}");
                        if (cur == Side.X)
                        {
                            cells[i, j] = (int)cur;
                            p.Color = Color.Green;
                            g.DrawLine(p, new Point(j*20, i*20), new Point((j+1)*20, (i+1)*20));
                            g.DrawLine(p, new Point((j + 1) * 20, i * 20), new Point(j * 20, (i + 1) * 20));
                            findWin(game, cur, i, j);
                            cur = Side.O;
                            //this.BackColor = Color.Blue;
                        }
                        else
                        {
                            cells[i, j] = (int)cur;
                            p.Color = Color.Blue;
                            g.DrawEllipse(p, new Rectangle(j * 20, i * 20, 20, 20));
                            findWin(game, cur, i, j);
                            cur = Side.X;
                            //this.BackColor = Color.Green;
                        }
                    }
                }
            };
            game.Refresh();
            this.Controls.Add(game);
        }
        /// <summary>
        /// Определяет, приводит ли ход к победе.
        /// </summary>
        /// <author>
        /// Николай Мелещенко
        /// </author>
        /// <param name="pnl">Игровая панель</param>
        /// <param name="side">Текущий ход</param>
        /// <param name="r">Строка</param>
        /// <param name="c">Столбец</param>
        public void findWin(Panel pnl, Side side, int r, int c)
        {
            int rowV = r - 4, colH = c - 4, rowN = r + 4, countV = 0, countH = 0, countS = 0, countN = 0;
            bool V = true, H = true, S = true, N = true;
            if (rowV < 0) rowV = 0;
            if (colH < 0) colH = 0;
            if (rowN > 29) rowN = 29;
            Point startV = new Point(),
                  startH = new Point(),
                  startS = new Point(),
                  startN = new Point(),
                    end;
            for (int i = 0; i < 9; i++)
            {
                try
                {
                    if (V && cells[rowV + i, c] == (int)side)
                    {
                        countV++;
                        if (countV == 1)
                            startV = new Point(c * 20 + 10, (rowV + i) * 20);
                        if (countV == 5)
                        {
                            end = new Point(c * 20 + 10, (rowV + i) * 20 + 20);
                            DrawLine(pnl, Color.Black, 4, startV, end);
                            MessageBox.Show($"Победа {side.ToString()}!");
                            return;
                        }
                    }
                    else
                        countV = 0;
                }catch
                {
                    V = false;
                }
                try
                {
                    if (H && cells[r, colH + i] == (int)side)
                    {
                        countH++;
                        if (countH == 1)
                            startH = new Point((colH + i) * 20, r * 20 + 10);
                        if (countH == 5)
                        {
                            end = new Point((colH + i) * 20 + 20, r * 20 + 10);
                            DrawLine(pnl, Color.Black, 4, startH, end);
                            MessageBox.Show($"Победа {side.ToString()}!");
                            return;
                        }
                    }
                    else
                        countH = 0;
                }catch
                {
                    H = false;
                }
                try
                {
                    if (S && cells[rowV + i, colH + i] == (int)side)
                    {
                        countS++;
                        if (countS == 1)
                            startS = new Point((colH + i) * 20, (rowV + i) * 20);
                        if (countS == 5)
                        {
                            end = new Point((colH + i) * 20 + 20, (rowV + i) * 20 + 20);
                            DrawLine(pnl, Color.Black, 4, startS, end);
                            MessageBox.Show($"Победа {side.ToString()}!");
                            return;
                        }
                    }
                    else
                        countS = 0;
                }catch
                {
                    S = false;
                }
                try
                {
                    if (N && cells[rowN - i, colH + i] == (int)side)
                    {
                        countN++;
                        if (countN == 1)
                            startN = new Point((colH + i) * 20, (rowN + i) * 20 + 20);
                        if (countN == 5)
                        {
                            end = new Point((colH + i) * 20 + 20, (rowN - i) * 20);
                            DrawLine(pnl, Color.Black, 4, startN, end);
                            MessageBox.Show($"Победа {side.ToString()}!");
                            return;
                        }
                    }
                    else
                        countN = 0;
                }catch
                {
                    N = false;
                }

            }
        }
        public void DrawLine(Panel panel, Color color, int width, Point f, Point s)
        {
            using (var p = new Pen(color, width))
            {
                using (var g = panel.CreateGraphics())
                {
                    g.DrawLine(p, f, s);
                }
            }
        }
    }

    enum Side
    {
        X = -1,
        O = -2,
        None = 0
    }
}
