// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Notification
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

public sealed class Notification : System.Windows.Window
{
  private const int WaitTime = 6;
  private int _tickCount;
  private DispatcherTimer _timerClose;
  private bool _shouldBeClosed;

  private ShowAnimation ShowAnimation { get; set; }

  public Notification()
  {
    this.WindowStyle = WindowStyle.None;
    this.AllowsTransparency = true;
  }

  public static Notification Show(object content, ShowAnimation showAnimation = ShowAnimation.None, bool staysOpen = false)
  {
    Notification notification1 = new Notification();
    notification1.Content = content;
    notification1.Opacity = 0.0;
    notification1.ShowAnimation = showAnimation;
    Notification notification2 = notification1;
    notification2.Show();
    Rect workArea = SystemParameters.WorkArea;
    double toValue1 = workArea.Width - notification2.ActualWidth;
    double toValue2 = workArea.Height - notification2.ActualHeight;
    switch (showAnimation)
    {
      case ShowAnimation.None:
        notification2.Opacity = 1.0;
        notification2.Left = toValue1;
        notification2.Top = toValue2;
        break;
      case ShowAnimation.HorizontalMove:
        notification2.Opacity = 1.0;
        notification2.Left = workArea.Width;
        notification2.Top = toValue2;
        notification2.BeginAnimation(System.Windows.Window.LeftProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(toValue1));
        break;
      case ShowAnimation.VerticalMove:
        notification2.Opacity = 1.0;
        notification2.Left = toValue1;
        notification2.Top = workArea.Height;
        notification2.BeginAnimation(System.Windows.Window.TopProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(toValue2));
        break;
      case ShowAnimation.Fade:
        notification2.Left = toValue1;
        notification2.Top = toValue2;
        notification2.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(1.0));
        break;
      default:
        notification2.Opacity = 1.0;
        notification2.Left = toValue1;
        notification2.Top = toValue2;
        break;
    }
    if (!staysOpen)
      notification2.StartTimer();
    return notification2;
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    if (this._shouldBeClosed)
      return;
    Rect workArea = SystemParameters.WorkArea;
    switch (this.ShowAnimation)
    {
      case ShowAnimation.HorizontalMove:
        DoubleAnimation animation1 = AnimationHelper.CreateAnimation(workArea.Width);
        animation1.Completed += new EventHandler(this.Animation_Completed);
        this.BeginAnimation(System.Windows.Window.LeftProperty, (AnimationTimeline) animation1);
        e.Cancel = true;
        this._shouldBeClosed = true;
        break;
      case ShowAnimation.VerticalMove:
        DoubleAnimation animation2 = AnimationHelper.CreateAnimation(workArea.Height);
        animation2.Completed += new EventHandler(this.Animation_Completed);
        this.BeginAnimation(System.Windows.Window.TopProperty, (AnimationTimeline) animation2);
        e.Cancel = true;
        this._shouldBeClosed = true;
        break;
      case ShowAnimation.Fade:
        DoubleAnimation animation3 = AnimationHelper.CreateAnimation(0.0);
        animation3.Completed += new EventHandler(this.Animation_Completed);
        this.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation3);
        e.Cancel = true;
        this._shouldBeClosed = true;
        break;
    }
  }

  private void Animation_Completed(object sender, EventArgs e) => this.Close();

  private void StartTimer()
  {
    this._timerClose = new DispatcherTimer()
    {
      Interval = TimeSpan.FromSeconds(1.0)
    };
    this._timerClose.Tick += (EventHandler) ((_param1, _param2) =>
    {
      if (this.IsMouseOver)
      {
        this._tickCount = 0;
      }
      else
      {
        ++this._tickCount;
        if (this._tickCount < 6)
          return;
        this.Close();
      }
    });
    this._timerClose.Start();
  }
}
