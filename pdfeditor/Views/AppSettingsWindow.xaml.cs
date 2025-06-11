// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.AppSettingsWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Commom.HotKeys;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfeditor.Models.Viewer;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Views;

public partial class AppSettingsWindow : Window, IComponentConnector
{
  public List<string> ThemesList;
  public List<string> BackgroundColorNamesList;
  private static List<BackgroundColorSetting> viewerBackgroundColorValues;
  public static readonly DependencyProperty HeaderedControlSplitLineVisibleProperty = DependencyProperty.RegisterAttached("HeaderedControlSplitLineVisible", typeof (bool), typeof (AppSettingsWindow), new PropertyMetadata((object) true));
  internal TabItem Tab_General;
  internal StackPanel ItemsStackPanel;
  internal TextBox AuthorTextBox;
  internal Rectangle SplitLine2;
  internal TextBox SearchTextBox;
  internal Button SearchButton;
  internal Rectangle SplitLine;
  private bool _contentLoaded;

  public AppSettingsWindow()
  {
    this.InitializeComponent();
    this.DataContext = (object) Ioc.Default.GetRequiredService<AppSettingsViewModel>();
    this.Loaded += new RoutedEventHandler(this.AppSettingsWindow_Loaded);
    AppSettingsWindow.SetHeaderedControlSplitLineVisible((DependencyObject) this.ItemsStackPanel.Children.OfType<HeaderedContentControl>().LastOrDefault<HeaderedContentControl>(), false);
    this.AuthorTextBox.Text = AnnotationAuthorUtil.GetAuthorName();
  }

  public AppSettingsViewModel VM => this.DataContext as AppSettingsViewModel;

  private void AppSettingsWindow_Loaded(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("SettingsWindow", "Show", "Count", 1L);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    this.VM.OnAppSettingsWindowClosing(this.DialogResult.GetValueOrDefault());
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    if (!this.DialogResult.GetValueOrDefault())
      return;
    AnnotationAuthorUtil.SetAuthorName(this.AuthorTextBox.Text.Trim());
    CommomLib.Commom.Log.WriteLog("AuthorName Changed: " + (string.IsNullOrWhiteSpace(this.AuthorTextBox.Text) ? "Empty" : "Custom"));
  }

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("SettingsWindow", "OkBtn", "Count", 1L);
    this.DialogResult = new bool?(true);
  }

  public static bool GetHeaderedControlSplitLineVisible(DependencyObject obj)
  {
    return (bool) obj.GetValue(AppSettingsWindow.HeaderedControlSplitLineVisibleProperty);
  }

  public static void SetHeaderedControlSplitLineVisible(DependencyObject obj, bool value)
  {
    obj.SetValue(AppSettingsWindow.HeaderedControlSplitLineVisibleProperty, (object) value);
  }

  private void RestoreButton_Click(object sender, RoutedEventArgs e)
  {
    HotKeyManager.ResetAllKeysToDefault();
  }

  private void SearchButton_Click(object sender, RoutedEventArgs e)
  {
  }

  private void ThemesCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
  }

  private void InitialThemes()
  {
  }

  private void InitialBackgroundColors()
  {
  }

  private void InitialShowStatusBarCheckBox()
  {
  }

  private void BackgroundColorsCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
  }

  private void ShowStatusBarCheckBox_Click(object sender, RoutedEventArgs e)
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Render, (Delegate) (() =>
    {
      if (!(sender is CheckBox checkBox2))
        return;
      ((MainView) App.Current.MainWindow).SetFooterVisible(checkBox2.IsChecked.Value);
    }));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/appsettingswindow.xaml", UriKind.Relative));
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
        this.Tab_General = (TabItem) target;
        break;
      case 2:
        this.ItemsStackPanel = (StackPanel) target;
        break;
      case 3:
        this.AuthorTextBox = (TextBox) target;
        break;
      case 4:
        this.SplitLine2 = (Rectangle) target;
        break;
      case 5:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      case 6:
        this.SearchTextBox = (TextBox) target;
        break;
      case 7:
        this.SearchButton = (Button) target;
        this.SearchButton.Click += new RoutedEventHandler(this.SearchButton_Click);
        break;
      case 8:
        this.SplitLine = (Rectangle) target;
        break;
      case 9:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.RestoreButton_Click);
        break;
      case 10:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
