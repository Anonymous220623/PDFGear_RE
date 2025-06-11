// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.Grouping.StyleGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections.Grouping;

public class StyleGroup : 
  CommonObject,
  IStyle,
  IExtendedFormat,
  IParentApplication,
  IOptimizedUpdate,
  IXFIndex
{
  private RangeGroup m_rangeGroup;
  private FontGroup m_font;
  private BordersGroup m_borders;

  public StyleGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private void FindParents()
  {
    this.m_rangeGroup = this.FindParent(typeof (RangeGroup)) as RangeGroup;
    if (this.m_rangeGroup == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent range group.");
  }

  public IStyle this[int index] => this.m_rangeGroup[index].CellStyle;

  public int Count => this.m_rangeGroup.Count;

  public WorkbookImpl Workbook => this.m_rangeGroup.Workbook;

  public IBorders Borders
  {
    get
    {
      if (this.m_borders == null)
        this.m_borders = new BordersGroup(this.Application, (object) this);
      return (IBorders) this.m_borders;
    }
  }

  public bool BuiltIn
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool builtIn = this[0].BuiltIn;
      for (int index = 1; index < count; ++index)
      {
        if (builtIn != this[index].BuiltIn)
          return false;
      }
      return builtIn;
    }
  }

  public ExcelPattern FillPattern
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelPattern.None;
      ExcelPattern fillPattern = this[0].FillPattern;
      for (int index = 1; index < count; ++index)
      {
        if (fillPattern != this[index].FillPattern)
          return ExcelPattern.None;
      }
      return fillPattern;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FillPattern = value;
    }
  }

  public ExcelKnownColors FillBackground
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelKnownColors.None;
      ExcelKnownColors fillBackground = this[0].FillBackground;
      for (int index = 1; index < count; ++index)
      {
        if (fillBackground != this[index].FillBackground)
          return ExcelKnownColors.None;
      }
      return fillBackground;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FillBackground = value;
    }
  }

  public Color FillBackgroundRGB
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ColorExtension.Empty;
      Color fillBackgroundRgb = this[0].FillBackgroundRGB;
      for (int index = 1; index < count; ++index)
      {
        if (fillBackgroundRgb != this[index].FillBackgroundRGB)
          return ColorExtension.Empty;
      }
      return fillBackgroundRgb;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FillBackgroundRGB = value;
    }
  }

  public ExcelKnownColors FillForeground
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelKnownColors.None;
      ExcelKnownColors fillForeground = this[0].FillForeground;
      for (int index = 1; index < count; ++index)
      {
        if (fillForeground != this[index].FillForeground)
          return ExcelKnownColors.None;
      }
      return fillForeground;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FillForeground = value;
    }
  }

  public Color FillForegroundRGB
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ColorExtension.Empty;
      Color fillForegroundRgb = this[0].FillForegroundRGB;
      for (int index = 1; index < count; ++index)
      {
        if (fillForegroundRgb != this[index].FillForegroundRGB)
          return ColorExtension.Empty;
      }
      return fillForegroundRgb;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FillForegroundRGB = value;
    }
  }

  public IFont Font
  {
    get
    {
      if (this.m_font == null)
        this.m_font = new FontGroup(this.Application, (object) this);
      return (IFont) this.m_font;
    }
  }

  public IInterior Interior => throw new NotImplementedException();

  public bool FormulaHidden
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool formulaHidden = this[0].FormulaHidden;
      for (int index = 1; index < count; ++index)
      {
        if (formulaHidden != this[index].FormulaHidden)
          return false;
      }
      return formulaHidden;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FormulaHidden = value;
    }
  }

  public ExcelHAlign HorizontalAlignment
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelHAlign.HAlignGeneral;
      ExcelHAlign horizontalAlignment = this[0].HorizontalAlignment;
      for (int index = 1; index < count; ++index)
      {
        if (horizontalAlignment != this[index].HorizontalAlignment)
          return ExcelHAlign.HAlignGeneral;
      }
      return horizontalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].HorizontalAlignment = value;
    }
  }

  public bool IncludeAlignment
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool includeAlignment = this[0].IncludeAlignment;
      for (int index = 1; index < count; ++index)
      {
        if (includeAlignment != this[index].IncludeAlignment)
          return false;
      }
      return includeAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IncludeAlignment = value;
    }
  }

  public bool IncludeBorder
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool includeBorder = this[0].IncludeBorder;
      for (int index = 1; index < count; ++index)
      {
        if (includeBorder != this[index].IncludeBorder)
          return false;
      }
      return includeBorder;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IncludeBorder = value;
    }
  }

  public bool IncludeFont
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool includeFont = this[0].IncludeFont;
      for (int index = 1; index < count; ++index)
      {
        if (includeFont != this[index].IncludeFont)
          return false;
      }
      return includeFont;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IncludeFont = value;
    }
  }

  public bool IncludeNumberFormat
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool includeNumberFormat = this[0].IncludeNumberFormat;
      for (int index = 1; index < count; ++index)
      {
        if (includeNumberFormat != this[index].IncludeNumberFormat)
          return false;
      }
      return includeNumberFormat;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IncludeNumberFormat = value;
    }
  }

  public bool IncludePatterns
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool includePatterns = this[0].IncludePatterns;
      for (int index = 1; index < count; ++index)
      {
        if (includePatterns != this[index].IncludePatterns)
          return false;
      }
      return includePatterns;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IncludePatterns = value;
    }
  }

  public bool IncludeProtection
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool includeProtection = this[0].IncludeProtection;
      for (int index = 1; index < count; ++index)
      {
        if (includeProtection != this[index].IncludeProtection)
          return false;
      }
      return includeProtection;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IncludeProtection = value;
    }
  }

  public int IndentLevel
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return int.MinValue;
      int indentLevel = this[0].IndentLevel;
      for (int index = 1; index < count; ++index)
      {
        if (indentLevel != this[index].IndentLevel)
          return int.MinValue;
      }
      return indentLevel;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IndentLevel = value;
    }
  }

  public bool Locked
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool locked = this[0].Locked;
      for (int index = 1; index < count; ++index)
      {
        if (locked != this[index].Locked)
          return false;
      }
      return locked;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Locked = value;
    }
  }

  public string Name
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (string) null;
      string name = this[0].Name;
      for (int index = 1; index < count; ++index)
      {
        if (name != this[index].Name)
          return (string) null;
      }
      return name;
    }
  }

  public string NumberFormat
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (string) null;
      string numberFormat = this[0].NumberFormat;
      for (int index = 1; index < count; ++index)
      {
        if (numberFormat != this[index].NumberFormat)
          return (string) null;
      }
      return numberFormat;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].NumberFormat = value;
    }
  }

  public string NumberFormatLocal
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (string) null;
      string numberFormatLocal = this[0].NumberFormatLocal;
      for (int index = 1; index < count; ++index)
      {
        if (numberFormatLocal != this[index].NumberFormatLocal)
          return (string) null;
      }
      return numberFormatLocal;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].NumberFormatLocal = value;
    }
  }

  public int NumberFormatIndex
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return int.MinValue;
      int numberFormatIndex = this[0].NumberFormatIndex;
      for (int index = 1; index < count; ++index)
      {
        if (numberFormatIndex != this[index].NumberFormatIndex)
          return int.MinValue;
      }
      return numberFormatIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].NumberFormatIndex = value;
    }
  }

  public INumberFormat NumberFormatSettings
  {
    get
    {
      return this.NumberFormatIndex < 0 ? (INumberFormat) null : (INumberFormat) this.Workbook.InnerFormats[this.NumberFormatIndex];
    }
  }

  public int Rotation
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return int.MinValue;
      int rotation = this[0].Rotation;
      for (int index = 1; index < count; ++index)
      {
        if (rotation != this[index].Rotation)
          return int.MinValue;
      }
      return rotation;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Rotation = value;
    }
  }

  public bool ShrinkToFit
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool shrinkToFit = this[0].ShrinkToFit;
      for (int index = 1; index < count; ++index)
      {
        if (shrinkToFit != this[index].ShrinkToFit)
          return false;
      }
      return shrinkToFit;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].ShrinkToFit = value;
    }
  }

  public ExcelVAlign VerticalAlignment
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelVAlign.VAlignTop;
      ExcelVAlign verticalAlignment = this[0].VerticalAlignment;
      for (int index = 1; index < count; ++index)
      {
        if (verticalAlignment != this[index].VerticalAlignment)
          return ExcelVAlign.VAlignTop;
      }
      return verticalAlignment;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].VerticalAlignment = value;
    }
  }

  public bool WrapText
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool wrapText = this[0].WrapText;
      for (int index = 1; index < count; ++index)
      {
        if (wrapText != this[index].WrapText)
          return false;
      }
      return wrapText;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].WrapText = value;
    }
  }

  public bool IsInitialized
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool isInitialized = this[0].IsInitialized;
      for (int index = 1; index < count; ++index)
      {
        if (isInitialized != this[index].IsInitialized)
          return false;
      }
      return isInitialized;
    }
  }

  public ExcelReadingOrderType ReadingOrder
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelReadingOrderType.Context;
      ExcelReadingOrderType readingOrder = this[0].ReadingOrder;
      for (int index = 1; index < count; ++index)
      {
        if (readingOrder != this[index].ReadingOrder)
          return ExcelReadingOrderType.Context;
      }
      return readingOrder;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].ReadingOrder = value;
    }
  }

  public bool IsFirstSymbolApostrophe
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool symbolApostrophe = this[0].IsFirstSymbolApostrophe;
      for (int index = 1; index < count; ++index)
      {
        if (symbolApostrophe != this[index].IsFirstSymbolApostrophe)
          return false;
      }
      return symbolApostrophe;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].IsFirstSymbolApostrophe = value;
    }
  }

  public bool JustifyLast
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool justifyLast = this[0].JustifyLast;
      for (int index = 1; index < count; ++index)
      {
        if (justifyLast != this[index].JustifyLast)
          return false;
      }
      return justifyLast;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].JustifyLast = value;
    }
  }

  public ExcelKnownColors PatternColorIndex
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelKnownColors.None;
      ExcelKnownColors patternColorIndex = this[0].PatternColorIndex;
      for (int index = 1; index < count; ++index)
      {
        if (patternColorIndex != this[index].PatternColorIndex)
          return ExcelKnownColors.None;
      }
      return patternColorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].PatternColorIndex = value;
    }
  }

  public Color PatternColor
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ColorExtension.Empty;
      Color patternColor = this[0].PatternColor;
      for (int index = 1; index < count; ++index)
      {
        if (patternColor != this[index].PatternColor)
          return ColorExtension.Empty;
      }
      return patternColor;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].PatternColor = value;
    }
  }

  public ExcelKnownColors ColorIndex
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ExcelKnownColors.None;
      ExcelKnownColors colorIndex = this[0].ColorIndex;
      for (int index = 1; index < count; ++index)
      {
        if (colorIndex != this[index].ColorIndex)
          return ExcelKnownColors.None;
      }
      return colorIndex;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].ColorIndex = value;
    }
  }

  public Color Color
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ColorExtension.Empty;
      Color color = this[0].Color;
      for (int index = 1; index < count; ++index)
      {
        if (color != this[index].Color)
          return ColorExtension.Empty;
      }
      return color;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Color = value;
    }
  }

  public bool IsModified
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool isModified = this[0].IsModified;
      for (int index = 1; index < count; ++index)
      {
        if (isModified != this[index].IsModified)
          return false;
      }
      return isModified;
    }
  }

  public virtual void BeginUpdate()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].BeginUpdate();
  }

  public virtual void EndUpdate()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].EndUpdate();
  }

  public bool HasBorder => throw new ArgumentException("No need to implement");

  public int XFormatIndex
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return int.MinValue;
      int xformatIndex = ((IXFIndex) this[0]).XFormatIndex;
      for (int index = 1; index < count; ++index)
      {
        if (xformatIndex != ((IXFIndex) this[index]).XFormatIndex)
          return int.MinValue;
      }
      return xformatIndex;
    }
  }
}
