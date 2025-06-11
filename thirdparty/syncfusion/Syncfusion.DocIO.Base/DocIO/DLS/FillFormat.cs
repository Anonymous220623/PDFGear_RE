// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FillFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class FillFormat
{
  internal const byte IsDefaultFillKey = 0;
  internal const byte IsDefaultFillColorKey = 1;
  private Color m_BackColor;
  private Color m_ForeColor;
  private Color m_recolorTarget;
  private PatternType m_Pattern = PatternType.Mixed;
  private TextureAlignment m_TextureAlignment;
  private double m_TextureHorizontalScale;
  private double m_TextureOffsetX;
  private double m_TextureOffsetY;
  private double m_TextureVerticalScale;
  private float m_Transparency;
  private FillType m_FillType;
  private ImageRecord m_ImageRecord;
  private FlipOrientation m_FlipOrientation;
  private TileRectangle m_SourceRectangle;
  private TileRectangle m_FillRectangle;
  private GradientFill m_GradientFill;
  private BlipCompressionType m_compressionMode;
  private BlipFormat m_blipFormat;
  private string m_alternateHRef;
  private float m_angle;
  private FillAspect m_fillAspect;
  private float m_focus;
  private float m_focusPositionX;
  private float m_focusPositionY;
  private float m_positionX;
  private float m_positionY;
  private float m_focusSizeX;
  private float m_focusSizeY;
  private float m_secondaryOpacity;
  private byte m_flagA;
  private List<DictionaryEntry> m_fillSchemeColor;
  private ShapeBase m_shape;
  private ChildShape m_childShape;
  private float m_contrast;
  private byte m_bFlags = 11;
  private WPicture m_picture;

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

  internal TileRectangle FillRectangle
  {
    get
    {
      if (this.m_FillRectangle == null)
        this.m_FillRectangle = new TileRectangle();
      return this.m_FillRectangle;
    }
    set => this.m_FillRectangle = value;
  }

  internal TileRectangle SourceRectangle
  {
    get
    {
      if (this.m_SourceRectangle == null)
        this.m_SourceRectangle = new TileRectangle();
      return this.m_SourceRectangle;
    }
    set => this.m_SourceRectangle = value;
  }

  internal FlipOrientation FlipOrientation
  {
    get => this.m_FlipOrientation;
    set => this.m_FlipOrientation = value;
  }

  internal ImageRecord ImageRecord
  {
    get => this.m_ImageRecord;
    set => this.m_ImageRecord = value;
  }

  internal bool IsDefaultFill
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsDefaultFillColor
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool Fill
  {
    get => ((int) this.m_flagA & 1) != 0;
    set
    {
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsFillStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsFillStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsFillStyleInline = true;
      this.m_flagA = (byte) ((int) this.m_flagA & 254 | (value ? 1 : 0));
      this.FillFormatChanged();
    }
  }

  public Color Color
  {
    get => this.m_BackColor;
    set
    {
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsFillStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsFillStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsFillStyleInline = true;
      this.m_BackColor = value;
      this.FillFormatChanged();
    }
  }

  internal Color ReColorTarget
  {
    get => this.m_recolorTarget;
    set => this.m_recolorTarget = value;
  }

  internal Color ForeColor
  {
    get => this.m_ForeColor;
    set => this.m_ForeColor = value;
  }

  internal PatternType Pattern
  {
    get => this.m_Pattern;
    set => this.m_Pattern = value;
  }

  internal bool RotateWithObject
  {
    get => ((int) this.m_flagA & 2) >> 1 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 253 | (value ? 1 : 0) << 1);
  }

  internal TextureAlignment TextureAlignment
  {
    get => this.m_TextureAlignment;
    set => this.m_TextureAlignment = value;
  }

  internal double TextureHorizontalScale
  {
    get => this.m_TextureHorizontalScale;
    set => this.m_TextureHorizontalScale = value;
  }

  internal double TextureOffsetX
  {
    get => this.m_TextureOffsetX;
    set => this.m_TextureOffsetX = value;
  }

  internal double TextureOffsetY
  {
    get => this.m_TextureOffsetY;
    set => this.m_TextureOffsetY = value;
  }

  internal bool TextureTile
  {
    get => ((int) this.m_flagA & 4) >> 2 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 251 | (value ? 1 : 0) << 2);
  }

  internal double TextureVerticalScale
  {
    get => this.m_TextureVerticalScale;
    set => this.m_TextureVerticalScale = value;
  }

  public float Transparency
  {
    get => this.m_Transparency;
    set
    {
      if (this.m_shape != null && this.m_shape.Document != null && !this.m_shape.Document.IsOpening)
      {
        if (this.m_shape is Shape)
          (this.m_shape as Shape).IsFillStyleInline = true;
        else if (this.m_shape is GroupShape)
          (this.m_shape as GroupShape).IsFillStyleInline = true;
      }
      else if (this.m_childShape != null && this.m_childShape.Document != null && !this.m_childShape.Document.IsOpening)
        this.m_childShape.IsFillStyleInline = true;
      this.m_Transparency = value;
      this.FillFormatChanged();
    }
  }

  internal float Contrast
  {
    get => this.m_contrast;
    set => this.m_contrast = value;
  }

  internal FillType FillType
  {
    get => this.m_FillType;
    set => this.m_FillType = value;
  }

  internal BlipCompressionType BlipCompressionMode
  {
    get => this.m_compressionMode;
    set => this.m_compressionMode = value;
  }

  internal BlipFormat BlipFormat
  {
    get
    {
      if (this.m_blipFormat == null)
        this.m_blipFormat = new BlipFormat(this.m_shape);
      return this.m_blipFormat;
    }
    set => this.m_blipFormat = value;
  }

  internal bool AlignWithShape
  {
    get => ((int) this.m_flagA & 8) >> 3 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 247 | (value ? 1 : 0) << 3);
  }

  internal bool DetectMouseClick
  {
    get => ((int) this.m_flagA & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 239 | (value ? 1 : 0) << 4);
  }

  internal bool ReColor
  {
    get => ((int) this.m_flagA & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 223 | (value ? 1 : 0) << 5);
  }

  internal string AlternateHRef
  {
    get => this.m_alternateHRef;
    set => this.m_alternateHRef = value;
  }

  internal float Angle
  {
    get => this.m_angle;
    set => this.m_angle = value;
  }

  internal FillAspect Aspect
  {
    get => this.m_fillAspect;
    set => this.m_fillAspect = value;
  }

  internal float Focus
  {
    get => this.m_focus;
    set => this.m_focus = value;
  }

  internal float FocusPositionX
  {
    get => this.m_focusPositionX;
    set => this.m_focusPositionX = value;
  }

  internal float FocusPositionY
  {
    get => this.m_focusPositionY;
    set => this.m_focusPositionY = value;
  }

  internal float PositionX
  {
    get => this.m_positionX;
    set => this.m_positionX = value;
  }

  internal float PositionY
  {
    get => this.m_positionY;
    set => this.m_positionY = value;
  }

  internal float FocusSizeX
  {
    get => this.m_focusSizeX;
    set => this.m_focusSizeX = value;
  }

  internal float FocusSizeY
  {
    get => this.m_focusSizeY;
    set => this.m_focusSizeY = value;
  }

  internal float SecondaryOpacity
  {
    get => this.m_secondaryOpacity;
    set => this.m_secondaryOpacity = value;
  }

  internal bool Visible
  {
    get => ((int) this.m_flagA & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 191 | (value ? 1 : 0) << 6);
  }

  internal List<DictionaryEntry> FillSchemeColorTransforms
  {
    get
    {
      if (this.m_fillSchemeColor == null)
        this.m_fillSchemeColor = new List<DictionaryEntry>();
      return this.m_fillSchemeColor;
    }
    set => this.m_fillSchemeColor = value;
  }

  public FillFormat(Shape shape)
    : this((ShapeBase) shape)
  {
  }

  internal FillFormat(ShapeBase shape)
  {
    this.m_shape = shape;
    this.m_flagA = (byte) 1;
    this.Visible = true;
    this.m_FillType = FillType.FillSolid;
    this.m_BackColor = Color.White;
    this.FillFormatChanged();
  }

  internal FillFormat(ChildShape shape)
  {
    this.m_childShape = shape;
    this.Visible = true;
    if (this.m_childShape.DocxProps.ContainsKey("gradFill"))
      this.m_childShape.DocxProps.Remove("gradFill");
    if (this.m_childShape.DocxProps.ContainsKey("blipFill"))
      this.m_childShape.DocxProps.Remove("blipFill");
    if (this.m_childShape.DocxProps.ContainsKey("pattFill"))
      this.m_childShape.DocxProps.Remove("pattFill");
    if (!this.m_childShape.Docx2007Props.ContainsKey("fill"))
      return;
    this.m_childShape.Docx2007Props.Remove("fill");
  }

  internal FillFormat(WPicture picture) => this.m_picture = picture;

  private void FillFormatChanged()
  {
    if (this.m_shape is Shape)
    {
      if ((this.m_shape as Shape).DocxProps.ContainsKey("gradFill"))
        (this.m_shape as Shape).DocxProps.Remove("gradFill");
      if ((this.m_shape as Shape).DocxProps.ContainsKey("blipFill"))
        (this.m_shape as Shape).DocxProps.Remove("blipFill");
      if ((this.m_shape as Shape).DocxProps.ContainsKey("pattFill"))
        (this.m_shape as Shape).DocxProps.Remove("pattFill");
      if (!(this.m_shape as Shape).Docx2007Props.ContainsKey("fill"))
        return;
      (this.m_shape as Shape).Docx2007Props.Remove("fill");
    }
    else
    {
      if (!(this.m_shape is GroupShape))
        return;
      if ((this.m_shape as GroupShape).DocxProps.ContainsKey("gradFill"))
        (this.m_shape as GroupShape).DocxProps.Remove("gradFill");
      if ((this.m_shape as GroupShape).DocxProps.ContainsKey("blipFill"))
        (this.m_shape as GroupShape).DocxProps.Remove("blipFill");
      if (!(this.m_shape as GroupShape).DocxProps.ContainsKey("pattFill"))
        return;
      (this.m_shape as GroupShape).DocxProps.Remove("pattFill");
    }
  }

  internal FillFormat Clone()
  {
    FillFormat fillFormat = (FillFormat) this.MemberwiseClone();
    if (this.m_ImageRecord != null)
    {
      ImageRecord imageRecord = new ImageRecord(this.m_picture != null ? this.m_picture.Document : (this.m_childShape != null ? this.m_childShape.Document : this.m_shape.Document), this.m_ImageRecord);
      fillFormat.m_ImageRecord = imageRecord;
    }
    if (this.FillRectangle != null)
      fillFormat.FillRectangle = this.FillRectangle.Clone();
    if (this.SourceRectangle != null)
      fillFormat.SourceRectangle = this.SourceRectangle.Clone();
    if (this.BlipFormat != null)
      fillFormat.BlipFormat = this.BlipFormat.Clone();
    if (this.GradientFill != null && this.GradientFill.GradientStops != null && this.GradientFill.GradientStops.Count > 0)
      fillFormat.GradientFill = this.GradientFill.Clone();
    return fillFormat;
  }

  internal void Close()
  {
    if (this.m_GradientFill != null)
    {
      this.m_GradientFill.Close();
      this.m_GradientFill = (GradientFill) null;
    }
    if (this.m_FillRectangle != null)
      this.m_FillRectangle = (TileRectangle) null;
    if (this.m_SourceRectangle != null)
      this.m_SourceRectangle = (TileRectangle) null;
    if (this.m_ImageRecord != null)
    {
      this.m_ImageRecord.Close();
      this.m_ImageRecord = (ImageRecord) null;
    }
    if (this.m_blipFormat != null)
    {
      this.m_blipFormat.Close();
      this.m_blipFormat = (BlipFormat) null;
    }
    if (this.m_fillSchemeColor != null)
    {
      this.m_fillSchemeColor.Clear();
      this.m_fillSchemeColor = (List<DictionaryEntry>) null;
    }
    this.m_shape = (ShapeBase) null;
    this.m_picture = (WPicture) null;
  }
}
