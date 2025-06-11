// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.LineFormat
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class LineFormat : ILineFormat
{
  private ArrowheadLength _beginArrowheadLength;
  private ArrowheadStyle _beginArrowheadStyle;
  private ArrowheadWidth _beginArrowheadWidth;
  private LineDashStyle _dashStyle;
  private ArrowheadLength _endArrowheadLength;
  private ArrowheadStyle _endArrowheadStyle;
  private ArrowheadWidth _endArrowheadWidth;
  private Syncfusion.Presentation.Drawing.Fill _fillFormat;
  private LineCapStyle _lineCap;
  private LineJoinType _lineJoinType;
  private LineStyle _lineStyle;
  private int _width;
  private Shape _shape;
  private string _refIndex;
  private bool _widthSet;
  private Syncfusion.Presentation.Presentation _presentation;
  private BaseSlide _baseSlide;
  private BorderType _borderType;
  private long _currentCellIndex;

  internal LineFormat(Shape shape)
  {
    this._shape = shape;
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this._shape);
    this._baseSlide = this._shape.BaseSlide;
    this._presentation = this._shape.BaseSlide.Presentation;
  }

  internal LineFormat(Shape shape, BorderType borderType, long cellIndex)
  {
    this._shape = shape;
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this._shape, this);
    this._baseSlide = this._shape.BaseSlide;
    this._presentation = this._shape.BaseSlide.Presentation;
    this._borderType = borderType;
    this._currentCellIndex = cellIndex;
  }

  public LineFormat(Syncfusion.Presentation.Presentation presentation)
  {
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(presentation);
    this._presentation = presentation;
  }

  internal BaseSlide BaseSlide => this._baseSlide;

  internal Syncfusion.Presentation.Presentation Presentation => this._presentation;

  internal BorderType BorderType => this._borderType;

  public ArrowheadLength BeginArrowheadLength
  {
    get => this._beginArrowheadLength;
    set
    {
      this._beginArrowheadLength = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetBeginArrowheadLength(value);
    }
  }

  public ArrowheadStyle BeginArrowheadStyle
  {
    get => this._beginArrowheadStyle;
    set
    {
      this._beginArrowheadStyle = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetBeginArrowheadStyle(value);
    }
  }

  public ArrowheadWidth BeginArrowheadWidth
  {
    get => this._beginArrowheadWidth;
    set
    {
      this._beginArrowheadWidth = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetBeginArrowheadWidth(value);
    }
  }

  public LineCapStyle CapStyle
  {
    get => this._lineCap;
    set
    {
      this._lineCap = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetCapStyle(value);
    }
  }

  internal LineCapStyle GetDefaultCapStyle()
  {
    return this._shape != null && this._lineCap == LineCapStyle.None ? this._shape.GetLineCapStyleFromStyle("lnRef") : this._lineCap;
  }

  internal string Index
  {
    get => this._refIndex;
    set => this._refIndex = value;
  }

  public LineDashStyle DashStyle
  {
    get => this._dashStyle;
    set
    {
      this._dashStyle = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetDashStyle(value);
    }
  }

  public ArrowheadLength EndArrowheadLength
  {
    get => this._endArrowheadLength;
    set
    {
      this._endArrowheadLength = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetEndArrowheadLength(value);
    }
  }

  public ArrowheadStyle EndArrowheadStyle
  {
    get => this._endArrowheadStyle;
    set
    {
      this._endArrowheadStyle = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetEndArrowheadStyle(value);
    }
  }

  public ArrowheadWidth EndArrowheadWidth
  {
    get => this._endArrowheadWidth;
    set
    {
      this._endArrowheadWidth = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetEndArrowheadWidth(value);
    }
  }

  public IFill Fill => (IFill) this._fillFormat;

  internal IFill GetDefaultFillFormat()
  {
    if (this._fillFormat.FillType == FillType.Automatic)
    {
      IFill defaultFillFormat = (IFill) new Syncfusion.Presentation.Drawing.Fill(this._shape);
      if (this._shape != null && (this._shape.ShapeType == ShapeType.Sp || this._shape.ShapeType == ShapeType.CxnSp))
      {
        if (this._shape.DrawingType == DrawingType.PlaceHolder)
        {
          FillType fillType = FillType.Automatic;
          if (this._shape.BaseSlide is Slide)
          {
            Slide baseSlide = (Slide) this._shape.BaseSlide;
            string layoutIndex = baseSlide.ObtainLayoutIndex();
            if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
            {
              LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
              foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
              {
                if (shape.PlaceholderFormat != null)
                {
                  if (this._shape.PlaceholderFormat != null)
                  {
                    if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._shape.PlaceholderFormat))
                    {
                      fillType = ((LineFormat) shape.LineFormat).GetDefaultFillFormat().FillType;
                      if (fillType != FillType.Automatic)
                      {
                        defaultFillFormat = ((LineFormat) shape.LineFormat).GetDefaultFillFormat();
                        break;
                      }
                      break;
                    }
                  }
                  else
                    break;
                }
              }
              if (fillType == FillType.Automatic)
              {
                foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
                {
                  if (shape.PlaceholderFormat != null)
                  {
                    if (this._shape.PlaceholderFormat != null)
                    {
                      if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._shape.PlaceholderFormat, true) || this._shape.CheckMasterWithLayoutShape(shape, (Shapes) layoutSlide.Shapes))
                      {
                        if (((LineFormat) shape.LineFormat).GetDefaultFillFormat().FillType != FillType.Automatic)
                        {
                          defaultFillFormat = ((LineFormat) shape.LineFormat).GetDefaultFillFormat();
                          break;
                        }
                        break;
                      }
                    }
                    else
                      break;
                  }
                }
              }
            }
          }
          else if (this._shape.BaseSlide is NotesSlide)
          {
            foreach (Shape shape in (IEnumerable<ISlideItem>) this._shape.BaseSlide.Presentation.NotesMaster.Shapes)
            {
              if (shape.PlaceholderFormat != null)
              {
                if (this._shape.PlaceholderFormat != null)
                {
                  if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._shape.PlaceholderFormat, true))
                  {
                    if (((LineFormat) shape.LineFormat).GetDefaultFillFormat().FillType != FillType.Automatic)
                    {
                      defaultFillFormat = ((LineFormat) shape.LineFormat).GetDefaultFillFormat();
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
            }
          }
          return defaultFillFormat;
        }
        if (this._fillFormat.FillType == FillType.Automatic && this._shape.DrawingType != DrawingType.PlaceHolder)
        {
          defaultFillFormat.FillType = FillType.Solid;
          if (this._shape.PreservedElements.Count == 0)
            return (IFill) this._fillFormat;
          IColor color = (IColor) null;
          if (this._shape.PreservedElements.ContainsKey("style"))
            color = this._shape.GetThemeColor("lnRef");
          if (color != null && color.ToArgb() != ColorObject.Transparent.ToArgb())
          {
            defaultFillFormat.SolidFill.Color = color;
          }
          else
          {
            if (color == null || color.ToArgb() != ColorObject.Transparent.ToArgb() || !(color as ColorObject).IsUpdatedColor)
              return (IFill) this._fillFormat;
            defaultFillFormat.SolidFill.Color = color;
          }
          return defaultFillFormat;
        }
      }
    }
    return (IFill) this._fillFormat;
  }

  public LineJoinType LineJoinType
  {
    get => this._lineJoinType;
    set
    {
      this._lineJoinType = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetLineJoinType(value);
    }
  }

  internal LineJoinType GetDefaultLineJoinType()
  {
    return this._shape != null && this._lineJoinType == LineJoinType.None ? this._shape.GetLineJoinTypeFromStyle("lnRef") : this._lineJoinType;
  }

  public LineStyle Style
  {
    get => this._lineStyle;
    set
    {
      this._lineStyle = value;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetStyle(value);
    }
  }

  public double Weight
  {
    get => Helper.EmuToPoint(this._width);
    set
    {
      this._width = value >= 0.0 && value <= 1584.0 ? Helper.PointToEmu(value) : throw new ArgumentException("Invalid Weight" + value.ToString());
      this._widthSet = true;
      if (!this.IsConflictingBorder(this.BorderType))
        return;
      (this._shape as Table).GetAdjacentCellBorder(this._currentCellIndex, this.BorderType)?.SetWeight(value);
    }
  }

  internal bool IsConflictingBorder(BorderType borderType)
  {
    return (borderType == BorderType.Top || borderType == BorderType.Left) && this._shape is Table;
  }

  internal long GetCurrentCellIndex() => this._currentCellIndex;

  internal double GetDefaultWidth()
  {
    if (this._widthSet || this._shape == null)
      return Helper.EmuToPoint(this._width);
    if (this._shape.BaseSlide is NotesSlide)
    {
      foreach (Shape shape in (IEnumerable<ISlideItem>) this._shape.BaseSlide.Presentation.NotesMaster.Shapes)
      {
        if (shape.PlaceholderFormat != null)
        {
          if (this._shape.PlaceholderFormat != null)
          {
            if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._shape.PlaceholderFormat, true))
              return ((LineFormat) shape.LineFormat).GetDefaultWidth();
          }
          else
            break;
        }
      }
    }
    return this._shape.GetLineWidthFromStyle("lnRef");
  }

  internal int GetWidth() => this._width;

  internal void SetWidth(int value)
  {
    this._width = value;
    this._widthSet = true;
  }

  internal Syncfusion.Presentation.Drawing.Fill GetFillFormat() => this._fillFormat;

  internal void SetWeight(double value)
  {
    this._width = Helper.PointToEmu(value);
    this._widthSet = true;
  }

  internal void SetBeginArrowheadLength(ArrowheadLength value)
  {
    this._beginArrowheadLength = value;
  }

  internal void SetEndArrowheadLength(ArrowheadLength value) => this._endArrowheadLength = value;

  internal void SetBeginArrowheadStyle(ArrowheadStyle value) => this._beginArrowheadStyle = value;

  internal void SetEndArrowheadStyle(ArrowheadStyle value) => this._endArrowheadStyle = value;

  internal void SetBeginArrowheadWidth(ArrowheadWidth value) => this._beginArrowheadWidth = value;

  internal void SetEndArrowheadWidth(ArrowheadWidth value) => this._endArrowheadWidth = value;

  internal void SetCapStyle(LineCapStyle value) => this._lineCap = value;

  internal void SetLineJoinType(LineJoinType value) => this._lineJoinType = value;

  internal void SetDashStyle(LineDashStyle value) => this._dashStyle = value;

  internal void SetStyle(LineStyle value) => this._lineStyle = value;

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._fillFormat != null)
    {
      this._fillFormat.Close();
      this._fillFormat = (Syncfusion.Presentation.Drawing.Fill) null;
    }
    this._shape = (Shape) null;
    this._presentation = (Syncfusion.Presentation.Presentation) null;
    this._baseSlide = (BaseSlide) null;
  }

  public LineFormat Clone()
  {
    LineFormat lineFormat = (LineFormat) this.MemberwiseClone();
    lineFormat._fillFormat = this._fillFormat.Clone();
    return lineFormat;
  }

  internal void SetParent(Shape shape)
  {
    this._shape = shape;
    if (this._fillFormat == null)
      return;
    this._fillFormat.SetParent((object) shape);
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
  }
}
