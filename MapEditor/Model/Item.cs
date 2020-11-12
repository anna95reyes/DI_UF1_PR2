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
    public class Item
    {


        private string mName;

        public string Name
        {
            get { return mName; }
            set {
                //if (!ValidaName(mName)) throw new Exception("Nom incorrecte.");
                mName = value;
            }
        }

        public bool ValidaName(String name)
        {
            return name.Trim().Length < 4;
        }

        private string mDesc;

        public string Desc
        {
            get { return mDesc; }
            set {
                mDesc = value;
            }
        }

        public bool ValidaDesc(String desc)
        {
            return desc.Trim().Length < 10;
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

        public Item() { }

        public Item(string name, string desc)
        {
            Name = name;
            Desc = desc;
            ImageSource = new BitmapImage();
        }

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

        public static Item getItem(int index)
        {
            return _items[index];
        }

    }
}
