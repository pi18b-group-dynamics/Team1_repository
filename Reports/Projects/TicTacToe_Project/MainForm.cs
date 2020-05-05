using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe_Project
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
            //Панель с меню
            Panel menu = new Panel()
            {
                Dock = DockStyle.Fill
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
    }
}