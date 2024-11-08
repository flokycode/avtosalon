using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;




namespace Prodazha_avto
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private string connectionString = "Data Source=stud-mssql.sttec.yar.ru,38325;Initial Catalog=user270_db;User ID=user270_db;Password=user270;TrustServerCertificate=True;MultipleActiveResultSets=True;App=EntityFramework;";

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string lastName = LastNameTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string middleName = MiddleNameTextBox.Text;
            string passportNumber = PassportNumberTextBox.Text;
            string address = AddressTextBox.Text;
            string phone = PhoneTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password; // Get password from PasswordBox
            int role = 2;

            if (string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Insert into database
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO CarUser (last_name, first_name, middle_name, passport_num, addresss, phone, login, password, role) VALUES (@LastName, @FirstName, @MiddleName, @PassportNum, @Address, @Phone, @Login, @Password, @Role)", con);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@MiddleName", middleName);
                    cmd.Parameters.AddWithValue("@PassportNum", passportNumber);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password); // Consider hashing the password
                    cmd.Parameters.AddWithValue("@Role", role);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields(); // Optional: Clear fields after successful registration
                    }
                    else
                    {
                        MessageBox.Show("Registration failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearFields()
        {
            LastNameTextBox.Clear();
            FirstNameTextBox.Clear();
            MiddleNameTextBox.Clear();
            PassportNumberTextBox.Clear();
            AddressTextBox.Clear();
            PhoneTextBox.Clear();
            LoginTextBox.Clear();
            PasswordBox.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}