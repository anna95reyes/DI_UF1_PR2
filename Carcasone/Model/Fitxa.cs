using Carcasone.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;

namespace Carcasone.Model
{
    public class Fitxa : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static Fitxa isStartingTile;
        private static ObservableCollection<Fitxa> _fitxes;
        private static int autonumeric;
        private static Random rnd = new Random();

        private int codi;
        private string title;
        private string imagePath;
        private int repeticions;
        private string notes;
        private SideType[] sides;
        private Boolean isMonastery;
        private String extens;

        public Fitxa(string title, string imagePath, int repeticions, string notes, SideType[] sides, bool isMonastery, String extens)
        {
            Codi = autonumeric++;
            Title = title;
            ImagePath = imagePath;
            Repeticions = repeticions;
            Notes = notes;
            Sides = sides;
            IsMonastery = isMonastery;
            Extens = extens;
        }

        

        public static Fitxa IsStartingTile
        {
            get
            {
                return isStartingTile;
            }
            set
            {
                isStartingTile = value;
            }
        }
        private int Codi {
            get
            {
                return codi;
            }
            set
            {
                if (!validaCodi(value)) throw new Exception("Codi incorrecte");
                codi = value;
            }
        }
        public string Title { 
            get
            {
                return title;
            }
            set
            {
                if (!validaTitle(value)) throw new Exception("El Title es obligatori");
                title = value;
            }  
        }
        public string ImagePath {
            get { 
                return imagePath; 
            }
            set
            {
                if (!validaImagePath(value)) throw new Exception("La fitxa ha de tenir una imatge");
                imagePath = value;
            }
        }
        public int Repeticions { 
            get
            {
                return repeticions;
            }
            set
            {
                if (!validaRepeticions(value)) throw new Exception("El numero de repeticions es obligatori");
                repeticions = value;
            }  
        }
        public string Notes { 
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }  
        }
        public SideType[] Sides
        {
            get
            {
                return sides;
            }
            set
            {
                if (value != null && !validaSides(value)) throw new Exception("ha d'haver-hi 4 bores");
                sides = value;
            }
        }
        public bool IsMonastery
        {
            get
            {
                return isMonastery;
            }
            set
            {
                isMonastery = value;
            }
        }
        public String Extens {
            get
            {
                return extens;
            }
            set 
            {
                if (value != null && !validaExtensio(value)) throw new Exception("la extensio es obligatoria");
                extens = value;
            }
        }

        public static ObservableCollection<Fitxa> getFitxes()
        {
            if (_fitxes == null)
            {
                _fitxes = new ObservableCollection<Fitxa>();

                SideType[] s1 = new SideType[4];
                s1[(int)PosType.TOP] = SideType.CASTLE;
                s1[(int)PosType.RIGHT] = SideType.FIELD;
                s1[(int)PosType.BOTTOM] = SideType.PATH;
                s1[(int)PosType.LEFT] = SideType.FIELD;
                Fitxa f1 = new Fitxa("Castle Wall Entry", "ms-appx:///Assets/tiles/CastleWallEntry0.png", 4, "", s1, false, CardEditorPage.getExtensions()[0]);

                SideType[] s2 = new SideType[4];
                s2[(int)PosType.TOP] = SideType.FIELD;
                s2[(int)PosType.RIGHT] = SideType.FIELD;
                s2[(int)PosType.BOTTOM] = SideType.PATH;
                s2[(int)PosType.LEFT] = SideType.FIELD;
                Fitxa f2 = new Fitxa("Monastery Road", "ms-appx:///Assets/tiles/MonasteryRoad0.png", 2, "", s2, true, CardEditorPage.getExtensions()[0]);

                SideType[] s3 = new SideType[4];
                s3[(int)PosType.TOP] = SideType.FIELD;
                s3[(int)PosType.RIGHT] = SideType.FIELD;
                s3[(int)PosType.BOTTOM] = SideType.FIELD;
                s3[(int)PosType.LEFT] = SideType.FIELD;
                Fitxa f3 = new Fitxa("Monastery", "ms-appx:///Assets/tiles/Monastery0.png", 3, "", s3, true, CardEditorPage.getExtensions()[1]);

                SideType[] s4 = new SideType[4];
                s4[(int)PosType.TOP] = SideType.PATH;
                s4[(int)PosType.RIGHT] = SideType.FIELD;
                s4[(int)PosType.BOTTOM] = SideType.PATH;
                s4[(int)PosType.LEFT] = SideType.FIELD;
                Fitxa f4 = new Fitxa("Road", "ms-appx:///Assets/tiles/Road0.png", 1, "", s4, false, CardEditorPage.getExtensions()[1]);

                _fitxes.Add(f1);
                _fitxes.Add(f2);
                _fitxes.Add(f3);
                _fitxes.Add(f4);

                isStartingTile = getFitxes()[generarRandom(0, getFitxes().Count - 1)];
                
            }
            return _fitxes;
        }

        public static int generarRandom(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        public static bool addFitxa(Fitxa novaFitxa)
        {
            if (_fitxes.Contains(novaFitxa)) {
                return false; 
            }
            _fitxes.Add(novaFitxa);
            return true;
        }

        public static bool removeFitxa(Fitxa fitxa)
        {
            if (!_fitxes.Contains(fitxa))
            {
                return false;
            }
            _fitxes.Remove(fitxa);
            return true;
        }

        public int getCodi()
        {
            return Codi;
        }

        #region validacions
        public static bool validaCodi(int codi)
        {
            return codi >= 0;
        }

        public static bool validaTitle(String title)
        {
            return title.Length > 0;
        }

        public static bool validaImagePath(String imagePath)
        {
            return imagePath.Length > 0;
        }

        public static bool validaRepeticions(int repeticions)
        {
            return repeticions > 0;
        }


        public static bool validaSides(SideType[] sides)
        {
            return sides.Length == 4;
        }

        public static bool validaExtensio(String extensio)
        {
            return extensio.Length > 0;
        }
        #endregion validacions

        public override bool Equals(object obj)
        {
            return obj is Fitxa fitxa &&
                   Codi == fitxa.Codi;
        }

        public override int GetHashCode()
        {
            return -1172468304 + Codi.GetHashCode();
        }

        

    }
}
