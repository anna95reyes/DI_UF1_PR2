using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition.Interactions;
using Windows.UI.Xaml.Media.Imaging;

namespace MapEditor.Model
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //INotifyPropertyChanged

        private const int NAME_MINIM = 4;
        private const int DESC_MINIM = 10;
        public Item()
        {
            Name = "";
            Desc = "";
            ImageSource = new BitmapImage();
        }

        private string mName;

        public string Name
        {
            get { return mName; }
            set {
                mName = value;
            }
        }

        public static bool ValidaName(String name)
        {
            if (name.Trim().Length < NAME_MINIM || name==null) return false;
            return true;
        }

        private string mDesc;

        public string Desc
        {
            get { return mDesc; }
            set {
                mDesc = value;
            }
        }

        public static bool ValidaDesc(String desc)
        {
            if (desc.Trim().Length < DESC_MINIM || desc == null) return false;
            return true;
        }

        private BitmapImage mImageSource;

        public BitmapImage ImageSource
        {
            get { return mImageSource; }
            set {                
                mImageSource = value;
            }
        }

        private static ObservableCollection<Item> _items;

        public static ObservableCollection<Item> getItems()
        {
            
            if(_items==null)
            {
                _items = new ObservableCollection<Item>();
                _items.Add(new Item() { Name = "Saphire", Desc = "Ancient Saphire of the dark", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/medal.png")) });
                _items.Add(new Item() { Name = "Poison delerum", Desc = "Obscure poison for assassins.", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/potion.png")) });
                _items.Add(new Item() { Name = "Decorative cup", Desc = "For drinking after the battle.", ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/skull.png")) });
            }

            return _items;
        }
    }
}
