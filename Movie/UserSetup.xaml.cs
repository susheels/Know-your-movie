using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.user;
using System.IO.IsolatedStorage;
using SQLite;
using System.IO;
using Windows.Storage;


namespace Movie
{
    public partial class UserSetup : PhoneApplicationPage, App42Callback
    {
        // The database path.
        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "sample.sqlite"));

        bool flag = true;
        // The sqlite connection.
        private SQLiteConnection dbConn;

        UserService userService = null;

        public UserSetup()
        {

            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            InitializeComponent();

            userService = App42API.BuildUserService();

        }


        private void userbut_Click(object sender, RoutedEventArgs e)
        {
            userService.CreateUser(uname.Text, pass.Text, email.Text, this);
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
                    //UserOutput.Text = string.Empty;
                    //UserOutput.Text = "Exception Caught successfully:-" + exception.Message;
                }
            });
        }


        public void OnSuccess(object response)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                User user = (User)response;
                {
                    if (flag)
                    {

                        User.Profile profile = new User.Profile(user);
                       
                        profile.SetLine1("0");    // for wins
                        profile.SetLine2("0");    // for games played
                        flag = false;
                        userService.CreateOrUpdateProfile(user, this);
                    }
                    String jsonResponse = user.ToString();


                    Global.localUsername = user.GetUserName();
                    Global.email = user.GetEmail();
                    Global.password = pass.Text;

                    NavigationService.Navigate(new Uri("/transition.xaml", UriKind.Relative));


                }

            });
        }
    }
}