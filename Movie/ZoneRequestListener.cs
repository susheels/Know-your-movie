using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using System.Windows;

namespace Movie
{
    class ZoneRequestListener: com.shephertz.app42.gaming.multiplayer.client.listener.ZoneRequestListener
    {
        private Loading mainPage;

        public ZoneRequestListener(Loading mainPage)
        {
            // TODO: Complete member initialization
            this.mainPage = mainPage;
        }
        /// Invoked when a response for DeleteRoom request is receieved.
        /// <param name="eventObj"></param>
        public void onDeleteRoomDone(RoomEvent eventObj)
        {
            Global.deleteSuccess = true;

            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {

                System.Diagnostics.Debug.WriteLine("zone listener - room deleted");
            
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Yay! Deleted room :)");
                });
            }
        }
        /// Invoked when a response for GetAllRooms request is receieved.
        /// <param name="eventObj"></param>
        public void onGetAllRoomsDone(AllRoomsEvent eventObj) { ; }
        /// Invoked when a response for CreateRoom request is receieved.
        /// <param name="eventObj"></param>
        public void onCreateRoomDone(RoomEvent eventObj)
        {
            System.Diagnostics.Debug.WriteLine("room created");
            
            Global.warpClient.JoinRoom(eventObj.getData().getId());
        }
        /// Invoked when a response for GetOnlineUsers request is receieved.
        /// <param name="eventObj"></param>
        public void onGetOnlineUsersDone(AllUsersEvent eventObj) { ; }
        /// <summary>
        /// Invoked when a response for GetLiveUserInfo request is receieved.
        /// <param name="eventObj"></param>
        public void onGetLiveUserInfoDone(LiveUserInfoEvent eventObj) { ; }
        /// Invoked when a response for SetCustomUserData request is receieved.
        /// <param name="eventObj"></param>
        public void onSetCustomUserDataDone(LiveUserInfoEvent eventObj) { ; }
        /// Invoked when a response from GetRoomWithNUser and GetRoomWithProperties 
        /// <param name="matchedRoomsEvent"></param>
        public void onGetMatchedRoomsDone(MatchedRoomsEvent matchedRoomsEvent)
        {
            if (matchedRoomsEvent.getResult() == WarpResponseResultCode.SUCCESS)
            {
                /* Deployment.Current.Dispatcher.BeginInvoke(() =>
                 {
                     MessageBox.Show(matchedRoomsEvent.getRoomsData);
                 });*/
            }
        }
    }
}
