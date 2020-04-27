using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    class Cell: Panel
    {
        public int Col { get; set; }
        public int Row { get; set; }
        public int Weg { get; set; } = -1;
        public Side Side { get; set; } = Side.None;
    }
}
