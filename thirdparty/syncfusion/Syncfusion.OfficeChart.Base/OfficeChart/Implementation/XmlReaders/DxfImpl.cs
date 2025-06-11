// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.DxfImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders;

internal class DxfImpl
{
  private BordersCollection m_borders;
  private FillImpl m_fill;
  private FontImpl m_font;
  private FormatImpl m_format;

  public FormatImpl FormatRecord
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public FillImpl Fill
  {
    get => this.m_fill;
    set => this.m_fill = value;
  }

  public FontImpl Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public BordersCollection Borders
  {
    get => this.m_borders;
    set => this.m_borders = value;
  }

  public DxfImpl Clone(WorkbookImpl book)
  {
    DxfImpl dxfImpl = (DxfImpl) this.MemberwiseClone();
    dxfImpl.m_borders = (BordersCollection) this.m_borders.Clone((object) book);
    dxfImpl.m_fill = this.m_fill.Clone();
    dxfImpl.m_font = this.m_font.Clone((object) book);
    dxfImpl.m_format = (FormatImpl) this.m_format.Clone((object) book);
    return dxfImpl;
  }
}
