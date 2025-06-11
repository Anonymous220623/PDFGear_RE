// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.PageLayoutProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class PageLayoutProperties : MarginBorderProperties
{
  private double m_pageWidth;
  private PrintOrientation m_pageOrientation;
  private double m_pageHeight;
  private PageOrder m_printPageOrder;
  private string m_scaleTo;
  private TableCentering m_tableCentering;
  private string m_printableObjects;
  private string m_firstPageNumber;
  private double m_paddingLeft;
  private double m_paddingRight;
  private double m_paddingTop;
  private double m_paddingBottom;

  internal double PaddingBottom
  {
    get => this.m_paddingBottom;
    set => this.m_paddingBottom = value;
  }

  internal double PaddingTop
  {
    get => this.m_paddingTop;
    set => this.m_paddingTop = value;
  }

  internal double PaddingRight
  {
    get => this.m_paddingRight;
    set => this.m_paddingRight = value;
  }

  internal double PaddingLeft
  {
    get => this.m_paddingLeft;
    set => this.m_paddingLeft = value;
  }

  internal double PageWidth
  {
    get => this.m_pageWidth;
    set => this.m_pageWidth = value;
  }

  internal double PageHeight
  {
    get => this.m_pageHeight;
    set => this.m_pageHeight = value;
  }

  internal PrintOrientation PageOrientation
  {
    get => this.m_pageOrientation;
    set => this.m_pageOrientation = value;
  }

  internal PageOrder PrintPageOrder
  {
    get => this.m_printPageOrder;
    set => this.m_printPageOrder = value;
  }

  internal string ScaleTo
  {
    get => this.m_scaleTo;
    set => this.m_scaleTo = value;
  }

  internal TableCentering TableCentering
  {
    get => this.m_tableCentering;
    set => this.m_tableCentering = value;
  }

  internal string PrintableObjects
  {
    get => this.m_printableObjects;
    set => this.m_printableObjects = value;
  }

  internal string FirstPageNumber
  {
    get => this.m_firstPageNumber;
    set => this.m_firstPageNumber = value;
  }
}
