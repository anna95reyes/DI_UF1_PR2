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
        private ObservableCollection<String> extensions;

        public Fitxa(int codi, string title, string imagePath, int repeticions, string notes, SideType[] sides, bool isMonastery, ObservableCollection<string> extensions)
        {
            Codi = codi;
            Title = title;
            ImagePath = imagePath;
            Repeticions = repeticions;
            Notes = notes;
            Sides = sides;
            IsMonastery = isMonastery;
            Extensions = new ObservableCollection<string>();
        }

        public static bool IsStartingTile { get => isStartingTile; set => isStartingTile = value; }
        public int Codi { get => codi; set => codi = value; }
        public string Title { get => title; set => title = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
        public int Repeticions { get => repeticions; set => repeticions = value; }
        public string Notes { get => notes; set => notes = value; }
        public SideType[] Sides { get => sides; set => sides = value; }
        public bool IsMonastery { get => isMonastery; set => isMonastery = value; }
        public ObservableCollection<string> Extensions { get => extensions; set => extensions = value; }

        public static ObservableCollection<Fitxa> getFitxes()
        {
            if (_fitxes == null)
            {
                _fitxes = new ObservableCollection<Fitxa>();

                SideType[] s1 = { SideType.CASTLE };
                ObservableCollection<string> e1 = new ObservableCollection<string>();
                e1.Add("Basic");
                Fitxa f1 = new Fitxa(1, "Castle Center", "ms-appx:///Assets/tiles/CastleCenter0.png", 4, "", s1, false, e1);

                _fitxes.Add(f1);
            }
            return _fitxes;
        }
    }
}
