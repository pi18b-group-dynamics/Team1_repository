using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// Режим игры. PvE или PvP.
    /// </summary>
    public enum GameMode
    {
        /// <summary>
        /// Против ИИ.
        /// </summary>
        PvE,
        /// <summary>
        /// Против человека.
        /// </summary>
        PvP
    }
}
