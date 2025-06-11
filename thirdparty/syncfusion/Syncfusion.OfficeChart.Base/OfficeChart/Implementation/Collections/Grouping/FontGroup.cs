// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.FontGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class FontGroup : CommonObject, IOfficeFont, IParentApplication, IOptimizedUpdate
{
  private StyleGroup m_styleGroup;

  public FontGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private void FindParents()
  {
    this.m_styleGroup = this.FindParent(typeof (StyleGroup)) as StyleGroup;
    if (this.m_styleGroup == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent style group.");
  }

  public IOfficeFont this[int index] => this.m_styleGroup[index].Font;

  public int Count => this.m_styleGroup.Count;

  public bool Bold
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool bold = this[0].Bold;
      for (int index = 1; index < count; ++index)
      {
        if (bold != this[index].Bold)
          return false;
      }
      return bold;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Bold = value;
    }
  }

  public OfficeKnownColors Color
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return OfficeKnownColors.Black;
      OfficeKnownColors color = this[0].Color;
      for (int index = 1; index < count; ++index)
      {
        if (color != this[index].Color)
          return OfficeKnownColors.Black;
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

  public System.Drawing.Color RGBColor
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return ColorExtension.Empty;
      System.Drawing.Color rgbColor = this[0].RGBColor;
      for (int index = 1; index < count; ++index)
      {
        if (rgbColor != this[index].RGBColor)
          return ColorExtension.Empty;
      }
      return rgbColor;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].RGBColor = value;
    }
  }

  public bool Italic
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool italic = this[0].Italic;
      for (int index = 1; index < count; ++index)
      {
        if (italic != this[index].Italic)
          return false;
      }
      return italic;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Italic = value;
    }
  }

  public bool MacOSOutlineFont
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool macOsOutlineFont = this[0].MacOSOutlineFont;
      for (int index = 1; index < count; ++index)
      {
        if (macOsOutlineFont != this[index].MacOSOutlineFont)
          return false;
      }
      return macOsOutlineFont;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].MacOSOutlineFont = value;
    }
  }

  public bool MacOSShadow
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool macOsShadow = this[0].MacOSShadow;
      for (int index = 1; index < count; ++index)
      {
        if (macOsShadow != this[index].MacOSShadow)
          return false;
      }
      return macOsShadow;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].MacOSShadow = value;
    }
  }

  public double Size
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (double) int.MinValue;
      double size = this[0].Size;
      for (int index = 1; index < count; ++index)
      {
        if (size != this[index].Size)
          return double.MinValue;
      }
      return size;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Size = value;
    }
  }

  public bool Strikethrough
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool strikethrough = this[0].Strikethrough;
      for (int index = 1; index < count; ++index)
      {
        if (strikethrough != this[index].Strikethrough)
          return false;
      }
      return strikethrough;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Strikethrough = value;
    }
  }

  public bool Subscript
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool subscript = this[0].Subscript;
      for (int index = 1; index < count; ++index)
      {
        if (subscript != this[index].Subscript)
          return false;
      }
      return subscript;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Subscript = value;
    }
  }

  public bool Superscript
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return false;
      bool superscript = this[0].Superscript;
      for (int index = 1; index < count; ++index)
      {
        if (superscript != this[index].Superscript)
          return false;
      }
      return superscript;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Superscript = value;
    }
  }

  public OfficeUnderline Underline
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return OfficeUnderline.None;
      OfficeUnderline underline = this[0].Underline;
      for (int index = 1; index < count; ++index)
      {
        if (underline != this[index].Underline)
          return OfficeUnderline.None;
      }
      return underline;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Underline = value;
    }
  }

  public string FontName
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (string) null;
      string fontName = this[0].FontName;
      for (int index = 1; index < count; ++index)
      {
        if (fontName != this[index].FontName)
          return (string) null;
      }
      return fontName;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].FontName = value;
    }
  }

  public OfficeFontVerticalAlignment VerticalAlignment
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return OfficeFontVerticalAlignment.Baseline;
      OfficeFontVerticalAlignment verticalAlignment = this[0].VerticalAlignment;
      for (int index = 1; index < count; ++index)
      {
        if (verticalAlignment != this[index].VerticalAlignment)
          return OfficeFontVerticalAlignment.Baseline;
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

  public bool IsAutoColor => false;

  public Font GenerateNativeFont() => throw new NotSupportedException();

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }
}
