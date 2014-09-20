using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using System.Windows;

namespace Movie
{
    class NotificationListener:com.shephertz.app42.gaming.multiplayer.client.listener.NotifyListener
    {
        private Loading _page;

        public NotificationListener(Loading page)
        {
            System.Diagnostics.Debug.WriteLine("notif listener from loading");
            
            _page = page;
        }

        
        private Question _page2;

        public NotificationListener(Question page)
        {
            System.Diagnostics.Debug.WriteLine("notif listener from question");
        
            _page2 = page;
            
        }

        private Results _page3;

        public NotificationListener(Results page)
        {
            System.Diagnostics.Debug.WriteLine("notif listener from results");
        
            _page3 = page;

        }
        public void onRoomCreated(RoomData eventObj)
        {
            System.Diagnostics.Debug.WriteLine("notif listener  - room created");
        
        }

        public void onRoomDestroyed(RoomData eventObj)
        {
            System.Diagnostics.Debug.WriteLine("notif listener - room destroyed");
        
        }

        public void onUserLeftRoom(RoomData eventObj, String username)
        {
            if (Global.localUsername != username)
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    System.Diagnostics.Debug.WriteLine("someone left");
                });
        }

        public void onUserJoinedRoom(RoomData eventObj, String username)
        {
            System.Diagnostics.Debug.WriteLine(username + " joined room");
            /*
            if(!username.Equals(Global.localUsername))
            {
                Global.opponentUsername = username;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _page.ready.Text = Global.localUsername + " \nvs\n" + Global.opponentUsername;
                });
                        
            }
            else if(!eventObj.getRoomOwner().Equals(Global.localUsername))
            {
                Global.opponentUsername = eventObj.getRoomOwner();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _page.ready.Text = Global.localUsername + " \nvs\n" + Global.opponentUsername;
                });

            }
            */
        }

        public void onUserLeftLobby(LobbyData eventObj, String username)
        {

        }

        public void onUserJoinedLobby(LobbyData eventObj, String username)
        {

        }

        public void onChatReceived(ChatEvent eventObj)
        {

        }

        public void onUpdatePeersReceived(UpdateEvent eventObj)
        {

        }

        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties)
        {

        }


        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
        {
            
            var result = Global.p.All(x => properties.Any(y => x.Value == y.Value));
            if(!Convert.ToBoolean(result))
            {
                string text1 = String.Join(":", Global.p.Select(x => String.Format("{0}", x.Value)).ToArray());
                string text2 = String.Join(":", properties.Select(x => String.Format("{0}", x.Value)).ToArray());

                System.Diagnostics.Debug.WriteLine("property change detected\n"+ text1 +"\n" + text2);
                
            }
            Global.p = properties;
            if (_page != null)
            {
                if (_page3 != null) { _page3 = null; }
                if (_page2 != null) { _page2 = null; }

                if (Convert.ToInt16(properties["availUsers"]) == 2)
                {
                    _page.timeout = -100;
                }

                if (Convert.ToBoolean(properties["ready0"]) && Convert.ToBoolean(properties["ready1"]))
                {
                    System.Diagnostics.Debug.WriteLine("both are ready");
        
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        //_page.ready.Text = Global.localUsername + " \nvs\n" + Global.opponentUsername;
                        _page.but.Visibility = Visibility.Visible;
                        _page.prog.Visibility = Visibility.Collapsed;
                    });

                }
            }
            
            if (_page2 != null)
            {
                if (_page != null) { _page = null; }
                if (_page3 != null) { _page3 = null; }
                if (Convert.ToBoolean(properties["ready0"]) && Convert.ToBoolean(properties["ready1"]))
                {
                    System.Diagnostics.Debug.WriteLine("both ready for next question");
        
                    Global.p1Score = Convert.ToInt16(properties["score" + (Global.PlayerIsFirst ? 0 : 1)]);
                    Global.p2Score = Convert.ToInt16(properties["score" + (Global.PlayerIsFirst ? 1 : 0)]);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        _page2.p1.Value = Global.p1Score;
                        _page2.p2.Value = Global.p2Score;
                       // System.Threading.Thread.Sleep(1000);
                    });
                    System.Diagnostics.Debug.WriteLine("score updated on UI");
        
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {                       
                        _page2.milk();
                        System.Diagnostics.Debug.WriteLine("milk called");
        
                    });


                }

                if (Convert.ToBoolean(properties["endgame0"]) && Convert.ToBoolean(properties["endgame1"]))
                {
                    System.Diagnostics.Debug.WriteLine("both have ended");
        
                    Global.p1Score = Convert.ToInt16(properties["score" + (Global.PlayerIsFirst ? 0 : 1)]);
                    Global.p2Score = Convert.ToInt16(properties["score" + (Global.PlayerIsFirst ? 1 : 0)]);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        _page2.p1.Value = Global.p1Score;
                        _page2.p2.Value = Global.p2Score;
                        
                    });
                    _page2.flag = 0;
                    System.Diagnostics.Debug.WriteLine("score ui updated at end");
        
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        _page2.navigateR();
                    });
                    System.Diagnostics.Debug.WriteLine("navigating to results");
        

                }

            }

            if(_page3 != null)
            {
                if (_page2 != null) { _page2 = null; }
                if (_page != null) { _page = null; }

              // sup
                
            }

        }

        public void onPrivateChatReceived(string sender, string message)
        {
            //throw new NotImplementedException();
        }

        public void onMoveCompleted(MoveEvent moveEvent)
        {
            //throw new NotImplementedException();
        }

        public void onUserPaused(string locid, bool isLobby, string username)
        {
            //throw new NotImplementedException();
        }

        public void onUserResumed(string locid, bool isLobby, string username)
        {
            //throw new NotImplementedException();
        }

        public void onGameStarted(string sender, string roomId, string nextTurn)
        {
            //throw new NotImplementedException();
        }

        public void onGameStopped(string sender, string roomId)
        {
            //throw new NotImplementedException();
        }

        public void onPrivateUpdateReceived(string sender, byte[] update, bool fromUdp)
        {
            //throw new NotImplementedException();
        }
    }
}
