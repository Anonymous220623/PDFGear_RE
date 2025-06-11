// Decompiled with JetBrains decompiler
// Type: FileWatcher.Views.SettingsView
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace FileWatcher.Views;

public partial class SettingsView : Window, IComponentConnector
{
  internal StackPanel Panel;
  private bool _contentLoaded;

  public SettingsView()
  {
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.SettingsView_Loaded);
  }

  private void SettingsView_Loaded(object sender, RoutedEventArgs e) => this.UpdateState();

  private void StackPanel_Click(object sender, RoutedEventArgs e) => this.UpdateState();

  private void UpdateState()
  {
    foreach (CheckBox checkBox in this.Panel.Children.OfType<CheckBox>().Where<CheckBox>((Func<CheckBox, bool>) (c => c.Content is string)))
    {
      string path = "";
      switch ((string) checkBox.Content)
      {
        case "Desktop":
          path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
          break;
        case "Download":
          path = KnownFolders.GetPath(KnownFolder.Downloads);
          break;
        case "Documents":
          path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          break;
      }
      if (!string.IsNullOrEmpty(path))
      {
        if (checkBox.IsChecked.GetValueOrDefault())
          App.Current.Watcher.AddPath(path, "*.pdf", true);
        else
          App.Current.Watcher.RemovePath(path);
      }
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/FileWatcher;component/views/settingsview.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
    {
      this.Panel = (StackPanel) target;
      this.Panel.AddHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.StackPanel_Click));
    }
    else
      this._contentLoaded = true;
  }
}
