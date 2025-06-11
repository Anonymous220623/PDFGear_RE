// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ColorFilter : IFilter
{
  private ExcelColorFilterType m_colorFilterType;
  private Color m_color;

  public ExcelFilterType FilterType => ExcelFilterType.ColorFilter;

  public ExcelColorFilterType ColorFilterType
  {
    get => this.m_colorFilterType;
    internal set => this.m_colorFilterType = value;
  }

  public Color Color
  {
    get => this.m_color;
    internal set => this.m_color = value;
  }

  public ColorFilter() => this.m_colorFilterType = (ExcelColorFilterType) 0;

  internal ColorFilter Clone() => (ColorFilter) this.MemberwiseClone();
}
