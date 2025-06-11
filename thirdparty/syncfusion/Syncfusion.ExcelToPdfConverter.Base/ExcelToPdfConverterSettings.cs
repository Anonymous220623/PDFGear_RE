// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.ExcelToPdfConverterSettings
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO;
using System.Drawing;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class ExcelToPdfConverterSettings
{
  private bool _renderBySheet;
  private bool _autoDetectComplexScript;
  private HeaderFooterOption _HFOption;
  private GridLinesDisplayStyle _displayGridLines;
  private bool _embedFonts;
  private bool _enableRTL;
  private bool _exportBookmarks = true;
  private bool _exportDocumentProperties = true;
  private bool _exportQulaityImage;
  private LayoutOptions _layoutOptions = LayoutOptions.Automatic;
  private bool _needBlankSheet = true;
  private bool _convertBlankPage = true;
  private PdfDocument _pdfDocument;
  private bool _throwWhenExcelFileIsEmpty;
  private SizeF _customPaperSize;
  private PdfConformanceLevel _pdfConformanceLevel;
  private IWarning m_warning;
  private bool _enableFormFields;

  public ExcelToPdfConverterSettings() => this.LayoutOptions = LayoutOptions.Automatic;

  public bool RenderBySheet
  {
    get => this._renderBySheet;
    set => this._renderBySheet = value;
  }

  public IWarning Warning
  {
    get => this.m_warning;
    set => this.m_warning = value;
  }

  public bool AutoDetectComplexScript
  {
    get => this._autoDetectComplexScript;
    set => this._autoDetectComplexScript = value;
  }

  public PdfDocument TemplateDocument
  {
    get
    {
      if (this._pdfDocument == null)
        this._pdfDocument = this._pdfConformanceLevel != PdfConformanceLevel.None ? new PdfDocument(this._pdfConformanceLevel) : new PdfDocument();
      return this._pdfDocument;
    }
    set => this._pdfDocument = value;
  }

  public GridLinesDisplayStyle DisplayGridLines
  {
    get => this._displayGridLines;
    set => this._displayGridLines = value;
  }

  public bool EmbedFonts
  {
    get => this._embedFonts;
    set => this._embedFonts = value;
  }

  public bool ExportBookmarks
  {
    get => this._exportBookmarks;
    set => this._exportBookmarks = value;
  }

  public bool ExportDocumentProperties
  {
    get => this._exportDocumentProperties;
    set => this._exportDocumentProperties = value;
  }

  public HeaderFooterOption HeaderFooterOption
  {
    get
    {
      if (this._HFOption == null)
        this._HFOption = new HeaderFooterOption();
      return this._HFOption;
    }
  }

  public LayoutOptions LayoutOptions
  {
    get => this._layoutOptions;
    set => this._layoutOptions = value;
  }

  internal bool EnableRTL
  {
    get => this._enableRTL;
    set => this._enableRTL = value;
  }

  public bool ThrowWhenExcelFileIsEmpty
  {
    get => this._throwWhenExcelFileIsEmpty;
    set => this._throwWhenExcelFileIsEmpty = value;
  }

  public bool ExportQualityImage
  {
    get => this._exportQulaityImage;
    set => this._exportQulaityImage = value;
  }

  public bool IsConvertBlankSheet
  {
    get => this._needBlankSheet;
    set => this._needBlankSheet = value;
  }

  public bool IsConvertBlankPage
  {
    get => this._convertBlankPage;
    set => this._convertBlankPage = value;
  }

  public SizeF CustomPaperSize
  {
    get => this._customPaperSize;
    set => this._customPaperSize = value;
  }

  public PdfConformanceLevel PdfConformanceLevel
  {
    get => this._pdfConformanceLevel;
    set => this._pdfConformanceLevel = value;
  }

  public bool EnableFormFields
  {
    get => this._enableFormFields;
    set => this._enableFormFields = value;
  }

  internal static SizeF GetExcelSheetSize(
    ExcelPaperSize paperSize,
    ExcelToPdfConverterSettings settings)
  {
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    SizeF excelSheetSize = new SizeF();
    if (!settings.CustomPaperSize.IsEmpty)
      return new SizeF(pdfUnitConvertor.ConvertUnits(settings.CustomPaperSize.Width, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(settings.CustomPaperSize.Height, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
    switch (paperSize)
    {
      case ExcelPaperSize.PaperLetter:
      case ExcelPaperSize.PaperLetterSmall:
      case ExcelPaperSize.PaperNote:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperTabloid:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(17f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperLedger:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(17f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperLegal:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(14f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperStatement:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(5.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperExecutive:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(7.25f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(10.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperA3:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(11.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(16.54f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperA4:
      case ExcelPaperSize.PaperA4Small:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.27f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperA5:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(5.83f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(8.27f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperB4:
        excelSheetSize = new SizeF(PdfPageSize.B4.Width, PdfPageSize.B4.Height);
        break;
      case ExcelPaperSize.PaperB5:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(176f, PdfGraphicsUnit.Millimeter, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(250f, PdfGraphicsUnit.Millimeter, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperFolio:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(13f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperQuarto:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.47f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(10.83f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.Paper10x14:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(10f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(14f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.Paper11x17:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(17f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelope9:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(3.87f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(8.87f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelope10:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.12f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(9.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelope11:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(10.37f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelope12:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.75f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelope14:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperCsheet:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(17f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(22f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperDsheet:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(22f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(34f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEsheet:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(34f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(44f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeDL:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.33f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(8.66f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeC5:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(6.38f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(9.02f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeC3:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(12.76f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(18.03f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeC4:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(9.02f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(12.76f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeC6:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.49f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(6.38f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeC65:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.49f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(9.02f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeB4:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(9.84f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(13.9f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeB5:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(6.93f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(9.84f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeB6:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(6.93f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(4.92f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeItaly:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(4.33f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(9.06f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopeMonarch:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(3.87f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(7.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperEnvelopePersonal:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(3.85f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(6.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperFanfoldUS:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(14.87f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperFanfoldStdGerman:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(12f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.PaperFanfoldLegalGerman:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(13f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.ISOB4:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(250f, PdfGraphicsUnit.Millimeter, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(353f, PdfGraphicsUnit.Millimeter, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.JapaneseDoublePostcard:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(7.87f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(5.83f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.StandardPaper9By11:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(9f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.StandardPaper10By11:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(10f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.StandardPaper15By11:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(15f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.InviteEnvelope:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.66f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(8.66f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.LetterExtraPaper9275By12:
      case ExcelPaperSize.LetterExtraTransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(9.275f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(12f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.LegalExtraPaper9275By15:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(9.275f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(15f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.TabloidExtraPaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(11.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(18f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A4ExtraPaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(9.27f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(12.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.LetterTransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.275f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A4TransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.27f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.SuperASuperAA4Paper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.94f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(14.02f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.SuperBSuperBA3Paper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(12.01f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(19.17f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.LetterPlusPaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(12.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A4PlusPaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.27f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(12.99f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A5TransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(5.83f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(8.27f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.JISB5TransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(7.17f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(10.12f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A3ExtraPaper:
      case ExcelPaperSize.A3ExtraTransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(12.68f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(17.52f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A5ExtraPpaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(6.85f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(9.25f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.ISOB5ExtraPaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(7.91f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(10.87f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A2Paper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(16.54f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(23.39f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      case ExcelPaperSize.A3TransversePaper:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(11.69f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(16.54f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
      default:
        excelSheetSize = new SizeF(pdfUnitConvertor.ConvertUnits(8.5f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertUnits(11f, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point));
        break;
    }
    return excelSheetSize;
  }
}
