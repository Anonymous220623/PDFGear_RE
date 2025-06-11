// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.PageLayout
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class PageLayout : DefaultPageLayout, INamedObject
{
  private string m_name;
  private PageUsage m_pageUsage;
  private int m_columnsCount;
  private float m_columnsGap;

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal PageUsage PageUsage
  {
    get => this.m_pageUsage;
    set => this.m_pageUsage = value;
  }

  internal int ColumnsCount
  {
    get => this.m_columnsCount;
    set => this.m_columnsCount = value;
  }

  internal float ColumnsGap
  {
    get => this.m_columnsGap;
    set => this.m_columnsGap = value;
  }
}
