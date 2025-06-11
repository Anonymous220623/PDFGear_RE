// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ChildShape
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ChildShape : ShapeCommon, IEntity, ILeafWidget, IWidget
{
  private List<string> m_styleProps;
  internal List<Stream> m_pictureProps;
  private double m_arcSize;
  private string m_Adjustments;
  internal byte m_bFlags1 = 16 /*0x10*/;
  private float m_rotation;
  internal bool? flipH;
  internal bool? flipV;
  private float m_rotationToRender;
  private List<EffectFormat> m_effectList;
  private byte m_bFlags;
  private Dictionary<string, DictionaryEntry> m_relations;
  private Dictionary<string, ImageRecord> m_imageRelations;
  private float x_value;
  private float y_value;
  private float m_horzPos;
  private float m_verPos;
  private string m_type;
  private WTextBody m_textBody;
  private TextFrame m_textFrame;
  private LineFormat m_lineFormat;
  private FillFormat m_fillFormat;
  private WChart m_chart;
  private XmlParagraphItem m_xmlParagraphItem;
  private float m_leftPosition;
  private float m_topPosition;
  private AutoShapeType m_autoShapeType;
  internal Dictionary<string, string> m_shapeGuide;
  private List<ShapeStyleReference> m_shapeStyleItems;
  private Color m_fontRefColor = Color.Empty;
  internal Dictionary<string, Stream> m_docx2007Props;
  private float m_lineFromXPosition;
  private float m_lineFromYPosition;
  private float m_lineToXPosition;
  private float m_lineToYPosition;
  internal ImageRecord m_imageRecord;
  private EntityType m_elementType;
  private bool m_visible = true;
  internal bool skipPositionUpdate;
  private RectangleF m_textLayoutingBounds;
  private List<Path2D> m_vmlPathPoints;
  internal bool m_isVMLPathUpdated;
  private Dictionary<string, string> m_guideList;
  private Dictionary<string, string> m_avList;
  private List<Path2D> m_path2DList;

  internal List<Path2D> Path2DList
  {
    get => this.m_path2DList;
    set => this.m_path2DList = value;
  }

  internal List<Path2D> VMLPathPoints
  {
    get => this.m_vmlPathPoints;
    set => this.m_vmlPathPoints = value;
  }

  internal bool IsEffectStyleInline
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 254 | (value ? 1 : 0));
  }

  internal Dictionary<string, string> ShapeGuide
  {
    get
    {
      if (this.m_shapeGuide == null)
        this.m_shapeGuide = new Dictionary<string, string>();
      return this.m_shapeGuide;
    }
  }

  internal bool IsFillStyleInline
  {
    get => ((int) this.m_bFlags1 & 4) >> 2 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 251 | (value ? 1 : 0) << 2);
  }

  internal bool FlipHorizantal
  {
    get => this.flipH.HasValue && this.flipH.Value;
    set => this.flipH = new bool?(value);
  }

  internal bool FlipVertical
  {
    get => this.flipV.HasValue && this.flipV.Value;
    set => this.flipV = new bool?(value);
  }

  internal bool IsScenePropertiesInline
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal List<ShapeStyleReference> ShapeStyleReferences
  {
    get
    {
      if (this.m_shapeStyleItems == null)
        this.m_shapeStyleItems = new List<ShapeStyleReference>();
      return this.m_shapeStyleItems;
    }
    set => this.m_shapeStyleItems = value;
  }

  internal bool IsShapePropertiesInline
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsLineStyleInline
  {
    get => ((int) this.m_bFlags1 & 2) >> 1 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 253 | (value ? 1 : 0) << 1);
  }

  internal Color FontRefColor
  {
    get => this.m_fontRefColor;
    set => this.m_fontRefColor = value;
  }

  internal float X
  {
    get => this.x_value;
    set => this.x_value = value;
  }

  internal float Y
  {
    get => this.y_value;
    set => this.y_value = value;
  }

  internal float HorizontalPosition
  {
    get => this.m_horzPos;
    set => this.m_horzPos = value;
  }

  internal float VerticalPosition
  {
    get => this.m_verPos;
    set => this.m_verPos = value;
  }

  internal string Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal EntityType ElementType
  {
    get => this.m_elementType;
    set => this.m_elementType = value;
  }

  internal bool HasTextBody
  {
    get
    {
      return (this.m_elementType == EntityType.AutoShape || this.m_elementType == EntityType.Shape || this.m_elementType == EntityType.TextBox) && this.m_textBody != null;
    }
  }

  internal float LeftMargin
  {
    get => this.m_leftPosition;
    set => this.m_leftPosition = value;
  }

  internal float TopMargin
  {
    get => this.m_topPosition;
    set => this.m_topPosition = value;
  }

  internal float LineFromXPosition
  {
    get => this.m_lineFromXPosition;
    set => this.m_lineFromXPosition = value;
  }

  internal float LineFromYPosition
  {
    get => this.m_lineFromYPosition;
    set => this.m_lineFromYPosition = value;
  }

  internal float LineToXPosition
  {
    get => this.m_lineToXPosition;
    set => this.m_lineToXPosition = value;
  }

  internal float LineToYPosition
  {
    get => this.m_lineToYPosition;
    set => this.m_lineToYPosition = value;
  }

  internal WTextBody TextBody
  {
    get
    {
      if (this.m_textBody == null)
        this.m_textBody = new WTextBody(this.Document, (Entity) this);
      return this.m_textBody;
    }
    set => this.m_textBody = value;
  }

  internal TextFrame TextFrame
  {
    get
    {
      if (this.m_textFrame == null)
        this.m_textFrame = new TextFrame(this);
      return this.m_textFrame;
    }
    set => this.m_textFrame = value;
  }

  internal LineFormat LineFormat
  {
    get
    {
      if (this.m_lineFormat == null)
        this.m_lineFormat = new LineFormat(this);
      return this.m_lineFormat;
    }
    set => this.m_lineFormat = value;
  }

  internal FillFormat FillFormat
  {
    get
    {
      if (this.m_fillFormat == null)
        this.m_fillFormat = new FillFormat(this);
      return this.m_fillFormat;
    }
    set => this.m_fillFormat = value;
  }

  internal WChart Chart
  {
    get => this.Document.IsOpening ? (this.m_chart = new WChart(this.m_doc)) : this.m_chart;
    set => this.m_chart = value;
  }

  internal XmlParagraphItem XmlParagraphItem
  {
    get => this.m_xmlParagraphItem;
    set => this.m_xmlParagraphItem = value;
  }

  internal AutoShapeType AutoShapeType
  {
    get => this.m_autoShapeType;
    set => this.m_autoShapeType = value;
  }

  internal bool IsPicture => this.m_imageRecord != null;

  public bool Visible
  {
    get => this.m_visible;
    set => this.m_visible = value;
  }

  internal byte[] ImageBytes => this.m_imageRecord.ImageBytes;

  internal ImageRecord ImageRecord => this.m_imageRecord;

  internal new WordDocument Document => this.m_doc;

  internal List<EffectFormat> EffectList
  {
    get
    {
      if (this.m_effectList == null)
        this.m_effectList = new List<EffectFormat>();
      return this.m_effectList;
    }
    set => this.m_effectList = value;
  }

  internal bool IsHorizontalRule
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsTextBoxShape
  {
    get => ((int) this.m_bFlags1 & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 191 | (value ? 1 : 0) << 6);
  }

  internal string Adjustments
  {
    get => this.m_Adjustments;
    set => this.m_Adjustments = value;
  }

  internal float Rotation
  {
    get => this.m_rotation;
    set => this.m_rotation = value;
  }

  internal double ArcSize
  {
    get => this.m_arcSize;
    set => this.m_arcSize = value;
  }

  internal Dictionary<string, Stream> Docx2007Props
  {
    get
    {
      if (this.m_docx2007Props == null)
        this.m_docx2007Props = new Dictionary<string, Stream>();
      return this.m_docx2007Props;
    }
    set => this.m_docx2007Props = value;
  }

  internal bool UseStandardColorHR
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool UseNoShadeHR
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal Dictionary<string, DictionaryEntry> Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new Dictionary<string, DictionaryEntry>();
      return this.m_relations;
    }
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

  internal List<Stream> DocxPictureVisualProps
  {
    get
    {
      if (this.m_pictureProps == null)
        this.m_pictureProps = new List<Stream>();
      return this.m_pictureProps;
    }
    set => this.m_pictureProps = value;
  }

  internal bool LayoutInCell
  {
    get => ((int) this.m_bFlags1 & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 239 | (value ? 1 : 0) << 4);
  }

  internal bool Is2007Shape
  {
    get => ((int) this.m_bFlags1 & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 223 | (value ? 1 : 0) << 5);
  }

  internal Dictionary<string, ImageRecord> ImageRelations
  {
    get
    {
      if (this.m_imageRelations == null)
        this.m_imageRelations = new Dictionary<string, ImageRecord>();
      return this.m_imageRelations;
    }
  }

  public override EntityType EntityType => EntityType.ChildShape;

  internal RectangleF TextLayoutingBounds
  {
    get => this.m_textLayoutingBounds;
    set => this.m_textLayoutingBounds = value;
  }

  internal float RotationToRender
  {
    get => this.m_rotationToRender;
    set => this.m_rotationToRender = value;
  }

  internal bool FlipHorizantalToRender
  {
    get => ((int) this.m_bFlags1 & 8) >> 3 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 247 | (value ? 1 : 0) << 3);
  }

  internal bool FlipVerticalToRender
  {
    get => ((int) this.m_bFlags1 & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  internal ChildShape(IWordDocument doc, AutoShapeType autoShapeType)
    : this(doc)
  {
    this.m_autoShapeType = autoShapeType;
  }

  internal ChildShape(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) this);
    this.FillFormat.Color = Color.White;
    this.FillFormat.Fill = true;
    this.FillFormat.FillType = FillType.FillSolid;
    this.LineFormat.Color = Color.Black;
    this.LineFormat.DashStyle = LineDashing.Solid;
    this.LineFormat.Line = true;
    this.LineFormat.Style = LineStyle.Single;
    this.LineFormat.Transparency = 0.0f;
    this.LineFormat.m_Weight = 1f;
    this.TextFrame.TextVerticalAlignment = VerticalAlignment.Top;
  }

  internal override void Detach()
  {
    base.Detach();
    if (this.DeepDetached)
      return;
    this.Document.FloatingItems.Remove((Entity) this);
  }

  internal override void AttachToDocument()
  {
    if (this.GetTextWrappingStyle() != TextWrappingStyle.Inline)
      this.Document.FloatingItems.Add((Entity) this);
    this.IsCloned = false;
    if (this.TextBody == null)
      return;
    this.TextBody.AttachToDocument();
  }

  protected override object CloneImpl()
  {
    ChildShape owner = (ChildShape) base.CloneImpl();
    owner.IsCloned = true;
    if (this.ElementType == EntityType.Shape || this.ElementType == EntityType.AutoShape)
    {
      if (this.m_textBody != null)
      {
        owner.m_textBody = this.m_textBody.Clone() as WTextBody;
        owner.m_textBody.SetOwner((OwnerHolder) owner);
      }
      if (this.m_shapeGuide != null && this.m_shapeGuide.Count > 0)
        this.Document.CloneProperties(this.m_shapeGuide, ref owner.m_shapeGuide);
    }
    else if (this.ElementType == EntityType.TextBox)
    {
      if (this.m_textBody != null)
      {
        owner.m_textBody = this.m_textBody.Clone() as WTextBody;
        owner.m_textBody.SetOwner((OwnerHolder) owner);
      }
    }
    else if (this.ElementType == EntityType.Chart && this.Chart != null)
      owner.Chart = this.Chart.Clone() as WChart;
    else if (this.IsPicture)
    {
      owner.m_imageRecord = new ImageRecord(this.Document, owner.ImageRecord);
      if (this.DocxPictureVisualProps != null && this.DocxPictureVisualProps.Count > 0)
      {
        owner.DocxPictureVisualProps = new List<Stream>();
        foreach (Stream pictureVisualProp in this.DocxPictureVisualProps)
          owner.DocxPictureVisualProps.Add(this.Document.CloneStream(pictureVisualProp));
      }
      this.Document.HasPicture = true;
    }
    owner.CloneShapeFormat(this);
    if (this.TextFrame != null && this.TextFrame.InternalMargin != null)
      owner.TextFrame.m_intMargin = this.TextFrame.InternalMargin.Clone();
    if (this.m_docx2007Props != null && this.m_docx2007Props.Count > 0)
      owner.Document.CloneProperties(this.Docx2007Props, ref owner.m_docx2007Props);
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (this.TextBody != null)
      this.TextBody.CloneRelationsTo(doc, nextOwner);
    base.CloneRelationsTo(doc, nextOwner);
    if (this.IsPicture && this.m_imageRecord != null)
    {
      Size size = this.m_imageRecord.Size;
      ImageFormat imageFormat = this.m_imageRecord.ImageFormat;
      int length = this.m_imageRecord.Length;
      this.m_imageRecord = !this.m_imageRecord.IsMetafile ? doc.Images.LoadImage(this.m_imageRecord.ImageBytes) : doc.Images.LoadMetaFileImage(this.m_imageRecord.m_imageBytes, true);
      this.m_imageRecord.Size = size;
      this.m_imageRecord.ImageFormat = imageFormat;
      this.m_imageRecord.Length = length;
    }
    this.IsCloned = false;
  }

  internal Dictionary<string, string> GetGuideList()
  {
    return this.m_guideList ?? (this.m_guideList = new Dictionary<string, string>());
  }

  internal Dictionary<string, string> GetAvList()
  {
    return this.m_avList ?? (this.m_avList = new Dictionary<string, string>());
  }

  internal void CloneShapeFormat(ChildShape childShape)
  {
    bool flag = this.Document != null && this.Document.DocHasThemes;
    if (childShape.FillFormat != null && (childShape.IsFillStyleInline || childShape.Is2007Shape && !childShape.FillFormat.IsDefaultFill))
    {
      this.FillFormat = childShape.FillFormat.Clone();
      this.IsFillStyleInline = true;
    }
    else if (flag && childShape.ShapeStyleReferences != null && childShape.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = childShape.ShapeStyleReferences[1].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.FillFormats.Count > styleRefIndex)
      {
        uint maxValue1 = uint.MaxValue;
        FillFormat fillFormat = childShape.Document.Themes.FmtScheme.FillFormats[styleRefIndex - 1];
        this.FillFormat = fillFormat.Clone();
        if (fillFormat.FillType == FillType.FillSolid)
        {
          this.FillFormat.Color = childShape.ShapeStyleReferences[1].StyleRefColor;
          this.FillFormat.Transparency = childShape.ShapeStyleReferences[1].StyleRefOpacity;
        }
        else if (fillFormat.FillType == FillType.FillGradient)
        {
          for (int index = 0; index < fillFormat.GradientFill.GradientStops.Count; ++index)
          {
            this.FillFormat.GradientFill.GradientStops[index].Color = this.StyleColorTransform(fillFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, childShape.ShapeStyleReferences[1].StyleRefColor, ref maxValue1);
            maxValue1 = uint.MaxValue;
          }
        }
        else if (fillFormat.FillType == FillType.FillPatterned)
        {
          List<DictionaryEntry> fillTransformation1 = new List<DictionaryEntry>();
          List<DictionaryEntry> fillTransformation2 = new List<DictionaryEntry>();
          if (fillFormat.FillSchemeColorTransforms.Count > 0)
          {
            for (int index = 0; index < fillFormat.FillSchemeColorTransforms.Count; ++index)
            {
              if (this.StartsWithExt(fillFormat.FillSchemeColorTransforms[index].Key.ToString(), "fgClr"))
                fillTransformation1.Add(fillFormat.FillSchemeColorTransforms[index]);
              if (this.StartsWithExt(fillFormat.FillSchemeColorTransforms[index].Key.ToString(), "bgClr"))
                fillTransformation2.Add(fillFormat.FillSchemeColorTransforms[index]);
            }
          }
          this.FillFormat.ForeColor = this.StyleColorTransform(fillTransformation1, childShape.ShapeStyleReferences[1].StyleRefColor, ref maxValue1);
          uint maxValue2 = uint.MaxValue;
          this.FillFormat.Color = this.StyleColorTransform(fillTransformation2, childShape.ShapeStyleReferences[1].StyleRefColor, ref maxValue2);
        }
        this.IsFillStyleInline = true;
      }
    }
    if (childShape.IsLineStyleInline && childShape.LineFormat != null)
    {
      this.LineFormat = childShape.LineFormat.Clone();
      this.IsLineStyleInline = true;
    }
    else if (flag && childShape.ShapeStyleReferences != null && childShape.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = childShape.ShapeStyleReferences[0].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.LnStyleScheme.Count > styleRefIndex)
      {
        uint maxValue3 = uint.MaxValue;
        LineFormat lineFormat = childShape.Document.Themes.FmtScheme.LnStyleScheme[styleRefIndex - 1];
        this.LineFormat = childShape.Document.Themes.FmtScheme.LnStyleScheme[styleRefIndex - 1].Clone();
        if (lineFormat.LineFormatType == LineFormatType.Solid)
        {
          this.LineFormat.Color = childShape.ShapeStyleReferences[0].StyleRefColor;
          this.LineFormat.Transparency = childShape.ShapeStyleReferences[0].StyleRefOpacity;
        }
        else if (lineFormat.LineFormatType == LineFormatType.Gradient)
        {
          for (int index = 0; index < lineFormat.GradientFill.GradientStops.Count; ++index)
          {
            this.FillFormat.GradientFill.GradientStops[index].Color = this.StyleColorTransform(lineFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, childShape.ShapeStyleReferences[1].StyleRefColor, ref maxValue3);
            maxValue3 = uint.MaxValue;
          }
        }
        else if (lineFormat.LineFormatType == LineFormatType.Patterned)
        {
          List<DictionaryEntry> fillTransformation3 = new List<DictionaryEntry>();
          List<DictionaryEntry> fillTransformation4 = new List<DictionaryEntry>();
          if (lineFormat.LineSchemeColorTransforms.Count > 0)
          {
            for (int index = 0; index < lineFormat.LineSchemeColorTransforms.Count; ++index)
            {
              if (this.StartsWithExt(lineFormat.LineSchemeColorTransforms[index].Key.ToString(), "fgClr"))
                fillTransformation3.Add(lineFormat.LineSchemeColorTransforms[index]);
              if (this.StartsWithExt(lineFormat.LineSchemeColorTransforms[index].Key.ToString(), "bgClr"))
                fillTransformation4.Add(lineFormat.LineSchemeColorTransforms[index]);
            }
          }
          this.FillFormat.ForeColor = this.StyleColorTransform(fillTransformation3, childShape.ShapeStyleReferences[1].StyleRefColor, ref maxValue3);
          uint maxValue4 = uint.MaxValue;
          this.FillFormat.Color = this.StyleColorTransform(fillTransformation4, childShape.ShapeStyleReferences[1].StyleRefColor, ref maxValue4);
        }
        this.IsLineStyleInline = true;
      }
    }
    if (childShape.EffectList == null)
      return;
    List<EffectFormat> effectFormatList = new List<EffectFormat>();
    for (int index = 0; index < childShape.EffectList.Count; ++index)
    {
      EffectFormat effectFormat = new EffectFormat(this);
      if (childShape.EffectList[index].IsEffectListItem)
      {
        if (childShape.EffectList[index].IsShadowEffect && childShape.EffectList[index].ShadowFormat != null)
        {
          effectFormat.ShadowFormat = childShape.EffectList[index].ShadowFormat.Clone();
          effectFormat.IsShadowEffect = true;
        }
        else if (childShape.EffectList[index].IsGlowEffect && childShape.EffectList[index].GlowFormat != null)
        {
          effectFormat.GlowFormat = childShape.EffectList[index].GlowFormat.Clone();
          effectFormat.IsGlowEffect = true;
        }
        else if (childShape.EffectList[index].IsReflection && childShape.EffectList[index].ReflectionFormat != null)
        {
          effectFormat.ReflectionFormat = childShape.EffectList[index].ReflectionFormat.Clone();
          effectFormat.IsReflection = true;
        }
        else if (childShape.EffectList[index].IsSoftEdge)
        {
          effectFormat.NoSoftEdges = childShape.EffectList[index].NoSoftEdges;
          effectFormat.SoftEdgeRadius = childShape.EffectList[index].SoftEdgeRadius;
        }
        effectFormatList.Add(effectFormat);
        effectFormatList[index].IsEffectListItem = true;
      }
      else if ((childShape.EffectList[index].IsShapeProperties || childShape.EffectList[index].IsSceneProperties) && childShape.EffectList[index].ThreeDFormat != null)
      {
        effectFormat.ThreeDFormat = childShape.EffectList[index].ThreeDFormat.Clone();
        effectFormat.IsSceneProperties = childShape.EffectList[index].IsSceneProperties;
        effectFormat.IsShapeProperties = childShape.EffectList[index].IsShapeProperties;
        effectFormatList.Add(effectFormat);
      }
    }
    this.IsEffectStyleInline = true;
    this.EffectList.Clear();
    this.EffectList = effectFormatList;
    if (this.m_guideList != null)
      this.Document.CloneProperties(this.m_guideList, ref childShape.m_guideList);
    if (this.m_avList != null)
      this.Document.CloneProperties(this.m_avList, ref childShape.m_avList);
    if (this.Path2DList == null)
      return;
    List<Path2D> path2DList = new List<Path2D>();
    for (int index = 0; index < this.Path2DList.Count; ++index)
    {
      Path2D path2D = this.Path2DList[index].Clone();
      path2DList.Add(path2D);
    }
    childShape.Path2DList = path2DList;
  }

  internal void CloneShapeFormat(Shape shape)
  {
    bool flag = this.Document != null && this.Document.DocHasThemes;
    if (shape.FillFormat != null && (shape.IsFillStyleInline || shape.Is2007Shape && !shape.FillFormat.IsDefaultFill))
    {
      this.FillFormat = shape.FillFormat.Clone();
      this.IsFillStyleInline = true;
    }
    else if (flag && shape.ShapeStyleReferences != null && shape.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = shape.ShapeStyleReferences[1].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.FillFormats.Count > styleRefIndex)
      {
        uint maxValue1 = uint.MaxValue;
        FillFormat fillFormat = shape.Document.Themes.FmtScheme.FillFormats[styleRefIndex - 1];
        this.FillFormat = fillFormat.Clone();
        if (fillFormat.FillType == FillType.FillSolid)
        {
          this.FillFormat.Color = shape.ShapeStyleReferences[1].StyleRefColor;
          this.FillFormat.Transparency = shape.ShapeStyleReferences[1].StyleRefOpacity;
        }
        else if (fillFormat.FillType == FillType.FillGradient)
        {
          for (int index = 0; index < fillFormat.GradientFill.GradientStops.Count; ++index)
          {
            this.FillFormat.GradientFill.GradientStops[index].Color = this.StyleColorTransform(fillFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, shape.ShapeStyleReferences[1].StyleRefColor, ref maxValue1);
            maxValue1 = uint.MaxValue;
          }
        }
        else if (fillFormat.FillType == FillType.FillPatterned)
        {
          List<DictionaryEntry> fillTransformation1 = new List<DictionaryEntry>();
          List<DictionaryEntry> fillTransformation2 = new List<DictionaryEntry>();
          if (fillFormat.FillSchemeColorTransforms.Count > 0)
          {
            for (int index = 0; index < fillFormat.FillSchemeColorTransforms.Count; ++index)
            {
              if (this.StartsWithExt(fillFormat.FillSchemeColorTransforms[index].Key.ToString(), "fgClr"))
                fillTransformation1.Add(fillFormat.FillSchemeColorTransforms[index]);
              if (this.StartsWithExt(fillFormat.FillSchemeColorTransforms[index].Key.ToString(), "bgClr"))
                fillTransformation2.Add(fillFormat.FillSchemeColorTransforms[index]);
            }
          }
          this.FillFormat.ForeColor = this.StyleColorTransform(fillTransformation1, shape.ShapeStyleReferences[1].StyleRefColor, ref maxValue1);
          uint maxValue2 = uint.MaxValue;
          this.FillFormat.Color = this.StyleColorTransform(fillTransformation2, shape.ShapeStyleReferences[1].StyleRefColor, ref maxValue2);
        }
        this.IsFillStyleInline = true;
      }
    }
    if (shape.IsLineStyleInline && shape.LineFormat != null)
    {
      this.LineFormat = shape.LineFormat.Clone();
      this.IsLineStyleInline = true;
    }
    else if (flag && shape.ShapeStyleReferences != null && shape.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = shape.ShapeStyleReferences[0].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.LnStyleScheme.Count > styleRefIndex)
      {
        uint maxValue3 = uint.MaxValue;
        LineFormat lineFormat = shape.Document.Themes.FmtScheme.LnStyleScheme[styleRefIndex - 1];
        this.LineFormat = shape.Document.Themes.FmtScheme.LnStyleScheme[styleRefIndex - 1].Clone();
        if (lineFormat.LineFormatType == LineFormatType.Solid)
        {
          this.LineFormat.Color = shape.ShapeStyleReferences[0].StyleRefColor;
          this.LineFormat.Transparency = shape.ShapeStyleReferences[0].StyleRefOpacity;
        }
        else if (lineFormat.LineFormatType == LineFormatType.Gradient)
        {
          for (int index = 0; index < lineFormat.GradientFill.GradientStops.Count; ++index)
          {
            this.FillFormat.GradientFill.GradientStops[index].Color = this.StyleColorTransform(lineFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, shape.ShapeStyleReferences[1].StyleRefColor, ref maxValue3);
            maxValue3 = uint.MaxValue;
          }
        }
        else if (lineFormat.LineFormatType == LineFormatType.Patterned)
        {
          List<DictionaryEntry> fillTransformation3 = new List<DictionaryEntry>();
          List<DictionaryEntry> fillTransformation4 = new List<DictionaryEntry>();
          if (lineFormat.LineSchemeColorTransforms.Count > 0)
          {
            for (int index = 0; index < lineFormat.LineSchemeColorTransforms.Count; ++index)
            {
              if (this.StartsWithExt(lineFormat.LineSchemeColorTransforms[index].Key.ToString(), "fgClr"))
                fillTransformation3.Add(lineFormat.LineSchemeColorTransforms[index]);
              if (this.StartsWithExt(lineFormat.LineSchemeColorTransforms[index].Key.ToString(), "bgClr"))
                fillTransformation4.Add(lineFormat.LineSchemeColorTransforms[index]);
            }
          }
          this.FillFormat.ForeColor = this.StyleColorTransform(fillTransformation3, shape.ShapeStyleReferences[1].StyleRefColor, ref maxValue3);
          uint maxValue4 = uint.MaxValue;
          this.FillFormat.Color = this.StyleColorTransform(fillTransformation4, shape.ShapeStyleReferences[1].StyleRefColor, ref maxValue4);
        }
        this.IsLineStyleInline = true;
      }
    }
    if (shape.EffectList == null)
      return;
    List<EffectFormat> effectFormatList = new List<EffectFormat>();
    for (int index = 0; index < shape.EffectList.Count; ++index)
    {
      EffectFormat effectFormat = new EffectFormat(this);
      if (shape.EffectList[index].IsEffectListItem)
      {
        if (shape.EffectList[index].IsShadowEffect && shape.EffectList[index].ShadowFormat != null)
        {
          effectFormat.ShadowFormat = shape.EffectList[index].ShadowFormat.Clone();
          effectFormat.IsShadowEffect = true;
        }
        else if (shape.EffectList[index].IsGlowEffect && shape.EffectList[index].GlowFormat != null)
        {
          effectFormat.GlowFormat = shape.EffectList[index].GlowFormat.Clone();
          effectFormat.IsGlowEffect = true;
        }
        else if (shape.EffectList[index].IsReflection && shape.EffectList[index].ReflectionFormat != null)
        {
          effectFormat.ReflectionFormat = shape.EffectList[index].ReflectionFormat.Clone();
          effectFormat.IsReflection = true;
        }
        else if (shape.EffectList[index].IsSoftEdge)
        {
          effectFormat.NoSoftEdges = shape.EffectList[index].NoSoftEdges;
          effectFormat.SoftEdgeRadius = shape.EffectList[index].SoftEdgeRadius;
        }
        effectFormatList.Add(effectFormat);
        effectFormatList[index].IsEffectListItem = true;
      }
      else if ((shape.EffectList[index].IsShapeProperties || shape.EffectList[index].IsSceneProperties) && shape.EffectList[index].ThreeDFormat != null)
      {
        effectFormat.ThreeDFormat = shape.EffectList[index].ThreeDFormat.Clone();
        effectFormat.IsSceneProperties = shape.EffectList[index].IsSceneProperties;
        effectFormat.IsShapeProperties = shape.EffectList[index].IsShapeProperties;
        effectFormatList.Add(effectFormat);
      }
    }
    this.IsEffectStyleInline = true;
    this.EffectList.Clear();
    this.EffectList = effectFormatList;
  }

  internal GroupShape GetOwnerGroupShape()
  {
    Entity owner = this.Owner;
    while (!(owner is GroupShape))
      owner = owner.Owner;
    return owner as GroupShape;
  }

  internal void CloneChildShapeFormatToShape(Shape shape)
  {
    bool flag = this.Document != null && this.Document.DocHasThemes;
    if (this.FillFormat != null && (this.IsFillStyleInline || this.Is2007Shape && !this.FillFormat.IsDefaultFill))
    {
      shape.FillFormat = this.FillFormat.Clone();
      shape.IsFillStyleInline = true;
    }
    else if (flag && this.ShapeStyleReferences != null && this.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = this.ShapeStyleReferences[1].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.FillFormats.Count > styleRefIndex)
      {
        uint maxValue1 = uint.MaxValue;
        FillFormat fillFormat = shape.Document.Themes.FmtScheme.FillFormats[styleRefIndex - 1];
        shape.FillFormat = fillFormat.Clone();
        if (fillFormat.FillType == FillType.FillSolid)
        {
          shape.FillFormat.Color = this.ShapeStyleReferences[1].StyleRefColor;
          shape.FillFormat.Transparency = this.ShapeStyleReferences[1].StyleRefOpacity;
        }
        else if (fillFormat.FillType == FillType.FillGradient)
        {
          for (int index = 0; index < fillFormat.GradientFill.GradientStops.Count; ++index)
          {
            shape.FillFormat.GradientFill.GradientStops[index].Color = this.StyleColorTransform(fillFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue1);
            maxValue1 = uint.MaxValue;
          }
        }
        else if (fillFormat.FillType == FillType.FillPatterned)
        {
          List<DictionaryEntry> fillTransformation1 = new List<DictionaryEntry>();
          List<DictionaryEntry> fillTransformation2 = new List<DictionaryEntry>();
          if (fillFormat.FillSchemeColorTransforms.Count > 0)
          {
            for (int index = 0; index < fillFormat.FillSchemeColorTransforms.Count; ++index)
            {
              if (this.StartsWithExt(fillFormat.FillSchemeColorTransforms[index].Key.ToString(), "fgClr"))
                fillTransformation1.Add(fillFormat.FillSchemeColorTransforms[index]);
              if (this.StartsWithExt(fillFormat.FillSchemeColorTransforms[index].Key.ToString(), "bgClr"))
                fillTransformation2.Add(fillFormat.FillSchemeColorTransforms[index]);
            }
          }
          shape.FillFormat.ForeColor = this.StyleColorTransform(fillTransformation1, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue1);
          uint maxValue2 = uint.MaxValue;
          shape.FillFormat.Color = this.StyleColorTransform(fillTransformation2, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue2);
        }
        shape.IsFillStyleInline = true;
      }
    }
    if (this.LineFormat != null)
    {
      shape.LineFormat = this.LineFormat.Clone();
      shape.IsLineStyleInline = true;
    }
    else if (flag && this.ShapeStyleReferences != null && this.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = this.ShapeStyleReferences[0].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.LnStyleScheme.Count > styleRefIndex)
      {
        uint maxValue3 = uint.MaxValue;
        LineFormat lineFormat = shape.Document.Themes.FmtScheme.LnStyleScheme[styleRefIndex - 1];
        shape.LineFormat = shape.Document.Themes.FmtScheme.LnStyleScheme[styleRefIndex - 1].Clone();
        if (lineFormat.LineFormatType == LineFormatType.Solid)
        {
          shape.LineFormat.Color = this.ShapeStyleReferences[0].StyleRefColor;
          shape.LineFormat.Transparency = this.ShapeStyleReferences[0].StyleRefOpacity;
        }
        else if (lineFormat.LineFormatType == LineFormatType.Gradient)
        {
          for (int index = 0; index < lineFormat.GradientFill.GradientStops.Count; ++index)
          {
            shape.FillFormat.GradientFill.GradientStops[index].Color = this.StyleColorTransform(lineFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue3);
            maxValue3 = uint.MaxValue;
          }
        }
        else if (lineFormat.LineFormatType == LineFormatType.Patterned)
        {
          List<DictionaryEntry> fillTransformation3 = new List<DictionaryEntry>();
          List<DictionaryEntry> fillTransformation4 = new List<DictionaryEntry>();
          if (lineFormat.LineSchemeColorTransforms.Count > 0)
          {
            for (int index = 0; index < lineFormat.LineSchemeColorTransforms.Count; ++index)
            {
              if (this.StartsWithExt(lineFormat.LineSchemeColorTransforms[index].Key.ToString(), "fgClr"))
                fillTransformation3.Add(lineFormat.LineSchemeColorTransforms[index]);
              if (this.StartsWithExt(lineFormat.LineSchemeColorTransforms[index].Key.ToString(), "bgClr"))
                fillTransformation4.Add(lineFormat.LineSchemeColorTransforms[index]);
            }
          }
          shape.FillFormat.ForeColor = this.StyleColorTransform(fillTransformation3, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue3);
          uint maxValue4 = uint.MaxValue;
          shape.FillFormat.Color = this.StyleColorTransform(fillTransformation4, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue4);
        }
        shape.IsLineStyleInline = true;
      }
    }
    if (this.EffectList == null)
      return;
    List<EffectFormat> effectFormatList = new List<EffectFormat>();
    for (int index = 0; index < this.EffectList.Count; ++index)
    {
      EffectFormat effectFormat = new EffectFormat(this);
      if (this.EffectList[index].IsEffectListItem)
      {
        if (this.EffectList[index].IsShadowEffect && this.EffectList[index].ShadowFormat != null)
        {
          effectFormat.ShadowFormat = this.EffectList[index].ShadowFormat.Clone();
          effectFormat.IsShadowEffect = true;
        }
        else if (this.EffectList[index].IsGlowEffect && this.EffectList[index].GlowFormat != null)
        {
          effectFormat.GlowFormat = this.EffectList[index].GlowFormat.Clone();
          effectFormat.IsGlowEffect = true;
        }
        else if (this.EffectList[index].IsReflection && this.EffectList[index].ReflectionFormat != null)
        {
          effectFormat.ReflectionFormat = this.EffectList[index].ReflectionFormat.Clone();
          effectFormat.IsReflection = true;
        }
        else if (this.EffectList[index].IsSoftEdge)
        {
          effectFormat.NoSoftEdges = this.EffectList[index].NoSoftEdges;
          effectFormat.SoftEdgeRadius = this.EffectList[index].SoftEdgeRadius;
        }
        effectFormatList.Add(effectFormat);
        effectFormatList[index].IsEffectListItem = true;
      }
      else if ((this.EffectList[index].IsShapeProperties || this.EffectList[index].IsSceneProperties) && this.EffectList[index].ThreeDFormat != null)
      {
        effectFormat.ThreeDFormat = this.EffectList[index].ThreeDFormat.Clone();
        effectFormat.IsSceneProperties = this.EffectList[index].IsSceneProperties;
        effectFormat.IsShapeProperties = this.EffectList[index].IsShapeProperties;
        effectFormatList.Add(effectFormat);
      }
    }
    shape.IsEffectStyleInline = true;
    shape.EffectList.Clear();
    shape.EffectList = effectFormatList;
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  internal Color StyleColorTransform(
    List<DictionaryEntry> fillTransformation,
    Color themeColor,
    ref uint opacity)
  {
    bool flag = false;
    foreach (DictionaryEntry dictionaryEntry in fillTransformation)
    {
      string empty1 = string.Empty;
      switch (dictionaryEntry.Key.ToString().StartsWith("fgClr") || dictionaryEntry.Key.ToString().StartsWith("bgClr") ? dictionaryEntry.Key.ToString().Substring(5) : dictionaryEntry.Key.ToString())
      {
        case "alpha":
          flag = false;
          string str1 = dictionaryEntry.Value.ToString();
          if (!string.IsNullOrEmpty(str1))
          {
            double percentage = this.GetPercentage(str1);
            opacity = (uint) (percentage * 65536.0 / 100.0);
            if (opacity > 65536U /*0x010000*/)
            {
              opacity = 65536U /*0x010000*/;
              continue;
            }
            continue;
          }
          continue;
        case "alphaMod":
          flag = false;
          string str2 = dictionaryEntry.Value.ToString();
          if (!string.IsNullOrEmpty(str2))
          {
            double percentage = this.GetPercentage(str2);
            opacity = (uint) ((opacity == uint.MaxValue ? 65536.0 : (double) opacity) * (percentage / 100.0));
            if (opacity > 65536U /*0x010000*/)
            {
              opacity = 65536U /*0x010000*/;
              continue;
            }
            continue;
          }
          continue;
        case "alphaOff":
          if (!flag)
          {
            string str3 = dictionaryEntry.Value.ToString();
            if (!string.IsNullOrEmpty(str3))
            {
              double percentage = this.GetPercentage(str3);
              opacity = (uint) ((opacity == uint.MaxValue ? 0.0 : (double) opacity) + Math.Round(percentage * 65536.0 / 100.0));
              if (opacity > 65536U /*0x010000*/)
              {
                opacity = 65536U /*0x010000*/;
                continue;
              }
              continue;
            }
            continue;
          }
          continue;
        default:
          string empty2 = dictionaryEntry.Value.ToString();
          if (string.IsNullOrEmpty(empty2) && (dictionaryEntry.Key.ToString() == "comp" || dictionaryEntry.Key.ToString() == "gamma" || dictionaryEntry.Key.ToString() == "gray" || dictionaryEntry.Key.ToString() == "invGamma" || dictionaryEntry.Key.ToString() == "inv"))
            empty2 = string.Empty;
          if (!string.IsNullOrEmpty(empty2))
            flag = this.ColorTransform(dictionaryEntry.Key.ToString(), empty2, ref themeColor);
          if (flag)
          {
            opacity = uint.MaxValue;
            continue;
          }
          continue;
      }
    }
    return themeColor;
  }

  private double GetPercentage(string value)
  {
    double result;
    if (value.EndsWith("%"))
    {
      double.TryParse(value.Replace("%", ""), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    }
    else
    {
      double.TryParse(value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      result /= 1000.0;
    }
    return result;
  }

  private bool ColorTransform(string localName, string value, ref Color themeColor)
  {
    switch (localName)
    {
      case "blue":
        string str1 = value;
        if (!string.IsNullOrEmpty(str1))
        {
          byte blue = (byte) Math.Round((double) byte.MaxValue * WordColor.ConvertsLinearRGBtoRGB(this.GetPercentage(str1) / 100.0));
          themeColor = Color.FromArgb((int) themeColor.A, (int) themeColor.R, (int) themeColor.G, (int) blue);
        }
        return true;
      case "blueMod":
        string str2 = value;
        if (!string.IsNullOrEmpty(str2))
        {
          double percentage = this.GetPercentage(str2);
          themeColor = Color.FromArgb((int) themeColor.A, (int) themeColor.R, (int) themeColor.G, (int) WordColor.ConvertbyModulation(themeColor.B, percentage));
        }
        return true;
      case "blueOff":
        string str3 = value;
        if (!string.IsNullOrEmpty(str3))
        {
          double percentage = this.GetPercentage(str3);
          themeColor = Color.FromArgb((int) themeColor.A, (int) themeColor.R, (int) themeColor.G, (int) WordColor.ConvertbyOffset(themeColor.B, percentage));
        }
        return true;
      case "green":
        string str4 = value;
        if (!string.IsNullOrEmpty(str4))
        {
          byte green = (byte) Math.Round((double) byte.MaxValue * WordColor.ConvertsLinearRGBtoRGB(this.GetPercentage(str4) / 100.0));
          themeColor = Color.FromArgb((int) themeColor.A, (int) themeColor.R, (int) green, (int) themeColor.B);
        }
        return true;
      case "greenMod":
        string str5 = value;
        if (!string.IsNullOrEmpty(str5))
        {
          double percentage = this.GetPercentage(str5);
          themeColor = Color.FromArgb((int) themeColor.A, (int) themeColor.R, (int) WordColor.ConvertbyModulation(themeColor.G, percentage), (int) themeColor.B);
        }
        return true;
      case "greenOff":
        string str6 = value;
        if (!string.IsNullOrEmpty(str6))
        {
          double percentage = this.GetPercentage(str6);
          themeColor = Color.FromArgb((int) themeColor.A, (int) themeColor.R, (int) WordColor.ConvertbyOffset(themeColor.G, percentage), (int) themeColor.B);
        }
        return true;
      case "red":
        string str7 = value;
        if (!string.IsNullOrEmpty(str7))
        {
          byte red = (byte) Math.Round((double) byte.MaxValue * WordColor.ConvertsLinearRGBtoRGB(this.GetPercentage(str7) / 100.0));
          themeColor = Color.FromArgb((int) themeColor.A, (int) red, (int) themeColor.G, (int) themeColor.B);
        }
        return true;
      case "redMod":
        string str8 = value;
        if (!string.IsNullOrEmpty(str8))
        {
          double percentage = this.GetPercentage(str8);
          themeColor = Color.FromArgb((int) themeColor.A, (int) WordColor.ConvertbyModulation(themeColor.R, percentage), (int) themeColor.G, (int) themeColor.B);
        }
        return true;
      case "redOff":
        string str9 = value;
        if (!string.IsNullOrEmpty(str9))
        {
          double percentage = this.GetPercentage(str9);
          themeColor = Color.FromArgb((int) themeColor.A, (int) WordColor.ConvertbyOffset(themeColor.R, percentage), (int) themeColor.G, (int) themeColor.B);
        }
        return true;
      case "hue":
        string s1 = value;
        if (!string.IsNullOrEmpty(s1))
        {
          double result;
          double.TryParse(s1, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          result /= 60000.0;
          WordColor.ConvertbyHue(ref themeColor, result);
        }
        return true;
      case "hueMod":
        string str10 = value;
        if (!string.IsNullOrEmpty(str10))
        {
          double ratio = this.GetPercentage(str10) / 100.0;
          WordColor.ConvertbyHueMod(ref themeColor, ratio);
        }
        return true;
      case "hueOff":
        string s2 = value;
        if (!string.IsNullOrEmpty(s2))
        {
          double result;
          double.TryParse(s2, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          result /= 60000.0;
          WordColor.ConvertbyHueOffset(ref themeColor, result);
        }
        return true;
      case "sat":
        string str11 = value;
        if (!string.IsNullOrEmpty(str11))
        {
          double percentage = this.GetPercentage(str11);
          WordColor.ConvertbySat(ref themeColor, percentage);
        }
        return true;
      case "satMod":
        string str12 = value;
        if (!string.IsNullOrEmpty(str12))
        {
          double percentage = this.GetPercentage(str12);
          WordColor.ConvertbySatMod(ref themeColor, percentage);
        }
        return true;
      case "satOff":
        string str13 = value;
        if (!string.IsNullOrEmpty(str13))
        {
          double percentage = this.GetPercentage(str13);
          WordColor.ConvertbySatOffset(ref themeColor, percentage);
        }
        return true;
      case "lum":
        string str14 = value;
        if (!string.IsNullOrEmpty(str14))
        {
          double percentage = this.GetPercentage(str14);
          WordColor.ConvertbyLum(ref themeColor, percentage);
        }
        return true;
      case "lumMod":
        string str15 = value;
        if (!string.IsNullOrEmpty(str15))
        {
          double percentage = this.GetPercentage(str15);
          WordColor.ConvertbyLumMod(ref themeColor, percentage);
        }
        return true;
      case "lumOff":
        string str16 = value;
        if (str16 != null)
        {
          double percentage = this.GetPercentage(str16);
          WordColor.ConvertbyLumOffset(ref themeColor, percentage);
        }
        return true;
      case "comp":
        themeColor = WordColor.ComplementColor(themeColor);
        return true;
      case "gamma":
        themeColor = WordColor.GammaColor(themeColor);
        return true;
      case "gray":
        themeColor = WordColor.GrayColor(themeColor);
        return true;
      case "invGamma":
        themeColor = WordColor.InverseGammaColor(themeColor);
        return true;
      case "inv":
        themeColor = WordColor.InverseColor(themeColor);
        return true;
      case "tint":
        string str17 = value;
        if (!string.IsNullOrEmpty(str17))
        {
          double tint = this.GetPercentage(str17) / 100.0;
          themeColor = WordColor.ConvertColorByTint(themeColor, tint);
        }
        return true;
      case "shade":
        string str18 = value;
        if (!string.IsNullOrEmpty(str18))
        {
          double shade = this.GetPercentage(str18) / 100.0;
          themeColor = WordColor.ConvertColorByShade(themeColor, shade);
        }
        return true;
      default:
        return false;
    }
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => new SizeF(this.Width, this.Height);

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    if (Entity.IsVerticalTextDirection(this.TextFrame.TextDirection))
      this.m_layoutInfo.IsVerticalText = true;
    if (this.Visible)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_textBody != null)
    {
      this.m_textBody.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal override void Close()
  {
    if (this.m_textBody != null)
    {
      this.m_textBody.Close();
      this.m_textBody = (WTextBody) null;
    }
    if (this.m_fillFormat != null)
    {
      this.m_fillFormat.Close();
      this.m_fillFormat = (FillFormat) null;
    }
    if (this.m_lineFormat != null)
    {
      this.m_lineFormat.Close();
      this.m_lineFormat = (LineFormat) null;
    }
    if (this.m_effectList != null)
    {
      for (int index = 0; index < this.m_effectList.Count; ++index)
        this.m_effectList[index]?.Close();
      this.m_effectList.Clear();
      this.m_effectList = (List<EffectFormat>) null;
    }
    if (this.m_styleProps != null)
    {
      this.m_styleProps.Clear();
      this.m_styleProps = (List<string>) null;
    }
    if (this.m_docx2007Props != null)
    {
      foreach (Stream stream in this.m_docx2007Props.Values)
        stream.Close();
      this.m_docx2007Props.Clear();
      this.m_docx2007Props = (Dictionary<string, Stream>) null;
    }
    if (this.m_relations != null)
    {
      this.m_relations.Clear();
      this.m_relations = (Dictionary<string, DictionaryEntry>) null;
    }
    if (this.m_imageRelations != null)
    {
      foreach (ImageRecord imageRecord in this.m_imageRelations.Values)
        imageRecord.Close();
      this.m_imageRelations.Clear();
      this.m_imageRelations = (Dictionary<string, ImageRecord>) null;
    }
    if (this.m_shapeStyleItems != null)
    {
      this.m_shapeStyleItems.Clear();
      this.m_shapeStyleItems = (List<ShapeStyleReference>) null;
    }
    if (this.m_shapeGuide != null)
    {
      this.m_shapeGuide.Clear();
      this.m_shapeGuide = (Dictionary<string, string>) null;
    }
    if (this.m_vmlPathPoints != null)
    {
      foreach (Path2D vmlPathPoint in this.m_vmlPathPoints)
        vmlPathPoint.Close();
      this.m_vmlPathPoints.Clear();
      this.m_vmlPathPoints = (List<Path2D>) null;
    }
    if (this.m_guideList != null)
    {
      this.m_guideList.Clear();
      this.m_guideList = (Dictionary<string, string>) null;
    }
    if (this.m_avList != null)
    {
      this.m_avList.Clear();
      this.m_avList = (Dictionary<string, string>) null;
    }
    if (this.Path2DList != null)
    {
      foreach (Path2D path2D in this.m_path2DList)
        path2D.Close();
      this.m_path2DList.Clear();
      this.m_path2DList = (List<Path2D>) null;
    }
    base.Close();
  }
}
