// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.PageLayout
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
