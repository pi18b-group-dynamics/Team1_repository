using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// Структура игры.
    /// </summary>
    public struct Game
    {
        /// <summary>
        /// Матрица весов.
        /// </summary>
        public static int[,] Cells { get; set; }
        /// <summary>
        /// Текущий ход.
        /// </summary>
        public static Side Side { get; set; }
        /// <summary>
        /// Режим игры.
        /// </summary>
        public static GameMode GameMode { get; set; } = GameMode.PvP;
        /// <summary>
        /// Заполненность всех ячеек.
        /// </summary>
        public static bool FullCells
        {
            get
            {
                foreach(int t in Cells)
                {
                    if (t >= 0)
                        return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Рестарт игры.
        /// </summary>
        public static void Restart()
        {
            Cells = new int[Settings.Size, Settings.Size];
            Side = Side.X;
        }
    }
}
