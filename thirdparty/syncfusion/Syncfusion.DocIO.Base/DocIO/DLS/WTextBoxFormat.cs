// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTextBoxFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTextBoxFormat : FormatBase
{
  internal const float DEF_LINE_WIDTH = 0.75f;
  internal const byte LineWidthKey = 11;
  private HorizontalOrigin m_horizRelation;
  private VerticalOrigin m_vertRelation;
  private WidthOrigin m_widthRelation;
  private HeightOrigin m_heightRelation;
  private float m_width;
  private float m_height;
  private Color m_fillColor;
  private Color m_lineColor;
  private TextBoxLineStyle m_lineStyle;
  private TextWrappingStyle m_wrapStyle;
  private float m_wrapDistanceBottom;
  private float m_wrapDistanceLeft = 9f;
  private float m_wrapDistanceRight = 9f;
  private float m_wrapDistanceTop;
  private float m_horPosition;
  private float m_verPosition;
  private int m_spid;
  internal float m_txbxLineWidth;
  private LineDashing m_lineDashing;
  private TextWrappingType m_wrappingType;
  private WrapMode m_wrapMode;
  private float m_txID;
  private byte m_bFlags = 72;
  private ShapeHorizontalAlignment m_horizAlignment;
  private ShapeVerticalAlignment m_verticalAlignment;
  private Syncfusion.DocIO.DLS.VerticalAlignment m_textVerticalAlignment;
  private InternalMargin m_intMargin;
  private Background m_background;
  private int m_orderIndex = int.MaxValue;
  private List<string> m_styleProps;
  private float m_widthRelPercent;
  private float m_heightRelPercent;
  private float m_horRelPercent = float.MinValue;
  private float m_verRelPercent = float.MinValue;
  private TextDirection m_textDirection;
  private Color m_textThemeColor;
  internal short WrapCollectionIndex = -1;
  private WrapPolygon m_wrapPolygon;
  private List<Stream> m_docxProps;
  private string m_name;
  private float m_rotation;
  private byte m_bflag;
  private string m_path;
  private float m_coordinateXOrigin;
  private float m_coordinateYOrigin;
  private string m_coordinateSize;
  private List<Path2D> m_vmlPathPoints;
  internal bool m_isVMLPathUpdated;

  internal bool IsWrappingBoundsAdded
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public WidthOrigin WidthOrigin
  {
    get => this.m_widthRelation;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.WidthOrigin = value;
      this.m_widthRelation = value;
    }
  }

  public HeightOrigin HeightOrigin
  {
    get => this.m_heightRelation;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.HeightOrigin = value;
      this.m_heightRelation = value;
    }
  }

  public HorizontalOrigin HorizontalOrigin
  {
    get => this.m_horizRelation;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.HorizontalOrigin = value;
      this.m_horizRelation = value;
    }
  }

  public VerticalOrigin VerticalOrigin
  {
    get => this.m_vertRelation;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.VerticalOrigin = value;
      this.m_vertRelation = value;
    }
  }

  public TextWrappingStyle TextWrappingStyle
  {
    get => this.m_wrapStyle;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.WrapFormat.TextWrappingStyle = value;
      this.m_wrapStyle = value;
      if (this.m_wrapStyle == TextWrappingStyle.Behind)
        this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | 1);
      else
        this.m_bFlags &= (byte) 254;
      if (this.m_wrapStyle != TextWrappingStyle.Inline)
        return;
      this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | 64 /*0x40*/);
    }
  }

  internal float WrapDistanceBottom
  {
    get
    {
      return (double) this.m_wrapDistanceBottom < 0.0 || (double) this.m_wrapDistanceBottom > 1584.0 ? 0.0f : this.m_wrapDistanceBottom;
    }
    set => this.m_wrapDistanceBottom = value;
  }

  internal float WrapDistanceLeft
  {
    get
    {
      return (double) this.m_wrapDistanceLeft < 0.0 || (double) this.m_wrapDistanceLeft > 1584.0 ? 0.0f : this.m_wrapDistanceLeft;
    }
    set => this.m_wrapDistanceLeft = value;
  }

  internal float WrapDistanceRight
  {
    get
    {
      return (double) this.m_wrapDistanceRight < 0.0 || (double) this.m_wrapDistanceRight > 1584.0 ? 0.0f : this.m_wrapDistanceRight;
    }
    set => this.m_wrapDistanceRight = value;
  }

  internal float WrapDistanceTop
  {
    get
    {
      return (double) this.m_wrapDistanceTop < 0.0 || (double) this.m_wrapDistanceTop > 1584.0 ? 0.0f : this.m_wrapDistanceTop;
    }
    set => this.m_wrapDistanceTop = value;
  }

  public Color FillColor
  {
    get => this.m_background != null ? this.m_background.Color : Color.White;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.FillFormat.Color = value;
      this.FillEfects.Color = value;
      this.FillEfects.Type = BackgroundType.Color;
    }
  }

  public TextBoxLineStyle LineStyle
  {
    get => this.m_lineStyle;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
      {
        switch (value)
        {
          case TextBoxLineStyle.Simple:
            (this.OwnerBase as WTextBox).Shape.LineFormat.Style = Syncfusion.DocIO.DLS.LineStyle.Single;
            break;
          case TextBoxLineStyle.Double:
            (this.OwnerBase as WTextBox).Shape.LineFormat.Style = Syncfusion.DocIO.DLS.LineStyle.ThinThin;
            break;
          case TextBoxLineStyle.ThickThin:
            (this.OwnerBase as WTextBox).Shape.LineFormat.Style = Syncfusion.DocIO.DLS.LineStyle.ThickThin;
            break;
          case TextBoxLineStyle.ThinThick:
            (this.OwnerBase as WTextBox).Shape.LineFormat.Style = Syncfusion.DocIO.DLS.LineStyle.ThinThick;
            break;
          case TextBoxLineStyle.Triple:
            (this.OwnerBase as WTextBox).Shape.LineFormat.Style = Syncfusion.DocIO.DLS.LineStyle.ThickBetweenThin;
            break;
        }
      }
      this.m_lineStyle = value;
    }
  }

  public float Width
  {
    get => this.m_width;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.Width = value;
      this.m_width = value;
    }
  }

  public float Height
  {
    get => this.m_height;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.Height = value;
      this.m_height = value;
    }
  }

  public Color LineColor
  {
    get => this.m_lineColor;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.LineFormat.Color = value;
      this.m_lineColor = value;
    }
  }

  public bool NoLine
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.LineFormat.Line = !value;
      this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
    }
  }

  internal WrapMode WrappingMode
  {
    get => this.m_wrapMode;
    set => this.m_wrapMode = value;
  }

  public float HorizontalPosition
  {
    get => this.m_horPosition;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.HorizontalPosition = value;
      this.m_horPosition = value;
    }
  }

  internal bool IsBelowText
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
      if (value && this.TextWrappingStyle == TextWrappingStyle.InFrontOfText)
      {
        this.m_wrapStyle = TextWrappingStyle.Behind;
      }
      else
      {
        if (value || this.TextWrappingStyle != TextWrappingStyle.Behind)
          return;
        this.m_wrapStyle = TextWrappingStyle.InFrontOfText;
      }
    }
  }

  public float VerticalPosition
  {
    get => this.m_verPosition;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.VerticalPosition = value;
      this.m_verPosition = value;
    }
  }

  public TextWrappingType TextWrappingType
  {
    get => this.m_wrappingType;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.WrapFormat.TextWrappingType = value;
      this.m_wrappingType = value;
    }
  }

  internal int TextBoxShapeID
  {
    get => this.m_spid;
    set => this.m_spid = value;
  }

  public float LineWidth
  {
    get => this.HasKeyValue(11) ? (float) this.PropertiesHash[11] : this.m_txbxLineWidth;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.LineFormat.Weight = value;
      this.m_txbxLineWidth = value;
      this.SetKeyValue(11, (object) value);
    }
  }

  public LineDashing LineDashing
  {
    get => this.m_lineDashing;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.LineFormat.DashStyle = value;
      this.m_lineDashing = value;
    }
  }

  public ShapeHorizontalAlignment HorizontalAlignment
  {
    get => this.m_horizAlignment;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.HorizontalAlignment = value;
      this.m_horizAlignment = value;
    }
  }

  public ShapeVerticalAlignment VerticalAlignment
  {
    get => this.m_verticalAlignment;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.VerticalAlignment = value;
      this.m_verticalAlignment = value;
    }
  }

  public Syncfusion.DocIO.DLS.VerticalAlignment TextVerticalAlignment
  {
    get => this.m_textVerticalAlignment;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.TextVerticalAlignment = value;
      this.m_textVerticalAlignment = value;
    }
  }

  internal float TextBoxIdentificator
  {
    get => this.m_txID;
    set => this.m_txID = value;
  }

  internal bool IsHeaderTextBox
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public InternalMargin InternalMargin
  {
    get
    {
      if (this.m_intMargin == null)
      {
        this.m_intMargin = new InternalMargin();
        this.m_intMargin.SetOwner(this.OwnerBase);
      }
      return this.m_intMargin;
    }
  }

  public float Rotation
  {
    get => this.m_rotation;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.Rotation = value;
      this.m_rotation = value;
    }
  }

  public bool FlipHorizontal
  {
    get => ((int) this.m_bflag & 16 /*0x10*/) >> 4 != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.FlipHorizontal = value;
      this.m_bflag = (byte) ((int) this.m_bflag & 239 | (value ? 1 : 0) << 4);
    }
  }

  public bool FlipVertical
  {
    get => ((int) this.m_bflag & 32 /*0x20*/) >> 5 != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.FlipVertical = value;
      this.m_bflag = (byte) ((int) this.m_bflag & 223 | (value ? 1 : 0) << 5);
    }
  }

  public Background FillEfects
  {
    get
    {
      if (this.m_background == null)
        this.m_background = new Background(this.Document, BackgroundType.NoBackground);
      return this.m_background;
    }
  }

  internal bool AllowInCell
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal int OrderIndex
  {
    get
    {
      if (this.m_orderIndex == int.MaxValue && this.Document != null && !this.Document.IsOpening && this.Document.Escher != null)
      {
        int shapeOrderIndex = this.Document.Escher.GetShapeOrderIndex(this.TextBoxShapeID);
        if (shapeOrderIndex != -1)
          this.m_orderIndex = shapeOrderIndex;
      }
      return this.m_orderIndex;
    }
    set => this.m_orderIndex = value;
  }

  internal List<string> DocxStyleProps
  {
    get
    {
      if (this.m_styleProps == null)
        this.m_styleProps = new List<string>();
      return this.m_styleProps;
    }
  }

  internal bool HasDocxProps => this.m_styleProps != null;

  public bool AutoFit
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
      {
        if (value)
        {
          (this.OwnerBase as WTextBox).Shape.TextFrame.ShapeAutoFit = true;
          (this.OwnerBase as WTextBox).Shape.TextFrame.NoAutoFit = false;
          (this.OwnerBase as WTextBox).Shape.TextFrame.NormalAutoFit = false;
        }
        else
        {
          (this.OwnerBase as WTextBox).Shape.TextFrame.NoAutoFit = true;
          (this.OwnerBase as WTextBox).Shape.TextFrame.ShapeAutoFit = false;
          (this.OwnerBase as WTextBox).Shape.TextFrame.NormalAutoFit = false;
        }
      }
      this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
    }
  }

  internal float WidthRelativePercent
  {
    get => this.m_widthRelPercent;
    set => this.m_widthRelPercent = value;
  }

  internal float HeightRelativePercent
  {
    get => this.m_heightRelPercent;
    set => this.m_heightRelPercent = value;
  }

  internal float HorizontalRelativePercent
  {
    get => this.m_horRelPercent;
    set => this.m_horRelPercent = value;
  }

  internal float VerticalRelativePercent
  {
    get => this.m_verRelPercent;
    set => this.m_verRelPercent = value;
  }

  public TextDirection TextDirection
  {
    get => this.m_textDirection;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.TextDirection = value;
      this.m_textDirection = value;
    }
  }

  internal Color TextThemeColor
  {
    get => this.m_textThemeColor;
    set => this.m_textThemeColor = value;
  }

  public bool AllowOverlap
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set
    {
      if (this.TextWrappingStyle == TextWrappingStyle.Inline)
        return;
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.WrapFormat.AllowOverlap = value;
      this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
    }
  }

  internal WrapPolygon WrapPolygon
  {
    get
    {
      if (this.m_wrapPolygon == null)
      {
        this.m_wrapPolygon = new WrapPolygon();
        this.m_wrapPolygon.Edited = false;
        this.m_wrapPolygon.Vertices.Add(new PointF(0.0f, 0.0f));
        this.m_wrapPolygon.Vertices.Add(new PointF(0.0f, 21600f));
        this.m_wrapPolygon.Vertices.Add(new PointF(21600f, 21600f));
        this.m_wrapPolygon.Vertices.Add(new PointF(21600f, 0.0f));
        this.m_wrapPolygon.Vertices.Add(new PointF(0.0f, 0.0f));
      }
      return this.m_wrapPolygon;
    }
    set => this.m_wrapPolygon = value;
  }

  internal List<Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new List<Stream>();
      return this.m_docxProps;
    }
  }

  internal string Path
  {
    get => this.m_path;
    set => this.m_path = value;
  }

  internal string CoordinateSize
  {
    get => this.m_coordinateSize;
    set => this.m_coordinateSize = value;
  }

  internal float CoordinateXOrigin
  {
    get => this.m_coordinateXOrigin;
    set => this.m_coordinateXOrigin = value;
  }

  internal float CoordinateYOrigin
  {
    get => this.m_coordinateYOrigin;
    set => this.m_coordinateYOrigin = value;
  }

  internal List<Path2D> VMLPathPoints
  {
    get => this.m_vmlPathPoints;
    set => this.m_vmlPathPoints = value;
  }

  public WTextBoxFormat(WordDocument doc)
    : base((IWordDocument) doc, true)
  {
    this.m_wrapStyle = TextWrappingStyle.InFrontOfText;
    this.m_fillColor = Color.White;
    this.m_lineColor = Color.Black;
    this.m_lineStyle = TextBoxLineStyle.Simple;
    this.m_horizRelation = HorizontalOrigin.Column;
    this.m_vertRelation = VerticalOrigin.Paragraph;
    this.m_txbxLineWidth = 0.75f;
    this.m_lineDashing = LineDashing.Solid;
    this.m_wrapMode = WrapMode.Square;
    this.m_horizAlignment = ShapeHorizontalAlignment.None;
    this.m_verticalAlignment = ShapeVerticalAlignment.None;
    this.m_textVerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Top;
    this.m_background = new Background(doc, BackgroundType.NoBackground);
  }

  public override void ClearFormatting()
  {
    this.SetTextWrappingStyleValue(TextWrappingStyle.InFrontOfText);
    this.FillColor = Color.White;
    this.LineColor = Color.Black;
    this.LineStyle = TextBoxLineStyle.Simple;
    this.HorizontalOrigin = HorizontalOrigin.Column;
    this.VerticalOrigin = VerticalOrigin.Paragraph;
    this.LineWidth = 0.75f;
    this.LineDashing = LineDashing.Solid;
    this.WrappingMode = WrapMode.Square;
    this.HorizontalAlignment = ShapeHorizontalAlignment.None;
    this.VerticalAlignment = ShapeVerticalAlignment.None;
    this.TextVerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Top;
    this.m_background = new Background(this.Document, BackgroundType.NoBackground);
    if (this.DocxProps != null && this.DocxProps.Count > 0)
      this.DocxProps.Clear();
    if (this.DocxStyleProps != null && this.DocxStyleProps.Count > 0)
      this.DocxStyleProps.Clear();
    if (this.OwnerBase == null || !(this.OwnerBase is WTextBox))
      return;
    (this.OwnerBase as WTextBox).IsShape = false;
  }

  protected override object GetDefValue(int key) => (object) null;

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("FillColor"))
      this.FillColor = reader.ReadColor("FillColor");
    if (reader.HasAttribute("Height"))
      this.Height = reader.ReadFloat("Height");
    if (reader.HasAttribute("HorizontalOrigin"))
      this.HorizontalOrigin = (HorizontalOrigin) reader.ReadEnum("HorizontalOrigin", typeof (HorizontalOrigin));
    if (reader.HasAttribute("LineStyle"))
      this.LineStyle = (TextBoxLineStyle) reader.ReadEnum("LineStyle", typeof (TextBoxLineStyle));
    if (reader.HasAttribute("WrappingStyle"))
      this.SetTextWrappingStyleValue((TextWrappingStyle) reader.ReadEnum("WrappingStyle", typeof (TextWrappingStyle)));
    if (reader.HasAttribute("VerticalOrigin"))
      this.VerticalOrigin = (VerticalOrigin) reader.ReadEnum("VerticalOrigin", typeof (VerticalOrigin));
    if (reader.HasAttribute("Width"))
      this.Width = reader.ReadFloat("Width");
    if (reader.HasAttribute("LineColor"))
      this.LineColor = reader.ReadColor("LineColor");
    if (reader.HasAttribute("HorizontalPosition"))
      this.HorizontalPosition = reader.ReadFloat("HorizontalPosition");
    if (reader.HasAttribute("LineDashing"))
      this.LineDashing = (LineDashing) reader.ReadEnum("LineDashing", typeof (LineDashing));
    if (reader.HasAttribute("LineWidth"))
      this.LineWidth = reader.ReadFloat("LineWidth");
    if (reader.HasAttribute("VerticalPosition"))
      this.VerticalPosition = reader.ReadFloat("VerticalPosition");
    if (reader.HasAttribute("WrappingMode"))
      this.WrappingMode = (WrapMode) reader.ReadEnum("WrappingMode", typeof (WrapMode));
    if (reader.HasAttribute("WrappingType"))
      this.TextWrappingType = (TextWrappingType) reader.ReadEnum("WrappingType", typeof (TextWrappingType));
    if (reader.HasAttribute("IsBelowText"))
      this.IsBelowText = reader.ReadBoolean("IsBelowText");
    if (reader.HasAttribute("NoLine"))
      this.NoLine = reader.ReadBoolean("NoLine");
    if (reader.HasAttribute("NoFill"))
      this.FillColor = Color.Empty;
    if (reader.HasAttribute("HorizontalAlignment"))
      this.HorizontalAlignment = (ShapeHorizontalAlignment) reader.ReadEnum("HorizontalAlignment", typeof (ShapeHorizontalAlignment));
    if (reader.HasAttribute("VerticalAlignment"))
      this.VerticalAlignment = (ShapeVerticalAlignment) reader.ReadEnum("VerticalAlignment", typeof (ShapeVerticalAlignment));
    if (reader.HasAttribute("ShapeID"))
      this.TextBoxShapeID = reader.ReadInt("ShapeID");
    if (!reader.HasAttribute("IsHeader"))
      return;
    this.IsHeaderTextBox = reader.ReadBoolean("IsHeader");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.FillColor == Color.Empty)
      writer.WriteValue("NoFill", true);
    else if (this.FillColor != Color.White)
      writer.WriteValue("FillColor", this.FillColor);
    if ((double) this.Height != 0.0)
      writer.WriteValue("Height", this.Height);
    if (this.HorizontalOrigin != HorizontalOrigin.Column)
      writer.WriteValue("HorizontalOrigin", (Enum) this.HorizontalOrigin);
    if (this.LineStyle != TextBoxLineStyle.Simple)
      writer.WriteValue("LineStyle", (Enum) this.LineStyle);
    if (this.TextWrappingStyle != TextWrappingStyle.Square)
      writer.WriteValue("WrappingStyle", (Enum) this.TextWrappingStyle);
    if (this.VerticalOrigin != VerticalOrigin.Paragraph)
      writer.WriteValue("VerticalOrigin", (Enum) this.VerticalOrigin);
    if ((double) this.Width != 0.0)
      writer.WriteValue("Width", this.Width);
    if (this.LineColor != Color.Black)
      writer.WriteValue("LineColor", this.LineColor);
    if ((double) this.HorizontalPosition != 0.0)
      writer.WriteValue("HorizontalPosition", this.HorizontalPosition);
    if (this.LineDashing != LineDashing.Solid)
      writer.WriteValue("LineDashing", (Enum) this.LineDashing);
    if ((double) this.LineWidth != 0.75)
      writer.WriteValue("LineWidth", this.LineWidth);
    if ((double) this.VerticalPosition != 0.0)
      writer.WriteValue("VerticalPosition", this.VerticalPosition);
    if (this.WrappingMode != WrapMode.None)
      writer.WriteValue("WrappingMode", (Enum) this.WrappingMode);
    if (this.TextWrappingType != TextWrappingType.Both)
      writer.WriteValue("WrappingType", (Enum) this.TextWrappingType);
    if (this.IsBelowText)
      writer.WriteValue("IsBelowText", this.IsBelowText);
    if (this.NoLine)
      writer.WriteValue("NoLine", this.NoLine);
    if (this.HorizontalAlignment != ShapeHorizontalAlignment.None)
      writer.WriteValue("HorizontalAlignment", (Enum) this.HorizontalAlignment);
    if (this.VerticalAlignment != ShapeVerticalAlignment.None)
      writer.WriteValue("VerticalAlignment", (Enum) this.VerticalAlignment);
    if (this.TextBoxShapeID != 0)
      writer.WriteValue("ShapeID", this.TextBoxShapeID);
    if (!this.IsHeaderTextBox)
      return;
    writer.WriteValue("IsHeader", this.IsHeaderTextBox);
  }

  internal bool HasKeyValue(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  internal void SetKeyValue(int propKey, object value) => this[propKey] = value;

  internal override void Close()
  {
    base.Close();
    if (this.m_background != null)
    {
      this.m_background.Close();
      this.m_background = (Background) null;
    }
    if (this.m_styleProps != null)
    {
      this.m_styleProps.Clear();
      this.m_styleProps = (List<string>) null;
    }
    if (this.m_docxProps == null)
      return;
    foreach (Stream docxProp in this.m_docxProps)
      docxProp.Close();
    this.m_docxProps.Clear();
    this.m_docxProps = (List<Stream>) null;
  }

  public WTextBoxFormat Clone()
  {
    WTextBoxFormat wtextBoxFormat = (WTextBoxFormat) this.MemberwiseClone();
    if (this.WrapPolygon != null)
      wtextBoxFormat.WrapPolygon = this.WrapPolygon.Clone();
    if (this.m_intMargin != null)
      wtextBoxFormat.m_intMargin = this.m_intMargin.Clone();
    if (this.m_background != null)
      wtextBoxFormat.m_background = this.m_background.Clone();
    return wtextBoxFormat;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (this.m_background == null)
      return;
    this.m_background.UpdateImageRecord(doc);
  }

  internal void UpdateFillEffects(MsofbtSpContainer container, WordDocument doc)
  {
    this.m_background = new Background(doc, container);
  }

  internal void SetTextWrappingStyleValue(TextWrappingStyle textWrappingStyle)
  {
    this.m_wrapStyle = textWrappingStyle;
  }
}
