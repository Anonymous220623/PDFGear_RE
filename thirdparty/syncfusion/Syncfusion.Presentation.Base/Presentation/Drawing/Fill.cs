// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Fill
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Fill : IFill
{
  private object _fillOptions;
  private FillType _fillType;
  private object _parent;
  private LineFormat _ownerBorder;

  internal Fill(Font font) => this._parent = (object) font;

  internal Fill(Background background) => this._parent = (object) background;

  internal Fill(Shape baseshape) => this._parent = (object) baseshape;

  internal Fill(Shape baseshape, LineFormat border)
  {
    this._parent = (object) baseshape;
    this._ownerBorder = border;
  }

  public Fill(Syncfusion.Presentation.Presentation presentation)
  {
    this._parent = (object) presentation;
  }

  public Fill(Cell cell) => this._parent = (object) cell;

  internal Fill(Theme collection) => this._parent = (object) collection;

  public FillType FillType
  {
    get => this._fillType;
    set
    {
      this.SetFillType(value);
      if (this.OwnerBorder == null || value != FillType.None && value != FillType.Solid || !this.OwnerBorder.IsConflictingBorder(this.OwnerBorder.BorderType))
        return;
      LineFormat adjacentCellBorder = (this.Parent as Table).GetAdjacentCellBorder(this.OwnerBorder.GetCurrentCellIndex(), this.OwnerBorder.BorderType);
      if (adjacentCellBorder == null)
        return;
      (adjacentCellBorder.Fill as Fill).SetFillType(value);
    }
  }

  public IGradientFill GradientFill
  {
    get
    {
      if (this._fillType == FillType.Automatic)
        this.FillType = FillType.Gradient;
      if (this.FillType != FillType.Gradient)
        throw new Exception("FillType should be GradientFill");
      if (this._fillOptions == null || this.FillType == FillType.Gradient && !(this._fillOptions is Syncfusion.Presentation.Drawing.GradientFill))
        this._fillOptions = (object) new Syncfusion.Presentation.Drawing.GradientFill(this);
      return (IGradientFill) this._fillOptions;
    }
  }

  public IPatternFill PatternFill
  {
    get
    {
      if (this._fillType == FillType.Automatic)
        this.FillType = FillType.Pattern;
      if (this.FillType != FillType.Pattern)
        throw new Exception("FillType should be PatternFill");
      if (this._fillOptions == null || this.FillType == FillType.Pattern && !(this._fillOptions is Syncfusion.Presentation.Drawing.PatternFill))
        this._fillOptions = (object) new Syncfusion.Presentation.Drawing.PatternFill(this);
      return (IPatternFill) this._fillOptions;
    }
  }

  public ISolidFill SolidFill
  {
    get
    {
      if (this._fillType == FillType.Automatic)
        this.SetFillType(FillType.Solid);
      if (this.FillType != FillType.Solid)
        throw new Exception("FillType should be SolidFill");
      if (this._fillOptions == null || this.FillType == FillType.Solid && !(this._fillOptions is Syncfusion.Presentation.Drawing.SolidFill))
        this._fillOptions = (object) new Syncfusion.Presentation.Drawing.SolidFill(this);
      return (ISolidFill) this._fillOptions;
    }
  }

  public IPictureFill PictureFill
  {
    get
    {
      if (this._fillType == FillType.Automatic)
        this.FillType = FillType.Picture;
      if (this.FillType != FillType.Picture && this.FillType != FillType.Texture)
        throw new Exception("FillType should be PictureFill or TextureFill");
      if (this._parent is Shape)
      {
        if (this._fillOptions == null)
          this._fillOptions = (object) new TextureFill(this, this._parent as Shape);
      }
      else if (this._fillOptions == null || (this.FillType == FillType.Picture || this.FillType == FillType.Texture) && !(this._fillOptions is TextureFill))
        this._fillOptions = (object) new TextureFill(this);
      return (IPictureFill) this._fillOptions;
    }
  }

  internal Background Background => this._parent as Background;

  internal Syncfusion.Presentation.Presentation Presentation
  {
    get
    {
      if (this._parent is Cell parent1)
        return parent1.Table.BaseSlide.Presentation;
      if (this._parent is Shape parent2)
        return parent2.BaseSlide.Presentation;
      if (this.Parent is Font parent3)
        return parent3.Paragraph.Presentation;
      if (this._parent is Syncfusion.Presentation.Presentation parent4)
        return parent4;
      if (this._parent is Theme)
        return ((Theme) this._parent).BaseSlide.Presentation;
      return !(this._parent is Background parent5) ? (Syncfusion.Presentation.Presentation) null : parent5.BaseSlide.Presentation;
    }
  }

  internal LineFormat OwnerBorder => this._ownerBorder;

  internal object Parent => this._parent;

  internal BaseSlide BaseSlide
  {
    get
    {
      if (this._parent is Syncfusion.Presentation.Presentation)
        return (BaseSlide) null;
      if (this._parent is Cell parent1)
        return parent1.Table.BaseSlide;
      if (this._parent is Font parent2)
        return parent2.Paragraph.BaseSlide;
      if (this._parent is Shape parent3)
        return parent3.BaseSlide;
      return this._parent is Theme ? ((Theme) this._parent).BaseSlide : this.Background.BaseSlide;
    }
  }

  internal void SetFillType(FillType value)
  {
    if (this.Parent is Background)
    {
      Background parent = this.Parent as Background;
      if (parent.GetFillFormat().FillType == FillType.Automatic)
        parent.SetFill((IFill) this);
    }
    this._fillType = value;
    if (this._parent is ICell)
      ((Cell) this._parent).IsFillSet = true;
    if (this._parent is Background)
    {
      Background parent = (Background) this._parent;
      if (parent.BaseSlide.Presentation.Created || value != FillType.Automatic || value != FillType.None)
        parent.Type = BackgroundType.OwnBackground;
    }
    if (this._parent is GroupShape && value != FillType.Automatic)
    {
      foreach (Shape shape in (IEnumerable<ISlideItem>) ((GroupShape) this._parent).Shapes)
        shape.IsGroupFill = true;
    }
    if (value != FillType.Picture)
      return;
    (this.PictureFill as TextureFill).TileMode = TileMode.Stretch;
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._fillOptions != null)
    {
      if (this._fillOptions is Syncfusion.Presentation.Drawing.GradientFill)
        ((Syncfusion.Presentation.Drawing.GradientFill) this._fillOptions).Close();
      if (this._fillOptions is Syncfusion.Presentation.Drawing.PatternFill)
        ((Syncfusion.Presentation.Drawing.PatternFill) this._fillOptions).Close();
      if (this._fillOptions is TextureFill)
        ((TextureFill) this._fillOptions).Close();
      if (this._fillOptions is Syncfusion.Presentation.Drawing.SolidFill)
        ((Syncfusion.Presentation.Drawing.SolidFill) this._fillOptions).Close();
      this._fillOptions = (object) null;
    }
    this._parent = (object) null;
  }

  public Fill Clone()
  {
    Fill fill = (Fill) this.MemberwiseClone();
    this.CloneFillOptions(fill);
    return fill;
  }

  private void CloneFillOptions(Fill fill)
  {
    if (this._fillOptions == null)
      return;
    if (this._fillOptions is Syncfusion.Presentation.Drawing.SolidFill fillOptions1)
    {
      fill._fillOptions = (object) fillOptions1.Clone();
      ((Syncfusion.Presentation.Drawing.SolidFill) fill._fillOptions).SetParent(fill);
    }
    if (this._fillOptions is Syncfusion.Presentation.Drawing.GradientFill fillOptions2)
    {
      fill._fillOptions = (object) fillOptions2.Clone();
      ((Syncfusion.Presentation.Drawing.GradientFill) fill._fillOptions).SetParent(fill);
    }
    if (this._fillOptions is Syncfusion.Presentation.Drawing.PatternFill fillOptions3)
    {
      fill._fillOptions = (object) fillOptions3.Clone();
      ((Syncfusion.Presentation.Drawing.PatternFill) fill._fillOptions).SetParent(fill);
    }
    if (!(this._fillOptions is TextureFill fillOptions4))
      return;
    fill._fillOptions = (object) fillOptions4.Clone();
    ((TextureFill) fill._fillOptions).SetParent(fill);
  }

  internal object GetFillOptions() => this._fillOptions;

  internal void SetFillOptions(object fillOptions) => this._fillOptions = fillOptions;

  internal void SetParent(Background background) => this._parent = (object) background;

  internal void SetParent(object shape)
  {
    this._parent = shape;
    if (this._fillOptions == null || !(this._fillOptions is TextureFill fillOptions))
      return;
    this._fillOptions = (object) fillOptions.Clone();
    ((TextureFill) this._fillOptions).SetParent(shape as Shape);
  }
}
