using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe
{
    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    class MainForm : Form
    {
        private float cellSize;
        bool turn;
        public MainForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(700, 750);
            this.Text = "TicTacToe";
            //Отступ от левого края формы для кнопок меню
            int pvMargin = 80,
                //Высота основных кнопок
                pvHeight = 200;
            //Панель с игровым полем
            Panel main = new Panel()
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
            Label label = new Label()
            {
                Size = new Size(200,30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 15, FontStyle.Regular),
                Top = 10,
                Left = (this.ClientSize.Width - 200) / 2
            };
            //Панель с меню
            Panel menu = new Panel()
            {
                Dock = DockStyle.Fill
            };
            Panel grid = new Panel()
            {
                Size = new Size(600,600),
                Left = (this.ClientSize.Width - 600) /2,
                Top = 50,
                BorderStyle = BorderStyle.FixedSingle,
            };
            grid.Paint += (a, b) =>
            {
                cellSize = (float)grid.ClientSize.Width / (float)Settings.Size;
                using (var p = new Pen(Color.Black, 1))
                {
                    for (int i = 1; i < Settings.Size; i++)
                    {
                        DrawLine(p, grid, new PointF(i * cellSize, 0), new PointF(i * cellSize, grid.ClientSize.Height));
                        DrawLine(p, grid, new PointF(0, i * cellSize), new PointF(grid.ClientSize.Width, i * cellSize));
                    }
                }
                Label(label);
            };
            grid.MouseClick += (a, b) =>
            {
                //индексы массива весов
                int i = (int)(b.Y / cellSize), j = (int)(b.X / cellSize);
                //если ячейка занята не рисовать
                if (Game.Cells[i, j] < 0)
                    return;
                Game.Cells[i, j] = (int)Game.Side;
                switch(Game.GameMode)
                {
                    case GameMode.PvP:
                        if (Game.Side == Side.X)
                        {
                            DrawX(grid, i, j);
                            if (CheckWinner(grid, i, j))
                            {
                                if (Winner())
                                {
                                    Restart(grid);
                                }
                                else
                                {
                                    main.Visible = false;
                                    menu.Visible = true;
                                }
                                return;
                            }
                            Game.Side = Side.O;
                            Label(label);
                        }
                        else
                        {
                            DrawO(grid, i, j);
                            if (CheckWinner(grid, i, j))
                            {
                                if (Winner())
                                {
                                    Restart(grid);
                                }
                                else
                                {
                                    main.Visible = false;
                                    menu.Visible = true;
                                }
                                return;
                            }
                            Game.Side = Side.X;
                            Label(label);
                        }
                        break;
                    case GameMode.PvE:
                        UpdateWeights(i, j);
                        if (Game.Side == Side.X)
                        {
                            DrawX(grid, i, j);
                            if (CheckWinner(grid, i, j))
                            {
                                if (Winner())
                                {
                                    Restart(grid);
                                }
                                else
                                {
                                    main.Visible = false;
                                    menu.Visible = true;
                                }
                                return;
                            }
                            Game.Side = Side.O;
                            //Label(label);
                            /*string tmp = "";
                            foreach (int t in Game.Cells)
                                tmp += $", {t}";
                            MessageBox.Show(tmp);*/
                            int[] coords = AI();
                            if (coords != null)
                            {
                                Game.Cells[coords[0], coords[1]] = (int)Game.Side;
                                UpdateWeights(coords[0], coords[1]);
                                DrawO(grid, coords[0], coords[1]);
                                if (CheckWinner(grid, coords[0], coords[1]))
                                {
                                    if (Winner())
                                    {
                                        Restart(grid);
                                    }
                                    else
                                    {
                                        main.Visible = false;
                                        menu.Visible = true;
                                    }
                                    return;
                                }
                                Game.Side = Side.X;
                                Label(label);
                            }
                        }
                        break;
                    default: break;
                }
                if (!turn)
                    turn = true;
                if (Game.FullCells)
                {
                    if (IsDraw())
                    {
                        Restart(grid);
                    }
                    else
                    {
                        main.Visible = false;
                        menu.Visible = true;
                    }
                }
            };
            Button back = new Button()
            {
                Text = "Назад",
                Width = grid.Width/2 - 10,
                Height = 50,
                Font = new Font("Arial", 15, FontStyle.Regular),
                Top = grid.Bottom + 25,
                Left = grid.Left,
                FlatStyle = FlatStyle.Flat
            };
            back.Click += (a, b) =>
            {
                if (turn && MessageBox.Show("Игра не окончена.\nПрекратить игру?", "Назад",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                main.Visible = false;
                menu.Visible = true;
            };
            Button restart = new Button()
            {
                Text = "Рестарт",
                Width = grid.Width / 2 - 10,
                Height = 50,
                Font = new Font("Arial", 15, FontStyle.Regular),
                Top = grid.Bottom + 25,
                Left = back.Right + 20,
                FlatStyle = FlatStyle.Flat
            };
            restart.Click += (a, b) =>
            {
                if (turn && MessageBox.Show("Игра не окончена.\nНачать заново?", "Рестарт",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                Restart(grid);
            };
            Button pve = new Button()
            {
                Text = "PvE",
                Width = this.ClientSize.Width - pvMargin * 2,
                Height = pvHeight,
                Font = new Font("Arial", 45, FontStyle.Regular),
                Location = new Point(pvMargin, pvMargin),
                FlatStyle = FlatStyle.Flat
            };
            pve.Click += (a, b) =>
            {
                Game.GameMode = GameMode.PvE;
                Restart(grid);
                menu.Visible = false;
                main.Visible = true;
            };
            Button pvp = new Button()
            {
                Text = "PvP",
                Width = this.ClientSize.Width - pvMargin * 2,
                Height = pvHeight,
                Font = new Font("Arial", 45, FontStyle.Regular),
                Top = pve.Bottom + 30,
                Left = pve.Left,
                FlatStyle = FlatStyle.Flat
            };
            pvp.Click += (a, b) =>
            {
                Game.GameMode = GameMode.PvP;
                Restart(grid);
                menu.Visible = false;
                main.Visible = true;
            };
            Button options = new Button()
            {
                Text = "Настройки",
                Width = pvp.Width / 3 - 10,
                Height = pvHeight / 2,
                Font = new Font("Arial", 18, FontStyle.Regular),
                Top = pvp.Bottom + 30,
                Left = pvp.Left,
                FlatStyle = FlatStyle.Flat
            };
            options.Click += (a, b) =>
            {
                new SettingsForm().ShowDialog();
            };
            Button help = new Button()
            {
                Text = "Справка",
                Width = pvp.Width / 3 - 10,
                Height = pvHeight / 2,
                Font = new Font("Arial", 18, FontStyle.Regular),
                Top = pvp.Bottom + 30,
                Left = options.Right + 15,
                FlatStyle = FlatStyle.Flat
            };
            help.Click += (a, b) =>
            {
                Process.Start(@"Help.txt");
            };
            Button exit = new Button()
            {
                Text = "Выход",
                Width = pvp.Width / 3 - 10,
                Height = pvHeight / 2,
                Font = new Font("Arial", 18, FontStyle.Regular),
                Top = pvp.Bottom + 30,
                Left = help.Right + 15,
                FlatStyle = FlatStyle.Flat
            };
            exit.Click += (a, b) =>
            {
                this.Dispose();
                Environment.Exit(0);
            };
            main.Controls.Add(label);
            main.Controls.Add(grid);
            main.Controls.Add(back);
            main.Controls.Add(restart);
            menu.Controls.Add(pve);
            menu.Controls.Add(pvp);
            menu.Controls.Add(options);
            menu.Controls.Add(help);
            menu.Controls.Add(exit);
            this.Controls.Add(menu);
            this.Controls.Add(main);
        }
        /// <summary>
        /// Рисует линию на панеле через две точки.
        /// </summary>
        /// <param name="p">Кисть</param>
        /// <param name="pnl">Панель</param>
        /// <param name="p1">Начальная точка</param>
        /// <param name="p2">Конечная точка</param>
        public void DrawLine(Pen p, Panel pnl, PointF p1, PointF p2)
        {
            using (var g = pnl.CreateGraphics())
            {
                g.DrawLine(p, p1, p2);
            }
        }
        /// <summary>
        /// Рисует овал на панеле.
        /// </summary>
        /// <param name="p">Кисть</param>
        /// <param name="pnl">Панель</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        public void DrawEllipse(Pen p, Panel pnl, float x, float y, float width, float height)
        {
            using (var g = pnl.CreateGraphics())
            {
                g.DrawEllipse(p, x, y, width, height);
            }
        }
        /// <summary>
        /// Рисует крестик в ячейке.
        /// </summary>
        /// <param name="grid">Игровое поле</param>
        /// <param name="i">Строка</param>
        /// <param name="j">Столбец</param>
        public void DrawX(Panel grid, int i, int j)
        {
            using (var pen = new Pen(Settings.XColor, 4))
            {
                DrawLine(pen, grid, new PointF(j * cellSize + 5, i * cellSize + 5), new PointF((j + 1) * cellSize - 5, (i + 1) * cellSize - 5));//отрисовка крестика
                DrawLine(pen, grid, new PointF(j * cellSize + 5, (i + 1) * cellSize - 5), new PointF((j + 1) * cellSize - 5, i * cellSize + 5));//отрисовка крестика
            }
        }
        /// <summary>
        /// Рисует нолик в ячейке.
        /// </summary>
        /// <param name="grid">Игровое поле</param>
        /// <param name="i">Строка</param>
        /// <param name="j">Столбец</param>
        public void DrawO(Panel grid, int i, int j)
        {
            using (var pen = new Pen(Settings.OColor, 3))
            {
                DrawEllipse(pen, grid, j * cellSize + 5, i * cellSize + 5, cellSize - 10, cellSize - 10);
            }
        }
        /// <summary>
        /// Меняет текст метки с текущим ходом.
        /// </summary>
        /// <param name="label">Метка с текущим ходом</param>
        public void Label(Label label)
        {
            switch(Game.Side)
            {
                case Side.X:
                    label.Text = "Ходят крестики";
                    label.ForeColor = Settings.XColor;
                    break;
                case Side.O:
                    label.Text = "Ходят нолики";
                    label.ForeColor = Settings.OColor;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Определяет победу.
        /// </summary>
        /// <param name="grid">Игровое поле</param>
        /// <param name="row">Строка</param>
        /// <param name="col">Столбец</param>
        

       
        /// <summary>
        /// Рестарт игры
        /// </summary>
        /// <param name="grid">Игровое поле</param>
        public void Restart(Panel grid)
        {
            Game.Restart();
            grid.Refresh();
            turn = false;
        }
        
        
       
        public bool IsDraw()
        {
            if(Game.FullCells && MessageBox.Show("Ничья.\nНачать заново?","Ничья",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
