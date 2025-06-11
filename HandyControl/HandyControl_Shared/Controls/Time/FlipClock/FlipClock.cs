// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.FlipClock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

public class FlipClock : Control
{
  private readonly DispatcherTimer _dispatcherTimer;
  private bool _isDisposed;
  public static readonly DependencyProperty NumberListProperty = DependencyProperty.Register(nameof (NumberList), typeof (List<int>), typeof (FlipClock), new PropertyMetadata((object) new List<int>()
  {
    0,
    0,
    0,
    0,
    0,
    0
  }));
  public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(nameof (DisplayTime), typeof (DateTime), typeof (FlipClock), new PropertyMetadata((object) new DateTime(), new PropertyChangedCallback(FlipClock.OnDisplayTimeChanged)));

  public List<int> NumberList
  {
    get => (List<int>) this.GetValue(FlipClock.NumberListProperty);
    set => this.SetValue(FlipClock.NumberListProperty, (object) value);
  }

  private static void OnDisplayTimeChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
  {
    FlipClock flipClock = (FlipClock) s;
    DateTime newValue = (DateTime) e.NewValue;
    flipClock.NumberList = new List<int>()
    {
      newValue.Hour / 10,
      newValue.Hour % 10,
      newValue.Minute / 10,
      newValue.Minute % 10,
      newValue.Second / 10,
      newValue.Second % 10
    };
  }

  public DateTime DisplayTime
  {
    get => (DateTime) this.GetValue(FlipClock.DisplayTimeProperty);
    set => this.SetValue(FlipClock.DisplayTimeProperty, (object) value);
  }

  public FlipClock()
  {
    this._dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render)
    {
      Interval = TimeSpan.FromMilliseconds(200.0)
    };
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.FlipClock_IsVisibleChanged);
  }

  ~FlipClock() => this.Dispose();

  public void Dispose()
  {
    if (this._isDisposed)
      return;
    this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.FlipClock_IsVisibleChanged);
    this._dispatcherTimer.Stop();
    this._isDisposed = true;
    GC.SuppressFinalize((object) this);
  }

  private void FlipClock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (this.IsVisible)
    {
      this._dispatcherTimer.Tick += new EventHandler(this.DispatcherTimer_Tick);
      this._dispatcherTimer.Start();
    }
    else
    {
      this._dispatcherTimer.Stop();
      this._dispatcherTimer.Tick -= new EventHandler(this.DispatcherTimer_Tick);
    }
  }

  private void DispatcherTimer_Tick(object sender, EventArgs e) => this.DisplayTime = DateTime.Now;
}
