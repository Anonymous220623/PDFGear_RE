// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.HeaderFooter
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class HeaderFooter
{
  private string headerFooterName;
  private List<HeaderFooterSection> headerFooterSections;
  private RectangleF templateSize;

  public HeaderFooter() => this.headerFooterSections = new List<HeaderFooterSection>();

  internal RectangleF TemplateSize
  {
    get => this.templateSize;
    set => this.templateSize = value;
  }

  internal string HeaderFooterName
  {
    get => this.headerFooterName;
    set => this.headerFooterName = value;
  }

  internal List<HeaderFooterSection> HeaderFooterSections
  {
    get => this.headerFooterSections;
    set => this.headerFooterSections = value;
  }

  internal HeaderFooter Clone()
  {
    HeaderFooter headerFooter = (HeaderFooter) this.MemberwiseClone();
    List<HeaderFooterSection> headerFooterSectionList = new List<HeaderFooterSection>();
    for (int index = 0; index < this.HeaderFooterSections.Count; ++index)
    {
      HeaderFooterSection headerFooterSection = this.HeaderFooterSections[0].Clone();
      headerFooterSectionList.Add(headerFooterSection);
    }
    headerFooter.HeaderFooterSections = headerFooterSectionList;
    return headerFooter;
  }
}
