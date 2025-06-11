// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Page
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class Page : IPage
{
  private bool m_bIsEvenPage;
  private bool m_bIsFirstPage;
  private WorksheetBaseImpl m_sheet;
  private PageSetupBaseImpl m_pageSetupBase;
  private int m_headerStringLimit = (int) byte.MaxValue;
  private int m_footerStringLimit = (int) byte.MaxValue;
  [CLSCompliant(false)]
  private string[] m_arrHeaders = new string[3]
  {
    string.Empty,
    string.Empty,
    string.Empty
  };
  [CLSCompliant(false)]
  private string[] m_arrFooters = new string[3]
  {
    string.Empty,
    string.Empty,
    string.Empty
  };
  private readonly string[] DEF_HEADER_NAMES = new string[9]
  {
    "LH",
    "CH",
    "RH",
    "LHEVEN",
    "CHEVEN",
    "RHEVEN",
    "LHFIRST",
    "CHFIRST",
    "RHFIRST"
  };
  private readonly string[] DEF_FOOTER_NAMES = new string[9]
  {
    "LF",
    "CF",
    "RF",
    "LFEVEN",
    "CFEVEN",
    "RFEVEN",
    "LFFIRST",
    "CFFIRST",
    "RFFIRST"
  };
  private Regex m_regex = new Regex("[&][\\\"][\\w\\W]*?[\\\"]");

  internal Page(PageSetupBaseImpl pageSetupBase)
  {
    this.m_pageSetupBase = pageSetupBase;
    this.m_sheet = (WorksheetBaseImpl) pageSetupBase.Parent;
  }

  private void SetChanged() => this.m_sheet.SetChanged();

  internal bool IsEvenPage
  {
    get => this.m_bIsEvenPage;
    set => this.m_bIsEvenPage = value;
  }

  internal bool IsFirstPage
  {
    get => this.m_bIsFirstPage;
    set => this.m_bIsFirstPage = value;
  }

  public string LeftHeader
  {
    get => this.m_arrHeaders[0];
    set
    {
      if (!(this.m_arrHeaders[0] != value))
        return;
      if (value != "" && this.LeftHeader.Length == 0)
        this.m_headerStringLimit -= 2;
      else if (this.LeftHeader.Length != 0 && value == "")
        this.m_headerStringLimit += 2;
      if (value.Length + this.CenterHeader.Length + this.RightHeader.Length > this.m_headerStringLimit)
      {
        this.CenterHeader = this.m_regex.Replace(this.CenterHeader, string.Empty);
        this.RightHeader = this.m_regex.Replace(this.RightHeader, string.Empty);
        if (value.Length + this.CenterHeader.Length + this.RightHeader.Length > this.m_headerStringLimit)
          throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      }
      this.m_arrHeaders[0] = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string LeftFooter
  {
    get => this.m_arrFooters[0];
    set
    {
      if (!(this.m_arrFooters[0] != value))
        return;
      if (value != "" && this.LeftFooter.Length == 0)
        this.m_footerStringLimit -= 2;
      else if (this.LeftFooter.Length != 0 && value == "")
        this.m_footerStringLimit += 2;
      if (value.Length + this.CenterFooter.Length + this.RightFooter.Length > this.m_footerStringLimit)
      {
        this.CenterFooter = this.m_regex.Replace(this.CenterFooter, string.Empty);
        this.RightFooter = this.m_regex.Replace(this.RightFooter, string.Empty);
        if (value.Length + this.CenterFooter.Length + this.RightFooter.Length > this.m_footerStringLimit)
          throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      }
      this.m_arrFooters[0] = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string CenterHeader
  {
    get => this.m_arrHeaders[1];
    set
    {
      if (!(this.m_arrHeaders[1] != value))
        return;
      if (value != "" && this.CenterHeader.Length == 0)
        this.m_headerStringLimit -= 2;
      else if (this.CenterHeader.Length != 0 && value == "")
        this.m_headerStringLimit += 2;
      if (this.LeftHeader.Length + value.Length + this.RightHeader.Length > this.m_headerStringLimit)
      {
        this.LeftHeader = this.m_regex.Replace(this.LeftHeader, string.Empty);
        this.RightHeader = this.m_regex.Replace(this.RightHeader, string.Empty);
        if (this.LeftHeader.Length + value.Length + this.RightHeader.Length > this.m_headerStringLimit)
          throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      }
      this.m_arrHeaders[1] = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string CenterFooter
  {
    get => this.m_arrFooters[1];
    set
    {
      if (!(this.m_arrFooters[1] != value))
        return;
      if (value != "" && this.CenterFooter.Length == 0)
        this.m_footerStringLimit -= 2;
      else if (this.CenterFooter.Length != 0 && value == "")
        this.m_footerStringLimit += 2;
      if (this.LeftFooter.Length + value.Length + this.RightFooter.Length > this.m_footerStringLimit)
      {
        this.LeftFooter = this.m_regex.Replace(this.LeftFooter, string.Empty);
        this.RightFooter = this.m_regex.Replace(this.RightFooter, string.Empty);
        if (this.LeftFooter.Length + value.Length + this.RightFooter.Length > this.m_footerStringLimit)
          throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      }
      this.m_arrFooters[1] = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string RightHeader
  {
    get => this.m_arrHeaders[2];
    set
    {
      if (!(this.m_arrHeaders[2] != value))
        return;
      if (value != "" && this.RightHeader.Length == 0)
        this.m_headerStringLimit -= 2;
      else if (this.RightHeader.Length != 0 && value == "")
        this.m_headerStringLimit += 2;
      if (this.LeftHeader.Length + this.CenterHeader.Length + value.Length > this.m_headerStringLimit)
      {
        this.LeftHeader = this.m_regex.Replace(this.LeftHeader, string.Empty);
        this.CenterHeader = this.m_regex.Replace(this.CenterHeader, string.Empty);
        if (this.LeftHeader.Length + this.CenterHeader.Length + value.Length > this.m_headerStringLimit)
          throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      }
      this.m_arrHeaders[2] = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string RightFooter
  {
    get => this.m_arrFooters[2];
    set
    {
      if (!(this.m_arrFooters[2] != value))
        return;
      if (value != "" && this.RightFooter.Length == 0)
        this.m_footerStringLimit -= 2;
      else if (this.RightFooter.Length != 0 && value == "")
        this.m_footerStringLimit += 2;
      if (this.LeftFooter.Length + this.CenterFooter.Length + value.Length > this.m_footerStringLimit)
      {
        this.LeftFooter = this.m_regex.Replace(this.LeftFooter, string.Empty);
        this.CenterFooter = this.m_regex.Replace(this.CenterFooter, string.Empty);
        if (this.LeftFooter.Length + this.CenterFooter.Length + value.Length > this.m_footerStringLimit)
          throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      }
      this.m_arrFooters[2] = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public Image LeftHeaderImage
  {
    get
    {
      return (!this.IsEvenPage ? (!this.IsFirstPage ? (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[0]] : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[6]]) : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[3]])?.Picture;
    }
    set
    {
      if (this.IsEvenPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[3], value);
      else if (this.IsFirstPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[6], value);
      else
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[0], value);
    }
  }

  public Image LeftFooterImage
  {
    get
    {
      return (!this.IsEvenPage ? (!this.IsFirstPage ? (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[0]] : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[6]]) : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[3]])?.Picture;
    }
    set
    {
      if (this.IsEvenPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[3], value);
      else if (this.IsFirstPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[6], value);
      else
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[0], value);
    }
  }

  public Image CenterHeaderImage
  {
    get
    {
      return (!this.IsEvenPage ? (!this.IsFirstPage ? (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[1]] : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[7]]) : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[4]])?.Picture;
    }
    set
    {
      if (this.IsEvenPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[4], value);
      else if (this.IsFirstPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[7], value);
      else
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[1], value);
    }
  }

  public Image CenterFooterImage
  {
    get
    {
      return (!this.IsEvenPage ? (!this.IsFirstPage ? (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[1]] : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[7]]) : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[4]])?.Picture;
    }
    set
    {
      if (this.IsEvenPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[4], value);
      else if (this.IsFirstPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[7], value);
      else
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[1], value);
    }
  }

  public Image RightHeaderImage
  {
    get
    {
      return (!this.IsEvenPage ? (!this.IsFirstPage ? (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[2]] : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[8]]) : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_HEADER_NAMES[5]])?.Picture;
    }
    set
    {
      if (this.IsEvenPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[5], value);
      else if (this.IsFirstPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[8], value);
      else
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_HEADER_NAMES[2], value);
    }
  }

  public Image RightFooterImage
  {
    get
    {
      return (!this.IsEvenPage ? (!this.IsFirstPage ? (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[2]] : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[8]]) : (BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[this.DEF_FOOTER_NAMES[5]])?.Picture;
    }
    set
    {
      if (this.IsEvenPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[5], value);
      else if (this.IsFirstPage)
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[8], value);
      else
        this.m_sheet.HeaderFooterShapes.SetPicture(this.DEF_FOOTER_NAMES[2], value);
    }
  }

  internal string FullHeaderString
  {
    get => this.m_pageSetupBase.CreateHeaderFooterString(this.m_arrHeaders);
    set => this.m_arrHeaders = this.m_pageSetupBase.ParseHeaderFooterString(value);
  }

  internal string FullFooterString
  {
    get => this.m_pageSetupBase.CreateHeaderFooterString(this.m_arrFooters);
    set => this.m_arrFooters = this.m_pageSetupBase.ParseHeaderFooterString(value);
  }

  internal Page Clone(PageSetupBaseImpl pageSetupBase)
  {
    Page page = (Page) this.MemberwiseClone();
    page.m_pageSetupBase = pageSetupBase;
    page.m_sheet = pageSetupBase.Parent as WorksheetBaseImpl;
    if (this.m_arrHeaders != null)
      page.m_arrHeaders = CloneUtils.CloneStringArray(this.m_arrHeaders);
    if (this.m_arrFooters != null)
      page.m_arrFooters = CloneUtils.CloneStringArray(this.m_arrFooters);
    return page;
  }

  internal void Dispose()
  {
    if (this.m_sheet != null)
      this.m_sheet = (WorksheetBaseImpl) null;
    if (this.m_pageSetupBase != null)
      this.m_pageSetupBase = (PageSetupBaseImpl) null;
    if (this.m_arrHeaders != null)
      this.m_arrHeaders = (string[]) null;
    if (this.m_arrFooters == null)
      return;
    this.m_arrFooters = (string[]) null;
  }
}
