
using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.game;
using com.shephertz.app42.paas.sdk.windows.storage;
using com.shephertz.app42.paas.sdk.windows.upload;
using com.shephertz.app42.paas.sdk.windows.user;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using com.shephertz.app42.gaming.multiplayer.client;


namespace Movie
{

    public partial class Loading : PhoneApplicationPage, App42Callback
    {

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        UploadService uploadService = null;
        StorageService storage;

        TextBlock[] MediaElementData = new TextBlock[2];

        int i,k;

        public int[] sel = new int[5];

        App42Callback requestCallback;

        private DispatcherTimer _dispatcherTimer;

        public int timeout = 0;
        
        public Loading()
        {
            InitializeComponent();

            Global.deleteSuccess = false;
            Global.disconnectSuccess = false;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("On navigated to loading");
            timeout = 0;

            MediaElementData[0] = t1;
            MediaElementData[1] = t2;

            Global.MyAudio[0] = App.GlobalMediaElement0;
            Global.MyAudio[1] = App.GlobalMediaElement1;
            Global.MyAudio[2] = App.GlobalMediaElement2;
            Global.MyAudio[3] = App.GlobalMediaElement3;
            Global.MyAudio[4] = App.GlobalMediaElement4;

            Global.opponentUsername = "";
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ready.Text = Global.localUsername + " \nvs\n";
            });

            _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _dispatcherTimer.Tick += UpdateMediaData;
            _dispatcherTimer.Start();

            i = 0;
            k = 0;
            B(false);

            requestCallback = this;
            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            App42Log.SetDebug(true);

            ServiceAPI api = new ServiceAPI(Constants.API_KEY, Constants.SECRET_KEY);

            storage = api.BuildStorageService();
            uploadService = api.BuildUploadService();

            System.Diagnostics.Debug.WriteLine("Before warp");

            WarpClient.initialize(Constants.API_KEY, Constants.SECRET_KEY);

            WarpClient.setRecoveryAllowance(60);

            Global.warpClient = WarpClient.GetInstance();


            //trivia
            if ((int)settings["trigger"] == 0)
                TriviaTB.Text = Global.TriviaB[(new Random()).Next(Global.TriviaB.Length)];
            else
                TriviaTB.Text = Global.TriviaH[(new Random()).Next(Global.TriviaH.Length)];

            Global.disconnectSuccess = false;
            Global.deleteSuccess = false;
            Global.conListenObj = new ConnectionListener(this);
            Global.roomReqListenerObj = new RoomReqListener(this);
            Global.zoneReqListenerObj = new ZoneRequestListener(this);
            Global.notificationListenerObj = new NotificationListener(this);
     
            Global.warpClient.AddConnectionRequestListener(Global.conListenObj);
            Global.warpClient.AddRoomRequestListener(Global.roomReqListenerObj);
            Global.warpClient.AddNotificationListener(Global.notificationListenerObj);
            Global.warpClient.AddZoneRequestListener(Global.zoneReqListenerObj);

            Global.warpClient.Connect(Global.localUsername);

            System.Diagnostics.Debug.WriteLine("after warp");
        }

        private void UpdateMediaData(object sender, EventArgs e)
        {
            Global.MyAudio[0].Pause();
            Global.MyAudio[1].Pause();
            Global.MyAudio[2].Pause();
            Global.MyAudio[3].Pause();
            Global.MyAudio[4].Pause();

            timeout++; 

            int p = 0;
            for (int a = 0; a < 2; a++)
            {
                if (Global.MyAudio[a].Source == null) ;

                string result = "BUFFER\n";
                result += "progress:" + Global.MyAudio[a].DownloadProgress + "\n";
                result += "State:" + Global.MyAudio[a].CurrentState + "\n";
                result += "Duration:" + Math.Round(Global.MyAudio[a].NaturalDuration.TimeSpan.TotalSeconds) + "\n";
                p += (Global.MyAudio[a].NaturalDuration.TimeSpan.TotalSeconds > 0) ? 1 : 0;
                MediaElementData[a].Text = result;
            }
            if(p==2)
            {
                System.Diagnostics.Debug.WriteLine("p=2");
                Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "ready" + (Global.PlayerIsFirst ? 0 : 1), true } }, null);
                _dispatcherTimer.Stop();
            }
            if (timeout == 30)
            {
                System.Diagnostics.Debug.WriteLine("timeout");
                Global.MyAudio[0].Source = null;
                Global.MyAudio[1].Source = null;
                Global.MyAudio[2].Source = null;
                Global.MyAudio[3].Source = null;
                Global.MyAudio[4].Source = null;

                if (Global.roomJoined)
                    Global.warpClient.LeaveRoom(Global.DynRoomId);
                else
                    Global.warpClient.Disconnect();

            }
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


                System.Diagnostics.Debug.WriteLine("exit due to time out");
                _dispatcherTimer.Stop();

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                
            }
        }
        public void f()
        {
            string key = "qid";
            if (i < 5)
            {
                storage.FindDocumentByKeyValue(Constants.dbName, Constants.collectionName[(int)settings["trigger"]], key, (sel[i] + ""), requestCallback);
                System.Diagnostics.Debug.WriteLine("getting question");
            }
            else if (i == 5)
            {
                if (k == 5)
                {
                    B(true);
                    System.Diagnostics.Debug.WriteLine("got all");
                }
            }
        }

        public void g()
        {
            int op = (int)settings["trigger"];
            if (k < 5)
            {
                System.Diagnostics.Debug.WriteLine("getting clips");
                if(op == 0)
                    uploadService.GetFileByName("B" + sel[k], this);
                else
                    uploadService.GetFileByName("H" + sel[k], this);
            }
            else if (k == 5)
            {
                if (i == 5)
                {
                    B(true);
                    System.Diagnostics.Debug.WriteLine("got all c");
                }
            }
        }

        public void B(bool x)
        {
           /* if(x)
            {
                if (Constants.Buff > 2)
                {
                    but.Visibility = Visibility.Visible;
                    Constants.Buff = 0;
                }
            }
            else
            {
                but.Visibility = Visibility.Collapsed;
            }*/
        }
        
        public void OnSuccess(Object response)
        {

            if ((response.GetType().ToString()).EndsWith("Storage"))
            {
                System.Diagnostics.Debug.WriteLine("received question " + i);
                Storage storage = (Storage)response;
                IList<Storage.JSONDocument> jsonDocList = storage.GetJsonDocList();
                Global.temp = jsonDocList[0].GetJsonDoc();
                string jsonString = @"{    'paramss': [" + Global.temp + "]  }";

                RootObject childlist = new RootObject();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(childlist.GetType());
                childlist = ser.ReadObject(ms) as RootObject;

                foreach (var d in childlist.paramss)
                {
                    Global.s[i, 0] = "" + d.qid;
                    Global.s[i, 1] = d.o0;
                    Global.s[i, 2] = d.o1;
                    Global.s[i, 3] = d.o2;
                    Global.s[i, 4] = d.o3;
                    Global.s[i, 5] = d.ans + "";
                }
                ms.Close();
                System.Diagnostics.Debug.WriteLine("processing done - q" + i);
                i++;
                f();
            }

            else //if ((response.GetType().ToString()).EndsWith("Upload"))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    System.Diagnostics.Debug.WriteLine("received clip url " + k);
                    Upload upload = (Upload)response;
                    IList<Upload.File> fileList = upload.GetFileList(); // This will have only single file fetched as user can have only one file with given name   

                    Global.links[k] = upload.GetFileList()[0].GetUrl().ToString();  //This will return file url info 
                     
                    if(k < 2)
                    {
                        System.Diagnostics.Debug.WriteLine("started clip " + k);
                        Global.MyAudio[k].Stop();
                        Global.MyAudio[k].Source = new Uri(Global.links[k], UriKind.Absolute);
                        Global.MyAudio[k].Stop();
                    }

                    /*Global.MyAudio[k].Stop();

                    string MediaUrl = "/Audio/B"+k+".mp3";
                    Global.MyAudio[k].Source = new Uri(MediaUrl, UriKind.RelativeOrAbsolute);

                    Global.MyAudio[k].Play();
                    Global.MyAudio[k].Stop();*/

                    k++;
                    g();

                });
            }

        }

  
        
        public void OnException(App42Exception exception)
        {
            Global.temp = exception.Message;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            simple.Visibility = System.Windows.Visibility.Collapsed;
            Quit.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("lets go - button click");
            Global.warpClient.RemoveNotificationListener(Global.notificationListenerObj);
            _dispatcherTimer.Stop();
            NavigationService.Navigate(new Uri("/Question.xaml", UriKind.Relative));
        }

        private void Forfeit_Click(object sender, RoutedEventArgs e)
        {
            Global.MyAudio[0].Source = null;
            Global.MyAudio[1].Source = null;
            Global.MyAudio[2].Source = null;
            Global.MyAudio[3].Source = null;
            Global.MyAudio[4].Source = null;

            if (Global.roomJoined)
            {
                Global.warpClient.LeaveRoom(Global.DynRoomId);
                Global.roomJoined = false;
            }
            else if (Global.connectSuccess)
                Global.warpClient.Disconnect();
            else
                Global.disconnectSuccess = true;

            System.Diagnostics.Debug.WriteLine("exit through back key");
            
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Quit.Visibility = System.Windows.Visibility.Collapsed;
            simple.Visibility = System.Windows.Visibility.Visible;
        }

    }
    public class Paramss
    {
        public int qid { get; set; }
        public string o0 { get; set; }
        public string o1 { get; set; }
        public string o2 { get; set; }
        public string o3 { get; set; }
        public int ans { get; set; }
    }

    public class RootObject
    {
        
        public List<Paramss> paramss { get; set; }
    }
   
}