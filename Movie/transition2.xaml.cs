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

namespace Movie
{
    public partial class transition2 : PhoneApplicationPage,App42Callback
    {
        UserService userService = App42API.BuildUserService();
        string win;
        string games;
        bool flag;
        public transition2()
        {
           
            InitializeComponent();
            App42API.Initialize(Constants.API_KEY, Constants.SECRET_KEY);
            userService.GetUser(Global.localUsername, this);
            flag = true;
            
        }

        public void OnException(App42Exception exception)
        {
            throw new NotImplementedException();
        }

        public void OnSuccess(object response)
        {
            User user = (User)response;
            if (flag)
            {
                win =  user.GetProfile().GetLine1();
                games =  user.GetProfile().GetLine2();

                games = Convert.ToInt32((games) + 1) + "";
                User.Profile profile = new User.Profile(user);

                profile.SetLine2(games);  

                if (Global.p1Score > Global.p2Score)
                {

                    win = (Convert.ToInt32(win) + 1) + "";
                    
                    profile.SetLine1(win);
                }
                

                // "wins is : " + user.GetProfile().GetLine1();
                //  games playes is  user.GetProfile().SetLine2();

                flag = false;
                userService.CreateOrUpdateProfile(user, this);  

            }
        }
    }
}