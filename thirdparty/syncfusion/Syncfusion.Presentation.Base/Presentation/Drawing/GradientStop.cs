// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.GradientStop
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class GradientStop : IGradientStop
{
  private const int MaxValue = 100000;
  private GradientStops _collection;
  private ColorObject _colorObject;
  private float _position;
  private ColorObject _colorObjectForImgConversion;

  internal GradientStop(GradientStops collection)
  {
    this._collection = collection;
    this._colorObject = new ColorObject(true);
  }

  internal GradientStops GradientStops => this._collection;

  public IColor Color
  {
    get
    {
      MasterSlide master = this.GetMaster(this._collection);
      if (master != null)
        this._colorObject.UpdateColorObject((object) master);
      else
        this._colorObject.UpdateColorObject((object) this._collection.Presentation);
      return (IColor) this._colorObject;
    }
    set
    {
      if (this._collection.GetGradientFill().FillFormat.Parent is Background)
      {
        Background parent = this._collection.GetGradientFill().FillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._collection.GetGradientFill().FillFormat);
      }
      this._colorObject.SetColor(ColorType.RGB, ((ColorObject) value).ToArgb());
    }
  }

  public float Position
  {
    get => this._position / 1000f;
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Invalid Position " + value.ToString());
      if (this._collection.GetGradientFill().FillFormat.Parent is Background)
      {
        Background parent = this._collection.GetGradientFill().FillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._collection.GetGradientFill().FillFormat);
      }
      this._position = (float) (int) ((double) value * 1000.0 + 0.5);
    }
  }

  public float Brightness => this._colorObject.GetBrightness();

  public int Transparency
  {
    get
    {
      return 100 - this._colorObject.ColorTransFormCollection.GetColorModeValue(ColorMode.Alpha) / 1000;
    }
    set
    {
      if (value < 0 || value > 100)
        throw new ArgumentException("Invalid Transparency " + value.ToString());
      if (this._collection.GetGradientFill().FillFormat.Parent is Background)
      {
        Background parent = this._collection.GetGradientFill().FillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._collection.GetGradientFill().FillFormat);
      }
      this._colorObject.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, (100 - value) * 1000);
    }
  }

  internal ColorObject GetColorObject() => this._colorObject;

  internal IColor GetDefaultColor()
  {
    if (this._colorObject.GetColorString() == "phClr")
    {
      if (this._colorObject.IsGradient)
      {
        this.GetColorObjectForImageConversion();
        this._colorObjectForImgConversion = this._colorObject.CloneColorObject();
        this._colorObjectForImgConversion.ThemeColorValue = this._colorObject.ReplaceColor;
        this._colorObjectForImgConversion.UpdateColorObject((object) this._collection.Presentation);
        return (IColor) this._colorObjectForImgConversion;
      }
      this._colorObjectForImgConversion.UpdateColorObject((object) this._collection.Presentation);
      return (IColor) this._colorObjectForImgConversion;
    }
    MasterSlide master = this.GetMaster(this._collection);
    if (master != null)
      this._colorObject.UpdateColorObject((object) master);
    else
      this._colorObject.UpdateColorObject((object) this._collection.Presentation);
    return (IColor) this._colorObject;
  }

  internal MasterSlide GetMaster(GradientStops collection)
  {
    BaseSlide baseSlide = collection.BaseSlide;
    switch (baseSlide)
    {
      case IMasterSlide _:
        return baseSlide as MasterSlide;
      case LayoutSlide _:
        return (MasterSlide) ((LayoutSlide) baseSlide).MasterSlide;
      case Slide _:
        return (MasterSlide) ((Slide) baseSlide).LayoutSlide.MasterSlide;
      default:
        return (MasterSlide) null;
    }
  }

  internal ColorObject GetColorObjectForImageConversion()
  {
    return this._colorObjectForImgConversion ?? (this._colorObjectForImgConversion = new ColorObject(true));
  }

  internal int GetMaxValue() => 100000;

  internal void SetColorObject(ColorObject colorObject) => this._colorObject = colorObject;

  internal void Close() => this.CloseAll();

  private void CloseAll()
  {
    if (this._colorObject != null)
    {
      this._colorObject.Close();
      this._colorObject = (ColorObject) null;
    }
    this._collection = (GradientStops) null;
  }

  public GradientStop Clone()
  {
    GradientStop gradientStop = (GradientStop) this.MemberwiseClone();
    gradientStop._colorObject = this._colorObject.CloneColorObject();
    return gradientStop;
  }

  internal void SetParent(GradientStops newParent) => this._collection = newParent;
}
