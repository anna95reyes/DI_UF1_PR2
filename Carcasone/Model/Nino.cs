using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcasone.Model
{
    public class Nino
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 0,1,2,3
        /// </summary>
        private PosType pos;
        private int player;
        private string imagePath;

        public Nino(PosType pos, int player, string imagePath)
        {
            Pos = pos;
            Player = player;
            ImagePath = imagePath;
        }

        public PosType Pos { get => pos; set => pos = value; }
        public int Player { get => player; set => player = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }


    }
}
