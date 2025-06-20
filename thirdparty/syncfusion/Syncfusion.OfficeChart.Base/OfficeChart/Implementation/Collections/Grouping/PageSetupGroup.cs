﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.PageSetupGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class PageSetupGroup : CommonObject, IPageSetup, IPageSetupBase, IParentApplication
{
  private WorksheetGroup m_sheetGroup;

  public PageSetupGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private void FindParents()
  {
    this.m_sheetGroup = this.FindParent(typeof (WorksheetGroup)) as WorksheetGroup;
    if (this.m_sheetGroup == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent group.");
  }

  public bool AutoFirstPageNumber
  {
    get
    {
      bool autoFirstPageNumber = this.m_sheetGroup[0].PageSetup.AutoFirstPageNumber;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.AutoFirstPageNumber != autoFirstPageNumber)
          return false;
      }
      return autoFirstPageNumber;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.AutoFirstPageNumber = value;
    }
  }

  public int FitToPagesTall
  {
    get
    {
      int fitToPagesTall = this.m_sheetGroup[0].PageSetup.FitToPagesTall;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.FitToPagesTall != fitToPagesTall)
          return int.MinValue;
      }
      return fitToPagesTall;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.FitToPagesTall = value;
    }
  }

  public int FitToPagesWide
  {
    get
    {
      int fitToPagesWide = this.m_sheetGroup[0].PageSetup.FitToPagesWide;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.FitToPagesWide != fitToPagesWide)
          return int.MinValue;
      }
      return fitToPagesWide;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.FitToPagesWide = value;
    }
  }

  public bool PrintGridlines
  {
    get
    {
      bool printGridlines = this.m_sheetGroup[0].PageSetup.PrintGridlines;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintGridlines != printGridlines)
          return false;
      }
      return printGridlines;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintGridlines = value;
    }
  }

  public bool PrintHeadings
  {
    get
    {
      bool printHeadings = this.m_sheetGroup[0].PageSetup.PrintHeadings;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintHeadings != printHeadings)
          return false;
      }
      return printHeadings;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintHeadings = value;
    }
  }

  public string PrintArea
  {
    get
    {
      string printArea = this.m_sheetGroup[0].PageSetup.PrintArea;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintArea != printArea)
          return (string) null;
      }
      return printArea;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintArea = value;
    }
  }

  public string PrintTitleColumns
  {
    get
    {
      string printTitleColumns = this.m_sheetGroup[0].PageSetup.PrintTitleColumns;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintTitleColumns != printTitleColumns)
          return (string) null;
      }
      return printTitleColumns;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintTitleColumns = value;
    }
  }

  public string PrintTitleRows
  {
    get
    {
      string printTitleRows = this.m_sheetGroup[0].PageSetup.PrintTitleRows;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintTitleRows != printTitleRows)
          return (string) null;
      }
      return printTitleRows;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintTitleRows = value;
    }
  }

  public bool IsSummaryRowBelow
  {
    get
    {
      bool isSummaryRowBelow = this.m_sheetGroup[0].PageSetup.IsSummaryRowBelow;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.IsSummaryRowBelow != isSummaryRowBelow)
          return false;
      }
      return isSummaryRowBelow;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.IsSummaryRowBelow = value;
    }
  }

  public bool IsSummaryColumnRight
  {
    get
    {
      bool summaryColumnRight = this.m_sheetGroup[0].PageSetup.IsSummaryColumnRight;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.IsSummaryColumnRight != summaryColumnRight)
          return false;
      }
      return summaryColumnRight;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.IsSummaryColumnRight = value;
    }
  }

  public bool IsFitToPage
  {
    get
    {
      bool isFitToPage1 = this.m_sheetGroup[0].PageSetup.IsFitToPage;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        bool isFitToPage2 = this.m_sheetGroup[i].PageSetup.IsFitToPage;
        if (isFitToPage2 != isFitToPage1 || !isFitToPage2)
          return false;
      }
      return isFitToPage1;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.IsFitToPage = value;
    }
  }

  public bool BlackAndWhite
  {
    get
    {
      bool blackAndWhite = this.m_sheetGroup[0].PageSetup.BlackAndWhite;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.BlackAndWhite != blackAndWhite)
          return false;
      }
      return blackAndWhite;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.BlackAndWhite = value;
    }
  }

  public double BottomMargin
  {
    get
    {
      double bottomMargin = this.m_sheetGroup[0].PageSetup.BottomMargin;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.BottomMargin != bottomMargin)
          return double.MinValue;
      }
      return bottomMargin;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.BottomMargin = value;
    }
  }

  public string CenterFooter
  {
    get
    {
      string centerFooter = this.m_sheetGroup[0].PageSetup.CenterFooter;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.CenterFooter != centerFooter)
          return (string) null;
      }
      return centerFooter;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.CenterFooter = value;
    }
  }

  public Image CenterFooterImage
  {
    get => (Image) null;
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.CenterFooterImage = value;
    }
  }

  public string CenterHeader
  {
    get
    {
      string centerHeader = this.m_sheetGroup[0].PageSetup.CenterHeader;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.CenterHeader != centerHeader)
          return (string) null;
      }
      return centerHeader;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.CenterHeader = value;
    }
  }

  public Image CenterHeaderImage
  {
    get => (Image) null;
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.CenterHeaderImage = value;
    }
  }

  public bool CenterHorizontally
  {
    get
    {
      bool centerHorizontally = this.m_sheetGroup[0].PageSetup.CenterHorizontally;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.CenterHorizontally != centerHorizontally)
          return false;
      }
      return centerHorizontally;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.CenterHorizontally = value;
    }
  }

  public bool CenterVertically
  {
    get
    {
      bool centerVertically = this.m_sheetGroup[0].PageSetup.CenterVertically;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.CenterVertically != centerVertically)
          return false;
      }
      return centerVertically;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.CenterVertically = value;
    }
  }

  public int Copies
  {
    get
    {
      int copies = this.m_sheetGroup[0].PageSetup.Copies;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.Copies != copies)
          return int.MinValue;
      }
      return copies;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.Copies = value;
    }
  }

  public bool Draft
  {
    get
    {
      bool draft = this.m_sheetGroup[0].PageSetup.Draft;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.Draft != draft)
          return false;
      }
      return draft;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.Draft = value;
    }
  }

  public short FirstPageNumber
  {
    get
    {
      short firstPageNumber = this.m_sheetGroup[0].PageSetup.FirstPageNumber;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if ((int) this.m_sheetGroup[i].PageSetup.FirstPageNumber != (int) firstPageNumber)
          return short.MinValue;
      }
      return firstPageNumber;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.FirstPageNumber = value;
    }
  }

  public double FooterMargin
  {
    get
    {
      double footerMargin = this.m_sheetGroup[0].PageSetup.FooterMargin;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.FooterMargin != footerMargin)
          return double.MinValue;
      }
      return footerMargin;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.FooterMargin = value;
    }
  }

  public double HeaderMargin
  {
    get
    {
      double headerMargin = this.m_sheetGroup[0].PageSetup.HeaderMargin;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.HeaderMargin != headerMargin)
          return double.MinValue;
      }
      return headerMargin;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.HeaderMargin = value;
    }
  }

  public string LeftFooter
  {
    get
    {
      string leftFooter = this.m_sheetGroup[0].PageSetup.LeftFooter;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.LeftFooter != leftFooter)
          return (string) null;
      }
      return leftFooter;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.LeftFooter = value;
    }
  }

  public Image LeftFooterImage
  {
    get => (Image) null;
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.LeftFooterImage = value;
    }
  }

  public string LeftHeader
  {
    get
    {
      string leftHeader = this.m_sheetGroup[0].PageSetup.LeftHeader;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.LeftHeader != leftHeader)
          return (string) null;
      }
      return leftHeader;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.LeftHeader = value;
    }
  }

  public Image LeftHeaderImage
  {
    get => (Image) null;
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.LeftHeaderImage = value;
    }
  }

  public double LeftMargin
  {
    get
    {
      double leftMargin = this.m_sheetGroup[0].PageSetup.LeftMargin;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.LeftMargin != leftMargin)
          return double.MinValue;
      }
      return leftMargin;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.LeftMargin = value;
    }
  }

  public OfficeOrder Order
  {
    get
    {
      OfficeOrder order = this.m_sheetGroup[0].PageSetup.Order;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.Order != order)
          return OfficeOrder.DownThenOver;
      }
      return order;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.Order = value;
    }
  }

  public OfficePageOrientation Orientation
  {
    get
    {
      OfficePageOrientation orientation = this.m_sheetGroup[0].PageSetup.Orientation;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.Orientation != orientation)
          return OfficePageOrientation.Portrait;
      }
      return orientation;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.Orientation = value;
    }
  }

  public OfficePaperSize PaperSize
  {
    get
    {
      OfficePaperSize paperSize = this.m_sheetGroup[0].PageSetup.PaperSize;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PaperSize != paperSize)
          return OfficePaperSize.PaperA4;
      }
      return paperSize;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PaperSize = value;
    }
  }

  public OfficePrintLocation PrintComments
  {
    get
    {
      OfficePrintLocation printComments = this.m_sheetGroup[0].PageSetup.PrintComments;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintComments != printComments)
          return OfficePrintLocation.PrintInPlace;
      }
      return printComments;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintComments = value;
    }
  }

  public OfficePrintErrors PrintErrors
  {
    get
    {
      OfficePrintErrors printErrors = this.m_sheetGroup[0].PageSetup.PrintErrors;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintErrors != printErrors)
          return OfficePrintErrors.PrintErrorsDisplayed;
      }
      return printErrors;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintErrors = value;
    }
  }

  public bool PrintNotes
  {
    get
    {
      bool printNotes = this.m_sheetGroup[0].PageSetup.PrintNotes;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintNotes != printNotes)
          return false;
      }
      return printNotes;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintNotes = value;
    }
  }

  public int PrintQuality
  {
    get
    {
      int printQuality = this.m_sheetGroup[0].PageSetup.PrintQuality;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.PrintQuality != printQuality)
          return int.MinValue;
      }
      return printQuality;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.PrintQuality = value;
    }
  }

  public string RightFooter
  {
    get
    {
      string rightFooter = this.m_sheetGroup[0].PageSetup.RightFooter;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.RightFooter != rightFooter)
          return (string) null;
      }
      return rightFooter;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.RightFooter = value;
    }
  }

  public string RightHeader
  {
    get
    {
      string rightHeader = this.m_sheetGroup[0].PageSetup.RightHeader;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.RightHeader != rightHeader)
          return (string) null;
      }
      return rightHeader;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.RightHeader = value;
    }
  }

  public Image RightFooterImage
  {
    get => (Image) null;
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.RightFooterImage = value;
    }
  }

  public Image RightHeaderImage
  {
    get => (Image) null;
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.RightHeaderImage = value;
    }
  }

  public double RightMargin
  {
    get
    {
      double rightMargin = this.m_sheetGroup[0].PageSetup.RightMargin;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.RightMargin != rightMargin)
          return double.MinValue;
      }
      return rightMargin;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.RightMargin = value;
    }
  }

  public double TopMargin
  {
    get
    {
      double topMargin = this.m_sheetGroup[0].PageSetup.TopMargin;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.TopMargin != topMargin)
          return double.MinValue;
      }
      return topMargin;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.TopMargin = value;
    }
  }

  public int Zoom
  {
    get
    {
      int zoom = this.m_sheetGroup[0].PageSetup.Zoom;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.Zoom != zoom)
          return int.MinValue;
      }
      return zoom;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.Zoom = value;
    }
  }

  public bool AlignHFWithPageMargins
  {
    get
    {
      bool hfWithPageMargins = this.m_sheetGroup[0].PageSetup.AlignHFWithPageMargins;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.AlignHFWithPageMargins != hfWithPageMargins)
          return false;
      }
      return hfWithPageMargins;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.AlignHFWithPageMargins = value;
    }
  }

  public bool DifferentFirstPageHF
  {
    get
    {
      bool differentFirstPageHf = this.m_sheetGroup[0].PageSetup.DifferentFirstPageHF;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.DifferentFirstPageHF != differentFirstPageHf)
          return false;
      }
      return differentFirstPageHf;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.DifferentFirstPageHF = value;
    }
  }

  public bool DifferentOddAndEvenPagesHF
  {
    get
    {
      bool oddAndEvenPagesHf = this.m_sheetGroup[0].PageSetup.DifferentOddAndEvenPagesHF;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.DifferentOddAndEvenPagesHF != oddAndEvenPagesHf)
          return false;
      }
      return oddAndEvenPagesHf;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.DifferentOddAndEvenPagesHF = value;
    }
  }

  public bool HFScaleWithDoc
  {
    get
    {
      bool hfScaleWithDoc = this.m_sheetGroup[0].PageSetup.HFScaleWithDoc;
      int i = 1;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
      {
        if (this.m_sheetGroup[i].PageSetup.HFScaleWithDoc != hfScaleWithDoc)
          return false;
      }
      return hfScaleWithDoc;
    }
    set
    {
      int i = 0;
      for (int count = this.m_sheetGroup.Count; i < count; ++i)
        this.m_sheetGroup[i].PageSetup.HFScaleWithDoc = value;
    }
  }

  public Bitmap BackgoundImage
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }
}
