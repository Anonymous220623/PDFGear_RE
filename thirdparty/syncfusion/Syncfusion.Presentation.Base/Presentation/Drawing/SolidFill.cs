// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.SolidFill
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.TableImplementation;
using System;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class SolidFill : ISolidFill
{
  private Fill _fillFormat;
  private ColorObject _internalColor;

  internal SolidFill(Fill fillFormat)
  {
    this._fillFormat = fillFormat;
    this._internalColor = new ColorObject(true);
  }

  public IColor Color
  {
    get
    {
      this._internalColor.UpdateColorObject((object) this._fillFormat.Presentation);
      return (IColor) this._internalColor;
    }
    set
    {
      this.SetColor(value);
      if (this._fillFormat.OwnerBorder == null || !this._fillFormat.OwnerBorder.IsConflictingBorder(this._fillFormat.OwnerBorder.BorderType))
        return;
      LineFormat adjacentCellBorder = (this._fillFormat.Parent as Table).GetAdjacentCellBorder(this._fillFormat.OwnerBorder.GetCurrentCellIndex(), this._fillFormat.OwnerBorder.BorderType);
      if (adjacentCellBorder == null)
        return;
      ((adjacentCellBorder.Fill as Fill).SolidFill as SolidFill).SetColor(value);
    }
  }

  public int Transparency
  {
    get
    {
      return 100 - this._internalColor.ColorTransFormCollection.GetColorModeValue(ColorMode.Alpha) / 1000;
    }
    set
    {
      this.SetTransparancy(value);
      if (this._fillFormat.OwnerBorder == null || !this._fillFormat.OwnerBorder.IsConflictingBorder(this._fillFormat.OwnerBorder.BorderType))
        return;
      LineFormat adjacentCellBorder = (this._fillFormat.Parent as Table).GetAdjacentCellBorder(this._fillFormat.OwnerBorder.GetCurrentCellIndex(), this._fillFormat.OwnerBorder.BorderType);
      if (adjacentCellBorder == null)
        return;
      ((adjacentCellBorder.Fill as Fill).SolidFill as SolidFill).SetTransparancy(value);
    }
  }

  internal void SetColor(IColor value)
  {
    if (value.A < byte.MaxValue)
      this._internalColor.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, (int) ((100.0 - (double) ((int) value.A * 100) / (double) byte.MaxValue) * 1000.0));
    if (this._fillFormat.Parent is Background)
    {
      Background parent = this._fillFormat.Parent as Background;
      if (parent.GetFillFormat().FillType == FillType.Automatic)
        parent.SetFill((IFill) this._fillFormat);
    }
    this._internalColor.SetColor(ColorType.RGB, ((ColorObject) value).ToArgb());
  }

  internal void SetTransparancy(int value)
  {
    if (value < 0 || value > 100)
      throw new ArgumentException("Invalid Transparency " + value.ToString());
    if (this._fillFormat.Parent is Background)
    {
      Background parent = this._fillFormat.Parent as Background;
      if (parent.GetFillFormat().FillType == FillType.Automatic)
        parent.SetFill((IFill) this._fillFormat);
    }
    this._internalColor.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, (100 - value) * 1000);
  }

  internal ColorObject GetColorObject() => this._internalColor;

  internal void SetColorObject(ColorObject colorObject) => this._internalColor = colorObject;

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._internalColor != null)
    {
      this._internalColor.Close();
      this._internalColor = (ColorObject) null;
    }
    this._fillFormat = (Fill) null;
  }

  public SolidFill Clone()
  {
    SolidFill solidFill = (SolidFill) this.MemberwiseClone();
    solidFill._internalColor = this._internalColor.CloneColorObject();
    return solidFill;
  }

  internal void SetParent(Fill fill) => this._fillFormat = fill;
}
