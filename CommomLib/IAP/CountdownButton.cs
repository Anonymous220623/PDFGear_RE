// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.CountdownButton
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.IAP;

[System.Windows.Markup.ContentProperty("Text")]
internal class CountdownButton : Button
{
  private DispatcherTimer timer;
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (CountdownButton), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is CountdownButton countdownButton2) || object.Equals(a.NewValue, a.OldValue))
      return;
    countdownButton2.UpdateContent();
  })));
  public static readonly DependencyProperty MaxCountdownSecondsProperty = DependencyProperty.Register(nameof (MaxCountdownSeconds), typeof (int), typeof (CountdownButton), new PropertyMetadata((object) 60, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is CountdownButton countdownButton4) || object.Equals(a.NewValue, a.OldValue))
      return;
    countdownButton4.UpdateCountdownSeconds();
  })));
  public static readonly DependencyProperty CountdownSecondsProperty = DependencyProperty.Register(nameof (CountdownSeconds), typeof (int), typeof (CountdownButton), new PropertyMetadata((object) 0, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is CountdownButton countdownButton6) || object.Equals(a.NewValue, a.OldValue))
      return;
    countdownButton6.UpdateCountdownSeconds();
  })));

  public CountdownButton()
  {
    this.timer = new DispatcherTimer();
    this.timer.Interval = TimeSpan.FromSeconds(1.0);
    this.timer.Tick += new EventHandler(this.Timer_Tick);
  }

  public string Text
  {
    get => (string) this.GetValue(CountdownButton.TextProperty);
    set => this.SetValue(CountdownButton.TextProperty, (object) value);
  }

  public int MaxCountdownSeconds
  {
    get => (int) this.GetValue(CountdownButton.MaxCountdownSecondsProperty);
    set => this.SetValue(CountdownButton.MaxCountdownSecondsProperty, (object) value);
  }

  public int CountdownSeconds
  {
    get => (int) this.GetValue(CountdownButton.CountdownSecondsProperty);
    set => this.SetValue(CountdownButton.CountdownSecondsProperty, (object) value);
  }

  public void Countdown()
  {
    if (this.CountdownSeconds > 0)
      return;
    this.CountdownSeconds = this.MaxCountdownSeconds;
  }

  private void UpdateContent()
  {
    if (this.CountdownSeconds <= 0)
      this.Content = (object) this.Text;
    else
      this.Content = (object) $"{this.CountdownSeconds}s";
  }

  private void UpdateCountdownSeconds()
  {
    if (this.CountdownSeconds <= 0)
    {
      if (this.timer.IsEnabled)
      {
        this.timer.Stop();
        this.CountdownSeconds = 0;
      }
    }
    else if (!this.timer.IsEnabled)
    {
      this.CountdownSeconds = this.MaxCountdownSeconds;
      this.timer.Start();
    }
    this.UpdateContent();
  }

  protected override void OnClick()
  {
    if (this.CountdownSeconds > 0)
      return;
    base.OnClick();
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    if (this.CountdownSeconds - 1 <= 0)
    {
      this.timer.Stop();
      this.CountdownSeconds = 0;
    }
    else
      --this.CountdownSeconds;
  }
}
