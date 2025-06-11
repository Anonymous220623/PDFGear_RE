// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.LineFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class LineFormat
{
  internal const byte LineJoinKey = 0;
  internal const byte LineCapKey = 1;
  internal const byte BeginArrowheadLengthKey = 2;
  internal const byte BeginArrowheadStyleKey = 3;
  internal const byte BeginArrowheadWidthKey = 4;
  internal const byte DashStyleKey = 5;
  internal const byte EndArrowheadLengthKey = 6;
  internal const byte EndArrowheadStyleKey = 7;
  internal const byte EndArrowheadWidthKey = 8;
  internal const byte InsertPenKey = 9;
  internal const byte StyleKey = 10;
  internal const byte LineWeightKey = 11;
  internal const byte ColorKey = 12;
  internal const byte LineKey = 13;
  private Color m_BackColor;
  private LineEndLength m_BeginArrowheadLength;
  private ArrowheadStyle m_BeginArrowheadStyle;
  private LineEndWidth m_BeginArrowheadWidth;
  private LineDashing m_DashStyle;
  private LineEndLength m_EndArrowheadLength;
  private ArrowheadStyle m_EndArrowheadStyle;
  private LineEndWidth m_EndArrowheadWidth;
  private bool m_InsetPen;
  private LineStyle m_Style;
  private float m_Transparency;
  internal float m_Weight;
  private bool m_Line;
  private LineCap m_LineCap;
  private GradientFill m_GradientFill;
  private LineFormatType m_LineFormatType;
  private LineJoin m_LineJoin;
  private PatternType m_Pattern = PatternType.Mixed;
  private Color m_ForeColor;
  private ImageRecord m_ImageRecord;
  private string m_miterJoinLimit;
  private List<DictionaryEntry> m_lineSchemeColor;
  internal ushort m_flag;
  private ChildShape m_childShape;
  private Entity m_shape;
  internal Dictionary<string, Stream> m_docxProps;
  internal byte m_flagA;
  internal Dictionary<int, object> m_propertiesHash;

  internal ImageRecord ImageRecord
  {
    get => this.m_ImageRecord;
    set => this.m_ImageRecord = value;
  }

  internal Color ForeColor
  {
    get => this.m_ForeColor;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 57343 /*0xDFFF*/ | 8192 /*0x2000*/);
      this.m_ForeColor = value;
    }
  }

  internal PatternType Pattern
  {
    get => this.m_Pattern;
    set => this.m_Pattern = value;
  }

  internal LineJoin LineJoin
  {
    get => this.HasKeyValue(0) ? (LineJoin) this.PropertiesHash[0] : this.m_LineJoin;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65534 | 1);
      this.m_LineJoin = value;
      this.SetKeyValue(0, (object) value);
    }
  }

  internal LineFormatType LineFormatType
  {
    get => this.m_LineFormatType;
    set => this.m_LineFormatType = value;
  }

  internal GradientFill GradientFill
  {
    get
    {
      if (this.m_GradientFill == null)
        this.m_GradientFill = new GradientFill();
      return this.m_GradientFill;
    }
    set => this.m_GradientFill = value;
  }

  internal LineCap LineCap
  {
    get => this.m_LineCap;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65533 | 2);
      this.m_LineCap = value;
    }
  }

  public bool Line
  {
    get => this.HasKeyValue(13) ? (bool) this.PropertiesHash[13] : this.m_Line;
    set
    {
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsLineStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsLineStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsLineStyleInline = true;
      this.m_Line = value;
      this.SetKeyValue(13, (object) value);
    }
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }

  public Color Color
  {
    get => this.m_BackColor;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 61439 /*0xEFFF*/ | 4096 /*0x1000*/);
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsLineStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsLineStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsLineStyleInline = true;
      this.m_BackColor = value;
    }
  }

  internal LineEndLength BeginArrowheadLength
  {
    get => this.m_BeginArrowheadLength;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65531 | 4);
      this.m_BeginArrowheadLength = value;
    }
  }

  public ArrowheadStyle BeginArrowheadStyle
  {
    get => this.m_BeginArrowheadStyle;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65527 | 8);
      this.m_BeginArrowheadStyle = value;
    }
  }

  internal LineEndWidth BeginArrowheadWidth
  {
    get => this.m_BeginArrowheadWidth;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65519 | 16 /*0x10*/);
      this.m_BeginArrowheadWidth = value;
    }
  }

  public LineDashing DashStyle
  {
    get => this.m_DashStyle;
    set
    {
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsLineStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsLineStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsLineStyleInline = true;
      this.m_flag = (ushort) ((int) this.m_flag & 65503 | 32 /*0x20*/);
      this.m_DashStyle = value;
    }
  }

  internal LineEndLength EndArrowheadLength
  {
    get => this.m_EndArrowheadLength;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65471 | 64 /*0x40*/);
      this.m_EndArrowheadLength = value;
    }
  }

  public ArrowheadStyle EndArrowheadStyle
  {
    get => this.m_EndArrowheadStyle;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65407 | 128 /*0x80*/);
      this.m_EndArrowheadStyle = value;
    }
  }

  internal LineEndWidth EndArrowheadWidth
  {
    get => this.m_EndArrowheadWidth;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65279 | 256 /*0x0100*/);
      this.m_EndArrowheadWidth = value;
    }
  }

  internal bool InsetPen
  {
    get => this.m_InsetPen;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65023 | 512 /*0x0200*/);
      this.m_InsetPen = value;
    }
  }

  public LineStyle Style
  {
    get => this.m_Style;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 64511 | 1024 /*0x0400*/);
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsLineStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsLineStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsLineStyleInline = true;
      this.m_Style = value;
    }
  }

  public float Transparency
  {
    get => this.m_Transparency;
    set
    {
      this.m_Transparency = value;
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsLineStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsLineStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsLineStyleInline = true;
      this.m_Transparency = value;
    }
  }

  public float Weight
  {
    get => this.HasKeyValue(11) ? (float) this.PropertiesHash[11] : this.m_Weight;
    set
    {
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsLineStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsLineStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsLineStyleInline = true;
      this.m_flag = (ushort) ((int) this.m_flag & 63487 | 2048 /*0x0800*/);
      this.m_Weight = value;
      this.SetKeyValue(11, (object) value);
    }
  }

  internal bool Is2007StrokeDefined
  {
    get => ((int) this.m_flagA & 1) != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 254 | (value ? 1 : 0));
  }

  internal string MiterJoinLimit
  {
    get => this.m_miterJoinLimit;
    set => this.m_miterJoinLimit = value;
  }

  internal List<DictionaryEntry> LineSchemeColorTransforms
  {
    get
    {
      if (this.m_lineSchemeColor == null)
        this.m_lineSchemeColor = new List<DictionaryEntry>();
      return this.m_lineSchemeColor;
    }
    set => this.m_lineSchemeColor = value;
  }

  public LineFormat(Shape shape)
    : this((ShapeBase) shape)
  {
  }

  internal LineFormat(ShapeBase shape)
  {
    this.m_shape = (Entity) shape;
    this.m_BackColor = Color.Black;
    this.m_DashStyle = LineDashing.Solid;
    this.m_Line = true;
    this.m_Style = LineStyle.Single;
    this.m_Transparency = 0.0f;
    this.m_Weight = 1f;
    this.m_LineJoin = LineJoin.Miter;
    this.m_LineCap = LineCap.Flat;
    this.m_BeginArrowheadLength = LineEndLength.MediumLenArrow;
    this.m_BeginArrowheadWidth = LineEndWidth.MediumWidthArrow;
    this.m_EndArrowheadLength = LineEndLength.MediumLenArrow;
    this.m_EndArrowheadWidth = LineEndWidth.MediumWidthArrow;
    this.LineFormatChanged();
  }

  internal LineFormat(ChildShape shape)
  {
    this.m_childShape = shape;
    this.m_BackColor = Color.Black;
    this.m_DashStyle = LineDashing.Solid;
    this.m_Line = true;
    this.m_Style = LineStyle.Single;
    this.m_Transparency = 0.0f;
    this.m_Weight = 1f;
    this.m_LineJoin = LineJoin.Miter;
    this.m_LineCap = LineCap.Flat;
    this.m_BeginArrowheadLength = LineEndLength.MediumLenArrow;
    this.m_BeginArrowheadWidth = LineEndWidth.MediumWidthArrow;
    this.m_EndArrowheadLength = LineEndLength.MediumLenArrow;
    this.m_EndArrowheadWidth = LineEndWidth.MediumWidthArrow;
    this.LineFormatChanged();
  }

  private void LineFormatChanged()
  {
    if (this.DocxProps.ContainsKey("gradFill"))
      this.DocxProps.Remove("gradFill");
    if (this.DocxProps.ContainsKey("pattFill"))
      this.DocxProps.Remove("pattFill");
    if (!(this.m_shape is Shape))
      return;
    if (this.m_shape is Shape && (this.m_shape as Shape).Docx2007Props.ContainsKey("stroke"))
      (this.m_shape as Shape).Docx2007Props.Remove("stroke");
    this.m_Line = true;
  }

  internal bool HasKeyValue(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  internal void SetKeyValue(int propKey, object value) => this[propKey] = value;

  internal void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_GradientFill != null)
    {
      this.m_GradientFill.Close();
      this.m_GradientFill = (GradientFill) null;
    }
    if (this.m_ImageRecord != null)
    {
      this.m_ImageRecord.Close();
      this.m_ImageRecord = (ImageRecord) null;
    }
    if (this.m_lineSchemeColor != null)
    {
      this.m_lineSchemeColor.Clear();
      this.m_lineSchemeColor = (List<DictionaryEntry>) null;
    }
    if (this.m_docxProps == null)
      return;
    foreach (Stream stream in this.m_docxProps.Values)
      stream.Close();
    this.m_docxProps.Clear();
    this.m_docxProps = (Dictionary<string, Stream>) null;
  }

  internal bool HasKey(int propertyKey)
  {
    return ((int) this.m_flag & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  internal LineFormat Clone()
  {
    LineFormat lineFormat = (LineFormat) this.MemberwiseClone();
    if (this.GradientFill != null && this.GradientFill.GradientStops != null && this.GradientFill.GradientStops.Count > 0)
      lineFormat.GradientFill = this.GradientFill.Clone();
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
      (this.m_shape != null ? this.m_shape.Document : (this.m_childShape != null ? this.m_childShape.Document : (WordDocument) null))?.CloneProperties(this.DocxProps, ref lineFormat.m_docxProps);
    return lineFormat;
  }
}
