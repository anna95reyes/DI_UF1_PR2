﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcasone.Model
{
    public class FitxaMapa
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static ObservableCollection<FitxaMapa> _fitxesMapa;

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
        private PosFitxaMapaType[] posOcupada;

        /// <summary>
        /// Si la fitxa conté un nino llavors
        /// elNino != null
        /// </summary>

        private Nino elNino;

        public FitxaMapa(Fitxa fitxa, Point posMapa, int rotacio, SideType[] sides, PosFitxaMapaType[] posOcupada)
        {
            Fitxa = fitxa;
            PosMapa = posMapa;
            Rotacio = rotacio;
            Sides = sides;
            PosOcupada = posOcupada;
        }

        public FitxaMapa(Fitxa fitxa, Point posMapa, int rotacio, SideType[] sides, PosFitxaMapaType[] posOcupada, Nino elNino)
        {
            Fitxa = fitxa;
            PosMapa = posMapa;
            Rotacio = rotacio;
            Sides = sides;
            PosOcupada = posOcupada;
            ElNino = elNino;
        }

        public FitxaMapa(FitxaMapa f)
        {
            Fitxa = f.Fitxa;
            PosMapa = f.PosMapa;
            Rotacio = f.Rotacio;
            Sides = f.Sides;
            PosOcupada = f.PosOcupada;
            ElNino = f.ElNino;
        }


        public Fitxa Fitxa { get => fitxa; set => fitxa = value; }
        public Point PosMapa { get => posMapa; set => posMapa = value; }
        public int Rotacio
        {
            get
            {
                return rotacio;
            }
            set
            {
                rotacio = value;
            }
        }
        public SideType[] Sides { get => sides; set => sides = value; }
        public Nino ElNino { get => elNino; set => elNino = value; }
        public PosFitxaMapaType[] PosOcupada { get => posOcupada; set => posOcupada = value; }

        public static ObservableCollection<FitxaMapa> getFitxesMapa()
        {
            if (_fitxesMapa == null)
            {
                _fitxesMapa = new ObservableCollection<FitxaMapa>();

                for (int i = 0; i < Fitxa.getFitxes().Count; i++)
                {
                    for (int j = 0; j < Fitxa.getFitxes()[i].Repeticions; j++)
                    {
                        FitxaMapa fm = new FitxaMapa(Fitxa.getFitxes()[i], new Point(5, 5), 0, Fitxa.getFitxes()[i].Sides, new PosFitxaMapaType[]{ PosFitxaMapaType.POS_LLIURE, PosFitxaMapaType.POS_LLIURE, PosFitxaMapaType.POS_LLIURE, PosFitxaMapaType.POS_LLIURE });
                        _fitxesMapa.Add(fm);
                    }
                    
                }
            }
            return _fitxesMapa;
        }

        public static FitxaMapa fitxaMapaStarting()
        {
            int i = 0;
            FitxaMapa fm = null;

            while (i < getFitxesMapa().Count && !getFitxesMapa()[i].Fitxa.Equals(Fitxa.StartingTile))
            {
                i++;
            }

            if (getFitxesMapa()[i].Fitxa.Equals(Fitxa.StartingTile))
            {
                fm = _fitxesMapa[i];
            }

            return fm;
        }
    }
}
