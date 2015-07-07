using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;


namespace VariationDataSource
{
    public class Variations : ObservableCollection<Variation>
    {
        public Variations()
        {
            //Variation os = new Variation() { Parameter = "OS", Value = "XP, Vista, Win7" };
            //Variation plat = new Variation() { Parameter = "plat", Value = "32, 64" };
            //this.Add(os);
            //this.Add(plat);
        }
           
        }

    public class Variation
    {
        private string parameter;

        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
