// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.ChatButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Copilot;

public partial class ChatButton : Button
{
  public static readonly DependencyProperty IsTipsShowProperty = DependencyProperty.Register(nameof (IsTipsShow), typeof (bool), typeof (ChatButton), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatButton chatButton2) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatButton2.UpdateTipsShowStates(chatButton2.IsLoaded);
  })));
  public new static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof (bool), typeof (ChatButton), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatButton chatButton4))
      return;
    chatButton4.UpdateVisibleState();
  })));
  private DispatcherTimer timer;

  static ChatButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ChatButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ChatButton)));
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.UpdateVisibleState(false);
    this.UpdateTipsShowStates(false);
  }

  public bool IsTipsShow
  {
    get => (bool) this.GetValue(ChatButton.IsTipsShowProperty);
    set => this.SetValue(ChatButton.IsTipsShowProperty, (object) value);
  }

  public static bool GetIsVisible(DependencyObject obj)
  {
    return (bool) obj.GetValue(ChatButton.IsVisibleProperty);
  }

  public static void SetIsVisible(DependencyObject obj, bool value)
  {
    obj.SetValue(ChatButton.IsVisibleProperty, (object) value);
  }

  private void UpdateVisibleState(bool useTransitions = true)
  {
    if (ChatButton.GetIsVisible((DependencyObject) this))
    {
      VisualStateManager.GoToState((FrameworkElement) this, "Visible", useTransitions);
      this.Focusable = true;
    }
    else
    {
      VisualStateManager.GoToState((FrameworkElement) this, "Invisible", useTransitions);
      this.Focusable = false;
      this.timer?.Stop();
      this.timer = (DispatcherTimer) null;
      this.IsTipsShow = false;
    }
  }

  private void UpdateTipsShowStates(bool useTransitions = true)
  {
    if (this.IsTipsShow)
      VisualStateManager.GoToState((FrameworkElement) this, "TipsShowedState", useTransitions);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "TipsHidedState", useTransitions);
  }

  public void ShowTips()
  {
    this.IsTipsShow = true;
    if (this.timer != null)
    {
      this.timer.Stop();
      this.timer.Start();
    }
    else
    {
      this.timer = new DispatcherTimer(DispatcherPriority.Normal);
      this.timer.Interval = TimeSpan.FromSeconds(5.0);
      this.timer.Tick += new EventHandler(this.Timer_Tick);
      this.timer.Start();
    }
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    if (this.timer != null)
    {
      this.timer.Stop();
      this.timer = (DispatcherTimer) null;
    }
    this.IsTipsShow = false;
  }
}
