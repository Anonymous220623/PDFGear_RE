// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.PatternFill
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class PatternFill : IPatternFill
{
  private ColorObject _backcolor;
  private PatternFillType _patternFillType;
  private ColorObject _forecolor;
  private Fill _fillFormat;

  internal PatternFill(Fill fillFormat)
  {
    this._fillFormat = fillFormat;
    this._backcolor = new ColorObject(true);
    this._forecolor = new ColorObject(true);
  }

  internal Syncfusion.Presentation.Presentation Presentation => this._fillFormat.Presentation;

  public IColor BackColor
  {
    get
    {
      this._backcolor.UpdateColorObject((object) this._fillFormat.Presentation);
      return (IColor) this._backcolor;
    }
    set
    {
      if (value.A < byte.MaxValue)
        this._backcolor.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, (int) ((100.0 - (double) ((int) value.A * 100) / (double) byte.MaxValue) * 1000.0));
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      this._backcolor.SetColor(ColorType.RGB, value.ToArgb());
    }
  }

  public IColor ForeColor
  {
    get
    {
      this._forecolor.UpdateColorObject((object) this._fillFormat.Presentation);
      return (IColor) this._forecolor;
    }
    set
    {
      if (value.A < byte.MaxValue)
        this._forecolor.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, (int) ((100.0 - (double) ((int) value.A * 100) / (double) byte.MaxValue) * 1000.0));
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      this._forecolor.SetColor(ColorType.RGB, value.ToArgb());
    }
  }

  public PatternFillType Pattern
  {
    get => this._patternFillType;
    set
    {
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      this._patternFillType = value;
    }
  }

  internal Fill Fill => this._fillFormat;

  internal ColorObject GetBackColorObject() => this._backcolor;

  internal ColorObject GetForeColorObject() => this._forecolor;

  internal void SetBackColorObject(ColorObject colorObject) => this._backcolor = colorObject;

  internal void SetForeColorObject(ColorObject colorObject) => this._forecolor = colorObject;

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._forecolor != null)
    {
      this._forecolor.Close();
      this._forecolor = (ColorObject) null;
    }
    if (this._backcolor != null)
    {
      this._backcolor.Close();
      this._backcolor = (ColorObject) null;
    }
    this._fillFormat = (Fill) null;
  }

  public PatternFill Clone()
  {
    PatternFill patternFill = (PatternFill) this.MemberwiseClone();
    patternFill._backcolor = this._backcolor.CloneColorObject();
    patternFill._forecolor = this._forecolor.CloneColorObject();
    return patternFill;
  }

  internal void SetParent(Fill fill) => this._fillFormat = fill;
}
