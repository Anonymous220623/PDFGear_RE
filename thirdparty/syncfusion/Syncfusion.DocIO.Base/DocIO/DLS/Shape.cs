// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Shape
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Shape : ShapeBase, IEntity, ILeafWidget, IWidget
{
  private AutoShapeType m_AutoShapeType;
  private WTextBody m_TextBody;
  private FillFormat m_FillFormat;
  private LineFormat m_LineFormat;
  private List<string> m_styleProps;
  public string ShapeTypeID;
  private Dictionary<string, DictionaryEntry> m_relations;
  private TextFrame m_TextFrame;
  private string m_Adjustments;
  private double m_arcSize;
  private Color m_fontRefColor = Color.Empty;
  private RectangleF m_textLayoutingBounds;
  private float m_rotation;
  private byte m_bFlags;
  private List<EffectFormat> m_effectList;
  private List<ShapeStyleReference> m_shapeStyleItems;
  private WPicture m_fallbackPic;
  private List<Path2D> m_vmlPathPoints;
  private Dictionary<string, string> m_guideList;
  private Dictionary<string, string> m_avList;
  private List<Path2D> m_path2DList;
  internal bool m_isVMLPathUpdated;
  internal Dictionary<string, Stream> m_docx2007Props;
  private Dictionary<string, ImageRecord> m_imageRelations;
  internal Dictionary<string, string> m_shapeGuide;

  internal List<Path2D> VMLPathPoints
  {
    get => this.m_vmlPathPoints;
    set => this.m_vmlPathPoints = value;
  }

  internal bool Is2007Shape
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal RectangleF TextLayoutingBounds
  {
    get => this.m_textLayoutingBounds;
    set => this.m_textLayoutingBounds = value;
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

  public float Rotation
  {
    get => this.m_rotation;
    set => this.m_rotation = value;
  }

  internal double ArcSize
  {
    get => this.m_arcSize;
    set => this.m_arcSize = value;
  }

  internal bool IsHorizontalRule
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal string Adjustments
  {
    get => this.m_Adjustments;
    set => this.m_Adjustments = value;
  }

  public TextFrame TextFrame
  {
    get
    {
      if (this.m_TextFrame == null)
        this.m_TextFrame = new TextFrame(this);
      return this.m_TextFrame;
    }
    set => this.m_TextFrame = value;
  }

  internal Color FontRefColor
  {
    get => this.m_fontRefColor;
    set => this.m_fontRefColor = value;
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

  internal Dictionary<string, ImageRecord> ImageRelations
  {
    get
    {
      if (this.m_imageRelations == null)
        this.m_imageRelations = new Dictionary<string, ImageRecord>();
      return this.m_imageRelations;
    }
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

  public AutoShapeType AutoShapeType
  {
    get => this.m_AutoShapeType;
    internal set => this.m_AutoShapeType = value;
  }

  public WTextBody TextBody
  {
    get
    {
      if (this.m_TextBody == null)
        this.m_TextBody = new WTextBody(this.Document, (Entity) this);
      return this.m_TextBody;
    }
    set => this.m_TextBody = value;
  }

  public LineFormat LineFormat
  {
    get
    {
      if (this.m_LineFormat == null)
        this.m_LineFormat = new LineFormat(this);
      return this.m_LineFormat;
    }
    set => this.m_LineFormat = value;
  }

  public FillFormat FillFormat
  {
    get
    {
      if (this.m_FillFormat == null)
        this.m_FillFormat = new FillFormat(this);
      return this.m_FillFormat;
    }
    set => this.m_FillFormat = value;
  }

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

  public override EntityType EntityType => EntityType.AutoShape;

  public bool FlipHorizontal
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  public bool FlipVertical
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsEffectStyleInline
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 254 | (value ? 1 : 0));
  }

  internal bool IsLineStyleInline
  {
    get => ((int) this.m_bFlags1 & 2) >> 1 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsFillStyleInline
  {
    get => ((int) this.m_bFlags1 & 4) >> 2 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsScenePropertiesInline
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  internal bool IsShapePropertiesInline
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
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

  internal Dictionary<string, DictionaryEntry> Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new Dictionary<string, DictionaryEntry>();
      return this.m_relations;
    }
  }

  internal WPicture FallbackPic
  {
    get => this.m_fallbackPic;
    set => this.m_fallbackPic = value;
  }

  internal List<Path2D> Path2DList
  {
    get => this.m_path2DList;
    set => this.m_path2DList = value;
  }

  internal Shape(IWordDocument doc)
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
    this.HorizontalOrigin = HorizontalOrigin.Column;
    this.VerticalOrigin = VerticalOrigin.Paragraph;
    this.HorizontalAlignment = ShapeHorizontalAlignment.None;
    this.VerticalAlignment = ShapeVerticalAlignment.None;
    this.WrapFormat.SetTextWrappingStyleValue(TextWrappingStyle.InFrontOfText);
    this.TextFrame.TextVerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Top;
  }

  public Shape(IWordDocument doc, AutoShapeType autoShapeType)
    : this(doc)
  {
    this.m_AutoShapeType = autoShapeType;
  }

  internal void InitializeVMLDefaultValues()
  {
    this.WrapFormat.TextWrappingStyle = TextWrappingStyle.Inline;
    this.FillFormat.Color = Color.White;
    this.LineFormat.ForeColor = Color.Black;
    this.LineFormat.Color = Color.Empty;
    this.WrapFormat.AllowOverlap = true;
  }

  internal override void Detach()
  {
    base.Detach();
    if (this.DeepDetached)
      return;
    this.Document.AutoShapeCollection.Remove(this);
    this.Document.FloatingItems.Remove((Entity) this);
  }

  internal override void AttachToDocument()
  {
    this.Document.AutoShapeCollection.Add(this);
    if (this.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      this.Document.FloatingItems.Add((Entity) this);
    this.IsCloned = false;
    if (this.TextBody == null)
      return;
    this.TextBody.AttachToDocument();
  }

  protected override object CloneImpl()
  {
    Shape shape = (Shape) base.CloneImpl();
    shape.IsCloned = true;
    if (this.m_TextBody != null)
    {
      shape.m_TextBody = this.m_TextBody.Clone() as WTextBody;
      shape.m_TextBody.SetOwner((OwnerHolder) shape);
    }
    this.CloneShapeFormat(shape);
    if (this.m_docx2007Props != null && this.m_docx2007Props.Count > 0)
      shape.Document.CloneProperties(this.Docx2007Props, ref shape.m_docx2007Props);
    return (object) shape;
  }

  internal void CloneShapeFormat(Shape shape)
  {
    bool flag = this.Document != null && this.Document.DocHasThemes;
    if (this.IsFillStyleInline && this.FillFormat != null)
    {
      shape.FillFormat = this.FillFormat.Clone();
      shape.IsFillStyleInline = true;
    }
    else if (flag && this.ShapeStyleReferences != null && this.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = this.ShapeStyleReferences[1].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.FillFormats.Count > styleRefIndex - 1)
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
          maxValue2 = uint.MaxValue;
        }
        shape.IsFillStyleInline = true;
      }
    }
    if (this.IsLineStyleInline && this.LineFormat != null)
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
          maxValue4 = uint.MaxValue;
        }
        shape.IsLineStyleInline = true;
      }
    }
    if (this.EffectList != null)
    {
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
    if (this.m_guideList != null)
      this.Document.CloneProperties(this.m_guideList, ref shape.m_guideList);
    if (this.m_avList != null)
      this.Document.CloneProperties(this.m_avList, ref shape.m_avList);
    if (this.Path2DList == null)
      return;
    List<Path2D> path2DList = new List<Path2D>();
    for (int index = 0; index < this.Path2DList.Count; ++index)
    {
      Path2D path2D = this.Path2DList[index].Clone();
      path2DList.Add(path2D);
    }
    shape.Path2DList = path2DList;
  }

  internal Color StyleColorTransform(
    List<DictionaryEntry> fillTransformation,
    Color themeColor,
    ref uint opacity)
  {
    bool flag = false;
    foreach (DictionaryEntry dictionaryEntry in fillTransformation)
    {
      string empty1 = string.Empty;
      switch (this.StartsWithExt(dictionaryEntry.Key.ToString(), "fgClr") || this.StartsWithExt(dictionaryEntry.Key.ToString(), "bgClr") ? dictionaryEntry.Key.ToString().Substring(5) : dictionaryEntry.Key.ToString())
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

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (this.m_TextBody != null)
      this.m_TextBody.CloneRelationsTo(doc, nextOwner);
    base.CloneRelationsTo(doc, nextOwner);
  }

  public void ApplyCharacterFormat(WCharacterFormat charFormat)
  {
    if (charFormat == null)
      return;
    this.SetParagraphItemCharacterFormat(charFormat);
  }

  void IWidget.InitLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    this.WrapFormat.IsWrappingBoundsAdded = false;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  private void SetShapeWidth(WSection section)
  {
    if ((double) this.RelativeWidth == 0.0)
      return;
    switch (this.RelativeWidthHorizontalOrigin)
    {
      case HorizontalOrigin.Page:
        this.Width = section.PageSetup.PageSize.Width * (this.RelativeWidth / 100f);
        break;
      case HorizontalOrigin.LeftMargin:
      case HorizontalOrigin.InsideMargin:
        this.Width = Layouter.GetLeftMargin(section) * (this.RelativeWidth / 100f);
        break;
      case HorizontalOrigin.RightMargin:
      case HorizontalOrigin.OutsideMargin:
        this.Width = Layouter.GetRightMargin(section) * (this.RelativeWidth / 100f);
        break;
      default:
        this.Width = (float) (((double) section.PageSetup.PageSize.Width - (double) Layouter.GetLeftMargin(section) - (double) Layouter.GetRightMargin(section)) * ((double) this.RelativeWidth / 100.0));
        break;
    }
  }

  private void SetShapeHeight(WSection section)
  {
    if ((double) this.RelativeHeight == 0.0)
      return;
    switch (this.RelativeHeightVerticalOrigin)
    {
      case VerticalOrigin.Page:
        this.Height = section.PageSetup.PageSize.Height * (this.RelativeHeight / 100f);
        break;
      case VerticalOrigin.TopMargin:
      case VerticalOrigin.InsideMargin:
        this.Height = (float) (((double) section.PageSetup.Margins.Top + (section.Document.DOP.GutterAtTop ? (double) section.PageSetup.Margins.Gutter : 0.0)) * ((double) this.RelativeHeight / 100.0));
        break;
      case VerticalOrigin.BottomMargin:
      case VerticalOrigin.OutsideMargin:
        this.Height = section.PageSetup.Margins.Bottom * (this.RelativeHeight / 100f);
        break;
      default:
        this.Height = (float) (((double) section.PageSetup.PageSize.Height - (double) section.PageSetup.Margins.Top - (section.Document.DOP.GutterAtTop ? (double) section.PageSetup.Margins.Gutter : 0.0) - (double) section.PageSetup.Margins.Bottom) * ((double) this.RelativeHeight / 100.0));
        break;
    }
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    if (this.IsHorizontalRule && (double) this.WidthScale != 0.0)
    {
      Entity ownerSection = this.GetOwnerSection((Entity) this);
      if (ownerSection is WSection && ownerSection != null)
        return new SizeF((ownerSection as WSection).PageSetup.ClientWidth * (this.WidthScale / 100f), this.Height);
    }
    float width = this.Width;
    float height = this.Height;
    if ((double) this.TextFrame.WidthRelativePercent != 0.0)
      width = this.GetWidthRelativeToPercent(true);
    if ((double) this.TextFrame.HeightRelativePercent != 0.0)
      height = this.GetHeightRelativeToPercent(true);
    return new SizeF(width, height);
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    Entity ownerSection = this.GetOwnerSection((Entity) this);
    if (ownerSection is WSection && ownerSection != null)
    {
      if (this.IsRelativeWidth)
        this.SetShapeWidth(ownerSection as WSection);
      if (this.IsRelativeHeight)
        this.SetShapeHeight(ownerSection as WSection);
    }
    WParagraph wparagraph = this.OwnerParagraph;
    if (this.Owner is InlineContentControl || this.Owner is XmlParagraphItem)
      wparagraph = this.GetOwnerParagraphValue();
    if (wparagraph.IsInCell && ((IWidget) wparagraph).LayoutInfo.IsClipped)
      this.m_layoutInfo.IsClipped = true;
    if (Entity.IsVerticalTextDirection(this.TextFrame.TextDirection))
      this.m_layoutInfo.IsVerticalText = true;
    if (this.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkipBottomAlign = true;
    if (this.ParaItemCharFormat.Hidden)
      this.m_layoutInfo.IsSkip = true;
    if (!this.Visible && this.GetTextWrappingStyle() != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkip = true;
    if (!this.IsDeleteRevision || this.Document.RevisionOptions.ShowDeletedText)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_TextBody != null)
    {
      this.m_TextBody.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal bool IsNoNeedToConsiderLineWidth() => !this.LineFormat.Line && this.Is2007Shape;

  internal Dictionary<string, string> GetGuideList()
  {
    return this.m_guideList ?? (this.m_guideList = new Dictionary<string, string>());
  }

  internal Dictionary<string, string> GetAvList()
  {
    return this.m_avList ?? (this.m_avList = new Dictionary<string, string>());
  }

  internal void SetGuideList(Dictionary<string, string> value) => this.m_guideList = value;

  internal void SetAvList(Dictionary<string, string> value) => this.m_avList = value;

  internal override void Close()
  {
    if (this.m_TextBody != null)
    {
      this.m_TextBody.Close();
      this.m_TextBody = (WTextBody) null;
    }
    if (this.m_FillFormat != null)
    {
      this.m_FillFormat.Close();
      this.m_FillFormat = (FillFormat) null;
    }
    if (this.m_LineFormat != null)
    {
      this.m_LineFormat.Close();
      this.m_LineFormat = (LineFormat) null;
    }
    if (this.m_effectList != null)
    {
      this.m_effectList.Clear();
      this.m_effectList = (List<EffectFormat>) null;
    }
    if (this.m_styleProps != null)
    {
      this.m_styleProps.Clear();
      this.m_styleProps = (List<string>) null;
    }
    if (this.m_relations != null)
    {
      this.m_relations.Clear();
      this.m_relations = (Dictionary<string, DictionaryEntry>) null;
    }
    if (this.m_shapeStyleItems != null)
    {
      this.m_shapeStyleItems.Clear();
      this.m_shapeStyleItems = (List<ShapeStyleReference>) null;
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

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  internal byte[] GetAsImage()
  {
    try
    {
      DocumentLayouter documentLayouter = new DocumentLayouter();
      byte[] asImage = documentLayouter.ConvertAsImage((IWidget) this);
      documentLayouter.Close();
      return asImage;
    }
    catch
    {
      return (byte[]) null;
    }
  }
}
