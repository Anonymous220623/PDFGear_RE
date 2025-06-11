// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.PageSetupImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class PageSetupImpl : PageSetupBaseImpl, IPageSetup, IPageSetupBase, IParentApplication
{
  internal static readonly string DEF_AREA_XlS = NameRecord.PREDEFINED_NAMES[6];
  internal static readonly string DEF_AREA_XlSX = NameRecord.PREDEFINED_NAMES[15];
  private static readonly string DEF_TITLE_XLS = NameRecord.PREDEFINED_NAMES[7];
  private static readonly string DEF_TITLE_XLSX = NameRecord.PREDEFINED_NAMES[14];
  private static readonly FormulaToken[] DEF_PRINT_AREA_TOKENS = new FormulaToken[7]
  {
    FormulaToken.tRef3d1,
    FormulaToken.tRef3d2,
    FormulaToken.tRef3d3,
    FormulaToken.tArea3d1,
    FormulaToken.tArea3d2,
    FormulaToken.tArea3d3,
    FormulaToken.tCellRangeList
  };
  private ushort m_usPrintHeaders;
  private ushort m_usPrintGridlines;
  private ushort m_usGridset = 1;
  private GutsRecord m_Guts;
  private DefaultRowHeightRecord m_DefRowHeight;
  private WSBoolRecord m_WSBool;
  private WorksheetImpl m_worksheet;
  private string m_strRelationId;

  public bool PrintGridlines
  {
    get => this.m_usPrintGridlines == (ushort) 1;
    set
    {
      ushort num = value ? (ushort) 1 : (ushort) 0;
      if ((int) this.m_usPrintGridlines == (int) num)
        return;
      this.m_usPrintGridlines = num;
      this.m_usGridset = (ushort) 1;
      this.SetChanged();
    }
  }

  public bool PrintHeadings
  {
    get => this.m_usPrintHeaders != (ushort) 0;
    set
    {
      ushort num = value ? (ushort) 1 : (ushort) 0;
      if ((int) this.m_usPrintHeaders == (int) num)
        return;
      this.m_usPrintHeaders = num;
      this.SetChanged();
    }
  }

  public string PrintArea
  {
    get => this.ExtractPrintArea();
    set
    {
      if (!(value != this.ExtractPrintArea()))
        return;
      this.ParsePrintAreaExpression(value);
    }
  }

  public string PrintTitleColumns
  {
    get => this.ExtractPrintTitleRowColumn(false);
    set
    {
      if (!(value != this.ExtractPrintTitleRowColumn(false)))
        return;
      this.ParsePrintTitleColumns(value);
    }
  }

  public string PrintTitleRows
  {
    get => this.ExtractPrintTitleRowColumn(true);
    set
    {
      if (!(value != this.ExtractPrintTitleRowColumn(true)))
        return;
      this.ParsePrintTitleRows(value);
    }
  }

  public override bool IsFitToPage
  {
    get => this.m_WSBool.IsFitToPage;
    set
    {
      if (this.m_WSBool.IsFitToPage == value)
        return;
      this.m_WSBool.IsFitToPage = value;
      this.SetChanged();
    }
  }

  public bool IsSummaryRowBelow
  {
    get => this.m_WSBool.IsRowSumsBelow;
    set => this.m_WSBool.IsRowSumsBelow = value;
  }

  public bool IsSummaryColumnRight
  {
    get => this.m_WSBool.IsRowSumsRight;
    set => this.m_WSBool.IsRowSumsRight = value;
  }

  public int DefaultRowHeight
  {
    get => (int) this.m_DefRowHeight.Height;
    set => this.m_DefRowHeight.Height = (ushort) value;
  }

  public bool DefaultRowHeightFlag
  {
    get => ((int) this.m_DefRowHeight.OptionFlags & 1) == 1;
    set
    {
      if (value)
        this.m_DefRowHeight.OptionFlags |= (ushort) 1;
      else if (this.m_worksheet.IsZeroHeight)
      {
        this.m_DefRowHeight.OptionFlags |= (ushort) 2;
      }
      else
      {
        DefaultRowHeightRecord defRowHeight = this.m_DefRowHeight;
        int optionFlags = (int) this.m_DefRowHeight.OptionFlags;
        defRowHeight.OptionFlags = (ushort) 0;
      }
    }
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    this.FillGutsRecord();
    return base.GetStoreSize(version) + 2 + 4 + 2 + 4 + 2 + 4 + this.m_Guts.GetStoreSize(version) + 4 + this.m_DefRowHeight.GetStoreSize(version) + 4 + this.m_WSBool.GetStoreSize(version) + 4;
  }

  public string RelationId
  {
    get => this.m_strRelationId;
    set => this.m_strRelationId = value;
  }

  public WorksheetImpl Worksheet => this.m_worksheet;

  public PageSetupImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeCollections();
    this.CreateNecessaryRecords();
  }

  [CLSCompliant(false)]
  public PageSetupImpl(IApplication application, object parent, BiffReader reader)
    : base(application, parent)
  {
    this.InitializeCollections();
    this.Parse(reader);
  }

  [CLSCompliant(false)]
  public PageSetupImpl(
    IApplication application,
    object parent,
    BiffRecordRaw[] data,
    int position)
    : base(application, parent)
  {
    this.InitializeCollections();
    this.Parse((IList<BiffRecordRaw>) data, position);
  }

  public PageSetupImpl(
    IApplication application,
    object parent,
    List<BiffRecordRaw> data,
    int position)
    : base(application, parent)
  {
    this.InitializeCollections();
    this.Parse((IList<BiffRecordRaw>) data, position);
    this.CreateNecessaryRecords();
  }

  protected override void FindParents()
  {
    base.FindParents();
    this.m_worksheet = (WorksheetImpl) (this.FindParent(typeof (WorksheetImpl)) ?? throw new ArgumentException("PageSetup class must be a leaf of Worksheet object tree"));
  }

  private void CreateNecessaryRecords()
  {
    if (this.m_Guts == null)
      this.m_Guts = (GutsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Guts);
    if (this.m_DefRowHeight == null)
      this.m_DefRowHeight = (DefaultRowHeightRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DefaultRowHeight);
    else if (this.m_DefRowHeight.OptionFlags == (ushort) 2)
      this.m_worksheet.IsZeroHeight = true;
    if (this.m_WSBool != null)
      return;
    this.m_WSBool = (WSBoolRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WSBool);
  }

  [CLSCompliant(false)]
  protected override bool ParseRecord(BiffRecordRaw record)
  {
    bool record1 = record != null ? base.ParseRecord(record) : throw new ArgumentNullException(nameof (record));
    if (!record1)
    {
      record1 = true;
      switch (record.TypeCode)
      {
        case TBIFFRecord.VerticalPageBreaks:
          break;
        case TBIFFRecord.HorizontalPageBreaks:
          break;
        case TBIFFRecord.PrintHeaders:
          this.m_usPrintHeaders = ((PrintHeadersRecord) record).IsPrintHeaders;
          break;
        case TBIFFRecord.PrintGridlines:
          this.m_usPrintGridlines = ((PrintGridlinesRecord) record).IsPrintGridlines;
          break;
        case TBIFFRecord.Guts:
          this.m_Guts = (GutsRecord) record;
          break;
        case TBIFFRecord.WSBool:
          this.m_WSBool = (WSBoolRecord) record;
          break;
        case TBIFFRecord.Gridset:
          this.m_usGridset = ((GridsetRecord) record).GridsetFlag;
          break;
        case TBIFFRecord.DefaultRowHeight:
          this.m_DefRowHeight = (DefaultRowHeightRecord) record;
          break;
        default:
          record1 = false;
          break;
      }
    }
    return record1;
  }

  [CLSCompliant(false)]
  public void Parse(BiffReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    throw new NotImplementedException();
  }

  private void SkipUnknownRecords(IList data, ref int pos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (pos < 0 || pos > data.Count)
      throw new ArgumentOutOfRangeException(nameof (pos), "Value cannot be less than 0 and greater than data.Count");
    while (data[pos] is UnknownRecord)
      ++pos;
  }

  [CLSCompliant(false)]
  protected override void SerializeStartRecords(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_Guts == null)
      throw new ArgumentNullException("m_Guts");
    if (this.m_DefRowHeight == null)
      throw new ArgumentNullException("m_DefRowHeight");
    if (this.m_WSBool == null)
      throw new ArgumentNullException("m_WSBool");
    this.FillGutsRecord();
    PrintHeadersRecord record1 = (PrintHeadersRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PrintHeaders);
    record1.IsPrintHeaders = this.m_usPrintHeaders;
    records.Add((IBiffStorage) record1);
    PrintGridlinesRecord record2 = (PrintGridlinesRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PrintGridlines);
    record2.IsPrintGridlines = this.m_usPrintGridlines;
    records.Add((IBiffStorage) record2);
    GridsetRecord record3 = (GridsetRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Gridset);
    record3.GridsetFlag = this.m_usGridset;
    records.Add((IBiffStorage) record3);
    records.Add((IBiffStorage) this.m_Guts);
    records.Add((IBiffStorage) this.m_DefRowHeight);
    records.Add((IBiffStorage) this.m_WSBool);
  }

  protected void FillGutsRecord()
  {
    this.m_Guts.MaxRowLevel = (ushort) 0;
    this.m_Guts.MaxColumnLevel = (ushort) 0;
    int firstRow = this.m_worksheet.FirstRow;
    if (firstRow > 0)
    {
      int iRowIndex = firstRow;
      for (int lastRow = this.m_worksheet.LastRow; iRowIndex <= lastRow; ++iRowIndex)
      {
        IOutline rowOutline = WorksheetHelper.GetRowOutline((IInternalWorksheet) this.m_worksheet, iRowIndex);
        if (rowOutline != null && (int) rowOutline.OutlineLevel > (int) this.m_Guts.MaxRowLevel)
          this.m_Guts.MaxRowLevel = rowOutline.OutlineLevel;
      }
    }
    foreach (IOutline outline in this.m_worksheet.ColumnInformation)
    {
      if (outline != null && (int) outline.OutlineLevel > (int) this.m_Guts.MaxColumnLevel)
        this.m_Guts.MaxColumnLevel = outline.OutlineLevel;
    }
    if (this.m_Guts.MaxRowLevel != (ushort) 0)
    {
      ++this.m_Guts.MaxRowLevel;
      this.m_Guts.LeftRowGutter = (ushort) ((int) this.m_Guts.MaxRowLevel * 14 - 1);
    }
    else
      this.m_Guts.LeftRowGutter = (ushort) 0;
    if (this.m_Guts.MaxColumnLevel != (ushort) 0)
    {
      ++this.m_Guts.MaxColumnLevel;
      this.m_Guts.TopColumnGutter = (ushort) ((int) this.m_Guts.MaxColumnLevel * 14 - 1);
    }
    else
      this.m_Guts.TopColumnGutter = (ushort) 0;
  }

  private void InitializeCollections()
  {
  }

  protected string ConvertTo3dRangeName(string value)
  {
    Match match1 = FormulaUtil.CellRangeRegex.Match(value);
    if (match1.Success)
      return $"'{this.m_worksheet.Name}'!{match1.Result("${Column1}${Row1}:${Column2}${Row2}")}";
    Match match2 = FormulaUtil.CellRegex.Match(value);
    if (match2.Success)
      return $"'{this.m_worksheet.Name}'!{match2.Result("${Column1}${Row1}")}";
    if (FormulaUtil.FullRowRangeRegex.Match(value).Success)
      return $"'{this.m_worksheet.Name}'!{value}";
    return FormulaUtil.FullColumnRangeRegex.Match(value).Success ? $"'{this.m_worksheet.Name}'!{value}" : (string) null;
  }

  protected void ParsePrintAreaExpression(string value)
  {
    if (value == null || value.Length == 0)
    {
      this.m_worksheet.Names.Remove(PageSetupImpl.DEF_AREA_XlS);
    }
    else
    {
      NameImpl nameImpl = this.m_worksheet.Workbook.Version == OfficeVersion.Excel97to2003 ? this.m_worksheet.InnerNames.GetOrCreateName(PageSetupImpl.DEF_AREA_XlS) : this.m_worksheet.InnerNames.GetOrCreateName(PageSetupImpl.DEF_AREA_XlSX);
      NameRecord record = nameImpl.Record;
      int num = 0;
      WorkbookImpl parentWorkbook = this.m_worksheet.ParentWorkbook;
      FormulaUtil formulaUtil = parentWorkbook.FormulaUtil;
      bool r1C1ReferenceMode = parentWorkbook.CalculationOptions.R1C1ReferenceMode;
      int iSheetReference = parentWorkbook.AddSheetReference((IWorksheet) this.m_worksheet);
      Dictionary<Type, ReferenceIndexAttribute> indexes = new Dictionary<Type, ReferenceIndexAttribute>();
      indexes.Add(typeof (Area3DPtg), new ReferenceIndexAttribute(1));
      indexes.Add(typeof (Ref3DPtg), new ReferenceIndexAttribute(1));
      indexes.Add(typeof (AreaPtg), new ReferenceIndexAttribute(1));
      indexes.Add(typeof (RefPtg), new ReferenceIndexAttribute(1));
      OfficeParseFormulaOptions options = r1C1ReferenceMode ? OfficeParseFormulaOptions.InName | OfficeParseFormulaOptions.UseR1C1 : OfficeParseFormulaOptions.InName;
      Ptg[] ptgArray1 = formulaUtil.ParseString(value, (IWorksheet) this.m_worksheet, indexes, 0, (Dictionary<string, string>) null, options, 0, 0);
      int length = ptgArray1.Length;
      Ptg[] ptgArray2 = new Ptg[length];
      OfficeVersion version = this.m_worksheet.ParentWorkbook.Version;
      for (int index = 0; index < length; ++index)
      {
        Ptg ptg = ptgArray1[index];
        if (ptg is IToken3D token3D)
          ptg = token3D.Get3DToken(iSheetReference);
        if (Array.IndexOf<FormulaToken>(PageSetupImpl.DEF_PRINT_AREA_TOKENS, ptg.TokenCode) == -1)
          throw new ArgumentException("Print area has incorrect format");
        ptgArray2[index] = ptg;
        num += ptg.GetSize(version);
      }
      record.FormulaTokens = ptgArray2;
      ((IParseable) nameImpl).Parse();
    }
  }

  protected void ParsePrintTitleColumns(string value)
  {
    bool flag = value != null && value.Length > 0;
    NameRecord record = (this.m_worksheet.Workbook.Version == OfficeVersion.Excel97to2003 ? this.m_worksheet.InnerNames.GetOrCreateName(PageSetupImpl.DEF_TITLE_XLS) : this.m_worksheet.InnerNames.GetOrCreateName(PageSetupImpl.DEF_TITLE_XLSX)).Record;
    Ptg[] formulaTokens = record.FormulaTokens;
    Area3DPtg area3Dptg1 = (Area3DPtg) null;
    if (formulaTokens != null)
    {
      switch (formulaTokens.Length)
      {
        case 1:
          Area3DPtg area3Dptg2 = formulaTokens[0] as Area3DPtg;
          IWorkbook workbook = this.m_worksheet.Workbook;
          if (area3Dptg2.FirstRow != 0 || area3Dptg2.LastRow != workbook.MaxRowCount - 1)
          {
            area3Dptg1 = area3Dptg2;
            break;
          }
          break;
        case 4:
          area3Dptg1 = formulaTokens[2] as Area3DPtg;
          break;
      }
    }
    Ptg ptg1 = (Ptg) null;
    if (flag)
    {
      value = this.ConvertTo3dRangeName(value);
      ptg1 = this.m_worksheet.ParentWorkbook.FormulaUtil.ParseString(value)[0];
      ptg1.TokenCode = FormulaToken.tArea3d1;
    }
    List<Ptg> ptgList = new List<Ptg>();
    if (area3Dptg1 != null && flag)
    {
      WorkbookImpl parentWorkbook = this.m_worksheet.ParentWorkbook;
      FormulaUtil formulaUtil = parentWorkbook.FormulaUtil;
      OfficeVersion version = parentWorkbook.Version;
      Ptg ptg2 = FormulaUtil.CreatePtg(FormulaToken.tCellRangeList, formulaUtil.OperandsSeparator);
      int size = ptg1.GetSize(version) + area3Dptg1.GetSize(version) + ptg2.GetSize(version);
      ptgList.AddRange((IEnumerable<Ptg>) new Ptg[4]
      {
        (Ptg) new MemFuncPtg(size),
        ptg1,
        (Ptg) area3Dptg1,
        ptg2
      });
    }
    else if (area3Dptg1 != null && !flag)
      ptgList.Add((Ptg) area3Dptg1);
    else if (flag)
    {
      ptgList.Add(ptg1);
    }
    else
    {
      this.m_worksheet.Names.Remove(PageSetupImpl.DEF_TITLE_XLS);
      return;
    }
    record.FormulaTokens = ptgList.ToArray();
  }

  protected void ParsePrintTitleRows(string value)
  {
    bool flag = value != null && value.Length > 0;
    NameRecord record = (this.m_worksheet.Workbook.Version == OfficeVersion.Excel97to2003 ? this.m_worksheet.InnerNames.GetOrCreateName(PageSetupImpl.DEF_TITLE_XLS) : this.m_worksheet.InnerNames.GetOrCreateName(PageSetupImpl.DEF_TITLE_XLSX)).Record;
    Ptg[] formulaTokens = record.FormulaTokens;
    Area3DPtg area3Dptg1 = (Area3DPtg) null;
    if (formulaTokens != null)
    {
      switch (formulaTokens.Length)
      {
        case 1:
          Area3DPtg area3Dptg2 = formulaTokens[0] as Area3DPtg;
          IWorkbook workbook = this.m_worksheet.Workbook;
          if (area3Dptg2.FirstRow == 0 || area3Dptg2.LastRow == workbook.MaxRowCount - 1)
          {
            area3Dptg1 = area3Dptg2;
            break;
          }
          break;
        case 4:
          area3Dptg1 = formulaTokens[1] as Area3DPtg;
          break;
      }
    }
    Ptg ptg1 = (Ptg) null;
    if (flag)
    {
      value = this.ConvertTo3dRangeName(value);
      ptg1 = this.m_worksheet.ParentWorkbook.FormulaUtil.ParseString(value)[0];
      ptg1.TokenCode = FormulaToken.tArea3d1;
    }
    List<Ptg> ptgList = new List<Ptg>();
    if (area3Dptg1 != null && flag)
    {
      WorkbookImpl parentWorkbook = this.m_worksheet.ParentWorkbook;
      FormulaUtil formulaUtil = parentWorkbook.FormulaUtil;
      OfficeVersion version = parentWorkbook.Version;
      Ptg ptg2 = FormulaUtil.CreatePtg(FormulaToken.tCellRangeList, formulaUtil.OperandsSeparator);
      int size = area3Dptg1.GetSize(version) + ptg1.GetSize(version) + ptg2.GetSize(version);
      ptgList.AddRange((IEnumerable<Ptg>) new Ptg[4]
      {
        (Ptg) new MemFuncPtg(size),
        (Ptg) area3Dptg1,
        ptg1,
        ptg2
      });
    }
    else if (area3Dptg1 != null && !flag)
      ptgList.Add((Ptg) area3Dptg1);
    else if (flag)
    {
      ptgList.Add(ptg1);
    }
    else
    {
      this.m_worksheet.Names.Remove(PageSetupImpl.DEF_TITLE_XLS);
      return;
    }
    record.FormulaTokens = ptgList.ToArray();
  }

  protected string ExtractPrintArea()
  {
    INames names = this.m_worksheet.Names;
    NameImpl nameImpl = this.m_worksheet.Workbook.Version == OfficeVersion.Excel97to2003 ? (NameImpl) names[PageSetupImpl.DEF_AREA_XlS] : (NameImpl) names[PageSetupImpl.DEF_AREA_XlSX];
    return nameImpl != null ? this.GetAddressGlobalWithoutName(nameImpl.Record.FormulaTokens) : (string) null;
  }

  protected string ExtractPrintTitleRowColumn(bool bRowExtract)
  {
    INames names = this.m_worksheet.Names;
    NameImpl nameImpl = this.m_worksheet.Workbook.Version == OfficeVersion.Excel97to2003 ? (NameImpl) names[PageSetupImpl.DEF_TITLE_XLS] : (NameImpl) names[PageSetupImpl.DEF_TITLE_XLSX];
    if (nameImpl != null)
    {
      Ptg[] formulaTokens = nameImpl.Record.FormulaTokens;
      if (formulaTokens.Length <= 0 || formulaTokens.Length > 4)
        throw new ArgumentOutOfRangeException("Print_Titles Name record", "Print_Titles Name record has wrong quantity of formula tokens.");
      if (formulaTokens.Length == 4)
        return bRowExtract ? this.GetAddressGlobalWithoutName(new Ptg[1]
        {
          formulaTokens[2]
        }) : this.GetAddressGlobalWithoutName(new Ptg[1]
        {
          formulaTokens[1]
        });
      if (formulaTokens.Length == 3)
        return bRowExtract ? this.GetAddressGlobalWithoutName(new Ptg[1]
        {
          formulaTokens[1]
        }) : this.GetAddressGlobalWithoutName(new Ptg[1]
        {
          formulaTokens[0]
        });
      if (formulaTokens.Length == 1)
      {
        string globalWithoutName = this.GetAddressGlobalWithoutName(formulaTokens);
        Area3DPtg area3Dptg = formulaTokens[0] as Area3DPtg;
        return bRowExtract ? (area3Dptg.FirstRow == 0 && area3Dptg.LastRow == this.m_worksheet.ParentWorkbook.MaxRowCount - 1 ? (string) null : globalWithoutName) : (area3Dptg.FirstRow == 0 && area3Dptg.LastRow == this.m_worksheet.ParentWorkbook.MaxRowCount - 1 ? globalWithoutName : (string) null);
      }
    }
    return (string) null;
  }

  protected string GetAddressGlobalWithoutName(Ptg[] token)
  {
    return this.m_worksheet.ParentWorkbook.FormulaUtil.ParsePtgArray(token, 0, 0, false, (NumberFormatInfo) null, true);
  }

  public PageSetupImpl Clone(object parent)
  {
    PageSetupImpl pageSetupImpl = (PageSetupImpl) this.MemberwiseClone();
    pageSetupImpl.SetParent(parent);
    pageSetupImpl.FindParents();
    this.m_Guts = (GutsRecord) CloneUtils.CloneCloneable((ICloneable) this.m_Guts);
    this.m_DefRowHeight = (DefaultRowHeightRecord) CloneUtils.CloneCloneable((ICloneable) this.m_DefRowHeight);
    this.m_WSBool = (WSBoolRecord) CloneUtils.CloneCloneable((ICloneable) this.m_WSBool);
    this.m_unknown = (PrinterSettingsRecord) CloneUtils.CloneCloneable((ICloneable) this.m_unknown);
    this.m_setup = (PrintSetupRecord) CloneUtils.CloneCloneable((ICloneable) this.m_setup);
    if (this.m_headerFooter != null)
      this.m_headerFooter = (HeaderAndFooterRecord) CloneUtils.CloneCloneable((ICloneable) this.m_headerFooter);
    this.m_arrHeaders = CloneUtils.CloneStringArray(this.m_arrHeaders);
    this.m_arrFooters = CloneUtils.CloneStringArray(this.m_arrFooters);
    return pageSetupImpl;
  }

  public override void Dispose()
  {
    base.Dispose();
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
    this.m_arrHeaders = (string[]) null;
    if (this.m_backgroundImage != null)
      this.m_backgroundImage.Dispose();
    this.m_bIsDisposed = true;
  }
}
