// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.GroupShape
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
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class GroupShape : ShapeBase, IEntity, ILeafWidget, IWidget
{
  private FillFormat m_fillFormat;
  private LineFormat m_lineFormat;
  private float m_rotation;
  internal bool? flipH;
  internal bool? flipV;
  private List<EffectFormat> m_effectList;
  private ChildShapeCollection m_childShapes;
  private byte m_bFlags;
  private List<ShapeStyleReference> m_shapeStyleItems;
  private AutoShapeType m_autoShapeType;
  private float m_xValue;
  private float m_yValue;
  private float m_extentXValue;
  private float m_extentYValue;
  internal Dictionary<string, Stream> m_docx2007Props;
  private float m_leftPosition;
  private float m_topPosition;
  private Dictionary<string, DictionaryEntry> m_relations;
  private Dictionary<string, ImageRecord> m_imageRelations;
  private List<string> m_styleProps;

  internal ChildShapeCollection ChildShapes
  {
    get
    {
      if (this.m_childShapes == null)
        this.m_childShapes = new ChildShapeCollection(this.Document);
      return this.m_childShapes;
    }
    set => this.m_childShapes = value;
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

  internal Dictionary<string, ImageRecord> ImageRelations
  {
    get
    {
      if (this.m_imageRelations == null)
        this.m_imageRelations = new Dictionary<string, ImageRecord>();
      return this.m_imageRelations;
    }
  }

  public float Rotation
  {
    get => this.m_rotation;
    set => this.m_rotation = value;
  }

  public bool FlipHorizontal
  {
    get => this.flipH.HasValue && this.flipH.Value;
    set => this.flipH = new bool?(value);
  }

  public bool FlipVertical
  {
    get => this.flipV.HasValue && this.flipV.Value;
    set => this.flipV = new bool?(value);
  }

  public override EntityType EntityType => EntityType.GroupShape;

  internal AutoShapeType AutoShapeType
  {
    get => this.m_autoShapeType;
    set => this.m_autoShapeType = value;
  }

  internal LineFormat LineFormat
  {
    get
    {
      if (this.m_lineFormat == null)
        this.m_lineFormat = new LineFormat((ShapeBase) this);
      return this.m_lineFormat;
    }
    set => this.m_lineFormat = value;
  }

  internal bool IsScenePropertiesInline
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsLineStyleInline
  {
    get => ((int) this.m_bFlags1 & 2) >> 1 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 253 | (value ? 1 : 0) << 1);
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

  internal bool IsEffectStyleInline
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 254 | (value ? 1 : 0));
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

  internal bool Is2007Shape
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsShapePropertiesInline
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal FillFormat FillFormat
  {
    get
    {
      if (this.m_fillFormat == null)
        this.m_fillFormat = new FillFormat((ShapeBase) this);
      return this.m_fillFormat;
    }
    set => this.m_fillFormat = value;
  }

  internal bool IsFillStyleInline
  {
    get => ((int) this.m_bFlags1 & 4) >> 2 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 251 | (value ? 1 : 0) << 2);
  }

  internal float X
  {
    get => this.m_xValue;
    set => this.m_xValue = value;
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

  internal float Y
  {
    get => this.m_yValue;
    set => this.m_yValue = value;
  }

  internal float ExtentXValue
  {
    get => this.m_extentXValue;
    set => this.m_extentXValue = value;
  }

  internal float ExtentYValue
  {
    get => this.m_extentYValue;
    set => this.m_extentYValue = value;
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

  internal List<string> DocxStyleProps
  {
    get
    {
      if (this.m_styleProps == null)
        this.m_styleProps = new List<string>();
      return this.m_styleProps;
    }
  }

  internal void Add(ParagraphItem[] childShapes)
  {
    foreach (ParagraphItem childShape in childShapes)
      this.Add(childShape);
  }

  public void Add(ParagraphItem childShape)
  {
    if (!this.Document.IsOpening && childShape.EntityType != EntityType.ChildShape && childShape.GetTextWrappingStyle() == TextWrappingStyle.Inline)
      throw new Exception("Inline objects cannot be grouped");
    ChildShape childShape1 = (ChildShape) null;
    switch (childShape.EntityType)
    {
      case EntityType.Picture:
        childShape1 = this.ConvertPictureToChildShape(childShape as WPicture);
        break;
      case EntityType.TextBox:
        childShape1 = this.ConvertTextboxToChildShape(childShape as WTextBox);
        break;
      case EntityType.Chart:
        childShape1 = this.ConvertChartToChildShape(childShape as WChart);
        break;
      case EntityType.AutoShape:
        childShape1 = this.ConvertShapeToChildShape(childShape as Shape);
        break;
      case EntityType.GroupShape:
        ChildGroupShape childGroupShape = this.ConvertGroupShapeToChildGroupShape(childShape as GroupShape);
        childGroupShape.SetOwner((OwnerHolder) this);
        this.ChildShapes.Add((ChildShape) childGroupShape);
        break;
      case EntityType.ChildShape:
        childShape1 = childShape as ChildShape;
        break;
      case EntityType.ChildGroupShape:
        childShape1 = (ChildShape) (childShape as ChildGroupShape);
        break;
      default:
        throw new InvalidOperationException($"Cannot add object of type {childShape.EntityType} to Group Shape");
    }
    if (childShape1 == null)
      return;
    childShape1.SetOwner((OwnerHolder) this);
    this.ChildShapes.Add(childShape1);
  }

  internal ChildShape ConvertShapeToChildShape(Shape shape)
  {
    ChildShape owner = new ChildShape((IWordDocument) this.Document);
    owner.ElementType = EntityType.AutoShape;
    owner.AlternativeText = shape.AlternativeText;
    owner.ArcSize = shape.ArcSize;
    owner.AutoShapeType = shape.AutoShapeType;
    owner.Title = shape.Title;
    owner.Width = shape.Width;
    owner.WidthScale = shape.WidthScale;
    owner.Height = shape.Height;
    owner.HeightScale = shape.HeightScale;
    owner.Rotation = shape.Rotation;
    owner.Name = shape.Name;
    owner.Adjustments = shape.Adjustments;
    owner.FlipHorizantal = shape.FlipHorizontal;
    owner.FlipVertical = shape.FlipVertical;
    owner.FontRefColor = shape.FontRefColor;
    owner.Is2007Shape = shape.Is2007Shape;
    owner.LayoutInCell = shape.LayoutInCell;
    owner.UseNoShadeHR = shape.UseNoShadeHR;
    owner.UseStandardColorHR = shape.UseStandardColorHR;
    owner.TextFrame.HasInternalMargin = shape.TextFrame.HasInternalMargin;
    owner.TextFrame.HeightOrigin = shape.TextFrame.HeightOrigin;
    owner.TextFrame.HeightRelativePercent = shape.TextFrame.HeightRelativePercent;
    owner.TextFrame.HorizontalRelativePercent = shape.TextFrame.HorizontalRelativePercent;
    owner.TextFrame.NoAutoFit = shape.TextFrame.NoAutoFit;
    owner.TextFrame.NormalAutoFit = shape.TextFrame.NormalAutoFit;
    owner.TextFrame.NoWrap = shape.TextFrame.NoWrap;
    owner.TextFrame.ShapeAutoFit = shape.TextFrame.ShapeAutoFit;
    owner.TextFrame.TextDirection = shape.TextFrame.TextDirection;
    owner.TextFrame.TextVerticalAlignment = shape.TextFrame.TextVerticalAlignment;
    owner.TextFrame.VerticalRelativePercent = shape.TextFrame.VerticalRelativePercent;
    owner.TextFrame.WidthOrigin = shape.TextFrame.WidthOrigin;
    owner.TextFrame.WidthRelativePercent = shape.TextFrame.WidthRelativePercent;
    owner.TextFrame.InternalMargin.Bottom = shape.TextFrame.InternalMargin.Bottom;
    owner.TextFrame.InternalMargin.Left = shape.TextFrame.InternalMargin.Left;
    owner.TextFrame.InternalMargin.Right = shape.TextFrame.InternalMargin.Right;
    owner.TextFrame.InternalMargin.Top = shape.TextFrame.InternalMargin.Top;
    owner.LineFormat = shape.LineFormat.Clone();
    owner.X = owner.HorizontalPosition = shape.HorizontalPosition;
    owner.Y = owner.VerticalPosition = shape.VerticalPosition;
    owner.CloneShapeFormat(shape);
    if (shape.TextBody != null)
    {
      owner.TextBody = shape.TextBody.Clone() as WTextBody;
      owner.TextBody.SetOwner((OwnerHolder) owner);
    }
    if (shape.m_docxProps != null && shape.m_docxProps.Count > 0)
      this.Document.CloneProperties(shape.DocxProps, ref owner.m_docxProps);
    if (shape.m_docx2007Props != null && shape.m_docx2007Props.Count > 0)
      this.Document.CloneProperties(shape.Docx2007Props, ref owner.m_docx2007Props);
    if (shape.m_shapeGuide != null && shape.m_shapeGuide.Count > 0)
      this.Document.CloneProperties(shape.ShapeGuide, ref owner.m_shapeGuide);
    return owner;
  }

  internal ChildShape ConvertTextboxToChildShape(WTextBox textBox)
  {
    ChildShape owner = new ChildShape((IWordDocument) this.Document);
    owner.ElementType = EntityType.TextBox;
    owner.SetOwner((OwnerHolder) this);
    owner.IsTextBoxShape = true;
    owner.Name = textBox.Name;
    owner.Visible = textBox.Visible;
    owner.Height = textBox.TextBoxFormat.Height;
    owner.Width = textBox.TextBoxFormat.Width;
    owner.AutoShapeType = AutoShapeType.Rectangle;
    owner.X = owner.HorizontalPosition = textBox.TextBoxFormat.HorizontalPosition;
    owner.Y = owner.VerticalPosition = textBox.TextBoxFormat.VerticalPosition;
    owner.TextFrame.HeightOrigin = textBox.TextBoxFormat.HeightOrigin;
    owner.TextFrame.HeightRelativePercent = textBox.TextBoxFormat.HeightRelativePercent;
    owner.TextFrame.HorizontalRelativePercent = textBox.TextBoxFormat.HorizontalRelativePercent;
    owner.TextFrame.InternalMargin.Bottom = textBox.TextBoxFormat.InternalMargin.Bottom;
    owner.TextFrame.InternalMargin.Left = textBox.TextBoxFormat.InternalMargin.Left;
    owner.TextFrame.InternalMargin.Right = textBox.TextBoxFormat.InternalMargin.Right;
    owner.TextFrame.InternalMargin.Top = textBox.TextBoxFormat.InternalMargin.Top;
    owner.TextFrame.TextDirection = textBox.TextBoxFormat.TextDirection;
    owner.TextFrame.TextVerticalAlignment = textBox.TextBoxFormat.TextVerticalAlignment;
    owner.TextFrame.VerticalRelativePercent = textBox.TextBoxFormat.VerticalRelativePercent;
    owner.TextFrame.WidthOrigin = textBox.TextBoxFormat.WidthOrigin;
    owner.TextFrame.WidthRelativePercent = textBox.TextBoxFormat.WidthRelativePercent;
    owner.TextFrame.ShapeAutoFit = textBox.TextBoxFormat.AutoFit;
    owner.FontRefColor = textBox.TextBoxFormat.TextThemeColor;
    owner.Rotation = textBox.TextBoxFormat.Rotation;
    owner.FlipVertical = textBox.TextBoxFormat.FlipVertical;
    owner.FlipHorizantal = textBox.TextBoxFormat.FlipHorizontal;
    if (textBox.TextBoxFormat.FillEfects.Type == BackgroundType.Color)
    {
      owner.FillFormat.Fill = true;
      owner.FillFormat.Color = textBox.TextBoxFormat.FillColor;
      owner.FillFormat.IsDefaultFill = false;
      owner.IsFillStyleInline = true;
    }
    else if (textBox.TextBoxFormat.FillEfects.Type == BackgroundType.Gradient)
    {
      owner.FillFormat.FillType = FillType.FillGradient;
      owner.FillFormat.IsDefaultFill = false;
      owner.IsFillStyleInline = true;
      owner.FillFormat.ForeColor = textBox.TextBoxFormat.FillEfects.Color;
      owner.FillFormat.Color = textBox.TextBoxFormat.FillEfects.Gradient.Color2;
    }
    else if (textBox.TextBoxFormat.FillEfects.Type == BackgroundType.Picture)
    {
      owner.FillFormat.FillType = FillType.FillPicture;
      owner.FillFormat.ImageRecord = new ImageRecord(this.Document, textBox.TextBoxFormat.FillEfects.ImageBytes);
      owner.FillFormat.IsDefaultFill = false;
      owner.IsFillStyleInline = true;
    }
    else
    {
      owner.FillFormat.Fill = false;
      owner.FillFormat.Color = Color.Empty;
      owner.FillFormat.IsDefaultFill = false;
    }
    owner.LineFormat.Line = !textBox.TextBoxFormat.NoLine;
    owner.LineFormat.Color = textBox.TextBoxFormat.LineColor;
    owner.LineFormat.Weight = textBox.TextBoxFormat.LineWidth;
    owner.LineFormat.DashStyle = textBox.TextBoxFormat.LineDashing;
    if (textBox.TextBoxBody != null)
    {
      owner.TextBody = textBox.TextBoxBody.Clone() as WTextBody;
      owner.TextBody.SetOwner((OwnerHolder) owner);
    }
    if (textBox.DocxProps != null && textBox.DocxProps.Count > 0)
      this.Document.CloneProperties(textBox.DocxProps, ref owner.m_docxProps);
    return owner;
  }

  internal ChildShape ConvertChartToChildShape(WChart chart)
  {
    ChildShape childShape = new ChildShape((IWordDocument) this.Document);
    childShape.ElementType = EntityType.Chart;
    childShape.SetOwner((OwnerHolder) this);
    childShape.Chart = chart;
    childShape.Width = chart.Width;
    childShape.Height = chart.Height;
    childShape.WidthScale = chart.WidthScale;
    childShape.HeightScale = chart.HeightScale;
    childShape.Title = chart.Title;
    childShape.AlternativeText = chart.AlternativeText;
    childShape.Name = chart.Name;
    childShape.LayoutInCell = chart.LayoutInCell;
    childShape.X = childShape.HorizontalPosition = chart.HorizontalPosition;
    childShape.Y = childShape.VerticalPosition = chart.VerticalPosition;
    if (chart.m_docxProps != null && chart.m_docxProps.Count > 0)
      this.Document.CloneProperties(chart.DocxProps, ref childShape.m_docxProps);
    return childShape;
  }

  internal ChildShape ConvertPictureToChildShape(WPicture picture)
  {
    ChildShape childShape = new ChildShape((IWordDocument) this.Document);
    childShape.ElementType = EntityType.Picture;
    childShape.AlternativeText = picture.AlternativeText;
    childShape.FillFormat = picture.FillFormat.Clone();
    childShape.FillFormat.SourceRectangle = picture.FillRectangle.Clone();
    childShape.Height = picture.Height;
    if ((double) picture.HeightScale > 0.0)
      childShape.HeightScale = picture.HeightScale;
    childShape.Width = picture.Width;
    if ((double) picture.WidthScale > 0.0)
      childShape.WidthScale = picture.WidthScale;
    childShape.X = childShape.HorizontalPosition = picture.HorizontalPosition;
    childShape.Y = childShape.VerticalPosition = picture.VerticalPosition;
    childShape.Title = picture.Title;
    childShape.Rotation = (float) (int) picture.Rotation;
    childShape.Name = picture.Name;
    childShape.LayoutInCell = picture.LayoutInCell;
    ImageRecord imageRecord = new ImageRecord(this.Document, picture.ImageRecord);
    childShape.m_imageRecord = imageRecord;
    if (picture.ImageRecord != null)
      childShape.m_imageRecord.ImageId = picture.ImageRecord.ImageId;
    childShape.FlipHorizantal = picture.FlipHorizontal;
    childShape.FlipVertical = picture.FlipVertical;
    if (picture.m_docxVisualShapeProps != null && picture.m_docxVisualShapeProps.Count > 0)
      this.Document.CloneProperties(picture.DocxVisualShapeProps, ref childShape.m_pictureProps);
    this.Document.HasPicture = true;
    return childShape;
  }

  internal ChildGroupShape ConvertGroupShapeToChildGroupShape(GroupShape groupShape)
  {
    ChildGroupShape owner = new ChildGroupShape((IWordDocument) this.Document);
    owner.ElementType = EntityType.ChildGroupShape;
    owner.AlternativeText = groupShape.AlternativeText;
    owner.AutoShapeType = groupShape.AutoShapeType;
    if (groupShape.m_docxProps != null && groupShape.m_docxProps.Count > 0)
      this.Document.CloneProperties(groupShape.DocxProps, ref owner.m_docxProps);
    if (groupShape.m_docx2007Props != null && groupShape.m_docx2007Props.Count > 0)
      this.Document.CloneProperties(groupShape.Docx2007Props, ref owner.m_docx2007Props);
    for (int index = 0; index < groupShape.DocxStyleProps.Count; ++index)
      owner.DocxStyleProps.Add(groupShape.DocxStyleProps[index]);
    owner.EffectList = groupShape.EffectList;
    owner.ExtentXValue = groupShape.ExtentXValue;
    owner.ExtentYValue = groupShape.ExtentYValue;
    owner.FillFormat = groupShape.FillFormat.Clone();
    owner.Height = groupShape.Height;
    owner.HeightScale = groupShape.HeightScale;
    owner.Is2007Shape = groupShape.Is2007Shape;
    owner.IsChangedCFormat = groupShape.IsChangedCFormat;
    owner.IsDetachedTextChanged = groupShape.IsDetachedTextChanged;
    owner.IsEffectStyleInline = groupShape.IsEffectStyleInline;
    owner.IsFillStyleInline = groupShape.IsFillStyleInline;
    owner.IsLineStyleInline = groupShape.IsLineStyleInline;
    owner.IsMappedItem = groupShape.IsMappedItem;
    owner.IsScenePropertiesInline = groupShape.IsScenePropertiesInline;
    owner.IsShapePropertiesInline = groupShape.IsShapePropertiesInline;
    owner.LayoutInCell = groupShape.LayoutInCell;
    owner.LeftMargin = groupShape.LeftMargin;
    owner.LineFormat = groupShape.LineFormat.Clone();
    owner.Name = groupShape.Name;
    owner.OffsetXValue = groupShape.X;
    owner.OffsetYValue = groupShape.Y;
    owner.ParaItemCharFormat.ApplyBase((FormatBase) groupShape.ParaItemCharFormat);
    owner.Rotation = groupShape.Rotation;
    owner.ShapeID = groupShape.ShapeID;
    owner.SkipDocxItem = groupShape.SkipDocxItem;
    owner.Title = groupShape.Title;
    owner.TopMargin = groupShape.TopMargin;
    owner.Visible = groupShape.Visible;
    owner.Width = groupShape.Width;
    owner.WidthScale = groupShape.WidthScale;
    owner.HorizontalPosition = groupShape.HorizontalPosition;
    owner.VerticalPosition = groupShape.VerticalPosition;
    foreach (ShapeStyleReference shapeStyleReference in groupShape.ShapeStyleReferences)
      owner.ShapeStyleReferences.Add(shapeStyleReference);
    foreach (KeyValuePair<string, DictionaryEntry> relation in groupShape.Relations)
      owner.Relations.Add(relation.Key, relation.Value);
    foreach (KeyValuePair<int, object> keyValuePair in groupShape.PropertiesHash)
      owner.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    for (int index = 0; index < groupShape.ChildShapes.Count; ++index)
    {
      ChildShape childShape = groupShape.ChildShapes[index].Clone() as ChildShape;
      childShape.SetOwner((OwnerHolder) owner);
      owner.ChildShapes.Add(childShape);
    }
    return owner;
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
    if (this.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      this.Document.FloatingItems.Add((Entity) this);
    this.IsCloned = false;
  }

  internal void UpdatePositionForGroupShapeAndChildShape()
  {
    this.CalculateGroupShapeBounds();
    foreach (ChildShape childShape in (CollectionImpl) this.ChildShapes)
    {
      childShape.X = childShape.HorizontalPosition - this.HorizontalPosition;
      childShape.Y = childShape.VerticalPosition - this.VerticalPosition;
    }
  }

  internal void CalculateGroupShapeBounds()
  {
    float num1 = 0.0f;
    for (int index = 0; index < this.ChildShapes.Count; ++index)
    {
      float horizontalPosition = this.ChildShapes[index].HorizontalPosition;
      if (index == 0)
        num1 = horizontalPosition;
      else if ((double) horizontalPosition < (double) num1)
        num1 = horizontalPosition;
    }
    float num2 = 0.0f;
    for (int index = 0; index < this.ChildShapes.Count; ++index)
    {
      float verticalPosition = this.ChildShapes[index].VerticalPosition;
      if (index == 0)
        num2 = verticalPosition;
      else if ((double) verticalPosition < (double) num2)
        num2 = verticalPosition;
    }
    float num3 = 0.0f;
    for (int index = 0; index < this.ChildShapes.Count; ++index)
    {
      float num4 = this.ChildShapes[index].VerticalPosition + this.ChildShapes[index].Height;
      if (index == 0)
        num3 = num4;
      else if ((double) num4 > (double) num3)
        num3 = num4;
    }
    float num5 = 0.0f;
    for (int index = 0; index < this.ChildShapes.Count; ++index)
    {
      float num6 = this.ChildShapes[index].HorizontalPosition + this.ChildShapes[index].Width;
      if (index == 0)
        num5 = num6;
      else if ((double) num6 > (double) num5)
        num5 = num6;
    }
    this.HorizontalPosition = num1;
    this.VerticalPosition = num2;
    this.Width = num5 - num1;
    this.Height = num3 - num2;
    this.ExtentXValue = this.Width;
    this.ExtentYValue = this.Height;
  }

  public ParagraphItem[] Ungroup()
  {
    ParagraphItem[] paragraphItemArray = this.Ungroup(this.ChildShapes, new PointF(this.HorizontalPosition, this.VerticalPosition));
    int inOwnerCollection = this.GetIndexInOwnerCollection();
    WParagraph ownerParagraph = this.OwnerParagraph;
    this.RemoveSelf();
    for (int index = 0; index < paragraphItemArray.Length && inOwnerCollection <= ownerParagraph.ChildEntities.Count; ++index)
    {
      ownerParagraph.ChildEntities.Insert(inOwnerCollection, (IEntity) paragraphItemArray[index]);
      ++inOwnerCollection;
    }
    return paragraphItemArray;
  }

  internal ParagraphItem[] Ungroup(ChildShapeCollection childShapes, PointF positionOfGroupShape)
  {
    List<ParagraphItem> paragraphItemList = new List<ParagraphItem>();
    MarginsF margins = this.Document.LastSection.PageSetup.Margins;
    for (int index = 0; index < childShapes.Count; ++index)
    {
      switch (childShapes[index].ElementType)
      {
        case EntityType.Picture:
          WPicture picture = this.ConvertChildShapeToPicture(childShapes[index]);
          picture.HorizontalOrigin = HorizontalOrigin.Page;
          picture.VerticalOrigin = VerticalOrigin.Page;
          picture.HorizontalPosition = childShapes[index].X + positionOfGroupShape.X;
          picture.VerticalPosition = childShapes[index].Y + positionOfGroupShape.Y;
          paragraphItemList.Add((ParagraphItem) picture);
          break;
        case EntityType.TextBox:
          WTextBox textbox = this.ConvertChildShapeToTextbox(childShapes[index]);
          textbox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Page;
          textbox.TextBoxFormat.VerticalOrigin = VerticalOrigin.Page;
          textbox.TextBoxFormat.HorizontalPosition = childShapes[index].X + positionOfGroupShape.X;
          textbox.TextBoxFormat.VerticalPosition = childShapes[index].Y + positionOfGroupShape.Y;
          paragraphItemList.Add((ParagraphItem) textbox);
          break;
        case EntityType.Chart:
          WChart chart = this.ConvertChildShapeToChart(childShapes[index]);
          chart.HorizontalOrigin = HorizontalOrigin.Page;
          chart.VerticalOrigin = VerticalOrigin.Page;
          chart.HorizontalPosition = childShapes[index].X + positionOfGroupShape.X;
          chart.VerticalPosition = childShapes[index].Y + positionOfGroupShape.Y;
          paragraphItemList.Add((ParagraphItem) chart);
          break;
        case EntityType.AutoShape:
          Shape shape = this.ConvertChildShapeToShape(childShapes[index]);
          shape.HorizontalOrigin = HorizontalOrigin.Page;
          shape.VerticalOrigin = VerticalOrigin.Page;
          shape.HorizontalPosition = childShapes[index].X + positionOfGroupShape.X;
          shape.VerticalPosition = childShapes[index].Y + positionOfGroupShape.Y;
          paragraphItemList.Add((ParagraphItem) shape);
          break;
        case EntityType.ChildGroupShape:
          GroupShape groupShape = this.ConvertChildGroupShapeToGroupShape(this.ChildShapes[index] as ChildGroupShape);
          groupShape.HorizontalPosition = childShapes[index].X + positionOfGroupShape.X;
          groupShape.VerticalPosition = childShapes[index].Y + positionOfGroupShape.Y;
          paragraphItemList.Add((ParagraphItem) groupShape);
          break;
      }
    }
    return paragraphItemList.ToArray();
  }

  internal Shape ConvertChildShapeToShape(ChildShape childShape)
  {
    Shape shape = new Shape((IWordDocument) this.Document);
    shape.AlternativeText = childShape.AlternativeText;
    shape.ArcSize = childShape.ArcSize;
    shape.WrapFormat.AllowOverlap = true;
    shape.AutoShapeType = childShape.AutoShapeType;
    shape.Title = childShape.Title;
    shape.Width = childShape.Width;
    shape.WidthScale = childShape.WidthScale;
    shape.Height = childShape.Height;
    shape.HeightScale = childShape.HeightScale;
    shape.Rotation = (float) (int) childShape.Rotation;
    shape.Name = childShape.Name;
    shape.Adjustments = childShape.Adjustments;
    shape.FlipHorizontal = childShape.FlipHorizantal;
    shape.FlipVertical = childShape.FlipVertical;
    shape.FontRefColor = childShape.FontRefColor;
    shape.Is2007Shape = childShape.Is2007Shape;
    shape.LayoutInCell = childShape.LayoutInCell;
    shape.UseNoShadeHR = childShape.UseNoShadeHR;
    shape.UseStandardColorHR = childShape.UseStandardColorHR;
    shape.TextFrame.HasInternalMargin = childShape.TextFrame.HasInternalMargin;
    shape.TextFrame.HeightOrigin = childShape.TextFrame.HeightOrigin;
    shape.TextFrame.HeightRelativePercent = childShape.TextFrame.HeightRelativePercent;
    shape.TextFrame.HorizontalRelativePercent = childShape.TextFrame.HorizontalRelativePercent;
    shape.TextFrame.NoAutoFit = childShape.TextFrame.NoAutoFit;
    shape.TextFrame.NormalAutoFit = childShape.TextFrame.NormalAutoFit;
    shape.TextFrame.NoWrap = childShape.TextFrame.NoWrap;
    shape.TextFrame.ShapeAutoFit = childShape.TextFrame.ShapeAutoFit;
    shape.TextFrame.TextDirection = childShape.TextFrame.TextDirection;
    shape.TextFrame.TextVerticalAlignment = childShape.TextFrame.TextVerticalAlignment;
    shape.TextFrame.VerticalRelativePercent = childShape.TextFrame.VerticalRelativePercent;
    shape.TextFrame.WidthOrigin = childShape.TextFrame.WidthOrigin;
    shape.TextFrame.WidthRelativePercent = childShape.TextFrame.WidthRelativePercent;
    shape.TextFrame.InternalMargin.Bottom = childShape.TextFrame.InternalMargin.Bottom;
    shape.TextFrame.InternalMargin.Left = childShape.TextFrame.InternalMargin.Left;
    shape.TextFrame.InternalMargin.Right = childShape.TextFrame.InternalMargin.Right;
    shape.TextFrame.InternalMargin.Top = childShape.TextFrame.InternalMargin.Top;
    childShape.CloneChildShapeFormatToShape(shape);
    shape.FillFormat.Fill = childShape.FillFormat.Fill;
    shape.HorizontalOrigin = HorizontalOrigin.Margin;
    shape.VerticalOrigin = VerticalOrigin.Margin;
    shape.LineFormat.Line = childShape.LineFormat.Line;
    shape.LineFormat.Weight = childShape.LineFormat.Weight;
    if (childShape.Is2007Shape && childShape.m_isVMLPathUpdated)
      shape.VMLPathPoints = childShape.VMLPathPoints;
    if (childShape.Path2DList != null && childShape.Path2DList.Count > 0)
      shape.Path2DList = childShape.Path2DList;
    if (childShape.GetAvList().Count > 0)
      shape.SetAvList(childShape.GetAvList());
    if (childShape.GetGuideList().Count > 0)
      shape.SetGuideList(childShape.GetGuideList());
    if (childShape.TextBody != null)
    {
      shape.TextBody = childShape.TextBody.Clone() as WTextBody;
      shape.TextBody.SetOwner((OwnerHolder) shape);
    }
    if (childShape.m_docxProps != null && childShape.m_docxProps.Count > 0)
      this.Document.CloneProperties(childShape.DocxProps, ref shape.m_docxProps);
    if (childShape.m_docx2007Props != null && childShape.m_docx2007Props.Count > 0)
      this.Document.CloneProperties(childShape.Docx2007Props, ref shape.m_docx2007Props);
    if (childShape.m_shapeGuide != null && childShape.m_shapeGuide.Count > 0)
      this.Document.CloneProperties(childShape.ShapeGuide, ref shape.m_shapeGuide);
    return shape;
  }

  internal WPicture ConvertChildShapeToPicture(ChildShape childShape)
  {
    WPicture picture = new WPicture((IWordDocument) this.Document);
    ImageRecord imageRecord = new ImageRecord(this.Document, childShape.m_imageRecord);
    picture.LoadImage(imageRecord);
    picture.TextWrappingStyle = childShape.GetOwnerGroupShape().GetTextWrappingStyle();
    picture.LayoutInCell = childShape.LayoutInCell;
    picture.AlternativeText = childShape.AlternativeText;
    picture.FillFormat = childShape.FillFormat.Clone();
    picture.FillRectangle = childShape.FillFormat.SourceRectangle.Clone();
    picture.AllowOverlap = true;
    picture.HeightScale = childShape.HeightScale;
    picture.Height = childShape.Height;
    picture.WidthScale = childShape.WidthScale;
    picture.Width = childShape.Width;
    picture.HorizontalPosition = childShape.X;
    picture.VerticalPosition = childShape.Y;
    childShape.Title = picture.Title;
    picture.Rotation = childShape.Rotation;
    picture.Name = childShape.Name;
    picture.HorizontalOrigin = HorizontalOrigin.Margin;
    picture.VerticalOrigin = VerticalOrigin.Margin;
    if (childShape.m_pictureProps != null && childShape.m_pictureProps.Count > 0)
      this.Document.CloneProperties(childShape.DocxPictureVisualProps, ref picture.m_docxVisualShapeProps);
    this.Document.HasPicture = true;
    return picture;
  }

  internal GroupShape ConvertChildGroupShapeToGroupShape(ChildGroupShape childGroupShape)
  {
    GroupShape owner = new GroupShape((IWordDocument) this.Document);
    owner.AlternativeText = childGroupShape.AlternativeText;
    owner.AutoShapeType = childGroupShape.AutoShapeType;
    owner.WrapFormat.AllowOverlap = true;
    if (childGroupShape.m_docxProps != null && childGroupShape.m_docxProps.Count > 0)
      this.Document.CloneProperties(childGroupShape.DocxProps, ref owner.m_docxProps);
    if (childGroupShape.m_docx2007Props != null && childGroupShape.m_docx2007Props.Count > 0)
      this.Document.CloneProperties(childGroupShape.Docx2007Props, ref owner.m_docx2007Props);
    for (int index = 0; index < childGroupShape.DocxStyleProps.Count; ++index)
      owner.DocxStyleProps.Add(childGroupShape.DocxStyleProps[index]);
    owner.EffectList = childGroupShape.EffectList;
    owner.X = childGroupShape.X;
    owner.Y = childGroupShape.Y;
    owner.ExtentXValue = childGroupShape.ExtentXValue;
    owner.ExtentYValue = childGroupShape.ExtentYValue;
    owner.FillFormat = childGroupShape.FillFormat.Clone();
    owner.Height = childGroupShape.Height;
    owner.HeightScale = childGroupShape.HeightScale;
    owner.Is2007Shape = childGroupShape.Is2007Shape;
    owner.IsChangedCFormat = childGroupShape.IsChangedCFormat;
    owner.IsDetachedTextChanged = childGroupShape.IsDetachedTextChanged;
    owner.IsEffectStyleInline = childGroupShape.IsEffectStyleInline;
    owner.IsFillStyleInline = childGroupShape.IsFillStyleInline;
    owner.IsLineStyleInline = childGroupShape.IsLineStyleInline;
    owner.IsMappedItem = childGroupShape.IsMappedItem;
    owner.IsScenePropertiesInline = childGroupShape.IsScenePropertiesInline;
    owner.IsShapePropertiesInline = childGroupShape.IsShapePropertiesInline;
    owner.LayoutInCell = childGroupShape.LayoutInCell;
    owner.LeftMargin = childGroupShape.LeftMargin;
    owner.LineFormat = childGroupShape.LineFormat.Clone();
    owner.Name = childGroupShape.Name;
    owner.ParaItemCharFormat.ApplyBase((FormatBase) childGroupShape.ParaItemCharFormat);
    owner.Rotation = childGroupShape.Rotation;
    owner.ShapeID = childGroupShape.ShapeID;
    owner.SkipDocxItem = childGroupShape.SkipDocxItem;
    owner.Title = childGroupShape.Title;
    owner.TopMargin = childGroupShape.TopMargin;
    owner.Visible = childGroupShape.Visible;
    owner.Width = childGroupShape.Width;
    owner.WidthScale = childGroupShape.WidthScale;
    foreach (ShapeStyleReference shapeStyleReference in childGroupShape.ShapeStyleReferences)
      owner.ShapeStyleReferences.Add(shapeStyleReference);
    foreach (KeyValuePair<string, DictionaryEntry> relation in childGroupShape.Relations)
      owner.Relations.Add(relation.Key, relation.Value);
    foreach (KeyValuePair<int, object> keyValuePair in childGroupShape.PropertiesHash)
      owner.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    for (int index = 0; index < childGroupShape.ChildShapes.Count; ++index)
    {
      ChildShape childShape = childGroupShape.ChildShapes[index].Clone() as ChildShape;
      childShape.SetOwner((OwnerHolder) owner);
      childShape.skipPositionUpdate = true;
      owner.ChildShapes.Add(childShape);
      childShape.skipPositionUpdate = false;
    }
    return owner;
  }

  internal WTextBox ConvertChildShapeToTextbox(ChildShape childShape)
  {
    WTextBox owner = new WTextBox((IWordDocument) this.Document);
    owner.Name = childShape.Name;
    owner.Visible = childShape.Visible;
    owner.TextBoxFormat.AllowOverlap = true;
    owner.TextBoxFormat.Height = childShape.Height;
    owner.TextBoxFormat.Width = childShape.Width;
    owner.TextBoxFormat.HorizontalPosition = childShape.X;
    owner.TextBoxFormat.VerticalPosition = childShape.Y;
    owner.TextBoxFormat.HeightOrigin = childShape.TextFrame.HeightOrigin;
    owner.TextBoxFormat.HeightRelativePercent = childShape.TextFrame.HeightRelativePercent;
    owner.TextBoxFormat.InternalMargin.Bottom = childShape.TextFrame.InternalMargin.Bottom;
    owner.TextBoxFormat.InternalMargin.Left = childShape.TextFrame.InternalMargin.Left;
    owner.TextBoxFormat.InternalMargin.Right = childShape.TextFrame.InternalMargin.Right;
    owner.TextBoxFormat.InternalMargin.Top = childShape.TextFrame.InternalMargin.Top;
    owner.TextBoxFormat.TextDirection = childShape.TextFrame.TextDirection;
    owner.TextBoxFormat.TextVerticalAlignment = childShape.TextFrame.TextVerticalAlignment;
    owner.TextBoxFormat.VerticalRelativePercent = childShape.TextFrame.VerticalRelativePercent;
    owner.TextBoxFormat.WidthOrigin = childShape.TextFrame.WidthOrigin;
    owner.TextBoxFormat.WidthRelativePercent = childShape.TextFrame.WidthRelativePercent;
    owner.TextBoxFormat.AutoFit = childShape.TextFrame.ShapeAutoFit;
    owner.TextBoxFormat.Rotation = childShape.Rotation;
    owner.TextBoxFormat.FlipHorizontal = childShape.FlipHorizantal;
    owner.TextBoxFormat.FlipVertical = childShape.FlipVertical;
    if (!childShape.FillFormat.IsDefaultFill)
    {
      if (!childShape.FillFormat.Fill)
        owner.TextBoxFormat.FillColor = Color.Empty;
      else if (childShape.FillFormat.FillType == FillType.FillGradient)
      {
        owner.TextBoxFormat.FillEfects.Type = BackgroundType.Gradient;
        owner.TextBoxFormat.FillEfects.Gradient.Color2 = childShape.FillFormat.Color;
      }
      else if (childShape.FillFormat.FillType == FillType.FillPicture)
      {
        owner.TextBoxFormat.FillEfects.Type = BackgroundType.Picture;
        owner.TextBoxFormat.FillEfects.ImageRecord = new ImageRecord(this.Document, childShape.FillFormat.ImageRecord.ImageBytes);
      }
      else if (childShape.FillFormat.FillType == FillType.FillPatterned)
      {
        owner.TextBoxFormat.FillEfects.Type = BackgroundType.Color;
        owner.TextBoxFormat.FillColor = childShape.FillFormat.ForeColor;
      }
      else
        owner.TextBoxFormat.FillColor = childShape.FillFormat.Color;
    }
    else if (!childShape.Is2007Shape)
      owner.TextBoxFormat.FillColor = Color.Empty;
    owner.TextBoxFormat.NoLine = !childShape.LineFormat.Line;
    if (childShape.LineFormat.HasKey(12))
      owner.TextBoxFormat.LineColor = childShape.LineFormat.Color;
    if (childShape.LineFormat.HasKey(11))
      owner.TextBoxFormat.LineWidth = childShape.LineFormat.Weight;
    if (childShape.LineFormat.HasKey(5))
      owner.TextBoxFormat.LineDashing = childShape.LineFormat.DashStyle;
    owner.TextBoxFormat.TextThemeColor = childShape.FontRefColor;
    owner.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Margin;
    owner.TextBoxFormat.VerticalOrigin = VerticalOrigin.Margin;
    if (owner.IsShape)
      owner.Shape.FillFormat.Transparency = childShape.FillFormat.Transparency;
    if (childShape.TextBody != null)
    {
      owner.m_textBody = childShape.TextBody.Clone() as WTextBody;
      owner.m_textBody.SetOwner((OwnerHolder) owner);
    }
    if (childShape.m_docxProps != null && childShape.m_docxProps.Count > 0)
      this.Document.CloneProperties(childShape.DocxProps, ref owner.m_docxProps);
    return owner;
  }

  internal WChart ConvertChildShapeToChart(ChildShape childShape)
  {
    WChart wchart = new WChart(this.Document);
    WChart chart = childShape.Chart;
    chart.Width = childShape.Width;
    chart.Height = childShape.Height;
    chart.WidthScale = childShape.WidthScale;
    chart.HeightScale = childShape.HeightScale;
    chart.Title = childShape.Title;
    chart.AlternativeText = childShape.AlternativeText;
    chart.Name = childShape.Name;
    chart.LayoutInCell = childShape.LayoutInCell;
    chart.WrapFormat.AllowOverlap = true;
    chart.HorizontalOrigin = HorizontalOrigin.Margin;
    chart.VerticalOrigin = VerticalOrigin.Margin;
    if (childShape.m_docxProps != null && childShape.m_docxProps.Count > 0)
      this.Document.CloneProperties(childShape.DocxProps, ref chart.m_docxProps);
    return chart;
  }

  public GroupShape(IWordDocument document)
    : base((WordDocument) document)
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
    if (!this.Document.IsOpening)
    {
      this.HorizontalOrigin = HorizontalOrigin.Page;
      this.VerticalOrigin = VerticalOrigin.Page;
    }
    else
    {
      this.HorizontalOrigin = HorizontalOrigin.Column;
      this.VerticalOrigin = VerticalOrigin.Paragraph;
    }
    this.HorizontalAlignment = ShapeHorizontalAlignment.None;
    this.VerticalAlignment = ShapeVerticalAlignment.None;
    this.WrapFormat.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
  }

  public GroupShape(IWordDocument document, ParagraphItem[] childShapes)
    : base((WordDocument) document)
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
    this.LineFormat.Weight = 1f;
    if (!this.Document.IsOpening)
    {
      this.HorizontalOrigin = HorizontalOrigin.Page;
      this.VerticalOrigin = VerticalOrigin.Page;
    }
    else
    {
      this.HorizontalOrigin = HorizontalOrigin.Column;
      this.VerticalOrigin = VerticalOrigin.Paragraph;
    }
    this.HorizontalAlignment = ShapeHorizontalAlignment.None;
    this.VerticalAlignment = ShapeVerticalAlignment.None;
    this.WrapFormat.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
    this.Add(childShapes);
  }

  internal void InitializeVMLDefaultValues()
  {
    this.FillFormat.Color = Color.White;
    this.LineFormat.ForeColor = Color.Black;
    this.LineFormat.Color = Color.Empty;
    this.WrapFormat.AllowOverlap = true;
  }

  protected override object CloneImpl()
  {
    GroupShape groupShape = (GroupShape) base.CloneImpl();
    groupShape.IsCloned = true;
    groupShape.ChildShapes = new ChildShapeCollection(this.Document);
    for (int index = 0; index < this.ChildShapes.Count; ++index)
    {
      Entity entity = this.ChildShapes[index].Clone();
      entity.SetOwner((OwnerHolder) groupShape);
      if (entity is ChildGroupShape)
      {
        bool skipPositionUpdate = (entity as ChildGroupShape).skipPositionUpdate;
        (entity as ChildGroupShape).skipPositionUpdate = true;
        groupShape.ChildShapes.Add((ChildShape) (entity as ChildGroupShape));
        (entity as ChildGroupShape).skipPositionUpdate = skipPositionUpdate;
      }
      else
      {
        bool skipPositionUpdate = (entity as ChildShape).skipPositionUpdate;
        (entity as ChildShape).skipPositionUpdate = true;
        groupShape.ChildShapes.Add(entity as ChildShape);
        (entity as ChildShape).skipPositionUpdate = skipPositionUpdate;
      }
    }
    this.CloneShapeFormat(groupShape);
    return (object) groupShape;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    int index = 0;
    for (int count = this.ChildShapes.Count; index < count; ++index)
      this.ChildShapes[index].CloneRelationsTo(doc, nextOwner);
  }

  internal bool HasChildGroupShape()
  {
    foreach (ChildShape childShape in (CollectionImpl) this.ChildShapes)
    {
      if (childShape is ChildGroupShape)
        return true;
    }
    return false;
  }

  internal void ApplyCharacterFormat(WCharacterFormat charFormat)
  {
    if (charFormat == null)
      return;
    this.SetParagraphItemCharacterFormat(charFormat);
  }

  internal void CloneShapeFormat(GroupShape shape)
  {
    ChildShape childShape = new ChildShape((IWordDocument) this.Document);
    bool flag = this.Document != null && this.Document.DocHasThemes;
    if (this.IsFillStyleInline && this.FillFormat != null)
    {
      shape.FillFormat = this.FillFormat.Clone();
      shape.IsFillStyleInline = true;
    }
    else if (flag && this.ShapeStyleReferences != null && this.ShapeStyleReferences.Count > 0)
    {
      int styleRefIndex = this.ShapeStyleReferences[1].StyleRefIndex;
      if (styleRefIndex > 0 && this.Document.Themes.FmtScheme.FillFormats.Count > styleRefIndex)
      {
        uint maxValue = uint.MaxValue;
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
            shape.FillFormat.GradientFill.GradientStops[index].Color = childShape.StyleColorTransform(fillFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue);
            maxValue = uint.MaxValue;
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
          shape.FillFormat.ForeColor = childShape.StyleColorTransform(fillTransformation1, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue);
          maxValue = uint.MaxValue;
          shape.FillFormat.Color = childShape.StyleColorTransform(fillTransformation2, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue);
          maxValue = uint.MaxValue;
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
        uint maxValue = uint.MaxValue;
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
            shape.FillFormat.GradientFill.GradientStops[index].Color = childShape.StyleColorTransform(lineFormat.GradientFill.GradientStops[index].FillSchemeColorTransforms, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue);
            maxValue = uint.MaxValue;
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
          shape.FillFormat.ForeColor = childShape.StyleColorTransform(fillTransformation3, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue);
          maxValue = uint.MaxValue;
          shape.FillFormat.Color = childShape.StyleColorTransform(fillTransformation4, this.ShapeStyleReferences[1].StyleRefColor, ref maxValue);
          maxValue = uint.MaxValue;
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
        this.Width = (float) (((double) section.PageSetup.Margins.Left + (section.Document.DOP.GutterAtTop ? 0.0 : (double) section.PageSetup.Margins.Gutter)) * ((double) this.RelativeWidth / 100.0));
        break;
      case HorizontalOrigin.RightMargin:
      case HorizontalOrigin.OutsideMargin:
        this.Width = section.PageSetup.Margins.Right * (this.RelativeWidth / 100f);
        break;
      default:
        this.Width = (float) (((double) section.PageSetup.PageSize.Width - (double) section.PageSetup.Margins.Left - (section.Document.DOP.GutterAtTop ? (double) section.PageSetup.Margins.Gutter : 0.0) - (double) section.PageSetup.Margins.Right) * ((double) this.RelativeWidth / 100.0));
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

  SizeF ILeafWidget.Measure(DrawingContext dc) => new SizeF(this.Width, this.Height);

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
    if (this.Owner is InlineContentControl)
      wparagraph = this.GetOwnerParagraphValue();
    if (wparagraph.IsInCell && ((IWidget) wparagraph).LayoutInfo.IsClipped)
      this.m_layoutInfo.IsClipped = true;
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
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal override void Close()
  {
    if (this.m_childShapes != null)
    {
      foreach (OwnerHolder childShape in (CollectionImpl) this.m_childShapes)
        childShape.Close();
      this.m_childShapes.Clear();
      this.m_childShapes = (ChildShapeCollection) null;
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
      foreach (EffectFormat effect in this.m_effectList)
        effect.Close();
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
