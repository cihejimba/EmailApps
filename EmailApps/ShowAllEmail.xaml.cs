using System;
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
using System.Data;

namespace EmailApps
{
    /// <summary>
    /// Interaction logic for ShowAllEmail.xaml
    /// </summary>
    public partial class ShowAllEmail : Page
    {
        DataTable dt = new DataTable();
        ConnectionClass con = new ConnectionClass();
        public ShowAllEmail()
        {
            InitializeComponent();
            string Query = @"SELECT * from tbl_EmailAddressInfo ";
            dt= con.GetDataTable(Query);
           ddlEmailType.ItemsSource= dt.DefaultView;
          
          
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DataRow[] dtrow = dt.Select("Domain='"+ddlEmailType.SelectedValue.ToString()+"'");
            dataGrid1.ItemsSource = dtrow.CopyToDataTable().DefaultView;
            

        }
      

        private void emailretriveProcess_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.ShowDialog();
        }
    }
}
