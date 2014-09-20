using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.user;
using System.IO.IsolatedStorage;
using SQLite;
using System.IO;
using Windows.Storage;


namespace Movie
{
    public partial class Sign_in : PhoneApplicationPage, App42Callback
    {
        // The database path.
        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "sample.sqlite"));


        // The sqlite connection.
        private SQLiteConnection dbConn;

        UserService userService = null;
        public Sign_in()
        {
            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            InitializeComponent();
            userService = App42API.BuildUserService();
        }

        private void signb_Click(object sender, RoutedEventArgs e)
        {
            userService.Authenticate(uname.Text, pname.Text, this);
        }

        public void OnException(App42Exception exception)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (exception.GetAppErrorCode() == 1401)
                {
                    MessageBox.Show("Please Enter valid api key and secret key getting from AppHQ Management Console!!!", "App42 Security Exception", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(exception.Message);

                }
            });
        }


        public void OnSuccess(object response)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                User user = (User)response;
                String jsonResponse = user.ToString();
                Global.app42sessionid = user.GetSessionId();
                Global.localUsername = user.GetUserName();
                Global.email = user.GetEmail();
                Global.password = pname.Text;


                NavigationService.Navigate(new Uri("/transition.xaml", UriKind.Relative));

            });
            

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserSetup.xaml", UriKind.Relative));

        }

        

    }
}