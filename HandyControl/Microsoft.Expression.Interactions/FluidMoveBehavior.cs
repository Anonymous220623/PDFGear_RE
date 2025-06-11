// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.FluidMoveBehavior
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Interactivity;

public sealed class FluidMoveBehavior : FluidMoveBehaviorBase
{
  private static readonly DependencyProperty CacheDuringOverlayProperty = DependencyProperty.RegisterAttached("CacheDuringOverlay", typeof (object), typeof (FluidMoveBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), typeof (Duration), typeof (FluidMoveBehavior), new PropertyMetadata((object) new Duration(TimeSpan.FromSeconds(1.0))));
  public static readonly DependencyProperty EaseXProperty = DependencyProperty.Register(nameof (EaseX), typeof (IEasingFunction), typeof (FluidMoveBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EaseYProperty = DependencyProperty.Register(nameof (EaseY), typeof (IEasingFunction), typeof (FluidMoveBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty FloatAboveProperty = DependencyProperty.Register(nameof (FloatAbove), typeof (bool), typeof (FluidMoveBehavior), new PropertyMetadata(ValueBoxes.TrueBox));
  private static readonly DependencyProperty HasTransformWrapperProperty = DependencyProperty.RegisterAttached("HasTransformWrapper", typeof (bool), typeof (FluidMoveBehavior), new PropertyMetadata(ValueBoxes.FalseBox));
  private static readonly DependencyProperty InitialIdentityTagProperty = DependencyProperty.RegisterAttached("InitialIdentityTag", typeof (object), typeof (FluidMoveBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty InitialTagPathProperty = DependencyProperty.Register(nameof (InitialTagPath), typeof (string), typeof (FluidMoveBehavior), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty InitialTagProperty = DependencyProperty.Register(nameof (InitialTag), typeof (TagType), typeof (FluidMoveBehavior), new PropertyMetadata((object) TagType.Element));
  private static readonly DependencyProperty OverlayProperty = DependencyProperty.RegisterAttached("Overlay", typeof (object), typeof (FluidMoveBehavior), new PropertyMetadata((PropertyChangedCallback) null));
  private static readonly Dictionary<object, Storyboard> TransitionStoryboardDictionary = new Dictionary<object, Storyboard>();

  public Duration Duration
  {
    get => (Duration) this.GetValue(FluidMoveBehavior.DurationProperty);
    set => this.SetValue(FluidMoveBehavior.DurationProperty, (object) value);
  }

  public IEasingFunction EaseX
  {
    get => (IEasingFunction) this.GetValue(FluidMoveBehavior.EaseXProperty);
    set => this.SetValue(FluidMoveBehavior.EaseXProperty, (object) value);
  }

  public IEasingFunction EaseY
  {
    get => (IEasingFunction) this.GetValue(FluidMoveBehavior.EaseYProperty);
    set => this.SetValue(FluidMoveBehavior.EaseYProperty, (object) value);
  }

  public bool FloatAbove
  {
    get => (bool) this.GetValue(FluidMoveBehavior.FloatAboveProperty);
    set => this.SetValue(FluidMoveBehavior.FloatAboveProperty, ValueBoxes.BooleanBox(value));
  }

  public TagType InitialTag
  {
    get => (TagType) this.GetValue(FluidMoveBehavior.InitialTagProperty);
    set => this.SetValue(FluidMoveBehavior.InitialTagProperty, (object) value);
  }

  public string InitialTagPath
  {
    get => (string) this.GetValue(FluidMoveBehavior.InitialTagPathProperty);
    set => this.SetValue(FluidMoveBehavior.InitialTagPathProperty, (object) value);
  }

  protected override bool ShouldSkipInitialLayout
  {
    get => base.ShouldSkipInitialLayout || this.InitialTag == TagType.DataContext;
  }

  private static void AddTransform(FrameworkElement child, Transform transform)
  {
    if (!(child.RenderTransform is TransformGroup transformGroup))
    {
      transformGroup = new TransformGroup()
      {
        Children = {
          child.RenderTransform
        }
      };
      child.RenderTransform = (Transform) transformGroup;
      FluidMoveBehavior.SetHasTransformWrapper((DependencyObject) child, true);
    }
    transformGroup.Children.Add(transform);
  }

  private Storyboard CreateTransitionStoryboard(
    FrameworkElement child,
    bool usingBeforeLoaded,
    ref Rect layoutRect,
    ref Rect currentRect)
  {
    Duration duration = this.Duration;
    Storyboard storyboard = new Storyboard();
    storyboard.Duration = duration;
    Storyboard transitionStoryboard = storyboard;
    double num1 = !usingBeforeLoaded || Math.Abs(layoutRect.Width) < 0.001 ? 1.0 : currentRect.Width / layoutRect.Width;
    double num2 = !usingBeforeLoaded || Math.Abs(layoutRect.Height) < 0.001 ? 1.0 : currentRect.Height / layoutRect.Height;
    double num3 = currentRect.Left - layoutRect.Left;
    double num4 = currentRect.Top - layoutRect.Top;
    TransformGroup transformGroup = new TransformGroup();
    ScaleTransform scaleTransform = new ScaleTransform()
    {
      ScaleX = num1,
      ScaleY = num2
    };
    transformGroup.Children.Add((Transform) scaleTransform);
    TranslateTransform translateTransform = new TranslateTransform()
    {
      X = num3,
      Y = num4
    };
    transformGroup.Children.Add((Transform) translateTransform);
    FluidMoveBehavior.AddTransform(child, (Transform) transformGroup);
    string str = "(FrameworkElement.RenderTransform).";
    if (child.RenderTransform is TransformGroup renderTransform && FluidMoveBehavior.GetHasTransformWrapper((DependencyObject) child))
      str = $"{str}(TransformGroup.Children)[{(object) (renderTransform.Children.Count - 1)}].";
    if (usingBeforeLoaded)
    {
      if (Math.Abs(num1 - 1.0) > 0.001)
      {
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        doubleAnimation.Duration = duration;
        doubleAnimation.From = new double?(num1);
        doubleAnimation.To = new double?(1.0);
        DoubleAnimation element = doubleAnimation;
        Storyboard.SetTarget((DependencyObject) element, (DependencyObject) child);
        Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath(str + "(TransformGroup.Children)[0].(ScaleTransform.ScaleX)", Array.Empty<object>()));
        element.EasingFunction = this.EaseX;
        transitionStoryboard.Children.Add((Timeline) element);
      }
      if (Math.Abs(num2 - 1.0) > 0.001)
      {
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        doubleAnimation.Duration = duration;
        doubleAnimation.From = new double?(num2);
        doubleAnimation.To = new double?(1.0);
        DoubleAnimation element = doubleAnimation;
        Storyboard.SetTarget((DependencyObject) element, (DependencyObject) child);
        Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath(str + "(TransformGroup.Children)[0].(ScaleTransform.ScaleY)", Array.Empty<object>()));
        element.EasingFunction = this.EaseY;
        transitionStoryboard.Children.Add((Timeline) element);
      }
    }
    if (Math.Abs(num3) > 0.001)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      doubleAnimation.Duration = duration;
      doubleAnimation.From = new double?(num3);
      doubleAnimation.To = new double?(0.0);
      DoubleAnimation element = doubleAnimation;
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) child);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath(str + "(TransformGroup.Children)[1].(TranslateTransform.X)", Array.Empty<object>()));
      element.EasingFunction = this.EaseX;
      transitionStoryboard.Children.Add((Timeline) element);
    }
    if (Math.Abs(num4) > 0.001)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      doubleAnimation.Duration = duration;
      doubleAnimation.From = new double?(num4);
      doubleAnimation.To = new double?(0.0);
      DoubleAnimation element = doubleAnimation;
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) child);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath(str + "(TransformGroup.Children)[1].(TranslateTransform.Y)", Array.Empty<object>()));
      element.EasingFunction = this.EaseY;
      transitionStoryboard.Children.Add((Timeline) element);
    }
    return transitionStoryboard;
  }

  protected override void EnsureTags(FrameworkElement child)
  {
    base.EnsureTags(child);
    if (this.InitialTag != TagType.DataContext || child.ReadLocalValue(FluidMoveBehavior.InitialIdentityTagProperty) is BindingExpression)
      return;
    child.SetBinding(FluidMoveBehavior.InitialIdentityTagProperty, (BindingBase) new Binding(this.InitialTagPath));
  }

  private static bool GetHasTransformWrapper(DependencyObject obj)
  {
    return (bool) obj.GetValue(FluidMoveBehavior.HasTransformWrapperProperty);
  }

  private static object GetInitialIdentityTag(DependencyObject obj)
  {
    return obj.GetValue(FluidMoveBehavior.InitialIdentityTagProperty);
  }

  private static object GetOverlay(DependencyObject obj)
  {
    return obj.GetValue(FluidMoveBehavior.OverlayProperty);
  }

  private static Transform GetTransform(FrameworkElement child)
  {
    return child.RenderTransform is TransformGroup renderTransform && renderTransform.Children.Count > 0 ? renderTransform.Children[renderTransform.Children.Count - 1] : (Transform) new TranslateTransform();
  }

  private static bool IsClose(double a, double b) => Math.Abs(a - b) < 1E-07;

  private static bool IsEmptyRect(Rect rect)
  {
    return rect.IsEmpty || double.IsNaN(rect.Left) || double.IsNaN(rect.Top);
  }

  private static void RemoveTransform(FrameworkElement child)
  {
    if (!(child.RenderTransform is TransformGroup renderTransform))
      return;
    if (FluidMoveBehavior.GetHasTransformWrapper((DependencyObject) child))
    {
      child.RenderTransform = renderTransform.Children[0];
      FluidMoveBehavior.SetHasTransformWrapper((DependencyObject) child, false);
    }
    else
      renderTransform.Children.RemoveAt(renderTransform.Children.Count - 1);
  }

  private static void SetHasTransformWrapper(DependencyObject obj, bool value)
  {
    obj.SetValue(FluidMoveBehavior.HasTransformWrapperProperty, ValueBoxes.BooleanBox(value));
  }

  private static void SetOverlay(DependencyObject obj, object value)
  {
    obj.SetValue(FluidMoveBehavior.OverlayProperty, value);
  }

  private static void TransferLocalValue(
    FrameworkElement element,
    DependencyProperty source,
    DependencyProperty dest)
  {
    object obj = element.ReadLocalValue(source);
    if (obj is BindingExpressionBase bindingExpressionBase)
      element.SetBinding(dest, bindingExpressionBase.ParentBindingBase);
    else if (obj == DependencyProperty.UnsetValue)
      element.ClearValue(dest);
    else
      element.SetValue(dest, element.GetAnimationBaseValue(source));
    element.ClearValue(source);
  }

  internal override void UpdateLayoutTransitionCore(
    FrameworkElement child,
    FrameworkElement root,
    object tag,
    FluidMoveBehaviorBase.TagData newTagData)
  {
    bool flag1 = false;
    bool usingBeforeLoaded = false;
    object initialIdentityTag = FluidMoveBehavior.GetInitialIdentityTag((DependencyObject) child);
    FluidMoveBehaviorBase.TagData tagData1;
    bool flag2 = FluidMoveBehaviorBase.TagDictionary.TryGetValue(tag, out tagData1);
    if (flag2 && tagData1.InitialTag != initialIdentityTag)
    {
      flag2 = false;
      FluidMoveBehaviorBase.TagDictionary.Remove(tag);
    }
    Rect rect;
    if (!flag2)
    {
      FluidMoveBehaviorBase.TagData tagData2;
      if (initialIdentityTag != null && FluidMoveBehaviorBase.TagDictionary.TryGetValue(initialIdentityTag, out tagData2))
      {
        rect = FluidMoveBehaviorBase.TranslateRect(tagData2.AppRect, root, newTagData.Parent);
        flag1 = true;
        usingBeforeLoaded = true;
      }
      else
        rect = Rect.Empty;
      tagData1 = new FluidMoveBehaviorBase.TagData()
      {
        ParentRect = Rect.Empty,
        AppRect = Rect.Empty,
        Parent = newTagData.Parent,
        Child = child,
        Timestamp = DateTime.Now,
        InitialTag = initialIdentityTag
      };
      FluidMoveBehaviorBase.TagDictionary.Add(tag, tagData1);
    }
    else if (!object.Equals((object) tagData1.Parent, (object) VisualTreeHelper.GetParent((DependencyObject) child)))
    {
      rect = FluidMoveBehaviorBase.TranslateRect(tagData1.AppRect, root, newTagData.Parent);
      flag1 = true;
    }
    else
      rect = tagData1.ParentRect;
    FrameworkElement originalChild = child;
    if (!FluidMoveBehavior.IsEmptyRect(rect) && !FluidMoveBehavior.IsEmptyRect(newTagData.ParentRect) && (!FluidMoveBehavior.IsClose(rect.Left, newTagData.ParentRect.Left) || !FluidMoveBehavior.IsClose(rect.Top, newTagData.ParentRect.Top)) || !object.Equals((object) child, (object) tagData1.Child) && FluidMoveBehavior.TransitionStoryboardDictionary.ContainsKey(tag))
    {
      Rect currentRect = rect;
      bool flag3 = false;
      Storyboard storyboard;
      if (FluidMoveBehavior.TransitionStoryboardDictionary.TryGetValue(tag, out storyboard))
      {
        object overlay1;
        AdornerContainer adornerContainer = (AdornerContainer) (overlay1 = FluidMoveBehavior.GetOverlay((DependencyObject) tagData1.Child));
        flag3 = overlay1 != null;
        FrameworkElement child1 = tagData1.Child;
        if (overlay1 != null && adornerContainer.Child is Canvas child2)
          child1 = child2.Children[0] as FrameworkElement;
        if (!usingBeforeLoaded)
          currentRect = FluidMoveBehavior.GetTransform(child1).TransformBounds(currentRect);
        FluidMoveBehavior.TransitionStoryboardDictionary.Remove(tag);
        storyboard.Stop();
        FluidMoveBehavior.RemoveTransform(child1);
        if (overlay1 != null)
        {
          AdornerLayer.GetAdornerLayer((Visual) root).Remove((Adorner) adornerContainer);
          FluidMoveBehavior.TransferLocalValue(tagData1.Child, FluidMoveBehavior.CacheDuringOverlayProperty, UIElement.RenderTransformProperty);
          FluidMoveBehavior.SetOverlay((DependencyObject) tagData1.Child, (object) null);
        }
      }
      object overlay = (object) null;
      if (flag3 || flag1 && this.FloatAbove)
      {
        Canvas canvas1 = new Canvas();
        canvas1.Width = newTagData.ParentRect.Width;
        Rect parentRect = newTagData.ParentRect;
        canvas1.Height = parentRect.Height;
        canvas1.IsHitTestVisible = false;
        Canvas canvas2 = canvas1;
        Rectangle rectangle = new Rectangle();
        parentRect = newTagData.ParentRect;
        rectangle.Width = parentRect.Width;
        parentRect = newTagData.ParentRect;
        rectangle.Height = parentRect.Height;
        rectangle.IsHitTestVisible = false;
        rectangle.Fill = (Brush) new VisualBrush((Visual) child);
        Rectangle element = rectangle;
        canvas2.Children.Add((UIElement) element);
        AdornerContainer adornerContainer = new AdornerContainer((UIElement) child)
        {
          Child = (UIElement) canvas2
        };
        overlay = (object) adornerContainer;
        FluidMoveBehavior.SetOverlay((DependencyObject) originalChild, overlay);
        AdornerLayer.GetAdornerLayer((Visual) root).Add((Adorner) adornerContainer);
        FluidMoveBehavior.TransferLocalValue(child, UIElement.RenderTransformProperty, FluidMoveBehavior.CacheDuringOverlayProperty);
        child.RenderTransform = (Transform) new TranslateTransform(-10000.0, -10000.0);
        canvas2.RenderTransform = (Transform) new TranslateTransform(10000.0, 10000.0);
        child = (FrameworkElement) element;
      }
      Rect parentRect1 = newTagData.ParentRect;
      Storyboard transitionStoryboard = this.CreateTransitionStoryboard(child, usingBeforeLoaded, ref parentRect1, ref currentRect);
      FluidMoveBehavior.TransitionStoryboardDictionary.Add(tag, transitionStoryboard);
      transitionStoryboard.Completed += (EventHandler) ((_param1, _param2) =>
      {
        Storyboard objA;
        if (!FluidMoveBehavior.TransitionStoryboardDictionary.TryGetValue(tag, out objA) || !object.Equals((object) objA, (object) transitionStoryboard))
          return;
        FluidMoveBehavior.TransitionStoryboardDictionary.Remove(tag);
        transitionStoryboard.Stop();
        FluidMoveBehavior.RemoveTransform(child);
        child.InvalidateMeasure();
        if (overlay == null)
          return;
        AdornerLayer.GetAdornerLayer((Visual) root).Remove((Adorner) overlay);
        FluidMoveBehavior.TransferLocalValue(originalChild, FluidMoveBehavior.CacheDuringOverlayProperty, UIElement.RenderTransformProperty);
        FluidMoveBehavior.SetOverlay((DependencyObject) originalChild, (object) null);
      });
      transitionStoryboard.Begin();
    }
    tagData1.ParentRect = newTagData.ParentRect;
    tagData1.AppRect = newTagData.AppRect;
    tagData1.Parent = newTagData.Parent;
    tagData1.Child = newTagData.Child;
    tagData1.Timestamp = newTagData.Timestamp;
  }
}
