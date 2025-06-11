// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.ShapeFillImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class ShapeFillImpl : CommonObject, IInternalFill, IOfficeFill, IGradient
{
  private const int DEF_SHAD_STYLE_VERTICAL = 166;
  private const int DEF_SHAD_STYLE_VERTICAL2007 = 90;
  private const int DEF_SHAD_STYLE_DIAGONAL_UP = 121;
  private const int DEF_SHAD_STYLE_DIAGONAL_DOWN = 211;
  internal const int DEF_COLOR_CONSTANT = 8;
  private const int DEF_ONE_COLOR_STYLE_VALUE = 1073741835 /*0x4000000B*/;
  public const string DEF_PATTERN_PREFIX = "Patt";
  internal const string DEF_TEXTURE_PREFIX = "Text";
  private const string DEF_GRAD_PREFIX = "Grad";
  private const string DEF_PATTERN_ENUM_PREFIX = "Pat_";
  private const byte DEF_NOT_VISIBLE_VALUE = 16 /*0x10*/;
  public const int DEF_COMMENT_COLOR_INDEX = 80 /*0x50*/;
  private const int DEF_CORNER_STYLE = 5;
  private const int DEF_CENTER_STYLE = 6;
  private const int DEF_OFFSET = 25;
  internal const int MaxValue = 100000;
  internal const int HorizontalAngle = 5400000;
  internal const int VerticalAngle = 0;
  internal const int DiagonalUpAngle = 2700000;
  internal const int DiagonalDownAngle = 18900000;
  private static readonly byte[] DEF_VARIANT_FIRST_ARR = new byte[4]
  {
    (byte) 100,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private static readonly byte[] DEF_VARIANT_THIRD_ARR = new byte[4]
  {
    (byte) 206,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue
  };
  private static readonly byte[] DEF_VARIANT_FOURTH_ARR = new byte[4]
  {
    (byte) 50,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private static readonly byte[] DEF_VARIANT_CENTER_ADD_DATA = new byte[4]
  {
    (byte) 0,
    (byte) 128 /*0x80*/,
    (byte) 0,
    (byte) 0
  };
  private static readonly byte[] DEF_VARIANT_CORNER_ADD_DATA = new byte[4]
  {
    (byte) 0,
    (byte) 0,
    (byte) 1,
    (byte) 0
  };
  private static readonly byte[] DEF_BITMAP_INDEX = new byte[4]
  {
    (byte) 128 /*0x80*/,
    (byte) 122,
    (byte) 31 /*0x1F*/,
    (byte) 240 /*0xF0*/
  };
  public static readonly Color DEF_COMENT_PARSE_COLOR = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 222);
  internal static Rectangle RectangleFromCenter = Rectangle.FromLTRB(50000, 50000, 50000, 50000);
  internal static Rectangle[] RectanglesCorner = new Rectangle[4]
  {
    Rectangle.FromLTRB(0, 0, 100000, 100000),
    Rectangle.FromLTRB(100000, 0, 0, 100000),
    Rectangle.FromLTRB(0, 100000, 100000, 0),
    Rectangle.FromLTRB(100000, 100000, 0, 0)
  };
  private static Dictionary<string, byte[]> m_dicResources = new Dictionary<string, byte[]>();
  protected OfficeFillType m_fillType;
  private OfficeGradientStyle m_gradStyle;
  private OfficeGradientVariants m_gradVariant = OfficeGradientVariants.ShadingVariants_2;
  private double m_transparencyTo;
  private double m_transparencyFrom;
  private OfficeGradientColor m_gradientColor = OfficeGradientColor.TwoColor;
  private OfficeGradientPattern m_gradPattern = OfficeGradientPattern.Pat_5_Percent;
  private OfficeTexture m_gradTexture = OfficeTexture.Papyrus;
  private WorkbookImpl m_book;
  private ChartColor m_backColor = new ChartColor(ColorExtension.Black);
  private ChartColor m_foreColor = new ChartColor(ColorExtension.Gray);
  private OfficeGradientPreset m_presetGrad = OfficeGradientPreset.Grad_Early_Sunset;
  protected Image m_picture;
  private string m_strPictureName;
  private bool m_bVisible = true;
  private int m_imageIndex = -1;
  private double m_gradDegree = 0.2;
  private MsofbtOPT.FOPTE m_parsePictureData;
  protected bool m_bIsShapeFill = true;
  private bool m_bTile;
  private Rectangle m_fillrect;
  private Rectangle m_srcRect;
  private float m_amt;
  private GradientStops m_preseredGradient;
  private bool m_bSupportedGradient = true;
  private float m_textureVerticalScale;
  private float m_textureHorizontalScale;
  private float m_textureOffsetX;
  private float m_textureOffsetY;
  private string m_alignment;
  private string m_tileFlipping;
  private static Assembly s_asem = typeof (ShapeFillImpl).Assembly;
  private static byte[] m_arrPreset = new byte[1320];
  private static Dictionary<OfficeGradientPreset, byte[]> s_dicPresetStops;

  static ShapeFillImpl()
  {
    int index1 = 0;
    for (int index2 = 1; index2 <= 24; ++index2)
    {
      byte[] resData = ShapeFillImpl.GetResData("Grad" + index2.ToString());
      int length = resData.Length;
      ShapeFillImpl.m_arrPreset[index1] = (byte) length;
      int index3 = index1 + 1;
      resData.CopyTo((Array) ShapeFillImpl.m_arrPreset, index3);
      index1 = index3 + length;
    }
  }

  public static byte[] GetResData(string strID)
  {
    byte[] resData;
    if (!ShapeFillImpl.m_dicResources.TryGetValue(strID, out resData))
    {
      resData = (byte[]) new ResourceManager("Syncfusion.OfficeChart.TexturePatternGradient", ShapeFillImpl.s_asem).GetObject(strID);
      ShapeFillImpl.m_dicResources[strID] = resData;
    }
    return resData;
  }

  internal static Color ParseColor(WorkbookImpl book, byte[] value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    return value[5] != (byte) 8 ? Color.FromArgb((int) byte.MaxValue, (int) value[2], (int) value[3], (int) value[4]) : (value[2] == (byte) 80 /*0x50*/ ? ShapeFillImpl.DEF_COMENT_PARSE_COLOR : book.GetPaletteColor((OfficeKnownColors) value[2]));
  }

  internal static void ParseColor(WorkbookImpl book, byte[] value, ChartColor color)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (color == (ChartColor) null)
      throw new ArgumentNullException(nameof (color));
    if (value[5] == (byte) 8)
    {
      if (value[2] == (byte) 80 /*0x50*/)
        color.SetIndexed(OfficeKnownColors.BlackCustom | OfficeKnownColors.Dark_red);
      else
        color.SetIndexed((OfficeKnownColors) value[2]);
    }
    else if (value[5] == (byte) 16 /*0x10*/)
      color.SetRGB(Color.FromArgb(0, 0, 0, 0));
    else
      color.SetRGB(Color.FromArgb((int) byte.MaxValue, (int) value[2], (int) value[3], (int) value[4]), (IWorkbook) book);
  }

  public static GradientStops GetPresetGradientStops(OfficeGradientPreset preset)
  {
    return new GradientStops(ShapeFillImpl.GetPresetGradientStopsData(preset));
  }

  public static byte[] GetPresetGradientStopsData(OfficeGradientPreset preset)
  {
    if (ShapeFillImpl.s_dicPresetStops == null)
      ShapeFillImpl.FillPresetsGradientStops();
    return ShapeFillImpl.s_dicPresetStops[preset];
  }

  public ShapeFillImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.m_foreColor = new ChartColor(ColorExtension.White);
    this.m_backColor = new ChartColor(ColorExtension.Empty);
    this.m_backColor.AfterChange += new ChartColor.AfterChangeHandler(this.ChangeVisible);
    this.m_foreColor.AfterChange += new ChartColor.AfterChangeHandler(this.ChangeVisible);
  }

  public ShapeFillImpl(IApplication application, object parent, OfficeFillType fillType)
    : this(application, parent)
  {
    this.m_fillType = fillType;
  }

  private void FindParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ApplicationException("Cann't find parent book");
  }

  public GradientStops GradientStops
  {
    get
    {
      GradientStops gradientStops = (GradientStops) null;
      if (this.m_fillType == OfficeFillType.Gradient && this.m_bSupportedGradient)
      {
        if (this.m_gradientColor == OfficeGradientColor.Preset)
          gradientStops = ShapeFillImpl.GetPresetGradientStops(this.m_presetGrad);
        else if (this.m_gradientColor == OfficeGradientColor.OneColor)
        {
          gradientStops = new GradientStops();
          GradientStopImpl gradientStopImpl1 = new GradientStopImpl(this.ForeColorObject, 0, 100000);
          gradientStops.Add(gradientStopImpl1);
          byte num = (byte) (this.m_gradDegree * (double) byte.MaxValue);
          int maxValue = (int) sbyte.MaxValue;
          int shade = (int) num <= maxValue ? (int) num * 100000 / (int) byte.MaxValue : -1;
          GradientStopImpl gradientStopImpl2 = new GradientStopImpl(this.ForeColorObject, 100000, 100000, (int) num > maxValue ? ((int) byte.MaxValue - (int) num) * 100000 / (int) byte.MaxValue : -1, shade);
          gradientStops.Add(gradientStopImpl2);
        }
        else if (this.m_gradientColor == OfficeGradientColor.TwoColor)
        {
          gradientStops = new GradientStops();
          GradientStopImpl gradientStopImpl3 = new GradientStopImpl(this.ForeColorObject, 0, 100000);
          gradientStops.Add(gradientStopImpl3);
          GradientStopImpl gradientStopImpl4 = new GradientStopImpl(this.BackColorObject, 100000, 100000);
          gradientStops.Add(gradientStopImpl4);
        }
        if (ShapeFillImpl.IsInverted(this.m_gradStyle, this.m_gradVariant))
          gradientStops.InvertGradientStops();
        if (ShapeFillImpl.IsDoubled(this.m_gradStyle, this.m_gradVariant))
          gradientStops.DoubleGradientStops();
        gradientStops.Angle = ShapeFillImpl.GradientAngle(this.m_gradStyle);
        gradientStops.FillToRect = ShapeFillImpl.GradientFillToRect(this.m_gradStyle, this.m_gradVariant);
        gradientStops.GradientType = ShapeFillImpl.GetGradientType(this.m_gradStyle);
      }
      return gradientStops;
    }
  }

  public bool Tile
  {
    get => this.m_bTile;
    set => this.m_bTile = value;
  }

  public GradientStops PreservedGradient
  {
    get => this.m_preseredGradient;
    set => this.m_preseredGradient = value;
  }

  public Rectangle FillRect
  {
    get => this.m_fillrect;
    set => this.m_fillrect = value;
  }

  public Rectangle SourceRect
  {
    get => this.m_srcRect;
    set => this.m_srcRect = value;
  }

  internal MsofbtOPT.FOPTE ParsePictureData => this.m_parsePictureData;

  public bool IsGradientSupported
  {
    get => this.m_bSupportedGradient;
    set => this.m_bSupportedGradient = value;
  }

  public OfficeFillType FillType
  {
    get => this.m_fillType;
    set
    {
      if (this.FillType == value && !(this.Parent is ChartWallOrFloorImpl))
        return;
      if (value == OfficeFillType.Picture)
        throw new ArgumentException("For set picture type use UserPicture method.");
      if (value == OfficeFillType.Texture)
        this.m_gradTexture = OfficeTexture.Papyrus;
      if (value == OfficeFillType.Gradient)
        this.m_gradVariant = OfficeGradientVariants.ShadingVariants_1;
      this.m_fillType = value;
      this.ChangeVisible();
    }
  }

  public OfficeGradientStyle GradientStyle
  {
    get
    {
      this.ValidateGradientType();
      return this.m_gradStyle;
    }
    set
    {
      this.ValidateGradientType();
      this.m_gradStyle = value;
      this.ChangeVisible();
    }
  }

  public OfficeGradientVariants GradientVariant
  {
    get
    {
      this.ValidateGradientType();
      return this.m_gradVariant;
    }
    set
    {
      this.ValidateGradientType();
      bool flag = value == OfficeGradientVariants.ShadingVariants_3 || value == OfficeGradientVariants.ShadingVariants_4;
      if (this.m_gradStyle == OfficeGradientStyle.FromCenter && flag)
        throw new NotSupportedException("This variant doesn't support center shading style.");
      this.m_gradVariant = value;
      this.ChangeVisible();
    }
  }

  public virtual double TransparencyTo
  {
    get
    {
      this.ValidateGradientType();
      return this.m_transparencyTo;
    }
    set
    {
      this.ValidateGradientType();
      this.m_transparencyTo = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (TransparencyTo));
      this.ChangeVisible();
    }
  }

  public virtual double TransparencyFrom
  {
    get => this.m_transparencyFrom;
    set
    {
      this.m_transparencyFrom = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (TransparencyFrom));
      this.ChangeVisible();
    }
  }

  public float TransparencyColor
  {
    get => this.m_amt;
    set
    {
      this.m_amt = (double) value >= 0.0 && 1.0 >= (double) value ? value : throw new ArgumentException("The specified value is out of range");
    }
  }

  public double Transparency
  {
    get
    {
      this.ValidateSolidType();
      return this.m_transparencyFrom;
    }
    set
    {
      this.ValidateSolidType();
      this.m_transparencyFrom = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException("Transparency is out of range");
    }
  }

  public OfficeGradientColor GradientColorType
  {
    get
    {
      this.ValidateGradientType();
      return this.m_gradientColor;
    }
    set
    {
      this.ValidateGradientType();
      this.m_gradientColor = value;
      this.ChangeVisible();
    }
  }

  public OfficeGradientPattern Pattern
  {
    get
    {
      this.ValidatePatternType();
      return this.m_gradPattern;
    }
    set
    {
      this.m_fillType = OfficeFillType.Pattern;
      this.m_gradPattern = value;
      this.ChangeVisible();
    }
  }

  public OfficeTexture Texture
  {
    get
    {
      this.ValidateTextureType();
      return this.m_gradTexture;
    }
    set
    {
      if (this.m_gradTexture == OfficeTexture.User_Defined)
        throw new ArgumentException("This method support only preset textured");
      this.m_fillType = OfficeFillType.Texture;
      this.m_gradTexture = value;
      this.ChangeVisible();
    }
  }

  public OfficeKnownColors BackColorIndex
  {
    get => this.BackColorObject.GetIndexed((IWorkbook) this.m_book);
    set => this.BackColorObject.SetIndexed(value);
  }

  public OfficeKnownColors ForeColorIndex
  {
    get => this.ForeColorObject.GetIndexed((IWorkbook) this.m_book);
    set => this.ForeColorObject.SetIndexed(value);
  }

  public virtual Color BackColor
  {
    get => this.BackColorObject.GetRGB((IWorkbook) this.m_book);
    set => this.BackColorObject.SetRGB(value, (IWorkbook) this.m_book);
  }

  public virtual Color ForeColor
  {
    get => this.ForeColorObject.GetRGB((IWorkbook) this.m_book);
    set => this.ForeColorObject.SetRGB(value, (IWorkbook) this.m_book);
  }

  public virtual ChartColor BackColorObject => this.m_backColor;

  public virtual ChartColor ForeColorObject => this.m_foreColor;

  public OfficeGradientPreset PresetGradientType
  {
    get
    {
      this.ValidateGradientType();
      if (this.m_gradientColor != OfficeGradientColor.Preset)
        throw new NotSupportedException("This property supported only if checked preset color type.");
      return this.m_presetGrad;
    }
    set
    {
      this.ValidateGradientType();
      this.m_gradientColor = OfficeGradientColor.Preset;
      this.m_presetGrad = value;
      this.ChangeVisible();
    }
  }

  public Image Picture
  {
    get
    {
      this.ValidatePictureProperties();
      return this.m_picture;
    }
  }

  public string PictureName
  {
    get
    {
      this.ValidatePictureProperties();
      return this.m_strPictureName;
    }
  }

  public virtual bool Visible
  {
    get => this.m_bVisible;
    set
    {
      if (this.Visible == value)
        return;
      this.m_bVisible = value;
    }
  }

  public double GradientDegree
  {
    get
    {
      this.ValidateGradientType();
      if (this.m_gradientColor != OfficeGradientColor.OneColor)
        throw new NotSupportedException("This property supports only if checked one color gradient");
      return this.m_gradDegree;
    }
    set
    {
      this.ValidateGradientType();
      if (this.m_gradientColor != OfficeGradientColor.OneColor)
        throw new NotSupportedException("This property supports only if checked one color gradient");
      this.m_gradDegree = value >= 0.0 && value <= 1.0 ? value : throw new ArgumentOutOfRangeException("Gradient degree is out of range.");
      this.ChangeVisible();
    }
  }

  public float TextureVerticalScale
  {
    get => this.m_textureVerticalScale;
    set
    {
      this.m_textureVerticalScale = (double) value < 21475.0 ? value : throw new ArgumentException("The specified value is out of range");
    }
  }

  public float TextureHorizontalScale
  {
    get => this.m_textureHorizontalScale;
    set
    {
      this.m_textureHorizontalScale = (double) value < 21475.0 ? value : throw new ArgumentException("The specified value is out of range");
    }
  }

  public float TextureOffsetX
  {
    get => this.m_textureOffsetX;
    set
    {
      this.m_textureOffsetX = (double) value < 169056.0 ? value : throw new ArgumentException("The specified value is out of range");
    }
  }

  public float TextureOffsetY
  {
    get => this.m_textureOffsetY;
    set
    {
      this.m_textureOffsetY = (double) value < 169056.0 ? value : throw new ArgumentException("The specified value is out of range");
    }
  }

  public string Alignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  public string TileFlipping
  {
    get => this.m_tileFlipping;
    set => this.m_tileFlipping = value;
  }

  public void UserPicture(string path)
  {
    if (path == null || path.Length == 0)
      throw new ArgumentException("Path canot be null or empty.");
    string name = File.Exists(path) ? Path.GetFileNameWithoutExtension(path) : throw new FileNotFoundException("File represents by current path doesn't exist.");
    this.UserPicture(Image.FromFile(path), name);
  }

  public void UserPicture(Image im, string name)
  {
    if (name == null || name.Length == 0)
      throw new ArgumentException("name canot be null or empty.");
    if (im == null)
      throw new ArgumentNullException(nameof (im));
    this.m_fillType = OfficeFillType.Picture;
    this.m_picture = im;
    this.m_strPictureName = name;
    this.ChangeVisible();
    this.m_imageIndex = this.SetPictureToBse(im, name);
  }

  public void UserTexture(string path)
  {
    if (path == null || path.Length == 0)
      throw new ArgumentException("path canot be null or empty.");
    string name = File.Exists(path) ? Path.GetFileNameWithoutExtension(path) : throw new FileNotFoundException("File represents by current path doesn't exist.");
    this.UserTexture(Image.FromFile(path), name);
  }

  public void UserTexture(Image im, string name)
  {
    if (name == null || name.Length == 0)
      throw new ArgumentException("name canot be null or empty.");
    if (im == null)
      throw new ArgumentNullException(nameof (im));
    this.m_fillType = OfficeFillType.Texture;
    this.m_gradTexture = OfficeTexture.User_Defined;
    this.m_picture = im;
    this.m_strPictureName = name;
    this.ChangeVisible();
    this.m_imageIndex = this.SetPictureToBse(im, name);
  }

  public void Patterned(OfficeGradientPattern pattern)
  {
    this.Pattern = pattern;
    this.ChangeVisible();
  }

  public void PresetGradient(OfficeGradientPreset grad)
  {
    this.PresetGradient(grad, OfficeGradientStyle.Horizontal);
  }

  public void PresetGradient(OfficeGradientPreset grad, OfficeGradientStyle shadStyle)
  {
    this.PresetGradient(grad, shadStyle, OfficeGradientVariants.ShadingVariants_1);
  }

  public void PresetGradient(
    OfficeGradientPreset grad,
    OfficeGradientStyle shadStyle,
    OfficeGradientVariants shadVar)
  {
    if (shadStyle == OfficeGradientStyle.FromCenter && shadVar > OfficeGradientVariants.ShadingVariants_2)
      throw new ArgumentException("From centr style support only var_1 or var_2");
    this.m_fillType = OfficeFillType.Gradient;
    this.m_gradientColor = OfficeGradientColor.Preset;
    this.m_presetGrad = grad;
    this.m_gradStyle = shadStyle;
    this.m_gradVariant = shadVar;
    this.m_bSupportedGradient = true;
    this.ChangeVisible();
  }

  public void PresetTextured(OfficeTexture texture)
  {
    this.Texture = texture;
    this.ChangeVisible();
  }

  public void TwoColorGradient() => this.TwoColorGradient(OfficeGradientStyle.Horizontal);

  public void TwoColorGradient(OfficeGradientStyle style)
  {
    this.TwoColorGradient(style, OfficeGradientVariants.ShadingVariants_1);
  }

  public void TwoColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant)
  {
    if (style == OfficeGradientStyle.FromCenter && variant > OfficeGradientVariants.ShadingVariants_2)
      throw new ArgumentException("From centr style support only var_1 or var_2");
    this.m_fillType = OfficeFillType.Gradient;
    this.m_gradientColor = OfficeGradientColor.TwoColor;
    this.m_gradStyle = style;
    this.m_gradVariant = variant;
    this.m_bSupportedGradient = true;
    this.ChangeVisible();
  }

  public void OneColorGradient() => this.OneColorGradient(OfficeGradientStyle.Horizontal);

  public void OneColorGradient(OfficeGradientStyle style)
  {
    this.OneColorGradient(style, OfficeGradientVariants.ShadingVariants_1);
  }

  public void OneColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant)
  {
    if (style == OfficeGradientStyle.FromCenter && variant > OfficeGradientVariants.ShadingVariants_2)
      throw new ArgumentException("From centr style support only var_1 or var_2");
    this.m_fillType = OfficeFillType.Gradient;
    this.m_gradientColor = OfficeGradientColor.OneColor;
    this.m_gradStyle = style;
    this.m_gradVariant = variant;
    this.m_bSupportedGradient = true;
    this.ChangeVisible();
  }

  public void Solid()
  {
    this.m_fillType = OfficeFillType.SolidColor;
    this.ChangeVisible();
  }

  public int CompareTo(IGradient twin)
  {
    if (twin == null)
      return 1;
    int num1 = this.m_gradStyle == twin.GradientStyle ? 0 : 1;
    if (num1 != 0)
      return num1;
    int num2 = this.m_gradVariant == twin.GradientVariant ? 0 : 1;
    if (num2 != 0)
      return num2;
    int num3 = this.m_backColor == twin.BackColorObject ? 0 : 1;
    if (num3 != 0)
      return num3;
    int num4 = this.m_foreColor == twin.ForeColorObject ? 0 : 1;
    return num4 != 0 ? num4 : num4;
  }

  [CLSCompliant(false)]
  public bool ParseOption(MsofbtOPT.FOPTE option)
  {
    switch (option.Id)
    {
      case MsoOptions.FillType:
        this.ParseFillType(option.UInt32Value);
        return true;
      case MsoOptions.ForeColor:
        ShapeFillImpl.ParseColor(this.m_book, option.MainData, this.ForeColorObject);
        return true;
      case MsoOptions.Transparency:
        this.m_transparencyFrom = ShapeLineFormatImpl.ParseTransparency(option.UInt32Value);
        return true;
      case MsoOptions.BackColor:
        ShapeFillImpl.ParseColor(this.m_book, option.MainData, this.BackColorObject);
        this.m_gradDegree = this.ParseGradientDegree(option.MainData);
        return true;
      case MsoOptions.GradientTransparency:
        this.m_transparencyTo = ShapeLineFormatImpl.ParseTransparency(option.UInt32Value);
        return true;
      case MsoOptions.PatternTexture:
        if (this.m_fillType == OfficeFillType.Picture || this.m_fillType == OfficeFillType.Texture)
          this.m_parsePictureData = option;
        return true;
      case MsoOptions.PattTextName:
        byte[] additionalData = option.AdditionalData;
        if (!option.IsValid && additionalData != null && additionalData.Length > 0)
          this.ParsePattTextName(additionalData);
        return true;
      case MsoOptions.ShadStyle:
        this.ParseShadingStyle(option.MainData);
        return true;
      case MsoOptions.ShadVariant:
        this.ParseShadingVariant(option.MainData[2]);
        return true;
      case MsoOptions.ShadingStyleCorner_1:
        if (this.m_gradStyle == OfficeGradientStyle.FromCorner && option.MainData[4] == (byte) 1)
          this.m_gradVariant = OfficeGradientVariants.ShadingVariants_2;
        return true;
      case MsoOptions.ShadingStyleCorner_2:
        this.ParseCornerVariants(option.MainData[4]);
        return true;
      case MsoOptions.PresetGradientData:
        this.ParsePresetGradient(option.AdditionalData);
        return true;
      case MsoOptions.GradientColorType:
        this.ParseGradientColor(option.UInt32Value);
        return true;
      case MsoOptions.NoFillHitTest:
        this.ParseVisible(option.MainData);
        return true;
      default:
        return false;
    }
  }

  private void ParseFillType(uint value)
  {
    this.m_fillType = (OfficeFillType) value;
    if (value == 5U)
    {
      this.m_fillType = OfficeFillType.Gradient;
      this.m_gradStyle = OfficeGradientStyle.FromCorner;
      this.m_gradVariant = OfficeGradientVariants.ShadingVariants_1;
    }
    if (value != 6U)
      return;
    this.m_fillType = OfficeFillType.Gradient;
    this.m_gradStyle = OfficeGradientStyle.FromCenter;
  }

  private void ParseShadingStyle(byte[] arr)
  {
    if (arr == null)
      throw new ArgumentNullException(nameof (arr));
    if (this.m_gradStyle == OfficeGradientStyle.FromCenter || this.m_gradStyle == OfficeGradientStyle.FromCorner)
      return;
    switch (arr[4])
    {
      case 90:
      case 166:
        this.m_gradStyle = OfficeGradientStyle.Vertical;
        break;
      case 121:
        this.m_gradStyle = OfficeGradientStyle.DiagonalUp;
        break;
      case 211:
        this.m_gradStyle = OfficeGradientStyle.DiagonalDown;
        this.m_gradVariant = OfficeGradientVariants.ShadingVariants_1;
        break;
    }
  }

  private void ParseShadingVariant(byte value)
  {
    if (this.m_gradStyle == OfficeGradientStyle.FromCorner)
      return;
    if (this.m_gradStyle == OfficeGradientStyle.FromCenter)
    {
      this.m_gradVariant = value == (byte) 100 ? OfficeGradientVariants.ShadingVariants_1 : OfficeGradientVariants.ShadingVariants_2;
    }
    else
    {
      switch (value)
      {
        case 50:
          this.m_gradVariant = this.m_gradStyle == OfficeGradientStyle.Horizontal ? OfficeGradientVariants.ShadingVariants_3 : OfficeGradientVariants.ShadingVariants_4;
          break;
        case 100:
          this.m_gradVariant = this.m_gradStyle == OfficeGradientStyle.DiagonalDown ? OfficeGradientVariants.ShadingVariants_2 : OfficeGradientVariants.ShadingVariants_1;
          break;
        case 206:
          this.m_gradVariant = this.m_gradStyle == OfficeGradientStyle.Horizontal ? OfficeGradientVariants.ShadingVariants_4 : OfficeGradientVariants.ShadingVariants_3;
          break;
        default:
          this.m_gradVariant = this.m_gradStyle == OfficeGradientStyle.DiagonalDown ? OfficeGradientVariants.ShadingVariants_1 : OfficeGradientVariants.ShadingVariants_2;
          break;
      }
    }
  }

  private void ParsePattTextName(byte[] addData)
  {
    bool flag1 = this.m_fillType == OfficeFillType.Pattern;
    bool flag2 = this.m_fillType == OfficeFillType.Picture;
    bool flag3 = this.m_fillType == OfficeFillType.Texture;
    if (!flag1 && !flag3 && !flag2)
      return;
    if (addData == null)
      throw new ArgumentNullException(nameof (addData));
    string strName = "";
    int index1 = 0;
    for (int index2 = addData.Length - 2; index1 < index2; index1 += 2)
      strName += (string) (object) (char) addData[index1];
    if (flag2)
      this.ParsePictureOrUserDefinedTexture(strName, true);
    else if (flag3 && strName[0] >= '0' && strName[0] <= '9')
    {
      this.ParsePictureOrUserDefinedTexture(strName, false);
    }
    else
    {
      string str1 = strName.Replace(' ', '_');
      if (flag1)
      {
        string str2 = "Pat_" + str1;
        try
        {
          this.m_gradPattern = (OfficeGradientPattern) Enum.Parse(typeof (OfficeGradientPattern), str2, true);
        }
        catch
        {
          this.m_gradPattern = OfficeGradientPattern.Pat_5_Percent;
        }
      }
      else
      {
        try
        {
          this.m_gradTexture = (OfficeTexture) Enum.Parse(typeof (OfficeTexture), str1, true);
        }
        catch
        {
          this.ParsePictureOrUserDefinedTexture(strName, false);
        }
      }
    }
  }

  private void ParseGradientColor(uint value)
  {
    if (value == 0U)
      this.m_gradientColor = OfficeGradientColor.Preset;
    else if (value == 1073741835U /*0x4000000B*/)
      this.m_gradientColor = OfficeGradientColor.OneColor;
    else
      this.m_gradientColor = OfficeGradientColor.TwoColor;
  }

  private void ParsePresetGradient(byte[] value)
  {
    if (value == null || this.m_fillType != OfficeFillType.Gradient)
      return;
    int num1 = 1;
    int index1 = 0;
    int length = value.Length;
    bool flag;
    for (flag = false; num1 < 25 && !flag; ++num1)
    {
      int num2 = (int) ShapeFillImpl.m_arrPreset[index1];
      int num3 = index1 + 1;
      if (num2 == length)
      {
        for (int index2 = 0; index2 < num2 && (int) value[index2] == (int) ShapeFillImpl.m_arrPreset[num3 + index2]; ++index2)
        {
          if (num2 - index2 == 1)
            flag = true;
        }
      }
      index1 = num3 + num2;
    }
    if (!flag)
      return;
    this.m_presetGrad = (OfficeGradientPreset) (num1 - 1);
  }

  private void ParsePictureOrUserDefinedTexture(string strName, bool bIsPicture)
  {
    this.m_strPictureName = strName != null && strName.Length != 0 ? strName : throw new ArgumentException("strName canot be null or empty.");
    this.ParsePictureOrUserDefinedTexture(bIsPicture);
  }

  protected void ParsePictureOrUserDefinedTexture(bool bIsPicture)
  {
    if (!bIsPicture)
      this.m_gradTexture = OfficeTexture.User_Defined;
    byte[] additionalData = this.m_parsePictureData.AdditionalData;
    if (additionalData == null || additionalData.Length == 0)
    {
      this.m_imageIndex = (int) this.m_parsePictureData.UInt32Value;
      this.m_picture = this.m_book.ShapesData.GetPicture(this.m_imageIndex).PictureRecord.Picture;
    }
    else
    {
      byte[] numArray = new byte[additionalData.Length - 25];
      Array.Copy((Array) additionalData, 25, (Array) numArray, 0, numArray.Length);
      MemoryStream ms = new MemoryStream();
      ShapeFillImpl.UpdateBitMapHederToStream(ms, additionalData);
      ms.Write(numArray, 0, numArray.Length);
      this.m_picture = ApplicationImpl.CreateImage((Stream) ms);
    }
    this.m_parsePictureData = (MsofbtOPT.FOPTE) null;
  }

  public static void UpdateBitMapHederToStream(MemoryStream ms, byte[] arr)
  {
    if (ms == null)
      throw new ArgumentNullException(nameof (ms));
    if (arr == null)
      throw new ArgumentNullException(nameof (arr));
    if (!BiffRecordRaw.CompareArrays(arr, 0, ShapeFillImpl.DEF_BITMAP_INDEX, 0, ShapeFillImpl.DEF_BITMAP_INDEX.Length))
      return;
    int iFullSize = arr.Length + 14 - 25;
    uint uint32_1 = BitConverter.ToUInt32(arr, 25);
    uint uint32_2 = BitConverter.ToUInt32(arr, 57);
    MsoBitmapPicture.AddBitMapHeaderToStream(ms, iFullSize, uint32_1, uint32_2);
  }

  private void ParseVisible(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if ((byte) ((uint) data[2] & 16U /*0x10*/) > (byte) 0)
    {
      this.m_bVisible = true;
    }
    else
    {
      byte num = (byte) ((uint) data[4] & 16U /*0x10*/);
      if (data[3] == (byte) 0 && data[5] == (byte) 0 && num > (byte) 0)
        this.m_bVisible = false;
      else
        this.m_bVisible = true;
    }
  }

  [CLSCompliant(false)]
  public IFopteOptionWrapper Serialize(IFopteOptionWrapper opt)
  {
    opt = opt != null ? this.SerializeFillType(opt) : throw new ArgumentNullException(nameof (opt));
    opt = this.SerializeTransparency(opt);
    opt = this.SerializeVisible(opt);
    ShapeLineFormatImpl.SerializeColor(opt, this.ForeColorObject, this.m_book, MsoOptions.ForeColor);
    ShapeLineFormatImpl.SerializeColor(opt, this.BackColorObject, this.m_book, MsoOptions.BackColor);
    switch (this.m_fillType)
    {
      case OfficeFillType.SolidColor:
        return this.SerializeSolidColor(opt);
      case OfficeFillType.Pattern:
      case OfficeFillType.Texture:
        return this.SerializePatternTexture(opt);
      case OfficeFillType.Picture:
        return this.SerializePicture(opt);
      case OfficeFillType.UnknownGradient:
        return opt;
      case OfficeFillType.Gradient:
        return this.SerializeGradient(opt);
      default:
        throw new ApplicationException("Unknown fill type");
    }
  }

  private IFopteOptionWrapper SerializeGradient(IFopteOptionWrapper opt)
  {
    opt = opt != null ? this.SerializeShadVariant(opt) : throw new ArgumentNullException(nameof (opt));
    opt = this.SerializeGradientStyle(opt);
    if (this.m_gradStyle != OfficeGradientStyle.Horizontal)
      opt = this.SerializeShadStyle(opt);
    if (this.m_gradientColor == OfficeGradientColor.Preset)
      opt = this.SerializeGradientPreset(opt);
    return opt;
  }

  private IFopteOptionWrapper SerializePatternTexture(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    bool flag = this.m_fillType == OfficeFillType.Pattern;
    if (!flag && this.m_gradTexture == OfficeTexture.User_Defined)
      return this.SerializePicture(opt);
    int num;
    string str1;
    string str2;
    if (flag)
    {
      num = (int) this.m_gradPattern;
      str1 = this.m_gradPattern.ToString();
      str2 = "Patt";
    }
    else
    {
      num = (int) this.m_gradTexture;
      str1 = this.m_gradTexture.ToString();
      str2 = "Text";
    }
    byte[] resData = ShapeFillImpl.GetResData(str2 + num.ToString());
    ShapeImpl.SerializeForte(opt, MsoOptions.PatternTexture, 0, resData, true);
    if (flag)
      str1 = str1.Substring("Pat_".Length).Replace("_Percent", "%");
    byte[] byteArray = this.ConvertNameToByteArray(str1.Replace('_', ' '));
    ShapeImpl.SerializeForte(opt, MsoOptions.PattTextName, 0, byteArray, true);
    return opt;
  }

  private IFopteOptionWrapper SerializePicture(IFopteOptionWrapper opt)
  {
    opt = opt != null ? this.SetPicture(opt) : throw new ArgumentNullException(nameof (opt));
    if (!string.IsNullOrEmpty(this.m_strPictureName))
    {
      byte[] byteArray = this.ConvertNameToByteArray(this.m_strPictureName);
      ShapeImpl.SerializeForte(opt, MsoOptions.PattTextName, 0, byteArray, true);
    }
    return opt;
  }

  private IFopteOptionWrapper SerializeSolidColor(IFopteOptionWrapper opt) => opt;

  private IFopteOptionWrapper SerializeFillType(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    int num = (int) this.m_fillType;
    if (this.m_fillType == OfficeFillType.Gradient && this.m_gradStyle == OfficeGradientStyle.FromCorner)
      num = 5;
    if (this.m_fillType == OfficeFillType.Gradient && this.m_gradStyle == OfficeGradientStyle.FromCenter)
      num = 6;
    ShapeImpl.SerializeForte(opt, MsoOptions.FillType, num);
    return opt;
  }

  private IFopteOptionWrapper SerializeShadStyle(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    byte[] arr = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue
    };
    switch (this.m_gradStyle)
    {
      case OfficeGradientStyle.Vertical:
        arr[2] = (byte) 166;
        break;
      case OfficeGradientStyle.DiagonalUp:
        arr[2] = (byte) 121;
        break;
      case OfficeGradientStyle.DiagonalDown:
        arr[2] = (byte) 211;
        break;
      default:
        return opt;
    }
    ShapeImpl.SerializeForte(opt, MsoOptions.ShadStyle, arr);
    return opt;
  }

  private IFopteOptionWrapper SerializeShadVariant(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    if (this.m_gradStyle == OfficeGradientStyle.FromCorner)
      return this.SerializeShadVariantCorner(opt);
    if (this.m_gradStyle == OfficeGradientStyle.FromCenter)
      return this.SerializeShadVariantCenter(opt);
    if (this.m_gradVariant < OfficeGradientVariants.ShadingVariants_3)
    {
      if (!(this.m_gradStyle == OfficeGradientStyle.DiagonalDown ? this.m_gradVariant != OfficeGradientVariants.ShadingVariants_2 : this.m_gradVariant == OfficeGradientVariants.ShadingVariants_2))
        ShapeImpl.SerializeForte(opt, MsoOptions.ShadVariant, ShapeFillImpl.DEF_VARIANT_FIRST_ARR);
      return opt;
    }
    if (this.m_gradStyle == OfficeGradientStyle.Horizontal ? this.m_gradVariant != OfficeGradientVariants.ShadingVariants_3 : this.m_gradVariant == OfficeGradientVariants.ShadingVariants_3)
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadVariant, ShapeFillImpl.DEF_VARIANT_THIRD_ARR);
    else
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadVariant, ShapeFillImpl.DEF_VARIANT_FOURTH_ARR);
    return opt;
  }

  private IFopteOptionWrapper SerializeShadVariantCenter(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    if (this.m_gradVariant == OfficeGradientVariants.ShadingVariants_1)
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadVariant, ShapeFillImpl.DEF_VARIANT_FIRST_ARR);
    ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_1, ShapeFillImpl.DEF_VARIANT_CENTER_ADD_DATA);
    ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_2, ShapeFillImpl.DEF_VARIANT_CENTER_ADD_DATA);
    ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_3, ShapeFillImpl.DEF_VARIANT_CENTER_ADD_DATA);
    ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_4, ShapeFillImpl.DEF_VARIANT_CENTER_ADD_DATA);
    return opt;
  }

  private IFopteOptionWrapper SerializeShadVariantCorner(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    ShapeImpl.SerializeForte(opt, MsoOptions.ShadVariant, ShapeFillImpl.DEF_VARIANT_FIRST_ARR);
    if (this.m_gradVariant == OfficeGradientVariants.ShadingVariants_1)
      return opt;
    if (this.m_gradVariant == OfficeGradientVariants.ShadingVariants_2 || this.m_gradVariant == OfficeGradientVariants.ShadingVariants_4)
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_1, ShapeFillImpl.DEF_VARIANT_CORNER_ADD_DATA);
    if (this.m_gradVariant == OfficeGradientVariants.ShadingVariants_3 || this.m_gradVariant == OfficeGradientVariants.ShadingVariants_4)
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_2, ShapeFillImpl.DEF_VARIANT_CORNER_ADD_DATA);
    if (this.m_gradVariant == OfficeGradientVariants.ShadingVariants_2 || this.m_gradVariant == OfficeGradientVariants.ShadingVariants_4)
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_3, ShapeFillImpl.DEF_VARIANT_CORNER_ADD_DATA);
    if (this.m_gradVariant == OfficeGradientVariants.ShadingVariants_3 || this.m_gradVariant == OfficeGradientVariants.ShadingVariants_4)
      ShapeImpl.SerializeForte(opt, MsoOptions.ShadingStyleCorner_4, ShapeFillImpl.DEF_VARIANT_CORNER_ADD_DATA);
    return opt;
  }

  private IFopteOptionWrapper SerializeGradientStyle(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    if (this.m_gradientColor != OfficeGradientColor.OneColor)
      return opt;
    ShapeImpl.SerializeForte(opt, MsoOptions.GradientColorType, 1073741835 /*0x4000000B*/);
    this.SerializeGradientDegree(opt);
    return opt;
  }

  private IFopteOptionWrapper SerializeGradientPreset(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    byte[] resData = ShapeFillImpl.GetResData("Grad" + ((int) this.m_presetGrad).ToString());
    ShapeImpl.SerializeForte(opt, MsoOptions.PresetGradientData, 0, resData, true);
    ShapeImpl.SerializeForte(opt, MsoOptions.GradientColorType, 0);
    return opt;
  }

  private IFopteOptionWrapper SerializeVisible(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    byte[] arr = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 31 /*0x1F*/,
      (byte) 0
    };
    if (this.m_bVisible)
      arr[0] = (byte) 28;
    ShapeImpl.SerializeForte(opt, MsoOptions.NoFillHitTest, arr);
    return opt;
  }

  private IFopteOptionWrapper SerializeGradientDegree(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    byte[] arr = new byte[4]
    {
      (byte) 240 /*0xF0*/,
      (byte) 1,
      (byte) 0,
      (byte) 16 /*0x10*/
    };
    double num = this.m_gradDegree;
    if (num >= 0.5)
    {
      arr[1] = (byte) 2;
      num = 1.0 - num;
    }
    arr[2] = (byte) (num * (double) byte.MaxValue * 2.0);
    ShapeImpl.SerializeForte(opt, MsoOptions.BackColor, arr);
    return opt;
  }

  private void ValidateGradientType()
  {
    if (this.m_fillType != OfficeFillType.Gradient)
      throw new NotSupportedException("This property can be set only when Gradient Style is selected.");
  }

  private void ValidatePictureProperties()
  {
    bool flag = this.m_fillType == OfficeFillType.Texture && this.m_gradTexture == OfficeTexture.User_Defined;
    if (this.m_fillType != OfficeFillType.Picture && !flag)
      throw new NotSupportedException("This property support only if defined user texture of picture");
  }

  private void ValidatePatternType()
  {
    if (this.m_fillType != OfficeFillType.Pattern)
      throw new NotSupportedException("This property suports only if chacked pattern style.");
  }

  private void ValidateTextureType()
  {
    if (this.m_fillType != OfficeFillType.Texture)
      throw new NotSupportedException("This property suports only if chacked texture style.");
  }

  private void ValidateSolidType()
  {
    if (this.m_fillType != OfficeFillType.SolidColor)
      throw new NotSupportedException("This property supports only if Checked Solid style.");
  }

  private byte[] ConvertNameToByteArray(string strName)
  {
    int num = strName != null && strName.Length != 0 ? strName.Length : throw new ArgumentException("strName canot be null or empty.");
    byte[] byteArray = new byte[num * 2 + 2];
    for (int index = 0; index < num; ++index)
    {
      char lower = strName[index];
      if (char.IsUpper(lower) && index > 0)
        lower = char.ToLower(lower);
      byteArray[2 * index] = (byte) lower;
      byteArray[2 * index + 1] = (byte) 0;
    }
    byteArray[2 * num] = (byte) 0;
    byteArray[2 * num + 1] = (byte) 0;
    return byteArray;
  }

  private double ParseGradientDegree(byte[] value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    double gradientDegree = this.m_gradDegree;
    if (value[5] == (byte) 16 /*0x10*/)
    {
      gradientDegree = (double) value[4] * 0.5 / (double) byte.MaxValue;
      if (value[3] == (byte) 2)
        gradientDegree = 1.0 - gradientDegree;
    }
    return gradientDegree;
  }

  private void ParseCornerVariants(byte value)
  {
    if (this.m_gradStyle != OfficeGradientStyle.FromCorner || value != (byte) 1)
      return;
    this.m_gradVariant = this.m_gradVariant == OfficeGradientVariants.ShadingVariants_1 ? OfficeGradientVariants.ShadingVariants_3 : OfficeGradientVariants.ShadingVariants_4;
  }

  private static void FillPresetsGradientStops()
  {
    ShapeFillImpl.s_dicPresetStops = new Dictionary<OfficeGradientPreset, byte[]>();
    OfficeGradientPreset[] officeGradientPresetArray = new OfficeGradientPreset[24]
    {
      OfficeGradientPreset.Grad_Early_Sunset,
      OfficeGradientPreset.Grad_Late_Sunset,
      OfficeGradientPreset.Grad_Nightfall,
      OfficeGradientPreset.Grad_Daybreak,
      OfficeGradientPreset.Grad_Horizon,
      OfficeGradientPreset.Grad_Desert,
      OfficeGradientPreset.Grad_Ocean,
      OfficeGradientPreset.Grad_Calm_Water,
      OfficeGradientPreset.Grad_Fire,
      OfficeGradientPreset.Grad_Fog,
      OfficeGradientPreset.Grad_Moss,
      OfficeGradientPreset.Grad_Peacock,
      OfficeGradientPreset.Grad_Wheat,
      OfficeGradientPreset.Grad_Parchment,
      OfficeGradientPreset.Grad_Mahogany,
      OfficeGradientPreset.Grad_Rainbow,
      OfficeGradientPreset.Grad_RainbowII,
      OfficeGradientPreset.Grad_Gold,
      OfficeGradientPreset.Grad_GoldII,
      OfficeGradientPreset.Grad_Brass,
      OfficeGradientPreset.Grad_Chrome,
      OfficeGradientPreset.Grad_ChromeII,
      OfficeGradientPreset.Grad_Silver,
      OfficeGradientPreset.Grad_Sapphire
    };
    ResourceManager resourceManager = new ResourceManager("Syncfusion.OfficeChart.PresetGradients", ShapeFillImpl.s_asem);
    int index = 0;
    for (int length = officeGradientPresetArray.Length; index < length; ++index)
    {
      OfficeGradientPreset key = officeGradientPresetArray[index];
      byte[] numArray = (byte[]) resourceManager.GetObject(key.ToString());
      ShapeFillImpl.s_dicPresetStops.Add(key, numArray);
    }
  }

  [CLSCompliant(false)]
  protected virtual IFopteOptionWrapper SetPicture(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    ShapeImpl.SerializeForte(opt, MsoOptions.PatternTexture, this.m_imageIndex, (byte[]) null, true);
    return opt;
  }

  protected virtual int SetPictureToBse(Image im, string strName)
  {
    if (im == null)
      throw new ArgumentNullException(nameof (im));
    WorkbookShapeDataImpl shapesData = this.m_book.ShapesData;
    if (this.m_imageIndex >= 0)
    {
      MsofbtBSE picture = shapesData.GetPicture(this.m_imageIndex);
      if (picture != null && picture.RefCount <= 1U)
        shapesData.RemovePicture((uint) (this.m_imageIndex - 1), true);
    }
    return shapesData.AddPicture(im, ExcelImageFormat.Original, strName);
  }

  [CLSCompliant(false)]
  protected virtual IFopteOptionWrapper SerializeTransparency(IFopteOptionWrapper opt)
  {
    ShapeLineFormatImpl.SerializeTransparency(opt, MsoOptions.GradientTransparency, this.m_transparencyTo);
    ShapeLineFormatImpl.SerializeTransparency(opt, MsoOptions.Transparency, this.m_transparencyFrom);
    return opt;
  }

  internal virtual void ChangeVisible() => this.Visible = true;

  public virtual ShapeFillImpl Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ShapeFillImpl shapeFillImpl = (ShapeFillImpl) this.MemberwiseClone();
    shapeFillImpl.SetParent(parent);
    shapeFillImpl.FindParents();
    shapeFillImpl.m_picture = (Image) CloneUtils.CloneCloneable((ICloneable) this.m_picture);
    return shapeFillImpl;
  }

  public void CopyFrom(ShapeFillImpl fill)
  {
    this.m_fillType = fill.m_fillType;
    this.m_gradStyle = fill.m_gradStyle;
    this.m_gradVariant = fill.m_gradVariant;
    this.m_transparencyTo = fill.m_transparencyTo;
    this.m_transparencyFrom = fill.m_transparencyFrom;
    this.m_gradientColor = fill.m_gradientColor;
    this.m_gradPattern = fill.m_gradPattern;
    this.m_gradTexture = fill.m_gradTexture;
    this.m_book = fill.m_book;
    this.m_backColor.CopyFrom(fill.m_backColor, false);
    this.m_foreColor.CopyFrom(fill.m_foreColor, false);
    this.m_presetGrad = fill.m_presetGrad;
    this.m_picture = (Image) fill.m_picture.Clone();
    this.m_strPictureName = fill.m_strPictureName;
    this.m_bVisible = fill.m_bVisible;
    this.m_imageIndex = fill.m_imageIndex;
    this.m_gradDegree = fill.m_gradDegree;
    this.m_parsePictureData = (MsofbtOPT.FOPTE) fill.m_parsePictureData.Clone();
    this.m_bIsShapeFill = fill.m_bIsShapeFill;
  }

  public static bool IsInverted(OfficeGradientStyle gradientStyle, OfficeGradientVariants variant)
  {
    bool flag = false;
    switch (gradientStyle)
    {
      case OfficeGradientStyle.Horizontal:
      case OfficeGradientStyle.Vertical:
      case OfficeGradientStyle.DiagonalUp:
      case OfficeGradientStyle.FromCenter:
        flag = ShapeFillImpl.StandardInverted(variant);
        break;
      case OfficeGradientStyle.DiagonalDown:
        flag = ShapeFillImpl.DiagonalDownInverted(variant);
        break;
      case OfficeGradientStyle.FromCorner:
        flag = false;
        break;
    }
    return flag;
  }

  private static bool DiagonalDownInverted(OfficeGradientVariants variant)
  {
    return variant == OfficeGradientVariants.ShadingVariants_1 || variant == OfficeGradientVariants.ShadingVariants_4;
  }

  private static bool StandardInverted(OfficeGradientVariants variant)
  {
    return variant == OfficeGradientVariants.ShadingVariants_2 || variant == OfficeGradientVariants.ShadingVariants_4;
  }

  public static bool IsDoubled(OfficeGradientStyle gradientStyle, OfficeGradientVariants variant)
  {
    bool flag = false;
    switch (gradientStyle)
    {
      case OfficeGradientStyle.Horizontal:
      case OfficeGradientStyle.Vertical:
      case OfficeGradientStyle.DiagonalUp:
      case OfficeGradientStyle.DiagonalDown:
      case OfficeGradientStyle.FromCenter:
        flag = ShapeFillImpl.StandardDoubled(variant);
        break;
      case OfficeGradientStyle.FromCorner:
        flag = false;
        break;
    }
    return flag;
  }

  private static bool StandardDoubled(OfficeGradientVariants variant)
  {
    return variant == OfficeGradientVariants.ShadingVariants_3 || variant == OfficeGradientVariants.ShadingVariants_4;
  }

  private static int GradientAngle(OfficeGradientStyle gradientStyle)
  {
    int num = -1;
    switch (gradientStyle)
    {
      case OfficeGradientStyle.Horizontal:
        num = 5400000;
        break;
      case OfficeGradientStyle.Vertical:
        num = 0;
        break;
      case OfficeGradientStyle.DiagonalUp:
        num = 2700000;
        break;
      case OfficeGradientStyle.DiagonalDown:
        num = 18900000;
        break;
    }
    return num;
  }

  private static Rectangle GradientFillToRect(
    OfficeGradientStyle gradientStyle,
    OfficeGradientVariants variant)
  {
    Rectangle rect = Rectangle.Empty;
    switch (gradientStyle)
    {
      case OfficeGradientStyle.FromCorner:
        rect = ShapeFillImpl.RectanglesCorner[(int) variant];
        break;
      case OfficeGradientStyle.FromCenter:
        rect = ShapeFillImpl.RectangleFromCenter;
        break;
    }
    return rect;
  }

  private static GradientType GetGradientType(OfficeGradientStyle gradStyle)
  {
    switch (gradStyle)
    {
      case OfficeGradientStyle.Horizontal:
      case OfficeGradientStyle.Vertical:
      case OfficeGradientStyle.DiagonalUp:
      case OfficeGradientStyle.DiagonalDown:
        return GradientType.Liniar;
      case OfficeGradientStyle.FromCorner:
      case OfficeGradientStyle.FromCenter:
        return GradientType.Rect;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  internal void Clear()
  {
    if (this.m_backColor != (ChartColor) null)
      this.m_backColor.Dispose();
    if (this.m_foreColor != (ChartColor) null)
      this.m_foreColor.Dispose();
    if (this.m_picture != null)
      this.m_picture.Dispose();
    if (this.m_preseredGradient != null)
      this.m_preseredGradient.Dispose();
    this.m_backColor = (ChartColor) null;
    this.m_foreColor = (ChartColor) null;
    this.m_picture = (Image) null;
    this.m_parsePictureData = (MsofbtOPT.FOPTE) null;
  }
}
