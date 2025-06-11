// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.ShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class ShapeImpl : 
  CommonObject,
  IShape,
  IParentApplication,
  IDisposable,
  ICloneParent,
  INamedObject
{
  private const int DEF_APLPHA_KNOWN_COLORS = 8;
  protected const int DEF_SIZETEXTTOFITSHAPE_FALSE_VALUE = 524296 /*0x080008*/;
  protected const int DEF_SIZETEXTTOFITSHAPE_TRUE_VALUE = 655370 /*0x0A000A*/;
  protected const int DEF_NOFILLHITTEST_VALUE = 1048592 /*0x100010*/;
  public const int DEF_FULL_COLUMN_OFFSET = 1024 /*0x0400*/;
  public const int DEF_FULL_ROW_OFFSET = 256 /*0x0100*/;
  private const int DEF_LINE_WEIGHT = 9525;
  public const double DEF_TRANSPARENCY_MULL = 655.0;
  public const double DEF_TRANSPARENCY_MULL_100 = 65500.0;
  internal const double LineWieghtMultiplier = 12700.0;
  internal const double MAX_SHAPE_WIDTH_HEIGHT = 225408.0;
  internal const double MIN_SHAPE_WIDTH_HEIGHT = 0.0;
  protected static readonly Color DEF_FORE_COLOR = ColorExtension.White;
  protected static readonly Color DEF_BACK_COLOR = ColorExtension.White;
  private static readonly Type[] DEF_PARENT_TYPES = new Type[2]
  {
    typeof (ShapesCollection),
    typeof (WorkbookImpl)
  };
  private static readonly MsoOptions[] FillOptions = new MsoOptions[4]
  {
    MsoOptions.FillType,
    MsoOptions.ForeColor,
    MsoOptions.BackColor,
    MsoOptions.NoFillHitTest
  };
  private static readonly MsoOptions[] LineOptions = new MsoOptions[16 /*0x10*/]
  {
    MsoOptions.NoLineDrawDash,
    MsoOptions.LineStyle,
    MsoOptions.LineWeight,
    MsoOptions.LineDashStyle,
    MsoOptions.ContainRoundDot,
    MsoOptions.LineTransparency,
    MsoOptions.LineColor,
    MsoOptions.LineBackColor,
    MsoOptions.ContainLinePattern,
    MsoOptions.LinePattern,
    MsoOptions.LineStartArrow,
    MsoOptions.LineEndArrow,
    MsoOptions.StartArrowLength,
    MsoOptions.EndArrowLength,
    MsoOptions.StartArrowWidth,
    MsoOptions.EndArrowWidth
  };
  private double m_startX;
  private double m_startY;
  private double m_toX;
  private double m_toY;
  private double m_chartShapeX;
  private double m_chartShapeY;
  private double m_chartShapeWidth;
  private double m_chartShapeHeight;
  protected bool m_bSupportOptions;
  private bool m_validComment = true;
  private string m_strName = string.Empty;
  private string m_onAction = string.Empty;
  private string m_strAlternativeText = string.Empty;
  private MsoBase m_record;
  private WorkbookImpl m_book;
  private ExcelShapeType m_shapeType;
  [CLSCompliant(false)]
  protected MsofbtSp m_shape;
  private MsofbtClientAnchor m_clientAnchor;
  private string m_presetGeometry;
  protected ShapeCollectionBase m_shapes;
  private OBJRecord m_object;
  [CLSCompliant(false)]
  protected MsofbtOPT m_options;
  private RectangleF m_rectAbsolute = new RectangleF();
  private ShapeFillImpl m_fill;
  private ShapeLineFormatImpl m_lineFormat;
  protected bool m_bUpdateLineFill = true;
  private Stream m_xmlDataStream;
  private Stream m_xmlTypeStream;
  private Relation m_imageRelation;
  private string m_strImageRelationId;
  private bool m_bUpdatePositions = true;
  private bool m_bVmlShape;
  private int m_iShapeId;
  private string m_macroName;
  private Ptg[] m_macroTokens;
  private bool m_shapeVisibility = true;
  private ShadowImpl m_shadow;
  private ThreeDFormatImpl m_3D;
  private bool m_enableAlternateContent;
  private List<ShapeImpl> m_childShapes;
  private MsofbtChildAnchor m_childAnchor;
  private MsoUnknown m_unknown;
  private Dictionary<string, string> m_styleProperties;
  private string m_preserveStyleString;
  private bool m_isHyperlink;
  private int m_shapeRotation;
  private Stream m_formulaMacroStream;
  private bool m_bHasBorder;
  internal List<Stream> preservedShapeStreams;
  internal List<Stream> preservedCnxnShapeStreams;
  internal List<Stream> preservedInnerCnxnShapeStreams;
  internal List<Stream> preservedPictureStreams;
  internal Stream m_graphicFrame;
  private bool m_bIsAbsoluteAnchor;
  private bool m_lockWithSheet = true;
  private bool m_printWithSheet = true;
  private Stream m_streamExtLst;
  private IHyperLink m_hyperLink;
  internal Dictionary<string, Stream> m_preservedElements;
  private bool m_isCustomGeom;
  private Stream m_styleStream;
  private ShapeFrame m_shapeFrame;
  private ShapeFrame m_groupFrame;
  private bool m_isGroupFill;
  private bool m_isGroupLine;
  private bool m_bAutoSize;
  internal bool IsEquationShape;
  internal List<string> preserveStreamOrder;

  [CLSCompliant(false)]
  public static void SerializeForte(IFopteOptionWrapper options, MsoOptions id, byte[] arr)
  {
    ShapeImpl.SerializeForte(options, id, arr, (byte[]) null, false);
  }

  [CLSCompliant(false)]
  public static void SerializeForte(
    IFopteOptionWrapper options,
    MsoOptions id,
    byte[] arr,
    byte[] addData,
    bool isValid)
  {
    if (arr == null)
      throw new ArgumentNullException(nameof (arr));
    ShapeImpl.SerializeForte(options, id, BitConverter.ToInt32(arr, 0), addData, isValid);
  }

  [CLSCompliant(false)]
  public static void SerializeForte(IFopteOptionWrapper options, MsoOptions id, int value)
  {
    ShapeImpl.SerializeForte(options, id, value, (byte[]) null, false);
  }

  [CLSCompliant(false)]
  public static void SerializeForte(
    IFopteOptionWrapper options,
    MsoOptions id,
    int value,
    byte[] addData,
    bool isValid)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    MsofbtOPT.FOPTE option = new MsofbtOPT.FOPTE();
    option.Id = id;
    option.UInt32Value = (uint) value;
    option.IsValid = isValid;
    option.IsComplex = false;
    if (addData != null)
    {
      option.IsComplex = true;
      option.AdditionalData = addData;
      option.UInt32Value = (uint) addData.Length;
    }
    options.AddOptionSorted(option);
  }

  public ShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_bSupportOptions = true;
    this.SetParents();
    if ((!(this is ComboBoxShapeImpl) || (this as ComboBoxShapeImpl).ComboType != ExcelComboType.AutoFilter) && (!(this is FormControlShapeImpl) || (this as FormControlShapeImpl).ShapeType != ExcelShapeType.FormControl))
      this.AttachEvents();
    this.m_bHasBorder = true;
    if (this.m_shapes.Worksheet == null)
      this.m_bUpdatePositions = false;
    this.m_shapeFrame = new ShapeFrame(this);
    this.m_clientAnchor = (MsofbtClientAnchor) MsoFactory.GetRecord(MsoRecords.msofbtClientAnchor);
  }

  public ShapeImpl(IApplication application, object parent, ShapeImpl instance)
    : this(application, parent)
  {
    this.m_bIsDisposed = instance.m_bIsDisposed;
    this.m_bSupportOptions = instance.m_bSupportOptions;
    this.m_rectAbsolute = instance.m_rectAbsolute;
    this.m_shapeType = instance.m_shapeType;
    this.m_strAlternativeText = instance.m_strAlternativeText;
    this.m_strName = instance.m_strName;
    MsoBase record = instance.m_record;
    if (record != null)
      this.m_record = (MsoBase) CloneUtils.CloneMsoBase(record, (MsoBase) null);
    else if (this.m_shape != null)
      this.m_shape = (MsofbtSp) CloneUtils.CloneMsoBase((MsoBase) instance.m_shape, (MsoBase) null);
    this.UpdateRecord(instance.m_clientAnchor);
    this.m_object = (OBJRecord) CloneUtils.CloneCloneable((ICloneable) instance.m_object);
  }

  [CLSCompliant(false)]
  public ShapeImpl(IApplication application, object parent, MsoBase[] records, int index)
    : this(application, parent)
  {
    this.m_record = records[index];
    this.ParseRecord();
  }

  [CLSCompliant(false)]
  public ShapeImpl(IApplication application, object parent, MsofbtSpContainer container)
    : this(application, parent, container, ExcelParseOptions.Default)
  {
  }

  [CLSCompliant(false)]
  public ShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer container,
    ExcelParseOptions options)
    : this(application, parent)
  {
    this.m_record = (MsoBase) container;
    this.ParseRecord(options);
    this.m_bSupportOptions = true;
  }

  [CLSCompliant(false)]
  public ShapeImpl(IApplication application, object parent, MsoBase shapeRecord)
    : this(application, parent, shapeRecord, ExcelParseOptions.Default)
  {
  }

  [CLSCompliant(false)]
  public ShapeImpl(
    IApplication application,
    object parent,
    MsoBase shapeRecord,
    ExcelParseOptions options)
    : this(application, parent)
  {
    this.m_record = shapeRecord;
  }

  protected virtual void CreateDefaultFillLineFormats()
  {
  }

  private void ParseRecord() => this.ParseRecord(ExcelParseOptions.Default);

  private void ParseRecord(ExcelParseOptions options)
  {
    this.m_shapeType = ExcelShapeType.Unknown;
    MsofbtSpContainer record = this.m_record as MsofbtSpContainer;
    this.CreateDefaultFillLineFormats();
    if (record == null)
      return;
    List<MsoBase> itemsList = record.ItemsList;
    int index = 0;
    for (int count = itemsList.Count; index < count; ++index)
    {
      MsoBase msoBase = itemsList[index];
      switch (msoBase.MsoRecordType)
      {
        case MsoRecords.msofbtSpgrContainer:
          this.ParseShapeGroupContainer((MsofbtSpgrContainer) msoBase);
          break;
        case MsoRecords.msofbtSpgr:
          this.ParseShapeGroup((MsofbtSpgr) msoBase);
          break;
        case MsoRecords.msofbtSp:
          this.ParseShape((MsofbtSp) msoBase);
          break;
        case MsoRecords.msofbtOPT:
          if (this.m_bUpdateLineFill)
          {
            this.ParseOptions((MsofbtOPT) msoBase);
            break;
          }
          MsofbtOPT options1 = msoBase as MsofbtOPT;
          this.m_options = options1;
          this.ExtractNecessaryOptions(options1);
          break;
        case MsoRecords.msofbtChildAnchor:
          this.ParseChildAnchor((MsofbtChildAnchor) msoBase);
          break;
        case MsoRecords.msofbtClientAnchor:
          this.ParseClientAnchor((MsofbtClientAnchor) msoBase);
          break;
        case MsoRecords.msofbtClientData:
          this.ParseClientData((MsofbtClientData) msoBase, options);
          break;
        default:
          if (msoBase.MsoRecordType == MsoRecords.msofbtClientTextbox)
          {
            this.ParseOtherRecords(msoBase, options);
            break;
          }
          this.ParseUnKnown((MsoUnknown) msoBase);
          break;
      }
    }
    if (this.Id == 0)
      return;
    this.m_iShapeId = this.Id;
  }

  [CLSCompliant(false)]
  protected virtual void ParseClientData(MsofbtClientData clientData, ExcelParseOptions options)
  {
    this.m_object = clientData.ObjectRecord;
    List<ObjSubRecord> recordsList = this.m_object.RecordsList;
    this.m_book.CurrentObjectId = Math.Max(this.m_book.CurrentObjectId, (int) (recordsList[0] as ftCmo).ID);
    int index1 = 1;
    for (int count = recordsList.Count; index1 < count; ++index1)
    {
      ObjSubRecord objSubRecord = recordsList[index1];
      if (objSubRecord.Type == TObjSubRecordType.ftMacro)
      {
        this.m_macroTokens = ((ftMacro) objSubRecord).Tokens;
        break;
      }
    }
    int index2 = 0;
    for (int count = recordsList.Count; index2 < count; ++index2)
    {
      ObjSubRecord objSubRecord = recordsList[index2];
      if (objSubRecord.Type == TObjSubRecordType.ftCmo)
      {
        this.m_printWithSheet = ((ftCmo) objSubRecord).Printable;
        break;
      }
    }
  }

  [CLSCompliant(false)]
  protected virtual void ParseOtherRecords(MsoBase subRecord, ExcelParseOptions options)
  {
  }

  private void ParseOptions(MsofbtOPT options)
  {
    this.m_options = options != null ? options : throw new ArgumentNullException(nameof (options));
    MsofbtOPT.FOPTE[] properties = options.Properties;
    int index = 0;
    for (int length = properties.Length; index < length; ++index)
      this.ParseOption(properties[index]);
  }

  private bool ParseFill(MsofbtOPT.FOPTE option)
  {
    if (this.m_fill == null && Array.IndexOf<MsoOptions>(ShapeImpl.FillOptions, option.Id) >= 0)
      this.m_fill = new ShapeFillImpl(this.Application, (object) this);
    return this.m_fill != null && this.m_fill.ParseOption(option);
  }

  private bool ParseLineFormat(MsofbtOPT.FOPTE option)
  {
    if (this.m_lineFormat == null && Array.IndexOf<MsoOptions>(ShapeImpl.LineOptions, option.Id) >= 0)
      this.m_lineFormat = new ShapeLineFormatImpl(this.Application, (object) this);
    return this.m_lineFormat != null && this.m_lineFormat.ParseOption(option);
  }

  [CLSCompliant(false)]
  protected virtual bool ParseOption(MsofbtOPT.FOPTE option)
  {
    if (this.ParseFill(option) || this.ParseLineFormat(option))
      return true;
    switch (option.Id)
    {
      case MsoOptions.SizeTextToFitShape:
        this.m_bAutoSize = option.UInt32Value == 655370U /*0x0A000A*/;
        return true;
      case MsoOptions.ShapeName:
        this.m_strName = this.ParseName(option);
        return true;
      case MsoOptions.AlternativeText:
        this.m_strAlternativeText = this.ParseName(option);
        break;
    }
    return false;
  }

  [CLSCompliant(false)]
  protected virtual void ParseShape(MsofbtSp shapeRecord)
  {
    this.m_shape = shapeRecord != null ? (MsofbtSp) shapeRecord.Clone() : throw new ArgumentNullException(nameof (shapeRecord));
  }

  [CLSCompliant(false)]
  public virtual void ParseClientAnchor(MsofbtClientAnchor clientAnchor)
  {
    this.m_clientAnchor = clientAnchor != null ? clientAnchor : throw new ArgumentNullException(nameof (clientAnchor));
    if (this.m_shapes.Worksheet != null)
    {
      int maxColumnCount = this.m_shapes.Workbook.MaxColumnCount;
      if (this.m_clientAnchor.RightColumn >= maxColumnCount && this.m_clientAnchor.LeftColumn >= maxColumnCount)
        clientAnchor.RightColumn = clientAnchor.LeftColumn = 0;
      else if (this.m_clientAnchor.RightColumn >= maxColumnCount)
        clientAnchor.RightColumn = clientAnchor.LeftColumn;
      else if (this.m_clientAnchor.LeftColumn >= maxColumnCount)
      {
        clientAnchor.RightColumn = (int) byte.MaxValue;
        clientAnchor.LeftColumn = 0;
      }
      int maxRowCount = this.m_shapes.Workbook.MaxRowCount;
      if (this.m_clientAnchor.BottomRow >= maxRowCount)
        this.m_clientAnchor.BottomRow = maxRowCount;
    }
    if (this.m_clientAnchor.IsShortVersion || this is CommentShapeImpl)
      return;
    this.EvaluateTopLeftPosition();
    this.UpdateHeight();
    this.UpdateWidth();
  }

  protected virtual void SetParents()
  {
    this.m_shapes = this.FindParent(typeof (ShapeCollectionBase), true) as ShapeCollectionBase;
    this.m_book = this.m_shapes != null ? this.m_shapes.Workbook : throw new ArgumentNullException("Can't find parent collection.");
  }

  internal void ChangeParent(object parent) => this.SetParent(parent);

  protected void AttachEvents()
  {
    if (this.m_shapes.WorksheetBase is WorksheetImpl worksheetBase)
    {
      (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent += new EventHandler(this.NormalFont_OnAfterChange);
      worksheetBase.ColumnWidthChanged += new ValueChangedEventHandler(this.Worksheet_ColumnWidthChanged);
      worksheetBase.RowHeightChanged += new ValueChangedEventHandler(this.Worksheet_RowHeightChanged);
      worksheetBase.MacroNameChanged += new ValueChangedEventHandler(this.CodeName_Changed);
    }
    (this.Workbook as WorkbookImpl).MacroNameChanged += new ValueChangedEventHandler(this.CodeName_Changed);
  }

  protected void DetachEvents()
  {
    if (this.m_shapes.WorksheetBase is WorksheetImpl worksheetBase)
    {
      if (this.m_book.Styles != null && this.m_book.Styles.Contains("Normal"))
        (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent -= new EventHandler(this.NormalFont_OnAfterChange);
      worksheetBase.ColumnWidthChanged -= new ValueChangedEventHandler(this.Worksheet_ColumnWidthChanged);
      worksheetBase.RowHeightChanged -= new ValueChangedEventHandler(this.Worksheet_RowHeightChanged);
      worksheetBase.MacroNameChanged -= new ValueChangedEventHandler(this.CodeName_Changed);
    }
    (this.Workbook as WorkbookImpl).MacroNameChanged -= new ValueChangedEventHandler(this.CodeName_Changed);
  }

  [CLSCompliant(false)]
  protected virtual void ParseShapeGroup(MsofbtSpgr shapeGroup)
  {
  }

  [CLSCompliant(false)]
  protected virtual void ParseShapeGroupContainer(MsofbtSpgrContainer subRecord)
  {
  }

  [CLSCompliant(false)]
  protected virtual void ParseChildAnchor(MsofbtChildAnchor childAnchor)
  {
    this.m_childAnchor = childAnchor != null ? childAnchor : throw new ArgumentNullException(nameof (childAnchor));
  }

  protected virtual void ParseUnKnown(MsoUnknown UnKnown)
  {
    this.m_unknown = UnKnown != null ? UnKnown : throw new ArgumentNullException("MsOUnknown");
  }

  [CLSCompliant(false)]
  protected Color GetColorValue(MsofbtOPT.FOPTE option)
  {
    byte[] bytes = BitConverter.GetBytes(option.UInt32Value);
    return bytes[3] == (byte) 8 ? this.m_book.GetPaletteColor((ExcelKnownColors) bytes[0]) : Color.FromArgb(0, (int) bytes[0], (int) bytes[1], (int) bytes[2]);
  }

  private byte GetByte(MsofbtOPT.FOPTE option, int iByteIndex)
  {
    return BitConverter.GetBytes(option.UInt32Value)[iByteIndex];
  }

  [CLSCompliant(false)]
  protected string ParseName(MsofbtOPT.FOPTE option)
  {
    byte[] bytes = option != null ? option.AdditionalData : throw new ArgumentNullException(nameof (option));
    string str1 = (string) null;
    if (bytes == null)
      return (string) null;
    if (bytes.Length <= 0)
      return str1 = string.Empty;
    string str2 = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
    return str2.Substring(0, str2.Length - 1);
  }

  private void ExtractNecessaryOptions(MsofbtOPT options)
  {
    IList<MsofbtOPT.FOPTE> fopteList = options != null ? options.PropertyList : throw new ArgumentNullException(nameof (options));
    int index = 0;
    for (int count = fopteList.Count; index < count; ++index)
      this.ExtractNecessaryOption(fopteList[index]);
  }

  [CLSCompliant(false)]
  protected virtual bool ExtractNecessaryOption(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    switch (option.Id)
    {
      case MsoOptions.SizeTextToFitShape:
        this.m_bAutoSize = option.UInt32Value == 655370U /*0x0A000A*/;
        return true;
      case MsoOptions.ShapeName:
        this.m_strName = this.ParseName(option);
        return true;
      case MsoOptions.AlternativeText:
        this.m_strAlternativeText = this.ParseName(option);
        return true;
      default:
        return false;
    }
  }

  public virtual int Height
  {
    get => (int) this.m_rectAbsolute.Height;
    set => this.SetHeight((double) value);
  }

  internal double HeightDouble
  {
    get => (double) this.m_rectAbsolute.Height;
    set => this.SetHeight(value);
  }

  public virtual int Id
  {
    get
    {
      return this.m_record is MsofbtSpContainer record && record.ItemsList[0] is MsofbtSp items ? items.ShapeId : 0;
    }
  }

  public IThreeDFormat ThreeD
  {
    get
    {
      if (this.m_3D == null)
        this.m_3D = new ThreeDFormatImpl(this.Application, (object) this);
      return (IThreeDFormat) this.m_3D;
    }
  }

  internal bool ValidComment
  {
    get => this.m_validComment;
    set => this.m_validComment = value;
  }

  public IShadow Shadow
  {
    get
    {
      if (this.m_shadow == null)
        this.m_shadow = new ShadowImpl(this.Application, (object) this);
      return (IShadow) this.m_shadow;
    }
  }

  public virtual int Left
  {
    get => (int) this.m_rectAbsolute.X;
    set => this.SetLeftPosition((double) value);
  }

  internal double LeftDouble
  {
    get => (double) this.m_rectAbsolute.X;
    set => this.SetLeftPosition(value);
  }

  internal bool EnableAlternateContent
  {
    get => this.m_enableAlternateContent;
    set => this.m_enableAlternateContent = value;
  }

  public virtual string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public virtual int Top
  {
    get => (int) this.m_rectAbsolute.Y;
    set => this.SetTopPosition((double) value);
  }

  internal double TopDouble
  {
    get => (double) this.m_rectAbsolute.Y;
    set => this.SetTopPosition(value);
  }

  public virtual int Width
  {
    get => (int) this.m_rectAbsolute.Width;
    set => this.SetWidth((double) value);
  }

  internal double WidthDouble
  {
    get => (double) this.m_rectAbsolute.Width;
    set => this.SetWidth(value);
  }

  public ExcelShapeType ShapeType
  {
    get => this.m_shapeType;
    set => this.m_shapeType = value;
  }

  public bool IsShapeVisible
  {
    get => this.m_shapeVisibility;
    set
    {
      this.m_shapeVisibility = value;
      if (this.m_book.Loading || !(this is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (IsShapeVisible));
    }
  }

  public virtual string AlternativeText
  {
    get => this.m_strAlternativeText;
    set => this.m_strAlternativeText = value;
  }

  public virtual bool IsMoveWithCell
  {
    get => this.ClientAnchor.IsMoveWithCell;
    set
    {
      this.ClientAnchor.IsMoveWithCell = value;
      if (this.m_book.Loading || !(this is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (IsMoveWithCell));
    }
  }

  public virtual bool IsSizeWithCell
  {
    get => this.ClientAnchor.IsSizeWithCell;
    set
    {
      this.ClientAnchor.IsSizeWithCell = value;
      if (this.m_book.Loading || !(this is GroupShapeImpl))
        return;
      this.SetInnerShapes((object) value, nameof (IsSizeWithCell));
    }
  }

  public virtual IFill Fill
  {
    get
    {
      if (!this.m_bSupportOptions)
        throw new NotSupportedException("This shape doesn't support fill properties.");
      if (!this.m_bUpdateLineFill)
        this.ParseLineFill(this.m_options);
      else if (this.m_fill == null)
      {
        this.m_fill = new ShapeFillImpl(this.Application, (object) this);
        if (this.ShapeType == ExcelShapeType.CheckBox)
          this.m_fill.Visible = false;
        if (this.VmlShape && this.ShapeType == ExcelShapeType.TextBox)
          this.m_fill.Visible = false;
      }
      return (IFill) this.m_fill;
    }
  }

  public virtual IShapeLineFormat Line
  {
    get
    {
      if (!this.m_bSupportOptions)
        throw new NotSupportedException("This shape doesn't support line properties.");
      if (!this.m_bUpdateLineFill)
        this.ParseLineFill(this.m_options);
      else if (this.m_lineFormat == null)
        this.m_lineFormat = new ShapeLineFormatImpl(this.Application, (object) this);
      return (IShapeLineFormat) this.m_lineFormat;
    }
  }

  internal string PresetGeometry
  {
    get => this.m_presetGeometry;
    set => this.m_presetGeometry = value;
  }

  internal string MacroName
  {
    get => this.m_macroName;
    set => this.m_macroName = value;
  }

  public bool AutoSize
  {
    get => this.m_bAutoSize;
    set => this.m_bAutoSize = value;
  }

  public Stream XmlDataStream
  {
    get => this.m_xmlDataStream;
    set => this.m_xmlDataStream = value;
  }

  public Stream XmlTypeStream
  {
    get => this.m_xmlTypeStream;
    set => this.m_xmlTypeStream = value;
  }

  public bool VmlShape
  {
    get => this.m_bVmlShape;
    set => this.m_bVmlShape = value;
  }

  public string OnAction
  {
    get => this.m_onAction;
    set => this.m_onAction = value;
  }

  public string ImageRelationId
  {
    get => this.m_strImageRelationId;
    set => this.m_strImageRelationId = value;
  }

  public Relation ImageRelation
  {
    get => this.m_imageRelation;
    set => this.m_imageRelation = value;
  }

  public virtual int ShapeRotation
  {
    get => this.m_shapeRotation;
    set
    {
      this.m_shapeRotation = value <= 3600 || value >= -3600 ? value : throw new ArgumentException("The rotation value should be between -3600 and 3600");
      this.m_shapeFrame.Rotation = this.m_shapeRotation * 60000;
    }
  }

  public virtual ITextFrame TextFrame
  {
    get => throw new NotImplementedException("This property doesn't support in this class");
  }

  internal Stream FormulaMacroStream
  {
    get => this.m_formulaMacroStream;
    set => this.m_formulaMacroStream = value;
  }

  public IHyperLink Hyperlink
  {
    get => this.m_hyperLink;
    internal set
    {
      if (this.ShapeType != ExcelShapeType.AutoShape && this.ShapeType != ExcelShapeType.Picture && this.ShapeType != ExcelShapeType.TextBox)
        throw new NotSupportedException("HyperLink");
      this.m_hyperLink = value;
    }
  }

  internal Stream StyleStream
  {
    get => this.m_styleStream;
    set => this.m_styleStream = value;
  }

  public void Remove()
  {
    this.OnDelete();
    this.m_shapes.Remove((IShape) this);
  }

  public void Scale(int scaleWidth, int scaleHeight)
  {
    if (scaleWidth < 0)
      throw new ArgumentOutOfRangeException(nameof (scaleWidth));
    if (scaleHeight < 0)
      throw new ArgumentOutOfRangeException(nameof (scaleHeight));
    this.Width = (int) ((double) (this.Width * scaleWidth) / 100.0);
    this.Height = (int) ((double) (this.Height * scaleHeight) / 100.0);
  }

  protected override void OnDispose()
  {
    base.OnDispose();
    if (this.m_bIsDisposed)
    {
      if (this.m_streamExtLst != null)
        this.m_streamExtLst = (Stream) null;
      if (this.m_graphicFrame != null)
        this.m_graphicFrame = (Stream) null;
      if (this.preservedShapeStreams != null)
      {
        this.preservedShapeStreams.Clear();
        this.preservedShapeStreams = (List<Stream>) null;
      }
      if (this.preservedCnxnShapeStreams != null)
      {
        this.preservedCnxnShapeStreams.Clear();
        this.preservedCnxnShapeStreams = (List<Stream>) null;
      }
      if (this.preservedInnerCnxnShapeStreams != null)
      {
        this.preservedInnerCnxnShapeStreams.Clear();
        this.preservedInnerCnxnShapeStreams = (List<Stream>) null;
      }
      if (this.preservedPictureStreams != null)
      {
        this.preservedPictureStreams.Clear();
        this.preservedPictureStreams = (List<Stream>) null;
      }
      if (this.m_formulaMacroStream != null)
        this.m_formulaMacroStream = (Stream) null;
      if (this.m_xmlDataStream != null)
        this.m_xmlDataStream = (Stream) null;
      if (this.m_xmlTypeStream != null)
        this.m_xmlTypeStream = (Stream) null;
      this.m_record = (MsoBase) null;
      this.m_shape = (MsofbtSp) null;
      this.m_clientAnchor = (MsofbtClientAnchor) null;
      this.m_object = (OBJRecord) null;
      this.m_options = (MsofbtOPT) null;
      this.m_childAnchor = (MsofbtChildAnchor) null;
      this.m_unknown = (MsoUnknown) null;
      if (this.m_fill != null)
      {
        this.m_fill.Dispose();
        this.m_fill = (ShapeFillImpl) null;
      }
      if (this.m_lineFormat != null)
      {
        this.m_lineFormat.Dispose();
        this.m_lineFormat = (ShapeLineFormatImpl) null;
      }
      if (this.m_shadow != null)
      {
        this.m_shadow.Dispose();
        this.m_shadow = (ShadowImpl) null;
      }
      if (this.m_3D != null)
      {
        this.m_3D.Dispose();
        this.m_3D = (ThreeDFormatImpl) null;
      }
      if (this.m_styleProperties != null)
      {
        this.m_styleProperties.Clear();
        this.m_fill = (ShapeFillImpl) null;
      }
      if (this.m_childShapes != null)
        this.m_childShapes.Clear();
      if (this.m_shapeFrame != null)
        this.m_shapeFrame = (ShapeFrame) null;
      if (this.m_groupFrame != null)
        this.m_groupFrame = (ShapeFrame) null;
    }
    if (this is ComboBoxShapeImpl && (this as ComboBoxShapeImpl).ComboType == ExcelComboType.AutoFilter || this is FormControlShapeImpl && (this as FormControlShapeImpl).ShapeType == ExcelShapeType.FormControl)
      return;
    this.DetachEvents();
  }

  [CLSCompliant(false)]
  public void Serialize(MsofbtSpgrContainer spgrContainer)
  {
    if (this.ChildShapes.Count > 0)
      this.SerializeShape(spgrContainer, true);
    else
      this.SerializeShape(spgrContainer);
  }

  [CLSCompliant(false)]
  public void Serialize(MsofbtSpgrContainer spgrContainer, bool isGroupShape)
  {
    this.SerializeShape(spgrContainer, isGroupShape);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeShape(MsofbtSpgrContainer spgrContainer)
  {
    if (this.m_record == null)
      return;
    spgrContainer.AddItem(this.m_record);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeShape(MsofbtSpgrContainer spgrContainer, bool isGroupShape)
  {
    List<ShapeImpl> childShapes = this.ChildShapes;
    if (childShapes.Count > 0)
    {
      this.Worksheet.TypedOptionButtons.PrepareForSerialization();
      MsofbtSpgrContainer record = (MsofbtSpgrContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpgrContainer);
      spgrContainer.AddItem((MsoBase) record);
      foreach (ShapeImpl shapeImpl in childShapes)
        shapeImpl.Serialize(record, true);
    }
    else
    {
      if (this.m_record == null)
        return;
      spgrContainer.AddItem(this.m_record);
    }
  }

  private void SerializeMsoOptions(MsofbtSpContainer container)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    if (this.m_options == null)
      this.m_options = this.CreateDefaultOptions();
    if (this.m_shapeType == ExcelShapeType.Unknown)
      return;
    if (this.m_bUpdateLineFill)
      this.m_options = this.SerializeMsoOptions(this.m_options);
    List<MsoBase> itemsList = container.ItemsList;
    int index = 0;
    for (int count = itemsList.Count; index < count; ++index)
    {
      if (itemsList[index] is MsofbtOPT)
      {
        itemsList[index] = (MsoBase) this.m_options;
        break;
      }
    }
  }

  [CLSCompliant(false)]
  protected MsofbtOPT SerializeMsoOptions(MsofbtOPT opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    if (this.m_fill != null)
    {
      this.UpdateFillFopte(opt);
      opt = (MsofbtOPT) this.m_fill.Serialize((IFopteOptionWrapper) opt);
    }
    if (this.m_bSupportOptions && this.m_lineFormat != null)
      this.m_lineFormat.Serialize(opt);
    this.SerializeCommentShadow(opt);
    return opt;
  }

  private void SerializeTransparency(MsofbtOPT opt, int value)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    int num = 100 - value;
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.Transparency, (int) ((double) num * 655.0));
  }

  [CLSCompliant(false)]
  protected virtual MsofbtOPT SerializeOptions(MsoBase parent)
  {
    if (this.m_options == null)
      this.m_options = this.CreateDefaultOptions();
    return this.m_options;
  }

  [CLSCompliant(false)]
  protected void SerializeSizeTextToFit(MsofbtOPT options)
  {
    int num = this.m_bAutoSize ? 655370 /*0x0A000A*/ : 524296 /*0x080008*/;
    this.SerializeOptionSorted(options, MsoOptions.SizeTextToFitShape, (uint) num);
  }

  [CLSCompliant(false)]
  protected void SerializeHitTest(MsofbtOPT options)
  {
    this.SerializeOptionSorted(options, MsoOptions.NoFillHitTest, 1048592U /*0x100010*/);
  }

  [CLSCompliant(false)]
  protected void SerializeOption(MsofbtOPT options, MsoOptions id, uint value)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    options.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = id,
      UInt32Value = value,
      IsValid = false,
      IsComplex = false
    });
  }

  [CLSCompliant(false)]
  protected MsofbtOPT.FOPTE SerializeOption(MsofbtOPT options, MsoOptions id, int value)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    MsofbtOPT.FOPTE option = new MsofbtOPT.FOPTE();
    option.Id = id;
    option.Int32Value = value;
    option.IsValid = false;
    option.IsComplex = false;
    options.AddOptionsOrReplace(option);
    return option;
  }

  [CLSCompliant(false)]
  protected void SerializeOptionSorted(MsofbtOPT options, MsoOptions id, uint value)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    options.AddOptionSorted(new MsofbtOPT.FOPTE()
    {
      Id = id,
      UInt32Value = value,
      IsValid = false,
      IsComplex = false
    });
  }

  [CLSCompliant(false)]
  protected void SerializeShapeVisibility(MsofbtOPT options)
  {
    MsofbtOPT.FOPTE option = new MsofbtOPT.FOPTE();
    option.Id = MsoOptions.CommentShowAlways;
    byte[] destinationArray;
    if (options.IndexOf(MsoOptions.CommentShowAlways) == options.PropertyList.Count)
    {
      destinationArray = new byte[4]
      {
        (byte) 0,
        (byte) 0,
        (byte) 2,
        (byte) 0
      };
    }
    else
    {
      destinationArray = new byte[4];
      Array.Copy((Array) options.PropertyList[options.IndexOf(MsoOptions.CommentShowAlways)].MainData, 2, (Array) destinationArray, 0, 4);
    }
    destinationArray[0] = !this.IsShapeVisible ? (byte) 2 : (byte) 0;
    option.UInt32Value = BitConverter.ToUInt32(destinationArray, 0);
    options.AddOptionSorted(option);
  }

  [CLSCompliant(false)]
  protected void SerializeShapeName(MsofbtOPT options)
  {
    this.SerializeName(options, MsoOptions.ShapeName, this.m_strName);
  }

  [CLSCompliant(false)]
  protected void SerializeName(MsofbtOPT options, MsoOptions optionId, string name)
  {
    if (name == null)
      return;
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    int length = name.Length;
    string s = name;
    if (length > 0 && name[length - 1] != char.MinValue)
      s += (string) (object) char.MinValue;
    byte[] bytes = Encoding.Unicode.GetBytes(s);
    options.AddOptionSorted(new MsofbtOPT.FOPTE()
    {
      Id = optionId,
      UInt32Value = (uint) bytes.Length,
      IsValid = true,
      IsComplex = true,
      AdditionalData = bytes
    });
  }

  [CLSCompliant(false)]
  protected virtual MsofbtOPT CreateDefaultOptions()
  {
    return (MsofbtOPT) MsoFactory.GetRecord(MsoRecords.msofbtOPT);
  }

  private void UpdateFillFopte(MsofbtOPT option)
  {
    if (option == null)
      throw new ArgumentNullException("opt");
    for (int index = 384; index <= 412; ++index)
      option.RemoveOption(index);
  }

  [CLSCompliant(false)]
  protected virtual void SerializeCommentShadow(MsofbtOPT option)
  {
  }

  internal double ChartShapeX
  {
    get => this.m_chartShapeX;
    set => this.m_chartShapeX = value;
  }

  internal double ChartShapeY
  {
    get => this.m_chartShapeY;
    set => this.m_chartShapeY = value;
  }

  internal double ChartShapeWidth
  {
    get => this.m_chartShapeWidth;
    set => this.m_chartShapeWidth = value;
  }

  internal double ChartShapeHeight
  {
    get => this.m_chartShapeHeight;
    set => this.m_chartShapeHeight = value;
  }

  internal double StartX
  {
    get => this.m_startX;
    set => this.m_startX = value;
  }

  internal double StartY
  {
    get => this.m_startY;
    set => this.m_startY = value;
  }

  internal double ToX
  {
    get => this.m_toX;
    set => this.m_toX = value;
  }

  internal double ToY
  {
    get => this.m_toY;
    set => this.m_toY = value;
  }

  internal bool LockWithSheet
  {
    get => this.m_lockWithSheet;
    set => this.m_lockWithSheet = value;
  }

  internal bool PrintWithSheet
  {
    get => this.m_printWithSheet;
    set => this.m_printWithSheet = value;
  }

  internal Stream GraphicFrameStream
  {
    get => this.m_graphicFrame;
    set => this.m_graphicFrame = value;
  }

  internal bool HasBorder
  {
    get => this.m_bHasBorder;
    set => this.m_bHasBorder = value;
  }

  public IWorkbook Workbook => (IWorkbook) this.m_book;

  public WorkbookImpl ParentWorkbook => this.m_book;

  public ShapeCollectionBase ParentShapes => this.m_shapes;

  public WorksheetBaseImpl Worksheet => this.m_shapes.WorksheetBase;

  [CLSCompliant(false)]
  public OBJRecord Obj => this.m_object;

  [CLSCompliant(false)]
  public MsofbtClientAnchor ClientAnchor => this.m_clientAnchor;

  public virtual int TopRow
  {
    get => this.ClientAnchor.TopRow + 1;
    set
    {
      this.ClientAnchor.TopRow = value - 1;
      if (!this.m_bUpdatePositions)
        return;
      this.OnTopRowChanged();
    }
  }

  public virtual int LeftColumn
  {
    get => this.ClientAnchor.LeftColumn + 1;
    set
    {
      this.ClientAnchor.LeftColumn = value - 1;
      if (!this.m_bUpdatePositions)
        return;
      this.OnLeftColumnChange();
      if (this.m_shapes.Worksheet == null)
        return;
      if (this.Left > 0)
        this.LeftColumnOffset = ShapeImpl.ConvertPixelsIntoWidthOffset(this.Left, this.m_shapes.Worksheet.GetColumnWidthInPixels(value));
      this.EvaluateLeftPosition();
    }
  }

  public virtual int BottomRow
  {
    get => this.ClientAnchor.BottomRow + 1;
    set
    {
      this.ClientAnchor.BottomRow = value - 1;
      if (!this.m_bUpdatePositions)
        return;
      this.UpdateHeight();
    }
  }

  public virtual int RightColumn
  {
    get => this.ClientAnchor.RightColumn + 1;
    set
    {
      this.ClientAnchor.RightColumn = value - 1;
      if (!this.m_bUpdatePositions)
        return;
      this.UpdateWidth();
    }
  }

  public virtual int TopRowOffset
  {
    get => this.ClientAnchor.TopOffset;
    set
    {
      this.ClientAnchor.TopOffset = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (TopRowOffset));
      this.OnTopRowChanged();
    }
  }

  public virtual int LeftColumnOffset
  {
    get => this.ClientAnchor.LeftOffset;
    set
    {
      this.ClientAnchor.LeftOffset = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (LeftColumnOffset));
      this.OnLeftColumnChange();
    }
  }

  public virtual int BottomRowOffset
  {
    get => this.ClientAnchor.BottomOffset;
    set
    {
      this.ClientAnchor.BottomOffset = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (BottomRowOffset));
      this.UpdateHeight();
    }
  }

  public virtual int RightColumnOffset
  {
    get => this.ClientAnchor.RightOffset;
    set
    {
      this.ClientAnchor.RightOffset = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (RightColumnOffset));
      this.UpdateWidth();
    }
  }

  [CLSCompliant(false)]
  public uint OldObjId
  {
    get => this.m_object != null ? (uint) (this.m_object.RecordsList[0] as ftCmo).ID : 0U;
    set
    {
      if (this.m_object == null)
        return;
      (this.m_object.RecordsList[0] as ftCmo).ID = (ushort) value;
    }
  }

  [CLSCompliant(false)]
  public MsoBase Record => this.m_record;

  [CLSCompliant(false)]
  public MsofbtSp InnerSpRecord => this.m_shape;

  public bool IsShortVersion
  {
    get => this.m_clientAnchor.IsShortVersion;
    set => this.m_clientAnchor.IsShortVersion = value;
  }

  public int ShapeCount
  {
    get => !(this.m_record is MsofbtSpgrContainer record) ? 1 : record.ItemsList.Count;
  }

  public bool UpdatePositions
  {
    get => this.m_bUpdatePositions;
    set => this.m_bUpdatePositions = value;
  }

  public virtual int Instance => this.m_shape == null ? -1 : this.m_shape.Instance;

  public bool HasFill
  {
    get => this.m_fill != null && this.m_fill.Visible;
    internal set
    {
      if (!value)
      {
        this.m_fill = (ShapeFillImpl) null;
      }
      else
      {
        if (this.m_fill != null || !value)
          return;
        this.m_fill = new ShapeFillImpl(this.Application, this.Parent);
      }
    }
  }

  internal bool IsGroupFill
  {
    get => this.m_isGroupFill;
    set => this.m_isGroupFill = value;
  }

  internal bool IsGroupLine
  {
    get => this.m_isGroupLine;
    set => this.m_isGroupLine = value;
  }

  public bool HasLineFormat
  {
    get => this.m_lineFormat != null && this.m_lineFormat.Visible;
    internal set
    {
      if (value)
        return;
      this.m_lineFormat = (ShapeLineFormatImpl) null;
    }
  }

  public int ShapeId
  {
    get => this.m_iShapeId;
    set => this.m_iShapeId = value;
  }

  [CLSCompliant(false)]
  public MsofbtSp ShapeRecord
  {
    get
    {
      if (this.m_shape == null)
        this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
      return this.m_shape;
    }
  }

  internal bool IsActiveX
  {
    get
    {
      ftPioGrbit subRecord = (ftPioGrbit) this.Obj.FindSubRecord(TObjSubRecordType.ftPioGrbit);
      return subRecord != null && subRecord.IsActiveX;
    }
  }

  internal Dictionary<string, string> StyleProperties
  {
    get
    {
      return this.m_styleProperties == null ? new Dictionary<string, string>() : this.m_styleProperties;
    }
    set => this.m_styleProperties = value;
  }

  internal string PreserveStyleString
  {
    get => this.m_preserveStyleString;
    set => this.m_preserveStyleString = value;
  }

  internal bool IsHyperlink
  {
    get => this.m_isHyperlink;
    set => this.m_isHyperlink = value;
  }

  internal bool IsAbsoluteAnchor
  {
    get => this.m_bIsAbsoluteAnchor;
    set => this.m_bIsAbsoluteAnchor = value;
  }

  internal Stream NvPrExtLstStream
  {
    get => this.m_streamExtLst;
    set => this.m_streamExtLst = value;
  }

  internal GroupShapeImpl Group => this.Parent as GroupShapeImpl;

  internal bool IsGroup => this.m_shapeType == ExcelShapeType.Group;

  internal Dictionary<string, Stream> PreservedElements
  {
    get
    {
      if (this.m_preservedElements == null)
        this.m_preservedElements = new Dictionary<string, Stream>();
      return this.m_preservedElements;
    }
  }

  internal bool IsCustomGeometry
  {
    get => this.m_isCustomGeom;
    set => this.m_isCustomGeom = value;
  }

  internal ShapeFrame ShapeFrame
  {
    get => this.m_shapeFrame;
    set => this.m_shapeFrame = value;
  }

  internal ShapeFrame GroupFrame
  {
    get => this.m_groupFrame;
    set => this.m_groupFrame = value;
  }

  internal double GetBorderThickness()
  {
    ShapeLineFormatImpl line = this.Line as ShapeLineFormatImpl;
    return !line.IsWidthExist && line.Visible && this.m_book.LineStyles != null && line.DefaultLineStyleIndex != 0 && this.m_book.LineStyles.ContainsKey(line.DefaultLineStyleIndex) ? this.m_book.LineStyles[line.DefaultLineStyleIndex].Weight : line.Weight;
  }

  internal Color GetBorderColor()
  {
    Color empty = ColorExtension.Empty;
    TextBoxShapeImpl textBoxShapeImpl = this as TextBoxShapeImpl;
    AutoShapeImpl autoShapeImpl = this as AutoShapeImpl;
    CommentShapeImpl commentShapeImpl = this as CommentShapeImpl;
    return autoShapeImpl != null && (!autoShapeImpl.Line.Visible && !autoShapeImpl.ShapeExt.IsCreated || autoShapeImpl.ShapeExt.IsCreated && autoShapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && !autoShapeImpl.Line.Visible) || textBoxShapeImpl != null && !textBoxShapeImpl.Line.Visible || commentShapeImpl != null && !commentShapeImpl.Line.Visible ? empty : this.GetDefaultColor(PreservedFlag.Line, "lnRef");
  }

  internal Color GetFillColor()
  {
    Color empty = ColorExtension.Empty;
    TextBoxShapeImpl textBoxShapeImpl = this as TextBoxShapeImpl;
    return this is AutoShapeImpl autoShapeImpl && !autoShapeImpl.Fill.Visible || textBoxShapeImpl != null && !textBoxShapeImpl.Fill.Visible ? empty : this.GetDefaultColor(PreservedFlag.Fill, "fillRef");
  }

  internal Color GetDefaultColor(PreservedFlag flag, string elementTag)
  {
    Color color = ColorExtension.Empty;
    bool flag1 = false;
    TextBoxShapeImpl textBoxShapeImpl = this as TextBoxShapeImpl;
    AutoShapeImpl autoShape = this as AutoShapeImpl;
    CommentShapeImpl commentShapeImpl = this as CommentShapeImpl;
    CheckBoxShapeImpl checkBoxShapeImpl = this as CheckBoxShapeImpl;
    OptionButtonShapeImpl optionButtonShapeImpl = this as OptionButtonShapeImpl;
    ComboBoxShapeImpl comboBoxShapeImpl = this as ComboBoxShapeImpl;
    if (autoShape != null)
    {
      if (autoShape.ShapeExt.IsCreated)
      {
        switch (flag)
        {
          case PreservedFlag.Fill:
            color = Color.FromArgb(0, 68, 114, 196);
            break;
          case PreservedFlag.Line:
            color = Color.FromArgb(0, 47, 82, 143);
            break;
          default:
            color = ColorExtension.Black;
            break;
        }
      }
      else
      {
        switch (flag)
        {
          case PreservedFlag.Fill:
            color = Color.FromArgb(0, 91, 155, 213);
            break;
          case PreservedFlag.Line:
            color = Color.FromArgb(0, 65, 113, 156);
            break;
          default:
            color = ColorExtension.White;
            break;
        }
      }
    }
    else if (textBoxShapeImpl != null)
    {
      switch (flag)
      {
        case PreservedFlag.Fill:
          if ((textBoxShapeImpl.Fill as ShapeFillImpl).ForeColorObject.Value == -1)
          {
            color = ColorExtension.White;
            flag1 = true;
            break;
          }
          color = textBoxShapeImpl.Fill.ForeColor;
          flag1 = true;
          break;
        case PreservedFlag.Line:
          ShapeLineFormatImpl line1 = textBoxShapeImpl.Line as ShapeLineFormatImpl;
          if (line1.SchemeColorPreservedElements.ContainsKey("shade") && this.ParentWorkbook.DataHolder != null)
          {
            Stream data = (Stream) null;
            line1.SchemeColorPreservedElements.TryGetValue("shade", out data);
            double shade = (double) ChartParserCommon.ParseIntValueTag(UtilityMethods.CreateReader(data, "shade")) / 100000.0;
            color = this.ParentWorkbook.DataHolder.Parser.ConvertColorByShadeBlip(line1.ForeColor, shade);
            break;
          }
          color = textBoxShapeImpl.Line.ForeColor;
          break;
        default:
          color = ColorExtension.Black;
          break;
      }
    }
    else if (commentShapeImpl != null)
    {
      switch (flag)
      {
        case PreservedFlag.Fill:
          if ((commentShapeImpl.Fill as ShapeFillImpl).ForeColorObject.Value == -1)
          {
            color = ColorExtension.White;
            flag1 = true;
            break;
          }
          color = commentShapeImpl.Fill.ForeColor;
          flag1 = true;
          break;
        case PreservedFlag.Line:
          ShapeLineFormatImpl line2 = commentShapeImpl.Line as ShapeLineFormatImpl;
          if (line2.SchemeColorPreservedElements.ContainsKey("shade") && this.ParentWorkbook.DataHolder != null)
          {
            Stream data = (Stream) null;
            line2.SchemeColorPreservedElements.TryGetValue("shade", out data);
            double shade = (double) ChartParserCommon.ParseIntValueTag(UtilityMethods.CreateReader(data, "shade")) / 100000.0;
            color = this.ParentWorkbook.DataHolder.Parser.ConvertColorByShadeBlip(line2.ForeColor, shade);
            break;
          }
          color = commentShapeImpl.Line.ForeColor;
          break;
        default:
          color = ColorExtension.Black;
          break;
      }
    }
    else if (checkBoxShapeImpl != null)
    {
      switch (flag)
      {
        case PreservedFlag.Fill:
          if ((checkBoxShapeImpl.Fill as ShapeFillImpl).ForeColorObject.Value == -1)
          {
            color = ColorExtension.White;
            flag1 = true;
            break;
          }
          color = checkBoxShapeImpl.Fill.ForeColor;
          flag1 = true;
          break;
        case PreservedFlag.Line:
          color = checkBoxShapeImpl.Line.ForeColor;
          break;
      }
    }
    else if (optionButtonShapeImpl != null)
    {
      switch (flag)
      {
        case PreservedFlag.Fill:
          if ((optionButtonShapeImpl.Fill as ShapeFillImpl).ForeColorObject.Value == -1)
          {
            color = ColorExtension.White;
            flag1 = true;
            break;
          }
          color = optionButtonShapeImpl.Fill.ForeColor;
          flag1 = true;
          break;
        case PreservedFlag.Line:
          color = optionButtonShapeImpl.Line.ForeColor;
          break;
      }
    }
    else if (comboBoxShapeImpl != null)
    {
      switch (flag)
      {
        case PreservedFlag.Fill:
          if ((comboBoxShapeImpl.Fill as ShapeFillImpl).ForeColorObject.Value == -1)
          {
            color = ColorExtension.White;
            flag1 = true;
            break;
          }
          color = comboBoxShapeImpl.Fill.ForeColor;
          flag1 = true;
          break;
        case PreservedFlag.Line:
          color = comboBoxShapeImpl.Line.ForeColor;
          break;
      }
    }
    if (textBoxShapeImpl != null)
    {
      switch (flag)
      {
        case PreservedFlag.Fill:
          if (textBoxShapeImpl.StyleStream != null && !textBoxShapeImpl.IsFill && !textBoxShapeImpl.IsNoFill)
          {
            XmlReader reader = UtilityMethods.CreateReader(textBoxShapeImpl.StyleStream, elementTag);
            if (reader.MoveToAttribute("idx") && int.Parse(reader.Value) != 0)
            {
              reader.Read();
              if (this.ParentWorkbook.DataHolder.Parser.m_dicThemeColors != null)
              {
                color = ChartParserCommon.ReadColor(reader, out int _, out int _, out int _, this.ParentWorkbook.DataHolder.Parser);
                break;
              }
              break;
            }
            break;
          }
          break;
        case PreservedFlag.Line:
          ShapeLineFormatImpl line3 = textBoxShapeImpl.Line as ShapeLineFormatImpl;
          if (textBoxShapeImpl.StyleStream != null && (!textBoxShapeImpl.IsLineProperties || !line3.IsNoFill) && !line3.IsSolidFill)
          {
            XmlReader reader = UtilityMethods.CreateReader(textBoxShapeImpl.StyleStream, elementTag);
            if (reader.MoveToAttribute("idx") && int.Parse(reader.Value) != 0)
            {
              reader.Read();
              if (this.ParentWorkbook.DataHolder.Parser.m_dicThemeColors != null)
              {
                color = ChartParserCommon.ReadColor(reader, out int _, out int _, out int _, this.ParentWorkbook.DataHolder.Parser);
                break;
              }
              break;
            }
            break;
          }
          break;
        default:
          if (!flag1 && textBoxShapeImpl.StyleStream != null)
          {
            XmlReader reader = UtilityMethods.CreateReader(textBoxShapeImpl.StyleStream, elementTag);
            reader.Read();
            if (this.ParentWorkbook.DataHolder.Parser.m_dicThemeColors != null)
            {
              color = ChartParserCommon.ReadColor(reader, out int _, out int _, out int _, this.ParentWorkbook.DataHolder.Parser);
              break;
            }
            break;
          }
          break;
      }
    }
    else if (autoShape != null)
    {
      if (autoShape.ShapeExt.Logger.GetPreservedItem(flag) || flag == PreservedFlag.Fill && autoShape.IsGroupFill)
      {
        switch (flag)
        {
          case PreservedFlag.Fill:
            return autoShape.Fill.ForeColor;
          case PreservedFlag.Line:
            return autoShape.Line.ForeColor;
        }
      }
      else
      {
        string streamTag = "Style";
        if (flag == PreservedFlag.Fill && autoShape.ShapeExt.PreservedElements.ContainsKey("Fill"))
        {
          streamTag = "Fill";
          elementTag = "solidFill";
        }
        if (flag == PreservedFlag.Line && autoShape.ShapeExt.PreservedElements.ContainsKey("Line"))
        {
          streamTag = "Line";
          elementTag = "solidFill";
        }
        this.GetStyleColor(autoShape, flag, streamTag, elementTag, ref color);
      }
    }
    return color;
  }

  internal void GetStyleColor(
    AutoShapeImpl autoShape,
    PreservedFlag flag,
    string streamTag,
    string elementTag,
    ref Color color)
  {
    Stream data = (Stream) null;
    if (autoShape.ShapeExt.PreservedElements.TryGetValue(streamTag, out data))
    {
      if (data == null || data.Length <= 0L)
        return;
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data, elementTag);
      if (flag == PreservedFlag.Line && reader.NodeType == XmlNodeType.None)
      {
        this.GetStyleColor(autoShape, flag, "Style", "lnRef", ref color);
      }
      else
      {
        reader.Read();
        Color color1 = ChartParserCommon.ReadColor(reader, out int _, out int _, out int _, this.ParentWorkbook.DataHolder.Parser);
        if (this.ParentWorkbook.DataHolder.Parser.m_dicThemeColors == null && this.CheckIfColorEmpty(color1))
          return;
        color = color1;
      }
    }
    else
    {
      if (autoShape.ShapeExt.IsCreated || flag != PreservedFlag.RichText)
        return;
      color = ColorExtension.Black;
    }
  }

  private bool CheckIfColorEmpty(Color color)
  {
    return color.A == (byte) 0 && color.B == (byte) 0 && color.G == (byte) 0 && color.R == (byte) 0;
  }

  public virtual void GenerateDefaultName()
  {
    this.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this.m_shapes, "Shape ");
  }

  protected virtual void OnDelete()
  {
  }

  [CLSCompliant(false)]
  protected void SetObject(OBJRecord value)
  {
    this.m_object = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public virtual IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    ShapeImpl shapeImpl = (ShapeImpl) this.MemberwiseClone();
    shapeImpl.SetParent(parent);
    shapeImpl.SetParents();
    shapeImpl.CopyFrom(this, hashNewNames, dicFontIndexes);
    if (this.m_shapeFrame != null)
      shapeImpl.ShapeFrame = this.m_shapeFrame.Clone((object) shapeImpl);
    if (this.m_groupFrame != null)
      shapeImpl.GroupFrame = this.m_groupFrame.Clone((object) shapeImpl);
    shapeImpl.CloneLineFill(this);
    if (this.m_hyperLink != null)
      shapeImpl.m_hyperLink = (IHyperLink) (this.m_hyperLink as HyperLinkImpl).Clone((object) shapeImpl);
    if (shapeImpl.ShapeType == ExcelShapeType.AutoShape)
    {
      AutoShapeImpl autoShapeImpl = shapeImpl as AutoShapeImpl;
      autoShapeImpl.ShapeExt = (this as AutoShapeImpl).ShapeExt.Clone(shapeImpl);
      autoShapeImpl.ShapeExt.ShapeID = CollectionBaseEx<IShape>.GenerateID((ICollection<IShape>) this.m_shapes);
    }
    if (addToCollections)
      shapeImpl.m_shapes.AddShape(shapeImpl);
    shapeImpl.AttachEvents();
    shapeImpl.OldObjId = 0U;
    return (IShape) shapeImpl;
  }

  public object Clone(object parent)
  {
    return (object) this.Clone(parent, (Dictionary<string, string>) null, (Dictionary<int, int>) null, true);
  }

  public virtual void CopyFrom(
    ShapeImpl shape,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes)
  {
    MsoBase record = shape.m_record;
    if (record != null)
      this.m_record = (MsoBase) record.Clone();
    this.UpdateRecord(shape.ClientAnchor);
    this.m_iShapeId = shape.m_iShapeId;
  }

  public bool CanInsertRowColumn(int iIndex, int iCount, bool bRow, int iMaxIndex)
  {
    if (!this.IsMoveWithCell && !this.IsSizeWithCell)
      return true;
    if (this.IsIndexLess(iIndex, bRow))
    {
      if (this.IsMoveWithCell)
        return this.GetLowerBound(bRow) + iCount >= 0 && this.GetUpperBound(bRow) + iCount <= iMaxIndex;
    }
    else if (this.IsIndexMiddle(iIndex, bRow) && this.IsSizeWithCell)
      return this.GetUpperBound(bRow) + iCount <= iMaxIndex;
    return true;
  }

  private int GetLowerBound(bool bRow) => !bRow ? this.LeftColumn : this.TopRow;

  private int GetUpperBound(bool bRow) => !bRow ? this.RightColumn : this.BottomRow;

  public void RemoveRowColumn(int iIndex, int iCount, bool bRow)
  {
    int num = bRow ? this.BottomRow : this.RightColumn;
    bool flag = iIndex <= num;
    if (!this.IsMoveWithCell && !this.IsSizeWithCell)
    {
      this.UpdateNotSizeNotMoveShape(bRow, iIndex, -iCount);
      flag = false;
    }
    if (flag)
    {
      int countAbove = this.GetCountAbove(iIndex, iCount, bRow);
      if (countAbove > 0)
        this.UpdateAboveRowColumnIndexes(-countAbove, bRow);
      iCount -= countAbove;
      flag = iCount > 0;
    }
    if (flag && this.IndicatesFirst(iIndex, iCount, bRow))
    {
      this.UpdateFirstRowColumnIndexes(bRow, -1);
      --iCount;
      flag = iCount > 0;
    }
    if (flag)
    {
      int countInside = this.GetCountInside(iIndex, iCount, bRow);
      if (countInside > 0)
        this.UpdateInsideRowColumnIndexes(-countInside, bRow);
      iCount -= countInside;
      flag = iCount > 0;
    }
    if (!flag)
      return;
    this.UpdateLastRowColumnIndex(bRow);
  }

  public void InsertRowColumn(int iIndex, int iCount, bool bRow)
  {
    if (!this.IsSizeWithCell && !this.IsMoveWithCell)
      this.UpdateNotSizeNotMoveShape(bRow, iIndex, iCount);
    else if (this.IsIndexLess(iIndex, bRow))
    {
      if (!this.IsMoveWithCell)
        return;
      this.IncreaseAndUpdateAll(iCount, bRow);
    }
    else
    {
      if (!this.IsIndexMiddle(iIndex, bRow) || !this.IsSizeWithCell)
        return;
      this.IncreaseAndUpdateEnd(iCount, bRow);
    }
  }

  public virtual void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
  }

  public void SetName(string strShapeName)
  {
    switch (strShapeName)
    {
      case null:
        throw new ArgumentNullException(nameof (strShapeName));
      case "":
        throw new ArgumentException("strShapeName - string cannot be empty.");
      default:
        this.m_strName = strShapeName;
        break;
    }
  }

  public virtual void RegisterInSubCollection()
  {
  }

  public virtual bool CanCopyShapesOnRangeCopy(
    Rectangle sourceRec,
    Rectangle destRec,
    out Rectangle newPosition)
  {
    int leftColumn = this.LeftColumn;
    int topRow = this.TopRow;
    bool flag1 = leftColumn == this.RightColumn && (leftColumn > sourceRec.Right || leftColumn < sourceRec.Left) || topRow == this.BottomRow && (topRow > sourceRec.Bottom || topRow < sourceRec.Top);
    bool flag2 = this.IsMoveWithCell && !flag1;
    newPosition = new Rectangle(0, 0, 0, 0);
    if (flag2)
    {
      newPosition.Y = topRow - sourceRec.Top + destRec.Top;
      flag2 = sourceRec.Top - 1 <= topRow && newPosition.Top > 0;
    }
    if (flag2)
    {
      newPosition.X = leftColumn - sourceRec.Left + destRec.Left;
      flag2 = sourceRec.Left - 1 <= leftColumn && newPosition.Left > 0;
    }
    if (flag2)
    {
      int num = destRec.Bottom - (sourceRec.Bottom - this.BottomRow);
      flag2 = sourceRec.Bottom + 1 >= this.BottomRow && num <= this.m_book.MaxRowCount;
      newPosition.Height = num - newPosition.Y;
    }
    if (flag2)
    {
      int num = destRec.Right - (sourceRec.Right - this.RightColumn);
      flag2 = sourceRec.Right + 1 >= this.RightColumn && num <= this.m_book.MaxColumnCount;
      newPosition.Width = num - newPosition.X;
    }
    return flag2;
  }

  public virtual ShapeImpl CopyMoveShapeOnRangeCopyMove(
    WorksheetImpl sheet,
    Rectangle destRec,
    bool bIsCopy)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    ShapeImpl shapeImpl = this;
    ShapesCollection shapes = (ShapesCollection) sheet.Shapes;
    if (bIsCopy)
      shapeImpl = (ShapeImpl) shapeImpl.Clone((object) shapes, (Dictionary<string, string>) null, (Dictionary<int, int>) null, true);
    else if (sheet != this.Worksheet)
    {
      ((ShapesCollection) this.Worksheet.Shapes).Remove((IShape) this);
      shapes.AddShape(shapeImpl);
    }
    int height = this.Height;
    int width = this.Width;
    int iPixels1 = ShapeImpl.ConvertHeightOffsetIntoPixels((int) ApplicationImpl.ConvertToPixels(sheet.InnerGetRowHeight(shapeImpl.TopRow, false), MeasureUnits.Point), shapeImpl.ClientAnchor.TopOffset, false);
    shapeImpl.ClientAnchor.TopRow = destRec.Top - 1;
    if ((double) iPixels1 < ApplicationImpl.ConvertToPixels(sheet.InnerGetRowHeight(shapeImpl.TopRow, false), MeasureUnits.Point))
      shapeImpl.ClientAnchor.TopOffset = ShapeImpl.ConvertPixelsIntoHeightOffset(iPixels1, (int) ApplicationImpl.ConvertToPixels(sheet.InnerGetRowHeight(shapeImpl.TopRow, false), MeasureUnits.Point));
    int iPixels2 = ShapeImpl.ConvertWidthOffsetIntoPixels(this.m_shapes.Worksheet.GetColumnWidthInPixels(shapeImpl.LeftColumn), shapeImpl.LeftColumnOffset, false);
    shapeImpl.ClientAnchor.LeftColumn = destRec.Left - 1;
    if (iPixels2 < sheet.GetColumnWidthInPixels(shapeImpl.LeftColumn))
      shapeImpl.ClientAnchor.LeftOffset = ShapeImpl.ConvertPixelsIntoWidthOffset(iPixels2, sheet.GetColumnWidthInPixels(shapeImpl.LeftColumn));
    int iPixels3 = ShapeImpl.ConvertHeightOffsetIntoPixels((int) ApplicationImpl.ConvertToPixels(sheet.InnerGetRowHeight(shapeImpl.BottomRow, false), MeasureUnits.Point), shapeImpl.ClientAnchor.BottomOffset, false);
    int iPixels4 = ShapeImpl.ConvertWidthOffsetIntoPixels(this.m_shapes.Worksheet.GetColumnWidthInPixels(shapeImpl.RightColumn), shapeImpl.RightColumnOffset, false);
    if (this.IsSizeWithCell)
    {
      shapeImpl.ClientAnchor.BottomRow = destRec.Bottom - 1;
      shapeImpl.ClientAnchor.RightColumn = destRec.Right - 1;
      shapeImpl.UpdateWidth();
      shapeImpl.UpdateHeight();
    }
    else
    {
      shapeImpl.Height = height;
      shapeImpl.Width = width;
      shapeImpl.UpdateBottomRow();
      shapeImpl.UpdateRightColumn();
    }
    if ((double) iPixels3 < ApplicationImpl.ConvertToPixels(sheet.InnerGetRowHeight(shapeImpl.BottomRow, false), MeasureUnits.Point))
      shapeImpl.ClientAnchor.BottomOffset = ShapeImpl.ConvertPixelsIntoHeightOffset(iPixels3, (int) ApplicationImpl.ConvertToPixels(sheet.InnerGetRowHeight(shapeImpl.BottomRow, false), MeasureUnits.Point));
    if (iPixels4 < sheet.GetColumnWidthInPixels(shapeImpl.RightColumn))
      shapeImpl.ClientAnchor.RightOffset = ShapeImpl.ConvertPixelsIntoWidthOffset(iPixels4, sheet.GetColumnWidthInPixels(shapeImpl.RightColumn));
    shapeImpl.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this.m_shapes, shapeImpl.Name);
    if (shapeImpl.ParentWorkbook != this.Workbook && this.OnAction != string.Empty)
      shapeImpl.OnAction = this.m_shapes.UpdateMacro(this, shapeImpl, this.OnAction);
    return shapeImpl;
  }

  public void CopyFillOptions(ShapeImpl sourceShape, IDictionary dicFontIndexes)
  {
    if (sourceShape.m_lineFormat != null)
      this.m_lineFormat = sourceShape.m_lineFormat.Clone((object) this);
    if (sourceShape.m_fill == null)
      return;
    this.m_fill = sourceShape.m_fill.Clone((object) this);
  }

  public void PrepareForSerialization()
  {
    if (this.OldObjId == 0U)
    {
      ++this.m_book.CurrentObjectId;
      this.OldObjId = (uint) this.m_book.CurrentObjectId;
    }
    this.OnPrepareForSerialization();
    this.ShapeRecord.ShapeId = this.m_iShapeId;
  }

  protected virtual void OnPrepareForSerialization()
  {
    if (this.m_record is MsofbtSpContainer record)
      this.SerializeMsoOptions(record);
    if (this.m_object == null)
      return;
    this.UpdateMacroInfo();
  }

  private void UpdateMacroInfo()
  {
    List<ObjSubRecord> recordsList = this.m_object.RecordsList;
    ftMacro ftMacro = (ftMacro) null;
    int index = 0;
    for (int count = recordsList.Count; index < count; ++index)
    {
      ObjSubRecord objSubRecord = recordsList[index];
      if (objSubRecord.Type == TObjSubRecordType.ftMacro)
      {
        ftMacro = (ftMacro) objSubRecord;
        if (this.m_macroTokens == null)
        {
          recordsList.RemoveAt(index);
          break;
        }
        break;
      }
    }
    if (ftMacro == null && this.m_macroTokens != null)
    {
      ftMacro = new ftMacro();
      recordsList.Insert(recordsList.Count - 2, (ObjSubRecord) ftMacro);
    }
    if (this.m_macroTokens == null)
      return;
    ftMacro.Tokens = this.m_macroTokens;
  }

  internal void SetInstance(int instance) => this.ShapeRecord.Instance = instance;

  public void SetOption(MsoOptions option, int value)
  {
    this.ShapeOptions.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = option,
      Int32Value = value
    });
  }

  private MsofbtOPT ShapeOptions
  {
    get
    {
      if (this.m_options == null)
        this.m_options = this.CreateDefaultOptions();
      return this.m_options;
    }
  }

  internal List<ShapeImpl> ChildShapes
  {
    get
    {
      if (this.m_childShapes == null)
        this.m_childShapes = new List<ShapeImpl>();
      return this.m_childShapes;
    }
  }

  internal MsofbtChildAnchor ChildAnchor => this.m_childAnchor;

  internal MsoUnknown UnKnown => this.m_unknown;

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (this.m_macroTokens == null)
      return;
    this.m_book.FormulaUtil.UpdateNameIndex(this.m_macroTokens, arrNewIndex);
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (this.m_macroTokens == null)
      return;
    this.m_book.FormulaUtil.UpdateNameIndex(this.m_macroTokens, dicNewIndex);
  }

  private void UpdateLeftColumn()
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
    {
      this.ClientAnchor.LeftColumn = (int) this.m_rectAbsolute.Left;
      this.ClientAnchor.LeftOffset = 0;
    }
    else
    {
      int num = 0;
      int iColumnIndex = 0;
      for (; (double) num <= (double) this.m_rectAbsolute.Left; num += worksheet.GetColumnWidthInPixels(iColumnIndex))
      {
        ++iColumnIndex;
        if (iColumnIndex > this.m_book.MaxColumnCount)
        {
          this.ClientAnchor.LeftColumn = this.m_book.MaxColumnCount - 1;
          this.ClientAnchor.LeftOffset = 1024 /*0x0400*/;
          return;
        }
      }
      int columnWidthInPixels = worksheet.GetColumnWidthInPixels(iColumnIndex);
      int iPixels = (int) this.m_rectAbsolute.Left - (num - columnWidthInPixels);
      this.ClientAnchor.LeftColumn = iColumnIndex - 1;
      this.ClientAnchor.LeftOffset = ShapeImpl.ConvertPixelsIntoWidthOffset(iPixels, columnWidthInPixels);
    }
  }

  internal void ClearShapeOffset(bool clear)
  {
    if (!clear)
      return;
    this.ClientAnchor.LeftOffset = 0;
    this.ClientAnchor.RightOffset = 0;
    this.ClientAnchor.BottomOffset = 0;
    this.ClientAnchor.TopOffset = 0;
  }

  protected internal void UpdateRightColumn(int iCount)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
    {
      this.m_rectAbsolute.X = (float) this.ClientAnchor.LeftColumn;
      this.m_rectAbsolute.Y = this.m_rectAbsolute.Top;
      this.ClientAnchor.RightColumn = (int) ((double) this.m_rectAbsolute.Left + (double) this.m_rectAbsolute.Width);
      this.ClientAnchor.RightOffset = 0;
    }
    else
    {
      int width = this.Width;
      int leftColumn = this.LeftColumn;
      int iOffset = this.LeftColumnOffset;
      while (width >= 0)
      {
        if (leftColumn > this.m_book.MaxColumnCount)
        {
          this.RightColumn = this.m_book.MaxColumnCount;
          this.RightColumnOffset = 1024 /*0x0400*/;
          break;
        }
        int columnWidthInPixels = worksheet.GetColumnWidthInPixels(leftColumn + iCount);
        int num = (columnWidthInPixels > worksheet.GetColumnWidthInPixels(leftColumn) ? columnWidthInPixels : worksheet.GetColumnWidthInPixels(leftColumn)) - this.OffsetInPixels(leftColumn, iOffset, true);
        if (num < 0)
          num = 0;
        if (num < 0)
          throw new ArgumentOutOfRangeException("Calculated value can't be less than zero, error in coordinates update.");
        if (num > width)
        {
          this.RightColumn = leftColumn;
          this.RightColumnOffset = iOffset + this.PixelsInOffset(leftColumn, width, true);
          break;
        }
        width -= num;
        ++leftColumn;
        iOffset = 0;
      }
    }
  }

  protected internal void UpdateRightColumn() => this.UpdateRightColumn(0);

  private void UpdateTopRow()
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
    {
      this.ClientAnchor.TopRow = (int) this.m_rectAbsolute.Y;
      this.ClientAnchor.TopOffset = 0;
    }
    else
    {
      int num = 0;
      int iRow = 0;
      for (; (double) num <= (double) this.m_rectAbsolute.Top; num += (int) ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(iRow, false), MeasureUnits.Point))
        ++iRow;
      int pixels = (int) ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(iRow, false), MeasureUnits.Point);
      int iPixels = (int) ((double) this.m_rectAbsolute.Top - (double) (num - pixels));
      this.ClientAnchor.TopRow = iRow - 1;
      this.ClientAnchor.TopOffset = ShapeImpl.ConvertPixelsIntoHeightOffset(iPixels, pixels);
    }
  }

  protected internal void UpdateBottomRow()
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
    {
      this.m_rectAbsolute.X = this.m_rectAbsolute.Left;
      this.m_rectAbsolute.Y = (float) this.ClientAnchor.TopRow;
      this.ClientAnchor.BottomRow = (int) ((double) this.m_rectAbsolute.Top + (double) this.m_rectAbsolute.Height);
      this.ClientAnchor.BottomOffset = 0;
    }
    else
    {
      int height = this.Height;
      int topRow = this.TopRow;
      int iOffset = this.TopRowOffset;
      int maxRowCount = this.m_book.MaxRowCount;
      while (height >= 0)
      {
        if (topRow > maxRowCount)
        {
          this.BottomRow = maxRowCount;
          this.BottomRowOffset = 256 /*0x0100*/;
          break;
        }
        int pixels = (int) ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(topRow, false), MeasureUnits.Point);
        int num = pixels - this.OffsetInPixels((double) pixels, iOffset, false);
        if (num < 0)
          throw new ArgumentOutOfRangeException("Calculated value can't be less than zero, error in coordinates update.");
        if (num > height)
        {
          this.BottomRow = topRow;
          this.BottomRowOffset = iOffset + this.PixelsInOffset((double) pixels, height, false);
          break;
        }
        height -= num;
        ++topRow;
        iOffset = 0;
      }
    }
  }

  internal void UpdateAnchorPoints()
  {
    this.EvaluateTopPosition();
    this.EvaluateLeftPosition();
  }

  protected internal void UpdateWidth()
  {
    this.m_rectAbsolute.Width = (float) this.GetWidth(this.LeftColumn, this.LeftColumnOffset, this.RightColumn, this.RightColumnOffset, false);
    this.m_shapeFrame.OffsetCX = (long) ApplicationImpl.ConvertFromPixel((double) this.m_rectAbsolute.Width, MeasureUnits.EMU);
  }

  protected internal void UpdateHeight()
  {
    this.m_rectAbsolute.Height = (float) (int) this.GetHeight(this.TopRow, this.TopRowOffset, this.BottomRow, this.BottomRowOffset, false);
    this.m_shapeFrame.OffsetCY = (long) ApplicationImpl.ConvertFromPixel((double) this.m_rectAbsolute.Height, MeasureUnits.EMU);
  }

  internal int OffsetInPixels(int iRowColumn, int iOffset, bool isXOffset)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return 0;
    return isXOffset ? this.OffsetInPixels((double) worksheet.GetColumnWidthInPixels(iRowColumn), iOffset, isXOffset) : this.OffsetInPixels(ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(iRowColumn, false), MeasureUnits.Point), iOffset, isXOffset);
  }

  internal int OffsetInPixels(double iWidthHeight, int iOffset, bool isXOffset)
  {
    return this.m_shapes.Worksheet == null ? 0 : (int) Math.Round(!isXOffset ? (double) iOffset * iWidthHeight / 256.0 : (double) iOffset * iWidthHeight / 1024.0);
  }

  internal int PixelsInOffset(int iCurRowColumn, int iPixels, bool isXSize)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return 0;
    if (iPixels < 0)
      throw new ArgumentOutOfRangeException("IPixels", "Can't be less than zero.");
    return isXSize ? this.PixelsInOffset((double) worksheet.GetColumnWidthInPixels(iCurRowColumn), iPixels, isXSize) : this.PixelsInOffset((double) (int) ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(iCurRowColumn, false), MeasureUnits.Point), iPixels, isXSize);
  }

  internal int PixelsInOffset(double iWidthHeight, int iPixels, bool isXSize)
  {
    if (this.m_shapes.Worksheet == null)
      return 0;
    if (iPixels < 0)
      throw new ArgumentOutOfRangeException("IPixels", "Can't be less than zero.");
    return isXSize ? (iWidthHeight != 0.0 ? (int) ((double) (iPixels * 1024 /*0x0400*/) / iWidthHeight) : (int) iWidthHeight) : (iWidthHeight != 0.0 ? (int) ((double) (iPixels * 256 /*0x0100*/) / iWidthHeight) : 256 /*0x0100*/);
  }

  internal int GetWidth(
    int iColumn1,
    int iOffset1,
    int iColumn2,
    int iOffset2,
    bool bIsOffsetInPixels)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return iColumn2 - iColumn1;
    if (iColumn1 < 1 || iColumn1 > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumn1));
    if (iOffset2 == 0)
    {
      --iColumn2;
      iOffset2 = 1024 /*0x0400*/;
    }
    if (iColumn1 > iColumn2)
      return 0;
    if (iColumn2 < 1 || iColumn2 > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (iColumn2));
    if (iOffset1 < 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset1));
    if (iOffset2 < 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset2));
    if (iColumn1 == iColumn2 && iOffset1 > iOffset2)
      return 0;
    int columnWidthInPixels1 = worksheet.GetColumnWidthInPixels(iColumn1);
    int columnWidthInPixels2 = worksheet.GetColumnWidthInPixels(iColumn2);
    int num = ShapeImpl.ConvertWidthOffsetIntoPixels(columnWidthInPixels1, Math.Min(iOffset1, 1024 /*0x0400*/), bIsOffsetInPixels);
    int width = ShapeImpl.ConvertWidthOffsetIntoPixels(columnWidthInPixels2, Math.Min(iOffset2, 1024 /*0x0400*/), bIsOffsetInPixels) - num;
    for (int iColumnIndex = iColumn1; iColumnIndex < iColumn2; ++iColumnIndex)
      width += worksheet.GetColumnWidthInPixels(iColumnIndex);
    return width;
  }

  internal float GetHeight(
    int iRow1,
    int iOffset1,
    int iRow2,
    int iOffset2,
    bool bIsOffsetInPixels)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return (float) (iRow2 - iRow1);
    if (iRow1 < 1 || iRow1 > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRow1));
    if (iRow2 < 1 || iRow2 > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRow2));
    if (iRow2 < iRow1 || iRow1 == iRow2 && iOffset1 > iOffset2)
      return 0.0f;
    int pixels1 = (int) ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(iRow1, false), MeasureUnits.Point);
    int pixels2 = (int) ApplicationImpl.ConvertToPixels(worksheet.InnerGetRowHeight(iRow2, false), MeasureUnits.Point);
    int num = ShapeImpl.ConvertHeightOffsetIntoPixels(pixels1, iOffset1, bIsOffsetInPixels);
    return (float) (ShapeImpl.ConvertHeightOffsetIntoPixels(pixels2, iOffset2, bIsOffsetInPixels) - num) + (worksheet.RowHeightHelper.GetTotal(iRow2 - 1) - worksheet.RowHeightHelper.GetTotal(iRow1 - 1));
  }

  internal static int ConvertWidthOffsetIntoPixels(int iColWidth, int iOffset, bool bIsInPixels)
  {
    int num = iOffset;
    if (!bIsInPixels)
      num = (int) Math.Round((double) (iOffset * iColWidth) / 1024.0);
    return num;
  }

  internal static int ConvertHeightOffsetIntoPixels(
    int iRowHeight,
    int iOffset,
    bool bIsOffsetInPixels)
  {
    int num = iOffset;
    if (!bIsOffsetInPixels)
      num = (int) Math.Round((double) (iOffset * iRowHeight) / 256.0);
    return num;
  }

  internal static int ConvertPixelsIntoWidthOffset(int iPixels, int iColWidth)
  {
    return iColWidth == 0 ? 0 : iPixels * 1024 /*0x0400*/ / iColWidth;
  }

  internal static int ConvertPixelsIntoHeightOffset(int iPixels, int iRowHeight)
  {
    if (iPixels < 0 || iPixels > iRowHeight)
      throw new ArgumentOutOfRangeException(nameof (iPixels));
    return iPixels * 256 /*0x0100*/ / iRowHeight;
  }

  public void EvaluateTopLeftPosition()
  {
    this.EvaluateLeftPosition();
    this.EvaluateTopPosition();
  }

  private void EvaluateLeftPosition()
  {
    this.m_rectAbsolute.X = (float) this.GetWidth(1, 0, this.LeftColumn, this.LeftColumnOffset, false);
    this.m_shapeFrame.OffsetX = (long) ApplicationImpl.ConvertFromPixel((double) this.m_rectAbsolute.X, MeasureUnits.EMU);
  }

  private void EvaluateRightPosition()
  {
    this.m_rectAbsolute.Y = (float) (int) this.GetHeight(1, 0, this.TopRow, this.TopRowOffset, false);
  }

  private void EvaluateTopPosition()
  {
    this.m_rectAbsolute.Y = (float) (int) this.GetHeight(1, 0, this.TopRow, this.TopRowOffset, false);
    this.m_shapeFrame.OffsetY = (long) ApplicationImpl.ConvertFromPixel((double) this.m_rectAbsolute.Y, MeasureUnits.EMU);
  }

  [CLSCompliant(false)]
  protected void SetClientAnchor(MsofbtClientAnchor anchor)
  {
    this.m_clientAnchor = anchor != null ? anchor : throw new ArgumentOutOfRangeException(nameof (anchor));
  }

  private void OnLeftColumnChange()
  {
    if (this.IsSizeWithCell)
      this.UpdateWidth();
    else
      this.UpdateRightColumn();
  }

  private void OnTopRowChanged()
  {
    this.EvaluateTopPosition();
    if (this.IsSizeWithCell)
      this.UpdateHeight();
    else
      this.UpdateBottomRow();
  }

  private bool IsIndexLess(int iRowColumnIndex, bool bIsRow)
  {
    return !bIsRow ? iRowColumnIndex <= this.LeftColumn : iRowColumnIndex <= this.TopRow;
  }

  private bool IsIndexMiddle(int iRowColumnIndex, bool bIsRow)
  {
    return !bIsRow ? iRowColumnIndex <= this.RightColumn : iRowColumnIndex <= this.BottomRow;
  }

  private bool IsIndexLast(int iRowColumnIndex, bool bIsRow) => throw new NotImplementedException();

  private void IncreaseAndUpdateAll(int iCount, bool bIsRow)
  {
    if (bIsRow)
    {
      if (this.ClientAnchor.TopRow + iCount >= 0)
        this.ClientAnchor.TopRow += iCount;
      else
        this.ClientAnchor.TopOffset = 0;
      this.ClientAnchor.BottomRow += iCount;
      this.EvaluateTopPosition();
    }
    else
    {
      if (this.ClientAnchor.LeftColumn + iCount >= 0)
        this.ClientAnchor.LeftColumn += iCount;
      else
        this.ClientAnchor.LeftOffset = 0;
      this.ClientAnchor.RightColumn += iCount;
      this.EvaluateLeftPosition();
    }
  }

  private void IncreaseAndUpdateEnd(int iCount, bool bIsRow)
  {
    if (bIsRow)
    {
      this.ClientAnchor.BottomRow += iCount;
      this.UpdateHeight();
    }
    else
    {
      this.ClientAnchor.RightColumn += iCount;
      this.UpdateWidth();
    }
  }

  private int GetCountAbove(int iIndex, int iCount, bool bIsRow)
  {
    int num = bIsRow ? this.TopRow : this.LeftColumn;
    return iIndex < num ? Math.Min(iCount, num - iIndex) : 0;
  }

  private int GetCountInside(int iIndex, int iCount, bool bIsRow)
  {
    int val1_1 = bIsRow ? this.BottomRow - 1 : this.RightColumn - 1;
    if (iIndex > val1_1)
      return 0;
    int val1_2 = bIsRow ? this.TopRow + 1 : this.LeftColumn + 1;
    int val2 = iIndex + iCount - 1;
    int num = Math.Max(val1_2, iIndex) - Math.Min(val1_1, val2) + 1;
    return num <= 0 ? 0 : num;
  }

  private bool IndicatesFirst(int iIndex, int iCount, bool bIsRow)
  {
    int num1 = bIsRow ? this.TopRow : this.LeftColumn;
    int num2 = bIsRow ? this.BottomRow : this.RightColumn;
    if (num1 == num2)
      return false;
    int num3 = num1 - iIndex;
    return num3 >= 0 && num3 < iCount;
  }

  private void UpdateAboveRowColumnIndexes(int iCount, bool bIsRow)
  {
    if (bIsRow)
    {
      this.ClientAnchor.TopRow += iCount;
      this.ClientAnchor.BottomRow += iCount;
      this.EvaluateTopPosition();
    }
    else
    {
      this.ClientAnchor.LeftColumn += iCount;
      this.ClientAnchor.RightColumn += iCount;
      this.EvaluateLeftPosition();
    }
  }

  private void UpdateFirstRowColumnIndexes(bool bIsRow, int iCount)
  {
    if (bIsRow)
    {
      this.ClientAnchor.TopOffset = 0;
      this.ClientAnchor.BottomRow += iCount;
      int width = this.Width;
      this.EvaluateTopPosition();
      if (!this.IsSizeWithCell)
        this.Width = width;
    }
    else
    {
      this.ClientAnchor.LeftOffset = 0;
      this.ClientAnchor.RightColumn += iCount;
      int height = this.Height;
      this.EvaluateLeftPosition();
      if (!this.IsSizeWithCell)
        this.Height = height;
    }
    if (this.IsSizeWithCell)
      return;
    this.UpdateNotSizeNotMoveShape(bIsRow, 0, iCount);
  }

  protected virtual void UpdateNotSizeNotMoveShape(bool bRow, int iIndex, int iCount)
  {
    if (bRow)
    {
      this.UpdateTopRow();
      this.UpdateBottomRow();
    }
    else
    {
      this.UpdateLeftColumn();
      this.UpdateRightColumn();
    }
  }

  private void UpdateInsideRowColumnIndexes(int iCount, bool bRow)
  {
    if (this.IsSizeWithCell)
    {
      if (bRow)
      {
        this.ClientAnchor.BottomRow += iCount;
        this.EvaluateTopPosition();
      }
      else
      {
        this.ClientAnchor.RightColumn += iCount;
        this.EvaluateRightPosition();
      }
    }
    else
      this.UpdateNotSizeNotMoveShape(bRow, 0, iCount);
  }

  private void UpdateLastRowColumnIndex(bool bRow)
  {
    if (this.IsSizeWithCell)
    {
      if (bRow)
      {
        this.ClientAnchor.BottomOffset = 0;
        this.EvaluateTopPosition();
      }
      else
      {
        this.ClientAnchor.RightOffset = 0;
        this.EvaluateLeftPosition();
      }
    }
    else
      this.UpdateNotSizeNotMoveShape(bRow, 0, 1);
  }

  private void UpdateRecord(MsofbtClientAnchor anchor)
  {
    if (anchor == null)
      throw new ArgumentNullException(nameof (anchor));
    if (!(this.m_record is MsofbtSpContainer record))
    {
      this.m_clientAnchor = (MsofbtClientAnchor) anchor.Clone();
    }
    else
    {
      IList itemsList = (IList) record.ItemsList;
      int index = 0;
      for (int count = itemsList.Count; index < count; ++index)
        this.UpdateMso(itemsList[index] as MsoBase);
    }
  }

  private void ParseLineFill(MsofbtOPT options)
  {
    this.m_fill = new ShapeFillImpl(this.Application, (object) this);
    this.m_fill.Visible = false;
    this.m_lineFormat = new ShapeLineFormatImpl(this.Application, (object) this);
    this.m_lineFormat.Visible = false;
    this.m_bUpdateLineFill = true;
    if (options == null)
      return;
    this.ParseOptions(options);
  }

  [CLSCompliant(false)]
  protected virtual bool UpdateMso(MsoBase mso)
  {
    switch (mso)
    {
      case null:
        throw new ArgumentNullException(nameof (mso));
      case MsofbtClientAnchor _:
        this.m_clientAnchor = mso as MsofbtClientAnchor;
        return true;
      case MsofbtClientData _:
        this.m_object = (mso as MsofbtClientData).ObjectRecord;
        return true;
      case MsofbtOPT _:
        this.m_options = mso as MsofbtOPT;
        return true;
      case MsofbtSp _:
        this.m_shape = mso as MsofbtSp;
        return true;
      default:
        return false;
    }
  }

  protected void CloneLineFill(ShapeImpl sourceShape)
  {
    if (sourceShape == null)
      throw new ArgumentNullException(nameof (sourceShape));
    if (!this.m_bUpdateLineFill)
      return;
    if (sourceShape.m_fill != null)
      this.m_fill = sourceShape.m_fill.Clone((object) this);
    if (sourceShape.m_lineFormat == null)
      return;
    this.m_lineFormat = sourceShape.m_lineFormat.Clone((object) this);
  }

  private void CodeName_Changed(object sender, ValueChangedEventArgs e)
  {
    string[] strArray1 = this.OnAction.Split('.');
    if (strArray1.Length <= 1)
      return;
    string[] strArray2 = strArray1[0].Split('!');
    if (strArray2.Length > 1)
      strArray1[0] = strArray2[1];
    if (strArray1[0] == e.oldValue.ToString())
      strArray1[0] = e.newValue.ToString();
    this.OnAction = $"{strArray1[0]}.{strArray1[1]}";
  }

  private void Worksheet_ColumnWidthChanged(object sender, ValueChangedEventArgs e)
  {
    bool flag = this.m_clientAnchor == null || this.m_clientAnchor.LeftOffset == this.m_clientAnchor.RightOffset && this.m_clientAnchor.RightOffset == this.m_clientAnchor.TopOffset && this.m_clientAnchor.TopOffset == this.m_clientAnchor.BottomOffset && this.m_clientAnchor.TopOffset == 0 && this.m_clientAnchor.LeftColumn == this.m_clientAnchor.RightColumn && this.m_clientAnchor.TopRow == this.m_clientAnchor.BottomRow;
    if (this.m_book.Loading || flag)
      return;
    int oldValue = (int) e.oldValue;
    if (oldValue > this.RightColumn)
      return;
    if (oldValue < this.LeftColumn)
    {
      if (this.IsMoveWithCell)
      {
        this.EvaluateLeftPosition();
      }
      else
      {
        this.UpdateLeftColumn();
        this.UpdateRightColumn();
      }
    }
    else if (oldValue == this.LeftColumn)
    {
      if (this.IsSizeWithCell)
      {
        this.UpdateLeftColumn();
        this.UpdateWidth();
      }
      else
      {
        this.UpdateLeftColumn();
        this.UpdateRightColumn();
      }
    }
    else if (oldValue == this.RightColumn)
    {
      if (this.IsSizeWithCell)
        this.LeaveRelativeBottomRightCorner();
      else
        this.UpdateRightColumn();
    }
    else if (this.IsSizeWithCell)
      this.UpdateWidth();
    else
      this.UpdateRightColumn();
  }

  private void LeaveRelativeBottomRightCorner()
  {
    if (this.m_book.Loading)
      return;
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return;
    int width = this.GetWidth(this.LeftColumn, this.LeftColumnOffset, this.RightColumn, this.RightColumnOffset, false);
    int columnWidthInPixels = worksheet.GetColumnWidthInPixels(this.RightColumn);
    int num = this.RightColumnOffset + ShapeImpl.ConvertPixelsIntoWidthOffset(Math.Min(this.Width - width, columnWidthInPixels), columnWidthInPixels);
    this.RightColumnOffset = num < 0 ? 0 : num;
    this.UpdateWidth();
  }

  private void NormalFont_OnAfterChange(object sender, EventArgs e)
  {
    if (this.m_book.Loading)
      return;
    if (!this.IsMoveWithCell)
    {
      this.UpdateLeftColumn();
      this.UpdateTopRow();
    }
    else
      this.EvaluateTopLeftPosition();
    if (!this.IsSizeWithCell)
    {
      this.UpdateRightColumn();
      this.UpdateBottomRow();
    }
    else
    {
      this.UpdateWidth();
      this.UpdateHeight();
    }
  }

  private void Worksheet_RowHeightChanged(object sender, ValueChangedEventArgs e)
  {
    if (this.m_book.Loading)
      return;
    int oldValue = (int) e.oldValue;
    if (oldValue > this.BottomRow)
      return;
    if (oldValue < this.TopRow)
    {
      if (this.IsMoveWithCell)
      {
        this.EvaluateTopPosition();
      }
      else
      {
        this.UpdateTopRow();
        this.UpdateBottomRow();
      }
    }
    else if (oldValue == this.TopRow)
    {
      if (this.IsSizeWithCell)
      {
        this.UpdateTopRow();
        this.UpdateHeight();
      }
      else
      {
        this.UpdateTopRow();
        this.UpdateBottomRow();
      }
    }
    else if (oldValue == this.BottomRow)
    {
      if (this.IsSizeWithCell)
        this.LeaveRelativeBottomRightCorner();
      else
        this.UpdateBottomRow();
    }
    else if (this.IsSizeWithCell)
      this.UpdateHeight();
    else
      this.UpdateBottomRow();
  }

  internal void CheckLeftOffset()
  {
    int leftOffset = this.ClientAnchor.LeftOffset;
    int num1 = this.ClientAnchor.LeftColumn + 1;
    int columnWidthInPixels = this.m_shapes.Worksheet.GetColumnWidthInPixels(num1);
    int num2 = this.OffsetInPixels(num1, leftOffset, true);
    if (columnWidthInPixels >= num2)
      return;
    ++this.ClientAnchor.LeftColumn;
    this.ClientAnchor.LeftOffset = num2 - num1;
  }

  internal void UpdateGroupFrame(bool isAll)
  {
    if (this.Group == null)
      return;
    long left;
    long top;
    long width;
    long height;
    this.UpdateGroupPositions(out left, out top, out width, out height);
    int rotation = this.ShapeFrame.Rotation;
    if ((!isAll ? 1 : (!this.IsGroup ? 1 : 0)) != 0)
    {
      RectangleF shapeRect = new RectangleF((float) ApplicationImpl.ConvertToPixels((double) left, MeasureUnits.EMU), (float) ApplicationImpl.ConvertToPixels((double) top, MeasureUnits.EMU), (float) ApplicationImpl.ConvertToPixels((double) width, MeasureUnits.EMU), (float) ApplicationImpl.ConvertToPixels((double) height, MeasureUnits.EMU));
      RectangleF updatedRectangle = this.GetUpdatedRectangle(this, shapeRect);
      shapeRect.X = updatedRectangle.X;
      shapeRect.Y = updatedRectangle.Y;
      left = (long) ApplicationImpl.ConvertFromPixel((double) shapeRect.Left, MeasureUnits.EMU);
      top = (long) ApplicationImpl.ConvertFromPixel((double) shapeRect.Top, MeasureUnits.EMU);
      rotation = this.GetShapeRotation() * 60000;
    }
    this.m_groupFrame.SetAnchor(rotation, left, top, width, height);
  }

  internal void UpdateGroupFrame()
  {
    if (this.Group == null)
      return;
    long left;
    long top;
    long width;
    long height;
    this.UpdateGroupPositions(out left, out top, out width, out height);
    this.m_groupFrame.SetAnchor(this.ShapeFrame.Rotation, left, top, width, height);
  }

  private void UpdateGroupPositions(out long left, out long top, out long width, out long height)
  {
    this.m_groupFrame = new ShapeFrame(this);
    ShapeFrame shapeFrame = this.Group.GroupFrame ?? this.Group.ShapeFrame;
    double num1 = Math.Round((double) shapeFrame.OffsetCX / (double) shapeFrame.ChOffsetCX, 4);
    double num2 = Math.Round((double) shapeFrame.OffsetCY / (double) shapeFrame.ChOffsetCY, 4);
    left = (long) Math.Round((double) shapeFrame.OffsetX + (double) (this.ShapeFrame.OffsetX - shapeFrame.ChOffsetX) * num1);
    top = (long) Math.Round((double) shapeFrame.OffsetY + (double) (this.ShapeFrame.OffsetY - shapeFrame.ChOffsetY) * num2);
    width = this.ShapeFrame.OffsetCX;
    height = this.ShapeFrame.OffsetCY;
    int num3 = this.ShapeFrame.Rotation == -1 ? 0 : Math.Abs(this.ShapeFrame.Rotation % 21600000);
    if (num3 >= 2700000 && num3 <= 8099999 || num3 >= 13500000 && num3 <= 18899999)
    {
      double num4 = num1;
      num1 = num2;
      num2 = num4;
      left = (long) ((double) left - (double) width * (num1 - num2) / 2.0);
      top = (long) ((double) top - (double) height * (num2 - num1) / 2.0);
    }
    width = (long) Math.Round((double) width * num1);
    height = (long) Math.Round((double) height * num2);
    this.m_groupFrame.SetChildAnchor(this.ShapeFrame.ChOffsetX, this.ShapeFrame.ChOffsetY, this.ShapeFrame.ChOffsetCX, this.ShapeFrame.ChOffsetCY);
  }

  private RectangleF GetChildShapePositionToDraw(
    RectangleF groupShapeBounds,
    float groupShapeRotation,
    RectangleF childShapeBounds)
  {
    double num1 = (double) groupShapeBounds.X + (double) groupShapeBounds.Width / 2.0;
    double num2 = (double) groupShapeBounds.Y + (double) groupShapeBounds.Height / 2.0;
    if ((double) groupShapeRotation > 360.0)
      groupShapeRotation %= 360f;
    double num3 = (double) groupShapeRotation * Math.PI / 180.0;
    double num4 = Math.Sin(num3);
    double num5 = Math.Cos(num3);
    double num6 = (double) childShapeBounds.X + (double) childShapeBounds.Width / 2.0;
    double num7 = (double) childShapeBounds.Y + (double) childShapeBounds.Height / 2.0;
    double num8 = num1 + ((double) childShapeBounds.X - num1) * num5 - ((double) childShapeBounds.Y - num2) * num4;
    double num9 = num2 + ((double) childShapeBounds.X - num1) * num4 + ((double) childShapeBounds.Y - num2) * num5;
    double num10 = num1 + (num6 - num1) * num5 - (num7 - num2) * num4;
    double num11 = num2 + (num6 - num1) * num4 + (num7 - num2) * num5;
    double num12 = (360.0 - (double) groupShapeRotation) * Math.PI / 180.0;
    double num13 = Math.Sin(num12);
    double num14 = Math.Cos(num12);
    return new RectangleF((float) (num10 + (num8 - num10) * num14 - (num9 - num11) * num13), (float) (num11 + (num8 - num10) * num13 + (num9 - num11) * num14), childShapeBounds.Width, childShapeBounds.Height);
  }

  internal int GetShapeRotation()
  {
    ShapeImpl shapeImpl = this;
    int shapeRotation = shapeImpl.ShapeRotation;
    for (GroupShapeImpl group = shapeImpl.Group; group != null; group = group.Group)
    {
      int num = group.ShapeRotation;
      if (group.FlipVertical ^ group.FlipHorizontal)
        num = 360 - num;
      shapeRotation = (shapeRotation + num) % 360;
      if (group.FlipVertical ^ group.FlipHorizontal)
        shapeRotation = 360 - shapeRotation;
    }
    return shapeRotation;
  }

  private RectangleF GetUpdatedRectangle(ShapeImpl shape, RectangleF shapeRect)
  {
    GroupShapeImpl group = shape.Group;
    int shapeRotation1 = shape.ShapeRotation;
    for (; group != null; group = group.Group)
    {
      float pixels1;
      float pixels2;
      float pixels3;
      float pixels4;
      if (group.GroupFrame != null)
      {
        pixels1 = (float) ApplicationImpl.ConvertToPixels((double) group.GroupFrame.OffsetX, MeasureUnits.EMU);
        pixels2 = (float) ApplicationImpl.ConvertToPixels((double) group.GroupFrame.OffsetY, MeasureUnits.EMU);
        pixels3 = (float) ApplicationImpl.ConvertToPixels((double) group.GroupFrame.OffsetCX, MeasureUnits.EMU);
        pixels4 = (float) ApplicationImpl.ConvertToPixels((double) group.GroupFrame.OffsetCY, MeasureUnits.EMU);
      }
      else
      {
        pixels1 = (float) ApplicationImpl.ConvertToPixels((double) group.ShapeFrame.OffsetX, MeasureUnits.EMU);
        pixels2 = (float) ApplicationImpl.ConvertToPixels((double) group.ShapeFrame.OffsetY, MeasureUnits.EMU);
        pixels3 = (float) ApplicationImpl.ConvertToPixels((double) group.ShapeFrame.OffsetCX, MeasureUnits.EMU);
        pixels4 = (float) ApplicationImpl.ConvertToPixels((double) group.ShapeFrame.OffsetCY, MeasureUnits.EMU);
      }
      RectangleF rectangleF = new RectangleF(pixels1, pixels2, pixels3, pixels4);
      shapeRect = this.GetChildShapePositionToDraw(rectangleF, group.FlipVertical ^ group.FlipHorizontal ? (float) (360 - group.ShapeRotation) : (float) group.ShapeRotation, shapeRect);
      PointF[] pointFArray = new PointF[4]
      {
        shapeRect.Location,
        new PointF(shapeRect.X + shapeRect.Width, shapeRect.Y),
        new PointF(shapeRect.Right, shapeRect.Bottom),
        new PointF(shapeRect.X, shapeRect.Y + shapeRect.Height)
      };
      this.GetTransformMatrix(rectangleF, group.FlipVertical, group.FlipHorizontal).TransformPoints(pointFArray);
      shapeRect = ShapeImpl.CreateRect(pointFArray);
      int shapeRotation2 = group.ShapeRotation;
    }
    return shapeRect;
  }

  private static RectangleF CreateRect(PointF[] points)
  {
    float x1 = float.MaxValue;
    float y1 = float.MaxValue;
    float num1 = float.MinValue;
    float num2 = float.MinValue;
    int length = points.Length;
    for (int index = 0; index < length; ++index)
    {
      float x2 = points[index].X;
      float y2 = points[index].Y;
      if ((double) x2 < (double) x1)
        x1 = x2;
      if ((double) x2 > (double) num1)
        num1 = x2;
      if ((double) y2 < (double) y1)
        y1 = y2;
      if ((double) y2 > (double) num2)
        num2 = y2;
    }
    return new RectangleF(x1, y1, num1 - x1, num2 - y1);
  }

  private Matrix GetTransformMatrix(RectangleF bounds, bool flipV, bool flipH)
  {
    Matrix matrix = new Matrix();
    PointF pointF = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    Matrix target1 = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
    Matrix target2 = new Matrix(-1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    if (flipV)
    {
      this.MatrixMultiply(matrix, target1, MatrixOrder.Append);
      this.MatrixTranslate(matrix, 0.0f, pointF.Y * 2f, MatrixOrder.Append);
    }
    if (flipH)
    {
      this.MatrixMultiply(matrix, target2, MatrixOrder.Append);
      this.MatrixTranslate(matrix, pointF.X * 2f, 0.0f, MatrixOrder.Append);
    }
    return matrix;
  }

  private void MatrixTranslate(Matrix matrix, float x, float y, MatrixOrder matrixOrder)
  {
    matrix.Translate(x, y, matrixOrder);
  }

  private void MatrixMultiply(Matrix matrix, Matrix target, MatrixOrder matrixOrder)
  {
    matrix.Multiply(target, MatrixOrder.Append);
  }

  private Matrix GetTransformMatrix(RectangleF bounds, float ang, bool flipV, bool flipH)
  {
    Matrix transformMatrix = this.GetTransformMatrix(bounds, flipV, flipH);
    PointF point = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    transformMatrix.RotateAt(ang, point, MatrixOrder.Append);
    return transformMatrix;
  }

  internal RectangleF UpdateShapeBounds(RectangleF rect, int rotation)
  {
    if (rotation > 44 && rotation < 135 || rotation > 224 /*0xE0*/ && rotation < 315)
    {
      PointF[] pts = new PointF[3]
      {
        new PointF(rect.X, rect.Y),
        new PointF(rect.Right, rect.Y),
        new PointF(rect.X, rect.Bottom)
      };
      this.GetTransformMatrix(rect, -90f, false, false).TransformPoints(pts);
      rect = new RectangleF(pts[1].X, pts[1].Y, rect.Height, rect.Width);
    }
    return rect;
  }

  internal void SetPostion(long offsetX, long offsetY, long offsetCX, long offsetCY)
  {
    this.LeftDouble = offsetX >= 0L ? ApplicationImpl.ConvertToPixels((double) offsetX, MeasureUnits.EMU) : 0.0;
    this.TopDouble = offsetY >= 0L ? ApplicationImpl.ConvertToPixels((double) offsetY, MeasureUnits.EMU) : 0.0;
    this.WidthDouble = offsetCX >= 0L ? ApplicationImpl.ConvertToPixels((double) offsetCX, MeasureUnits.EMU) : 0.0;
    this.HeightDouble = offsetCY >= 0L ? ApplicationImpl.ConvertToPixels((double) offsetCY, MeasureUnits.EMU) : 0.0;
  }

  private void SetLeftPosition(double value)
  {
    if ((double) this.m_rectAbsolute.X == value)
      return;
    this.m_rectAbsolute.X = (float) value;
    this.m_shapeFrame.OffsetX = (long) ApplicationImpl.ConvertFromPixel(value, MeasureUnits.EMU);
    this.UpdateLeftColumn();
    this.UpdateRightColumn();
  }

  private void SetTopPosition(double value)
  {
    if ((double) this.m_rectAbsolute.Y == value)
      return;
    this.m_rectAbsolute.Y = (float) value;
    this.m_shapeFrame.OffsetY = (long) ApplicationImpl.ConvertFromPixel(value, MeasureUnits.EMU);
    this.UpdateTopRow();
    this.UpdateBottomRow();
  }

  private void SetWidth(double value)
  {
    if (value < 0.0)
      throw new ArgumentOutOfRangeException("Width");
    if ((double) this.m_rectAbsolute.Width == value)
      return;
    this.m_rectAbsolute.Width = (float) value;
    this.m_shapeFrame.OffsetCX = (long) ApplicationImpl.ConvertFromPixel(value, MeasureUnits.EMU);
    this.UpdateRightColumn();
  }

  private void SetHeight(double value)
  {
    if (value < 0.0)
      throw new ArgumentOutOfRangeException("Height");
    if ((double) this.m_rectAbsolute.Height == value)
      return;
    this.m_rectAbsolute.Height = (float) value;
    this.m_shapeFrame.OffsetCY = (long) ApplicationImpl.ConvertFromPixel(value, MeasureUnits.EMU);
    this.UpdateBottomRow();
  }

  internal void SetInnerShapes(object value, string property)
  {
    foreach (IShape shape in (this as GroupShapeImpl).Items)
    {
      ShapeImpl shapeImpl = shape as ShapeImpl;
      switch (property)
      {
        case "IsShapeVisible":
          shapeImpl.IsShapeVisible = (bool) value;
          break;
        case "IsMoveWithCell":
          shapeImpl.IsMoveWithCell = (bool) value;
          break;
        case "IsSizeWithCell":
          shapeImpl.IsSizeWithCell = (bool) value;
          break;
      }
    }
  }
}
