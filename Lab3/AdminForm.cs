using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Lab3
{
    public partial class AdminForm : Form
    {
        private MySqlConnection connection;
        private MySqlDataAdapter adapter;
        private DataTable usersTable;
        DatabaseHelper dbHelper = new DatabaseHelper();
        
        public AdminForm()
        {
            InitializeComponent();
            connection = new MySqlConnection(dbHelper.GetConnectionString());
            LoadUserData();
        }

        private void LoadUserData()
        {
            string query = "SELECT * FROM users"; // Загрузка пользователей с ролью
            adapter = new MySqlDataAdapter(query, connection);
            usersTable = new DataTable();
            adapter.Fill(usersTable);
            dataGridView1.DataSource = usersTable;
        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text; // Предполагается, что у вас есть текстовое поле для ввода имени пользователя
            string password = textBoxPassword.Text; // Текстовое поле для пароля
            string role = comboBoxRole.SelectedItem.ToString(); // Комбобокс для выбора роли

            try
            {
                AddUser(username, password, role);
                LoadUserData(); // Обновление данных в DataGridView
                MessageBox.Show("Пользователь добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении пользователя: " + ex.Message);
            }
        }

        private void AddUser(string username, string password, string role)
        {
            using (var connection = new MySqlConnection(dbHelper.GetConnectionString()))
            {
                connection.Open();
                string query = "INSERT INTO users (Username, Password, Role) VALUES (@Username, @Password, @Role)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            // Получаем ID пользователя из текстового поля
            string userIdText = textBoxUserId.Text;

            // Проверяем, ввел ли пользователь ID
            if (string.IsNullOrWhiteSpace(userIdText))
            {
                MessageBox.Show("Пожалуйста, введите ID пользователя для удаления.");
                return;
            }

            // Проверяем, что введенный ID является числом
            if (!int.TryParse(userIdText, out int userId))
            {
                MessageBox.Show("Пожалуйста, введите корректный ID пользователя.");
                return;
            }

            // Подтверждаем действие удаления
            var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя с ID '{userId}'?", "Подтверждение удаления", MessageBoxButtons.YesNo);

            // Если пользователь подтвердил удаление
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Вызываем метод для удаления пользователя
                    DeleteUser(userId);
                    // Обновляем данные в DataGridView
                    LoadUserData();
                    MessageBox.Show("Пользователь успешно удален.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}");
                }
            }
        }

        private void DeleteUser(int userId)
        {
            using (var connection = new MySqlConnection(dbHelper.GetConnectionString()))
            {
                connection.Open();
                string query = "DELETE FROM users WHERE User_ID = @UserId"; // Предположим, что у вас есть столбец Id в таблице users

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery(); // Выполняем запрос
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string username = dataGridView1.Rows[e.RowIndex].Cells["Username"].Value.ToString();
            string password = dataGridView1.Rows[e.RowIndex].Cells["Password"].Value.ToString();
            string role = dataGridView1.Rows[e.RowIndex].Cells["Role"].Value.ToString();

            try
            {
                UpdateUser(username, password, role);
                MessageBox.Show("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
            }
        }

        private void UpdateUser(string username, string password, string role)
        {
            using (var connection = new MySqlConnection(dbHelper.GetConnectionString()))
            {
                connection.Open();
                string query = "UPDATE users SET Password = @Password, Role = @Role WHERE Username = @Username";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // Здесь вы можете выполнить дополнительные действия при загрузке формы, если необходимо
        }

        

    }
}
