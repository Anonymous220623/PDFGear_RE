// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Background
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class Background : IBackground
{
  private BlackWhiteMode _blackWhiteMode;
  private BaseSlide _baseSlide;
  private ColorObject _colorObject;
  private Syncfusion.Presentation.Drawing.Fill _fillFormat;
  private bool _shadeToTitle;
  private int _styleIndex;
  private BackgroundType _type;
  private BackgroundStyle _backgroundStyle;
  private bool _isDefaultMap;

  internal Background(BaseSlide baseSlide)
  {
    this._baseSlide = baseSlide;
    this._colorObject = new ColorObject(true);
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this);
    this._isDefaultMap = true;
  }

  internal BlackWhiteMode BlackWhiteMode
  {
    get => this._blackWhiteMode;
    set => this._blackWhiteMode = value;
  }

  internal BaseSlide BaseSlide => this._baseSlide;

  internal bool ShadeToTitle
  {
    get => this._shadeToTitle;
    set => this._shadeToTitle = value;
  }

  public IFill Fill
  {
    get
    {
      if (this._fillFormat.FillType != FillType.Automatic)
        return (IFill) this._fillFormat;
      Syncfusion.Presentation.Drawing.Fill defaultFillFormat = this.GetDefaultFillFormat() as Syncfusion.Presentation.Drawing.Fill;
      return defaultFillFormat.FillType != FillType.Automatic ? this.CloneFill(defaultFillFormat) : (IFill) this._fillFormat;
    }
  }

  public int Index
  {
    get => this._styleIndex;
    set => this._styleIndex = value;
  }

  public BackgroundType Type
  {
    get => this._type;
    set => this._type = value;
  }

  internal IFill CloneFill(Syncfusion.Presentation.Drawing.Fill fill)
  {
    if (this.BaseSlide != fill.BaseSlide)
    {
      Syncfusion.Presentation.Drawing.Fill fill1 = this._fillFormat.Clone();
      fill1.FillType = fill.FillType;
      object fillOptions = fill1.GetFillOptions();
      switch (fill.FillType)
      {
        case FillType.Solid:
          fillOptions = (object) (fill.SolidFill as SolidFill).Clone();
          (fillOptions as SolidFill).SetParent(fill1);
          break;
        case FillType.Gradient:
          fillOptions = (object) (fill.GradientFill as GradientFill).Clone();
          (fillOptions as GradientFill).SetParent(fill1);
          break;
        case FillType.Picture:
        case FillType.Texture:
          fillOptions = (object) (fill.PictureFill as TextureFill).Clone();
          (fillOptions as TextureFill).SetParent(fill1);
          break;
        case FillType.Pattern:
          fillOptions = (object) (fill.PatternFill as PatternFill).Clone();
          (fillOptions as PatternFill).SetParent(fill1);
          break;
      }
      fill1.SetFillOptions(fillOptions);
      fill = fill1;
    }
    return (IFill) fill;
  }

  internal IFill GetDefaultFillFormat()
  {
    if (this._fillFormat.FillType != FillType.Automatic)
      return (IFill) this._fillFormat;
    if (this._baseSlide is Slide)
    {
      string layoutIndex = ((Slide) this._baseSlide).ObtainLayoutIndex();
      if (layoutIndex != null)
        return ((Background) this._baseSlide.Presentation.GetSlideLayout()[layoutIndex].Background).GetDefaultFillFormat();
    }
    else if (this._baseSlide is LayoutSlide)
      return ((Background) ((BaseSlide) ((LayoutSlide) this._baseSlide).MasterSlide).Background).GetDefaultFillFormat();
    return (IFill) this._fillFormat;
  }

  internal BackgroundStyle BackgroundStyle
  {
    get => this.GetBackGroundStyle();
    set
    {
      this._backgroundStyle = value;
      this.SetBackGroundStyle();
    }
  }

  internal BackgroundStyle GetBackGroundStyle()
  {
    switch (this._colorObject.GetColorString())
    {
      case "lt1":
        switch (this.Index)
        {
          case 1001:
            return BackgroundStyle.Style1;
          case 1002:
            return BackgroundStyle.Style5;
          case 1003:
            return BackgroundStyle.Style9;
        }
        break;
      case "lt2":
        switch (this.Index)
        {
          case 1001:
            return BackgroundStyle.Style2;
          case 1002:
            return BackgroundStyle.Style6;
          case 1003:
            return BackgroundStyle.Style10;
        }
        break;
      case "dk1":
        switch (this.Index)
        {
          case 1001:
            return BackgroundStyle.Style4;
          case 1002:
            return BackgroundStyle.Style8;
          case 1003:
            return BackgroundStyle.Style12;
        }
        break;
      case "dk2":
        switch (this.Index)
        {
          case 1001:
            return BackgroundStyle.Style3;
          case 1002:
            return BackgroundStyle.Style7;
          case 1003:
            return BackgroundStyle.Style11;
        }
        break;
    }
    return BackgroundStyle.Style1;
  }

  internal void SetBackGroundStyle()
  {
    switch (this._backgroundStyle)
    {
      case BackgroundStyle.Style1:
        this.SetBackgroundAttributes("lt1", 1001);
        break;
      case BackgroundStyle.Style2:
        this.SetBackgroundAttributes("lt2", 1001);
        break;
      case BackgroundStyle.Style3:
        this.SetBackgroundAttributes("dk2", 1001);
        break;
      case BackgroundStyle.Style4:
        this.SetBackgroundAttributes("dk1", 1001);
        break;
      case BackgroundStyle.Style5:
        this.SetBackgroundAttributes("lt1", 1002);
        break;
      case BackgroundStyle.Style6:
        this.SetBackgroundAttributes("lt2", 1002);
        break;
      case BackgroundStyle.Style7:
        this.SetBackgroundAttributes("dk2", 1002);
        break;
      case BackgroundStyle.Style8:
        this.SetBackgroundAttributes("dk1", 1002);
        break;
      case BackgroundStyle.Style9:
        this.SetBackgroundAttributes("lt1", 1003);
        break;
      case BackgroundStyle.Style10:
        this.SetBackgroundAttributes("lt2", 1003);
        break;
      case BackgroundStyle.Style11:
        this.SetBackgroundAttributes("dk2", 1003);
        break;
      case BackgroundStyle.Style12:
        this.SetBackgroundAttributes("dk1", 1003);
        break;
    }
  }

  private void SetBackgroundAttributes(string color, int index)
  {
    ColorObject colorObject = new ColorObject(true);
    colorObject.SetColor(ColorType.Theme, color);
    this.SetColorObject(colorObject);
    this.Index = index;
  }

  internal void SetColorObject(ColorObject colorObject) => this._colorObject = colorObject;

  internal ColorObject GetColorObject()
  {
    if (!this._isDefaultMap)
      this._baseSlide.IsColorMapChanged = true;
    return this._colorObject;
  }

  internal void SetBgStyle(BackgroundStyle style) => this._backgroundStyle = style;

  internal Syncfusion.Presentation.Drawing.Fill GetFillFormat() => this._fillFormat;

  internal bool GetDefaultMap() => this._isDefaultMap;

  internal void SetDefaultMap(bool value) => this._isDefaultMap = value;

  internal void SetFill(IFill fillFormat) => this._fillFormat = fillFormat as Syncfusion.Presentation.Drawing.Fill;

  internal void SetFillFormat(Syncfusion.Presentation.Drawing.Fill fillFormat)
  {
    this._fillFormat = fillFormat.Clone();
    switch (this._fillFormat.FillType)
    {
      case FillType.Solid:
        ((SolidFill) this._fillFormat.SolidFill).SetColorObject(this._colorObject);
        break;
      case FillType.Gradient:
        using (IEnumerator<IGradientStop> enumerator = ((GradientStops) this._fillFormat.GradientFill.GradientStops).GetEnumerator())
        {
          while (enumerator.MoveNext())
            ((GradientStop) enumerator.Current).GetColorObjectForImageConversion().SetColor(ColorType.Theme, this._colorObject.GetColorString());
          break;
        }
      case FillType.Picture:
        List<ColorObject> duoTone = ((TextureFill) this._fillFormat.PictureFill).DuoTone;
        if (duoTone == null || duoTone.Count == 0)
          break;
        List<ColorObject> forImageConversion = ((TextureFill) this._fillFormat.PictureFill).GetDuoToneForImageConversion();
        foreach (ColorObject colorObject in ((TextureFill) this._fillFormat.PictureFill).DuoTone)
          forImageConversion.Add(colorObject.CloneColorObject());
        forImageConversion[0].SetColor(ColorType.Theme, this._colorObject.GetColorString());
        forImageConversion[1].SetColor(ColorType.Theme, this._colorObject.GetColorString());
        break;
    }
  }

  internal void Close()
  {
    if (this._colorObject != null)
      this._colorObject.Close();
    if (this._fillFormat != null)
      this._fillFormat.Close();
    this._baseSlide = (BaseSlide) null;
  }

  public Background Clone()
  {
    Background background = (Background) this.MemberwiseClone();
    background._colorObject = this._colorObject.CloneColorObject();
    background._fillFormat = this._fillFormat.Clone();
    background._fillFormat.SetParent(background);
    return background;
  }

  internal void SetParent(BaseSlide newParent) => this._baseSlide = newParent;
}
