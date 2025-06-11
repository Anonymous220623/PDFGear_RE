// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.GrowlWindow
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using HandyControl.Tools.Interop;
using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public sealed class GrowlWindow : Window
{
  internal Panel GrowlPanel { get; set; }

  internal GrowlWindow()
  {
    this.WindowStyle = WindowStyle.None;
    this.AllowsTransparency = true;
    StackPanel stackPanel = new StackPanel();
    stackPanel.VerticalAlignment = VerticalAlignment.Top;
    this.GrowlPanel = (Panel) stackPanel;
    ScrollViewer scrollViewer = new ScrollViewer();
    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
    scrollViewer.IsInertiaEnabled = true;
    scrollViewer.Content = (object) this.GrowlPanel;
    this.Content = (object) scrollViewer;
  }

  internal void Init()
  {
    Rect workArea = SystemParameters.WorkArea;
    this.Height = workArea.Height;
    this.Left = workArea.Right - this.Width;
    this.Top = 0.0;
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    InteropMethods.IntDestroyMenu(this.GetHwndSource().CreateHandleRef());
  }
}
