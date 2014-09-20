using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using com.shephertz.app42.gaming.multiplayer.client;


namespace Movie
{
    
    public partial class Question : PhoneApplicationPage
    {
        System.IO.IsolatedStorage.IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        
        System.Windows.Threading.DispatcherTimer MainTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer BackKeyTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer BonusTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer AnsDispTimer = new System.Windows.Threading.DispatcherTimer();
            
        public int flag;
        int BonusTimerCounter, mainTimerCounter, BackTimerCounter, AnswerTimerCounter, ans;
        System.Windows.Shapes.Rectangle[] r = new System.Windows.Shapes.Rectangle[4];
            
        int cMax = 7;
        public int warpflag = 0;
        
        public Question()
        {
            InitializeComponent();           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            p1.Value = 0;
            p2.Value = 0;
            Global.p1Score = 0;
            Global.p2Score = 0;

            flag = 0;
            mainTimerCounter = cMax;
            BackTimerCounter = 3;
            AnswerTimerCounter = 1;
            BonusTimerCounter = Global.MyAudio[0].NaturalDuration.TimeSpan.Seconds;
            clock.Text = "";
            BonusMain.Text = "";

            Quit.Visibility = System.Windows.Visibility.Collapsed;

            BonusTimer.Interval = TimeSpan.FromSeconds(1);
            BonusTimer.Tick += BonusTimerTick;
            BonusTimer.Start();

            MainTimer.Interval = TimeSpan.FromSeconds(1);
            MainTimer.Tick += OnMainTimerTick;

            BackKeyTimer.Interval = TimeSpan.FromSeconds(1);
            BackKeyTimer.Tick += OnBackKeyTimerTick;

            AnsDispTimer.Interval = TimeSpan.FromSeconds(1);
            AnsDispTimer.Tick += OnAnswerDisplayTick;

            System.Diagnostics.Debug.WriteLine("navigated to question");

            r[0] = Op1;
            r[1] = Op2;
            r[2] = Op3;
            r[3] = Op4;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                myname.Text = Global.localUsername;
                opponent.Text = Global.opponentUsername;
            });


            Global.notificationListenerObj = new NotificationListener(this);
            Global.warpClient.AddNotificationListener(Global.notificationListenerObj);
            System.Diagnostics.Debug.WriteLine("add notif listener from Q");
            Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "ready" + (Global.PlayerIsFirst ? 0 : 1), false } }, null);
            System.Diagnostics.Debug.WriteLine("update ready = false for first question");
        }

        public void B(bool x)
        {
            bt0.IsEnabled = x;
            bt1.IsEnabled = x;
            bt2.IsEnabled = x;
            bt3.IsEnabled = x;
        }

        public int score(int clipLength, int timer1Reading, int timer2Reading, int selected)
        {
            double bonus = 0.0, score = 0.0;
            
            bool isCorrect = (selected == ans);

            if (timer1Reading > 0 && timer2Reading == 7)
            {
                bonus = isCorrect ? (((1.0 + timer1Reading) / clipLength) * 20.0) + 10.0 : (((1.0 + timer1Reading) / clipLength)) * (-10.0);
            }
            else if (timer2Reading <= 6 && timer1Reading == 0)
            {
                score += isCorrect ? (((timer2Reading - 1) / 5.0) * 10.0) + 10.0 : 0;
            }
            score += bonus;

            return (int)Math.Round(score);
        }

        public void tempF(int num)
        {
            Global.MyAudio[flag].Stop();
            B(false);
            AnswerTimerCounter = 1;
            int cliplength = Global.MyAudio[flag].NaturalDuration.TimeSpan.Seconds;

            MainTimer.Stop();
            BonusTimer.Stop();

            int scoreUpdate = score(cliplength, BonusTimerCounter, mainTimerCounter, num);

            p1.Value = Global.p1Score + scoreUpdate;
            Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "score" + (Global.PlayerIsFirst ? 0 : 1), p1.Value } }, null);
            System.Diagnostics.Debug.WriteLine("score update request sent from " + (p1.Value-scoreUpdate) + " to " + p1.Value);
            //p1.Value = Global.p1Score;
            //p2.Value = Global.p2Score;
            //p2.Value = p2.Value + new Random().Next(24,31);
            
    
            AnsDispTimer.Start();

            if (num == ans)
            {
                r[num].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            }
            else
            {
                r[ans].Fill.Opacity = 100;
                r[ans].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                
                r[num].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
            r[num].Fill.Opacity = 100;

            mainTimerCounter = cMax;
            
        }

        private void but1_Click(object sender, RoutedEventArgs e)
        {
            tempF(0);
        }

        private void but2_Click(object sender, RoutedEventArgs e)
        {
            tempF(1);
        }
        
        private void but3_Click(object sender, RoutedEventArgs e)
        {
            tempF(2);
        }
        
        private void but4_Click(object sender, RoutedEventArgs e)
        {
            tempF(3);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            BackKeyTimer.Stop();
            BackTimerCounter = 3;
            Quit.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel.Visibility = System.Windows.Visibility.Visible;
            if (BonusTimerCounter <= 0)
                MainTimer.Start();
            else
                BonusTimer.Start();
                 

        }

        private void Forfeit_Click(object sender, RoutedEventArgs e)
        {
            BackKeyTimer.Stop();
            Global.MyAudio[0].Source = null;
            Global.MyAudio[1].Source = null;
            Global.MyAudio[2].Source = null;
            Global.MyAudio[3].Source = null;
            Global.MyAudio[4].Source = null;
            System.Diagnostics.Debug.WriteLine("forfeited");
            NavigationService.Navigate(new Uri("/Results.xaml", UriKind.Relative));    
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {

            ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
            Quit.Visibility = System.Windows.Visibility.Visible;
            BackTimerCounter = 3;

            if (BonusTimerCounter <= 0)
                MainTimer.Stop();
            else
                BonusTimer.Stop();
            
            BackKeyTimer.Start();

            if (BackTimerCounter == 3) e.Cancel = true;

        }

        void OnBackKeyTimerTick(Object sender, EventArgs args) // back
        {
            BackTimerCounter--;
            if (BackTimerCounter == 0)
            {
                System.Diagnostics.Debug.WriteLine("back key timed out - goin to main page");
                BackKeyTimer.Stop();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            }
        }
        void  BonusTimerTick(Object sender, EventArgs args) // Bonus
        {
            
            BonusTimerCounter--;

            if (BonusTimerCounter == (Global.MyAudio[flag].NaturalDuration.TimeSpan.Seconds-1))
            {
                System.Diagnostics.Debug.WriteLine("bonus timer running");
                BonusMain.Text = "Bonus";

                Global.MyAudio[flag].Stop();
                Global.MyAudio[flag].Play();


                ans = Convert.ToInt16(Global.s[flag, 5]);

                tb0.Text = Global.s[flag, 1];
                tb1.Text = Global.s[flag, 2];
                tb2.Text = Global.s[flag, 3];
                tb3.Text = Global.s[flag, 4];

                B(true);

            }
            
            if (BonusTimerCounter > 0 && BonusTimerCounter <= Global.MyAudio[flag].NaturalDuration.TimeSpan.Seconds)
            {
                clock.Text = BonusTimerCounter.ToString();
            }
            else if (BonusTimerCounter == 0)
            {
                clock.Text = BonusTimerCounter.ToString();
                BonusTimer.Stop();
                mainTimerCounter = cMax;
                MainTimer.Start();
                System.Diagnostics.Debug.WriteLine("bonus timer ended");
            }
        }

        public void navigateR()
        {
            System.Diagnostics.Debug.WriteLine("func - go to results");
            Global.warpClient.RemoveNotificationListener(Global.notificationListenerObj);
            NavigationService.Navigate(new Uri("/Results.xaml", UriKind.Relative));
        }

        void OnAnswerDisplayTick(Object sender, EventArgs args) // answer display
        {       
            AnswerTimerCounter--;
            if (AnswerTimerCounter == 0)
            {
                System.Diagnostics.Debug.WriteLine("answer timer ended");
                for (int i = 0; i < 4; i++)
                {
                    r[i].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    r[i].Fill.Opacity = 0;
                }

                mainTimerCounter = cMax;

                if (flag == 4)
                {
                    
                    flag = 0;
                    Global.MyAudio[0].Source = null;
                    Global.MyAudio[1].Source = null;
                    Global.MyAudio[2].Source = null;
                    Global.MyAudio[3].Source = null;
                    Global.MyAudio[4].Source = null;

                    Global.p1Score = (int)p1.Value;
                    Global.p2Score = (int)p2.Value;

                    Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "endgame" + (Global.PlayerIsFirst ? 0 : 1), true } }, null);
                    System.Diagnostics.Debug.WriteLine("game ended update sent");

                }
                else
                {
                    Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "ready" + (Global.PlayerIsFirst ? 0 : 1), true } }, null);
                    System.Diagnostics.Debug.WriteLine("ready for next round");
                    if(flag==0)
                    {
                        Global.MyAudio[flag].Source = null;
                        Global.MyAudio[2].Stop();
                        Global.MyAudio[2].Source = new Uri(Global.links[2], UriKind.Absolute);
                        Global.MyAudio[2].Stop();
                        System.Diagnostics.Debug.WriteLine("clear 0 buffer 2");
                    }
                    if (flag == 1)
                    {
                        Global.MyAudio[flag].Source = null;
                        Global.MyAudio[3].Stop();
                        Global.MyAudio[3].Source = new Uri(Global.links[3], UriKind.Absolute);
                        Global.MyAudio[3].Stop();
                        System.Diagnostics.Debug.WriteLine("clear 1 buffer 3");
                    }
                    if (flag == 2)
                    {
                        Global.MyAudio[flag].Source = null;
                        Global.MyAudio[4].Stop();
                        Global.MyAudio[4].Source = new Uri(Global.links[4], UriKind.Absolute);
                        Global.MyAudio[4].Stop();
                        System.Diagnostics.Debug.WriteLine("clear 2 buffer 4");
                    }

                    //flag++;
                    //RoundNum.Text = (flag + 1).ToString();
                    
                    //clock.Text = "";

                    //BonusTimerCounter = Global.MyAudio[flag].NaturalDuration.TimeSpan.Seconds;

                    //BonusTimer.Start();
                   
                    flag++;
                    //milk();
                
                }

            }
        }

        public void milk()
        {

            Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "ready" + (Global.PlayerIsFirst ? 0 : 1), false } }, null);
            System.Diagnostics.Debug.WriteLine("update that you are not ready from milk :P");
            
            RoundNum.Text = (flag + 1).ToString();

            clock.Text = "";

            BonusTimerCounter = Global.MyAudio[flag].NaturalDuration.TimeSpan.Seconds;

            BonusTimer.Start();
        }

        void OnMainTimerTick(Object sender, EventArgs args)
        {
            mainTimerCounter--;
            if (mainTimerCounter == (cMax - 1))
            {
                BonusMain.Text = "Main";
                System.Diagnostics.Debug.WriteLine("main timer start");
                //Global.MyAudio[flag].Stop();

                ans = Convert.ToInt16(Global.s[flag, 5]);

                tb0.Text = Global.s[flag, 1];
                tb1.Text = Global.s[flag, 2];
                tb2.Text = Global.s[flag, 3];
                tb3.Text = Global.s[flag, 4];

            }


            if (mainTimerCounter > 6) { clock.Text = "5"; }

            if (mainTimerCounter > 0 && mainTimerCounter < 7)
            {
                clock.Text = (mainTimerCounter - 1).ToString();
            }
            else if (mainTimerCounter == 0)
            {
                MainTimer.Stop();
                System.Diagnostics.Debug.WriteLine("main timer stop");

                Global.MyAudio[flag].Stop();
                B(false);
                r[ans].Fill.Opacity = 100;
                r[ans].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                AnswerTimerCounter = 1;
                AnsDispTimer.Start();
                //Global.warpClient.UpdateRoomProperties(Global.DynRoomId, new Dictionary<string, object>() { { "score" + (Global.PlayerIsFirst ? 0 : 1), p1.Value } }, null);
                System.Diagnostics.Debug.WriteLine("NO :P update score from main timer timeout");
            }
            
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            Global.MyAudio[flag].Stop();

            Global.MyAudio[flag].Play();
        }

    }

}