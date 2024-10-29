using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using System.Configuration;

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
          string username = textBox1.Text;
            string password = textBox2.Text;
            DatabaseHelper dbHelper = new DatabaseHelper();
            AuthService authService = new AuthService(dbHelper.GetConnectionString());

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
