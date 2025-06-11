// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.ShapeImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class ShapeImpl : 
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
  protected bool m_bSupportOptions;
  private string m_strName = string.Empty;
  private string m_strAlternativeText = string.Empty;
  private MsoBase m_record;
  private WorkbookImpl m_book;
  private OfficeShapeType m_shapeType;
  [CLSCompliant(false)]
  protected MsofbtSp m_shape;
  private MsofbtClientAnchor m_clientAnchor;
  protected ShapeCollectionBase m_shapes;
  private OBJRecord m_object;
  [CLSCompliant(false)]
  protected MsofbtOPT m_options;
  private Rectangle m_rectAbsolute = new Rectangle();
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
  private Ptg[] m_macroTokens;
  private bool m_shapeVisibility = true;
  private ShadowImpl m_shadow;
  private ThreeDFormatImpl m_3D;
  private bool m_enableAlternateContent;
  private List<ShapeImpl> m_childShapes;
  private MsofbtChildAnchor m_childAnchor;
  private Dictionary<string, string> m_styleProperties;
  private string m_preserveStyleString;
  private bool m_isHyperlink;
  private int m_shapeRotation;
  private bool m_bHasBorder;
  internal List<Stream> preservedShapeStreams;
  internal List<Stream> preservedCnxnShapeStreams;
  internal List<Stream> preservedInnerCnxnShapeStreams;
  internal List<Stream> preservedPictureStreams;
  internal Stream m_graphicFrame;
  private bool m_bIsAbsoluteAnchor;
  private bool m_bAutoSize;
  internal bool IsEquationShape;

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
    this.SetParents();
    this.AttachEvents();
    this.m_bHasBorder = true;
    if (this.m_shapes.Worksheet == null)
      this.m_bUpdatePositions = false;
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
    : this(application, parent, container, OfficeParseOptions.Default)
  {
  }

  [CLSCompliant(false)]
  public ShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer container,
    OfficeParseOptions options)
    : this(application, parent)
  {
    this.m_record = (MsoBase) container;
    this.ParseRecord(options);
    this.m_bSupportOptions = true;
  }

  [CLSCompliant(false)]
  public ShapeImpl(IApplication application, object parent, MsoBase shapeRecord)
    : this(application, parent, shapeRecord, OfficeParseOptions.Default)
  {
  }

  [CLSCompliant(false)]
  public ShapeImpl(
    IApplication application,
    object parent,
    MsoBase shapeRecord,
    OfficeParseOptions options)
    : this(application, parent)
  {
    this.m_record = shapeRecord;
  }

  protected virtual void CreateDefaultFillLineFormats()
  {
  }

  private void ParseRecord() => this.ParseRecord(OfficeParseOptions.Default);

  private void ParseRecord(OfficeParseOptions options)
  {
    this.m_shapeType = OfficeShapeType.Unknown;
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
          this.ParseOtherRecords(msoBase, options);
          break;
      }
    }
    if (this.Id == 0)
      return;
    this.m_iShapeId = this.Id;
  }

  [CLSCompliant(false)]
  protected virtual void ParseClientData(MsofbtClientData clientData, OfficeParseOptions options)
  {
    this.m_object = clientData.ObjectRecord;
    List<ObjSubRecord> recordsList = this.m_object.RecordsList;
    this.m_book.CurrentObjectId = Math.Max(this.m_book.CurrentObjectId, (int) (recordsList[0] as ftCmo).ID);
    int index = 1;
    for (int count = recordsList.Count; index < count; ++index)
    {
      ObjSubRecord objSubRecord = recordsList[index];
      if (objSubRecord.Type == TObjSubRecordType.ftMacro)
      {
        this.m_macroTokens = ((ftMacro) objSubRecord).Tokens;
        break;
      }
    }
  }

  [CLSCompliant(false)]
  protected virtual void ParseOtherRecords(MsoBase subRecord, OfficeParseOptions options)
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
    if (this.m_clientAnchor.IsShortVersion)
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

  protected void AttachEvents()
  {
    if (!(this.m_shapes.WorksheetBase is WorksheetImpl worksheetBase))
      return;
    (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent += new EventHandler(this.NormalFont_OnAfterChange);
    worksheetBase.ColumnWidthChanged += new ValueChangedEventHandler(this.Worksheet_ColumnWidthChanged);
    worksheetBase.RowHeightChanged += new ValueChangedEventHandler(this.Worksheet_RowHeightChanged);
  }

  protected void DetachEvents()
  {
    if (!(this.m_shapes.WorksheetBase is WorksheetImpl worksheetBase))
      return;
    if (this.m_book.Styles != null && this.m_book.Styles.Contains("Normal"))
      (this.m_book.Styles["Normal"].Font as FontWrapper).AfterChangeEvent -= new EventHandler(this.NormalFont_OnAfterChange);
    worksheetBase.ColumnWidthChanged -= new ValueChangedEventHandler(this.Worksheet_ColumnWidthChanged);
    worksheetBase.RowHeightChanged -= new ValueChangedEventHandler(this.Worksheet_RowHeightChanged);
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

  [CLSCompliant(false)]
  protected Color GetColorValue(MsofbtOPT.FOPTE option)
  {
    byte[] bytes = BitConverter.GetBytes(option.UInt32Value);
    return bytes[3] == (byte) 8 ? this.m_book.GetPaletteColor((OfficeKnownColors) bytes[0]) : Color.FromArgb(0, (int) bytes[0], (int) bytes[1], (int) bytes[2]);
  }

  private byte GetByte(MsofbtOPT.FOPTE option, int iByteIndex)
  {
    return BitConverter.GetBytes(option.UInt32Value)[iByteIndex];
  }

  [CLSCompliant(false)]
  protected string ParseName(MsofbtOPT.FOPTE option)
  {
    byte[] bytes = option != null ? option.AdditionalData : throw new ArgumentNullException(nameof (option));
    string name;
    if (bytes == null)
    {
      name = (string) null;
    }
    else
    {
      string str = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
      name = str.Substring(0, str.Length - 1);
    }
    return name;
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
    get => this.m_rectAbsolute.Height;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (Height));
      if (this.m_rectAbsolute.Height == value)
        return;
      this.m_rectAbsolute.Height = value;
      this.UpdateBottomRow();
    }
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
    get => this.m_rectAbsolute.X;
    set
    {
      if (this.m_rectAbsolute.X == value)
        return;
      this.m_rectAbsolute.X = value;
      this.UpdateLeftColumn();
      this.UpdateRightColumn();
    }
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
    get => this.m_rectAbsolute.Y;
    set
    {
      if (this.m_rectAbsolute.Y == value)
        return;
      this.m_rectAbsolute.Y = value;
      this.UpdateTopRow();
      this.UpdateBottomRow();
    }
  }

  public virtual int Width
  {
    get => this.m_rectAbsolute.Width;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (Width));
      if (this.m_rectAbsolute.Width == value)
        return;
      this.m_rectAbsolute.Width = value;
      this.UpdateRightColumn();
    }
  }

  public OfficeShapeType ShapeType
  {
    get => this.m_shapeType;
    set => this.m_shapeType = value;
  }

  public bool IsShapeVisible
  {
    get => this.m_shapeVisibility;
    set => this.m_shapeVisibility = value;
  }

  public virtual string AlternativeText
  {
    get => this.m_strAlternativeText;
    set => this.m_strAlternativeText = value;
  }

  public virtual bool IsMoveWithCell
  {
    get => this.ClientAnchor.IsMoveWithCell;
    set => this.ClientAnchor.IsMoveWithCell = value;
  }

  public virtual bool IsSizeWithCell
  {
    get => this.ClientAnchor.IsSizeWithCell;
    set => this.ClientAnchor.IsSizeWithCell = value;
  }

  public virtual IOfficeFill Fill
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
        if (this.ShapeType == OfficeShapeType.CheckBox)
          this.m_fill.Visible = false;
      }
      return (IOfficeFill) this.m_fill;
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
    get
    {
      return this.m_macroTokens == null ? (string) null : this.m_book.FormulaUtil.ParsePtgArray(this.m_macroTokens);
    }
    set
    {
      this.m_macroTokens = value != null ? this.m_book.FormulaUtil.ParseString(value) : (Ptg[]) null;
    }
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
    }
  }

  public virtual ITextFrame TextFrame
  {
    get => throw new NotImplementedException("This property doesn't support in this class");
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
    if (this.m_shapeType == OfficeShapeType.Unknown)
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
  protected void SerializeShapeName(MsofbtOPT options)
  {
    this.SerializeName(options, MsoOptions.ShapeName, this.m_strName);
  }

  [CLSCompliant(false)]
  protected void SerializeName(MsofbtOPT options, MsoOptions optionId, string name)
  {
    switch (name)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (options == null)
          throw new ArgumentNullException(nameof (options));
        int length = name.Length;
        string s = name;
        if (name[length - 1] != char.MinValue)
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
        break;
    }
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
    shapeImpl.CloneLineFill(this);
    if (shapeImpl.ShapeType == OfficeShapeType.AutoShape)
    {
      AutoShapeImpl autoShapeImpl = shapeImpl as AutoShapeImpl;
      autoShapeImpl.ShapeExt = (this as AutoShapeImpl).ShapeExt.Clone(shapeImpl);
      autoShapeImpl.ShapeExt.Worksheet = shapeImpl.m_shapes.Worksheet;
      autoShapeImpl.ShapeExt.ClientAnchor.Worksheet = shapeImpl.m_shapes.Worksheet;
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
    if (flag2 && this.m_book.Version != OfficeVersion.Excel97to2003 && this.m_xmlDataStream != null && this.m_xmlDataStream.Length > 0L)
      flag2 = false;
    return flag2;
  }

  public virtual ShapeImpl CopyMoveShapeOnRangeCopyMove(
    WorksheetImpl sheet,
    Rectangle destRec,
    bool bIsCopy)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    ShapeImpl newShape = this;
    ShapesCollection shapes = (ShapesCollection) sheet.Shapes;
    if (bIsCopy)
      newShape = (ShapeImpl) newShape.Clone((object) shapes, (Dictionary<string, string>) null, (Dictionary<int, int>) null, true);
    else if (sheet != this.Worksheet)
    {
      ((ShapeCollectionBase) this.Worksheet.Shapes).Remove((IShape) this);
      shapes.AddShape(newShape);
    }
    int height = this.Height;
    int width = this.Width;
    newShape.ClientAnchor.TopRow = destRec.Top - 1;
    newShape.ClientAnchor.LeftColumn = destRec.Left - 1;
    if (this.IsSizeWithCell)
    {
      newShape.ClientAnchor.BottomRow = destRec.Bottom - 1;
      newShape.ClientAnchor.RightColumn = destRec.Right - 1;
      newShape.UpdateWidth();
      newShape.UpdateHeight();
    }
    else
    {
      newShape.Height = height;
      newShape.Width = width;
      newShape.UpdateBottomRow();
      newShape.UpdateRightColumn();
    }
    return newShape;
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
      this.ClientAnchor.LeftColumn = this.m_rectAbsolute.Left;
      this.ClientAnchor.LeftOffset = 0;
    }
    else
    {
      int num = 0;
      int iColumnIndex = 0;
      for (; num <= this.m_rectAbsolute.Left; num += worksheet.GetColumnWidthInPixels(iColumnIndex))
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
      int iPixels = this.m_rectAbsolute.Left - (num - columnWidthInPixels);
      this.ClientAnchor.LeftColumn = iColumnIndex - 1;
      this.ClientAnchor.LeftOffset = this.ConvertPixelsIntoWidthOffset(iPixels, columnWidthInPixels);
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
      this.m_rectAbsolute.Location = new Point(this.ClientAnchor.LeftColumn, this.m_rectAbsolute.Top);
      this.ClientAnchor.RightColumn = this.m_rectAbsolute.Left + this.m_rectAbsolute.Width;
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
      this.ClientAnchor.TopRow = this.m_rectAbsolute.Y;
      this.ClientAnchor.TopOffset = 0;
    }
    else
    {
      int num = 0;
      int iRowIndex = 0;
      for (; num <= this.m_rectAbsolute.Top; num += worksheet.GetRowHeightInPixels(iRowIndex))
        ++iRowIndex;
      int rowHeightInPixels = worksheet.GetRowHeightInPixels(iRowIndex);
      int iPixels = this.m_rectAbsolute.Top - (num - rowHeightInPixels);
      this.ClientAnchor.TopRow = iRowIndex - 1;
      this.ClientAnchor.TopOffset = this.ConvertPixelsIntoHeightOffset(iPixels, rowHeightInPixels);
    }
  }

  protected internal void UpdateBottomRow()
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
    {
      this.m_rectAbsolute.Location = new Point(this.m_rectAbsolute.Left, this.ClientAnchor.TopRow);
      this.ClientAnchor.BottomRow = this.m_rectAbsolute.Top + this.m_rectAbsolute.Height;
      this.ClientAnchor.BottomOffset = 0;
    }
    else
    {
      int height = this.Height;
      int topRow = this.TopRow;
      int iOffset = this.TopRowOffset;
      while (height >= 0)
      {
        if (topRow > this.m_book.MaxRowCount)
        {
          this.BottomRow = this.m_book.MaxRowCount;
          this.BottomRowOffset = 256 /*0x0100*/;
          break;
        }
        int num = worksheet.GetRowHeightInPixels(topRow) - this.OffsetInPixels(topRow, iOffset, false);
        if (num < 0)
          throw new ArgumentOutOfRangeException("Calculated value can't be less than zero, error in coordinates update.");
        if (num > height)
        {
          this.BottomRow = topRow;
          this.BottomRowOffset = iOffset + this.PixelsInOffset(topRow, height, false);
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
    this.UpdateTopRow();
    this.UpdateBottomRow();
    this.UpdateLeftColumn();
    this.UpdateRightColumn();
  }

  protected internal void UpdateWidth()
  {
    this.m_rectAbsolute.Width = this.GetWidth(this.LeftColumn, this.LeftColumnOffset, this.RightColumn, this.RightColumnOffset, false);
  }

  protected internal void UpdateHeight()
  {
    this.m_rectAbsolute.Height = this.GetHeight(this.TopRow, this.TopRowOffset, this.BottomRow, this.BottomRowOffset, false);
  }

  internal int OffsetInPixels(int iRowColumn, int iOffset, bool isXOffset)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return 0;
    double a;
    if (isXOffset)
    {
      int columnWidthInPixels = worksheet.GetColumnWidthInPixels(iRowColumn);
      a = (double) (iOffset * columnWidthInPixels) / 1024.0;
    }
    else
    {
      double rowHeightInPixels = (double) worksheet.GetRowHeightInPixels(iRowColumn);
      a = (double) iOffset * rowHeightInPixels / 256.0;
    }
    return (int) Math.Round(a);
  }

  internal int PixelsInOffset(int iCurRowColumn, int iPixels, bool isXSize)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return 0;
    if (iPixels < 0)
      throw new ArgumentOutOfRangeException("IPixels", "Can't be less than zero.");
    if (isXSize)
    {
      int columnWidthInPixels = worksheet.GetColumnWidthInPixels(iCurRowColumn);
      return columnWidthInPixels != 0 ? iPixels * 1024 /*0x0400*/ / columnWidthInPixels : columnWidthInPixels;
    }
    int rowHeightInPixels = worksheet.GetRowHeightInPixels(iCurRowColumn);
    return rowHeightInPixels == 0 ? 256 /*0x0100*/ : iPixels * 256 /*0x0100*/ / rowHeightInPixels;
  }

  private int GetWidth(
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
    int num = this.ConvertWidthOffsetIntoPixels(columnWidthInPixels1, Math.Min(iOffset1, 1024 /*0x0400*/), bIsOffsetInPixels);
    int width = this.ConvertWidthOffsetIntoPixels(columnWidthInPixels2, Math.Min(iOffset2, 1024 /*0x0400*/), bIsOffsetInPixels) - num;
    for (int iColumnIndex = iColumn1; iColumnIndex < iColumn2; ++iColumnIndex)
      width += worksheet.GetColumnWidthInPixels(iColumnIndex);
    return width;
  }

  private int GetHeight(int iRow1, int iOffset1, int iRow2, int iOffset2, bool bIsOffsetInPixels)
  {
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return iRow2 - iRow1;
    if (iRow1 < 1 || iRow1 > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRow1));
    if (iRow2 < 1 || iRow2 > this.m_book.MaxRowCount)
      throw new ArgumentOutOfRangeException(nameof (iRow2));
    if (iRow2 < iRow1)
      return 0;
    if (iOffset1 < 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset1));
    if (iOffset2 < 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset2));
    if (iRow1 == iRow2 && iOffset1 > iOffset2)
      return 0;
    int rowHeightInPixels1 = worksheet.GetRowHeightInPixels(iRow1);
    int rowHeightInPixels2 = worksheet.GetRowHeightInPixels(iRow2);
    int num = ShapeImpl.ConvertHeightOffsetIntoPixels(rowHeightInPixels1, iOffset1, bIsOffsetInPixels);
    int height = ShapeImpl.ConvertHeightOffsetIntoPixels(rowHeightInPixels2, iOffset2, bIsOffsetInPixels) - num;
    if (this.m_book.IsWorkbookOpening)
    {
      height += worksheet.RowHeightHelper.GetTotal(iRow2 - 1) - worksheet.RowHeightHelper.GetTotal(iRow1 - 1);
    }
    else
    {
      for (int iRowIndex = iRow1; iRowIndex < iRow2; ++iRowIndex)
        height += worksheet.GetRowHeightInPixels(iRowIndex);
    }
    return height;
  }

  private int ConvertWidthOffsetIntoPixels(int iColWidth, int iOffset, bool bIsInPixels)
  {
    int num = iOffset;
    if (!bIsInPixels)
      num = (int) Math.Round((double) (iOffset * iColWidth) / 1024.0);
    return num;
  }

  private static int ConvertHeightOffsetIntoPixels(
    int iRowHeight,
    int iOffset,
    bool bIsOffsetInPixels)
  {
    int num = iOffset;
    if (!bIsOffsetInPixels)
      num = (int) Math.Round((double) (iOffset * iRowHeight) / 256.0);
    return num;
  }

  private int ConvertPixelsIntoWidthOffset(int iPixels, int iColWidth)
  {
    return iPixels * 1024 /*0x0400*/ / iColWidth;
  }

  private int ConvertPixelsIntoHeightOffset(int iPixels, int iRowHeight)
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
    this.m_rectAbsolute.X = this.GetWidth(1, 0, this.LeftColumn, this.LeftColumnOffset, false);
  }

  private void EvaluateRightPosition()
  {
    this.m_rectAbsolute.Y = this.GetHeight(1, 0, this.TopRow, this.TopRowOffset, false);
  }

  private void EvaluateTopPosition()
  {
    this.m_rectAbsolute.Y = this.GetHeight(1, 0, this.TopRow, this.TopRowOffset, false);
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

  private void Worksheet_ColumnWidthChanged(object sender, ValueChangedEventArgs e)
  {
    bool flag = this.m_clientAnchor == null || this.m_clientAnchor.LeftOffset == this.m_clientAnchor.RightOffset && this.m_clientAnchor.RightOffset == this.m_clientAnchor.TopOffset && this.m_clientAnchor.TopOffset == this.m_clientAnchor.BottomOffset && 0 == this.m_clientAnchor.TopOffset;
    if (this.m_book.IsWorkbookOpening || flag)
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
    if (this.m_book.IsWorkbookOpening)
      return;
    WorksheetImpl worksheet = this.m_shapes.Worksheet;
    if (worksheet == null)
      return;
    int width = this.GetWidth(this.LeftColumn, this.LeftColumnOffset, this.RightColumn, this.RightColumnOffset, false);
    int columnWidthInPixels = worksheet.GetColumnWidthInPixels(this.RightColumn);
    this.RightColumnOffset += this.ConvertPixelsIntoWidthOffset(Math.Min(this.Width - width, columnWidthInPixels), columnWidthInPixels);
    this.UpdateWidth();
  }

  private void NormalFont_OnAfterChange(object sender, EventArgs e)
  {
    if (this.m_book.IsWorkbookOpening)
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
    if (this.m_book.IsWorkbookOpening)
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
}
