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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public int panel = MainWindow.количествоСтрок % 3;
        public StackPanel NewStackpanel = new StackPanel();

        string name;
        string price;
        string photo;

        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Книги1.mdb";
        string sqlQuery2 = "SELECT COUNT(*) FROM Книги";
        int количествоСтрок;

        StackPanel stackpanel = MainWindow.MyStackPanel;

        public Window2()
        {
            InitializeComponent();

        }

        private static void Window(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1();
            win2.Show();
        }

        private void Add(object sender, RoutedEventArgs e)
        {

            using (OleDbConnection connectionf = new OleDbConnection(connectionString))
            {
                количествоСтрок = 0;
                string sqlQuery2 = "SELECT COUNT(*) FROM Книги";



                // Создание объекта команды с указанием SQL запроса и подключения
                using (OleDbCommand command1 = new OleDbCommand(sqlQuery2, connectionf))
                {
                    try
                    {
                        // Открытие подключения
                        connectionf.Open();

                        // Выполнение команды и получение результата
                        количествоСтрок = (int)command1.ExecuteScalar();
                        connectionf.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            name = name_text.Text;
            price = price_text.Text;
            photo = photo_text.Text;

            if (panel == 3 || panel == 0)
            {
                panel = 1;
                NewStackpanel = new StackPanel();
                NewStackpanel.Orientation = Orientation.Horizontal;
                stackpanel.Children.Add(NewStackpanel);
            }
            else
            {
                panel++;
            }
            Border NewBorder = new Border();
            NewBorder.Width = 160;
            NewBorder.Height = 180;
            NewBorder.Background = Brushes.Azure;
            NewBorder.MouseEnter += Button_MouseEnter;
            NewBorder.MouseLeave += Button_MouseLeave;
            Canvas canvas = new Canvas();
            Image NewImage = new Image();
            TextBlock NewTextBlock = new TextBlock();
            Label NewLabel = new Label();
            Button newButton = new Button();
            newButton.Name = name_text.Text;
            newButton.Click += Border_Down;

            NewImage.Source = new BitmapImage(new Uri(photo, UriKind.RelativeOrAbsolute));

            NewLabel.Content = price;
            NewTextBlock.Text = name;
            NewTextBlock.FontSize = 10;
            canvas.Children.Add(NewImage);
            canvas.Children.Add(newButton);
            canvas.Children.Add(NewTextBlock);
            canvas.Children.Add(NewLabel);
            NewBorder.Child = canvas;
            NewStackpanel.Children.Add(NewBorder);

            string query = "INSERT INTO Книги (Название, Цена, Описание, Фото, Активен, Ключ) VALUES (@Название, @Цена, @Описание, @Фото, @Активен, @Ключ)";

            // Создание подключения и команды
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    connection.Open();
                    // Добавление параметров
                    command.Parameters.AddWithValue("@Название", name_text.Text);
                    command.Parameters.AddWithValue("@Цена", price_text.Text);
                    command.Parameters.AddWithValue("@Описание", info_text.Text);
                    command.Parameters.AddWithValue("@Фото", photo_text.Text);
                    command.Parameters.AddWithValue("@Активен", true);
                    command.Parameters.AddWithValue("@Ключ", name_text.Text);
                    command.ExecuteNonQuery();
                }
            }
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

        public void Border_Down(object sender, RoutedEventArgs e)
        {
            MainWindow.ClickName = Convert.ToString((sender as Control).Name);
            Button clickedBorder = sender as Button;
            MainWindow.ClickBorder = clickedBorder;
            Window1 win2 = new Window1();
            win2.Show();
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
