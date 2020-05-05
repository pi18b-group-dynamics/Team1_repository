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
                Size = new Size(200, 30),
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
                Size = new Size(600, 600),
                Left = (this.ClientSize.Width - 600) / 2,
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
            Button back = new Button()
            {
                Text = "Назад",
                Width = grid.Width / 2 - 10,
                Height = 50,
                Font = new Font("Arial", 15, FontStyle.Regular),
                Top = grid.Bottom + 25,
                Left = grid.Left,
                FlatStyle = FlatStyle.Flat
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
            Button pve = new Button()
            {
                Text = "PvE",
                Width = this.ClientSize.Width - pvMargin * 2,
                Height = pvHeight,
                Font = new Font("Arial", 45, FontStyle.Regular),
                Location = new Point(pvMargin, pvMargin),
                FlatStyle = FlatStyle.Flat
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
            menu.Controls.Add(pve);
            menu.Controls.Add(pvp);
            menu.Controls.Add(options);
            menu.Controls.Add(help);
            menu.Controls.Add(exit);
            this.Controls.Add(menu);
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
        /// Меняет текст метки с текущим ходом.
        /// </summary>
        /// <param name="label">Метка с текущим ходом</param>
        public void Label(Label label)
        {
            switch (Game.Side)
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
    }
}