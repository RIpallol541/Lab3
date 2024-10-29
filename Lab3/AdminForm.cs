using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Lab3
{
    public partial class AdminForm : Form
    {
        private Database _database;
        private MySqlDataAdapter adapter;
        private DataTable usersTable;

        public AdminForm()
        {
            InitializeComponent();
            DatabaseHelper dbHelper = new DatabaseHelper();
            string connectionString = dbHelper.GetConnectionString();
            string localDatabasePath = "..\\Lab3\\Resources\\data\\backup_file.db"; // Укажите путь к вашей локальной базе
            _database = new Database(connectionString, localDatabasePath);
            _database.OpenConnection(); // Открываем соединение при инициализации
            LoadUserData();
        }

        private void LoadUserData()
        {
            string query = "SELECT * FROM users"; // Загрузка пользователей с ролью
            adapter = new MySqlDataAdapter(query, _database.GetConnection());
            usersTable = new DataTable();
            adapter.Fill(usersTable);
            dataGridView1.DataSource = usersTable;
        }

        // Метод для резервного копирования базы данных
        private void BackupButton_Click(object sender, EventArgs e)
        {
            string backupPath = ".\\Lab3\\Resources\\data\\backup_file.db"; // Укажите путь для резервной копии
            _database.BackupDatabase(backupPath);
        }

        // Метод для восстановления базы данных
        private void RestoreButton_Click(object sender, EventArgs e)
        {
            string backupPath = ".\\Lab3\\Resources\\data\\backup_file.db"; // Укажите путь к резервной копии
            _database.RestoreDatabase(backupPath);
        }

        // Не забудьте закрыть соединение при закрытии формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _database.CloseConnection();
            base.OnFormClosing(e);
        }
    }
}
