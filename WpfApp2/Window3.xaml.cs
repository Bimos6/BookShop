using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Xml.Linq;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            Construct();
        }

        private void Construct()
        {
            string название = "";
            string цена = "";
            string описание = "";
            string фото = "";

            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Книги1.mdb";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                int количествоСтрок = 0;
                string sqlQuery2 = "SELECT COUNT(*) FROM История";



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
                for(int i = 1; i < количествоСтрок+1; i++)
                {
                    string sqlQuery1 = $"SELECT Название, Цена, Описание, Фото FROM История WHERE Код = {i}";
                    using (OleDbCommand command = new OleDbCommand(sqlQuery1, connection))
                    {
                        try
                        {
                            connection.Open();

                            using (OleDbDataReader reader = command.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    // Получение значений полей
                                    название = reader["Название"].ToString();
                                    цена = reader["Цена"].ToString();
                                    описание = reader["Описание"].ToString();
                                    фото = reader["Фото"].ToString();
                                }

                            }
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    StackPanel NewStackpanel = new StackPanel();
                    Border NewBorder = new Border();
                    NewBorder.Height = 160;
                    Canvas canvas = new Canvas();
                    Image NewImage = new Image();
                    TextBlock NewTextBlock = new TextBlock();
                    Label NewLabel1 = new Label();
                    Label NewLabel2 = new Label();


                    NewImage.Source = new BitmapImage(new Uri(фото, UriKind.RelativeOrAbsolute));
                    NewLabel1.Content = название;
                    NewLabel2.Content = цена;
                    NewLabel2.Margin = new Thickness(210, 0, 0, 0);
                    NewLabel2.Width = 71;
                    NewTextBlock.Text = описание;
                    NewTextBlock.Foreground = Brushes.DarkOrchid;

                    canvas.Children.Add(NewImage);
                    canvas.Children.Add(NewTextBlock);
                    canvas.Children.Add(NewLabel1);
                    canvas.Children.Add(NewLabel2);
                    NewBorder.Child = canvas;
                    NewStackpanel.Children.Add(NewBorder);

                    stackpanel.Children.Add(NewStackpanel);
                }
               
            }
        }
    }
}
