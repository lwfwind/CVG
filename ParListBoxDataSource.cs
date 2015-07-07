using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using VariationDataSource;

namespace ParListBoxDataSource
{
    public class Parameters : ObservableCollection<Parameter>
    {

        public Parameters(Variations Variations)
        {
            //Variations Variations = new Variations();
            foreach (Variation v in Variations)
            {
                string[] values = v.Value.Replace("，", ",").Split(',');

                Parameter p = new Parameter(v.Parameter.Trim());

                foreach (string value in values)
                {
                    p.Values.Add(value.Trim());
                }
                this.Add(p);
            }

            //parameter A = new parameter("A");
            //A.Values.Add("1");
            //parameter B = new parameter("B");
            //B.Values.Add("2");

            //this.Add(A);
            //this.Add(B);
        

        }

   
    }
  

    public class Parameter
    {
        private ObservableCollection<string> values;

        public ObservableCollection<string> Values
        {
            get { return values; }
            set { values = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Parameter(string name)
        {
            this.name = name;
            values = new ObservableCollection<string>();
        }
    }
}
