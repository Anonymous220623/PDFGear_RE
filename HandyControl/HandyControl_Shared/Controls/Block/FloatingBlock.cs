// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.FloatingBlock
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

public class FloatingBlock : Control
{
  public static readonly DependencyProperty ToXProperty = DependencyProperty.RegisterAttached("ToX", typeof (double), typeof (FloatingBlock), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ToYProperty = DependencyProperty.RegisterAttached("ToY", typeof (double), typeof (FloatingBlock), new PropertyMetadata((object) -100.0));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.RegisterAttached("Duration", typeof (Duration), typeof (FloatingBlock), new PropertyMetadata((object) new Duration(TimeSpan.FromSeconds(2.0))));
  public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof (double), typeof (FloatingBlock), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (FloatingBlock), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached(nameof (ContentTemplate), typeof (DataTemplate), typeof (FloatingBlock), new PropertyMetadata((object) null, new PropertyChangedCallback(FloatingBlock.OnDataChanged)));
  private static readonly DependencyProperty ReadyToFloatProperty = DependencyProperty.RegisterAttached("ReadyToFloat", typeof (bool), typeof (FloatingBlock), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(nameof (Content), typeof (object), typeof (FloatingBlock), new PropertyMetadata((object) null, new PropertyChangedCallback(FloatingBlock.OnDataChanged)));

  public static void SetToX(DependencyObject element, double value)
  {
    element.SetValue(FloatingBlock.ToXProperty, (object) value);
  }

  public static double GetToX(DependencyObject element)
  {
    return (double) element.GetValue(FloatingBlock.ToXProperty);
  }

  public static void SetToY(DependencyObject element, double value)
  {
    element.SetValue(FloatingBlock.ToYProperty, (object) value);
  }

  public static double GetToY(DependencyObject element)
  {
    return (double) element.GetValue(FloatingBlock.ToYProperty);
  }

  public static void SetDuration(DependencyObject element, Duration value)
  {
    element.SetValue(FloatingBlock.DurationProperty, (object) value);
  }

  public static Duration GetDuration(DependencyObject element)
  {
    return (Duration) element.GetValue(FloatingBlock.DurationProperty);
  }

  public static void SetHorizontalOffset(DependencyObject element, double value)
  {
    element.SetValue(FloatingBlock.HorizontalOffsetProperty, (object) value);
  }

  public static double GetHorizontalOffset(DependencyObject element)
  {
    return (double) element.GetValue(FloatingBlock.HorizontalOffsetProperty);
  }

  public static void SetVerticalOffset(DependencyObject element, double value)
  {
    element.SetValue(FloatingBlock.VerticalOffsetProperty, (object) value);
  }

  public static double GetVerticalOffset(DependencyObject element)
  {
    return (double) element.GetValue(FloatingBlock.VerticalOffsetProperty);
  }

  public static void SetContentTemplate(DependencyObject element, DataTemplate value)
  {
    element.SetValue(FloatingBlock.ContentTemplateProperty, (object) value);
  }

  public static DataTemplate GetContentTemplate(DependencyObject element)
  {
    return (DataTemplate) element.GetValue(FloatingBlock.ContentTemplateProperty);
  }

  public DataTemplate ContentTemplate
  {
    get => (DataTemplate) this.GetValue(FloatingBlock.ContentTemplateProperty);
    set => this.SetValue(FloatingBlock.ContentTemplateProperty, (object) value);
  }

  private static void SetReadyToFloat(DependencyObject element, bool value)
  {
    element.SetValue(FloatingBlock.ReadyToFloatProperty, ValueBoxes.BooleanBox(value));
  }

  private static bool GetReadyToFloat(DependencyObject element)
  {
    return (bool) element.GetValue(FloatingBlock.ReadyToFloatProperty);
  }

  public static void SetContent(DependencyObject element, object value)
  {
    element.SetValue(FloatingBlock.ContentProperty, value);
  }

  public static object GetContent(DependencyObject element)
  {
    return element.GetValue(FloatingBlock.ContentProperty);
  }

  public object Content
  {
    get => this.GetValue(FloatingBlock.ContentProperty);
    set => this.SetValue(FloatingBlock.ContentProperty, value);
  }

  private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is UIElement uiElement))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    uiElement.PreviewMouseLeftButtonDown -= FloatingBlock.\u003C\u003EO.\u003C0\u003E__Target_PreviewMouseLeftButtonDown ?? (FloatingBlock.\u003C\u003EO.\u003C0\u003E__Target_PreviewMouseLeftButtonDown = new MouseButtonEventHandler(FloatingBlock.Target_PreviewMouseLeftButtonDown));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    uiElement.PreviewMouseLeftButtonUp -= FloatingBlock.\u003C\u003EO.\u003C1\u003E__Target_PreviewMouseLeftButtonUp ?? (FloatingBlock.\u003C\u003EO.\u003C1\u003E__Target_PreviewMouseLeftButtonUp = new MouseButtonEventHandler(FloatingBlock.Target_PreviewMouseLeftButtonUp));
    if (e.NewValue == null)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    uiElement.PreviewMouseLeftButtonDown += FloatingBlock.\u003C\u003EO.\u003C0\u003E__Target_PreviewMouseLeftButtonDown ?? (FloatingBlock.\u003C\u003EO.\u003C0\u003E__Target_PreviewMouseLeftButtonDown = new MouseButtonEventHandler(FloatingBlock.Target_PreviewMouseLeftButtonDown));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    uiElement.PreviewMouseLeftButtonUp += FloatingBlock.\u003C\u003EO.\u003C1\u003E__Target_PreviewMouseLeftButtonUp ?? (FloatingBlock.\u003C\u003EO.\u003C1\u003E__Target_PreviewMouseLeftButtonUp = new MouseButtonEventHandler(FloatingBlock.Target_PreviewMouseLeftButtonUp));
  }

  private static void Target_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    FloatingBlock.SetReadyToFloat(sender as DependencyObject, true);
  }

  private static void Target_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is UIElement element) || !FloatingBlock.GetReadyToFloat((DependencyObject) element))
      return;
    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) element);
    if (adornerLayer != null)
    {
      AdornerContainer adornerContainer = new AdornerContainer((UIElement) adornerLayer);
      adornerContainer.IsHitTestVisible = false;
      AdornerContainer adorner = adornerContainer;
      FloatingBlock block = FloatingBlock.CreateBlock((Visual) element, adorner);
      adorner.Child = (UIElement) block;
      adornerLayer.Add((Adorner) adorner);
    }
    FloatingBlock.SetReadyToFloat((DependencyObject) element, false);
  }

  private static FloatingBlock CreateBlock(Visual element, AdornerContainer adorner)
  {
    Point position = Mouse.GetPosition((IInputElement) adorner.AdornedElement);
    TranslateTransform translateTransform = new TranslateTransform()
    {
      X = position.X + FloatingBlock.GetHorizontalOffset((DependencyObject) element),
      Y = position.Y + FloatingBlock.GetVerticalOffset((DependencyObject) element)
    };
    FloatingBlock floatingBlock = new FloatingBlock();
    floatingBlock.Content = FloatingBlock.GetContent((DependencyObject) element);
    floatingBlock.ContentTemplate = FloatingBlock.GetContentTemplate((DependencyObject) element);
    floatingBlock.RenderTransform = (Transform) new TransformGroup()
    {
      Children = {
        (Transform) translateTransform
      }
    };
    FloatingBlock block = floatingBlock;
    double totalMilliseconds = FloatingBlock.GetDuration((DependencyObject) element).TimeSpan.TotalMilliseconds;
    DoubleAnimation animation1 = AnimationHelper.CreateAnimation(FloatingBlock.GetToX((DependencyObject) element) + translateTransform.X, totalMilliseconds);
    Storyboard.SetTargetProperty((DependencyObject) animation1, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)", Array.Empty<object>()));
    Storyboard.SetTarget((DependencyObject) animation1, (DependencyObject) block);
    DoubleAnimation animation2 = AnimationHelper.CreateAnimation(FloatingBlock.GetToY((DependencyObject) element) + translateTransform.Y, totalMilliseconds);
    Storyboard.SetTargetProperty((DependencyObject) animation2, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)", Array.Empty<object>()));
    Storyboard.SetTarget((DependencyObject) animation2, (DependencyObject) block);
    DoubleAnimation animation3 = AnimationHelper.CreateAnimation(0.0, totalMilliseconds);
    Storyboard.SetTargetProperty((DependencyObject) animation3, new PropertyPath("Opacity", Array.Empty<object>()));
    Storyboard.SetTarget((DependencyObject) animation3, (DependencyObject) block);
    Storyboard storyboard = new Storyboard();
    storyboard.Completed += (EventHandler) ((s, e) =>
    {
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(element);
      if (adornerLayer != null)
        adornerLayer.Remove((Adorner) adorner);
      else if (adorner.Parent is AdornerLayer parent2)
        parent2.Remove((Adorner) adorner);
      adorner.Child = (UIElement) null;
    });
    storyboard.Children.Add((Timeline) animation1);
    storyboard.Children.Add((Timeline) animation2);
    storyboard.Children.Add((Timeline) animation3);
    storyboard.Begin();
    return block;
  }
}
