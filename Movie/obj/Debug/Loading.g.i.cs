﻿#pragma checksum "C:\Users\susheel.suresh\Desktop\Movie_quiz AD\Know Your Movie 21-07-14\Movie\Loading.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5B1CB1C014BBAD62803BC333930EB09"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Movie {
    
    
    public partial class Loading : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid simple;
        
        internal System.Windows.Controls.TextBlock TriviaTB;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ProgressBar prog;
        
        internal System.Windows.Controls.Button but;
        
        internal System.Windows.Controls.TextBlock t1;
        
        internal System.Windows.Controls.TextBlock t2;
        
        internal System.Windows.Controls.TextBlock ready;
        
        internal System.Windows.Controls.Grid Quit;
        
        internal System.Windows.Controls.Button Forfeit;
        
        internal System.Windows.Controls.Button Continue;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Movie;component/Loading.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.simple = ((System.Windows.Controls.Grid)(this.FindName("simple")));
            this.TriviaTB = ((System.Windows.Controls.TextBlock)(this.FindName("TriviaTB")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.prog = ((System.Windows.Controls.ProgressBar)(this.FindName("prog")));
            this.but = ((System.Windows.Controls.Button)(this.FindName("but")));
            this.t1 = ((System.Windows.Controls.TextBlock)(this.FindName("t1")));
            this.t2 = ((System.Windows.Controls.TextBlock)(this.FindName("t2")));
            this.ready = ((System.Windows.Controls.TextBlock)(this.FindName("ready")));
            this.Quit = ((System.Windows.Controls.Grid)(this.FindName("Quit")));
            this.Forfeit = ((System.Windows.Controls.Button)(this.FindName("Forfeit")));
            this.Continue = ((System.Windows.Controls.Button)(this.FindName("Continue")));
        }
    }
}
