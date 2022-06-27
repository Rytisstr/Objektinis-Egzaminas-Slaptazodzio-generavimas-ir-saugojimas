using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using XSystem.Security.Cryptography;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        int lenght;
        public SqlConnection cnn;
        public string connectionString = @"Data Source=DESKTOP-RDV1TTQ\SQLEXPRESS;Initial Catalog=PaswdGenDervinis; User ID=xd; Password=123";
        public string key = MainWindow.pswddd + MainWindow.salt;

        public string data = DateTime.Today.ToShortDateString(); /*nustatoma data*/
        public Login()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            //tikrinama ar programa neturi klaidu, jei yra klaidu programa parodo klaidos zinute
            try
            {

                lenght = Convert.ToInt16(lengthh.Text); /*ilgis konvertuojamas is teksto i int*/


                //tikrinama ar checkboxai yra pazymeti
                bool lower = (bool)small.IsChecked;
                bool upper = (bool)Big.IsChecked;
                bool number = (bool)Numbers.IsChecked;
                bool symbols = (bool)Symbols.IsChecked;

                //siunciami duomenys i random pass generatoriaus klase passgen
                PassGen pss = new PassGen(); 
                string password = pss.PGen(lenght, lower, upper, number, symbols);
                Result.Text = password; // atspausdina gauta random passworda

            }

            catch {
                MessageBox.Show("Please select atleast 2 types");           
            }
        }
        //sita funkcija uzdaro visa programa paspaudus isjungimo mygtuka
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //uzkrauna datagrida,kad rodytu duombazej esancius passwordus
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cnn = new SqlConnection(connectionString);
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM dbo.Passwords",sqlCon);
                DataTable dt = new DataTable();
                sqlDa.Fill(dt);
                Rodyti.ItemsSource = dt.DefaultView;
            }
        }

        private void Prideti_Click(object sender, RoutedEventArgs e)
        {
            var hashedps = Sifravimas.TrDes(key, Result.Text); /*uzhashinamas gautas random passwordas*/

            //iraso duomenis i db
            cnn = new SqlConnection(connectionString);
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter cmd = new SqlDataAdapter("INSERT INTO dbo.Passwords (pass,imone,data) Values ('" + hashedps + "','"+ imone.Text +"', '"+ data +"')", sqlCon);
                DataTable dtbl = new DataTable();
                cmd.Fill(dtbl);
            }

            //atnaujina db, kad matyti naujus passwordus
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM dbo.Passwords", sqlCon);
                DataTable dt = new DataTable();
                sqlDa.Fill(dt);
                Rodyti.ItemsSource = dt.DefaultView;
            }
        }
    }

}

