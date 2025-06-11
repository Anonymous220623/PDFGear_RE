// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.GradientFill
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class GradientFill : IGradientFill
{
  private Fill _fillFormat;
  private Syncfusion.Presentation.Drawing.GradientStops _gradientStopCollection;
  private object _shapeProperties;
  private object _tileRect;
  private bool? _isRotWithShape;
  private GradientFillType _grapdientType;

  internal GradientFill(Fill fillFormat)
  {
    this._gradientStopCollection = new Syncfusion.Presentation.Drawing.GradientStops(this);
    this._fillFormat = fillFormat;
    this._grapdientType = GradientFillType.Linear;
  }

  internal GradientFillType Type
  {
    get => this._grapdientType;
    set => this._grapdientType = value;
  }

  public IGradientStops GradientStops => (IGradientStops) this._gradientStopCollection;

  internal bool? RotWithShape
  {
    get
    {
      return this._fillFormat.Background != null && this._fillFormat.Background.BaseSlide is Slide ? new bool?() : this._isRotWithShape;
    }
    set => this._isRotWithShape = value;
  }

  internal object ShadeProperties
  {
    get => this._shapeProperties;
    set => this._shapeProperties = value;
  }

  internal object TileRectangle
  {
    get => this._tileRect;
    set => this._tileRect = value;
  }

  internal Fill FillFormat => this._fillFormat;

  internal void SetGradientStops(IGradientStops gradientStops)
  {
    this._gradientStopCollection = gradientStops as Syncfusion.Presentation.Drawing.GradientStops;
  }

  internal LineShadeImpl GetLineShade()
  {
    return this._shapeProperties is LineShadeImpl shapeProperties ? shapeProperties : (LineShadeImpl) null;
  }

  internal PathShadeImpl GetPathShade() => this._shapeProperties as PathShadeImpl;

  internal PathShadeImpl ObtainTileRectangle() => this._tileRect as PathShadeImpl;

  internal bool IsLineShade() => this.GetLineShade() != null;

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._gradientStopCollection != null)
    {
      this._gradientStopCollection.Close();
      this._gradientStopCollection = (Syncfusion.Presentation.Drawing.GradientStops) null;
    }
    if (this._shapeProperties != null)
    {
      this.CloseObject(this._shapeProperties);
      this._shapeProperties = (object) null;
    }
    if (this._tileRect != null)
    {
      this.CloseObject(this._tileRect);
      this._tileRect = (object) null;
    }
    this._fillFormat = (Fill) null;
  }

  private void CloseObject(object value)
  {
    if (value is LineShadeImpl)
      ;
    if (!(value is PathShadeImpl))
      ;
  }

  public GradientFill Clone()
  {
    GradientFill gradientFill = (GradientFill) this.MemberwiseClone();
    gradientFill._gradientStopCollection = this._gradientStopCollection.Clone();
    gradientFill._gradientStopCollection.SetParent(gradientFill);
    if (this._shapeProperties != null)
    {
      if (this._shapeProperties is LineShadeImpl shapeProperties1)
        gradientFill._shapeProperties = (object) shapeProperties1.Clone();
      if (this._shapeProperties is PathShadeImpl shapeProperties2)
        gradientFill._shapeProperties = (object) shapeProperties2.Clone();
    }
    if (this._tileRect != null && this._tileRect is PathShadeImpl tileRect)
      gradientFill._tileRect = (object) tileRect.Clone();
    return gradientFill;
  }

  internal void SetParent(Fill fill) => this._fillFormat = fill;
}
