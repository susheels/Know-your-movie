﻿#pragma checksum "C:\Users\susheel.suresh\Desktop\Movie_quiz AD\Know Your Movie 21-07-14\Movie\Sign_in.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "474EB19B6B6633019F0B5C9EBC3515EA"
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
    
    
    public partial class Sign_in : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.PhoneTextBox uname;
        
        internal Microsoft.Phone.Controls.PhoneTextBox pname;
        
        internal System.Windows.Controls.Button signb;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Movie;component/Sign_in.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.uname = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("uname")));
            this.pname = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("pname")));
            this.signb = ((System.Windows.Controls.Button)(this.FindName("signb")));
        }
    }
}

