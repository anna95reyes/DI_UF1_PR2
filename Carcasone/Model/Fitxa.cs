using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcasone.Model
{
    public class Fitxa
    {

        private static Boolean isStartingTile;
        private static ObservableCollection<Fitxa> _fitxes;

        private int codi;
        private string title;
        private string imagePath;
        private int repeticions;
        private string notes;
        private SideType[] sides;
        private Boolean isMonastery;

        public Fitxa(int codi, string title, string imagePath, int repeticions, string notes, SideType[] sides, bool isMonastery)
        {
            Codi = codi;
            Title = title;
            ImagePath = imagePath;
            Repeticions = repeticions;
            Notes = notes;
            Sides = sides;
            IsMonastery = isMonastery;
        }

        public static bool IsStartingTile { get => isStartingTile; set => isStartingTile = value; }
        public int Codi { get => codi; set => codi = value; }
        public string Title { get => title; set => title = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
        public int Repeticions { get => repeticions; set => repeticions = value; }
        public string Notes { get => notes; set => notes = value; }
        public SideType[] Sides
        {
            get => sides;
            set
            {
                if (value.Length != 4) throw new Exception("hi han 4 bores");
                sides = value;
            }
        }
        public bool IsMonastery { get => isMonastery; set => isMonastery = value; }

        public static ObservableCollection<Fitxa> getFitxes()
        {
            if (_fitxes == null)
            {
                _fitxes = new ObservableCollection<Fitxa>();

                SideType[] s1 = new SideType[4];
                s1[0] = SideType.CASTLE;
                Fitxa f1 = new Fitxa(1, "Castle Center", "ms-appx:///Assets/tiles/CastleCenter0.png", 4, "", s1, false);

                SideType[] s2 = new SideType[4];
                s2[0] = SideType.FIELD;
                s2[1] = SideType.FIELD;
                s2[2] = SideType.FIELD;
                s2[3] = SideType.PATH;
                Fitxa f2 = new Fitxa(2, "Castle Center", "ms-appx:///Assets/tiles/MonasteryRoad0.png", 4, "", s2, true);

                SideType[] s3 = new SideType[4];
                s3[0] = SideType.FIELD;
                s3[1] = SideType.FIELD;
                s3[2] = SideType.FIELD;
                s3[3] = SideType.FIELD;
                Fitxa f3 = new Fitxa(3, "Castle Center", "ms-appx:///Assets/tiles/Monastery0.png", 4, "", s3, true);

                SideType[] s4 = new SideType[4];
                s4[0] = SideType.FIELD;
                s4[1] = SideType.PATH;
                s4[2] = SideType.FIELD;
                s4[3] = SideType.PATH;
                Fitxa f4 = new Fitxa(3, "Castle Center", "ms-appx:///Assets/tiles/Road0.png", 4, "", s4, false);

                _fitxes.Add(f1);
                _fitxes.Add(f2);
                _fitxes.Add(f3);
                _fitxes.Add(f4);
            }
            return _fitxes;
        }
    }
}
