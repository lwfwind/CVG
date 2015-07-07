using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace VariationGeneration
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }
        private DataTable queryresult;
        public DataTable QueryResult
        {
            get { return queryresult; }
            set
            {
                if (queryresult != value)
                {
                    queryresult = value;
                    OnPropertyChanged("QueryResult");
                }
            }
        }
    }
}
