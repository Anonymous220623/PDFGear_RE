// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.WorksheetBaseImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Office;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.Xlsb;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public abstract class WorksheetBaseImpl : 
  CommonObject,
  INamedObject,
  IParseable,
  ITabSheet,
  IParentApplication,
  ICloneParent
{
  public const int DEF_MAX_PASSWORDLEN = 255 /*0xFF*/;
  private const ushort DEF_PASSWORD_CONST = 52811;
  [CLSCompliant(false)]
  public const int DEF_MIN_COLUMN_INDEX = 2147483647 /*0x7FFFFFFF*/;
  public const int DEF_MIN_ROW_INDEX = -1;
  public const ExcelKnownColors DEF_DEFAULT_TAB_COLOR = ~ExcelKnownColors.Black;
  [Obsolete("This constant is obsolete and will be removed soon. Please, use MaxRowCount property of the IWorkbook interface. Sorry for inconvenience.")]
  public const int DEF_MAX_ROW_ONE_INDEX = 65536 /*0x010000*/;
  [Obsolete("This constant is obsolete and will be removed soon. Please, use MaxColumnCount property of the IWorkbook interface. Sorry for inconvenience.")]
  public const int DEF_MAX_COLUMN_ONE_INDEX = 256 /*0x0100*/;
  private const int MaxSheetNameLength = 31 /*0x1F*/;
  private const int ProtectionAllOptions = 32767 /*0x7FFF*/;
  private static readonly TBIFFRecord[] DEF_NOTMSORECORDS = new TBIFFRecord[9]
  {
    TBIFFRecord.PivotViewDefinition,
    TBIFFRecord.Note,
    TBIFFRecord.WindowTwo,
    TBIFFRecord.DCON | TBIFFRecord.QuickTip,
    TBIFFRecord.HeaderFooterImage,
    (TBIFFRecord) 237,
    TBIFFRecord.ChartUnits,
    TBIFFRecord.ChartChart,
    TBIFFRecord.DCON
  };
  private static readonly Color DEF_DEFAULT_TAB_COLOR_RGB = ColorExtension.Empty;
  private bool m_bParseOnDemand;
  private bool m_bParseDataOnDemand;
  protected WorkbookImpl m_book;
  private string m_strName = string.Empty;
  private bool m_bChanged = true;
  private int m_iRealIndex;
  protected int m_iMsoStartIndex = -1;
  private int m_iCurMsoIndex;
  protected ExcelParseOptions m_parseOptions;
  private List<BiffRecordRaw> m_arrMSODrawings;
  protected List<BiffRecordRaw> m_arrRecords = new List<BiffRecordRaw>();
  internal ShapesCollection m_shapes;
  internal WorksheetChartsCollection m_charts;
  internal PicturesCollection m_pictures;
  private bool m_bIsSupported = true;
  private int m_iZoom = 100;
  internal SheetProtectionRecord m_sheetProtection;
  protected RangeProtectionRecord m_rangeProtectionRecord;
  private PasswordRecord m_password;
  protected string m_strCodeName;
  private bool m_bParsed = true;
  private bool m_bParsing;
  private bool m_bSkipParsing;
  private WindowTwoRecord m_windowTwo;
  private PageLayoutView m_layout;
  [CLSCompliant(false)]
  protected int m_iFirstColumn = int.MaxValue;
  protected int m_iLastColumn = int.MaxValue;
  protected int m_iFirstRow = -1;
  protected int m_iLastRow = -1;
  internal ColorObject m_tabColor;
  internal HeaderFooterShapeCollection m_headerFooterShapes;
  private int m_iIndex;
  private ExcelSheetProtection m_parseProtection = ~ExcelSheetProtection.None;
  internal BOFRecord m_bof = (BOFRecord) BiffRecordFactory.GetRecord(TBIFFRecord.BOF);
  protected bool KeepRecord;
  private WorksheetVisibility m_visiblity;
  protected internal WorksheetDataHolder m_dataHolder;
  private bool m_bUnknownVmlShapes;
  internal TextBoxCollection m_textBoxes;
  internal CheckBoxCollection m_checkBoxes;
  internal OptionButtonCollection m_optionButtons;
  internal ComboBoxCollection m_comboBoxes;
  private bool m_bTransitionEvaluation;
  protected bool m_isCustomHeight;
  private BiffRecordRaw m_previousRecord;
  protected ErrorIndicatorsCollection m_errorIndicators;
  private string m_algorithmName;
  private byte[] m_hashValue;
  private byte[] m_saltValue;
  private uint m_spinCount;
  internal string sharedBgImageName;
  internal short m_sheetLayoutOptions;

  public WorksheetBaseImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.InitializeCollections();
  }

  [CLSCompliant(false)]
  public WorksheetBaseImpl(
    IApplication application,
    object parent,
    BiffReader reader,
    ExcelParseOptions options,
    bool bSkipParsing,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
    : this(application, parent)
  {
    this.KeepRecord = true;
    this.Parse(reader, options, bSkipParsing, hashNewXFormatIndexes, decryptor);
  }

  internal VbaModule VbaModule
  {
    get
    {
      VbaModule vbaModule = (VbaModule) null;
      if (!(this.Workbook as WorkbookImpl).Loading && this.Workbook.HasMacros && (this.Workbook as WorkbookImpl).VbaProject != null)
      {
        IVbaModules modules = (this.Workbook as WorkbookImpl).VbaProject.Modules;
        if (modules != null)
          vbaModule = modules[this.CodeName] as VbaModule;
      }
      return vbaModule;
    }
  }

  public string Name
  {
    get => this.m_strName;
    set
    {
      if (!(value != this.m_strName))
        return;
      int length = value.Length;
      if (value[0] == '\'' || value[length - 1] == '\'')
        throw new ArgumentOutOfRangeException("Apostrophe can't be used as first and/or last character of the worksheet's name.");
      if (this.m_book.Application.IgnoreSheetNameException && this.m_book.Worksheets[value] != null)
        value = (this.m_book.Worksheets as WorksheetsCollection).GenerateDefaultName(value);
      if (value.Length > 31 /*0x1F*/)
      {
        value = value.Substring(0, 31 /*0x1F*/);
        value = this.GenerateUniqueName(new WorksheetBaseImpl.NameGetter(this.GetName), value);
      }
      ValueChangedEventArgs args = new ValueChangedEventArgs((object) this.m_strName, (object) value, nameof (Name));
      this.m_strName = value;
      this.OnNameChanged(args);
    }
  }

  public bool IsSaved
  {
    get => !this.m_bChanged;
    set => this.m_bChanged = !value;
  }

  protected internal CommentsCollection InnerComments => this.m_shapes.InnerComments;

  protected internal PicturesCollection InnerPictures
  {
    get
    {
      if (this.m_pictures == null)
        this.m_pictures = new PicturesCollection(this.Application, (object) this);
      return this.m_pictures;
    }
  }

  protected internal WorksheetChartsCollection InnerCharts
  {
    get
    {
      this.CheckParseOnDemand();
      if (this.m_charts == null)
        this.m_charts = new WorksheetChartsCollection(this.Application, (object) this);
      return this.m_charts;
    }
  }

  protected internal ShapesCollection InnerShapes
  {
    [DebuggerStepThrough] get
    {
      this.CheckParseOnDemand();
      return this.m_shapes;
    }
  }

  public IShapes Shapes
  {
    get
    {
      this.CheckParseOnDemand();
      return (IShapes) this.m_shapes;
    }
  }

  public ShapeCollectionBase InnerShapesBase
  {
    get
    {
      this.CheckParseOnDemand();
      return (ShapeCollectionBase) this.m_shapes;
    }
    internal set => this.m_shapes = (ShapesCollection) value;
  }

  public HeaderFooterShapeCollection HeaderFooterShapes
  {
    get
    {
      this.CheckParseOnDemand();
      if (this.m_headerFooterShapes == null)
        this.m_headerFooterShapes = new HeaderFooterShapeCollection(this.Application, (object) this);
      return this.m_headerFooterShapes;
    }
  }

  public HeaderFooterShapeCollection InnerHeaderFooterShapes
  {
    get
    {
      this.CheckParseOnDemand();
      return this.m_headerFooterShapes;
    }
    internal set => this.m_headerFooterShapes = value;
  }

  public IComments Comments
  {
    get
    {
      this.CheckParseOnDemand();
      return this.m_shapes.Comments;
    }
  }

  public IChartShapes Charts
  {
    get
    {
      this.CheckParseOnDemand();
      this.ParseData();
      if (this.m_charts == null)
        this.m_charts = new WorksheetChartsCollection(this.Application, (object) this);
      return (IChartShapes) this.m_charts;
    }
  }

  public IPictures Pictures
  {
    get
    {
      this.CheckParseOnDemand();
      if (this.m_pictures == null)
        this.m_pictures = new PicturesCollection(this.Application, (object) this);
      return (IPictures) this.m_pictures;
    }
  }

  internal event ValueChangedEventHandler MacroNameChanged;

  public string CodeName
  {
    get => string.IsNullOrEmpty(this.m_strCodeName) ? this.m_strName : this.m_strCodeName;
    internal set
    {
      string strCodeName = this.m_strCodeName;
      if (this.VbaModule != null)
      {
        this.VbaModule.Name = value;
        if (this.VbaModule != null)
          this.VbaModule.Attributes["VB_NAME"].Value = value;
      }
      this.m_strCodeName = value;
      ValueChangedEventArgs e = new ValueChangedEventArgs((object) strCodeName, (object) value, nameof (CodeName));
      if (this.MacroNameChanged == null)
        return;
      this.MacroNameChanged((object) this, e);
    }
  }

  internal bool HasCodeName => this.m_strCodeName != null;

  [CLSCompliant(false)]
  public WindowTwoRecord WindowTwo
  {
    get
    {
      if (this.m_windowTwo == null)
        this.m_windowTwo = (WindowTwoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WindowTwo);
      if (this.BOF != null && this.BOF.Type == BOFRecord.TType.TYPE_CHART)
        this.m_windowTwo.OriginalLength = 10;
      return this.m_windowTwo;
    }
  }

  public virtual bool ProtectContents
  {
    get => (this.InnerProtection & ExcelSheetProtection.Content) != ExcelSheetProtection.None;
    internal set
    {
      if (value)
        this.InnerProtection |= ExcelSheetProtection.Content;
      else
        this.InnerProtection &= ~ExcelSheetProtection.Content;
    }
  }

  public virtual bool ProtectDrawingObjects
  {
    get
    {
      if (!this.ProtectContents)
        return false;
      return (this.InnerProtection & ExcelSheetProtection.Objects) == ExcelSheetProtection.None || (this.m_parseProtection & ExcelSheetProtection.Objects) != ExcelSheetProtection.None;
    }
  }

  public virtual bool ProtectScenarios
  {
    get
    {
      if (!this.ProtectContents)
        return false;
      return (this.InnerProtection & ExcelSheetProtection.Scenarios) == ExcelSheetProtection.None || (this.m_parseProtection & ExcelSheetProtection.Scenarios) != ExcelSheetProtection.None;
    }
  }

  public bool IsPasswordProtected
  {
    get
    {
      this.CheckParseOnDemand();
      return this.m_password != null && this.m_password.IsPassword != (ushort) 0;
    }
  }

  public bool IsParsed
  {
    get => this.m_bParsed;
    set => this.m_bParsed = value;
  }

  public bool IsParsing
  {
    get => this.m_bParsing;
    set => this.m_bParsing = value;
  }

  public bool IsSkipParsing => this.m_bSkipParsing;

  public bool IsSupported
  {
    get => this.m_bIsSupported;
    protected set => this.m_bIsSupported = value;
  }

  public WorkbookImpl ParentWorkbook => this.m_book;

  public virtual int FirstRow
  {
    get
    {
      this.ParseData();
      return this.m_iFirstRow;
    }
    set => this.m_iFirstRow = value;
  }

  [CLSCompliant(false)]
  public virtual int FirstColumn
  {
    get
    {
      this.ParseData();
      return this.m_iFirstColumn;
    }
    set => this.m_iFirstColumn = value;
  }

  public virtual int LastRow
  {
    get
    {
      this.ParseData();
      return this.m_iLastRow;
    }
    set => this.m_iLastRow = value;
  }

  public virtual int LastColumn
  {
    get
    {
      this.ParseData();
      return this.m_iLastColumn;
    }
    set => this.m_iLastColumn = value;
  }

  public int Zoom
  {
    get => this.m_iZoom;
    set
    {
      this.m_iZoom = value >= 10 && value <= 400 ? value : throw new ArgumentOutOfRangeException(nameof (Zoom), "Zoom must be in range from 10 till 400.");
    }
  }

  public virtual ColorObject TabColorObject
  {
    get
    {
      if (this.m_tabColor == (ColorObject) null)
        this.m_tabColor = new ColorObject(~ExcelKnownColors.Black);
      return this.m_tabColor;
    }
  }

  public virtual ExcelKnownColors TabColor
  {
    get
    {
      return !(this.m_tabColor != (ColorObject) null) ? ~ExcelKnownColors.Black : this.m_tabColor.GetIndexed((IWorkbook) this.m_book);
    }
    set
    {
      if (this.m_tabColor == (ColorObject) null)
        this.m_tabColor = new ColorObject(ExcelKnownColors.None);
      this.m_tabColor.SetIndexed(value);
    }
  }

  public virtual Color TabColorRGB
  {
    get
    {
      return !(this.m_tabColor != (ColorObject) null) ? WorksheetBaseImpl.DEF_DEFAULT_TAB_COLOR_RGB : this.m_tabColor.GetRGB((IWorkbook) this.m_book);
    }
    set
    {
      if (this.m_tabColor == (ColorObject) null)
        this.m_tabColor = new ColorObject(ExcelKnownColors.None);
      this.m_tabColor.SetRGB(value, (IWorkbook) this.m_book);
    }
  }

  public ExcelKnownColors GridLineColor
  {
    get => (ExcelKnownColors) this.m_windowTwo.HeaderColor;
    set
    {
      this.WindowTwo.IsDefaultHeader = false;
      this.WindowTwo.HeaderColor = (int) value;
    }
  }

  public bool DefaultGridlineColor
  {
    get => this.WindowTwo.IsDefaultHeader;
    set => this.WindowTwo.IsDefaultHeader = value;
  }

  public IWorkbook Workbook => (IWorkbook) this.m_book;

  public bool IsRightToLeft
  {
    get => this.WindowTwo.IsArabic;
    set => this.WindowTwo.IsArabic = value;
  }

  public abstract PageSetupBaseImpl PageSetupBase { get; }

  public bool IsSelected => this.WindowTwo.IsSelected;

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  public virtual ExcelSheetProtection Protection
  {
    get
    {
      if (this.m_sheetProtection == null)
        return ExcelSheetProtection.None;
      return this.m_sheetProtection.ProtectedOptions == (int) short.MaxValue ? ExcelSheetProtection.All : (ExcelSheetProtection) this.m_sheetProtection.ProtectedOptions;
    }
  }

  protected internal virtual ExcelSheetProtection InnerProtection
  {
    get
    {
      return this.m_sheetProtection == null ? this.UnprotectedOptions : (ExcelSheetProtection) this.m_sheetProtection.ProtectedOptions;
    }
    internal set => this.m_sheetProtection.ProtectedOptions = (int) value;
  }

  protected virtual ExcelSheetProtection UnprotectedOptions => ExcelSheetProtection.None;

  internal BOFRecord BOF => this.m_bof;

  public WorksheetVisibility Visibility
  {
    get => this.m_visiblity;
    set
    {
      if (this.Visibility == value)
        return;
      WorksheetVisibility visibility = this.Visibility;
      this.m_visiblity = value;
      if (!this.m_book.Loading && value != WorksheetVisibility.Visible)
      {
        int realIndex = this.RealIndex;
        WorkbookObjectsCollection objects = this.m_book.Objects;
        int iIndex1 = realIndex + 1;
        for (int count = objects.Count; iIndex1 < count; ++iIndex1)
        {
          if (this.FindUnhided(objects, iIndex1))
            return;
        }
        for (int iIndex2 = realIndex - 1; iIndex2 >= 0; --iIndex2)
        {
          if (this.FindUnhided(objects, iIndex2))
            return;
        }
        this.m_visiblity = visibility;
        throw new NotSupportedException("A workbook must contain at least one visible worksheet.");
      }
    }
  }

  internal WorksheetDataHolder DataHolder
  {
    get => this.m_dataHolder;
    set
    {
      this.m_dataHolder = value;
      if (value == null)
        return;
      this.m_bParsed = false;
    }
  }

  public int TopVisibleRow
  {
    get => (int) this.WindowTwo.TopRow + 1;
    set
    {
      if (value <= 0)
        throw new ArgumentOutOfRangeException();
      this.WindowTwo.TopRow = (ushort) (value - 1);
    }
  }

  public int LeftVisibleColumn
  {
    get => (int) this.WindowTwo.LeftColumn + 1;
    set
    {
      if (value <= 0)
        throw new ArgumentOutOfRangeException();
      this.WindowTwo.LeftColumn = (ushort) (value - 1);
    }
  }

  internal PasswordRecord Password => this.m_password;

  public bool UnknownVmlShapes
  {
    get => this.m_bUnknownVmlShapes;
    set => this.m_bUnknownVmlShapes = value;
  }

  public TextBoxCollection TypedTextBoxes
  {
    get
    {
      if (this.m_textBoxes == null)
        this.m_textBoxes = new TextBoxCollection(this.Application, (object) this);
      return this.m_textBoxes;
    }
  }

  internal TextBoxCollection InnerTextBoxes
  {
    get
    {
      this.CheckParseOnDemand();
      return this.m_textBoxes;
    }
  }

  public ITextBoxes TextBoxes => (ITextBoxes) this.TypedTextBoxes;

  public CheckBoxCollection TypedCheckBoxes
  {
    get
    {
      this.CheckParseOnDemand();
      if (this.m_checkBoxes == null)
        this.m_checkBoxes = new CheckBoxCollection(this.Application, (object) this);
      return this.m_checkBoxes;
    }
  }

  public OptionButtonCollection TypedOptionButtons
  {
    get
    {
      if (this.m_optionButtons == null)
        this.m_optionButtons = new OptionButtonCollection(this.Application, (object) this);
      return this.m_optionButtons;
    }
  }

  public ComboBoxCollection TypedComboBoxes
  {
    get
    {
      this.CheckParseOnDemand();
      if (this.m_comboBoxes == null)
        this.m_comboBoxes = new ComboBoxCollection(this.Application, (object) this);
      return this.m_comboBoxes;
    }
  }

  protected internal CheckBoxCollection InnerCheckBoxes => this.m_checkBoxes;

  public ICheckBoxes CheckBoxes => (ICheckBoxes) this.TypedCheckBoxes;

  public IOptionButtons OptionButtons => (IOptionButtons) this.TypedOptionButtons;

  public IComboBoxes ComboBoxes => (IComboBoxes) this.TypedComboBoxes;

  public bool HasPictures => this.m_pictures != null && this.m_pictures.Count > 0;

  public bool HasCharts => this.m_charts != null && this.m_charts.Count > 0;

  public bool HasVmlShapes
  {
    get
    {
      return this.UnknownVmlShapes || this.m_checkBoxes != null && this.m_checkBoxes.Count > 0 || this.InnerComments != null && this.InnerComments.Count > 0 || this.m_optionButtons != null && this.m_optionButtons.Count > 0 || this.FindVmlShape();
    }
  }

  private bool FindVmlShape()
  {
    int index = 0;
    for (int count = this.m_shapes.Count; index < count; ++index)
    {
      if ((this.m_shapes[index] as ShapeImpl).VmlShape)
        return true;
    }
    return false;
  }

  public int VmlShapesCount
  {
    get
    {
      int vmlShapesCount = 0;
      int index = 0;
      for (int count = this.m_shapes.Count; index < count; ++index)
      {
        if ((this.m_shapes[index] as ShapeImpl).VmlShape)
          ++vmlShapesCount;
      }
      return vmlShapesCount;
    }
  }

  protected abstract ExcelSheetProtection DefaultProtectionOptions { get; }

  private bool ProtectionMeaningDirect
  {
    get
    {
      return (this.DefaultProtectionOptions & ExcelSheetProtection.Content) != ExcelSheetProtection.None;
    }
  }

  protected virtual bool ContainsProtection
  {
    get => this.m_sheetProtection != null && this.m_sheetProtection.ProtectedOptions != 17408;
  }

  protected SheetProtectionRecord SheetProtection => this.m_sheetProtection;

  public bool IsTransitionEvaluation
  {
    get => this.m_bTransitionEvaluation;
    set => this.m_bTransitionEvaluation = value;
  }

  public bool ParseOnDemand
  {
    get => this.m_bParseOnDemand;
    set => this.m_bParseOnDemand = value;
  }

  internal virtual bool ParseDataOnDemand
  {
    get => this.m_bParseDataOnDemand;
    set => this.m_bParseDataOnDemand = value;
  }

  public string AlgorithmName
  {
    get => this.m_algorithmName;
    set => this.m_algorithmName = value;
  }

  public byte[] HashValue
  {
    get => this.m_hashValue;
    set => this.m_hashValue = value;
  }

  public byte[] SaltValue
  {
    get => this.m_saltValue;
    set => this.m_saltValue = value;
  }

  public uint SpinCount
  {
    get => this.m_spinCount;
    set => this.m_spinCount = value;
  }

  internal VbaModule AddVbaModule()
  {
    VbaProject vbaProject = (this.Workbook as WorkbookImpl).VbaProject as VbaProject;
    VbaModule vbaModule = (VbaModule) null;
    if (vbaProject != null)
    {
      if (string.IsNullOrEmpty(this.m_strCodeName))
        this.m_strCodeName = this.GenerateUniqueName(new WorksheetBaseImpl.NameGetter(this.GetCodeName), "Sheet" + (object) (this.Index + 1));
      if (vbaProject.Modules[this.CodeName] == null)
      {
        vbaModule = vbaProject.Modules.Add(this.CodeName, VbaModuleType.Document) as VbaModule;
        vbaModule.InitializeAttributes(this.CodeName, "0{00020820-0000-0000-C000-000000000046}");
        vbaModule.CodeNameChanged += new NameChangedEventHandler(this.CodeNameChanged);
      }
      else
        vbaModule = vbaProject.Modules[this.CodeName] as VbaModule;
    }
    return vbaModule;
  }

  internal void CodeNameChanged(object sender, string name) => this.m_strCodeName = name;

  internal void ClearEvents() => this.NameChanged = (ValueChangedEventHandler) null;

  private bool FindUnhided(WorkbookObjectsCollection objects, int iIndex)
  {
    if (((WorksheetBaseImpl) objects[iIndex]).Visibility != WorksheetVisibility.Visible)
      return false;
    this.m_book.ActiveSheetIndex = iIndex;
    this.m_book.DisplayedTab = iIndex;
    return true;
  }

  protected virtual void FindParents()
  {
    this.m_book = (WorkbookImpl) (this.FindParent(typeof (WorkbookImpl)) ?? throw new ApplicationException("Worksheet must be a member of Workbook object tree"));
    this.m_iRealIndex = this.m_book.ObjectCount;
  }

  protected virtual void OnNameChanged(ValueChangedEventArgs args)
  {
    this.RaiseNameChangedEvent(args);
    this.SetChanged();
  }

  protected void RaiseNameChangedEvent(ValueChangedEventArgs args)
  {
    if (this.NameChanged == null)
      return;
    this.NameChanged((object) this, args);
  }

  public void SetChanged()
  {
    if (this.m_book.Loading)
      return;
    this.m_book.Saved = false;
    this.m_book.IsCellModified = true;
    this.IsSaved = false;
  }

  protected virtual void InitializeCollections()
  {
    this.m_shapes = new ShapesCollection(this.Application, (object) this);
    this.m_pictures = new PicturesCollection(this.Application, (object) this);
  }

  protected virtual void ClearAll(ExcelWorksheetCopyFlags flags)
  {
    if (this.m_arrMSODrawings != null)
      this.m_arrMSODrawings.Clear();
    if (this.m_shapes != null)
      this.m_shapes.Clear();
    if (this.m_charts != null)
      this.m_charts.Clear();
    if (this.m_pictures != null)
      this.m_pictures.Clear();
    if (this.m_headerFooterShapes == null)
      return;
    this.m_headerFooterShapes.Clear();
  }

  public virtual void Activate()
  {
    if (!this.m_book.Loading && !this.IsSelected)
    {
      for (int Index = 0; Index < this.m_book.Worksheets.Count; ++Index)
        (this.m_book.Worksheets[Index] as WorksheetBaseImpl).WindowTwo.IsSelected = false;
    }
    if ((int) this.m_book.WindowOne.SelectedTab != this.RealIndex && !this.m_book.Loading)
    {
      this.AppImplementation.SetActiveWorksheet(this);
      this.m_book.SetActiveWorksheet(this);
      this.m_book.InnerWorksheetGroup.Add((ITabSheet) this);
    }
    else
    {
      if ((int) this.m_book.WindowOne.SelectedTab == this.RealIndex && !this.m_book.Loading)
        return;
      this.AppImplementation.SetActiveWorksheet(this);
      this.m_book.SetActiveWorksheet(this);
      this.m_book.InnerWorksheetGroup.Select((ITabSheet) this);
    }
  }

  public virtual void Select() => this.Activate();

  public void Unselect() => this.Unselect(true);

  public void Unselect(bool bCheckNumber)
  {
    WindowOneRecord windowOne = this.m_book.WindowOne;
    if (!this.WindowTwo.IsSelected || (!bCheckNumber || windowOne.NumSelectedTabs <= (ushort) 1) && bCheckNumber)
      return;
    this.WindowTwo.IsSelected = false;
    if (this.m_iRealIndex == this.m_book.ActiveSheetIndex)
      return;
    --this.m_book.WindowOne.NumSelectedTabs;
    if (!bCheckNumber)
      return;
    this.m_book.InnerWorksheetGroup.Remove((ITabSheet) this);
  }

  public void Protect(string password) => this.Protect(password, this.DefaultProtectionOptions);

  public void Protect(string password, ExcelSheetProtection options)
  {
    if (this.IsPasswordProtected)
      throw new ApplicationException("Sheet is already protected, before use unprotect method");
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (password.Length > (int) byte.MaxValue)
      throw new ArgumentOutOfRangeException("Length of the password can't be more than " + (object) (int) byte.MaxValue);
    if (this.m_book.Version > ExcelVersion.Excel2010 && password.Length > 0)
      this.AdvancedSheetProtection(password);
    this.Protect(password.Length > 0 ? WorksheetBaseImpl.GetPasswordHash(password) : (ushort) 1, options);
  }

  protected virtual ExcelSheetProtection PrepareProtectionOptions(ExcelSheetProtection options)
  {
    return options;
  }

  public void Unprotect()
  {
    this.m_password = (PasswordRecord) null;
    this.m_sheetProtection = (SheetProtectionRecord) null;
  }

  public void Unprotect(string password)
  {
    if (this.m_password == null && this.m_sheetProtection == null)
      return;
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (password.Length > (int) byte.MaxValue)
      throw new ArgumentOutOfRangeException("Length of the password can't be more than " + (object) (int) byte.MaxValue);
    if (this.AlgorithmName != null)
    {
      if (!SecurityHelper2010.VerifyPassword(password, this.AlgorithmName, this.SaltValue, this.HashValue, this.SpinCount))
        throw new ArgumentException("Wrong password");
      this.m_password = (PasswordRecord) null;
      this.m_sheetProtection = (SheetProtectionRecord) null;
      this.AlgorithmName = (string) null;
      this.HashValue = (byte[]) null;
      this.SaltValue = (byte[]) null;
    }
    else
    {
      if (this.IsPasswordProtected && (this.m_password == null || (int) this.m_password.IsPassword != (int) WorksheetBaseImpl.GetPasswordHash(password) && (this.m_password.IsPassword != (ushort) 1 || !(password == ""))) && (!this.IsPasswordProtected || this.m_password != null || !(password == string.Empty)))
        throw new ArgumentException("Wrong password");
      this.m_password = (PasswordRecord) null;
      this.m_sheetProtection = (SheetProtectionRecord) null;
    }
  }

  protected virtual void OnRealIndexChanged(int iOldIndex)
  {
  }

  public void SelectTab()
  {
    if (this.WindowTwo.IsSelected)
      return;
    this.WindowTwo.IsSelected = true;
  }

  public virtual void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.m_shapes.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public virtual void UpdateExtendedFormatIndex(Dictionary<int, int> dictFormats)
  {
  }

  public virtual object Clone(object parent) => this.Clone(parent, true);

  public virtual object Clone(object parent, bool cloneShapes)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    WorksheetBaseImpl worksheetBaseImpl = (WorksheetBaseImpl) this.MemberwiseClone();
    FileDataHolder dataHolder = new FileDataHolder(this.m_book);
    worksheetBaseImpl.SetParent(parent);
    worksheetBaseImpl.FindParents();
    worksheetBaseImpl.m_password = (PasswordRecord) CloneUtils.CloneCloneable((ICloneable) this.m_password);
    worksheetBaseImpl.m_windowTwo = (WindowTwoRecord) CloneUtils.CloneCloneable((ICloneable) this.m_windowTwo);
    worksheetBaseImpl.m_bof = (BOFRecord) CloneUtils.CloneCloneable((ICloneable) this.m_bof);
    worksheetBaseImpl.m_arrMSODrawings = CloneUtils.CloneCloneable(this.m_arrMSODrawings);
    worksheetBaseImpl.m_arrRecords = CloneUtils.CloneCloneable(this.m_arrRecords);
    if (this.m_charts != null)
      worksheetBaseImpl.m_charts = new WorksheetChartsCollection(this.Application, (object) worksheetBaseImpl);
    if (this.m_pictures != null)
      worksheetBaseImpl.m_pictures = new PicturesCollection(this.Application, (object) worksheetBaseImpl);
    if (cloneShapes)
      this.CloneShapes(worksheetBaseImpl);
    worksheetBaseImpl.m_headerFooterShapes = (HeaderFooterShapeCollection) CloneUtils.CloneCloneable((ICloneParent) this.m_headerFooterShapes, (object) worksheetBaseImpl);
    if (this.m_dataHolder != null)
      worksheetBaseImpl.m_dataHolder = worksheetBaseImpl.m_book.DataHolder != null ? this.m_dataHolder.Clone(worksheetBaseImpl.m_book.DataHolder) : this.m_dataHolder.Clone(dataHolder);
    return (object) worksheetBaseImpl;
  }

  public void CloneShapes(WorksheetBaseImpl result)
  {
    result.m_shapes = (ShapesCollection) this.m_shapes.Clone((object) result);
  }

  protected internal virtual void UpdateStyleIndexes(int[] styleIndexes)
  {
    if (styleIndexes == null)
      throw new ArgumentNullException(nameof (styleIndexes));
  }

  internal void Protect(ushort password, ExcelSheetProtection options)
  {
    if (this.m_password == null)
      this.m_password = (PasswordRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Password);
    options = this.PrepareProtectionOptions(options);
    this.m_password.IsPassword = password;
    this.m_sheetProtection = (SheetProtectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SheetProtection);
    this.m_sheetProtection.ProtectedOptions = (int) (ushort) options;
    this.m_sheetProtection.ContainProtection = true;
  }

  public abstract void MarkUsedReferences(bool[] usedItems);

  public abstract void UpdateReferenceIndexes(int[] arrUpdatedIndexes);

  [CLSCompliant(false)]
  protected internal void Parse(
    BiffReader reader,
    ExcelParseOptions options,
    bool bSkipParsing,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    int iBOFCounter = 0;
    bool bSkipStyles = false;
    do
    {
      iBOFCounter = this.ParseNextRecord(reader, iBOFCounter, options, bSkipStyles, hashNewXFormatIndexes, decryptor);
    }
    while (iBOFCounter != 0 && !reader.IsEOF);
    this.PrepareProtection();
    this.m_bParsed = false;
    this.m_bSkipParsing = bSkipParsing;
    this.IsSaved = true;
  }

  protected void PrepareProtection()
  {
    if (this.m_sheetProtection != null || this.m_parseProtection == ExcelSheetProtection.None || this.m_parseProtection == ~ExcelSheetProtection.None)
      return;
    this.m_sheetProtection = (SheetProtectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SheetProtection);
    this.m_sheetProtection.ProtectedOptions = (int) this.m_parseProtection;
    this.m_sheetProtection.ContainProtection = true;
  }

  [CLSCompliant(false)]
  protected virtual int ParseNextRecord(
    BiffReader reader,
    int iBOFCounter,
    ExcelParseOptions options,
    bool bSkipStyles,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    BiffRecordRaw record = reader.GetRecord(decryptor);
    if (this.ParseOnDemand)
    {
      switch (record.TypeCode)
      {
        case TBIFFRecord.EOF:
          --iBOFCounter;
          if (this.KeepRecord)
          {
            this.m_arrRecords.Add(record);
            break;
          }
          break;
        case TBIFFRecord.BOF:
          ++iBOFCounter;
          if (iBOFCounter == 1)
          {
            this.m_bof = (BOFRecord) record;
            BOFRecord.TType type = this.m_bof.Type;
            this.m_bIsSupported = type == BOFRecord.TType.TYPE_WORKSHEET || type == BOFRecord.TType.TYPE_CHART;
          }
          if (this.KeepRecord)
          {
            this.m_arrRecords.Add(record);
            break;
          }
          break;
        default:
          if (this.KeepRecord)
          {
            this.m_arrRecords.Add(record);
            break;
          }
          break;
      }
    }
    else
    {
      if (bSkipStyles && !this.m_book.ModifyRecordToSkipStyle(record))
        return iBOFCounter;
      if (this.KeepRecord)
        this.m_arrRecords.Add(record);
      if (iBOFCounter <= 1)
      {
        switch (record.TypeCode)
        {
          case TBIFFRecord.EOF:
            --iBOFCounter;
            break;
          case TBIFFRecord.Protect:
            this.ParseProtect((ProtectRecord) record);
            break;
          case TBIFFRecord.Password:
            this.ParsePassword((PasswordRecord) record);
            break;
          case TBIFFRecord.WindowProtect:
            if (((WindowProtectRecord) record).IsProtected && !this.Workbook.IsWindowProtection && !this.Workbook.IsCellProtection)
            {
              this.Workbook.Protect(true, false);
              break;
            }
            break;
          case TBIFFRecord.ObjectProtect:
            this.ParseObjectProtect((ObjectProtectRecord) record);
            break;
          case TBIFFRecord.WindowZoom:
            this.ParseWindowZoom((WindowZoomRecord) record);
            break;
          case TBIFFRecord.ScenProtect:
            this.ParseScenProtect((ScenProtectRecord) record);
            break;
          case TBIFFRecord.MSODrawing:
            if (!this.KeepRecord)
            {
              this.KeepRecord = true;
              this.m_arrRecords.Add(record);
            }
            if (this.m_iMsoStartIndex < 0)
            {
              this.m_iMsoStartIndex = this.m_arrRecords.Count - 1;
              break;
            }
            break;
          case TBIFFRecord.CodeName:
            this.m_strCodeName = ((CodeNameRecord) record).CodeName;
            break;
          case TBIFFRecord.Dimensions:
            this.ParseDimensions((DimensionsRecord) record);
            break;
          case TBIFFRecord.DefaultRowHeight:
            this.m_isCustomHeight = ((DefaultRowHeightRecord) record).CustomHeight;
            break;
          case TBIFFRecord.WindowTwo:
            this.ParseWindowTwo((WindowTwoRecord) record);
            break;
          case TBIFFRecord.BOF:
            ++iBOFCounter;
            if (iBOFCounter == 1)
            {
              this.m_bof = (BOFRecord) record;
              BOFRecord.TType type = this.m_bof.Type;
              this.m_bIsSupported = type == BOFRecord.TType.TYPE_WORKSHEET || type == BOFRecord.TType.TYPE_CHART;
              break;
            }
            break;
          case TBIFFRecord.ContinueFrt:
            if (this.m_previousRecord != null && this.m_previousRecord == (RangeProtectionRecord) this.m_previousRecord)
            {
              byte[] arrDest = new byte[8221];
              reader.DataProvider.ReadArray(0, arrDest);
              RangeProtectionRecord previousRecord = this.m_previousRecord as RangeProtectionRecord;
              if (previousRecord.m_continueRecords == null)
                previousRecord.m_continueRecords = new List<UnknownRecord>();
              previousRecord.m_continueRecords.Add((UnknownRecord) record);
              if (record.Length < arrDest.Length)
              {
                int iOffset = 8065;
                ExcelIgnoreError option = (ExcelIgnoreError) reader.DataProvider.ReadUInt16(iOffset);
                if (option != ExcelIgnoreError.None)
                {
                  previousRecord.ErrorIndicator = new ErrorIndicatorImpl(option);
                  this.m_errorIndicators.Add(previousRecord.ErrorIndicator);
                  break;
                }
                break;
              }
              break;
            }
            this.ParseRecord(record, bSkipStyles, hashNewXFormatIndexes);
            break;
          case TBIFFRecord.SheetLayout:
            this.ParseSheetLayout((SheetLayoutRecord) record);
            break;
          case TBIFFRecord.HeaderFooterImage:
            this.HeaderFooterShapes.ParseMsoStructures(((MSODrawingGroupRecord) record).StructuresList, options);
            break;
          case TBIFFRecord.SheetProtection:
            SheetProtectionRecord protectionRecord = (SheetProtectionRecord) record;
            if (this.m_parseProtection != ~ExcelSheetProtection.None && protectionRecord.ContainProtection && (this.ProtectionMeaningDirect && (this.m_parseProtection & ExcelSheetProtection.Content) != ExcelSheetProtection.None || !this.ProtectionMeaningDirect && (this.m_parseProtection & ExcelSheetProtection.Content) == ExcelSheetProtection.None))
            {
              this.m_sheetProtection = protectionRecord;
              break;
            }
            break;
          case TBIFFRecord.RangeProtection:
            this.m_previousRecord = record;
            this.m_rangeProtectionRecord = (RangeProtectionRecord) record;
            break;
          case TBIFFRecord.PageLayoutView:
            this.ParsePageLayoutView((PageLayoutView) record);
            break;
          default:
            this.ParseRecord(record, bSkipStyles, hashNewXFormatIndexes);
            break;
        }
      }
      else if (record.TypeCode == TBIFFRecord.EOF)
        --iBOFCounter;
      else if (record.TypeCode == TBIFFRecord.BOF)
        ++iBOFCounter;
    }
    return iBOFCounter;
  }

  [CLSCompliant(false)]
  protected void ParseProtect(ProtectRecord protectRecord)
  {
    if (!protectRecord.IsProtected)
      return;
    this.m_parseProtection = this.DefaultProtectionOptions;
    if (this.ProtectionMeaningDirect)
      this.m_parseProtection |= ExcelSheetProtection.Content;
    else
      this.m_parseProtection &= ~ExcelSheetProtection.Content;
  }

  [CLSCompliant(false)]
  protected void ParsePassword(PasswordRecord passwordRecord) => this.m_password = passwordRecord;

  [CLSCompliant(false)]
  protected void ParseObjectProtect(ObjectProtectRecord objectProtect)
  {
    if (!objectProtect.IsProtected)
      return;
    if (this.ProtectionMeaningDirect)
      this.m_parseProtection |= ExcelSheetProtection.Objects;
    else
      this.m_parseProtection &= ~ExcelSheetProtection.Objects;
  }

  [CLSCompliant(false)]
  protected void ParseScenProtect(ScenProtectRecord scenProtect)
  {
    if (!scenProtect.IsProtected)
      return;
    if (this.ProtectionMeaningDirect)
      this.m_parseProtection |= ExcelSheetProtection.Scenarios;
    else
      this.m_parseProtection &= ~ExcelSheetProtection.Scenarios;
  }

  protected virtual void PrepareVariables(ExcelParseOptions options, bool bSkipParsing)
  {
    this.m_arrRecords.Clear();
    this.m_iMsoStartIndex = -1;
    this.m_parseOptions = options;
  }

  [CLSCompliant(false)]
  private void ParsePageLayoutView(PageLayoutView layout)
  {
    if (layout == null)
      throw new ArgumentNullException("windowTwo");
    WorksheetImpl worksheetImpl = this as WorksheetImpl;
    if (layout.LayoutView)
      worksheetImpl.View = SheetView.PageLayout;
    this.m_layout = layout;
  }

  [CLSCompliant(false)]
  protected virtual void ParseWindowTwo(WindowTwoRecord windowTwo)
  {
    this.m_windowTwo = windowTwo != null ? windowTwo : throw new ArgumentNullException(nameof (windowTwo));
    if (this.m_windowTwo.IsSelected)
      this.m_book.WorksheetGroup.Add((ITabSheet) this);
    if (!this.m_windowTwo.IsSavedInPageBreakPreview || !(this is WorksheetImpl))
      return;
    (this as WorksheetImpl).View = SheetView.PageBreakPreview;
  }

  [CLSCompliant(false)]
  protected virtual void ParseRecord(
    BiffRecordRaw raw,
    bool bIgnoreStyles,
    Dictionary<int, int> hashNewXFormatIndexes)
  {
  }

  [CLSCompliant(false)]
  protected virtual void ParseDimensions(DimensionsRecord dimensions)
  {
    if (dimensions == null)
      throw new ArgumentNullException(nameof (dimensions));
    if (dimensions.LastColumn == (ushort) 0 && dimensions.LastRow == 0)
    {
      this.m_iFirstColumn = int.MaxValue;
      this.m_iFirstRow = -1;
      this.m_iLastColumn = int.MaxValue;
      this.m_iLastRow = -1;
    }
    else
    {
      this.m_iFirstColumn = (int) dimensions.FirstColumn + 1;
      this.m_iFirstRow = dimensions.FirstRow + 1;
      this.m_iLastColumn = Math.Min((int) dimensions.LastColumn, this.m_book.MaxColumnCount);
      if (this.m_iLastColumn == 0)
        this.m_iLastColumn = 1;
      this.m_iLastRow = Math.Min(dimensions.LastRow, this.m_book.MaxRowCount);
      if (this.m_iLastRow != 0)
        return;
      this.m_iLastRow = 1;
    }
  }

  [CLSCompliant(false)]
  protected virtual void ParseWindowZoom(WindowZoomRecord windowZoom)
  {
    this.m_iZoom = windowZoom != null ? windowZoom.Zoom : throw new ArgumentNullException(nameof (windowZoom));
  }

  [CLSCompliant(false)]
  protected void ParseSheetLayout(SheetLayoutRecord sheetLayout)
  {
    if (sheetLayout == null)
      throw new ArgumentNullException(nameof (sheetLayout));
    switch (sheetLayout.ColorType)
    {
      case ColorType.Automatic:
      case ColorType.Indexed:
      case ColorType.None:
        this.TabColor = (ExcelKnownColors) sheetLayout.ColorIndex;
        break;
      case ColorType.RGB:
        byte alpha = (byte) (sheetLayout.ColorValue >> 24);
        byte blue = (byte) (sheetLayout.ColorValue >> 16 /*0x10*/);
        byte green = (byte) (sheetLayout.ColorValue >> 8);
        byte colorValue = (byte) sheetLayout.ColorValue;
        this.TabColorRGB = Color.FromArgb((int) alpha, (int) colorValue, (int) green, (int) blue);
        this.m_tabColor.Tint = sheetLayout.TintShade;
        break;
      case ColorType.Theme:
        this.m_tabColor = new ColorObject(ColorType.Theme, sheetLayout.ColorValue, sheetLayout.TintShade);
        break;
    }
    this.m_sheetLayoutOptions = sheetLayout.Options;
  }

  [CLSCompliant(false)]
  public virtual void Serialize(OffsetArrayList records) => throw new NotImplementedException();

  [CLSCompliant(false)]
  protected virtual void SerializeMsoDrawings(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_shapes == null || this.m_shapes.Count <= 0 || (this.Application.SkipOnSave & SkipExtRecords.Drawings) == SkipExtRecords.Drawings)
      return;
    this.m_shapes.Serialize(records);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeProtection(OffsetArrayList records, bool bContentNotNecessary)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (bContentNotNecessary || this.ProtectContents)
    {
      if (this.ProtectContents)
      {
        ProtectRecord record = (ProtectRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Protect);
        record.IsProtected = true;
        records.Add((IBiffStorage) record);
      }
      if (this.ProtectScenarios)
      {
        ScenProtectRecord record = (ScenProtectRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ScenProtect);
        record.IsProtected = true;
        records.Add((IBiffStorage) record);
      }
      if (this.ProtectDrawingObjects)
      {
        ObjectProtectRecord record = (ObjectProtectRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ObjectProtect);
        record.IsProtected = true;
        records.Add((IBiffStorage) record);
      }
    }
    if (this.m_password == null || this.m_password.IsPassword == (ushort) 1)
      return;
    records.Add((IBiffStorage) this.m_password);
  }

  [CLSCompliant(false)]
  protected void SerializeSheetProtection(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_sheetProtection == null)
      return;
    SheetProtectionRecord protectionRecord = (SheetProtectionRecord) this.m_sheetProtection.Clone();
    protectionRecord.ProtectedOptions &= -32769;
    records.Add((IBiffStorage) protectionRecord);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeHeaderFooterPictures(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_headerFooterShapes == null || this.m_headerFooterShapes.Count <= 0)
      return;
    this.m_headerFooterShapes.Serialize(records);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeWindowTwo(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this == this.m_book.ActiveSheet)
    {
      this.WindowTwo.IsPaged = true;
      this.WindowTwo.IsSelected = true;
    }
    records.Add((IBiffStorage) this.WindowTwo);
  }

  [CLSCompliant(false)]
  protected virtual void SerializePageLayoutView(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    WorksheetImpl worksheetImpl = this as WorksheetImpl;
    if (this.m_layout == null || worksheetImpl.View != SheetView.PageLayout)
      return;
    this.m_layout.LayoutView = true;
    records.Add((IBiffStorage) this.m_layout);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeMacrosSupport(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if ((this.Application.SkipOnSave & SkipExtRecords.Macros) == SkipExtRecords.Macros || this.m_strCodeName == null)
      return;
    CodeNameRecord record = (CodeNameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CodeName);
    record.CodeName = this.m_strCodeName;
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  protected void SerializeWindowZoom(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    WindowZoomRecord record = (WindowZoomRecord) BiffRecordFactory.GetRecord(TBIFFRecord.WindowZoom);
    record.Zoom = this.m_iZoom;
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  protected void SerializeSheetLayout(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (!(this.m_tabColor != (ColorObject) null))
      return;
    SheetLayoutRecord record = (SheetLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SheetLayout);
    record.ColorIndex = (int) this.m_tabColor.GetIndexed((IWorkbook) this.m_book);
    if (this.m_tabColor.ColorType == ColorType.RGB || this.m_tabColor.ColorType == ColorType.Theme)
    {
      record.ColorType = this.m_tabColor.ColorType;
      if (record.ColorType == ColorType.RGB)
      {
        byte num1 = (byte) (this.m_tabColor.m_color >> 24);
        byte num2 = (byte) (this.m_tabColor.m_color >> 16 /*0x10*/);
        byte num3 = (byte) (this.m_tabColor.m_color >> 8);
        byte color = (byte) this.m_tabColor.m_color;
        record.ColorValue = BitConverter.ToInt32(new byte[4]
        {
          num2,
          num3,
          color,
          num1
        }, 0);
      }
      else
        record.ColorValue = this.m_tabColor.m_color;
      record.TintShade = this.m_tabColor.Tint;
      record.Options = this.m_sheetLayoutOptions;
      record.Unknown = 40;
    }
    records.Add((IBiffStorage) record);
  }

  [CLSCompliant(false)]
  public static ushort GetPasswordHash(string password)
  {
    if (password == null)
      return 0;
    ushort num = 0;
    int index = 0;
    for (int length = password.Length; index < length; ++index)
    {
      ushort uint16FromBits = WorksheetBaseImpl.GetUInt16FromBits(WorksheetBaseImpl.RotateBits(WorksheetBaseImpl.GetCharBits15(password[index]), index + 1));
      num ^= uint16FromBits;
    }
    return (ushort) ((int) num ^ password.Length ^ 52811);
  }

  private static bool[] GetCharBits15(char charToConvert)
  {
    bool[] charBits15 = new bool[15];
    ushort uint16 = Convert.ToUInt16(charToConvert);
    ushort num = 1;
    for (int index = 0; index < 15; ++index)
    {
      charBits15[index] = ((int) uint16 & (int) num) == (int) num;
      num <<= 1;
    }
    return charBits15;
  }

  private static ushort GetUInt16FromBits(bool[] bits)
  {
    if (bits == null)
      throw new ArgumentNullException(nameof (bits));
    if (bits.Length > 16 /*0x10*/)
      throw new ArgumentOutOfRangeException("There can't be more than 16 bits");
    ushort uint16FromBits = 0;
    ushort num = 1;
    int index = 0;
    for (int length = bits.Length; index < length; ++index)
    {
      if (bits[index])
        uint16FromBits += num;
      num <<= 1;
    }
    return uint16FromBits;
  }

  private static bool[] RotateBits(bool[] bits, int count)
  {
    if (bits == null)
      throw new ArgumentNullException(nameof (bits));
    if (bits.Length == 0)
      return bits;
    if (count < 0)
      throw new ArgumentOutOfRangeException("Count can't be less than zero");
    bool[] flagArray = new bool[bits.Length];
    int index1 = 0;
    for (int length = bits.Length; index1 < length; ++index1)
    {
      int index2 = (index1 + count) % length;
      flagArray[index2] = bits[index1];
    }
    return flagArray;
  }

  public static int Round(int value, int degree)
  {
    if (degree == 0)
      throw new ArgumentOutOfRangeException("degree can't be 0");
    int num = value % degree;
    return value - num + degree;
  }

  [Category("Property Changed")]
  public event ValueChangedEventHandler NameChanged;

  public int RealIndex
  {
    get => this.m_iRealIndex;
    set
    {
      if (this.m_iRealIndex == value)
        return;
      int iRealIndex = this.m_iRealIndex;
      this.m_iRealIndex = value;
      this.OnRealIndexChanged(iRealIndex);
    }
  }

  int ITabSheet.TabIndex => this.m_iRealIndex;

  public virtual void Parse()
  {
    if (!this.IsParsed)
      this.IsParsed = false;
    this.ParseData();
    bool isParsed = this.IsParsed;
    this.IsParsing = true;
    this.IsParsed = false;
    this.ExtractMSODrawing(this.m_iMsoStartIndex, this.m_parseOptions);
    this.IsParsing = false;
    this.IsParsed = isParsed;
    if (!this.IsSupported || !this.IsParsed)
      return;
    this.m_arrRecords.Clear();
  }

  protected internal void ParseData() => this.ParseData((Dictionary<int, int>) null);

  protected internal abstract void ParseData(Dictionary<int, int> dictUpdatedSSTIndexes);

  internal abstract void ParseBinaryData(
    Dictionary<int, int> dictUpdatedSSTIndexes,
    XlsbDataHolder holder);

  protected void ExtractMSODrawing(int startIndex, ExcelParseOptions options)
  {
    if (this.m_arrMSODrawings != null)
      this.m_arrMSODrawings.Clear();
    else
      this.m_arrMSODrawings = new List<BiffRecordRaw>();
    if (startIndex < 0)
      return;
    int index1 = startIndex;
    int count = this.m_arrRecords.Count;
    int num1 = 0;
    for (; index1 < count; ++index1)
    {
      TBIFFRecord typeCode = this.m_arrRecords[index1].TypeCode;
      if (num1 != 0 || Array.IndexOf<TBIFFRecord>(WorksheetBaseImpl.DEF_NOTMSORECORDS, typeCode) == -1)
      {
        switch (typeCode)
        {
          case TBIFFRecord.EOF:
            --num1;
            goto default;
          case TBIFFRecord.Continue:
            MSODrawingRecord record = BiffRecordFactory.GetRecord(TBIFFRecord.MSODrawing) as MSODrawingRecord;
            ContinueRecord arrRecord1 = this.m_arrRecords[index1] as ContinueRecord;
            record.m_data = new byte[arrRecord1.m_data.Length];
            arrRecord1.m_data.CopyTo((Array) record.m_data, 0);
            record.RecordLength = arrRecord1.Length;
            this.m_arrMSODrawings.Add((BiffRecordRaw) record);
            break;
          case TBIFFRecord.TextObject:
            TextObjectRecord arrRecord2 = (TextObjectRecord) this.m_arrRecords[index1];
            this.m_arrMSODrawings.Add((BiffRecordRaw) arrRecord2);
            if (arrRecord2.TextLen > (ushort) 0)
            {
              int index2 = index1 + 1;
              int textLen = (int) arrRecord2.TextLen;
              while (textLen > 0)
              {
                byte[] data = this.m_arrRecords[index2].Data;
                bool flag = data[0] != (byte) 0;
                int num2 = data.Length - 1;
                textLen -= flag ? num2 / 2 : num2;
                this.m_arrMSODrawings.Add(this.m_arrRecords[index2]);
                ++index2;
              }
              int formattingRunsLen = (int) arrRecord2.FormattingRunsLen;
              while (formattingRunsLen > 0)
              {
                ContinueRecord arrRecord3 = (ContinueRecord) this.m_arrRecords[index2];
                formattingRunsLen -= arrRecord3.Length;
                this.m_arrMSODrawings.Add((BiffRecordRaw) arrRecord3);
                ++index2;
              }
              index1 = index2 - 1;
              break;
            }
            break;
          case TBIFFRecord.BOF:
            ++num1;
            goto default;
          default:
            this.m_arrMSODrawings.Add(this.m_arrRecords[index1]);
            break;
        }
      }
      else
        break;
    }
    this.m_shapes.ParseMsoStructures(this.CombineMsoDrawings(), options);
    this.m_shapes.RegenerateComboBoxNames();
  }

  private List<MsoBase> CombineMsoDrawings()
  {
    List<byte[]> arrCombined = new List<byte[]>();
    List<MsoBase> msoBaseList = new List<MsoBase>();
    int num = 0;
    int iCombinedLength = 0;
    if (this.m_arrMSODrawings.Count > 0)
    {
      int index = 0;
      for (int count = this.m_arrMSODrawings.Count; index < count; ++index)
      {
        BiffRecordRaw arrMsoDrawing = this.m_arrMSODrawings[index];
        if (num == 0 && arrMsoDrawing is MSODrawingRecord)
        {
          byte[] data = arrMsoDrawing.Data;
          iCombinedLength += data.Length;
          arrCombined.Add(data);
        }
        else if (arrMsoDrawing.TypeCode == TBIFFRecord.BOF)
          ++num;
        else if (arrMsoDrawing.TypeCode == TBIFFRecord.EOF)
          --num;
      }
      MemoryStream memoryStream = new MemoryStream(WorksheetBaseImpl.CombineArrays(iCombinedLength, arrCombined));
      while (memoryStream.Position < (long) iCombinedLength)
      {
        MsoBase msoRecord = MsoFactory.CreateMsoRecord((MsoBase) null, (Stream) memoryStream, new GetNextMsoDrawingData(this.GetNextMsoData));
        msoBaseList.Add(msoRecord);
      }
    }
    return msoBaseList;
  }

  private BiffRecordRaw[] GetNextMsoData()
  {
    List<BiffRecordRaw> biffRecordRawList = new List<BiffRecordRaw>();
    bool flag = false;
    int num = 0;
    while (!flag)
    {
      if (this.m_iCurMsoIndex >= this.m_arrMSODrawings.Count)
        throw new ApplicationException("Can't find data for MSODrawing");
      if (!(this.m_arrMSODrawings[this.m_iCurMsoIndex] is MSODrawingRecord))
        flag = true;
      else
        ++this.m_iCurMsoIndex;
    }
    for (; flag && this.m_iCurMsoIndex < this.m_arrMSODrawings.Count && (num != 0 || !(this.m_arrMSODrawings[this.m_iCurMsoIndex] is MSODrawingRecord)); ++this.m_iCurMsoIndex)
    {
      biffRecordRawList.Add(this.m_arrMSODrawings[this.m_iCurMsoIndex]);
      if (this.m_arrMSODrawings[this.m_iCurMsoIndex] is BOFRecord)
        ++num;
      else if (this.m_arrMSODrawings[this.m_iCurMsoIndex] is EOFRecord)
        --num;
    }
    return biffRecordRawList.ToArray();
  }

  public static byte[] CombineArrays(int iCombinedLength, List<byte[]> arrCombined)
  {
    if (arrCombined == null || arrCombined.Count == 0)
      return new byte[0];
    int count = arrCombined.Count;
    byte[] dst = new byte[iCombinedLength];
    int dstOffset = 0;
    for (int index = 0; index < count; ++index)
    {
      byte[] src = arrCombined[index];
      int length = src.Length;
      Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset, length);
      dstOffset += length;
    }
    return dst;
  }

  public void CopyFrom(
    WorksheetBaseImpl worksheet,
    Dictionary<string, string> hashStyleNames,
    Dictionary<string, string> hashWorksheetNames,
    Dictionary<int, int> dicFontIndexes,
    ExcelWorksheetCopyFlags flags,
    Dictionary<int, int> hashExtFormatIndexes)
  {
    if ((flags & ExcelWorksheetCopyFlags.ClearBefore) != ExcelWorksheetCopyFlags.None)
      this.ClearAll(flags);
    if ((flags & ExcelWorksheetCopyFlags.CopyOptions) != ExcelWorksheetCopyFlags.None)
      this.CopyOptions(worksheet);
    if ((flags & ExcelWorksheetCopyFlags.CopyShapes) != ExcelWorksheetCopyFlags.None)
      this.CopyShapes(worksheet, hashWorksheetNames, dicFontIndexes);
    if ((flags & ExcelWorksheetCopyFlags.CopyPageSetup) == ExcelWorksheetCopyFlags.None)
      return;
    this.CopyHeaderFooterImages(worksheet, hashWorksheetNames, (IDictionary) dicFontIndexes);
  }

  protected void CopyHeaderFooterImages(
    WorksheetBaseImpl sourceSheet,
    Dictionary<string, string> hashNewNames,
    IDictionary dicFontIndexes)
  {
    PageSetupBaseImpl pageSetupBase1 = this.PageSetupBase;
    PageSetupBaseImpl pageSetupBase2 = sourceSheet.PageSetupBase;
    CloneUtils.CloneCloneable((ICloneParent) sourceSheet.m_headerFooterShapes, (object) this);
  }

  protected void CopyShapes(
    WorksheetBaseImpl sourceSheet,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes)
  {
    ShapeCollectionBase shapes = sourceSheet.Shapes as ShapeCollectionBase;
    int index = 0;
    for (int count = shapes.Count; index < count; ++index)
      this.m_shapes.AddCopy((ShapeImpl) shapes[index], hashNewNames, dicFontIndexes);
  }

  protected virtual void CopyOptions(WorksheetBaseImpl sourceSheet)
  {
    this.m_sheetProtection = (SheetProtectionRecord) CloneUtils.CloneCloneable((ICloneable) sourceSheet.m_sheetProtection);
    if (sourceSheet.m_password != null)
      this.m_password = (PasswordRecord) sourceSheet.m_password.Clone();
    this.m_iZoom = sourceSheet.m_iZoom;
    if (sourceSheet.m_windowTwo != null)
    {
      this.m_windowTwo = (WindowTwoRecord) sourceSheet.m_windowTwo.Clone();
      this.m_windowTwo.IsSelected = false;
      this.m_windowTwo.IsPaged = false;
    }
    this.CopyTabColor(sourceSheet);
    string strCodeName = sourceSheet.m_strCodeName;
    if (string.IsNullOrEmpty(strCodeName))
      return;
    if (this.IsNameExist(new WorksheetBaseImpl.NameGetter(this.GetCodeName), strCodeName))
    {
      if (strCodeName == null || strCodeName.Length <= 0)
        return;
      this.CodeName = this.GenerateUniqueName(new WorksheetBaseImpl.NameGetter(this.GetCodeName), strCodeName);
    }
    else
      this.CodeName = strCodeName;
  }

  private bool IsNameExist(WorksheetBaseImpl.NameGetter getName, string sourceCodeName)
  {
    bool flag = false;
    int index = 0;
    for (int count = this.m_book.TabSheets.Count; index < count; ++index)
    {
      if (this.Index != index && (this.m_book.TabSheets[index] != null ? getName(this.m_book.TabSheets[index]) : string.Empty) == sourceCodeName)
        return true;
    }
    return flag;
  }

  private string GenerateUniqueName(WorksheetBaseImpl.NameGetter getName, string sourceCodeName)
  {
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    ITabSheets tabSheets = this.m_book.TabSheets;
    bool flag = true;
    int index = 0;
    for (int count = tabSheets.Count; index < count; ++index)
    {
      if (this.Index != index)
      {
        string key = tabSheets[index] != null ? getName(tabSheets[index]) : string.Empty;
        if (key == sourceCodeName)
          flag = false;
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, (object) null);
      }
    }
    if (!flag)
    {
      int num = 0;
      string key = sourceCodeName;
      while (dictionary.ContainsKey(key))
      {
        ++num;
        key = $"{sourceCodeName}_{(object) num}";
        if (key.Length > 31 /*0x1F*/)
        {
          num = 0;
          key = sourceCodeName = sourceCodeName.Remove(sourceCodeName.Length - 1);
        }
      }
      sourceCodeName = key;
    }
    return sourceCodeName;
  }

  private string GetCodeName(ITabSheet tabSheet) => tabSheet.CodeName;

  private string GetName(ITabSheet tabSheet) => tabSheet.Name;

  private void CopyTabColor(WorksheetBaseImpl sourceSheet)
  {
    if (sourceSheet == null)
      throw new ArgumentNullException(nameof (sourceSheet));
    if (sourceSheet.m_tabColor == (ColorObject) null)
    {
      this.m_tabColor = sourceSheet.m_tabColor;
    }
    else
    {
      if (this.m_tabColor == (ColorObject) null)
        this.m_tabColor = new ColorObject(ExcelKnownColors.None);
      this.m_tabColor.CopyFrom(sourceSheet.m_tabColor, true);
    }
  }

  private void CheckParseOnDemand()
  {
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this.m_pictures != null)
    {
      this.m_pictures.Clear();
      this.m_pictures = (PicturesCollection) null;
    }
    if (this.m_shapes != null)
    {
      for (int index = 0; index < this.m_shapes.Count; ++index)
        (this.m_shapes[index] as ShapeImpl).Dispose();
      this.m_shapes.Clear();
      this.m_shapes = (ShapesCollection) null;
    }
    if (this.NameChanged != null)
      this.NameChanged = (ValueChangedEventHandler) null;
    if (this.m_tabColor != (ColorObject) null)
    {
      this.m_tabColor.Dispose();
      this.m_tabColor = (ColorObject) null;
    }
    if (this.m_book == null || !this.m_book.HasVbaProject || this.VbaModule == null)
      return;
    this.VbaModule.CodeNameChanged -= new NameChangedEventHandler(this.CodeNameChanged);
  }

  private void AdvancedSheetProtection(string password)
  {
    this.AlgorithmName = "SHA-512";
    this.SaltValue = this.CreateSalt(16 /*0x10*/);
    this.SpinCount = 100000U;
    HashAlgorithm algorithm = SecurityHelper2010.GetAlgorithm(this.AlgorithmName);
    byte[] buffer1 = SecurityHelper.CombineArray(this.SaltValue, Encoding.Unicode.GetBytes(password));
    byte[] hash = algorithm.ComputeHash(buffer1);
    for (uint index = 0; index < this.SpinCount; ++index)
    {
      byte[] bytes = BitConverter.GetBytes(index);
      byte[] buffer2 = SecurityHelper.CombineArray(hash, bytes);
      hash = algorithm.ComputeHash(buffer2);
    }
    this.HashValue = hash;
  }

  protected byte[] CreateSalt(int length)
  {
    byte[] salt = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      salt[index] = (byte) random.Next(maxValue);
    return salt;
  }

  internal void ChartDispose() => base.Dispose();

  private delegate string NameGetter(ITabSheet tabSheet);
}
