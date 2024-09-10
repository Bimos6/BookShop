using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CompanyCoreLib;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public static StackPanel MyStackPanel { get; set; }

        public static string ClickName { get; set; }

        public static Button ClickBorder { get; set; }

        public static MainWindow Instance { get; private set; }

        public StackPanel NewStackpanel = new StackPanel();

        public static int количествоСтрок = 0;
        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Книги1.mdb";

        public MainWindow()
        {

            InitializeComponent();


            Loaded += MainWindow_Loaded;
            MyStackPanel = stackpanel;
            Instance = this;
            NewStackpanel.Orientation = Orientation.Horizontal;
            stackpanel.Children.Add(NewStackpanel);

            string название = "";
            string цена = "";
            string фото = "";
           
            
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    количествоСтрок = 0;
                    string sqlQuery2 = "SELECT COUNT(*) FROM Книги";



                    // Создание объекта команды с указанием SQL запроса и подключения
                    using (OleDbCommand command1 = new OleDbCommand(sqlQuery2, connection))
                    {
                        try
                        {
                            // Открытие подключения
                            connection.Open();

                            // Выполнение команды и получение результата
                            количествоСтрок = (int)command1.ExecuteScalar();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    int panel = 0;
                    using (OleDbConnection connectionz = new OleDbConnection(connectionString))
                    {
                        string sql = "SELECT * FROM Книги;";

                        connectionz.Open();

                        // Создание объекта команды
                        using (OleDbCommand command = new OleDbCommand(sql, connectionz))
                        {
                            // Выполнение SQL-запроса и получение результата
                            using (OleDbDataReader reader = command.ExecuteReader())
                            {
                                // Перебор строк результата
                                for (int i = 0; reader.Read(); i++)
                                {
                                    название = reader["Ключ"].ToString();
                                    цена = reader["Цена"].ToString();
                                    фото = reader["Фото"].ToString();


                                    if (panel % 3 == 0)
                                    {
                                        NewStackpanel = new StackPanel();
                                        NewStackpanel.Orientation = Orientation.Horizontal;
                                        stackpanel.Children.Add(NewStackpanel);
                                        panel = 0;
                                    }
                                    panel++;

                                    Border NewBorder = new Border();
                                    NewBorder.Height = 160;
                                    Canvas canvas = new Canvas();
                                    Image NewImage = new Image();
                                    TextBlock NewTextBlock = new TextBlock();
                                    Label NewLabel = new Label();
                                    Button newButton = new Button();

                                    newButton.Name = название;

                                    int count = 0;
                                    using (OleDbConnection connection3 = new OleDbConnection(connectionString))
                                    {
                                        string query = "SELECT COUNT(*) FROM Книги WHERE Название = @BookTitle AND Активен = false";

                                        using (OleDbCommand command1 = new OleDbCommand(query, connection3))
                                        {
                                            command1.Parameters.AddWithValue("@BookTitle", название);

                                            connection3.Open();
                                            count = (int)command1.ExecuteScalar();
                                        }
                                    }

                                    if (count > 0)
                                    {
                                        count = 0;
                                        newButton.Opacity = 100;
                                        newButton.Content = "Не активно";
                                        newButton.Background = Brushes.Red;
                                        newButton.MinHeight = 165;
                                        newButton.Width = 160;
                                        newButton.Margin = new Thickness(0, 195, 0, 0);
                                        newButton.IsEnabled = false;
                                    }
                                    else
                                    {
                                        newButton.Click += Border_Down;
                                    }

                                    NewTextBlock.Foreground = Brushes.DarkOrchid;
                                    NewTextBlock.FontSize = 13;
                                    NewTextBlock.Margin = new Thickness(0, 10, 0, 0);
                                    NewImage.Source = new BitmapImage(new Uri(фото, UriKind.RelativeOrAbsolute));
                                    NewLabel.VerticalAlignment = VerticalAlignment.Center;
                                    Canvas.SetLeft(NewTextBlock, 95);
                                    Canvas.SetTop(NewTextBlock, 230);


                                    NewTextBlock.Text = название;
                                    NewLabel.Content = цена;

                                    canvas.Children.Add(NewImage);
                                    canvas.Children.Add(NewTextBlock);
                                    canvas.Children.Add(NewLabel);
                                    canvas.Children.Add(newButton);
                                    NewBorder.Child = canvas;
                                    NewStackpanel.Children.Add(NewBorder);
                                }
                            }
                        }
                    }
                }
        }
        
        private void button_menu_Click(object sender, RoutedEventArgs e)
        {
            var a = menu.Visibility == Visibility.Visible? Visibility.Hidden: Visibility.Visible;
            menu.Visibility = a;
        }

        private void button_add_tovar(object sender, RoutedEventArgs e)
        {
            Window2 win3 = new Window2();
            win3.Show();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Установка окна на полный экран
            WindowState = WindowState.Maximized;
        }

        private void button_story(object sender, RoutedEventArgs e)
        {
            Window3 win4 = new Window3();
            win4.Show();
        }
        private void button_story2(object sender, RoutedEventArgs e)
        {
            Window5 win5 = new Window5();
            win5.Show();
        }

        private void Tovar_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void Tovar_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new SolidColorBrush(Colors.Azure);
            }
        }

        public void Border_Down(object sender, RoutedEventArgs e)
        {
            ClickName = Convert.ToString((sender as Control).Name);
            Button clickedBorder = sender as Button;
            ClickBorder = clickedBorder;
            Window1 win2 = new Window1();
            win2.Show();
        }

        public void Search(object sender, RoutedEventArgs e)
        {
            string ser = search_text.Text;
            StackPanel panel = FindName("panel") as StackPanel;

            if (search_text.Text != null)
            {
                // Удаляем все элементы из StackPanel
                stackpanel.Children.Clear();
                using (OleDbConnection connectionz = new OleDbConnection(connectionString))
                {
                    string sql = "SELECT * FROM Книги WHERE Название = @BookName";
                    connectionz.Open();

                    // Создание объекта команды
                    using (OleDbCommand command = new OleDbCommand(sql, connectionz))
                    {
                        command.Parameters.AddWithValue("@BookName", ser);
                        string название = "";
                        string цена = "";
                        string фото = "";

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                название = reader["Ключ"].ToString();
                                цена = reader["Цена"].ToString();
                                фото = reader["Фото"].ToString();

                                NewStackpanel = new StackPanel();
                                NewStackpanel.Orientation = Orientation.Horizontal;
                                stackpanel.Children.Add(NewStackpanel);

                                Border NewBorder = new Border();
                                NewBorder.Height = 160;
                                Canvas canvas = new Canvas();
                                Image NewImage = new Image();
                                TextBlock NewTextBlock = new TextBlock();
                                Label NewLabel = new Label();
                                Button newButton = new Button();

                                newButton.Name = название;

                                int count = 0;
                                using (OleDbConnection connection3 = new OleDbConnection(connectionString))
                                {
                                    string query = "SELECT COUNT(*) FROM Книги WHERE Название = @BookTitle AND Активен = false";

                                    using (OleDbCommand command1 = new OleDbCommand(query, connection3))
                                    {
                                        command1.Parameters.AddWithValue("@BookTitle", название);

                                        connection3.Open();
                                        count = (int)command1.ExecuteScalar();
                                    }
                                }

                                if (count > 0)
                                {
                                    count = 0;
                                    newButton.Opacity = 100;
                                    newButton.Content = "Не активно";
                                    newButton.Background = Brushes.Red;
                                    newButton.MinHeight = 165;
                                    newButton.Width = 160;
                                    newButton.Margin = new Thickness(0, 195, 0, 0);
                                    newButton.IsEnabled = false;
                                }
                                else
                                {
                                    newButton.Click += Border_Down;
                                }

                                NewTextBlock.Foreground = Brushes.DarkOrchid;
                                NewTextBlock.FontSize = 13;
                                NewTextBlock.Margin = new Thickness(0, 10, 0, 0);
                                NewImage.Source = new BitmapImage(new Uri(фото, UriKind.RelativeOrAbsolute));
                                NewLabel.VerticalAlignment = VerticalAlignment.Center;
                                Canvas.SetLeft(NewTextBlock, 95);
                                Canvas.SetTop(NewTextBlock, 230);


                                NewTextBlock.Text = название;
                                NewLabel.Content = цена;

                                canvas.Children.Add(NewImage);
                                canvas.Children.Add(NewTextBlock);
                                canvas.Children.Add(NewLabel);
                                canvas.Children.Add(newButton);
                                NewBorder.Child = canvas;
                                NewStackpanel.Children.Add(NewBorder);
                            }
                        }   
                    }
                }
            }
            else
            {
                MessageBox.Show("пусто");
            }
        }

        private void All(object sender, RoutedEventArgs e)
        {
            stackpanel.Children.Clear();

            string название = "";
            string цена = "";
            string фото = "";


            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                количествоСтрок = 0;
                string sqlQuery2 = "SELECT COUNT(*) FROM Книги";



                // Создание объекта команды с указанием SQL запроса и подключения
                using (OleDbCommand command1 = new OleDbCommand(sqlQuery2, connection))
                {
                    try
                    {
                        // Открытие подключения
                        connection.Open();

                        // Выполнение команды и получение результата
                        количествоСтрок = (int)command1.ExecuteScalar();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                int panel2 = 0;
                using (OleDbConnection connectionz = new OleDbConnection(connectionString))
                {
                    string sql = "SELECT * FROM Книги;";

                    connectionz.Open();

                    // Создание объекта команды
                    using (OleDbCommand command = new OleDbCommand(sql, connectionz))
                    {
                        // Выполнение SQL-запроса и получение результата
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            // Перебор строк результата
                            for (int i = 0; reader.Read(); i++)
                            {
                                название = reader["Ключ"].ToString();
                                цена = reader["Цена"].ToString();
                                фото = reader["Фото"].ToString();


                                if (panel2 % 3 == 0)
                                {
                                    NewStackpanel = new StackPanel();
                                    NewStackpanel.Orientation = Orientation.Horizontal;
                                    stackpanel.Children.Add(NewStackpanel);
                                    panel2 = 0;
                                }
                                panel2++;

                                Border NewBorder = new Border();
                                NewBorder.Height = 160;
                                Canvas canvas = new Canvas();
                                Image NewImage = new Image();
                                TextBlock NewTextBlock = new TextBlock();
                                Label NewLabel = new Label();
                                Button newButton = new Button();

                                newButton.Name = название;

                                int count = 0;
                                using (OleDbConnection connection3 = new OleDbConnection(connectionString))
                                {
                                    string query = "SELECT COUNT(*) FROM Книги WHERE Название = @BookTitle AND Активен = false";

                                    using (OleDbCommand command1 = new OleDbCommand(query, connection3))
                                    {
                                        command1.Parameters.AddWithValue("@BookTitle", название);

                                        connection3.Open();
                                        count = (int)command1.ExecuteScalar();
                                    }
                                }

                                if (count > 0)
                                {
                                    count = 0;
                                    newButton.Opacity = 100;
                                    newButton.Content = "Не активно";
                                    newButton.Background = Brushes.Red;
                                    newButton.MinHeight = 165;
                                    newButton.Width = 160;
                                    newButton.Margin = new Thickness(0, 195, 0, 0);
                                    newButton.IsEnabled = false;
                                }
                                else
                                {
                                    newButton.Click += Border_Down;
                                }

                                NewTextBlock.Foreground = Brushes.DarkOrchid;
                                NewTextBlock.FontSize = 13;
                                NewTextBlock.Margin = new Thickness(0, 10, 0, 0);
                                NewImage.Source = new BitmapImage(new Uri(фото, UriKind.RelativeOrAbsolute));
                                NewLabel.VerticalAlignment = VerticalAlignment.Center;
                                Canvas.SetLeft(NewTextBlock, 95);
                                Canvas.SetTop(NewTextBlock, 230);


                                NewTextBlock.Text = название;
                                NewLabel.Content = цена;

                                canvas.Children.Add(NewImage);
                                canvas.Children.Add(NewTextBlock);
                                canvas.Children.Add(NewLabel);
                                canvas.Children.Add(newButton);
                                NewBorder.Child = canvas;
                                NewStackpanel.Children.Add(NewBorder);
                            }
                        }
                    }
                }
            }
        }
    }
}

