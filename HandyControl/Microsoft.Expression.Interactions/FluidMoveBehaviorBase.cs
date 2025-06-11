// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.FluidMoveBehaviorBase
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

#nullable disable
namespace HandyControl.Interactivity;

public abstract class FluidMoveBehaviorBase : Behavior<FrameworkElement>
{
  public static readonly DependencyProperty AppliesToProperty = DependencyProperty.Register(nameof (AppliesTo), typeof (FluidMoveScope), typeof (FluidMoveBehaviorBase), new PropertyMetadata((object) FluidMoveScope.Self));
  protected static readonly DependencyProperty IdentityTagProperty = DependencyProperty.RegisterAttached("IdentityTag", typeof (object), typeof (FluidMoveBehaviorBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof (IsActive), typeof (bool), typeof (FluidMoveBehaviorBase), new PropertyMetadata(ValueBoxes.TrueBox));
  private static DateTime LastPurgeTick = DateTime.MinValue;
  private static readonly TimeSpan MinTickDelta = TimeSpan.FromSeconds(0.5);
  private static DateTime NextToLastPurgeTick = DateTime.MinValue;
  internal static Dictionary<object, FluidMoveBehaviorBase.TagData> TagDictionary = new Dictionary<object, FluidMoveBehaviorBase.TagData>();
  public static readonly DependencyProperty TagPathProperty = DependencyProperty.Register(nameof (TagPath), typeof (string), typeof (FluidMoveBehaviorBase), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty TagProperty = DependencyProperty.Register(nameof (Tag), typeof (TagType), typeof (FluidMoveBehaviorBase), new PropertyMetadata((object) TagType.Element));

  public FluidMoveScope AppliesTo
  {
    get => (FluidMoveScope) this.GetValue(FluidMoveBehaviorBase.AppliesToProperty);
    set => this.SetValue(FluidMoveBehaviorBase.AppliesToProperty, (object) value);
  }

  public bool IsActive
  {
    get => (bool) this.GetValue(FluidMoveBehaviorBase.IsActiveProperty);
    set => this.SetValue(FluidMoveBehaviorBase.IsActiveProperty, ValueBoxes.BooleanBox(value));
  }

  protected virtual bool ShouldSkipInitialLayout => this.Tag == TagType.DataContext;

  public TagType Tag
  {
    get => (TagType) this.GetValue(FluidMoveBehaviorBase.TagProperty);
    set => this.SetValue(FluidMoveBehaviorBase.TagProperty, (object) value);
  }

  public string TagPath
  {
    get => (string) this.GetValue(FluidMoveBehaviorBase.TagPathProperty);
    set => this.SetValue(FluidMoveBehaviorBase.TagPathProperty, (object) value);
  }

  private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
  {
    if (!this.IsActive)
      return;
    if (DateTime.Now - FluidMoveBehaviorBase.LastPurgeTick >= FluidMoveBehaviorBase.MinTickDelta)
    {
      List<object> objectList = (List<object>) null;
      foreach (KeyValuePair<object, FluidMoveBehaviorBase.TagData> tag in FluidMoveBehaviorBase.TagDictionary)
      {
        if (tag.Value.Timestamp < FluidMoveBehaviorBase.NextToLastPurgeTick)
        {
          if (objectList == null)
            objectList = new List<object>();
          objectList.Add(tag.Key);
        }
      }
      if (objectList != null)
      {
        foreach (object key in objectList)
          FluidMoveBehaviorBase.TagDictionary.Remove(key);
      }
      FluidMoveBehaviorBase.NextToLastPurgeTick = FluidMoveBehaviorBase.LastPurgeTick;
      FluidMoveBehaviorBase.LastPurgeTick = DateTime.Now;
    }
    if (this.AppliesTo == FluidMoveScope.Self)
    {
      this.UpdateLayoutTransition(this.AssociatedObject);
    }
    else
    {
      if (!(this.AssociatedObject is Panel associatedObject))
        return;
      foreach (FrameworkElement child in associatedObject.Children)
        this.UpdateLayoutTransition(child);
    }
  }

  protected virtual void EnsureTags(FrameworkElement child)
  {
    if (this.Tag != TagType.DataContext || child.ReadLocalValue(FluidMoveBehaviorBase.IdentityTagProperty) is BindingExpression)
      return;
    child.SetBinding(FluidMoveBehaviorBase.IdentityTagProperty, (BindingBase) new Binding(this.TagPath));
  }

  protected static object GetIdentityTag(DependencyObject obj)
  {
    return obj.GetValue(FluidMoveBehaviorBase.IdentityTagProperty);
  }

  private static FrameworkElement GetVisualRoot(FrameworkElement child)
  {
    while (VisualTreeHelper.GetParent((DependencyObject) child) is FrameworkElement parent && AdornerLayer.GetAdornerLayer((Visual) parent) != null)
      child = parent;
    return child;
  }

  protected override void OnAttached()
  {
    base.OnAttached();
    this.AssociatedObject.LayoutUpdated += new EventHandler(this.AssociatedObject_LayoutUpdated);
  }

  protected override void OnDetaching()
  {
    base.OnDetaching();
    this.AssociatedObject.LayoutUpdated -= new EventHandler(this.AssociatedObject_LayoutUpdated);
  }

  protected static void SetIdentityTag(DependencyObject obj, object value)
  {
    obj.SetValue(FluidMoveBehaviorBase.IdentityTagProperty, value);
  }

  internal static Rect TranslateRect(Rect rect, FrameworkElement from, FrameworkElement to)
  {
    if (from == null || to == null)
      return rect;
    Point point1 = new Point(rect.Left, rect.Top);
    Point point2 = from.TransformToVisual((Visual) to).Transform(point1);
    return new Rect(point2.X, point2.Y, rect.Width, rect.Height);
  }

  private void UpdateLayoutTransition(FrameworkElement child)
  {
    if ((child.Visibility == Visibility.Collapsed || !child.IsLoaded) && this.ShouldSkipInitialLayout)
      return;
    FrameworkElement visualRoot = FluidMoveBehaviorBase.GetVisualRoot(child);
    FluidMoveBehaviorBase.TagData newTagData = new FluidMoveBehaviorBase.TagData()
    {
      Parent = VisualTreeHelper.GetParent((DependencyObject) child) as FrameworkElement,
      ParentRect = ExtendedVisualStateManager.GetLayoutRect(child),
      Child = child,
      Timestamp = DateTime.Now
    };
    try
    {
      newTagData.AppRect = FluidMoveBehaviorBase.TranslateRect(newTagData.ParentRect, newTagData.Parent, visualRoot);
    }
    catch (ArgumentException ex)
    {
      if (this.ShouldSkipInitialLayout)
        return;
    }
    this.EnsureTags(child);
    object tag = FluidMoveBehaviorBase.GetIdentityTag((DependencyObject) child) ?? (object) child;
    this.UpdateLayoutTransitionCore(child, visualRoot, tag, newTagData);
  }

  internal abstract void UpdateLayoutTransitionCore(
    FrameworkElement child,
    FrameworkElement root,
    object tag,
    FluidMoveBehaviorBase.TagData newTagData);

  internal class TagData
  {
    public Rect AppRect { get; set; }

    public FrameworkElement Child { get; set; }

    public object InitialTag { get; set; }

    public FrameworkElement Parent { get; set; }

    public Rect ParentRect { get; set; }

    public DateTime Timestamp { get; set; }
  }
}
