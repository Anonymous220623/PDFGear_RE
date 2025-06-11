// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ChartColor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using System;
using System.Drawing;
using System.Reflection;

#nullable disable
namespace Syncfusion.OfficeChart;

public class ChartColor : IDisposable
{
  private ColorType m_colorType;
  private int m_color;
  private double m_tintAndShade;
  private double m_satMod;
  private double m_lumOff;
  private double m_sat;
  private double m_lumMod;
  private bool m_bIsSchemeColor;
  private string m_schemaName;
  private string m_hexColor;

  public event ChartColor.AfterChangeHandler AfterChange;

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

  public ChartColor(Color color)
    : this(ColorType.RGB, color.ToArgb())
  {
  }

  public ChartColor(OfficeKnownColors color)
    : this(ColorType.Indexed, (int) color)
  {
  }

  public ChartColor(ColorType colorType, int colorValue)
    : this(colorType, colorValue, 0.0)
  {
  }

  public ChartColor(ColorType colorType, int colorValue, double tint)
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

  internal OfficeKnownColors GetIndexed(IWorkbook book)
  {
    return this.m_colorType != ColorType.Indexed ? (book as WorkbookImpl).GetNearestColor(this.GetRGB(book), 8) : (OfficeKnownColors) this.m_color;
  }

  public void SetIndexed(OfficeKnownColors value) => this.SetIndexed(value, true);

  public void SetIndexed(OfficeKnownColors value, bool raiseEvent)
  {
    if (this.m_colorType == ColorType.Indexed && (OfficeKnownColors) this.m_color == value)
      return;
    this.m_colorType = ColorType.Indexed;
    this.m_color = (int) value;
    this.Normalize(false);
    this.m_tintAndShade = 0.0;
    if (!raiseEvent || this.AfterChange == null)
      return;
    this.AfterChange();
  }

  internal void SetIndexed(OfficeKnownColors value, bool raiseEvent, WorkbookImpl book)
  {
    if (this.m_colorType == ColorType.Indexed && (OfficeKnownColors) this.m_color == value)
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

  internal Color GetRGB(IWorkbook book)
  {
    Color color;
    switch (this.m_colorType)
    {
      case ColorType.Indexed:
        color = book.GetPaletteColor((OfficeKnownColors) this.m_color);
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
      if (index2 < length1 && index1 < length2 && WorkbookImpl.ThemeColorPalette[index2].Length > index1)
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

  internal void SetRGB(Color value, IWorkbook book) => this.SetRGB(value, book, 0.0);

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

  public static implicit operator ChartColor(Color color) => new ChartColor(color);

  public static bool operator ==(ChartColor first, ChartColor second)
  {
    object obj1 = (object) first;
    object obj2 = (object) second;
    if (obj1 == null && obj2 == null)
      return true;
    return (obj1 != null || obj2 == null) && (obj1 == null || obj2 != null) && first.m_colorType == second.m_colorType && first.m_color == second.m_color && first.m_tintAndShade == second.m_tintAndShade;
  }

  public static bool operator !=(ChartColor first, ChartColor second)
  {
    object obj1 = (object) first;
    object obj2 = (object) second;
    if (obj1 == null && obj2 == null)
      return false;
    return obj1 == null && obj2 != null || obj1 != null && obj2 == null || first.m_colorType != second.m_colorType || first.m_color != second.m_color || first.m_tintAndShade != second.m_tintAndShade;
  }

  internal void CopyFrom(ChartColor colorObject, bool callEvent)
  {
    this.m_color = !(colorObject == (ChartColor) null) ? colorObject.m_color : throw new ArgumentNullException(nameof (colorObject));
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

  public void SetIndexedNoEvent(OfficeKnownColors value)
  {
    this.m_colorType = ColorType.Indexed;
    this.m_color = (int) value;
  }

  internal ChartColor Clone() => (ChartColor) this.MemberwiseClone();

  internal void Normalize() => this.Normalize(true);

  internal void Normalize(bool raiseEvent)
  {
    if (this.m_colorType != ColorType.Indexed)
      return;
    int color = this.m_color;
    if (this.m_color == 0)
      this.m_color += 64 /*0x40*/;
    else if (this.m_color < 8 && this.AfterChange != null && this.AfterChange.Method != (MethodInfo) null && (this.AfterChange.Method.Name == "UpdateForeColor" || this.AfterChange.Method.Name == "ColorChangeEventHandler"))
      this.m_color += 8;
    if (color == this.m_color)
      return;
    this.SetIndexed((OfficeKnownColors) this.m_color, raiseEvent);
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
    this.SetIndexed((OfficeKnownColors) this.m_color, raiseEvent);
  }

  public override bool Equals(object obj)
  {
    ChartColor chartColor = obj as ChartColor;
    return obj != null && chartColor == this;
  }

  internal void SetTheme(int themeIndex, IWorkbook book) => this.SetTheme(themeIndex, book, 0.0);

  internal void SetTheme(int themeIndex, IWorkbook book, double dTintValue)
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

  internal void SetRGB(Color rgb, IWorkbook book, double dTintValue)
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
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateColor);
          break;
        case "UpdatePatternColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdatePatternColor);
          break;
        case "UpdateTopBorderColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateTopBorderColor);
          break;
        case "UpdateBottomBorderColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateBottomBorderColor);
          break;
        case "UpdateLeftBorderColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateLeftBorderColor);
          break;
        case "UpdateRightBorderColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateRightBorderColor);
          break;
        case "UpdateDiagonalBorderColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ExtendedFormatImpl).UpdateDiagonalBorderColor);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartBorderImpl)
    {
      switch (name)
      {
        case "UpdateColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ChartBorderImpl).UpdateColor);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartInteriorImpl)
    {
      switch (name)
      {
        case "UpdateForeColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ChartInteriorImpl).UpdateForeColor);
          break;
        case "UpdateBackColor":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ChartInteriorImpl).UpdateBackColor);
          break;
      }
    }
    else if (this.AfterChange.Target is ChartSerieDataFormatImpl)
    {
      switch (name)
      {
        case "MarkerForeColorChanged":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ChartSerieDataFormatImpl).MarkerForeColorChanged);
          break;
        case "MarkerBackColorChanged":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ChartSerieDataFormatImpl).MarkerBackColorChanged);
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
    else if (this.AfterChange.Target is FontImpl)
    {
      switch (name)
      {
        case "UpdateRecord":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as FontImpl).UpdateRecord);
          break;
      }
    }
    else if (this.AfterChange.Target is FontWrapper)
    {
      switch (name)
      {
        case "UpdateRecord":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as FontWrapper).ColorObjectUpdate);
          break;
      }
    }
    else if (this.AfterChange.Target is ShadowImpl)
    {
      switch (name)
      {
        case "ShadowColorChanged":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ShadowImpl).ShadowColorChanged);
          break;
      }
    }
    else if ((object) (this.AfterChange.Target as ShapeFillImpl) != null)
    {
      switch (name)
      {
        case "ShadowColorChanged":
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ShapeFillImpl).ChangeVisible);
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
          this.AfterChange -= new ChartColor.AfterChangeHandler((this.AfterChange.Target as ChartBorderImpl).ClearAutoColor);
          break;
      }
    }
  }

  public delegate void AfterChangeHandler();
}
