// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Calculate;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Parsing;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class ExcelToPdfConverter : IDisposable
{
  private const float DefaultMargin = 34f;
  private const float LeftandRightMargin = 20f;
  private const float TopandBottomMargin = 10f;
  private const string NewLineKey = "\n";
  private const string CarriageReturnKey = "\r";
  private const string TabKey = "_x0009_";
  private const string Dollar = "_x0024_";
  private const string ExponentialFormatString = "{0:0.##E+00}";
  private const float ScriptFactor = 1.5f;
  private const string NumberFormat = "General";
  private const string TextNumberFormat = "@";
  private const int ColumnBitsInCellIndex = 32 /*0x20*/;
  private const string SmallFontBoldTagName = "&b";
  private const string SmallFontUnderlineTagName = "&u";
  private const string SmallFontItalicTagName = "&i";
  private const string SmallFontStrikethroughTagName = "&s";
  private const string BigFontBoldTagName = "&B";
  private const string BigFontUnderlineTagName = "&U";
  private const string BigFontItalicTagName = "&I";
  private const string BigFontStrikethroughTagName = "&S";
  private readonly PointF[] _arrowPoints = new PointF[4]
  {
    new PointF(0.0f, 0.0f),
    new PointF(-5f, -10f),
    new PointF(5f, -10f),
    new PointF(0.0f, 0.0f)
  };
  private readonly PointF[] _arrowOpenPoints = new PointF[3]
  {
    new PointF(-5f, -11f),
    new PointF(0.0f, -2f),
    new PointF(5f, -11f)
  };
  private readonly PointF[] _arrowStealthPoints = new PointF[5]
  {
    new PointF(0.0f, -6f),
    new PointF(5f, -10f),
    new PointF(0.0f, 0.0f),
    new PointF(-5f, -10f),
    new PointF(0.0f, -6f)
  };
  private readonly PointF[] _arrowDiamondPoints = new PointF[5]
  {
    new PointF(-5f, 0.0f),
    new PointF(0.0f, -5f),
    new PointF(5f, 0.0f),
    new PointF(0.0f, 5f),
    new PointF(-5f, 0.0f)
  };
  private bool _isHeaderPageCount;
  private bool _isFooterPageCount;
  private Dictionary<int, HeaderFooterImpl> _headerFooterImplCollection = new Dictionary<int, HeaderFooterImpl>();
  private int _sectionCount;
  private bool _isCancelled;
  private CancellationTokenSource _cancellationTokenSource;
  private readonly Rectangle _coordinates2007 = new Rectangle(0, 1, 2076450, 1557338);
  private CFApplier _conditionalFormatApplier;
  private PdfDocument _pdfDocument;
  private PdfPage _currentPage;
  private IWorkbook _workBook;
  private IWorksheet _workSheet;
  private ChartImpl _chartImpl;
  private WorkbookImpl _workBookImpl;
  private PdfUnitConvertor _pdfUnitConverter;
  private ExcelToPdfConverterSettings _excelToPdfSettings;
  private PdfTemplate _pdfPageTemplate;
  private PdfGraphics _pdfGraphics;
  private ExcelEngine _excelEngine;
  private PdfSection _pdfSection;
  private List<PdfTemplate> _pdfTemplateCollection;
  private Dictionary<string, string> _predefinedHeaderFooter = new Dictionary<string, string>();
  private Dictionary<long, Dictionary<ExcelBordersIndex, Color>> _tableBorderColorList;
  private Dictionary<long, Dictionary<ExcelBordersIndex, Color>> _tableBorderList = new Dictionary<long, Dictionary<ExcelBordersIndex, Color>>();
  private Dictionary<IRange, Color> _fontTableColors;
  private Dictionary<int, IBorders> _extBorders = new Dictionary<int, IBorders>();
  private List<HeaderFooter> _headerFooter;
  private float _borderWidth;
  private int _pageCount = 1;
  private bool _bookmark;
  private float _scaledPageWidth;
  private float _scaledPageHeight;
  private List<string> _fontList = new List<string>();
  private readonly List<string> _excludefontList = new List<string>();
  private readonly string[] _latinfontList = new string[6]
  {
    "Arial",
    "Microsoft Sans Serif",
    "Segoe UI",
    "Tahoma",
    "Times New Roman",
    "Courier New"
  };
  private TableStyleRenderer _tableStyle;
  private string _cellDisplayText;
  private SortedList<long, ExtendedFormatImpl> _sortedListCf = new SortedList<long, ExtendedFormatImpl>();
  private Dictionary<Font, PdfFont> _fontCollection = new Dictionary<Font, PdfFont>();
  private Dictionary<Font, PdfFont> _fontUnicodeCollection = new Dictionary<Font, PdfFont>();
  private Dictionary<string, PdfFont> _alternateFontCollection = new Dictionary<string, PdfFont>();
  private float _sheetHeight;
  private float _sheetWidth;
  private List<SplitText> _splitTextCollection = new List<SplitText>();
  private bool _isNewPage;
  private float _adjacentRectWidth;
  private PdfStringFormat _maxRowPdfTextformat;
  private PdfFont _maxPdfFont;
  private int[] _unicodeChar = new int[2]{ 34, 183 };
  private char[] _numberFormatChar = new char[1]{ '€' };
  private float _headerMargin;
  private float _footerMargin;
  private float _topMargin;
  private float _bottomMargin;
  private float _leftMargin;
  private float _rightMargin;
  private PdfTemplate _footerTemplate;
  private RectangleF _footerBounds;
  private List<int> _stylewithoutBorderFill;
  private PageSetupOption _pageSetupOption;
  private ExcelToPdfLayoutSetting _excelToPdfPageLayout;
  private bool _isPrintTitleRowPage;
  private bool _isPrintTitleColumnPage;
  private ItemSizeHelper _columnWidthGetter;
  private ItemSizeHelper _rowHeightGetter;
  private IPageSetup _pageSetup;
  private bool _hasPrintTitleColumn;
  private IRange _sheetUsedRange;
  private List<char> _removableCharaters = new List<char>();
  private bool _hasContent = true;
  private PivotTableImpl _pivotImpl;
  private IRange _pivotTableRange;
  private Dictionary<int, IRange> _pivotTableList = new Dictionary<int, IRange>();
  private Dictionary<ExcelBordersIndex, double> _sortedBorders = new Dictionary<ExcelBordersIndex, double>();
  private Dictionary<ExcelLineStyle, List<ExcelBordersIndex>> _dicBorderLineStyle = new Dictionary<ExcelLineStyle, List<ExcelBordersIndex>>();
  private HelperMethods _helper;
  private bool _allowHeaderFooterOnce = true;
  private bool _allowHeader;
  private bool _allowFooter;
  private int _chartIndex;
  private int _sheetIndex;
  private float _startX;
  private float _headerHeight;
  private IRange _borderRange;
  private readonly Dictionary<IChart, MemoryStream> _chartImageCollection = new Dictionary<IChart, MemoryStream>(1);
  private Dictionary<long, MergedCellInfo> _mergedRegions;
  private double _maxRowFontSize = 1.0;
  private float _rightWidth;
  private float _leftWidth;
  private List<MergeCellsRecord.MergedRegion> _lstRegions;
  private IRange _currentCell;
  private RectangleF _currentCellRect;
  private Dictionary<long, RectangleF> _commentCellPosition = new Dictionary<long, RectangleF>();
  private PdfStringFormat _pdfTextformat;
  private RectangleF _hfImageBounds = RectangleF.Empty;
  private bool _isHfrtfProcess;
  private bool _isfontColorModified;
  private float _scaledCellWidth = 1f;
  private float _scaledCellHeight = 1f;
  private readonly Dictionary<Rectangle, PdfTemplate> _printTitleTemplateCollection = new Dictionary<Rectangle, PdfTemplate>();
  private bool _drawprinttitle;
  private PdfTemplate _printTitleTemplate;
  private bool _isFirstRowOfPage = true;
  private bool isFirstColumnOfPage = true;
  private bool isCurrentCellNotMerged = true;
  private float _centerHorizontalValue;
  private float _centerVerticalValue;
  private PdfLoadedDocument _loadDoc;
  private int _startPageIndex;
  private int _endPageIndex;
  private bool _isConverted;
  private List<string> _systemFontNameCollection;
  private IStyle style;

  internal List<string> SystemFontsName
  {
    get
    {
      if (this._systemFontNameCollection == null)
        this._systemFontNameCollection = new List<string>();
      return this._systemFontNameCollection;
    }
  }

  internal IWarning Warning
  {
    get => this._excelToPdfSettings != null ? this._excelToPdfSettings.Warning : (IWarning) null;
  }

  internal bool IsHFRTFProcess
  {
    get => this._isHfrtfProcess;
    set => this._isHfrtfProcess = value;
  }

  internal float LeftWidth => this._leftWidth;

  internal float RightWidth => this._rightWidth;

  internal ExcelToPdfConverterSettings ExcelToPdfSettings => this._excelToPdfSettings;

  internal RectangleF HfImageBounds
  {
    get => this._hfImageBounds;
    set => this._hfImageBounds = value;
  }

  public ExcelToPdfConverter()
  {
    this._conditionalFormatApplier = new CFApplier();
    this._excelToPdfSettings = new ExcelToPdfConverterSettings();
    this._pdfUnitConverter = new PdfUnitConvertor();
    this._pdfTemplateCollection = new List<PdfTemplate>();
    this.IntializeFonts();
    this.SystemFonts();
    this.IntializeRemovableCharacters();
  }

  public ExcelToPdfConverter(IWorkbook workbook)
    : this()
  {
    this._workBook = workbook;
  }

  public ExcelToPdfConverter(IWorksheet worksheet)
    : this()
  {
    this._workSheet = worksheet;
    this._workBookImpl = (WorkbookImpl) worksheet.Application.ActiveWorkbook;
  }

  public ExcelToPdfConverter(IChart chart)
    : this()
  {
    this._chartImpl = chart is ChartImpl ? chart as ChartImpl : (chart as ChartShapeImpl).ChartObject;
  }

  public ExcelToPdfConverter(string filePath)
    : this()
  {
    IFormatProvider currentCulture = (IFormatProvider) CultureInfo.CurrentCulture;
    if (!File.Exists(filePath))
      throw new FileNotFoundException(nameof (filePath));
    this._excelEngine = new ExcelEngine();
    IApplication excel = this._excelEngine.Excel;
    if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
    {
      excel.DefaultVersion = ExcelVersion.Excel2007;
      this._workBook = excel.Workbooks.Open(filePath, ExcelOpenType.Automatic);
    }
    else
    {
      if (!filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
        throw new NotSupportedException(string.Format(currentCulture, "The current file format is not supported :{0}", (object) Path.GetExtension(filePath)));
      excel.DefaultVersion = ExcelVersion.Excel97to2003;
      this._workBook = excel.Workbooks.Open(filePath, ExcelOpenType.Automatic);
    }
  }

  public ExcelToPdfConverter(Stream stream)
    : this()
  {
    if (!stream.CanSeek || !stream.CanRead)
      throw new ArgumentException("The stream cannot be read", nameof (stream));
    this._excelEngine = new ExcelEngine();
    this._workBook = this._excelEngine.Excel.Workbooks.Open(stream, ExcelOpenType.Automatic);
  }

  public static LayoutOptions GetLayoutOptions(PageSetupBaseImpl pageSetup)
  {
    if (!pageSetup.IsFitToPage)
      return pageSetup.Zoom != 100 ? LayoutOptions.CustomScaling : LayoutOptions.NoScaling;
    if (pageSetup.FitToPagesWide == 1 && pageSetup.FitToPagesTall == 1)
      return LayoutOptions.FitSheetOnOnePage;
    if (pageSetup.FitToPagesWide == 1 && pageSetup.FitToPagesTall == 0)
      return LayoutOptions.FitAllColumnsOnOnePage;
    return pageSetup.FitToPagesWide == 0 && pageSetup.FitToPagesTall == 1 ? LayoutOptions.FitAllRowsOnOnePage : LayoutOptions.CustomScaling;
  }

  public event CurrentProgressChangedEventHandler CurrentProgressChanged;

  public event SheetBeforeDrawnEventHandler SheetBeforeDrawn;

  public event SheetAfterDrawnEventHandler SheetAfterDrawn;

  internal void RaiseWarning(string description, WarningType type)
  {
    if (this.Warning == null)
      return;
    this.Warning.ShowWarning(new WarningInfo()
    {
      Type = type,
      Description = description
    });
    if (!this.Warning.Cancel || this._cancellationTokenSource == null)
      return;
    this._isCancelled = true;
    this._cancellationTokenSource.Cancel();
    this._cancellationTokenSource.Token.ThrowIfCancellationRequested();
  }

  public PdfDocument Convert()
  {
    if (this.Warning != null)
    {
      this._isCancelled = false;
      try
      {
        this._cancellationTokenSource = new CancellationTokenSource();
        Task.Factory.StartNew(new Action(this.ConvertDocument), this._cancellationTokenSource.Token).Wait();
      }
      catch (Exception ex)
      {
      }
      if (this._isCancelled)
        return (PdfDocument) null;
    }
    else
      this.ConvertDocument();
    return this._pdfDocument;
  }

  internal void ConvertDocument()
  {
    this._sheetIndex = 0;
    PdfDocument.EnableCache = false;
    LayoutOptions layoutOptions = this._excelToPdfSettings.LayoutOptions;
    if (this._pdfDocument == null)
      this._pdfDocument = this._excelToPdfSettings.PdfConformanceLevel == PdfConformanceLevel.None ? new PdfDocument() : new PdfDocument(this._excelToPdfSettings.PdfConformanceLevel);
    bool flag = false;
    if (this._workBook != null)
    {
      this._pdfDocument.EnableMemoryOptimization = true;
      this._workBookImpl = (WorkbookImpl) this._workBook;
      if (this.Warning != null)
        this._workBookImpl.WarningCallBack += new WorkbookImpl.WarningEventHandler(this.RaiseWarning);
      this._workBookImpl.IsConverting = true;
      this.UpdateInvisibleStyleCollection();
      int count = this._workBook.Worksheets.Count;
      int num = count + this._workBook.Charts.Count;
      for (int sheetsCount = 0; sheetsCount < num; ++sheetsCount)
      {
        bool isCellModified = this._workBookImpl.IsCellModified;
        if (this._sheetIndex < count && sheetsCount == this._workBook.Worksheets[this._sheetIndex].TabIndex)
        {
          this._workSheet = this._workBook.Worksheets[this._sheetIndex];
          if (this._workBook.BuiltInDocumentProperties[ExcelBuiltInProperty.ApplicationName].Text == "Essential XlsIO" || (this._workBook as WorkbookImpl).IsCellModified)
          {
            if (this._workSheet.CalcEngine == null)
            {
              this._workSheet.EnableSheetCalculations();
              flag = true;
            }
            this._workSheet.CalcEngine.MaximumRecursiveCalls = 10000;
            this._workSheet.CalcEngine.IterationMaxCount = 10000;
            this._workSheet.CalcEngine.ThrowCircularException = true;
            CalcEngine.MaxStackDepth = 10000;
            this._workBookImpl.CalcEngineMemberValuesOnSheet(false);
            this._workSheet.CalcEngine.UseDatesInCalculations = true;
            this._workSheet.CalcEngine.UseNoAmpersandQuotes = true;
          }
          this.DrawWorkSheet(this._workSheet, count, layoutOptions);
          this.DrawCommentsInSheetEnd();
          this._commentCellPosition.Clear();
          ++this._sheetIndex;
        }
        else if (this._chartIndex < this._workBook.Charts.Count)
        {
          IChart chart = this._workBook.Charts[this._chartIndex];
          if ((this._workBook.Application as ApplicationImpl).ChartToImageConverter != null)
            this.DrawChartSheet(chart, sheetsCount, LayoutOptions.Automatic);
          ++this._chartIndex;
        }
        this._workBookImpl.IsCellModified = isCellModified;
        this._workBookImpl.CalcEngineMemberValuesOnSheet(true);
      }
    }
    else if (this._workSheet != null)
    {
      this._workBook = this._workSheet.Workbook;
      this._workBookImpl = (WorkbookImpl) this._workBook;
      if (this.Warning != null)
        this._workBookImpl.WarningCallBack += new WorkbookImpl.WarningEventHandler(this.RaiseWarning);
      bool isCellModified = this._workBookImpl.IsCellModified;
      (this._workBook as WorkbookImpl).IsConverting = true;
      this.UpdateInvisibleStyleCollection();
      if (this._workBook.BuiltInDocumentProperties[ExcelBuiltInProperty.ApplicationName].Text == "Essential XlsIO" || (this._workBook as WorkbookImpl).IsCellModified)
      {
        if (this._workSheet.CalcEngine == null)
        {
          this._workSheet.EnableSheetCalculations();
          flag = true;
        }
        this._workBookImpl.CalcEngineMemberValuesOnSheet(false);
        this._workSheet.CalcEngine.MaximumRecursiveCalls = 10000;
        this._workSheet.CalcEngine.IterationMaxCount = 10000;
        this._workSheet.CalcEngine.ThrowCircularException = true;
        CalcEngine.MaxStackDepth = 10000;
        this._workSheet.CalcEngine.UseDatesInCalculations = true;
        this._workSheet.CalcEngine.UseNoAmpersandQuotes = true;
      }
      this.DrawWorkSheet(this._workSheet, 1, layoutOptions);
      this._workBookImpl.IsCellModified = isCellModified;
      this._workBookImpl.CalcEngineMemberValuesOnSheet(true);
      this.DrawCommentsInSheetEnd();
      this._commentCellPosition.Clear();
    }
    else if (this._chartImpl != null)
    {
      this._workBook = (IWorkbook) this._chartImpl.InnerWorkbook;
      this._workBookImpl = (WorkbookImpl) this._workBook;
      if (this.Warning != null)
        this._workBookImpl.WarningCallBack += new WorkbookImpl.WarningEventHandler(this.RaiseWarning);
      if ((this._chartImpl.Application as ApplicationImpl).ChartToImageConverter != null)
      {
        this._workBookImpl.CalcEngineMemberValuesOnSheet(false);
        this.DrawChartSheet((IChart) this._chartImpl, 0, LayoutOptions.Automatic);
        this._workBookImpl.CalcEngineMemberValuesOnSheet(true);
      }
    }
    this._pageCount = 1;
    if (this._headerFooterImplCollection.Count > 0)
    {
      for (int index1 = 0; index1 < this._pdfDocument.Sections.Count; ++index1)
      {
        HeaderFooterImpl headerFooterImpl;
        if (this._headerFooterImplCollection.TryGetValue(index1, out headerFooterImpl))
        {
          if (!headerFooterImpl.PageSetup.AutoFirstPageNumber)
            this._pageCount = (int) headerFooterImpl.PageSetup.FirstPageNumber;
          for (int index2 = 0; index2 < this._pdfDocument.Sections[index1].Pages.Count; ++index2)
          {
            float headerHeight = 0.0f;
            float footerHeight = 0.0f;
            this.DrawPdfPageHeaderFooter(this._pdfDocument.Sections[index1], headerFooterImpl.HeaderFooterCollection, (IPageSetupBase) headerFooterImpl.PageSetup, headerFooterImpl.TopMargin, headerFooterImpl.BottomMargin, this._pdfDocument.Sections[index1].Pages[index2], out headerHeight, out footerHeight, headerFooterImpl.LayoutOptions, headerFooterImpl.IsHeader, headerFooterImpl.IsFooter, headerFooterImpl.PdfPageTemplateCollection[index2], true, headerFooterImpl.FooterMargin, headerFooterImpl.HeaderMargin);
            ++this._pageCount;
          }
        }
        else
        {
          for (int index3 = 0; index3 < this._pdfDocument.Sections[index1].Pages.Count; ++index3)
            ++this._pageCount;
        }
      }
    }
    if (this._excelToPdfSettings.ExportDocumentProperties)
      this.SetDocumentProperties();
    if (this._stylewithoutBorderFill != null)
    {
      this._stylewithoutBorderFill.Clear();
      this._stylewithoutBorderFill = (List<int>) null;
    }
    this._extBorders.Clear();
    (this._workBook as WorkbookImpl).IsConverting = false;
    this._isConverted = true;
    if (!flag)
      return;
    if (this._workSheet != null)
    {
      this._workSheet.DisableSheetCalculations();
    }
    else
    {
      if (this._workBook == null || this._workBook.Worksheets.Count <= 0)
        return;
      this._workBook.Worksheets[0].DisableSheetCalculations();
    }
  }

  private void DrawCommentsInSheetEnd()
  {
    if (this._workSheet.PageSetup.PrintComments != ExcelPrintLocation.PrintSheetEnd)
      return;
    GridLinesDisplayStyle displayGridLines = this._excelToPdfSettings.DisplayGridLines;
    this._excelToPdfSettings.DisplayGridLines = GridLinesDisplayStyle.Invisible;
    LayoutOptions layoutOptions = this._excelToPdfSettings.LayoutOptions;
    int count = this._workBook.Worksheets.Count;
    IWorksheet workSheet = this._workSheet;
    IWorksheet worksheet = this._workBook.Worksheets.Create("TempSheet");
    this._workSheet = worksheet;
    int index1 = worksheet.Index;
    int num = 1;
    IComment[] commentArray = new IComment[workSheet.Comments.Count];
    for (int Index = 0; Index < commentArray.Length; ++Index)
      commentArray[Index] = (IComment) workSheet.Comments[Index];
    for (int index2 = 0; index2 < commentArray.Length; ++index2)
    {
      for (int index3 = index2 + 1; index3 < commentArray.Length; ++index3)
      {
        if (RangeImpl.GetCellIndex(commentArray[index2].Column, commentArray[index2].Row) > RangeImpl.GetCellIndex(commentArray[index3].Column, commentArray[index3].Row))
        {
          IComment comment = commentArray[index2];
          commentArray[index2] = commentArray[index3];
          commentArray[index3] = comment;
        }
      }
    }
    for (int index4 = 0; index4 < workSheet.Comments.Count; ++index4)
    {
      string name1 = "A" + (object) num;
      string name2 = "B" + (object) num;
      this._workSheet.Range[name1].Text = " Cell: \nNote: ";
      this._workSheet.Range[name2].Text = $"{RangeImpl.GetCellName(commentArray[index4].Column, commentArray[index4].Row)}\n{commentArray[index4].Text}\n";
      this._workSheet.Range[name1].AutofitColumns();
      this._workSheet.Range[name2].ColumnWidth = 70.0;
      this._workSheet.UsedRange.AutofitRows();
      this._workSheet.Range[name1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
      this._workSheet.Range[name2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
      this._workSheet.UsedRange.CellStyle.VerticalAlignment = ExcelVAlign.VAlignTop;
      this._workSheet.Range[name1].CellStyle.Font.Bold = true;
      this._workSheet.IsGridLinesVisible = false;
      ++num;
    }
    this.DrawWorkSheet(this._workSheet, count, layoutOptions);
    this._workSheet = workSheet;
    this._workBook.Worksheets.Remove(index1);
    this._excelToPdfSettings.DisplayGridLines = displayGridLines;
  }

  public void Print() => this.Print(new PrinterSettings());

  public void Print(PrinterSettings printerSettings)
  {
    this.PrintDocument(this._isConverted ? this._pdfDocument : this.Convert(), printerSettings);
  }

  public void Print(ExcelToPdfConverterSettings converterSettings)
  {
    this.Print(new PrinterSettings(), converterSettings);
  }

  public void Print(PrinterSettings printerSettings, ExcelToPdfConverterSettings converterSettings)
  {
    this.PrintDocument(this._isConverted ? this._pdfDocument : this.Convert(converterSettings), printerSettings);
  }

  private void PrintDocument(PdfDocument pdfDocument, PrinterSettings printerSettings)
  {
    MemoryStream file = new MemoryStream();
    pdfDocument.Save((Stream) file);
    file.Position = 0L;
    this._loadDoc = new PdfLoadedDocument((Stream) file);
    this._startPageIndex = 1;
    this._endPageIndex = this._loadDoc.Pages.Count;
    System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
    printDocument.PrinterSettings = printerSettings;
    if (printDocument.PrinterSettings.FromPage == 0)
      printDocument.PrinterSettings.FromPage = this._startPageIndex;
    if (printDocument.PrinterSettings.ToPage == 0)
      printDocument.PrinterSettings.ToPage = this._endPageIndex;
    if (printDocument.PrinterSettings.FromPage <= 0 || printDocument.PrinterSettings.ToPage > this._loadDoc.Pages.Count)
      return;
    this._startPageIndex = printDocument.PrinterSettings.FromPage - 1;
    this._endPageIndex = printDocument.PrinterSettings.ToPage;
    printDocument.PrintPage += new PrintPageEventHandler(this.PrintPage);
    printDocument.Print();
  }

  private void PrintPage(object sender, PrintPageEventArgs e)
  {
    if (this._loadDoc.Pages[this._startPageIndex] != null)
    {
      int width = (int) e.Graphics.VisibleClipBounds.Width;
      int height = (int) e.Graphics.VisibleClipBounds.Height;
      Image image = (Image) this._loadDoc.ExportAsMetafile(this._startPageIndex);
      e.Graphics.DrawImage(image, new Rectangle(0, 0, width, height));
      image.Dispose();
    }
    ++this._startPageIndex;
    if (this._startPageIndex < this._endPageIndex)
    {
      e.HasMorePages = true;
    }
    else
    {
      this._startPageIndex = 0;
      e.HasMorePages = false;
    }
  }

  private void UpdateInvisibleStyleCollection()
  {
    this._stylewithoutBorderFill = new List<int>();
    (this._workBook as WorkbookImpl).m_bisUnusedXFRemoved = true;
    ExtendedFormatsCollection innerExtFormats = (this._workBook as WorkbookImpl).InnerExtFormats;
    for (int index = 0; index < innerExtFormats.Count; ++index)
    {
      ExtendedFormatImpl extendedFormatImpl = innerExtFormats[index];
      IBorders borders = extendedFormatImpl.Borders;
      if (!this._extBorders.ContainsKey(extendedFormatImpl.Index))
        this._extBorders.Add(extendedFormatImpl.Index, borders);
      if (!this.CheckCellBorderStyle(borders) && !extendedFormatImpl.HasBorder && (extendedFormatImpl.FillPattern == ExcelPattern.None || extendedFormatImpl.ColorIndex == ExcelKnownColors.None) && (extendedFormatImpl.Color.ToArgb() == -1 || !(extendedFormatImpl.Color.Name != "ffffffff") || !(extendedFormatImpl.Color.Name != "ffffff")))
        this._stylewithoutBorderFill.Add(extendedFormatImpl.Index);
    }
  }

  private void InitializaSheetSettings()
  {
    this._isNewPage = false;
    this.SplitTexts.Clear();
    this._adjacentRectWidth = 0.0f;
    this._columnWidthGetter = (ItemSizeHelper) null;
    this._rowHeightGetter = (ItemSizeHelper) null;
  }

  internal void DrawWorkSheet(IWorksheet worksheet, int sheetsCount, LayoutOptions layoutOptions)
  {
    if (this._workSheet.PivotTables.Count > 0)
    {
      for (int index = 0; index < this._workSheet.PivotTables.Count; ++index)
      {
        IPivotTable pivotTable = this._workSheet.PivotTables[index];
        if ((pivotTable as PivotTableImpl).CustomStyleName != null)
          this.RaiseWarning($"Custom style in the pivot Table : \"{pivotTable.Name}\" is not supported.", WarningType.PivotTableSettings);
        if (((PivotTableImpl) pivotTable).Cache != null && ((PivotTableImpl) pivotTable).Cache.SourceRange != null && (this._workBook.BuiltInDocumentProperties[ExcelBuiltInProperty.ApplicationName].Text == "Essential XlsIO" || (this._workBook as WorkbookImpl).IsCellModified || pivotTable.Options.RowLayout != PivotTableRowLayout.Tabular || (pivotTable as PivotTableImpl).CustomStyleName == null) && (((PivotTableImpl) pivotTable).Cache.IsRefreshOnLoad || pivotTable.Options.RowLayout != PivotTableRowLayout.Tabular))
          pivotTable.Layout();
        this._pivotImpl = pivotTable as PivotTableImpl;
        this._pivotTableRange = pivotTable.Location;
      }
    }
    else
    {
      this._pivotImpl = (PivotTableImpl) null;
      this._pivotTableRange = (IRange) null;
    }
    this._sheetUsedRange = this._workSheet.PageSetup.PrintArea == null || this._workSheet.PageSetup.PrintArea.Contains("#REF") ? this.GetActualUsedRange(this._workSheet, this._workSheet.UsedRange, true, true) : this._workSheet.UsedRange;
    this._mergedRegions = (worksheet as WorksheetImpl).BuildMergedRegions();
    this._pageSetupOption = new PageSetupOption((WorksheetImpl) this._workSheet, this._sheetUsedRange, this);
    this._helper = new HelperMethods(this._pageSetupOption);
    this.InitializaSheetSettings();
    if (!this.RaiseSheetBeforeDrawn(this._workSheet.Index))
    {
      if (this._excelToPdfSettings.ExportBookmarks || this._bookmark)
        this._excelToPdfSettings.ExportBookmarks = true;
      WorksheetImpl workSheet = (WorksheetImpl) this._workSheet;
      this.OnProgressChanged(sheetsCount, this._workSheet.Index);
      if ((!workSheet.IsEmpty && (worksheet.Rows.Length > 0 || worksheet.Columns.Length > 0) || workSheet.Shapes.Count > 0) && workSheet.Visibility == WorksheetVisibility.Visible)
      {
        this._hasContent = false;
        this._pdfPageTemplate = (PdfTemplate) null;
        this._pdfTemplateCollection = new List<PdfTemplate>();
        this._headerFooter = new List<HeaderFooter>();
        this._excelToPdfSettings.EnableRTL = this._workSheet.IsRightToLeft;
        if (this._workSheet.ListObjects.Count != 0)
        {
          this._tableStyle = new TableStyleRenderer(this._workSheet.ListObjects);
          this._tableBorderColorList = this._tableStyle.ApplyStyles(this._workSheet, out this._fontTableColors);
        }
        if (layoutOptions == LayoutOptions.Automatic)
          this._excelToPdfSettings.LayoutOptions = Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.GetLayoutOptions(this._workSheet.PageSetup as PageSetupBaseImpl);
        PageSetupOption pageSetupOption = this._pageSetupOption;
        int layoutOptions1 = (int) this._excelToPdfSettings.LayoutOptions;
        IRange[] actualUsedRange;
        if (!this._pageSetupOption.HasPrintArea)
          actualUsedRange = new IRange[1]
          {
            this._sheetUsedRange
          };
        else
          actualUsedRange = this._pageSetupOption.PrintAreas;
        this._pdfDocument = this.DrawSheet(this._workSheet, pageSetupOption.GetBreakRanges((LayoutOptions) layoutOptions1, actualUsedRange));
        this._sortedListCf.Clear();
      }
      else if (this._excelToPdfSettings.ThrowWhenExcelFileIsEmpty && workSheet.Visibility != WorksheetVisibility.Hidden && this._hasContent)
        throw new ExcelToPDFConverterException("The Empty Excel Document cannot be converted to an PDF document. You can disable this exception by using Boolean ThrowWhenExcelFileIsEmpty property to True or False.");
      this.OnSheetAfterDrawn(this._workSheet.Index);
    }
    this._mergedRegions.Clear();
    this._mergedRegions = (Dictionary<long, MergedCellInfo>) null;
  }

  internal PdfDocument DrawChartSheet(IChart chart, int sheetsCount, LayoutOptions layoutOptions)
  {
    this._pageSetupOption = new PageSetupOption();
    this._helper = new HelperMethods(this._pageSetupOption);
    this.InitializaSheetSettings();
    this._hasContent = false;
    this._pdfPageTemplate = (PdfTemplate) null;
    this._pdfTemplateCollection = new List<PdfTemplate>();
    if (this._headerFooter == null)
      this._headerFooter = new List<HeaderFooter>();
    ChartPageSetupImpl pageSetup = chart.PageSetup as ChartPageSetupImpl;
    this._pageCount = this._sheetIndex + this._chartIndex + 1;
    this._pdfTemplateCollection.Clear();
    this._leftMargin = this.Pdf_UnitConverter.ConvertUnits((float) pageSetup.LeftMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._rightMargin = this.Pdf_UnitConverter.ConvertUnits((float) pageSetup.RightMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._headerMargin = this.Pdf_UnitConverter.ConvertUnits((float) pageSetup.HeaderMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._topMargin = this.Pdf_UnitConverter.ConvertUnits((float) pageSetup.TopMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._bottomMargin = this.Pdf_UnitConverter.ConvertUnits((float) pageSetup.BottomMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._footerMargin = this.Pdf_UnitConverter.ConvertUnits((float) pageSetup.FooterMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._pdfSection = this._pdfDocument.Sections.Add();
    this._sectionCount = this._pdfDocument.Sections.Count - 1;
    this._pdfSection.PageSettings.Margins.Left = this._leftMargin;
    this._pdfSection.PageSettings.Margins.Right = this._rightMargin;
    this._pdfSection.PageSettings.Margins.Top = this._topMargin;
    this._pdfSection.PageSettings.Margins.Bottom = this._bottomMargin;
    this._pdfSection.PageSettings.Size = ExcelToPdfConverterSettings.GetExcelSheetSize(pageSetup.PaperSize, this.ExcelToPdfSettings);
    this._pdfSection.PageSettings.Orientation = (PdfPageOrientation) Enum.Parse(typeof (PdfPageOrientation), pageSetup.Orientation.ToString(), true);
    this._pdfPageTemplate = new PdfTemplate(this._pdfSection.PageSettings.Size);
    if (chart.Parent is ChartShapeImpl)
    {
      ChartShapeImpl parent = chart.Parent as ChartShapeImpl;
      chart.Width = (double) parent.Width;
      chart.Height = (double) parent.Height;
      if (chart.Width == 0.0 || chart.Height == 0.0)
        return this._pdfDocument;
      this._pdfPageTemplate = new PdfTemplate(new SizeF((float) chart.Width, (float) chart.Height));
    }
    else if (chart.Width == 0.0 && chart.Height == 0.0)
    {
      chart.Width = (double) this._pdfPageTemplate.Width;
      chart.Height = (double) this._pdfPageTemplate.Height;
    }
    this._pdfTemplateCollection.Add(this._pdfPageTemplate);
    if (!string.IsNullOrEmpty(pageSetup.FullHeaderString) || !string.IsNullOrEmpty(pageSetup.FullFooterString))
      this._headerFooter = this.DrawHeadersAndFooters(this._pdfSection, (WorksheetImpl) null, chart as ChartImpl);
    this.InitializePdfPage();
    this.DrawChart(chart, this._pdfPageTemplate);
    if ((chart.PageSetup as PageSetupBaseImpl).FullHeaderString != string.Empty)
      this._pdfSection.PageSettings.Margins.Top = 0.0f;
    if ((chart.PageSetup as PageSetupBaseImpl).FullHeaderString != string.Empty)
      this._pdfSection.PageSettings.Margins.Bottom = 0.0f;
    float headerHeight = 0.0f;
    float footerHeight = 0.0f;
    if (this._headerFooter.Count != 0)
    {
      if (!this._excelToPdfSettings.RenderBySheet && (this._isHeaderPageCount || this._isFooterPageCount) && this._pdfDocument.Sections.Count > this._sectionCount)
        this.AddHeaderFooterImpl(this._headerFooter, (WorksheetImpl) null, chart as ChartImpl, this._pdfPageTemplate);
      if (this._headerFooter.Count != 0)
        this.DrawPdfPageHeaderFooter(this._pdfSection, this._headerFooter, (IPageSetupBase) (chart as ChartImpl).PageSetupBase, this._topMargin, this._bottomMargin, this._pdfSection.Pages[0], out headerHeight, out footerHeight, this._excelToPdfSettings.LayoutOptions, this._excelToPdfSettings.HeaderFooterOption.ShowFooter, this._excelToPdfSettings.HeaderFooterOption.ShowFooter, this._pdfPageTemplate, false, this._footerMargin, this._headerMargin);
    }
    float y = (double) this._pdfSection.PageSettings.Margins.Top == 0.0 ? this._topMargin : 0.0f;
    if (chart.Parent is ChartShapeImpl)
    {
      SizeF size = this._pdfSection.Pages[0].Size;
      size.Width -= this._leftMargin + this._rightMargin;
      size.Height -= this._topMargin + this._bottomMargin;
      float width = size.Width;
      float height = size.Height;
      if ((double) this._pdfPageTemplate.Width / (double) size.Width >= (double) this._pdfPageTemplate.Height / (double) size.Height)
        size.Height = this._pdfPageTemplate.Height * (size.Width / this._pdfPageTemplate.Width);
      else
        size.Width = this._pdfPageTemplate.Width * (size.Height / this._pdfPageTemplate.Height);
      this._pdfSection.Pages[0].Graphics.DrawPdfTemplate(this._pdfPageTemplate, new PointF((float) (((double) width - (double) size.Width) / 2.0), y + (float) (((double) height - (double) size.Height) / 2.0)), size);
    }
    else
      this._pdfSection.Pages[0].Graphics.DrawPdfTemplate(this._pdfPageTemplate, new PointF(0.0f, y), new SizeF(this._pdfPageTemplate.Width - (this._leftMargin + this._rightMargin), this._pdfPageTemplate.Height - (this._topMargin + this._bottomMargin)));
    if (this._allowHeader && !string.IsNullOrEmpty((chart as ChartImpl).PageSetupBase.FullHeaderString) && this._headerFooter.Count > 0)
    {
      if (this._excelToPdfSettings.HeaderFooterOption.ShowHeader)
        this._pdfSection.Template.Top = this.AddPDFHeaderFooter((IPageSetupBase) (chart as ChartImpl).PageSetupBase, this._pdfSection, true, true);
      else
        this._headerFooter.RemoveAt(0);
    }
    if (this._allowFooter && !string.IsNullOrEmpty((chart as ChartImpl).PageSetupBase.FullFooterString) && this._headerFooter.Count > 0)
    {
      if (this._excelToPdfSettings.HeaderFooterOption.ShowFooter)
        this._pdfSection.Template.Bottom = this.AddPDFHeaderFooter((IPageSetupBase) (chart as ChartImpl).PageSetupBase, this._pdfSection, false, true);
      else
        this._headerFooter.RemoveAt(0);
    }
    if (this._headerFooter.Count > 0)
      this._headerFooter.RemoveRange(0, this._headerFooter.Count);
    this._isHeaderPageCount = false;
    this._isFooterPageCount = false;
    return this._pdfDocument;
  }

  internal void DrawChart(IChart chart, PdfTemplate chartTemplate)
  {
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, chartTemplate.Width, chartTemplate.Height);
    MemoryStream imageAsStream = new MemoryStream();
    chart.SaveAsImage((Stream) imageAsStream);
    if (imageAsStream.Length > 0L)
    {
      PdfImage image = this._pdfGraphics.GetImage((Stream) imageAsStream, this._pdfDocument);
      chartTemplate.Graphics.DrawImage(image, new RectangleF((float) (int) rectangleF.X, (float) (int) rectangleF.Y, (float) (int) rectangleF.Width, (float) (int) rectangleF.Height));
      if (image is PdfBitmap)
        (image as PdfBitmap).Dispose();
    }
    imageAsStream.Flush();
    imageAsStream.Close();
  }

  public PdfDocument Convert(ExcelToPdfConverterSettings converterSettings)
  {
    if (converterSettings.PdfConformanceLevel != PdfConformanceLevel.None)
      converterSettings.EmbedFonts = true;
    this._pdfDocument = converterSettings.TemplateDocument;
    this._excelToPdfSettings = converterSettings;
    return this.Convert();
  }

  internal void OnProgressChanged(int noOfSheets, int activeSheetIndex)
  {
    if (this.CurrentProgressChanged == null)
      return;
    this.CurrentProgressChanged((object) this, new CurrentProgressChangedEventArgs(noOfSheets, activeSheetIndex, (object) this));
  }

  internal void OnSheetBeforeDrawn(SheetBeforeDrawnEventArgs args)
  {
    if (this.SheetBeforeDrawn == null)
      return;
    this.SheetBeforeDrawn((object) this, args);
  }

  internal void OnSheetAfterDrawn(int activeSheetIndex)
  {
    if (this.SheetAfterDrawn == null)
      return;
    this.SheetAfterDrawn((object) this, new SheetAfterDrawnEventArgs(activeSheetIndex, (object) this));
  }

  private bool RaiseSheetBeforeDrawn(int activeSheetIndex)
  {
    if (this.SheetBeforeDrawn == null)
      return false;
    SheetBeforeDrawnEventArgs args = new SheetBeforeDrawnEventArgs(activeSheetIndex, (object) this);
    this.OnSheetBeforeDrawn(args);
    return args.Skip;
  }

  public void Dispose()
  {
    if (this._predefinedHeaderFooter != null)
      this._predefinedHeaderFooter = (Dictionary<string, string>) null;
    if (this._tableBorderColorList != null)
      this._tableBorderColorList = (Dictionary<long, Dictionary<ExcelBordersIndex, Color>>) null;
    if (this._fontTableColors != null)
      this._fontTableColors = (Dictionary<IRange, Color>) null;
    if (this._headerFooter != null)
      this._headerFooter = (List<HeaderFooter>) null;
    if (this._headerFooterImplCollection != null)
    {
      this._headerFooterImplCollection.Clear();
      this._headerFooterImplCollection = (Dictionary<int, HeaderFooterImpl>) null;
    }
    if (this._fontList != null)
      this._fontList = (List<string>) null;
    if (this._tableStyle != null)
      this._tableStyle = (TableStyleRenderer) null;
    if (this._sortedListCf != null)
      this._sortedListCf = (SortedList<long, ExtendedFormatImpl>) null;
    if (this._fontCollection != null)
      this._fontCollection = (Dictionary<Font, PdfFont>) null;
    if (this._fontUnicodeCollection != null)
      this._fontUnicodeCollection = (Dictionary<Font, PdfFont>) null;
    if (this._alternateFontCollection != null)
      this._alternateFontCollection = (Dictionary<string, PdfFont>) null;
    if (this._splitTextCollection != null)
      this._splitTextCollection = (List<SplitText>) null;
    if (this._unicodeChar != null)
      this._unicodeChar = (int[]) null;
    if (this._numberFormatChar != null)
      this._numberFormatChar = (char[]) null;
    if (this._pageSetupOption != null)
      this._pageSetupOption = (PageSetupOption) null;
    if (this._excelToPdfPageLayout != null)
      this._excelToPdfPageLayout = (ExcelToPdfLayoutSetting) null;
    if (this._columnWidthGetter != null)
      this._columnWidthGetter = (ItemSizeHelper) null;
    if (this._rowHeightGetter != null)
      this._rowHeightGetter = (ItemSizeHelper) null;
    if (this._removableCharaters != null)
      this._removableCharaters = (List<char>) null;
    if (this._pivotTableList != null)
      this._pivotTableList = (Dictionary<int, IRange>) null;
    if (this._sortedBorders != null)
      this._sortedBorders = (Dictionary<ExcelBordersIndex, double>) null;
    if (this._dicBorderLineStyle != null)
      this._dicBorderLineStyle = (Dictionary<ExcelLineStyle, List<ExcelBordersIndex>>) null;
    if (this._helper != null)
      this._helper = (HelperMethods) null;
    if (this._conditionalFormatApplier != null)
      this._conditionalFormatApplier = (CFApplier) null;
    if (this._excelToPdfSettings != null)
      this._excelToPdfSettings = (ExcelToPdfConverterSettings) null;
    if (this._workBookImpl != null)
      this._workBookImpl = (WorkbookImpl) null;
    if (this._currentPage != null)
      this._currentPage = (PdfPage) null;
    if (this._pdfUnitConverter != null)
      this._pdfUnitConverter = (PdfUnitConvertor) null;
    if (this._pdfPageTemplate != null)
      this._pdfPageTemplate = (PdfTemplate) null;
    if (this._pdfGraphics != null)
      this._pdfGraphics = (PdfGraphics) null;
    if (this._pdfSection != null)
      this._pdfSection = (PdfSection) null;
    if (this._maxPdfFont != null)
      this._maxPdfFont = (PdfFont) null;
    if (this._pdfTemplateCollection != null)
      this._pdfTemplateCollection = (List<PdfTemplate>) null;
    if (this._pdfDocument != null)
      this._pdfDocument = (PdfDocument) null;
    if (this._loadDoc != null)
      this._loadDoc = (PdfLoadedDocument) null;
    if (this._excelEngine != null)
      this._excelEngine = (ExcelEngine) null;
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    if (this._excelEngine != null)
      this._excelEngine = (ExcelEngine) null;
    if (this._fontCollection == null)
      return;
    this._fontCollection.Clear();
  }

  public IChartToImageConverter ChartToImageConverter
  {
    get => this._workBook.Application.ChartToImageConverter;
    set => this._workBook.Application.ChartToImageConverter = value;
  }

  internal float SheetWidth
  {
    get => this._sheetWidth;
    set => this._sheetWidth = value;
  }

  internal float SheetHeight
  {
    get => this._sheetHeight;
    set => this._sheetHeight = value;
  }

  internal List<SplitText> SplitTexts => this._splitTextCollection;

  internal bool IsNewPage
  {
    get => this._isNewPage;
    set => this._isNewPage = value;
  }

  internal float AdjacentRectWidth => this._adjacentRectWidth;

  internal float TopMargin => this._topMargin;

  internal float BottomMargin => this._bottomMargin;

  internal float HeaderMargin => this._headerMargin;

  internal float FooterMargin => this._footerMargin;

  internal bool HasHeader
  {
    get => ((WorksheetBaseImpl) this._workSheet).PageSetupBase.FullHeaderString != string.Empty;
  }

  internal bool HasFooter
  {
    get => ((WorksheetBaseImpl) this._workSheet).PageSetupBase.FullFooterString != string.Empty;
  }

  internal float LeftMargin => this._leftMargin;

  internal float RightMargin => this._rightMargin;

  internal bool IsPrintTitleRowPage => this._isPrintTitleRowPage;

  internal bool IsPrintTitleColumnPage => this._isPrintTitleColumnPage;

  internal ItemSizeHelper RowHeightGetter
  {
    get
    {
      if (this._rowHeightGetter == null)
      {
        this._rowHeightGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this._workSheet.GetRowHeightInPixels));
        double[] scaledWidthHeight = (this._workBook as WorkbookImpl).GetCellScaledWidthHeight(this._workSheet);
        this._scaledCellWidth = (float) scaledWidthHeight[0];
        this._scaledCellHeight = (float) scaledWidthHeight[1];
        this._rowHeightGetter.ScaledCellHeight = (double) this._scaledCellHeight;
        this._rowHeightGetter.ScaledCellWidth = (double) this._scaledCellWidth;
      }
      return this._rowHeightGetter;
    }
  }

  internal ItemSizeHelper ColumnWidthGetter
  {
    get
    {
      if (this._columnWidthGetter == null)
      {
        this._columnWidthGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this._workSheet.GetColumnWidthInPixels));
        double[] scaledWidthHeight = (this._workBook as WorkbookImpl).GetCellScaledWidthHeight(this._workSheet);
        this._scaledCellWidth = (float) scaledWidthHeight[0];
        this._scaledCellHeight = (float) scaledWidthHeight[1];
        this._columnWidthGetter.ScaledCellHeight = (double) this._scaledCellHeight;
        this._columnWidthGetter.ScaledCellWidth = (double) this._scaledCellWidth;
      }
      return this._columnWidthGetter;
    }
  }

  internal PdfUnitConvertor Pdf_UnitConverter => this._pdfUnitConverter;

  internal IPageSetup SheetPageSetup
  {
    get => this._pageSetup;
    set => this._pageSetup = value;
  }

  internal PageSetupOption ExcelToPdfPagesetup => this._pageSetupOption;

  internal float ScaledCellWidth
  {
    get => this._scaledCellWidth;
    set => this._scaledCellWidth = value;
  }

  internal float ScaledCellHeight
  {
    get => this._scaledCellHeight;
    set => this._scaledCellHeight = value;
  }

  internal PdfStringFormat MaxRowPdfTextformat => this._maxRowPdfTextformat;

  internal PdfFont MaxPdfFont => this._maxPdfFont;

  internal double MaxRowFontSize => this._maxRowFontSize;

  private bool HasVisibleStyle(WorksheetImpl sheet, RangeImpl range)
  {
    for (int row = range.Row; row <= range.LastRow; ++row)
    {
      if (row > 0 && sheet.IsRowVisible(row))
      {
        for (int column = range.Column; column <= range.LastColumn; ++column)
        {
          if (column > 0 && sheet.IsColumnVisible(column) && (sheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Blank || !this._stylewithoutBorderFill.Contains(sheet.GetXFIndex(row, column))))
            return true;
        }
      }
    }
    return false;
  }

  private PdfDocument DrawSheet(IWorksheet wkSheet, IRange[] printAreas)
  {
    WorksheetImpl worksheetImpl = (WorksheetImpl) wkSheet;
    if (worksheetImpl.ConditionalFormats.Count > 0)
      this._sortedListCf = worksheetImpl.ApplyCF();
    this.SheetPageSetup = wkSheet.PageSetup;
    if (!this.SheetPageSetup.AutoFirstPageNumber)
      this._pageCount = (int) this.SheetPageSetup.FirstPageNumber;
    float widthValue = 0.0f;
    float heightValue = 0.0f;
    List<float> floatList1 = new List<float>();
    List<float> floatList2 = new List<float>();
    this._pdfTemplateCollection.Clear();
    PageSetupBaseImpl pageSetup = this._workSheet.PageSetup as PageSetupBaseImpl;
    if (pageSetup.DifferentOddAndEvenPagesHF)
      this.RaiseWarning($"Different odd and even pages option applied in the worksheet : \"{wkSheet.Name}\" is not supported", WarningType.PageSettings);
    if (pageSetup.DifferentFirstPageHF)
      this.RaiseWarning($"Different first page option applied in the worksheet : \"{wkSheet.Name}\" is not supported", WarningType.PageSettings);
    if (this._workSheet.PageSetup.PrintHeadings)
      this.RaiseWarning($"Row and column headings print option applied in the worksheet : \"{wkSheet.Name}\" is not supported", WarningType.PageSettings);
    if (this._workSheet.SparklineGroups.Count > 0)
      this.RaiseWarning($"Sparklines in the worksheet : \"{wkSheet.Name}\" is not supported.", WarningType.DrawingObjects);
    this._leftMargin = this.Pdf_UnitConverter.ConvertUnits((float) this.SheetPageSetup.LeftMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._rightMargin = this.Pdf_UnitConverter.ConvertUnits((float) this.SheetPageSetup.RightMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._headerMargin = this.Pdf_UnitConverter.ConvertUnits((float) this.SheetPageSetup.HeaderMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._topMargin = this.Pdf_UnitConverter.ConvertUnits((float) this.SheetPageSetup.TopMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._bottomMargin = this.Pdf_UnitConverter.ConvertUnits((float) this.SheetPageSetup.BottomMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._footerMargin = this.Pdf_UnitConverter.ConvertUnits((float) this.SheetPageSetup.FooterMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    this._pdfSection = this._pdfDocument.Sections.Add();
    this._sectionCount = this._pdfDocument.Sections.Count - 1;
    if (worksheetImpl.PageSetup.PrintArea == null && this.SplitTexts.Count == 0)
    {
      int num = 0;
      for (int index = printAreas.Length - 1; index >= 0; --index)
      {
        RangeImpl printArea = printAreas[index] as RangeImpl;
        if (!this.CanDrawShape(printArea, this._workSheet) && !this.CheckMergedRegion((IRange) printArea) && !this.HasVisibleStyle(worksheetImpl, printArea))
          ++num;
        else
          break;
      }
      if (num != 0)
      {
        List<IRange> rangeList = new List<IRange>((IEnumerable<IRange>) printAreas);
        printAreas = new IRange[rangeList.Count - num];
        rangeList.CopyTo(0, printAreas, 0, rangeList.Count - num);
      }
    }
    foreach (IRange printArea in printAreas)
    {
      IRange range1 = printArea;
      int num1 = range1.Row;
      int num2 = range1.Column;
      int lastRow = range1.LastRow;
      int lastColumn = range1.LastColumn;
      if (!this._pageSetupOption.HasVerticalBreak && !this._pageSetupOption.HasHorizontalBreak && !this._pageSetupOption.HasPrintArea)
      {
        num2 = 1;
        num1 = 1;
      }
      widthValue = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(num2, lastColumn), PdfGraphicsUnit.Point);
      heightValue = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(num1, lastRow), PdfGraphicsUnit.Point);
      int index1 = 0;
      int num3 = 0;
      int num4 = 0;
      int[] numArray = new int[16384 /*0x4000*/];
      int[] rows = new int[1048576 /*0x100000*/];
      int columnStartIndex = 1;
      int rowStartIndex = 1;
      this._sheetHeight = 0.0f;
      this._sheetWidth = 0.0f;
      float usedRangeHeight = heightValue;
      float usedRangeWidth = widthValue;
      this._pdfSection.PageSettings.Margins.Left = this._leftMargin;
      this._pdfSection.PageSettings.Margins.Right = this._rightMargin;
      this._pdfSection.PageSettings.Margins.Top = this._topMargin;
      this._pdfSection.PageSettings.Margins.Bottom = this._bottomMargin;
      if (this.HasFooter)
        this._pdfSection.PageSettings.Margins.Bottom = 0.0f;
      if (this.HasHeader)
        this._pdfSection.PageSettings.Margins.Top = 0.0f;
      if (!string.IsNullOrEmpty(worksheetImpl.PageSetupBase.FullHeaderString) || !string.IsNullOrEmpty(worksheetImpl.PageSetupBase.FullFooterString))
        this._headerFooter = this.DrawHeadersAndFooters(this._pdfSection, worksheetImpl, (ChartImpl) null);
      this._excelToPdfPageLayout = new ExcelToPdfLayoutSetting(this);
      switch (this._excelToPdfSettings.LayoutOptions)
      {
        case LayoutOptions.FitSheetOnOnePage:
          this._excelToPdfPageLayout.FitSheetOnPage(this._pdfSection, this._pageSetup, usedRangeWidth, usedRangeHeight);
          rows[0] = num1;
          rows[1] = lastRow;
          numArray[0] = num2;
          numArray[1] = lastColumn;
          num4 = 1;
          num3 = 1;
          break;
        case LayoutOptions.NoScaling:
          this._excelToPdfPageLayout.NoScaling(this._pdfSection, this.SheetPageSetup, usedRangeWidth, usedRangeHeight);
          rows = this._helper.RowBreaker(num1, lastRow, this._sheetHeight, this.RowHeightGetter, out rowStartIndex, this._excelToPdfSettings);
          rows[rowStartIndex] = lastRow;
          num4 = rowStartIndex;
          numArray = this._helper.ColumnBreaker(num2, lastColumn, this._sheetWidth, this.ColumnWidthGetter, out columnStartIndex, this._excelToPdfSettings);
          numArray[columnStartIndex] = lastColumn;
          num3 = columnStartIndex;
          break;
        case LayoutOptions.FitAllColumnsOnOnePage:
          this._excelToPdfPageLayout.FitAllColumnOnOnePage(this._pdfSection, this.SheetPageSetup, usedRangeWidth, usedRangeHeight);
          numArray[0] = num2;
          numArray[1] = lastColumn;
          num3 = 1;
          rows = this._helper.RowBreaker(num1, lastRow, this._sheetHeight, this.RowHeightGetter, out rowStartIndex, this._excelToPdfSettings);
          rows[rowStartIndex] = lastRow;
          num4 = rowStartIndex;
          break;
        case LayoutOptions.FitAllRowsOnOnePage:
          this._excelToPdfPageLayout.FitAllRowsOnOnePage(this._pdfSection, this.SheetPageSetup, usedRangeWidth, usedRangeHeight);
          this._pdfPageTemplate = new PdfTemplate(this.SheetWidth + 3f, this.SheetHeight + 3f);
          rows[0] = num1;
          rows[1] = lastRow;
          num4 = 1;
          numArray = this._helper.ColumnBreaker(num2, lastColumn, this._sheetWidth, this.ColumnWidthGetter, out columnStartIndex, this._excelToPdfSettings);
          numArray[columnStartIndex] = lastColumn;
          num3 = columnStartIndex;
          break;
        case LayoutOptions.CustomScaling:
          this._excelToPdfPageLayout.CustomScaling(this._pdfSection, this.SheetPageSetup, usedRangeWidth, usedRangeHeight, printAreas);
          rows = this._helper.RowBreaker(num1, lastRow, this._sheetHeight, this.RowHeightGetter, out rowStartIndex, this._excelToPdfSettings);
          rows[rowStartIndex] = lastRow;
          num4 = rowStartIndex;
          numArray = this._helper.ColumnBreaker(num2, lastColumn, this._sheetWidth, this.ColumnWidthGetter, out columnStartIndex, this._excelToPdfSettings);
          numArray[columnStartIndex] = lastColumn;
          num3 = columnStartIndex;
          break;
      }
      int num5 = numArray[index1] == 0 ? 1 : numArray[index1];
      int index2 = index1 + 1;
      int num6 = numArray[index2];
      float xValue = 0.0f;
      int num7 = 1;
      float sheetWidth = this._sheetWidth;
      float sheetHeight = this._sheetHeight;
      if (this.SheetPageSetup.Order == ExcelOrder.DownThenOver)
      {
        int num8 = 0;
        int num9 = 0;
        for (; num8 < num3; ++num8)
        {
          int index3 = 0;
          int num10 = rows[index3];
          int index4 = index3 + 1;
          int num11 = rows[index4];
          float yValue = 0.0f;
          for (int index5 = 0; index5 < num4; ++index5)
          {
            this._isPrintTitleRowPage = System.Array.IndexOf<int>(this._pageSetupOption.RowIndexes.ToArray(), index4) >= 0;
            this._isPrintTitleColumnPage = this._hasPrintTitleColumn = System.Array.IndexOf<int>(this._pageSetupOption.ColumnIndexes.ToArray(), index2) >= 0;
            if (this._isPrintTitleColumnPage && (double) this._sheetWidth > (double) this._pageSetupOption.TitleColumnWidth && num5 >= this._pageSetupOption.PrintTitleFirstColumn && num5 <= this._pageSetupOption.PrintTitleLastColumn)
              num5 = this._pageSetupOption.PrintTitleLastColumn + 1;
            this.UpdateSheetWidthHeight(num5, num10);
            if (num5 <= num6)
            {
              RangeImpl range2 = worksheetImpl.Range[num10, num5, num11, num6] as RangeImpl;
              bool isBlank = !this.CheckSplitText(num10, num11) && !this.CanDrawShape(range2, (IWorksheet) worksheetImpl) && !this.CheckMergedRegion((IRange) range2) && !this.HasVisibleStyle(worksheetImpl, range2);
              if (!isBlank || this._excelToPdfSettings.IsConvertBlankPage)
              {
                if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.CustomScaling)
                {
                  if (!pageSetup.IsFitToPage)
                  {
                    double[] fontValue = this._helper.CalculateFontValue(this._excelToPdfSettings);
                    this._pdfPageTemplate = new PdfTemplate(this._sheetWidth / (float) fontValue[0], this._sheetHeight / (float) fontValue[1]);
                    this._sheetWidth = this._pdfPageTemplate.Width;
                    this._sheetHeight = this._pdfPageTemplate.Height;
                  }
                  else
                    this._pdfPageTemplate = new PdfTemplate(this._sheetWidth, this._sheetHeight);
                }
                else
                  this._pdfPageTemplate = this._excelToPdfSettings.LayoutOptions == LayoutOptions.FitAllColumnsOnOnePage || this._excelToPdfSettings.LayoutOptions == LayoutOptions.FitAllRowsOnOnePage ? new PdfTemplate(this._sheetWidth + 3f, this._sheetHeight + 3f) : new PdfTemplate(this._sheetWidth, this._sheetHeight);
                this._pdfTemplateCollection.Add(this._pdfPageTemplate);
                this.InitializePdfPage();
                this.UpdateScaledPage(this._pdfPageTemplate, this._currentPage, this._workSheet.PageSetup as PageSetupBaseImpl, usedRangeWidth, usedRangeHeight);
                this.GetCenterValues(num10, num5, num11, num6, floatList1, floatList2);
                this._centerHorizontalValue = this.CalculateCenterHorizontally(floatList1[floatList1.Count - 1], this._workSheet.PageSetup as PageSetupBaseImpl);
                this._centerVerticalValue = this.CalculateCenterVertically(floatList2[floatList2.Count - 1], this._workSheet.PageSetup as PageSetupBaseImpl);
                this._pdfDocument = this.DrawRow((IWorksheet) worksheetImpl, num5, num6, num10, num11, xValue, yValue, this._sheetWidth, isBlank);
                if (this._excelToPdfSettings.ExportBookmarks)
                {
                  this.AddBookMark();
                  this._excelToPdfSettings.ExportBookmarks = false;
                  this._bookmark = true;
                }
                ++num9;
              }
            }
            ++index4;
            num10 = num11 + 1;
            num11 = rows[index4];
            yValue += this._sheetHeight;
            ++num7;
            this._startX = 0.0f;
            this._headerHeight = 0.0f;
            this._sheetHeight = sheetHeight;
            this._sheetWidth = sheetWidth;
          }
          xValue += this._sheetWidth;
          ++index2;
          num5 = num6 + 1;
          num6 = numArray[index2];
          this._sheetHeight = sheetHeight;
          this._sheetWidth = sheetWidth;
        }
      }
      else if (this.SheetPageSetup.Order == ExcelOrder.OverThenDown)
      {
        int index6 = 0;
        int num12 = rows[index6];
        int index7 = index6 + 1;
        int num13 = rows[index7];
        int num14 = 0;
        int num15 = 0;
        for (; num14 < num4; ++num14)
        {
          int index8 = 0;
          int num16 = numArray[index8];
          int index9 = index8 + 1;
          int num17 = numArray[index9];
          float yValue = 0.0f;
          for (int index10 = 0; index10 < num3; ++index10)
          {
            this._isPrintTitleRowPage = System.Array.IndexOf<int>(this._pageSetupOption.RowIndexes.ToArray(), index7) >= 0;
            this._isPrintTitleColumnPage = this._hasPrintTitleColumn = System.Array.IndexOf<int>(this._pageSetupOption.ColumnIndexes.ToArray(), index9) >= 0;
            if (this._isPrintTitleColumnPage && (double) this._sheetWidth > (double) this._pageSetupOption.TitleColumnWidth && num16 >= this._pageSetupOption.PrintTitleFirstColumn && num16 <= this._pageSetupOption.PrintTitleLastColumn)
              num16 = this._pageSetupOption.PrintTitleLastColumn + 1;
            this.UpdateSheetWidthHeight(num16, num12);
            if (num16 <= num17)
            {
              RangeImpl range3 = worksheetImpl.Range[num12, num16, num13, num17] as RangeImpl;
              bool isBlank = !this.CheckSplitText(num12, num13) && !this.CanDrawShape(range3, (IWorksheet) worksheetImpl) && !this.CheckMergedRegion((IRange) range3) && !this.HasVisibleStyle(worksheetImpl, range3);
              if (!isBlank || this._excelToPdfSettings.IsConvertBlankPage)
              {
                if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.CustomScaling)
                {
                  if (!pageSetup.IsFitToPage)
                  {
                    double[] fontValue = this._helper.CalculateFontValue(this._excelToPdfSettings);
                    this._pdfPageTemplate = new PdfTemplate(this._sheetWidth / (float) fontValue[0], this._sheetHeight / (float) fontValue[1]);
                    this._sheetWidth = this._pdfPageTemplate.Width;
                    this._sheetHeight = this._pdfPageTemplate.Height;
                  }
                  else
                    this._pdfPageTemplate = new PdfTemplate(this._sheetWidth, this._sheetHeight);
                }
                else
                  this._pdfPageTemplate = this._excelToPdfSettings.LayoutOptions == LayoutOptions.FitAllColumnsOnOnePage || this._excelToPdfSettings.LayoutOptions == LayoutOptions.FitAllRowsOnOnePage ? new PdfTemplate(this._sheetWidth + 3f, this._sheetHeight + 3f) : new PdfTemplate(this._sheetWidth, this._sheetHeight);
                this._pdfTemplateCollection.Add(this._pdfPageTemplate);
                this.InitializePdfPage();
                this.UpdateScaledPage(this._pdfPageTemplate, this._currentPage, this._workSheet.PageSetup as PageSetupBaseImpl, usedRangeWidth, usedRangeHeight);
                this.GetCenterValues(num12, num16, num13, num17, floatList1, floatList2);
                this._pdfDocument = this.DrawRow((IWorksheet) worksheetImpl, num16, num17, num12, num13, xValue, yValue, this._sheetWidth, isBlank);
                if (this._excelToPdfSettings.ExportBookmarks)
                {
                  this.AddBookMark();
                  this._excelToPdfSettings.ExportBookmarks = false;
                  this._bookmark = true;
                }
                ++num15;
              }
            }
            xValue += this._sheetWidth;
            ++index9;
            num16 = num17 + 1;
            num17 = numArray[index9];
            this._sheetHeight = sheetHeight;
            this._sheetWidth = sheetWidth;
          }
          ++index7;
          num12 = num13 + 1;
          num13 = rows[index7];
          float num18 = yValue + this._sheetHeight;
          ++num7;
          this._sheetHeight = sheetHeight;
          this._sheetWidth = sheetWidth;
        }
      }
      if (this.SplitTexts.Count != 0 && !this._workSheet.IsRightToLeft && this._workSheet.PageSetup.PrintArea == null)
        this.DrawSplitText(floatList1, floatList2, rows);
    }
    if (this._pdfSection.Pages.Count != 0 && this._pdfTemplateCollection.Count != 0)
    {
      float headerHeight = 0.0f;
      for (int index = 0; index < this._pdfSection.Pages.Count; ++index)
      {
        float num = widthValue;
        if (this.SheetPageSetup.CenterHorizontally)
          widthValue = floatList1[index];
        if (this.SheetPageSetup.CenterVertically)
          heightValue = floatList2[index];
        float startX = 0.0f;
        PdfTemplate pdfTemplate = this._pdfPageTemplate = this._pdfTemplateCollection[index];
        PdfPage page = this._pdfSection.Pages[index];
        this.FindPageSettings(pdfTemplate, page, wkSheet, ref startX, ref headerHeight, widthValue, heightValue);
        if ((double) startX == 0.0 && this._excelToPdfSettings.EnableRTL)
          startX = page.GetClientSize().Width - this._scaledPageWidth;
        page.Graphics.DrawPdfTemplate(pdfTemplate, new PointF(startX, headerHeight), new SizeF(this._scaledPageWidth, this._scaledPageHeight));
        if (this._footerTemplate != null)
        {
          page.Graphics.DrawPdfTemplate(this._footerTemplate, this._footerBounds.Location, this._footerBounds.Size);
          this._footerTemplate = (PdfTemplate) null;
          this._footerBounds = RectangleF.Empty;
        }
        headerHeight = 0.0f;
        widthValue = num;
        ++this._pageCount;
        if (index == 0)
          ++this._sectionCount;
      }
    }
    if (this._allowHeader && !string.IsNullOrEmpty(worksheetImpl.PageSetupBase.FullHeaderString) && this._headerFooter.Count > 0)
    {
      if (this._excelToPdfSettings.HeaderFooterOption.ShowHeader)
        this._pdfSection.Template.Top = this.AddPDFHeaderFooter((IPageSetupBase) worksheetImpl.PageSetupBase, this._pdfSection, true, false);
      else
        this._headerFooter.RemoveAt(0);
    }
    if (this._allowFooter && !string.IsNullOrEmpty(worksheetImpl.PageSetupBase.FullFooterString) && this._headerFooter.Count > 0)
    {
      if (this._excelToPdfSettings.HeaderFooterOption.ShowFooter)
        this._pdfSection.Template.Bottom = this.AddPDFHeaderFooter((IPageSetupBase) worksheetImpl.PageSetupBase, this._pdfSection, false, false);
      else
        this._headerFooter.RemoveAt(0);
    }
    this._isPrintTitleRowPage = false;
    this._isPrintTitleColumnPage = false;
    floatList1.Clear();
    floatList2.Clear();
    this._printTitleTemplateCollection.Clear();
    foreach (KeyValuePair<IChart, MemoryStream> chartImage in this._chartImageCollection)
      chartImage.Value.Dispose();
    this._chartImageCollection.Clear();
    if (this._headerFooter.Count > 0)
      this._headerFooter.RemoveRange(0, this._headerFooter.Count);
    this._isHeaderPageCount = false;
    this._isFooterPageCount = false;
    return this._pdfDocument;
  }

  private bool CheckSplitText(int firstRow, int lastRow)
  {
    if (this.SplitTexts.Count != 0)
    {
      for (int index = 0; index < this.SplitTexts.Count; ++index)
      {
        SplitText splitText = this.SplitTexts[index];
        if (splitText.Row >= firstRow && splitText.Row <= lastRow && splitText.AdjacentColumn != 0)
          return true;
      }
    }
    return false;
  }

  private void UpdateSheetWidthHeight(int firstColumn, int firstRow)
  {
    switch (this._excelToPdfSettings.LayoutOptions)
    {
      case LayoutOptions.FitSheetOnOnePage:
      case LayoutOptions.NoScaling:
      case LayoutOptions.FitAllColumnsOnOnePage:
      case LayoutOptions.FitAllRowsOnOnePage:
        float sheetWidth = this.SheetWidth;
        float sheetHeight = this.SheetHeight;
        float num1 = 0.0f;
        float num2 = 0.0f;
        int rowEnd1 = firstColumn;
        int rowEnd2 = firstRow;
        if (this.IsPrintTitleColumnPage)
          num1 = this._pageSetupOption.TitleColumnWidth;
        if (this.IsPrintTitleRowPage)
          num2 = this._pageSetupOption.TitleRowHeight;
        for (; rowEnd1 <= this._workBook.MaxColumnCount && (double) sheetWidth >= (double) num1 + (double) this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, rowEnd1), PdfGraphicsUnit.Point); ++rowEnd1)
          this.SheetWidth = num1 + this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, rowEnd1), PdfGraphicsUnit.Point);
        for (; rowEnd2 <= this._workBook.MaxRowCount && (double) sheetHeight >= (double) num2 + (double) this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow, rowEnd2), PdfGraphicsUnit.Point); ++rowEnd2)
          this.SheetHeight = num2 + this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow, rowEnd2), PdfGraphicsUnit.Point);
        this.SheetWidth += 1.5f;
        this.SheetHeight += 1.5f;
        break;
    }
  }

  private void GetCenterValues(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    List<float> widthVal,
    List<float> heightVal)
  {
    float num1 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, lastColumn), PdfGraphicsUnit.Point);
    float num2 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow, lastRow), PdfGraphicsUnit.Point);
    if (this.IsPrintTitleColumnPage)
      num1 += this._pageSetupOption.TitleColumnWidth;
    if (this.IsPrintTitleRowPage)
      num2 += this._pageSetupOption.TitleRowHeight;
    widthVal.Add(num1);
    heightVal.Add(num2);
  }

  private void FindPageSettings(
    PdfTemplate pdfTemplate,
    PdfPage page,
    IWorksheet wkSheet,
    ref float startX,
    ref float headerHeight,
    float widthValue,
    float heightValue)
  {
    float footerHeight = 0.0f;
    headerHeight = 0.0f;
    PageSetupBaseImpl pageSetup = this._workSheet.PageSetup as PageSetupBaseImpl;
    if (!this._excelToPdfSettings.RenderBySheet && (this._isHeaderPageCount || this._isFooterPageCount))
    {
      if (this._pdfDocument.Sections.Count > this._sectionCount)
      {
        if (this._headerFooter.Count != 0)
          this.AddHeaderFooterImpl(this._headerFooter, wkSheet as WorksheetImpl, (ChartImpl) null, pdfTemplate);
      }
      else if (this._headerFooterImplCollection.Count > 0)
        this._headerFooterImplCollection[this._sectionCount - 1].PdfPageTemplateCollection.Add(pdfTemplate);
    }
    if (this._headerFooter.Count != 0)
      this.DrawPdfPageHeaderFooter(this._pdfSection, this._headerFooter, (IPageSetupBase) (wkSheet as WorksheetImpl).PageSetupBase, this._topMargin, this._bottomMargin, page, out headerHeight, out footerHeight, this._excelToPdfSettings.LayoutOptions, this._excelToPdfSettings.HeaderFooterOption.ShowHeader, this._excelToPdfSettings.HeaderFooterOption.ShowFooter, pdfTemplate, false, this._footerMargin, this._headerMargin);
    this.UpdateScaledPage(pdfTemplate, page, pageSetup, widthValue, heightValue);
    startX = this.CalculateCenterHorizontally(widthValue, pageSetup);
    headerHeight = this.CalculateCenterVertically(heightValue, pageSetup);
  }

  internal void AddHeaderFooterImpl(
    List<HeaderFooter> headerFooterCollection,
    WorksheetImpl worksheet,
    ChartImpl chart,
    PdfTemplate pdfTemplate)
  {
    HeaderFooterImpl headerFooterImpl = new HeaderFooterImpl();
    headerFooterImpl.BottomMargin = this._bottomMargin;
    headerFooterImpl.TopMargin = this._topMargin;
    headerFooterImpl.FooterMargin = this._footerMargin;
    headerFooterImpl.HeaderMargin = this._headerMargin;
    if (worksheet != null)
      headerFooterImpl.PageSetup = worksheet.PageSetupBase;
    else if (chart != null)
      headerFooterImpl.PageSetup = chart.PageSetupBase;
    headerFooterImpl.PdfSection = this._pdfSection;
    headerFooterImpl.PdfPageTemplateCollection.Add(pdfTemplate);
    for (int index1 = 0; index1 < this._headerFooter.Count; ++index1)
    {
      if (this._isHeaderPageCount && this._headerFooter[index1].HeaderFooterName == "Header")
      {
        HeaderFooter headerFooter = this._headerFooter[0].Clone();
        headerFooterImpl.HeaderFooterCollection.Add(headerFooter);
        headerFooterImpl.HeaderFooterCollection[headerFooterImpl.HeaderFooterCollection.Count - 1].HeaderFooterSections = new List<HeaderFooterSection>();
        for (int index2 = 0; index2 < this._headerFooter[0].HeaderFooterSections.Count; ++index2)
        {
          if (this._headerFooter[0].HeaderFooterSections[index2].IsPageCount)
          {
            headerFooterImpl.HeaderFooterCollection[headerFooterImpl.HeaderFooterCollection.Count - 1].HeaderFooterSections.Add(this._headerFooter[0].HeaderFooterSections[index2]);
            this._headerFooter[0].HeaderFooterSections.RemoveAt(index2);
            --index2;
          }
        }
        if (this._headerFooter[0].HeaderFooterSections.Count == 0)
        {
          this._headerFooter.RemoveAt(0);
          --index1;
        }
      }
      else if (this._isFooterPageCount && this._headerFooter[index1].HeaderFooterName == "Footer")
      {
        HeaderFooter headerFooter = this._headerFooter[index1].Clone();
        headerFooterImpl.HeaderFooterCollection.Add(headerFooter);
        headerFooterImpl.HeaderFooterCollection[headerFooterImpl.HeaderFooterCollection.Count - 1].HeaderFooterSections = new List<HeaderFooterSection>();
        for (int index3 = 0; index3 < this._headerFooter[index1].HeaderFooterSections.Count; ++index3)
        {
          if (this._headerFooter[index1].HeaderFooterSections[index3].IsPageCount)
          {
            headerFooterImpl.HeaderFooterCollection[headerFooterImpl.HeaderFooterCollection.Count - 1].HeaderFooterSections.Add(this._headerFooter[index1].HeaderFooterSections[index3]);
            this._headerFooter[index1].HeaderFooterSections.RemoveAt(index3);
            --index3;
          }
        }
        if (this._headerFooter[index1].HeaderFooterSections.Count == 0)
          this._headerFooter.RemoveAt(index1);
      }
    }
    headerFooterImpl.IsHeader = this._excelToPdfSettings.HeaderFooterOption.ShowHeader;
    headerFooterImpl.IsFooter = this._excelToPdfSettings.HeaderFooterOption.ShowFooter;
    headerFooterImpl.LayoutOptions = this._excelToPdfSettings.LayoutOptions;
    this._headerFooterImplCollection.Add(this._sectionCount, headerFooterImpl);
  }

  private float CalculateCenterVertically(float heightValue, PageSetupBaseImpl pageSetup)
  {
    float centerVertically = 0.0f;
    if (this.SheetPageSetup.CenterVertically)
    {
      if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.CustomScaling && !pageSetup.IsFitToPage)
        heightValue = (float) ((double) heightValue * (double) pageSetup.Zoom / 100.0);
      float num1 = this._pdfSection.PageSettings.Height - (this._topMargin + this._bottomMargin);
      if ((double) this._scaledPageHeight < (double) num1)
      {
        centerVertically += (float) (((double) num1 - (double) this._scaledPageHeight) / 2.0);
        if ((double) this._pdfPageTemplate.Height > (double) heightValue)
          centerVertically += (float) (((double) this._scaledPageHeight - (double) this._scaledPageHeight * (double) heightValue / (double) this._pdfPageTemplate.Height) / 2.0);
      }
      else if ((double) heightValue < (double) this._scaledPageHeight)
      {
        float num2 = this._scaledPageHeight;
        if ((double) num2 == (double) this._pdfSection.PageSettings.Height)
          num2 = num1;
        centerVertically += (float) (((double) num2 - (double) heightValue) / 2.0);
      }
    }
    if (this.HasHeader && (double) this._pdfSection.PageSettings.Margins.Top == 0.0)
      centerVertically += this._topMargin;
    return centerVertically;
  }

  private float CalculateCenterHorizontally(float widthValue, PageSetupBaseImpl pageSetup)
  {
    float centerHorizontally = 0.0f;
    if (this.SheetPageSetup.CenterHorizontally)
    {
      float num1 = this._pdfSection.PageSettings.Width - (this._leftMargin + this._rightMargin);
      if ((double) this._scaledPageWidth <= (double) num1)
      {
        centerHorizontally += (float) (((double) num1 - (double) this._scaledPageWidth) / 2.0);
        if ((double) this._pdfPageTemplate.Width > (double) widthValue)
          centerHorizontally += (float) (((double) this._scaledPageWidth - (double) this._scaledPageWidth * (double) widthValue / (double) this._pdfPageTemplate.Width) / 2.0);
      }
      else
      {
        if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.CustomScaling && !pageSetup.IsFitToPage)
          widthValue = (float) ((double) widthValue * (double) pageSetup.Zoom / 100.0);
        if ((double) widthValue < (double) this._scaledPageWidth)
        {
          float num2 = this._scaledPageWidth;
          if ((double) num2 == (double) this._pdfSection.PageSettings.Width)
            num2 = num1;
          centerHorizontally += (float) (((double) num2 - (double) widthValue) / 2.0);
        }
      }
    }
    return centerHorizontally;
  }

  private void UpdateScaledPage(
    PdfTemplate pdfTemplate,
    PdfPage page,
    PageSetupBaseImpl pageSetup,
    float usedRangeWidth,
    float usedRangeHeight)
  {
    if (this._excelToPdfSettings.LayoutOptions != LayoutOptions.NoScaling && this._excelToPdfSettings.LayoutOptions != LayoutOptions.CustomScaling)
    {
      double maxWidth = (double) page.Size.Width - ((double) this._leftMargin + (double) this._rightMargin);
      double maxHeight = (double) page.Size.Height - ((double) this._topMargin + (double) this._bottomMargin);
      this.GetScaledPage(pdfTemplate.Width, pdfTemplate.Height, (float) maxWidth, (float) maxHeight, true);
    }
    else if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.CustomScaling)
    {
      if (!pageSetup.IsFitToPage)
      {
        this._scaledPageWidth = page.Size.Width - (this._leftMargin + this._rightMargin);
        this._scaledPageHeight = page.Size.Height - (this._topMargin + this._bottomMargin);
      }
      else if (pageSetup.FitToPagesWide == 1 && pageSetup.FitToPagesTall == 0 || pageSetup.FitToPagesWide == 0 && pageSetup.FitToPagesTall == 1 || pageSetup.FitToPagesWide == 1 && ((double) usedRangeHeight + ((double) this._topMargin + (double) this._bottomMargin) < (double) page.Size.Height || (double) usedRangeWidth + ((double) this._leftMargin + (double) this._rightMargin) <= (double) page.Size.Width))
        this.GetScaledPage(pdfTemplate.Width, pdfTemplate.Height, page.Size.Width - (this._leftMargin + this._rightMargin), page.Size.Height - (this._topMargin + this._bottomMargin), true);
      else
        this.GetScaledPage(pdfTemplate.Width - (this._leftMargin + this._rightMargin), pdfTemplate.Height, page.Size.Width, page.Size.Height - (this._topMargin + this._bottomMargin), true);
    }
    else
    {
      this._scaledPageWidth = (double) pdfTemplate.Width <= (double) page.Section.PageSettings.Width - ((double) this._leftMargin + (double) this._rightMargin) ? pdfTemplate.Width : page.Section.PageSettings.Width - (this._leftMargin + this._rightMargin);
      if ((double) pdfTemplate.Height > (double) page.Section.PageSettings.Height - ((double) this._topMargin + (double) this._bottomMargin))
        this._scaledPageHeight = page.Section.PageSettings.Height - (this._topMargin + this._bottomMargin);
      else
        this._scaledPageHeight = pdfTemplate.Height;
    }
  }

  private float[] GetMarginForCenterAlignment(bool isFitToPage)
  {
    float[] forCenterAlignment = new float[4];
    double[] fontValue = this._helper.CalculateFontValue(this._excelToPdfSettings);
    if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.CustomScaling && !isFitToPage)
    {
      float num1 = this._pdfSection.PageSettings.Width / 72f * 2.54f;
      float num2 = (float) ((double) this._pageSetupOption.TitleColumnWidth * fontValue[0] / 72.0 * 2.5399999618530273);
      float num3;
      float num4;
      if (this._workSheet.PageSetup.CenterHorizontally)
      {
        num3 = (float) Math.Round(((double) num1 - (double) num2) / 2.0, 2);
        num4 = (float) Math.Round(((double) num1 - (double) num2) / 2.0, 2);
      }
      else
      {
        num3 = this._leftMargin;
        num4 = this._rightMargin;
      }
      if ((double) num3 < 0.0)
        num3 = 0.0f;
      if ((double) num4 < 0.0)
        num4 = 0.0f;
      float num5 = this._pdfSection.PageSettings.Height / 72f * 2.54f;
      float num6 = (float) ((double) this._pageSetupOption.TitleRowHeight * fontValue[1] / 72.0 * 2.5399999618530273);
      float num7;
      float num8;
      if (this._workSheet.PageSetup.CenterVertically)
      {
        num7 = (float) (((double) num1 - (double) num6) / 2.0);
        if ((double) num7 > 0.0)
          num7 = (float) Math.Round((double) num7, 2);
        num8 = (float) (((double) num1 - (double) num6) / 2.0);
        if ((double) num8 > 0.0)
          num8 = (float) Math.Round((double) num8, 2);
      }
      else
      {
        num7 = this._topMargin;
        num8 = this._bottomMargin;
      }
      if ((double) num7 < 0.0)
        num7 = 0.0f;
      if ((double) num8 < 0.0)
        num8 = 0.0f;
      forCenterAlignment[0] = num3;
      forCenterAlignment[1] = num4;
      forCenterAlignment[2] = num7;
      forCenterAlignment[3] = num8;
    }
    return forCenterAlignment;
  }

  private void DrawSplitText(
    List<float> centerWidthValues,
    List<float> centerHeightValues,
    int[] rows)
  {
    while (this.SplitTexts.Count > 0)
    {
      this._pdfPageTemplate = new PdfTemplate(this._sheetWidth, this._sheetHeight);
      this._pdfTemplateCollection.Add(this._pdfPageTemplate);
      this.InitializePdfPage();
      int index1 = 0;
      int num1 = rows[index1];
      int index2 = index1 + 1;
      int row = rows[index2];
      float num2 = 0.0f;
      float num3 = 0.0f;
      for (int index3 = 0; index3 < this.SplitTexts.Count; ++index3)
      {
        bool flag1 = false;
        SplitText splitText = this.SplitTexts[index3];
        if (num1 <= splitText.Row && row >= splitText.Row)
        {
          flag1 = true;
          RectangleF originRect = splitText.OriginRect;
          if (splitText.RichText != null)
          {
            float num4 = 0.0f;
            for (int index4 = 0; index4 < splitText.RichText.Count; ++index4)
            {
              string str = splitText.RichText[index4];
              IFont font = splitText.RichTextFont[index4];
              bool flag2 = this.CheckUnicode(str);
              string fontName = font.FontName;
              if (flag2)
                fontName = this.SwitchFonts(str, (font as FontImpl).CharSet, fontName);
              Font systemFont = this._workBookImpl.GetSystemFont(font, fontName);
              bool isEmbedFont = this._excelToPdfSettings.EmbedFonts || Encoding.UTF8.GetByteCount(str) != str.Length;
              PdfTrueTypeFont pdfTrueTypeFont = this.GetPdfTrueTypeFont(systemFont, isEmbedFont, (Stream) null);
              num4 += pdfTrueTypeFont.MeasureString(splitText.RichText[index4]).Width;
              systemFont.Dispose();
              pdfTrueTypeFont.Dispose();
            }
            if ((double) originRect.Width < (double) num4)
              originRect.Width = num4;
            this.DrawRTFText(originRect, originRect, this._pdfPageTemplate.Graphics, new List<IFont>((IEnumerable<IFont>) splitText.RichTextFont), new List<string>((IEnumerable<string>) splitText.RichText), false, true, false, false, false);
          }
          else if (!this._workBookImpl.IsNullOrWhiteSpace(splitText.Text))
          {
            float width = splitText.TextFont.MeasureString(splitText.Text).Width;
            if ((double) originRect.Width < (double) width)
              originRect.Width = width;
            this._pdfPageTemplate.Graphics.DrawString(splitText.Text, splitText.TextFont, splitText.Brush, originRect, splitText.Format);
          }
          if ((double) originRect.Right <= (double) this._pdfPageTemplate.Width)
          {
            this.SplitTexts.Remove(splitText);
            --index3;
            if ((double) originRect.Right > (double) num2)
              num2 = originRect.Right;
          }
          else
          {
            splitText.OriginRect = new RectangleF(originRect.X - this._pdfPageTemplate.Width, originRect.Y, originRect.Width, originRect.Height);
            num2 = this._pdfPageTemplate.Width;
          }
          if ((double) originRect.Bottom > (double) num3)
            num3 = originRect.Bottom;
        }
        if (!flag1)
        {
          this._pdfPageTemplate = new PdfTemplate(this._sheetWidth, this._sheetHeight);
          this._pdfTemplateCollection.Add(this._pdfPageTemplate);
          this.InitializePdfPage();
          ++index2;
          num1 = row + 1;
          row = rows[index2];
          --index3;
        }
      }
      centerWidthValues.Add(num2);
      centerHeightValues.Add(num3);
    }
    if (this._pdfTextformat == null)
      return;
    this._pdfTextformat = (PdfStringFormat) null;
  }

  internal PdfTrueTypeFont GetPdfTrueTypeFont(
    Font font,
    bool isEmbedFont,
    Stream alternateFontStream)
  {
    return alternateFontStream == null || alternateFontStream.Length <= 0L ? new PdfTrueTypeFont(font, isEmbedFont) : new PdfTrueTypeFont(alternateFontStream, font.Size, isEmbedFont, (string) null, this.GetFontStyle(font.Style));
  }

  private PdfFontStyle GetFontStyle(FontStyle style)
  {
    PdfFontStyle fontStyle = PdfFontStyle.Regular;
    if ((style & FontStyle.Bold) == FontStyle.Bold)
      fontStyle = PdfFontStyle.Bold;
    if ((style & FontStyle.Italic) == FontStyle.Italic)
      fontStyle |= PdfFontStyle.Italic;
    if ((style & FontStyle.Underline) == FontStyle.Underline)
      fontStyle |= PdfFontStyle.Underline;
    if ((style & FontStyle.Strikeout) == FontStyle.Strikeout)
      fontStyle |= PdfFontStyle.Strikeout;
    return fontStyle;
  }

  private void DrawSplitText(SplitText splitText)
  {
    SplitText splitText1 = splitText;
    RectangleF originRect = splitText1.OriginRect;
    PdfStringFormat format = splitText.Format;
    format.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
    PdfFont textFont = splitText.TextFont;
    PdfBrush brush = splitText1.Brush;
    float width = textFont.MeasureString(splitText.Text, format).Width;
    string text = splitText.Text;
    if (format.TextDirection == PdfTextDirection.RightToLeft)
    {
      bool flag = true;
      if (format.Alignment == PdfTextAlignment.Right)
      {
        format.Alignment = PdfTextAlignment.Left;
        flag = true;
      }
      if (flag)
      {
        if ((double) width < (double) originRect.Width)
          originRect.X += originRect.Width - width;
        else if ((double) originRect.X > 0.0)
          originRect.X += width - originRect.Width;
        this._pdfPageTemplate.Graphics.DrawString(text, textFont, brush, originRect, format);
      }
      else
        this._pdfPageTemplate.Graphics.DrawString(text, textFont, brush, originRect, format);
    }
    else
      this._pdfPageTemplate.Graphics.DrawString(text, textFont, brush, originRect, format);
  }

  private void DrawPdfPageHeaderFooter(
    PdfSection section,
    List<HeaderFooter> headerFooterCollection,
    IPageSetupBase pageSetupBase,
    float topmargin,
    float bottomMargin,
    PdfPage page,
    out float headerHeight,
    out float footerHeight,
    LayoutOptions layoutOptions,
    bool isHeader,
    bool isFooter,
    PdfTemplate pdfTemplate,
    bool isFooterRender,
    float footerMargin,
    float headerMargin)
  {
    headerHeight = 0.0f;
    footerHeight = 0.0f;
    double num1 = (double) this.Pdf_UnitConverter.ConvertUnits((float) pageSetupBase.TopMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    double num2 = (double) this.Pdf_UnitConverter.ConvertUnits((float) pageSetupBase.BottomMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    int pageCount = this._pageCount;
    IFormatProvider invariantCulture = (IFormatProvider) CultureInfo.InvariantCulture;
    foreach (HeaderFooter headerFooter in headerFooterCollection)
    {
      if (headerFooter.HeaderFooterName == "Header" && isHeader && !this._allowHeader)
      {
        float maxHeight = this.GetMaxHeight(headerFooter.HeaderFooterSections);
        PdfTemplate headeFooterTemplate = this.GetHeadeFooterTemplate(pageSetupBase as PageSetupBaseImpl, page, maxHeight, layoutOptions, pdfTemplate);
        foreach (HeaderFooterSection headerFooterSection in headerFooter.HeaderFooterSections)
          this.GetHeaderFooterInformation(headerFooterSection, headerFooterCollection, section, pageSetupBase, headerFooterSection.HeaderFooterCollections, headeFooterTemplate, headerFooterSection.Width, headerFooterSection.Height, headerFooterSection.SectionName, headerFooter.HeaderFooterName, System.Convert.ToString(pageCount, invariantCulture));
        headerHeight = headeFooterTemplate.Height / (headeFooterTemplate.Width / page.GetClientSize().Width);
        page.Graphics.DrawPdfTemplate(headeFooterTemplate, new PointF(0.0f, headerMargin), new SizeF(page.GetClientSize().Width, headerHeight));
      }
      else if (headerFooter.HeaderFooterName == "Footer" && isFooter && !this._allowFooter)
      {
        float maxHeight = this.GetMaxHeight(headerFooter.HeaderFooterSections);
        PdfTemplate headeFooterTemplate = this.GetHeadeFooterTemplate(pageSetupBase as PageSetupBaseImpl, page, maxHeight, layoutOptions, pdfTemplate);
        foreach (HeaderFooterSection headerFooterSection in headerFooter.HeaderFooterSections)
          this.GetHeaderFooterInformation(headerFooterSection, headerFooterCollection, section, pageSetupBase, headerFooterSection.HeaderFooterCollections, headeFooterTemplate, headerFooterSection.Width, headerFooterSection.Height, headerFooterSection.SectionName, headerFooter.HeaderFooterName, System.Convert.ToString(pageCount, invariantCulture));
        footerHeight = headeFooterTemplate.Height / (headeFooterTemplate.Width / page.GetClientSize().Width);
        if (pageSetupBase.Parent is IWorksheet)
        {
          this._footerTemplate = headeFooterTemplate;
          this._footerBounds = new RectangleF(new PointF(0.0f, page.Size.Height - (footerHeight + footerMargin + page.Section.PageSettings.Margins.Top)), new SizeF(page.GetClientSize().Width, footerHeight));
          if (isFooterRender && this._footerTemplate != null)
          {
            page.Graphics.DrawPdfTemplate(this._footerTemplate, this._footerBounds.Location, this._footerBounds.Size);
            this._footerTemplate = (PdfTemplate) null;
            this._footerBounds = RectangleF.Empty;
          }
        }
        else
          page.Graphics.DrawPdfTemplate(headeFooterTemplate, new PointF(0.0f, page.Size.Height - (footerHeight + footerMargin + page.Section.PageSettings.Margins.Top)), new SizeF(page.GetClientSize().Width, footerHeight));
      }
    }
  }

  private PdfTemplate GetHeadeFooterTemplate(
    PageSetupBaseImpl pageSetupBase,
    PdfPage page,
    float height,
    LayoutOptions layoutOptions,
    PdfTemplate pdfTemplate)
  {
    return !pageSetupBase.HFScaleWithDoc ? new PdfTemplate(page.GetClientSize().Width, height) : (layoutOptions != LayoutOptions.FitAllColumnsOnOnePage || (double) pdfTemplate.Width <= (double) page.GetClientSize().Width ? (layoutOptions != LayoutOptions.FitAllRowsOnOnePage || (double) pdfTemplate.Height <= (double) page.Section.PageSettings.Height - ((double) this._topMargin + (double) this._bottomMargin) ? (layoutOptions != LayoutOptions.FitSheetOnOnePage ? (layoutOptions != LayoutOptions.CustomScaling || pageSetupBase.IsFitToPage ? new PdfTemplate(page.GetClientSize().Width, height) : new PdfTemplate(page.GetClientSize().Width / ((float) pageSetupBase.Zoom / 100f), height)) : ((double) pdfTemplate.Width <= (double) page.GetClientSize().Width || (double) pdfTemplate.Height <= (double) page.Section.PageSettings.Height - ((double) this._topMargin + (double) this._bottomMargin) ? ((double) pdfTemplate.Width <= (double) page.GetClientSize().Width ? ((double) pdfTemplate.Height <= (double) page.Section.PageSettings.Height - ((double) this._topMargin + (double) this._bottomMargin) ? new PdfTemplate(page.GetClientSize().Width, height) : new PdfTemplate(this.RequiredWidth(page.GetClientSize().Width, pdfTemplate.Height, page.Section.PageSettings.Height - (this._topMargin + this._bottomMargin)), height)) : new PdfTemplate(pdfTemplate.Width, height)) : ((double) pdfTemplate.Width <= (double) pdfTemplate.Height ? new PdfTemplate(this.RequiredWidth(page.GetClientSize().Width, pdfTemplate.Height, page.Section.PageSettings.Height - (this._topMargin + this._bottomMargin)), height) : new PdfTemplate(pdfTemplate.Width, height)))) : new PdfTemplate(this.RequiredWidth(page.GetClientSize().Width, pdfTemplate.Height, page.Section.PageSettings.Height - (this._topMargin + this._bottomMargin)), height)) : new PdfTemplate(pdfTemplate.Width, height));
  }

  private PdfDocument DrawRow(
    IWorksheet wkSheet,
    int startColumn,
    int endColumn,
    int startRow,
    int endRow,
    float xValue,
    float yValue,
    float originalWidth,
    bool isBlank)
  {
    WorksheetImpl wkSheet1 = (WorksheetImpl) wkSheet;
    this._pdfGraphics.OptimizeIdenticalImages = true;
    float num1 = startRow <= endRow ? this.GetBorderXandY(wkSheet[startRow, startColumn], true) / 2f : 0.0f;
    float num2 = startColumn <= endColumn ? this.GetBorderXandY(wkSheet[startRow, startColumn], false) / 2f : 0.0f;
    float num3 = xValue;
    float num4 = yValue;
    int num5 = 1;
    if (this.IsPrintTitleRowPage || this.IsPrintTitleColumnPage)
    {
      num5 = 0;
      this._drawprinttitle = true;
    }
    for (; num5 <= 1; ++num5)
    {
      float rangeHeight = !this.IsPrintTitleRowPage || !this._drawprinttitle ? this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(startRow, endRow), PdfGraphicsUnit.Point) : this._pageSetupOption.TitleRowHeight;
      float rangeWidth = !this.IsPrintTitleColumnPage || !this._drawprinttitle ? this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(startColumn, endColumn), PdfGraphicsUnit.Point) : this._pageSetupOption.TitleColumnWidth;
      if ((double) rangeWidth > (double) originalWidth)
        rangeWidth = originalWidth;
      int num6;
      int num7;
      if (this.IsPrintTitleRowPage && this._drawprinttitle)
      {
        num6 = this._pageSetupOption.PrintTitleFirstRow;
        num7 = this._pageSetupOption.PrintTitleLastRow;
      }
      else
      {
        if (this.IsPrintTitleRowPage)
        {
          num2 = this._pageSetupOption.TitleRowHeight + num2;
          if (this._pageSetupOption.PrintTitleFirstRow == startRow)
            startRow += this._pageSetupOption.PrintTitleLastRow;
        }
        num6 = startRow;
        num7 = endRow;
      }
      int num8;
      int num9;
      if (this.IsPrintTitleColumnPage && this._drawprinttitle)
      {
        num8 = this._pageSetupOption.PrintTitleFirstColumn;
        num9 = this._pageSetupOption.PrintTitleLastColumn;
      }
      else
      {
        if (this.IsPrintTitleColumnPage)
        {
          num1 = this._pageSetupOption.TitleColumnWidth + num1;
          if (this._pageSetupOption.PrintTitleFirstColumn == startColumn)
            startColumn += this._pageSetupOption.PrintTitleLastColumn;
        }
        num8 = startColumn;
        num9 = endColumn;
      }
      if (this._drawprinttitle)
      {
        float height = rangeHeight;
        float width = rangeWidth;
        if (!this.IsPrintTitleColumnPage)
          width = originalWidth;
        RectangleF rectangle = new RectangleF(num1, num2, width, height);
        this._pdfGraphics.DrawRectangle((PdfBrush) new PdfSolidBrush(new PdfColor(Color.White)), rectangle);
        if (this._printTitleTemplateCollection.ContainsKey(new Rectangle(num6, num8, num7, num9)))
        {
          this._printTitleTemplate = this._printTitleTemplateCollection[new Rectangle(num6, num8, num7, num9)];
        }
        else
        {
          this._printTitleTemplate = new PdfTemplate(rectangle.Right, rectangle.Bottom);
          PdfGraphics pdfGraphics = this._pdfGraphics;
          this._pdfGraphics = this._printTitleTemplate.Graphics;
          this.DrawRow((IWorksheet) wkSheet1, num8, num9, num6, num7, xValue, yValue, originalWidth, num1, num2, rangeWidth, rangeHeight);
          this._printTitleTemplateCollection.Add(new Rectangle(num6, num8, num7, num9), this._printTitleTemplate);
          this._pdfGraphics = pdfGraphics;
        }
        if ((double) this._printTitleTemplate.Width > (double) this._pdfPageTemplate.Width)
        {
          PdfTemplate pdfTemplate = new PdfTemplate(this._printTitleTemplate.Width, this._pdfPageTemplate.Height);
          pdfTemplate.Graphics.DrawPdfTemplate(this._pdfPageTemplate, new PointF(0.0f, 0.0f));
          pdfTemplate.Graphics.DrawPdfTemplate(this._printTitleTemplate, new PointF(0.0f, 0.0f));
          this._pdfPageTemplate = pdfTemplate;
          this._pdfGraphics = pdfTemplate.Graphics;
          this._pdfTemplateCollection.RemoveAt(this._pdfTemplateCollection.Count - 1);
          this._pdfTemplateCollection.Insert(this._pdfTemplateCollection.Count, pdfTemplate);
        }
        else
          this._pdfGraphics.DrawPdfTemplate(this._printTitleTemplate, new PointF(0.0f, 0.0f));
      }
      else if (!isBlank)
      {
        if (this._printTitleTemplate != null)
        {
          PdfGraphicsState state = this._pdfPageTemplate.Graphics.Save();
          this._pdfPageTemplate.Graphics.SetClip(new RectangleF(this.IsPrintTitleColumnPage ? this._printTitleTemplate.Width : 0.0f, this.IsPrintTitleRowPage ? this._printTitleTemplate.Height : 0.0f, this._pdfPageTemplate.Width, this._pdfPageTemplate.Height));
          this.DrawRow((IWorksheet) wkSheet1, num8, num9, num6, num7, xValue, yValue, originalWidth, num1, num2, rangeWidth, rangeHeight);
          if (state.Graphics == this._pdfPageTemplate.Graphics)
            this._pdfPageTemplate.Graphics.Restore(state);
        }
        else
          this.DrawRow((IWorksheet) wkSheet1, num8, num9, num6, num7, xValue, yValue, originalWidth, num1, num2, rangeWidth, rangeHeight);
      }
      if (this.IsPrintTitleRowPage && this.IsPrintTitleColumnPage && this._drawprinttitle)
      {
        float num10 = num1;
        float num11 = num2;
        float startX1 = num1 + this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(num8, num9), PdfGraphicsUnit.Point) + this.GetBorderXandY(wkSheet[num6, num8], true) / 2f;
        int num12 = num8;
        int num13 = num9;
        int num14 = num6;
        int num15 = num7;
        int num16 = startColumn;
        int endColIndex1 = endColumn;
        this.DrawRow((IWorksheet) wkSheet1, num16, endColIndex1, num6, num7, xValue, yValue, originalWidth, startX1, num2, rangeWidth, rangeHeight);
        float startX2 = num10;
        float startY1 = num2 + this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(num6, num7), PdfGraphicsUnit.Point) + this.GetBorderXandY(wkSheet[num6, num16], false) / 2f;
        int startRowIndex1 = startRow;
        int endRowIndex1 = endRow;
        int startColIndex = num12;
        int endColIndex2 = num13;
        this.DrawRow((IWorksheet) wkSheet1, startColIndex, endColIndex2, startRowIndex1, endRowIndex1, xValue, yValue, originalWidth, startX2, startY1, rangeWidth, rangeHeight);
        int startRowIndex2 = num14;
        int endRowIndex2 = num15;
        float startY2 = num11;
        this._pdfGraphics.DrawRectangle((PdfBrush) new PdfSolidBrush(new PdfColor(Color.White)), new RectangleF(0.0f, 0.0f, rangeWidth + startX2, rangeHeight + startY2));
        this.DrawRow((IWorksheet) wkSheet1, startColIndex, endColIndex2, startRowIndex2, endRowIndex2, xValue, yValue, originalWidth, startX2, startY2, rangeWidth, rangeHeight);
      }
      if (this._drawprinttitle)
      {
        this._drawprinttitle = false;
      }
      else
      {
        this._isPrintTitleColumnPage = false;
        this._isPrintTitleRowPage = false;
      }
      num2 = startColumn <= endColumn ? this.GetBorderXandY(wkSheet[startRow, startColumn], false) / 2f : 0.0f;
      num1 = startRow <= endRow ? this.GetBorderXandY(wkSheet[startRow, startColumn], true) / 2f : 0.0f;
      xValue = num3;
      yValue = num4;
    }
    this._drawprinttitle = false;
    this._printTitleTemplate = (PdfTemplate) null;
    return this._pdfDocument;
  }

  private PdfDocument DrawRow(
    IWorksheet wkSheet,
    int startColIndex,
    int endColIndex,
    int startRowIndex,
    int endRowIndex,
    float xValue,
    float yValue,
    float originalWidth,
    float startX,
    float startY,
    float rangeWidth,
    float rangeHeight)
  {
    WorksheetImpl sheet = (WorksheetImpl) wkSheet;
    if (sheet.IsRightToLeft && !this.IsPrintTitleRowPage && this._excelToPdfSettings.LayoutOptions == LayoutOptions.NoScaling)
    {
      float x = 0.0f;
      if (sheet.IsRightToLeft)
        x = this._pdfPageTemplate.Width - rangeWidth;
      this._pdfPageTemplate.Graphics.SetClip(new RectangleF(x, 0.0f, rangeWidth, rangeHeight));
    }
    if (this._excelToPdfSettings.DisplayGridLines == GridLinesDisplayStyle.Auto && sheet.PageSetup.PrintGridlines || this._excelToPdfSettings.DisplayGridLines == GridLinesDisplayStyle.Visible)
      this.DrawLines(sheet, startRowIndex, startColIndex, endRowIndex, endColIndex, this._pdfGraphics, rangeWidth, rangeHeight, startX, startY);
    this.DrawBackGround(sheet, startRowIndex, startColIndex, endRowIndex, endColIndex, this._pdfGraphics, new Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.CellMethod(this.DrawBackground), originalWidth, startX, startY);
    if (sheet.HasMergedCells)
      this.DrawMergeBackground(sheet, startRowIndex, startColIndex, endRowIndex, endColIndex, this._pdfGraphics, originalWidth, startX, startY);
    this.DrawCells(sheet, startRowIndex, startColIndex, endRowIndex, endColIndex, this._pdfGraphics, originalWidth, startX, startY, rangeWidth);
    this.IterateMerges(sheet, startRowIndex, startColIndex, endRowIndex, endColIndex, this._pdfGraphics, new Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.MergeMethod(this.DrawMerge), originalWidth, startX, startY);
    this.DrawShapes(sheet, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY);
    return this._pdfDocument;
  }

  private void DrawShapes(
    WorksheetImpl sheet,
    int startRowIndex,
    int startColIndex,
    int endRowIndex,
    int endColIndex,
    float startX,
    float startY)
  {
    this.InitializeDirectPDF(this._pdfGraphics);
    IShape[] shapes = new IShape[sheet.Shapes.Count];
    for (int index = 0; index < sheet.Shapes.Count; ++index)
      shapes[index] = sheet.Shapes[index];
    this.DrawShapesCollection(shapes, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY);
  }

  private void DrawGroupShape(
    IGroupShape groupShape,
    int startRowIndex,
    int startColIndex,
    int endRowIndex,
    int endColIndex,
    float startX,
    float startY)
  {
    this.DrawShapesCollection(groupShape.Items, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY);
  }

  private void DrawShapesCollection(
    IShape[] shapes,
    int startRowIndex,
    int startColIndex,
    int endRowIndex,
    int endColIndex,
    float startX,
    float startY)
  {
    foreach (IShape shape in shapes)
    {
      if (shape.IsShapeVisible)
      {
        switch (shape.ShapeType)
        {
          case ExcelShapeType.Unknown:
          case ExcelShapeType.AutoShape:
          case ExcelShapeType.Comment:
          case ExcelShapeType.TextBox:
          case ExcelShapeType.CheckBox:
            switch (shape)
            {
              case TextBoxShapeImpl _:
              case AutoShapeImpl _:
              case CheckBoxShapeImpl _:
              case OptionButtonShapeImpl _:
              case ComboBoxShapeImpl _:
label_8:
                this.DrawShape(shape as ShapeImpl, this._pdfGraphics, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY);
                continue;
              case CommentShapeImpl _:
                if (this._pageSetup.PrintComments != ExcelPrintLocation.PrintInPlace)
                  continue;
                goto label_8;
              default:
                continue;
            }
          case ExcelShapeType.Chart:
            this.DrawChart(shape as ChartShapeImpl, this._pdfGraphics, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY);
            continue;
          case ExcelShapeType.Group:
            if ((shape as GroupShapeImpl).Group == null)
              (shape as GroupShapeImpl).LayoutGroupShape(true);
            this.DrawGroupShape(shape as IGroupShape, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY);
            continue;
          case ExcelShapeType.Picture:
            this.DrawImage(shape, (Image) null, this._pdfGraphics, startRowIndex, startColIndex, endRowIndex, endColIndex, startX, startY, false, (PdfPath) null);
            continue;
          default:
            this.RaiseWarning($"Shape : \"{shape.Name}\" in the worksheet \"{this._workSheet.Name}\" is not supported", WarningType.DrawingObjects);
            continue;
        }
      }
    }
  }

  private float GetBorderXandY(IRange Range, bool isXaxis)
  {
    (Range.CellStyle as CellStyle).AskAdjacent = false;
    return this.GetBorderWidth(!isXaxis ? Range.Borders[ExcelBordersIndex.EdgeTop] : Range.Borders[ExcelBordersIndex.EdgeLeft]);
  }

  private void ApplyTransparency(PdfGraphics graphics, IShape shape, bool line)
  {
    if (line)
    {
      if (shape.Line.Transparency == 0.0)
        return;
      graphics.SetTransparency(1f - (float) shape.Line.Transparency);
    }
    else
    {
      if (shape.Fill.Transparency == 0.0)
        return;
      graphics.SetTransparency(1f - (float) shape.Fill.Transparency);
    }
  }

  private void DrawShape(
    ShapeImpl shape,
    PdfGraphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    float num1 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn - 1), PdfGraphicsUnit.Point);
    float num2 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow - 1), PdfGraphicsUnit.Point);
    float num3 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(lastColumn), PdfGraphicsUnit.Point);
    float num4 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(lastRow), PdfGraphicsUnit.Point);
    float num5 = num1 - startX;
    float num6 = num2 - startY;
    float y1 = num6;
    float num7;
    float num8;
    float num9;
    float num10;
    if (shape.GroupFrame != null)
    {
      num7 = (float) ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetX, MeasureUnits.EMU);
      num8 = (float) ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetY, MeasureUnits.EMU);
      num9 = (float) ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetCX, MeasureUnits.EMU);
      num10 = (float) ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetCY, MeasureUnits.EMU);
    }
    else
    {
      num7 = (float) shape.LeftDouble;
      num8 = (float) shape.TopDouble;
      num9 = (float) shape.WidthDouble;
      num10 = (float) shape.HeightDouble;
    }
    bool inBetween;
    if (!shape.PrintWithSheet || !this.CanDrawShape(firstRow, lastRow, firstColumn, lastColumn, shape, out inBetween) || (double) num7 >= (double) this._pdfPageTemplate.Width && firstColumn > shape.LeftColumn && lastColumn < shape.RightColumn && !inBetween || ((double) this.Pdf_UnitConverter.ConvertFromPixels(num8 * this._scaledCellHeight, PdfGraphicsUnit.Point) > (double) num4 || (double) this.Pdf_UnitConverter.ConvertFromPixels((num8 + num10) * this._scaledCellHeight, PdfGraphicsUnit.Point) < (double) num6 || (double) this.Pdf_UnitConverter.ConvertFromPixels(num7 * this._scaledCellWidth, PdfGraphicsUnit.Point) > (double) num3 || (double) this.Pdf_UnitConverter.ConvertFromPixels((num7 + num9) * this._scaledCellWidth, PdfGraphicsUnit.Point) < (double) num5) && ((double) this.Pdf_UnitConverter.ConvertFromPixels(num8 * this._scaledCellHeight, PdfGraphicsUnit.Point) > (double) this._currentPage.Size.Height || (double) this.Pdf_UnitConverter.ConvertFromPixels(num7 * this._scaledCellWidth, PdfGraphicsUnit.Point) > (double) this._currentPage.Size.Width))
      return;
    RectangleF rectangleF = new RectangleF(this.Pdf_UnitConverter.ConvertFromPixels(num7 * this._scaledCellWidth, PdfGraphicsUnit.Point), this.Pdf_UnitConverter.ConvertFromPixels(num8 * this._scaledCellHeight, PdfGraphicsUnit.Point), this.Pdf_UnitConverter.ConvertFromPixels(num9 * this._scaledCellWidth, PdfGraphicsUnit.Point), this.Pdf_UnitConverter.ConvertFromPixels(num10 * this._scaledCellHeight, PdfGraphicsUnit.Point));
    if ((double) rectangleF.Width == 0.0)
      rectangleF.Width = 0.1f;
    if ((double) rectangleF.Height == 0.0)
      rectangleF.Height = 0.1f;
    if (this._workSheet.IsRightToLeft && firstRow <= shape.TopRow && lastRow >= shape.BottomRow)
    {
      float num11 = this._pdfPageTemplate.Width - (rectangleF.Width + rectangleF.Left + startX);
      float x;
      if ((double) rectangleF.Y < (double) num11 || (double) num8 > (double) num7)
      {
        x = num11 - rectangleF.Left;
        y1 = startY;
      }
      else
        x = num11 - rectangleF.Left;
      if ((double) rectangleF.Y > (double) num6 && this._pdfDocument.Pages.Count > 1)
      {
        rectangleF.Y -= x + x / 2f - rectangleF.Height;
        y1 = startY;
      }
      rectangleF.Offset(x, y1);
    }
    else
    {
      if ((double) num6 > 0.0 && (!this.IsPrintTitleRowPage || this.SheetPageSetup.Orientation != ExcelPageOrientation.Portrait || (double) num5 > 0.0))
        num6 -= startY;
      if ((double) num5 > 0.0 && this._isNewPage)
        num6 += startY;
      rectangleF.Offset(-num5, -num6);
    }
    graphics.ResetTransform();
    CommentShapeImpl comment = shape as CommentShapeImpl;
    CheckBoxShapeImpl checkBox = shape as CheckBoxShapeImpl;
    OptionButtonShapeImpl optionButton = shape as OptionButtonShapeImpl;
    ComboBoxShapeImpl comboBox = shape as ComboBoxShapeImpl;
    TextBoxShapeImpl textBoxShapeImpl = shape as TextBoxShapeImpl;
    IRichTextString richText = (IRichTextString) null;
    PdfPen pen = this.CreatePen(shape, shape.Line);
    PdfPath pdfPath = new PdfPath();
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.Black);
    double weight = shape.Line.Weight;
    float x1 = rectangleF.X;
    double y2 = (double) rectangleF.Y;
    if (comment == null && checkBox == null && optionButton == null && comboBox == null)
    {
      rectangleF = shape.UpdateShapeBounds(rectangleF, shape.GetShapeRotation());
      this.Rotate(graphics, shape, rectangleF);
    }
    if (this._excelToPdfSettings.EnableFormFields && comment == null && !(shape is AutoShapeImpl))
    {
      if (this.HasHeader)
        rectangleF.Y += this._topMargin;
      switch (shape)
      {
        case TextBoxShapeImpl _:
          PdfTextBoxField field1 = new PdfTextBoxField((PdfPageBase) this._currentPage, shape.Name);
          field1.Text = textBoxShapeImpl.Text;
          field1.BackColor = new PdfColor(Color.FromArgb((int) byte.MaxValue, (int) textBoxShapeImpl.Fill.ForeColor.R, (int) textBoxShapeImpl.Fill.ForeColor.G, (int) textBoxShapeImpl.Fill.ForeColor.B));
          if ((double) this._centerHorizontalValue != 0.0)
            rectangleF.X += (float) (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.LeftMarginPt;
          field1.Bounds = rectangleF;
          if (!string.IsNullOrEmpty(textBoxShapeImpl.RichText.Text))
          {
            IFont font = textBoxShapeImpl.RichText.GetFont(0);
            PdfTrueTypeFont pdfTrueTypeFont = new PdfTrueTypeFont(new Font(font.FontName, (float) font.Size));
            field1.Font = (PdfFont) pdfTrueTypeFont;
            field1.ForeColor = new PdfColor(Color.FromArgb((int) byte.MaxValue, (int) font.RGBColor.R, (int) font.RGBColor.G, (int) font.RGBColor.B));
          }
          field1.BorderWidth = (float) (int) textBoxShapeImpl.Line.Weight;
          if (textBoxShapeImpl.Line.Weight > 0.0 && textBoxShapeImpl.Line.Weight < 1.0)
            field1.BorderWidth = 1f;
          field1.BorderColor = new PdfColor(Color.FromArgb((int) byte.MaxValue, (int) textBoxShapeImpl.Line.ForeColor.R, (int) textBoxShapeImpl.Line.ForeColor.G, (int) textBoxShapeImpl.Line.ForeColor.B));
          if (textBoxShapeImpl.Line.DashStyle == ExcelShapeDashLineStyle.Dashed)
            field1.BorderStyle = PdfBorderStyle.Dashed;
          this._pdfDocument.Form.Fields.Add((PdfField) field1);
          break;
        case ComboBoxShapeImpl _:
          PdfComboBoxField field2 = new PdfComboBoxField((PdfPageBase) this._currentPage, shape.Name);
          field2.Bounds = rectangleF;
          PdfTrueTypeFont pdfTrueTypeFont1 = new PdfTrueTypeFont(new Font("Segoe UI", 8f));
          field2.Font = (PdfFont) pdfTrueTypeFont1;
          for (int index = 0; index < comboBox.ListFillRange.Count; ++index)
          {
            if (comboBox.ListFillRange.Cells[index].Text != null)
              field2.Items.Add(new PdfListFieldItem(comboBox.ListFillRange.Cells[index].Text, "Value " + (object) index));
          }
          field2.BorderColor = (PdfColor) Color.FromArgb(172, 172, 172);
          this._pdfDocument.Form.Fields.Add((PdfField) field2);
          break;
        case CheckBoxShapeImpl _:
          float x2 = rectangleF.X + 2.7f;
          float y3 = (float) ((double) rectangleF.Y + (double) rectangleF.Height / 2.0 - 3.2999999523162842);
          PdfCheckBoxField field3 = new PdfCheckBoxField((PdfPageBase) this._currentPage, shape.Name);
          field3.Bounds = new RectangleF(x2, y3, 6.5f, 6.5f);
          if (checkBox.CheckState == ExcelCheckState.Checked)
            field3.Checked = true;
          this._pdfDocument.Form.Fields.Add((PdfField) field3);
          break;
        case OptionButtonShapeImpl _:
          float x3 = rectangleF.X + 1f;
          float y4 = (float) ((double) rectangleF.Y + (double) rectangleF.Height / 2.0 - 4.1999998092651367);
          PdfRadioButtonListField field4 = new PdfRadioButtonListField((PdfPageBase) this._currentPage, shape.Name);
          this._pdfDocument.Form.Fields.Add((PdfField) field4);
          PdfRadioButtonListItem radioButtonListItem = new PdfRadioButtonListItem();
          radioButtonListItem.Bounds = new RectangleF(x3, y4, 8.2f, 8.2f);
          field4.Items.Add(radioButtonListItem);
          if (optionButton.CheckState == ExcelCheckState.Checked)
          {
            field4.SelectedIndex = 0;
            break;
          }
          break;
      }
      if (optionButton == null && checkBox == null)
        return;
      rectangleF.X -= (float) weight / 2f;
      rectangleF.Y -= (float) weight / 2f;
      pdfPath.AddRectangle(rectangleF);
      this.DrawShapeFillAndLine(pdfPath, shape, pen, graphics, rectangleF, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
      rectangleF.X = x1;
      this.DrawShapeRTFText(richText, shape, rectangleF, graphics);
    }
    else
    {
      if (shape is TextBoxShapeImpl || comment != null || shape is CheckBoxShapeImpl || optionButton != null || comboBox != null)
      {
        if (optionButton != null || checkBox != null)
        {
          rectangleF.X -= (float) weight / 2f;
          rectangleF.Y -= (float) weight / 2f;
        }
        pdfPath.AddRectangle(rectangleF);
        if (comment != null)
        {
          PdfPath commentPath;
          PdfPen commentPen;
          this.GetCommentPath(rectangleF, comment, pdfPath, pdfBrush, out commentPath, out commentPen);
          this.DrawShapeFillAndLine(commentPath, shape, commentPen, graphics, rectangleF, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
        }
        else if (comboBox != null)
        {
          shape.Line.Visible = true;
          pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb(172, 172, 172));
          comboBox.Fill.ForeColor = Color.White;
          pen = new PdfPen(pdfBrush, 0.4f);
        }
      }
      else
        pdfPath = this.GetGraphicsPath(rectangleF, ref pen, graphics, shape as AutoShapeImpl);
      this.DrawShapeFillAndLine(pdfPath, shape, pen, graphics, rectangleF, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
      if (shape is ComboBoxShapeImpl)
      {
        RectangleF rect1;
        this.GetComboBoxPath(rectangleF, comboBox, pdfPath, pdfBrush, out PdfPen _, graphics, out rect1, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
        rectangleF.Width -= rect1.Width;
      }
      else if (shape is CheckBoxShapeImpl)
      {
        rectangleF.X = x1;
        this.GetCheckBoxPath(rectangleF, checkBox, pdfPath, pdfBrush, out PdfPen _, graphics, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
      }
      else if (shape is OptionButtonShapeImpl)
      {
        rectangleF.X = x1;
        this.GetOptionButtonPath(rectangleF, optionButton, pdfPath, pdfBrush, out PdfPen _, out PdfPen _, graphics, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
      }
      if (shape is TextBoxShapeImpl && textBoxShapeImpl.TextLink != null)
      {
        string textLink = textBoxShapeImpl.TextLink;
        int num12 = 0;
        string str = textLink.Remove(0, 1);
        for (int index1 = 0; index1 < this._workBook.Names.Count; ++index1)
        {
          if (str == this._workBook.Names[index1].Name)
          {
            for (int index2 = 0; index2 < textBoxShapeImpl.Text.Length && textBoxShapeImpl.Text[index2] == ' '; ++index2)
              ++num12;
            textBoxShapeImpl.Text = (this._workBook.Names[index1] as NameImpl).DisplayText.TrimStart();
            if (num12 > 0)
            {
              for (int index3 = 0; index3 < num12; ++index3)
                textBoxShapeImpl.Text = textBoxShapeImpl.Text.Insert(0, " ");
            }
            textBoxShapeImpl.RichText.Text = textBoxShapeImpl.Text;
          }
        }
      }
      if (shape is TextBoxShapeImpl)
        richText = (shape as TextBoxShapeImpl).RichText;
      else if (shape is CommentShapeImpl)
        richText = (shape as CommentShapeImpl).RichText;
      else if (shape is AutoShapeImpl)
        richText = (shape as AutoShapeImpl).TextFrame.TextRange.RichText;
      this.DrawShapeRTFText(richText, shape, rectangleF, graphics);
      this.SetShapeHyperlink(shape, rectangleF);
      graphics.ResetTransform();
    }
  }

  private void GetCommentPath(
    RectangleF rect,
    CommentShapeImpl comment,
    PdfPath pdfPath,
    PdfBrush pdfBrush,
    out PdfPath commentPath,
    out PdfPen commentPen)
  {
    commentPath = new PdfPath();
    commentPen = new PdfPen(pdfBrush, 1.9f);
    RectangleF rectangleF = this._commentCellPosition[RangeImpl.GetCellIndex(comment.Column, comment.Row)];
    double num1 = 180.0 / Math.PI;
    float x_pivot = rectangleF.X + rectangleF.Width;
    float y = rectangleF.Y;
    float num2 = (double) rectangleF.X + (double) rectangleF.Width < (double) rect.X ? rect.X : rect.X + rect.Width;
    float angle = (float) (Math.Atan2(((double) rectangleF.Y <= (double) rect.Y + (double) rect.Height ? (double) rect.Y : (double) (rect.Y + rect.Height)) - (double) y, (double) num2 - (double) x_pivot) * num1);
    int n = 3;
    float num3 = (float) ((double) rectangleF.X + (double) rectangleF.Width + 2.0);
    float num4 = rectangleF.Y + 0.4f;
    float[,] points1;
    if ((double) angle <= 0.0 && (double) angle > -90.0)
      points1 = new float[3, 2]
      {
        {
          num3,
          num4
        },
        {
          num3 + 2.5f,
          num4 + 1.3f
        },
        {
          num3 + 2.5f,
          num4 - 1.3f
        }
      };
    else if ((double) angle < -90.0 && (double) angle > -180.0)
    {
      float num5 = num3 - 0.5f;
      float num6 = num4 - 0.5f;
      points1 = new float[3, 2]
      {
        {
          num5,
          num6
        },
        {
          num5 + 2.5f,
          num6 + 1.3f
        },
        {
          num5 + 2.5f,
          num6 - 1.3f
        }
      };
    }
    else if ((double) angle > 90.0 && (double) angle <= 180.0)
    {
      float num7 = num4 - 1f;
      points1 = new float[3, 2]
      {
        {
          num3,
          num7
        },
        {
          num3 + 2.5f,
          num7 + 1.3f
        },
        {
          num3 + 2.5f,
          num7 - 1.3f
        }
      };
    }
    else
    {
      float num8 = num3 + 0.5f;
      float num9 = num4 - 0.3f;
      points1 = new float[3, 2]
      {
        {
          num8,
          num9
        },
        {
          num8 + 2.5f,
          num9 + 1.3f
        },
        {
          num8 + 2.5f,
          num9 - 1.3f
        }
      };
    }
    float[,] numArray = this.Rotate(points1, n, x_pivot, y, angle);
    PointF[] points2 = new PointF[3]
    {
      new PointF(numArray[0, 0], numArray[0, 1]),
      new PointF(numArray[1, 0], numArray[1, 1]),
      new PointF(numArray[2, 0], numArray[2, 1])
    };
    commentPath.AddPolygon(points2);
    float x1 = (double) rectangleF.X + (double) rectangleF.Width < (double) rect.X ? rect.X : rect.X + rect.Width;
    float y1 = (double) rectangleF.Y <= (double) rect.Y + (double) rect.Height ? rect.Y : rect.Y + rect.Height;
    float x2 = (float) (((double) numArray[2, 0] + (double) numArray[1, 0]) / 2.0);
    float y2 = (float) (((double) numArray[2, 1] + (double) numArray[1, 1]) / 2.0);
    if ((double) angle < 0.0 && (double) rect.X < (double) x2 && (double) rect.X + (double) rect.Width > (double) x2 && (double) rect.Y < (double) y2 && (double) rect.Y + (double) rect.Width > (double) y2)
      return;
    pdfPath.AddLine(x1, y1, x2, y2);
  }

  private float[,] Rotate(float[,] points, int n, float x_pivot, float y_pivot, float angle)
  {
    int index = 0;
    float[,] numArray = new float[3, 2];
    for (; index < n; ++index)
    {
      float num1 = points[index, 0] - x_pivot;
      float num2 = points[index, 1] - y_pivot;
      points[index, 0] = x_pivot + (float) ((double) num1 * Math.Cos((double) angle * Math.PI / 180.0) - (double) num2 * Math.Sin((double) angle * Math.PI / 180.0));
      points[index, 1] = y_pivot + (float) ((double) num1 * Math.Sin((double) angle * Math.PI / 180.0) + (double) num2 * Math.Cos((double) angle * Math.PI / 180.0));
      numArray[index, 0] = points[index, 0];
      numArray[index, 1] = points[index, 1];
    }
    return numArray;
  }

  private void GetComboBoxPath(
    RectangleF rect,
    ComboBoxShapeImpl comboBox,
    PdfPath pdfPath,
    PdfBrush pdfBrush,
    out PdfPen comboBoxPen,
    PdfGraphics graphics,
    out RectangleF rect1,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    PdfPath pdfPath1 = new PdfPath();
    PdfPath pdfPath2 = new PdfPath();
    pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb(172, 172, 172));
    comboBoxPen = new PdfPen(pdfBrush, 0.4f);
    comboBox.Fill.Visible = true;
    comboBox.Fill.ForeColor = Color.FromArgb(224 /*0xE0*/, 224 /*0xE0*/, 224 /*0xE0*/);
    float y = rect.Y;
    float num;
    if ((double) rect.Height < (double) rect.Width)
    {
      float x = rect.X + rect.Width - rect.Height;
      num = rect.Height;
      rect1 = new RectangleF(x, y, rect.Height, rect.Height);
    }
    else
    {
      float x = rect.X;
      num = rect.Width;
      rect1 = new RectangleF(x, y, rect.Width, rect.Height);
    }
    pdfPath1.AddRectangle(rect1);
    this.DrawShapeFillAndLine(pdfPath1, (ShapeImpl) comboBox, comboBoxPen, graphics, rect1, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
    PointF[] points = new PointF[3]
    {
      new PointF(rect1.X + num / 2f, (float) ((double) rect1.Y + (double) rect1.Height - ((double) rect1.Height - (double) num / 4.0 + 0.10000000149011612) / 2.0)),
      new PointF(rect1.X + num / 4f, rect1.Y + (float) (((double) rect1.Height - (double) num / 4.0 + 0.10000000149011612) / 2.0)),
      new PointF(rect1.X + (float) (3.0 * (double) num / 4.0), rect1.Y + (float) (((double) rect1.Height - (double) num / 4.0 + 0.10000000149011612) / 2.0))
    };
    pdfPath2.AddPolygon(points);
    pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.Empty);
    comboBoxPen = new PdfPen(pdfBrush, 0.2f);
    comboBox.Fill.ForeColor = ColorExtension.Black;
    this.DrawShapeFillAndLine(pdfPath2, (ShapeImpl) comboBox, comboBoxPen, graphics, rect1, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
  }

  private void GetCheckBoxPath(
    RectangleF rect,
    CheckBoxShapeImpl checkBox,
    PdfPath pdfPath,
    PdfBrush pdfBrush,
    out PdfPen checkBoxPen,
    PdfGraphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    PdfPath pdfPath1 = new PdfPath();
    checkBoxPen = new PdfPen(pdfBrush, 0.4f);
    checkBox.Fill.Visible = true;
    checkBox.Fill.ForeColor = ColorExtension.White;
    checkBox.Line.Visible = true;
    float x = rect.X + 2.7f;
    float y = (float) ((double) rect.Y + (double) rect.Height / 2.0 - 3.0);
    RectangleF rectangleF = new RectangleF(x, y, 6f, 6f);
    pdfPath1.AddRectangle(rectangleF);
    this.DrawShapeFillAndLine(pdfPath1, (ShapeImpl) checkBox, checkBoxPen, graphics, rectangleF, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
    if (checkBox.CheckState == ExcelCheckState.Checked)
    {
      float num1 = x + 0.4f;
      float num2 = y + 0.3f;
      pdfPath1.AddLine(num1 + 1.5f, num2 + 3f, num1 + 2.2f, num2 + 4f);
      pdfPath1.AddLine(num1 + 2.2f, num2 + 4f, num1 + 4f, num2 + 1.4f);
    }
    if (checkBox.CheckState == ExcelCheckState.Mixed)
      checkBox.Fill.ForeColor = ColorExtension.DarkGray;
    this.DrawShapeFillAndLine(pdfPath1, (ShapeImpl) checkBox, checkBoxPen, graphics, rectangleF, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
  }

  private void GetOptionButtonPath(
    RectangleF rect,
    OptionButtonShapeImpl optionButton,
    PdfPath pdfPath,
    PdfBrush pdfBrush,
    out PdfPen OptionButtonPen,
    out PdfPen OptionButtonPen1,
    PdfGraphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    PdfPath pdfPath1 = new PdfPath();
    PdfPath pdfPath2 = new PdfPath();
    OptionButtonPen = new PdfPen(pdfBrush, 0.4f);
    OptionButtonPen1 = new PdfPen(pdfBrush, 0.4f);
    optionButton.Fill.Visible = true;
    optionButton.Fill.ForeColor = ColorExtension.White;
    optionButton.Line.Visible = true;
    float x = rect.X + 3f;
    float y = (float) ((double) rect.Y + (double) rect.Height / 2.0 - 2.9000000953674316);
    RectangleF rectangleF1 = new RectangleF(x, y, 5.8f, 5.8f);
    RectangleF rectangleF2 = new RectangleF(x + 1.1f, y + 1.1f, 3.6f, 3.6f);
    pdfPath1.AddEllipse(rectangleF1);
    this.DrawShapeFillAndLine(pdfPath1, (ShapeImpl) optionButton, OptionButtonPen, graphics, rectangleF1, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
    if (optionButton.CheckState == ExcelCheckState.Checked)
    {
      pdfPath2.AddEllipse(rectangleF2);
      optionButton.Fill.ForeColor = ColorExtension.Black;
    }
    this.DrawShapeFillAndLine(pdfPath2, (ShapeImpl) optionButton, OptionButtonPen1, graphics, rectangleF2, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
  }

  private bool IsLine(ShapeImpl shape)
  {
    if (shape is AutoShapeImpl)
    {
      switch ((shape as AutoShapeImpl).ShapeExt.AutoShapeType)
      {
        case AutoShapeType.FlowChartConnector:
        case AutoShapeType.FlowChartOffPageConnector:
        case AutoShapeType.Line:
        case AutoShapeType.StraightConnector:
        case AutoShapeType.ElbowConnector:
        case AutoShapeType.CurvedConnector:
        case AutoShapeType.BentConnector2:
        case AutoShapeType.BentConnector4:
        case AutoShapeType.CurvedConnector2:
        case AutoShapeType.CurvedConnector4:
        case AutoShapeType.CurvedConnector5:
          return true;
      }
    }
    return false;
  }

  private void DrawShapeRTFText(
    IRichTextString richText,
    ShapeImpl shape,
    RectangleF rect,
    PdfGraphics graphics)
  {
    bool isWrapText = true;
    bool isVerticalTextOverflow = false;
    bool isHorizontalTextOverflow = false;
    List<IFont> richTextFonts = new List<IFont>();
    List<string> drawString = new List<string>();
    PdfStringFormat format1 = new PdfStringFormat();
    OptionButtonShapeImpl optionButtonShapeImpl = shape as OptionButtonShapeImpl;
    ComboBoxShapeImpl comboBoxShapeImpl = shape as ComboBoxShapeImpl;
    CheckBoxShapeImpl checkBoxShapeImpl = shape as CheckBoxShapeImpl;
    this._workBookImpl = (WorkbookImpl) this._workBook;
    bool flag = false;
    if (!(shape is OptionButtonShapeImpl) && !(shape is ComboBoxShapeImpl) && checkBoxShapeImpl == null && string.IsNullOrEmpty(richText.Text))
      return;
    this._currentCellRect = this.GetBoundsToLayoutShapeTextBody(shape as AutoShapeImpl, rect);
    if (optionButtonShapeImpl != null || shape is CheckBoxShapeImpl)
    {
      this._currentCellRect.X += 12.8f;
      this._currentCellRect.Width -= 6.8f;
    }
    if (comboBoxShapeImpl != null)
    {
      this._currentCellRect.X += 1.5f;
      this._currentCellRect.Width -= 1.5f;
    }
    if (!(shape is OptionButtonShapeImpl) && !(shape is ComboBoxShapeImpl) && checkBoxShapeImpl == null)
    {
      this.UpdateShapeBoundsToLayoutTextBody(ref this._currentCellRect, rect, shape);
      if (shape is TextBoxShapeImpl)
      {
        string textVerticalType = ((Excel2007TextRotation) (shape as TextBoxShapeImpl).TextRotation).ToString();
        (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.TextDirection = Helper.SetTextDirection(textVerticalType);
      }
      TextDirection textDirection = shape is AutoShapeImpl ? shape.TextFrame.TextDirection : (shape is CommentShapeImpl ? (shape as CommentShapeImpl).TextBodyPropertiesHolder.TextDirection : (shape is TextBoxShapeImpl ? (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.TextDirection : TextDirection.Horizontal));
      switch (textDirection)
      {
        case TextDirection.RotateAllText90:
        case TextDirection.RotateAllText270:
          float width1 = this._currentCellRect.Width;
          this._currentCellRect.Width = this._currentCellRect.Height;
          this._currentCellRect.Height = width1;
          break;
      }
      float shapeRotation = (float) shape.ShapeRotation;
      this.ApplyRotation(shape, rect, shapeRotation, graphics);
      this.RotateText(this._currentCellRect, textDirection);
      IFont font1 = richText.GetFont(0);
      format1.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
      format1.Alignment = this.GetTextAlignmentFromShape((IShape) shape);
      bool isEmbedFont = this._excelToPdfSettings.EmbedFonts || Encoding.UTF8.GetByteCount(richText.Text) != richText.Text.Length;
      this.GetPdfTrueTypeFont(this.GetFont(font1, font1.FontName, (int) font1.Size), isEmbedFont, (Stream) null);
      format1.LineAlignment = this.GetVerticalAlignmentFromShape((IShape) shape);
      if ((double) this.GetRotationAngle(textDirection) > 0.0)
        this.UpdatePdfAlignment(format1, (int) this.GetRotationAngle(textDirection), shape);
      this._pdfTextformat = format1;
      ushort[] numArray = new ushort[richText.Text.Length];
      KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, richText.Text, richText.Text.Length, numArray);
      if (this._pdfTextformat.RightToLeft = this.CheckIfRTL(numArray) || this.CheckForArabicOrHebrew(richText.Text))
        this._pdfTextformat.TextDirection = PdfTextDirection.RightToLeft;
      drawString = this._workBookImpl.GetDrawString(richText.Text, richText as RichTextString, out richTextFonts, richText.GetFont(0));
      if (drawString.Count > 1 && drawString[drawString.Count - 1].Equals("\n"))
        drawString.RemoveAt(drawString.Count - 1);
      if (shape is AutoShapeImpl)
      {
        for (int index = 0; index < richTextFonts.Count; ++index)
        {
          if (this.FontColorNeedsUpdation(richTextFonts[index]))
          {
            if (richTextFonts[index] is FontWrapper parent)
            {
              richTextFonts[index] = (IFont) new FontWrapper(parent.Font.Clone((object) parent), false, false)
              {
                RGBColor = shape.GetDefaultColor(PreservedFlag.RichText, "fontRef")
              };
            }
            else
            {
              FontImpl fontImpl = (richTextFonts[index] as FontImpl).Clone() as FontImpl;
              fontImpl.RGBColor = shape.GetDefaultColor(PreservedFlag.RichText, "fontRef");
              richTextFonts[index] = (IFont) fontImpl;
            }
          }
        }
      }
      if (shape is CommentShapeImpl)
      {
        for (int index = 0; index < richTextFonts.Count; ++index)
        {
          if (richTextFonts[index].Color == (ExcelKnownColors.Green | ExcelKnownColors.BlackCustom))
          {
            if (richTextFonts[index] is FontWrapper parent)
            {
              richTextFonts[index] = (IFont) new FontWrapper(parent.Font.Clone((object) parent), false, false)
              {
                RGBColor = shape.GetDefaultColor(PreservedFlag.RichText, "fontRef")
              };
            }
            else
            {
              FontImpl fontImpl = (richTextFonts[index] as FontImpl).Clone() as FontImpl;
              fontImpl.RGBColor = shape.GetDefaultColor(PreservedFlag.RichText, "fontRef");
              richTextFonts[index] = (IFont) fontImpl;
            }
          }
        }
      }
      if (shape is AutoShapeImpl)
      {
        AutoShapeImpl autoShapeImpl = shape as AutoShapeImpl;
        if (autoShapeImpl.TextFrameInternal != null && autoShapeImpl.TextFrameInternal.TextBodyProperties != null)
        {
          isWrapText = autoShapeImpl.TextFrameInternal.TextBodyProperties.WrapTextInShape;
          if (!isWrapText)
            isWrapText = false;
          if (autoShapeImpl.TextFrameInternal.TextBodyProperties.TextVertOverflowType == TextVertOverflowType.OverFlow)
            isVerticalTextOverflow = true;
          if (autoShapeImpl.TextFrameInternal.TextBodyProperties.TextHorzOverflowType == TextHorzOverflowType.OverFlow)
            isHorizontalTextOverflow = true;
          if (autoShapeImpl.TextFrameInternal.TextBodyProperties.PresetWrapTextInShape)
          {
            double leftMarginPt = (shape.TextFrame as TextFrame).TextBodyProperties.LeftMarginPt;
            double rightMarginPt = (shape.TextFrame as TextFrame).TextBodyProperties.RightMarginPt;
            double topMarginPt = (shape.TextFrame as TextFrame).TextBodyProperties.TopMarginPt;
            double bottomMarginPt = (shape.TextFrame as TextFrame).TextBodyProperties.BottomMarginPt;
            StringFormat format2 = new StringFormat(StringFormatFlags.NoWrap);
            format2.Alignment = StringAlignment.Near;
            format2.LineAlignment = StringAlignment.Near;
            flag = true;
            string text = richText.Text;
            Bitmap bitmap1 = new Bitmap(1, 1);
            string fontName = richTextFonts[0].FontName;
            int emSize = int.Parse(richTextFonts[0].Size.ToString());
            FontStyle style = FontStyle.Regular;
            if (richTextFonts[0].Bold)
              style |= FontStyle.Bold;
            if (richTextFonts[0].Italic)
              style |= FontStyle.Italic;
            if (richTextFonts[0].Underline != ExcelUnderline.None)
              style |= FontStyle.Underline;
            if (richTextFonts[0].Strikethrough)
              style |= FontStyle.Strikeout;
            Font font2 = new Font(fontName, (float) emSize, style, GraphicsUnit.Pixel);
            System.Drawing.Graphics graphics1 = System.Drawing.Graphics.FromImage((Image) bitmap1);
            int width2 = (int) graphics1.MeasureString(text, font2).Width;
            int height = (int) graphics1.MeasureString(text, font2).Height;
            int num = 1;
            while ((double) rect.Height > (double) height || (double) rect.Width > (double) width2)
            {
              font2 = new Font(fontName, (float) (emSize + num), style, GraphicsUnit.Pixel);
              width2 = (int) graphics1.MeasureString(text, font2).Width;
              height = (int) graphics1.MeasureString(text, font2).Height;
              ++num;
            }
            Bitmap bitmap2 = new Bitmap(width2, height, PixelFormat.Format32bppArgb);
            System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage((Image) bitmap2);
            graphics2.Clear(Color.Transparent);
            graphics2.CompositingQuality = CompositingQuality.HighQuality;
            graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics2.SmoothingMode = SmoothingMode.HighQuality;
            graphics2.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            Brush brush = (Brush) new SolidBrush(Color.FromArgb((int) richTextFonts[0].RGBColor.R, (int) richTextFonts[0].RGBColor.G, (int) richTextFonts[0].RGBColor.B));
            graphics2.DrawString(text, font2, brush, 0.0f, 0.0f, format2);
            graphics2.Flush();
            graphics2.Dispose();
            graphics.DrawImage(this.GetPdfImage((Image) bitmap2), new PointF(rect.X, rect.Y), new SizeF(rect.Width, rect.Height));
          }
        }
      }
      else
      {
        string empty = string.Empty;
        TextBoxShapeBase textBoxShapeBase = shape as TextBoxShapeBase;
        if (textBoxShapeBase.UnknownBodyProperties != null)
        {
          textBoxShapeBase.UnknownBodyProperties.TryGetValue("wrap", out empty);
          if (empty == "none")
            isWrapText = false;
          textBoxShapeBase.UnknownBodyProperties.TryGetValue("vertOverflow", out empty);
          if (empty == "overflow")
            isVerticalTextOverflow = true;
          textBoxShapeBase.UnknownBodyProperties.TryGetValue("horzOverflow", out empty);
          if (empty == "overflow")
            isHorizontalTextOverflow = true;
        }
      }
    }
    else
    {
      if (optionButtonShapeImpl != null)
        drawString.Add(optionButtonShapeImpl.Text);
      else if (checkBoxShapeImpl != null)
      {
        drawString.Add(checkBoxShapeImpl.Text);
      }
      else
      {
        drawString.Add(comboBoxShapeImpl.SelectedValue);
        isWrapText = false;
      }
      format1.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
      format1.Alignment = this.GetTextAlignmentFromShape((IShape) shape);
      format1.LineAlignment = this.GetVerticalAlignmentFromShape((IShape) shape);
      this._pdfTextformat = format1;
      if (this.style == null)
      {
        this.style = this._workBook.Styles.Add("NewStyle");
        this.style.Color = Color.Black;
        this.style.Font.FontName = "Segoe UI";
        this.style.Font.Size = 8.0;
      }
      IFont font = this.style.Font;
      richTextFonts.Add(font);
    }
    if (flag)
      return;
    this.DrawRTFText(this._currentCellRect, this._currentCellRect, graphics, richTextFonts, drawString, true, isWrapText, isHorizontalTextOverflow, isVerticalTextOverflow, false);
  }

  private void UpdateShapeBoundsToLayoutTextBody(
    ref RectangleF layoutRect,
    RectangleF shapeBounds,
    ShapeImpl shape)
  {
    float num1 = 7.2f;
    float num2 = 7.2f;
    float num3 = 3.6f;
    float num4 = 3.6f;
    switch (shape)
    {
      case AutoShapeImpl _:
        layoutRect.Height -= layoutRect.Y;
        layoutRect.Y += shapeBounds.Y;
        layoutRect.Width -= layoutRect.X;
        layoutRect.X += shapeBounds.X;
        num1 = (float) (shape.TextFrame as TextFrame).TextBodyProperties.LeftMarginPt;
        num2 = (float) (shape.TextFrame as TextFrame).TextBodyProperties.RightMarginPt;
        num3 = (float) (shape.TextFrame as TextFrame).TextBodyProperties.TopMarginPt;
        num4 = (float) (shape.TextFrame as TextFrame).TextBodyProperties.BottomMarginPt;
        break;
      case TextBoxShapeImpl _:
        num1 = (float) (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.LeftMarginPt;
        num2 = (float) (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.RightMarginPt;
        num3 = (float) (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.TopMarginPt;
        num4 = (float) (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.BottomMarginPt;
        break;
      case CommentShapeImpl _:
        num1 = 2f;
        num2 = 2f;
        num3 = 0.0f;
        num4 = 0.0f;
        break;
    }
    layoutRect.X += num1;
    layoutRect.Y += num3;
    layoutRect.Width -= num1 + num2;
    layoutRect.Height -= num3 + num4;
  }

  private RectangleF GetBoundsToLayoutShapeTextBody(AutoShapeImpl shapeImpl, RectangleF bounds)
  {
    if (shapeImpl == null)
      return bounds;
    Dictionary<string, float> shapeFormula = new ShapePath(bounds, shapeImpl.ShapeExt.ShapeGuide).ParseShapeFormula(shapeImpl.ShapeExt.AutoShapeType);
    switch (shapeImpl.ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Parallelogram:
      case AutoShapeType.Hexagon:
      case AutoShapeType.Cross:
      case AutoShapeType.SmileyFace:
      case AutoShapeType.NoSymbol:
      case AutoShapeType.FlowChartTerminator:
      case AutoShapeType.FlowChartSummingJunction:
      case AutoShapeType.FlowChartOr:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.Wave:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.SnipSameSideCornerRectangle:
      case AutoShapeType.Teardrop:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Trapezoid:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
      case AutoShapeType.FlowChartCollate:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Octagon:
      case AutoShapeType.Plaque:
      case AutoShapeType.RoundedRectangularCallout:
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.IsoscelesTriangle:
        return new RectangleF(shapeFormula["x1"], bounds.Height / 2f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.RightTriangle:
        return new RectangleF(bounds.Width / 12f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Oval:
      case AutoShapeType.Donut:
      case AutoShapeType.BlockArc:
      case AutoShapeType.Arc:
      case AutoShapeType.CircularArrow:
      case AutoShapeType.FlowChartConnector:
      case AutoShapeType.FlowChartSequentialAccessStorage:
      case AutoShapeType.DoubleWave:
      case AutoShapeType.CloudCallout:
      case AutoShapeType.Chord:
      case AutoShapeType.Cloud:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RegularPentagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["it"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Can:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.Cube:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.Bevel:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.FoldedCorner:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.Heart:
        return new RectangleF(shapeFormula["il"], bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LightningBolt:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y4"], shapeFormula["x9"], shapeFormula["y10"]);
      case AutoShapeType.Sun:
        return new RectangleF(shapeFormula["x9"], shapeFormula["y9"], shapeFormula["x8"], shapeFormula["y8"]);
      case AutoShapeType.Moon:
        return new RectangleF(shapeFormula["g12w"], shapeFormula["g15h"], shapeFormula["g0w"], shapeFormula["g16h"]);
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.FlowChartAlternateProcess:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LeftBracket:
      case AutoShapeType.LeftBrace:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.RightBracket:
      case AutoShapeType.RightBrace:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RightArrow:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.UpArrow:
      case AutoShapeType.MathEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], bounds.Height);
      case AutoShapeType.DownArrow:
        return new RectangleF(shapeFormula["x1"], 0.0f, shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y2"]);
      case AutoShapeType.UpDownArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y4"]);
      case AutoShapeType.QuadArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y4"]);
      case AutoShapeType.LeftRightUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y5"]);
      case AutoShapeType.UTurnArrow:
      case AutoShapeType.FlowChartProcess:
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.StraightConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.LeftUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["x4"], shapeFormula["y5"]);
      case AutoShapeType.BentUpArrow:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.StripedRightArrow:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y1"], shapeFormula["x6"], shapeFormula["y2"]);
      case AutoShapeType.NotchedRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Pentagon:
      case AutoShapeType.RoundSingleCornerRectangle:
        return new RectangleF(0.0f, 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Chevron:
        return new RectangleF(shapeFormula["il"], 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.RightArrowCallout:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.LeftArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.UpArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, bounds.Height);
      case AutoShapeType.DownArrowCallout:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.UpDownArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.QuadArrowCallout:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x7"], shapeFormula["y7"]);
      case AutoShapeType.FlowChartData:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x5"], bounds.Height);
      case AutoShapeType.FlowChartPredefinedProcess:
        return new RectangleF(bounds.Width / 8f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartInternalStorage:
        return new RectangleF(bounds.Width / 8f, bounds.Height / 8f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartDocument:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartMultiDocument:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x5"], shapeFormula["y8"]);
      case AutoShapeType.FlowChartPreparation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartManualInput:
      case AutoShapeType.FlowChartCard:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartManualOperation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.FlowChartOffPageConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartPunchedTape:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.FlowChartSort:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartExtract:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 2f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartMerge:
        return new RectangleF(bounds.Width / 4f, 0.0f, shapeFormula["x2"], bounds.Height / 2f);
      case AutoShapeType.FlowChartStoredData:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDelay:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartMagneticDisk:
        return new RectangleF(0.0f, bounds.Height / 3f, bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.FlowChartDirectAccessStorage:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDisplay:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.Explosion1:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x21"], shapeFormula["y9"]);
      case AutoShapeType.Explosion2:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x19"], shapeFormula["y17"]);
      case AutoShapeType.Star4Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx2"], shapeFormula["sy2"]);
      case AutoShapeType.Star5Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy3"]);
      case AutoShapeType.Star8Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy4"]);
      case AutoShapeType.UpRibbon:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x9"], shapeFormula["y2"]);
      case AutoShapeType.DownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x9"], bounds.Height);
      case AutoShapeType.CurvedUpRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y6"], shapeFormula["x5"], shapeFormula["rh"]);
      case AutoShapeType.CurvedDownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["q1"], shapeFormula["x5"], shapeFormula["y6"]);
      case AutoShapeType.VerticalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x6"], shapeFormula["y4"]);
      case AutoShapeType.HorizontalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x4"], shapeFormula["y6"]);
      case AutoShapeType.DiagonalStripe:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x3"], shapeFormula["y3"]);
      case AutoShapeType.Pie:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Decagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.Heptagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y1"], shapeFormula["x5"], shapeFormula["ib"]);
      case AutoShapeType.Dodecagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.Star6Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy2"]);
      case AutoShapeType.Star7Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy1"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star10Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star12Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy5"]);
      case AutoShapeType.RoundSameSideCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["tdx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["dx"], shapeFormula["dx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.SnipSingleCornerRectangle:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Frame:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.L_Shape:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.MathPlus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.MathMinus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.MathMultiply:
        return new RectangleF(shapeFormula["xA"], shapeFormula["yB"], shapeFormula["xE"], shapeFormula["yH"]);
      case AutoShapeType.MathDivision:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y3"], shapeFormula["x3"], shapeFormula["y4"]);
      case AutoShapeType.MathNotEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x8"], shapeFormula["y4"]);
      default:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
    }
  }

  private void SetCustomGeometry(string pathList, ShapeImpl shapeImpl)
  {
    Stream input = (Stream) new MemoryStream(Encoding.UTF8.GetBytes(pathList.Replace('\'', ' ')));
    input.Position = 0L;
    XmlReader reader = XmlReader.Create(input);
    (shapeImpl as AutoShapeImpl).ShapeExt.Path2DList = new List<Path2D>();
    ShapeParser.ParsePath2D(reader, (shapeImpl as AutoShapeImpl).ShapeExt.Path2DList);
  }

  private PdfPath GetCustomGeomentryPath(RectangleF bounds, PdfPath path, ShapeImpl shapeImpl)
  {
    foreach (Path2D path2D in (shapeImpl as AutoShapeImpl).ShapeExt.Path2DList)
    {
      double width = path2D.Width;
      double height = path2D.Height;
      this.GetGeomentryPath(path, path2D.PathElements, width, height, bounds);
    }
    return path;
  }

  private void GetGeomentryPath(
    PdfPath path,
    List<double> pathElements,
    double pathWidth,
    double pathHeight,
    RectangleF bounds)
  {
    PointF point1 = PointF.Empty;
    double num = 0.0;
    for (int index = 0; index < pathElements.Count; index = index + ((int) num + 1) + 1)
    {
      switch ((ushort) pathElements[index])
      {
        case 1:
          path.CloseFigure();
          point1 = PointF.Empty;
          num = 0.0;
          break;
        case 2:
          path.CloseFigure();
          num = pathElements[index + 1] * 2.0;
          point1 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          break;
        case 3:
          num = pathElements[index + 1] * 2.0;
          PointF point2 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          path.AddLine(point1, point2);
          point1 = point2;
          break;
        case 4:
          num = pathElements[index + 1] * 2.0;
          RectangleF rectangle = new RectangleF();
          rectangle.X = bounds.X;
          rectangle.Y = bounds.Y;
          rectangle.Width = this.EmuToPoint((int) pathElements[index + 2]) * 2f;
          rectangle.Height = this.EmuToPoint((int) pathElements[index + 3]) * 2f;
          float startAngle = (float) pathElements[index + 4] / 60000f;
          float sweepAngle = (float) pathElements[index + 5] / 60000f;
          path.AddArc(rectangle, startAngle, sweepAngle);
          point1 = path.PathPoints[path.PathPoints.Length - 1];
          break;
        case 5:
          num = pathElements[index + 1] * 2.0;
          PointF[] points1 = new PointF[3]
          {
            point1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds))
          };
          path.AddBeziers(points1);
          point1 = points1[2];
          break;
        case 6:
          num = pathElements[index + 1] * 2.0;
          PointF[] points2 = new PointF[4]
          {
            point1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 6], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 7], bounds))
          };
          path.AddBeziers(points2);
          point1 = points2[3];
          break;
      }
    }
  }

  private float GetGeomentryPathXValue(double pathWidth, double x, RectangleF bounds)
  {
    if (pathWidth == 0.0)
      return bounds.X + this.EmuToPoint((int) x);
    double num = x * 100.0 / pathWidth;
    return (float) ((double) bounds.Width * num / 100.0) + bounds.X;
  }

  private float GetGeomentryPathYValue(double pathHeight, double y, RectangleF bounds)
  {
    if (pathHeight == 0.0)
      return bounds.Y + this.EmuToPoint((int) y);
    double num = y * 100.0 / pathHeight;
    return (float) ((double) bounds.Height * num / 100.0) + bounds.Y;
  }

  private float GetRotationAngle(TextDirection textDirection)
  {
    float rotationAngle = 0.0f;
    switch (textDirection)
    {
      case TextDirection.RotateAllText90:
        rotationAngle = 90f;
        break;
      case TextDirection.RotateAllText270:
        rotationAngle = 270f;
        break;
    }
    return rotationAngle;
  }

  private bool IsShapeNeedToBeFill(ShapeImpl shape)
  {
    if (!(shape is AutoShapeImpl))
      return true;
    switch ((shape as AutoShapeImpl).ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
      case AutoShapeType.BentConnector2:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector2:
      case AutoShapeType.CurvedConnector4:
      case AutoShapeType.CurvedConnector5:
        return false;
      default:
        return true;
    }
  }

  private void DrawShapeFillAndLine(
    PdfPath pdfPath,
    ShapeImpl shape,
    PdfPen pdfPen,
    PdfGraphics graphics,
    RectangleF bounds,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    AutoShapeImpl autoShapeImpl = shape as AutoShapeImpl;
    if (pdfPath.PointCount <= 0)
      return;
    if ((shape.Fill.Visible || shape is AutoShapeImpl && (shape as AutoShapeImpl).ShapeExt.AutoShapeType == AutoShapeType.Unknown) && this.IsShapeNeedToBeFill(shape) && !this._workSheet.PageSetup.BlackAndWhite)
    {
      IFill fill = shape.Fill;
      this.FillBackground(graphics, shape, pdfPath, fill, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
    }
    if (shape.Line.Visible || shape is AutoShapeImpl && ((shape as AutoShapeImpl).ShapeExt.AutoShapeType == AutoShapeType.Unknown || autoShapeImpl.ShapeExt.IsCreated && autoShapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && autoShapeImpl.Line.Visible || autoShapeImpl.ShapeExt.IsCreated && !autoShapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line)))
    {
      if (pdfPen == null || pdfPen.Color.A <= (byte) 0)
        return;
      if (this.IsLine(shape) && !ApplicationImpl.EnablePartialTrustCodeStatic)
      {
        Pen gdiPen = this.CreateGDIPen(shape, shape.Line);
        if (this._workSheet.PageSetup.BlackAndWhite)
          gdiPen.Color = Color.Black;
        MemoryStream stream = new MemoryStream();
        RectangleF bounds1 = new RectangleF(bounds.X, bounds.Y, bounds.Width + 100f, bounds.Height + 100f);
        Image image = this.CreateImage(bounds1, stream);
        using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(image))
        {
          gr.PageUnit = GraphicsUnit.Point;
          gr.Clear(Color.Transparent);
          gr.DrawPath(gdiPen, this.GetGDIGraphicsPath(new RectangleF(50f, 50f, bounds.Width, bounds.Height), ref gdiPen, gr, shape as AutoShapeImpl));
        }
        stream.Position = 0L;
        graphics.DrawImage(PdfImage.FromImage(image), new PointF(bounds.X - 50f, bounds.Y - 50f), new SizeF(bounds1.Width, bounds1.Height));
        stream.Dispose();
        image.Dispose();
      }
      else
      {
        graphics.SetTransparency((float) (1.0 - shape.Line.Transparency));
        if (this._workSheet.PageSetup.BlackAndWhite)
          pdfPen.Color = (PdfColor) Color.Black;
        graphics.DrawPath(pdfPen, pdfPath);
      }
    }
    else
    {
      if (autoShapeImpl == null || autoShapeImpl.ShapeExt == null || autoShapeImpl.ShapeExt.PreservedElements.ContainsKey("Line") || !autoShapeImpl.ShapeExt.PreservedElements.ContainsKey("Style"))
        return;
      if (this._workSheet.PageSetup.BlackAndWhite)
        pdfPen.Color = (PdfColor) Color.Black;
      graphics.SetTransparency((float) (1.0 - shape.Line.Transparency));
      graphics.DrawPath(pdfPen, pdfPath);
    }
  }

  internal Image CreateImage(RectangleF bounds, MemoryStream stream)
  {
    bounds.Width = this.Pdf_UnitConverter.ConvertToPixels(bounds.Width, PdfGraphicsUnit.Point);
    bounds.Height = this.Pdf_UnitConverter.ConvertToPixels(bounds.Height, PdfGraphicsUnit.Point);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
    {
      bounds.Width = (float) (int) ((double) bounds.Width / 96.0 * (double) graphics.DpiX);
      bounds.Height = (float) (int) ((double) bounds.Height / 96.0 * (double) graphics.DpiY);
    }
    Image image = (Image) null;
    if ((double) bounds.Width == 0.0)
      bounds.Width = 1f;
    if ((double) bounds.Height == 0.0)
      bounds.Height = 1f;
    using (Bitmap bitmap = new Bitmap((int) bounds.Width, (int) bounds.Height))
    {
      using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap))
      {
        bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
        IntPtr hdc = graphics.GetHdc();
        Rectangle frameRect = new Rectangle(0, 0, (int) bounds.Width, (int) bounds.Height);
        image = (Image) new Metafile((Stream) stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
        graphics.ReleaseHdc();
      }
    }
    return image;
  }

  internal void FillBackground(
    PdfGraphics pdfGraphics,
    ShapeImpl shape,
    PdfPath path,
    IFill format,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    if (!format.Visible)
      return;
    bool isAutoShapeWithTextureOrPictureFill = false;
    switch (format.FillType)
    {
      case ExcelFillType.SolidColor:
        PdfColor color = (PdfColor) this.NormalizeColor(shape.GetFillColor());
        pdfGraphics.SetTransparency((float) (1.0 - format.Transparency));
        pdfGraphics.DrawPath((PdfBrush) new PdfSolidBrush(color), path);
        break;
      case ExcelFillType.Pattern:
        PdfHatchBrush pdfHatchBrush = this.GetPdfHatchBrush(format);
        pdfGraphics.DrawPath((PdfBrush) pdfHatchBrush, path);
        break;
      case ExcelFillType.Texture:
        Image image = (Image) null;
        if (shape is AutoShapeImpl && path != null)
          isAutoShapeWithTextureOrPictureFill = true;
        if (format.Texture != ExcelTexture.User_Defined)
          image = ChartSerializatorCommon.GetTexturePicture(format.Texture);
        else if (format.Picture != null)
          image = format.Picture;
        this.DrawImage((IShape) shape, image, pdfGraphics, firstRow, firstColumn, lastRow, lastColumn, startX, startY, isAutoShapeWithTextureOrPictureFill, path);
        break;
      case ExcelFillType.Picture:
        if (shape is AutoShapeImpl && path != null)
          isAutoShapeWithTextureOrPictureFill = true;
        this.DrawImage((IShape) shape, format.Picture, pdfGraphics, firstRow, firstColumn, lastRow, lastColumn, startX, startY, isAutoShapeWithTextureOrPictureFill, path);
        break;
    }
  }

  private bool FontColorNeedsUpdation(IFont font)
  {
    return font.Color.ToString() == "32767" || font is FontImpl && (font as FontImpl).ColorObject.Value == 8;
  }

  private PdfHatchBrush GetPdfHatchBrush(IFill format)
  {
    return new PdfHatchBrush(this.GetHatchStyle(format.Pattern), (PdfColor) this.NormalizeColor((Color) new PdfColor(format.ForeColor)), (PdfColor) this.NormalizeColor((Color) new PdfColor(format.BackColor)));
  }

  private PdfHatchStyle GetHatchStyle(ExcelGradientPattern pattern)
  {
    PdfHatchStyle hatchStyle = PdfHatchStyle.Horizontal;
    switch (pattern)
    {
      case ExcelGradientPattern.Pat_5_Percent:
        hatchStyle = PdfHatchStyle.Percent05;
        break;
      case ExcelGradientPattern.Pat_10_Percent:
        hatchStyle = PdfHatchStyle.Percent10;
        break;
      case ExcelGradientPattern.Pat_20_Percent:
        hatchStyle = PdfHatchStyle.Percent20;
        break;
      case ExcelGradientPattern.Pat_25_Percent:
        hatchStyle = PdfHatchStyle.Percent25;
        break;
      case ExcelGradientPattern.Pat_30_Percent:
        hatchStyle = PdfHatchStyle.Percent30;
        break;
      case ExcelGradientPattern.Pat_40_Percent:
        hatchStyle = PdfHatchStyle.Percent40;
        break;
      case ExcelGradientPattern.Pat_50_Percent:
        hatchStyle = PdfHatchStyle.Percent50;
        break;
      case ExcelGradientPattern.Pat_60_Percent:
        hatchStyle = PdfHatchStyle.Percent60;
        break;
      case ExcelGradientPattern.Pat_70_Percent:
        hatchStyle = PdfHatchStyle.Percent70;
        break;
      case ExcelGradientPattern.Pat_75_Percent:
        hatchStyle = PdfHatchStyle.Percent75;
        break;
      case ExcelGradientPattern.Pat_80_Percent:
        hatchStyle = PdfHatchStyle.Percent80;
        break;
      case ExcelGradientPattern.Pat_90_Percent:
        hatchStyle = PdfHatchStyle.Percent90;
        break;
      case ExcelGradientPattern.Pat_Dark_Horizontal:
        hatchStyle = PdfHatchStyle.DarkHorizontal;
        break;
      case ExcelGradientPattern.Pat_Dark_Vertical:
        hatchStyle = PdfHatchStyle.DarkVertical;
        break;
      case ExcelGradientPattern.Pat_Dark_Downward_Diagonal:
        hatchStyle = PdfHatchStyle.DarkDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Dark_Upward_Diagonal:
        hatchStyle = PdfHatchStyle.DarkUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Small_Checker_Board:
        hatchStyle = PdfHatchStyle.SmallCheckerBoard;
        break;
      case ExcelGradientPattern.Pat_Trellis:
        hatchStyle = PdfHatchStyle.Trellis;
        break;
      case ExcelGradientPattern.Pat_Light_Horizontal:
        hatchStyle = PdfHatchStyle.LightHorizontal;
        break;
      case ExcelGradientPattern.Pat_Light_Vertical:
        hatchStyle = PdfHatchStyle.LightVertical;
        break;
      case ExcelGradientPattern.Pat_Light_Downward_Diagonal:
        hatchStyle = PdfHatchStyle.LightDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Light_Upward_Diagonal:
        hatchStyle = PdfHatchStyle.LightUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Small_Grid:
        hatchStyle = PdfHatchStyle.SmallGrid;
        break;
      case ExcelGradientPattern.Pat_Dotted_Diamond:
        hatchStyle = PdfHatchStyle.DottedDiamond;
        break;
      case ExcelGradientPattern.Pat_Wide_Downward_Diagonal:
        hatchStyle = PdfHatchStyle.WideDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Wide_Upward_Diagonal:
        hatchStyle = PdfHatchStyle.WideUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Dashed_Upward_Diagonal:
        hatchStyle = PdfHatchStyle.DashedUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Dashed_Downward_Diagonal:
        hatchStyle = PdfHatchStyle.DashedDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Narrow_Vertical:
        hatchStyle = PdfHatchStyle.NarrowVertical;
        break;
      case ExcelGradientPattern.Pat_Narrow_Horizontal:
        hatchStyle = PdfHatchStyle.NarrowHorizontal;
        break;
      case ExcelGradientPattern.Pat_Dashed_Vertical:
        hatchStyle = PdfHatchStyle.DashedVertical;
        break;
      case ExcelGradientPattern.Pat_Dashed_Horizontal:
        hatchStyle = PdfHatchStyle.DashedHorizontal;
        break;
      case ExcelGradientPattern.Pat_Large_Confetti:
        hatchStyle = PdfHatchStyle.LargeConfetti;
        break;
      case ExcelGradientPattern.Pat_Large_Grid:
        hatchStyle = PdfHatchStyle.Cross;
        break;
      case ExcelGradientPattern.Pat_Horizontal_Brick:
        hatchStyle = PdfHatchStyle.HorizontalBrick;
        break;
      case ExcelGradientPattern.Pat_Large_Checker_Board:
        hatchStyle = PdfHatchStyle.LargeCheckerBoard;
        break;
      case ExcelGradientPattern.Pat_Small_Confetti:
        hatchStyle = PdfHatchStyle.SmallConfetti;
        break;
      case ExcelGradientPattern.Pat_Zig_Zag:
        hatchStyle = PdfHatchStyle.ZigZag;
        break;
      case ExcelGradientPattern.Pat_Solid_Diamond:
        hatchStyle = PdfHatchStyle.SolidDiamond;
        break;
      case ExcelGradientPattern.Pat_Diagonal_Brick:
        hatchStyle = PdfHatchStyle.DiagonalBrick;
        break;
      case ExcelGradientPattern.Pat_Outlined_Diamond:
        hatchStyle = PdfHatchStyle.OutlinedDiamond;
        break;
      case ExcelGradientPattern.Pat_Plaid:
        hatchStyle = PdfHatchStyle.Plaid;
        break;
      case ExcelGradientPattern.Pat_Sphere:
        hatchStyle = PdfHatchStyle.Sphere;
        break;
      case ExcelGradientPattern.Pat_Weave:
        hatchStyle = PdfHatchStyle.Weave;
        break;
      case ExcelGradientPattern.Pat_Dotted_Grid:
        hatchStyle = PdfHatchStyle.DottedGrid;
        break;
      case ExcelGradientPattern.Pat_Divot:
        hatchStyle = PdfHatchStyle.Divot;
        break;
      case ExcelGradientPattern.Pat_Shingle:
        hatchStyle = PdfHatchStyle.Shingle;
        break;
      case ExcelGradientPattern.Pat_Wave:
        hatchStyle = PdfHatchStyle.Wave;
        break;
    }
    return hatchStyle;
  }

  internal PdfPath GetGraphicsPath(
    RectangleF bounds,
    ref PdfPen pen,
    PdfGraphics gr,
    AutoShapeImpl shapeImpl)
  {
    this._pdfGraphics = gr;
    ShapePath shapePath = new ShapePath(bounds, shapeImpl.ShapeExt.ShapeGuide);
    PdfPath path1 = new PdfPath();
    PdfColor empty = PdfColor.Empty;
    switch (shapeImpl.ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.FlowChartProcess:
        path1.AddRectangle(bounds);
        return path1;
      case AutoShapeType.Parallelogram:
      case AutoShapeType.FlowChartData:
        return shapePath.GetParallelogramPath();
      case AutoShapeType.Trapezoid:
        return shapePath.GetTrapezoidPath();
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
        PointF[] linePoints1 = new PointF[4]
        {
          new PointF(bounds.X, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Y),
          new PointF(bounds.Right, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Bottom)
        };
        path1.AddLines(linePoints1);
        path1.CloseFigure();
        break;
      case AutoShapeType.RoundedRectangle:
        return shapePath.GetRoundedRectanglePath();
      case AutoShapeType.Octagon:
        return shapePath.GetOctagonPath();
      case AutoShapeType.IsoscelesTriangle:
        return shapePath.GetTrianglePath();
      case AutoShapeType.RightTriangle:
        PointF[] linePoints2 = new PointF[3]
        {
          new PointF(bounds.X, bounds.Bottom),
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        path1.AddLines(linePoints2);
        path1.CloseFigure();
        return path1;
      case AutoShapeType.Oval:
        path1.AddEllipse(bounds);
        return path1;
      case AutoShapeType.Hexagon:
        return shapePath.GetHexagonPath();
      case AutoShapeType.Cross:
        return shapePath.GetCrossPath();
      case AutoShapeType.RegularPentagon:
        return shapePath.GetRegularPentagonPath();
      case AutoShapeType.Can:
        return shapePath.GetCanPath();
      case AutoShapeType.Cube:
        return shapePath.GetCubePath();
      case AutoShapeType.Bevel:
        return shapePath.GetBevelPath();
      case AutoShapeType.FoldedCorner:
        return shapePath.GetFoldedCornerPath();
      case AutoShapeType.SmileyFace:
        for (int index = 0; index < 2; ++index)
        {
          bool isDrawEye = index == 1;
          PdfPath[] smileyFacePath = shapePath.GetSmileyFacePath(isDrawEye);
          IFill fill = shapeImpl.Fill;
          PdfColor color = PdfColor.Empty;
          if (fill.FillType == ExcelFillType.SolidColor)
          {
            color = (PdfColor) this.NormalizeColor(shapeImpl.GetFillColor());
            this._pdfGraphics.SetTransparency((float) (1.0 - fill.Transparency));
          }
          if (fill.FillType == ExcelFillType.Gradient)
            color = (PdfColor) this.NormalizeColor(fill.ForeColor);
          if (isDrawEye)
            color = (PdfColor) this.GetDarkerColor((Color) color, 80f);
          PdfBrush brush = (PdfBrush) new PdfSolidBrush(color);
          if (fill.FillType == ExcelFillType.Pattern)
            brush = (PdfBrush) this.GetPdfHatchBrush(fill);
          foreach (PdfPath path2 in smileyFacePath)
          {
            if (color != PdfColor.Empty)
              this._pdfGraphics.DrawPath(brush, path2);
            if (shapeImpl.ShapeExt.IsCreated && !shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && shapeImpl.ShapeExt.Line.Visible || !shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Line.Visible)
              this._pdfGraphics.DrawPath(pen, path2);
          }
        }
        break;
      case AutoShapeType.Donut:
        return shapePath.GetDonutPath();
      case AutoShapeType.NoSymbol:
        return shapePath.GetNoSymbolPath();
      case AutoShapeType.BlockArc:
        return shapePath.GetBlockArcPath();
      case AutoShapeType.Heart:
        return shapePath.GetHeartPath();
      case AutoShapeType.LightningBolt:
        return shapePath.GetLightningBoltPath();
      case AutoShapeType.Sun:
        return shapePath.GetSunPath();
      case AutoShapeType.Moon:
        return shapePath.GetMoonPath();
      case AutoShapeType.Arc:
        PdfPath[] arcPath = shapePath.GetArcPath();
        IFill fill1 = shapeImpl.Fill;
        Color color1 = ColorExtension.Empty;
        PdfColor color2 = PdfColor.Empty;
        if (fill1.FillType == ExcelFillType.SolidColor)
        {
          color1 = shapeImpl.GetFillColor();
          color2 = (PdfColor) this.NormalizeColor(color1);
          this._pdfGraphics.SetTransparency((float) (1.0 - fill1.Transparency));
        }
        PdfBrush brush1 = (PdfBrush) new PdfSolidBrush(color2);
        if (fill1.FillType == ExcelFillType.Pattern)
          brush1 = (PdfBrush) this.GetPdfHatchBrush(fill1);
        if (color2 != PdfColor.Empty && color1 != ColorExtension.Empty)
          this._pdfGraphics.DrawPath(brush1, arcPath[1]);
        if (shapeImpl.ShapeExt.IsCreated && !shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && shapeImpl.ShapeExt.Line.Visible || !shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Line.Visible)
        {
          this._pdfGraphics.DrawPath(pen, arcPath[0]);
          break;
        }
        break;
      case AutoShapeType.DoubleBracket:
        return shapePath.GetDoubleBracketPath();
      case AutoShapeType.DoubleBrace:
        return shapePath.GetDoubleBracePath();
      case AutoShapeType.Plaque:
        return shapePath.GetPlaquePath();
      case AutoShapeType.LeftBracket:
        return shapePath.GetLeftBracketPath();
      case AutoShapeType.RightBracket:
        return shapePath.GetRightBracketPath();
      case AutoShapeType.LeftBrace:
        return shapePath.GetLeftBracePath();
      case AutoShapeType.RightBrace:
        return shapePath.GetRightBracePath();
      case AutoShapeType.RightArrow:
        return shapePath.GetRightArrowPath();
      case AutoShapeType.LeftArrow:
        return shapePath.GetLeftArrowPath();
      case AutoShapeType.UpArrow:
        return shapePath.GetUpArrowPath();
      case AutoShapeType.DownArrow:
        return shapePath.GetDownArrowPath();
      case AutoShapeType.LeftRightArrow:
        return shapePath.GetLeftRightArrowPath();
      case AutoShapeType.UpDownArrow:
        return shapePath.GetUpDownArrowPath();
      case AutoShapeType.QuadArrow:
        return shapePath.GetQuadArrowPath();
      case AutoShapeType.LeftRightUpArrow:
        return shapePath.GetLeftRightUpArrowPath();
      case AutoShapeType.BentArrow:
        return shapePath.GetBentArrowPath();
      case AutoShapeType.UTurnArrow:
        return shapePath.GetUTrunArrowPath();
      case AutoShapeType.LeftUpArrow:
        return shapePath.GetLeftUpArrowPath();
      case AutoShapeType.BentUpArrow:
        return shapePath.GetBentUpArrowPath();
      case AutoShapeType.CurvedRightArrow:
        return shapePath.GetCurvedRightArrowPath();
      case AutoShapeType.CurvedLeftArrow:
        return shapePath.GetCurvedLeftArrowPath();
      case AutoShapeType.CurvedUpArrow:
        return shapePath.GetCurvedUpArrowPath();
      case AutoShapeType.CurvedDownArrow:
        return shapePath.GetCurvedDownArrowPath();
      case AutoShapeType.StripedRightArrow:
        return shapePath.GetStripedRightArrowPath();
      case AutoShapeType.NotchedRightArrow:
        return shapePath.GetNotchedRightArrowPath();
      case AutoShapeType.Pentagon:
        return shapePath.GetPentagonPath();
      case AutoShapeType.Chevron:
        return shapePath.GetChevronPath();
      case AutoShapeType.RightArrowCallout:
        return shapePath.GetRightArrowCalloutPath();
      case AutoShapeType.LeftArrowCallout:
        return shapePath.GetLeftArrowCalloutPath();
      case AutoShapeType.UpArrowCallout:
        return shapePath.GetUpArrowCalloutPath();
      case AutoShapeType.DownArrowCallout:
        return shapePath.GetDownArrowCalloutPath();
      case AutoShapeType.LeftRightArrowCallout:
        return shapePath.GetLeftRightArrowCalloutPath();
      case AutoShapeType.QuadArrowCallout:
        return shapePath.GetQuadArrowCalloutPath();
      case AutoShapeType.CircularArrow:
        return shapePath.GetCircularArrowPath();
      case AutoShapeType.FlowChartAlternateProcess:
        return shapePath.GetFlowChartAlternateProcessPath();
      case AutoShapeType.FlowChartPredefinedProcess:
        return shapePath.GetFlowChartPredefinedProcessPath();
      case AutoShapeType.FlowChartInternalStorage:
        return shapePath.GetFlowChartInternalStoragePath();
      case AutoShapeType.FlowChartDocument:
        return shapePath.GetFlowChartDocumentPath();
      case AutoShapeType.FlowChartMultiDocument:
        return shapePath.GetFlowChartMultiDocumentPath();
      case AutoShapeType.FlowChartTerminator:
        return shapePath.GetFlowChartTerminatorPath();
      case AutoShapeType.FlowChartPreparation:
        return shapePath.GetFlowChartPreparationPath();
      case AutoShapeType.FlowChartManualInput:
        return shapePath.GetFlowChartManualInputPath();
      case AutoShapeType.FlowChartManualOperation:
        return shapePath.GetFlowChartManualOperationPath();
      case AutoShapeType.FlowChartConnector:
        return shapePath.GetFlowChartConnectorPath();
      case AutoShapeType.FlowChartOffPageConnector:
        return shapePath.GetFlowChartOffPageConnectorPath();
      case AutoShapeType.FlowChartCard:
        return shapePath.GetFlowChartCardPath();
      case AutoShapeType.FlowChartPunchedTape:
        return shapePath.GetFlowChartPunchedTapePath();
      case AutoShapeType.FlowChartSummingJunction:
        return shapePath.GetFlowChartSummingJunctionPath();
      case AutoShapeType.FlowChartOr:
        return shapePath.GetFlowChartOrPath();
      case AutoShapeType.FlowChartCollate:
        return shapePath.GetFlowChartCollatePath();
      case AutoShapeType.FlowChartSort:
        return shapePath.GetFlowChartSortPath();
      case AutoShapeType.FlowChartExtract:
        return shapePath.GetFlowChartExtractPath();
      case AutoShapeType.FlowChartMerge:
        return shapePath.GetFlowChartMergePath();
      case AutoShapeType.FlowChartStoredData:
        return shapePath.GetFlowChartOnlineStoragePath();
      case AutoShapeType.FlowChartDelay:
        return shapePath.GetFlowChartDelayPath();
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return shapePath.GetFlowChartSequentialAccessStoragePath();
      case AutoShapeType.FlowChartMagneticDisk:
        return shapePath.GetFlowChartMagneticDiskPath();
      case AutoShapeType.FlowChartDirectAccessStorage:
        return shapePath.GetFlowChartDirectAccessStoragePath();
      case AutoShapeType.FlowChartDisplay:
        return shapePath.GetFlowChartDisplayPath();
      case AutoShapeType.Explosion1:
        return shapePath.GetExplosion1();
      case AutoShapeType.Explosion2:
        return shapePath.GetExplosion2();
      case AutoShapeType.Star4Point:
        return shapePath.GetStar4Point();
      case AutoShapeType.Star5Point:
        return shapePath.GetStar5Point();
      case AutoShapeType.Star8Point:
        return shapePath.GetStar8Point();
      case AutoShapeType.Star16Point:
        return shapePath.GetStar16Point();
      case AutoShapeType.Star24Point:
        return shapePath.GetStar24Point();
      case AutoShapeType.Star32Point:
        return shapePath.GetStar32Point();
      case AutoShapeType.UpRibbon:
        return shapePath.GetUpRibbon();
      case AutoShapeType.DownRibbon:
        return shapePath.GetDownRibbon();
      case AutoShapeType.CurvedUpRibbon:
        return shapePath.GetCurvedUpRibbon();
      case AutoShapeType.CurvedDownRibbon:
        return shapePath.GetCurvedDownRibbon();
      case AutoShapeType.VerticalScroll:
        return shapePath.GetVerticalScroll();
      case AutoShapeType.HorizontalScroll:
        PdfPath[] horizontalScroll = shapePath.GetHorizontalScroll();
        PdfColor color3 = PdfColor.Empty;
        IFill fill2 = shapeImpl.Fill;
        if (fill2.FillType == ExcelFillType.SolidColor)
        {
          color3 = (PdfColor) this.NormalizeColor(shapeImpl.GetFillColor());
          this._pdfGraphics.SetTransparency((float) (1.0 - fill2.Transparency));
        }
        if (fill2.FillType == ExcelFillType.Gradient)
          color3 = (PdfColor) this.NormalizeColor(fill2.ForeColor);
        PdfBrush brush2 = (PdfBrush) new PdfSolidBrush(color3);
        if (fill2.FillType == ExcelFillType.Pattern)
          brush2 = (PdfBrush) this.GetPdfHatchBrush(fill2);
        foreach (PdfPath path3 in horizontalScroll)
        {
          if (color3 != PdfColor.Empty)
            this._pdfGraphics.DrawPath(brush2, path3);
          if (shapeImpl.ShapeExt.IsCreated && !shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && shapeImpl.ShapeExt.Line.Visible || !shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Line.Visible)
            this._pdfGraphics.DrawPath(pen, path3);
        }
        break;
      case AutoShapeType.Wave:
        return shapePath.GetWave();
      case AutoShapeType.DoubleWave:
        return shapePath.GetDoubleWave();
      case AutoShapeType.RectangularCallout:
        return shapePath.GetRectangularCalloutPath();
      case AutoShapeType.RoundedRectangularCallout:
        return shapePath.GetRoundedRectangularCalloutPath();
      case AutoShapeType.OvalCallout:
        return shapePath.GetOvalCalloutPath();
      case AutoShapeType.CloudCallout:
        return shapePath.GetCloudCalloutPath();
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
        return shapePath.GetLineCallout1Path();
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2NoBorder:
        return shapePath.GetLineCallout2Path();
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3NoBorder:
        return shapePath.GetLineCallout3Path();
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return shapePath.GetLineCallout1AccentBarPath();
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return shapePath.GetLineCallout2AccentBarPath();
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return shapePath.GetLineCallout3AccentBarPath();
      case AutoShapeType.DiagonalStripe:
        return shapePath.GetDiagonalStripePath();
      case AutoShapeType.Pie:
        return shapePath.GetPiePath();
      case AutoShapeType.Decagon:
        return shapePath.GetDecagonPath();
      case AutoShapeType.Heptagon:
        return shapePath.GetHeptagonPath();
      case AutoShapeType.Dodecagon:
        return shapePath.GetDodecagonPath();
      case AutoShapeType.Star6Point:
        return shapePath.GetStar6Point();
      case AutoShapeType.Star7Point:
        return shapePath.GetStar7Point();
      case AutoShapeType.Star10Point:
        return shapePath.GetStar10Point();
      case AutoShapeType.Star12Point:
        return shapePath.GetStar12Point();
      case AutoShapeType.RoundSingleCornerRectangle:
        return shapePath.GetRoundSingleCornerRectanglePath();
      case AutoShapeType.RoundSameSideCornerRectangle:
        return shapePath.GetRoundSameSideCornerRectanglePath();
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return shapePath.GetRoundDiagonalCornerRectanglePath();
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return shapePath.GetSnipAndRoundSingleCornerRectanglePath();
      case AutoShapeType.SnipSingleCornerRectangle:
        return shapePath.GetSnipSingleCornerRectanglePath();
      case AutoShapeType.SnipSameSideCornerRectangle:
        return shapePath.GetSnipSameSideCornerRectanglePath();
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return shapePath.GetSnipDiagonalCornerRectanglePath();
      case AutoShapeType.Frame:
        return shapePath.GetFramePath();
      case AutoShapeType.HalfFrame:
        return shapePath.GetHalfFramePath();
      case AutoShapeType.Teardrop:
        return shapePath.GetTearDropPath();
      case AutoShapeType.Chord:
        return shapePath.GetChordPath();
      case AutoShapeType.L_Shape:
        return shapePath.GetL_ShapePath();
      case AutoShapeType.MathPlus:
        return shapePath.GetMathPlusPath();
      case AutoShapeType.MathMinus:
        return shapePath.GetMathMinusPath();
      case AutoShapeType.MathMultiply:
        return shapePath.GetMathMultiplyPath();
      case AutoShapeType.MathDivision:
        return shapePath.GetMathDivisionPath();
      case AutoShapeType.MathEqual:
        return shapePath.GetMathEqualPath();
      case AutoShapeType.MathNotEqual:
        return shapePath.GetMathNotEqualPath();
      case AutoShapeType.Cloud:
        return shapePath.GetCloudPath();
      case AutoShapeType.Line:
        path1.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return path1;
      case AutoShapeType.StraightConnector:
        path1.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return path1;
      case AutoShapeType.ElbowConnector:
        return shapePath.GetBentConnectorPath();
      case AutoShapeType.CurvedConnector:
        return shapePath.GetCurvedConnectorPath();
      case AutoShapeType.BentConnector2:
        return shapePath.GetBentConnector2Path();
      case AutoShapeType.BentConnector4:
        return shapePath.GetBentConnector4Path();
      case AutoShapeType.BentConnector5:
        return shapePath.GetBentConnector5Path();
      case AutoShapeType.CurvedConnector2:
        return shapePath.GetCurvedConnector2Path();
      case AutoShapeType.CurvedConnector4:
        return shapePath.GetCurvedConnector4Path();
      case AutoShapeType.CurvedConnector5:
        return shapePath.GetCurvedConnector5Path();
      default:
        if (shapeImpl.ShapeExt.IsCustomGeometry)
          return this.GetCustomGeomentryPath(bounds, path1, (ShapeImpl) shapeImpl);
        break;
    }
    return path1;
  }

  private Color GetDarkerColor(Color color, float correctionfactory)
  {
    return Color.FromArgb((int) ((double) color.R / 100.0 * (double) correctionfactory), (int) ((double) color.G / 100.0 * (double) correctionfactory), (int) ((double) color.B / 100.0 * (double) correctionfactory));
  }

  internal GraphicsPath GetGDIGraphicsPath(
    RectangleF bounds,
    ref Pen pen,
    System.Drawing.Graphics gr,
    AutoShapeImpl shapeImpl)
  {
    ShapePath shapePath = new ShapePath(bounds, shapeImpl.ShapeExt.ShapeGuide);
    GraphicsPath gdiGraphicsPath = new GraphicsPath();
    switch (shapeImpl.ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Line:
        gdiGraphicsPath.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return gdiGraphicsPath;
      case AutoShapeType.StraightConnector:
        gdiGraphicsPath.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return gdiGraphicsPath;
      case AutoShapeType.ElbowConnector:
        return shapePath.GetGDIBentConnectorPath();
      case AutoShapeType.CurvedConnector:
        return shapePath.GetGDICurvedConnectorPath();
      case AutoShapeType.BentConnector2:
        return shapePath.GetGDIBentConnector2Path();
      case AutoShapeType.BentConnector4:
        return shapePath.GetGDIBentConnector4Path();
      case AutoShapeType.BentConnector5:
        return shapePath.GetGDIBentConnector5Path();
      case AutoShapeType.CurvedConnector2:
        return shapePath.GetGDICurvedConnector2Path();
      case AutoShapeType.CurvedConnector4:
        return shapePath.GetGDICurvedConnector4Path();
      case AutoShapeType.CurvedConnector5:
        return shapePath.GetGDICurvedConnector5Path();
      default:
        return gdiGraphicsPath;
    }
  }

  private void DrawMergeBackground(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    float originalWidth,
    float startX,
    float startY)
  {
    if (!sheet.HasMergedCells)
      return;
    this._lstRegions = new List<MergeCellsRecord.MergedRegion>();
    MergeCellsImpl mergeCells = sheet.MergeCells;
    this.CacheMerges(firstRow, firstColumn, lastRow, lastColumn, this._lstRegions);
    if (this._lstRegions.Count <= 0)
      return;
    int index = 0;
    for (int count = this._lstRegions.Count; index < count; ++index)
    {
      mergeCells.GetFormat(this._lstRegions[index]);
      RectangleF mergedRectangle = this.GetMergedRectangle(sheet, this._lstRegions[index], firstRow, firstColumn, lastRow, lastColumn, startX, startY);
      if (this._excelToPdfSettings.EnableRTL)
      {
        mergedRectangle.X = originalWidth - mergedRectangle.X;
        mergedRectangle.X -= mergedRectangle.Width;
      }
      IRange cell = sheet[this._lstRegions[index].RowFrom + 1, this._lstRegions[index].ColumnFrom + 1];
      if (cell.CellStyle.FillPattern != ExcelPattern.None || sheet.ConditionalFormats.Count > 0)
        this.DrawBackground(cell, mergedRectangle, this._pdfGraphics);
    }
  }

  private void CacheMerges(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    List<MergeCellsRecord.MergedRegion> mergedRegions)
  {
    List<long> longList = new List<long>();
    for (int firstRow1 = firstRow; firstRow1 <= lastRow; ++firstRow1)
    {
      for (int firstColumn1 = firstColumn; firstColumn1 <= lastColumn; ++firstColumn1)
      {
        MergedCellInfo mergedCellInfo = new MergedCellInfo();
        if (this._mergedRegions.TryGetValue(RangeImpl.GetCellIndex(firstColumn1, firstRow1), out mergedCellInfo))
        {
          if (mergedCellInfo.IsFirst)
            mergedRegions.Add(new MergeCellsRecord.MergedRegion(firstRow1 - 1, firstRow1 + mergedCellInfo.RowSpan - 2, firstColumn1 - 1, firstColumn1 + mergedCellInfo.ColSpan - 2));
          else if (!longList.Contains(mergedCellInfo.FirstCellIndex))
          {
            int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(mergedCellInfo.FirstCellIndex);
            int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(mergedCellInfo.FirstCellIndex);
            if (rowFromCellIndex < firstRow || columnFromCellIndex < firstColumn)
            {
              longList.Add(mergedCellInfo.FirstCellIndex);
              mergedCellInfo = this._mergedRegions[mergedCellInfo.FirstCellIndex];
              mergedRegions.Add(new MergeCellsRecord.MergedRegion(rowFromCellIndex - 1, rowFromCellIndex + mergedCellInfo.RowSpan - 2, columnFromCellIndex - 1, columnFromCellIndex + mergedCellInfo.ColSpan - 2));
            }
          }
        }
      }
    }
    longList.Clear();
  }

  private PdfPen CreatePen(IShapeLineFormat lineFormat)
  {
    return new PdfPen(new PdfColor(this.NormalizeColor(lineFormat.ForeColor)), (float) lineFormat.Weight)
    {
      DashStyle = this.GetDashStyle(lineFormat)
    };
  }

  private PdfPen CreatePen(ShapeImpl shape, IShapeLineFormat lineFormat)
  {
    PdfPen pen = new PdfPen((PdfColor) this.NormalizeColor(shape.GetBorderColor()), (float) shape.GetBorderThickness());
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        pen.DashStyle = PdfDashStyle.Solid;
        break;
      case ExcelShapeDashLineStyle.Dotted:
      case ExcelShapeDashLineStyle.Dotted_Round:
        pen.DashStyle = PdfDashStyle.Dot;
        break;
      case ExcelShapeDashLineStyle.Dashed:
        pen.DashStyle = PdfDashStyle.Dash;
        break;
      case ExcelShapeDashLineStyle.Medium_Dashed:
        pen.DashStyle = PdfDashStyle.Custom;
        pen.DashPattern = new float[2]{ 12f, 4.5f };
        break;
      case ExcelShapeDashLineStyle.Dash_Dot:
        pen.DashStyle = PdfDashStyle.DashDot;
        break;
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        pen.DashStyle = PdfDashStyle.Custom;
        pen.DashPattern = new float[4]
        {
          12f,
          4.5f,
          1.3f,
          4.5f
        };
        break;
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        pen.DashStyle = PdfDashStyle.Custom;
        pen.DashPattern = new float[6]
        {
          12f,
          4.5f,
          1.3f,
          4.5f,
          1.3f,
          4.5f
        };
        break;
    }
    switch (lineFormat.Style)
    {
      case ExcelShapeLineStyle.Line_Thin_Thin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.3333333f,
          0.6666667f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thin_Thick:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.16666f,
          0.3f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thick_Thin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.6f,
          0.73333f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thick_Between_Thin:
        pen.CompoundArray = new float[6]
        {
          0.0f,
          0.1666667f,
          0.3333333f,
          0.6666667f,
          0.8333333f,
          1f
        };
        break;
    }
    PdfCustomLineCap customLineCap1 = this.GetCustomLineCap(lineFormat.BeginArrowHeadStyle, lineFormat.BeginArrowheadLength, lineFormat.BeginArrowheadWidth);
    PdfCustomLineCap customLineCap2 = this.GetCustomLineCap(lineFormat.EndArrowHeadStyle, lineFormat.EndArrowheadLength, lineFormat.EndArrowheadWidth);
    if (customLineCap1 != null)
      pen.CustomStartCap = customLineCap1;
    if (customLineCap2 != null)
      pen.CustomEndCap = customLineCap2;
    return pen;
  }

  private Pen CreateGDIPen(ShapeImpl shape, IShapeLineFormat lineFormat)
  {
    Pen gdiPen = new Pen(this.NormalizeColor(shape.GetBorderColor()), (float) shape.GetBorderThickness());
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        gdiPen.DashStyle = DashStyle.Solid;
        break;
      case ExcelShapeDashLineStyle.Dotted:
        gdiPen.DashStyle = DashStyle.Dot;
        break;
      case ExcelShapeDashLineStyle.Dotted_Round:
      case ExcelShapeDashLineStyle.Dash_Dot:
        gdiPen.DashStyle = DashStyle.DashDot;
        break;
      case ExcelShapeDashLineStyle.Dashed:
      case ExcelShapeDashLineStyle.Medium_Dashed:
        gdiPen.DashStyle = DashStyle.Dash;
        break;
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        gdiPen.DashPattern = new float[2]{ 1f, 0.5f };
        break;
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        gdiPen.DashStyle = DashStyle.DashDotDot;
        break;
    }
    switch (lineFormat.Style)
    {
      case ExcelShapeLineStyle.Line_Thin_Thin:
        gdiPen.CompoundArray = new float[4]
        {
          0.0f,
          0.3333333f,
          0.6666667f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thin_Thick:
        gdiPen.CompoundArray = new float[4]
        {
          0.0f,
          0.16666f,
          0.3f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thick_Thin:
        gdiPen.CompoundArray = new float[4]
        {
          0.0f,
          0.6f,
          0.73333f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thick_Between_Thin:
        gdiPen.CompoundArray = new float[6]
        {
          0.0f,
          0.1666667f,
          0.3333333f,
          0.6666667f,
          0.8333333f,
          1f
        };
        break;
    }
    CustomLineCap gdiCustomLineCap1 = this.GetGDICustomLineCap(lineFormat.BeginArrowHeadStyle, lineFormat.BeginArrowheadLength, lineFormat.BeginArrowheadWidth);
    CustomLineCap gdiCustomLineCap2 = this.GetGDICustomLineCap(lineFormat.EndArrowHeadStyle, lineFormat.EndArrowheadLength, lineFormat.EndArrowheadWidth);
    if (gdiCustomLineCap1 != null)
      gdiPen.CustomStartCap = gdiCustomLineCap1;
    if (gdiCustomLineCap2 != null)
      gdiPen.CustomEndCap = gdiCustomLineCap2;
    return gdiPen;
  }

  private float GetDefaultWidth(ShapeImpl shape, IShapeLineFormat line)
  {
    if (shape is AutoShapeImpl)
    {
      switch ((shape as AutoShapeImpl).ShapeExt.AutoShapeType)
      {
        case AutoShapeType.FlowChartConnector:
        case AutoShapeType.FlowChartOffPageConnector:
        case AutoShapeType.Line:
        case AutoShapeType.StraightConnector:
        case AutoShapeType.ElbowConnector:
        case AutoShapeType.CurvedConnector:
        case AutoShapeType.BentConnector2:
        case AutoShapeType.BentConnector4:
        case AutoShapeType.BentConnector5:
        case AutoShapeType.CurvedConnector2:
        case AutoShapeType.CurvedConnector4:
        case AutoShapeType.CurvedConnector5:
          return (float) shape.Width;
      }
    }
    return (float) line.Weight;
  }

  private PdfDashStyle GetDashStyle(IShapeLineFormat lineFormat)
  {
    PdfDashStyle dashStyle = PdfDashStyle.Solid;
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        dashStyle = PdfDashStyle.Solid;
        break;
      case ExcelShapeDashLineStyle.Dotted:
      case ExcelShapeDashLineStyle.Dotted_Round:
        dashStyle = PdfDashStyle.Dot;
        break;
      case ExcelShapeDashLineStyle.Dashed:
      case ExcelShapeDashLineStyle.Medium_Dashed:
        dashStyle = PdfDashStyle.Dash;
        break;
      case ExcelShapeDashLineStyle.Dash_Dot:
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        dashStyle = PdfDashStyle.DashDot;
        break;
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        dashStyle = PdfDashStyle.DashDotDot;
        break;
    }
    return dashStyle;
  }

  private PdfCustomLineCap GetCustomLineCap(
    ExcelShapeArrowStyle arrowheadStyle,
    ExcelShapeArrowLength arrowheadLength,
    ExcelShapeArrowWidth arrowheadWidth)
  {
    float baseInset;
    PdfPath lineGapGraphicsPath = this.GetCustomLineGapGraphicsPath(arrowheadStyle, arrowheadLength, arrowheadWidth, out baseInset);
    if (lineGapGraphicsPath == null)
      return (PdfCustomLineCap) null;
    PdfCustomLineCap customLineCap;
    if (arrowheadStyle == ExcelShapeArrowStyle.LineArrowOpen)
    {
      customLineCap = new PdfCustomLineCap((PdfPath) null, lineGapGraphicsPath, PdfLineCap.Round, baseInset);
      customLineCap.SetStrokeCaps(PdfLineCap.Round, PdfLineCap.Round);
    }
    else
      customLineCap = new PdfCustomLineCap(lineGapGraphicsPath, (PdfPath) null, PdfLineCap.Round | PdfLineCap.Square, baseInset);
    return customLineCap;
  }

  private PdfPath GetCustomLineGapGraphicsPath(
    ExcelShapeArrowStyle arrowheadStyle,
    ExcelShapeArrowLength arrowheadLength,
    ExcelShapeArrowWidth arrowheadWidth,
    out float baseInset)
  {
    baseInset = 0.0f;
    if (arrowheadStyle == ExcelShapeArrowStyle.LineNoArrow)
      return (PdfPath) null;
    PdfPath graphicsPath = new PdfPath((PdfBrush) null, PdfFillMode.Winding);
    float styleValue;
    if (this.GetArrowheadStyleValue(arrowheadStyle, graphicsPath, out styleValue))
      return (PdfPath) null;
    float num1 = (float) (2 + arrowheadLength);
    if (arrowheadLength == ExcelShapeArrowLength.ArrowHeadLong)
      ++num1;
    float num2 = (float) (2 + arrowheadWidth);
    if (arrowheadWidth == ExcelShapeArrowWidth.ArrowHeadWide)
      ++num2;
    baseInset = num1 * styleValue;
    new Matrix().Scale(num2 / 10f, num1 / 10f);
    return graphicsPath;
  }

  private bool GetArrowheadStyleValue(
    ExcelShapeArrowStyle arrowheadStyle,
    PdfPath graphicsPath,
    out float styleValue)
  {
    switch (arrowheadStyle)
    {
      case ExcelShapeArrowStyle.LineArrow:
        graphicsPath.AddLines(this._arrowPoints);
        graphicsPath.CloseFigure();
        styleValue = 1f;
        break;
      case ExcelShapeArrowStyle.LineArrowStealth:
        graphicsPath.AddLines(this._arrowStealthPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.55f;
        break;
      case ExcelShapeArrowStyle.LineArrowDiamond:
        graphicsPath.AddLines(this._arrowDiamondPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ExcelShapeArrowStyle.LineArrowOval:
        graphicsPath.AddEllipse(-5f, -5f, 10f, 10f);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ExcelShapeArrowStyle.LineArrowOpen:
        graphicsPath.AddLines(this._arrowOpenPoints);
        styleValue = 0.3f;
        break;
      default:
        styleValue = 0.0f;
        return true;
    }
    return false;
  }

  private CustomLineCap GetGDICustomLineCap(
    ExcelShapeArrowStyle arrowheadStyle,
    ExcelShapeArrowLength arrowheadLength,
    ExcelShapeArrowWidth arrowheadWidth)
  {
    float baseInset;
    GraphicsPath lineGapGraphicsPath = this.GetGDICustomLineGapGraphicsPath(arrowheadStyle, arrowheadLength, arrowheadWidth, out baseInset);
    if (lineGapGraphicsPath == null)
      return (CustomLineCap) null;
    CustomLineCap gdiCustomLineCap;
    if (arrowheadStyle == ExcelShapeArrowStyle.LineArrowOpen)
    {
      gdiCustomLineCap = new CustomLineCap((GraphicsPath) null, lineGapGraphicsPath, LineCap.Round, baseInset);
      gdiCustomLineCap.SetStrokeCaps(LineCap.Round, LineCap.Round);
    }
    else
      gdiCustomLineCap = new CustomLineCap(lineGapGraphicsPath, (GraphicsPath) null, LineCap.Triangle, baseInset);
    return gdiCustomLineCap;
  }

  private GraphicsPath GetGDICustomLineGapGraphicsPath(
    ExcelShapeArrowStyle arrowheadStyle,
    ExcelShapeArrowLength arrowheadLength,
    ExcelShapeArrowWidth arrowheadWidth,
    out float baseInset)
  {
    baseInset = 0.0f;
    if (arrowheadStyle == ExcelShapeArrowStyle.LineNoArrow)
      return (GraphicsPath) null;
    GraphicsPath graphicsPath = new GraphicsPath(FillMode.Winding);
    float styleValue;
    if (this.GetGDIArrowheadStyleValue(arrowheadStyle, graphicsPath, out styleValue))
      return (GraphicsPath) null;
    float num1 = (float) (2 + arrowheadLength);
    if (arrowheadLength == ExcelShapeArrowLength.ArrowHeadLong)
      ++num1;
    float num2 = (float) (2 + arrowheadWidth);
    if (arrowheadWidth == ExcelShapeArrowWidth.ArrowHeadWide)
      ++num2;
    baseInset = num1 * styleValue;
    Matrix matrix = new Matrix();
    matrix.Scale(num2 / 10f, num1 / 10f);
    graphicsPath.Transform(matrix);
    return graphicsPath;
  }

  private bool GetGDIArrowheadStyleValue(
    ExcelShapeArrowStyle arrowheadStyle,
    GraphicsPath graphicsPath,
    out float styleValue)
  {
    switch (arrowheadStyle)
    {
      case ExcelShapeArrowStyle.LineArrow:
        graphicsPath.AddLines(this._arrowPoints);
        graphicsPath.CloseFigure();
        styleValue = 1f;
        break;
      case ExcelShapeArrowStyle.LineArrowStealth:
        graphicsPath.AddLines(this._arrowStealthPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.55f;
        break;
      case ExcelShapeArrowStyle.LineArrowDiamond:
        graphicsPath.AddLines(this._arrowDiamondPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ExcelShapeArrowStyle.LineArrowOval:
        graphicsPath.AddEllipse(-5f, -5f, 10f, 10f);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ExcelShapeArrowStyle.LineArrowOpen:
        graphicsPath.AddLines(this._arrowOpenPoints);
        styleValue = 0.3f;
        break;
      default:
        styleValue = 0.0f;
        return true;
    }
    return false;
  }

  private float GetShapeLineWidth(IShapeLineFormat lineFormat)
  {
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        this._borderWidth = 0.5f;
        break;
      case ExcelShapeDashLineStyle.Dotted:
      case ExcelShapeDashLineStyle.Dotted_Round:
      case ExcelShapeDashLineStyle.Dashed:
      case ExcelShapeDashLineStyle.Dash_Dot:
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        this._borderWidth = 1f;
        break;
      case ExcelShapeDashLineStyle.Medium_Dashed:
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        this._borderWidth = 2f;
        break;
    }
    return this._borderWidth;
  }

  private List<HeaderFooter> DrawHeadersAndFooters(
    PdfSection pdfSection,
    WorksheetImpl sheetImpl,
    ChartImpl chart)
  {
    RectangleF rectangleF = new RectangleF();
    List<HeaderFooter> headerFooterList = new List<HeaderFooter>();
    HeaderFooter headerFooter1 = (HeaderFooter) null;
    PageSetupBaseImpl pageSetupBase;
    string sheetOrchartname;
    if (sheetImpl != null)
    {
      pageSetupBase = sheetImpl.PageSetupBase;
      sheetOrchartname = sheetImpl.Name;
    }
    else
    {
      pageSetupBase = chart.PageSetupBase;
      sheetOrchartname = chart.FindParent(typeof (WorksheetImpl)) == null ? chart.Name : (chart.FindParent(typeof (WorksheetImpl)) as WorksheetImpl).Name;
    }
    if (this._workBookImpl == null && sheetImpl != null)
      this._workBookImpl = (WorkbookImpl) sheetImpl.Workbook;
    else if (this._workBookImpl == null)
      this._workBookImpl = (WorkbookImpl) chart.Workbook;
    if (this._predefinedHeaderFooter.Count == 0 && sheetImpl != null)
      this.IntializeHeaderFooter((IWorksheet) sheetImpl);
    else if (this._predefinedHeaderFooter.Count == 0 && chart != null)
      this.IntializeHeaderFooter((IChart) chart);
    if (!string.IsNullOrEmpty(pageSetupBase.FullHeaderString))
    {
      headerFooter1 = new HeaderFooter();
      rectangleF = new RectangleF(0.0f, 0.0f, pdfSection.PageSettings.Width, 0.0f);
      headerFooter1.TemplateSize = rectangleF;
      headerFooter1.HeaderFooterName = "Header";
    }
    Dictionary<string, string> pageSetups1 = new Dictionary<string, string>();
    if (!string.IsNullOrEmpty(pageSetupBase.LeftHeader))
      pageSetups1.Add("L", pageSetupBase.LeftHeader);
    if (!string.IsNullOrEmpty(pageSetupBase.CenterHeader))
      pageSetups1.Add("C", pageSetupBase.CenterHeader);
    if (!string.IsNullOrEmpty(pageSetupBase.RightHeader))
      pageSetups1.Add("R", pageSetupBase.RightHeader);
    if (!string.IsNullOrEmpty(pageSetupBase.FullHeaderString))
    {
      headerFooter1.HeaderFooterSections = this.GetHeaderFooterOptions(pageSetups1, pdfSection, (IPageSetupBase) pageSetupBase, pdfSection.PageSettings.Width - (pdfSection.PageSettings.Margins.Left + pdfSection.PageSettings.Margins.Right), "Header", sheetOrchartname);
      headerFooterList.Add(headerFooter1);
      this._allowHeader = this._allowHeaderFooterOnce;
    }
    if (!string.IsNullOrEmpty(pageSetupBase.FullFooterString))
    {
      HeaderFooter headerFooter2 = new HeaderFooter();
      rectangleF = new RectangleF(0.0f, 0.0f, pdfSection.PageSettings.Width, 0.0f);
      headerFooter2.TemplateSize = rectangleF;
      headerFooter2.HeaderFooterName = "Footer";
      Dictionary<string, string> pageSetups2 = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty(pageSetupBase.LeftFooter))
        pageSetups2.Add("L", pageSetupBase.LeftFooter);
      if (!string.IsNullOrEmpty(pageSetupBase.CenterFooter))
        pageSetups2.Add("C", pageSetupBase.CenterFooter);
      if (!string.IsNullOrEmpty(pageSetupBase.RightFooter))
        pageSetups2.Add("R", pageSetupBase.RightFooter);
      if (!string.IsNullOrEmpty(pageSetupBase.FullFooterString))
      {
        headerFooter2.HeaderFooterSections = this.GetHeaderFooterOptions(pageSetups2, pdfSection, (IPageSetupBase) pageSetupBase, pdfSection.PageSettings.Width - (pdfSection.PageSettings.Margins.Left + pdfSection.PageSettings.Margins.Right), "Footer", sheetOrchartname);
        headerFooterList.Add(headerFooter2);
        this._allowFooter = this._allowHeaderFooterOnce;
      }
    }
    return headerFooterList;
  }

  private void DrawLines(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    float width,
    float height,
    float startX,
    float startY)
  {
    float x1_1 = startX;
    float num1 = startY;
    PdfPen pen = sheet.DefaultGridlineColor ? new PdfPen(PdfBrushes.Black) : new PdfPen(sheet.Workbook.GetPaletteColor(sheet.GridLineColor));
    if (this.IsPrintTitleRowPage && firstRow != this._pageSetupOption.PrintTitleFirstRow)
      pen.Width = 0.1f;
    graphics.DrawLine(pen, x1_1, num1, width + x1_1, num1);
    pen.Width = 0.1f;
    Dictionary<int, List<int>> dictionary1 = new Dictionary<int, List<int>>();
    Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
    if (sheet.HasMergedCells)
    {
      foreach (KeyValuePair<long, MergedCellInfo> mergedRegion in this._mergedRegions)
      {
        int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(mergedRegion.Key);
        int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(mergedRegion.Key);
        int num2 = rowFromCellIndex + (mergedRegion.Value.RowSpan - 1);
        int num3 = columnFromCellIndex + (mergedRegion.Value.ColSpan - 1);
        if (mergedRegion.Value.IsFirst && rowFromCellIndex <= lastRow)
        {
          for (int key = rowFromCellIndex; key < num2; ++key)
          {
            List<int> intList = new List<int>();
            for (int index = columnFromCellIndex; index <= num3; ++index)
            {
              if (firstColumn <= index && index <= lastColumn)
                intList.Add(index);
            }
            if (dictionary1 != null && dictionary1.ContainsKey(key))
            {
              for (int index = 0; index < intList.Count; ++index)
              {
                if (!dictionary1[key].Contains(intList[index]) && firstColumn <= intList[index] && intList[index] <= lastColumn)
                  dictionary1[key].Add(intList[index]);
              }
            }
            else
              dictionary1.Add(key, intList);
          }
          for (int key = columnFromCellIndex; key < num3; ++key)
          {
            List<int> intList = new List<int>();
            for (int index = rowFromCellIndex; index <= num2; ++index)
            {
              if (firstRow <= index && index <= lastRow)
                intList.Add(index);
            }
            if (dictionary2 != null && dictionary2.ContainsKey(key))
            {
              for (int index = 0; index < intList.Count; ++index)
              {
                if (!dictionary2[key].Contains(intList[index]) && firstRow <= intList[index] && intList[index] <= lastRow)
                  dictionary2[key].Add(intList[index]);
              }
            }
            else
              dictionary2.Add(key, intList);
          }
        }
      }
    }
    for (int index = firstRow; index <= lastRow; ++index)
    {
      num1 += this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetSize(index), PdfGraphicsUnit.Point);
      float x1_2 = startX;
      if (sheet.HasMergedCells && dictionary1 != null && dictionary1.ContainsKey(index) && dictionary1[index].Count != 0)
      {
        dictionary1[index].Sort();
        int itemIndex = firstColumn;
        int rowIndex1 = firstColumn - 1;
        foreach (int num4 in dictionary1[index])
        {
          if (itemIndex == num4)
          {
            itemIndex = num4 + 1;
          }
          else
          {
            int rowIndex2 = num4 - 1;
            float num5;
            if (itemIndex == rowIndex2)
            {
              x1_2 += this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowIndex1), PdfGraphicsUnit.Point);
              num5 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
            }
            else
            {
              x1_2 += this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowIndex1), PdfGraphicsUnit.Point);
              num5 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowIndex2), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point);
            }
            rowIndex1 = itemIndex - 1;
            graphics.DrawLine(pen, x1_2, num1, x1_2 + num5, num1);
            itemIndex = num4 + 1;
          }
        }
        if (itemIndex - 1 != lastColumn)
        {
          int rowIndex3 = lastColumn;
          float x1_3;
          float num6;
          if (itemIndex == rowIndex3)
          {
            x1_3 = x1_2 + (this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowIndex1), PdfGraphicsUnit.Point));
            num6 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
          }
          else
          {
            x1_3 = x1_2 + (this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowIndex1), PdfGraphicsUnit.Point));
            num6 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowIndex3), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point);
          }
          graphics.DrawLine(pen, x1_3, num1, x1_3 + num6, num1);
        }
      }
      else
        graphics.DrawLine(pen, startX, num1, width + x1_2, num1);
    }
    float num7 = startX;
    pen.Width = 1f;
    if (lastRow != this._pageSetupOption.PrintTitleLastRow)
      graphics.DrawLine(pen, startX, num1, width + num7, num1);
    startY -= 0.5f;
    float y2_1 = num1 + 0.5f;
    if (this.IsPrintTitleColumnPage && firstColumn != this._pageSetupOption.PrintTitleFirstColumn)
      pen.Width = 0.1f;
    graphics.DrawLine(pen, num7, startY, num7, y2_1);
    pen.Width = 0.1f;
    float y2_2 = y2_1;
    for (int index = firstColumn; index <= lastColumn; ++index)
    {
      num7 += this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(index), PdfGraphicsUnit.Point);
      float y1_1 = 0.25f;
      if (sheet.HasMergedCells && dictionary2 != null && dictionary2.ContainsKey(index) && dictionary2[index].Count != 0)
      {
        dictionary2[index].Sort();
        int itemIndex = firstRow;
        int rowIndex4 = firstRow - 1;
        foreach (int num8 in dictionary2[index])
        {
          if (itemIndex == num8)
          {
            itemIndex = num8 + 1;
          }
          else
          {
            int rowIndex5 = num8 - 1;
            float num9;
            if (itemIndex == rowIndex5)
            {
              y1_1 += this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(rowIndex4), PdfGraphicsUnit.Point);
              num9 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
            }
            else
            {
              y1_1 += this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(rowIndex4), PdfGraphicsUnit.Point);
              num9 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(rowIndex5), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point);
            }
            rowIndex4 = itemIndex - 1;
            graphics.DrawLine(pen, num7, y1_1, num7, y1_1 + num9);
            itemIndex = num8 + 1;
          }
        }
        if (itemIndex - 1 != lastRow)
        {
          int rowIndex6 = lastRow;
          float y1_2;
          float num10;
          if (itemIndex == rowIndex6)
          {
            y1_2 = y1_1 + (this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(rowIndex4), PdfGraphicsUnit.Point));
            num10 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
          }
          else
          {
            y1_2 = y1_1 + (this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(rowIndex4), PdfGraphicsUnit.Point));
            num10 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(rowIndex6), PdfGraphicsUnit.Point) - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(itemIndex - 1), PdfGraphicsUnit.Point);
          }
          graphics.DrawLine(pen, num7, y1_2, num7, y1_2 + num10);
        }
      }
      else
        graphics.DrawLine(pen, num7, startY, num7, y2_2);
    }
    pen.Width = 1f;
    if (lastColumn == this._pageSetupOption.PrintTitleLastColumn)
      return;
    graphics.DrawLine(pen, num7, startY, num7, y2_2);
  }

  private void IterateMerges(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.MergeMethod method,
    float originalWidth,
    float startX,
    float startY)
  {
    if (this._lstRegions == null || this._lstRegions.Count <= 0)
      return;
    int index = 0;
    for (int count = this._lstRegions.Count; index < count; ++index)
      method(sheet, this._lstRegions[index], firstRow, firstColumn, lastRow, lastColumn, graphics, originalWidth, startX, startY);
    this._lstRegions.Clear();
    this._lstRegions = (List<MergeCellsRecord.MergedRegion>) null;
  }

  private void DrawMerge(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    float originalWidth,
    float startX,
    float startY)
  {
    ExtendedFormatImpl format = sheet.MergeCells.GetFormat(mergedRegion);
    RectangleF mergedRectangle = this.GetMergedRectangle(sheet, mergedRegion, firstRow, firstColumn, lastRow, lastColumn, startX, startY);
    if (this._excelToPdfSettings.EnableRTL)
    {
      mergedRectangle.X = originalWidth - mergedRectangle.X;
      mergedRectangle.X -= mergedRectangle.Width;
    }
    IRange cell = sheet[mergedRegion.RowFrom + 1, mergedRegion.ColumnFrom + 1];
    float num = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(mergedRegion.ColumnFrom + 1, lastColumn), PdfGraphicsUnit.Point);
    float mergedWidth = 0.0f;
    if (lastColumn == sheet.LastColumn)
    {
      if ((double) mergedRectangle.Right > (double) this._sheetWidth)
      {
        mergedWidth = mergedRectangle.Width;
        mergedRectangle.Width = num;
      }
    }
    else if ((double) mergedRectangle.Width > (double) num)
      mergedWidth = mergedRectangle.Width;
    int lastCell = 0;
    if ((double) mergedRectangle.Height <= 0.0 || (double) mergedRectangle.Width <= 0.0)
      return;
    if (cell.MergeArea.LastRow == lastRow)
      lastCell = 1;
    else if (cell.MergeArea.LastRow > lastRow)
      lastCell = 2;
    this.DrawCell(format, cell, mergedRectangle, mergedRectangle, graphics, mergedWidth, this.GetPrintText(cell), cell.HasStyle, lastCell);
  }

  private string GetPrintText(IRange cell)
  {
    string printText = cell.DisplayText;
    WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
    if (worksheet.PageSetup.PrintErrors != ExcelPrintErrors.PrintErrorsDisplayed)
    {
      switch (worksheet.GetCellType(cell.Row, cell.Column, true))
      {
        case WorksheetImpl.TRangeValueType.Error:
        case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
          switch (worksheet.PageSetup.PrintErrors)
          {
            case ExcelPrintErrors.PrintErrorsBlank:
              printText = "";
              break;
            case ExcelPrintErrors.PrintErrorsDash:
              printText = "--";
              break;
            case ExcelPrintErrors.PrintErrorsNA:
              printText = "#N/A";
              break;
          }
          break;
      }
    }
    return printText;
  }

  private void ApplyMergeCellCondiFormat(IRange cell, RectangleF rectangle, PdfGraphics graphics)
  {
    ExtendedFormatImpl wrapped = (cell.CellStyle as CellStyle).Wrapped;
    long key = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column;
    if (this._sortedListCf.ContainsKey(key))
      this._sortedListCf.TryGetValue(key, out wrapped);
    this.DrawBackground((IInternalExtendedFormat) wrapped, rectangle, graphics, cell);
  }

  private void DrawBackGround(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.CellMethod method,
    float originalWidth,
    float startX,
    float startY)
  {
    bool flag = false;
    if (sheet.ConditionalFormats.Count > 0)
      flag = true;
    MigrantRangeImpl cell = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = num1 + startX;
    float num4 = num2 + startY;
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      float y1 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow, index1 - 1), PdfGraphicsUnit.Point) + startY;
      float num5 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetSize(index1), PdfGraphicsUnit.Point);
      for (int index2 = firstColumn; (double) num5 > 0.0 && index2 <= lastColumn; ++index2)
      {
        cell.ResetRowColumn(index1, index2);
        if (((cell.CellStyle as CellStyle).Wrapped.FillPattern != ExcelPattern.None || flag && this._sortedListCf.ContainsKey(RangeImpl.GetCellIndex(index2, index1)) || sheet.PivotTables.Count > 0) && !this.IsMerged((IRange) cell))
        {
          float num6 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, index2 - 1), PdfGraphicsUnit.Point) + startX;
          float num7 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(index2), PdfGraphicsUnit.Point);
          if (this._excelToPdfSettings.EnableRTL)
            num6 = originalWidth - num6 - num7;
          float x = num6;
          float width = num7;
          float y2 = y1;
          float height = num5;
          long cellIndex = RangeImpl.GetCellIndex(index2, index1);
          if (this._sortedListCf.ContainsKey(cellIndex) && this._sortedListCf[cellIndex] is ExtendedFormatStandAlone formatStandAlone && formatStandAlone.HasDataBar)
          {
            float num8 = Math.Abs((float) formatStandAlone.DataBarPercent);
            if ((double) num8 != (double) int.MinValue)
            {
              bool isNegativeBar = formatStandAlone.IsNegativeBar;
              if (isNegativeBar && formatStandAlone.NegativeBarPoint > 0.0 || formatStandAlone.NegativeBarPoint > 0.0)
              {
                float num9 = num7 * (float) formatStandAlone.NegativeBarPoint;
                PdfPen pen = new PdfPen(formatStandAlone.BarAxisColor, 1f);
                pen.DashStyle = PdfDashStyle.Dash;
                if (!sheet.PageSetup.BlackAndWhite)
                  graphics.DrawLine(pen, new PointF(x + num9, y1), new PointF(x + num9, y1 + num5));
                if (formatStandAlone.DataBarPercent < 0.0)
                {
                  if (formatStandAlone.HasDataBarBorder)
                  {
                    num6 = (double) num8 != 1.0 ? (float) ((double) num6 + (double) num9 - (double) num9 * (double) num8 + 1.0) : (float) ((double) num6 + (double) num9 - (double) num9 * (double) num8 + 1.0 + 1.5);
                    width = (float) ((double) x + (double) num9 - (double) num6 - 1.0);
                  }
                  else
                  {
                    num6 = (double) num8 != 1.0 ? (float) ((double) num6 + (double) num9 - (double) num9 * (double) num8 + 0.5) : (float) ((double) num6 + (double) num9 - (double) num9 * (double) num8 + 0.5 + 1.0);
                    width = (float) ((double) x + (double) num9 - (double) num6 - 0.5);
                  }
                  if (formatStandAlone.DataBarDirection != DataBarDirection.rightToLeft)
                    formatStandAlone.Color = formatStandAlone.NegativeFillColor;
                }
                else
                {
                  if (formatStandAlone.HasDataBarBorder)
                  {
                    width = (double) num8 != 1.0 ? (float) (((double) num7 - (double) num9) * (double) num8 - 1.0) : (float) (((double) num7 - (double) num9) * (double) num8 - 1.0 - 1.5);
                    num6 += num9 + 1f;
                  }
                  else
                  {
                    width = (double) num8 != 1.0 ? (float) (((double) num7 - (double) num9) * (double) num8 - 0.5) : (float) (((double) num7 - (double) num9) * (double) num8 - 0.5 - 1.5);
                    num6 += num9 + 0.5f;
                  }
                  if (formatStandAlone.DataBarDirection == DataBarDirection.rightToLeft)
                    formatStandAlone.Color = formatStandAlone.NegativeFillColor;
                }
                x = num6;
              }
              else
                width = num7 * num8;
              bool hasDataBarBorder = formatStandAlone.HasDataBarBorder;
              DataBarDirection dataBarDirection = formatStandAlone.DataBarDirection;
              if (dataBarDirection == DataBarDirection.rightToLeft && dataBarDirection != DataBarDirection.context || formatStandAlone.MinValue < 0.0 && formatStandAlone.MaxValue < 0.0)
                num6 = ((double) num6 <= (double) num7 ? num7 - num6 : num6 + num7) - num7 * num8;
              if ((double) num8 == 1.0 && dataBarDirection != DataBarDirection.rightToLeft && formatStandAlone.NegativeBarPoint == 0.0)
              {
                width -= 3.2f;
                x = num6 + 2f;
              }
              if ((double) num8 != 1.0 && dataBarDirection != DataBarDirection.rightToLeft && formatStandAlone.NegativeBarPoint == 0.0)
              {
                width -= 2f;
                x = num6 + 2f;
              }
              if ((double) num8 == 1.0 && dataBarDirection == DataBarDirection.rightToLeft && formatStandAlone.NegativeBarPoint == 0.0)
              {
                width -= 2.5f;
                x = num6 + 2f;
              }
              if ((double) num8 != 1.0 && dataBarDirection == DataBarDirection.rightToLeft && formatStandAlone.NegativeBarPoint == 0.0)
              {
                --width;
                x = num6;
              }
              y2 = y1 + 2.5f;
              height = num5 - 5f;
              double result;
              double.TryParse(cell.CalculatedValue, out result);
              if (result < 0.0)
                formatStandAlone.Color = formatStandAlone.NegativeFillColor;
              if (hasDataBarBorder && (double) width != 0.0 && !sheet.PageSetup.BlackAndWhite)
              {
                RectangleF rectangle = new RectangleF(x, y2, width, height);
                PdfPen pen = (PdfPen) null;
                if (isNegativeBar && formatStandAlone.DataBarPercent < 0.0)
                  pen = new PdfPen(formatStandAlone.NegativeDataBarBorderColor);
                else if (formatStandAlone.DataBarPercent > 0.0)
                  pen = new PdfPen(formatStandAlone.DataBarBorderColor);
                graphics.DrawRectangle(pen, rectangle);
              }
            }
          }
          if (this._excelToPdfSettings.DisplayGridLines == GridLinesDisplayStyle.Auto && sheet.PageSetup.PrintGridlines || this._excelToPdfSettings.DisplayGridLines == GridLinesDisplayStyle.Visible)
          {
            float num10 = 0.5f;
            if (index2 == firstColumn)
            {
              x += num10;
              width -= num10;
            }
            else if (index2 == lastColumn)
              width -= num10;
            if (index1 == firstRow)
            {
              y2 += num10;
              height -= num10;
            }
            else if (index1 == lastRow)
              height -= num10;
          }
          RectangleF rect = new RectangleF(x, y2, width, height);
          method((IRange) cell, rect, graphics);
        }
      }
    }
    if (cell == null)
      ;
  }

  private bool IsMerged(IRange cell)
  {
    return this._mergedRegions.ContainsKey(RangeImpl.GetCellIndex(cell.Column, cell.Row));
  }

  private void DrawBackground(IRange cell, RectangleF rectangle, PdfGraphics graphics)
  {
    ExtendedFormatImpl extendedFormatImpl = (cell.CellStyle as CellStyle).Wrapped;
    int num1 = 1;
    int num2 = 1;
    for (int index = 0; index < cell.Worksheet.PivotTables.Count; ++index)
    {
      if (cell.Worksheet.PivotTables[index].Location != null && cell.Row == cell.Worksheet.PivotTables[index].Location.Row && cell.Column == cell.Worksheet.PivotTables[index].Location.Column)
      {
        this._pivotTableRange = cell.Worksheet.PivotTables[index].Location;
        this._pivotImpl = cell.Worksheet.PivotTables[index] as PivotTableImpl;
      }
    }
    if (this._pivotTableRange != null)
    {
      num1 = this._pivotTableRange.Row;
      num2 = this._pivotTableRange.Column;
    }
    PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer(cell.Worksheet);
    if (cell.Worksheet.PivotTables.Count != 0 && this._pivotImpl.PageFields.Count != 0)
    {
      if (num1 - 2 == cell.Row && num2 == cell.Column)
        extendedFormatImpl = tableStyleRenderer.GetPageFilterLabel(this._pivotImpl.BuiltInStyle);
      if (num1 - 2 == cell.Row && num2 + 1 == cell.Column)
        extendedFormatImpl = tableStyleRenderer.GetPageFilterValue(this._pivotImpl.BuiltInStyle);
    }
    if (this._pivotTableRange != null && this._pivotTableRange.Row <= cell.Row && this._pivotTableRange.LastRow >= cell.LastRow && this._pivotTableRange.Column <= cell.Column && this._pivotTableRange.LastColumn >= cell.LastColumn && this._pivotImpl.PivotLayout != null)
    {
      if (cell.Row - num1 <= this._pivotImpl.PivotLayout.maxRowCount && this._pivotImpl.PivotLayout[cell.Row - num1].Count > cell.Column - num2 && (cell.CellStyle.Color.Name == Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).Name || cell.CellStyle.Color.Name == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).Name))
        extendedFormatImpl = this._pivotImpl.PivotLayout[cell.Row - num1, cell.Column - num2].XF;
    }
    else
    {
      long key = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column;
      if (this._sortedListCf.ContainsKey(key))
        this._sortedListCf.TryGetValue(key, out extendedFormatImpl);
    }
    this.DrawBackground((IInternalExtendedFormat) extendedFormatImpl, rectangle, graphics, cell);
  }

  private void DrawBackground(
    IInternalExtendedFormat internalExtendedFormat,
    RectangleF rect,
    PdfGraphics graphics,
    IRange cell)
  {
    if ((double) rect.Height <= 0.0 || (double) rect.Width <= 0.0)
      return;
    bool flag = false;
    if (this._sortedListCf.ContainsKey(((long) cell.Row << 32 /*0x20*/) + (long) cell.Column))
      flag = true;
    if (internalExtendedFormat.FillPattern == ExcelPattern.None)
    {
      rect.Offset(1f, 1f);
      rect.Width -= 3f;
      rect.Height -= 3f;
      IBorders borders = internalExtendedFormat.Borders;
      rect = this.UpdateRectangleCoordinates(rect, borders);
      PdfBrush transparent = PdfBrushes.Transparent;
      graphics.DrawRectangle(transparent, rect);
    }
    else if (internalExtendedFormat.FillPattern == ExcelPattern.None && (internalExtendedFormat.Color != Color.White || internalExtendedFormat.Color == Color.Empty && flag))
    {
      PdfBrush brush = (PdfBrush) new PdfSolidBrush(new PdfColor(internalExtendedFormat.Color));
      graphics.DrawRectangle(brush, rect);
    }
    else if (RangeImpl.GetHatchStyle(internalExtendedFormat.FillPattern) != ~HatchStyle.Horizontal && !this._workSheet.PageSetup.BlackAndWhite)
    {
      HatchBrush hatchBrush = new HatchBrush(RangeImpl.GetHatchStyle(internalExtendedFormat.FillPattern), this.NormalizeColor(internalExtendedFormat.PatternColor), this.NormalizeColor(internalExtendedFormat.Color));
      if ((double) rect.Width < 1.0)
        rect.Width = 1f;
      if ((double) rect.Height < 1.0)
        rect.Height = 1f;
      Bitmap bitmap = new Bitmap((int) rect.Width, (int) rect.Height);
      System.Drawing.Graphics graphics1 = System.Drawing.Graphics.FromImage((Image) bitmap);
      graphics1.FillRectangle((Brush) hatchBrush, 0.0f, 0.0f, rect.Width, rect.Height);
      graphics.DrawImage(this.GetPdfImage((Image) bitmap), rect);
      graphics1.Dispose();
      bitmap.Dispose();
      hatchBrush.Dispose();
    }
    else
    {
      PdfBrush brush = this.GetBrush(internalExtendedFormat);
      if (this._workSheet.PageSetup.BlackAndWhite)
        brush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.White);
      graphics.DrawRectangle(brush, rect);
    }
  }

  private void DrawMergedBackground(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    float originalWidth,
    float startX,
    float startY)
  {
    MergeCellsImpl mergeCells = sheet.MergeCells;
    ExtendedFormatImpl format = mergeCells.GetFormat(mergedRegion);
    float num1 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetSize(mergedRegion.RowFrom + 1), PdfGraphicsUnit.Point);
    float num2 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(mergedRegion.ColumnFrom + 1), PdfGraphicsUnit.Point);
    int num3 = firstRow > mergedRegion.RowFrom + 1 ? firstRow : mergedRegion.RowFrom + 1;
    int rowEnd = firstColumn > mergedRegion.ColumnFrom + 1 ? firstColumn : mergedRegion.ColumnFrom + 1;
    float num4 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow, num3), PdfGraphicsUnit.Point) - num1;
    float num5 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, rowEnd), PdfGraphicsUnit.Point) - num2;
    float y = num4 + startY;
    float x = num5 + startX;
    if (this._excelToPdfSettings.EnableRTL)
      x = originalWidth - x;
    float height = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(num3, mergedRegion.RowTo + 1), PdfGraphicsUnit.Point);
    float mergedWidth = this.GetMergedWidth(firstColumn, lastColumn, mergedRegion.ColumnFrom + 1, mergedRegion.ColumnTo + 1);
    float num6 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, lastColumn), PdfGraphicsUnit.Point);
    float width = this.Pdf_UnitConverter.ConvertFromPixels(mergedWidth, PdfGraphicsUnit.Point);
    if ((double) width > (double) num6)
      width = num6 - x;
    if (this._excelToPdfSettings.EnableRTL)
      x -= width;
    IRange cell = sheet[num3, mergedRegion.ColumnFrom + 1];
    RectangleF rect = new RectangleF(x, y, width, height);
    if ((double) height > 0.0 && (double) width > 0.0)
    {
      ++rect.Width;
      ++rect.Height;
      this.DrawBackground((IInternalExtendedFormat) format, rect, graphics, cell);
    }
    if (cell != null)
      ;
    if (format != null)
      ;
    if (mergeCells != null)
      ;
    if (sheet != null)
      sheet = (WorksheetImpl) null;
    if (mergedRegion != null)
      mergedRegion = (MergeCellsRecord.MergedRegion) null;
    if (graphics == null)
      return;
    graphics.Flush();
    graphics = (PdfGraphics) null;
  }

  private float GetMergedWidth(
    int firstColumn,
    int lastColumn,
    int mergedFirstColumn,
    int mergedLastColumn)
  {
    int rowStart = firstColumn;
    if (mergedFirstColumn <= lastColumn)
      rowStart = mergedFirstColumn;
    int rowEnd = mergedLastColumn > lastColumn ? lastColumn : mergedLastColumn;
    return this.ColumnWidthGetter.GetTotal(rowStart, rowEnd);
  }

  private void DrawCells(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    float originalWidth,
    float startX,
    float startY,
    float rangeWidth)
  {
    MigrantRangeImpl cell1 = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    MigrantRangeImpl cell2 = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    MigrantRangeImpl cell3 = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    int maxColumnCount = cell1.Workbook.MaxColumnCount;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = num1 + startX;
    float num4 = num2 + startY;
    this._isFirstRowOfPage = true;
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      this._adjacentRectWidth = 0.0f;
      this._borderRange = (IRange) null;
      float y = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow, index1 - 1), PdfGraphicsUnit.Point) + startY;
      float height = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetSize(index1), PdfGraphicsUnit.Point);
      this._maxRowFontSize = 1.0;
      for (int iColumn = firstColumn; iColumn <= lastColumn; ++iColumn)
      {
        cell3.ResetRowColumn(index1, iColumn);
        if (!cell3.IsBlank && cell3.CellStyle.Font.Size > this._maxRowFontSize && !this.IsMerged((IRange) cell3))
        {
          this._maxRowFontSize = cell3.CellStyle.Font.Size;
          int extendedFormatIndex = (int) cell3.ExtendedFormatIndex;
          ExtendedFormatImpl innerExtFormat = cell3.Workbook.InnerExtFormats[extendedFormatIndex];
          this._maxRowPdfTextformat = new PdfStringFormat();
          this._maxRowPdfTextformat.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
          this._maxRowPdfTextformat.WordWrap = innerExtFormat.WrapText ? PdfWordWrapType.Word : PdfWordWrapType.None;
          this._maxRowPdfTextformat.Alignment = this.GetHorizontalAlignmentFromExtendedFormat((IExtendedFormat) innerExtFormat, (IRange) cell3);
          this._maxRowPdfTextformat.LineAlignment = this.GetVerticalAlignmentFromExtendedFormat((IExtendedFormat) innerExtFormat);
          this._maxRowPdfTextformat.FirstLineIndent = (float) (innerExtFormat.IndentLevel * 5);
          IFont font = innerExtFormat.Font;
          if (this._workSheet.PageSetup.BlackAndWhite)
            font.Color = ExcelKnownColors.Black;
          Font nativeFont = ((FontImpl) font).GenerateNativeFont();
          this._maxPdfFont = this.GetPdfFont(nativeFont, this._excelToPdfSettings.EmbedFonts, (Stream) null);
          if (innerExtFormat != null)
            ;
          if (nativeFont != null)
            ;
          if (font != null)
            ;
        }
      }
      this.isFirstColumnOfPage = true;
      if ((double) height > 0.0)
      {
        for (int index2 = firstColumn; index2 <= lastColumn; ++index2)
        {
          cell1.ResetRowColumn(index1, index2);
          if (!sheet.CellRecords.Contains(index1, index2) && this._splitTextCollection.Count == 0 && sheet.PivotTables.Count == 0 || cell1.Comment != null && cell1.Comment.IsVisible)
          {
            IBorders borders = (IBorders) null;
            if (!this._extBorders.TryGetValue((int) cell1.ExtendedFormatIndex, out borders))
              borders = cell1.Workbook.InnerExtFormats[(int) cell1.ExtendedFormatIndex].Borders;
            if ((!cell1.HasStyle || !this.CheckCellBorderStyle(borders)) && !this.CheckCellStyles((IRange) cell1) && (cell1.Comment == null || !cell1.Comment.IsVisible))
              continue;
          }
          float x = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, index2 - 1), PdfGraphicsUnit.Point) + startX;
          if (this._excelToPdfSettings.EnableRTL)
            x = originalWidth - x;
          if (this._workSheet.PageSetup.BlackAndWhite)
            cell1.CellStyle.Font.Color = ExcelKnownColors.Black;
          if (!this.IsMerged((IRange) cell1))
          {
            this.isCurrentCellNotMerged = false;
            float width = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(index2), PdfGraphicsUnit.Point);
            if (firstColumn == lastColumn && this.SheetPageSetup.PaperSize == ExcelPaperSize.PaperA4 && this._excelToPdfSettings.LayoutOptions == LayoutOptions.NoScaling && this.SheetPageSetup.Orientation == ExcelPageOrientation.Portrait && (double) width > (double) this._sheetWidth)
              width = this._sheetWidth;
            if (this._excelToPdfSettings.EnableRTL)
              x -= width;
            if ((double) width > 0.0)
            {
              RectangleF rectangleF = new RectangleF(x, y, width, height);
              bool lastColumnChanged = false;
              RectangleF adjacentCells = this.GetAdjacentCells((IRange) cell1, rectangleF, firstColumn, cell2, originalWidth, ref lastColumnChanged);
              if (this._hasPrintTitleColumn)
                adjacentCells.X = x;
              float mergedWidth = 0.0f;
              int lastCell = 0;
              if (cell1.Row == lastRow)
                lastCell = 1;
              this.DrawCell(cell1, rectangleF, adjacentCells, graphics, mergedWidth, lastCell);
              this.isFirstColumnOfPage = false;
            }
            this.isCurrentCellNotMerged = true;
          }
        }
        this._isFirstRowOfPage = false;
      }
    }
    if (cell1 != null)
      ;
    if (cell2 != null)
      ;
    if (cell3 != null)
      ;
    if (sheet != null)
      sheet = (WorksheetImpl) null;
    if (this._tableBorderList.Count > 0)
      this._tableBorderList.Clear();
    if (graphics == null)
      return;
    graphics.Flush();
    graphics = (PdfGraphics) null;
  }

  private void DrawCell(
    MigrantRangeImpl cell,
    RectangleF cellRect,
    RectangleF adjacentRect,
    PdfGraphics graphics,
    float mergedWidth,
    int lastCell)
  {
    if (cell == null)
      throw new ArgumentNullException(nameof (cell));
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    string printText = this.GetPrintText((IRange) cell);
    int extendedFormatIndex = (int) cell.ExtendedFormatIndex;
    ExtendedFormatImpl innerExtFormat = cell.Workbook.InnerExtFormats[extendedFormatIndex];
    bool hasStyle = cell.HasStyle;
    if (this.SplitTexts.Count > 0 || cell.Worksheet.PivotTables.Count > 0 || hasStyle || !string.IsNullOrEmpty(printText) || this.CheckCellStyles((IRange) cell))
      this.DrawCell(innerExtFormat, (IRange) cell, cellRect, adjacentRect, graphics, mergedWidth, printText, hasStyle, lastCell);
    else if (cell.Comment != null && cell.Comment.IsVisible)
      this._commentCellPosition.Add(RangeImpl.GetCellIndex(cell.Column, cell.Row), cellRect);
    if (innerExtFormat != null)
      ;
    if (cell != null)
      cell = (MigrantRangeImpl) null;
    if (graphics == null)
      return;
    graphics.Flush();
    graphics = (PdfGraphics) null;
  }

  private void DrawCell(
    ExtendedFormatImpl extendedFormatImpl,
    IRange cell,
    RectangleF cellRect,
    RectangleF adjacentRect,
    PdfGraphics graphics,
    float mergedWidth,
    string cellText,
    bool isHasStyle,
    int lastCell)
  {
    bool flag1 = false;
    this._currentCell = cell;
    this._currentCellRect = cellRect;
    this._adjacentRectWidth += adjacentRect.Width;
    int indentLevel = extendedFormatImpl.IndentLevel;
    bool flag2 = false;
    string fontName1 = extendedFormatImpl.Font.FontName;
    double size = extendedFormatImpl.Font.Size;
    SplitText splitText1 = (SplitText) null;
    bool flag3 = false;
    bool flag4 = true;
    int num1 = 1;
    int num2 = 1;
    for (int index = 0; index < cell.Worksheet.PivotTables.Count; ++index)
    {
      if (cell.Worksheet.PivotTables[index].Location != null && cell.Row == cell.Worksheet.PivotTables[index].Location.Row && cell.Column == cell.Worksheet.PivotTables[index].Location.Column)
      {
        this._pivotTableRange = cell.Worksheet.PivotTables[index].Location;
        this._pivotImpl = cell.Worksheet.PivotTables[index] as PivotTableImpl;
      }
    }
    if (this._pivotTableRange != null)
    {
      num1 = this._pivotTableRange.Row;
      num2 = this._pivotTableRange.Column;
    }
    if (cell.Worksheet.PivotTables.Count != 0 && this._pivotImpl.PageFields.Count != 0)
    {
      PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer(cell.Worksheet);
      if (num1 - 2 == cell.Row && num2 == cell.Column)
      {
        extendedFormatImpl = tableStyleRenderer.GetPageFilterLabel(this._pivotImpl.BuiltInStyle);
        extendedFormatImpl.VerticalAlignment = ExcelVAlign.VAlignTop;
      }
      if (num1 - 2 == cell.Row && num2 + 1 == cell.Column)
      {
        extendedFormatImpl = tableStyleRenderer.GetPageFilterValue(this._pivotImpl.BuiltInStyle);
        extendedFormatImpl.VerticalAlignment = ExcelVAlign.VAlignTop;
      }
      extendedFormatImpl.Font.FontName = fontName1;
      extendedFormatImpl.Font.Size = size;
    }
    if (cell.Worksheet.PivotTables.Count != 0 && this._pivotTableRange != null && this._pivotTableRange.Row <= cell.Row)
    {
      if (this._pivotImpl.EndLocation != null && this._pivotImpl.EndLocation.LastRow >= cell.LastRow && this._pivotTableRange.Column <= cell.Column && this._pivotImpl.EndLocation.LastColumn >= cell.LastColumn && this._pivotImpl.PivotLayout != null)
      {
        int rowIndex = cell.Row - num1;
        if (this._pivotImpl.PivotLayout.maxRowCount >= rowIndex && this._pivotImpl.PivotLayout[rowIndex].Count > cell.Column - num2)
        {
          extendedFormatImpl = this._pivotImpl.PivotLayout[cell.Row - num1, cell.Column - num2].XF;
          extendedFormatImpl.VerticalAlignment = cell.CellStyle.VerticalAlignment;
          if (cell.CellStyle.HorizontalAlignment != ExcelHAlign.HAlignGeneral)
          {
            extendedFormatImpl.HorizontalAlignment = cell.CellStyle.HorizontalAlignment;
            extendedFormatImpl.IndentLevel = indentLevel;
          }
          if (!extendedFormatImpl.IsPivotFormat)
          {
            extendedFormatImpl.Font.FontName = fontName1;
            extendedFormatImpl.Font.Size = size;
          }
          if (extendedFormatImpl.IsPivotFormat && extendedFormatImpl.NumberFormat != "General")
          {
            cell.NumberFormat = extendedFormatImpl.NumberFormat;
            cellText = cell.DisplayText;
          }
        }
      }
    }
    else
    {
      long key = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column;
      if (this._sortedListCf.ContainsKey(key))
      {
        this._sortedListCf.TryGetValue(key, out extendedFormatImpl);
        if (extendedFormatImpl != null && extendedFormatImpl is ExtendedFormatStandAlone)
          flag2 = (extendedFormatImpl as ExtendedFormatStandAlone).ShowIconOnly;
      }
    }
    long key1 = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column;
    if (this._sortedListCf.ContainsKey(key1))
    {
      this._sortedListCf.TryGetValue(key1, out extendedFormatImpl);
      if (extendedFormatImpl != null && extendedFormatImpl is ExtendedFormatStandAlone)
        flag4 = (extendedFormatImpl as ExtendedFormatStandAlone).ShowValue;
    }
    if (cell.Comment != null && cell.Comment.IsVisible && cell.Worksheet.PageSetup.PrintComments != ExcelPrintLocation.PrintNoComments)
    {
      long cellIndex = RangeImpl.GetCellIndex(cell.Column, cell.Row);
      if (this._commentCellPosition != null && !this._commentCellPosition.ContainsKey(cellIndex))
        this._commentCellPosition.Add(cellIndex, cellRect);
    }
    PdfBrush pdfBrush = (PdfBrush) null;
    PdfFont pdfFont = (PdfFont) null;
    this._pdfTextformat = new PdfStringFormat();
    this._pdfTextformat.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
    this._pdfTextformat.WordWrap = extendedFormatImpl.WrapText ? PdfWordWrapType.Word : PdfWordWrapType.None;
    this._pdfTextformat.Alignment = this.GetHorizontalAlignmentFromExtendedFormat((IExtendedFormat) extendedFormatImpl, cell);
    this._pdfTextformat.LineAlignment = this.GetVerticalAlignmentFromExtendedFormat((IExtendedFormat) extendedFormatImpl);
    bool flag5 = false;
    if (this.SplitTexts.Count != 0)
    {
      for (int index = 0; index < this.SplitTexts.Count; ++index)
      {
        SplitText splitText2 = this.SplitTexts[index];
        if (this._isNewPage && !splitText2.PageTemplate.Equals((object) this._pdfPageTemplate) && cell.Row == splitText2.Row && splitText2.AdjacentColumn != 0 && this._workSheet[cell.Row, cell.Column].DisplayText == string.Empty)
        {
          pdfFont = splitText2.TextFont;
          pdfBrush = splitText2.Brush;
          if (this._workSheet.PageSetup.BlackAndWhite)
            pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) Color.Black);
          this._pdfTextformat = splitText2.Format;
          if (splitText2.RichText != null && splitText2.RichText.Count > 1)
          {
            flag3 = true;
            splitText1 = splitText2;
          }
          cellText = splitText2.Text;
          adjacentRect = splitText2.OriginRect;
          this.SplitTexts.RemoveAt(index);
          flag5 = true;
          break;
        }
      }
    }
    IFont font1 = extendedFormatImpl.Font;
    string fontName2 = font1.FontName;
    bool isConditonalFormat = false;
    IBorders borders = (IBorders) null;
    if (this.isCurrentCellNotMerged || !this._extBorders.TryGetValue(extendedFormatImpl.Index, out borders))
    {
      borders = extendedFormatImpl.Borders;
      if (this._workSheet.PageSetup.BlackAndWhite)
      {
        borders.Color = ExcelKnownColors.Black;
        font1.Color = ExcelKnownColors.Black;
      }
      isConditonalFormat = cell.ConditionalFormats.Count > 0;
    }
    if (flag2)
    {
      float num3 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]) * 2f;
      float borderWidth = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]);
      adjacentRect.X += num3;
      adjacentRect.Width -= num3 + borderWidth;
      adjacentRect.Y -= this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]);
      IRange adjacentRange = this.GetAdjacentRange(cell);
      if ((double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]) > (double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]))
        adjacentRect.Y += this.GetBorderWidth(adjacentRange.Borders[ExcelBordersIndex.EdgeBottom]);
      else
        adjacentRect.Y += this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]);
      this.DrawIconSet(graphics, extendedFormatImpl, ref adjacentRect, (PdfFont) null, cellText);
    }
    if (!flag4)
    {
      float num4 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]) * 2f;
      float borderWidth = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]);
      adjacentRect.X += num4;
      adjacentRect.Width -= num4 + borderWidth;
      adjacentRect.Y -= this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]);
      IRange adjacentRange = this.GetAdjacentRange(cell);
      if ((double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]) > (double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]))
        adjacentRect.Y += this.GetBorderWidth(adjacentRange.Borders[ExcelBordersIndex.EdgeBottom]);
      else
        adjacentRect.Y += this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]);
    }
    if (cellText != "" && !flag2 && flag4)
    {
      Color numberFormatColor = (cell as RangeImpl).GetNumberFormatColor(extendedFormatImpl);
      if (extendedFormatImpl.FillPattern == ExcelPattern.Gradient)
        this.RaiseWarning($"Gradient fill in the cell : \"{cell.Address}\" is not supported", WarningType.FillPattern);
      byte charSet = (font1 as FontImpl).CharSet;
      bool flag6 = false;
      bool flag7 = false;
      FontStyle fontStyle = (this._workBook as WorkbookImpl).GetFontStyle(font1);
      Stream stream = (Stream) null;
      font1.FontName = (this._workBook.Application as ApplicationImpl).TryGetFontStream(font1.FontName, (float) font1.Size, fontStyle, out stream);
      string str1 = cellText;
      if (str1 != null)
        flag6 = this.CheckUnicode(str1);
      if (flag6 && stream == null)
      {
        flag7 = this.IsSymbol(str1);
        if (!flag7)
          font1.FontName = this.SwitchFonts(str1, charSet, fontName2);
      }
      Color color = this.NormalizeColor(font1.RGBColor);
      if (numberFormatColor != Color.Empty)
      {
        color = numberFormatColor;
        adjacentRect = cellRect;
      }
      if (this._fontTableColors != null && this._fontTableColors.Count != 0)
      {
        foreach (KeyValuePair<IRange, Color> fontTableColor in this._fontTableColors)
        {
          if (this.CheckRange(fontTableColor.Key, cell))
          {
            color = fontTableColor.Value;
            if (this._workSheet.PageSetup.BlackAndWhite)
              color = Color.Black;
          }
        }
      }
      if (!flag5)
        pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) color);
      if (this._pdfTextformat.Alignment == PdfTextAlignment.Justify)
        this._pdfTextformat.WordWrap = PdfWordWrapType.Word;
      if (cellText != "" && !cell.WrapText)
      {
        if (cellText.Contains("\n"))
          cellText = cellText.Replace("\n", string.Empty);
        if (cellText.Contains("\r"))
          cellText = cellText.Replace("\r", string.Empty);
      }
      cellText = this.GetBottomText(cellText, extendedFormatImpl.Rotation);
      string fontName3 = font1.FontName;
      Font nativeFont = ((FontImpl) font1).GenerateNativeFont();
      if (fontName3 != nativeFont.Name)
        this.RaiseWarning($"\"{fontName3}\" font in the cell : \"{cell.Address}\" is not installed in the system. It is substitued with the font : \"{nativeFont.Name}\"", WarningType.FontSubstitution);
      ushort[] numArray = new ushort[cellText.Length];
      KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, cellText, cellText.Length, numArray);
      bool flag8;
      this._pdfTextformat.RightToLeft = flag8 = this.CheckIfRTL(numArray);
      if (flag8)
        this._pdfTextformat.TextDirection = PdfTextDirection.RightToLeft;
      if (flag8 && flag6 && this.CheckForArabicOrHebrew(cellText) && cell.NumberFormat != "General" && cell.NumberFormat != "@")
        this._pdfTextformat.TextDirection = PdfTextDirection.LeftToRight;
      if (flag8 && cell.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
        this._pdfTextformat.Alignment = this.GetRTLAlignment(numArray);
      if (cellText.StartsWith(" "))
        this._pdfTextformat.MeasureTrailingSpaces = true;
      if (stream != null && stream.Length > 0L)
        this._excelToPdfSettings.EmbedFonts = true;
      bool isEmbedFont = this._excelToPdfSettings.EmbedFonts || Encoding.UTF8.GetByteCount(cellText) != cellText.Length;
      if (stream != null && stream.Length > 0L)
        isEmbedFont = true;
      if (flag6 && !this._fontList.Contains(nativeFont.Name))
      {
        nativeFont = extendedFormatImpl.Font.GenerateNativeFont();
        flag1 = true;
      }
      if (!flag5)
        pdfFont = this.GetPdfFont(nativeFont, isEmbedFont, stream);
      SizeF pdfMeasureSize = SizeF.Empty;
      PdfStringLayoutResult result = (PdfStringLayoutResult) null;
      bool hasRichText = cell.HasRichText;
      float width1;
      float num5;
      if (!hasRichText && !flag3)
      {
        pdfMeasureSize = pdfFont.MeasureString(cellText, new SizeF(0.0f, 0.0f), this._pdfTextformat, out result);
        width1 = pdfMeasureSize.Width;
        num5 = pdfMeasureSize.Height;
      }
      else
      {
        SizeF sizeF = (cell.Worksheet as WorksheetImpl).MeasureCell(cell, false, false);
        width1 = this._pdfUnitConverter.ConvertFromPixels(sizeF.Width, PdfGraphicsUnit.Point);
        num5 = this._pdfUnitConverter.ConvertFromPixels(sizeF.Height, PdfGraphicsUnit.Point);
      }
      if (extendedFormatImpl.IndentLevel > 0)
      {
        float num6 = (float) extendedFormatImpl.IndentLevel * 7.2f;
        if (!(cell as RangeImpl).HasNumber && !(cell as RangeImpl).HasFormula || !cellText.Trim().StartsWith("$") || this._pdfTextformat.Alignment != PdfTextAlignment.Right)
        {
          switch (this._pdfTextformat.Alignment)
          {
            case PdfTextAlignment.Left:
              adjacentRect.X += num6;
              break;
            case PdfTextAlignment.Right:
              adjacentRect.Width -= num6;
              break;
          }
        }
        if (this._pdfTextformat.WordWrap == PdfWordWrapType.Word)
        {
          switch (this._pdfTextformat.Alignment)
          {
            case PdfTextAlignment.Left:
              adjacentRect.Width -= num6;
              break;
            case PdfTextAlignment.Right:
              adjacentRect.X += num6;
              adjacentRect.Width -= num6;
              break;
          }
        }
        if (Math.Ceiling((double) adjacentRect.Right) > Math.Ceiling((double) this._pdfPageTemplate.Width) && Math.Ceiling((double) width1 + (double) adjacentRect.X) < Math.Ceiling((double) this._pdfPageTemplate.Width))
          adjacentRect.Width -= num6;
      }
      if (cell.HorizontalAlignment == ExcelHAlign.HAlignFill)
      {
        extendedFormatImpl.Rotation = 0;
        cellText = this.FillText(adjacentRect, cellText, pdfFont);
        pdfMeasureSize = pdfFont.MeasureString(cellText, new SizeF(0.0f, 0.0f), this._pdfTextformat, out result);
        width1 = pdfMeasureSize.Width;
      }
      if (cell.CellStyleName == "Hyperlink" && cell.Hyperlinks.Count > 0)
        this.SetHyperLink(adjacentRect, cell);
      string formula = cell.Formula;
      if (formula != null && formula.Contains("HYPERLINK"))
        this.SetHyperLink(adjacentRect, cell, formula);
      if (extendedFormatImpl.Rotation != (int) byte.MaxValue && extendedFormatImpl.Rotation != 0 && !string.IsNullOrEmpty(cellText))
      {
        char[] charArray = cellText.ToCharArray();
        SizeF sizeF1 = pdfFont.MeasureString(charArray[0].ToString(), this._pdfTextformat);
        SizeF firstCharWidth = new SizeF(sizeF1.Width, sizeF1.Height);
        float width2 = pdfFont.MeasureString(cellText, this._pdfTextformat).Width;
        SizeF sizeF2 = pdfFont.MeasureString(charArray[charArray.Length - 1].ToString(), this._pdfTextformat);
        SizeF lastCharSize = new SizeF(sizeF2.Width, sizeF2.Height);
        if (this._pdfTextformat.Alignment == PdfTextAlignment.Right || cell.HorizontalAlignment == ExcelHAlign.HAlignGeneral && this.GetCounterClockwiseRotation(extendedFormatImpl.Rotation) <= 0)
        {
          adjacentRect.Width -= this._borderWidth;
          adjacentRect.Height -= this._borderWidth;
        }
        else
        {
          adjacentRect.X += this._borderWidth;
          adjacentRect.Height -= this._borderWidth;
        }
        MergedCellInfo mergedCellInfo = new MergedCellInfo();
        if (extendedFormatImpl.Rotation == 90 && this._mergedRegions.TryGetValue(RangeImpl.GetCellIndex(cell.Column, cell.Row), out mergedCellInfo) && cellText.Contains("\n") && (double) num5 > (double) adjacentRect.Width)
        {
          StringBuilder stringBuilder = new StringBuilder();
          string[] strArray = cellText.Split(new string[1]
          {
            "\n"
          }, StringSplitOptions.RemoveEmptyEntries);
          for (int index = 0; index < strArray.Length; ++index)
          {
            double width3 = (double) adjacentRect.Width;
            SizeF sizeF3 = pdfFont.MeasureString(strArray[index], this._pdfTextformat);
            double height1 = (double) sizeF3.Height;
            sizeF3 = pdfFont.MeasureString(stringBuilder.ToString(), this._pdfTextformat);
            double height2 = (double) sizeF3.Height;
            double num7 = height1 + height2;
            if (width3 > num7)
              stringBuilder.Append(strArray[index] + "\n");
          }
          cellText = stringBuilder.ToString();
        }
        this.DrawRotatedText(adjacentRect, extendedFormatImpl, cell, cellText, graphics, pdfFont, pdfBrush, this._pdfTextformat, firstCharWidth, width2, lastCharSize);
      }
      else
      {
        WorksheetImpl.TRangeValueType cellType = (cell.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true);
        if ((double) width1 > (double) adjacentRect.Width && (cellType == WorksheetImpl.TRangeValueType.Number || cellType == (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula)))
        {
          cellText = !(cell.NumberFormat == "General") ? this.FillText(adjacentRect, "#", pdfFont) : string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.##E+00}", (object) System.Convert.ToDouble(cellText));
          pdfMeasureSize = pdfFont.MeasureString(cellText, new SizeF(0.0f, 0.0f), this._pdfTextformat, out result);
          width1 = pdfMeasureSize.Width;
          num5 = pdfMeasureSize.Height;
        }
        if (Math.Ceiling((double) adjacentRect.Right) > Math.Ceiling((double) this._pdfPageTemplate.Width) && !this.IsMerged(cell) && this._pdfTextformat.WordWrap != PdfWordWrapType.Word && this._pdfTextformat.Alignment != PdfTextAlignment.Right && !this._pdfTextformat.RightToLeft)
        {
          if (this._excelToPdfSettings.LayoutOptions == LayoutOptions.FitAllColumnsOnOnePage || this._excelToPdfSettings.LayoutOptions == LayoutOptions.FitSheetOnOnePage)
          {
            if (this._workSheet.PageSetup.PrintArea == null)
            {
              if (this._drawprinttitle)
              {
                PdfTemplate pdfTemplate = new PdfTemplate(adjacentRect.Right, this._printTitleTemplate.Height);
                pdfTemplate.Graphics.DrawPdfTemplate(this._printTitleTemplate, new PointF(0.0f, 0.0f));
                this._printTitleTemplate = pdfTemplate;
                this._pdfGraphics = graphics = this._printTitleTemplate.Graphics;
              }
              else
              {
                PdfTemplate pdfTemplate = new PdfTemplate(adjacentRect.Right, this._pdfPageTemplate.Height);
                pdfTemplate.Graphics.DrawPdfTemplate(this._pdfPageTemplate, new PointF(0.0f, 0.0f));
                this._pdfPageTemplate = pdfTemplate;
                this._pdfGraphics = graphics = this._pdfPageTemplate.Graphics;
                this._pdfTemplateCollection.RemoveAt(this._pdfTemplateCollection.Count - 1);
                this._pdfTemplateCollection.Insert(this._pdfTemplateCollection.Count, this._pdfPageTemplate);
              }
            }
          }
          else if ((double) this._pageSetupOption.TitleColumnWidth < (double) this._pdfPageTemplate.Width && ((double) width1 > (double) this._pdfPageTemplate.Width - (double) adjacentRect.X || flag5))
          {
            SplitText splitText3 = new SplitText();
            splitText3.Text = cellText;
            splitText3.TextFont = pdfFont;
            if (hasRichText)
            {
              RangeRichTextString richText = cell.RichText as RangeRichTextString;
              List<IFont> richTextFonts = new List<IFont>();
              splitText3.RichText = this._workBookImpl.GetDrawString(cell.Text, (RichTextString) richText, out richTextFonts, font1);
              splitText3.RichTextFont = richTextFonts;
            }
            if (flag3)
            {
              splitText3.RichText = new List<string>((IEnumerable<string>) splitText1.RichText);
              splitText3.RichTextFont = new List<IFont>((IEnumerable<IFont>) splitText1.RichTextFont);
            }
            splitText3.Sheet = cell.Worksheet;
            splitText3.Format = this._pdfTextformat;
            splitText3.Brush = pdfBrush;
            splitText3.OriginRect = this._pdfTextformat.Alignment != PdfTextAlignment.Center ? new RectangleF(adjacentRect.X - this._pdfPageTemplate.Width, adjacentRect.Y, adjacentRect.Width, adjacentRect.Height) : new RectangleF((float) ((double) adjacentRect.X - (double) this._pdfPageTemplate.Width + (double) this._leftWidth + ((double) cellRect.Width - (double) width1) / 2.0), adjacentRect.Y, width1, adjacentRect.Height);
            splitText3.Row = cell.Row;
            splitText3.AdjacentColumn = cell.Column + 1;
            splitText3.PageTemplate = this._pdfPageTemplate;
            if (cellText != string.Empty)
              this._splitTextCollection.Add(splitText3);
          }
        }
        SizeF sizeF;
        if (cellText != string.Empty)
        {
          sizeF = pdfFont.MeasureString(cellText[cellText.Length - 1].ToString(), this._pdfTextformat);
          if ((double) sizeF.Width > (double) adjacentRect.Width)
            cellText = string.Empty;
        }
        if (cellText != string.Empty && !cell.WrapText && result != null && (double) result.LineHeight > (double) adjacentRect.Height)
        {
          ref RectangleF local = ref adjacentRect;
          sizeF = pdfFont.MeasureString(cellText[0].ToString(), this._pdfTextformat);
          double height = (double) sizeF.Height;
          local.Height = (float) height;
        }
        if (this._pdfTextformat.Alignment == PdfTextAlignment.Left)
        {
          float num8 = borders[ExcelBordersIndex.EdgeLeft].LineStyle == ExcelLineStyle.Double ? 2f : 1f;
          float num9 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]) * 0.5f;
          float num10 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]) * 0.5f;
          bool flag9 = borders[ExcelBordersIndex.EdgeLeft].LineStyle != ExcelLineStyle.None || borders[ExcelBordersIndex.EdgeRight].LineStyle != ExcelLineStyle.None;
          adjacentRect.X += flag9 ? num9 + num8 : num9;
          adjacentRect.Width -= flag9 ? num9 + num10 + num8 : num9 + num10;
        }
        else if (this._pdfTextformat.Alignment == PdfTextAlignment.Right)
        {
          if (!hasRichText || !flag1)
            adjacentRect.X -= this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]);
          else
            adjacentRect.X = adjacentRect.X + (adjacentRect.Width - width1) - this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]);
        }
        else if (this._pdfTextformat.Alignment == PdfTextAlignment.Center && cell.HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection && !cell.IsBlank)
        {
          double num11 = (double) this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(cell.Column), PdfGraphicsUnit.Point);
          int lastColumn = this._workSheet.UsedRange.LastColumn;
          for (int index = cell.Column + 1; index <= lastColumn; ++index)
          {
            IRange range = this._workSheet[cell.Row, index];
            if (range.HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection && range.IsBlank)
              num11 += (double) this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(index), PdfGraphicsUnit.Point);
            else
              break;
          }
          if (num11 >= (double) width1 || this._pdfTextformat.WordWrap == PdfWordWrapType.Word)
            adjacentRect.Width = (float) num11;
        }
        else if (this._pdfTextformat.Alignment == PdfTextAlignment.Justify)
        {
          float borderWidth1 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]);
          float borderWidth2 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]);
          adjacentRect.X += borderWidth1;
          adjacentRect.Width -= borderWidth1 + borderWidth2;
        }
        if (this._pdfTextformat.LineAlignment == PdfVerticalAlignment.Top)
        {
          float borderWidth = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]);
          adjacentRect.Y += borderWidth;
          if ((double) num5 > (double) adjacentRect.Height)
            adjacentRect.Height -= borderWidth;
        }
        else if (this._pdfTextformat.LineAlignment == PdfVerticalAlignment.Bottom && (double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]) > (double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]))
        {
          adjacentRect.Y -= this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]);
          IRange adjacentRange = this.GetAdjacentRange(cell);
          if ((double) this.GetBorderWidth(adjacentRange.Borders[ExcelBordersIndex.EdgeBottom]) > (double) this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]))
            adjacentRect.Y += this.GetBorderWidth(adjacentRange.Borders[ExcelBordersIndex.EdgeBottom]);
          else
            adjacentRect.Y += this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]);
        }
        if (!hasRichText && !flag3 && (!flag7 || flag7 && cell.HasNumber))
        {
          if (this._pdfTextformat.RightToLeft)
          {
            bool flag10 = false;
            if (this._pdfTextformat.Alignment == PdfTextAlignment.Right)
            {
              this._pdfTextformat.Alignment = PdfTextAlignment.Left;
              flag10 = true;
            }
            if (flag10)
            {
              if ((double) width1 < (double) adjacentRect.Width)
                adjacentRect.X += adjacentRect.Width - width1;
              else if ((double) adjacentRect.X > 0.0 && this._pdfTextformat.WordWrap == PdfWordWrapType.None)
                adjacentRect.X += width1 - adjacentRect.Width;
              this.DrawIconSet(graphics, extendedFormatImpl, ref adjacentRect, pdfFont, cellText);
              this.DrawString(cellText, pdfFont, pdfBrush, adjacentRect, this._pdfTextformat, graphics, pdfMeasureSize, result);
            }
            else
            {
              this.DrawIconSet(graphics, extendedFormatImpl, ref adjacentRect, pdfFont, cellText);
              this.DrawString(cellText, pdfFont, pdfBrush, adjacentRect, this._pdfTextformat, graphics, pdfMeasureSize, result);
            }
          }
          else if (cellText.Length > 0)
          {
            if (cell.WrapText)
            {
              if (cellText[cellText.Length - 1] == '\n')
                cellText += " ";
            }
          }
          try
          {
            if (cell.CellStyle.Font.Superscript)
              this._pdfTextformat.SubSuperScript = PdfSubSuperScript.SuperScript;
            else if (cell.CellStyle.Font.Subscript)
              this._pdfTextformat.SubSuperScript = PdfSubSuperScript.SubScript;
            if (!this._pdfTextformat.RightToLeft)
            {
              this.DrawIconSet(graphics, extendedFormatImpl, ref adjacentRect, pdfFont, cellText);
              this.DrawString(cellText, pdfFont, pdfBrush, adjacentRect, this._pdfTextformat, graphics, pdfMeasureSize, result);
            }
          }
          catch (Exception ex)
          {
            this.DrawIconSet(graphics, extendedFormatImpl, ref adjacentRect, pdfFont, cellText);
            this.DrawString(cellText, pdfFont, pdfBrush, adjacentRect, this._pdfTextformat, graphics, pdfMeasureSize, result);
          }
          if (nativeFont != null)
            ;
          if (pdfFont != null)
            pdfFont = (PdfFont) null;
        }
        else if (hasRichText || flag7)
        {
          cellText = PdfGraphics.NormalizeText(pdfFont, cell.Text);
          RangeRichTextString richText = cell.RichText as RangeRichTextString;
          List<IFont> richTextFonts = new List<IFont>();
          List<string> drawString = this._workBookImpl.GetDrawString(cell.Text, (RichTextString) richText, out richTextFonts, font1);
          string str2;
          if (flag7)
          {
            for (int index1 = 0; index1 < drawString.Count; index1 = index1 + str2.Length + 1)
            {
              str2 = drawString[index1];
              IFont font2 = richTextFonts[index1];
              drawString.RemoveAt(index1);
              richTextFonts.RemoveAt(index1);
              for (int index2 = 0; index2 < str2.Length; ++index2)
              {
                drawString.Insert(index1 + index2, str2[index2].ToString());
                richTextFonts.Insert(index1 + index2, font2);
              }
            }
          }
          this.DrawRTFText(cellRect, adjacentRect, graphics, richTextFonts, drawString, false, true, false, false, false);
        }
        else if (flag3)
          this.DrawRTFText(cellRect, adjacentRect, graphics, splitText1.RichTextFont, splitText1.RichText, false, true, false, false, false);
      }
      if (pdfFont != null)
        ;
    }
    this._leftWidth = 0.0f;
    this._rightWidth = 0.0f;
    if (isHasStyle && this.CheckCellBorderStyle(borders) || this.CheckCellStyles(cell))
    {
      if (this._workSheet.PageSetup.BlackAndWhite)
        borders.Color = ExcelKnownColors.Black;
      this.DrawBordersAsMSExcel(borders, cellRect, graphics, cell, isConditonalFormat, lastCell);
    }
    font1.FontName = fontName2;
    if (cell != null)
      cell = (IRange) null;
    if (this._currentCell != null)
      this._currentCell = (IRange) null;
    if (extendedFormatImpl != null)
      extendedFormatImpl = (ExtendedFormatImpl) null;
    if (graphics == null)
      return;
    graphics.Flush();
    graphics = (PdfGraphics) null;
  }

  internal PdfFont GetPdfFont(Font nativeFont, bool isEmbedFont, Stream alternateFontStream)
  {
    PdfFont pdfFont;
    if (PdfDocument.EnableCache)
      pdfFont = (PdfFont) this.GetPdfTrueTypeFont(nativeFont, isEmbedFont, (Stream) null);
    else if (isEmbedFont && alternateFontStream != null && alternateFontStream.Length > 0L)
    {
      string key = nativeFont.OriginalFontName + (object) nativeFont.Size;
      if (nativeFont.Bold)
        key += "b";
      if (nativeFont.Strikeout)
        key += "s";
      if (nativeFont.Italic)
        key += "i";
      if (nativeFont.Underline)
        key += "u";
      if (!this._alternateFontCollection.ContainsKey(key))
      {
        pdfFont = (PdfFont) this.GetPdfTrueTypeFont(nativeFont, isEmbedFont, alternateFontStream);
        this._alternateFontCollection.Add(key, pdfFont);
      }
      else
        pdfFont = this._alternateFontCollection[key];
    }
    else if (isEmbedFont)
    {
      if (!this._fontUnicodeCollection.ContainsKey(nativeFont))
      {
        pdfFont = (PdfFont) this.GetPdfTrueTypeFont(nativeFont, true, (Stream) null);
        this._fontUnicodeCollection.Add(nativeFont, pdfFont);
      }
      else
        pdfFont = this._fontUnicodeCollection[nativeFont];
    }
    else if (!this._fontCollection.ContainsKey(nativeFont))
    {
      pdfFont = (PdfFont) this.GetPdfTrueTypeFont(nativeFont, false, (Stream) null);
      this._fontCollection.Add(nativeFont, pdfFont);
    }
    else
      pdfFont = this._fontCollection[nativeFont];
    return pdfFont;
  }

  private void DrawIconSet(
    PdfGraphics graphics,
    ExtendedFormatImpl extendedFormatImpl,
    ref RectangleF adjacentRect,
    PdfFont font,
    string cellText)
  {
    if (!(extendedFormatImpl is ExtendedFormatStandAlone formatStandAlone) || formatStandAlone.AdvancedCFIcon == null)
      return;
    PdfImage pdfImage = this.GetPdfImage(this.GetDrawingImage(formatStandAlone.AdvancedCFIcon));
    int num1 = pdfImage.Width;
    int num2 = pdfImage.Height;
    if (font != null)
    {
      double num3 = (double) font.Height / (double) pdfImage.Height;
      num1 = (int) ((double) pdfImage.Width * num3);
      num2 = (int) ((double) pdfImage.Height * num3);
    }
    float num4 = 0.5f;
    IBorders borders = extendedFormatImpl.Borders;
    float x = adjacentRect.X;
    float y = adjacentRect.Y;
    float width = (float) num1;
    float height = (float) num2;
    if (this._pdfTextformat.Alignment == PdfTextAlignment.Left)
    {
      float num5 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]) * num4;
      x -= num5;
    }
    else if (this._pdfTextformat.Alignment == PdfTextAlignment.Center)
    {
      x += num4;
      float borderWidth = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]);
      adjacentRect.X += borderWidth - num4;
    }
    else if (this._pdfTextformat.Alignment == PdfTextAlignment.Right)
      x = x + this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]) + (this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]) * num4 - num4);
    if (this._pdfTextformat.LineAlignment == PdfVerticalAlignment.Top)
    {
      y = y - this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]) + this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]) * num4;
      height -= num4;
    }
    else if (this._pdfTextformat.LineAlignment == PdfVerticalAlignment.Middle)
    {
      float num6 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]) * num4;
      y += num6;
      height -= num4;
    }
    else if (this._pdfTextformat.LineAlignment == PdfVerticalAlignment.Bottom)
    {
      float num7 = this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]) * num4;
      y += num7;
      height -= num4;
    }
    if (this._workSheet != null && !this._workSheet.PageSetup.BlackAndWhite)
      graphics.DrawImage(pdfImage, new RectangleF(x, y, width, height));
    if (this._pdfTextformat.Alignment != PdfTextAlignment.Left && this._pdfTextformat.Alignment != PdfTextAlignment.Justify && (this._currentCell == null || !this._currentCell.CellStyle.ShrinkToFit))
      return;
    adjacentRect.X += (float) num1;
    if (cellText.Contains("$"))
      return;
    adjacentRect.Width -= (float) num1;
  }

  private PdfTextAlignment GetRTLAlignment(ushort[] characterCodes)
  {
    if (characterCodes == null)
      throw new ArgumentNullException(nameof (characterCodes));
    return characterCodes[0] == (ushort) 2 || characterCodes[0] == (ushort) 6 ? PdfTextAlignment.Right : PdfTextAlignment.Left;
  }

  private string FillText(RectangleF bounds, string text, PdfFont pdfFont)
  {
    StringBuilder stringBuilder = new StringBuilder(text);
    float width = pdfFont.MeasureString(text, this._pdfTextformat).Width;
    for (float num = width; (double) bounds.Width > (double) width + (double) num; width += num)
      stringBuilder.Append(text);
    return stringBuilder.ToString();
  }

  private void DrawRTFText(
    RectangleF cellRect,
    RectangleF adjacentRect,
    PdfGraphics graphics,
    List<IFont> richTextFont,
    List<string> drawString,
    bool isShape,
    bool isWrapText,
    bool isHorizontalTextOverflow,
    bool isVerticalTextOverflow,
    bool isHeaderFooter)
  {
    if (this._workSheet != null && this._workSheet.PageSetup.BlackAndWhite)
    {
      foreach (IFont font in richTextFont)
        font.Color = ExcelKnownColors.Black;
    }
    PDFRenderer pdfRenderer = new PDFRenderer(this._currentCell, graphics, this, this._pdfTextformat, richTextFont, drawString, this._workBookImpl);
    pdfRenderer.DrawRTFText(cellRect, adjacentRect, isShape, isWrapText, isHorizontalTextOverflow, isVerticalTextOverflow, false, isHeaderFooter);
    this._isHfrtfProcess = pdfRenderer.IsHfRtfProcess;
  }

  private bool CheckCellStyles(IRange cell)
  {
    ExtendedFormatImpl extendedFormatImpl;
    return this._sortedListCf.TryGetValue(((long) cell.Row << 32 /*0x20*/) + (long) cell.Column, out extendedFormatImpl) && this.CheckCellBorderStyle(extendedFormatImpl.Borders);
  }

  private bool CheckCellBorderStyle(IBorders borders)
  {
    if (borders[ExcelBordersIndex.DiagonalDown].LineStyle != ExcelLineStyle.None)
    {
      if (borders != null)
        borders = (IBorders) null;
      return true;
    }
    if (borders[ExcelBordersIndex.DiagonalUp].LineStyle != ExcelLineStyle.None)
    {
      if (borders != null)
        borders = (IBorders) null;
      return true;
    }
    if (borders[ExcelBordersIndex.EdgeBottom].LineStyle != ExcelLineStyle.None)
    {
      if (borders != null)
        borders = (IBorders) null;
      return true;
    }
    if (borders[ExcelBordersIndex.EdgeLeft].LineStyle != ExcelLineStyle.None)
    {
      if (borders != null)
        borders = (IBorders) null;
      return true;
    }
    if (borders[ExcelBordersIndex.EdgeRight].LineStyle != ExcelLineStyle.None)
    {
      if (borders != null)
        borders = (IBorders) null;
      return true;
    }
    if (borders[ExcelBordersIndex.EdgeTop].LineStyle != ExcelLineStyle.None)
    {
      if (borders != null)
        borders = (IBorders) null;
      return true;
    }
    if (borders != null)
      borders = (IBorders) null;
    return false;
  }

  private void DrawString(
    string text,
    PdfFont pdfFont,
    PdfBrush pdfBrush,
    RectangleF rect,
    PdfStringFormat format,
    PdfGraphics graphics,
    SizeF pdfMeasureSize,
    PdfStringLayoutResult result)
  {
    if (string.IsNullOrEmpty(text))
      return;
    if (pdfMeasureSize == SizeF.Empty || result == null)
      pdfMeasureSize = pdfFont.MeasureString(text, new SizeF(0.0f, 0.0f), format, out result);
    float width = pdfMeasureSize.Width;
    if (this._currentCell.CellStyle.ShrinkToFit && this._pdfTextformat.WordWrap == PdfWordWrapType.None && (double) width > (double) rect.Width)
    {
      PdfTemplate template = new PdfTemplate(width, pdfMeasureSize.Height);
      template.Graphics.DrawString(text, pdfFont, pdfBrush, template.GetBounds(), this._pdfTextformat);
      float height = template.Height / (width / rect.Width);
      switch (this._pdfTextformat.LineAlignment)
      {
        case PdfVerticalAlignment.Top:
          graphics.DrawPdfTemplate(template, rect.Location, new SizeF(rect.Size.Width, height));
          break;
        case PdfVerticalAlignment.Middle:
          graphics.DrawPdfTemplate(template, new PointF(rect.X, rect.Y + (float) (((double) rect.Height - (double) height) / 2.0)), new SizeF(rect.Size.Width, height));
          break;
        case PdfVerticalAlignment.Bottom:
          graphics.DrawPdfTemplate(template, new PointF(rect.X, rect.Y + (rect.Height - height)), new SizeF(rect.Size.Width, height));
          break;
      }
    }
    else
    {
      if (format.Alignment == PdfTextAlignment.Center && format.WordWrap != PdfWordWrapType.Word && ((double) this._leftWidth != 0.0 || (double) this._rightWidth != 0.0))
      {
        RectangleF rectangleF = new RectangleF(rect.X + this._leftWidth, rect.Y, rect.Width - (this._leftWidth + this._rightWidth), rect.Height);
        PdfTemplate template = new PdfTemplate(rect.Width, rect.Height);
        RectangleF layoutRectangle = new RectangleF(this._leftWidth + (float) (((double) rectangleF.Width - (double) width) / 2.0), 0.0f, width, rect.Height);
        template.Graphics.DrawString(text, pdfFont, pdfBrush, layoutRectangle, format, this._maxRowFontSize, this._maxPdfFont, this._maxRowPdfTextformat);
        graphics.DrawPdfTemplate(template, rect.Location, template.Size);
      }
      else if (format.WordWrap == PdfWordWrapType.Word || (double) rect.Width >= (double) width)
      {
        float lineHeight = result.LineHeight;
        float num1 = format.WordWrap != PdfWordWrapType.Word ? lineHeight : pdfFont.MeasureString(text, rect.Width, this._pdfTextformat).Height;
        if ((double) rect.Height < (double) num1)
        {
          float scaledHeight = (float) this._workBookImpl.GetScaledHeight(this._currentCell.CellStyle.Font.FontName, this._currentCell.CellStyle.Font.Size, this._currentCell.Worksheet);
          float num2 = rect.Height / this._scaledCellHeight;
          RowStorage rowStorage = (this._currentCell as RangeImpl).RowStorage;
          if (rowStorage != null && !rowStorage.IsBadFontHeight)
          {
            if (rowStorage.IsSpaceAboveRow)
              num2 -= 0.5f;
            if (rowStorage.IsSpaceBelowRow)
              num2 -= 0.5f;
          }
          bool flag = false;
          float num3 = 0.0f;
          Dictionary<double, float> dictionary = (Dictionary<double, float>) null;
          float num4 = lineHeight;
          float num5 = num2;
          if (ApplicationImpl.FontsHeight.TryGetValue(this._currentCell.CellStyle.Font.FontName, out dictionary) && dictionary.TryGetValue(this._currentCell.CellStyle.Font.Size, out num3))
          {
            num4 = num3;
            flag = true;
          }
          float num6 = num2 * scaledHeight;
          float y = 0.0f;
          if ((double) num6 < (double) lineHeight)
          {
            switch (this._pdfTextformat.LineAlignment)
            {
              case PdfVerticalAlignment.Middle:
                y = (float) (((double) num6 - (double) lineHeight) / 2.0);
                break;
              case PdfVerticalAlignment.Bottom:
                y = num6 - lineHeight;
                break;
            }
            PdfTemplate template = new PdfTemplate(rect.Width, rect.Height);
            template.Graphics.DrawString(text, pdfFont, pdfBrush, new RectangleF(0.0f, y, rect.Width, lineHeight), this._pdfTextformat);
            graphics.DrawPdfTemplate(template, new PointF(rect.X, rect.Y), new SizeF(template.Width, template.Height));
          }
          else if (flag)
          {
            if (Math.Floor((double) num5 * 100.0) / 100.0 % (Math.Floor((double) num4 * 100.0) / 100.0) >= 0.01)
            {
              float num7 = lineHeight * (float) ((int) ((double) num5 / (double) num4) + 1);
              switch (this._pdfTextformat.LineAlignment)
              {
                case PdfVerticalAlignment.Middle:
                  y = (float) (((double) rect.Height - (double) num7) / 2.0);
                  break;
                case PdfVerticalAlignment.Bottom:
                  y = rect.Height - num7;
                  break;
              }
              PdfTemplate template = new PdfTemplate(rect.Width, rect.Height);
              template.Graphics.DrawString(text, pdfFont, pdfBrush, new RectangleF(0.0f, y, rect.Width, (double) y != 0.0 ? num1 : rect.Height), this._pdfTextformat);
              graphics.DrawPdfTemplate(template, new PointF(rect.X, rect.Y), new SizeF(template.Width, template.Height));
            }
            else
              graphics.DrawString(text, pdfFont, pdfBrush, rect, format, this._maxRowFontSize, this._maxPdfFont ?? pdfFont, this._maxRowPdfTextformat ?? format);
          }
          else if (Math.Floor((double) num6 * 100.0) / 100.0 % (Math.Floor((double) lineHeight * 100.0) / 100.0) >= 0.01)
          {
            float num8 = lineHeight * (float) ((int) ((double) num6 / (double) lineHeight) + 1);
            switch (this._pdfTextformat.LineAlignment)
            {
              case PdfVerticalAlignment.Middle:
                y = (float) (((double) rect.Height - (double) num8) / 2.0);
                break;
              case PdfVerticalAlignment.Bottom:
                y = rect.Height - num8;
                break;
            }
            PdfTemplate template = new PdfTemplate(rect.Width, rect.Height);
            template.Graphics.DrawString(text, pdfFont, pdfBrush, new RectangleF(0.0f, y, rect.Width, (double) y != 0.0 ? num1 : rect.Height), this._pdfTextformat);
            graphics.DrawPdfTemplate(template, new PointF(rect.X, rect.Y), new SizeF(template.Width, template.Height));
          }
          else
            graphics.DrawString(text, pdfFont, pdfBrush, rect, format, this._maxRowFontSize, this._maxPdfFont ?? pdfFont, this._maxRowPdfTextformat ?? format);
        }
        else
        {
          if (format.WordWrap != PdfWordWrapType.Word)
            graphics.LayoutResult = result;
          graphics.DrawString(text, pdfFont, pdfBrush, rect, format, this._maxRowFontSize, this._maxPdfFont ?? pdfFont, this._maxRowPdfTextformat ?? format);
          graphics.LayoutResult = (PdfStringLayoutResult) null;
        }
      }
      else
      {
        RectangleF layoutRectangle = RectangleF.Empty;
        switch (format.Alignment)
        {
          case PdfTextAlignment.Left:
            if ((double) this._currentCellRect.X < (double) rect.X)
              rect.Width += this._currentCellRect.X - rect.X;
            layoutRectangle = new RectangleF(rect.X, rect.Y, width, rect.Height);
            break;
          case PdfTextAlignment.Center:
            layoutRectangle = new RectangleF(rect.X + (float) (((double) rect.Width - (double) width) / 2.0), rect.Y, width, rect.Height);
            break;
          case PdfTextAlignment.Right:
            rect.Width += rect.X - this._currentCellRect.X;
            rect.X = this._currentCellRect.X;
            layoutRectangle = new RectangleF(rect.X + (rect.Width - width), rect.Y, width, rect.Height);
            break;
        }
        PdfGraphicsState state = graphics.Save();
        graphics.SetClip(rect);
        graphics.LayoutResult = result;
        graphics.DrawString(text, pdfFont, pdfBrush, layoutRectangle, format, this._maxRowFontSize, this._maxPdfFont ?? pdfFont, this._maxRowPdfTextformat ?? format);
        graphics.LayoutResult = (PdfStringLayoutResult) null;
        graphics.Restore(state);
      }
      this._leftWidth = 0.0f;
      this._rightWidth = 0.0f;
      if (pdfBrush != null)
        pdfBrush = (PdfBrush) null;
      if (graphics != null)
      {
        graphics.Flush();
        graphics = (PdfGraphics) null;
      }
      if (pdfFont != null)
        pdfFont = (PdfFont) null;
      if (format == null)
        return;
      format = (PdfStringFormat) null;
    }
  }

  private void DrawRotatedText(
    RectangleF adjacentRect,
    ExtendedFormatImpl extendedFormatImpl,
    IRange cell,
    string value,
    PdfGraphics graphics,
    PdfFont nativeFont,
    PdfBrush brush,
    PdfStringFormat format,
    SizeF firstCharWidth,
    float totalWidth,
    SizeF lastCharSize)
  {
    this._cellDisplayText = value;
    IWorksheet worksheet = cell.Worksheet;
    int clockwiseRotation = this.GetCounterClockwiseRotation(extendedFormatImpl.Rotation);
    PdfGraphicsState state = graphics.Save();
    PointF vector = this.GetVector(clockwiseRotation, totalWidth, adjacentRect, extendedFormatImpl, lastCharSize, firstCharWidth, nativeFont, format);
    if (!value.Equals(this._cellDisplayText) && cell.WrapText || clockwiseRotation == 90 || clockwiseRotation == -90)
    {
      if (clockwiseRotation <= 0)
      {
        graphics.TranslateTransform(adjacentRect.X, adjacentRect.Bottom);
        if (cell.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
          format.Alignment = PdfTextAlignment.Right;
      }
      else
      {
        graphics.TranslateTransform(adjacentRect.Right, adjacentRect.Top);
        if (cell.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
          format.Alignment = PdfTextAlignment.Left;
      }
      format = this.UpdatePdfAlignment(format, clockwiseRotation, (ShapeImpl) null);
      format.WordWrap = PdfWordWrapType.Word;
      graphics.RotateTransform((float) clockwiseRotation);
      graphics.DrawString(value, nativeFont, brush, new RectangleF(0.0f, 0.0f, adjacentRect.Height, adjacentRect.Width), format);
    }
    else
    {
      graphics.TranslateTransform(vector.X, vector.Y);
      graphics.RotateTransform((float) clockwiseRotation);
      graphics.DrawString(this._cellDisplayText, nativeFont, brush, PointF.Empty);
    }
    graphics.Restore(state);
  }

  private PdfStringFormat UpdatePdfAlignment(
    PdfStringFormat format,
    int rotationAngle,
    ShapeImpl shape)
  {
    PdfTextAlignment pdfTextAlignment = PdfTextAlignment.Justify;
    PdfVerticalAlignment verticalAlignment = PdfVerticalAlignment.Bottom;
    TextBoxShapeImpl textBoxShapeImpl = shape as TextBoxShapeImpl;
    if (shape == null)
    {
      if (rotationAngle <= 0)
      {
        switch (format.Alignment)
        {
          case PdfTextAlignment.Left:
            verticalAlignment = PdfVerticalAlignment.Top;
            break;
          case PdfTextAlignment.Center:
            verticalAlignment = PdfVerticalAlignment.Middle;
            break;
          case PdfTextAlignment.Right:
            verticalAlignment = PdfVerticalAlignment.Bottom;
            break;
        }
        switch (format.LineAlignment)
        {
          case PdfVerticalAlignment.Top:
            pdfTextAlignment = PdfTextAlignment.Right;
            break;
          case PdfVerticalAlignment.Middle:
            pdfTextAlignment = PdfTextAlignment.Center;
            break;
          case PdfVerticalAlignment.Bottom:
            pdfTextAlignment = PdfTextAlignment.Left;
            break;
        }
      }
      else
      {
        switch (format.Alignment)
        {
          case PdfTextAlignment.Left:
            verticalAlignment = PdfVerticalAlignment.Bottom;
            break;
          case PdfTextAlignment.Center:
            verticalAlignment = PdfVerticalAlignment.Middle;
            break;
          case PdfTextAlignment.Right:
            verticalAlignment = PdfVerticalAlignment.Top;
            break;
        }
        switch (format.LineAlignment)
        {
          case PdfVerticalAlignment.Top:
            pdfTextAlignment = PdfTextAlignment.Left;
            break;
          case PdfVerticalAlignment.Middle:
            pdfTextAlignment = PdfTextAlignment.Center;
            break;
          case PdfVerticalAlignment.Bottom:
            pdfTextAlignment = PdfTextAlignment.Right;
            break;
        }
      }
    }
    else if (shape is AutoShapeImpl || textBoxShapeImpl != null && textBoxShapeImpl.IsCreated)
    {
      if (rotationAngle == 270)
      {
        switch (format.Alignment)
        {
          case PdfTextAlignment.Left:
            verticalAlignment = PdfVerticalAlignment.Top;
            break;
          case PdfTextAlignment.Center:
            verticalAlignment = PdfVerticalAlignment.Middle;
            break;
          case PdfTextAlignment.Right:
            verticalAlignment = PdfVerticalAlignment.Bottom;
            break;
        }
        switch (format.LineAlignment)
        {
          case PdfVerticalAlignment.Top:
            pdfTextAlignment = PdfTextAlignment.Left;
            break;
          case PdfVerticalAlignment.Middle:
            pdfTextAlignment = PdfTextAlignment.Center;
            break;
          case PdfVerticalAlignment.Bottom:
            pdfTextAlignment = PdfTextAlignment.Right;
            break;
        }
      }
      else
      {
        switch (format.Alignment)
        {
          case PdfTextAlignment.Left:
            verticalAlignment = PdfVerticalAlignment.Bottom;
            break;
          case PdfTextAlignment.Center:
            verticalAlignment = PdfVerticalAlignment.Middle;
            break;
          case PdfTextAlignment.Right:
            verticalAlignment = PdfVerticalAlignment.Top;
            break;
        }
        switch (format.LineAlignment)
        {
          case PdfVerticalAlignment.Top:
            pdfTextAlignment = PdfTextAlignment.Right;
            break;
          case PdfVerticalAlignment.Middle:
            pdfTextAlignment = PdfTextAlignment.Center;
            break;
          case PdfVerticalAlignment.Bottom:
            pdfTextAlignment = PdfTextAlignment.Left;
            break;
        }
      }
    }
    else if (textBoxShapeImpl != null)
    {
      pdfTextAlignment = format.Alignment;
      verticalAlignment = format.LineAlignment;
    }
    format.Alignment = pdfTextAlignment;
    format.LineAlignment = verticalAlignment;
    return format;
  }

  private void DrawBorders(IBorders borders, RectangleF rect, PdfGraphics graphics, IRange cell)
  {
    Color empty1 = Color.Empty;
    Color empty2 = Color.Empty;
    Color empty3 = Color.Empty;
    Color empty4 = Color.Empty;
    if (this._workSheet.ListObjects.Count != 0 && this._tableBorderColorList != null && this._tableBorderColorList.Count != 0)
    {
      long cellIndex = RangeImpl.GetCellIndex(cell.Column, cell.Row);
      Dictionary<ExcelBordersIndex, Color> dictionary = (Dictionary<ExcelBordersIndex, Color>) null;
      if (this._tableBorderColorList.TryGetValue(cellIndex, out dictionary))
      {
        if (empty1.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeLeft, out empty1);
        if (empty2.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeRight, out empty2);
        if (empty3.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeTop, out empty3);
        if (empty4.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeBottom, out empty4);
        this._tableBorderColorList.Remove(cellIndex);
      }
    }
    IBorder border1 = borders[ExcelBordersIndex.EdgeLeft];
    ExcelBordersIndex index1 = ExcelBordersIndex.EdgeLeft;
    if (this._workSheet.IsRightToLeft)
    {
      border1 = borders[ExcelBordersIndex.EdgeRight];
      index1 = ExcelBordersIndex.EdgeRight;
    }
    this.DrawBorder(borders, border1, rect.Left, rect.Top, rect.Left, rect.Bottom, graphics, cell, empty1, index1);
    IBorder border2 = borders[ExcelBordersIndex.EdgeTop];
    ExcelBordersIndex index2 = ExcelBordersIndex.EdgeTop;
    this.DrawBorder(borders, border2, rect.Left, rect.Top, rect.Right, rect.Top, graphics, cell, empty3, index2);
    IBorder border3 = borders[ExcelBordersIndex.EdgeRight];
    ExcelBordersIndex index3 = ExcelBordersIndex.EdgeRight;
    if (this._workSheet.IsRightToLeft)
    {
      border3 = borders[ExcelBordersIndex.EdgeLeft];
      index3 = ExcelBordersIndex.EdgeLeft;
    }
    this.DrawBorder(borders, border3, rect.Right, rect.Top, rect.Right, rect.Bottom, graphics, cell, empty2, index3);
    IBorder border4 = borders[ExcelBordersIndex.EdgeBottom];
    ExcelBordersIndex index4 = ExcelBordersIndex.EdgeBottom;
    this.DrawBorder(borders, border4, rect.Left, rect.Bottom, rect.Right, rect.Bottom, graphics, cell, empty4, index4);
    IBorder border5 = borders[ExcelBordersIndex.DiagonalDown];
    ExcelBordersIndex index5 = ExcelBordersIndex.DiagonalDown;
    if (border5.ShowDiagonalLine)
      this.DrawBorder(borders, border5, rect.Left, rect.Top, rect.Right, rect.Bottom, graphics, cell, Color.Empty, index5);
    IBorder border6 = borders[ExcelBordersIndex.DiagonalUp];
    ExcelBordersIndex index6 = ExcelBordersIndex.DiagonalUp;
    if (!border6.ShowDiagonalLine)
      return;
    this.DrawBorder(borders, border6, rect.Left, rect.Bottom, rect.Right, rect.Top, graphics, cell, Color.Empty, index6);
  }

  private void DrawBordersAsMSExcel(
    IBorders borders,
    RectangleF rect,
    PdfGraphics graphics,
    IRange cell,
    bool isConditonalFormat,
    int lastCell)
  {
    ExcelLineStyle lineStyle = borders[ExcelBordersIndex.EdgeBottom].LineStyle;
    Color color = Color.Empty;
    Color rightColor = Color.Empty;
    Color topColor = Color.Empty;
    Color bottomColor = Color.Empty;
    long cellIndex = RangeImpl.GetCellIndex(cell.Column, cell.Row);
    if (this._workSheet.ListObjects.Count != 0 && this._tableBorderColorList != null && this._tableBorderColorList.Count != 0)
    {
      Dictionary<ExcelBordersIndex, Color> dictionary = (Dictionary<ExcelBordersIndex, Color>) null;
      if (this._tableBorderColorList.TryGetValue(cellIndex, out dictionary))
      {
        this._tableBorderList.Add(cellIndex, dictionary);
        if (color.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeLeft, out color);
        if (rightColor.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeRight, out rightColor);
        if (topColor.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeTop, out topColor);
        if (bottomColor.IsEmpty)
          dictionary.TryGetValue(ExcelBordersIndex.EdgeBottom, out bottomColor);
        if (this._workSheet.PageSetup.PrintTitleRows == null && this._workSheet.PageSetup.PrintTitleColumns == null)
          this._tableBorderColorList.Remove(cellIndex);
      }
    }
    if (this._workSheet.PageSetup.BlackAndWhite)
    {
      color = Color.Black;
      rightColor = Color.Black;
      topColor = Color.Black;
      bottomColor = Color.Black;
    }
    IBorder border1 = borders[ExcelBordersIndex.EdgeLeft];
    IBorder border2 = borders[ExcelBordersIndex.EdgeRight];
    IBorder border3 = borders[ExcelBordersIndex.EdgeTop];
    IBorder border4 = borders[ExcelBordersIndex.EdgeBottom];
    IBorder border5 = borders[ExcelBordersIndex.DiagonalDown];
    IBorder border6 = borders[ExcelBordersIndex.DiagonalUp];
    if (lastCell == 1 && border4.LineStyle == ExcelLineStyle.None)
      border4 = (cell.MergeArea == null ? this._workSheet.Range[cell.Row + 1, cell.Column] : this._workSheet.Range[cell.MergeArea.LastRow + 1, cell.Column]).Borders[ExcelBordersIndex.EdgeTop];
    else if (lastCell == 2)
      border4.LineStyle = ExcelLineStyle.None;
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    if (this._sortedListCf.ContainsKey(cellIndex))
      this._sortedListCf.TryGetValue(cellIndex, out extendedFormatImpl);
    bool flag1 = this._borderRange != null && this._borderRange.Row == cell.Row && cell.Column > this._borderRange.Column && cell.LastColumn <= this._borderRange.LastColumn;
    bool flag2 = this._borderRange != null && this._borderRange.Row == cell.Row && this._borderRange.Column <= cell.Column && this._borderRange.LastColumn > cell.LastColumn;
    bool flag3 = border3.LineStyle != ExcelLineStyle.Dashed && border3.LineStyle != ExcelLineStyle.Dash_dot && border3.LineStyle != ExcelLineStyle.Dash_dot_dot && border3.LineStyle != ExcelLineStyle.Medium_dash_dot && border3.LineStyle != ExcelLineStyle.Medium_dash_dot_dot && border3.LineStyle != ExcelLineStyle.Slanted_dash_dot;
    if (!flag1 && !flag2 && flag3 && extendedFormatImpl == null && !border5.ShowDiagonalLine && !border6.ShowDiagonalLine && border1.LineStyle == border2.LineStyle && border1.Color == border2.Color && border1.ColorRGB == border2.ColorRGB && border2.LineStyle == border3.LineStyle && border2.Color == border3.Color && border2.ColorRGB == border3.ColorRGB && border3.LineStyle == border4.LineStyle && border3.Color == border4.Color && border3.ColorRGB == border4.ColorRGB && border1.LineStyle != ExcelLineStyle.Double)
    {
      PdfPen pen = this.CreatePen(borders[ExcelBordersIndex.EdgeRight], color);
      graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
    }
    else
    {
      this.SortBorderLineStyle();
      this.FillBorderLineStyle(borders);
      List<ExcelBordersIndex> excelBordersIndexList1 = new List<ExcelBordersIndex>();
      List<ExcelBordersIndex> excelBordersIndexList2 = new List<ExcelBordersIndex>();
      ExcelLineStyle[] array = new ExcelLineStyle[this._dicBorderLineStyle.Count];
      this._dicBorderLineStyle.Keys.CopyTo(array, 0);
      foreach (ExcelLineStyle key1 in array)
      {
        if (this._dicBorderLineStyle[key1].Count > 1)
        {
          this._sortedBorders.Clear();
          excelBordersIndexList1 = this._dicBorderLineStyle[key1];
          foreach (ExcelBordersIndex index in excelBordersIndexList1)
            this.BorderColorContrast(borders, index);
          if (this._sortedBorders != null && this._sortedBorders.Count > 0)
          {
            this.SortDictionaryByValue();
            foreach (ExcelBordersIndex key2 in this._sortedBorders.Keys)
              excelBordersIndexList2.Add(key2);
          }
          this._dicBorderLineStyle[key1].Clear();
          this._dicBorderLineStyle[key1] = excelBordersIndexList2;
        }
      }
      int num = 1;
      ExcelBordersIndex fillEdge = (ExcelBordersIndex) 0;
      foreach (ExcelLineStyle key in this._dicBorderLineStyle.Keys)
      {
        if (this._dicBorderLineStyle[key].Count > 0)
        {
          if (this._dicBorderLineStyle[key].Count == 1)
          {
            bool isFirstBorder = num == 1;
            ExcelBordersIndex excelBordersIndex = this._dicBorderLineStyle[key][0];
            IBorder border7 = borders[excelBordersIndex];
            this.DrawAllBorders(borders, border7, rect, graphics, cell, Color.Empty, excelBordersIndex, color, rightColor, topColor, bottomColor, isFirstBorder, fillEdge, isConditonalFormat);
            fillEdge |= excelBordersIndex;
            ++num;
          }
          else
          {
            foreach (ExcelBordersIndex excelBordersIndex in this._dicBorderLineStyle[key])
            {
              bool isFirstBorder = num == 1;
              IBorder border8 = borders[excelBordersIndex];
              this.DrawAllBorders(borders, border8, rect, graphics, cell, Color.Empty, excelBordersIndex, color, rightColor, topColor, bottomColor, isFirstBorder, fillEdge, isConditonalFormat);
              fillEdge |= excelBordersIndex;
              ++num;
            }
          }
        }
      }
      if (array != null)
        ;
      if (excelBordersIndexList1.Count != 0)
        excelBordersIndexList1.Clear();
      if (excelBordersIndexList2.Count != 0)
        excelBordersIndexList2.Clear();
    }
    if (lineStyle == ExcelLineStyle.None)
      return;
    border4.LineStyle = lineStyle;
  }

  private void SortBorderLineStyle()
  {
    this._dicBorderLineStyle.Clear();
    this._dicBorderLineStyle.Add(ExcelLineStyle.Double, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Hair, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Dotted, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Dash_dot_dot, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Dash_dot, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Dashed, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Thin, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Medium_dash_dot_dot, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Slanted_dash_dot, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Medium_dash_dot, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Medium_dashed, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Medium, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.Thick, new List<ExcelBordersIndex>());
    this._dicBorderLineStyle.Add(ExcelLineStyle.None, new List<ExcelBordersIndex>());
  }

  private void FillBorderLineStyle(IBorders borders)
  {
    List<ExcelBordersIndex> excelBordersIndexList = (List<ExcelBordersIndex>) null;
    List<ExcelLineStyle> excelLineStyleList = new List<ExcelLineStyle>();
    int count = 0;
    ExcelLineStyle lineStyle1 = borders[ExcelBordersIndex.EdgeLeft].LineStyle;
    if (lineStyle1 != ExcelLineStyle.None && this._dicBorderLineStyle.ContainsKey(lineStyle1))
    {
      this._dicBorderLineStyle.TryGetValue(lineStyle1, out excelBordersIndexList);
      excelBordersIndexList.Add(ExcelBordersIndex.EdgeLeft);
      this._dicBorderLineStyle[lineStyle1] = excelBordersIndexList;
      ++count;
      excelLineStyleList.Add(lineStyle1);
    }
    ExcelLineStyle lineStyle2 = borders[ExcelBordersIndex.EdgeTop].LineStyle;
    if (lineStyle2 != ExcelLineStyle.None && this._dicBorderLineStyle.ContainsKey(lineStyle2))
    {
      this._dicBorderLineStyle.TryGetValue(lineStyle2, out excelBordersIndexList);
      excelBordersIndexList.Add(ExcelBordersIndex.EdgeTop);
      this._dicBorderLineStyle[lineStyle2] = excelBordersIndexList;
      ++count;
      if (!excelLineStyleList.Contains(lineStyle2))
        excelLineStyleList.Add(lineStyle2);
    }
    ExcelLineStyle lineStyle3 = borders[ExcelBordersIndex.EdgeRight].LineStyle;
    if (borders[ExcelBordersIndex.EdgeRight].LineStyle != ExcelLineStyle.None && this._dicBorderLineStyle.ContainsKey(borders[ExcelBordersIndex.EdgeRight].LineStyle))
    {
      this._dicBorderLineStyle.TryGetValue(lineStyle3, out excelBordersIndexList);
      excelBordersIndexList.Add(ExcelBordersIndex.EdgeRight);
      this._dicBorderLineStyle[lineStyle3] = excelBordersIndexList;
      ++count;
      if (!excelLineStyleList.Contains(lineStyle3))
        excelLineStyleList.Add(lineStyle3);
    }
    ExcelLineStyle lineStyle4 = borders[ExcelBordersIndex.EdgeBottom].LineStyle;
    if (lineStyle4 != ExcelLineStyle.None && this._dicBorderLineStyle.ContainsKey(lineStyle4))
    {
      this._dicBorderLineStyle.TryGetValue(lineStyle4, out excelBordersIndexList);
      excelBordersIndexList.Add(ExcelBordersIndex.EdgeBottom);
      this._dicBorderLineStyle[lineStyle4] = excelBordersIndexList;
      ++count;
      if (!excelLineStyleList.Contains(lineStyle4))
        excelLineStyleList.Add(lineStyle4);
    }
    ExcelLineStyle lineStyle5 = borders[ExcelBordersIndex.DiagonalDown].LineStyle;
    if (lineStyle5 != ExcelLineStyle.None && this._dicBorderLineStyle.ContainsKey(lineStyle5))
    {
      excelBordersIndexList = new List<ExcelBordersIndex>();
      this._dicBorderLineStyle.TryGetValue(lineStyle5, out excelBordersIndexList);
      excelBordersIndexList.Add(ExcelBordersIndex.DiagonalDown);
      this._dicBorderLineStyle[lineStyle5] = excelBordersIndexList;
      ++count;
      if (!excelLineStyleList.Contains(lineStyle5))
        excelLineStyleList.Add(lineStyle5);
    }
    ExcelLineStyle lineStyle6 = borders[ExcelBordersIndex.DiagonalUp].LineStyle;
    if (lineStyle6 != ExcelLineStyle.None && this._dicBorderLineStyle.ContainsKey(lineStyle6))
    {
      excelBordersIndexList = new List<ExcelBordersIndex>();
      this._dicBorderLineStyle.TryGetValue(lineStyle6, out excelBordersIndexList);
      excelBordersIndexList.Add(ExcelBordersIndex.DiagonalUp);
      this._dicBorderLineStyle[lineStyle6] = excelBordersIndexList;
      ++count;
      if (!excelLineStyleList.Contains(lineStyle6))
        excelLineStyleList.Add(lineStyle6);
    }
    if (excelLineStyleList.Count == 1)
    {
      Dictionary<ExcelLineStyle, List<ExcelBordersIndex>> dictionary = new Dictionary<ExcelLineStyle, List<ExcelBordersIndex>>();
      dictionary.Add(excelLineStyleList[0], this._dicBorderLineStyle[excelLineStyleList[0]]);
      this._dicBorderLineStyle.Clear();
      this._dicBorderLineStyle = dictionary;
    }
    else
      this._dicBorderLineStyle = this.RemoveUnwantedBorderStyle(this._dicBorderLineStyle, count);
  }

  private Dictionary<ExcelLineStyle, List<ExcelBordersIndex>> RemoveUnwantedBorderStyle(
    Dictionary<ExcelLineStyle, List<ExcelBordersIndex>> m_dicBorderLineStyle,
    int count)
  {
    ExcelLineStyle[] array = new ExcelLineStyle[m_dicBorderLineStyle.Count];
    Dictionary<ExcelLineStyle, List<ExcelBordersIndex>> dictionary = new Dictionary<ExcelLineStyle, List<ExcelBordersIndex>>();
    m_dicBorderLineStyle.Keys.CopyTo(array, 0);
    foreach (ExcelLineStyle key in array)
    {
      if (count <= 0)
        return dictionary;
      if (m_dicBorderLineStyle[key].Count > 0)
      {
        dictionary.Add(key, m_dicBorderLineStyle[key]);
        count -= m_dicBorderLineStyle[key].Count;
      }
    }
    return dictionary;
  }

  private void BorderColorContrast(IBorders borders, ExcelBordersIndex index)
  {
    double num = 1.0 - (0.3 * (double) borders[index].ColorRGB.R + 0.59 * (double) borders[index].ColorRGB.G + 0.11 * (double) borders[index].ColorRGB.B) / (double) byte.MaxValue;
    if (this._sortedBorders.ContainsKey(index))
      return;
    this._sortedBorders.Add(index, num);
  }

  private void SortDictionaryByValue()
  {
    List<KeyValuePair<ExcelBordersIndex, double>> keyValuePairList = new List<KeyValuePair<ExcelBordersIndex, double>>((IEnumerable<KeyValuePair<ExcelBordersIndex, double>>) this._sortedBorders);
    keyValuePairList.Sort((Comparison<KeyValuePair<ExcelBordersIndex, double>>) ((firstPair, secondPair) => firstPair.Value.CompareTo(secondPair.Value)));
    this._sortedBorders.Clear();
    foreach (KeyValuePair<ExcelBordersIndex, double> keyValuePair in keyValuePairList)
      this._sortedBorders.Add(keyValuePair.Key, keyValuePair.Value);
    if (keyValuePairList.Count == 0)
      return;
    keyValuePairList.Clear();
  }

  private void DrawAllBorders(
    IBorders borders,
    IBorder border,
    RectangleF rect,
    PdfGraphics graphics,
    IRange cell,
    Color borderColor,
    ExcelBordersIndex borderIndex,
    Color leftColor,
    Color rightColor,
    Color topColor,
    Color bottomColor,
    bool isFirstBorder,
    ExcelBordersIndex fillEdge,
    bool isConditionalFormat)
  {
    if (this._workSheet.IsRightToLeft)
    {
      switch (borderIndex)
      {
        case ExcelBordersIndex.EdgeLeft:
          borderIndex = ExcelBordersIndex.EdgeRight;
          break;
        case ExcelBordersIndex.EdgeRight:
          borderIndex = ExcelBordersIndex.EdgeLeft;
          break;
      }
    }
    switch (borderIndex)
    {
      case ExcelBordersIndex.DiagonalDown:
        if (!border.ShowDiagonalLine)
          break;
        this.DrawSingleBorder(borders, border, rect.Left, rect.Top, rect.Right, rect.Bottom, graphics, cell, Color.Empty, borderIndex, isFirstBorder, fillEdge);
        break;
      case ExcelBordersIndex.DiagonalUp:
        if (!border.ShowDiagonalLine)
          break;
        this.DrawSingleBorder(borders, border, rect.Left, rect.Bottom, rect.Right, rect.Top, graphics, cell, Color.Empty, borderIndex, isFirstBorder, fillEdge);
        break;
      case ExcelBordersIndex.EdgeLeft:
        if (this._borderRange != null && this._borderRange.Row == cell.Row && cell.Column > this._borderRange.Column && cell.LastColumn <= this._borderRange.LastColumn)
          break;
        if (!this._excelToPdfSettings.EnableRTL && !this.isFirstColumnOfPage && !this.isCurrentCellNotMerged && this._workSheet.IsColumnVisible(cell.Column - 1))
        {
          IBorders borders1 = (IBorders) null;
          Color empty = Color.Empty;
          int xfIndex = (this._workSheet as WorksheetImpl).GetXFIndex(cell.Row, cell.Column - 1);
          Dictionary<ExcelBordersIndex, Color> dictionary;
          if (this._tableBorderList.TryGetValue(RangeImpl.GetCellIndex(cell.Column - 1, cell.Row), out dictionary) && empty.IsEmpty)
            dictionary.TryGetValue(ExcelBordersIndex.EdgeRight, out empty);
          if (!this._extBorders.TryGetValue(xfIndex, out borders1))
            borders1 = (this._workSheet.Workbook as WorkbookImpl).InnerExtFormats[xfIndex].Borders;
          IBorder border1 = borders1[ExcelBordersIndex.EdgeRight];
          if (empty.IsEmpty && (border1 != null && border1.LineStyle == border.LineStyle && border1.Color == border.Color && border1.ColorRGB == border.ColorRGB || !isConditionalFormat && border1 != null && ((double) this.GetBorderPriority(border1.LineStyle) > (double) this.GetBorderPriority(border.LineStyle) || border1.LineStyle == border.LineStyle && (this.GetBrightness(border1.ColorRGB) < this.GetBrightness(border.ColorRGB) || this.GetBrightness(border1.ColorRGB) == this.GetBrightness(border.ColorRGB) && border1.Color == border.Color))))
            break;
        }
        if (this._workSheet.IsRightToLeft)
          border = borders[ExcelBordersIndex.EdgeRight];
        this.DrawSingleBorder(borders, border, rect.Left, rect.Top, rect.Left, rect.Bottom, graphics, cell, leftColor, borderIndex, isFirstBorder, fillEdge);
        break;
      case ExcelBordersIndex.EdgeTop:
        if (!this._isFirstRowOfPage && !this.isCurrentCellNotMerged && this._workSheet.IsRowVisible(cell.Row - 1))
        {
          IBorders borders2 = (IBorders) null;
          Color empty = Color.Empty;
          int xfIndex = (this._workSheet as WorksheetImpl).GetXFIndex(cell.Row - 1, cell.Column);
          Dictionary<ExcelBordersIndex, Color> dictionary;
          if (this._tableBorderList.TryGetValue(RangeImpl.GetCellIndex(cell.Column, cell.Row - 1), out dictionary) && empty.IsEmpty)
            dictionary.TryGetValue(ExcelBordersIndex.EdgeBottom, out empty);
          if (!this._extBorders.TryGetValue(xfIndex, out borders2))
            borders2 = (this._workSheet.Workbook as WorkbookImpl).InnerExtFormats[xfIndex].Borders;
          IBorder border2 = borders2[ExcelBordersIndex.EdgeBottom];
          if (empty.IsEmpty && (border2 != null && border2.LineStyle == border.LineStyle && border2.Color == border.Color && border2.ColorRGB == border.ColorRGB || !isConditionalFormat && border2 != null && ((double) this.GetBorderPriority(border2.LineStyle) > (double) this.GetBorderPriority(border.LineStyle) || border2.LineStyle == border.LineStyle && (this.GetBrightness(border2.ColorRGB) <= this.GetBrightness(border.ColorRGB) || this.GetBrightness(border2.ColorRGB) == this.GetBrightness(border.ColorRGB) && border2.Color == border.Color))))
            break;
        }
        this.DrawSingleBorder(borders, border, rect.Left, rect.Top, rect.Right, rect.Top, graphics, cell, topColor, borderIndex, isFirstBorder, fillEdge);
        break;
      case ExcelBordersIndex.EdgeBottom:
        this.DrawSingleBorder(borders, border, rect.Left, rect.Bottom, rect.Right, rect.Bottom, graphics, cell, bottomColor, borderIndex, isFirstBorder, fillEdge);
        break;
      case ExcelBordersIndex.EdgeRight:
        if (this._borderRange != null && this._borderRange.Row == cell.Row && this._borderRange.Column <= cell.Column && this._borderRange.LastColumn > cell.LastColumn)
          break;
        if (this._workSheet.IsRightToLeft)
          border = borders[ExcelBordersIndex.EdgeLeft];
        this.DrawSingleBorder(borders, border, rect.Right, rect.Top, rect.Right, rect.Bottom, graphics, cell, rightColor, borderIndex, isFirstBorder, fillEdge);
        break;
    }
  }

  private void DrawSingleBorder(
    IBorders borders,
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    PdfGraphics graphics,
    IRange cell,
    Color borderColor,
    ExcelBordersIndex index,
    bool isFirstBorder,
    ExcelBordersIndex fillEdge)
  {
    bool canDrawBorder = false;
    bool isTop = index == ExcelBordersIndex.EdgeTop;
    if (index == ExcelBordersIndex.EdgeLeft || index == ExcelBordersIndex.EdgeRight || index == ExcelBordersIndex.DiagonalUp || index == ExcelBordersIndex.DiagonalDown)
      canDrawBorder = true;
    else if (border.LineStyle != ExcelLineStyle.None)
      canDrawBorder = this.CanDrawBorder(borders, border, cell, index, isTop);
    if (cell.HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection)
    {
      switch (index)
      {
        case ExcelBordersIndex.EdgeLeft:
          if (cell.Column > 1 && this._workSheet[cell.Row, cell.Column - 1].HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection && cell.IsBlank)
          {
            canDrawBorder = false;
            break;
          }
          break;
        case ExcelBordersIndex.EdgeRight:
          if (cell.Column < this._workBook.MaxColumnCount)
          {
            IRange range = this._workSheet[cell.Row, cell.Column + 1];
            if (range.HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection && range.IsBlank)
            {
              canDrawBorder = false;
              break;
            }
            break;
          }
          break;
      }
    }
    bool flag1 = this.CheckCFBorder(index, cell, canDrawBorder);
    if (border.LineStyle == ExcelLineStyle.Double && flag1)
    {
      this.DrawDoubleBorder(borders, border, x1, y1, x2, y2, graphics, cell, borderColor);
    }
    else
    {
      if (!flag1)
        return;
      PdfPen pen = this.CreatePen(border, borderColor);
      if ((double) pen.Width != 0.5)
      {
        double num = (double) pen.Width / 2.0;
      }
      if (isFirstBorder)
      {
        graphics.DrawLine(pen, x1, y1, x2, y2);
      }
      else
      {
        switch (index)
        {
          case ExcelBordersIndex.DiagonalDown:
          case ExcelBordersIndex.DiagonalUp:
            graphics.DrawLine(pen, x1, y1, x2, y2);
            break;
          case ExcelBordersIndex.EdgeLeft:
          case ExcelBordersIndex.EdgeRight:
            bool flag2 = (fillEdge & ExcelBordersIndex.EdgeTop) == ExcelBordersIndex.EdgeTop;
            bool flag3 = (fillEdge & ExcelBordersIndex.EdgeBottom) == ExcelBordersIndex.EdgeBottom;
            if (flag2)
              y1 -= this.GetBorderWidth(borders[ExcelBordersIndex.EdgeTop]) / 2f;
            if (flag3)
              y2 += this.GetBorderWidth(borders[ExcelBordersIndex.EdgeBottom]) / 2f;
            graphics.DrawLine(pen, x1, y1, x2, y2);
            break;
          case ExcelBordersIndex.EdgeTop:
          case ExcelBordersIndex.EdgeBottom:
            bool flag4 = (fillEdge & ExcelBordersIndex.EdgeLeft) == ExcelBordersIndex.EdgeLeft;
            bool flag5 = (fillEdge & ExcelBordersIndex.EdgeRight) == ExcelBordersIndex.EdgeRight;
            if (flag4)
              x1 -= this.GetBorderWidth(borders[ExcelBordersIndex.EdgeLeft]) / 2f;
            if (flag5)
              x2 += this.GetBorderWidth(borders[ExcelBordersIndex.EdgeRight]) / 2f;
            graphics.DrawLine(pen, x1, y1, x2, y2);
            break;
        }
      }
    }
  }

  private bool CheckCFBorder(ExcelBordersIndex index, IRange cell, bool canDrawBorder)
  {
    string[] array = new string[15]
    {
      "Thick",
      "Double",
      "Medium",
      "Thin",
      "Medium_dashed",
      "Medium_dot",
      "Medium_dash_dot",
      " Medium_dash_dot_dot",
      "Dashed",
      "Dotted",
      "Dash_dot",
      "Dash_dot_dot",
      "Slanted_dash_dot",
      "Hair",
      "None"
    };
    long key1 = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column;
    ExtendedFormatImpl extendedFormatImpl1 = (ExtendedFormatImpl) null;
    if (this._sortedListCf.ContainsKey(key1))
      this._sortedListCf.TryGetValue(key1, out extendedFormatImpl1);
    if (extendedFormatImpl1 != null)
    {
      switch (index)
      {
        case ExcelBordersIndex.EdgeLeft:
          long key2 = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column - 1L;
          ExtendedFormatImpl extendedFormatImpl2 = (ExtendedFormatImpl) null;
          if (this._sortedListCf.ContainsKey(key2))
            this._sortedListCf.TryGetValue(key2, out extendedFormatImpl2);
          if (extendedFormatImpl2 != null && extendedFormatImpl2.Borders[ExcelBordersIndex.EdgeRight].LineStyle != ExcelLineStyle.None && System.Array.IndexOf<string>(array, extendedFormatImpl1.Borders[ExcelBordersIndex.EdgeLeft].LineStyle.ToString()) > System.Array.IndexOf<string>(array, extendedFormatImpl2.Borders[ExcelBordersIndex.EdgeRight].LineStyle.ToString()))
          {
            canDrawBorder = false;
            break;
          }
          break;
        case ExcelBordersIndex.EdgeRight:
          long key3 = ((long) cell.Row << 32 /*0x20*/) + (long) cell.Column + 1L;
          ExtendedFormatImpl extendedFormatImpl3 = (ExtendedFormatImpl) null;
          if (this._sortedListCf.ContainsKey(key3))
            this._sortedListCf.TryGetValue(key3, out extendedFormatImpl3);
          if (extendedFormatImpl3 != null && extendedFormatImpl1.Borders[ExcelBordersIndex.EdgeLeft].LineStyle != ExcelLineStyle.None && System.Array.IndexOf<string>(array, extendedFormatImpl1.Borders[ExcelBordersIndex.EdgeRight].LineStyle.ToString()) > System.Array.IndexOf<string>(array, extendedFormatImpl3.Borders[ExcelBordersIndex.EdgeLeft].LineStyle.ToString()))
          {
            canDrawBorder = false;
            break;
          }
          break;
      }
    }
    return canDrawBorder;
  }

  private void DrawBorder(
    IBorders borders,
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    PdfGraphics graphics,
    IRange cell,
    Color borderColor,
    ExcelBordersIndex index)
  {
    bool flag = false;
    bool isTop = index == ExcelBordersIndex.EdgeTop;
    if (border.LineStyle != ExcelLineStyle.None)
      flag = this.CanDrawBorder(borders, border, cell, index, isTop);
    if (index == ExcelBordersIndex.EdgeLeft || index == ExcelBordersIndex.EdgeRight || index == ExcelBordersIndex.DiagonalUp || index == ExcelBordersIndex.DiagonalDown)
      flag = true;
    if (border.LineStyle == ExcelLineStyle.Double && flag)
    {
      this.DrawDoubleBorder(borders, border, x1, y1, x2, y2, graphics, cell, borderColor);
    }
    else
    {
      if (!flag)
        return;
      this.DrawOrdinaryBorder(border, x1, y1, x2, y2, graphics, borderColor, index);
    }
  }

  private IRange GetLeftCell(RangeImpl range) => this.GetCell(0, -1, range);

  private IRange GetRightCell(RangeImpl range) => this.GetCell(0, 1, range);

  private IRange GetTopCell(RangeImpl range) => this.GetCell(-1, 0, range);

  private IRange GetBottomCell(RangeImpl range) => this.GetCell(1, 0, range);

  private IRange GetCell(int rowDelta, int colDelta, RangeImpl range)
  {
    int row = range.Row + rowDelta;
    int column = range.Column + colDelta;
    IRange cell = (IRange) null;
    if (row > 0 && row <= range.Workbook.MaxRowCount && column > 0 && column <= range.Workbook.MaxColumnCount)
      cell = range[row, column];
    return cell;
  }

  private bool CanDrawBorder(
    IBorders borders,
    IBorder border,
    IRange cell,
    ExcelBordersIndex index,
    bool isTop)
  {
    IWorksheet worksheet = cell.Worksheet;
    int row = cell.Row;
    int column = cell.Column;
    int maxRowCount = worksheet.Workbook.MaxRowCount;
    int maxColumnCount = worksheet.Workbook.MaxColumnCount;
    IBorder border1 = borders[index];
    int extendedFormatIndex1 = (int) (cell as RangeImpl).ExtendedFormatIndex;
    IBorders borders1 = (IBorders) null;
    if (!this._extBorders.TryGetValue(extendedFormatIndex1, out borders1))
      borders1 = (cell as RangeImpl).Workbook.InnerExtFormats[extendedFormatIndex1].Borders;
    (cell.CellStyle as CellStyle).AskAdjacent = false;
    IRange range;
    IBorder border2;
    if (isTop && row - 1 > 0)
    {
      range = worksheet[row - 1, column];
      (range.CellStyle as CellStyle).AskAdjacent = false;
      int extendedFormatIndex2 = (int) (range as RangeImpl).ExtendedFormatIndex;
      IBorders borders2 = (IBorders) null;
      if (!this._extBorders.TryGetValue(extendedFormatIndex2, out borders2))
        borders2 = (range as RangeImpl).Workbook.InnerExtFormats[extendedFormatIndex2].Borders;
      if (borders1[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.Double)
        return true;
      border2 = borders2[ExcelBordersIndex.EdgeBottom];
    }
    else
    {
      if (row + 1 >= maxRowCount)
        return false;
      range = worksheet[row + 1, column];
      int extendedFormatIndex3 = (int) (range as RangeImpl).ExtendedFormatIndex;
      IBorders borders3 = (IBorders) null;
      if (!this._extBorders.TryGetValue(extendedFormatIndex3, out borders3))
      {
        borders3 = (range as RangeImpl).Workbook.InnerExtFormats[extendedFormatIndex3].Borders;
        this._extBorders.Add(extendedFormatIndex3, borders3);
      }
      (range.CellStyle as CellStyle).AskAdjacent = false;
      if (borders1[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.Double)
        return true;
      border2 = borders3[ExcelBordersIndex.EdgeTop];
    }
    bool flag = border2.LineStyle == ExcelLineStyle.None || borders1[ExcelBordersIndex.EdgeBottom].LineStyle != ExcelLineStyle.Double;
    (cell.CellStyle as CellStyle).AskAdjacent = true;
    if (range != null)
      (range.CellStyle as CellStyle).AskAdjacent = true;
    return flag;
  }

  private void DrawDoubleBorder(
    IBorders borders,
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    PdfGraphics graphics,
    IRange cell,
    Color borderColor)
  {
    PdfPen pen = this.CreatePen(border, borderColor);
    ExcelBordersIndex borderIndex = (border as BorderImpl).BorderIndex;
    int deltaX;
    int deltaY;
    switch (borderIndex)
    {
      case ExcelBordersIndex.EdgeLeft:
        deltaX = -1;
        deltaY = 0;
        break;
      case ExcelBordersIndex.EdgeTop:
        deltaX = 0;
        deltaY = -1;
        break;
      case ExcelBordersIndex.EdgeBottom:
        deltaX = 0;
        deltaY = 1;
        break;
      case ExcelBordersIndex.EdgeRight:
        deltaX = 1;
        deltaY = 0;
        break;
      default:
        deltaX = 1;
        deltaY = 1;
        break;
    }
    this.DrawInnerBorderLine(graphics, pen, borders, borderIndex, x1, y1, x2, y2, deltaX, deltaY, cell);
    this.DrawOuterBorderLine(graphics, pen, borderIndex, x1, y1, x2, y2, deltaX, deltaY, cell);
  }

  private void DrawOuterBorderLine(
    PdfGraphics graphics,
    PdfPen pen,
    ExcelBordersIndex borderIndex,
    float x1,
    float y1,
    float x2,
    float y2,
    int deltaX,
    int deltaY,
    IRange cell)
  {
    ExcelBordersIndex start;
    ExcelBordersIndex end;
    this.GetStartEndBorderIndex(borderIndex, out start, out end);
    int deltaX1_1 = deltaX;
    int deltaX1_2 = deltaX;
    int deltaY1_1 = deltaY;
    int deltaY1_2 = deltaY;
    int row = cell.Row + deltaY;
    int column = cell.Column + deltaX;
    if (column == 0)
      column = 1;
    if (row == 0)
      row = 1;
    IBorders borders = cell.Worksheet[row, column].Borders;
    this.UpdateBorderDelta(cell.Worksheet, row, column, deltaX, deltaY, ref deltaX1_1, ref deltaY1_1, true, borders, end, start, true);
    this.UpdateBorderDelta(cell.Worksheet, row, column, deltaY, deltaY, ref deltaX1_2, ref deltaY1_2, true, borders, start, end, false);
    graphics.DrawLine(pen, x1 + (float) deltaX1_1, y1 + (float) deltaY1_1, x2 + (float) deltaX1_2, y2 + (float) deltaY1_2);
  }

  private void DrawInnerBorderLine(
    PdfGraphics graphics,
    PdfPen pen,
    IBorders borders,
    ExcelBordersIndex borderIndex,
    float x1,
    float y1,
    float x2,
    float y2,
    int deltaX,
    int deltaY,
    IRange cell)
  {
    ExcelBordersIndex start;
    ExcelBordersIndex end;
    this.GetStartEndBorderIndex(borderIndex, out start, out end);
    int deltaX1_1 = deltaX;
    int deltaX1_2 = deltaX;
    int deltaY1_1 = deltaY;
    int deltaY1_2 = deltaY;
    int row = cell.Row;
    int column = cell.Column;
    IWorksheet worksheet = cell.Worksheet;
    this.UpdateBorderDelta(worksheet, row, column, -deltaX, -deltaY, ref deltaX1_1, ref deltaY1_1, false, borders, end, start, true);
    this.UpdateBorderDelta(worksheet, row, column, -deltaX, -deltaY, ref deltaX1_2, ref deltaY1_2, false, borders, start, end, false);
    graphics.DrawLine(pen, x1 - (float) deltaX1_1, y1 - (float) deltaY1_1, x2 - (float) deltaX1_2, y2 - (float) deltaY1_2);
  }

  private void DrawOrdinaryBorder(
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    PdfGraphics graphics,
    Color borderColor,
    ExcelBordersIndex index)
  {
    if (border.LineStyle == ExcelLineStyle.None)
      return;
    PdfPen pen = this.CreatePen(border, borderColor);
    if ((double) pen.Width != 0.5)
    {
      float num = pen.Width / 2f;
      switch (index)
      {
        case ExcelBordersIndex.EdgeTop:
          x1 -= num;
          x2 -= num;
          break;
        case ExcelBordersIndex.EdgeBottom:
          x1 -= num;
          x2 += num;
          break;
        case ExcelBordersIndex.EdgeRight:
          y1 -= num;
          break;
      }
    }
    graphics.DrawLine(pen, x1, y1, x2, y2);
  }

  private PdfDocument DrawImages(WorksheetImpl sheet)
  {
    IPictures pictures = sheet.Pictures;
    List<int> maxList1 = new List<int>();
    List<int> maxList2 = new List<int>();
    float maxHeight = this._pdfDocument.PageSettings.Height - (this._pdfDocument.PageSettings.Margins.Top + this._pdfDocument.PageSettings.Margins.Bottom);
    float maxWidth = this._pdfDocument.PageSettings.Width - (this._pdfDocument.PageSettings.Margins.Left + this._pdfDocument.PageSettings.Margins.Right);
    int Index = 0;
    for (int count = pictures.Count; Index < count; ++Index)
    {
      IPictureShape pictureShape = pictures[Index];
      maxList1.Add(pictureShape.Top + pictureShape.Height);
      maxList2.Add(pictureShape.Left + pictureShape.Width);
    }
    int maxValue1 = this.GetMaxValue(maxList1);
    int maxValue2 = this.GetMaxValue(maxList2);
    Image image = (Image) new Bitmap(maxValue2, maxValue1);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image))
    {
      graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, maxValue2, maxValue1));
      foreach (IPictureShape pictureShape in (IEnumerable) pictures)
        graphics.DrawImage(pictureShape.Picture, new Rectangle(pictureShape.Left, pictureShape.Top, pictureShape.Width, pictureShape.Height));
    }
    PdfGraphics graphics1 = this._pdfDocument.Pages.Add().Graphics;
    Image scaledPicture = this.GetScaledPicture(image, (int) maxWidth, (int) maxHeight);
    PdfImage pdfImage = this.GetPdfImage(scaledPicture);
    graphics1.DrawImage(pdfImage, new RectangleF(0.0f, 0.0f, (float) scaledPicture.Width, (float) scaledPicture.Height));
    if (maxList1.Count != 0)
      maxList1.Clear();
    if (maxList2.Count != 0)
      maxList2.Clear();
    return this._pdfDocument;
  }

  private void DrawImage(
    IShape shape,
    Image image,
    PdfGraphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY,
    bool isAutoShapeWithTextureOrPictureFill,
    PdfPath path)
  {
    float num1 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn - 1), PdfGraphicsUnit.Point);
    float num2 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow - 1), PdfGraphicsUnit.Point);
    float num3 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(lastColumn), PdfGraphicsUnit.Point);
    float num4 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(lastRow), PdfGraphicsUnit.Point);
    float num5 = num1 - startX;
    float num6 = num2 - startY;
    float num7;
    float num8 = num7 = num5;
    float num9;
    float y1 = num9 = num6;
    float num10 = num7;
    float num11 = num9;
    ShapeImpl shapeImpl = shape as ShapeImpl;
    float num12;
    float num13;
    float num14;
    float num15;
    if (shapeImpl.GroupFrame != null)
    {
      num12 = (float) ApplicationImpl.ConvertToPixels((double) shapeImpl.GroupFrame.OffsetX, MeasureUnits.EMU);
      num13 = (float) ApplicationImpl.ConvertToPixels((double) shapeImpl.GroupFrame.OffsetY, MeasureUnits.EMU);
      num14 = (float) ApplicationImpl.ConvertToPixels((double) shapeImpl.GroupFrame.OffsetCX, MeasureUnits.EMU);
      num15 = (float) ApplicationImpl.ConvertToPixels((double) shapeImpl.GroupFrame.OffsetCY, MeasureUnits.EMU);
    }
    else
    {
      num12 = (float) shapeImpl.LeftDouble;
      num13 = (float) shapeImpl.TopDouble;
      num14 = (float) shapeImpl.WidthDouble;
      num15 = (float) shapeImpl.HeightDouble;
    }
    ShapeImpl shape1 = shape as ShapeImpl;
    bool inBetween;
    bool flag1 = this.CanDrawShape(firstRow, lastRow, firstColumn, lastColumn, shape1, out inBetween);
    if (flag1 && image == null && shape is IPictureShape)
      image = this.GetDrawingImage((shape as IPictureShape).Picture);
    if (image != null && (image.RawFormat.Equals((object) ImageFormat.Emf) || image.RawFormat.Equals((object) ImageFormat.Wmf)) && ApplicationImpl.EnablePartialTrustCodeStatic || image == null || !flag1 || (double) num12 >= (double) this._pdfPageTemplate.Width && firstColumn > shape1.LeftColumn && lastColumn < shape1.RightColumn && !inBetween || ((double) this.Pdf_UnitConverter.ConvertFromPixels(num13 * this._scaledCellHeight, PdfGraphicsUnit.Point) > (double) num4 || (double) this.Pdf_UnitConverter.ConvertFromPixels((num13 + num15) * this._scaledCellHeight, PdfGraphicsUnit.Point) < (double) num11 || (double) this.Pdf_UnitConverter.ConvertFromPixels(num12 * this._scaledCellWidth, PdfGraphicsUnit.Point) > (double) num3 || (double) this.Pdf_UnitConverter.ConvertFromPixels((num12 + num14) * this._scaledCellWidth, PdfGraphicsUnit.Point) < (double) num10) && ((double) this.Pdf_UnitConverter.ConvertFromPixels(num13 * this._scaledCellHeight, PdfGraphicsUnit.Point) > (double) this._currentPage.Size.Height || (double) this.Pdf_UnitConverter.ConvertFromPixels(num12 * this._scaledCellWidth, PdfGraphicsUnit.Point) > (double) this._currentPage.Size.Width))
      return;
    RectangleF rectangleF = new RectangleF(this.Pdf_UnitConverter.ConvertFromPixels(num12 * this._scaledCellWidth, PdfGraphicsUnit.Point), this.Pdf_UnitConverter.ConvertFromPixels(num13 * this._scaledCellHeight, PdfGraphicsUnit.Point), this.Pdf_UnitConverter.ConvertFromPixels(num14 * this._scaledCellWidth, PdfGraphicsUnit.Point), this.Pdf_UnitConverter.ConvertFromPixels(num15 * this._scaledCellHeight, PdfGraphicsUnit.Point));
    if (this._workSheet.IsRightToLeft && firstRow <= shape1.TopRow && lastRow >= shape1.BottomRow)
    {
      float x = this._pdfPageTemplate.Width - (rectangleF.Width + rectangleF.Left + startX);
      if ((double) rectangleF.Y < (double) x || (double) num13 > (double) num12)
      {
        x -= rectangleF.Left;
        y1 = startY;
      }
      if ((double) rectangleF.Y > (double) num11 && this._pdfDocument.Pages.Count > 1)
      {
        rectangleF.Y -= x + x / 2f - rectangleF.Height;
        y1 = startY;
      }
      rectangleF.Offset(x, y1);
    }
    else
    {
      if ((double) num11 > 0.0 && (!this.IsPrintTitleRowPage || (double) num10 > 0.0))
        num11 -= startY;
      if ((double) num10 > 0.0 && this._isNewPage)
        num11 += startY;
      rectangleF.Offset(-num10, -num11);
    }
    ImageAttributes imageAttributes1 = (ImageAttributes) null;
    if (shape is BitmapShapeImpl)
    {
      BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
      if (this._workSheet.PageSetup.BlackAndWhite)
      {
        Bitmap bitmap = new Bitmap(image);
        for (int y2 = 0; y2 < image.Height; ++y2)
        {
          for (int x = 0; x < image.Width; ++x)
          {
            Color pixel = bitmap.GetPixel(x, y2);
            int a = (int) pixel.A;
            int num16 = ((int) pixel.R + (int) pixel.G + (int) pixel.B) / 3;
            bitmap.SetPixel(x, y2, Color.FromArgb(a, num16, num16, num16));
          }
        }
        image = (Image) bitmap;
      }
      if (bitmapShapeImpl.GrayScale || bitmapShapeImpl.Threshold > 0)
        image = (Image) this.ApplyRecolor(bitmapShapeImpl, image);
      if (bitmapShapeImpl.ColorChange != null && bitmapShapeImpl.ColorChange.Count == 2)
      {
        if (bitmapShapeImpl.ColorChange[1].GetRGB(this._workBook).A == (byte) 0)
        {
          Bitmap bitmap = new Bitmap(image);
          Color pixel = bitmap.GetPixel(1, 1);
          bitmap.MakeTransparent(pixel);
          MemoryStream memoryStream = new MemoryStream();
          bitmap.Save((Stream) memoryStream, ImageFormat.Png);
          image = Image.FromStream((Stream) memoryStream);
        }
        else
        {
          ImageAttributes imageAttributes2 = new ImageAttributes();
          imageAttributes1 = this.ColorChange(bitmapShapeImpl, imageAttributes2);
        }
      }
      if (bitmapShapeImpl.DuoTone != null && bitmapShapeImpl.DuoTone.Count == 2)
        image = this.ApplyDuoTone(image, bitmapShapeImpl.DuoTone);
      double transparency = 1.0;
      if (bitmapShapeImpl.Amount / 100000 < 1)
      {
        if (transparency < 0.0)
          transparency = 0.0;
        if (imageAttributes1 == null)
          imageAttributes1 = new ImageAttributes();
        this.ApplyImageTransparency(imageAttributes1, (float) transparency);
      }
      if (imageAttributes1 != null)
      {
        Bitmap bitmap = new Bitmap(image.Width, image.Height);
        System.Drawing.Graphics graphics1 = System.Drawing.Graphics.FromImage((Image) bitmap);
        graphics1.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes1);
        image = (Image) bitmap;
        graphics1.Dispose();
      }
    }
    MemoryStream memoryStream1 = new MemoryStream();
    if (shape is BitmapShapeImpl)
    {
      BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
      double leftOffset = (double) bitmapShapeImpl.CropLeftOffset / 1000.0;
      double topOffset = (double) (bitmapShapeImpl.CropTopOffset / 1000);
      double rightOffset = (double) (bitmapShapeImpl.CropRightOffset / 1000);
      double bottomOffset = (double) (bitmapShapeImpl.CropBottomOffset / 1000);
      bool flag2 = image.RawFormat.Equals((object) ImageFormat.Png);
      if ((double) rectangleF.Height < (double) image.Size.Height && (double) rectangleF.Width < (double) image.Width && (bitmapShapeImpl.CropLeftOffset > 0 || bitmapShapeImpl.CropTopOffset > 0 || bitmapShapeImpl.CropRightOffset > 0 || bitmapShapeImpl.CropBottomOffset > 0))
        image = Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.CropHFImage(image, leftOffset, topOffset, rightOffset, bottomOffset, bitmapShapeImpl.HasTransparency);
      if (bitmapShapeImpl.HasTransparency || flag2)
        image.Save((Stream) memoryStream1, ImageFormat.Png);
      else if (this._excelToPdfSettings.ExportQualityImage && !image.RawFormat.Equals((object) ImageFormat.Png) && !this.ContainsTransparent(new Bitmap(image)))
        image.Save((Stream) memoryStream1, ImageFormat.Tiff);
    }
    PdfGraphicsState state = graphics.Save();
    if (shape.ShapeRotation != 0 && shape is BitmapShapeImpl && (shape as ShapeImpl).GroupFrame == null)
    {
      BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
      if ((bitmapShapeImpl.OffsetX != 0L || bitmapShapeImpl.OffsetY != 0L || bitmapShapeImpl.ExtentsX != 0L || bitmapShapeImpl.ExtentsX != 0L) && (double) this.EmuToPoint((int) bitmapShapeImpl.OffsetX) <= (double) this._currentPage.Size.Width && (double) this.EmuToPoint((int) bitmapShapeImpl.OffsetY) <= (double) this._currentPage.Size.Height)
      {
        rectangleF.X = this.EmuToPoint((int) bitmapShapeImpl.OffsetX);
        rectangleF.Y = this.EmuToPoint((int) bitmapShapeImpl.OffsetY);
        rectangleF.Width = this.EmuToPoint((int) bitmapShapeImpl.ExtentsX);
        rectangleF.Height = this.EmuToPoint((int) bitmapShapeImpl.ExtentsY);
      }
    }
    graphics.ResetTransform();
    if (isAutoShapeWithTextureOrPictureFill && path != null)
      graphics.SetClip(path);
    if (shape is BitmapShapeImpl)
      this.Rotate(graphics, (ShapeImpl) (shape as BitmapShapeImpl), rectangleF);
    if (memoryStream1.Length != 0L)
    {
      if (shapeImpl is BitmapShapeImpl && (shapeImpl as BitmapShapeImpl).HasTransparency)
      {
        PdfImage image1 = graphics.GetImage((Stream) memoryStream1, this._pdfDocument);
        image1.Matte = new int[3];
        graphics.DrawImage(image1, rectangleF);
        if (image1 is PdfBitmap)
          (image1 as PdfBitmap).Dispose();
      }
      else
      {
        if (shape is BitmapShapeImpl && !shape.Line.Visible && (double) rectangleF.Height > 0.0 && (double) rectangleF.Width > 0.0)
        {
          PdfPen pen = this.CreatePen(shape.Line);
          if (this._workSheet.PageSetup.BlackAndWhite)
            pen.Color = (PdfColor) Color.Black;
          pen.Width = 0.75f;
          rectangleF = new RectangleF(rectangleF.X - pen.Width / 2f, rectangleF.Y - pen.Width / 2f, rectangleF.Width - pen.Width, rectangleF.Height - pen.Width);
        }
        graphics.DrawImage(graphics.GetImage((Stream) memoryStream1, this._pdfDocument), rectangleF);
      }
    }
    else
    {
      if (!(image is Metafile) && shape.ShapeType != ExcelShapeType.Chart)
      {
        double pixels1 = (double) this._pdfUnitConverter.ConvertToPixels(rectangleF.Width, PdfGraphicsUnit.Point);
        double pixels2 = (double) this._pdfUnitConverter.ConvertToPixels(rectangleF.Height, PdfGraphicsUnit.Point);
        if (Math.Min(pixels1 / (double) image.Width, pixels2 / (double) image.Height) <= 0.1)
          image = this.GetResizedImage(image, (int) pixels1, (int) pixels2);
      }
      PdfImage pdfImage = this.GetPdfImage(image);
      graphics.DrawImage(pdfImage, rectangleF);
    }
    if (shape is BitmapShapeImpl && shape.Line.Visible)
    {
      PdfPen pen = this.CreatePen(shape.Line);
      if (this._workSheet.PageSetup.BlackAndWhite)
        pen.Color = (PdfColor) Color.Black;
      graphics.DrawRectangle(pen, rectangleF.X - pen.Width / 2f, rectangleF.Y - pen.Width / 2f, rectangleF.Width + pen.Width, rectangleF.Height + pen.Width);
    }
    this.SetShapeHyperlink(shape1, rectangleF);
    graphics.Restore(state);
    imageAttributes1?.Dispose();
    memoryStream1.Close();
    graphics.ResetTransform();
  }

  private void ApplyImageTransparency(ImageAttributes imgAttribute, float transparency)
  {
    imgAttribute.SetColorMatrix(new ColorMatrix()
    {
      Matrix33 = transparency
    }, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
  }

  private Bitmap ApplyRecolor(BitmapShapeImpl picture, Image image)
  {
    Bitmap bitmap = image.PixelFormat.ToString().ToLower().Contains("rgb") ? (Bitmap) image : this.CreateNonIndexedImage(image);
    for (int x = 0; x < bitmap.Width; ++x)
    {
      for (int y = 0; y < bitmap.Height; ++y)
      {
        Color pixel = bitmap.GetPixel(x, y);
        if (picture.GrayScale)
        {
          byte num = (byte) (0.299 * (double) pixel.R + 0.587 * (double) pixel.G + 0.114 * (double) pixel.B);
          bitmap.SetPixel(x, y, Color.FromArgb((int) pixel.A, (int) num, (int) num, (int) num));
        }
        else
        {
          int maxValue = (0.299 * (double) pixel.R + 0.587 * (double) pixel.G + 0.114 * (double) pixel.B) / 2.5 >= (double) (picture.Threshold / 1000) ? (int) byte.MaxValue : 0;
          bitmap.SetPixel(x, y, Color.FromArgb((int) pixel.A, maxValue, maxValue, maxValue));
        }
      }
    }
    return bitmap;
  }

  private ImageAttributes ColorChange(BitmapShapeImpl pictureImpl, ImageAttributes imageAttributes)
  {
    List<ColorObject> colorChange = pictureImpl.ColorChange;
    ColorObject colorObject1 = colorChange[0];
    ColorObject colorObject2 = colorChange[1];
    ColorMap[] map = new ColorMap[1]{ new ColorMap() };
    map[0].OldColor = colorObject1.GetRGB(this._workBook);
    map[0].NewColor = !pictureImpl.IsUseAlpha ? Color.FromArgb((int) colorObject2.GetRGB(this._workBook).R, (int) colorObject2.GetRGB(this._workBook).G, (int) colorObject2.GetRGB(this._workBook).B) : colorObject2.GetRGB(this._workBook);
    imageAttributes.SetRemapTable(map);
    return imageAttributes;
  }

  private Image ApplyDuoTone(Image image, List<ColorObject> duotone)
  {
    if (duotone.Count != 2)
      return image;
    ColorObject colorObject1 = new ColorObject(ColorExtension.Empty);
    ColorObject colorObject2 = new ColorObject(ColorExtension.Empty);
    Bitmap bitmap1 = image as Bitmap;
    Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, bitmap1.PixelFormat);
    ColorObject colorObject3 = duotone[1];
    ColorObject colorObject4 = duotone[0];
    Color color1 = Color.FromArgb((int) byte.MaxValue - (int) colorObject4.GetRGB(this._workBook).A, colorObject4.GetRGB(this._workBook));
    Color color2 = Color.FromArgb((int) byte.MaxValue - (int) colorObject3.GetRGB(this._workBook).A, colorObject3.GetRGB(this._workBook));
    Rectangle rect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
    BitmapData bitmapdata1 = bitmap1.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
    BitmapData bitmapdata2 = bitmap2.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
    int length1 = Math.Abs(bitmapdata1.Stride) * bitmap1.Height;
    byte[] destination = new byte[length1];
    Marshal.Copy(bitmapdata1.Scan0, destination, 0, length1);
    int length2 = Math.Abs(bitmapdata2.Stride) * bitmap2.Height;
    byte[] numArray = new byte[length2];
    Marshal.Copy(bitmapdata2.Scan0, numArray, 0, length2);
    for (int index = 0; index < length2; index += 4)
    {
      Color inputPixelColor = Color.FromArgb((int) destination[index + 3], (int) destination[index + 2], (int) destination[index + 1], (int) destination[index]);
      float num = (float) Math.Sqrt(0.299 * (double) inputPixelColor.R * (double) inputPixelColor.R + 0.587 * (double) inputPixelColor.G * (double) inputPixelColor.G + 0.114 * (double) inputPixelColor.B * (double) inputPixelColor.B);
      float factor = num / (float) byte.MaxValue;
      Color empty = Color.Empty;
      Color color3 = (double) num == (double) byte.MaxValue || (double) num == 0.0 ? ((double) num != (double) byte.MaxValue ? Color.FromArgb((int) inputPixelColor.A, color1) : Color.FromArgb((int) inputPixelColor.A, color2)) : this.ExecuteLinearInterpolation(color1, color2, inputPixelColor, factor);
      numArray[index] = color3.B;
      numArray[index + 1] = color3.G;
      numArray[index + 2] = color3.R;
      numArray[index + 3] = color3.A;
    }
    Marshal.Copy(numArray, 0, bitmapdata2.Scan0, length2);
    bitmap1.UnlockBits(bitmapdata1);
    bitmap2.UnlockBits(bitmapdata2);
    bitmap1.Dispose();
    return (Image) bitmap2;
  }

  private Color ExecuteLinearInterpolation(
    Color firstColor,
    Color secondColor,
    Color inputPixelColor,
    float factor)
  {
    int red = (int) ((1.0 - (double) factor) * (double) firstColor.R + (double) factor * (double) secondColor.R);
    int green = (int) ((1.0 - (double) factor) * (double) firstColor.G + (double) factor * (double) secondColor.G);
    int blue = (int) ((1.0 - (double) factor) * (double) firstColor.B + (double) factor * (double) secondColor.B);
    return Color.FromArgb((int) inputPixelColor.A, red, green, blue);
  }

  private Bitmap CreateNonIndexedImage(Image sourceImage)
  {
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb)))
      graphics.DrawImage(sourceImage, 0, 0);
    return sourceImage as Bitmap;
  }

  private float EmuToPoint(int emu) => (float) System.Convert.ToDouble((double) emu / 12700.0);

  private void Rotate(PdfGraphics graphics, ShapeImpl shapeImpl, RectangleF rectangleF)
  {
    float shapeRotation1 = (float) shapeImpl.ShapeRotation;
    if ((double) shapeRotation1 > 360.0)
      shapeRotation1 %= 360f;
    bool flipV1 = false;
    bool flipH = false;
    switch (shapeImpl)
    {
      case AutoShapeImpl _:
        flipV1 = (shapeImpl as AutoShapeImpl).ShapeExt.FlipVertical;
        flipH = (shapeImpl as AutoShapeImpl).ShapeExt.FlipHorizontal;
        break;
      case TextBoxShapeImpl _:
        flipV1 = (shapeImpl as TextBoxShapeImpl).FlipVertical;
        flipH = (shapeImpl as TextBoxShapeImpl).FlipHorizontal;
        break;
      case BitmapShapeImpl _:
        flipV1 = (shapeImpl as BitmapShapeImpl).FlipVertical;
        flipH = (shapeImpl as BitmapShapeImpl).FlipHorizontal;
        break;
    }
    if (shapeImpl.Group != null)
    {
      float shapeRotation2 = (float) shapeImpl.GetShapeRotation();
      if (this.IsGroupFlipH(shapeImpl.Group) || this.IsGroupFlipV(shapeImpl.Group))
      {
        int flipVcount = this.GetFlipVCount(shapeImpl.Group, flipV1 ? 1 : 0);
        int flipHcount = this.GetFlipHCount(shapeImpl.Group, flipH ? 1 : 0);
        graphics.Transform = this.GetTransformMatrix(rectangleF, shapeRotation2, flipVcount % 2 != 0, flipHcount % 2 != 0);
      }
      else if ((double) shapeRotation2 != 0.0 || flipV1 || flipH)
        graphics.Transform = this.GetTransformMatrix(rectangleF, shapeRotation2, flipV1, flipH);
    }
    else if ((double) shapeRotation1 != 0.0 || flipV1 || flipH)
      graphics.Transform = this.GetTransformMatrix(rectangleF, shapeRotation1, flipV1, flipH);
    if (!(shapeImpl is AutoShapeImpl) || !(shapeImpl as AutoShapeImpl).ShapeExt.PreservedElements.ContainsKey("Scene3d"))
      return;
    bool flip = false;
    bool flipV2 = false;
    float latFromScene3D = this.GetLatFromScene3D(shapeImpl as AutoShapeImpl, out flip);
    if ((double) latFromScene3D == 0.0)
      return;
    graphics.Transform = this.GetTransformMatrix(rectangleF, latFromScene3D, flipV2, flip);
  }

  private bool IsGroupFlipH(GroupShapeImpl group)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipHorizontal)
        return true;
    }
    return false;
  }

  private bool IsGroupFlipV(GroupShapeImpl group)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipVertical)
        return true;
    }
    return false;
  }

  private int GetFlipHCount(GroupShapeImpl group, int count)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipHorizontal)
        ++count;
    }
    return count;
  }

  private int GetFlipVCount(GroupShapeImpl group, int count)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipVertical)
        ++count;
    }
    return count;
  }

  private float GetLatFromScene3D(AutoShapeImpl shapeImpl, out bool flip)
  {
    float latFromScene3D = 0.0f;
    int num = 0;
    XmlReader reader = UtilityMethods.CreateReader(shapeImpl.ShapeExt.PreservedElements["Scene3d"], "rot");
    if (reader.MoveToAttribute("lat"))
      latFromScene3D = (float) (int) (System.Convert.ToInt64(reader.Value) / 60000L);
    if (reader.MoveToAttribute("lon"))
      num = (int) (System.Convert.ToInt64(reader.Value) / 60000L);
    flip = num != 180;
    return latFromScene3D;
  }

  private void ApplyRotation(
    ShapeImpl shape,
    RectangleF bounds,
    float rotationAngle,
    PdfGraphics graphics)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (shape is AutoShapeImpl)
    {
      flag1 = (shape as AutoShapeImpl).ShapeExt.FlipVertical;
      flag2 = (shape as AutoShapeImpl).ShapeExt.FlipHorizontal;
    }
    else if (shape is TextBoxShapeImpl)
    {
      flag1 = (shape as TextBoxShapeImpl).FlipVertical;
      flag2 = (shape as TextBoxShapeImpl).FlipHorizontal;
    }
    if (shape.Group != null && (this.IsGroupFlipH(shape.Group) || this.IsGroupFlipV(shape.Group)))
    {
      int flipVcount = this.GetFlipVCount(shape.Group, flag1 ? 1 : 0);
      int flipHcount = this.GetFlipHCount(shape.Group, flag2 ? 1 : 0);
      rotationAngle = (float) shape.GetShapeRotation();
      if (flipVcount % 2 != 0)
      {
        graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, true, true);
      }
      else
      {
        if (flipHcount % 2 == 0)
          return;
        graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, false, false);
      }
    }
    else if (flag1)
    {
      graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, true, true);
    }
    else
    {
      if (!flag2)
        return;
      graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, false, false);
    }
  }

  private void RotateText(RectangleF bounds, TextDirection textDirectionType)
  {
    switch (textDirectionType)
    {
      case TextDirection.RotateAllText90:
        this._pdfGraphics.TranslateTransform(bounds.X + bounds.Y + bounds.Height, bounds.Y - bounds.X);
        this._pdfGraphics.RotateTransform(90f);
        break;
      case TextDirection.RotateAllText270:
        this._pdfGraphics.TranslateTransform(bounds.X - bounds.Y, bounds.X + bounds.Y + bounds.Width);
        this._pdfGraphics.RotateTransform(270f);
        break;
    }
  }

  private Matrix GetTransformMatrix(RectangleF bounds, float ang, bool flipV, bool flipH)
  {
    Matrix transformMatrix = new Matrix();
    Matrix matrix1 = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
    Matrix matrix2 = new Matrix(-1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    PointF point = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    if (flipV)
    {
      transformMatrix.Multiply(matrix1, MatrixOrder.Append);
      transformMatrix.Translate(0.0f, point.Y * 2f, MatrixOrder.Append);
    }
    if (flipH)
    {
      transformMatrix.Multiply(matrix2, MatrixOrder.Append);
      transformMatrix.Translate(point.X * 2f, 0.0f, MatrixOrder.Append);
    }
    transformMatrix.RotateAt(ang, point, MatrixOrder.Append);
    return transformMatrix;
  }

  private PdfTransformationMatrix GetTransformationMatrix(RectangleF bounds, float angle)
  {
    Matrix matrix1 = new Matrix();
    matrix1.RotateAt(angle, new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f), MatrixOrder.Append);
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    PdfTransformationMatrix matrix2 = new PdfTransformationMatrix();
    matrix2.Matrix = matrix1;
    transformationMatrix.Scale(1f, -1f);
    transformationMatrix.Multiply(matrix2);
    transformationMatrix.Scale(1f, -1f);
    return transformationMatrix;
  }

  private void SetShapeHyperlink(ShapeImpl shape, RectangleF bounds)
  {
    if (!shape.IsHyperlink)
      return;
    string address = shape.Hyperlink.Address;
    if (RangeImpl.GetWorksheetName(ref address) != null)
      return;
    bounds = this.ReSize(bounds);
    PdfUriAnnotation annotation = new PdfUriAnnotation(bounds, address);
    annotation.Border.Width = 0.0f;
    this._currentPage.Annotations.Add((PdfAnnotation) annotation);
  }

  private void DrawChart(
    ChartShapeImpl chart,
    PdfGraphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    if (this.ChartToImageConverter == null)
      return;
    MemoryStream imageAsStream = (MemoryStream) null;
    if (!this._chartImageCollection.TryGetValue((IChart) chart, out imageAsStream))
    {
      imageAsStream = new MemoryStream();
      chart.SaveAsImage((Stream) imageAsStream);
      imageAsStream.Position = 0L;
      this._chartImageCollection.Add((IChart) chart, imageAsStream);
    }
    Image image = Image.FromStream((Stream) imageAsStream);
    if (imageAsStream.Length > 0L)
      this.DrawImage((IShape) chart, image, graphics, firstRow, firstColumn, lastRow, lastColumn, startX, startY, false, (PdfPath) null);
    image.Dispose();
  }

  public static Image CropHFImage(
    Image cropableImage,
    double leftOffset,
    double topOffset,
    double rightOffset,
    double bottomOffset,
    bool isTransparent)
  {
    double width = (double) cropableImage.Width;
    double height = (double) cropableImage.Height;
    leftOffset = width * (leftOffset / 100.0);
    topOffset = height * (topOffset / 100.0);
    rightOffset = width * (rightOffset / 100.0);
    bottomOffset = height * (bottomOffset / 100.0);
    RectangleF rect = new RectangleF((float) -leftOffset, (float) -topOffset, (float) width, (float) height);
    Bitmap bitmap1 = new Bitmap((int) (width - leftOffset - rightOffset), (int) (height - topOffset - bottomOffset));
    bitmap1.SetResolution(cropableImage.VerticalResolution, cropableImage.HorizontalResolution);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap1);
    graphics.Clear(Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    if (isTransparent)
    {
      graphics.CompositingMode = CompositingMode.SourceCopy;
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
      graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
    }
    graphics.DrawImage(cropableImage, rect);
    MemoryStream memoryStream = new MemoryStream();
    bitmap1.Save((Stream) memoryStream, ImageFormat.Png);
    Bitmap bitmap2 = Image.FromStream((Stream) memoryStream) as Bitmap;
    memoryStream.Dispose();
    return (Image) bitmap2;
  }

  private float[] GetHFScaledWidthHeight(string preservedStyles)
  {
    string[] strArray = preservedStyles.Split(';');
    float[] scaledWidthHeight = new float[2];
    foreach (string str in strArray)
    {
      if (str.Contains("width"))
      {
        string style = str.Replace("width:", " ");
        scaledWidthHeight[0] = this.GetValueFromStyle(style) * this._scaledCellWidth;
      }
      else if (str.Contains("height"))
      {
        string style = str.Replace("height:", " ");
        scaledWidthHeight[1] = this.GetValueFromStyle(style) * this._scaledCellHeight;
      }
    }
    return scaledWidthHeight;
  }

  private float GetValueFromStyle(string style)
  {
    if (style.Contains("pt"))
    {
      style = style.Replace("pt", " ");
      return float.Parse(style, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    if (style.Contains("in"))
    {
      style = style.Replace("in", " ");
      return this.Pdf_UnitConverter.ConvertUnits(float.Parse(style, (IFormatProvider) CultureInfo.InvariantCulture), PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    }
    if (style.Contains("mm"))
    {
      style = style.Replace("mm", " ");
      return this.Pdf_UnitConverter.ConvertUnits(float.Parse(style, (IFormatProvider) CultureInfo.InvariantCulture), PdfGraphicsUnit.Millimeter, PdfGraphicsUnit.Point);
    }
    if (style.Contains("cm"))
    {
      style = style.Replace("cm", " ");
      return this.Pdf_UnitConverter.ConvertUnits(float.Parse(style, (IFormatProvider) CultureInfo.InvariantCulture), PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
    }
    if (style.Contains("pc"))
    {
      style = style.Replace("pc", " ");
      return this.Pdf_UnitConverter.ConvertUnits(float.Parse(style, (IFormatProvider) CultureInfo.InvariantCulture), PdfGraphicsUnit.Pica, PdfGraphicsUnit.Point);
    }
    style = style.Replace("px", " ");
    return this.Pdf_UnitConverter.ConvertUnits(float.Parse(style, (IFormatProvider) CultureInfo.InvariantCulture), PdfGraphicsUnit.Pixel, PdfGraphicsUnit.Point);
  }

  private Image GetHeaderFooterImages(
    IPageSetupBase pageSetupBase,
    ref float width,
    ref float height,
    PdfTemplate pageTemplate,
    string align,
    string name,
    PdfSection pdfSection)
  {
    Image cropableImage = (Image) null;
    string strShapeName = string.Empty;
    if (align == "Left" && pageSetupBase.LeftHeaderImage != null && name == "Header")
    {
      if ((double) width != 0.0 && (double) height != 0.0)
      {
        cropableImage = this.GetDrawingImage(pageSetupBase.LeftHeaderImage);
      }
      else
      {
        width = 0.0f;
        this.GetScaledPictureWidthHeight(this.GetDrawingImage(pageSetupBase.LeftHeaderImage), ref width, ref height);
        cropableImage = this.GetDrawingImage(pageSetupBase.LeftHeaderImage);
      }
      strShapeName = "LH";
    }
    else if (align == "Center" && pageSetupBase.CenterHeaderImage != null && name == "Header")
    {
      if ((double) width != 0.0 && (double) height != 0.0)
      {
        cropableImage = this.GetDrawingImage(pageSetupBase.CenterHeaderImage);
      }
      else
      {
        width = 0.0f;
        this.GetScaledPictureWidthHeight(this.GetDrawingImage(pageSetupBase.CenterHeaderImage), ref width, ref height);
        cropableImage = this.GetDrawingImage(pageSetupBase.CenterHeaderImage);
      }
      strShapeName = "CH";
    }
    else if (align == "Right" && pageSetupBase.RightHeaderImage != null && name == "Header")
    {
      if ((double) width != 0.0 && (double) height != 0.0)
      {
        cropableImage = this.GetDrawingImage(pageSetupBase.RightHeaderImage);
      }
      else
      {
        width = 0.0f;
        this.GetScaledPictureWidthHeight(this.GetDrawingImage(pageSetupBase.RightHeaderImage), ref width, ref height);
        cropableImage = this.GetDrawingImage(pageSetupBase.RightHeaderImage);
      }
      strShapeName = "RH";
    }
    else if (align == "Left" && pageSetupBase.LeftFooterImage != null && name == "Footer")
    {
      if ((double) width != 0.0 && (double) height != 0.0)
      {
        cropableImage = this.GetDrawingImage(pageSetupBase.LeftFooterImage);
      }
      else
      {
        width = 0.0f;
        this.GetScaledPictureWidthHeight(this.GetDrawingImage(pageSetupBase.LeftFooterImage), ref width, ref height);
        cropableImage = this.GetDrawingImage(pageSetupBase.LeftFooterImage);
      }
      strShapeName = "LF";
    }
    else if (align == "Center" && pageSetupBase.CenterFooterImage != null && name == "Footer")
    {
      if ((double) width != 0.0 && (double) height != 0.0)
      {
        cropableImage = this.GetDrawingImage(pageSetupBase.CenterFooterImage);
      }
      else
      {
        width = 0.0f;
        this.GetScaledPictureWidthHeight(this.GetDrawingImage(pageSetupBase.CenterFooterImage), ref width, ref height);
        cropableImage = this.GetDrawingImage(pageSetupBase.CenterFooterImage);
      }
      strShapeName = "CF";
    }
    else if (align == "Right" && pageSetupBase.RightFooterImage != null && name == "Footer")
    {
      if ((double) width != 0.0 && (double) height != 0.0)
      {
        cropableImage = this.GetDrawingImage(pageSetupBase.RightFooterImage);
      }
      else
      {
        width = 0.0f;
        this.GetScaledPictureWidthHeight(this.GetDrawingImage(pageSetupBase.RightFooterImage), ref width, ref height);
        cropableImage = this.GetDrawingImage(pageSetupBase.RightFooterImage);
      }
      strShapeName = "RF";
    }
    if (cropableImage != null)
    {
      BitmapShapeImpl bitmapShapeImpl = (pageSetupBase as PageSetupBaseImpl).FindParent(typeof (ChartImpl)) != null ? ((pageSetupBase as PageSetupBaseImpl).FindParent(typeof (ChartImpl)) as ChartImpl).HeaderFooterShapes[strShapeName] as BitmapShapeImpl : (pageSetupBase as PageSetupImpl).Worksheet.HeaderFooterShapes[strShapeName] as BitmapShapeImpl;
      double leftOffset = (double) bitmapShapeImpl.CropLeftOffset * 100.0 / 65536.0;
      double topOffset = (double) bitmapShapeImpl.CropTopOffset * 100.0 / 65536.0;
      double rightOffset = (double) bitmapShapeImpl.CropRightOffset * 100.0 / 65536.0;
      double bottomOffset = (double) bitmapShapeImpl.CropBottomOffset * 100.0 / 65536.0;
      if (bitmapShapeImpl.CropLeftOffset != 0 || bitmapShapeImpl.CropTopOffset != 0 || bitmapShapeImpl.CropRightOffset != 0 || bitmapShapeImpl.CropLeftOffset != 0)
        cropableImage = Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.CropHFImage(cropableImage, leftOffset, topOffset, rightOffset, bottomOffset, bitmapShapeImpl.HasTransparency);
    }
    return cropableImage;
  }

  internal Image GetDrawingImage(Image image) => image;

  internal PdfImage GetPdfImage(Image image) => PdfImage.FromImage(image);

  internal SizeF CalculateZoomValue(PdfSection pdfSection, int zoomValue)
  {
    if (zoomValue <= 100)
    {
      this._sheetHeight = (float) ((double) pdfSection.PageSettings.Height * (double) (100 - zoomValue) / 100.0 + ((double) pdfSection.PageSettings.Height + (double) this._headerMargin + (double) this._footerMargin));
      this._sheetWidth = (float) ((double) pdfSection.PageSettings.Width * (double) (100 - zoomValue) / 100.0) + pdfSection.PageSettings.Width;
      if (zoomValue > 50)
      {
        this._sheetHeight -= (float) ((double) this._sheetHeight * (double) (100 - zoomValue) / 100.0);
        this._sheetWidth -= (float) ((double) this._sheetWidth * (double) (100 - zoomValue) / 100.0);
        if (zoomValue == 100)
        {
          this._sheetHeight += (float) ((double) this._footerMargin + (double) this._headerMargin + 2.0 * (double) this._topMargin + 2.0 * (double) this._bottomMargin);
          this._sheetWidth = (float) ((double) this._sheetWidth + (double) this._sheetWidth + (2.0 * (double) this._rightMargin + 2.0 * (double) this._leftMargin));
        }
        else if (zoomValue >= 90)
        {
          this._sheetHeight += pdfSection.PageSettings.Margins.Bottom + pdfSection.PageSettings.Margins.Top;
          this._sheetWidth -= this._rightMargin + this._leftMargin;
        }
        else if (zoomValue >= 80 /*0x50*/)
        {
          this._sheetHeight += this._footerMargin + this._headerMargin;
          this._sheetWidth += (float) (2.0 * (double) this._rightMargin + 2.0 * (double) this._leftMargin);
        }
        else if (zoomValue >= 70)
        {
          this._sheetHeight += pdfSection.PageSettings.Margins.Bottom + pdfSection.PageSettings.Margins.Top;
          this._sheetWidth += pdfSection.PageSettings.Margins.Right + pdfSection.PageSettings.Margins.Left;
        }
        else if (zoomValue >= 60)
        {
          this._sheetHeight = (float) ((double) this._sheetHeight + (double) this._sheetHeight / 4.0 + ((double) this._footerMargin + (double) this._headerMargin + (double) this._topMargin + 2.0 * (double) this._bottomMargin));
          this._sheetWidth = (float) ((double) this._sheetWidth + (double) this._sheetWidth / 2.0 + (3.0 * (double) this._rightMargin + 3.0 * (double) this._leftMargin));
        }
        else
        {
          this._sheetHeight = (float) ((double) this._sheetHeight + (double) this._sheetHeight / 2.0 + ((double) this._footerMargin + (double) this._headerMargin + (double) this._topMargin + (double) this._bottomMargin));
          this._sheetWidth = (float) ((double) this._sheetWidth + (double) this._sheetWidth / 2.0 + (3.0 * (double) this._rightMargin + 3.0 * (double) this._leftMargin));
        }
      }
      else
      {
        if (zoomValue >= 40)
          this._sheetHeight += this._footerMargin + this._headerMargin;
        else if (zoomValue >= 30)
        {
          this._sheetHeight += (float) ((double) this._sheetHeight * (double) (100 - zoomValue) / 100.0);
          this._sheetHeight -= this._footerMargin + this._headerMargin + this._topMargin + this._bottomMargin;
        }
        else if (zoomValue >= 20)
        {
          this._sheetHeight += (float) ((double) this._sheetHeight * (double) (100 - zoomValue) / 100.0);
          this._sheetHeight += this._footerMargin + this._headerMargin;
        }
        else if (zoomValue >= 10)
        {
          this._sheetHeight += (float) ((double) this._sheetHeight * (double) (100 - zoomValue) / 100.0);
          this._sheetHeight += this._footerMargin + this._headerMargin + this._topMargin + this._bottomMargin;
        }
        this._sheetWidth += this._rightMargin + this._leftMargin;
      }
    }
    return new SizeF(this._sheetWidth, this._sheetHeight);
  }

  private void InitializePdfPage()
  {
    this._currentPage = this._pdfSection.Pages.Add();
    if (this._pdfSection.Pages.Count > 1)
      this._isNewPage = true;
    this._pdfGraphics = this._pdfPageTemplate == null ? this._currentPage.Graphics : this._pdfPageTemplate.Graphics;
    this.InitializeDirectPDF(this._pdfGraphics);
  }

  private void InitializeDirectPDF(PdfGraphics pdfGraphics)
  {
    if (pdfGraphics.IsDirectPDF)
      return;
    pdfGraphics.IsDirectPDF = true;
    pdfGraphics.NativeGraphics = System.Drawing.Graphics.FromImage((Image) new Bitmap(1, 1));
  }

  private void IntializeHeaderFooter(IWorksheet sheet)
  {
    this._predefinedHeaderFooter.Add("&A", sheet.Name);
    this._predefinedHeaderFooter.Add("&D", DateTime.Now.ToShortDateString());
    this._predefinedHeaderFooter.Add("&T", DateTime.Now.ToShortTimeString());
    if (this._workBookImpl == null)
      return;
    this._predefinedHeaderFooter.Add("&Z", Path.GetDirectoryName(this._workBookImpl.FullFileName));
    this._predefinedHeaderFooter.Add("&F", Path.GetFileNameWithoutExtension(this._workBookImpl.FullFileName));
  }

  private void IntializeHeaderFooter(IChart chart)
  {
    this._predefinedHeaderFooter.Add("&A", chart.Name);
    this._predefinedHeaderFooter.Add("&D", DateTime.Now.ToShortDateString());
    this._predefinedHeaderFooter.Add("&T", DateTime.Now.ToShortTimeString());
    if (this._workBookImpl == null)
      return;
    this._predefinedHeaderFooter.Add("&Z", Path.GetDirectoryName(this._workBookImpl.FullFileName));
    this._predefinedHeaderFooter.Add("&F", Path.GetFileNameWithoutExtension(this._workBookImpl.FullFileName));
  }

  private void IntializeFonts()
  {
    this._fontList.Add("Verdana");
    this._fontList.Add("Times New Roman");
    this._fontList.Add("Microsoft Sans Serif");
    this._fontList.Add("Tahoma");
    this._fontList.Add("Arial");
    this._fontList.Add("SimSun");
    this._fontList.Add("MingLiU");
    this._fontList.Add("Calibri");
    this._excludefontList.Add("Arial");
    this._excludefontList.Add("Arial Unicode MS");
    this._excludefontList.Add("Microsoft Sans Serif");
    this._excludefontList.Add("Segoe UI");
    this._excludefontList.Add("Tahoma");
    this._excludefontList.Add("Times New Roman");
  }

  internal void SystemFonts()
  {
    foreach (FontFamily family in new InstalledFontCollection().Families)
      this.SystemFontsName.Add(family.Name);
  }

  private void IntializeRemovableCharacters()
  {
    this._removableCharaters.Add('\n');
    this._removableCharaters.Add('\r');
  }

  private RectangleF GetAdjacentCells(
    IRange cell,
    RectangleF rect,
    int firstColumn,
    MigrantRangeImpl cell2,
    float originalWidth,
    ref bool lastColumnChanged)
  {
    RectangleF adjacentCells = rect;
    RangeImpl cell1 = cell as RangeImpl;
    WorksheetImpl.TRangeValueType cellType = (cell1.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true);
    switch (cellType)
    {
      case WorksheetImpl.TRangeValueType.Formula:
      case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
      case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
        RangeImpl.UpdateCellValue((object) cell.Worksheet, cell.Column, cell.Row, true);
        cellType = (cell1.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true);
        break;
    }
    cell1.updateCellValue = false;
    if (!cell.IsBlank && cellType != WorksheetImpl.TRangeValueType.Number && cellType != (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula) && !cell.WrapText && cell.VerticalAlignment != ExcelVAlign.VAlignJustify && cell.HorizontalAlignment != ExcelHAlign.HAlignJustify && cell.HorizontalAlignment != ExcelHAlign.HAlignFill && !cell.CellStyle.ShrinkToFit)
    {
      float height = rect.Height;
      int row = cell.Row;
      int itemIndex = cell.Column;
      if (itemIndex == 0)
        itemIndex = 1;
      int lastColumnIndex = itemIndex;
      WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
      int maxColumnCount = worksheet.ParentWorkbook.MaxColumnCount;
      float currentWidth = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
      if (cell.HorizontalAlignment == ExcelHAlign.HAlignLeft)
      {
        float num1 = cell.Borders[ExcelBordersIndex.EdgeLeft].LineStyle == ExcelLineStyle.Double ? 2f : 1f;
        float num2 = this.GetBorderWidth(cell.Borders[ExcelBordersIndex.EdgeLeft]) * 0.5f;
        float num3 = this.GetBorderWidth(cell.Borders[ExcelBordersIndex.EdgeRight]) * 0.5f;
        currentWidth -= num1 + num2 + num3;
      }
      if (!this.IsMerged(cell))
      {
        ExcelHAlign horizontalAlignment = cell.CellStyle.HorizontalAlignment;
        int deltaIndex1 = 1;
        bool flag1 = false;
        string displayText = cell.DisplayText;
        if (displayText != null)
          flag1 = this.CheckUnicode(displayText);
        if (!this._excelToPdfSettings.EnableRTL)
        {
          if (horizontalAlignment == ExcelHAlign.HAlignRight)
            deltaIndex1 = -1;
        }
        else if (horizontalAlignment == ExcelHAlign.HAlignLeft)
          deltaIndex1 = -1;
        else if (!flag1 && !this.CheckForArabicOrHebrew(displayText) && horizontalAlignment == ExcelHAlign.HAlignGeneral)
          return adjacentCells;
        cell2.ResetRowColumn(row, lastColumnIndex + deltaIndex1);
        bool flag2 = cell2.IsBlank;
        if (horizontalAlignment == ExcelHAlign.HAlignCenter)
        {
          cell2.ResetRowColumn(row, lastColumnIndex + 1);
          bool isBlank = cell2.IsBlank;
          cell2.ResetRowColumn(row, lastColumnIndex - 1);
          flag2 = isBlank | cell2.IsBlank;
        }
        if (flag2)
        {
          float requiredWidth = this.Pdf_UnitConverter.ConvertFromPixels(worksheet.MeasureCell(cell, false, false).Width, PdfGraphicsUnit.Point);
          if ((double) requiredWidth > (double) currentWidth)
          {
            float newWidth1;
            float x;
            if (horizontalAlignment != ExcelHAlign.HAlignCenter)
            {
              x = this.GetAdjacentRectangle(cell, firstColumn, cell2, out lastColumnChanged, row, lastColumnIndex, deltaIndex1, maxColumnCount, requiredWidth, currentWidth, worksheet, out newWidth1);
            }
            else
            {
              float newWidth2;
              this.GetAdjacentRectangle(cell, firstColumn, cell2, out lastColumnChanged, row, lastColumnIndex, deltaIndex1, maxColumnCount, currentWidth + (float) (((double) requiredWidth - (double) currentWidth) / 2.0), currentWidth, worksheet, out newWidth2);
              this._rightWidth = newWidth2 - currentWidth;
              int deltaIndex2 = -1;
              x = this.GetAdjacentRectangle(cell, firstColumn, cell2, out lastColumnChanged, row, lastColumnIndex, deltaIndex2, maxColumnCount, currentWidth + (float) (((double) requiredWidth - (double) currentWidth) / 2.0), currentWidth, worksheet, out newWidth2);
              this._leftWidth = newWidth2 - currentWidth;
              newWidth1 = currentWidth + this._leftWidth + this._rightWidth;
            }
            if (this._excelToPdfSettings.EnableRTL)
              x = originalWidth - x - newWidth1;
            adjacentCells = new RectangleF(x, rect.Y, newWidth1, height);
            int extendedFormatIndex = (int) cell1.ExtendedFormatIndex;
            ExtendedFormatImpl innerExtFormat = cell1.Workbook.InnerExtFormats[extendedFormatIndex];
            if (this.GetHorizontalAlignmentFromExtendedFormat((IExtendedFormat) innerExtFormat, (IRange) cell1) == PdfTextAlignment.Left && !this._excelToPdfSettings.EnableRTL || this.GetHorizontalAlignmentFromExtendedFormat((IExtendedFormat) innerExtFormat, (IRange) cell1) == PdfTextAlignment.Right && this._excelToPdfSettings.EnableRTL)
              this._borderRange = cell[cell.Row, cell.Column, cell.Row, cell2.Column - 1];
          }
        }
      }
    }
    return adjacentCells;
  }

  private float GetAdjacentRectangle(
    IRange cell,
    int firstColumn,
    MigrantRangeImpl cell2,
    out bool lastColumnChanged,
    int rowIndex,
    int lastColumnIndex,
    int deltaIndex,
    int lastPossibleColumn,
    float requiredWidth,
    float currentWidth,
    WorksheetImpl sheetImpl,
    out float newWidth)
  {
    lastColumnChanged = false;
    cell2.ResetRowColumn(rowIndex, lastColumnIndex + deltaIndex);
    while (cell2.IsBlank && !this.IsMerged((IRange) cell2) && lastColumnIndex < lastPossibleColumn && lastColumnIndex > 0 && (double) requiredWidth > (double) currentWidth)
    {
      int itemIndex = cell2.Column;
      if (itemIndex == 0)
        itemIndex = 1;
      currentWidth += this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
      lastColumnIndex += deltaIndex;
      cell2.ResetRowColumn(rowIndex, lastColumnIndex + deltaIndex);
    }
    int rowStart = Math.Min(lastColumnIndex, cell.Column);
    if (rowStart == 0)
      rowStart = 1;
    int rowEnd = Math.Max(lastColumnIndex, cell.Column);
    if (rowEnd > sheetImpl.UsedRange.LastColumn)
      lastColumnChanged = true;
    float adjacentRectangle = deltaIndex != -1 || rowStart >= firstColumn ? this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn, rowStart - 1), PdfGraphicsUnit.Point) : -this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowStart, rowStart + firstColumn - rowStart - 1), PdfGraphicsUnit.Point);
    newWidth = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(rowStart, rowEnd), PdfGraphicsUnit.Point);
    return adjacentRectangle;
  }

  protected Dictionary<PointF, SizeF> GetBackgroundHeightCoordinates(
    float startX,
    float startY,
    float imageWidth,
    float imageHeight,
    Dictionary<PointF, SizeF> imageCoordinates,
    PdfPage pdfPage)
  {
    if ((double) pdfPage.Size.Height >= (double) startY)
    {
      if (!imageCoordinates.ContainsKey(new PointF(startX, startY)))
        imageCoordinates.Add(new PointF(startX, startY), new SizeF(imageWidth, imageHeight));
      this.GetBackgroundHeightCoordinates(startX, imageHeight + startY, imageWidth, imageHeight, imageCoordinates, pdfPage);
    }
    return imageCoordinates;
  }

  protected Dictionary<PointF, SizeF> GetBackgroundWidthCoordinates(
    float startX,
    float startY,
    float imageWidth,
    float imageHeight,
    Dictionary<PointF, SizeF> imageCoordinates,
    PdfPage pdfPage)
  {
    if ((double) pdfPage.Size.Width >= (double) startX)
    {
      if (!imageCoordinates.ContainsKey(new PointF(startX, startY)))
        imageCoordinates.Add(new PointF(startX, startY), new SizeF(imageWidth, imageHeight));
      imageCoordinates = this.GetBackgroundHeightCoordinates(startX, startY, imageWidth, imageHeight, imageCoordinates, pdfPage);
      this.GetBackgroundWidthCoordinates(startX + imageWidth, startY, imageWidth, imageHeight, imageCoordinates, pdfPage);
    }
    return imageCoordinates;
  }

  private float GetBorderWidth(IBorder border)
  {
    switch (border.LineStyle)
    {
      case ExcelLineStyle.None:
      case ExcelLineStyle.Hair:
        this._borderWidth = 0.5f;
        break;
      case ExcelLineStyle.Thin:
      case ExcelLineStyle.Dashed:
      case ExcelLineStyle.Dotted:
      case ExcelLineStyle.Double:
      case ExcelLineStyle.Dash_dot:
      case ExcelLineStyle.Dash_dot_dot:
      case ExcelLineStyle.Slanted_dash_dot:
        this._borderWidth = 1f;
        break;
      case ExcelLineStyle.Medium:
      case ExcelLineStyle.Medium_dashed:
      case ExcelLineStyle.Medium_dash_dot:
      case ExcelLineStyle.Medium_dash_dot_dot:
        this._borderWidth = 2f;
        break;
      case ExcelLineStyle.Thick:
        this._borderWidth = 3f;
        break;
    }
    return this._borderWidth;
  }

  internal float GetBorderPriority(ExcelLineStyle lineStyle)
  {
    switch (lineStyle)
    {
      case ExcelLineStyle.None:
        return 0.5f;
      case ExcelLineStyle.Thin:
        return 1.4f;
      case ExcelLineStyle.Medium:
        return 2.4f;
      case ExcelLineStyle.Dashed:
        return 1.3f;
      case ExcelLineStyle.Dotted:
        return 1f;
      case ExcelLineStyle.Thick:
        return 3f;
      case ExcelLineStyle.Double:
        return 3.5f;
      case ExcelLineStyle.Hair:
        return 0.6f;
      case ExcelLineStyle.Medium_dashed:
        return 2.3f;
      case ExcelLineStyle.Dash_dot:
        return 1.2f;
      case ExcelLineStyle.Medium_dash_dot:
        return 2.2f;
      case ExcelLineStyle.Dash_dot_dot:
        return 1.1f;
      case ExcelLineStyle.Medium_dash_dot_dot:
        return 2f;
      case ExcelLineStyle.Slanted_dash_dot:
        return 2.1f;
      default:
        return 0.0f;
    }
  }

  internal double GetBrightness(Color color)
  {
    return 0.299 * (double) color.R + 0.587 * (double) color.G + 0.114 * (double) color.B;
  }

  private string GetBottomText(string value, int rotationAngle)
  {
    if (rotationAngle == (int) byte.MaxValue)
    {
      StringBuilder stringBuilder = new StringBuilder(value);
      int num = 0;
      int index = 1;
      int length = value.Length;
      while (num < length)
      {
        stringBuilder.Insert(index, '\n');
        ++num;
        index += 2;
      }
      value = stringBuilder.ToString();
    }
    return value;
  }

  private PdfBrush GetBrush(IInternalExtendedFormat internalExtendedFormat)
  {
    return internalExtendedFormat.FillPattern != ExcelPattern.Solid ? (PdfBrush) new PdfSolidBrush(new PdfColor(this.NormalizeColor(internalExtendedFormat.Color))) : (PdfBrush) new PdfSolidBrush(new PdfColor(this.NormalizeColor(internalExtendedFormat.Color)));
  }

  private int GetCounterClockwiseRotation(int rotationAngle)
  {
    if (rotationAngle > 90)
      rotationAngle -= 90;
    else
      rotationAngle = -rotationAngle;
    return rotationAngle;
  }

  private PdfDashStyle GetDashStyle(IBorder border)
  {
    PdfDashStyle dashStyle = PdfDashStyle.Solid;
    switch (border.LineStyle)
    {
      case ExcelLineStyle.Thin:
      case ExcelLineStyle.Medium:
      case ExcelLineStyle.Thick:
      case ExcelLineStyle.Double:
      case ExcelLineStyle.Hair:
        dashStyle = PdfDashStyle.Solid;
        break;
      case ExcelLineStyle.Dashed:
      case ExcelLineStyle.Medium_dashed:
        dashStyle = PdfDashStyle.Dash;
        break;
      case ExcelLineStyle.Dotted:
        dashStyle = PdfDashStyle.Dot;
        break;
      case ExcelLineStyle.Dash_dot:
      case ExcelLineStyle.Medium_dash_dot:
      case ExcelLineStyle.Slanted_dash_dot:
        dashStyle = PdfDashStyle.DashDot;
        break;
      case ExcelLineStyle.Dash_dot_dot:
      case ExcelLineStyle.Medium_dash_dot_dot:
        dashStyle = PdfDashStyle.DashDotDot;
        break;
    }
    return dashStyle;
  }

  private bool CheckIndex(int index, string text, string splitValue, string value)
  {
    bool flag = false;
    if (index >= text.Length)
    {
      if (splitValue.IndexOf(value) + text.Length == index)
        flag = true;
    }
    else if (index <= splitValue.Length && text.IndexOf(value) == index)
      flag = true;
    return flag;
  }

  private void ClearKeywords(
    ref string splitValue,
    ref string text,
    Dictionary<int, string> keywords,
    string sourceString)
  {
    foreach (KeyValuePair<int, string> keyword in keywords)
    {
      if (splitValue.Contains(keyword.Value) && keyword.Key >= text.Length)
        splitValue = splitValue.Remove(splitValue.IndexOf(keyword.Value));
      else if (text.Contains(keyword.Value) && keyword.Key <= splitValue.Length)
        text = text.Remove(text.IndexOf(keyword.Value));
    }
  }

  private string GetDisplayText(
    PdfFont font,
    PdfStringFormat format,
    string value,
    RectangleF cellRect,
    bool isRTL)
  {
    if ((double) font.MeasureString(value, format).Width <= (double) cellRect.Width)
      return value;
    string text = "#";
    float width = cellRect.Width;
    List<char> charList = new List<char>();
    charList.AddRange((IEnumerable<char>) value.ToCharArray());
    if (!isRTL)
    {
      for (int index = charList.Count - 1; index > 0; --index)
      {
        charList.RemoveAt(index);
        text = new string(charList.ToArray());
        if ((double) font.MeasureString(text, format).Width <= (double) width)
          break;
      }
    }
    else
    {
      for (int index = 0; index < charList.Count - 1; ++index)
      {
        charList.RemoveAt(index);
        text = new string(charList.ToArray());
        if ((double) font.MeasureString(text, format).Width <= (double) width)
          break;
      }
    }
    charList.Clear();
    return text;
  }

  private string GetDisplayText(
    PdfFont font,
    PdfStringFormat format,
    string value,
    RectangleF cellRect)
  {
    if ((double) font.MeasureString(value, format).Height <= (double) cellRect.Height)
      return value;
    string text1 = string.Empty;
    string text2 = value;
    int num = 0;
    for (int length1 = text2.Length; num < length1; ++num)
    {
      if ((double) font.MeasureString(text1, format).Height > (double) cellRect.Height)
      {
        for (int length2 = text1.Length - 1; length2 > 0; --length2)
        {
          text1 = text1.Substring(0, length2);
          if ((double) font.MeasureString(text1, format).Height <= (double) cellRect.Height)
            return text1;
        }
      }
      if (text2[num].Equals('\n'))
      {
        string text3 = text2.Substring(0, num);
        if ((double) font.MeasureString(text3, format).Height <= (double) cellRect.Height)
        {
          text1 += text3;
          text2 = text2.Substring(num);
          length1 = text2.Length;
          num = 0;
        }
        else
        {
          text1 += text3;
          break;
        }
      }
      else if (num == length1 - 1)
        text1 = (double) font.MeasureString(text2, format).Height > (double) cellRect.Height ? text2 : text1 + text2;
    }
    return text1;
  }

  private List<string> GetDisplayWrapTextList(
    PdfFont pdfFont,
    PdfStringFormat pdfFormat,
    string cellTextValue,
    RectangleF cellRect)
  {
    List<string> splitTextList = new List<string>();
    if ((double) pdfFont.MeasureString(cellTextValue, pdfFormat).Width <= (double) cellRect.Width)
    {
      splitTextList.Add(cellTextValue);
      return splitTextList;
    }
    if (pdfFormat.WordWrap != PdfWordWrapType.None)
      pdfFormat.WordWrap = PdfWordWrapType.Character;
    return this.GetDisplayWrapTextList(pdfFont, pdfFormat, cellTextValue, cellRect, splitTextList);
  }

  private List<string> GetDisplayWrapTextList(
    PdfFont pdfFont,
    PdfStringFormat pdfFormat,
    string cellTextValue,
    RectangleF cellRect,
    List<string> splitTextList)
  {
    string str1 = "#";
    List<string> stringList = new List<string>();
    List<char> charList = new List<char>();
    charList.AddRange((IEnumerable<char>) cellTextValue.ToCharArray());
    if ((double) pdfFont.MeasureString(cellTextValue, pdfFormat).Width <= (double) cellRect.Width)
    {
      if (splitTextList.Count != 0)
        splitTextList.Add("\r\n");
      splitTextList.Add(cellTextValue);
    }
    else
    {
      for (int index = charList.Count - 1; index >= 0; --index)
      {
        if (charList.Count > 1)
        {
          stringList.Add(charList[index].ToString());
          charList.RemoveAt(index);
        }
        string text = new string(charList.ToArray());
        if ((double) pdfFont.MeasureString(text, pdfFormat).Width <= (double) cellRect.Height)
        {
          if (splitTextList.Count != 0)
            splitTextList.Add("\r\n");
          splitTextList.Add(text);
          break;
        }
      }
    }
    if (stringList.Count != 0)
    {
      stringList.Reverse();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str2 in stringList)
        stringBuilder.Append(str2);
    }
    charList.Clear();
    stringList.Clear();
    str1 = (string) null;
    return splitTextList;
  }

  private Font GetFont(IFont font, string fontName, int size)
  {
    Font font1 = new Font(fontName, (float) size);
    FontStyle fontStyle1 = !font.Bold ? FontStyle.Regular : FontStyle.Bold;
    FontStyle fontStyle2 = FontStyle.Regular;
    if (font.Italic)
      fontStyle2 = FontStyle.Italic;
    FontStyle fontStyle3 = FontStyle.Regular;
    if (font.Underline.ToString() == "Single")
      fontStyle3 = FontStyle.Underline;
    return new Font(fontName, (float) size, fontStyle1 | fontStyle2 | fontStyle3);
  }

  internal Font GetFont(
    string name,
    int size,
    bool hasUnderline,
    bool hasStrikeThrough,
    Font fontSettings)
  {
    FontStyle fontStyle1 = (fontSettings.Style & FontStyle.Bold) != FontStyle.Regular ? FontStyle.Bold : FontStyle.Regular;
    FontStyle fontStyle2 = (fontSettings.Style & FontStyle.Italic) != FontStyle.Regular ? FontStyle.Italic : FontStyle.Regular;
    FontStyle fontStyle3 = hasUnderline ? FontStyle.Underline : FontStyle.Regular;
    FontStyle fontStyle4 = hasStrikeThrough ? FontStyle.Strikeout : FontStyle.Regular;
    Font font = new Font("Calibri", 12f);
    string familyName = "Calibri";
    if (name[0] == '"')
    {
      string[] strArray1 = name.Split('"')[1].Split(',');
      if (strArray1.Length > 1)
      {
        if (strArray1[0] != "-")
          familyName = strArray1[0];
        else if (strArray1[0] == "-")
        {
          fontStyle1 = FontStyle.Regular;
          fontStyle2 = FontStyle.Regular;
          fontStyle3 = FontStyle.Regular;
        }
        string[] strArray2 = strArray1[1].Split(' ');
        for (int index = 0; index < strArray2.Length; ++index)
        {
          if (strArray2[index] == "Bold")
            fontStyle1 = FontStyle.Bold;
          if (strArray2[index] == "Italic")
            fontStyle2 = FontStyle.Italic;
          if (strArray2[index] == "Regular")
            fontStyle1 = FontStyle.Regular;
        }
      }
    }
    else
      familyName = name;
    return new Font(familyName, (float) size, fontStyle1 | fontStyle2 | fontStyle3 | fontStyle4);
  }

  private Font GetFont(string name, int size)
  {
    FontStyle fontStyle1 = FontStyle.Regular;
    FontStyle fontStyle2 = FontStyle.Regular;
    Font font = new Font("Calibri", 12f);
    string familyName = "Calibri";
    if (name[0] == '"')
    {
      string[] strArray1 = name.Split('"')[1].Split(',');
      if (strArray1.Length > 1)
      {
        if (strArray1[0] != "-")
          familyName = strArray1[0];
        string[] strArray2 = strArray1[1].Split(' ');
        for (int index = 0; index < strArray2.Length; ++index)
        {
          if (strArray2[index] == "Bold")
            fontStyle1 = FontStyle.Bold;
          if (strArray2[index] == "Italic")
            fontStyle2 = FontStyle.Italic;
        }
      }
    }
    return new Font(familyName, (float) size, fontStyle1 | fontStyle2);
  }

  private void GetHeaderFooterInformation(
    HeaderFooterSection hfsection,
    List<HeaderFooter> headerFooterCollection,
    PdfSection pdfSection,
    IPageSetupBase pageSetupBase,
    Dictionary<string, HeaderFooterFontColorSettings> fontColorSettings,
    PdfTemplate pdfTemplate,
    float width,
    float height,
    string align,
    string name,
    string pageNumber)
  {
    this._pdfTextformat = new PdfStringFormat();
    this._pdfTextformat.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
    Image image = (Image) null;
    int imageWidth = 0;
    int imageHeight = 0;
    switch (align)
    {
      case "Left":
        this._pdfTextformat.Alignment = PdfTextAlignment.Left;
        break;
      case "Center":
        this._pdfTextformat.Alignment = PdfTextAlignment.Center;
        break;
      case "Right":
        this._pdfTextformat.Alignment = PdfTextAlignment.Right;
        break;
      default:
        this._pdfTextformat.Alignment = PdfTextAlignment.Justify;
        break;
    }
    if (name == "Footer")
      this._pdfTextformat.LineAlignment = PdfVerticalAlignment.Bottom;
    RichTextString richTextString = new RichTextString(this._workBook.Application, (pageSetupBase as PageSetupBaseImpl).FindParent(typeof (WorksheetImpl)) ?? pageSetupBase.Parent, false, true);
    if (hfsection.RTF != null)
    {
      for (int index1 = 0; index1 < hfsection.RTF.Count; ++index1)
      {
        RichTextString rtfString = hfsection.RTF[index1];
        string text1 = rtfString.Text;
        if (text1.Contains("&G"))
        {
          image = this.GetHeaderFooterImages(pageSetupBase, ref width, ref height, pdfTemplate, align, name, pdfSection);
          this.GetTemplateHeight(pageSetupBase, name, align, out imageHeight, out imageWidth);
          if (image != null)
          {
            string text2 = $"HeaderFooterImage:{(object) imageWidth}:{(object) imageHeight}";
            richTextString.AddText(text2, rtfString.GetFont(0));
          }
        }
        else if (text1.Contains("&P"))
        {
          string stringContent = pageNumber;
          this.UpdateHeaderFooterText(hfsection, stringContent, rtfString, richTextString, ref index1);
        }
        else if (text1.Contains("&N"))
        {
          int num = 0;
          for (int index2 = 0; index2 < this._pdfDocument.Sections.Count; ++index2)
            num += this._pdfDocument.Sections[index2].Pages.Count;
          string stringContent = num.ToString();
          this.UpdateHeaderFooterText(hfsection, stringContent, rtfString, richTextString, ref index1);
        }
        else
          richTextString.AddText(text1, rtfString.GetFont(0));
      }
    }
    string text = richTextString.Text;
    if (text == null || !(text != string.Empty))
      return;
    this._isHfrtfProcess = true;
    List<IFont> richTextFonts = new List<IFont>();
    List<string> drawString = this._workBookImpl.GetDrawString(richTextString.Text, richTextString, out richTextFonts, richTextString.GetFont(0));
    this.DrawRTFText(new RectangleF(0.0f, 0.0f, pdfTemplate.Width, pdfTemplate.Height), new RectangleF(0.0f, 0.0f, pdfTemplate.Width, pdfTemplate.Height), pdfTemplate.Graphics, richTextFonts, drawString, false, true, false, false, true);
    if (image != null && this._hfImageBounds != RectangleF.Empty)
    {
      PdfImage pdfImage = this.GetPdfImage(image);
      pdfTemplate.Graphics.DrawImage(pdfImage, this._hfImageBounds);
      this._hfImageBounds = RectangleF.Empty;
    }
    this._isHfrtfProcess = false;
  }

  private void UpdateHeaderFooterText(
    HeaderFooterSection hfsection,
    string stringContent,
    RichTextString rtfString,
    RichTextString richTextHFString,
    ref int index)
  {
    if (index + 1 < hfsection.RTF.Count && hfsection.RTF[index + 1].Text[0].Equals('+'))
    {
      string text1 = hfsection.RTF[index + 1].Text;
      if (char.IsDigit(hfsection.RTF[index + 1].Text[1]))
      {
        string s = (string) null;
        for (int index1 = 1; index1 < text1.Length && char.IsDigit(text1[index1]); ++index1)
          s += (string) (object) text1[index1];
        if (s.Length + 1 < text1.Length)
        {
          stringContent = (XmlConvert.ToInt32(stringContent) + XmlConvert.ToInt32(s)).ToString();
          richTextHFString.AddText(stringContent, rtfString.GetFont(0));
          string text2 = text1.Replace("+" + s, "");
          if (!string.IsNullOrEmpty(text2))
            richTextHFString.AddText(text2, hfsection.RTF[index + 1].GetFont(0));
          ++index;
        }
        else
        {
          richTextHFString.AddText(stringContent, rtfString.GetFont(0));
          string text3 = text1.Replace("+", "");
          if (!string.IsNullOrEmpty(text3))
            richTextHFString.AddText(text3, hfsection.RTF[index + 1].GetFont(0));
          ++index;
        }
      }
      else
      {
        if (!char.IsLetter(hfsection.RTF[index + 1].Text[1]))
          return;
        richTextHFString.AddText(stringContent, rtfString.GetFont(0));
        string text4 = text1.Replace("+", "");
        if (!string.IsNullOrEmpty(text4))
          richTextHFString.AddText(text4, hfsection.RTF[index + 1].GetFont(0));
        ++index;
      }
    }
    else if (index + 1 < hfsection.RTF.Count && hfsection.RTF[index + 1].Text[0].Equals('-'))
    {
      string text5 = hfsection.RTF[index + 1].Text;
      if (char.IsDigit(hfsection.RTF[index + 1].Text[1]))
      {
        string s = (string) null;
        for (int index2 = 1; index2 < text5.Length && char.IsDigit(text5[index2]); ++index2)
          s += (string) (object) text5[index2];
        if (s.Length + 1 < text5.Length)
        {
          stringContent = (XmlConvert.ToInt32(stringContent) - XmlConvert.ToInt32(s)).ToString();
          richTextHFString.AddText(stringContent, rtfString.GetFont(0));
          string text6 = text5.Replace("-" + s, "");
          if (!string.IsNullOrEmpty(text6))
            richTextHFString.AddText(text6, hfsection.RTF[index + 1].GetFont(0));
          ++index;
        }
        else
        {
          richTextHFString.AddText(stringContent, rtfString.GetFont(0));
          string text7 = text5.Replace("-", "");
          if (!string.IsNullOrEmpty(text7))
            richTextHFString.AddText(text7, hfsection.RTF[index + 1].GetFont(0));
          ++index;
        }
      }
      else
      {
        if (!char.IsLetter(hfsection.RTF[index + 1].Text[1]))
          return;
        richTextHFString.AddText(stringContent, rtfString.GetFont(0));
        string text8 = text5.Replace("-", "");
        if (!string.IsNullOrEmpty(text8))
          richTextHFString.AddText(text8, hfsection.RTF[index + 1].GetFont(0));
        ++index;
      }
    }
    else
      richTextHFString.AddText(stringContent, rtfString.GetFont(0));
  }

  private List<RichTextString> GetHeaderInformation(
    IPageSetupBase pageSetupBase,
    Dictionary<string, HeaderFooterFontColorSettings> fontColorSettings,
    string align,
    string name,
    out float height,
    out float width,
    float pageWidth)
  {
    Font font1 = new Font("Times New Roman", 12f);
    RichTextString richTextString = new RichTextString(this._workBook.Application, (object) this._workSheet ?? pageSetupBase.Parent, false, true);
    List<SizeF> sizes = new List<SizeF>();
    List<RichTextString> headerInformation = new List<RichTextString>();
    foreach (KeyValuePair<string, HeaderFooterFontColorSettings> fontColorSetting in fontColorSettings)
    {
      int imageHeight = 0;
      int imageWidth = 0;
      RichTextString richTextSection = new RichTextString(this._workBook.Application, (object) this._workSheet ?? pageSetupBase.Parent, false, true);
      string text = fontColorSetting.Key.Remove(fontColorSetting.Key.LastIndexOf('|'), fontColorSetting.Key.Length - fontColorSetting.Key.LastIndexOf('|'));
      if (!text.Equals(string.Empty))
      {
        if (text == "&G")
          this.GetTemplateHeight(pageSetupBase, name, align, out imageHeight, out imageWidth);
        Font nativeFont = fontColorSetting.Value == null ? new Font(this._workBook.StandardFont, (float) this._workBook.StandardFontSize) : fontColorSetting.Value.Font;
        IFont font2 = (IFont) new FontImpl(this._workBook.Application, (object) this._workSheet ?? pageSetupBase.Parent, nativeFont);
        if (fontColorSetting.Value != null)
          font2.RGBColor = fontColorSetting.Value.FontColor;
        if (fontColorSetting.Value != null && fontColorSetting.Value.HasSuperscript)
          font2.Superscript = true;
        if (fontColorSetting.Value != null && fontColorSetting.Value.HasSubscript)
          font2.Subscript = true;
        richTextString.AddText(text, font2);
        richTextSection.AddText(text, font2);
        SizeF sizeF = new SizeF(0.0f, 0.0f);
        int iStartPos = 0;
        int iIndex = 0;
        SizeF sizePart;
        for (int formattingRunsCount = richTextSection.TextObject.FormattingRunsCount; iIndex < formattingRunsCount; ++iIndex)
        {
          int positionByIndex = richTextSection.TextObject.GetPositionByIndex(iIndex);
          sizePart = this.GetSizePart(richTextSection, iStartPos, positionByIndex);
          sizeF.Width += sizePart.Width;
          sizeF.Height = Math.Max(sizePart.Height, sizeF.Height);
          iStartPos = positionByIndex;
        }
        sizePart = this.GetSizePart(richTextSection, iStartPos, richTextSection.Text.Length);
        sizeF.Width += sizePart.Width;
        sizeF.Height = Math.Max(sizePart.Height, sizeF.Height);
        if (imageWidth != 0)
          sizeF.Width = (float) imageWidth;
        if (imageHeight != 0)
          sizeF.Height = (float) imageHeight;
        sizes.Add(sizeF);
        headerInformation.Add(richTextSection);
      }
    }
    SizeF maxWidth = this.GetMaxWidth(sizes);
    width = maxWidth.Width;
    height = maxWidth.Height;
    return headerInformation;
  }

  private SizeF GetSizePart(RichTextString richTextSection, int iStartPos, int iEndPos)
  {
    if (iStartPos >= iEndPos)
      return new SizeF(0.0f, 0.0f);
    IFont font = richTextSection.GetFont(iStartPos);
    int length = iEndPos - iStartPos;
    string text = richTextSection.Text.Substring(iStartPos, length);
    if (richTextSection.CellIndex != 0L && richTextSection.Parent is WorksheetImpl)
    {
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(richTextSection.CellIndex);
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(richTextSection.CellIndex);
      if (!(richTextSection.Parent as WorksheetImpl)[rowFromCellIndex, columnFromCellIndex].WrapText)
        text = text.Replace("\r\n", string.Empty);
    }
    return new SizeF(this.MeasureString(text, font).Width, this.MeasureString("a", font).Height * (float) (text.Length - text.Replace("\n", "").Length + 1));
  }

  private SizeF MeasureString(string text, IFont font)
  {
    bool isEmbedFont = this._excelToPdfSettings.EmbedFonts || Encoding.UTF8.GetByteCount(text) != text.Length;
    return this.GetPdfFont(this.GetFont(font, font.FontName, (int) font.Size), isEmbedFont, (Stream) null).MeasureString(text, new PdfStringFormat()
    {
      MeasureTrailingSpaces = true
    });
  }

  private List<HeaderFooterSection> GetHeaderFooterOptions(
    Dictionary<string, string> pageSetups,
    PdfSection pdfSection,
    IPageSetupBase pageSetupBase,
    float templateWidth,
    string name,
    string sheetOrchartname)
  {
    if (pdfSection.Template.Top != null && name == "Header" || pdfSection.Template.Bottom != null && name == "Footer")
      return (List<HeaderFooterSection>) null;
    List<HeaderFooterSection> headerFooterOptions = new List<HeaderFooterSection>();
    IFormatProvider invariantCulture = (IFormatProvider) CultureInfo.InvariantCulture;
    HeaderFooterSection headerFooterSection = (HeaderFooterSection) null;
    float width = 0.0f;
    bool isControlRemoved = false;
    bool flag1 = false;
    bool flag2 = false;
    foreach (KeyValuePair<string, string> pageSetup in pageSetups)
    {
      if (pageSetup.Key == "L")
      {
        headerFooterSection = new HeaderFooterSection();
        width = 0.0f;
        headerFooterSection.Width = width;
        headerFooterSection.SectionName = "Left";
        headerFooterSection.TextAlignment = PdfTextAlignment.Left;
      }
      else if (pageSetup.Key == "C")
      {
        headerFooterSection = new HeaderFooterSection();
        width = templateWidth / 2f;
        headerFooterSection.Width = width;
        headerFooterSection.SectionName = "Center";
        headerFooterSection.TextAlignment = PdfTextAlignment.Center;
      }
      else if (pageSetup.Key == "R")
      {
        headerFooterSection = new HeaderFooterSection();
        width = templateWidth;
        headerFooterSection.Width = width;
        headerFooterSection.SectionName = "Right";
        headerFooterSection.TextAlignment = PdfTextAlignment.Right;
      }
      HeaderFooterFontColorSettings hfFontColorSettings = (HeaderFooterFontColorSettings) null;
      string[] splitted = this.GetSplitted(pageSetup.Value.Replace("\r", "\r").Replace("_x0009_", "\t").Replace("\n", "\n"));
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) splitted);
      Dictionary<string, HeaderFooterFontColorSettings> fontColorSettings = new Dictionary<string, HeaderFooterFontColorSettings>();
      float height = 0.0f;
      int num1 = 0;
      int num2 = 0;
      if (stringList.Count > 0)
      {
        foreach (string str in stringList)
        {
          switch (str)
          {
            case "&X":
              ++num1;
              break;
          }
          if (str != null && str.Equals("&Y"))
            ++num2;
        }
      }
      while (stringList.Count != 0)
      {
        if (stringList[0].StartsWith("&G", StringComparison.Ordinal))
        {
          fontColorSettings.Add(string.Format(invariantCulture, "&G|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[stringList.IndexOf("&G")]);
          flag2 = true;
          this._allowHeaderFooterOnce = false;
        }
        else if (stringList[0].StartsWith("&A", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          fontColorSettings.Add(string.Format(invariantCulture, sheetOrchartname + "|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&P", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          fontColorSettings.Add(string.Format(invariantCulture, "&P|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&N", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          fontColorSettings.Add(string.Format(invariantCulture, "&N|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
          this._allowHeaderFooterOnce = this._excelToPdfSettings.RenderBySheet && !flag2;
          headerFooterSection.IsPageCount = true;
          switch (name)
          {
            case "Header":
              this._isHeaderPageCount = true;
              break;
            case "Footer":
              this._isFooterPageCount = true;
              break;
          }
          flag1 = true;
        }
        else if (stringList[0].StartsWith("&D", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          fontColorSettings.Add(string.Format(invariantCulture, DateTime.Now.ToShortDateString() + "|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&T", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          fontColorSettings.Add(string.Format(invariantCulture, DateTime.Now.ToShortTimeString() + "|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&U", StringComparison.Ordinal))
        {
          stringList.Remove(stringList[0]);
          if (hfFontColorSettings == null)
          {
            string[] headerFooterValues = new string[1]
            {
              "\"+,Regular\""
            };
            hfFontColorSettings = this.GetHeaderFooterValues(headerFooterValues, headerFooterValues.Length, hfFontColorSettings, false);
          }
          hfFontColorSettings.Font = this.GetFont(hfFontColorSettings.Font.Name, (int) hfFontColorSettings.Font.Size, true, hfFontColorSettings.HasStrikeThrough, hfFontColorSettings.Font);
          hfFontColorSettings.HasUnderline = true;
        }
        else if (stringList[0].StartsWith("&S", StringComparison.Ordinal))
        {
          stringList.Remove(stringList[0]);
          if (hfFontColorSettings == null)
          {
            string[] headerFooterValues = new string[1]
            {
              "\"+,Regular\""
            };
            hfFontColorSettings = this.GetHeaderFooterValues(headerFooterValues, headerFooterValues.Length, hfFontColorSettings, false);
          }
          hfFontColorSettings.Font = this.GetFont(hfFontColorSettings.Font.Name, (int) hfFontColorSettings.Font.Size, hfFontColorSettings.HasUnderline, true, hfFontColorSettings.Font);
          hfFontColorSettings.HasStrikeThrough = true;
        }
        else if (stringList[0].StartsWith("&Z", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          if (this._workBookImpl.FullFileName != null)
            fontColorSettings.Add(string.Format(invariantCulture, Path.GetDirectoryName(this._workBookImpl.FullFileName) + "|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&F", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          if (this._workBookImpl.FullFileName != null)
            fontColorSettings.Add(string.Format(invariantCulture, Path.GetFileNameWithoutExtension(this._workBookImpl.FullFileName) + "|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&K", StringComparison.Ordinal))
        {
          stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
          string[] strArray = new string[1];
          stringList.CopyTo(0, strArray, 0, 1);
          hfFontColorSettings = this.GetHeaderFooterValues(strArray, strArray.Length, hfFontColorSettings, true);
          stringList.Remove(stringList[0]);
          if (!this._isfontColorModified)
            stringList.Insert(0, strArray[0].Substring(1, strArray[0].Length - 1));
        }
        else if (this.CheckFontValues(stringList[0]))
        {
          string[] strArray = new string[1];
          stringList.CopyTo(0, strArray, 0, 1);
          hfFontColorSettings = this.GetHeaderFooterValues(strArray, strArray.Length, hfFontColorSettings, false);
          stringList.Remove(stringList[0]);
        }
        else if (stringList[0].StartsWith("&S", StringComparison.Ordinal))
          stringList.Remove(stringList[0]);
        else if (stringList[0].StartsWith("&X", StringComparison.Ordinal))
        {
          stringList.Remove(stringList[0]);
          if (hfFontColorSettings == null)
          {
            string[] headerFooterValues = new string[1]
            {
              "\"+,Regular\""
            };
            hfFontColorSettings = this.GetHeaderFooterValues(headerFooterValues, headerFooterValues.Length, hfFontColorSettings, false);
          }
          if (num1 == 1)
            hfFontColorSettings.HasSuperscript = true;
        }
        else if (stringList[0].StartsWith("&Y", StringComparison.Ordinal))
        {
          stringList.Remove(stringList[0]);
          if (hfFontColorSettings == null)
          {
            string[] headerFooterValues = new string[1]
            {
              "\"+,Regular\""
            };
            hfFontColorSettings = this.GetHeaderFooterValues(headerFooterValues, headerFooterValues.Length, hfFontColorSettings, false);
          }
          if (num2 == 1)
            hfFontColorSettings.HasSubscript = true;
        }
        else if (stringList[0].StartsWith("&U", StringComparison.Ordinal))
          stringList.Remove(stringList[0]);
        else if (stringList[0].StartsWith("&E", StringComparison.Ordinal))
          stringList.Remove(stringList[0]);
        else if (stringList[0].StartsWith("&O", StringComparison.Ordinal))
          stringList.Remove(stringList[0]);
        else if (stringList[0].StartsWith("&H", StringComparison.Ordinal))
          stringList.Remove(stringList[0]);
        else if (stringList[0].Length != 0 && stringList[0] != string.Empty)
        {
          if (stringList[0].StartsWith("&\"", StringComparison.Ordinal))
          {
            string empty = string.Empty;
            if (stringList[0].StartsWith("&", StringComparison.Ordinal))
              stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
            string[] strArray = new string[1];
            stringList.CopyTo(0, strArray, 0, 1);
            hfFontColorSettings = this.GetHeaderFooterValues(strArray, strArray.Length, hfFontColorSettings, false);
            stringList.RemoveRange(0, 1);
          }
          else if (stringList[0].StartsWith("&", StringComparison.Ordinal) && stringList[0].Length >= 2 && char.IsDigit(stringList[0][1]))
          {
            string empty = string.Empty;
            if (!string.IsNullOrEmpty(stringList[0]))
            {
              string[] strArray = new string[1];
              if (stringList[0].StartsWith("&", StringComparison.Ordinal) && !this.CheckFontValues(stringList[0]))
                stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
              stringList.CopyTo(0, strArray, 0, 1);
              hfFontColorSettings = this.GetHeaderFooterValues(strArray, strArray.Length, hfFontColorSettings, false);
              stringList.RemoveRange(0, 1);
            }
          }
          else if (!string.IsNullOrEmpty(stringList[0]))
          {
            if (stringList[0].StartsWith("&", StringComparison.Ordinal))
            {
              stringList[0] = stringList[0].Remove(stringList[0].IndexOf('&'), 1);
              isControlRemoved = true;
            }
            if (stringList[0].Length > 0)
            {
              string[] strArray = new string[1];
              stringList.CopyTo(0, strArray, 0, 1);
              hfFontColorSettings = this.GetHeaderFooterValues(strArray, strArray.Length, hfFontColorSettings, isControlRemoved);
              stringList[0] = this.FormatHeaderFooterString(stringList[0]);
              fontColorSettings.Add(string.Format(invariantCulture, stringList[0] + "|{0}", (object) Guid.NewGuid()), hfFontColorSettings);
            }
            stringList.Remove(stringList[0]);
          }
        }
      }
      if (this.CheckIsRich(splitted) && !flag1)
        this._allowHeaderFooterOnce = false;
      if (fontColorSettings.Count != 0)
      {
        headerFooterSection.HeaderFooterCollections = fontColorSettings;
        headerFooterSection.RTF = this.GetHeaderInformation(pageSetupBase, fontColorSettings, headerFooterSection.SectionName, name, out height, out width, width);
        headerFooterSection.Height = height;
        headerFooterSection.Width = width;
        if (headerFooterSection.RTF != null && headerFooterSection.RTF.Count > 1 && !flag1)
          this._allowHeaderFooterOnce = false;
      }
      headerFooterOptions.Add(headerFooterSection);
    }
    return headerFooterOptions;
  }

  private string FormatHeaderFooterString(string headerFooterString)
  {
    StringBuilder stringBuilder = new StringBuilder(headerFooterString);
    stringBuilder.Replace("{", "{{").Replace("}", "}}");
    return stringBuilder.ToString();
  }

  private HeaderFooterFontColorSettings GetHeaderFooterValues(
    string[] headerFooterValues,
    int count,
    HeaderFooterFontColorSettings hfFontColorSettings,
    bool isControlRemoved)
  {
    HeaderFooterFontColorSettings headerFooterValues1 = hfFontColorSettings != null ? hfFontColorSettings.Clone() as HeaderFooterFontColorSettings : (HeaderFooterFontColorSettings) null;
    bool flag = headerFooterValues1 == null;
    if (headerFooterValues1 == null)
      headerFooterValues1 = new HeaderFooterFontColorSettings();
    IFormatProvider invariantCulture = (IFormatProvider) CultureInfo.InvariantCulture;
    double num1 = this._workBook != null ? this._workBook.StandardFontSize : this._workSheet.Workbook.StandardFontSize;
    string familyName = this._workBook != null ? this._workBook.StandardFont : this._workSheet.Workbook.StandardFont;
    int num2 = flag ? System.Convert.ToInt32(num1) : System.Convert.ToInt32(headerFooterValues1.Font.Size);
    Font fontSettings = flag ? new Font(familyName, (float) num2) : new Font(headerFooterValues1.Font.Name, (float) num2, headerFooterValues1.Font.Style);
    string name = flag ? familyName : headerFooterValues1.Font.Name;
    Color color = flag ? Color.FromArgb((int) byte.MaxValue, 0, 0, 0) : headerFooterValues1.FontColor;
    for (int index1 = 0; index1 < count; ++index1)
    {
      if (headerFooterValues[index1].Length >= 1 && headerFooterValues[index1].Length <= 2 && this.CheckDigit(headerFooterValues[index1]))
      {
        for (int index2 = 0; index2 < headerFooterValues[index1].Length; ++index2)
        {
          if (char.IsDigit(headerFooterValues[index1][index2]))
          {
            if (headerFooterValues[index1].Length != 1 && !headerFooterValues[index1].StartsWith("&", StringComparison.Ordinal))
            {
              if (char.IsDigit(headerFooterValues[index1][index2 + 1]))
              {
                num2 = int.Parse(new string(new char[2]
                {
                  headerFooterValues[index1][index2],
                  headerFooterValues[index1][index2 + 1]
                }), NumberStyles.Any, invariantCulture);
                break;
              }
            }
            else
            {
              num2 = int.Parse(headerFooterValues[index1][index2].ToString(), NumberStyles.Any, invariantCulture);
              break;
            }
          }
        }
      }
      if (isControlRemoved && headerFooterValues[index1].Substring(0, 1) == "K")
        color = this.GetHeaderFooterColor(headerFooterValues[index1]);
      else if (headerFooterValues[index1].IndexOf('"') == 0)
        name = headerFooterValues[index1];
      else if (headerFooterValues[index1].StartsWith("&", StringComparison.Ordinal))
      {
        if (headerFooterValues[index1].Equals("&u") || headerFooterValues[index1].Equals("&U"))
          headerFooterValues1.HasUnderline = true;
        if (headerFooterValues[index1].Equals("&s") || headerFooterValues[index1].Equals("&S"))
          headerFooterValues1.HasStrikeThrough = true;
        if (headerFooterValues[index1].Equals("&b") || headerFooterValues[index1].Equals("&B"))
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Insert(0, '"');
          stringBuilder.Append("Bold,");
          name = stringBuilder.ToString();
        }
        if (headerFooterValues[index1].Equals("&i") || headerFooterValues[index1].Equals("&I"))
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Insert(0, '"');
          stringBuilder.Append("Italic,");
          name = stringBuilder.ToString();
        }
      }
      else
      {
        string headerFooterValue = headerFooterValues[index1];
      }
    }
    if (name != null)
    {
      bool hasUnderline = hfFontColorSettings != null && hfFontColorSettings.HasUnderline;
      bool hasStrikeThrough = hfFontColorSettings != null && hfFontColorSettings.HasStrikeThrough;
      fontSettings = this.GetFont(name, num2, hasUnderline, hasStrikeThrough, fontSettings);
    }
    headerFooterValues1.Font = fontSettings;
    headerFooterValues1.FontColor = color;
    return headerFooterValues1;
  }

  private Color GetHeaderFooterColor(string colorValue)
  {
    Color headerFooterColor = new Color();
    bool flag1 = false;
    bool flag2 = false;
    IFormatProvider invariantCulture = (IFormatProvider) CultureInfo.InvariantCulture;
    if (colorValue.Substring(1, 6).Length > 1 && colorValue.Length < 8)
    {
      if (colorValue.Contains("+") || colorValue.Contains("-"))
      {
        int num = colorValue.Contains("+") ? colorValue.IndexOf('+') : colorValue.IndexOf('-');
        if (num != 0)
        {
          this._isfontColorModified = true;
          flag1 = true;
          headerFooterColor = Color.FromArgb((int) byte.MaxValue, this._workBookImpl.GetThemeColor(System.Convert.ToInt32(colorValue.Substring(1, num - 1))));
        }
      }
      else
      {
        foreach (char c in colorValue.Substring(1, 6).ToCharArray())
        {
          if (!char.IsLetterOrDigit(c))
          {
            flag2 = true;
            break;
          }
        }
      }
      if (!flag1 && !flag2)
      {
        this._isfontColorModified = true;
        headerFooterColor = Color.FromArgb(int.Parse(colorValue.Substring(1, 2), NumberStyles.HexNumber, invariantCulture), int.Parse(colorValue.Substring(3, 2), NumberStyles.HexNumber, invariantCulture), int.Parse(colorValue.Substring(5, 2), NumberStyles.HexNumber, invariantCulture));
      }
    }
    return headerFooterColor;
  }

  private bool CheckDigit(string values)
  {
    foreach (char c in values)
    {
      if (!char.IsDigit(c))
        return false;
    }
    return true;
  }

  private bool CheckFontValues(string fontValue)
  {
    string[] strArray = new string[9]
    {
      "&b",
      "&B",
      "&i",
      "&I",
      "&u",
      "&U",
      "&A",
      "&s",
      "&S"
    };
    foreach (string str in strArray)
    {
      if (fontValue.Equals(str))
        return true;
    }
    return false;
  }

  private bool CheckIsRich(string[] headervalues)
  {
    bool flag1 = true;
    bool flag2 = false;
    for (int index = 0; index < headervalues.Length; ++index)
    {
      bool flag3 = headervalues[index].StartsWith("&\"") || headervalues[index].Length > 2 && char.IsDigit(headervalues[index][1]) && headervalues[index][0] != '&';
      if (headervalues[index] != string.Empty && !flag3)
        flag2 = true;
      else if (flag2 && flag3)
        flag1 = false;
    }
    return flag1;
  }

  private PdfTextAlignment GetHorizontalAlignmentFromExtendedFormat(
    IExtendedFormat extendedFormatStyle,
    IRange cell)
  {
    PdfTextAlignment fromExtendedFormat = PdfTextAlignment.Left;
    switch (extendedFormatStyle.HorizontalAlignment)
    {
      case ExcelHAlign.HAlignGeneral:
        if (extendedFormatStyle.Rotation == (int) byte.MaxValue)
        {
          fromExtendedFormat = PdfTextAlignment.Center;
          break;
        }
        WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
        switch (worksheet.GetCellType(cell.Row, cell.Column, false))
        {
          case WorksheetImpl.TRangeValueType.Blank:
          case WorksheetImpl.TRangeValueType.String:
            fromExtendedFormat = PdfTextAlignment.Left;
            break;
          case WorksheetImpl.TRangeValueType.Error:
          case WorksheetImpl.TRangeValueType.Boolean:
            fromExtendedFormat = PdfTextAlignment.Center;
            break;
          case WorksheetImpl.TRangeValueType.Number:
            fromExtendedFormat = extendedFormatStyle.NumberFormat == "@" ? PdfTextAlignment.Left : PdfTextAlignment.Right;
            break;
          case WorksheetImpl.TRangeValueType.Formula:
            switch (worksheet.GetCellType(cell.Row, cell.Column, true))
            {
              case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
              case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
                fromExtendedFormat = PdfTextAlignment.Center;
                break;
              case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                fromExtendedFormat = extendedFormatStyle.NumberFormat == "@" ? PdfTextAlignment.Left : PdfTextAlignment.Right;
                break;
              case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
                fromExtendedFormat = PdfTextAlignment.Left;
                break;
            }
            break;
        }
        break;
      case ExcelHAlign.HAlignCenter:
      case ExcelHAlign.HAlignCenterAcrossSelection:
        fromExtendedFormat = PdfTextAlignment.Center;
        break;
      case ExcelHAlign.HAlignRight:
        fromExtendedFormat = PdfTextAlignment.Right;
        break;
      case ExcelHAlign.HAlignJustify:
        fromExtendedFormat = PdfTextAlignment.Justify;
        break;
      default:
        fromExtendedFormat = PdfTextAlignment.Left;
        break;
    }
    return fromExtendedFormat;
  }

  private PointF GetHorizontalLeft(
    int rotationAngle,
    float stringLength,
    RectangleF rect,
    ExcelVAlign verticalAlign,
    SizeF firstCharSize,
    PdfFont nativeFont,
    PdfStringFormat format)
  {
    int angle = rotationAngle;
    float num1 = stringLength;
    PointF empty1 = PointF.Empty;
    PointF horizontalLeft = PointF.Empty;
    if (angle <= 0)
      angle = -rotationAngle;
    switch (verticalAlign)
    {
      case ExcelVAlign.VAlignTop:
        PointF pointF1 = rotationAngle < 0 ? new PointF(rect.Left + firstCharSize.Width / 2f, rect.Top + firstCharSize.Width / 2f) : (rotationAngle < 45 || rotationAngle >= 90 ? new PointF(rect.Left + firstCharSize.Width, rect.Top) : new PointF(rect.Left + firstCharSize.Height / (float) (90 / rotationAngle), rect.Top + firstCharSize.Width / (float) (90 / rotationAngle)));
        if (rotationAngle == 90)
          pointF1 = new PointF(rect.Left + firstCharSize.Height, rect.Top + firstCharSize.Width);
        float x1 = pointF1.X;
        float y1 = pointF1.Y;
        double num2 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
        Math.Cos(this.DegreeToRadian((double) angle));
        float y2 = rotationAngle >= 0 ? pointF1.Y : (num2 <= (double) rect.Height ? y1 + (float) num2 : y1 + rect.Height - firstCharSize.Height);
        if ((double) y2 > (double) rect.Bottom + (double) rect.Top)
        {
          float num3 = rect.Top + rect.Bottom;
          x1 -= firstCharSize.Width;
          y2 = num3 - firstCharSize.Width;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Width;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num4 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num4 += format.LineSpacing;
                num4 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != -90)
              x1 += num4;
          }
          this._cellDisplayText = stringBuilder.ToString();
        }
        else
        {
          PointF empty2 = PointF.Empty with
          {
            X = rect.Right,
            Y = rect.Top + stringLength * (float) Math.Sin(this.DegreeToRadian((double) angle))
          };
          float num5 = empty2.X - x1;
          float num6 = empty2.Y - y2;
          float num7 = (float) Math.Sqrt((double) num5 * (double) num5 + (double) num6 * (double) num6);
          if ((double) num7 > (double) rect.Width)
          {
            rect.Width = num7;
            this._cellDisplayText = this.GetDisplayText(nativeFont, format, this._cellDisplayText, rect, false);
          }
        }
        horizontalLeft = new PointF(x1, y2);
        break;
      case ExcelVAlign.VAlignCenter:
        PointF pointF2 = new PointF(rect.Left, rect.Top);
        PointF pointF3 = new PointF(rect.Left, rect.Bottom);
        PointF pointF4 = new PointF((float) (((double) pointF2.X + (double) pointF3.X) / 2.0), (float) (((double) pointF2.Y + (double) pointF3.Y) / 2.0));
        float num8 = stringLength / 180f * (float) angle;
        if (rotationAngle > 0)
        {
          horizontalLeft = new PointF(pointF4.X, pointF4.Y - num8);
          if (rotationAngle < 45)
          {
            horizontalLeft.X += firstCharSize.Width;
            horizontalLeft.Y -= firstCharSize.Width / 2f;
          }
          else if (rotationAngle >= 45 && rotationAngle < 90)
          {
            horizontalLeft.X += firstCharSize.Height;
            horizontalLeft.Y -= firstCharSize.Width / 2f;
          }
          else
            horizontalLeft.X += firstCharSize.Height;
        }
        else
        {
          horizontalLeft = new PointF(pointF4.X, pointF4.Y + num8);
          horizontalLeft.X += firstCharSize.Width / 2f;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Width;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num9 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num9 += format.LineSpacing;
                num9 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != -90)
              horizontalLeft.X += num9;
          }
          this._cellDisplayText = stringBuilder.ToString();
          break;
        }
        break;
      case ExcelVAlign.VAlignBottom:
        PointF pointF5 = rotationAngle < 0 ? new PointF(rect.Left + firstCharSize.Width / 2f, rect.Bottom - firstCharSize.Height) : (rotationAngle > 45 ? new PointF(rect.Left + firstCharSize.Height, rect.Bottom - firstCharSize.Width) : new PointF(rect.Left + firstCharSize.Width, rect.Bottom - firstCharSize.Height));
        switch (rotationAngle)
        {
          case -90:
            pointF5 = new PointF(rect.Left + firstCharSize.Height / 4f, rect.Bottom - firstCharSize.Width);
            break;
          case 90:
            pointF5 = new PointF(rect.Left + firstCharSize.Height, rect.Bottom - firstCharSize.Width);
            break;
        }
        float x2 = pointF5.X;
        float y3 = pointF5.Y;
        double num10 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
        Math.Cos(this.DegreeToRadian((double) angle));
        if (rotationAngle > 0)
        {
          if (rotationAngle != 90)
            y3 -= (float) num10;
          else if (num10 > (double) rect.Height)
            y3 = y3 - rect.Height + firstCharSize.Height;
          else
            y3 -= (float) num10;
        }
        if ((double) y3 > (double) rect.Bottom + (double) rect.Top)
        {
          float num11 = rect.Top + rect.Bottom;
          x2 -= firstCharSize.Width / 2f;
          y3 = num11 - firstCharSize.Width / 2f;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Height;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num12 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num12 += format.LineSpacing;
                num12 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != -90)
              x2 += num12;
          }
          this._cellDisplayText = stringBuilder.ToString();
        }
        else
        {
          PointF empty3 = PointF.Empty with
          {
            X = rect.Right,
            Y = rect.Top + stringLength * (float) Math.Sin(this.DegreeToRadian((double) angle))
          };
          float num13 = empty3.X - x2;
          float num14 = empty3.Y - y3;
          float num15 = (float) Math.Sqrt((double) num13 * (double) num13 + (double) num14 * (double) num14);
          if ((double) num15 > (double) rect.Width)
          {
            rect.Width = num15;
            this._cellDisplayText = this.GetDisplayText(nativeFont, format, this._cellDisplayText, rect, false);
          }
        }
        horizontalLeft = new PointF(x2, y3);
        break;
    }
    return horizontalLeft;
  }

  private PointF GetHorizontalRight(
    int rotationAngle,
    float stringLength,
    RectangleF rect,
    ExcelVAlign verticalAlign,
    SizeF lastCharSize,
    SizeF firstCharSize,
    PdfFont nativeFont,
    PdfStringFormat format)
  {
    int angle = rotationAngle;
    float num1 = stringLength;
    PointF pointF1 = PointF.Empty;
    PointF horizontalRight = PointF.Empty;
    if (angle <= 0)
      angle = -rotationAngle;
    switch (verticalAlign)
    {
      case ExcelVAlign.VAlignTop:
        PointF pointF2 = rotationAngle < 0 ? (rotationAngle > -45 ? new PointF(rect.Right - lastCharSize.Width, rect.Top) : new PointF(rect.Right - lastCharSize.Height, rect.Top + lastCharSize.Width)) : (rotationAngle < 45 ? new PointF(rect.Right - lastCharSize.Width, rect.Top) : new PointF(rect.Right, rect.Top));
        float x1 = pointF2.X;
        float y1 = pointF2.Y;
        double num2 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
        double num3 = (double) num1 * Math.Cos(this.DegreeToRadian((double) angle));
        float x2 = x1 - (float) num3;
        float y2;
        if (rotationAngle < 0)
        {
          if (num2 > (double) rect.Height)
            num2 = (double) rect.Height - (double) firstCharSize.Width;
          y2 = y1 + (float) num2;
        }
        else
        {
          float y3 = pointF2.Y;
          if (rotationAngle >= 45)
          {
            x2 -= firstCharSize.Width;
            y2 = y3 + firstCharSize.Width;
          }
          else
          {
            x2 -= firstCharSize.Height / (float) (90 / angle);
            y2 = y3 + firstCharSize.Width;
          }
        }
        if ((double) y2 > (double) rect.Bottom + (double) rect.Top)
        {
          float num4 = rect.Top + rect.Bottom;
          x2 -= firstCharSize.Width;
          y2 = num4 - firstCharSize.Width;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Width;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num5 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num5 += format.LineSpacing;
                num5 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != 90)
              x2 -= num5;
          }
          this._cellDisplayText = stringBuilder.ToString();
        }
        else
        {
          PointF empty = PointF.Empty with
          {
            X = rect.Width,
            Y = rect.Y + stringLength * (float) Math.Sin(this.DegreeToRadian((double) angle))
          };
          float num6 = empty.X - x2;
          float num7 = empty.Y - y2;
          float num8 = (float) Math.Sqrt((double) num6 * (double) num6 + (double) num7 * (double) num7);
          if ((double) num8 > (double) rect.Width)
          {
            rect.Width = num8;
            this._cellDisplayText = this.GetDisplayText(nativeFont, format, this._cellDisplayText, rect, false);
          }
        }
        horizontalRight = new PointF(x2, y2);
        break;
      case ExcelVAlign.VAlignCenter:
        PointF pointF3 = new PointF(rect.Right, rect.Top);
        PointF pointF4 = new PointF(rect.Right, rect.Bottom);
        PointF pointF5 = new PointF((float) (((double) pointF3.X + (double) pointF4.X) / 2.0), (float) (((double) pointF3.Y + (double) pointF4.Y) / 2.0));
        float num9 = stringLength / 180f * (float) angle;
        if (rotationAngle > 0)
        {
          pointF1 = new PointF(pointF5.X, pointF5.Y + num9);
          if (rotationAngle > 0 && rotationAngle <= 45)
          {
            pointF1.X -= lastCharSize.Height / (float) (90 / angle);
            pointF1.Y += lastCharSize.Width;
          }
          else
          {
            pointF1.X -= lastCharSize.Width;
            pointF1.Y += lastCharSize.Width;
          }
        }
        else
        {
          pointF1 = new PointF(pointF5.X, pointF5.Y - num9);
          if (rotationAngle <= -45 && rotationAngle >= -90)
          {
            pointF1.X -= lastCharSize.Height;
            pointF1.Y -= lastCharSize.Width / (float) (90 / angle);
          }
          else
          {
            pointF1.X -= lastCharSize.Width;
            pointF1.Y -= lastCharSize.Width / (float) (90 / angle);
          }
        }
        double num10 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
        double num11 = (double) num1 * Math.Cos(this.DegreeToRadian((double) angle));
        float x3 = pointF1.X;
        float y4 = pointF1.Y;
        float x4 = x3 - (float) num11;
        float y5;
        if (rotationAngle > 0)
        {
          y5 = y4 - (float) num10;
          if ((double) y5 < (double) rect.Top)
            y5 = rect.Top;
        }
        else
        {
          y5 = y4 + (float) num10;
          if ((double) y5 > (double) rect.Bottom)
            y5 = rect.Bottom;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Width;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num12 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num12 += format.LineSpacing;
                num12 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != 90)
              x4 -= num12;
          }
          this._cellDisplayText = stringBuilder.ToString();
        }
        horizontalRight = new PointF(x4, y5);
        break;
      case ExcelVAlign.VAlignBottom:
        PointF pointF6 = rotationAngle < 0 ? (rotationAngle < -45 ? new PointF(rect.Right - lastCharSize.Width / (float) (90 / angle), rect.Bottom - lastCharSize.Width) : (rotationAngle > -23 ? new PointF(rect.Right - lastCharSize.Width, rect.Bottom - lastCharSize.Height) : new PointF(rect.Right - lastCharSize.Width, rect.Bottom - lastCharSize.Width))) : (rotationAngle > 45 ? new PointF(rect.Right - lastCharSize.Width / (float) (90 / angle), rect.Bottom - lastCharSize.Width / (float) (90 / angle)) : new PointF(rect.Right - lastCharSize.Width, rect.Bottom - lastCharSize.Height));
        float x5 = pointF6.X;
        float y6 = pointF6.Y;
        double num13 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
        double num14 = (double) num1 * Math.Cos(this.DegreeToRadian((double) angle));
        float x6 = x5 - (float) num14;
        if (rotationAngle < 0)
        {
          if (rotationAngle > -45)
          {
            y6 -= firstCharSize.Height / (float) (90 / angle);
            x6 -= firstCharSize.Width / (float) (90 / angle);
          }
          else if (rotationAngle > -90)
          {
            y6 -= firstCharSize.Width / (float) (90 / angle);
            x6 -= firstCharSize.Width / (float) (90 / angle);
          }
          else
            x6 -= firstCharSize.Width;
        }
        else if (rotationAngle != 90)
          y6 -= (float) num13;
        else if (num13 > (double) rect.Height)
          y6 = y6 - rect.Height + firstCharSize.Height;
        else
          y6 -= (float) num13;
        if ((double) y6 > (double) rect.Bottom + (double) rect.Top)
        {
          float num15 = rect.Top + rect.Bottom;
          x6 -= firstCharSize.Width;
          y6 = num15 - firstCharSize.Width;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Height;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num16 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          if (displayWrapTextList.Count > 0)
          {
            stringBuilder.Append(displayWrapTextList[0]);
            displayWrapTextList.RemoveAt(0);
          }
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num16 += format.LineSpacing;
                num16 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != 90)
              x6 -= num16;
          }
          this._cellDisplayText = stringBuilder.ToString();
        }
        else
        {
          PointF empty = PointF.Empty with
          {
            X = rect.Width,
            Y = rect.Y + stringLength * (float) Math.Sin(this.DegreeToRadian((double) angle))
          };
          float num17 = empty.X - x6;
          float num18 = empty.Y - y6;
          float num19 = (float) Math.Sqrt((double) num17 * (double) num17 + (double) num18 * (double) num18);
          if ((double) num19 > (double) rect.Width)
          {
            rect.Width = num19;
            this._cellDisplayText = this.GetDisplayText(nativeFont, format, this._cellDisplayText, rect, false);
          }
        }
        horizontalRight = new PointF(x6, y6);
        break;
    }
    return horizontalRight;
  }

  private PointF GetHorizontalCenter(
    int rotationAngle,
    float stringLength,
    RectangleF rect,
    ExcelVAlign verticalAlign,
    SizeF lastCharSize,
    SizeF firstCharSize,
    PdfFont nativeFont,
    PdfStringFormat format)
  {
    int angle = rotationAngle;
    float num1 = stringLength;
    PointF pointF1 = PointF.Empty;
    PointF horizontalCenter = PointF.Empty;
    if (angle <= 0)
      angle = -rotationAngle;
    switch (verticalAlign)
    {
      case ExcelVAlign.VAlignTop:
        PointF pointF2 = new PointF(rect.Left, rect.Top);
        PointF pointF3 = new PointF(rect.Right, rect.Top);
        PointF pointF4 = new PointF((float) (((double) pointF2.X + (double) pointF3.X) / 2.0), (float) (((double) pointF2.Y + (double) pointF3.Y) / 2.0));
        float num2 = stringLength / 180f * (float) (90 - angle);
        float x1 = pointF4.X;
        float y1 = pointF4.Y;
        float x2;
        float y2;
        if (rotationAngle < 0)
        {
          pointF1 = new PointF(pointF4.X + num2, pointF4.Y);
          pointF1.Y += lastCharSize.Width;
          double num3 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
          double num4 = (double) num1 * Math.Cos(this.DegreeToRadian((double) angle));
          x2 = pointF1.X - (float) num4;
          y2 = pointF1.Y + (float) num3;
          if ((double) y2 > (double) rect.Bottom)
            y2 = rect.Bottom - firstCharSize.Width;
        }
        else
        {
          x2 = x1 - num2;
          y2 = y1 + firstCharSize.Width;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Width;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num5 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num5 += format.LineSpacing;
                num5 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != 90)
              x2 -= num5;
            else
              x2 += num5;
          }
          this._cellDisplayText = stringBuilder.ToString();
        }
        horizontalCenter = new PointF(x2, y2);
        break;
      case ExcelVAlign.VAlignCenter:
        PointF pointF5 = new PointF(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
        float num6 = stringLength / 2f;
        double num7 = (double) num6 * Math.Sin(this.DegreeToRadian((double) angle));
        double num8 = (double) num6 * Math.Cos(this.DegreeToRadian((double) angle));
        if (rotationAngle < 0 && rotationAngle >= -90)
        {
          pointF5.Y += (float) num7;
          if (this._cellDisplayText.Contains("\n"))
          {
            float num9 = nativeFont.MeasureString(this._cellDisplayText, format).Height / firstCharSize.Height;
            pointF5.X -= (float) num8 + (float) ((double) firstCharSize.Height * (double) num9 / 2.0);
          }
          else
            pointF5.X -= (float) num8 + firstCharSize.Height / 2f;
          horizontalCenter = new PointF(pointF5.X, pointF5.Y);
          if ((double) horizontalCenter.Y > (double) rect.Bottom)
          {
            horizontalCenter.Y = rect.Bottom;
            horizontalCenter.Y -= firstCharSize.Width;
          }
        }
        else if (rotationAngle > 0 && rotationAngle <= 90)
        {
          pointF5.Y -= (float) num7;
          pointF5.X -= (float) num8 - firstCharSize.Height / 2f;
          horizontalCenter = new PointF(pointF5.X, pointF5.Y);
          if ((double) horizontalCenter.Y < (double) rect.Top)
          {
            horizontalCenter.Y = rect.Top;
            horizontalCenter.Y += firstCharSize.Width;
          }
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          if ((double) firstCharSize.Height > (double) rect.Height)
          {
            rect.Width -= firstCharSize.Width;
            horizontalCenter.Y += rect.Height;
          }
          else
            rect.Width = rect.Height - firstCharSize.Width;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num10 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          if (displayWrapTextList.Count > 0)
          {
            stringBuilder.Append(displayWrapTextList[0]);
            displayWrapTextList.RemoveAt(0);
          }
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num10 += format.LineSpacing;
                num10 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
            if (rotationAngle != 90)
              horizontalCenter.X -= num10;
            else
              horizontalCenter.X += num10;
          }
          this._cellDisplayText = stringBuilder.ToString();
          break;
        }
        break;
      case ExcelVAlign.VAlignBottom:
        PointF pointF6 = new PointF(rect.Left, rect.Bottom);
        PointF pointF7 = new PointF(rect.Right, rect.Bottom);
        PointF pointF8 = new PointF((float) (((double) pointF6.X + (double) pointF7.X) / 2.0), (float) (((double) pointF6.Y + (double) pointF7.Y) / 2.0));
        float num11 = stringLength / 180f * (float) (90 - angle);
        float x3 = pointF8.X;
        float y3 = pointF8.Y;
        float x4;
        float y4;
        if (rotationAngle < 0)
        {
          x4 = x3 - num11;
          y4 = rotationAngle == -90 ? y3 - firstCharSize.Height / 4f : y3 - firstCharSize.Height;
        }
        else
        {
          pointF1 = new PointF(pointF8.X + num11, pointF8.Y);
          if (rotationAngle != 90)
            pointF1.Y -= lastCharSize.Height;
          double num12 = (double) num1 * Math.Sin(this.DegreeToRadian((double) angle));
          double num13 = (double) num1 * Math.Cos(this.DegreeToRadian((double) angle));
          x4 = pointF1.X - (float) num13;
          y4 = pointF1.Y - (float) num12;
          if ((double) y4 < (double) rect.Top)
            y4 = rect.Top + firstCharSize.Width;
        }
        if (rotationAngle == 90 || rotationAngle == -90)
        {
          rect.Width = rect.Height - firstCharSize.Height;
          List<string> displayWrapTextList = this.GetDisplayWrapTextList(nativeFont, format, this._cellDisplayText, rect);
          float num14 = 0.0f;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(displayWrapTextList[0]);
          displayWrapTextList.RemoveAt(0);
          if (displayWrapTextList.Count != 0)
          {
            foreach (string str in displayWrapTextList)
            {
              if (str != "\r\n")
              {
                num14 += format.LineSpacing;
                num14 += nativeFont.MeasureString(str[0].ToString(), format).Height;
              }
              stringBuilder.Append(str);
            }
          }
          else
            num14 += nativeFont.MeasureString(this._cellDisplayText, format).Height / 2f;
          if (rotationAngle != 90)
            x4 -= num14;
          else
            x4 += num14;
          this._cellDisplayText = stringBuilder.ToString();
        }
        horizontalCenter = new PointF(x4, y4);
        break;
    }
    return horizontalCenter;
  }

  private float GetMaxHeight(List<HeaderFooterSection> sections)
  {
    List<float> maxList = new List<float>();
    foreach (HeaderFooterSection section in sections)
      maxList.Add(section.Height);
    return this.GetMaxValue(maxList);
  }

  private SizeF GetMaxWidth(List<SizeF> sizes)
  {
    SizeF maxWidth = new SizeF();
    float num = 0.0f;
    foreach (SizeF siz in sizes)
    {
      if ((double) maxWidth.Width < (double) siz.Width)
        maxWidth.Width = siz.Width;
      num += siz.Height;
    }
    maxWidth.Height = num;
    return maxWidth;
  }

  private float GetMaxValue(List<float> maxList)
  {
    float maxValue = (float) int.MinValue;
    for (int index = 0; index < maxList.Count; ++index)
    {
      float max = maxList[index];
      if ((double) max > (double) maxValue)
        maxValue = max;
    }
    return maxValue;
  }

  private int GetMaxValue(List<int> maxList)
  {
    int maxValue = int.MinValue;
    for (int index = 0; index < maxList.Count; ++index)
    {
      int max = maxList[index];
      if (max > maxValue)
        maxValue = max;
    }
    return maxValue;
  }

  private RectangleF GetMergedRectangle(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    float startX,
    float startY)
  {
    float num1 = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(mergedRegion.RowFrom), PdfGraphicsUnit.Point);
    float num2 = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(mergedRegion.ColumnFrom), PdfGraphicsUnit.Point);
    float num3 = num1 - this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(firstRow - 1), PdfGraphicsUnit.Point);
    float num4 = num2 - this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(firstColumn - 1), PdfGraphicsUnit.Point);
    float y = num3 + startY;
    float x = num4 + startX;
    IRange range = sheet[mergedRegion.RowFrom + 1, mergedRegion.ColumnFrom + 1];
    int extendedFormatIndex = (int) (range as RangeImpl).ExtendedFormatIndex;
    ExtendedFormatImpl innerExtFormat = (range as RangeImpl).Workbook.InnerExtFormats[extendedFormatIndex];
    if (innerExtFormat != null && innerExtFormat.Rotation != 90 && innerExtFormat.Rotation != 270 && lastRow - 1 < mergedRegion.RowTo)
      mergedRegion.RowTo = lastRow - 1;
    float height = this.Pdf_UnitConverter.ConvertFromPixels(this.RowHeightGetter.GetTotal(mergedRegion.RowFrom + 1, mergedRegion.RowTo + 1), PdfGraphicsUnit.Point);
    float width = this.Pdf_UnitConverter.ConvertFromPixels(this.ColumnWidthGetter.GetTotal(mergedRegion.ColumnFrom + 1, mergedRegion.ColumnTo + 1), PdfGraphicsUnit.Point);
    return new RectangleF(x, y, width, height);
  }

  private Image GetResizedImage(Image originalImage, int width, int height)
  {
    if (width <= 0 || height <= 0)
      return originalImage;
    if (originalImage.RawFormat.Equals((object) ImageFormat.Gif))
    {
      foreach (PropertyItem propertyItem in originalImage.PropertyItems)
      {
        if (propertyItem.Id == 20740)
        {
          int index = (int) propertyItem.Value[0];
          if (originalImage.Palette.Entries[index].A == (byte) 0)
            return originalImage;
        }
      }
    }
    Bitmap resizedImage = new Bitmap(width, height);
    resizedImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) resizedImage))
    {
      graphics.SmoothingMode = SmoothingMode.HighQuality;
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.DrawImage(originalImage, new Rectangle(0, 0, width, height));
    }
    return (Image) resizedImage;
  }

  private IRange GetAdjacentRange(IRange cell)
  {
    return this._workSheet[cell.Row == 1 ? cell.Row : cell.Row - 1, cell.Column, cell.LastRow == 1 ? cell.LastRow : cell.LastRow - 1, cell.LastColumn];
  }

  private void GetScaledPage(
    float originalWidth,
    float originalHeight,
    float maxWidth,
    float maxHeight,
    bool fitPage)
  {
    if (!fitPage)
    {
      if ((double) originalWidth <= (double) maxWidth)
        maxWidth = originalWidth;
      if ((double) (originalHeight * maxWidth / originalWidth) > (double) maxHeight)
        maxWidth = originalWidth * maxHeight / originalHeight;
      this._scaledPageWidth = maxWidth;
      this._scaledPageHeight = maxHeight;
    }
    else
    {
      float num1 = originalWidth / originalHeight;
      float num2;
      float num3;
      if ((double) maxHeight > 0.0 && (double) maxWidth > 0.0)
      {
        if ((double) originalWidth < (double) maxWidth && (double) originalHeight < (double) maxHeight)
        {
          num2 = originalWidth;
          num3 = originalHeight;
        }
        else if ((double) num1 > 1.0)
        {
          num2 = maxWidth;
          num3 = num2 / num1;
          if ((double) num3 > (double) maxHeight)
          {
            num3 = maxHeight;
            num2 = num3 * num1;
          }
        }
        else
        {
          num3 = maxHeight;
          num2 = num3 * num1;
          if ((double) num2 > (double) maxWidth)
          {
            num2 = maxWidth;
            num3 = num2 / num1;
          }
        }
      }
      else if ((double) maxHeight == 0.0 && (double) originalWidth > (double) maxWidth)
      {
        num3 = maxWidth / num1;
        num2 = (float) (int) ((double) num3 * (double) num1);
      }
      else if ((double) maxWidth == 0.0 && (double) originalHeight >= (double) maxHeight)
      {
        num3 = maxHeight;
        num2 = num3 * num1;
      }
      else
      {
        num2 = originalWidth;
        num3 = originalHeight;
      }
      this._scaledPageWidth = num2;
      this._scaledPageHeight = num3;
    }
  }

  private void GetScaledHFPage(
    float originalWidth,
    float originalHeight,
    float maxWidth,
    float maxHeight,
    bool fitPage)
  {
    if (!fitPage)
    {
      if ((double) originalWidth <= (double) maxWidth)
        maxWidth = originalWidth;
      if ((double) (originalHeight * maxWidth / originalWidth) <= (double) maxHeight)
        return;
      maxWidth = originalWidth * maxHeight / originalHeight;
    }
    else
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = originalWidth / originalHeight;
      if ((double) maxHeight > 0.0 && (double) maxWidth > 0.0)
      {
        if ((double) originalWidth < (double) maxWidth && (double) originalHeight < (double) maxHeight)
        {
          num1 = originalWidth;
          num2 = originalHeight;
        }
        else if ((double) num3 > 1.0)
        {
          if ((double) (maxWidth / num3) <= (double) maxHeight)
            return;
          num1 = maxHeight * num3;
        }
        else
        {
          if ((double) (maxHeight * num3) <= (double) maxWidth)
            return;
          num2 = maxWidth / num3;
        }
      }
      else if ((double) maxHeight == 0.0 && (double) originalWidth > (double) maxWidth)
        num1 = (float) (int) ((double) (maxWidth / num3) * (double) num3);
      else if ((double) maxWidth == 0.0 && (double) originalHeight >= (double) maxHeight)
      {
        num1 = maxHeight * num3;
      }
      else
      {
        num1 = originalWidth;
        num2 = originalHeight;
      }
    }
  }

  private Image GetScaledPicture(Image originalImage, int maxWidth, int maxHeight)
  {
    float width1 = this.Pdf_UnitConverter.ConvertFromPixels((float) originalImage.Width, PdfGraphicsUnit.Point);
    float height1 = this.Pdf_UnitConverter.ConvertFromPixels((float) originalImage.Height, PdfGraphicsUnit.Point);
    float num = (float) originalImage.Width / (float) originalImage.Height;
    int width2;
    int height2;
    if (maxHeight > 0 && maxWidth > 0)
    {
      if (originalImage.Width < maxWidth && originalImage.Height < maxHeight)
        return originalImage;
      if ((double) num > 1.0)
      {
        width2 = maxWidth;
        height2 = (int) ((double) width2 / (double) num);
        if (height2 > maxHeight)
        {
          height2 = maxHeight;
          width2 = (int) ((double) height2 * (double) num);
        }
      }
      else
      {
        height2 = maxHeight;
        width2 = (int) ((double) height2 * (double) num);
        if (width2 > maxWidth)
        {
          width2 = maxWidth;
          height2 = (int) ((double) width2 / (double) num);
        }
      }
    }
    else if (maxHeight == 0 && originalImage.Width > maxWidth)
    {
      width2 = maxWidth;
      height2 = (int) ((double) width2 / (double) num);
    }
    else
    {
      if (maxWidth != 0 || originalImage.Height <= maxHeight)
        return this.GetResizedImage(originalImage, (int) width1, (int) height1);
      height2 = maxHeight;
      width2 = (int) ((double) height2 * (double) num);
    }
    return this.GetResizedImage(originalImage, width2, height2);
  }

  private void GetScaledPictureWidthHeight(
    Image originalImage,
    ref float maxWidth,
    ref float maxHeight)
  {
    float num1 = this.Pdf_UnitConverter.ConvertFromPixels((float) originalImage.Width, PdfGraphicsUnit.Point);
    float num2 = this.Pdf_UnitConverter.ConvertFromPixels((float) originalImage.Height, PdfGraphicsUnit.Point);
    float num3 = (float) originalImage.Width / (float) originalImage.Height;
    float num4;
    float num5;
    if ((double) maxHeight > 0.0 && (double) maxWidth > 0.0)
    {
      if ((double) originalImage.Width < (double) maxWidth && (double) originalImage.Height < (double) maxHeight)
      {
        maxWidth = num1;
        maxHeight = num2;
        return;
      }
      if ((double) num3 > 1.0)
      {
        if ((double) (int) ((double) maxWidth / (double) num3) > (double) maxHeight)
          num4 = (float) (int) ((double) maxHeight * (double) num3);
      }
      else if ((double) (int) ((double) maxHeight * (double) num3) > (double) maxWidth)
        num5 = (float) (int) ((double) maxWidth / (double) num3);
    }
    else if ((double) maxHeight == 0.0 && (double) originalImage.Width > (double) maxWidth)
      num5 = (float) (int) ((double) maxWidth / (double) num3);
    else if ((double) maxWidth == 0.0 && (double) originalImage.Height > (double) maxHeight)
    {
      num4 = (float) (int) ((double) maxHeight * (double) num3);
    }
    else
    {
      maxWidth = (float) (int) num1;
      maxHeight = (float) (int) num2;
      return;
    }
    num4 = num1;
    num5 = num2;
  }

  private int GetNextIndex(char ch, int currentIndex, List<char> charList)
  {
    int index = currentIndex + 1;
    for (int count = charList.Count; index < count; ++index)
    {
      if (charList[index] == '"')
        return index + 1;
    }
    return 0;
  }

  private float GetSortedWidth(
    string align,
    Dictionary<string, PdfTemplate> temps,
    float dividedWidth)
  {
    float sortedWidth1 = 0.0f;
    Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment alignment1 = Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.None;
    float num = 0.0f;
    float sortedWidth2 = 0.0f;
    float sortedWidth3 = 0.0f;
    float sortedWidth4 = 0.0f;
    foreach (KeyValuePair<string, PdfTemplate> temp in temps)
    {
      Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment alignment2 = (Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment) Enum.Parse(typeof (Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment), temp.Key);
      if (temp.Key == align)
      {
        sortedWidth1 += dividedWidth;
        alignment1 = alignment2;
        num = temp.Value.Width;
      }
      else
      {
        PdfTemplate pdfTemplate = (PdfTemplate) null;
        switch (alignment1)
        {
          case Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.Left:
            sortedWidth2 = sortedWidth1;
            if (!temps.TryGetValue("Center", out pdfTemplate))
            {
              sortedWidth2 += dividedWidth;
              if ((double) sortedWidth1 < (double) num && !temps.TryGetValue("Right", out pdfTemplate))
              {
                sortedWidth2 += dividedWidth;
                continue;
              }
              continue;
            }
            continue;
          case Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.Center:
            sortedWidth3 = sortedWidth1;
            if (!temps.TryGetValue("Left", out pdfTemplate))
              sortedWidth3 += dividedWidth;
            else if ((double) sortedWidth2 < (double) dividedWidth)
              sortedWidth3 += dividedWidth - sortedWidth2;
            if (!temps.TryGetValue("Right", out pdfTemplate))
            {
              sortedWidth3 += dividedWidth;
              continue;
            }
            sortedWidth3 += pdfTemplate.Width;
            continue;
          case Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.Right:
            sortedWidth4 = sortedWidth1;
            if (!temps.TryGetValue("Center", out pdfTemplate))
            {
              sortedWidth4 += dividedWidth;
              if ((double) sortedWidth1 < (double) num && !temps.TryGetValue("Right", out pdfTemplate))
              {
                sortedWidth4 += dividedWidth;
                continue;
              }
              continue;
            }
            if ((double) sortedWidth3 < (double) dividedWidth)
            {
              sortedWidth4 += dividedWidth - sortedWidth3;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    switch (alignment1)
    {
      case Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.Left:
        return sortedWidth2;
      case Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.Center:
        return sortedWidth3;
      case Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter.Alignment.Right:
        return sortedWidth4;
      default:
        return sortedWidth1;
    }
  }

  private string[] GetSplitted(string pageSetup)
  {
    bool flag = false;
    string str = string.Empty;
    for (; !flag; flag = true)
    {
      pageSetup = pageSetup.Replace("$", "_x0024_");
      if (pageSetup.Contains("_x005F_x000D_") || pageSetup.Contains("_x005F_x005F_"))
      {
        pageSetup = pageSetup.Replace("_x000D_", "\n");
        pageSetup = pageSetup.Replace("_x005F\n", "_x000D_");
        pageSetup = pageSetup.Replace("_x005F_x005F", "_x005F");
      }
      else
        pageSetup = pageSetup.Replace("_x000D_", "\n");
      char[] charArray = pageSetup.ToCharArray();
      List<char> charList = new List<char>();
      charList.AddRange((IEnumerable<char>) charArray);
      int index = 0;
      while (index < charList.Count)
      {
        if (charList[index] == '&')
        {
          charList.Insert(index, '$');
          if (charList.Count > index + 2)
          {
            if (charList[index + 2] == 'K')
            {
              if (index + 9 <= charList.Count)
              {
                charList.Insert(index + 9, '$');
                index += 9;
              }
              else
                charList.RemoveRange(index, charList.IndexOf('&', index + 2) >= 0 ? charList.IndexOf('&', index + 2) - index : charList.Count - index);
            }
            else if (charList[index + 2] != '"')
            {
              if (!char.IsDigit(charList[index + 2]))
              {
                charList.Insert(index + 3, '$');
                index += 4;
              }
              else if (charList.Count > index + 3 && char.IsDigit(charList[index + 3]))
              {
                charList.Insert(index + 4, '$');
                index += 5;
              }
              else
              {
                charList.Insert(index + 3, '$');
                index += 4;
              }
            }
            else
            {
              if (charList[index + 2] == '"')
              {
                int nextIndex = this.GetNextIndex('"', index + 2, charList);
                charList.Insert(nextIndex, '$');
                ++index;
              }
              index += 2;
            }
          }
          else
            index += 2;
        }
        else
          ++index;
      }
      char[] chArray = new char[charList.Count];
      char[] array = charList.ToArray();
      charList.Clear();
      str = new string(array);
      chArray = (char[]) null;
    }
    string[] arr = str.Split(new char[1]{ '$' }, StringSplitOptions.RemoveEmptyEntries);
    if (str.Contains("_x0024_"))
      arr = this.ReplaceDollar(arr);
    return arr;
  }

  private string[] ReplaceDollar(string[] arr)
  {
    for (int index = 0; index < arr.Length; ++index)
    {
      if (arr[index].Contains("_x0024_"))
        arr[index] = arr[index].Replace("_x0024_", "$");
    }
    return arr;
  }

  private void GetStartEndBorderIndex(
    ExcelBordersIndex borderIndex,
    out ExcelBordersIndex start,
    out ExcelBordersIndex end)
  {
    start = (ExcelBordersIndex) -1;
    end = (ExcelBordersIndex) -1;
    switch (borderIndex)
    {
      case ExcelBordersIndex.DiagonalDown:
        start = ExcelBordersIndex.EdgeTop;
        end = ExcelBordersIndex.EdgeRight;
        break;
      case ExcelBordersIndex.DiagonalUp:
        start = ExcelBordersIndex.EdgeTop;
        end = ExcelBordersIndex.EdgeLeft;
        break;
      case ExcelBordersIndex.EdgeLeft:
      case ExcelBordersIndex.EdgeRight:
        start = ExcelBordersIndex.EdgeTop;
        end = ExcelBordersIndex.EdgeBottom;
        break;
      case ExcelBordersIndex.EdgeTop:
      case ExcelBordersIndex.EdgeBottom:
        start = ExcelBordersIndex.EdgeLeft;
        end = ExcelBordersIndex.EdgeRight;
        break;
    }
  }

  private void GetTemplateHeight(
    IPageSetupBase pageSetup,
    string name,
    string align,
    out int imageHeight,
    out int imageWidth)
  {
    imageHeight = 0;
    imageWidth = 0;
    if (!((pageSetup as PageSetupBaseImpl).FindParent(typeof (WorksheetImpl)) is WorksheetBaseImpl parent))
      parent = (pageSetup as PageSetupBaseImpl).FindParent(typeof (ChartImpl)) as WorksheetBaseImpl;
    WorksheetBaseImpl worksheetBaseImpl = parent;
    switch (name)
    {
      case "Header":
        if (pageSetup.LeftHeaderImage != null && pageSetup.LeftHeader.Contains("&G") && align == "Left")
        {
          string preserveStyleString = (worksheetBaseImpl.HeaderFooterShapes["LH"] as ShapeImpl).PreserveStyleString;
          if (preserveStyleString != null)
          {
            float[] scaledWidthHeight = this.GetHFScaledWidthHeight(preserveStyleString);
            imageWidth = (int) scaledWidthHeight[0];
            imageHeight = (int) scaledWidthHeight[1];
          }
          else
            this.GetImageWidthHeightInPoint(this.GetDrawingImage(pageSetup.LeftHeaderImage), ref imageHeight, ref imageWidth);
        }
        if (pageSetup.CenterHeaderImage != null && pageSetup.CenterHeader.Contains("&G") && align == "Center")
        {
          string preserveStyleString = (worksheetBaseImpl.HeaderFooterShapes["CH"] as ShapeImpl).PreserveStyleString;
          if (preserveStyleString != null)
          {
            float[] scaledWidthHeight = this.GetHFScaledWidthHeight(preserveStyleString);
            imageHeight = (int) scaledWidthHeight[1];
            imageWidth = (int) scaledWidthHeight[0];
          }
          else
            this.GetImageWidthHeightInPoint(this.GetDrawingImage(pageSetup.CenterHeaderImage), ref imageHeight, ref imageWidth);
        }
        if (pageSetup.RightHeaderImage == null || !pageSetup.RightHeader.Contains("&G") || !(align == "Right"))
          break;
        string preserveStyleString1 = (worksheetBaseImpl.HeaderFooterShapes["RH"] as ShapeImpl).PreserveStyleString;
        if (preserveStyleString1 != null)
        {
          float[] scaledWidthHeight = this.GetHFScaledWidthHeight(preserveStyleString1);
          imageWidth = (int) scaledWidthHeight[0];
          imageHeight = (int) scaledWidthHeight[1];
          break;
        }
        this.GetImageWidthHeightInPoint(this.GetDrawingImage(pageSetup.RightHeaderImage), ref imageHeight, ref imageWidth);
        break;
      case "Footer":
        if (pageSetup.LeftFooterImage != null && pageSetup.LeftFooter.Contains("&G") && align == "Left")
        {
          string preserveStyleString2 = (worksheetBaseImpl.HeaderFooterShapes["LF"] as ShapeImpl).PreserveStyleString;
          if (preserveStyleString2 != null)
          {
            float[] scaledWidthHeight = this.GetHFScaledWidthHeight(preserveStyleString2);
            imageWidth = (int) scaledWidthHeight[0];
            imageHeight = (int) scaledWidthHeight[1];
          }
          else
            this.GetImageWidthHeightInPoint(this.GetDrawingImage(pageSetup.LeftFooterImage), ref imageHeight, ref imageWidth);
        }
        if (pageSetup.CenterFooterImage != null && pageSetup.CenterFooter.Contains("&G") && align == "Center")
        {
          string preserveStyleString3 = (worksheetBaseImpl.HeaderFooterShapes["CF"] as ShapeImpl).PreserveStyleString;
          if (preserveStyleString3 != null)
          {
            float[] scaledWidthHeight = this.GetHFScaledWidthHeight(preserveStyleString3);
            imageHeight = (int) scaledWidthHeight[1];
            imageWidth = (int) scaledWidthHeight[0];
          }
          else
            this.GetImageWidthHeightInPoint(this.GetDrawingImage(pageSetup.CenterFooterImage), ref imageHeight, ref imageWidth);
        }
        if (pageSetup.RightFooterImage == null || !pageSetup.RightFooter.Contains("&G") || !(align == "Right"))
          break;
        string preserveStyleString4 = (worksheetBaseImpl.HeaderFooterShapes["RF"] as ShapeImpl).PreserveStyleString;
        if (preserveStyleString4 != null)
        {
          float[] scaledWidthHeight = this.GetHFScaledWidthHeight(preserveStyleString4);
          imageWidth = (int) scaledWidthHeight[0];
          imageHeight = (int) scaledWidthHeight[1];
          break;
        }
        this.GetImageWidthHeightInPoint(this.GetDrawingImage(pageSetup.RightFooterImage), ref imageHeight, ref imageWidth);
        break;
    }
  }

  private void GetImageWidthHeightInPoint(Image image, ref int imageHeight, ref int imageWidth)
  {
    imageHeight = (int) ((double) this.Pdf_UnitConverter.ConvertUnits((float) Math.Round((double) image.Height * ApplicationImpl.ConvertToPixels(1.0, MeasureUnits.Inch) / (double) image.VerticalResolution), PdfGraphicsUnit.Pixel, PdfGraphicsUnit.Point) * (double) this._scaledCellHeight);
    imageWidth = (int) ((double) this.Pdf_UnitConverter.ConvertUnits((float) Math.Round((double) image.Width * ApplicationImpl.ConvertToPixels(1.0, MeasureUnits.Inch) / (double) image.HorizontalResolution), PdfGraphicsUnit.Pixel, PdfGraphicsUnit.Point) * (double) this._scaledCellWidth);
  }

  private PdfTextAlignment GetTextAlignmentFromShape(IShape shape)
  {
    PdfTextAlignment alignmentFromShape;
    switch (shape)
    {
      case AutoShapeImpl _:
        switch (shape.TextFrame.HorizontalAlignment)
        {
          case ExcelHorizontalAlignment.Left:
            alignmentFromShape = PdfTextAlignment.Left;
            break;
          case ExcelHorizontalAlignment.Center:
            alignmentFromShape = PdfTextAlignment.Center;
            break;
          case ExcelHorizontalAlignment.Right:
            alignmentFromShape = PdfTextAlignment.Right;
            break;
          default:
            alignmentFromShape = PdfTextAlignment.Justify;
            break;
        }
        break;
      case TextBoxShapeImpl _:
        switch ((shape as TextBoxShapeImpl).HAlignment)
        {
          case ExcelCommentHAlign.Left:
            alignmentFromShape = PdfTextAlignment.Left;
            break;
          case ExcelCommentHAlign.Center:
            alignmentFromShape = PdfTextAlignment.Center;
            break;
          case ExcelCommentHAlign.Right:
            alignmentFromShape = PdfTextAlignment.Right;
            break;
          case ExcelCommentHAlign.Distributed:
            alignmentFromShape = PdfTextAlignment.Left;
            break;
          default:
            alignmentFromShape = PdfTextAlignment.Justify;
            break;
        }
        break;
      case CommentShapeImpl _:
        switch ((shape as CommentShapeImpl).HAlignment)
        {
          case ExcelCommentHAlign.Left:
            alignmentFromShape = PdfTextAlignment.Left;
            break;
          case ExcelCommentHAlign.Center:
            alignmentFromShape = PdfTextAlignment.Center;
            break;
          case ExcelCommentHAlign.Right:
            alignmentFromShape = PdfTextAlignment.Right;
            break;
          case ExcelCommentHAlign.Distributed:
            alignmentFromShape = PdfTextAlignment.Left;
            break;
          default:
            alignmentFromShape = PdfTextAlignment.Justify;
            break;
        }
        break;
      default:
        alignmentFromShape = PdfTextAlignment.Left;
        break;
    }
    return alignmentFromShape;
  }

  private PointF GetVector(
    int rotationAngle,
    float stringLength,
    RectangleF rect,
    ExcelHAlign horizontalAlign,
    ExcelVAlign verticalAlign,
    SizeF lastCharSize,
    SizeF firstCharSize,
    PdfFont nativeFont,
    PdfStringFormat format)
  {
    PointF vector = PointF.Empty;
    switch (horizontalAlign)
    {
      case ExcelHAlign.HAlignGeneral:
        vector = rotationAngle >= 0 ? this.GetHorizontalRight(rotationAngle, stringLength, rect, verticalAlign, lastCharSize, firstCharSize, nativeFont, format) : this.GetHorizontalLeft(rotationAngle, stringLength, rect, verticalAlign, firstCharSize, nativeFont, format);
        break;
      case ExcelHAlign.HAlignLeft:
        vector = this.GetHorizontalLeft(rotationAngle, stringLength, rect, verticalAlign, firstCharSize, nativeFont, format);
        break;
      case ExcelHAlign.HAlignCenter:
      case ExcelHAlign.HAlignCenterAcrossSelection:
        vector = this.GetHorizontalCenter(rotationAngle, stringLength, rect, verticalAlign, lastCharSize, firstCharSize, nativeFont, format);
        break;
      case ExcelHAlign.HAlignRight:
        vector = this.GetHorizontalRight(rotationAngle, stringLength, rect, verticalAlign, lastCharSize, firstCharSize, nativeFont, format);
        break;
    }
    return vector;
  }

  private PointF GetVector(
    int rotationAngle,
    float width,
    RectangleF adjacentRect,
    ExtendedFormatImpl extendedFormatImpl,
    SizeF lastCharSize,
    SizeF firstCharSize,
    PdfFont nativeFont,
    PdfStringFormat format)
  {
    if (rotationAngle > 90 || rotationAngle < -90)
      throw new ArgumentException("Unexpected rotation angle.");
    return this.GetVector(rotationAngle, width, adjacentRect, extendedFormatImpl.HorizontalAlignment, extendedFormatImpl.VerticalAlignment, lastCharSize, firstCharSize, nativeFont, format);
  }

  private PdfVerticalAlignment GetVerticalAlignmentFromExtendedFormat(IExtendedFormat style)
  {
    PdfVerticalAlignment fromExtendedFormat;
    switch (style.VerticalAlignment)
    {
      case ExcelVAlign.VAlignTop:
        fromExtendedFormat = PdfVerticalAlignment.Top;
        break;
      case ExcelVAlign.VAlignCenter:
        fromExtendedFormat = PdfVerticalAlignment.Middle;
        break;
      default:
        fromExtendedFormat = PdfVerticalAlignment.Bottom;
        break;
    }
    return fromExtendedFormat;
  }

  private PdfVerticalAlignment GetVerticalAlignmentFromShape(IShape shape)
  {
    PdfVerticalAlignment alignmentFromShape;
    switch (shape)
    {
      case AutoShapeImpl _:
        switch (shape.TextFrame.VerticalAlignment)
        {
          case ExcelVerticalAlignment.Middle:
          case ExcelVerticalAlignment.MiddleCentered:
            alignmentFromShape = PdfVerticalAlignment.Middle;
            break;
          case ExcelVerticalAlignment.Bottom:
          case ExcelVerticalAlignment.BottomCentered:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          default:
            alignmentFromShape = PdfVerticalAlignment.Top;
            break;
        }
        break;
      case TextBoxShapeImpl _:
        switch ((shape as TextBoxShapeImpl).VAlignment)
        {
          case ExcelCommentVAlign.Center:
            alignmentFromShape = PdfVerticalAlignment.Middle;
            break;
          case ExcelCommentVAlign.Bottom:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          case ExcelCommentVAlign.Justify:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          case ExcelCommentVAlign.Distributed:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          default:
            alignmentFromShape = PdfVerticalAlignment.Top;
            break;
        }
        break;
      case CommentShapeImpl _:
        switch ((shape as CommentShapeImpl).VAlignment)
        {
          case ExcelCommentVAlign.Center:
            alignmentFromShape = PdfVerticalAlignment.Middle;
            break;
          case ExcelCommentVAlign.Bottom:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          case ExcelCommentVAlign.Justify:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          case ExcelCommentVAlign.Distributed:
            alignmentFromShape = PdfVerticalAlignment.Bottom;
            break;
          default:
            alignmentFromShape = PdfVerticalAlignment.Top;
            break;
        }
        break;
      default:
        alignmentFromShape = PdfVerticalAlignment.Middle;
        break;
    }
    return alignmentFromShape;
  }

  private void SetDocumentProperties()
  {
    this._pdfDocument.DocumentInformation.Author = this._workBook.BuiltInDocumentProperties.Author;
    this._pdfDocument.DocumentInformation.ModificationDate = DateTime.Now;
    this._pdfDocument.DocumentInformation.Creator = this._workBook.BuiltInDocumentProperties.ApplicationName;
    this._pdfDocument.DocumentInformation.Producer = this._workBook.BuiltInDocumentProperties.Company;
    this._pdfDocument.DocumentInformation.Title = this._workBook.BuiltInDocumentProperties.Title;
    this._pdfDocument.DocumentInformation.Subject = this._workBook.BuiltInDocumentProperties.Subject;
    this._pdfDocument.DocumentInformation.Keywords = this._workBook.BuiltInDocumentProperties.Keywords;
    this._pdfDocument.DocumentInformation.CreationDate = this._workBook.BuiltInDocumentProperties.CreationDate;
  }

  private void SetHyperLink(RectangleF cellRect, IRange range)
  {
    if (this._workSheet.HyperLinks.Count == 0)
      return;
    cellRect = this.ReSize(cellRect);
    if (range.Hyperlinks[0].Type == ExcelHyperLinkType.Url)
    {
      PdfUriAnnotation annotation = new PdfUriAnnotation(cellRect, range.Hyperlinks[0].Address);
      annotation.Border.Width = 0.0f;
      this._currentPage.Annotations.Add((PdfAnnotation) annotation);
    }
    else
    {
      if (range.Hyperlinks[0].Type != ExcelHyperLinkType.File)
        return;
      PdfFileLinkAnnotation annotation = new PdfFileLinkAnnotation(cellRect, range.Hyperlinks[0].Address);
      annotation.Border.Width = 0.0f;
      this._currentPage.Annotations.Add((PdfAnnotation) annotation);
    }
  }

  private void SetHyperLink(RectangleF cellRect, IRange range, string formula)
  {
    if (formula == null)
      return;
    string str = this.UpdateHyperLinkFormula(formula);
    IWorksheet worksheet = range.Worksheet;
    range.Formula = str;
    bool flag = false;
    if (worksheet.CalcEngine == null)
    {
      flag = true;
      worksheet.EnableSheetCalculations();
    }
    string calculatedValue = range.CalculatedValue;
    if (flag)
      worksheet.DisableSheetCalculations();
    if (calculatedValue != "")
    {
      cellRect = this.ReSize(cellRect);
      PdfUriAnnotation annotation = new PdfUriAnnotation(cellRect, calculatedValue);
      annotation.Border.Width = 0.0f;
      this._currentPage.Annotations.Add((PdfAnnotation) annotation);
    }
    range.Formula = formula;
  }

  internal string UpdateHyperLinkFormula(string formula)
  {
    formula = formula.TrimStart('=');
    string pattern1 = "HYPERLINK\\([^\\)]+\\)";
    foreach (Match match1 in Regex.Matches(formula, pattern1))
    {
      if (match1.Success)
      {
        string pattern2 = $"HYPERLINK\\((?<group1>\"{{0}}.*\"{{1}}){(object) this._workBook.Application.ArgumentsSeparator}(?<group2>\"{{0}}.*\"{{1}})\\)";
        Match match2 = Regex.Match(match1.ToString(), pattern2);
        if (match2.Success)
        {
          string str1 = match2.Groups["group1"].Value;
          string str2 = match2.Groups["group2"].Value;
          formula = formula.Replace(match1.ToString(), $"HYPERLINK({str2}{(object) this._workBook.Application.ArgumentsSeparator}{str1})");
        }
      }
    }
    return formula;
  }

  private RectangleF ReSize(RectangleF rect)
  {
    return new RectangleF(this._scaledPageWidth / this._pdfPageTemplate.Width * rect.X + this._centerHorizontalValue, this._scaledPageHeight / this._pdfPageTemplate.Height * rect.Y + this._centerVerticalValue, this._scaledPageWidth / this._pdfPageTemplate.Width * rect.Width, this._scaledPageHeight / this._pdfPageTemplate.Height * rect.Height);
  }

  internal IRange GetActualUsedRange(
    IWorksheet sheet,
    IRange usedRange,
    bool checkRow,
    bool checkColumn)
  {
    if (usedRange.Row == usedRange.LastRow && usedRange.Column == usedRange.LastColumn)
      return sheet.Shapes.Count != 0 || sheet.Pictures.Count != 0 || sheet.Charts.Count != 0 || sheet.ListObjects.Count != 0 ? this.UpdateUsedRange(sheet as WorksheetImpl, usedRange) : usedRange;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    List<int> intList4 = new List<int>();
    if (checkRow)
    {
      for (int lastRow = usedRange.LastRow; lastRow >= usedRange.Row; --lastRow)
      {
        if (!this.IsBlank(sheet.Range[lastRow, usedRange.Column, lastRow, usedRange.LastColumn]))
        {
          intList1.Add(lastRow);
          break;
        }
        intList3.Add(lastRow);
      }
    }
    if (checkColumn)
    {
      int num = int.MinValue;
      for (int column = usedRange.Column; column <= usedRange.LastColumn; ++column)
      {
        if (!this.IsBlank(sheet.Range[usedRange.Row, column, usedRange.LastRow, column]))
          num = column;
      }
      if (num != int.MinValue)
      {
        intList2.Add(num);
        for (; num + 1 <= usedRange.LastColumn; ++num)
          intList4.Add(num + 1);
      }
      else
      {
        for (int column = usedRange.Column; column <= usedRange.LastColumn; ++column)
          intList4.Add(column);
      }
    }
    HashSet<Rectangle> mergedRegions = (sheet as WorksheetImpl).MergeCells.MergedRegions;
    if (mergedRegions != null)
    {
      foreach (Rectangle rectangle in mergedRegions)
      {
        int row1 = rectangle.Y + 1;
        int lastRow = rectangle.Height + row1;
        int column = rectangle.X + 1;
        int lastColumn = rectangle.Width + column;
        IRange range = sheet[row1, column, lastRow, lastColumn];
        if (!range.IsBlank || range.CellStyle.Color.Name != "ffffffff")
        {
          if (checkRow)
          {
            if (!intList1.Contains(row1))
              intList1.Add(row1);
            if (!intList1.Contains(lastRow))
              intList1.Add(lastRow);
          }
          if (checkColumn)
          {
            if (range.LastColumn == sheet.Workbook.MaxColumnCount && range.Column <= sheet.Workbook.MaxColumnCount / 2)
            {
              int num = 0;
              for (int row2 = range.Row; row2 <= range.LastRow; ++row2)
              {
                RowStorage row3 = WorksheetHelper.GetOrCreateRow(sheet as IInternalWorksheet, row2 - 1, false);
                if (row3 != null)
                  num = num > row3.LastColumn + 1 ? num : row3.LastColumn + 1;
              }
              if (num > 0 && num < lastColumn)
                column = lastColumn = num;
            }
            if (!intList2.Contains(column))
              intList2.Add(column);
            if (!intList2.Contains(lastColumn))
              intList2.Add(lastColumn);
          }
        }
      }
    }
    intList3.Sort();
    intList4.Sort();
    intList1.Sort();
    intList2.Sort();
    WorksheetImpl sheetImpl = sheet as WorksheetImpl;
    bool flag = false;
    if (checkRow)
    {
      int num = intList1.Count > 0 ? intList1[intList1.Count - 1] : intList3[0];
      if (intList3.Count != 0 && intList3[intList3.Count - 1] > num)
      {
        for (int iRow = intList3[intList3.Count - 1]; iRow >= num && !flag; --iRow)
        {
          for (int column = usedRange.Column; column <= usedRange.LastColumn && !flag; ++column)
          {
            if (!this._stylewithoutBorderFill.Contains(sheetImpl.GetXFIndex(iRow, column)))
            {
              intList1.Add(iRow);
              flag = true;
            }
          }
        }
      }
    }
    if (checkColumn)
    {
      int num1 = int.MinValue;
      int num2 = intList2.Count > 0 ? intList2[intList2.Count - 1] : intList4[0];
      if (intList4.Count != 0 && intList4[intList4.Count - 1] > num2)
      {
        for (int index = intList4[0]; index <= intList4[intList4.Count - 1]; ++index)
        {
          for (int row = usedRange.Row; row <= usedRange.LastRow; ++row)
          {
            if (!this._stylewithoutBorderFill.Contains(sheetImpl.GetXFIndex(row, index)) && sheetImpl.CellRecords.GetExtendedFormatIndex(row, index) > 0)
              num1 = index;
          }
        }
      }
      if (num1 != int.MinValue)
        intList2.Add(num1);
    }
    intList2.Sort();
    intList1.Sort();
    if (checkRow && checkColumn)
    {
      if (intList2.Count != 0 && intList1.Count != 0)
        usedRange = sheet.Range[usedRange.Row, usedRange.Column, intList1[intList1.Count - 1], intList2[intList2.Count - 1]];
    }
    else if (checkRow)
    {
      if (intList1.Count != 0)
        usedRange = sheet.Range[usedRange.Row, usedRange.Column, intList1[intList1.Count - 1], usedRange.LastColumn];
    }
    else if (checkColumn && intList2.Count != 0)
      usedRange = sheet.Range[usedRange.Row, usedRange.Column, usedRange.LastRow, intList2[intList2.Count - 1]];
    return this.UpdateUsedRange(sheetImpl, usedRange);
  }

  private IRange UpdateUsedRange(WorksheetImpl sheetImpl, IRange actualUsedRange)
  {
    actualUsedRange = this.FindListObjectRange((IWorksheet) sheetImpl, actualUsedRange);
    actualUsedRange = this.UpdatePictureRange((IWorksheet) sheetImpl, actualUsedRange);
    actualUsedRange = this.UpdateShapeRange((IWorksheet) sheetImpl, actualUsedRange);
    actualUsedRange = this.UpdateChartsRange((IWorksheet) sheetImpl, actualUsedRange);
    return actualUsedRange;
  }

  private bool IsBlank(IRange range)
  {
    WorksheetImpl worksheet = range.Worksheet as WorksheetImpl;
    int row = range.Row;
    for (int lastRow = range.LastRow; row <= lastRow; ++row)
    {
      if (worksheet.IsRowVisible(row))
      {
        int column = range.Column;
        for (int lastColumn = range.LastColumn; column <= lastColumn; ++column)
        {
          if (worksheet.IsColumnVisible(column) && worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Blank)
            return false;
        }
      }
    }
    return true;
  }

  private void AddBookMark()
  {
    this._pdfDocument.Bookmarks.Add(this._workSheet.Name).Destination = new PdfDestination((PdfPageBase) this._currentPage, new PointF(0.0f, 0.0f));
  }

  private bool CheckRange(IRange tableRange, IRange sheetRange)
  {
    return sheetRange.Row >= tableRange.Row && sheetRange.Column >= tableRange.Column && sheetRange.LastRow <= tableRange.LastRow && sheetRange.LastColumn <= tableRange.LastColumn;
  }

  internal bool CheckUnicode(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0)
        return true;
    }
    return false;
  }

  private bool CheckUnicodeWithoutPunctuation(string unicodeText)
  {
    char[] chArray = !unicodeText.Contains("\n") ? unicodeText.ToCharArray() : unicodeText.Substring(0, unicodeText.IndexOf("\n")).ToCharArray();
    for (int index = 0; index < chArray.Length; ++index)
    {
      if (chArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, chArray[index]) < 0 && (chArray[index] < '‐' || chArray[index] > '⁞'))
        return true;
    }
    return false;
  }

  private bool CheckMergedRegion(IRange iRange)
  {
    for (int row = iRange.Row; row <= iRange.LastRow; ++row)
    {
      for (int column = iRange.Column; column <= iRange.LastColumn; ++column)
      {
        if (this._mergedRegions.ContainsKey(RangeImpl.GetCellIndex(column, row)))
          return true;
      }
    }
    return false;
  }

  private bool CheckIfRTL(ushort[] characterCodes)
  {
    if (characterCodes == null)
      throw new ArgumentNullException(nameof (characterCodes));
    bool flag = false;
    int index = 0;
    for (int length = characterCodes.Length; index < length; ++index)
    {
      if (characterCodes[index] == (ushort) 2 || characterCodes[index] == (ushort) 6)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private PdfPen CreatePen(IBorder border, Color borderColor)
  {
    if (borderColor.IsEmpty)
      borderColor = this.NormalizeColor(border.ColorRGB);
    return new PdfPen(new PdfColor(borderColor), this.GetBorderWidth(border))
    {
      DashStyle = this.GetDashStyle(border)
    };
  }

  private double DegreeToRadian(double angle) => Math.PI / 180.0 * angle;

  private Color NormalizeColor(Color color)
  {
    return color.A == (byte) 0 ? Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B) : color;
  }

  internal float RequiredWidth(float excelSheetWidth, float shHeight, float excelSheetHeight)
  {
    return excelSheetWidth * shHeight / excelSheetHeight;
  }

  internal float RequiredHeight(float excelSheetWidth, float shWidth, float excelSheetHeight)
  {
    return excelSheetHeight * shWidth / excelSheetWidth;
  }

  private void UpdateBorderDelta(
    IWorksheet sheet,
    int row,
    int column,
    int deltaX,
    int deltaY,
    ref int deltaX1,
    ref int deltaY1,
    bool isInvertCondition,
    IBorders borders,
    ExcelBordersIndex start,
    ExcelBordersIndex end,
    bool isLineStart)
  {
    int num1 = row + deltaX;
    int num2 = column + deltaY;
    IWorkbook workbook = sheet.Workbook;
    int maxRowCount = workbook.MaxRowCount;
    int maxColumnCount = workbook.MaxColumnCount;
    if (num1 <= 0 || num1 > maxRowCount || num2 <= 0 || num2 > maxColumnCount)
      return;
    IBorders borders1 = sheet[row + deltaX, column + deltaY].Borders;
    bool flag = borders[end].LineStyle == ExcelLineStyle.Double || borders1[start].LineStyle == ExcelLineStyle.Double;
    if (isInvertCondition)
      flag = !flag;
    int num3 = end == (ExcelBordersIndex) -1 || !flag ? (isLineStart ? 1 : -1) : (isLineStart ? -1 : 1);
    if (deltaY != 0)
    {
      deltaX1 = num3;
    }
    else
    {
      if (deltaX == 0)
        return;
      deltaY1 = num3;
    }
  }

  private RectangleF UpdateRectangleCoordinates(RectangleF cellRect, IBorders borders)
  {
    if (borders[ExcelBordersIndex.EdgeLeft].LineStyle != ExcelLineStyle.None)
    {
      cellRect.Offset(-1f, 0.0f);
      ++cellRect.Width;
    }
    if (borders[ExcelBordersIndex.EdgeTop].LineStyle != ExcelLineStyle.None)
    {
      cellRect.Offset(0.0f, -1f);
      ++cellRect.Height;
    }
    if (borders[ExcelBordersIndex.EdgeBottom].LineStyle != ExcelLineStyle.None)
      ++cellRect.Height;
    if (borders[ExcelBordersIndex.EdgeRight].LineStyle != ExcelLineStyle.None)
      ++cellRect.Width;
    return cellRect;
  }

  private IRange FindListObjectRange(IWorksheet sheetImpl, IRange originalRange)
  {
    foreach (IListObject listObject in (IEnumerable<IListObject>) sheetImpl.ListObjects)
    {
      if (originalRange.LastRow < listObject.Location.LastRow)
        originalRange = sheetImpl.Range[originalRange.Row, originalRange.Column, listObject.Location.LastRow, originalRange.LastColumn];
      if (originalRange.LastColumn < listObject.Location.LastColumn)
        originalRange = sheetImpl.Range[originalRange.Row, originalRange.Column, originalRange.LastRow, listObject.Location.LastColumn];
      if (originalRange.LastColumn < listObject.Location.LastColumn && originalRange.LastRow < listObject.Location.LastRow)
        originalRange = sheetImpl.Range[originalRange.Row, originalRange.Column, listObject.Location.LastRow, listObject.Location.LastColumn];
    }
    return originalRange;
  }

  private IRange UpdatePictureRange(IWorksheet sheetImpl, IRange originalRange)
  {
    int row = originalRange.Row > 0 ? originalRange.Row : 1;
    int column = originalRange.Column > 0 ? originalRange.Column : 1;
    int lastRow = originalRange.LastRow > 0 ? originalRange.LastRow : 1;
    int lastColumn = originalRange.LastColumn > 0 ? originalRange.LastColumn : 1;
    foreach (IPictureShape picture in (IEnumerable) sheetImpl.Pictures)
    {
      ShapeImpl shapeImpl = picture as ShapeImpl;
      if (originalRange.LastRow < shapeImpl.BottomRow)
      {
        originalRange = sheetImpl.Range[row, column, shapeImpl.BottomRow, lastColumn];
        lastRow = shapeImpl.BottomRow;
      }
      if (originalRange.LastColumn < shapeImpl.RightColumn)
      {
        originalRange = sheetImpl.Range[row, column, lastRow, shapeImpl.RightColumn];
        lastColumn = shapeImpl.RightColumn;
      }
    }
    return originalRange;
  }

  private IRange UpdateShapeRange(IWorksheet sheet, IRange originalRange)
  {
    int row = originalRange.Row > 0 ? originalRange.Row : 1;
    int column = originalRange.Column > 0 ? originalRange.Column : 1;
    int lastRow = originalRange.LastRow > 0 ? originalRange.LastRow : 1;
    int lastColumn = originalRange.LastColumn > 0 ? originalRange.LastColumn : 1;
    foreach (IShape shape in (IEnumerable) sheet.Shapes)
    {
      if (!(shape is ICommentShape) || (shape as ICommentShape).IsVisible && this._workSheet.PageSetup.PrintComments == ExcelPrintLocation.PrintInPlace)
      {
        ShapeImpl shapeImpl = shape as ShapeImpl;
        if (originalRange.LastRow < shapeImpl.BottomRow)
        {
          originalRange = sheet.Range[row, column, shapeImpl.BottomRow, lastColumn];
          lastRow = shapeImpl.BottomRow;
        }
        if (originalRange.Row > shapeImpl.TopRow)
        {
          originalRange = sheet.Range[shapeImpl.TopRow, column, lastRow, lastColumn];
          row = shapeImpl.TopRow;
        }
        if (originalRange.LastColumn < shapeImpl.RightColumn)
        {
          originalRange = sheet.Range[row, column, lastRow, shapeImpl.RightColumn];
          lastColumn = shapeImpl.RightColumn;
        }
        if (originalRange.Column > shapeImpl.LeftColumn)
        {
          originalRange = sheet.Range[row, shapeImpl.LeftColumn, lastRow, lastColumn];
          column = shapeImpl.LeftColumn;
        }
      }
    }
    return originalRange;
  }

  private IRange UpdateChartsRange(IWorksheet sheetImpl, IRange originalRange)
  {
    int row = originalRange.Row > 0 ? originalRange.Row : 1;
    int column = originalRange.Column > 0 ? originalRange.Column : 1;
    int lastRow = originalRange.LastRow > 0 ? originalRange.LastRow : 1;
    int lastColumn = originalRange.LastColumn > 0 ? originalRange.LastColumn : 1;
    foreach (IChart chart in (IEnumerable) sheetImpl.Charts)
    {
      ShapeImpl shapeImpl = chart as ShapeImpl;
      if (originalRange.LastRow < shapeImpl.BottomRow)
      {
        originalRange = sheetImpl.Range[row, column, shapeImpl.BottomRow, lastColumn];
        lastRow = shapeImpl.BottomRow;
      }
      if (originalRange.LastColumn < shapeImpl.RightColumn)
      {
        originalRange = sheetImpl.Range[row, column, lastRow, shapeImpl.RightColumn];
        lastColumn = shapeImpl.RightColumn;
      }
    }
    return originalRange;
  }

  internal PdfPageTemplateElement AddPDFHeaderFooter(
    IPageSetupBase PageSetup,
    PdfSection PdfSection,
    bool IsHeader,
    bool isChart)
  {
    float num = this.Pdf_UnitConverter.ConvertUnits((float) PageSetup.TopMargin, PdfGraphicsUnit.Inch, PdfGraphicsUnit.Point);
    HeaderFooter headerFooter;
    PdfPageTemplateElement pageTemplate;
    if (IsHeader)
    {
      headerFooter = this._headerFooter[0];
      float maxHeight = this.GetMaxHeight(headerFooter.HeaderFooterSections);
      float height = (double) maxHeight > (double) num ? maxHeight : num;
      if ((double) height < (double) this.TopMargin)
        height = this.TopMargin;
      pageTemplate = !isChart ? new PdfPageTemplateElement(new RectangleF(0.0f, 0.0f, this._pdfDocument.Pages[0].GetClientSize().Width, height)) : new PdfPageTemplateElement(new RectangleF(0.0f, 0.0f, this._pdfSection.PageSettings.GetActualSize().Width, height));
    }
    else
    {
      headerFooter = this._headerFooter.Count <= 1 ? this._headerFooter[0] : this._headerFooter[1];
      float maxHeight = this.GetMaxHeight(headerFooter.HeaderFooterSections);
      float height = (double) maxHeight > 50.0 ? 50f : maxHeight;
      if ((double) height < (double) this._bottomMargin)
        height = this._bottomMargin;
      pageTemplate = !isChart ? new PdfPageTemplateElement(new RectangleF(0.0f, 0.0f, this._pdfDocument.Pages[0].GetClientSize().Width, height)) : new PdfPageTemplateElement(new RectangleF(0.0f, 0.0f, this._pdfSection.PageSettings.GetActualSize().Width, height));
    }
    foreach (HeaderFooterSection headerFooterSection in headerFooter.HeaderFooterSections)
    {
      if (headerFooterSection.HeaderFooterCollections != null)
        this.AddHeaderFooterSection(pageTemplate, headerFooterSection);
    }
    this._headerFooter.RemoveAt(0);
    return pageTemplate;
  }

  internal void AddHeaderFooterSection(
    PdfPageTemplateElement pageTemplate,
    HeaderFooterSection Section)
  {
    string empty = string.Empty;
    string str1 = string.Empty;
    List<PdfAutomaticField> pdfAutomaticFieldList = new List<PdfAutomaticField>();
    Font font1 = new Font("Times New Roman", 11f);
    Color color = new Color();
    PdfStringFormat pdfStringFormat = new PdfStringFormat();
    pdfStringFormat.ComplexScript = this._excelToPdfSettings.AutoDetectComplexScript;
    PdfPageCountField pdfPageCountField = new PdfPageCountField();
    PdfPageNumberField pdfPageNumberField = new PdfPageNumberField();
    pdfStringFormat.Alignment = Section.TextAlignment;
    int num = 0;
    foreach (KeyValuePair<string, HeaderFooterFontColorSettings> footerCollection in Section.HeaderFooterCollections)
    {
      string str2 = footerCollection.Key.Remove(footerCollection.Key.IndexOf('|'), footerCollection.Key.Length - footerCollection.Key.IndexOf('|'));
      if (footerCollection.Value != null && string.IsNullOrEmpty(str1))
      {
        color = footerCollection.Value.FontColor;
        font1 = footerCollection.Value.Font;
      }
      switch (str2)
      {
        case "&P":
          PdfAutomaticField pdfAutomaticField1 = (PdfAutomaticField) pdfPageNumberField;
          pdfAutomaticFieldList.Add(pdfAutomaticField1);
          str1 = $"{str1}{{{(object) num}}}";
          ++num;
          continue;
        case "&N":
          PdfAutomaticField pdfAutomaticField2 = (PdfAutomaticField) pdfPageCountField;
          pdfAutomaticFieldList.Add(pdfAutomaticField2);
          str1 = $"{str1}{{{(object) num}}}";
          ++num;
          continue;
        default:
          str1 += str2;
          continue;
      }
    }
    bool isEmbedFont = this._excelToPdfSettings.EmbedFonts || Encoding.UTF8.GetByteCount(str1) != str1.Length;
    if (isEmbedFont)
    {
      IFont font2 = (IFont) new FontImpl(this._workBook.Application, (object) this._workSheet, font1);
      string fontName1 = font2.FontName;
      string fontName2 = this.SwitchFonts(str1, (font2 as FontImpl).CharSet, fontName1);
      font1 = this._workBookImpl.GetSystemFont(font2, fontName2);
    }
    PdfSolidBrush brush = new PdfSolidBrush((PdfColor) color);
    PdfAutomaticField[] array = new PdfAutomaticField[pdfAutomaticFieldList.Count];
    pdfAutomaticFieldList.CopyTo(array);
    PdfTrueTypeFont pdfTrueTypeFont = this.GetPdfTrueTypeFont(font1, isEmbedFont, (Stream) null);
    PdfCompositeField pdfCompositeField = pdfAutomaticFieldList.Count <= 0 ? new PdfCompositeField((PdfFont) pdfTrueTypeFont, (PdfBrush) brush, str1) : new PdfCompositeField((PdfFont) pdfTrueTypeFont, (PdfBrush) brush, str1, array);
    pdfCompositeField.Bounds = pageTemplate.Bounds;
    pdfCompositeField.StringFormat = pdfStringFormat;
    pdfCompositeField.StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
    pdfCompositeField.Draw(pageTemplate.Graphics);
    if (pdfTrueTypeFont == null)
      ;
  }

  internal string SwitchFonts(string testString, byte charSet, string fontName)
  {
    if (this.CanDrawString(fontName, testString))
      return fontName;
    if (this.CheckForArabicOrHebrew(testString))
    {
      if (!this._excludefontList.Contains(fontName))
        fontName = "Arial";
    }
    else if (this.CheckForLatin(testString) && System.Array.IndexOf<string>(this._latinfontList, fontName) == -1)
    {
      fontName = "Arial Unicode MS";
    }
    else
    {
      if (this.CheckForCJK(testString))
      {
        if (this.SystemFontsName.Contains("Arial Unicode MS"))
          return "Arial Unicode MS";
        if (this.CanDrawString("MS Gothic", testString))
          return "MS Gothic";
        return !this.CanDrawString("Microsoft JhengHei", testString) ? "Malgun Gothic" : "Microsoft JhengHei";
      }
      if (this.CheckForAmharic(testString))
        fontName = "Ebrima";
      else if (this.CheckForKhmer(testString))
        fontName = "Leelawadee UI";
      else if (this.CheckForMyanmar(testString))
        fontName = "Myanmar Text";
      else if (this.CheckForThai(testString))
        fontName = "Tahoma";
      else if (this.CheckForTamil(testString) || this.CheckForMarathi(testString) || this.CheckForGujarati(testString) || this.CheckForKanndada(testString) || this.CheckForMalayalam(testString) || this.CheckForPunjabi(testString) || this.CheckForSinhala(testString) || this.CheckForTelugu(testString) || this.CheckForBengali(testString) || this.CheckForOdia(testString))
        fontName = "Nirmala UI";
      else if (this.CheckforKorean(testString))
      {
        fontName = "Malgun Gothic";
      }
      else
      {
        if (this.CheckForGeneralPunctuation(testString) || this.CheckForDingbats(testString) || this.CheckForExtendedPlan(testString))
          return "Segoe UI Emoji";
        if (this.CheckForGeneralPunctuationSegeoUI(testString))
          return "Segoe UI";
        if (this.CheckForUnicodeSymbols(testString))
        {
          if (this.CanDrawString("Segoe UI Symbol", testString))
            return "Segoe UI Symbol";
          return !this.CanDrawString("Segoe UI Emoji", testString) ? "Cambria Math" : "Segoe UI Emoji";
        }
      }
    }
    return fontName;
  }

  internal bool CheckForUnicodeSymbols(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] >= '■' && charArray[index] <= '◿' || charArray[index] >= '⬀' && charArray[index] <= '\u2BFF')
        return true;
    }
    return false;
  }

  private bool CanDrawString(string fontName, string testString)
  {
    bool flag1 = false;
    if (this.CheckForMyanmar(testString) || this.CheckforKorean(testString) || this.CheckForMarathi(testString) || this.CheckForGujarati(testString) || this.CheckForKanndada(testString) || this.CheckForBengali(testString) || this.CheckForOdia(testString) || this.CheckForPunjabi(testString))
      flag1 = true;
    bool flag2 = false;
    Font nativeFont = new Font(fontName, 10f);
    if (!nativeFont.Name.Equals("Microsoft Sans Serif", StringComparison.OrdinalIgnoreCase))
    {
      PdfTrueTypeFont pdfFont = this.GetPdfFont(nativeFont, true, (Stream) null) as PdfTrueTypeFont;
      if (flag1)
      {
        pdfFont.MeasureString(testString, new PdfStringFormat()
        {
          ComplexScript = true
        });
        flag2 = pdfFont.IsContainsFont;
      }
      else if (this.CheckForArabicOrHebrew(testString))
      {
        pdfFont.MeasureString(testString, new PdfStringFormat()
        {
          TextDirection = PdfTextDirection.RightToLeft,
          Alignment = PdfTextAlignment.Right,
          ParagraphIndent = 35f
        });
        flag2 = pdfFont.IsContainsFont;
      }
      if (!flag1 && !flag2)
        flag2 = pdfFont.TtfReader.IsFontContainsString(testString);
    }
    nativeFont.Dispose();
    return flag2;
  }

  private bool IsSymbol(string testString)
  {
    return this.CheckForGeneralPunctuation(testString) || this.CheckForDingbats(testString) || this.CheckForExtendedPlan(testString) || this.CheckForGeneralPunctuationSegeoUI(testString);
  }

  private bool CheckForGeneralPunctuationSegeoUI(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    List<int> intList = new List<int>();
    intList.Add(9632);
    intList.Add(9644);
    intList.Add(9650);
    intList.Add(9658);
    intList.Add(9660);
    intList.Add(9668);
    intList.Add(9688);
    intList.Add(9689);
    intList.Add(65533);
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] >= '‐' && charArray[index] <= '‑' || charArray[index] == '\u2029' || charArray[index] >= '♠' && charArray[index] <= '♡' || charArray[index] == '♣' || charArray[index] >= '♥' && charArray[index] <= '♦' || charArray[index] == '♯' || charArray[index] >= '\u2776' && charArray[index] <= '\u277F' || charArray[index] >= 'Ա' && charArray[index] <= 'Ֆ' || charArray[index] >= 'ՙ' && charArray[index] <= '՟' || charArray[index] >= 'ա' && charArray[index] <= '֊' || charArray[index] >= '֍' && charArray[index] <= '֏' || charArray[index] == '\u24FF' || intList.Contains((int) charArray[index])))
        return true;
    }
    return false;
  }

  private bool CheckForExtendedPlan(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    List<int> intList = new List<int>();
    intList.Add(56527);
    intList.Add(56711);
    intList.Add(56720);
    intList.Add(56726);
    intList.Add(56744);
    intList.Add(56764);
    intList.Add(56803);
    intList.Add(56808);
    intList.Add(56815);
    intList.Add(56819);
    intList.Add(57072);
    intList.Add(56787);
    intList.Add(55357);
    intList.Add(55356);
    intList.Add(55358);
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] >= '\uDC00' && charArray[index] <= '\uDC2B' || charArray[index] >= '\uDD70' && charArray[index] <= '\uDD9A' || charArray[index] >= '\uDDE6' && charArray[index] <= '\uDDFF' || charArray[index] >= '\uDE01' && charArray[index] <= '\uDE02' || charArray[index] >= '\uDE10' && charArray[index] <= '\uDE3A' || charArray[index] >= '\uDE50' && charArray[index] <= '\uDE51' || charArray[index] >= '\uDF00' && charArray[index] <= '\uDF21' || charArray[index] >= '\uDF24' && charArray[index] <= '\uDFF0' || charArray[index] >= '\uDFF3' && charArray[index] <= '\uDFF5' || charArray[index] >= '\uDD49' && charArray[index] <= '\uDD67' || charArray[index] >= '\uDD6F' && charArray[index] <= '\uDD70' || charArray[index] >= '\uDD73' && charArray[index] <= '\uDD7A' || charArray[index] >= '\uDD8A' && charArray[index] <= '\uDD8D' || charArray[index] >= '\uDDA4' && charArray[index] <= '\uDDA5' || charArray[index] >= '\uDDB1' && charArray[index] <= '\uDDB2' || charArray[index] >= '\uDDC2' && charArray[index] <= '\uDDC4' || charArray[index] >= '\uDDD1' && charArray[index] <= '\uDDD3' || charArray[index] >= '\uDDDC' && charArray[index] <= '\uDDDE' || charArray[index] >= '\uDE80' && charArray[index] <= '\uDEEC' || charArray[index] >= '\uDD10' && charArray[index] <= '\uDD6B' || charArray[index] >= '\uDDFA' && charArray[index] <= '\uDE4F' || charArray[index] >= '\uDD80' && charArray[index] <= '\uDD97' || charArray[index] >= '\uDDD0' && charArray[index] <= '\uDDE6' || charArray[index] >= '\uDFF7' && charArray[index] <= '\uDFFF' || charArray[index] >= '\uDC00' && charArray[index] <= '\uDD3D' || intList.Contains((int) charArray[index])))
        return true;
    }
    return false;
  }

  private bool CheckForDingbats(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    List<int> intList = new List<int>();
    intList.Add(10013);
    intList.Add(10017);
    intList.Add(10024);
    intList.Add(10071);
    intList.Add(10145);
    intList.Add(10160);
    intList.Add(10175);
    intList.Add(9000);
    intList.Add(9167);
    intList.Add(8505);
    intList.Add(8419);
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] >= '✁' && charArray[index] <= '✅' || charArray[index] >= '✈' && charArray[index] <= '✐' || charArray[index] >= '✒' && charArray[index] <= '✘' || charArray[index] >= '✱' && charArray[index] <= '❌' || charArray[index] >= '❓' && charArray[index] <= '❕' || charArray[index] >= '➕' && charArray[index] <= '➗' || charArray[index] >= '⌚' && charArray[index] <= '⌛' || charArray[index] >= '⏩' && charArray[index] <= '⏳' || charArray[index] >= '⏸' && charArray[index] <= '⏺' || charArray[index] >= '↚' && charArray[index] <= '⇿' || intList.Contains((int) charArray[index])))
        return true;
    }
    return false;
  }

  private bool CheckForGeneralPunctuation(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    List<int> intList = new List<int>();
    intList.Add(12868);
    intList.Add(12869);
    intList.Add(12870);
    intList.Add(12871);
    intList.Add(12951);
    intList.Add(12953);
    intList.Add(57352);
    intList.Add(57353);
    intList.Add(12336);
    intList.Add(12349);
    intList.Add(9752);
    intList.Add(9766);
    intList.Add(9770);
    intList.Add(9855);
    intList.Add(9881);
    intList.Add(9937);
    intList.Add(9949);
    intList.Add(9955);
    intList.Add(9981);
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] == '⁉' || charArray[index] == '⭕' || charArray[index] >= '⬅' && charArray[index] <= '⬇' || charArray[index] >= '⬒' && charArray[index] <= '⬜' || charArray[index] >= '⭐' && charArray[index] <= '⭒' || charArray[index] >= '⤴' && charArray[index] <= '⤷' || charArray[index] >= '☀' && charArray[index] <= '☄' || charArray[index] >= '☎' && charArray[index] <= '☒' || charArray[index] >= '☔' && charArray[index] <= '☕' || charArray[index] >= '☚' && charArray[index] <= '☠' || charArray[index] >= '☢' && charArray[index] <= '☣' || charArray[index] >= '☮' && charArray[index] <= '☯' || charArray[index] >= '☸' && charArray[index] <= '☻' || charArray[index] >= '☿' && charArray[index] <= '♓' || charArray[index] >= '♠' && charArray[index] <= '♨' || charArray[index] >= '♲' && charArray[index] <= '♽' || charArray[index] >= '⚒' && charArray[index] <= '⚗' || charArray[index] >= '⚛' && charArray[index] <= '⚜' || charArray[index] >= '⚠' && charArray[index] <= '⚫' || charArray[index] >= '⚰' && charArray[index] <= '⚱' || charArray[index] >= '⚽' && charArray[index] <= '⚾' || charArray[index] >= '⛄' && charArray[index] <= '⛅' || charArray[index] >= '⛇' && charArray[index] <= '⛈' || charArray[index] >= '⛎' && charArray[index] <= '⛏' || charArray[index] >= '⛓' && charArray[index] <= '⛔' || charArray[index] >= '⛩' && charArray[index] <= '⛪' || charArray[index] >= '⛰' && charArray[index] <= '⛵' || charArray[index] >= '⛷' && charArray[index] <= '⛺' || intList.Contains((int) charArray[index])))
        return true;
    }
    return false;
  }

  private bool CheckForAmharic(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < 'ሀ' || charArray[index] > '\u139F') && (charArray[index] < 'ⶀ' || charArray[index] > '\u2DDF') && (charArray[index] < '\uAB00' || charArray[index] > '\uAB2F'))
        return false;
    }
    return true;
  }

  private bool CheckForKhmer(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    bool flag = true;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < 'ក' || charArray[index] > '\u17FF'))
      {
        flag = false;
        break;
      }
    }
    for (int index = 0; index < charArray.Length && (charArray[index] == ' ' || charArray[index] >= '0' && charArray[index] <= '9' || charArray[index] == ',' || charArray[index] == '.' || charArray[index] == '៛'); ++index)
    {
      if (charArray[index] == '៛')
        flag = true;
    }
    return flag;
  }

  private bool CheckForThai(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0E00' || charArray[index] > '\u0E7F'))
        return false;
    }
    return true;
  }

  private bool CheckForSinhala(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0D80' || charArray[index] > '\u0DFF'))
        return false;
    }
    return true;
  }

  private bool CheckForMyanmar(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < 'က' || charArray[index] > '႟'))
        return false;
    }
    return true;
  }

  private bool CheckForTamil(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0B80' || charArray[index] > '\u0BFF'))
        return false;
    }
    return true;
  }

  private bool CheckForTelugu(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < 'ఀ' || charArray[index] > '౿'))
        return false;
    }
    return true;
  }

  private bool CheckForPunjabi(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0A00' || charArray[index] > '\u0A7F'))
        return false;
    }
    return true;
  }

  private bool CheckForMalayalam(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0D00' || charArray[index] > 'ൿ'))
        return false;
    }
    return true;
  }

  private bool CheckForKanndada(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0C80' || charArray[index] > '\u0CFF'))
        return false;
    }
    return true;
  }

  private bool CheckForGujarati(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0A80' || charArray[index] > '\u0AFF'))
        return false;
    }
    return true;
  }

  private bool CheckForMarathi(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < 'ऀ' || charArray[index] > 'ॿ'))
        return false;
    }
    return true;
  }

  private bool CheckForBengali(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    bool flag = true;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < 'ঀ' || charArray[index] > '\u09FF'))
      {
        flag = false;
        break;
      }
    }
    for (int index = 0; index < charArray.Length && (charArray[index] == ' ' || charArray[index] >= '0' && charArray[index] <= '9' || charArray[index] == ',' || charArray[index] == '.' || charArray[index] == '৳'); ++index)
    {
      if (charArray[index] == '৳')
        flag = true;
    }
    return flag;
  }

  private bool CheckForOdia(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] < '\u0B00' || charArray[index] > '\u0B7F'))
        return false;
    }
    return true;
  }

  private bool CheckForLatin(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && ((charArray[index] < ' ' || charArray[index] > '~') && (charArray[index] < 'Ā' || charArray[index] > 'ͯ') || charArray[index] >= '\u0590' && charArray[index] <= '\u05FF'))
        return false;
    }
    return true;
  }

  private bool CheckForCJK(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] >= '一' && charArray[index] <= '\u9FFF' || charArray[index] >= '\u3040' && charArray[index] <= 'ヿ' || charArray[index] >= 'ｶ' && charArray[index] <= 'ﾝ'))
        return true;
    }
    return false;
  }

  private bool CheckForArabicOrHebrew(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && charArray[index] >= '\u0590' && charArray[index] <= 'ۿ')
        return true;
    }
    return false;
  }

  private bool CheckforKorean(string unicodeText)
  {
    char[] charArray = unicodeText.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] > 'ÿ' && System.Array.IndexOf<char>(this._numberFormatChar, charArray[index]) < 0 && (charArray[index] >= '가' && charArray[index] <= '힣' || charArray[index] >= 'ᄀ' && charArray[index] <= 'ᇿ' || charArray[index] >= '\u3130' && charArray[index] <= '\u318F' || charArray[index] >= 'ꥠ' && charArray[index] <= '\uA97F' || charArray[index] >= 'ힰ' && charArray[index] <= '\uD7FF'))
        return true;
    }
    return false;
  }

  private bool CanDrawShape(
    int firstrow,
    int lastRow,
    int firstColumn,
    int lastColumn,
    ShapeImpl shape,
    out bool inBetween)
  {
    bool flag1 = firstrow <= shape.TopRow && lastRow >= shape.TopRow || firstrow <= shape.BottomRow && lastRow >= shape.BottomRow;
    bool flag2 = firstrow > shape.TopRow && shape.BottomRow > lastRow;
    bool flag3 = firstColumn <= shape.LeftColumn && lastColumn >= shape.LeftColumn || firstColumn <= shape.RightColumn && lastColumn >= shape.RightColumn;
    bool flag4 = firstColumn > shape.LeftColumn && shape.RightColumn > lastColumn;
    inBetween = flag4 && flag1 || flag2 && flag3 || flag4 && flag2;
    return shape.TopRow == 0 || shape.LeftColumn == 0 || flag2 && flag3 || flag4 && flag1 || flag4 && flag2 || shape.TopRow != 0 && shape.LeftColumn != 0 && flag3 && flag1;
  }

  internal bool CanDrawShape(RangeImpl range, IWorksheet sheetImpl)
  {
    int row = range.Row;
    int lastRow = range.LastRow;
    int column = range.Column;
    int lastColumn = range.LastColumn;
    bool flag = false;
    for (int index = 0; !flag && index < sheetImpl.Shapes.Count; ++index)
    {
      ShapeImpl shape = sheetImpl.Shapes[index] as ShapeImpl;
      flag = this.CanDrawShape(row, lastRow, column, lastColumn, shape, out bool _);
    }
    return flag;
  }

  internal List<string> UpdateRTFValues(
    List<string> Drawstring,
    List<IFont> RTFFont,
    out List<IFont> updatedRTFFont,
    PdfStringLayoutResult result)
  {
    List<string> stringList = Drawstring;
    updatedRTFFont = RTFFont;
    int num1 = 0;
    int index1 = 0;
    int num2 = result.Lines[index1].Text.Length + 1;
    for (int index2 = 0; index2 < Drawstring.Count; ++index2)
    {
      stringList[index2] = stringList[index2].Replace("\n", " ");
      num1 += stringList[index2].Length;
      if (num1 >= num2)
      {
        int startIndex = 0;
        int num3 = num1 - stringList[index2].Length;
        string str1 = stringList[index2];
        string str2 = str1.Substring(startIndex, num2 - num3);
        if (str2 != string.Empty)
        {
          string str3 = str1.Substring(startIndex + num2 - num3, str1.Length - (startIndex + num2 - num3));
          if (!string.IsNullOrWhiteSpace(str3))
          {
            stringList.Insert(index2 + 1, str3);
            RTFFont.Insert(index2 + 1, RTFFont[index2]);
          }
          stringList[index2] = str2;
        }
        if (result.LineCount > index1 + 1)
          ++index1;
        num2 = index1 != result.Lines.Length ? result.Lines[index1].Text.Length + 1 : result.Lines[index1].Text.Length;
        num1 = 0;
      }
    }
    updatedRTFFont = RTFFont;
    return stringList;
  }

  internal bool ContainsTransparent(Bitmap image)
  {
    for (int y = 0; y < image.Height; ++y)
    {
      for (int x = 0; x < image.Width; ++x)
      {
        if (image.GetPixel(x, y).A != byte.MaxValue)
          return true;
      }
    }
    return false;
  }

  private enum Alignment
  {
    Left = 1,
    Center = 2,
    Right = 4,
    None = 5,
  }

  private delegate void MergeMethod(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    PdfGraphics graphics,
    float originalWidth,
    float startX,
    float startY);

  private delegate void CellMethod(IRange cell, RectangleF rect, PdfGraphics graphics);
}
