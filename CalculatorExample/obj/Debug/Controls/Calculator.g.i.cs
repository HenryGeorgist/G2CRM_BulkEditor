﻿#pragma checksum "..\..\..\Controls\Calculator.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "889F93D4B890CB84589476CC59D8B92A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CalculatorExample.Controls;
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


namespace CalculatorExample.Controls {
    
    
    /// <summary>
    /// Calculator
    /// </summary>
    public partial class Calculator : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition ExpressionTextBoxRow;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalculatorExample.Controls.AvailableFunctionsTreeView AvailableFunctions;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView AvailableFields;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalculatorExample.Controls.ExpressionTextBox TestWindow;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Result;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox IsCaseSensitive;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdErrorLog;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdExecute;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Controls\Calculator.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdClose;
        
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
            System.Uri resourceLocater = new System.Uri("/CalculatorExample;component/controls/calculator.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\Calculator.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ExpressionTextBoxRow = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 3:
            this.AvailableFunctions = ((CalculatorExample.Controls.AvailableFunctionsTreeView)(target));
            return;
            case 4:
            this.AvailableFields = ((System.Windows.Controls.TreeView)(target));
            
            #line 46 "..\..\..\Controls\Calculator.xaml"
            this.AvailableFields.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.AvailableFields_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TestWindow = ((CalculatorExample.Controls.ExpressionTextBox)(target));
            return;
            case 6:
            this.Result = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.IsCaseSensitive = ((System.Windows.Controls.CheckBox)(target));
            
            #line 53 "..\..\..\Controls\Calculator.xaml"
            this.IsCaseSensitive.Unchecked += new System.Windows.RoutedEventHandler(this.IsCaseSensitive_Unchecked);
            
            #line default
            #line hidden
            
            #line 53 "..\..\..\Controls\Calculator.xaml"
            this.IsCaseSensitive.Checked += new System.Windows.RoutedEventHandler(this.IsCaseSensitive_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CmdErrorLog = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\Controls\Calculator.xaml"
            this.CmdErrorLog.Click += new System.Windows.RoutedEventHandler(this.CmdErrorLog_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CmdExecute = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\Controls\Calculator.xaml"
            this.CmdExecute.Click += new System.Windows.RoutedEventHandler(this.CmdExecute_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CmdClose = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\Controls\Calculator.xaml"
            this.CmdClose.Click += new System.Windows.RoutedEventHandler(this.CmdClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

