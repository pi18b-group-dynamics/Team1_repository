﻿using System;
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
        public bool CheckWinner(Panel grid, int row, int col)
        {
            int win = Settings.Win - 1, rowV = row - win, colH = col - win, rowN = row + win,
                countV = 0, countH = 0, countS = 0, countN = 0;
            PointF startV = new PointF(),
                   startH = new PointF(),
                   startS = new PointF(),
                   startN = new PointF();
            for (int i = 0; i < Settings.Win * 2 - 1; i++)
            {
                try
                {
                    if (Game.Cells[rowV + i, col] == (int)Game.Side)
                    {
                        countV++;
                        if (countV == 1)
                            startV = new PointF(col * cellSize + cellSize / 2, (rowV + i) * cellSize);
                        if (countV == Settings.Win)
                        {
                            using (var p = new Pen(Color.Black, 4))
                            {
                                DrawLine(p, grid, startV, new PointF(col * cellSize + cellSize / 2, (rowV + i + 1) * cellSize));
                            }
                                return true;
                        }
                    }
                    else
                        countV = 0;
                }catch
                {
                }
                try
                {
                    if (Game.Cells[row, colH + i] == (int)Game.Side)
                    {
                        countH++;
                        if (countH == 1)
                            startH = new PointF((colH + i) * cellSize, row * cellSize + cellSize/2);
                        if (countH == Settings.Win)
                        {
                            using (var p = new Pen(Color.Black, 4))
                            {
                                DrawLine(p, grid, startH, new PointF((colH + i + 1) * cellSize, row * cellSize + cellSize / 2));
                            }
                                return true;
                        }
                    }
                    else
                        countH = 0;
                }
                catch
                {
                }
                try
                {
                    if (Game.Cells[rowV + i, colH + i] == (int)Game.Side)
                    {
                        countS++;
                        if (countS == 1)
                            startS = new PointF((colH + i) * cellSize, (rowV + i) * cellSize);
                        if (countS == Settings.Win)
                        {
                            using (var p = new Pen(Color.Black, 4))
                            {
                                DrawLine(p, grid, startS, new PointF((colH + i + 1) * cellSize, (rowV + i + 1) * cellSize));
                            }
                                return true;
                        }
                    }
                    else
                        countS = 0;
                }
                catch
                {
                }
                try
                {
                    if (Game.Cells[rowN - i, colH + i] == (int)Game.Side)
                    {
                        countN++;
                        if (countN == 1)
                            startN = new PointF((colH + i) * cellSize, (rowN - i + 1) * cellSize);
                        if (countN == Settings.Win)
                        {
                            using (var p = new Pen(Color.Black, 4))
                            {
                                DrawLine(p, grid, startN, new PointF((colH + i + 1) * cellSize, (rowN - i) * cellSize));
                            }
                                return true;
                        }
                    }
                    else
                        countN = 0;
                }
                catch
                {
                }
            }
            return false;
        }
        /// <summary>
        /// Выводит сообщение о победителе и запрашивает продолжение.
        /// </summary>
        public bool Winner()
        {
            switch (Game.Side)
            {
                case Side.X:
                    if (MessageBox.Show("Победили крестики.\nНачать заново?",
                        "Победа", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                        return false;
                case Side.O:
                    if (MessageBox.Show("Победили нолики.\nНачать заново?",
                         "Победа", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                        return false;
                default:
                    return false;
            }
        }
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
        /// <summary>
        /// Обновление матрицы весов для ИИ.
        /// </summary>
        /// <param name="i">Строка</param>
        /// <param name="j">Столбец</param>
        public void UpdateWeights(int row, int col)
        {
            int cV = 0, cH = 0, cS = 0, cN = 0,
                rowS = row - col, colS = col - row, rowN = row + col, colN;
            if (rowS < 0) rowS = 0;
            if (colS < 0) colS = 0;
            if (rowN > Settings.Size - 1) rowN = Settings.Size - 1;
            colN = col + row - rowN;
            bool fV = false, fH = false, fS = false, fN = false;
            for(int i = 0; i <= Settings.Size; i++)
            {
                try
                {
                    if (Game.Cells[i, col] == (int)Game.Side)
                    {
                        fV = true;
                        cV++;
                    }
                    else
                    {
                        if (fV)
                        {
                            try
                            {
                                if (Game.Cells[i, col] >= 0 && Game.Cells[i, col] <= cV)
                                    Game.Cells[i, col] = cV;
                            }
                            catch
                            { }
                            try
                            {
                                if (Game.Cells[i - cV - 1, col] >= 0)
                                    try
                                    {
                                        if (Game.Cells[i - cV - 2, col] == (int)Game.Side)
                                        {
                                            Game.Cells[i - cV - 1, col] += cV;
                                        }else
                                        {
                                            if (Game.Cells[i - cV - 1, col] <= cV)
                                                Game.Cells[i - cV - 1, col] = cV;
                                        }
                                    }catch
                                    { 
                                        if(Game.Cells[i - cV - 1, col] <= cV)
                                            Game.Cells[i - cV - 1, col] = cV;
                                    }
                            }
                            catch { }
                            fV = false;
                        }
                        cV = 0;
                    }
                }catch
                {
                    if (cV != 0 && fV)
                    {
                        try
                        {
                            if (Game.Cells[i - cV - 1, col] >= 0)
                                try
                                {
                                    if (Game.Cells[i - cV - 2, col] == (int)Game.Side)
                                    {
                                        Game.Cells[i - cV - 1, col] += cV;
                                    }
                                    else
                                    {
                                        if (Game.Cells[i - cV - 1, col] <= cV)
                                            Game.Cells[i - cV - 1, col] = cV;
                                    }
                                }
                                catch
                                {
                                    if (Game.Cells[i - cV - 1, col] <= cV)
                                        Game.Cells[i - cV - 1, col] = cV;
                                }
                        }
                        catch { }
                        fV = false;
                    }
                }
                try
                {
                    if (Game.Cells[row, i] == (int)Game.Side)
                    {
                        fH = true;
                        cH++;
                    }
                    else
                    {
                        if (fH)
                        {
                            try
                            {
                                if (Game.Cells[row, i] >= 0 && Game.Cells[row, i] <= cH)
                                    Game.Cells[row, i] = cH;
                            }
                            catch
                            { }
                            try
                            {
                                /*if (Game.Cells[row, i - cH - 1] >= 0 && Game.Cells[row, i - cH - 1] <= cH)
                                    Game.Cells[row, i - cH - 1] = cH;*/
                                if (Game.Cells[row, i - cH - 1] >= 0)
                                    try
                                    {
                                        if (Game.Cells[row, i - cH - 2] == (int)Game.Side)
                                        {
                                            Game.Cells[row, i - cH - 1] += cH;
                                        }
                                        else
                                        {
                                            if (Game.Cells[row, i - cH - 1] <= cH)
                                                Game.Cells[row, i - cH - 1] = cH;
                                        }
                                    }
                                    catch
                                    {
                                        if (Game.Cells[row, i - cH - 1] <= cH)
                                            Game.Cells[row, i - cH - 1] = cH;
                                    }
                            }
                            catch { }
                            fH = false;
                        }
                        cH = 0;
                    }
                }
                catch
                {
                    if (cH != 0 && fH)
                    {
                        try
                        {
                            if (Game.Cells[row, i - cH - 1] >= 0)
                                try
                                {
                                    if (Game.Cells[row, i - cH - 2] == (int)Game.Side)
                                    {
                                        Game.Cells[row, i - cH - 1] += cH;
                                    }
                                    else
                                    {
                                        if (Game.Cells[row, i - cH - 1] <= cH)
                                            Game.Cells[row, i - cH - 1] = cH;
                                    }
                                }
                                catch
                                {
                                    if (Game.Cells[row, i - cH - 1] <= cH)
                                        Game.Cells[row, i - cH - 1] = cH;
                                }
                        }
                        catch { }
                        fH = false;
                    }
                }
                try
                {
                    if (Game.Cells[rowS + i, colS + i] == (int)Game.Side)
                    {
                        fS = true;
                        cS++;
                    }
                    else
                    {
                        if (fS)
                        {
                            try
                            {
                                if (Game.Cells[rowS + i, colS + i] >= 0 && Game.Cells[rowS + i, colS + i] <= cS)
                                    Game.Cells[rowS + i, colS + i] = cS;
                            }
                            catch
                            { }
                            try
                            {
                                /*if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1]
                                    >= 0 && Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] <= cS)
                                    Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] = cS;*/
                                if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] >= 0)
                                    try
                                    {
                                        if (Game.Cells[rowS + i - cS - 2, colS + i - cS - 2] == (int)Game.Side)
                                        {
                                            Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] += cS;
                                        }
                                        else
                                        {
                                            if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] <= cS)
                                                Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] = cS;
                                        }
                                    }
                                    catch
                                    {
                                        if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] <= cS)
                                            Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] = cS;
                                    }
                            }
                            catch { }
                            fS = false;
                        }
                        cS = 0;
                    }
                }
                catch
                {
                    if (cS != 0 && fS)
                    {
                        try
                        {
                            if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] >= 0)
                                try
                                {
                                    if (Game.Cells[rowS + i - cS - 2, colS + i - cS - 2] == (int)Game.Side)
                                    {
                                        Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] += cS;
                                    }
                                    else
                                    {
                                        if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] <= cS)
                                            Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] = cS;
                                    }
                                }
                                catch
                                {
                                    if (Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] <= cS)
                                        Game.Cells[rowS + i - cS - 1, colS + i - cS - 1] = cS;
                                }
                        }
                        catch { }
                        fS = false;
                    }
                }
                try
                {
                    if (Game.Cells[rowN - i, colN + i] == (int)Game.Side)
                    {
                        fN = true;
                        cN++;
                    }
                    else
                    {
                        if (fN)
                        {
                            try
                            {
                                if (Game.Cells[rowN - i, colN + i] >= 0 && Game.Cells[rowN - i, colN + i] <= cN)
                                    Game.Cells[rowN - i, colN + i] = cN;
                            }
                            catch
                            { }
                            try
                            {
                                /*if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1]
                                    >= 0 && Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] <= cN)
                                    Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] = cN;*/
                                if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] >= 0)
                                    try
                                    {
                                        if (Game.Cells[rowN - i + cN + 2, colN + i - cN - 2] == (int)Game.Side)
                                        {
                                            Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] += cN;
                                        }
                                        else
                                        {
                                            if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] <= cN)
                                                Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] = cN;
                                        }
                                    }
                                    catch
                                    {
                                        if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] <= cN)
                                            Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] = cN;
                                    }
                            }
                            catch { }
                            fN = false;
                        }
                        cN = 0;
                    }
                }
                catch
                {
                    if (cN != 0 && fN)
                    {
                        try
                        {
                            if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] >= 0)
                                try
                                {
                                    if (Game.Cells[rowN - i + cN + 2, colN + i - cN - 2] == (int)Game.Side)
                                    {
                                        Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] += cN;
                                    }
                                    else
                                    {
                                        if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] <= cN)
                                            Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] = cN;
                                    }
                                }
                                catch
                                {
                                    if (Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] <= cN)
                                        Game.Cells[rowN - i + cN + 1, colN + i - cN - 1] = cN;
                                }
                        }
                        catch { }
                        fN = false;
                    }
                }
            }
        }
        /// <summary>
        /// Ход ИИ.
        /// </summary>
        /// <returns></returns>
        public int[] AI()
        {
            List<int[]> list = new List<int[]>();
            int max = 1;
            for(int i = 0; i < Settings.Size; i++)
                for(int j = 0; j < Settings.Size; j++)
                {
                    if (Game.Cells[i, j] == max)
                    {
                        int[] tmp = new int[2];
                        tmp[0] = i;
                        tmp[1] = j;
                        list.Add(tmp);
                    }
                    if (Game.Cells[i,j] > max)
                    {
                        max = Game.Cells[i, j];
                        list.Clear();
                        int[] tmp = new int[2];
                        tmp[0] = i;
                        tmp[1] = j;
                        list.Add(tmp);
                    }
                }
            if (list.Count == 0)
                return null;
            Random rnd = new Random();
            int index = rnd.Next(0, list.Count);
            return list[index];
        }
        /// <summary>
        /// Определяет ничью.
        /// </summary>
        /// <returns></returns>
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
