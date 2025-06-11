// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.BorderImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class BorderImpl : CommonObject, IBorder, IParentApplication, IDisposable
{
  public const int DEF_MAXBADCOLOR = 8;
  public const int DEF_BADCOLOR_INCREMENT = 64 /*0x40*/;
  private ExcelBordersIndex m_border;
  private IInternalExtendedFormat m_format;

  public ExcelKnownColors Color
  {
    get => this.ColorObject.GetIndexed((IWorkbook) this.m_format.Workbook);
    set
    {
      value = BorderImpl.NormalizeColor(value);
      if (this.m_format is CellStyle)
        (this.m_format as CellStyle).AskAdjacent = false;
      this.m_format.BeginUpdate();
      this.ColorObject.SetIndexed(value);
      this.m_format.EndUpdate();
    }
  }

  public ColorObject ColorObject
  {
    get
    {
      switch (this.m_border)
      {
        case ExcelBordersIndex.DiagonalDown:
          return this.m_format.DiagonalBorderColor;
        case ExcelBordersIndex.DiagonalUp:
          return this.m_format.DiagonalBorderColor;
        case ExcelBordersIndex.EdgeLeft:
          return this.m_format.LeftBorderColor;
        case ExcelBordersIndex.EdgeTop:
          return this.m_format.TopBorderColor;
        case ExcelBordersIndex.EdgeBottom:
          return this.m_format.BottomBorderColor;
        case ExcelBordersIndex.EdgeRight:
          return this.m_format.RightBorderColor;
        default:
          throw new ArgumentOutOfRangeException("Border index");
      }
    }
  }

  public System.Drawing.Color ColorRGB
  {
    get => this.ColorObject.GetRGB((IWorkbook) this.Workbook);
    set
    {
      if (this.m_format is CellStyle)
        (this.m_format as CellStyle).AskAdjacent = false;
      this.m_format.BeginUpdate();
      this.ColorObject.SetRGB(value, (IWorkbook) this.Workbook);
      this.m_format.EndUpdate();
    }
  }

  public ExcelLineStyle LineStyle
  {
    get
    {
      switch (this.m_border)
      {
        case ExcelBordersIndex.DiagonalDown:
          return this.m_format.DiagonalDownBorderLineStyle;
        case ExcelBordersIndex.DiagonalUp:
          return this.m_format.DiagonalUpBorderLineStyle;
        case ExcelBordersIndex.EdgeLeft:
          return this.m_format.LeftBorderLineStyle;
        case ExcelBordersIndex.EdgeTop:
          return this.m_format.TopBorderLineStyle;
        case ExcelBordersIndex.EdgeBottom:
          return this.m_format.BottomBorderLineStyle;
        case ExcelBordersIndex.EdgeRight:
          return this.m_format.RightBorderLineStyle;
        default:
          throw new ArgumentOutOfRangeException("Border index");
      }
    }
    set
    {
      switch (this.m_border)
      {
        case ExcelBordersIndex.DiagonalDown:
          this.m_format.DiagonalDownBorderLineStyle = value;
          break;
        case ExcelBordersIndex.DiagonalUp:
          this.m_format.DiagonalUpBorderLineStyle = value;
          break;
        case ExcelBordersIndex.EdgeLeft:
          this.m_format.LeftBorderLineStyle = value;
          break;
        case ExcelBordersIndex.EdgeTop:
          this.m_format.TopBorderLineStyle = value;
          break;
        case ExcelBordersIndex.EdgeBottom:
          this.m_format.BottomBorderLineStyle = value;
          break;
        case ExcelBordersIndex.EdgeRight:
          this.m_format.RightBorderLineStyle = value;
          break;
        default:
          throw new ArgumentOutOfRangeException("Border index");
      }
    }
  }

  public bool ShowDiagonalLine
  {
    get
    {
      switch (this.m_border)
      {
        case ExcelBordersIndex.DiagonalDown:
          return this.m_format.DiagonalDownVisible;
        case ExcelBordersIndex.DiagonalUp:
          return this.m_format.DiagonalUpVisible;
        default:
          return false;
      }
    }
    set
    {
      switch (this.m_border)
      {
        case ExcelBordersIndex.DiagonalDown:
          this.m_format.DiagonalDownVisible = value;
          break;
        case ExcelBordersIndex.DiagonalUp:
          this.m_format.DiagonalUpVisible = value;
          break;
      }
    }
  }

  internal ExcelBordersIndex BorderIndex
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  private WorkbookImpl Workbook => this.m_format.Workbook;

  private BorderImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  private BorderImpl(IApplication application, object parent, ExcelBordersIndex borderIndex)
    : this(application, parent)
  {
    this.m_border = borderIndex;
  }

  public BorderImpl(
    IApplication application,
    object parent,
    IInternalExtendedFormat impl,
    ExcelBordersIndex borderIndex)
    : this(application, parent, borderIndex)
  {
    this.m_format = impl;
  }

  public override bool Equals(object obj)
  {
    return obj is BorderImpl borderImpl && borderImpl.m_border == this.m_border && borderImpl.ShowDiagonalLine == this.ShowDiagonalLine && borderImpl.LineStyle == this.LineStyle && borderImpl.ColorObject == this.ColorObject;
  }

  public override int GetHashCode()
  {
    return this.m_border.GetHashCode() ^ this.ShowDiagonalLine.GetHashCode() ^ this.LineStyle.GetHashCode() ^ this.ColorObject.GetHashCode();
  }

  public void CopyFrom(IBorder baseBorder)
  {
    this.ColorObject.CopyFrom(baseBorder.ColorObject, true);
    this.LineStyle = baseBorder.LineStyle;
  }

  private void NormalizeColor()
  {
    if (this.LineStyle == ExcelLineStyle.None || this.ColorObject.ColorType != ColorType.Indexed)
      return;
    ColorObject colorObject = this.ColorObject;
    ExcelKnownColors excelKnownColors = BorderImpl.NormalizeColor(colorObject.GetIndexed((IWorkbook) null));
    colorObject.SetIndexed(excelKnownColors);
  }

  public static ExcelKnownColors NormalizeColor(ExcelKnownColors color)
  {
    int num = (int) color;
    if (num == 0)
    {
      num += 64 /*0x40*/;
      color = (ExcelKnownColors) num;
    }
    return (ExcelKnownColors) num;
  }

  public BorderImpl Clone(StyleImpl newFormat)
  {
    BorderImpl borderImpl = this.MemberwiseClone() as BorderImpl;
    borderImpl.m_format = (IInternalExtendedFormat) newFormat;
    return borderImpl;
  }

  void IDisposable.Dispose() => GC.SuppressFinalize((object) this);

  internal void Clear()
  {
    this.m_format.TopBorderColor.Dispose();
    this.m_format.BottomBorderColor.Dispose();
    this.m_format.LeftBorderColor.Dispose();
    this.m_format.RightBorderColor.Dispose();
    this.m_format.DiagonalBorderColor.Dispose();
    this.m_format.DiagonalBorderColor.Dispose();
    this.Dispose();
  }
}
