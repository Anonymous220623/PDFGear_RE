// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.BorderImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class BorderImpl : CommonObject, IBorder, IParentApplication, IDisposable
{
  public const int DEF_MAXBADCOLOR = 8;
  public const int DEF_BADCOLOR_INCREMENT = 64 /*0x40*/;
  private OfficeBordersIndex m_border;
  private IInternalExtendedFormat m_format;

  public OfficeKnownColors Color
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

  public ChartColor ColorObject
  {
    get
    {
      switch (this.m_border)
      {
        case OfficeBordersIndex.DiagonalDown:
          return this.m_format.DiagonalBorderColor;
        case OfficeBordersIndex.DiagonalUp:
          return this.m_format.DiagonalBorderColor;
        case OfficeBordersIndex.EdgeLeft:
          return this.m_format.LeftBorderColor;
        case OfficeBordersIndex.EdgeTop:
          return this.m_format.TopBorderColor;
        case OfficeBordersIndex.EdgeBottom:
          return this.m_format.BottomBorderColor;
        case OfficeBordersIndex.EdgeRight:
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

  public OfficeLineStyle LineStyle
  {
    get
    {
      switch (this.m_border)
      {
        case OfficeBordersIndex.DiagonalDown:
          return this.m_format.DiagonalDownBorderLineStyle;
        case OfficeBordersIndex.DiagonalUp:
          return this.m_format.DiagonalUpBorderLineStyle;
        case OfficeBordersIndex.EdgeLeft:
          return this.m_format.LeftBorderLineStyle;
        case OfficeBordersIndex.EdgeTop:
          return this.m_format.TopBorderLineStyle;
        case OfficeBordersIndex.EdgeBottom:
          return this.m_format.BottomBorderLineStyle;
        case OfficeBordersIndex.EdgeRight:
          return this.m_format.RightBorderLineStyle;
        default:
          throw new ArgumentOutOfRangeException("Border index");
      }
    }
    set
    {
      switch (this.m_border)
      {
        case OfficeBordersIndex.DiagonalDown:
          this.m_format.DiagonalDownBorderLineStyle = value;
          break;
        case OfficeBordersIndex.DiagonalUp:
          this.m_format.DiagonalUpBorderLineStyle = value;
          break;
        case OfficeBordersIndex.EdgeLeft:
          this.m_format.LeftBorderLineStyle = value;
          break;
        case OfficeBordersIndex.EdgeTop:
          this.m_format.TopBorderLineStyle = value;
          break;
        case OfficeBordersIndex.EdgeBottom:
          this.m_format.BottomBorderLineStyle = value;
          break;
        case OfficeBordersIndex.EdgeRight:
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
        case OfficeBordersIndex.DiagonalDown:
          return this.m_format.DiagonalDownVisible;
        case OfficeBordersIndex.DiagonalUp:
          return this.m_format.DiagonalUpVisible;
        default:
          return false;
      }
    }
    set
    {
      switch (this.m_border)
      {
        case OfficeBordersIndex.DiagonalDown:
          this.m_format.DiagonalDownVisible = value;
          break;
        case OfficeBordersIndex.DiagonalUp:
          this.m_format.DiagonalUpVisible = value;
          break;
      }
    }
  }

  internal OfficeBordersIndex BorderIndex
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  private WorkbookImpl Workbook => this.m_format.Workbook;

  private BorderImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  private BorderImpl(IApplication application, object parent, OfficeBordersIndex borderIndex)
    : this(application, parent)
  {
    this.m_border = borderIndex;
  }

  public BorderImpl(
    IApplication application,
    object parent,
    IInternalExtendedFormat impl,
    OfficeBordersIndex borderIndex)
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
    if (this.LineStyle == OfficeLineStyle.None || this.ColorObject.ColorType != ColorType.Indexed)
      return;
    ChartColor colorObject = this.ColorObject;
    OfficeKnownColors officeKnownColors = BorderImpl.NormalizeColor(colorObject.GetIndexed((IWorkbook) null));
    colorObject.SetIndexed(officeKnownColors);
  }

  public static OfficeKnownColors NormalizeColor(OfficeKnownColors color)
  {
    int num = (int) color;
    if (num == 0)
    {
      num += 64 /*0x40*/;
      color = (OfficeKnownColors) num;
    }
    return (OfficeKnownColors) num;
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
  }
}
