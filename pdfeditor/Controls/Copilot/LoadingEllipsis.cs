// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.LoadingEllipsis
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Copilot;

internal class LoadingEllipsis : Run
{
  private DispatcherTimer timer;
  private readonly bool delay;

  public LoadingEllipsis(bool delay)
  {
    this.Loaded += new RoutedEventHandler(this.LoadingEllipsis_Loaded);
    this.Unloaded += new RoutedEventHandler(this.LoadingEllipsis_Unloaded);
    this.Text = "...";
    this.delay = delay;
  }

  private void LoadingEllipsis_Loaded(object sender, RoutedEventArgs e) => this.CreateTimer();

  private void LoadingEllipsis_Unloaded(object sender, RoutedEventArgs e) => this.RemoveTimer();

  private void CreateTimer()
  {
    this.RemoveTimer();
    this.timer = new DispatcherTimer(DispatcherPriority.Normal)
    {
      Interval = TimeSpan.FromSeconds(this.delay ? 1.0 : 0.3)
    };
    this.timer.Tick += new EventHandler(this.Timer_Tick);
    this.timer.Start();
  }

  private void RemoveTimer()
  {
    if (this.timer == null)
      return;
    this.timer.Stop();
    this.timer.Tick -= new EventHandler(this.Timer_Tick);
    this.timer = (DispatcherTimer) null;
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    if (this.timer.Interval.TotalSeconds > 0.99)
    {
      this.timer.Stop();
      this.timer.Interval = TimeSpan.FromSeconds(0.3);
      this.timer.Start();
    }
    switch (this.Text)
    {
      case ".":
        this.Text = "..";
        break;
      case "..":
        this.Text = "...";
        break;
      case "...":
        this.Text = "";
        break;
      default:
        this.Text = ".";
        break;
    }
  }
}
