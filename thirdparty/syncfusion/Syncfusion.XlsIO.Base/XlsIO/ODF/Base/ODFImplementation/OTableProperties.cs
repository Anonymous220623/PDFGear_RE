// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.OTableProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class OTableProperties : CommonTableParaProperties
{
  private bool m_hasColor;
  private bool m_mayBreakBetweenRows;
  private bool m_display;
  private float m_tableWidth;
  private HoriAlignment m_horizontalAlignment;

  internal float TableWidth
  {
    get => this.m_tableWidth;
    set => this.m_tableWidth = value;
  }

  internal bool HasColor
  {
    get => this.m_hasColor;
    set => this.m_hasColor = value;
  }

  internal bool Display
  {
    get => this.m_display;
    set => this.m_display = value;
  }

  internal HoriAlignment HoriAlignment
  {
    get => this.m_horizontalAlignment;
    set => this.m_horizontalAlignment = value;
  }
}
