// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.StyleArrayWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class StyleArrayWrapper : 
  CommonObject,
  IStyle,
  IExtendedFormat,
  IParentApplication,
  IOptimizedUpdate,
  IXFIndex
{
  private List<IRange> m_arrRanges = new List<IRange>();
  private WorkbookImpl m_book;
  private IApplication m_application;

  public StyleArrayWrapper(IRange range)
    : base((range as RangeImpl).Application, (object) range)
  {
    this.m_arrRanges.AddRange((IEnumerable<IRange>) range.Cells);
    this.m_book = range.Worksheet.Workbook as WorkbookImpl;
    this.m_application = (range as RangeImpl).Application;
  }

  public StyleArrayWrapper(IApplication application, List<IRange> LstRange, IWorksheet worksheet)
    : base(application, (object) LstRange[0])
  {
    this.m_arrRanges = LstRange;
    this.m_book = worksheet.Workbook as WorkbookImpl;
    this.m_application = application;
  }

  public bool JustifyLast
  {
    get => throw new NotImplementedException("Not implemented property.");
    set => throw new NotImplementedException("Not implemented property.");
  }

  public string NumberFormatLocal
  {
    get => throw new NotImplementedException("Not implemented property.");
    set => throw new NotImplementedException("Not implemented property.");
  }

  public int XFormatIndex
  {
    get
    {
      int count = this.m_arrRanges.Count;
      if (count <= 0)
        return int.MinValue;
      int xformatIndex = ((IXFIndex) this.m_arrRanges[0].CellStyle).XFormatIndex;
      for (int index = 1; index < count; ++index)
      {
        IXFIndex cellStyle = (IXFIndex) this.m_arrRanges[index].CellStyle;
        if (xformatIndex != cellStyle.XFormatIndex)
          return int.MinValue;
      }
      return xformatIndex;
    }
  }

  public bool HasBorder
  {
    get
    {
      for (int index = 0; index < this.m_arrRanges.Count; ++index)
      {
        if (this.m_arrRanges[index].CellStyle.HasBorder)
          return true;
      }
      return false;
    }
  }

  public IBorders Borders
  {
    get
    {
      RangeImpl parent = (IRange) this.Parent as RangeImpl;
      return parent.IsEntireRow || parent.IsEntireColumn ? (IBorders) new BordersCollectionArrayWrapper(this.m_arrRanges, this.m_application) : (IBorders) new BordersCollectionArrayWrapper((IRange) this.Parent);
    }
  }

  public bool BuiltIn
  {
    get
    {
      bool builtIn = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          builtIn = arrRange.CellStyle.BuiltIn;
          flag = false;
        }
        else if (arrRange.CellStyle.BuiltIn != builtIn)
          return false;
      }
      return builtIn;
    }
  }

  public OfficePattern FillPattern
  {
    get
    {
      OfficePattern fillPattern = OfficePattern.None;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          fillPattern = arrRange.CellStyle.FillPattern;
          flag = false;
        }
        else if (arrRange.CellStyle.FillPattern != fillPattern)
          return OfficePattern.None;
      }
      return fillPattern;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.FillPattern = value;
    }
  }

  public OfficeKnownColors FillBackground
  {
    get
    {
      OfficeKnownColors fillBackground = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          fillBackground = arrRange.CellStyle.FillBackground;
          flag = false;
        }
        else if (arrRange.CellStyle.FillBackground != fillBackground)
          return OfficeKnownColors.Black;
      }
      return fillBackground;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.FillBackground = value;
    }
  }

  public Color FillBackgroundRGB
  {
    get => this.m_book.GetPaletteColor(this.FillBackground);
    set => this.FillBackground = this.m_book.GetNearestColor(value);
  }

  public OfficeKnownColors FillForeground
  {
    get
    {
      OfficeKnownColors fillForeground = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          fillForeground = arrRange.CellStyle.FillForeground;
          flag = false;
        }
        else if (arrRange.CellStyle.FillForeground != fillForeground)
          return OfficeKnownColors.Black;
      }
      return fillForeground;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.FillForeground = value;
    }
  }

  public Color FillForegroundRGB
  {
    get => this.m_book.GetPaletteColor(this.FillForeground);
    set => this.FillForeground = this.m_book.GetNearestColor(value);
  }

  public IOfficeFont Font
  {
    get
    {
      IOfficeFont font = (IOfficeFont) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          font = arrRange.CellStyle.Font;
          flag = false;
        }
        else if (arrRange.CellStyle.Font != font)
          return (IRange) this.Parent is RangeImpl parent && (parent.IsEntireRow || parent.IsEntireColumn) ? (IOfficeFont) new FontArrayWrapper(this.m_arrRanges, this.m_application) : (IOfficeFont) new FontArrayWrapper((IRange) this.Parent);
      }
      return font;
    }
  }

  public bool FormulaHidden
  {
    get
    {
      bool formulaHidden = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          formulaHidden = arrRange.CellStyle.FormulaHidden;
          flag = false;
        }
        else if (arrRange.CellStyle.FormulaHidden != formulaHidden)
          return false;
      }
      return formulaHidden;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.FormulaHidden = value;
    }
  }

  public OfficeHAlign HorizontalAlignment
  {
    get
    {
      OfficeHAlign horizontalAlignment = OfficeHAlign.HAlignGeneral;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          horizontalAlignment = arrRange.CellStyle.HorizontalAlignment;
          flag = false;
        }
        else if (arrRange.CellStyle.HorizontalAlignment != horizontalAlignment)
          return OfficeHAlign.HAlignGeneral;
      }
      return horizontalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.HorizontalAlignment = value;
    }
  }

  public bool IncludeAlignment
  {
    get
    {
      bool includeAlignment = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          includeAlignment = arrRange.CellStyle.IncludeAlignment;
          flag = false;
        }
        else if (arrRange.CellStyle.IncludeAlignment != includeAlignment)
          return false;
      }
      return includeAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IncludeAlignment = value;
    }
  }

  public bool IncludeBorder
  {
    get
    {
      bool includeBorder = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          includeBorder = arrRange.CellStyle.IncludeBorder;
          flag = false;
        }
        else if (arrRange.CellStyle.IncludeBorder != includeBorder)
          return false;
      }
      return includeBorder;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IncludeBorder = value;
    }
  }

  public bool IncludeFont
  {
    get
    {
      bool includeFont = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          includeFont = arrRange.CellStyle.IncludeFont;
          flag = false;
        }
        else if (arrRange.CellStyle.IncludeFont != includeFont)
          return false;
      }
      return includeFont;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IncludeFont = value;
    }
  }

  public bool IncludeNumberFormat
  {
    get
    {
      bool includeNumberFormat = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          includeNumberFormat = arrRange.CellStyle.IncludeNumberFormat;
          flag = false;
        }
        else if (arrRange.CellStyle.IncludeNumberFormat != includeNumberFormat)
          return false;
      }
      return includeNumberFormat;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IncludeNumberFormat = value;
    }
  }

  public bool IncludePatterns
  {
    get
    {
      bool includePatterns = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          includePatterns = arrRange.CellStyle.IncludePatterns;
          flag = false;
        }
        else if (arrRange.CellStyle.IncludePatterns != includePatterns)
          return false;
      }
      return includePatterns;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IncludePatterns = value;
    }
  }

  public bool IncludeProtection
  {
    get
    {
      bool includeProtection = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          includeProtection = arrRange.CellStyle.IncludeProtection;
          flag = false;
        }
        else if (arrRange.CellStyle.IncludeProtection != includeProtection)
          return false;
      }
      return includeProtection;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IncludeProtection = value;
    }
  }

  public int IndentLevel
  {
    get
    {
      int indentLevel = 0;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          indentLevel = arrRange.CellStyle.IndentLevel;
          flag = false;
        }
        else if (arrRange.CellStyle.IndentLevel != indentLevel)
          return 0;
      }
      return indentLevel;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IndentLevel = value;
    }
  }

  public bool IsInitialized
  {
    get
    {
      bool hasStyle = ((RangeImpl) this.m_arrRanges[0]).HasStyle;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        if (this.m_arrRanges[index].HasStyle != hasStyle)
          return false;
      }
      return hasStyle;
    }
  }

  public bool Locked
  {
    get
    {
      bool locked = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          locked = arrRange.CellStyle.Locked;
          flag = false;
        }
        else if (arrRange.CellStyle.Locked != locked)
          return false;
      }
      return locked;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.Locked = value;
    }
  }

  public string Name
  {
    get
    {
      string name = (string) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          name = arrRange.CellStyle.Name;
          flag = false;
        }
        else if (arrRange.CellStyle.Name != name)
          return (string) null;
      }
      return name;
    }
  }

  public string NumberFormat
  {
    get
    {
      string numberFormat = (string) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          numberFormat = arrRange.CellStyle.NumberFormat;
          flag = false;
        }
        else if (arrRange.CellStyle.NumberFormat != numberFormat)
          return (string) null;
      }
      return numberFormat;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.NumberFormat = value;
    }
  }

  public int NumberFormatIndex
  {
    get
    {
      int numberFormatIndex = int.MinValue;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          numberFormatIndex = arrRange.CellStyle.NumberFormatIndex;
          flag = false;
        }
        else if (arrRange.CellStyle.NumberFormatIndex != numberFormatIndex)
          return int.MinValue;
      }
      return numberFormatIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.NumberFormatIndex = value;
    }
  }

  public INumberFormat NumberFormatSettings
  {
    get
    {
      return this.NumberFormatIndex <= 0 ? (INumberFormat) null : this.m_arrRanges[0].CellStyle.NumberFormatSettings;
    }
  }

  public int Rotation
  {
    get
    {
      if (this.m_arrRanges.Count == 0)
        return 0;
      int rotation = this.m_arrRanges[0].CellStyle.Rotation;
      int index = 1;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        if (this.m_arrRanges[index].CellStyle.Rotation != rotation)
          return int.MinValue;
      }
      return rotation;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.Rotation = value;
    }
  }

  public bool ShrinkToFit
  {
    get
    {
      bool shrinkToFit = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          shrinkToFit = arrRange.CellStyle.ShrinkToFit;
          flag = false;
        }
        else if (arrRange.CellStyle.ShrinkToFit != shrinkToFit)
          return false;
      }
      return shrinkToFit;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.ShrinkToFit = value;
    }
  }

  public OfficeVAlign VerticalAlignment
  {
    get
    {
      OfficeVAlign verticalAlignment = OfficeVAlign.VAlignBottom;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          verticalAlignment = arrRange.CellStyle.VerticalAlignment;
          flag = false;
        }
        else if (arrRange.CellStyle.VerticalAlignment != verticalAlignment)
          return OfficeVAlign.VAlignBottom;
      }
      return verticalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.VerticalAlignment = value;
    }
  }

  public bool WrapText
  {
    get
    {
      bool wrapText = false;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          wrapText = arrRange.CellStyle.WrapText;
          flag = false;
        }
        else if (arrRange.CellStyle.WrapText != wrapText)
          return false;
      }
      return wrapText;
    }
    set
    {
      List<int> intList = new List<int>();
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        arrRange.CellStyle.WrapText = value;
        int row = arrRange.Row;
        IWorksheet worksheet = arrRange.Worksheet;
        if (!intList.Contains(row))
        {
          if (!WorksheetHelper.GetOrCreateRow(worksheet as IInternalWorksheet, row - 1, false).IsBadFontHeight && !(worksheet.Workbook as WorkbookImpl).IsWorkbookOpening)
            worksheet.AutofitRow(row);
          intList.Add(row);
        }
      }
    }
  }

  public OfficeReadingOrderType ReadingOrder
  {
    get
    {
      List<IRange> rangeList = this.m_arrRanges != null ? this.m_arrRanges : throw new ApplicationException("Blank collection");
      OfficeReadingOrderType readingOrder = rangeList[0].CellStyle.ReadingOrder;
      if (readingOrder == OfficeReadingOrderType.Context)
        return OfficeReadingOrderType.Context;
      int index = 1;
      for (int count = rangeList.Count; index < count; ++index)
      {
        IRange range = rangeList[index];
        if (readingOrder != range.CellStyle.ReadingOrder)
          return OfficeReadingOrderType.Context;
      }
      return readingOrder;
    }
    set
    {
      if (this.m_arrRanges == null)
        throw new ApplicationException("Blank collection");
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.ReadingOrder = value;
    }
  }

  public bool IsFirstSymbolApostrophe
  {
    get
    {
      bool symbolApostrophe = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count && !symbolApostrophe; ++index)
        symbolApostrophe = this.m_arrRanges[index].CellStyle.IsFirstSymbolApostrophe;
      return symbolApostrophe;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.IsFirstSymbolApostrophe = value;
    }
  }

  public OfficeKnownColors PatternColorIndex
  {
    get
    {
      OfficeKnownColors patternColorIndex = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          patternColorIndex = arrRange.CellStyle.PatternColorIndex;
          flag = false;
        }
        else if (arrRange.CellStyle.PatternColorIndex != patternColorIndex)
          return OfficeKnownColors.Black;
      }
      return patternColorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.PatternColorIndex = value;
    }
  }

  public Color PatternColor
  {
    get
    {
      Color patternColor = ColorExtension.Empty;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          patternColor = arrRange.CellStyle.PatternColor;
          flag = false;
        }
        else if (arrRange.CellStyle.PatternColor.ToArgb() != patternColor.ToArgb())
          return ColorExtension.Empty;
      }
      return patternColor;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.PatternColor = value;
    }
  }

  public OfficeKnownColors ColorIndex
  {
    get
    {
      OfficeKnownColors colorIndex = OfficeKnownColors.Black;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          colorIndex = arrRange.CellStyle.ColorIndex;
          flag = false;
        }
        else if (arrRange.CellStyle.ColorIndex != colorIndex)
          return OfficeKnownColors.Black;
      }
      return colorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.ColorIndex = value;
    }
  }

  public Color Color
  {
    get
    {
      Color color = ColorExtension.Empty;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          color = arrRange.CellStyle.Color;
          flag = false;
        }
        else if (arrRange.CellStyle.Color.ToArgb() != color.ToArgb())
          return ColorExtension.Empty;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
        this.m_arrRanges[index].CellStyle.Color = value;
    }
  }

  public IInterior Interior
  {
    get
    {
      IInterior interior = (IInterior) null;
      bool flag = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count; ++index)
      {
        IRange arrRange = this.m_arrRanges[index];
        if (flag)
        {
          interior = arrRange.CellStyle.Interior;
          flag = false;
        }
        else if (arrRange.CellStyle.Interior != interior)
          return (IInterior) new InteriorArrayWrapper((IRange) this.Parent);
      }
      return interior;
    }
  }

  public bool IsModified
  {
    get
    {
      bool isModified = true;
      int index = 0;
      for (int count = this.m_arrRanges.Count; index < count && !isModified; ++index)
        isModified = this.m_arrRanges[index].CellStyle.IsModified;
      return isModified;
    }
  }

  public virtual void BeginUpdate()
  {
    int index = 0;
    for (int count = this.m_arrRanges.Count; index < count; ++index)
      this.m_arrRanges[index].CellStyle.BeginUpdate();
  }

  public virtual void EndUpdate()
  {
    int index = 0;
    for (int count = this.m_arrRanges.Count; index < count; ++index)
      this.m_arrRanges[index].CellStyle.EndUpdate();
  }
}
