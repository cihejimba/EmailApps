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
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Data.SqlClient;

namespace EmailApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:Window
    {
        DataTable dt = new DataTable();
        ConnectionClass con = new ConnectionClass();
       
        public MainWindow()
        {
            InitializeComponent();
            List<string> lstColumns = new List<string>(new string[] { "EmailAddress","Domain","User" });
            dt = ConvertToDataTable(lstColumns);
        }

   

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {

            

            string url=txtInputURL.Text;
            if (!url.Contains("http"))
            {
                url = "http://" + url;
            }
            ExtractLink(url);
            txtInputURL.Text = "";
        }
        private void btnShowAllData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navService = NavigationService.GetNavigationService(this);
            ShowAllEmail email = new ShowAllEmail();
            this.Content = email;
            
        }

        private void ExtractLink(string url)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            List<string> listofURL = new List<string>();
            List<string> listOfEmail = ExtractEmail(url, wc);
            lblMessage.Content = "There are " + listOfEmail.Count.ToString() + " number of email are found";
          
           
        }

        private List<string> ExtractEmail(string url, System.Net.WebClient wc)
        {
            string text = wc.DownloadString(url);
            const string MatchEmailPattern =
          @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
          + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
          + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // Find matches.
            MatchCollection matches = rx.Matches(text);
            // Report the number of matches found.
            int noOfMatches = matches.Count;
            // Report on each match.
            List<string> listOfEmail = new List<string>();


            foreach (Match match in matches)
            {
                listOfEmail.Add(match.ToString());
                MailAddress addr = new MailAddress(match.ToString());
                string hostAddress = addr.Host;
                string user = addr.User;

                //DataRow dr=dt.NewRow();
                //dr["EmailAddress"] = match.ToString();
                //dr["Domain"] = hostAddress;
                //dr["User"] = user;
                // dt.Rows.Add(dr);
                // dt.AcceptChanges();
                string insertQuery = @"INSERT INTO tbl_EmailAddressInfo ([EmailAddress], [Domain],[User]) VALUES ('" + match.ToString() + "','" + hostAddress + "','" + user + "')";
                con.ExecuteQuery(insertQuery);
            }
            return listOfEmail;
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (var prop in data)
            {
                table.Columns.Add(prop.ToString());
            }
            return table;
        }
      
    }
}
