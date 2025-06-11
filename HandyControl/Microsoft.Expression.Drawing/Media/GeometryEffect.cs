// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.GeometryEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Expression.Media;

[TypeConverter(typeof (GeometryEffectConverter))]
public abstract class GeometryEffect : Freezable
{
  private static GeometryEffect defaultGeometryEffect;
  public static readonly DependencyProperty GeometryEffectProperty = DependencyProperty.RegisterAttached(nameof (GeometryEffect), typeof (GeometryEffect), typeof (GeometryEffect), (PropertyMetadata) new DrawingPropertyMetadata((object) GeometryEffect.DefaultGeometryEffect, DrawingPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(GeometryEffect.OnGeometryEffectChanged)));
  protected Geometry CachedGeometry;
  private bool _effectInvalidated;

  static GeometryEffect()
  {
    DrawingPropertyMetadata.DrawingPropertyChanged += (EventHandler<DrawingPropertyChangedEventArgs>) ((sender, args) =>
    {
      if (!(sender is GeometryEffect geometryEffect2) || !args.Metadata.AffectsRender)
        return;
      geometryEffect2.InvalidateGeometry(InvalidateGeometryReasons.PropertyChanged);
    });
  }

  public static GeometryEffect DefaultGeometryEffect
  {
    get
    {
      return GeometryEffect.defaultGeometryEffect ?? (GeometryEffect.defaultGeometryEffect = (GeometryEffect) new GeometryEffect.NoGeometryEffect());
    }
  }

  public Geometry OutputGeometry => this.CachedGeometry;

  protected internal DependencyObject Parent { get; private set; }

  protected internal virtual void Attach(DependencyObject obj)
  {
    if (this.Parent != null)
      this.Detach();
    this._effectInvalidated = true;
    this.CachedGeometry = (Geometry) null;
    if (!GeometryEffect.InvalidateParent(obj))
      return;
    this.Parent = obj;
  }

  public GeometryEffect CloneCurrentValue() => (GeometryEffect) base.CloneCurrentValue();

  protected override Freezable CreateInstanceCore()
  {
    return (Freezable) Activator.CreateInstance(this.GetType());
  }

  protected abstract GeometryEffect DeepCopy();

  protected internal virtual void Detach()
  {
    this._effectInvalidated = true;
    this.CachedGeometry = (Geometry) null;
    if (this.Parent == null)
      return;
    GeometryEffect.InvalidateParent(this.Parent);
    this.Parent = (DependencyObject) null;
  }

  public abstract bool Equals(GeometryEffect geometryEffect);

  public static GeometryEffect GetGeometryEffect(DependencyObject obj)
  {
    return (GeometryEffect) obj.GetValue(GeometryEffect.GeometryEffectProperty);
  }

  public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
  {
    if (this._effectInvalidated)
      return false;
    this._effectInvalidated = true;
    if (reasons != InvalidateGeometryReasons.ParentInvalidated)
      GeometryEffect.InvalidateParent(this.Parent);
    return true;
  }

  private static bool InvalidateParent(DependencyObject parent)
  {
    switch (parent)
    {
      case IShape shape:
        shape.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
        return true;
      case GeometryEffect geometryEffect:
        geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
        return true;
      default:
        return false;
    }
  }

  private static void OnGeometryEffectChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    GeometryEffect oldValue = e.OldValue as GeometryEffect;
    GeometryEffect newValue = e.NewValue as GeometryEffect;
    if (object.Equals((object) oldValue, (object) newValue))
      return;
    if (oldValue != null && obj.Equals((object) oldValue.Parent))
      oldValue.Detach();
    if (newValue == null)
      return;
    if (newValue.Parent != null)
      obj.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        GeometryEffect geometryEffect = newValue.CloneCurrentValue();
        obj.SetValue(GeometryEffect.GeometryEffectProperty, (object) geometryEffect);
      }), DispatcherPriority.Send, (object[]) null);
    else
      newValue.Attach(obj);
  }

  public bool ProcessGeometry(Geometry input)
  {
    bool flag = false;
    if (this._effectInvalidated)
    {
      flag |= this.UpdateCachedGeometry(input);
      this._effectInvalidated = false;
    }
    return flag;
  }

  public static void SetGeometryEffect(DependencyObject obj, GeometryEffect value)
  {
    obj.SetValue(GeometryEffect.GeometryEffectProperty, (object) value);
  }

  protected abstract bool UpdateCachedGeometry(Geometry input);

  private class NoGeometryEffect : GeometryEffect
  {
    protected override GeometryEffect DeepCopy()
    {
      return (GeometryEffect) new GeometryEffect.NoGeometryEffect();
    }

    public override bool Equals(GeometryEffect geometryEffect)
    {
      return geometryEffect == null || geometryEffect is GeometryEffect.NoGeometryEffect;
    }

    protected override bool UpdateCachedGeometry(Geometry input)
    {
      this.CachedGeometry = input;
      return false;
    }
  }
}
