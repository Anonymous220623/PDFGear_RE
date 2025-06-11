// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.ShapeFrame
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class ShapeFrame
{
  private BlackWhiteMode _blackWhiteMode;
  private bool? _flipH;
  private bool? _flipV;
  private bool _isFrameChanged;
  private long _offsetCx;
  private long _offsetCy;
  private long _offsetX;
  private long _offsetY;
  private int _rotation;
  private Shape _baseShape;
  private long _chOffsetCx;
  private long _chOffsetCy;
  private long _chOffsetY;
  private long _chOffsetX;
  private bool _isChOffset;
  private bool _isRotationSet;

  internal ShapeFrame(Shape shape) => this._baseShape = shape;

  internal float CenterX => (float) this._offsetX + (float) this._offsetCx / 2f;

  internal double ChildWidth
  {
    get => Helper.EmuToPoint(this._chOffsetCx);
    set
    {
      this._isFrameChanged = true;
      this._isChOffset = true;
      this._chOffsetCx = (long) Helper.PointToEmu(value);
    }
  }

  internal double ChildHeight
  {
    get => Helper.EmuToPoint(this._chOffsetCy);
    set
    {
      this._isFrameChanged = true;
      this._isChOffset = true;
      this._chOffsetCy = (long) Helper.PointToEmu(value);
    }
  }

  internal double ChildTop
  {
    get => Helper.EmuToPoint(this._chOffsetY);
    set
    {
      this._isFrameChanged = true;
      this._isChOffset = true;
      this._chOffsetY = (long) Helper.PointToEmu(value);
    }
  }

  internal double ChildLeft
  {
    get => Helper.EmuToPoint(this._chOffsetX);
    set
    {
      this._isFrameChanged = true;
      this._isChOffset = true;
      this._chOffsetX = (long) Helper.PointToEmu(value);
    }
  }

  internal bool IsChildOffsetSet
  {
    get => this._isChOffset;
    set => this._isChOffset = value;
  }

  internal float CenterY => (float) this._offsetY + (float) this._offsetCy / 2f;

  internal bool FlipHorizontal
  {
    get => this._flipH.HasValue && this._flipH.Value;
    set => this._flipH = new bool?(value);
  }

  internal bool FlipVertical
  {
    get => this._flipV.HasValue && this._flipV.Value;
    set => this._flipV = new bool?(value);
  }

  internal double Height
  {
    get => Helper.EmuToPoint(this._offsetCy);
    set
    {
      if (value > 169056.0 || value < 0.0)
        throw new ArgumentException("Invalid Height " + value.ToString());
      this._isFrameChanged = true;
      this._offsetCy = (long) Helper.PointToEmu(value);
      this.SetPositionForChart();
    }
  }

  internal ShapeFrame GetShapeFrame(IShapes shapes, bool isMasterSlide)
  {
    foreach (Shape shape in (IEnumerable<ISlideItem>) shapes)
    {
      if (shape.PlaceholderFormat != null)
      {
        if (this._baseShape.PlaceholderFormat != null)
        {
          if (isMasterSlide ? Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true) : Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
            return shape.ShapeFrame;
        }
        else
          break;
      }
    }
    return (ShapeFrame) null;
  }

  internal double GetDefaultHeight()
  {
    if (this._isFrameChanged || this._baseShape.DrawingType != DrawingType.PlaceHolder)
      return Helper.EmuToPoint(this._offsetCy);
    long emu = 0;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        ShapeFrame shapeFrame1 = this.GetShapeFrame(layoutSlide.Shapes, false);
        emu = shapeFrame1 != null ? shapeFrame1._offsetCy : 0L;
        if (emu == 0L)
        {
          ShapeFrame shapeFrame2 = this.GetShapeFrame(((BaseSlide) layoutSlide.MasterSlide).Shapes, true);
          emu = shapeFrame2 != null ? shapeFrame2._offsetCy : 0L;
        }
      }
    }
    else if (this._baseShape.BaseSlide is NotesSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame(this._baseShape.BaseSlide.Presentation.NotesMaster.Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetCy : 0L;
    }
    else if (this._baseShape.BaseSlide is LayoutSlide)
    {
      LayoutSlide baseSlide = this._baseShape.BaseSlide as LayoutSlide;
      ShapeFrame shapeFrame3 = this.GetShapeFrame(baseSlide.Shapes, false);
      emu = shapeFrame3 != null ? shapeFrame3._offsetCy : 0L;
      if (emu == 0L)
      {
        ShapeFrame shapeFrame4 = this.GetShapeFrame(((BaseSlide) baseSlide.MasterSlide).Shapes, true);
        emu = shapeFrame4 != null ? shapeFrame4._offsetCy : 0L;
      }
    }
    else if (this._baseShape.BaseSlide is MasterSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame((this._baseShape.BaseSlide as MasterSlide).Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetCy : 0L;
    }
    return Helper.EmuToPoint(emu);
  }

  internal double Left
  {
    get => Helper.EmuToPoint(this._offsetX);
    set
    {
      if (value > 169056.0 || value < -169056.0)
        throw new ArgumentException("Invalid Left " + value.ToString());
      this._isFrameChanged = true;
      this._offsetX = (long) Helper.PointToEmu(value);
      this.SetPositionForChart();
    }
  }

  internal double GetDefaultLeft()
  {
    if (this._isFrameChanged || this._baseShape.DrawingType != DrawingType.PlaceHolder)
      return Helper.EmuToPoint(this._offsetX);
    long emu = 0;
    bool flag = false;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        ShapeFrame shapeFrame1 = this.GetShapeFrame(layoutSlide.Shapes, false);
        emu = shapeFrame1 != null ? shapeFrame1._offsetX : 0L;
        if (shapeFrame1 != null)
          flag = shapeFrame1._isFrameChanged;
        if (emu == 0L && !flag)
        {
          ShapeFrame shapeFrame2 = this.GetShapeFrame(((BaseSlide) layoutSlide.MasterSlide).Shapes, true);
          emu = shapeFrame2 != null ? shapeFrame2._offsetX : 0L;
        }
      }
    }
    else if (this._baseShape.BaseSlide is NotesSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame(this._baseShape.BaseSlide.Presentation.NotesMaster.Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetX : 0L;
    }
    else if (this._baseShape.BaseSlide is LayoutSlide)
    {
      LayoutSlide baseSlide = this._baseShape.BaseSlide as LayoutSlide;
      ShapeFrame shapeFrame3 = this.GetShapeFrame(baseSlide.Shapes, false);
      emu = shapeFrame3 != null ? shapeFrame3._offsetX : 0L;
      if (shapeFrame3 != null)
        flag = shapeFrame3._isFrameChanged;
      if (emu == 0L && !flag)
      {
        ShapeFrame shapeFrame4 = this.GetShapeFrame(((BaseSlide) baseSlide.MasterSlide).Shapes, true);
        emu = shapeFrame4 != null ? shapeFrame4._offsetX : 0L;
      }
    }
    else if (this._baseShape.BaseSlide is MasterSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame((this._baseShape.BaseSlide as MasterSlide).Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetX : 0L;
    }
    return Helper.EmuToPoint(emu);
  }

  internal int Rotation
  {
    get => this._rotation;
    set => this._rotation = value;
  }

  internal int GetDefaultRotation()
  {
    if (this._isRotationSet || this._isFrameChanged || this._baseShape.DrawingType != DrawingType.PlaceHolder)
      return this._rotation;
    int defaultRotation = 0;
    bool flag = false;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                defaultRotation = shape.ShapeFrame._rotation;
                flag = shape.ShapeFrame._isFrameChanged;
                break;
              }
            }
            else
              break;
          }
        }
        if (defaultRotation == 0 && !flag)
        {
          foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  defaultRotation = shape.ShapeFrame._rotation;
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
    return defaultRotation;
  }

  internal double Top
  {
    get => Helper.EmuToPoint(this._offsetY);
    set
    {
      if (value > 169056.0 || value < -169056.0)
        throw new ArgumentException("Invalid Top " + value.ToString());
      this._isFrameChanged = true;
      this._offsetY = (long) Helper.PointToEmu(value);
      this.SetPositionForChart();
    }
  }

  internal double GetDefaultTop()
  {
    if (this._isFrameChanged || this._baseShape.DrawingType != DrawingType.PlaceHolder)
      return Helper.EmuToPoint(this._offsetY);
    long emu = 0;
    bool flag = false;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        ShapeFrame shapeFrame1 = this.GetShapeFrame(layoutSlide.Shapes, false);
        emu = shapeFrame1 != null ? shapeFrame1._offsetY : 0L;
        if (shapeFrame1 != null)
          flag = shapeFrame1._isFrameChanged;
        if (emu == 0L && !flag)
        {
          ShapeFrame shapeFrame2 = this.GetShapeFrame(((BaseSlide) layoutSlide.MasterSlide).Shapes, true);
          emu = shapeFrame2 != null ? shapeFrame2._offsetY : 0L;
        }
      }
    }
    else if (this._baseShape.BaseSlide is NotesSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame(this._baseShape.BaseSlide.Presentation.NotesMaster.Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetY : 0L;
    }
    else if (this._baseShape.BaseSlide is LayoutSlide)
    {
      LayoutSlide baseSlide = this._baseShape.BaseSlide as LayoutSlide;
      ShapeFrame shapeFrame3 = this.GetShapeFrame(baseSlide.Shapes, false);
      emu = shapeFrame3 != null ? shapeFrame3._offsetY : 0L;
      if (shapeFrame3 != null)
        flag = shapeFrame3._isFrameChanged;
      if (emu == 0L && !flag)
      {
        ShapeFrame shapeFrame4 = this.GetShapeFrame(((BaseSlide) baseSlide.MasterSlide).Shapes, true);
        emu = shapeFrame4 != null ? shapeFrame4._offsetY : 0L;
      }
    }
    else if (this._baseShape.BaseSlide is MasterSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame((this._baseShape.BaseSlide as MasterSlide).Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetY : 0L;
    }
    return Helper.EmuToPoint(emu);
  }

  internal double Width
  {
    get => Helper.EmuToPoint(this._offsetCx);
    set
    {
      if (value > 169056.0 || value < 0.0)
        throw new ArgumentException("Invalid Width " + value.ToString());
      this._isFrameChanged = true;
      this._offsetCx = (long) Helper.PointToEmu(value);
      this.SetPositionForChart();
    }
  }

  internal double GetDefaultWidth()
  {
    if (this._isFrameChanged || this._baseShape.DrawingType != DrawingType.PlaceHolder)
      return Helper.EmuToPoint(this._offsetCx);
    long emu = 0;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        ShapeFrame shapeFrame1 = this.GetShapeFrame(layoutSlide.Shapes, false);
        emu = shapeFrame1 != null ? shapeFrame1._offsetCx : 0L;
        if (emu == 0L)
        {
          ShapeFrame shapeFrame2 = this.GetShapeFrame(((BaseSlide) layoutSlide.MasterSlide).Shapes, true);
          emu = shapeFrame2 != null ? shapeFrame2._offsetCx : 0L;
        }
      }
    }
    else if (this._baseShape.BaseSlide is NotesSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame(this._baseShape.BaseSlide.Presentation.NotesMaster.Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetCx : 0L;
    }
    else if (this._baseShape.BaseSlide is LayoutSlide)
    {
      LayoutSlide baseSlide = this._baseShape.BaseSlide as LayoutSlide;
      ShapeFrame shapeFrame3 = this.GetShapeFrame(baseSlide.Shapes, false);
      emu = shapeFrame3 != null ? shapeFrame3._offsetCx : 0L;
      if (emu == 0L)
      {
        ShapeFrame shapeFrame4 = this.GetShapeFrame(((BaseSlide) baseSlide.MasterSlide).Shapes, true);
        emu = shapeFrame4 != null ? shapeFrame4._offsetCx : 0L;
      }
    }
    else if (this._baseShape.BaseSlide is MasterSlide)
    {
      ShapeFrame shapeFrame = this.GetShapeFrame((this._baseShape.BaseSlide as MasterSlide).Shapes, true);
      emu = shapeFrame != null ? shapeFrame._offsetCx : 0L;
    }
    return Helper.EmuToPoint(emu);
  }

  internal BlackWhiteMode BlackWhiteMode
  {
    get => this._blackWhiteMode;
    set => this._blackWhiteMode = value;
  }

  internal long OffsetCX => this._offsetCx;

  internal long OffsetCY => this._offsetCy;

  internal long OffsetX => this._offsetX;

  internal long OffsetY => this._offsetY;

  internal long ChOffsetCX => this._chOffsetCx;

  internal long ChOffsetCY => this._chOffsetCy;

  internal long ChOffsetX => this._chOffsetX;

  internal long ChOffsetY => this._chOffsetY;

  internal bool GetIsFrameChanged() => this._isFrameChanged;

  internal bool? GetFlipH() => this._flipH;

  internal bool? GetFlipV() => this._flipV;

  internal void SetAnchor(
    bool? flipV,
    bool? flipH,
    int rotation,
    long offsetX,
    long offsetY,
    long offsetCx,
    long offsetCy)
  {
    this._isFrameChanged = true;
    this._flipV = flipV;
    this._flipH = flipH;
    this._rotation = rotation;
    this._isRotationSet = true;
    this._offsetX = offsetX;
    this._offsetY = offsetY;
    this._offsetCx = offsetCx;
    this._offsetCy = offsetCy;
    this.SetPositionForChart();
  }

  private void SetPositionForChart()
  {
    if (!(this._baseShape is PresentationChart))
      return;
    ((PresentationChart) this._baseShape).GetChartImpl().Height = this.Height;
    ((PresentationChart) this._baseShape).GetChartImpl().Width = this.Width;
    ((PresentationChart) this._baseShape).GetChartImpl().XPos = this.Left;
    ((PresentationChart) this._baseShape).GetChartImpl().YPos = this.Top;
  }

  internal void SetChildAnchor(
    long childOffsetX,
    long childOffsetY,
    long childOffsetCx,
    long childOffsetCy)
  {
    this._chOffsetCx = childOffsetCx;
    this._chOffsetCy = childOffsetCy;
    this._chOffsetX = childOffsetX;
    this._chOffsetY = childOffsetY;
  }

  internal void SetRotationValue(int rotation)
  {
    this._rotation = rotation;
    this._isRotationSet = true;
  }

  internal Shape GetBaseShape() => this._baseShape;

  public ShapeFrame Clone() => (ShapeFrame) this.MemberwiseClone();

  internal void SetParent(Shape shape) => this._baseShape = shape;

  internal void Close() => this._baseShape = (Shape) null;
}
