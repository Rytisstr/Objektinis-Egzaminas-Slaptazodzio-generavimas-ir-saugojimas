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

    public partial class Create : Window
    {
        public SqlConnection cnn;
        public string connectionString = @"Data Source=DESKTOP-RDV1TTQ\SQLEXPRESS;Initial Catalog=PaswdGenDervinis; User ID=xd; Password=123";
        public string pass;
 
        public Create()
        {
            InitializeComponent();
        }


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

            //Tikrinama ar visi laukai yra uzpildyti
            if (String.IsNullOrEmpty(Pass.Password) || String.IsNullOrEmpty(Repeat.Password))
            {
                MessageBox.Show("Fill all fields");
            }
            //Jei visi laukai yra uzpildyti pradedamas registracijos veiksmas
            else {
                if (Pass.Password == Repeat.Password)
                {

                    var hasher = new SHA256Managed();
                    var unhashed = System.Text.Encoding.Unicode.GetBytes(Pass.Password);
                    var hashed = hasher.ComputeHash(unhashed);
                    string pwd = Convert.ToBase64String(hashed);
                    string password = pwd.Insert(10, MainWindow.salt);


                    //master slaptazodis irasomas i duombaze
                    cnn = new SqlConnection(connectionString);
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter cmd = new SqlDataAdapter("INSERT INTO dbo.Master (Pass) Values ('"+password+"')", sqlCon);
                        DataTable dtbl = new DataTable();
                        cmd.Fill(dtbl);
                    }
                }
                else {
                    MessageBox.Show("Passwords do not match");
                }
            }
        }
        //sita funkcija uzdaro visa programa paspaudus isjungimo mygtuka
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
