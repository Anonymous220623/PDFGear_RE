﻿#pragma checksum "..\..\..\..\controls\annotations\signaturedragcontrol.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E6383B132D02D51DF1065C327E5E13838BD0CF70D97C107F02542244113BA834"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations;
using pdfeditor.Properties;
using pdfeditor.Utils.Converters;


namespace pdfeditor.Controls.Annotations {
    
    
    /// <summary>
    /// SignatureDragControl
    /// </summary>
    public partial class SignatureDragControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 58 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle AnnotationDrag;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal pdfeditor.Controls.ResizeView DragResizeView;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Border1;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel OperationPanel;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Embed;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Embed_InBatch;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Delete;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Apply;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Delete_InBatch;
        
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
            System.Uri resourceLocater = new System.Uri("/pdfeditor;component/controls/annotations/signaturedragcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
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
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(target));
            
            #line 58 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
            this.LayoutRoot.SizeChanged += new System.Windows.SizeChangedEventHandler(this.LayoutRoot_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AnnotationDrag = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 3:
            this.DragResizeView = ((pdfeditor.Controls.ResizeView)(target));
            return;
            case 4:
            this.Border1 = ((System.Windows.Controls.Border)(target));
            return;
            case 5:
            this.OperationPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.Btn_Embed = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
            this.Btn_Embed.Click += new System.Windows.RoutedEventHandler(this.Btn_Embed_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Btn_Embed_InBatch = ((System.Windows.Controls.Button)(target));
            
            #line 87 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
            this.Btn_Embed_InBatch.Click += new System.Windows.RoutedEventHandler(this.Btn_Embed_InBatch_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Btn_Delete = ((System.Windows.Controls.Button)(target));
            
            #line 106 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
            this.Btn_Delete.Click += new System.Windows.RoutedEventHandler(this.Btn_Delete_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Btn_Apply = ((System.Windows.Controls.Button)(target));
            
            #line 123 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
            this.Btn_Apply.Click += new System.Windows.RoutedEventHandler(this.Btn_Apply_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Btn_Delete_InBatch = ((System.Windows.Controls.Button)(target));
            
            #line 142 "..\..\..\..\controls\annotations\signaturedragcontrol.xaml"
            this.Btn_Delete_InBatch.Click += new System.Windows.RoutedEventHandler(this.Btn_Delete_InBatch_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

