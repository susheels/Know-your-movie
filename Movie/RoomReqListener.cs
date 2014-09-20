using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Windows;


using System.IO.IsolatedStorage;

namespace Movie
{
    class RoomReqListener: com.shephertz.app42.gaming.multiplayer.client.listener.RoomRequestListener
    {

        bool flag = true;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private Loading _page;

        public RoomReqListener(Loading page)
        {
            System.Diagnostics.Debug.WriteLine("listener from loading");
            
            _page = page;
            
        }

        private Question _page2;

        public RoomReqListener(Question page)
        {
            System.Diagnostics.Debug.WriteLine("Listener from question");
            
            _page2 = page;
            
        }

        private Results _page3;

        public RoomReqListener(Results page)
        {
            System.Diagnostics.Debug.WriteLine("listener from result");
            
            _page3 = page;

        }

        public void onSubscribeRoomDone(RoomEvent eventObj)
        {
            System.Diagnostics.Debug.WriteLine("subscribe room");
            
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Global.warpClient.GetLiveRoomInfo(Global.DynRoomId);
            }
        }

        public void onUnSubscribeRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
              //  _page.showresult("Yay! UnSubscribe room :)");
            }
        }

        public void onJoinRoomDone(RoomEvent eventObj)
        {

            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Global.roomJoined = true;

                System.Diagnostics.Debug.WriteLine("joined room");
            
                Global.DynRoomId = eventObj.getData().getId();
                Global.warpClient.SubscribeRoom(Global.DynRoomId);
                
                if(!Global.PlayerIsFirst)
                    Global.opponentUsername = eventObj.getData().getRoomOwner();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _page.ready.Text = Global.localUsername + " \nvs\n" + Global.opponentUsername;
                });

            }
            else if (eventObj.getResult() == WarpResponseResultCode.RESOURCE_NOT_FOUND)
            {
                System.Diagnostics.Debug.WriteLine("room not found .. hence creating");
                Global.PlayerIsFirst = true;
                Global.dictionary = new Dictionary<string,object>();
                Global.dictionary.Add("category", Global.itemName[(int)settings["trigger"]]);
                Global.dictionary.Add("availUsers", 0);
                Global.dictionary.Add("score0", 0);
                Global.dictionary.Add("score1", 0);
                Global.dictionary.Add("status", "notyetstarted");
                
                Global.dictionary.Add("ready0", false);
                Global.dictionary.Add("ready1", false);
                
                // first for buffering - later per question

                Global.dictionary.Add("endgame0", false);
                Global.dictionary.Add("endgame1", false);

                int[] temp = randomize(Global.maxClips[(int)settings["trigger"]]);

                for (int i = 0; i < 5; i++ )
                {
                    Global.dictionary.Add("q" + (i+1), temp[i]);
                }


                Global.warpClient.CreateRoom(Global.localUsername, Global.localUsername, 2, Global.dictionary);
                System.Diagnostics.Debug.WriteLine("request to create room");
            
            }
            // Global.warpClient.LeaveRoom(eventObj.getData().getId());
        }

        public int[] randomize(int cMAX)
        {
            Random rnd = new Random();
            int n = 0;
            int x;
            String s = "";

            while (n < 5)
            {
                x = rnd.Next(cMAX);
                if (!(s.Contains(x + " ")))
                {
                    s = s + x + " ";
                    n++;
                }
            }
            s.Trim();
            string[] temp = s.Split(' ');
            int[] ar = new int[5];
            for (int zz = 0; zz < 5; zz++)
                ar[zz] = Convert.ToInt32(temp[zz]);
            return ar;
        }

        public void onLeaveRoomDone(RoomEvent eventObj)
        {

            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Global.deleteSuccess = true;

                System.Diagnostics.Debug.WriteLine("left room");
            
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Yay! Leave room :)");
                });
            }
        }

        public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            System.Diagnostics.Debug.WriteLine("getliveinfodone");
            
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS && (eventObj.getJoinedUsers() != null))
            {
                System.Diagnostics.Debug.WriteLine("found room .. reading properties");
            
                for (int i = 0; i < 5; i++)
                {
                    if (eventObj.getProperties().ContainsKey("q" + (i + 1)))
                        Global.ques[i] = eventObj.getProperties()["q" + (i + 1)].ToString();
                }
                if (eventObj.getJoinedUsers().Length == 1)
                {
                    System.Diagnostics.Debug.WriteLine("is player 1");
            
                    Global.PlayerIsFirst = true;
                    Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "availUsers", 1 } }, null);
                    System.Diagnostics.Debug.WriteLine("AVAIL  = 1");
            
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("is player 2\n AVAIL = 2");
            
                    // Global.warpClient.UpdateRoomProperties(eventObj.getData().getId());
                    Global.PlayerIsFirst = false;
                    Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "availUsers", 2 } }, null);
                }

                if(flag)
                {
                     for (int zz = 0; zz < 5; zz++)
                     {
                         _page.sel[zz] = Convert.ToInt16(Global.ques[zz]);
                     }
                     System.Diagnostics.Debug.WriteLine("retrieved all questions -  sent to sel - going to g and f");
            
                     _page.g();
                     _page.f();
                     flag = false;
                }
                

                // navigate to game play screen  
                // _page.moveToPlayScreen();
            }
        }
        public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj)
        {
        }
        public void onUpdatePropertyDone(LiveRoomInfoEvent lifeLiveRoomInfoEvent)
        {
            if (lifeLiveRoomInfoEvent.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Dictionary<string, object> d = lifeLiveRoomInfoEvent.getProperties();
                System.Diagnostics.Debug.WriteLine("property successfully updated by you");
            
                
            }
        }


        public void onLockPropertiesDone(byte result)
        {
            //throw new NotImplementedException();
        }

        public void onUnlockPropertiesDone(byte result)
        {
            //throw new NotImplementedException();
        }
    }
}
