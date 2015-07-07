using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TwayComboboxDataSource
{
    public class TwayComboboxdatas : ObservableCollection<Comboxboxdata>
    {
        public TwayComboboxdatas()
        { }
    }

    public class Comboxboxdata
    {
        private int number;
        public int Number
        { 
            get {return number;}
            set { number = value; }
        }
    }
}
