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
    public partial class UserForm : Form
    {
        private MySqlConnection connection;

        public UserForm()
        {
            InitializeComponent();
            // Инициализируем соединение с базой данных
            DatabaseHelper dbHelper = new DatabaseHelper();
            connection = new MySqlConnection(dbHelper.GetConnectionString());
            LoadBooksData(); // Загружаем данные книг при инициализации формы
        }

        private void LoadBooksData()
        {
            try
            {
                // Открываем соединение
                connection.Open();
                // SQL-запрос для получения данных из таблицы books
                string query = "SELECT * FROM books";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable booksTable = new DataTable();

                // Заполняем DataTable данными из базы данных
                adapter.Fill(booksTable);
                // Устанавливаем источник данных для DataGridView
                dataGridViewBooks.DataSource = booksTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
            finally
            {
                // Закрываем соединение
                connection.Close();
            }
        }
    }
}