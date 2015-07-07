using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Test.VariationGeneration;
using Microsoft.Test.VariationGeneration.Constraints;
using VariationDataSource;
using ParListBoxDataSource;
using TwayComboboxDataSource;
using Microsoft.Win32;
using Excel;
using System.Data;
using System.ComponentModel;
using VariationGeneration.DataTable_To_Excel;
using VariationGeneration.DataTable_To_Html;
using System.Windows.Threading;
using System.Windows.Media.Effects;
using CustomWindow;

namespace VariationGeneration 
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : EssentialWindow
    {
        Variations Variations = new Variations();
        TwayComboboxdatas Comboboxdatas = new TwayComboboxdatas();
        ViewModel viewmodel = new ViewModel();

        ////###########################################################################################
        ////Particles background
        ////###########################################################################################
        //List<Particle> particles = new List<Particle>();
        //List<Particle> deadList = new List<Particle>();
        //Random random = new Random();
        ////###########################################################################################   
        ////###########################################################################################

        public Window1()
        {
            InitializeComponent();

            ////###########################################################################################
            //timer.Interval = TimeSpan.FromMilliseconds(10);
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
            ////###########################################################################################

            this.textBox1.Focus();
           

            //##################################################################################
            //Initialize Listview2 in Result tab
            //##################################################################################
            var LV2_binding = new Binding("QueryResult") { Source = viewmodel };
            this.listView2.SetBinding(ListView.ItemsSourceProperty, LV2_binding);
            viewmodel.PropertyChanged += new PropertyChangedEventHandler(viewmodel_PropertyChanged);


            //##################################################################################
            //Initialize Listview1 in Parameters tab
            //##################################################################################
            var binding = new Binding { Source = Variations };
            this.listView1.SetBinding(ListView.ItemsSourceProperty, binding);

            GridView _view = new GridView();
            listView1.View = _view;

            GridViewColumn gridviewcol_par = new GridViewColumn();
            gridviewcol_par.Header = "Parameter";
            gridviewcol_par.DisplayMemberBinding = new Binding("Parameter");
            gridviewcol_par.Width = 88;
            _view.Columns.Add(gridviewcol_par);

            GridViewColumn gridviewcol_val = new GridViewColumn();
            gridviewcol_val.Header = "Values";
            gridviewcol_val.DisplayMemberBinding = new Binding("Value");
            gridviewcol_val.Width = 425;
            _view.Columns.Add(gridviewcol_val);

            //##################################################################################
            //Inatialize the Parameter and Values list in Constrains tab 
            //##################################################################################
            Parameters Ps = new Parameters(Variations);
            var binding2 = new Binding { Source = Ps };
            this.Par_ListBox.SetBinding(ListBox.ItemsSourceProperty, binding2);
            this.Par_ListBox.DisplayMemberPath = "Name";
            this.Par_ListBox.SelectedValuePath = "Name";
            this.Par_ListBox.SelectedIndex = 0;

            this.Values_listBox.SetBinding(ListBox.ItemsSourceProperty, new Binding("SelectedItem.Values") { Source = this.Par_ListBox });
            this.Values_listBox.SelectedIndex = 0;
            //this.Par_ListBox.SelectionChanged += (object _sender, SelectionChangedEventArgs _e) => { this.DataContext = this.Par_ListBox.SelectedItem; };
            //this.Values_listBox.SelectionChanged += (object _sender, SelectionChangedEventArgs _e) => { this.DataContext = this.Values_listBox.SelectedItem; };

            //##################################################################################
            //Initialize expression Combobox in constrains tab
            //##################################################################################
            this.expression_combobox.Items.Add("is");
            this.expression_combobox.Items.Add("is not");
            this.expression_combobox.SelectedIndex = 0;

            //##################################################################################
            //Initialize T-Way Combobox
            //##################################################################################
            var binding4 = new Binding { Source = Comboboxdatas };
            this.ComboBox1.SetBinding(ComboBox.ItemsSourceProperty, binding4);
            this.ComboBox1.DisplayMemberPath = "Number";
            this.ComboBox1.SelectedValuePath = "Number";

            //Add_Para_btn.IsEnabledChanged += new DependencyPropertyChangedEventHandler(Add_Para_btn_IsEnabledChanged);

            textBox1.TextChanged += new TextChangedEventHandler(textBox1_TextChanged);
            textBox2.TextChanged += new TextChangedEventHandler(textBox2_TextChanged);
            Iftextbox.TextChanged +=new TextChangedEventHandler(Iftextbox_TextChanged);

            listView1.AddHandler(ListViewItem.SelectedEvent, new RoutedEventHandler(listitemselected), true);
         
            listBox3.AddHandler(ListBoxItem.SelectedEvent, new RoutedEventHandler(listboxitemselected), true);
        }

        //#####################################################################################################
        //Create custom window
        //#####################################################################################################
        protected override Decorator GetWindowButtonsPlaceholder()
        {
            return WindowButtonsPlaceholder;
        }

        private void Header_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width + e.HorizontalChange > 10)
                this.Width += e.HorizontalChange;
            if (this.Height + e.VerticalChange > 10)
                this.Height += e.VerticalChange;

        }
        //#####################################################################################################   
        //#####################################################################################################

        void viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GridView view = GenerateGridView(viewmodel.QueryResult);
            this.listView2.View = view;
        }

        private GridView GenerateGridView(DataTable table)
        {
            GridView view = new GridView();
            foreach (DataColumn column in table.Columns)
            {
                view.Columns.Add(new GridViewColumn()
                {
                    Header = column.ColumnName,
                    DisplayMemberBinding = new Binding(column.ColumnName)
                    
                });
            }

            return view;
        }


        void listitemselected(object sender, RoutedEventArgs e)
        {
            ListViewItem lvi = e.OriginalSource as ListViewItem;
            if (lvi != null)
            {
                this.button6.IsEnabled = true;
            }

        }

        void listboxitemselected(object sender, RoutedEventArgs e)
        {
            ListBoxItem lbi = e.OriginalSource as ListBoxItem;
            if (lbi != null)
            {
                this.button5.IsEnabled = true;
            }
        }


        void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetBtnState();

        }
        void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetBtnState();
        }

        void SetBtnState()
        {
            Add_Para_btn.IsEnabled = (textBox1.Text.Trim() != string.Empty && textBox2.Text.Trim() != string.Empty);
        }

        void Iftextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.If_And_button.IsEnabled = this.Iftextbox.Text != string.Empty;
            this.If_Or_button.IsEnabled = this.Iftextbox.Text != string.Empty;
            if (this.Iftextbox.Text.ToString().Trim().EndsWith("And") || this.Iftextbox.Text.ToString().Trim().EndsWith("Or"))
            {
                this.Iftextbox.Background = Brushes.Pink;
            }
            else
            {
                this.Iftextbox.Background = Brushes.White;
            }
        }

        private void Thentextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.button4.IsEnabled = Thentextbox.Text != string.Empty && !(this.Thentextbox.Text.ToString().Trim().EndsWith("And")) && !(this.Thentextbox.Text.ToString().Trim().EndsWith("Or")) && !(this.Iftextbox.Text.ToString().Trim().EndsWith("And")) && !(this.Iftextbox.Text.ToString().Trim().EndsWith("Or"));
            this.Then_And_button.IsEnabled = this.Thentextbox.Text != string.Empty;
            this.Then_Or_button.IsEnabled = this.Thentextbox.Text != string.Empty;
            if (this.Thentextbox.Text.ToString().Trim().EndsWith("And") || this.Thentextbox.Text.ToString().Trim().EndsWith("Or"))
            {
                this.Thentextbox.Background = Brushes.Pink;
            }
            else
            {
                this.Thentextbox.Background = Brushes.White;
            }

           

        }
        private void If_And_button_Click(object sender, RoutedEventArgs e)
        {
            this.Iftextbox.Text = this.Iftextbox.Text + " And ";
            this.If_Or_button.IsEnabled = false;
            this.If_And_button.IsEnabled = false;

        }

        private void If_Or_button_Click(object sender, RoutedEventArgs e)
        {
            this.Iftextbox.Text = this.Iftextbox.Text + " Or ";
            this.If_And_button.IsEnabled = false;
            this.If_Or_button.IsEnabled = false;

        }

        private void Then_And_button_Click(object sender, RoutedEventArgs e)
        {
            this.Thentextbox.Text = this.Thentextbox.Text + " And ";
            this.Then_And_button.IsEnabled = false;
            this.Then_Or_button.IsEnabled = false;
        }

        private void Then_Or_button_Click(object sender, RoutedEventArgs e)
        {
            this.Thentextbox.Text = this.Thentextbox.Text + " Or ";
            this.Then_And_button.IsEnabled = false;
            this.Then_Or_button.IsEnabled = false;
        }



        private void Par_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Par_ListBox.HasItems)
            {
                if (this.Iftextbox.Text != string.Empty)
                {
                    if (this.Iftextbox.Text.ToString().Contains("And") || this.Iftextbox.Text.ToString().Contains("Or"))
                    {
                        if (!(this.Iftextbox.Text.ToString().Trim().EndsWith("And")) && !(this.Iftextbox.Text.ToString().Trim().EndsWith("Or")))
                        {
                            string[] iftexts = this.Iftextbox.Text.ToString().Trim().Replace("And", "_").Replace("Or", "_").Split('_');
                            StringBuilder pars = new StringBuilder();
                            foreach (string text in iftexts)
                            {
                                pars.Append(substring(text, "[", "]"));
                                pars.Append(",");
                            }
                            string Pars = pars.ToString();
                            if (Pars.Contains(this.Par_ListBox.SelectedValue.ToString()))
                            {
                                this.button2.IsEnabled = false;
                            }
                            else
                            {
                                this.button2.IsEnabled = true;
                            }
                        }

                    }
                    else
                    { 
                        if(substring(this.Iftextbox.Text.ToString().Trim(), "[", "]").Contains(this.Par_ListBox.SelectedValue.ToString()))
                        {
                            this.button2.IsEnabled = false;
                        }
                        else
                        {
                            this.button2.IsEnabled = true;
                        }
                    }
                   
                    
                }
                if (this.Values_listBox.Items.Count == 1)
                {
                   
                    this.button2.IsEnabled = false;

                }

            }
           
        }

        void updateComboBox()
        {
            Comboboxdatas.Clear();
            for (int i = 0; i < Variations.Count; i++)
            {
                Comboxboxdata cmbdata = new Comboxboxdata() { Number = i + 1 };
                Comboboxdatas.Add(cmbdata);

            }
            this.ComboBox1.ItemsSource = Comboboxdatas;
            if (Variations.Count > 1)
            {
                this.ComboBox1.SelectedValue = "2";
            }
            else
            {
                this.ComboBox1.SelectedValue = "1";
            }
        }

        private void Addbtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(VariationDataSource.Variation v in Variations)
            {
                if (v.Parameter == this.textBox1.Text.Trim())
                {
                    MessageBox.Show("The " + "\"" + v.Parameter + "\"" + " Parameter has been existed in the table");
                    this.textBox1.Clear();
                    this.textBox2.Clear();
                    return;
                }
            }
            string value = this.textBox2.Text.Trim().Replace("，", ",").TrimEnd(',');
            VariationDataSource.Variation V = new VariationDataSource.Variation() { Parameter = this.textBox1.Text.Trim(), Value = value };
            Variations.Add(V);
    
            this.textBox1.Clear();
            this.textBox2.Clear();
            this.textBox1.Focus();

            //Update T-way comboBox
            updateComboBox();
    
            //Update Par_ListBox
            Parameters Ps = new Parameters(Variations);
            Par_ListBox.ItemsSource = Ps;

            //Update constrain tab
            this.listBox3.Items.Clear();
            this.Iftextbox.Clear();
            this.Thentextbox.Clear();


        }
           
          
        private string substring(string str1, string str2, string str3)
        {
            string X = null;
            if (!str2.StartsWith(@"\"))
            {
                X = str1.Substring(str1.IndexOf(str2) + str2.Length - 1 + 1);
            }
            else
            {
                X = str1.Substring(str1.IndexOf(str2) + str2.Length + 1);
            }
         
             string Y = X.Substring(0, X.IndexOf(str3));
             return Y.Trim();

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.Iftextbox.IsEnabled = true;
            if (Iftextbox.Text.ToString().Trim().EndsWith("And") || Iftextbox.Text.ToString().Trim().EndsWith("Or"))
            {
                this.Iftextbox.Text = this.Iftextbox.Text + "[" + this.Par_ListBox.SelectedValue.ToString() + "]" + " " + this.expression_combobox.SelectedValue.ToString().Trim() + " " + "\"" + this.Values_listBox.SelectedValue.ToString() + "\"";
            
            }
            else
            {
                this.Iftextbox.Text = "[" + this.Par_ListBox.SelectedValue.ToString() + "]" + " " + this.expression_combobox.SelectedValue.ToString().Trim() + " " + "\"" + this.Values_listBox.SelectedValue.ToString() + "\"";
            }
            this.button2.IsEnabled = false;

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Thentextbox.IsEnabled = true;
            if (Thentextbox.Text.ToString().Trim().EndsWith("And") || Thentextbox.Text.ToString().Trim().EndsWith("Or"))
            {
                this.Thentextbox.Text = this.Thentextbox.Text + "[" + this.Par_ListBox.SelectedValue.ToString() + "]" + " " + this.expression_combobox.SelectedValue.ToString().Trim() + " " + "\"" + this.Values_listBox.SelectedValue.ToString() + "\"";

            }
            else
            {
                this.Thentextbox.Text = "[" + this.Par_ListBox.SelectedValue.ToString() + "]" + " " + this.expression_combobox.SelectedValue.ToString().Trim() + " " + "\"" + this.Values_listBox.SelectedValue.ToString() + "\"";
            }

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string constraint = "If " + this.Iftextbox.Text + Environment.NewLine + "Then " + this.Thentextbox.Text;
            this.listBox3.Items.Add(constraint);
            Thentextbox.Clear();
            Thentextbox.IsEnabled = false;
            Iftextbox.Clear();
            Iftextbox.IsEnabled = false;
            button2.IsEnabled = false;
           
        }


        private void button5_Click(object sender, RoutedEventArgs e)
        {

            while (listBox3.SelectedItems.Count > 0)
            {
                listBox3.Items.Remove(listBox3.SelectedItems[0]);
            }
            this.button5.IsEnabled = false;

        }

        private void Listview1_removebtn_Click(object sender, RoutedEventArgs e)
        {

            while (listView1.SelectedItems.Count > 0)
            {
                //Update listView1
                VariationDataSource.Variation v = listView1.SelectedItems[0] as VariationDataSource.Variation;
                //Variations Vs = listView1.ItemsSource as Variations;
                Variations.Remove(v);

                //Update Par_ListBox
                Parameters Ps = new Parameters(Variations);
                Par_ListBox.ItemsSource = Ps;

                //Update T-way ComboBox
                updateComboBox();

                //Update constrain tab
                this.listBox3.Items.Clear();
                this.Iftextbox.Clear();
                this.Thentextbox.Clear();

            }
            this.button6.IsEnabled = false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.Constaintstab.IsSelected)
            {
                this.expression_combobox.IsEnabled = this.listView1.HasItems;
                this.button3.IsEnabled = Par_ListBox.HasItems && Values_listBox.HasItems;
                if (!(this.Par_ListBox.HasItems && this.Values_listBox.HasItems))
                {
                    this.Iftextbox.Text = string.Empty;
                    this.Iftextbox.IsEnabled = false;
                    this.Thentextbox.Text = string.Empty;
                    this.Thentextbox.IsEnabled = false;
                }

              
            }
            if (this.Resulttab.IsSelected)
            {
                this.Generatebtn.IsEnabled = this.listView1.HasItems;
                this.Exportbutton.IsEnabled = this.listView2.HasItems;
            }
 
        }


        private void Generatebtn_Click(object sender, RoutedEventArgs e)
        {
            this.listView2.UpdateLayout();
            //##################################################################################
            //Inatialize Result tab
            //##################################################################################

            //Get Parameter
            List<Microsoft.Test.VariationGeneration.Parameter> listP = new List<Microsoft.Test.VariationGeneration.Parameter>();
            foreach (VariationDataSource.Variation v in Variations)
            {
                string[] values = v.Value.Replace("，", ",").Split(',');

                var P = new Microsoft.Test.VariationGeneration.Parameter(v.Parameter.Trim()) { };
                foreach (string value in values)
                {
                    Double isnumber;
                    if (value.Contains("(") && value.Contains(")") && Double.TryParse(substring(value,"(",")"),out isnumber))
                    {
                        P.Add(new ParameterValue(value.Substring(0, value.IndexOf("("))) { Weight = Convert.ToDouble(substring(value,"(",")")) });
                    }
                    else 
                    {
                        P.Add(value.Trim());
                    }
                  
                }
                listP.Add(P);
            }

            //Get Model
            Model model = null;
            int count = this.listBox3.Items.Count;
            if (count == 0)
            {
                model = new Model(listP);
            }
            else
            {
                //Get constraints
                var constraints = new List<Microsoft.Test.VariationGeneration.Constraint> { };
                for (int i = 0; i < count; i++)
                {
                    string constain = this.listBox3.Items[i].ToString();
                    string if_constrain = substring(constain, "If","Then").Trim();
                    string ifparametername = substring(if_constrain, "[", "]");
                    string ifvaluename = substring(if_constrain, "\"", "\"");
                    string if_express = substring(if_constrain, "]", "\"");
                    Microsoft.Test.VariationGeneration.Parameter if_p = listP.Find(p => p.Name.Equals(ifparametername));
                    string then_constrain = constain.Substring(constain.IndexOf("Then"));
                    string thenparametername = substring(then_constrain, "[", "]");
                    string thenvaluename = substring(then_constrain, "\"", "\"");
                    string then_express = substring(then_constrain, "]", "\"");
                    Microsoft.Test.VariationGeneration.Parameter then_p = listP.Find(p => p.Name.Equals(thenparametername));

                    //Judge if Parameters in listview1 contain ifparametername and thenparametername, if not , the constaint is not valid
                    StringBuilder builder = new StringBuilder();
                    foreach (VariationDataSource.Variation v in Variations)
                    {                       
                        builder.Append(v.Parameter + " "); 
                    }
                    if (builder.ToString().Contains(ifparametername) && builder.ToString().Contains(thenparametername))
                    {
                        IfThenConstraint ifthen = new IfThenConstraint(){};
                        //############################################################################
                        //if_constrain
                        //############################################################################
                        if (if_constrain.Contains("And") || if_constrain.Contains("Or"))
                        {
                            string[] if_constrains = if_constrain.Replace("And", "_And").Replace("Or","_Or").Split('_');
                            List<string> if_constrainslist = if_constrains.ToList<string>();

                            ConditionConstraint _if;

                            string ifparname_Fir = substring(if_constrainslist[0], "[", "]");
                            string ifvalname_Fir = substring(if_constrainslist[0], "\"", "\"");
                            string if_exp_Fir = substring(if_constrainslist[0], "]", "\"");
                            Microsoft.Test.VariationGeneration.Parameter if_par_Fir = listP.Find(p => p.Name.Equals(ifparname_Fir));

                            if (if_exp_Fir == "is")
                            {
                                _if = if_par_Fir.Equal(ifvalname_Fir);

                            }
                            else
                            {
                                _if = if_par_Fir.NotEqual(ifvalname_Fir);
                            }
                            if_constrainslist.RemoveAt(0);
                            foreach (string if_constraint in if_constrainslist)
                            { 
                               string ifparname = substring(if_constraint, "[", "]");
                               string ifvalname = substring(if_constraint, "\"", "\"");
                               string if_exp = substring(if_constraint, "]", "\"");
                               Microsoft.Test.VariationGeneration.Parameter if_par = listP.Find(p => p.Name.Equals(ifparname));
                               if(if_constraint.Trim().StartsWith("And"))
                               {
                                  
                                    if (if_exp == "is")
                                    {
                                        _if = _if.And(if_par.Equal(ifvalname));

                                    }
                                    else
                                    {
                                          _if = _if.And(if_par.NotEqual(ifvalname));
                                    }
                                 
                               }
                               if(if_constraint.Trim().StartsWith("Or"))
                               {
                                  
                                    if (if_exp == "is")
                                    {
                                        _if = _if.Or(if_par.Equal(ifvalname));

                                    }
                                    else
                                    {
                                        _if = _if.Or(if_par.NotEqual(ifvalname));
                                    }
                                 
                               }

                            }

                            ifthen.If = _if;
 
                        }
                        else
                        {                 
                            if (if_express == "is")
                            {
                                ifthen.If = if_p.Equal(ifvaluename);

                            }
                            else
                            {
                                ifthen.If = if_p.NotEqual(ifvaluename);
                            }
                    
                        }
                        //############################################################################ 
                        //############################################################################ 
 

                        //############################################################################
                        //Then_constrain    
                        //############################################################################
                        if (then_constrain.Contains("And") || then_constrain.Contains("Or"))
                        {
                            string[] then_constrains = then_constrain.Replace("And", "_And").Replace("Or", "_Or").Split('_');
                            List<string> then_constrainslist = then_constrains.ToList<string>();

                            ConditionConstraint _then;

                            string thenparname_Fir = substring(then_constrainslist[0], "[", "]");
                            string thenvalname_Fir = substring(then_constrainslist[0], "\"", "\"");
                            string then_exp_Fir = substring(then_constrainslist[0], "]", "\"");
                            Microsoft.Test.VariationGeneration.Parameter then_par_Fir = listP.Find(p => p.Name.Equals(thenparname_Fir));

                            if (then_exp_Fir == "is")
                            {
                                _then = then_par_Fir.Equal(thenvalname_Fir);

                            }
                            else
                            {
                                _then = then_par_Fir.NotEqual(thenvalname_Fir);
                            }
                            then_constrainslist.RemoveAt(0);
                            foreach (string then_constraint in then_constrainslist)
                            {
                                string thenparname = substring(then_constraint, "[", "]");
                                string thenvalname = substring(then_constraint, "\"", "\"");
                                string then_exp = substring(then_constraint, "]", "\"");
                                Microsoft.Test.VariationGeneration.Parameter then_par = listP.Find(p => p.Name.Equals(thenparname));
                                if (then_constraint.Trim().StartsWith("And"))
                                {

                                    if (then_exp == "is")
                                    {
                                        _then = _then.And(then_par.Equal(thenvalname));

                                    }
                                    else
                                    {
                                        _then = _then.And(then_par.NotEqual(thenvalname));
                                    }

                                }
                                if (then_constraint.Trim().StartsWith("Or"))
                                {

                                    if (then_exp == "is")
                                    {
                                        _then = _then.Or(then_par.Equal(thenvalname));

                                    }
                                    else
                                    {
                                        _then = _then.Or(then_par.NotEqual(thenvalname));
                                    }
                                }
                            }
                            ifthen.Then = _then;
                        }
                        else
                        {
                            if (then_express == "is")
                            {
                                ifthen.Then = then_p.Equal(thenvaluename);
                            }
                            else
                            {
                                ifthen.Then = then_p.NotEqual(thenvaluename);
                            }
                        }
                        //############################################################################
                        //############################################################################
                        
                        constraints.Add(ifthen);
                    }

                }
                model = new Model(listP, constraints);
            }

          
            System.Collections.ArrayList objects = new System.Collections.ArrayList();
            var variations = model.GenerateVariations(Convert.ToInt32(this.ComboBox1.SelectedValue), 12345);
         
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID"));
            int t = 1;
            foreach (var v in variations)
            {
                //Add columns
                foreach (var value in v)
                {
                   
                    if (!table.Columns.Contains(value.Key))
                    {
                        table.Columns.Add(new DataColumn(value.Key));
                    }
                    
                }
              
                //Add the numbet of ID
                objects.Add(t);
                t++;

                //Add Rows
                foreach (var value in v)
                {                   
                    objects.Add(value.Value);  

                }
                table.Rows.Add(objects.ToArray());
                objects.Clear();

            }

            viewmodel.QueryResult = table;

            this.listView2.UpdateLayout();
            this.Exportbutton.IsEnabled = this.listView2.HasItems;

        }

        private void Exportbutton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialogSaveFile = new SaveFileDialog();
            dialogSaveFile.AddExtension = true;//是否自动添加扩展名
            dialogSaveFile.Filter = "HTML[*.html]|*.html|Excel 2007[*.xlsx]|*.xlsx|Excel 2003[*.xls]|*.xls";
            dialogSaveFile.OverwritePrompt = true;//文件已存在是否提示覆盖
            //dialogSaveFile.FileName = "文件名";//默认文件名
            dialogSaveFile.CheckPathExists = true;//提示输入的文件名无效
            dialogSaveFile.Title = "Save As";
            String appOutputPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Output Sample";
            dialogSaveFile.InitialDirectory = appOutputPath;

            //Show dialog
            bool? b = dialogSaveFile.ShowDialog();

            //Click Save button
            if (b == true)
            {
                string filename = dialogSaveFile.FileName;
                if (dialogSaveFile.FilterIndex == 1)
                {
                    viewmodel.QueryResult.ToHtmlTable(filename);
                }
                else
                {
                    DataTableToExcel.DataTabletoExcel(viewmodel.QueryResult, filename);
                }


            }



        }

        private void Loaddata(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename);
            if (extension == ".txt")
            {
                //#########################################################
                //Read from Text file
                //#########################################################
                StreamReader streamReader = new StreamReader(filename);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string par = line.Replace("；", ";").Substring(0, line.IndexOf(":")).Trim();
                    string val = line.Replace("，", ",").Substring(line.IndexOf(':') + 1).Trim().TrimEnd(",".ToCharArray());
                    var v = new VariationDataSource.Variation() { Parameter = par, Value = val };
                    Variations.Add(v);
                }
            }
            if (extension == ".xls")
            {
                //#########################################################
                //Read from Excel 2003
                //#########################################################
                FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read);

                //Reading from a binary Excel file ('97-2003 format; *.xls)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

                DataTable result = excelReader.AsDataSet().Tables[0];

                int Rows_Count = result.Rows.Count;

                for (int i = 0; i < Rows_Count; i++)
                {
                    var v = new VariationDataSource.Variation() { Parameter = result.Rows[i][0].ToString().Replace("；", ";").Trim(), Value = result.Rows[i][1].ToString().Replace("，", ",").Trim().TrimEnd(",".ToCharArray()) };
                    Variations.Add(v);
                }

                //Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();



            }
            if (extension == ".xlsx")
            {
                //#########################################################
                //Read from Excel 2007
                //#########################################################
                FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read);

                //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);


                DataTable result = excelReader.AsDataSet().Tables[0];

                int Rows_Count = result.Rows.Count;

                for (int i = 0; i < Rows_Count; i++)
                {
                    var v = new VariationDataSource.Variation() { Parameter = result.Rows[i][0].ToString().Replace("；", ";").Trim(), Value = result.Rows[i][1].ToString().Replace("，", ",").Trim().TrimEnd(",".ToCharArray()) };
                    Variations.Add(v);
                }

                //Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();
            }

            //Update ComboBox
            updateComboBox();

            //Update Par_ListBox
            Parameters Ps = new Parameters(Variations);
            Par_ListBox.ItemsSource = Ps;

            //Update constrain tab
            this.listBox3.Items.Clear();
            this.Iftextbox.Clear();
            this.Thentextbox.Clear();

        }

        private void Load_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogOpenFile = new OpenFileDialog();
            //String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Import Sample";
            String appStartPath =  @"Import Sample";
            dialogOpenFile.InitialDirectory = appStartPath;
            dialogOpenFile.Filter = "All supported files[*.txt,*.xls,*.xlsx]|*.txt;*.xls;*.xlsx";
            dialogOpenFile.CheckPathExists = true;
            dialogOpenFile.CheckFileExists = true;
            dialogOpenFile.Title = "Load";

            //Show dialog
            bool? b = dialogOpenFile.ShowDialog();

            //Click Open button
            if (b == true)
            {
                string filename = dialogOpenFile.FileName;
                Loaddata(filename);

            }
           
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F1)
                System.Windows.Forms.Help.ShowHelp(null, "Variation Generator.chm");

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "Variation Generator.chm");
        }


        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //Drag and drop a file to Listview1
        //Make sure Set the AllowDrop property to True on the elements you want to allow dropping.
        //refer to http://code.msdn.microsoft.com/wpfsamples/ 
        //refer to http://www.wpftutorial.net/DragAndDrop.html
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        private void listView1_PreviewDragOver(object sender, DragEventArgs args)
        {
            // As an arbitrary design decision, we only want to deal with a single file.
            if (IsSingleFile(args) != null) args.Effects = DragDropEffects.Copy;
            else args.Effects = DragDropEffects.None;

            // Mark the event as handled, so Listview's native DragOver handler is not called.
            args.Handled = true;

        }

        private void listView1_PreviewDrop(object sender, DragEventArgs args)
        {
            // Mark the event as handled, so Listview's native Drop handler is not called.
            args.Handled = true;

            string filename = IsSingleFile(args);
            if (filename == null) return;
            Loaddata(filename);

        }

        // If the data object in args is a single file, this method will return the filename.
        // Otherwise, it returns null.
        private string IsSingleFile(DragEventArgs args)
        {
            // Check for files in the hovering data object.
            if (args.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] fileNames = args.Data.GetData(DataFormats.FileDrop, true) as string[];
                // Check fo a single file or folder.
                if (fileNames.Length == 1)
                {
                    // Check for a file (a directory will return false).
                    if (File.Exists(fileNames[0]))
                    {
                        // At this point we know there is a single file.
                        return fileNames[0];
                    }
                }
            }
            return null;
        }
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        ////###########################################################################################
        ////Particles background
        ////###########################################################################################
        //void timer_Tick(object sender, EventArgs e)
        //{
        //    UpdateParticules();
        //}

        //DispatcherTimer timer = new DispatcherTimer();

        //double elapsed = 0.1;
        //private void UpdateParticules()
        //{
        //    //更新粒子信息
        //    deadList.Clear();
        //    foreach (Particle p in this.particles)
        //    {
        //        if (p.Position.Y < -p.Size || p.Position.X < -p.Size || p.Position.X > Width + p.Size)
        //        {
        //            deadList.Add(p);
        //        }
        //        else
        //        {
        //            //更新位置
        //            p.Position.X += p.Velocity.X * elapsed;
        //            p.Position.Y += p.Velocity.Y * elapsed;
        //            p.Position.Z += p.Velocity.Z * elapsed;
        //            TranslateTransform t = (p.Ellipse.RenderTransform as TranslateTransform);
        //            t.X = p.Position.X;
        //            t.Y = p.Position.Y;

        //            //更新颜色信息
        //            p.Ellipse.Fill = p.Brush;
        //            p.Ellipse.Effect = p.Blur;
        //        }
        //    }

        //    //创建新的粒子
        //    for (int i = 0; i < 10 && this.particles.Count < 25; i++)
        //    {
        //        //尝试循环使用已有例子
        //        if (deadList.Count - 1 >= i)
        //        {
        //            SpawnParticle(deadList[i].Ellipse);
        //            deadList[i].Ellipse = null;
        //        }
        //        else
        //        {
        //            SpawnParticle(null);
        //        }
        //    }

        //    foreach (Particle p in deadList)
        //    {
        //        if (p.Ellipse != null) ParticleHost.Children.Remove(p.Ellipse);
        //        this.particles.Remove(p);
        //    }
        //}

        //private void SpawnParticle(Ellipse e)
        //{
        //    double x = RandomWithVariance(Width / 2, Width / 2);
        //    double y = Height;
        //    double z = 10 * (random.NextDouble() * 100);
        //    double speed = RandomWithVariance(20, 15);
        //    double size = RandomWithVariance(75, 50);

        //    Particle p = new Particle();
        //    p.Position = new Point3D(x, y, z);
        //    p.Size = size;

        //    //模糊
        //    var blur = new BlurEffect();
        //    blur.RenderingBias = RenderingBias.Performance;
        //    blur.Radius = RandomWithVariance(10, 15);
        //    p.Blur = blur;

        //    var brush = (Brush)Brushes.White.Clone();
        //    brush.Opacity = RandomWithVariance(0.5, 0.5);
        //    p.Brush = brush;

        //    TranslateTransform t;

        //    if (e != null)
        //    {
        //        e.Fill = null;
        //        e.Width = e.Height = size;
        //        p.Ellipse = e;

        //        t = e.RenderTransform as TranslateTransform;
        //    }
        //    else
        //    {
        //        p.Ellipse = new Ellipse();
        //        p.Ellipse.Width = p.Ellipse.Height = size;
        //        this.ParticleHost.Children.Add(p.Ellipse);

        //        t = new TranslateTransform();
        //        p.Ellipse.RenderTransform = t;
        //        p.Ellipse.RenderTransformOrigin = new Point(0.5, 0.5);

        //    }

        //    t.X = p.Position.X;
        //    t.Y = p.Position.Y;

        //    double velocityMultiplier = (random.NextDouble() + 0.25) * speed;
        //    double vX = (1.0 - (random.NextDouble() * 2.0)) * velocityMultiplier;
        //    double vY = -Math.Abs((1.0 - (random.NextDouble() * 2.0)) * velocityMultiplier);

        //    p.Velocity = new Point3D(vX, vY, 0);

        //    this.particles.Add(p);
        //}

        //private double RandomWithVariance(double midvalue, double variance)
        //{
        //    double min = Math.Max(midvalue - (variance / 2), 0);
        //    double max = midvalue + (variance / 2);
        //    double value = min + ((max - min) * random.NextDouble());
        //    return value;
        //}

        ////###########################################################################################   
        ////###########################################################################################

    }


}

