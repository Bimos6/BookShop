using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {

        public Window4()
        {
            InitializeComponent();

            name_text.Text = Window1.name;
            price_text.Text = Window1.price;
            info_text.Text = Window1.info;
            photo_text.Text = Window1.photo;
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.Azure);
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Книги1.mdb";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // SQL-запрос для обновления цены книги с названием "История"
                    string updateQuery1 = $"UPDATE Книги SET Цена = @НоваяЦена WHERE Название = '{MainWindow.ClickName}'";

                    // Создание команды с параметрами
                    using (OleDbCommand command = new OleDbCommand(updateQuery1, connection))
                    {
                        // Параметр для новой цены
                        command.Parameters.AddWithValue("@НоваяЦена", price_text.Text);
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    // SQL-запрос для обновления цены книги с названием "История"
                    string updateQuery2 = $"UPDATE Книги SET Ключ = @Ключ WHERE Название = '{MainWindow.ClickName}'";

                    // Создание команды с параметрами
                    using (OleDbCommand command = new OleDbCommand(updateQuery2, connection))
                    {
                        // Параметр для новой цены
                        command.Parameters.AddWithValue("@Ключ", name_text.Text);
                        MainWindow.ClickBorder.Name = name_text.Text;
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    string updateQuery3 = $"UPDATE Книги SET Описание = @НовоеОписание WHERE Название = '{MainWindow.ClickName}'";

                    // Создание команды с параметрами
                    using (OleDbCommand command = new OleDbCommand(updateQuery3, connection))
                    {
                        // Параметр для новой цены
                        command.Parameters.AddWithValue("@НовоеОписание", info_text.Text);
                        MainWindow.ClickBorder.Name = name_text.Text;
                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    string updateQuery4 = $"UPDATE Книги SET Фото = @НовоеФото WHERE Название = '{MainWindow.ClickName}'";

                    // Создание команды с параметрами
                    using (OleDbCommand command = new OleDbCommand(updateQuery4, connection))
                    {
                        // Параметр для новой цены
                        command.Parameters.AddWithValue("@НовоеФото", photo_text.Text);
                        MainWindow.ClickBorder.Name = name_text.Text;
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (OleDbException ex)
                {
                }
                connection.Close();
            }

            if (MainWindow.Instance != null)
            {
                MainWindow.Instance.Close();
            }

            MainWindow main = new MainWindow();

            main.Show();
        }
        private void File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*"; // Фильтр для всех файлов
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Начальная директория

            // Отображение диалогового окна и проверка результата
            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем выбранный путь к файлу
                photo_text.Text = openFileDialog.FileName;

                // Далее вы можете сохранить путь к файлу в переменной или использовать его по вашему усмотрению
                // Например, вы можете сохранить его в свойстве вашего класса или передать в другой метод для обработки
                // Например:
                // this.SelectedFilePath = selectedFilePath;
                // или
                // ProcessSelectedFile(selectedFilePath);
            }
        }
    }
}
