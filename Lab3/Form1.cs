using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{

    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=db_book;User ID=admin;Password=admin;SslMode=Required;";
            string username = textBox1.Text;
            string password = textBox2.Text;

            AuthService authService = new AuthService(connectionString);

            string role = authService.AuthenticateUser(username, password);

            if (role == "User")
            {
                MessageBox.Show("Вход выполнен как пользователь!");
                UserForm userForm = new UserForm(); // Создайте и откройте форму для пользователя
                userForm.Show();
                this.Hide(); // Скрыть текущую форму
            }
            else if (role == "Admin")
            {
                MessageBox.Show("Вход выполнен как администратор!");
                AdminForm adminForm = new AdminForm(); // Создайте и откройте форму для администратора
                adminForm.Show();
                this.Hide(); // Скрыть текущую форму
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}
