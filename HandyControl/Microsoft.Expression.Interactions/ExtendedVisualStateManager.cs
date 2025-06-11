// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.ExtendedVisualStateManager
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Interactivity;

public class ExtendedVisualStateManager : VisualStateManager
{
  internal static readonly DependencyProperty CachedBackgroundProperty = DependencyProperty.RegisterAttached("CachedBackground", typeof (object), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty CachedEffectProperty = DependencyProperty.RegisterAttached("CachedEffect", typeof (Effect), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  private static readonly List<DependencyProperty> ChildAffectingLayoutProperties;
  internal static readonly DependencyProperty CurrentStateProperty = DependencyProperty.RegisterAttached("CurrentState", typeof (VisualState), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty DidCacheBackgroundProperty = DependencyProperty.RegisterAttached("DidCacheBackground", typeof (bool), typeof (ExtendedVisualStateManager), new PropertyMetadata(ValueBoxes.FalseBox));
  private static readonly List<DependencyProperty> LayoutProperties;
  internal static readonly DependencyProperty LayoutStoryboardProperty = DependencyProperty.RegisterAttached("LayoutStoryboard", typeof (Storyboard), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  private static Storyboard LayoutTransitionStoryboard;
  private static List<FrameworkElement> MovingElements;
  internal static readonly DependencyProperty OriginalLayoutValuesProperty = DependencyProperty.RegisterAttached("OriginalLayoutValues", typeof (List<ExtendedVisualStateManager.OriginalLayoutValueRecord>), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RuntimeVisibilityPropertyProperty = DependencyProperty.RegisterAttached("RuntimeVisibilityProperty", typeof (DependencyProperty), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TransitionEffectProperty = DependencyProperty.RegisterAttached("TransitionEffect", typeof (TransitionEffect), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  internal static readonly DependencyProperty TransitionEffectStoryboardProperty = DependencyProperty.RegisterAttached("TransitionEffectStoryboard", typeof (Storyboard), typeof (ExtendedVisualStateManager), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty UseFluidLayoutProperty = DependencyProperty.RegisterAttached("UseFluidLayout", typeof (bool), typeof (ExtendedVisualStateManager), new PropertyMetadata(ValueBoxes.FalseBox));
  private bool _changingState;

  static ExtendedVisualStateManager()
  {
    ExtendedVisualStateManager.LayoutProperties = new List<DependencyProperty>()
    {
      Grid.ColumnProperty,
      Grid.ColumnSpanProperty,
      Grid.RowProperty,
      Grid.RowSpanProperty,
      Canvas.LeftProperty,
      Canvas.TopProperty,
      FrameworkElement.WidthProperty,
      FrameworkElement.HeightProperty,
      FrameworkElement.MinWidthProperty,
      FrameworkElement.MinHeightProperty,
      FrameworkElement.MaxWidthProperty,
      FrameworkElement.MaxHeightProperty,
      FrameworkElement.MarginProperty,
      FrameworkElement.HorizontalAlignmentProperty,
      FrameworkElement.VerticalAlignmentProperty,
      UIElement.VisibilityProperty,
      StackPanel.OrientationProperty
    };
    ExtendedVisualStateManager.ChildAffectingLayoutProperties = new List<DependencyProperty>()
    {
      StackPanel.OrientationProperty
    };
  }

  public static bool IsRunningFluidLayoutTransition
  {
    get => ExtendedVisualStateManager.LayoutTransitionStoryboard != null;
  }

  private static void AnimateTransitionEffect(
    FrameworkElement stateGroupsRoot,
    VisualTransition transition)
  {
    DoubleAnimation doubleAnimation = new DoubleAnimation();
    doubleAnimation.Duration = transition.GeneratedDuration;
    doubleAnimation.EasingFunction = transition.GeneratedEasingFunction;
    doubleAnimation.From = new double?(0.0);
    doubleAnimation.To = new double?(1.0);
    DoubleAnimation element1 = doubleAnimation;
    Storyboard storyboard = new Storyboard();
    storyboard.Duration = transition.GeneratedDuration;
    storyboard.Children.Add((Timeline) element1);
    Storyboard sb = storyboard;
    Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) stateGroupsRoot);
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(0).(1)", new object[2]
    {
      (object) UIElement.EffectProperty,
      (object) TransitionEffect.ProgressProperty
    }));
    if (stateGroupsRoot is Panel element2 && element2.Background == null)
    {
      ExtendedVisualStateManager.SetDidCacheBackground((DependencyObject) element2, true);
      ExtendedVisualStateManager.TransferLocalValue((FrameworkElement) element2, Panel.BackgroundProperty, ExtendedVisualStateManager.CachedBackgroundProperty);
      element2.Background = (Brush) Brushes.Transparent;
    }
    sb.Completed += (EventHandler) ((_param1, _param2) =>
    {
      if (!object.Equals((object) ExtendedVisualStateManager.GetTransitionEffectStoryboard((DependencyObject) stateGroupsRoot), (object) sb))
        return;
      ExtendedVisualStateManager.FinishTransitionEffectAnimation(stateGroupsRoot);
    });
    ExtendedVisualStateManager.SetTransitionEffectStoryboard((DependencyObject) stateGroupsRoot, sb);
    sb.Begin();
  }

  private static object CacheLocalValueHelper(
    DependencyObject dependencyObject,
    DependencyProperty property)
  {
    return dependencyObject.ReadLocalValue(property);
  }

  private static void control_LayoutUpdated(object sender, EventArgs e)
  {
    if (ExtendedVisualStateManager.LayoutTransitionStoryboard == null)
      return;
    foreach (FrameworkElement movingElement in ExtendedVisualStateManager.MovingElements)
    {
      if (movingElement.Parent is ExtendedVisualStateManager.WrapperCanvas parent)
      {
        Rect layoutRect = ExtendedVisualStateManager.GetLayoutRect((FrameworkElement) parent);
        Rect newRect = parent.NewRect;
        double num1 = parent.RenderTransform is TranslateTransform translateTransform ? translateTransform.X : 0.0;
        double num2 = translateTransform != null ? translateTransform.Y : 0.0;
        double num3 = newRect.Left - layoutRect.Left;
        double num4 = newRect.Top - layoutRect.Top;
        double num5 = num3;
        if (Math.Abs(num1 - num5) > 0.001 || Math.Abs(num2 - num4) > 0.001)
        {
          if (translateTransform == null)
          {
            translateTransform = new TranslateTransform();
            parent.RenderTransform = (Transform) translateTransform;
          }
          translateTransform.X = num3;
          translateTransform.Y = num4;
        }
      }
    }
  }

  private static void CopyLayoutProperties(
    FrameworkElement source,
    FrameworkElement target,
    bool restoring)
  {
    ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = restoring ? (ExtendedVisualStateManager.WrapperCanvas) source : (ExtendedVisualStateManager.WrapperCanvas) target;
    if (wrapperCanvas.LocalValueCache == null)
      wrapperCanvas.LocalValueCache = new Dictionary<DependencyProperty, object>();
    foreach (DependencyProperty layoutProperty in ExtendedVisualStateManager.LayoutProperties)
    {
      if (!ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(layoutProperty))
      {
        if (restoring)
        {
          ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(target, layoutProperty, wrapperCanvas.LocalValueCache[layoutProperty]);
        }
        else
        {
          object obj1 = target.GetValue(layoutProperty);
          object obj2 = ExtendedVisualStateManager.CacheLocalValueHelper((DependencyObject) source, layoutProperty);
          wrapperCanvas.LocalValueCache[layoutProperty] = obj2;
          if (ExtendedVisualStateManager.IsVisibilityProperty(layoutProperty))
            wrapperCanvas.DestinationVisibilityCache = (Visibility) source.GetValue(layoutProperty);
          else
            target.SetValue(layoutProperty, source.GetValue(layoutProperty));
          source.SetValue(layoutProperty, obj1);
        }
      }
    }
  }

  private static Storyboard CreateLayoutTransitionStoryboard(
    VisualTransition transition,
    List<FrameworkElement> movingElements,
    Dictionary<FrameworkElement, double> oldOpacities)
  {
    Duration duration = transition != null ? transition.GeneratedDuration : new Duration(TimeSpan.Zero);
    IEasingFunction generatedEasingFunction = transition?.GeneratedEasingFunction;
    Storyboard storyboard = new Storyboard();
    storyboard.Duration = duration;
    Storyboard transitionStoryboard = storyboard;
    foreach (FrameworkElement movingElement in movingElements)
    {
      if (movingElement.Parent is ExtendedVisualStateManager.WrapperCanvas parent)
      {
        DoubleAnimation doubleAnimation1 = new DoubleAnimation();
        doubleAnimation1.From = new double?(1.0);
        doubleAnimation1.To = new double?(0.0);
        doubleAnimation1.Duration = duration;
        doubleAnimation1.EasingFunction = generatedEasingFunction;
        DoubleAnimation element1 = doubleAnimation1;
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) parent);
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) ExtendedVisualStateManager.WrapperCanvas.SimulationProgressProperty));
        transitionStoryboard.Children.Add((Timeline) element1);
        parent.SimulationProgress = 1.0;
        Rect newRect = parent.NewRect;
        if (!ExtendedVisualStateManager.IsClose(parent.Width, newRect.Width))
        {
          DoubleAnimation doubleAnimation2 = new DoubleAnimation();
          doubleAnimation2.From = new double?(newRect.Width);
          doubleAnimation2.To = new double?(newRect.Width);
          doubleAnimation2.Duration = duration;
          DoubleAnimation element2 = doubleAnimation2;
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) parent);
          Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) FrameworkElement.WidthProperty));
          transitionStoryboard.Children.Add((Timeline) element2);
        }
        if (!ExtendedVisualStateManager.IsClose(parent.Height, newRect.Height))
        {
          DoubleAnimation doubleAnimation3 = new DoubleAnimation();
          doubleAnimation3.From = new double?(newRect.Height);
          doubleAnimation3.To = new double?(newRect.Height);
          doubleAnimation3.Duration = duration;
          DoubleAnimation element3 = doubleAnimation3;
          Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) parent);
          Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) FrameworkElement.HeightProperty));
          transitionStoryboard.Children.Add((Timeline) element3);
        }
        if (parent.DestinationVisibilityCache == Visibility.Collapsed)
        {
          Thickness margin = parent.Margin;
          if (!ExtendedVisualStateManager.IsClose(margin.Left, 0.0) || !ExtendedVisualStateManager.IsClose(margin.Top, 0.0) || !ExtendedVisualStateManager.IsClose(margin.Right, 0.0) || !ExtendedVisualStateManager.IsClose(margin.Bottom, 0.0))
          {
            ObjectAnimationUsingKeyFrames animationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
            animationUsingKeyFrames.Duration = duration;
            ObjectAnimationUsingKeyFrames element4 = animationUsingKeyFrames;
            DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
            discreteObjectKeyFrame.KeyTime = (KeyTime) TimeSpan.Zero;
            discreteObjectKeyFrame.Value = (object) new Thickness();
            DiscreteObjectKeyFrame keyFrame = discreteObjectKeyFrame;
            element4.KeyFrames.Add((ObjectKeyFrame) keyFrame);
            Storyboard.SetTarget((DependencyObject) element4, (DependencyObject) parent);
            Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath((object) FrameworkElement.MarginProperty));
            transitionStoryboard.Children.Add((Timeline) element4);
          }
          if (!ExtendedVisualStateManager.IsClose(parent.MinWidth, 0.0))
          {
            DoubleAnimation doubleAnimation4 = new DoubleAnimation();
            doubleAnimation4.From = new double?(0.0);
            doubleAnimation4.To = new double?(0.0);
            doubleAnimation4.Duration = duration;
            DoubleAnimation element5 = doubleAnimation4;
            Storyboard.SetTarget((DependencyObject) element5, (DependencyObject) parent);
            Storyboard.SetTargetProperty((DependencyObject) element5, new PropertyPath((object) FrameworkElement.MinWidthProperty));
            transitionStoryboard.Children.Add((Timeline) element5);
          }
          if (!ExtendedVisualStateManager.IsClose(parent.MinHeight, 0.0))
          {
            DoubleAnimation doubleAnimation5 = new DoubleAnimation();
            doubleAnimation5.From = new double?(0.0);
            doubleAnimation5.To = new double?(0.0);
            doubleAnimation5.Duration = duration;
            DoubleAnimation element6 = doubleAnimation5;
            Storyboard.SetTarget((DependencyObject) element6, (DependencyObject) parent);
            Storyboard.SetTargetProperty((DependencyObject) element6, new PropertyPath((object) FrameworkElement.MinHeightProperty));
            transitionStoryboard.Children.Add((Timeline) element6);
          }
        }
      }
    }
    foreach (FrameworkElement key in oldOpacities.Keys)
    {
      if (key.Parent is ExtendedVisualStateManager.WrapperCanvas parent)
      {
        double oldOpacity = oldOpacities[key];
        double a = parent.DestinationVisibilityCache == Visibility.Visible ? 1.0 : 0.0;
        if (!ExtendedVisualStateManager.IsClose(oldOpacity, 1.0) || !ExtendedVisualStateManager.IsClose(a, 1.0))
        {
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(oldOpacity);
          doubleAnimation.To = new double?(a);
          doubleAnimation.Duration = duration;
          doubleAnimation.EasingFunction = generatedEasingFunction;
          DoubleAnimation element = doubleAnimation;
          Storyboard.SetTarget((DependencyObject) element, (DependencyObject) parent);
          Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) UIElement.OpacityProperty));
          transitionStoryboard.Children.Add((Timeline) element);
        }
      }
    }
    return transitionStoryboard;
  }

  private static Storyboard ExtractLayoutStoryboard(VisualState state)
  {
    Storyboard storyboard = (Storyboard) null;
    if (state.Storyboard != null)
    {
      storyboard = ExtendedVisualStateManager.GetLayoutStoryboard((DependencyObject) state.Storyboard);
      if (storyboard == null)
      {
        storyboard = new Storyboard();
        for (int index = state.Storyboard.Children.Count - 1; index >= 0; --index)
        {
          Timeline child = state.Storyboard.Children[index];
          if (ExtendedVisualStateManager.LayoutPropertyFromTimeline(child, false) != null)
          {
            state.Storyboard.Children.RemoveAt(index);
            storyboard.Children.Add(child);
          }
        }
        ExtendedVisualStateManager.SetLayoutStoryboard((DependencyObject) state.Storyboard, storyboard);
      }
    }
    return storyboard ?? new Storyboard();
  }

  private static List<FrameworkElement> FindTargetElements(
    FrameworkElement control,
    FrameworkElement templateRoot,
    Storyboard layoutStoryboard,
    List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords,
    List<FrameworkElement> movingElements)
  {
    List<FrameworkElement> targetElements = new List<FrameworkElement>();
    if (movingElements != null)
      targetElements.AddRange((IEnumerable<FrameworkElement>) movingElements);
    foreach (Timeline child1 in layoutStoryboard.Children)
    {
      FrameworkElement timelineTarget = (FrameworkElement) ExtendedVisualStateManager.GetTimelineTarget(control, templateRoot, child1);
      if (timelineTarget != null)
      {
        if (!targetElements.Contains(timelineTarget))
          targetElements.Add(timelineTarget);
        if (ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(ExtendedVisualStateManager.LayoutPropertyFromTimeline(child1, false)) && timelineTarget is Panel panel)
        {
          foreach (FrameworkElement child2 in panel.Children)
          {
            if (!targetElements.Contains(child2) && !(child2 is ExtendedVisualStateManager.WrapperCanvas))
              targetElements.Add(child2);
          }
        }
      }
    }
    foreach (ExtendedVisualStateManager.OriginalLayoutValueRecord originalValueRecord in originalValueRecords)
    {
      if (!targetElements.Contains(originalValueRecord.Element))
        targetElements.Add(originalValueRecord.Element);
      if (ExtendedVisualStateManager.ChildAffectingLayoutProperties.Contains(originalValueRecord.Property) && originalValueRecord.Element is Panel element)
      {
        foreach (FrameworkElement child in element.Children)
        {
          if (!targetElements.Contains(child) && !(child is ExtendedVisualStateManager.WrapperCanvas))
            targetElements.Add(child);
        }
      }
    }
    for (int index = 0; index < targetElements.Count; ++index)
    {
      FrameworkElement reference = targetElements[index];
      FrameworkElement parent = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement;
      if (movingElements != null && movingElements.Contains(reference) && parent is ExtendedVisualStateManager.WrapperCanvas)
        parent = VisualTreeHelper.GetParent((DependencyObject) parent) as FrameworkElement;
      if (parent != null)
      {
        if (!targetElements.Contains(parent))
          targetElements.Add(parent);
        for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) parent); ++childIndex)
        {
          if (VisualTreeHelper.GetChild((DependencyObject) parent, childIndex) is FrameworkElement child && !targetElements.Contains(child) && !(child is ExtendedVisualStateManager.WrapperCanvas))
            targetElements.Add(child);
        }
      }
    }
    return targetElements;
  }

  private static VisualTransition FindTransition(
    VisualStateGroup group,
    VisualState previousState,
    VisualState state)
  {
    string str1 = previousState != null ? previousState.Name : string.Empty;
    string str2 = state != null ? state.Name : string.Empty;
    int num1 = -1;
    VisualTransition transition1 = (VisualTransition) null;
    foreach (VisualTransition transition2 in (IEnumerable) group.Transitions)
    {
      int num2 = 0;
      if (transition2.From == str1)
        ++num2;
      else if (!string.IsNullOrEmpty(transition2.From))
        continue;
      if (transition2.To == str2)
        num2 += 2;
      else if (!string.IsNullOrEmpty(transition2.To))
        continue;
      if (num2 > num1)
      {
        num1 = num2;
        transition1 = transition2;
      }
    }
    return transition1;
  }

  private static bool FinishesWithZeroOpacity(
    FrameworkElement control,
    FrameworkElement stateGroupsRoot,
    VisualState state,
    VisualState previousState)
  {
    if (state.Storyboard != null)
    {
      foreach (Timeline child in state.Storyboard.Children)
      {
        if (ExtendedVisualStateManager.TimelineIsAnimatingRootOpacity(child, control, stateGroupsRoot))
        {
          bool gotValue;
          object valueFromTimeline = ExtendedVisualStateManager.GetValueFromTimeline(child, out gotValue);
          return gotValue && valueFromTimeline is double num && Math.Abs(num) < 0.001;
        }
      }
    }
    if (previousState?.Storyboard == null)
      return Math.Abs(stateGroupsRoot.Opacity) < 0.001;
    foreach (Timeline child in previousState.Storyboard.Children)
      ExtendedVisualStateManager.TimelineIsAnimatingRootOpacity(child, control, stateGroupsRoot);
    return Math.Abs((double) stateGroupsRoot.GetAnimationBaseValue(UIElement.OpacityProperty)) < 0.001;
  }

  private static void FinishTransitionEffectAnimation(FrameworkElement stateGroupsRoot)
  {
    ExtendedVisualStateManager.SetTransitionEffectStoryboard((DependencyObject) stateGroupsRoot, (Storyboard) null);
    ExtendedVisualStateManager.TransferLocalValue(stateGroupsRoot, ExtendedVisualStateManager.CachedEffectProperty, UIElement.EffectProperty);
    if (!ExtendedVisualStateManager.GetDidCacheBackground((DependencyObject) stateGroupsRoot))
      return;
    ExtendedVisualStateManager.TransferLocalValue(stateGroupsRoot, ExtendedVisualStateManager.CachedBackgroundProperty, Panel.BackgroundProperty);
    ExtendedVisualStateManager.SetDidCacheBackground((DependencyObject) stateGroupsRoot, false);
  }

  internal static object GetCachedBackground(DependencyObject obj)
  {
    return obj.GetValue(ExtendedVisualStateManager.CachedBackgroundProperty);
  }

  internal static Effect GetCachedEffect(DependencyObject obj)
  {
    return (Effect) obj.GetValue(ExtendedVisualStateManager.CachedEffectProperty);
  }

  internal static VisualState GetCurrentState(DependencyObject obj)
  {
    return (VisualState) obj.GetValue(ExtendedVisualStateManager.CurrentStateProperty);
  }

  internal static bool GetDidCacheBackground(DependencyObject obj)
  {
    return (bool) obj.GetValue(ExtendedVisualStateManager.DidCacheBackgroundProperty);
  }

  internal static Rect GetLayoutRect(FrameworkElement element)
  {
    double num1 = element.ActualWidth;
    double num2 = element.ActualHeight;
    if (element is Image || element is MediaElement)
    {
      if (element.Parent is Canvas)
      {
        num1 = double.IsNaN(element.Width) ? num1 : element.Width;
        num2 = double.IsNaN(element.Height) ? num2 : element.Height;
      }
      else
      {
        num1 = element.RenderSize.Width;
        num2 = element.RenderSize.Height;
      }
    }
    double width = element.Visibility == Visibility.Collapsed ? 0.0 : num1;
    double height = element.Visibility == Visibility.Collapsed ? 0.0 : num2;
    Thickness margin = element.Margin;
    Rect layoutSlot = LayoutInformation.GetLayoutSlot(element);
    double num3 = 0.0;
    double num4 = 0.0;
    double num5;
    switch (element.HorizontalAlignment)
    {
      case HorizontalAlignment.Left:
        num5 = layoutSlot.Left + margin.Left;
        break;
      case HorizontalAlignment.Center:
        num5 = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0;
        break;
      case HorizontalAlignment.Right:
        num5 = layoutSlot.Right - margin.Right - width;
        break;
      case HorizontalAlignment.Stretch:
        num5 = Math.Max(layoutSlot.Left + margin.Left, (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0);
        break;
      default:
        num5 = num3;
        break;
    }
    double x = num5;
    double num6;
    switch (element.VerticalAlignment)
    {
      case VerticalAlignment.Top:
        num6 = layoutSlot.Top + margin.Top;
        break;
      case VerticalAlignment.Center:
        num6 = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        num6 = layoutSlot.Bottom - margin.Bottom - height;
        break;
      case VerticalAlignment.Stretch:
        num6 = Math.Max(layoutSlot.Top + margin.Top, (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0);
        break;
      default:
        num6 = num4;
        break;
    }
    double y = num6;
    return new Rect(x, y, width, height);
  }

  internal static Storyboard GetLayoutStoryboard(DependencyObject obj)
  {
    return (Storyboard) obj.GetValue(ExtendedVisualStateManager.LayoutStoryboardProperty);
  }

  private static Dictionary<FrameworkElement, double> GetOldOpacities(
    FrameworkElement control,
    FrameworkElement templateRoot,
    Storyboard layoutStoryboard,
    List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords,
    List<FrameworkElement> movingElements)
  {
    Dictionary<FrameworkElement, double> oldOpacities = new Dictionary<FrameworkElement, double>();
    if (movingElements != null)
    {
      foreach (FrameworkElement movingElement in movingElements)
      {
        if (movingElement.Parent is ExtendedVisualStateManager.WrapperCanvas parent)
          oldOpacities.Add(movingElement, parent.Opacity);
      }
    }
    for (int index = originalValueRecords.Count - 1; index >= 0; --index)
    {
      ExtendedVisualStateManager.OriginalLayoutValueRecord originalValueRecord = originalValueRecords[index];
      double num;
      if (ExtendedVisualStateManager.IsVisibilityProperty(originalValueRecord.Property) && !oldOpacities.TryGetValue(originalValueRecord.Element, out num))
      {
        num = (Visibility) originalValueRecord.Element.GetValue(originalValueRecord.Property) == Visibility.Visible ? 1.0 : 0.0;
        oldOpacities.Add(originalValueRecord.Element, num);
      }
    }
    foreach (Timeline child in layoutStoryboard.Children)
    {
      FrameworkElement timelineTarget = (FrameworkElement) ExtendedVisualStateManager.GetTimelineTarget(control, templateRoot, child);
      DependencyProperty dependencyProperty = ExtendedVisualStateManager.LayoutPropertyFromTimeline(child, true);
      double num;
      if (timelineTarget != null && ExtendedVisualStateManager.IsVisibilityProperty(dependencyProperty) && !oldOpacities.TryGetValue(timelineTarget, out num))
      {
        num = (Visibility) timelineTarget.GetValue(dependencyProperty) == Visibility.Visible ? 1.0 : 0.0;
        oldOpacities.Add(timelineTarget, num);
      }
    }
    return oldOpacities;
  }

  internal static List<ExtendedVisualStateManager.OriginalLayoutValueRecord> GetOriginalLayoutValues(
    DependencyObject obj)
  {
    return (List<ExtendedVisualStateManager.OriginalLayoutValueRecord>) obj.GetValue(ExtendedVisualStateManager.OriginalLayoutValuesProperty);
  }

  private static Dictionary<FrameworkElement, Rect> GetRectsOfTargets(
    IEnumerable<FrameworkElement> targets,
    ICollection<FrameworkElement> movingElements)
  {
    Dictionary<FrameworkElement, Rect> rectsOfTargets = new Dictionary<FrameworkElement, Rect>();
    foreach (FrameworkElement target in targets)
    {
      Rect rect;
      if (movingElements != null && movingElements.Contains(target) && target.Parent is ExtendedVisualStateManager.WrapperCanvas parent)
      {
        rect = ExtendedVisualStateManager.GetLayoutRect((FrameworkElement) parent);
        TranslateTransform renderTransform = parent.RenderTransform as TranslateTransform;
        double left = Canvas.GetLeft((UIElement) target);
        double top = Canvas.GetTop((UIElement) target);
        rect = new Rect(rect.Left + (double.IsNaN(left) ? 0.0 : left) + (renderTransform != null ? renderTransform.X : 0.0), rect.Top + (double.IsNaN(top) ? 0.0 : top) + (renderTransform != null ? renderTransform.Y : 0.0), target.ActualWidth, target.ActualHeight);
      }
      else
        rect = ExtendedVisualStateManager.GetLayoutRect(target);
      rectsOfTargets.Add(target, rect);
    }
    return rectsOfTargets;
  }

  public static DependencyProperty GetRuntimeVisibilityProperty(DependencyObject obj)
  {
    return (DependencyProperty) obj.GetValue(ExtendedVisualStateManager.RuntimeVisibilityPropertyProperty);
  }

  private static object GetTimelineTarget(
    FrameworkElement control,
    FrameworkElement templateRoot,
    Timeline timeline)
  {
    string targetName = Storyboard.GetTargetName((DependencyObject) timeline);
    if (string.IsNullOrEmpty(targetName))
      return (object) null;
    return control is UserControl ? control.FindName(targetName) : templateRoot.FindName(targetName);
  }

  public static TransitionEffect GetTransitionEffect(DependencyObject obj)
  {
    return (TransitionEffect) obj.GetValue(ExtendedVisualStateManager.TransitionEffectProperty);
  }

  internal static Storyboard GetTransitionEffectStoryboard(DependencyObject obj)
  {
    return (Storyboard) obj.GetValue(ExtendedVisualStateManager.TransitionEffectStoryboardProperty);
  }

  public static bool GetUseFluidLayout(DependencyObject obj)
  {
    return (bool) obj.GetValue(ExtendedVisualStateManager.UseFluidLayoutProperty);
  }

  private static object GetValueFromTimeline(Timeline timeline, out bool gotValue)
  {
    switch (timeline)
    {
      case ObjectAnimationUsingKeyFrames animationUsingKeyFrames1:
        gotValue = true;
        return animationUsingKeyFrames1.KeyFrames[0].Value;
      case DoubleAnimationUsingKeyFrames animationUsingKeyFrames2:
        gotValue = true;
        return (object) animationUsingKeyFrames2.KeyFrames[0].Value;
      case DoubleAnimation doubleAnimation:
        gotValue = true;
        return (object) doubleAnimation.To;
      case ThicknessAnimationUsingKeyFrames animationUsingKeyFrames3:
        gotValue = true;
        return (object) animationUsingKeyFrames3.KeyFrames[0].Value;
      case ThicknessAnimation thicknessAnimation:
        gotValue = true;
        return (object) thicknessAnimation.To;
      case Int32AnimationUsingKeyFrames animationUsingKeyFrames4:
        gotValue = true;
        return (object) animationUsingKeyFrames4.KeyFrames[0].Value;
      case Int32Animation int32Animation:
        gotValue = true;
        return (object) int32Animation.To;
      default:
        gotValue = false;
        return (object) null;
    }
  }

  protected override bool GoToStateCore(
    FrameworkElement control,
    FrameworkElement stateGroupsRoot,
    string stateName,
    VisualStateGroup group,
    VisualState state,
    bool useTransitions)
  {
    if (this._changingState)
      return false;
    VisualState currentState = ExtendedVisualStateManager.GetCurrentState((DependencyObject) group);
    if (!object.Equals((object) currentState, (object) state))
    {
      VisualTransition transition = ExtendedVisualStateManager.FindTransition(group, currentState, state);
      bool animateWithTransitionEffect = ExtendedVisualStateManager.PrepareTransitionEffectImage(stateGroupsRoot, useTransitions, transition);
      if (!ExtendedVisualStateManager.GetUseFluidLayout((DependencyObject) group))
        return this.TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions, transition, animateWithTransitionEffect, currentState);
      Storyboard layoutStoryboard = ExtendedVisualStateManager.ExtractLayoutStoryboard(state);
      List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords = ExtendedVisualStateManager.GetOriginalLayoutValues((DependencyObject) group);
      if (originalValueRecords == null)
      {
        originalValueRecords = new List<ExtendedVisualStateManager.OriginalLayoutValueRecord>();
        ExtendedVisualStateManager.SetOriginalLayoutValues((DependencyObject) group, originalValueRecords);
      }
      if (!useTransitions)
      {
        if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
          ExtendedVisualStateManager.StopAnimations();
        int num = this.TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, false, transition, animateWithTransitionEffect, currentState) ? 1 : 0;
        ExtendedVisualStateManager.SetLayoutStoryboardProperties(control, stateGroupsRoot, layoutStoryboard, originalValueRecords);
        return num != 0;
      }
      if (layoutStoryboard.Children.Count == 0 && originalValueRecords.Count == 0)
        return this.TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, true, transition, animateWithTransitionEffect, currentState);
      try
      {
        this._changingState = true;
        stateGroupsRoot.UpdateLayout();
        List<FrameworkElement> targetElements = ExtendedVisualStateManager.FindTargetElements(control, stateGroupsRoot, layoutStoryboard, originalValueRecords, ExtendedVisualStateManager.MovingElements);
        Dictionary<FrameworkElement, Rect> rectsOfTargets1 = ExtendedVisualStateManager.GetRectsOfTargets((IEnumerable<FrameworkElement>) targetElements, (ICollection<FrameworkElement>) ExtendedVisualStateManager.MovingElements);
        Dictionary<FrameworkElement, double> oldOpacities = ExtendedVisualStateManager.GetOldOpacities(control, stateGroupsRoot, layoutStoryboard, originalValueRecords, ExtendedVisualStateManager.MovingElements);
        if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          stateGroupsRoot.LayoutUpdated -= ExtendedVisualStateManager.\u003C\u003EO.\u003C0\u003E__control_LayoutUpdated ?? (ExtendedVisualStateManager.\u003C\u003EO.\u003C0\u003E__control_LayoutUpdated = new EventHandler(ExtendedVisualStateManager.control_LayoutUpdated));
          ExtendedVisualStateManager.StopAnimations();
          stateGroupsRoot.UpdateLayout();
        }
        this.TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, true, transition, animateWithTransitionEffect, currentState);
        ExtendedVisualStateManager.SetLayoutStoryboardProperties(control, stateGroupsRoot, layoutStoryboard, originalValueRecords);
        stateGroupsRoot.UpdateLayout();
        Dictionary<FrameworkElement, Rect> rectsOfTargets2 = ExtendedVisualStateManager.GetRectsOfTargets((IEnumerable<FrameworkElement>) targetElements, (ICollection<FrameworkElement>) null);
        ExtendedVisualStateManager.MovingElements = new List<FrameworkElement>();
        foreach (FrameworkElement key in targetElements)
        {
          if (rectsOfTargets1[key] != rectsOfTargets2[key])
            ExtendedVisualStateManager.MovingElements.Add(key);
        }
        foreach (FrameworkElement key in oldOpacities.Keys)
        {
          if (!ExtendedVisualStateManager.MovingElements.Contains(key))
            ExtendedVisualStateManager.MovingElements.Add(key);
        }
        ExtendedVisualStateManager.WrapMovingElementsInCanvases(ExtendedVisualStateManager.MovingElements, rectsOfTargets1, rectsOfTargets2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        stateGroupsRoot.LayoutUpdated += ExtendedVisualStateManager.\u003C\u003EO.\u003C0\u003E__control_LayoutUpdated ?? (ExtendedVisualStateManager.\u003C\u003EO.\u003C0\u003E__control_LayoutUpdated = new EventHandler(ExtendedVisualStateManager.control_LayoutUpdated));
        ExtendedVisualStateManager.LayoutTransitionStoryboard = ExtendedVisualStateManager.CreateLayoutTransitionStoryboard(transition, ExtendedVisualStateManager.MovingElements, oldOpacities);
        ExtendedVisualStateManager.LayoutTransitionStoryboard.Completed += new EventHandler(Handler);
        ExtendedVisualStateManager.LayoutTransitionStoryboard.Begin();
      }
      finally
      {
        this._changingState = false;
      }
    }
    return true;

    void Handler(object sender, EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      stateGroupsRoot.LayoutUpdated -= ExtendedVisualStateManager.\u003C\u003EO.\u003C0\u003E__control_LayoutUpdated ?? (ExtendedVisualStateManager.\u003C\u003EO.\u003C0\u003E__control_LayoutUpdated = new EventHandler(ExtendedVisualStateManager.control_LayoutUpdated));
      ExtendedVisualStateManager.StopAnimations();
    }
  }

  private static bool IsClose(double a, double b) => Math.Abs(a - b) < 1E-07;

  private static bool IsVisibilityProperty(DependencyProperty property)
  {
    return property == UIElement.VisibilityProperty || property.Name == "RuntimeVisibility";
  }

  private static DependencyProperty LayoutPropertyFromTimeline(
    Timeline timeline,
    bool forceRuntimeProperty)
  {
    PropertyPath targetProperty = Storyboard.GetTargetProperty((DependencyObject) timeline);
    if (targetProperty != null && targetProperty.PathParameters.Count != 0 && targetProperty.PathParameters[0] is DependencyProperty pathParameter)
    {
      if (pathParameter.Name == "RuntimeVisibility" && pathParameter.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
      {
        if (!ExtendedVisualStateManager.LayoutProperties.Contains(pathParameter))
          ExtendedVisualStateManager.LayoutProperties.Add(pathParameter);
        return !forceRuntimeProperty ? UIElement.VisibilityProperty : pathParameter;
      }
      if (pathParameter.Name == "RuntimeWidth" && pathParameter.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
      {
        if (!ExtendedVisualStateManager.LayoutProperties.Contains(pathParameter))
          ExtendedVisualStateManager.LayoutProperties.Add(pathParameter);
        return !forceRuntimeProperty ? FrameworkElement.WidthProperty : pathParameter;
      }
      if (pathParameter.Name == "RuntimeHeight" && pathParameter.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
      {
        if (!ExtendedVisualStateManager.LayoutProperties.Contains(pathParameter))
          ExtendedVisualStateManager.LayoutProperties.Add(pathParameter);
        return !forceRuntimeProperty ? FrameworkElement.HeightProperty : pathParameter;
      }
      if (ExtendedVisualStateManager.LayoutProperties.Contains(pathParameter))
        return pathParameter;
    }
    return (DependencyProperty) null;
  }

  private static bool PrepareTransitionEffectImage(
    FrameworkElement stateGroupsRoot,
    bool useTransitions,
    VisualTransition transition)
  {
    TransitionEffect transitionEffect1 = transition == null ? (TransitionEffect) null : ExtendedVisualStateManager.GetTransitionEffect((DependencyObject) transition);
    bool flag = false;
    if (transitionEffect1 != null)
    {
      TransitionEffect transitionEffect2 = transitionEffect1.CloneCurrentValue();
      if (useTransitions)
      {
        flag = true;
        RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) Math.Max(1.0, stateGroupsRoot.ActualWidth), (int) Math.Max(1.0, stateGroupsRoot.ActualHeight), 96.0, 96.0, PixelFormats.Pbgra32);
        renderTargetBitmap.Render((Visual) stateGroupsRoot);
        ImageBrush imageBrush = new ImageBrush()
        {
          ImageSource = (ImageSource) renderTargetBitmap
        };
        transitionEffect2.OldImage = (Brush) imageBrush;
      }
      Storyboard effectStoryboard = ExtendedVisualStateManager.GetTransitionEffectStoryboard((DependencyObject) stateGroupsRoot);
      if (effectStoryboard != null)
      {
        effectStoryboard.Stop();
        ExtendedVisualStateManager.FinishTransitionEffectAnimation(stateGroupsRoot);
      }
      if (useTransitions)
      {
        ExtendedVisualStateManager.TransferLocalValue(stateGroupsRoot, UIElement.EffectProperty, ExtendedVisualStateManager.CachedEffectProperty);
        stateGroupsRoot.Effect = (Effect) transitionEffect2;
      }
    }
    return flag;
  }

  private static void ReplaceCachedLocalValueHelper(
    FrameworkElement element,
    DependencyProperty property,
    object value)
  {
    if (value == DependencyProperty.UnsetValue)
      element.ClearValue(property);
    else if (value is BindingExpressionBase bindingExpressionBase)
      element.SetBinding(property, bindingExpressionBase.ParentBindingBase);
    else
      element.SetValue(property, value);
  }

  internal static void SetCachedBackground(DependencyObject obj, object value)
  {
    obj.SetValue(ExtendedVisualStateManager.CachedBackgroundProperty, value);
  }

  internal static void SetCachedEffect(DependencyObject obj, Effect value)
  {
    obj.SetValue(ExtendedVisualStateManager.CachedEffectProperty, (object) value);
  }

  internal static void SetCurrentState(DependencyObject obj, VisualState value)
  {
    obj.SetValue(ExtendedVisualStateManager.CurrentStateProperty, (object) value);
  }

  internal static void SetDidCacheBackground(DependencyObject obj, bool value)
  {
    obj.SetValue(ExtendedVisualStateManager.DidCacheBackgroundProperty, ValueBoxes.BooleanBox(value));
  }

  internal static void SetLayoutStoryboard(DependencyObject obj, Storyboard value)
  {
    obj.SetValue(ExtendedVisualStateManager.LayoutStoryboardProperty, (object) value);
  }

  private static void SetLayoutStoryboardProperties(
    FrameworkElement control,
    FrameworkElement templateRoot,
    Storyboard layoutStoryboard,
    List<ExtendedVisualStateManager.OriginalLayoutValueRecord> originalValueRecords)
  {
    foreach (ExtendedVisualStateManager.OriginalLayoutValueRecord originalValueRecord in originalValueRecords)
      ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(originalValueRecord.Element, originalValueRecord.Property, originalValueRecord.Value);
    originalValueRecords.Clear();
    foreach (Timeline child in layoutStoryboard.Children)
    {
      FrameworkElement timelineTarget = (FrameworkElement) ExtendedVisualStateManager.GetTimelineTarget(control, templateRoot, child);
      DependencyProperty dependencyProperty = ExtendedVisualStateManager.LayoutPropertyFromTimeline(child, true);
      if (timelineTarget != null && dependencyProperty != null)
      {
        bool gotValue;
        object valueFromTimeline = ExtendedVisualStateManager.GetValueFromTimeline(child, out gotValue);
        if (gotValue)
        {
          ExtendedVisualStateManager.OriginalLayoutValueRecord layoutValueRecord = new ExtendedVisualStateManager.OriginalLayoutValueRecord()
          {
            Element = timelineTarget,
            Property = dependencyProperty,
            Value = ExtendedVisualStateManager.CacheLocalValueHelper((DependencyObject) timelineTarget, dependencyProperty)
          };
          originalValueRecords.Add(layoutValueRecord);
          timelineTarget.SetValue(dependencyProperty, valueFromTimeline);
        }
      }
    }
  }

  internal static void SetOriginalLayoutValues(
    DependencyObject obj,
    List<ExtendedVisualStateManager.OriginalLayoutValueRecord> value)
  {
    obj.SetValue(ExtendedVisualStateManager.OriginalLayoutValuesProperty, (object) value);
  }

  public static void SetRuntimeVisibilityProperty(DependencyObject obj, DependencyProperty value)
  {
    obj.SetValue(ExtendedVisualStateManager.RuntimeVisibilityPropertyProperty, (object) value);
  }

  public static void SetTransitionEffect(DependencyObject obj, TransitionEffect value)
  {
    obj.SetValue(ExtendedVisualStateManager.TransitionEffectProperty, (object) value);
  }

  internal static void SetTransitionEffectStoryboard(DependencyObject obj, Storyboard value)
  {
    obj.SetValue(ExtendedVisualStateManager.TransitionEffectStoryboardProperty, (object) value);
  }

  public static void SetUseFluidLayout(DependencyObject obj, bool value)
  {
    obj.SetValue(ExtendedVisualStateManager.UseFluidLayoutProperty, ValueBoxes.BooleanBox(value));
  }

  private static void StopAnimations()
  {
    if (ExtendedVisualStateManager.LayoutTransitionStoryboard != null)
    {
      ExtendedVisualStateManager.LayoutTransitionStoryboard.Stop();
      ExtendedVisualStateManager.LayoutTransitionStoryboard = (Storyboard) null;
    }
    if (ExtendedVisualStateManager.MovingElements == null)
      return;
    ExtendedVisualStateManager.UnwrapMovingElementsFromCanvases(ExtendedVisualStateManager.MovingElements);
    ExtendedVisualStateManager.MovingElements = (List<FrameworkElement>) null;
  }

  private static bool TimelineIsAnimatingRootOpacity(
    Timeline timeline,
    FrameworkElement control,
    FrameworkElement stateGroupsRoot)
  {
    if (!object.Equals(ExtendedVisualStateManager.GetTimelineTarget(control, stateGroupsRoot, timeline), (object) stateGroupsRoot))
      return false;
    PropertyPath targetProperty = Storyboard.GetTargetProperty((DependencyObject) timeline);
    return targetProperty != null && targetProperty.PathParameters.Count != 0 && targetProperty.PathParameters[0] == UIElement.OpacityProperty;
  }

  private static void TransferLocalValue(
    FrameworkElement element,
    DependencyProperty sourceProperty,
    DependencyProperty destProperty)
  {
    object obj = ExtendedVisualStateManager.CacheLocalValueHelper((DependencyObject) element, sourceProperty);
    ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(element, destProperty, obj);
  }

  private bool TransitionEffectAwareGoToStateCore(
    FrameworkElement control,
    FrameworkElement stateGroupsRoot,
    string stateName,
    VisualStateGroup group,
    VisualState state,
    bool useTransitions,
    VisualTransition transition,
    bool animateWithTransitionEffect,
    VisualState previousState)
  {
    IEasingFunction easingFunction = (IEasingFunction) null;
    if (animateWithTransitionEffect)
    {
      easingFunction = transition.GeneratedEasingFunction;
      ExtendedVisualStateManager.DummyEasingFunction dummyEasingFunction = new ExtendedVisualStateManager.DummyEasingFunction()
      {
        DummyValue = ExtendedVisualStateManager.FinishesWithZeroOpacity(control, stateGroupsRoot, state, previousState) ? 0.01 : 0.0
      };
      transition.GeneratedEasingFunction = (IEasingFunction) dummyEasingFunction;
    }
    bool stateCore = base.GoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions);
    if (animateWithTransitionEffect)
    {
      transition.GeneratedEasingFunction = easingFunction;
      if (stateCore)
        ExtendedVisualStateManager.AnimateTransitionEffect(stateGroupsRoot, transition);
    }
    ExtendedVisualStateManager.SetCurrentState((DependencyObject) group, state);
    return stateCore;
  }

  private static void UnwrapMovingElementsFromCanvases(List<FrameworkElement> movingElements)
  {
    foreach (FrameworkElement movingElement in movingElements)
    {
      if (movingElement.Parent is ExtendedVisualStateManager.WrapperCanvas parent1)
      {
        object obj = ExtendedVisualStateManager.CacheLocalValueHelper((DependencyObject) movingElement, FrameworkElement.DataContextProperty);
        movingElement.DataContext = movingElement.DataContext;
        FrameworkElement parent = VisualTreeHelper.GetParent((DependencyObject) parent1) as FrameworkElement;
        parent1.Children.Remove((UIElement) movingElement);
        switch (parent)
        {
          case Panel panel:
            int index = panel.Children.IndexOf((UIElement) parent1);
            panel.Children.RemoveAt(index);
            panel.Children.Insert(index, (UIElement) movingElement);
            break;
          case Decorator decorator:
            decorator.Child = (UIElement) movingElement;
            break;
        }
        ExtendedVisualStateManager.CopyLayoutProperties((FrameworkElement) parent1, movingElement, true);
        ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(movingElement, FrameworkElement.DataContextProperty, obj);
      }
    }
  }

  private static void WrapMovingElementsInCanvases(
    List<FrameworkElement> movingElements,
    Dictionary<FrameworkElement, Rect> oldRects,
    Dictionary<FrameworkElement, Rect> newRects)
  {
    foreach (FrameworkElement movingElement in movingElements)
    {
      FrameworkElement parent = VisualTreeHelper.GetParent((DependencyObject) movingElement) as FrameworkElement;
      ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = new ExtendedVisualStateManager.WrapperCanvas()
      {
        OldRect = oldRects[movingElement],
        NewRect = newRects[movingElement]
      };
      object obj = ExtendedVisualStateManager.CacheLocalValueHelper((DependencyObject) movingElement, FrameworkElement.DataContextProperty);
      movingElement.DataContext = movingElement.DataContext;
      bool flag = true;
      switch (parent)
      {
        case Panel panel when !panel.IsItemsHost:
          int index = panel.Children.IndexOf((UIElement) movingElement);
          panel.Children.RemoveAt(index);
          panel.Children.Insert(index, (UIElement) wrapperCanvas);
          break;
        case Decorator decorator:
          decorator.Child = (UIElement) wrapperCanvas;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        wrapperCanvas.Children.Add((UIElement) movingElement);
        ExtendedVisualStateManager.CopyLayoutProperties(movingElement, (FrameworkElement) wrapperCanvas, false);
        ExtendedVisualStateManager.ReplaceCachedLocalValueHelper(movingElement, FrameworkElement.DataContextProperty, obj);
      }
    }
  }

  private class DummyEasingFunction : EasingFunctionBase
  {
    public static readonly DependencyProperty DummyValueProperty = DependencyProperty.Register(nameof (DummyValue), typeof (double), typeof (ExtendedVisualStateManager.DummyEasingFunction), new PropertyMetadata((object) 0.0));

    public double DummyValue
    {
      private get
      {
        return (double) this.GetValue(ExtendedVisualStateManager.DummyEasingFunction.DummyValueProperty);
      }
      set
      {
        this.SetValue(ExtendedVisualStateManager.DummyEasingFunction.DummyValueProperty, (object) value);
      }
    }

    protected override Freezable CreateInstanceCore()
    {
      return (Freezable) new ExtendedVisualStateManager.DummyEasingFunction();
    }

    protected override double EaseInCore(double normalizedTime) => this.DummyValue;
  }

  internal class OriginalLayoutValueRecord
  {
    public FrameworkElement Element { get; set; }

    public DependencyProperty Property { get; set; }

    public object Value { get; set; }
  }

  internal class WrapperCanvas : Canvas
  {
    internal static readonly DependencyProperty SimulationProgressProperty = DependencyProperty.Register(nameof (SimulationProgress), typeof (double), typeof (ExtendedVisualStateManager.WrapperCanvas), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressChanged)));

    public Visibility DestinationVisibilityCache { get; set; }

    public Dictionary<DependencyProperty, object> LocalValueCache { get; set; }

    public Rect NewRect { get; set; }

    public Rect OldRect { get; set; }

    public double SimulationProgress
    {
      get
      {
        return (double) this.GetValue(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressProperty);
      }
      set
      {
        this.SetValue(ExtendedVisualStateManager.WrapperCanvas.SimulationProgressProperty, (object) value);
      }
    }

    private static void SimulationProgressChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ExtendedVisualStateManager.WrapperCanvas wrapperCanvas = d as ExtendedVisualStateManager.WrapperCanvas;
      double newValue = (double) e.NewValue;
      if (wrapperCanvas == null || wrapperCanvas.Children.Count <= 0 || !(wrapperCanvas.Children[0] is FrameworkElement child))
        return;
      child.Width = Math.Max(0.0, wrapperCanvas.OldRect.Width * newValue + wrapperCanvas.NewRect.Width * (1.0 - newValue));
      FrameworkElement frameworkElement = child;
      Rect rect1 = wrapperCanvas.OldRect;
      double num1 = rect1.Height * newValue;
      rect1 = wrapperCanvas.NewRect;
      double num2 = rect1.Height * (1.0 - newValue);
      double num3 = Math.Max(0.0, num1 + num2);
      frameworkElement.Height = num3;
      FrameworkElement element1 = child;
      double num4 = newValue;
      Rect rect2 = wrapperCanvas.OldRect;
      double left1 = rect2.Left;
      rect2 = wrapperCanvas.NewRect;
      double left2 = rect2.Left;
      double num5 = left1 - left2;
      double length1 = num4 * num5;
      Canvas.SetLeft((UIElement) element1, length1);
      FrameworkElement element2 = child;
      double num6 = newValue;
      rect2 = wrapperCanvas.OldRect;
      double num7 = rect2.Top - wrapperCanvas.NewRect.Top;
      double length2 = num6 * num7;
      Canvas.SetTop((UIElement) element2, length2);
    }
  }
}
