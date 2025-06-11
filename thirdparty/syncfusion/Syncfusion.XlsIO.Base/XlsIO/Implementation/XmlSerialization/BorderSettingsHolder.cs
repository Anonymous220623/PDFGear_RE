// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.BorderSettingsHolder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class BorderSettingsHolder : IBorder, IParentApplication, ICloneable
{
  private ColorObject m_color = new ColorObject(ExcelKnownColors.Black);
  private ExcelLineStyle m_lineStyle;
  private bool m_bShowDiagonalLine;
  private bool m_bIsEmptyBorder = true;

  public ExcelKnownColors Color
  {
    get
    {
      return this.m_color.ColorType != ColorType.Indexed ? ExcelKnownColors.None : (ExcelKnownColors) this.m_color.Value;
    }
    set => this.m_color.SetIndexed(value);
  }

  public ColorObject ColorObject => this.m_color;

  public System.Drawing.Color ColorRGB
  {
    get
    {
      return this.m_color.ColorType != ColorType.RGB ? ColorExtension.Empty : ColorExtension.FromArgb(this.m_color.Value);
    }
    set => this.m_color.SetRGB(value);
  }

  public ExcelLineStyle LineStyle
  {
    get => this.m_lineStyle;
    set => this.m_lineStyle = value;
  }

  public bool ShowDiagonalLine
  {
    get => this.m_bShowDiagonalLine;
    set => this.m_bShowDiagonalLine = value;
  }

  internal bool IsEmptyBorder
  {
    get => this.m_bIsEmptyBorder;
    set => this.m_bIsEmptyBorder = value;
  }

  public IApplication Application => throw new NotImplementedException();

  public object Parent => throw new NotImplementedException();

  public object Clone() => this.MemberwiseClone();
}
