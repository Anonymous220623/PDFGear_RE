// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.PageSetupBaseImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class PageSetupBaseImpl : CommonObject, IPageSetupBase, IParentApplication, IBiffStorage
{
  public const double DEFAULT_TOPMARGIN = 1.0;
  public const double DEFAULT_BOTTOMMARGIN = 1.0;
  public const double DEFAULT_LEFTMARGIN = 0.75;
  public const double DEFAULT_RIGHTMARGIN = 0.75;
  private static readonly string[] DEF_HEADER_NAMES = new string[3]
  {
    "LH",
    "CH",
    "RH"
  };
  private static readonly string[] DEF_FOOTER_NAMES = new string[3]
  {
    "LF",
    "CF",
    "RF"
  };
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
  internal Dictionary<OfficePaperSize, double> dictPaperWidth = new Dictionary<OfficePaperSize, double>();
  internal Dictionary<OfficePaperSize, double> dictPaperHeight = new Dictionary<OfficePaperSize, double>();
  [CLSCompliant(false)]
  protected HeaderAndFooterRecord m_headerFooter;
  private int m_headerStringLimit = (int) byte.MaxValue;
  private int m_footerStringLimit = (int) byte.MaxValue;

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
        this.SetChanged();
      }
      if (this.m_sheet.ParentWorkbook.IsWorkbookOpening)
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
        this.SetChanged();
      }
      if (this.m_sheet.ParentWorkbook.IsWorkbookOpening)
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
      this.SetChanged();
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
      this.SetChanged();
    }
  }

  public string CenterFooter
  {
    get => this.m_arrFooters[1];
    set
    {
      if (!(this.m_arrFooters[0] != value))
        return;
      if (value != "" && this.CenterFooter.Length == 0)
        this.m_footerStringLimit -= 2;
      else if (this.CenterFooter.Length != 0 && value == "")
        this.m_footerStringLimit += 2;
      if (this.LeftFooter.Length + value.Length + this.RightFooter.Length > this.m_footerStringLimit)
        throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      this.m_arrFooters[1] = value;
      this.SetChanged();
    }
  }

  public Image CenterFooterImage
  {
    get
    {
      return ((BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[PageSetupBaseImpl.DEF_FOOTER_NAMES[1]])?.Picture;
    }
    set => this.m_sheet.HeaderFooterShapes.SetPicture(PageSetupBaseImpl.DEF_FOOTER_NAMES[1], value);
  }

  public Image CenterHeaderImage
  {
    get
    {
      return ((BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[PageSetupBaseImpl.DEF_HEADER_NAMES[1]])?.Picture;
    }
    set => this.m_sheet.HeaderFooterShapes.SetPicture(PageSetupBaseImpl.DEF_HEADER_NAMES[1], value);
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
        throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      this.m_arrHeaders[1] = value;
      this.SetChanged();
    }
  }

  public bool CenterHorizontally
  {
    get => this.m_bHCenter;
    set
    {
      if (this.m_bHCenter == value)
        return;
      this.m_bHCenter = value;
      this.SetChanged();
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
      this.SetChanged();
    }
  }

  public int Copies
  {
    get => (int) this.m_setup.Copies;
    set
    {
      if (value < 1)
      {
        if (!((WorkbookImpl) this.m_sheet.Workbook).IsWorkbookOpening)
          throw new ArgumentOutOfRangeException("Number of copies can not be less then 1");
        value = 1;
      }
      if ((int) this.m_setup.Copies == (int) (ushort) value)
        return;
      this.m_setup.Copies = (ushort) value;
      this.m_setup.IsNotValidSettings = false;
      this.SetChanged();
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
      this.SetChanged();
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
      this.SetChanged();
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
      this.SetChanged();
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
      this.SetChanged();
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
        throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      this.m_arrFooters[0] = value;
      this.SetChanged();
    }
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
        throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      this.m_arrHeaders[0] = value;
      this.SetChanged();
    }
  }

  public Image LeftFooterImage
  {
    get
    {
      return ((BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[PageSetupBaseImpl.DEF_FOOTER_NAMES[0]])?.Picture;
    }
    set => this.m_sheet.HeaderFooterShapes.SetPicture(PageSetupBaseImpl.DEF_FOOTER_NAMES[0], value);
  }

  public Image LeftHeaderImage
  {
    get
    {
      return ((BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[PageSetupBaseImpl.DEF_HEADER_NAMES[0]])?.Picture;
    }
    set => this.m_sheet.HeaderFooterShapes.SetPicture(PageSetupBaseImpl.DEF_HEADER_NAMES[0], value);
  }

  public double LeftMargin
  {
    get => this.m_dLeftMargin;
    set
    {
      if (this.m_dLeftMargin == value)
        return;
      this.m_dLeftMargin = value;
      this.SetChanged();
    }
  }

  public OfficeOrder Order
  {
    get => !this.m_setup.IsLeftToRight ? OfficeOrder.DownThenOver : OfficeOrder.OverThenDown;
    set
    {
      bool flag = value == OfficeOrder.OverThenDown;
      if (this.m_setup.IsLeftToRight == flag)
        return;
      this.m_setup.IsLeftToRight = flag;
      this.SetChanged();
    }
  }

  public OfficePageOrientation Orientation
  {
    get
    {
      return !this.m_setup.IsNotLandscape ? OfficePageOrientation.Landscape : OfficePageOrientation.Portrait;
    }
    set
    {
      this.m_setup.IsNotLandscape = value == OfficePageOrientation.Portrait;
      this.m_setup.IsNotValidSettings = false;
      this.m_setup.IsNoOrientation = false;
      this.SetChanged();
    }
  }

  public OfficePaperSize PaperSize
  {
    get => (OfficePaperSize) this.m_setup.PaperSize;
    set
    {
      this.m_setup.PaperSize = (ushort) value;
      this.m_setup.IsNotValidSettings = false;
      this.SetChanged();
    }
  }

  public OfficePrintLocation PrintComments
  {
    get
    {
      if (!this.m_setup.IsNotes)
        return OfficePrintLocation.PrintNoComments;
      return this.m_setup.IsPrintNotesAsDisplayed ? OfficePrintLocation.PrintInPlace : OfficePrintLocation.PrintSheetEnd;
    }
    set
    {
      switch (value)
      {
        case OfficePrintLocation.PrintInPlace:
          this.m_setup.IsNotes = true;
          this.m_setup.IsPrintNotesAsDisplayed = true;
          break;
        case OfficePrintLocation.PrintNoComments:
          this.m_setup.IsNotes = false;
          break;
        case OfficePrintLocation.PrintSheetEnd:
          this.m_setup.IsNotes = true;
          this.m_setup.IsPrintNotesAsDisplayed = false;
          break;
      }
      this.SetChanged();
    }
  }

  public OfficePrintErrors PrintErrors
  {
    get => this.m_setup.PrintErrors;
    set
    {
      if (this.m_setup.PrintErrors == value)
        return;
      this.m_setup.PrintErrors = value;
      this.SetChanged();
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
      this.SetChanged();
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
      this.SetChanged();
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
        throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      this.m_arrFooters[2] = value;
      this.SetChanged();
    }
  }

  public Image RightFooterImage
  {
    get
    {
      return ((BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[PageSetupBaseImpl.DEF_FOOTER_NAMES[2]])?.Picture;
    }
    set => this.m_sheet.HeaderFooterShapes.SetPicture(PageSetupBaseImpl.DEF_FOOTER_NAMES[2], value);
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
        throw new ArgumentOutOfRangeException(nameof (value), "The string is too long. Reduce the number of characters used.");
      this.m_arrHeaders[2] = value;
      this.SetChanged();
    }
  }

  public Image RightHeaderImage
  {
    get
    {
      return ((BitmapShapeImpl) this.m_sheet.HeaderFooterShapes[PageSetupBaseImpl.DEF_HEADER_NAMES[2]])?.Picture;
    }
    set => this.m_sheet.HeaderFooterShapes.SetPicture(PageSetupBaseImpl.DEF_HEADER_NAMES[2], value);
  }

  public double RightMargin
  {
    get => this.m_dRightMargin;
    set
    {
      if (this.m_dRightMargin == value)
        return;
      this.m_dRightMargin = value;
      this.SetChanged();
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
      this.SetChanged();
    }
  }

  public int Zoom
  {
    get => (int) this.m_setup.Scale;
    set
    {
      this.m_setup.Scale = value >= 10 && value <= 400 ? (ushort) value : throw new ArgumentOutOfRangeException("Zoom value must be beetween 10 and 400 percent.");
      this.m_setup.IsNotValidSettings = false;
      this.SetChanged();
    }
  }

  public Bitmap BackgoundImage
  {
    get => this.m_backgroundImage == null ? (Bitmap) null : this.m_backgroundImage.Picture;
    [SecuritySafeCritical] set
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
      return this.Orientation != OfficePageOrientation.Portrait ? paperSizeEntry.Height : paperSizeEntry.Width;
    }
  }

  public double PageHeight
  {
    get
    {
      PageSetupBaseImpl.PaperSizeEntry paperSizeEntry = this.AppImplementation.DicPaperSizeTable.ContainsKey((int) this.PaperSize) ? this.AppImplementation.DicPaperSizeTable[(int) this.PaperSize] : this.AppImplementation.DicPaperSizeTable[9];
      return this.Orientation != OfficePageOrientation.Portrait ? paperSizeEntry.Width : paperSizeEntry.Height;
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

  public string FullHeaderString
  {
    get => this.CreateHeaderFooterString(this.m_arrHeaders);
    set => this.m_arrHeaders = this.ParseHeaderFooterString(value);
  }

  public string FullFooterString
  {
    get => this.CreateHeaderFooterString(this.m_arrFooters);
    set => this.m_arrFooters = this.ParseHeaderFooterString(value);
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

  private void FillMaxPaperSize(ApplicationImpl application)
  {
    this.dictPaperWidth.Add(OfficePaperSize.A2Paper, application.ConvertUnits(420.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A2Paper, application.ConvertUnits(594.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A3ExtraPaper, application.ConvertUnits(322.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A3ExtraPaper, application.ConvertUnits(445.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A3ExtraTransversePaper, application.ConvertUnits(332.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A3ExtraTransversePaper, application.ConvertUnits(445.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A3TransversePaper, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A3TransversePaper, application.ConvertUnits(420.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A4ExtraPaper, application.ConvertUnits(236.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A4ExtraPaper, application.ConvertUnits(332.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A4PlusPaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A4PlusPaper, application.ConvertUnits(330.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A4TransversePaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A4TransversePaper, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A5ExtraPpaper, application.ConvertUnits(174.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A5ExtraPpaper, application.ConvertUnits(235.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.A5TransversePaper, application.ConvertUnits(148.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.A5TransversePaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.InviteEnvelope, application.ConvertUnits(220.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.InviteEnvelope, application.ConvertUnits(220.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.ISOB4, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.ISOB4, application.ConvertUnits(353.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.ISOB5ExtraPaper, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.ISOB5ExtraPaper, application.ConvertUnits(276.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.JapaneseDoublePostcard, application.ConvertUnits(200.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.JapaneseDoublePostcard, application.ConvertUnits(148.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.JISB5TransversePaper, application.ConvertUnits(182.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.JISB5TransversePaper, application.ConvertUnits(257.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.LegalExtraPaper9275By15, 9.275);
    this.dictPaperHeight.Add(OfficePaperSize.LegalExtraPaper9275By15, 15.0);
    this.dictPaperWidth.Add(OfficePaperSize.LetterExtraPaper9275By12, 9.275);
    this.dictPaperHeight.Add(OfficePaperSize.LetterExtraPaper9275By12, 12.0);
    this.dictPaperWidth.Add(OfficePaperSize.LetterExtraTransversePaper, 9.275);
    this.dictPaperHeight.Add(OfficePaperSize.LetterExtraTransversePaper, 12.0);
    this.dictPaperWidth.Add(OfficePaperSize.LetterPlusPaper, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.LetterPlusPaper, 12.69);
    this.dictPaperWidth.Add(OfficePaperSize.LetterTransversePaper, 8.275);
    this.dictPaperHeight.Add(OfficePaperSize.LetterTransversePaper, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.Paper10x14, 10.0);
    this.dictPaperHeight.Add(OfficePaperSize.Paper10x14, 14.0);
    this.dictPaperWidth.Add(OfficePaperSize.Paper11x17, 11.0);
    this.dictPaperHeight.Add(OfficePaperSize.Paper11x17, 17.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperA3, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperA3, application.ConvertUnits(420.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperA4, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperA4, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperA4Small, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperA4Small, application.ConvertUnits(297.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperA5, application.ConvertUnits(148.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperA5, application.ConvertUnits(210.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperB4, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperB4, application.ConvertUnits(353.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperB5, application.ConvertUnits(176.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperB5, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperCsheet, 17.0);
    this.dictPaperHeight.Add(OfficePaperSize.PaperCsheet, 22.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperDsheet, 22.0);
    this.dictPaperHeight.Add(OfficePaperSize.PaperDsheet, 34.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelope10, 4.125);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelope10, 9.5);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelope11, 4.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelope11, 10.375);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelope12, 4.75);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelope12, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelope14, 5.0);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelope14, 11.5);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelope9, 3.875);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelope9, 8.875);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeB4, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeB4, application.ConvertUnits(353.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeB5, application.ConvertUnits(176.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeB5, application.ConvertUnits(250.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeB6, application.ConvertUnits(176.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeB6, application.ConvertUnits(125.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeC3, application.ConvertUnits(324.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeC3, application.ConvertUnits(458.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeC4, application.ConvertUnits(229.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeC4, application.ConvertUnits(324.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeC5, application.ConvertUnits(162.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeC5, application.ConvertUnits(229.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeC6, application.ConvertUnits(114.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeC6, application.ConvertUnits(162.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeC65, application.ConvertUnits(114.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeC65, application.ConvertUnits(229.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeDL, application.ConvertUnits(110.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeDL, application.ConvertUnits(220.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeItaly, application.ConvertUnits(110.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeItaly, application.ConvertUnits(230.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopeMonarch, 3.875);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopeMonarch, 7.5);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEnvelopePersonal, 3.625);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEnvelopePersonal, 6.5);
    this.dictPaperWidth.Add(OfficePaperSize.PaperEsheet, 34.0);
    this.dictPaperHeight.Add(OfficePaperSize.PaperEsheet, 34.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperExecutive, 7.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperExecutive, 7.5);
    this.dictPaperWidth.Add(OfficePaperSize.PaperFanfoldLegalGerman, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperFanfoldLegalGerman, 13.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperFanfoldStdGerman, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperFanfoldStdGerman, 12.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperFanfoldUS, 14.875);
    this.dictPaperHeight.Add(OfficePaperSize.PaperFanfoldUS, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperFolio, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperFolio, 13.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperLedger, 17.0);
    this.dictPaperHeight.Add(OfficePaperSize.PaperLedger, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperLegal, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperLegal, 14.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperLetter, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperLetter, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperLetterSmall, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperLetterSmall, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperNote, 8.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperNote, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.PaperQuarto, application.ConvertUnits(215.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.PaperQuarto, application.ConvertUnits(275.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.PaperStatement, 5.5);
    this.dictPaperHeight.Add(OfficePaperSize.PaperStatement, 8.5);
    this.dictPaperWidth.Add(OfficePaperSize.PaperTabloid, 11.0);
    this.dictPaperHeight.Add(OfficePaperSize.PaperTabloid, 17.0);
    this.dictPaperWidth.Add(OfficePaperSize.StandardPaper10By11, 10.0);
    this.dictPaperHeight.Add(OfficePaperSize.StandardPaper10By11, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.StandardPaper15By11, 15.0);
    this.dictPaperHeight.Add(OfficePaperSize.StandardPaper15By11, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.StandardPaper9By11, 9.0);
    this.dictPaperHeight.Add(OfficePaperSize.StandardPaper9By11, 11.0);
    this.dictPaperWidth.Add(OfficePaperSize.SuperASuperAA4Paper, application.ConvertUnits(227.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.SuperASuperAA4Paper, application.ConvertUnits(356.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.SuperBSuperBA3Paper, application.ConvertUnits(305.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperHeight.Add(OfficePaperSize.SuperBSuperBA3Paper, application.ConvertUnits(487.0, MeasureUnits.Millimeter, MeasureUnits.Inch));
    this.dictPaperWidth.Add(OfficePaperSize.TabloidExtraPaper, 11.69);
    this.dictPaperHeight.Add(OfficePaperSize.TabloidExtraPaper, 18.0);
  }

  public PageSetupBaseImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_setup = (PrintSetupRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PrintSetup);
    this.m_headerFooter = new HeaderAndFooterRecord();
    this.FillMaxPaperSize(application as ApplicationImpl);
    this.FindParents();
  }

  protected virtual void FindParents()
  {
    this.m_sheet = CommonObject.FindParent(this.Parent, typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Parent worksheet.");
  }

  protected string[] ParseHeaderFooterString(string strToSplit)
  {
    if (strToSplit == null)
      throw new ArgumentNullException(nameof (strToSplit));
    string[] headerFooterString = new string[3]
    {
      string.Empty,
      string.Empty,
      string.Empty
    };
    int length1 = strToSplit.Length;
    if (length1 == 0)
      return headerFooterString;
    int num1 = strToSplit.IndexOf("&L");
    int length2 = strToSplit.IndexOf("&C");
    int length3 = strToSplit.IndexOf("&R");
    if (num1 == length2 && length2 == length3 && length3 == -1)
    {
      headerFooterString[1] = strToSplit;
      return headerFooterString;
    }
    if (num1 >= 0)
    {
      int num2 = length1;
      if (length2 > num1)
        num2 = length2;
      else if (length3 > num1)
        num2 = length3;
      headerFooterString[0] = strToSplit.Substring(num1 + 2, num2 - num1 - 2);
    }
    if (length2 >= 0)
    {
      int num3 = length1;
      if (length3 > length2)
        num3 = length3;
      headerFooterString[1] = strToSplit.Substring(length2 + 2, num3 - length2 - 2);
      if (length2 > 0 && num1 < 0)
        headerFooterString[0] = strToSplit.Substring(0, length2);
    }
    if (length3 >= 0)
    {
      int num4 = length1;
      headerFooterString[2] = strToSplit.Substring(length3 + 2, num4 - length3 - 2);
      if (length3 > 0 && length2 < 0 && num1 < 0)
        headerFooterString[1] = strToSplit.Substring(0, length3);
    }
    return headerFooterString;
  }

  protected string CreateHeaderFooterString(string[] parts)
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
    record1.Value = this.CreateHeaderFooterString(this.m_arrHeaders);
    records.Add((IBiffStorage) record1);
    HeaderFooterRecord record2 = (HeaderFooterRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Footer);
    record2.Value = this.CreateHeaderFooterString(this.m_arrFooters);
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
    if (this.m_headerFooter != null && this.m_headerFooter.Length != -1)
      records.Add((IBiffStorage) this.m_headerFooter);
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
        this.m_arrHeaders = this.ParseHeaderFooterString(((HeaderFooterRecord) record).Value);
        break;
      case TBIFFRecord.Footer:
        this.m_arrFooters = this.ParseHeaderFooterString(((HeaderFooterRecord) record).Value);
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

  public virtual int GetStoreSize(OfficeVersion version)
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
    if (this.m_headerFooter != null && this.m_headerFooter.Length > 0)
      storeSize += this.m_headerFooter.GetStoreSize(version) + 4;
    return storeSize;
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
      this.dictPaperHeight = (Dictionary<OfficePaperSize, double>) null;
    }
    if (this.dictPaperWidth != null)
    {
      this.dictPaperWidth.Clear();
      this.dictPaperWidth = (Dictionary<OfficePaperSize, double>) null;
    }
    GC.SuppressFinalize((object) this);
  }

  protected enum THeaderSide
  {
    Left,
    Center,
    Right,
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
