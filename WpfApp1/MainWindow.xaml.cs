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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using XSystem.Security.Cryptography;

namespace WpfApp1
{
    
    public partial class MainWindow : Window
    {
        public SqlConnection cnn;
        public string connectionString = @"Data Source=DESKTOP-RDV1TTQ\SQLEXPRESS;Initial Catalog=PaswdGenDervinis; User ID=xd; Password=123";
        public static string salt = "tXqg5m";
        public static string pswddd;
        public string pss;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void LButton_Click(object sender, RoutedEventArgs e)
        {

            // Uzkoduoja suvesta passworda sha256 metodu
            var hasher = new SHA256Managed();
            var unhashed = System.Text.Encoding.Unicode.GetBytes(pass.Password);
            var hashed = hasher.ComputeHash(unhashed);
            string pwd = Convert.ToBase64String(hashed);
            string psswd = pwd.Insert(10, salt);

            //tikrina suvesta, uzhashinta ir su pridetu saltu passworda su esanciu duombazej
            if (psswd == MasterPass())
            {
                //jei passwordas geras permetamas i sekanti langa
                pswddd = pass.Password;
                this.Hide();
                Login login = new Login();
                login.Show();
            }
            else {
                MessageBox.Show("Master password is incorrect");
            }
        }
        //sita funkcija uzdaro visa programa paspaudus isjungimo mygtuka
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //jei nera master passwordo galima ji susikurti cia
        private void CreateP_Click(object sender, RoutedEventArgs e)
        {

            //viskas apacioje tikrina ar egzistuoja master passwordas
            if (MasterPass() == null)
            {
                MessageBox.Show("There's no password");
                Create create = new Create();
                create.Show();
                this.Hide();
            }
            else if (MasterPass() != null)
            {
                MessageBox.Show("Master password has already been generated");
                return;
            }
            
        }


        //Naudojamas slaptazodis
        string MasterPass()
        {
            //Prisijungimas prie duomenu bazes ir Master slaptazodzio ieskojimas
            cnn = new SqlConnection(connectionString);
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("SELECT Pass From dbo.Master WHERE id = 1", sqlCon);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pss = String.Format("{0}", dr[0]);
                }
            }


            return pss;
        }

    }
}
