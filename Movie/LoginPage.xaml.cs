using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using SQLite;
using System.IO;
using Windows.Storage;
namespace Movie
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "sample.sqlite"));


        // The sqlite connection.
        private SQLiteConnection dbConn;
        System.Windows.Shapes.Ellipse[] el = new System.Windows.Shapes.Ellipse[5];

        public LoginPage()
        {
            InitializeComponent();

            System.Threading.Thread.Sleep(DateTime.Now.TimeOfDay.Seconds + 1500);

            myPanorama.DefaultItem = myPanorama.Items[0];

            el[0] = e1;
            el[1] = e2;
            el[2] = e3;
            el[3] = e4;
            el[4] = e5;

            el[0].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);

        }

        private void nav_Click(object sender, RoutedEventArgs e)
        {
            dbConn = new SQLiteConnection(DB_PATH);
            // Create the table Task, if it doesn't exist.
            dbConn.CreateTable<Task>();
            // Retrieve the task list from the database.
            // List<Task> retrievedTasks = dbConn.Table<Task>().ToList<Task>();
            var tp = dbConn.Query<Task>("select * from task where id='" + 1 + "'").FirstOrDefault();

            if (tp != null)
            {
                Global.localUsername = tp.Username;
                Global.email = tp.Email;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            }
            else
            {
                
                NavigationService.Navigate(new Uri("/Sign_in.xaml", UriKind.Relative));
            }
        }

        private void indicate(int index)
        {

            for (int i = 0; i < 5; i++ )
                el[i].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);

            el[index].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);

        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Panorama)sender).SelectedIndex)
            {

                case 0: // defines the first PanoramaItem
                    indicate(0);
                break;

                case 1: // second one
                    indicate(1);
                break;

                case 2: // third one
                    indicate(2);
                break;

                case 3: // fourth one
                    indicate(3);
                break;

                case 4: // fifth one
                    indicate(4);
                break;
            }
        }

    }
    public class Task
    {
        /// <summary>
        /// You can create an integer primary key and let the SQLite control it.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ScoreId { get; set; }

        public string Wins { get; set; }

        public string Games { get; set; }

        public string Email { get; set;}

        public string XP { get; set; }


        




        public override string ToString()
        {
            return Username + ":" + Password + " << ";
        }
    }
}