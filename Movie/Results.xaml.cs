using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using com.shephertz.app42.paas.sdk.windows.game;
using com.shephertz.app42.paas.sdk.windows.reward;
using com.shephertz.app42.paas.sdk.windows;
using System.IO.IsolatedStorage;
using SQLite;
using System.IO;
using Windows.Storage;
using com.shephertz.app42.paas.sdk.windows.achievement;


namespace Movie
{
    public partial class Results : PhoneApplicationPage, App42Callback
    {
        // The database path.
        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "sample.sqlite"));


        // The sqlite connection.
        private SQLiteConnection dbConn;
        GameService gameService = App42API.BuildGameService();
        ScoreBoardService scoreBoardService = App42API.BuildScoreBoardService();
        ScoreService scoreService = App42API.BuildScoreService();
        RewardService rewardService = App42API.BuildRewardService();
        AchievementService achievementService = App42API.BuildAchievementService();
        private DispatcherTimer _dispatcherTimer;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
       
        string achievementName;
        int level;
        string scoreId;
        double new_xp;
        double prev_xp;
        double pres_xp;
        public Results()
        {
            InitializeComponent();
            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            string gameName;
            if ((int)settings["trigger"] == 0)
            {
                gameName = "Bollywood_game";
            }
            else
            {
                gameName = "Hollywood_game";
            }


            new_xp = Global.findxp(Global.p1Score, Global.p2Score);
            dbConn = new SQLiteConnection(DB_PATH);

            var tpdata = dbConn.Query<Task>("select * from task where id='" + 1 + "'").FirstOrDefault();
            // Check result is empty or not
            if (tpdata == null)
                MessageBox.Show("Title Not Present in DataBase");
            else
            {

                scoreId = tpdata.ScoreId;
                prev_xp = Convert.ToDouble(tpdata.XP);
                pres_xp = new_xp + prev_xp;

                var tp = dbConn.Query<Task>("update task set XP='" + pres_xp + "' where Username = '" + tpdata.Username + "'").FirstOrDefault();
                // Update Database
                dbConn.Update(tp);
               level =  Global.levelFromXP(pres_xp);
                switch(level)
                {
                    case 1:
                        achievementName = "level_1";
                        break;
                    case 2:
                        achievementName = "level_2";
                        break;
                    case 3:
                        achievementName = "level_3";
                        break;
                    case 4:
                        achievementName = "level_4";
                        break;
                    
                }
               
            }

            scoreBoardService.SaveUserScore(gameName, Global.localUsername, Global.p1Score, this);
          
            scoreBoardService.EditScoreValueById(scoreId, pres_xp, this);
            
            achievementService.EarnAchievement(Global.localUsername, achievementName, "xp_game", "",this);  
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _dispatcherTimer.Tick += UpdateMediaData;
            _dispatcherTimer.Start();

            System.Diagnostics.Debug.WriteLine("navigated to results");
            Global.notificationListenerObj = new NotificationListener(this);
            Global.warpClient.AddNotificationListener(Global.notificationListenerObj);
            System.Diagnostics.Debug.WriteLine("added notif listener");

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                tb.Visibility = Visibility.Collapsed;
                WIN.Visibility = Visibility.Collapsed;
                LOSE.Visibility = Visibility.Collapsed;
                DRAW.Visibility = Visibility.Collapsed;

                myname.Text = Global.localUsername;
                opponent.Text = Global.opponentUsername;

                if (Global.p1Score > Global.p2Score)
                    WIN.Visibility = Visibility.Visible;
                else if (Global.p1Score < Global.p2Score)
                    LOSE.Visibility = Visibility.Visible;
                else
                    DRAW.Visibility = Visibility.Visible;

                p1scoring.Text = Convert.ToString(Global.p1Score);
                p2scoring.Text = Convert.ToString(Global.p2Score);
            });
            System.Diagnostics.Debug.WriteLine("updated UI");
        }

        private void UpdateMediaData(object sender, EventArgs e)
        {
            if (Global.deleteSuccess)
            {
                Global.warpClient.Disconnect();
                Global.deleteSuccess = false;
            }

            if (Global.disconnectSuccess)
            {
                Global.disconnectSuccess = false;
                Global.warpClient.RemoveConnectionRequestListener(Global.conListenObj);
                Global.warpClient.RemoveRoomRequestListener(Global.roomReqListenerObj);
                Global.warpClient.RemoveZoneRequestListener(Global.zoneReqListenerObj);
                Global.warpClient.RemoveNotificationListener(Global.notificationListenerObj);

                _dispatcherTimer.Stop();
                Global.opponentUsername = "";

                NavigationService.Navigate(new Uri("/transition2.xaml", UriKind.Relative));
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ret_Click(object sender, RoutedEventArgs e)
        {
            Global.warpClient.DeleteRoom(Global.DynRoomId);
            
            System.Diagnostics.Debug.WriteLine("end of ret");
        }


        public void OnException(App42Exception exception)
        {
           
        }

        public void OnSuccess(object response)
        {
           
        }
    }
}