// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.ShapeLineFormatImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class ShapeLineFormatImpl : CommonObject, IShapeLineFormat
{
  private const double DEF_LINE_WEIGHT_MULL = 12700.0;
  private const int DEF_LINE_MAX_WEIGHT = 1584;
  private const int DEF_PARSE_ARR_LENGTH = 5088;
  private static byte[] m_parsePattArray = new byte[5088];
  private double m_weight = 0.75;
  private Color m_foreColor = ColorExtension.Black;
  private Color m_backColor = ColorExtension.White;
  private WorkbookImpl m_book;
  private OfficeShapeArrowStyle m_beginArrowStyle;
  private OfficeShapeArrowStyle m_endArrowStyle;
  private OfficeShapeArrowLength m_beginArrowLength = OfficeShapeArrowLength.ArrowHeadMedium;
  private OfficeShapeArrowLength m_endArrowLength = OfficeShapeArrowLength.ArrowHeadMedium;
  private OfficeShapeArrowWidth m_beginArrowWidth = OfficeShapeArrowWidth.ArrowHeadMedium;
  private OfficeShapeArrowWidth m_endArrowWidth = OfficeShapeArrowWidth.ArrowHeadMedium;
  private OfficeShapeDashLineStyle m_dashStyle;
  private OfficeShapeLineStyle m_style = OfficeShapeLineStyle.Line_Single;
  private double m_transparency;
  private bool m_visible = true;
  private OfficeGradientPattern m_pattern = OfficeGradientPattern.Pat_5_Percent;
  private bool m_bContainPattern;
  private bool m_bRound;
  private PreservationLogger m_logger;

  static ShapeLineFormatImpl()
  {
    int index1 = 0;
    for (int index2 = 1; index2 < 49; ++index2)
    {
      byte[] resData = ShapeFillImpl.GetResData("Patt" + index2.ToString());
      ShapeLineFormatImpl.m_parsePattArray[index1] = (byte) resData.Length;
      int index3 = index1 + 1;
      resData.CopyTo((Array) ShapeLineFormatImpl.m_parsePattArray, index3);
      index1 = index3 + resData.Length;
    }
  }

  public ShapeLineFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.m_logger = new PreservationLogger();
  }

  internal ShapeLineFormatImpl(IApplication application, object parent, PreservationLogger logger)
    : base(application, parent)
  {
    this.FindParents();
    this.m_logger = logger;
  }

  private void FindParents()
  {
    this.m_book = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    if (this.m_book == null)
      throw new ApplicationException("Cann't find parent object.");
  }

  internal static double ParseTransparency(uint value) => (65500.0 - (double) value) / 65500.0;

  internal static void SerializeTransparency(IFopteOptionWrapper opt, MsoOptions id, double value)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    int num = (int) ((100.0 - value * 100.0) * 655.0);
    ShapeImpl.SerializeForte(opt, id, num);
  }

  internal static void SerializeColor(
    IFopteOptionWrapper opt,
    ChartColor color,
    WorkbookImpl book,
    MsoOptions id)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    bool flag = true;
    byte[] arr;
    if (color.ColorType == ColorType.Indexed)
    {
      arr = new byte[4]
      {
        (byte) color.Value,
        (byte) 0,
        (byte) 0,
        (byte) 8
      };
    }
    else
    {
      Color rgb = color.GetRGB((IWorkbook) book);
      arr = new byte[4]{ rgb.R, rgb.G, rgb.B, (byte) 2 };
      if (rgb.A == (byte) 0 && rgb.R == (byte) 0 && rgb.G == (byte) 0 && rgb.B == (byte) 0)
        flag = false;
    }
    if (!flag)
      return;
    ShapeImpl.SerializeForte(opt, id, arr);
  }

  public double Weight
  {
    get => this.m_weight;
    set
    {
      this.m_weight = value >= 0.0 && value <= 1584.0 ? value : throw new ArgumentOutOfRangeException(nameof (Weight));
      this.Visible = true;
    }
  }

  public Color ForeColor
  {
    get => this.m_foreColor;
    set
    {
      this.m_foreColor = value;
      this.Visible = true;
    }
  }

  public Color BackColor
  {
    get => this.m_backColor;
    set
    {
      this.m_backColor = value;
      this.Visible = true;
    }
  }

  public OfficeKnownColors ForeColorIndex
  {
    get => this.m_book.GetNearestColor(this.m_foreColor);
    set => this.ForeColor = this.m_book.GetPaletteColor(value);
  }

  public OfficeKnownColors BackColorIndex
  {
    get => this.m_book.GetNearestColor(this.m_backColor);
    set => this.BackColor = this.m_book.GetPaletteColor(value);
  }

  public OfficeShapeArrowStyle BeginArrowHeadStyle
  {
    get => this.m_beginArrowStyle;
    set
    {
      this.m_beginArrowStyle = value;
      this.Visible = true;
    }
  }

  public OfficeShapeArrowStyle EndArrowHeadStyle
  {
    get => this.m_endArrowStyle;
    set
    {
      this.m_endArrowStyle = value;
      this.Visible = true;
    }
  }

  public OfficeShapeArrowLength BeginArrowheadLength
  {
    get => this.m_beginArrowLength;
    set
    {
      this.m_beginArrowLength = value;
      this.Visible = true;
    }
  }

  public OfficeShapeArrowLength EndArrowheadLength
  {
    get => this.m_endArrowLength;
    set
    {
      this.m_endArrowLength = value;
      this.Visible = true;
    }
  }

  public OfficeShapeArrowWidth BeginArrowheadWidth
  {
    get => this.m_beginArrowWidth;
    set
    {
      this.m_beginArrowWidth = value;
      this.Visible = true;
    }
  }

  public OfficeShapeArrowWidth EndArrowheadWidth
  {
    get => this.m_endArrowWidth;
    set
    {
      this.m_endArrowWidth = value;
      this.Visible = true;
    }
  }

  public OfficeShapeDashLineStyle DashStyle
  {
    get => this.m_dashStyle;
    set
    {
      this.m_dashStyle = value;
      this.Visible = true;
    }
  }

  public OfficeShapeLineStyle Style
  {
    get => this.m_style;
    set
    {
      this.m_style = value;
      this.Visible = true;
    }
  }

  public double Transparency
  {
    get => this.m_transparency;
    set
    {
      this.m_transparency = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (value));
      this.Visible = true;
    }
  }

  public bool Visible
  {
    get => this.m_visible;
    set
    {
      this.m_visible = value;
      this.m_logger.SetFlag(PreservedFlag.Line);
    }
  }

  public OfficeGradientPattern Pattern
  {
    get
    {
      if (!this.m_bContainPattern)
        throw new NotSupportedException("Doesn't checked patterned style.");
      return this.m_pattern;
    }
    set
    {
      this.m_pattern = value;
      this.HasPattern = true;
      this.Visible = true;
    }
  }

  public bool HasPattern
  {
    get => this.m_bContainPattern;
    set
    {
      if (this.HasPattern == value)
        return;
      this.m_bContainPattern = value;
      this.Visible = true;
    }
  }

  public WorkbookImpl Workbook => this.m_book;

  public bool IsRound
  {
    get => this.m_bRound;
    set => this.m_bRound = value;
  }

  [CLSCompliant(false)]
  public bool ParseOption(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    if (this.ParseArrowsPropertys(option))
      return true;
    switch (option.Id)
    {
      case MsoOptions.LineColor:
        this.m_foreColor = ShapeFillImpl.ParseColor(this.m_book, option.MainData);
        return true;
      case MsoOptions.LineTransparency:
        this.m_transparency = ShapeLineFormatImpl.ParseTransparency(option.UInt32Value);
        return true;
      case MsoOptions.LineBackColor:
        this.m_backColor = ShapeFillImpl.ParseColor(this.m_book, option.MainData);
        return true;
      case MsoOptions.ContainLinePattern:
        this.m_bContainPattern = true;
        return true;
      case MsoOptions.LinePattern:
        this.m_pattern = this.ParsePattern(option);
        return true;
      case MsoOptions.LineWeight:
        this.m_weight = (double) option.UInt32Value / 12700.0;
        return true;
      case MsoOptions.LineStyle:
        this.m_style = (OfficeShapeLineStyle) ((int) option.UInt32Value + 1);
        return true;
      case MsoOptions.LineDashStyle:
        this.m_dashStyle = (OfficeShapeDashLineStyle) option.UInt32Value;
        return true;
      case MsoOptions.ContainRoundDot:
        this.m_dashStyle = OfficeShapeDashLineStyle.Dotted_Round;
        return true;
      case MsoOptions.NoLineDrawDash:
        this.ParseVisible(option.MainData);
        return true;
      default:
        return false;
    }
  }

  private bool ParseArrowsPropertys(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    switch (option.Id)
    {
      case MsoOptions.LineStartArrow:
        this.m_beginArrowStyle = (OfficeShapeArrowStyle) option.UInt32Value;
        return true;
      case MsoOptions.LineEndArrow:
        this.m_endArrowStyle = (OfficeShapeArrowStyle) option.UInt32Value;
        return true;
      case MsoOptions.StartArrowWidth:
        this.m_beginArrowWidth = (OfficeShapeArrowWidth) option.UInt32Value;
        return true;
      case MsoOptions.StartArrowLength:
        this.m_beginArrowLength = (OfficeShapeArrowLength) option.UInt32Value;
        return true;
      case MsoOptions.EndArrowWidth:
        this.m_endArrowWidth = (OfficeShapeArrowWidth) option.UInt32Value;
        return true;
      case MsoOptions.EndArrowLength:
        this.m_endArrowLength = (OfficeShapeArrowLength) option.UInt32Value;
        return true;
      default:
        return false;
    }
  }

  private OfficeGradientPattern ParsePattern(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    byte[] numArray;
    if (option.AdditionalData == null || option.AdditionalData.Length == 0)
    {
      if (option.UInt32Value <= 0U)
        return OfficeGradientPattern.Pat_5_Percent;
      MsofbtBSE picture = this.m_book.ShapesData.GetPicture((int) option.UInt32Value);
      MemoryStream memoryStream = new MemoryStream(picture.Length);
      picture.InfillInternalData((Stream) memoryStream, 0, (List<int>) null, (List<List<BiffRecordRaw>>) null);
      numArray = new byte[picture.Length - 36];
      memoryStream.Position = 36L;
      memoryStream.Read(numArray, 0, numArray.Length);
    }
    else
      numArray = option.AdditionalData;
    return this.GetPattern(numArray);
  }

  private void ParseVisible(byte[] data)
  {
    byte num1 = data != null ? data[4] : throw new ArgumentNullException(nameof (data));
    byte num2 = data[2];
    if ((num2 < (byte) 8 || num2 > (byte) 16 /*0x10*/) && data[3] == (byte) 0 && data[5] == (byte) 0 && (num1 >= (byte) 8 && num1 <= (byte) 16 /*0x10*/ || num1 == (byte) 24) || num1 == (byte) 24 && num2 == (byte) 16 /*0x10*/)
      this.m_visible = false;
    else
      this.m_visible = true;
  }

  [CLSCompliant(false)]
  public void Serialize(MsofbtOPT opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    if (this.m_visible)
      ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.LineWeight, (int) (this.m_weight * 12700.0));
    this.SerializeVisible(opt);
    if (!this.m_visible)
      return;
    ShapeLineFormatImpl.SerializeColor((IFopteOptionWrapper) opt, (ChartColor) this.m_foreColor, this.m_book, MsoOptions.LineColor);
    ShapeLineFormatImpl.SerializeColor((IFopteOptionWrapper) opt, (ChartColor) this.m_backColor, this.m_book, MsoOptions.LineBackColor);
    this.SerializeArrowProperties(opt);
    this.SerializeDashStyle(opt);
    this.SerializeLineStyle(opt);
    ShapeLineFormatImpl.SerializeTransparency((IFopteOptionWrapper) opt, MsoOptions.LineTransparency, this.m_transparency);
    this.SerializePattern(opt);
  }

  private void SerializeArrowProperties(MsofbtOPT opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.LineStartArrow, (int) this.m_beginArrowStyle);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.LineEndArrow, (int) this.m_endArrowStyle);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.StartArrowLength, (int) this.m_beginArrowLength);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.EndArrowLength, (int) this.m_endArrowLength);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.StartArrowWidth, (int) this.m_beginArrowWidth);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.EndArrowWidth, (int) this.m_endArrowWidth);
  }

  private void SerializeDashStyle(MsofbtOPT opt)
  {
    OfficeShapeDashLineStyle shapeDashLineStyle = this.m_dashStyle;
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    if (shapeDashLineStyle == OfficeShapeDashLineStyle.Dotted_Round)
    {
      shapeDashLineStyle = OfficeShapeDashLineStyle.Dotted;
      ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.ContainRoundDot, 0);
    }
    else
      opt.RemoveOption(471);
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.LineDashStyle, (int) shapeDashLineStyle);
  }

  private void SerializeLineStyle(MsofbtOPT opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    int style = (int) this.m_style;
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.LineStyle, style - 1);
  }

  private void SerializeVisible(MsofbtOPT opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    byte[] arr = new byte[4]
    {
      (byte) 8,
      (byte) 0,
      (byte) 8,
      (byte) 0
    };
    if (!this.m_visible)
    {
      arr[0] = (byte) 0;
      arr[2] = (byte) 24;
    }
    ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.NoLineDrawDash, arr);
  }

  private void SerializePattern(MsofbtOPT opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    byte[] resData = ShapeFillImpl.GetResData("Patt" + ((int) this.m_pattern).ToString());
    if (this.m_bContainPattern)
    {
      ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.LinePattern, 0, resData, true);
      ShapeImpl.SerializeForte((IFopteOptionWrapper) opt, MsoOptions.ContainLinePattern, 1);
    }
    else
    {
      opt.RemoveOption(453);
      opt.RemoveOption(452);
    }
  }

  private OfficeGradientPattern GetPattern(byte[] arr)
  {
    if (arr == null)
      throw new ArgumentNullException(nameof (arr));
    int pattern = 1;
    int num1 = 0;
    int length = arr.Length;
    while (num1 < ShapeLineFormatImpl.m_parsePattArray.Length)
    {
      int index = 0;
      int num2 = num1 + 1;
      if ((int) ShapeLineFormatImpl.m_parsePattArray[num2 - 1] == length)
      {
        while (index < length && (int) arr[index] == (int) ShapeLineFormatImpl.m_parsePattArray[num2 + index])
          ++index;
        if (index == length)
          return (OfficeGradientPattern) pattern;
      }
      num1 = num2 + (int) ShapeLineFormatImpl.m_parsePattArray[num2 - 1];
      ++pattern;
    }
    return OfficeGradientPattern.Pat_5_Percent;
  }

  public ShapeLineFormatImpl Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ShapeLineFormatImpl shapeLineFormatImpl = (ShapeLineFormatImpl) this.MemberwiseClone();
    shapeLineFormatImpl.SetParent(parent);
    shapeLineFormatImpl.FindParents();
    return shapeLineFormatImpl;
  }
}
