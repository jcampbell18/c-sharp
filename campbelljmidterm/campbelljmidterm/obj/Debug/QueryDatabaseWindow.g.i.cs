﻿#pragma checksum "..\..\QueryDatabaseWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "65264F2025032811E22C578EC0D41D8CFFF93AF9AAA6D78D948A89A4F65D5FB2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using campbelljmidterm;


namespace campbelljmidterm {
    
    
    /// <summary>
    /// QueryDatabaseWindow
    /// </summary>
    public partial class QueryDatabaseWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridQueryDatabase;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblExtension;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPanel;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbExtension;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOrExtension;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbExtension;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnApply;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvDatabaseResults;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEmptyDB;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\QueryDatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/campbelljmidterm;component/querydatabasewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\QueryDatabaseWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.gridQueryDatabase = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.lblExtension = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.stackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.cbExtension = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.lblOrExtension = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.tbExtension = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.btnApply = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\QueryDatabaseWindow.xaml"
            this.btnApply.Click += new System.Windows.RoutedEventHandler(this.OnApply);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lvDatabaseResults = ((System.Windows.Controls.ListView)(target));
            return;
            case 9:
            this.btnEmptyDB = ((System.Windows.Controls.Button)(target));
            
            #line 78 "..\..\QueryDatabaseWindow.xaml"
            this.btnEmptyDB.Click += new System.Windows.RoutedEventHandler(this.OnEmptyDatabase);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\QueryDatabaseWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.OnClose);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
