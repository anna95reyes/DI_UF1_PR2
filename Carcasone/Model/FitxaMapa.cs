using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcasone.Model
{
    public class FitxaMapa
    {
        private Fitxa fitxa;
        private Point posMapa;


        /// <summary>
        /// 4 valors possibles:
        /// 0: sense rotar
        /// 1: 90 graus
        /// 2: 180 graus
        /// 3: 270 graus
        /// </summary>
        private int rotacio = 0;
        private SideType[] sides;

        /// <summary>
        /// Si la fitxa conté un nuno llabors
        /// elNino != null
        /// </summary>

        private Nino elNino;

    }
}
