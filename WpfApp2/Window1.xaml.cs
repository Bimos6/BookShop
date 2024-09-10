using System;
using System.Collections.Generic;
using System.Data.OleDb;
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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public static string name;
        public static string price;
        public static string photo;
        public static string info;
        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Книги1.mdb";


        public Window1()
        {
            InitializeComponent();


            string query1 = $"SELECT Ключ FROM Книги WHERE Название = '{MainWindow.ClickName}';";
            string query2 = $"SELECT Цена FROM Книги WHERE Название = '{MainWindow.ClickName}';";
            string query3 = $"SELECT Описание FROM Книги WHERE Название = '{MainWindow.ClickName}';";
            string query4 = $"SELECT Фото FROM Книги WHERE Название = '{MainWindow.ClickName}';";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbCommand command1 = new OleDbCommand(query1, connection))
                {
                    try
                    {

                        // Выполнение запроса и получение результата
                        object result = command1.ExecuteScalar();

                        // Проверка наличия результата
                        if (result != null)
                        {
                            string bookName = result.ToString();
                            Name.Text = bookName;
                        }
                    }
                    catch (Exception ex) { }
                }
                using (OleDbCommand command2 = new OleDbCommand(query2, connection))
                    {
                        try
                        {

                            // Выполнение запроса и получение результата
                            object result = command2.ExecuteScalar();

                            // Проверка наличия результата
                            if (result != null)
                            {
                                string bookPrice = result.ToString();
                                Price.Text = bookPrice;
                            }
                        }
                        catch (Exception ex) { }
                    }    
                using (OleDbCommand command3 = new OleDbCommand(query3, connection))
                {
                    try
                    {

                        // Выполнение запроса и получение результата
                        object result = command3.ExecuteScalar();

                        // Проверка наличия результата
                        if (result != null)
                        {
                            string bookInfo = result.ToString();
                            Info.Text = bookInfo;
                        }
                    }
                    catch (Exception ex) { }
                }
                using (OleDbCommand command4 = new OleDbCommand(query4, connection))
                {
                    try
                    {

                        // Выполнение запроса и получение результата
                        object result = command4.ExecuteScalar();

                        // Проверка наличия результата
                        if (result != null)
                        {
                            string bookPhoto = result.ToString();
                            Photo.Source = new BitmapImage(new Uri(bookPhoto, UriKind.RelativeOrAbsolute));
                        }
                    }
                    catch (Exception ex) { }
                }
                connection.Close();

                name = Name.Text;
                price = Price.Text;
                info = Info.Text;
            }
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.Azure);
            }
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance != null)
            {
                MainWindow.Instance.Close();
            }

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string query7 = "UPDATE Книги SET Активен = false WHERE Название = @BookTitle";

                using (OleDbCommand command = new OleDbCommand(query7, connection))
                {
                    command.Parameters.AddWithValue("@BookTitle", Name.Text);

                    connection.Open();
                    command.ExecuteNonQuery(); // Выполняем запрос на обновление записи
                }
                connection.Close();
            }

            MainWindow main= new MainWindow();

            main.Show();


            string query = "INSERT INTO История (Название, Цена, Описание, Фото) VALUES (@Название, @Цена, @Описание, @Фото)";

            // Создание подключения и команды
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                connection.Open();
                // Добавление параметров
                command.Parameters.AddWithValue("@Название", Name.Text);
                command.Parameters.AddWithValue("@Цена", Price.Text);
                command.Parameters.AddWithValue("@Описание", Info.Text);
                string imagePath = Photo.Source.ToString();
                command.Parameters.AddWithValue("@Фото", imagePath);
                
                command.ExecuteNonQuery();
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            Window4 win5 = new Window4();
            win5.Show();
        }

        private void Delete(object sender, RoutedEventArgs e) 
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Открытие подключения
                connection.Open();

                // Создание SQL-запроса DELETE
                string sql = $"DELETE FROM Книги WHERE Название = @Name;";

                // Создание объекта команды
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("Name", Name.Text);
                        int rowsAffected = command.ExecuteNonQuery();

                        // Обработка результата, если это необходимо
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключений
                    }
                }
            }
            if (MainWindow.Instance != null)
            {
                MainWindow.Instance.Close();
            }

            MainWindow main = new MainWindow();

            main.Show();
        }
    }
}
