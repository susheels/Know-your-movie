using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Movie.Resources;
using System.IO.IsolatedStorage;
using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.game;
using com.shephertz.app42.paas.sdk.windows.reward;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.paas.sdk.windows.user;

namespace Movie
{
    public partial class MainPage : PhoneApplicationPage,App42Callback
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            GameService gameService = App42API.BuildGameService();
            ScoreBoardService scoreBoardService = App42API.BuildScoreBoardService();
            ScoreService scoreService = App42API.BuildScoreService();
            RewardService rewardService = App42API.BuildRewardService();
            UserService userService = App42API.BuildUserService();    

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;


            username.Text = Global.localUsername.ToUpper();
            email.Text = Global.email;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            scoreBoardService.GetTopNRankers("xp_game", 15, this);
            userService.GetUser(Global.localUsername, this);


        }

       protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("", "Do you wish to exit the app ?", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (m == MessageBoxResult.OK)
            {
                Application.Current.Terminate();
            }
           
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            //if(Global.warpClient != null)
            //    Global.warpClient.Disconnect();

        }
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private void bw_Click(object sender, RoutedEventArgs e)
        {
            settings["trigger"] = 0;
            NavigationService.Navigate(new Uri("/Loading.xaml", UriKind.Relative));
            
        }

        private void hw_Click(object sender, RoutedEventArgs e)
        {
            settings["trigger"] = 1;

            NavigationService.Navigate(new Uri("/Loading.xaml", UriKind.Relative));
            
            
        }

      


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        public void OnException(App42Exception exception)
        {
            throw new NotImplementedException();
        }

        public void OnSuccess(object response)
        {

            if ((response.GetType().ToString()).EndsWith("User"))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
               {
                   User user = (User)response;
                   wins.Text = user.GetProfile().GetLine1();
                   g_play.Text = user.GetProfile().GetLine2();
               });
            }
            else{
            Game game = (Game)response;

            Console.WriteLine("gameName is " + game.GetName());
            for (int i = 0; i < game.GetScoreList().Count; i++)
            {

                Global.s1[i, 0] = game.GetScoreList()[i].GetUserName();
                Global.s1[i, 1] = game.GetScoreList()[i].GetValue() + "";// xp



            }
           }
        }
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            t00.Text = Global.s1[0, 0];
            t10.Text = Global.s1[1, 0];
            t20.Text = Global.s1[2, 0];
            t30.Text = Global.s1[3, 0];
            t40.Text = Global.s1[4, 0];
            t50.Text = Global.s1[5, 0];
            t60.Text = Global.s1[6, 0];
            t70.Text = Global.s1[7, 0];
            t80.Text = Global.s1[8, 0];
            t90.Text = Global.s1[9, 0];
            t100.Text = Global.s1[10, 0];
            t110.Text = Global.s1[11, 0];
            t120.Text = Global.s1[12, 0];
            t130.Text = Global.s1[13, 0];
            t140.Text = Global.s1[14, 0];


            t01.Text = Global.s1[0, 1];
            t11.Text = Global.s1[1, 1];
            t21.Text = Global.s1[2, 1];
            t31.Text = Global.s1[3, 1];
            t41.Text = Global.s1[4, 1];
            t51.Text = Global.s1[5, 1];
            t61.Text = Global.s1[6, 1];
            t71.Text = Global.s1[7, 1];
            t81.Text = Global.s1[8, 1];
            t91.Text = Global.s1[9, 1];
            t101.Text = Global.s1[10, 1];
            t111.Text = Global.s1[11, 1];
            t121.Text = Global.s1[12, 1];
            t131.Text = Global.s1[13, 1];
            t141.Text = Global.s1[14, 1];








        }

       

        }
}