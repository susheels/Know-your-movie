using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using System.Windows;
using System.IO.IsolatedStorage;


namespace Movie
{
    class ConnectionListener: com.shephertz.app42.gaming.multiplayer.client.listener.ConnectionRequestListener
    {
        private Loading _page;
        int attempts = 0;

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public ConnectionListener(Loading result)
        {
            _page = result;
        }

        public void onConnectDone(ConnectEvent eventObj)
        {
            // instead of all the else statements
            Global.connectSuccess = false;
            /*-----------------------     SUCCESS IN CONNECTING     -----------------------*/
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Global.connectSuccess = true;
                attempts = 0;
                System.Diagnostics.Debug.WriteLine("conn successful");
                _page.timeout = 0;  
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Connection Successful :)");
                });
                Global.warpClient.JoinRoomWithProperties(new Dictionary<string, object>() { { "category", Global.itemName[(int)settings["trigger"]] }, { "availUsers", 1 } });
                System.Diagnostics.Debug.WriteLine("trying to join room");
            }
            /*-----------------------     RECOVERABLE CONNECTION ERROR     -----------------------*/
            else if (eventObj.getResult() == WarpResponseResultCode.CONNECTION_ERROR_RECOVERABLE)
            {
                Global.warpClient.RecoverConnection();

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("recoverable connection error");

                });
                System.Diagnostics.Debug.WriteLine("fail scenes");
            }
            /*-----------------------     RECOVERY SUCCESS     -----------------------*/
            else if (eventObj.getResult() == WarpResponseResultCode.SUCCESS_RECOVERED)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("connection error recovered");

                });
                System.Diagnostics.Debug.WriteLine("fail scenes");
            }
            /*-----------------------     CONNECTION FAILED      -----------------------*/
            else if(eventObj.getResult() == WarpResponseResultCode.CONNECTION_ERR)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("connection error");

                });
                System.Diagnostics.Debug.WriteLine("fail scenes");
                if(attempts<2)
                {
                    Global.warpClient.Connect(Global.localUsername);
                }
                attempts++;
                
            }
        }

        public void onDisconnectDone(ConnectEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Global.disconnectSuccess = true;

                System.Diagnostics.Debug.WriteLine("successfully disconnected");
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Disconnect Done :)");

                });
               // _page.showresult("disconnection success");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("not successfully disconnected");
                //Global.disconnectSuccess = true;
               // _page.showresult("diconnection failed");
            }
        }


        public void onInitUDPDone(byte resultCode)
        {
            //throw new NotImplementedException();
        }
    }
}
