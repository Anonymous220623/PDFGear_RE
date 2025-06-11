// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Copilot.ChatBubble
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Copilot;

public partial class ChatBubble : ContentControl
{
  private DispatcherTimer hideTimer;
  protected static readonly DependencyProperty IsVisibleOverrideProperty = DependencyProperty.Register(nameof (IsVisibleOverride), typeof (bool), typeof (ChatBubble), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ChatBubble chatBubble2) || object.Equals(a.NewValue, a.OldValue))
      return;
    chatBubble2.hideTimer.Stop();
    chatBubble2.UpdateVisibleState(chatBubble2.IsLoaded);
  })));
  internal static readonly DependencyProperty TemplateSettingsProperty = DependencyProperty.Register(nameof (TemplateSettings), typeof (ChatBubble.ChatBubbleTemplateSettings), typeof (ChatBubble), new PropertyMetadata((PropertyChangedCallback) null));

  static ChatBubble()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ChatBubble), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ChatBubble)));
  }

  public ChatBubble()
  {
    this.TemplateSettings = new ChatBubble.ChatBubbleTemplateSettings();
    this.hideTimer = new DispatcherTimer();
    this.hideTimer.Interval = TimeSpan.FromSeconds(2.0);
    this.hideTimer.Tick += new EventHandler(this.HideTimer_Tick);
    this.Unloaded += new RoutedEventHandler(this.ChatBubble_Unloaded);
    this.SizeChanged += new SizeChangedEventHandler(this.ChatBubble_SizeChanged);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.UpdateBackgroundSize();
    this.UpdateVisibleState(false);
  }

  public void ShowBubble(TimeSpan? autoHideTime)
  {
    this.hideTimer.Stop();
    this.IsVisibleOverride = true;
    if (!autoHideTime.HasValue)
      return;
    this.hideTimer.Interval = autoHideTime.Value;
    this.hideTimer.Start();
  }

  private void ChatBubble_Unloaded(object sender, RoutedEventArgs e) => this.hideTimer.Stop();

  private void ChatBubble_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateBackgroundSize();
  }

  private void HideTimer_Tick(object sender, EventArgs e)
  {
    this.hideTimer.Stop();
    this.IsVisibleOverride = false;
  }

  protected bool IsVisibleOverride
  {
    get => (bool) this.GetValue(ChatBubble.IsVisibleOverrideProperty);
    set => this.SetValue(ChatBubble.IsVisibleOverrideProperty, (object) value);
  }

  internal ChatBubble.ChatBubbleTemplateSettings TemplateSettings
  {
    get
    {
      return (ChatBubble.ChatBubbleTemplateSettings) this.GetValue(ChatBubble.TemplateSettingsProperty);
    }
    set => this.SetValue(ChatBubble.TemplateSettingsProperty, (object) value);
  }

  private void UpdateVisibleState(bool useTransitions)
  {
    if (this.IsVisibleOverride)
      VisualStateManager.GoToState((FrameworkElement) this, "VisibleState", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "InvisibleState", true);
  }

  private void UpdateBackgroundSize()
  {
    if (this.ActualWidth >= 40.0 && this.ActualHeight >= 10.0)
    {
      this.TemplateSettings.BackgroundPathVisibility = Visibility.Visible;
      this.TemplateSettings.RectGeometryRect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight - 10.0);
      this.TemplateSettings.TriangleGeometryTranslateX = this.ActualWidth - 40.0;
      this.TemplateSettings.TriangleGeometryTranslateY = this.ActualHeight - 10.0;
      this.TemplateSettings.BackgroundScaleTransformCenterX = this.ActualWidth - 40.0;
      this.TemplateSettings.BackgroundScaleTransformCenterY = this.ActualHeight;
    }
    else
      this.TemplateSettings.BackgroundPathVisibility = Visibility.Collapsed;
  }

  internal class ChatBubbleTemplateSettings : DependencyObject
  {
    public static readonly DependencyProperty TriangleGeometryTranslateXProperty = DependencyProperty.Register(nameof (TriangleGeometryTranslateX), typeof (double), typeof (ChatBubble.ChatBubbleTemplateSettings), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty TriangleGeometryTranslateYProperty = DependencyProperty.Register(nameof (TriangleGeometryTranslateY), typeof (double), typeof (ChatBubble.ChatBubbleTemplateSettings), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty RectGeometryRectProperty = DependencyProperty.Register(nameof (RectGeometryRect), typeof (Rect), typeof (ChatBubble.ChatBubbleTemplateSettings), new PropertyMetadata((object) new Rect(0.0, 0.0, 0.0, 0.0)));
    public static readonly DependencyProperty BackgroundPathVisibilityProperty = DependencyProperty.Register(nameof (BackgroundPathVisibility), typeof (Visibility), typeof (ChatBubble.ChatBubbleTemplateSettings), new PropertyMetadata((object) Visibility.Visible));
    public static readonly DependencyProperty BackgroundScaleTransformCenterXProperty = DependencyProperty.Register(nameof (BackgroundScaleTransformCenterX), typeof (double), typeof (ChatBubble.ChatBubbleTemplateSettings), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty BackgroundScaleTransformCenterYProperty = DependencyProperty.Register(nameof (BackgroundScaleTransformCenterY), typeof (double), typeof (ChatBubble.ChatBubbleTemplateSettings), new PropertyMetadata((object) 0.0));

    public double TriangleGeometryTranslateX
    {
      get
      {
        return (double) this.GetValue(ChatBubble.ChatBubbleTemplateSettings.TriangleGeometryTranslateXProperty);
      }
      set
      {
        this.SetValue(ChatBubble.ChatBubbleTemplateSettings.TriangleGeometryTranslateXProperty, (object) value);
      }
    }

    public double TriangleGeometryTranslateY
    {
      get
      {
        return (double) this.GetValue(ChatBubble.ChatBubbleTemplateSettings.TriangleGeometryTranslateYProperty);
      }
      set
      {
        this.SetValue(ChatBubble.ChatBubbleTemplateSettings.TriangleGeometryTranslateYProperty, (object) value);
      }
    }

    public Rect RectGeometryRect
    {
      get => (Rect) this.GetValue(ChatBubble.ChatBubbleTemplateSettings.RectGeometryRectProperty);
      set
      {
        this.SetValue(ChatBubble.ChatBubbleTemplateSettings.RectGeometryRectProperty, (object) value);
      }
    }

    public Visibility BackgroundPathVisibility
    {
      get
      {
        return (Visibility) this.GetValue(ChatBubble.ChatBubbleTemplateSettings.BackgroundPathVisibilityProperty);
      }
      set
      {
        this.SetValue(ChatBubble.ChatBubbleTemplateSettings.BackgroundPathVisibilityProperty, (object) value);
      }
    }

    public double BackgroundScaleTransformCenterX
    {
      get
      {
        return (double) this.GetValue(ChatBubble.ChatBubbleTemplateSettings.BackgroundScaleTransformCenterXProperty);
      }
      set
      {
        this.SetValue(ChatBubble.ChatBubbleTemplateSettings.BackgroundScaleTransformCenterXProperty, (object) value);
      }
    }

    public double BackgroundScaleTransformCenterY
    {
      get
      {
        return (double) this.GetValue(ChatBubble.ChatBubbleTemplateSettings.BackgroundScaleTransformCenterYProperty);
      }
      set
      {
        this.SetValue(ChatBubble.ChatBubbleTemplateSettings.BackgroundScaleTransformCenterYProperty, (object) value);
      }
    }
  }
}
