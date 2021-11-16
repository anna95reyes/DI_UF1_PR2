using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcasone.Model
{
    public class Extensio
    {
        private static ObservableCollection<Extensio> _extensions;

        private String ext;
        private ObservableCollection<Fitxa> fitxes;

        public Extensio(string ext)
        {
            Ext = ext;
            fitxes = new ObservableCollection<Fitxa>();
        }

        public string Ext { get => ext; set => ext = value; }

        public Boolean addFitxa(Fitxa novaFitxa)
        {
            if (fitxes.Contains(novaFitxa))
            {
                return false;
            }
            fitxes.Add(novaFitxa);
            return true;
        }

        public Boolean removeFitxa(Fitxa fitxa)
        {
            if (!fitxes.Contains(fitxa))
            {
                return false;
            }
            fitxes.Remove(fitxa);
            return true;
        }

        public static ObservableCollection<Extensio> getFitxes()
        {
            if (_extensions == null)
            {
                _extensions = new ObservableCollection<Extensio>();
                
                Extensio basic = new Extensio("Basic");
                basic.addFitxa(Fitxa.getFitxes()[0]);
                basic.addFitxa(Fitxa.getFitxes()[1]);

                Extensio theDarkAges = new Extensio("The dark ages");
                theDarkAges.addFitxa(Fitxa.getFitxes()[2]);
                theDarkAges.addFitxa(Fitxa.getFitxes()[3]);

                _extensions.Add(basic);
                _extensions.Add(theDarkAges);
            }
            return _extensions;
        }
    }
}
