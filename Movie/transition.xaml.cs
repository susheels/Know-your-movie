using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using com.shephertz.app42.paas.sdk.windows.game;
using com.shephertz.app42.paas.sdk.windows.reward;
using com.shephertz.app42.paas.sdk.windows;
using SQLite;
using System.IO;
using Windows.Storage;
namespace Movie
{
    public partial class transition : PhoneApplicationPage, App42Callback
    
    {
        // The database path.
        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "sample.sqlite"));


        // The sqlite connection.
        private SQLiteConnection dbConn;

        GameService gameService = App42API.BuildGameService();
        ScoreBoardService scoreBoardService = App42API.BuildScoreBoardService();
        ScoreService scoreService = App42API.BuildScoreService();
        


        public transition()
        {
            InitializeComponent();
            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            dbConn = new SQLiteConnection(DB_PATH);
            Task task = new Task()
            {
                Username = Global.localUsername,
                Password = Global.password,
                Email = Global.email,
                XP = "0",

            };
            // Insert the new task in the Task table.

            dbConn.Insert(task);
            scoreBoardService.SaveUserScore("xp_game", Global.localUsername, 0, this);
           
        

        }

        public void OnException(App42Exception exception)
        {
            
        }

        public void OnSuccess(object response)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
         {
             Game game = (Game)response;
             string scoreid = game.GetScoreList()[0].GetScoreId();

             dbConn = new SQLiteConnection(DB_PATH);
             var testdata = dbConn.Query<Task>("select * from task where id='" + 1 + "'").FirstOrDefault();
             // Check result is empty or not
             if (testdata == null)
                 MessageBox.Show("id Not Present in DataBase");
             else
             {

                 var tp = dbConn.Query<Task>("update task set Scoreid='" + scoreid  + "' where Username = '" +testdata.Username  + "'").FirstOrDefault();
                 // Update Database
                 dbConn.Update(tp);
                
             }
             NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));


         });
        }
    }
}