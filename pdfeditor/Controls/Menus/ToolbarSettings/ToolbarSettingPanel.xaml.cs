// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.ToolbarSettingPanel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class ToolbarSettingPanel : UserControl, IComponentConnector
{
  private Storyboard ShowContent;
  private Storyboard HideContent;
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingModel), typeof (ToolbarSettingPanel), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolbarSettingPanel.OnModelPropertyChanged)));
  internal Canvas HostCanvas;
  internal Border ClipBorder;
  internal Grid LayoutRoot;
  internal TranslateTransform LayoutRootTrans;
  internal DropShadowEffect LayoutRootShadow;
  internal Border bd;
  internal ItemsControl ContentItemsControl;
  private bool _contentLoaded;

  public ToolbarSettingPanel()
  {
    this.InitializeComponent();
    this.ShowContent = (Storyboard) this.HostCanvas.Resources[(object) nameof (ShowContent)];
    this.HideContent = (Storyboard) this.HostCanvas.Resources[(object) nameof (HideContent)];
    this.Loaded += new RoutedEventHandler(this.ToolbarSettingPanel_Loaded);
  }

  private void ToolbarSettingPanel_Loaded(object sender, RoutedEventArgs e)
  {
    this.ClipBorder.Visibility = Visibility.Collapsed;
    this.LayoutRootShadow.Opacity = 0.0;
    this.LayoutRootTrans.Y = -44.0;
  }

  private void UpdateModel()
  {
    this.ContentItemsControl.ItemsSource = (IEnumerable) this.Model;
    if (this.Model == null)
    {
      this.ShowContent.Stop();
      this.HideContent.Begin();
    }
    else
    {
      this.HideContent.Stop();
      this.ShowContent.Begin();
    }
  }

  public ToolbarSettingModel Model
  {
    get => (ToolbarSettingModel) this.GetValue(ToolbarSettingPanel.ModelProperty);
    set => this.SetValue(ToolbarSettingPanel.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue)
      return;
    ((ToolbarSettingPanel) d).UpdateModel();
  }

  private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ClipBorder.Width = e.NewSize.Width;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/toolbarsettingpanel.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((FrameworkElement) target).SizeChanged += new SizeChangedEventHandler(this.UserControl_SizeChanged);
        break;
      case 2:
        this.HostCanvas = (Canvas) target;
        break;
      case 3:
        this.ClipBorder = (Border) target;
        break;
      case 4:
        this.LayoutRoot = (Grid) target;
        break;
      case 5:
        this.LayoutRootTrans = (TranslateTransform) target;
        break;
      case 6:
        this.LayoutRootShadow = (DropShadowEffect) target;
        break;
      case 7:
        this.bd = (Border) target;
        break;
      case 8:
        this.ContentItemsControl = (ItemsControl) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
