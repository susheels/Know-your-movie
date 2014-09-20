using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Movie
{
    public partial class Question5 : PhoneApplicationPage
    {
        System.Windows.Threading.DispatcherTimer newTimer = new System.Windows.Threading.DispatcherTimer();

        public Question5()
        {
            InitializeComponent();
            newTimer.Interval = TimeSpan.FromSeconds(1);
            newTimer.Tick += OnTimerTick;
            newTimer.Start();
        }

        int counter = 6;
        void OnTimerTick(Object sender, EventArgs args)
        {
            counter--;
            if (counter < 0)
            {
                newTimer.Stop();
                counter = 6;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else if (counter > 0)
            {
                clock.Text = (counter-1).ToString();
                newTimer.Interval = TimeSpan.FromSeconds(1);
                newTimer.Tick += OnTimerTick;
                newTimer.Start();
            }
            else
            {
                Op1.Fill.Opacity = 100;
                Op1.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                clock.Text = "0";
            }
        }

        private void but1_Click(object sender, RoutedEventArgs e)
        {
            p1.Text = Convert.ToString(Convert.ToDecimal(p1.Text) + 20);
        }

        private void but2_Click(object sender, RoutedEventArgs e)
        {
            p1.Text = Convert.ToString(Convert.ToDecimal(p1.Text) + 20);
        }

        private void but3_Click(object sender, RoutedEventArgs e)
        {
            p2.Text = Convert.ToString(Convert.ToDecimal(p2.Text) + 20);
        }

        private void but4_Click(object sender, RoutedEventArgs e)
        {
            p2.Text = Convert.ToString(Convert.ToDecimal(p2.Text) + 20);
        }
    }
}