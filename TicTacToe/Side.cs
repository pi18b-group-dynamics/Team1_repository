using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// Текущий ход. Крестики или нолики.
    /// </summary>
    public enum Side
    {
        /// <summary>
        /// Крестики
        /// </summary>
        X = -1,
        /// <summary>
        /// Нолики
        /// </summary>
        O = -2,
        /// <summary>
        /// Ничего
        /// </summary>
        None = 0
    }
}
