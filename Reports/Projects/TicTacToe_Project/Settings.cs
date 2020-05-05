using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe_Project
{
    /// <summary>
    /// Структура настроек игры
    /// </summary>
    public struct Settings
    {
        private static int size = 30;
        private static int win = 5;
        /// <summary>
        /// Размер поля Size на Size ячеек.
        /// Принимает значения от 3 до 30.
        /// По умолчанию равен 3.
        /// </summary>
        public static int Size
        {
            get
            {
                return size;
            }
            set
            {
                if (value < 3 || value > 30)
                {
                    MessageBox.Show("Размер поля от 3 до 30!");
                }
                else
                    size = value;
            }
        }
        /// <summary>
        /// Количество ячеек для победы.
        /// Принимает значения от 3 до 5.
        /// По умолчанию равен 3.
        /// </summary>
        public static int Win
        {
            get
            {
                return win;
            }
            set
            {
                if (value < 3 || value > 5)
                {
                    MessageBox.Show("Количество ячеек для победы от 3 до 5!");
                }
                else
                    win = value;
            }
        }
        /// <summary>
        /// Цвет крестиков.
        /// По умолчанию цвет Синий.
        /// </summary>
        public static Color XColor { get; set; } = Color.Blue;
        /// <summary>
        /// Цвет ноликов.
        /// По умолчанию цвет Красный.
        /// </summary>
        public static Color OColor { get; set; } = Color.Red;
    }

}

