// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.GeometrySource`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Media;

public abstract class GeometrySource<TParameters> : IGeometrySource where TParameters : IGeometrySourceParameters
{
  private bool _geometryInvalidated;
  protected Geometry CachedGeometry;

  public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
  {
    if ((reasons & InvalidateGeometryReasons.TemplateChanged) != (InvalidateGeometryReasons) 0)
      this.CachedGeometry = (Geometry) null;
    if (this._geometryInvalidated)
      return false;
    this._geometryInvalidated = true;
    return true;
  }

  public bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds)
  {
    bool flag = false;
    if (parameters is TParameters parameters1)
    {
      Rect logicalBounds = this.ComputeLogicalBounds(layoutBounds, (IGeometrySourceParameters) parameters1);
      flag = ((flag ? 1 : 0) | (this.LayoutBounds != layoutBounds ? 1 : (this.LogicalBounds != logicalBounds ? 1 : 0))) != 0;
      if (this._geometryInvalidated | flag)
      {
        this.LayoutBounds = layoutBounds;
        this.LogicalBounds = logicalBounds;
        bool force = flag | this.UpdateCachedGeometry(parameters1);
        flag = force | this.ApplyGeometryEffect((IGeometrySourceParameters) parameters1, force);
      }
    }
    this._geometryInvalidated = false;
    return flag;
  }

  public Geometry Geometry { get; private set; }

  public Rect LayoutBounds { get; private set; }

  public Rect LogicalBounds { get; private set; }

  private bool ApplyGeometryEffect(IGeometrySourceParameters parameters, bool force)
  {
    bool flag = false;
    Geometry objB = this.CachedGeometry;
    GeometryEffect geometryEffect = parameters.GetGeometryEffect();
    if (geometryEffect != null)
    {
      if (force)
      {
        flag = true;
        geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ParentInvalidated);
      }
      if (geometryEffect.ProcessGeometry(this.CachedGeometry))
      {
        flag = true;
        objB = geometryEffect.OutputGeometry;
      }
    }
    if (!object.Equals((object) this.Geometry, (object) objB))
    {
      flag = true;
      this.Geometry = objB;
    }
    return flag;
  }

  protected virtual Rect ComputeLogicalBounds(
    Rect layoutBounds,
    IGeometrySourceParameters parameters)
  {
    return GeometryHelper.Inflate(layoutBounds, -parameters.GetHalfStrokeThickness());
  }

  protected abstract bool UpdateCachedGeometry(TParameters parameters);
}
