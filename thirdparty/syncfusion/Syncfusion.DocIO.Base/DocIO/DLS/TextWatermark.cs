// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextWatermark
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextWatermark : Watermark
{
  private string m_text = string.Empty;
  private string m_fontName = "Times New Roman";
  private float m_fontSize = 36f;
  private Color m_fontColor = Color.Gray;
  private byte m_bFlags = 1;
  private WatermarkLayout m_layout;
  private float m_shapeHeigh = -1f;
  private float m_shapeWidth = -1f;
  private TextWrappingStyle m_wrappingStyle = TextWrappingStyle.Behind;
  private HorizontalOrigin m_horizontalOrgin;
  private float m_horizontalPosition;
  private ShapeHorizontalAlignment m_horizontalAlignement;
  private VerticalOrigin m_verticalOrgin;
  private float m_verticalPosition;
  private ShapeVerticalAlignment m_verticalAlignement;
  private int m_rotation;
  private ShapePosition m_position;

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  public string FontName
  {
    get => this.m_fontName;
    set
    {
      this.m_fontName = value;
      this.m_shapeHeigh = -1f;
      this.m_shapeWidth = -1f;
    }
  }

  public float Size
  {
    get => this.m_fontSize;
    set
    {
      this.m_fontSize = value;
      this.m_shapeHeigh = -1f;
      this.m_shapeWidth = -1f;
    }
  }

  public Color Color
  {
    get => this.m_fontColor;
    set => this.m_fontColor = value;
  }

  public bool Semitransparent
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public WatermarkLayout Layout
  {
    get => this.m_layout;
    set
    {
      this.m_layout = value;
      if (this.Document != null && this.Document.IsOpening)
        return;
      if (this.m_layout == WatermarkLayout.Diagonal)
        this.m_rotation = 315;
      else
        this.m_rotation = 0;
    }
  }

  internal float Height
  {
    get => this.m_shapeHeigh;
    set => this.m_shapeHeigh = value;
  }

  internal float Width
  {
    get => this.m_shapeWidth;
    set => this.m_shapeWidth = value;
  }

  internal SizeF ShapeSize => this.GetShapeSizeValue();

  internal TextWrappingStyle TextWrappingStyle
  {
    get => this.m_wrappingStyle;
    set => this.m_wrappingStyle = value;
  }

  internal HorizontalOrigin HorizontalOrigin
  {
    get => this.m_horizontalOrgin;
    set => this.m_horizontalOrgin = value;
  }

  internal VerticalOrigin VerticalOrigin
  {
    get => this.m_verticalOrgin;
    set => this.m_verticalOrgin = value;
  }

  internal ShapeHorizontalAlignment HorizontalAlignment
  {
    get => this.m_horizontalAlignement;
    set => this.m_horizontalAlignement = value;
  }

  internal ShapeVerticalAlignment VerticalAlignment
  {
    get => this.m_verticalAlignement;
    set => this.m_verticalAlignement = value;
  }

  internal float HorizontalPosition
  {
    get => this.m_horizontalPosition;
    set => this.m_horizontalPosition = value;
  }

  internal float VerticalPosition
  {
    get => this.m_verticalPosition;
    set => this.m_verticalPosition = value;
  }

  internal int Rotation
  {
    get => this.m_rotation;
    set => this.m_rotation = value;
  }

  internal ShapePosition Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public TextWatermark()
    : base(WatermarkType.TextWatermark)
  {
    this.SetDefaultValues();
  }

  public TextWatermark(string text)
    : base(WatermarkType.TextWatermark)
  {
    this.m_text = text;
    this.SetDefaultValues();
  }

  public TextWatermark(string text, string fontName, int fontSize, WatermarkLayout layout)
    : base(WatermarkType.TextWatermark)
  {
    this.m_text = text;
    this.m_fontName = fontName;
    this.m_fontSize = (float) fontSize;
    this.SetDefaultValues();
    this.Layout = layout;
  }

  internal TextWatermark(WordDocument doc)
    : base(doc, WatermarkType.TextWatermark)
  {
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Text"))
      this.m_text = reader.ReadString("Text");
    if (reader.HasAttribute("TextFontName"))
      this.m_fontName = reader.ReadString("TextFontName");
    if (reader.HasAttribute("TextFontSize"))
      this.m_fontSize = reader.ReadFloat("TextFontSize");
    if (reader.HasAttribute("TextLayout"))
      this.m_layout = (WatermarkLayout) reader.ReadEnum("TextLayout", typeof (WatermarkLayout));
    if (reader.HasAttribute("Semitransparent"))
      this.Semitransparent = reader.ReadBoolean("Semitransparent");
    if (reader.HasAttribute("TextFontColor"))
      this.m_fontColor = reader.ReadColor("TextFontColor");
    if (reader.HasAttribute("ShapeHeight"))
      this.m_shapeHeigh = (float) reader.ReadInt("ShapeHeight");
    if (!reader.HasAttribute("ShapeWidth"))
      return;
    this.m_shapeWidth = (float) reader.ReadInt("ShapeWidth");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("Text", this.m_text);
    writer.WriteValue("TextFontName", this.m_fontName);
    writer.WriteValue("TextFontSize", this.m_fontSize);
    writer.WriteValue("TextLayout", (Enum) this.m_layout);
    if (this.m_fontColor != Color.Gray)
      writer.WriteValue("TextFontColor", this.m_fontColor);
    if (!this.Semitransparent)
      writer.WriteValue("Semitransparent", this.Semitransparent);
    if ((double) this.m_shapeHeigh != 0.0)
      writer.WriteValue("ShapeHeight", this.m_shapeHeigh);
    if ((double) this.m_shapeWidth == 0.0)
      return;
    writer.WriteValue("ShapeWidth", this.m_shapeWidth);
  }

  private void SetDefaultValues()
  {
    this.Rotation = 315;
    this.Position = ShapePosition.Absolute;
    this.HorizontalAlignment = ShapeHorizontalAlignment.Center;
    this.VerticalAlignment = ShapeVerticalAlignment.Center;
  }

  internal void SetDefaultSize()
  {
    SizeF shapeSize = this.ShapeSize;
    if ((double) this.Width == -1.0)
      this.Width = (float) Math.Round((double) shapeSize.Width * 0.69340002536773682, 0);
    if ((double) this.Height != -1.0)
      return;
    this.Height = (float) Math.Round((double) shapeSize.Height * 0.67000001668930054, 0);
  }

  private SizeF GetShapeSizeValue()
  {
    return UnitsConvertor.Instance.EmptyGraphics.MeasureString(this.m_text, this.Document.FontSettings.GetFont(this.m_fontName, this.m_fontSize, FontStyle.Regular));
  }
}
