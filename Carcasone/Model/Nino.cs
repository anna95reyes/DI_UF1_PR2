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

        public Nino(PosType pos, int player)
        {
            Pos = pos;
            Player = player;
            ImagePath = imatgeNino(Player);
        }

        public Nino(int player)
        {
            Player = player;
            ImagePath = imatgeNino(Player);
        }

        public PosType Pos { get => pos; set => pos = value; }
        public int Player
        {
            get
            {
                return player;
            }
            set
            {
                if (!validaPlayer(value)) throw new Exception("Player incorrecte");
                player = value;
            }
        }
    
        public string ImagePath { get => imagePath; set => imagePath = value; }

        private String imatgeNino(int player)
        {
            String uri = "";
            if (player == 1)
            {
                uri = "ms-appx:///Assets/meeple/blue_meeple.png";
            }
            else if (player == 2)
            {
                uri = "ms-appx:///Assets/meeple/red_meeple.png";
            }
            else if (player == 3)
            {
                uri = "ms-appx:///Assets/meeple/green_meeple.png";
            }
            else if (player == 4)
            {
                uri = "ms-appx:///Assets/meeple/yellow_meeple.png";
            }
            return uri;
        }

        #region validacions
        public static bool validaPlayer(int player)
        {
            return player >= 1 && player <=4;
        }
        #endregion validacions
    }
}
