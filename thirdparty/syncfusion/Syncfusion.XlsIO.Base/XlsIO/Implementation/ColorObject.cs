// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorObject
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using System;
using System.Drawing;
using System.Reflection;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ColorObject : IDisposable
{
  private ColorType m_colorType;
  internal int m_color;
  private double m_tintAndShade;
  private double m_satMod;
  private double m_lumOff;
  private double m_sat;
  private double m_lumMod;
  private bool m_bIsSchemeColor;
  private string m_schemaName;
  private string m_hexColor;

  public event ColorObject.AfterChangeHandler AfterChange;

  public int Value => this.m_color;

  public double Tint
  {
    get => this.m_tintAndShade;
    set => this.m_tintAndShade = value;
  }

  internal double Saturation
  {
    get => this.m_satMod;
    set => this.m_satMod = value;
  }

  internal double Luminance
  {
    get => this.m_lumMod;
    set => this.m_lumMod = value;
  }

  internal double LuminanceOffSet
  {
    get => this.m_lumOff;
    set => this.m_lumOff = value;
  }

  internal bool IsSchemeColor
  {
    get => this.m_bIsSchemeColor;
    set => this.m_bIsSchemeColor = value;
  }

  internal string SchemaName
  {
    get => this.m_schemaName;
    set => this.m_schemaName = value;
  }

  internal string HexColor
  {
    get => this.m_hexColor;
    set => this.m_hexColor = value;
  }

  public ColorObject(Color color)
    : this(ColorType.RGB, color.ToArgb())
  {
  }

  public ColorObject(ExcelKnownColors color)
    : this(ColorType.Indexed, (int) color)
  {
  }

  public ColorObject(ColorType colorType, int colorValue)
    : this(colorType, colorValue, 0.0)
  {
  }

  public ColorObject(ColorType colorType, int colorValue, double tint)
  {
    this.m_colorType = colorType;
    this.m_color = colorValue;
    this.m_tintAndShade = tint;
  }

  public ColorType ColorType
  {
    get => this.m_colorType;
    set => this.m_colorType = value;
  }

  public ExcelKnownColors GetIndexed(IWorkbook book)
  {
    return this.m_colorType != ColorType.Indexed ? (book as WorkbookImpl).GetNearestColor(this.GetRGB(book), 8) : (ExcelKnownColors) this.m_color;
  }

  public void SetIndexed(ExcelKnownColors value) => this.SetIndexed(value, true);

  public void SetIndexed(ExcelKnownColors value, bool raiseEvent)
  {
    if (this.m_colorType == ColorType.Indexed && (ExcelKnownColors) this.m_color == value)
      return;
    this.m_colorType = ColorType.Indexed;
    this.m_color = (int) value;
    this.Normalize(false, true);
    this.m_tintAndShade = 0.0;
    if (!raiseEvent || this.AfterChange == null)
      return;
    this.AfterChange();
  }

  internal void SetIndexed(
    Color color,
    WorkbookImpl workbook,
    ExcelKnownColors value,
    bool raiseEvent)
  {
    if (this.m_colorType == ColorType.Indexed && (ExcelKnownColors) this.m_color == value)
      return;
    this.m_colorType = ColorType.Indexed;
    this.m_color = (int) value;
    for (int index = 8; index < workbook.InnerPalette.Count; ++index)
    {
      if ((int) color.R == (int) workbook.InnerPalette[index].R && (int) color.G == (int) workbook.InnerPalette[index].G && (int) color.B == (int) workbook.InnerPalette[index].B)
      {
        this.m_color = index;
        break;
      }
    }
    this.Normalize(false, false);
    this.m_tintAndShade = 0.0;
    if (!raiseEvent || this.AfterChange == null)
      return;
    this.AfterChange();
  }

  public void SetIndexed(ExcelKnownColors value, bool raiseEvent, WorkbookImpl book)
  {
    if (this.m_colorType == ColorType.Indexed && (ExcelKnownColors) this.m_color == value)
      return;
    this.m_colorType = ColorType.Indexed;
    this.m_color = (int) value;
    if (!book.IsEqualColor)
      this.Normalize(false, book);
    this.m_tintAndShade = 0.0;
    if (!raiseEvent || this.AfterChange == null)
      return;
    this.AfterChange();
  }

  public Color GetRGB(IWorkbook book)
  {
    Color color;
    switch (this.m_colorType)
    {
      case ColorType.Indexed:
        color = book.GetPaletteColor((ExcelKnownColors) this.m_color);
        break;
      case ColorType.RGB:
        color = ColorExtension.FromArgb(this.m_color);
        break;
      case ColorType.Theme:
        color = (book as WorkbookImpl).GetThemeColor(this.m_color);
        break;
      default:
        throw new InvalidOperationException();
    }
    if (this.m_tintAndShade != 0.0)
    {
      int index1 = 0;
      int index2 = 0;
      int length1 = WorkbookImpl.ThemeColorPalette.Length;
      int length2 = WorkbookImpl.DefaultTints.Length;
      double[] defaultTints = WorkbookImpl.DefaultTints;
      for (int index3 = 0; index3 < defaultTints.Length && defaultTints[index3] != this.m_tintAndShade; ++index3)
        ++index1;
      Color[] defaultThemeColors = WorkbookImpl.DefaultThemeColors;
      for (int index4 = 0; index4 < defaultThemeColors.Length && !defaultThemeColors[index4].Equals((object) color); ++index4)
        ++index2;
      if (index2 < length1 && index1 < length2 && WorkbookImpl.ThemeColorPalette[index2].Length > index1 && WorkbookImpl.ThemeColorPalette[index2][index1].A != (byte) 0 && WorkbookImpl.ThemeColorPalette[index2][index1].R != (byte) 0 && WorkbookImpl.ThemeColorPalette[index2][index1].G != (byte) 0 && WorkbookImpl.ThemeColorPalette[index2][index1].B != (byte) 0)
      {
        color = WorkbookImpl.ThemeColorPalette[index2][index1];
      }
      else
      {
        double dTint = this.m_tintAndShade;
        if (dTint > 100.0)
          dTint = this.m_tintAndShade / 100000.0;
        color = Excel2007Parser.ConvertColorByTint(color, dTint);
      }
    }
    return color;
  }

  public void SetRGB(Color value, IWorkbook book) => this.SetRGB(value, book, 0.0);

  internal void SetRGB(Color value)
  {
    int argb1 = value.ToArgb();
    int argb2 = Color.Black.ToArgb();
    if (this.m_colorType == ColorType.RGB && this.m_color == argb1 && this.m_color != argb2)
      return;
    this.m_colorType = ColorType.RGB;
    this.m_color = argb1;
    this.m_tintAndShade = 0.0;
    if (this.AfterChange == null)
      return;
    this.AfterChange();
  }

  public static implicit operator ColorObject(Color color) => new ColorObject(color);

  public static bool operator ==(ColorObject first, ColorObject second)
  {
    object obj1 = (object) first;
    object obj2 = (object) second;
    if (obj1 == null && obj2 == null)
      return true;
    return (obj1 != null || obj2 == null) && (obj1 == null || obj2 != null) && first.m_colorType == second.m_colorType && first.m_color == second.m_color && first.m_tintAndShade == second.m_tintAndShade;
  }

  public static bool operator !=(ColorObject first, ColorObject second)
  {
    object obj1 = (object) first;
    object obj2 = (object) second;
    if (obj1 == null && obj2 == null)
      return false;
    return obj1 == null && obj2 != null || obj1 != null && obj2 == null || first.m_colorType != second.m_colorType || first.m_color != second.m_color || first.m_tintAndShade != second.m_tintAndShade;
  }

  internal void CopyFrom(ColorObject colorObject, bool callEvent)
  {
    this.m_color = !(colorObject == (ColorObject) null) ? colorObject.m_color : throw new ArgumentNullException(nameof (colorObject));
    this.m_colorType = colorObject.m_colorType;
    this.m_tintAndShade = colorObject.m_tintAndShade;
    if (!callEvent || this.AfterChange == null)
      return;
    this.AfterChange();
  }

  internal void ConvertToIndexed(IWorkbook book)
  {
    if (this.m_colorType == ColorType.Indexed)
      return;
    this.SetIndexed(this.GetIndexed(book));
  }

  public override int GetHashCode() => this.m_color.GetHashCode() ^ this.m_colorType.GetHashCode();

  public void SetIndexedNoEvent(ExcelKnownColors value)
  {
    this.m_colorType = ColorType.Indexed;
    this.m_color = (int) value;
  }

  internal ColorObject Clone() => (ColorObject) this.MemberwiseClone();

  internal void Normalize() => this.Normalize(true, true);

  internal void Normalize(bool raiseEvent, bool bskip)
  {
    if (this.m_colorType != ColorType.Indexed)
      return;
    int color = this.m_color;
    if (this.m_color == 0)
      this.m_color += 64 /*0x40*/;
    else if (this.m_color < 8 && this.AfterChange != null && this.AfterChange.Method != (MethodInfo) null && (this.AfterChange.Method.Name == "UpdateForeColor" || this.AfterChange.Method.Name == "ColorChangeEventHandler") && bskip)
      this.m_color += 8;
    if (color == this.m_color)
      return;
    this.SetIndexed((ExcelKnownColors) this.m_color, raiseEvent);
  }

  internal void Normalize(bool raiseEvent, WorkbookImpl book)
  {
    if (this.m_colorType != ColorType.Indexed)
      return;
    int color = this.m_color;
    if (this.m_color == 0)
      this.m_color += 64 /*0x40*/;
    if (color == this.m_color)
      return;
    this.SetIndexed((ExcelKnownColors) this.m_color, raiseEvent);
  }

  public override bool Equals(object obj)
  {
    ColorObject colorObject = obj as ColorObject;
    return obj != null && colorObject == this;
  }

  public void SetTheme(int themeIndex, IWorkbook book) => this.SetTheme(themeIndex, book, 0.0);

  public void SetTheme(int themeIndex, IWorkbook book, double dTintValue)
  {
    if (this.m_colorType == ColorType.Theme && this.m_color == themeIndex)
      return;
    this.m_colorType = ColorType.Theme;
    this.m_color = themeIndex;
    this.m_tintAndShade = dTintValue;
    if (this.AfterChange == null)
      return;
    this.AfterChange();
  }

  public void SetRGB(Color rgb, IWorkbook book, double dTintValue)
  {
    int argb1 = rgb.ToArgb();
    int argb2 = Color.Black.ToArgb();
    if (this.m_colorType == ColorType.RGB && this.m_color == argb1 && this.m_color != argb2)
      return;
    this.m_colorType = ColorType.RGB;
    this.m_color = argb1;
    this.m_tintAndShade = 0.0;
    if (this.AfterChange == null)
      return;
    this.AfterChange();
  }

  public void Dispose()
  {
    if (this.AfterChange != null)
      this.DetachEvents();
    GC.SuppressFinalize((object) this);
  }

  private void DetachEvents()
  {
    string name = this.AfterChange.Method.Name;
    if (this.AfterChange.Target is ExtendedFormatImpl)
    {
      switch (name)
      {
        case "UpdateColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateColor);
          break;
        case "UpdatePatternColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdatePatternColor);
          break;
        case "UpdateTopBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateTopBorderColor);
          break;
        case "UpdateBottomBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateBottomBorderColor);
          break;
        case "UpdateLeftBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateLeftBorderColor);
          break;
        case "UpdateRightBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateRightBorderColor);
          break;
        case "UpdateDiagonalBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateDiagonalBorderColor);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartBorderImpl)
    {
      switch (name)
      {
        case "UpdateColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ChartBorderImpl).UpdateColor);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartInteriorImpl)
    {
      switch (name)
      {
        case "UpdateForeColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ChartInteriorImpl).UpdateForeColor);
          break;
        case "UpdateBackColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ChartInteriorImpl).UpdateBackColor);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartSerieDataFormatImpl)
    {
      switch (name)
      {
        case "MarkerForeColorChanged":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ChartSerieDataFormatImpl).MarkerForeColorChanged);
          break;
        case "MarkerBackColorChanged":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ChartSerieDataFormatImpl).MarkerBackColorChanged);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartTextAreaImpl)
    {
      switch (name)
      {
        case "ColorChangeEventHandler":
          (this.AfterChange.Target as ChartTextAreaImpl).DetachEvents();
          break;
      }
    }
    else if (this.AfterChange.Target is ConditionalFormatImpl)
    {
      switch (name)
      {
        case "UpdateColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateColor);
          break;
        case "UpdateBackColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateBackColor);
          break;
        case "UpdateTopBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateTopBorderColor);
          break;
        case "UpdateBottomBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateBottomBorderColor);
          break;
        case "UpdateLeftBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateLeftBorderColor);
          break;
        case "UpdateRightBorderColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateRightBorderColor);
          break;
        case "UpdateFontColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ConditionalFormatImpl).UpdateFontColor);
          break;
      }
    }
    else if (this.AfterChange.Target is FontImpl)
    {
      switch (name)
      {
        case "UpdateRecord":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as FontImpl).UpdateRecord);
          break;
      }
    }
    else if (this.AfterChange.Target is FontWrapper)
    {
      switch (name)
      {
        case "UpdateRecord":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as FontWrapper).ColorObjectUpdate);
          break;
      }
    }
    else if (this.AfterChange.Target is ShadowImpl)
    {
      switch (name)
      {
        case "ShadowColorChanged":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ShadowImpl).ShadowColorChanged);
          break;
      }
    }
    else if (this.AfterChange.Target is ShapeFillImpl)
    {
      switch (name)
      {
        case "ShadowColorChanged":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ShapeFillImpl).ChangeVisible);
          break;
      }
    }
    else
    {
      if (!(this.AfterChange.Target is ChartBorderImpl))
        return;
      switch (name)
      {
        case "ClearAutoColor":
          this.AfterChange -= new ColorObject.AfterChangeHandler((this.AfterChange.Target as ChartBorderImpl).ClearAutoColor);
          break;
      }
    }
  }

  public delegate void AfterChangeHandler();
}
