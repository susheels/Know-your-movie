﻿#pragma checksum "C:\Users\susheel.suresh\Desktop\Movie_quiz AD\Know Your Movie 21-07-14\Movie\Results.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E5536AB8CDA0717DB3D5127FFCF31395"
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
    
    
    public partial class Results : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock WIN;
        
        internal System.Windows.Controls.TextBlock LOSE;
        
        internal System.Windows.Controls.TextBlock DRAW;
        
        internal System.Windows.Controls.TextBlock p1scoring;
        
        internal System.Windows.Controls.TextBlock p2scoring;
        
        internal System.Windows.Controls.Button ret;
        
        internal System.Windows.Controls.TextBlock tb;
        
        internal System.Windows.Controls.TextBlock myname;
        
        internal System.Windows.Controls.TextBlock opponent;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Movie;component/Results.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.WIN = ((System.Windows.Controls.TextBlock)(this.FindName("WIN")));
            this.LOSE = ((System.Windows.Controls.TextBlock)(this.FindName("LOSE")));
            this.DRAW = ((System.Windows.Controls.TextBlock)(this.FindName("DRAW")));
            this.p1scoring = ((System.Windows.Controls.TextBlock)(this.FindName("p1scoring")));
            this.p2scoring = ((System.Windows.Controls.TextBlock)(this.FindName("p2scoring")));
            this.ret = ((System.Windows.Controls.Button)(this.FindName("ret")));
            this.tb = ((System.Windows.Controls.TextBlock)(this.FindName("tb")));
            this.myname = ((System.Windows.Controls.TextBlock)(this.FindName("myname")));
            this.opponent = ((System.Windows.Controls.TextBlock)(this.FindName("opponent")));
        }
    }
}

