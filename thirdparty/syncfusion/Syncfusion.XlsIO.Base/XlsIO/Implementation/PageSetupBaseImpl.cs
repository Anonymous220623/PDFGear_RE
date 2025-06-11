// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PageSetupBaseImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class PageSetupBaseImpl : CommonObject, IPageSetupBase, IParentApplication, IBiffStorage
{
  public const double DEFAULT_TOPMARGIN = 1.0;
  public const double DEFAULT_BOTTOMMARGIN = 1.0;
  public const double DEFAULT_LEFTMARGIN = 0.75;
  public const double DEFAULT_RIGHTMARGIN = 0.75;
  private const double DEFAULT_TOPMARGIN_XML = 0.75;
  private const double DEFAULT_BOTTOMMARGIN_XML = 0.75;
  private const double DEFAULT_LEFTMARGIN_XML = 0.7;
  private const double DEFAULT_RIGHTMARGIN_XML = 0.7;
  private const double DEFAULT_HEADERMARGIN_XML = 0.3;
  private const double DEFAULT_FOOTERMARGIN_XML = 0.3;
  protected bool m_bHCenter;
  protected bool m_bVCenter;
  [CLSCompliant(false)]
  protected PrinterSettingsRecord m_unknown;
  [CLSCompliant(false)]
  protected PrintSetupRecord m_setup;
  [CLSCompliant(false)]
  protected double m_dBottomMargin = 1.0;
  [CLSCompliant(false)]
  protected double m_dLeftMargin = 0.75;
  [CLSCompliant(false)]
  protected double m_dRightMargin = 0.75;
  [CLSCompliant(false)]
  protected double m_dTopMargin = 1.0;
  [CLSCompliant(false)]
  protected string[] m_arrHeaders = new string[3]
  {
    string.Empty,
    string.Empty,
    string.Empty
  };
  [CLSCompliant(false)]
  protected string[] m_arrFooters = new string[3]
  {
    string.Empty,
    string.Empty,
    string.Empty
  };
  private WorksheetBaseImpl m_sheet;
  [CLSCompliant(false)]
  protected BitmapRecord m_backgroundImage;
  private bool m_bFitToPage;
  internal Dictionary<ExcelPaperSize, double> dictPaperWidth = new Dictionary<ExcelPaperSize, double>();
  internal Dictionary<ExcelPaperSize, double> dictPaperHeight = new Dictionary<ExcelPaperSize, double>();
  [CLSCompliant(false)]
  protected HeaderAndFooterRecord m_headerFooter;
  private Page m_evenPage;
  private Page m_oddPage;
  private Page m_firstPage;

  public virtual bool IsFitToPage
  {
    get => this.m_bFitToPage;
    set => this.m_bFitToPage = value;
  }

  public int FitToPagesTall
  {
    get => (int) this.m_setup.FitHeight;
    set
    {
      ushort num = (ushort) value;
      if ((int) this.m_setup.FitHeight != (int) num)
      {
        this.m_setup.FitHeight = num;
        bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
        this.SetChanged();
        if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified != isCellModified)
          (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
      }
      if (this.m_sheet.ParentWorkbook.Loading)
        return;
      this.IsFitToPage = value > 0 || this.FitToPagesWide > 0;
    }
  }

  public int FitToPagesWide
  {
    get => (int) this.m_setup.FitWidth;
    set
    {
      ushort num = (ushort) value;
      if ((int) this.m_setup.FitWidth != (int) num)
      {
        this.m_setup.FitWidth = num;
        bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
        this.SetChanged();
        if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified != isCellModified)
          (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
      }
      if (this.m_sheet.ParentWorkbook.Loading)
        return;
      this.IsFitToPage = value > 0 || this.FitToPagesTall > 0;
    }
  }

  public bool IsNotValidSettings
  {
    get => this.m_setup.IsNotValidSettings;
    internal set => this.m_setup.IsNotValidSettings = value;
  }

  public bool AutoFirstPageNumber
  {
    get => !this.m_setup.IsUsePage;
    set => this.m_setup.IsUsePage = !value;
  }

  public bool BlackAndWhite
  {
    get => this.m_setup.IsNoColor;
    set
    {
      if (this.m_setup.IsNoColor == value)
        return;
      this.m_setup.IsNoColor = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public double BottomMargin
  {
    get => this.m_dBottomMargin;
    set
    {
      if (this.m_dBottomMargin == value)
        return;
      this.m_dBottomMargin = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string CenterFooter
  {
    get => this.m_oddPage.CenterFooter;
    set => this.m_oddPage.CenterFooter = value;
  }

  public Image CenterFooterImage
  {
    get => this.m_oddPage.CenterFooterImage;
    set => this.m_oddPage.CenterFooterImage = value;
  }

  public Image CenterHeaderImage
  {
    get => this.m_oddPage.CenterHeaderImage;
    set => this.m_oddPage.CenterHeaderImage = value;
  }

  public string CenterHeader
  {
    get => this.m_oddPage.CenterHeader;
    set => this.m_oddPage.CenterHeader = value;
  }

  public bool CenterHorizontally
  {
    get => this.m_bHCenter;
    set
    {
      if (this.m_bHCenter == value)
        return;
      this.m_bHCenter = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public bool CenterVertically
  {
    get => this.m_bVCenter;
    set
    {
      if (this.m_bVCenter == value)
        return;
      this.m_bVCenter = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public int Copies
  {
    get => (int) this.m_setup.Copies;
    set
    {
      if (value < 1)
      {
        if (!this.m_sheet.IsParsing && !((WorkbookImpl) this.m_sheet.Workbook).Loading)
          throw new ArgumentOutOfRangeException("Number of copies can not be less then 1");
        value = 1;
      }
      if ((int) this.m_setup.Copies == (int) (ushort) value)
        return;
      this.m_setup.Copies = (ushort) value;
      this.m_setup.IsNotValidSettings = false;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public bool Draft
  {
    get => this.m_setup.IsDraft;
    set
    {
      if (this.m_setup.IsDraft == value)
        return;
      this.m_setup.IsDraft = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public short FirstPageNumber
  {
    get => this.m_setup.PageStart;
    set
    {
      if ((int) this.m_setup.PageStart == (int) value)
        return;
      this.m_setup.PageStart = value;
      this.AutoFirstPageNumber = false;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public double FooterMargin
  {
    get => this.m_setup.FooterMargin;
    set
    {
      if (this.m_setup.FooterMargin == value)
        return;
      this.m_setup.FooterMargin = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public double HeaderMargin
  {
    get => this.m_setup.HeaderMargin;
    set
    {
      if (this.m_setup.HeaderMargin == value)
        return;
      this.m_setup.HeaderMargin = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string LeftFooter
  {
    get => this.m_oddPage.LeftFooter;
    set => this.m_oddPage.LeftFooter = value;
  }

  public string LeftHeader
  {
    get => this.OddPage.LeftHeader;
    set => this.OddPage.LeftHeader = value;
  }

  public Image LeftFooterImage
  {
    get => this.m_oddPage.LeftFooterImage;
    set => this.m_oddPage.LeftFooterImage = value;
  }

  public Image LeftHeaderImage
  {
    get => this.m_oddPage.LeftHeaderImage;
    set => this.m_oddPage.LeftHeaderImage = value;
  }

  public double LeftMargin
  {
    get => this.m_dLeftMargin;
    set
    {
      if (this.m_dLeftMargin == value)
        return;
      this.m_dLeftMargin = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public ExcelOrder Order
  {
    get => !this.m_setup.IsLeftToRight ? ExcelOrder.DownThenOver : ExcelOrder.OverThenDown;
    set
    {
      bool flag = value == ExcelOrder.OverThenDown;
      if (this.m_setup.IsLeftToRight == flag)
        return;
      this.m_setup.IsLeftToRight = flag;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public ExcelPageOrientation Orientation
  {
    get
    {
      return !this.m_setup.IsNotLandscape ? ExcelPageOrientation.Landscape : ExcelPageOrientation.Portrait;
    }
    set
    {
      this.m_setup.IsNotLandscape = value == ExcelPageOrientation.Portrait;
      this.m_setup.IsNotValidSettings = false;
      this.m_setup.IsNoOrientation = false;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public ExcelPaperSize PaperSize
  {
    get => (ExcelPaperSize) this.m_setup.PaperSize;
    set
    {
      this.m_setup.PaperSize = (ushort) value;
      this.m_setup.IsNotValidSettings = false;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public ExcelPrintLocation PrintComments
  {
    get
    {
      if (!this.m_setup.IsNotes)
        return ExcelPrintLocation.PrintNoComments;
      return this.m_setup.IsPrintNotesAsDisplayed ? ExcelPrintLocation.PrintInPlace : ExcelPrintLocation.PrintSheetEnd;
    }
    set
    {
      switch (value)
      {
        case ExcelPrintLocation.PrintInPlace:
          this.m_setup.IsNotes = true;
          this.m_setup.IsPrintNotesAsDisplayed = true;
          break;
        case ExcelPrintLocation.PrintNoComments:
          this.m_setup.IsNotes = false;
          break;
        case ExcelPrintLocation.PrintSheetEnd:
          this.m_setup.IsNotes = true;
          this.m_setup.IsPrintNotesAsDisplayed = false;
          break;
      }
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public ExcelPrintErrors PrintErrors
  {
    get => this.m_setup.PrintErrors;
    set
    {
      if (this.m_setup.PrintErrors == value)
        return;
      this.m_setup.PrintErrors = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public bool PrintNotes
  {
    get => this.m_setup.IsNotes;
    set
    {
      if (this.m_setup.IsNotes == value)
        return;
      this.m_setup.IsNotes = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public int PrintQuality
  {
    get => (int) this.m_setup.HResolution;
    set
    {
      this.m_setup.HResolution = (ushort) value;
      this.m_setup.VResolution = (ushort) value;
      this.m_setup.IsNotValidSettings = false;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public string RightFooter
  {
    get => this.m_oddPage.RightFooter;
    set => this.m_oddPage.RightFooter = value;
  }

  public Image RightFooterImage
  {
    get => this.m_oddPage.RightFooterImage;
    set => this.m_oddPage.RightFooterImage = value;
  }

  public string RightHeader
  {
    get => this.m_oddPage.RightHeader;
    set => this.m_oddPage.RightHeader = value;
  }

  public Image RightHeaderImage
  {
    get => this.m_oddPage.RightHeaderImage;
    set => this.m_oddPage.RightHeaderImage = value;
  }

  public double RightMargin
  {
    get => this.m_dRightMargin;
    set
    {
      if (this.m_dRightMargin == value)
        return;
      this.m_dRightMargin = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public double TopMargin
  {
    get => this.m_dTopMargin;
    set
    {
      if (this.m_dTopMargin == value)
        return;
      this.m_dTopMargin = value;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public int Zoom
  {
    get => (int) this.m_setup.Scale;
    set
    {
      this.m_setup.Scale = value >= 10 && value <= 400 ? (ushort) value : throw new ArgumentOutOfRangeException("Zoom value must be beetween 10 and 400 percent.");
      this.m_setup.IsNotValidSettings = false;
      bool isCellModified = (this.m_sheet.Workbook as WorkbookImpl).IsCellModified;
      this.SetChanged();
      if ((this.m_sheet.Workbook as WorkbookImpl).IsCellModified == isCellModified)
        return;
      (this.m_sheet.Workbook as WorkbookImpl).IsCellModified = isCellModified;
    }
  }

  public Bitmap BackgoundImage
  {
    get => this.m_backgroundImage == null ? (Bitmap) null : this.m_backgroundImage.Picture;
    set
    {
      if (value == null)
      {
        this.m_backgroundImage = (BitmapRecord) null;
      }
      else
      {
        if (this.m_backgroundImage == null)
          this.m_backgroundImage = (BitmapRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Bitmap);
        this.m_backgroundImage.Picture = value;
      }
    }
  }

  public double PageWidth
  {
    get
    {
      PageSetupBaseImpl.PaperSizeEntry paperSizeEntry = this.AppImplementation.DicPaperSizeTable.ContainsKey((int) this.PaperSize) ? this.AppImplementation.DicPaperSizeTable[(int) this.PaperSize] : this.AppImplementation.DicPaperSizeTable[9];
      return this.Orientation != ExcelPageOrientation.Portrait ? paperSizeEntry.Height : paperSizeEntry.Width;
    }
  }

  public double PageHeight
  {
    get
    {
      PageSetupBaseImpl.PaperSizeEntry paperSizeEntry = this.AppImplementation.DicPaperSizeTable.ContainsKey((int) this.PaperSize) ? this.AppImplementation.DicPaperSizeTable[(int) this.PaperSize] : this.AppImplementation.DicPaperSizeTable[9];
      return this.Orientation != ExcelPageOrientation.Portrait ? paperSizeEntry.Width : paperSizeEntry.Height;
    }
  }

  public int HResolution
  {
    get => (int) this.m_setup.HResolution;
    set => this.m_setup.HResolution = (ushort) value;
  }

  public int VResolution
  {
    get => (int) this.m_setup.VResolution;
    set => this.m_setup.VResolution = (ushort) value;
  }

  public IPage EvenPage => (IPage) this.m_evenPage;

  internal IPage OddPage => (IPage) this.m_oddPage;

  public IPage FirstPage => (IPage) this.m_firstPage;

  public string FullHeaderString
  {
    get => this.m_oddPage.FullHeaderString;
    set => this.m_oddPage.FullHeaderString = value;
  }

  public string FullFooterString
  {
    get => this.m_oddPage.FullFooterString;
    set => this.m_oddPage.FullFooterString = value;
  }

  public bool AlignHFWithPageMargins
  {
    get => this.m_headerFooter.AlignHFWithPageMargins;
    set => this.m_headerFooter.AlignHFWithPageMargins = value;
  }

  public bool DifferentFirstPageHF
  {
    get => this.m_headerFooter.DifferentFirstPageHF;
    set => this.m_headerFooter.DifferentFirstPageHF = value;
  }

  public bool DifferentOddAndEvenPagesHF
  {
    get => this.m_headerFooter.DifferentOddAndEvenPagesHF;
    set => this.m_headerFooter.DifferentOddAndEvenPagesHF = value;
  }

  public bool HFScaleWithDoc
  {
    get => this.m_headerFooter.HFScaleWithDoc;
    set => this.m_headerFooter.HFScaleWithDoc = value;
  }

  internal string FullEvenHeaderString
  {
    get => this.m_evenPage.FullHeaderString;
    set => this.m_evenPage.FullHeaderString = value;
  }

  internal string FullEvenFooterString
  {
    get => this.m_evenPage.FullFooterString;
    set => this.m_evenPage.FullFooterString = value;
  }

  internal string FullFirstHeaderString
  {
    get => this.m_firstPage.FullHeaderString;
    set => this.m_firstPage.FullHeaderString = value;
  }

  internal string FullFirstFooterString
  {
    get => this.m_firstPage.FullFooterString;
    set => this.m_firstPage.FullFooterString = value;
  }

  private void FillMaxPaperSize(ApplicationImpl application)
  {
    this.dictPaperWidth.Add(ExcelPaperSize.A2Paper, application.ConvertUnits(420.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A2Paper, application.ConvertUnits(594.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A3ExtraPaper, application.ConvertUnits(322.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A3ExtraPaper, application.ConvertUnits(445.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A3ExtraTransversePaper, application.ConvertUnits(332.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A3ExtraTransversePaper, application.ConvertUnits(445.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A3TransversePaper, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A3TransversePaper, application.ConvertUnits(420.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A4ExtraPaper, application.ConvertUnits(236.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A4ExtraPaper, application.ConvertUnits(332.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A4PlusPaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A4PlusPaper, application.ConvertUnits(330.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A4TransversePaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A4TransversePaper, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A5ExtraPpaper, application.ConvertUnits(174.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A5ExtraPpaper, application.ConvertUnits(235.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.A5TransversePaper, application.ConvertUnits(148.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.A5TransversePaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.InviteEnvelope, application.ConvertUnits(220.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.InviteEnvelope, application.ConvertUnits(220.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.ISOB4, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.ISOB4, application.ConvertUnits(353.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.ISOB5ExtraPaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.ISOB5ExtraPaper, application.ConvertUnits(276.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.JapaneseDoublePostcard, application.ConvertUnits(200.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.JapaneseDoublePostcard, application.ConvertUnits(148.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.JISB5TransversePaper, application.ConvertUnits(182.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.JISB5TransversePaper, application.ConvertUnits(257.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.LegalExtraPaper9275By15, 9.275);
    this.dictPaperHeight.Add(ExcelPaperSize.LegalExtraPaper9275By15, 15.0);
    this.dictPaperWidth.Add(ExcelPaperSize.LetterExtraPaper9275By12, 9.275);
    this.dictPaperHeight.Add(ExcelPaperSize.LetterExtraPaper9275By12, 12.0);
    this.dictPaperWidth.Add(ExcelPaperSize.LetterExtraTransversePaper, 9.275);
    this.dictPaperHeight.Add(ExcelPaperSize.LetterExtraTransversePaper, 12.0);
    this.dictPaperWidth.Add(ExcelPaperSize.LetterPlusPaper, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.LetterPlusPaper, 12.69);
    this.dictPaperWidth.Add(ExcelPaperSize.LetterTransversePaper, 8.275);
    this.dictPaperHeight.Add(ExcelPaperSize.LetterTransversePaper, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.Paper10x14, 10.0);
    this.dictPaperHeight.Add(ExcelPaperSize.Paper10x14, 14.0);
    this.dictPaperWidth.Add(ExcelPaperSize.Paper11x17, 11.0);
    this.dictPaperHeight.Add(ExcelPaperSize.Paper11x17, 17.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperA3, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperA3, application.ConvertUnits(420.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperA4, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperA4, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperA4Small, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperA4Small, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperA5, application.ConvertUnits(148.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperA5, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperB4, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperB4, application.ConvertUnits(353.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperB5, application.ConvertUnits(176.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperB5, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperCsheet, 17.0);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperCsheet, 22.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperDsheet, 22.0);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperDsheet, 34.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelope10, 4.125);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelope10, 9.5);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelope11, 4.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelope11, 10.375);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelope12, 4.75);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelope12, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelope14, 5.0);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelope14, 11.5);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelope9, 3.875);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelope9, 8.875);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeB4, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeB4, application.ConvertUnits(353.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeB5, application.ConvertUnits(176.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeB5, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeB6, application.ConvertUnits(176.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeB6, application.ConvertUnits(125.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeC3, application.ConvertUnits(324.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeC3, application.ConvertUnits(458.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeC4, application.ConvertUnits(229.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeC4, application.ConvertUnits(324.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeC5, application.ConvertUnits(162.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeC5, application.ConvertUnits(229.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeC6, application.ConvertUnits(114.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeC6, application.ConvertUnits(162.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeC65, application.ConvertUnits(114.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeC65, application.ConvertUnits(229.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeDL, application.ConvertUnits(110.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeDL, application.ConvertUnits(220.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeItaly, application.ConvertUnits(110.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeItaly, application.ConvertUnits(230.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopeMonarch, 3.875);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopeMonarch, 7.5);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEnvelopePersonal, 3.625);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEnvelopePersonal, 6.5);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperEsheet, 34.0);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperEsheet, 34.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperExecutive, 7.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperExecutive, 7.5);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperFanfoldLegalGerman, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperFanfoldLegalGerman, 13.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperFanfoldStdGerman, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperFanfoldStdGerman, 12.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperFanfoldUS, 14.875);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperFanfoldUS, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperFolio, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperFolio, 13.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperLedger, 17.0);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperLedger, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperLegal, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperLegal, 14.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperLetter, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperLetter, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperLetterSmall, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperLetterSmall, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperNote, 8.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperNote, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperQuarto, application.ConvertUnits(215.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.PaperQuarto, application.ConvertUnits(275.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.PaperStatement, 5.5);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperStatement, 8.5);
    this.dictPaperWidth.Add(ExcelPaperSize.PaperTabloid, 11.0);
    this.dictPaperHeight.Add(ExcelPaperSize.PaperTabloid, 17.0);
    this.dictPaperWidth.Add(ExcelPaperSize.StandardPaper10By11, 10.0);
    this.dictPaperHeight.Add(ExcelPaperSize.StandardPaper10By11, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.StandardPaper15By11, 15.0);
    this.dictPaperHeight.Add(ExcelPaperSize.StandardPaper15By11, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.StandardPaper9By11, 9.0);
    this.dictPaperHeight.Add(ExcelPaperSize.StandardPaper9By11, 11.0);
    this.dictPaperWidth.Add(ExcelPaperSize.SuperASuperAA4Paper, application.ConvertUnits(227.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.SuperASuperAA4Paper, application.ConvertUnits(356.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.SuperBSuperBA3Paper, application.ConvertUnits(305.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(ExcelPaperSize.SuperBSuperBA3Paper, application.ConvertUnits(487.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(ExcelPaperSize.TabloidExtraPaper, 11.69);
    this.dictPaperHeight.Add(ExcelPaperSize.TabloidExtraPaper, 18.0);
  }

  public PageSetupBaseImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_setup = (PrintSetupRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PrintSetup);
    if ((parent is ChartImpl ? ((parent as ChartImpl).Workbook.Version != ExcelVersion.Excel97to2003 ? 1 : 0) : ((parent as WorksheetImpl).Workbook.Version != ExcelVersion.Excel97to2003 ? 1 : 0)) != 0)
    {
      this.m_dBottomMargin = 0.75;
      this.m_dTopMargin = 0.75;
      this.m_dLeftMargin = 0.7;
      this.m_dRightMargin = 0.7;
      this.m_setup.HeaderMargin = 0.3;
      this.m_setup.FooterMargin = 0.3;
      this.m_setup.IsNotValidSettings = true;
    }
    this.m_headerFooter = new HeaderAndFooterRecord();
    this.m_oddPage = new Page(this);
    this.m_evenPage = new Page(this);
    this.m_evenPage.IsEvenPage = true;
    this.m_firstPage = new Page(this);
    this.m_firstPage.IsFirstPage = true;
    this.FillMaxPaperSize(application as ApplicationImpl);
    this.FindParents();
    if (!(parent is ChartImpl) || (parent as ChartImpl).Parent is ChartShapeImpl)
      return;
    this.m_setup.IsNotLandscape = false;
    this.m_setup.IsNoOrientation = false;
    this.m_setup.IsNotValidSettings = true;
  }

  protected virtual void FindParents()
  {
    this.m_sheet = CommonObject.FindParent(this.Parent, typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Parent worksheet.");
  }

  protected internal string[] ParseHeaderFooterString(string strToSplit)
  {
    if (strToSplit == null)
      throw new ArgumentNullException(nameof (strToSplit));
    string[] headerFooterString = new string[3]
    {
      string.Empty,
      string.Empty,
      string.Empty
    };
    if (strToSplit.Length == 0)
      return headerFooterString;
    string pattern = "[&]?&[L,l,c,C,R,r]";
    MatchCollection matchCollection = Regex.Matches(strToSplit, pattern);
    List<int> intList = new List<int>();
    int length = strToSplit.Length;
    foreach (Match match in matchCollection)
    {
      if (match.Value.Equals("&L", StringComparison.CurrentCultureIgnoreCase) || match.Value.Equals("&C", StringComparison.CurrentCultureIgnoreCase) || match.Value.Equals("&R", StringComparison.CurrentCultureIgnoreCase))
        intList.Add(match.Index);
    }
    if (intList.Count == 0)
    {
      headerFooterString[1] = strToSplit;
      return headerFooterString;
    }
    intList.Sort();
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    for (int index = 0; index < intList.Count; ++index)
    {
      if (num1 == 0 && strToSplit[intList[index] + 1].ToString().Equals("L", StringComparison.CurrentCultureIgnoreCase))
      {
        if (index + 1 < intList.Count)
        {
          string[] strArray;
          (strArray = headerFooterString)[0] = strArray[0] + strToSplit.Substring(intList[index] + 2, intList[index + 1] - intList[index] - 2);
          ++num1;
        }
        else
        {
          string[] strArray;
          (strArray = headerFooterString)[0] = strArray[0] + strToSplit.Substring(intList[index] + 2, length - intList[index] - 2);
          ++num1;
        }
      }
      if (num3 == 0 && strToSplit[intList[index] + 1].ToString().Equals("C", StringComparison.CurrentCultureIgnoreCase))
      {
        if (index + 1 < intList.Count)
        {
          string[] strArray;
          (strArray = headerFooterString)[1] = strArray[1] + strToSplit.Substring(intList[index] + 2, intList[index + 1] - intList[index] - 2);
          ++num3;
        }
        else
        {
          string[] strArray;
          (strArray = headerFooterString)[1] = strArray[1] + strToSplit.Substring(intList[index] + 2, length - intList[index] - 2);
          ++num3;
        }
      }
      if (num2 == 0 && strToSplit[intList[index] + 1].ToString().Equals("R", StringComparison.CurrentCultureIgnoreCase))
      {
        if (index + 1 < intList.Count)
        {
          string[] strArray;
          (strArray = headerFooterString)[2] = strArray[2] + strToSplit.Substring(intList[index] + 2, intList[index + 1] - intList[index] - 2);
          ++num2;
        }
        else
        {
          string[] strArray;
          (strArray = headerFooterString)[2] = strArray[2] + strToSplit.Substring(intList[index] + 2, length - intList[index] - 2);
          ++num2;
        }
      }
    }
    return headerFooterString;
  }

  protected internal string CreateHeaderFooterString(string[] parts)
  {
    if (parts == null)
      throw new ArgumentNullException(nameof (parts));
    if (parts.Length < 3 || parts.Length > 3)
      throw new ArgumentException("Parts array must have only three elements", nameof (parts));
    string headerFooterString = string.Empty;
    if (parts[0] != null && parts[0].Length > 0)
      headerFooterString = $"{headerFooterString}&L{parts[0]}";
    if (parts[1] != null && parts[1].Length > 0)
      headerFooterString = $"{headerFooterString}&C{parts[1]}";
    if (parts[2] != null && parts[2].Length > 0)
      headerFooterString = $"{headerFooterString}&R{parts[2]}";
    return headerFooterString;
  }

  [CLSCompliant(false)]
  public virtual void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_setup == null)
      throw new ArgumentNullException("m_Setup");
    this.SerializeStartRecords(records);
    HeaderFooterRecord record1 = (HeaderFooterRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Header);
    record1.Value = (this.OddPage as Page).FullHeaderString;
    records.Add((IBiffStorage) record1);
    HeaderFooterRecord record2 = (HeaderFooterRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Footer);
    record2.Value = (this.OddPage as Page).FullFooterString;
    records.Add((IBiffStorage) record2);
    SheetCenterRecord record3 = (SheetCenterRecord) BiffRecordFactory.GetRecord(TBIFFRecord.HCenter);
    record3.IsCenter = this.m_bHCenter ? (ushort) 1 : (ushort) 0;
    records.Add((IBiffStorage) record3);
    SheetCenterRecord record4 = (SheetCenterRecord) BiffRecordFactory.GetRecord(TBIFFRecord.VCenter);
    record4.IsCenter = this.m_bVCenter ? (ushort) 1 : (ushort) 0;
    records.Add((IBiffStorage) record4);
    this.SerializeMargin(records, TBIFFRecord.LeftMargin, this.m_dLeftMargin, 0.75);
    this.SerializeMargin(records, TBIFFRecord.RightMargin, this.m_dRightMargin, 0.75);
    this.SerializeMargin(records, TBIFFRecord.TopMargin, this.m_dTopMargin, 1.0);
    this.SerializeMargin(records, TBIFFRecord.BottomMargin, this.m_dBottomMargin, 1.0);
    if (this.m_unknown != null)
      records.Add((IBiffStorage) this.m_unknown);
    records.Add((IBiffStorage) this.m_setup);
    if (this.m_headerFooter != null)
    {
      this.m_headerFooter.EvenHeaderString = this.FullEvenHeaderString;
      this.m_headerFooter.EvenFooterString = this.FullEvenFooterString;
      this.m_headerFooter.FirstHeaderString = this.FullFirstHeaderString;
      this.m_headerFooter.FirstFooterString = this.FullFirstFooterString;
      records.Add((IBiffStorage) this.m_headerFooter);
    }
    this.SerializeEndRecords(records);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeStartRecords(OffsetArrayList records)
  {
  }

  [CLSCompliant(false)]
  protected virtual void SerializeEndRecords(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_backgroundImage == null || this.m_backgroundImage.Picture == null)
      return;
    records.Add((IBiffStorage) this.m_backgroundImage);
  }

  public virtual int Parse(IList<BiffRecordRaw> data, int position)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (position < 0 || position > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (position), "Value cannot be less than 0 and greater than data.Count - 1");
    for (int count = data.Count; position < count; ++position)
    {
      if (!this.ParseRecord(data[position]))
      {
        --position;
        break;
      }
    }
    return position;
  }

  [CLSCompliant(false)]
  protected virtual bool ParseRecord(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.Header:
        (this.OddPage as Page).FullHeaderString = ((HeaderFooterRecord) record).Value;
        break;
      case TBIFFRecord.Footer:
        (this.OddPage as Page).FullFooterString = ((HeaderFooterRecord) record).Value;
        break;
      case TBIFFRecord.LeftMargin:
        this.m_dLeftMargin = ((MarginRecord) record).Margin;
        break;
      case TBIFFRecord.RightMargin:
        this.m_dRightMargin = ((MarginRecord) record).Margin;
        break;
      case TBIFFRecord.TopMargin:
        this.m_dTopMargin = ((MarginRecord) record).Margin;
        break;
      case TBIFFRecord.BottomMargin:
        this.m_dBottomMargin = ((MarginRecord) record).Margin;
        break;
      case TBIFFRecord.PrinterSettings:
        this.m_unknown = (PrinterSettingsRecord) record;
        break;
      case TBIFFRecord.HCenter:
        this.m_bHCenter = ((SheetCenterRecord) record).IsCenter != (ushort) 0;
        break;
      case TBIFFRecord.VCenter:
        this.m_bVCenter = ((SheetCenterRecord) record).IsCenter != (ushort) 0;
        break;
      case TBIFFRecord.PrintSetup:
        this.m_setup = (PrintSetupRecord) record;
        break;
      case TBIFFRecord.Bitmap:
        this.m_backgroundImage = (BitmapRecord) record;
        break;
      case TBIFFRecord.HeaderFooter:
        this.m_headerFooter = (HeaderAndFooterRecord) record;
        (this.EvenPage as Page).FullHeaderString = this.m_headerFooter.EvenHeaderString;
        (this.EvenPage as Page).FullFooterString = this.m_headerFooter.EvenFooterString;
        (this.FirstPage as Page).FullHeaderString = this.m_headerFooter.FirstHeaderString;
        (this.FirstPage as Page).FullFooterString = this.m_headerFooter.FirstFooterString;
        break;
      default:
        return false;
    }
    return true;
  }

  [CLSCompliant(false)]
  protected BiffRecordRaw GetOrCreateRecord(IList data, ref int pos, TBIFFRecord type)
  {
    BiffRecordRaw record = (BiffRecordRaw) data[pos];
    if (record.TypeCode != type)
      record = BiffRecordFactory.GetRecord(type);
    else
      ++pos;
    return record;
  }

  [CLSCompliant(false)]
  protected BiffRecordRaw GetRecordUpdatePos(IList data, ref int pos)
  {
    return (BiffRecordRaw) data[pos++];
  }

  [CLSCompliant(false)]
  protected BiffRecordRaw GetRecordUpdatePos(IList data, ref int pos, TBIFFRecord type)
  {
    BiffRecordRaw recordUpdatePos;
    do
    {
      recordUpdatePos = (BiffRecordRaw) data[pos];
      ++pos;
      if (pos >= data.Count)
      {
        recordUpdatePos = (BiffRecordRaw) null;
        break;
      }
    }
    while (recordUpdatePos.TypeCode != type);
    return recordUpdatePos;
  }

  private void SerializeMargin(
    OffsetArrayList records,
    TBIFFRecord code,
    double marginValue,
    double defaultValue)
  {
    if (marginValue == defaultValue)
      return;
    MarginRecord record = (MarginRecord) BiffRecordFactory.GetRecord(code);
    record.Margin = marginValue;
    records.Add((IBiffStorage) record);
  }

  protected void SetChanged() => this.m_sheet.SetChanged();

  internal PageSetupBaseImpl Clone(object parent)
  {
    PageSetupBaseImpl pageSetupBase = (PageSetupBaseImpl) this.MemberwiseClone();
    pageSetupBase.SetParent(parent);
    pageSetupBase.FindParents();
    if (this.m_oddPage != null)
      pageSetupBase.m_oddPage = this.m_oddPage.Clone(pageSetupBase);
    if (this.m_evenPage != null)
      pageSetupBase.m_evenPage = this.m_evenPage.Clone(pageSetupBase);
    if (this.m_firstPage != null)
      pageSetupBase.m_firstPage = this.m_firstPage.Clone(pageSetupBase);
    return pageSetupBase;
  }

  public TBIFFRecord TypeCode => TBIFFRecord.Unknown;

  public int RecordCode => 0;

  public bool NeedDataArray => false;

  public long StreamPos
  {
    get => -1;
    set
    {
    }
  }

  public virtual int GetStoreSize(ExcelVersion version)
  {
    int num = 12 + this.m_setup.GetStoreSize(version) + 4 + (this.m_unknown != null ? this.m_unknown.GetStoreSize(version) + 4 : 0) + (this.m_dBottomMargin != 1.0 ? 12 : 0) + (this.m_dTopMargin != 1.0 ? 12 : 0) + (this.m_dRightMargin != 0.75 ? 12 : 0) + (this.m_dLeftMargin != 0.75 ? 12 : 0);
    int length1 = this.FullHeaderString.Length;
    int length2 = this.FullFooterString.Length;
    if (length1 > 0)
      num += length1 * 2 + 3;
    int storeSize = num + 4;
    if (length2 > 0)
      storeSize += length2 * 2 + 3;
    if (this.m_backgroundImage != null)
      storeSize += this.m_backgroundImage.GetStoreSize(version) + 4;
    if (this.m_headerFooter != null)
      storeSize += this.m_headerFooter.GetStoreSize(version) + 4;
    return storeSize;
  }

  public int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    int num1 = this.FillStreamStart(writer, provider, encryptor, streamPosition);
    int num2 = num1 + this.SerializeHeaderFooterString(writer, provider, encryptor, TBIFFRecord.Header, this.FullHeaderString, streamPosition + num1);
    int num3 = num2 + this.SerializeHeaderFooterString(writer, provider, encryptor, TBIFFRecord.Footer, this.FullFooterString, streamPosition + num2);
    int num4 = num3 + this.WriteUShortRecord(writer, provider, encryptor, TBIFFRecord.HCenter, this.m_bHCenter ? (ushort) 1 : (ushort) 0, streamPosition + num3);
    int num5 = num4 + this.WriteUShortRecord(writer, provider, encryptor, TBIFFRecord.VCenter, this.m_bVCenter ? (ushort) 1 : (ushort) 0, streamPosition + num4);
    int num6 = num5 + this.FillStreamWithMargin(writer, provider, encryptor, TBIFFRecord.LeftMargin, this.m_dLeftMargin, 0.75, streamPosition + num5);
    int num7 = num6 + this.FillStreamWithMargin(writer, provider, encryptor, TBIFFRecord.RightMargin, this.m_dRightMargin, 0.75, streamPosition + num6);
    int num8 = num7 + this.FillStreamWithMargin(writer, provider, encryptor, TBIFFRecord.TopMargin, this.m_dTopMargin, 1.0, streamPosition + num7);
    int num9 = num8 + this.FillStreamWithMargin(writer, provider, encryptor, TBIFFRecord.BottomMargin, this.m_dBottomMargin, 1.0, streamPosition + num8);
    if (this.m_unknown != null)
      num9 += this.m_unknown.FillStream(writer, provider, encryptor, streamPosition + num9);
    int num10 = num9 + this.m_setup.FillStream(writer, provider, encryptor, streamPosition + num9);
    if (this.m_headerFooter != null)
    {
      this.m_headerFooter.EvenHeaderString = this.FullEvenHeaderString;
      this.m_headerFooter.EvenFooterString = this.FullEvenFooterString;
      this.m_headerFooter.FirstHeaderString = this.FullFirstHeaderString;
      this.m_headerFooter.FirstFooterString = this.FullFirstFooterString;
      num10 += this.m_headerFooter.FillStream(writer, provider, encryptor, streamPosition + num10);
    }
    return num10 + this.FillStreamEnd(writer, provider, encryptor, streamPosition + num10);
  }

  private int SerializeHeaderFooterString(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    TBIFFRecord code,
    string value,
    int streamPosition)
  {
    int iOffset1 = 0;
    provider.WriteUInt16(iOffset1, (ushort) code);
    int iOffset2 = iOffset1 + 2;
    int num1 = value != null ? value.Length * 2 : 0;
    if (num1 > 0)
      num1 += 3;
    provider.WriteInt16(iOffset2, (short) num1);
    int offset = iOffset2 + 2;
    provider.WriteString16BitUpdateOffset(ref offset, value);
    int num2 = num1 + 4;
    ByteArrayDataProvider arrayDataProvider = (ByteArrayDataProvider) provider;
    encryptor?.Encrypt(provider, 4, num2, (long) (streamPosition + 4));
    provider.WriteInto(writer, 0, num2, arrayDataProvider.InternalBuffer);
    return num2;
  }

  [CLSCompliant(false)]
  protected int WriteUShortRecord(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    TBIFFRecord code,
    ushort value,
    int streamPosition)
  {
    provider.WriteUInt16(0, (ushort) code);
    provider.WriteUInt16(2, (ushort) 2);
    provider.WriteUInt16(4, value);
    encryptor?.Encrypt(provider, 4, 2, (long) (streamPosition + 4));
    provider.WriteInto(writer, 0, 6, (byte[]) null);
    return 6;
  }

  private int FillStreamWithMargin(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    TBIFFRecord code,
    double value,
    double defaultValue,
    int streamPosition)
  {
    int num = 0;
    if (value != defaultValue)
    {
      provider.WriteUInt16(0, (ushort) code);
      provider.WriteUInt16(2, (ushort) 8);
      provider.WriteDouble(4, value);
      encryptor?.Encrypt(provider, 4, 8, (long) (streamPosition + 4));
      provider.WriteInto(writer, 0, 12, (byte[]) null);
      num = 12;
    }
    return num;
  }

  protected virtual int FillStreamStart(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    return 0;
  }

  protected virtual int FillStreamEnd(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    int num = 0;
    if (this.m_backgroundImage != null && this.m_backgroundImage.Picture != null)
      num = this.m_backgroundImage.FillStream(writer, provider, encryptor, streamPosition);
    return num;
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this.m_setup != null)
      this.m_setup = (PrintSetupRecord) null;
    if (this.m_unknown != null && this.m_sheet == null)
      this.m_unknown.Dispose();
    if (this.dictPaperHeight != null)
    {
      this.dictPaperHeight.Clear();
      this.dictPaperHeight = (Dictionary<ExcelPaperSize, double>) null;
    }
    if (this.dictPaperWidth != null)
    {
      this.dictPaperWidth.Clear();
      this.dictPaperWidth = (Dictionary<ExcelPaperSize, double>) null;
    }
    this.m_oddPage.Dispose();
    this.m_evenPage.Dispose();
    this.m_firstPage.Dispose();
    GC.SuppressFinalize((object) this);
  }

  protected internal enum THeaderSide
  {
    Left,
    Center,
    Right,
    EvenLeft,
    EvenCenter,
    EvenRight,
    FirstLeft,
    FirstCenter,
    FirstRight,
  }

  public sealed class PaperSizeEntry
  {
    public double Width;
    public double Height;

    private PaperSizeEntry()
    {
    }

    public PaperSizeEntry(double width, double height, MeasureUnits units)
    {
      this.Width = ApplicationImpl.ConvertUnitsStatic(width, units, MeasureUnits.Point);
      this.Height = ApplicationImpl.ConvertUnitsStatic(height, units, MeasureUnits.Point);
    }
  }
}
