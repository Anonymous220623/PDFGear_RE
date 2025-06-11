// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WPicture
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Convertors;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WPicture : ParagraphItem, ILeafWidget, IWidget, IWPicture, IParagraphItem, IEntity
{
  private float m_rotation;
  internal SizeF m_size;
  private float m_widthScale = 100f;
  private float m_heightScale = 100f;
  private HorizontalOrigin m_horizontalOrigin;
  private ShapePosition m_shapePosition;
  private VerticalOrigin m_verticalOrigin;
  private float m_horizPosition;
  private TileRectangle m_fillRectable;
  private float m_vertPosition;
  private float m_DistanceFromBottom;
  private float m_DistanceFromLeft = 9f;
  private float m_DistanceFromRight = 9f;
  private float m_DistanceFromTop;
  private TextWrappingStyle m_wrappingStyle;
  private TextWrappingType m_wrappingType;
  private ShapeHorizontalAlignment m_horAlignment;
  private ShapeVerticalAlignment m_vertAlignment;
  private int m_spid = -1;
  private InlineShapeObject m_inlinePictureShape;
  internal List<Stream> m_docxProps;
  internal List<Stream> m_docxVisualShapeProps;
  private List<Stream> m_signatureLineElements;
  private string m_altText;
  private string m_name;
  private string m_title;
  private WTextBody m_embedBody;
  private int m_orderIndex = int.MaxValue;
  private ImageRecord m_imageRecord;
  internal short WrapCollectionIndex = -1;
  private WrapPolygon m_wrapPolygon;
  private string m_href;
  private string m_ExternalLinkName;
  private string m_linktype;
  private ushort m_bFlags = 88;
  private FillFormat m_fillFormat;
  private Color m_chromaKeyColor;
  private string m_oPictureHRef;
  private byte[] m_svgImageData;
  private string m_svgExternalLinkName = string.Empty;

  internal TileRectangle FillRectangle
  {
    get
    {
      if (this.m_fillRectable == null)
        this.m_fillRectable = new TileRectangle();
      return this.m_fillRectable;
    }
    set => this.m_fillRectable = value;
  }

  internal bool HasBorder
  {
    get
    {
      if (this.TextWrappingStyle == TextWrappingStyle.Inline && this.IsShape)
        return ((!this.PictureShape.PictureDescriptor.BorderBottom.IsDefault || !this.PictureShape.PictureDescriptor.BorderLeft.IsDefault || !this.PictureShape.PictureDescriptor.BorderRight.IsDefault ? (false ? 1 : 0) : (this.PictureShape.PictureDescriptor.BorderTop.IsDefault ? 1 : 0)) | (this.PictureShape.PictureDescriptor.BorderBottom.BorderType != (byte) 0 || this.PictureShape.PictureDescriptor.BorderLeft.BorderType != (byte) 0 || this.PictureShape.PictureDescriptor.BorderRight.BorderType != (byte) 0 ? 0 : (this.PictureShape.PictureDescriptor.BorderTop.BorderType == (byte) 0 ? 1 : 0))) == 0;
      return this.PictureShape.ShapeContainer != null && this.PictureShape.ShapeContainer.ShapeOptions != null && this.PictureShape.ShapeContainer.ShapeOptions.LineProperties.HasDefined && this.PictureShape.ShapeContainer.ShapeOptions.LineProperties.UsefLine && this.PictureShape.ShapeContainer.ShapeOptions.LineProperties.Line;
    }
  }

  public override EntityType EntityType => EntityType.Picture;

  public float Height
  {
    get => (float) ((double) this.Size.Height * (double) this.m_heightScale / 100.0);
    set
    {
      if (this.Document != null && this.LockAspectRatio && !this.Document.IsOpening && !this.Document.IsMailMerge && !this.Document.IsCloning && !this.Document.IsClosing)
      {
        float heightScale = this.m_heightScale;
        this.SetHeightScaleValue((float) ((double) value / (double) this.Size.Height * 100.0));
        this.SetWidthScaleValue(this.m_widthScale * (this.m_heightScale / heightScale));
      }
      else
        this.SetHeightScaleValue((float) ((double) value / (double) this.Size.Height * 100.0));
    }
  }

  public float Rotation
  {
    get => this.m_rotation;
    set => this.m_rotation = value;
  }

  public float Width
  {
    get => (float) ((double) this.Size.Width * (double) this.m_widthScale / 100.0);
    set
    {
      if (this.Document != null && this.LockAspectRatio && !this.Document.IsOpening && !this.Document.IsMailMerge && !this.Document.IsCloning && !this.Document.IsClosing)
      {
        float widthScale = this.m_widthScale;
        this.SetWidthScaleValue((float) ((double) value / (double) this.Size.Width * 100.0));
        this.SetHeightScaleValue(this.m_heightScale * (this.m_widthScale / widthScale));
      }
      else
        this.SetWidthScaleValue((float) ((double) value / (double) this.Size.Width * 100.0));
    }
  }

  public float HeightScale
  {
    get => this.m_heightScale;
    set
    {
      this.m_heightScale = (double) value >= 0.0 && (double) value <= 10675.0 ? value : throw new ArgumentOutOfRangeException("Scale factor must be between 0 and 10675");
    }
  }

  public float WidthScale
  {
    get => this.m_widthScale;
    set
    {
      this.m_widthScale = (double) value >= 0.0 && (double) value <= 10675.0 ? value : throw new ArgumentOutOfRangeException("Scale factor must be between 0 and 10675");
    }
  }

  public bool LockAspectRatio
  {
    get => ((int) this.m_bFlags & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65279 | (value ? 1 : 0) << 8);
  }

  internal Syncfusion.DocIO.DLS.Entities.Image ImageForPartialTrustMode
  {
    get => this.GetImageForPartialTrustMode(this.ImageBytes);
  }

  public System.Drawing.Image Image
  {
    get => this.GetImage(this.ImageBytes, this.Document != null && !this.Document.IsOpening);
  }

  public byte[] ImageBytes
  {
    get => this.m_imageRecord == null ? (byte[]) null : this.m_imageRecord.ImageBytes;
  }

  public byte[] SvgData
  {
    get => this.m_svgImageData;
    internal set => this.m_svgImageData = value;
  }

  internal ImageRecord ImageRecord => this.m_imageRecord;

  internal ShapePosition Position
  {
    get => this.m_shapePosition;
    set => this.m_shapePosition = value;
  }

  public HorizontalOrigin HorizontalOrigin
  {
    get => this.m_horizontalOrigin;
    set => this.m_horizontalOrigin = value;
  }

  public VerticalOrigin VerticalOrigin
  {
    get => this.m_verticalOrigin;
    set => this.m_verticalOrigin = value;
  }

  public float HorizontalPosition
  {
    get => this.m_horizPosition;
    set => this.m_horizPosition = value;
  }

  public float VerticalPosition
  {
    get => this.m_vertPosition;
    set => this.m_vertPosition = value;
  }

  internal float DistanceFromBottom
  {
    get
    {
      return (double) this.m_DistanceFromBottom < 0.0 || (double) this.m_DistanceFromBottom > 1584.0 ? 0.0f : this.m_DistanceFromBottom;
    }
    set => this.m_DistanceFromBottom = value;
  }

  internal float DistanceFromLeft
  {
    get
    {
      return (double) this.m_DistanceFromLeft < 0.0 || (double) this.m_DistanceFromLeft > 1584.0 ? 0.0f : this.m_DistanceFromLeft;
    }
    set => this.m_DistanceFromLeft = value;
  }

  internal float DistanceFromRight
  {
    get
    {
      return (double) this.m_DistanceFromRight < 0.0 || (double) this.m_DistanceFromRight > 1584.0 ? 0.0f : this.m_DistanceFromRight;
    }
    set => this.m_DistanceFromRight = value;
  }

  internal float DistanceFromTop
  {
    get
    {
      return (double) this.m_DistanceFromTop < 0.0 || (double) this.m_DistanceFromTop > 1584.0 ? 0.0f : this.m_DistanceFromTop;
    }
    set => this.m_DistanceFromTop = value;
  }

  public TextWrappingStyle TextWrappingStyle
  {
    get => this.m_wrappingStyle;
    set
    {
      if (this.HasBorder)
      {
        if (this.m_wrappingStyle == TextWrappingStyle.Inline && value != TextWrappingStyle.Inline)
          this.PictureShape.ConvertToShape();
        else if (this.m_wrappingStyle != TextWrappingStyle.Inline && value == TextWrappingStyle.Inline)
          this.PictureShape.ConvertToInlineShape();
      }
      this.m_wrappingStyle = value;
      if (this.m_wrappingStyle == TextWrappingStyle.Behind)
        this.m_bFlags = (ushort) ((int) this.m_bFlags & 65534 | 1);
      else
        this.m_bFlags &= (ushort) 65534;
    }
  }

  public TextWrappingType TextWrappingType
  {
    get => this.m_wrappingType;
    set => this.m_wrappingType = value;
  }

  public ShapeHorizontalAlignment HorizontalAlignment
  {
    get => this.m_horAlignment;
    set => this.m_horAlignment = value;
  }

  public ShapeVerticalAlignment VerticalAlignment
  {
    get => this.m_vertAlignment;
    set => this.m_vertAlignment = value;
  }

  public bool IsBelowText
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      this.m_bFlags = (ushort) ((int) this.m_bFlags & 65534 | (value ? 1 : 0));
      if (value && this.TextWrappingStyle == TextWrappingStyle.InFrontOfText)
      {
        this.m_wrappingStyle = TextWrappingStyle.Behind;
      }
      else
      {
        if (value || this.TextWrappingStyle != TextWrappingStyle.Behind)
          return;
        this.m_wrappingStyle = TextWrappingStyle.InFrontOfText;
      }
    }
  }

  public WCharacterFormat CharacterFormat
  {
    get => this.m_charFormat;
    internal set => this.m_charFormat = value;
  }

  internal int ShapeId
  {
    get => this.m_spid;
    set => this.m_spid = value;
  }

  internal string OPictureHRef
  {
    get => this.m_oPictureHRef;
    set => this.m_oPictureHRef = value;
  }

  internal bool IsHeaderPicture
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65533 | (value ? 1 : 0) << 1);
  }

  internal InlineShapeObject PictureShape
  {
    get => this.m_inlinePictureShape;
    set
    {
      if (this.m_inlinePictureShape != null)
        this.m_inlinePictureShape.SetOwner((WordDocument) null, (OwnerHolder) null);
      this.m_inlinePictureShape = value;
      if (this.m_inlinePictureShape == null)
        return;
      this.m_inlinePictureShape.SetOwner((OwnerHolder) this);
    }
  }

  internal SizeF Size
  {
    get
    {
      if ((double) this.m_size.Width == -3.4028234663852886E+38 || (double) this.m_size.Height == -3.4028234663852886E+38)
      {
        if (WordDocument.EnablePartialTrustCode)
          this.CheckPicSizeForPartialTrustMode(this.ImageForPartialTrustMode);
        else
          this.CheckPicSize(this.GetImage(this.ImageBytes, this.Document != null && !this.Document.IsOpening));
      }
      return this.m_size;
    }
    set => this.m_size = value;
  }

  internal bool IsMetaFile => this.m_imageRecord != null && this.m_imageRecord.IsMetafile;

  internal List<Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new List<Stream>();
      return this.m_docxProps;
    }
    set => this.m_docxProps = value;
  }

  internal List<Stream> DocxVisualShapeProps
  {
    get
    {
      if (this.m_docxVisualShapeProps == null)
        this.m_docxVisualShapeProps = new List<Stream>();
      return this.m_docxVisualShapeProps;
    }
    set => this.m_docxVisualShapeProps = value;
  }

  internal List<Stream> SignatureLineElements
  {
    get
    {
      if (this.m_signatureLineElements == null)
        this.m_signatureLineElements = new List<Stream>();
      return this.m_signatureLineElements;
    }
  }

  public string AlternativeText
  {
    get => this.m_altText;
    set => this.m_altText = value;
  }

  public string Name
  {
    get
    {
      if (this.m_name == null && this.m_imageRecord != null)
        this.m_name = "Picture " + this.m_imageRecord.ImageId.ToString();
      return this.m_name;
    }
    set => this.m_name = value;
  }

  internal Color ChromaKeyColor
  {
    get => this.m_chromaKeyColor;
    set => this.m_chromaKeyColor = value;
  }

  public string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal WTextBody EmbedBody
  {
    get => this.m_embedBody;
    set => this.m_embedBody = value;
  }

  internal bool IsShape
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65531 | (value ? 1 : 0) << 2);
  }

  internal int OrderIndex
  {
    get
    {
      if (this.m_orderIndex == int.MaxValue && this.Document != null && !this.Document.IsOpening && this.Document.Escher != null)
      {
        int shapeOrderIndex = this.Document.Escher.GetShapeOrderIndex(this.ShapeId);
        if (shapeOrderIndex != -1)
          this.m_orderIndex = shapeOrderIndex;
      }
      return this.m_orderIndex;
    }
    set => this.m_orderIndex = value;
  }

  internal bool LayoutInCell
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool AllowOverlap
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65519 | (value ? 1 : 0) << 4);
  }

  internal string Href
  {
    get => this.m_href;
    set => this.m_href = value;
  }

  internal string ExternalLink
  {
    get => this.m_ExternalLinkName;
    set => this.m_ExternalLinkName = value;
  }

  internal string SvgExternalLink
  {
    get => this.m_svgExternalLinkName;
    set => this.m_svgExternalLinkName = value;
  }

  internal bool HasImageRecordReference
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65407 | (value ? 1 : 0) << 7);
  }

  internal string LinkType
  {
    get => this.m_linktype;
    set => this.m_linktype = value;
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

  public bool Visible
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool IsWrappingBoundsAdded
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65503 | (value ? 1 : 0) << 5);
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

  public bool FlipHorizontal
  {
    get => ((int) this.m_bFlags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 65023 | (value ? 1 : 0) << 9);
  }

  public bool FlipVertical
  {
    get => ((int) this.m_bFlags & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool IsDefaultPicOfContentControl
  {
    get => ((int) this.m_bFlags & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_bFlags = (ushort) ((int) this.m_bFlags & 63487 | (value ? 1 : 0) << 11);
  }

  public WPicture(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat(doc, (Entity) this);
    this.m_inlinePictureShape = new InlineShapeObject(doc);
    this.m_inlinePictureShape.SetOwner((OwnerHolder) this);
    this.m_size.Height = float.MinValue;
    this.m_size.Width = float.MinValue;
  }

  public void LoadImage(byte[] imageBytes)
  {
    if (imageBytes == null)
      throw new ArgumentNullException("Image bytes cannot be null or empty");
    this.ResetImageData();
    System.Drawing.Image image = this.GetImage(imageBytes, this.Document != null && !this.Document.IsOpening);
    if (image is System.Drawing.Imaging.Metafile)
      this.LoadImage(imageBytes, true);
    else if (image != null && (image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Tiff) || image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Bmp)))
      this.ConvertBitmap(image);
    else
      this.LoadImage(imageBytes, false);
    imageBytes = (byte[]) null;
    this.CheckPicSize(image);
    this.UpdateBlipImageRecord();
  }

  public void LoadImage(byte[] svgData, byte[] imageBytes)
  {
    WPicture.EvaluateSVGImageBytes(svgData);
    this.SvgData = svgData;
    string str = "{96DAC541-7B7A-43D3-8B79-37D633B846F1}";
    this.FillFormat.FillType = FillType.FillPicture;
    this.FillFormat.BlipFormat.ExtensionURI.Add(str);
    this.FillFormat.BlipFormat.ExtensionURI.Add("svgBlip");
    this.LoadImage(imageBytes);
  }

  internal static void EvaluateSVGImageBytes(byte[] svgData)
  {
    XmlReader xmlReader = svgData != null ? UtilityMethods.CreateReader((Stream) new MemoryStream(svgData)) : throw new ArgumentNullException("svgData should not be null");
    string localName = xmlReader.LocalName;
    xmlReader.Close();
    if (localName != "svg")
      throw new ArgumentException("SVG data should be *.svg format");
  }

  public void LoadImage(System.Drawing.Image image)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    this.ResetImageData();
    if (image is System.Drawing.Imaging.Metafile)
      this.m_imageRecord = this.Document.Images.LoadMetaFileImage(WPicture.LoadMetafile(image as System.Drawing.Imaging.Metafile), false);
    else if (image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Tiff) || image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Bmp))
      this.ConvertBitmap(image);
    else
      this.m_imageRecord = this.Document.Images.LoadImage(WPicture.LoadBitmap(image));
    this.CheckPicSize(image);
    this.UpdateBlipImageRecord();
  }

  private void UpdateBlipImageRecord()
  {
    if (this.Document == null || this.Document.IsOpening || this.Document.Escher == null || this.Document.Escher.Containers == null || !this.Document.Escher.Containers.ContainsKey(this.ShapeId) || !(this.Document.Escher.Containers[this.ShapeId] is MsofbtSpContainer container) || container.Bse == null || container.Bse.Blip == null)
      return;
    container.Bse.Blip.ImageRecord = this.m_imageRecord;
  }

  public IWParagraph AddCaption(
    string name,
    CaptionNumberingFormat format,
    CaptionPosition captionPosition)
  {
    WTextBody owner = this.OwnerParagraph.Owner as WTextBody;
    WParagraph wparagraph1 = (WParagraph) null;
    if (owner != null)
    {
      int inOwnerCollection = this.OwnerParagraph.GetIndexInOwnerCollection();
      wparagraph1 = new WParagraph((IWordDocument) this.Document);
      wparagraph1.AppendText(name + " ");
      name = name.Replace(" ", "_");
      wparagraph1.ApplyStyle(BuiltinStyle.Caption, false);
      ((WSeqField) wparagraph1.AppendField(name, FieldType.FieldSequence)).NumberFormat = format;
      int index1 = this.OwnerParagraph.Items.IndexOf((IEntity) this);
      if (captionPosition == CaptionPosition.AboveImage)
      {
        wparagraph1.ParagraphFormat.KeepFollow = true;
        int index2 = index1 == 0 ? inOwnerCollection : inOwnerCollection + 1;
        owner.Items.Insert(index2, (IEntity) wparagraph1);
        if (index1 > 0)
        {
          this.OwnerParagraph.Items.RemoveAt(index1);
          WParagraph wparagraph2 = new WParagraph((IWordDocument) this.Document);
          wparagraph2.Items.Insert(0, (IEntity) this);
          owner.Items.Insert(index2 + 1, (IEntity) wparagraph2);
        }
      }
      else
      {
        this.OwnerParagraph.ParagraphFormat.KeepFollow = true;
        owner.Items.Insert(inOwnerCollection + 1, (IEntity) wparagraph1);
      }
    }
    return (IWParagraph) wparagraph1;
  }

  internal override void AddSelf()
  {
    if (this.m_imageRecord == null)
      return;
    System.Drawing.Size size = this.m_imageRecord.Size;
    System.Drawing.Imaging.ImageFormat imageFormat = this.m_imageRecord.ImageFormat;
    int length = this.m_imageRecord.Length;
    this.m_imageRecord = !this.m_imageRecord.IsMetafile ? this.Document.Images.LoadImage(this.m_imageRecord.ImageBytes) : this.Document.Images.LoadMetaFileImage(this.m_imageRecord.m_imageBytes, true);
    this.m_imageRecord.Size = size;
    this.m_imageRecord.ImageFormat = imageFormat;
    this.m_imageRecord.Length = length;
  }

  protected override object CloneImpl()
  {
    WPicture owner = (WPicture) base.CloneImpl();
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
      this.Document.CloneProperties(this.m_docxProps, ref owner.m_docxProps);
    if (this.m_docxVisualShapeProps != null && this.m_docxVisualShapeProps.Count > 0)
      this.Document.CloneProperties(this.m_docxVisualShapeProps, ref owner.m_docxVisualShapeProps);
    if (this.m_signatureLineElements != null && this.m_signatureLineElements.Count > 0)
      this.Document.CloneProperties(this.m_signatureLineElements, ref owner.m_signatureLineElements);
    owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) owner);
    owner.m_charFormat.ImportContainer((FormatBase) this.m_charFormat);
    owner.m_inlinePictureShape = (InlineShapeObject) this.PictureShape.Clone();
    owner.m_inlinePictureShape.SetOwner((OwnerHolder) owner);
    if (owner.ImageRecord != null)
    {
      ImageRecord imageRecord = new ImageRecord(this.Document, this.m_imageRecord);
      owner.m_imageRecord = imageRecord;
    }
    if (this.SvgData != null)
      owner.SvgData = this.SvgData;
    if ((double) this.m_size.Width != -3.4028234663852886E+38 && (double) this.m_size.Height != -3.4028234663852886E+38)
      owner.Size = this.m_size;
    if (this.EmbedBody != null)
      owner.EmbedBody = (WTextBody) this.EmbedBody.Clone();
    if (this.WrapPolygon != null)
      owner.WrapPolygon = this.WrapPolygon.Clone();
    owner.IsCloned = true;
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (nextOwner.OwnerBase != null && nextOwner.OwnerBase is HeaderFooter || nextOwner is HeaderFooter)
      this.IsHeaderPicture = true;
    if (this.m_imageRecord != null)
    {
      System.Drawing.Size size = this.m_imageRecord.Size;
      System.Drawing.Imaging.ImageFormat imageFormat = this.m_imageRecord.ImageFormat;
      int length = this.m_imageRecord.Length;
      this.m_imageRecord = !this.m_imageRecord.IsMetafile ? doc.Images.LoadImage(this.m_imageRecord.ImageBytes) : doc.Images.LoadMetaFileImage(this.m_imageRecord.m_imageBytes, true);
      this.m_imageRecord.Size = size;
      this.m_imageRecord.ImageFormat = imageFormat;
      this.m_imageRecord.Length = length;
    }
    this.Document.CloneShapeEscher(doc, (IParagraphItem) this);
    this.PictureShape.CloneRelationsTo(doc, nextOwner);
    this.IsCloned = false;
    if (this.EmbedBody == null)
      return;
    this.EmbedBody.CloneRelationsTo(doc, nextOwner);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.Picture);
    writer.WriteValue("width", this.Size.Width);
    writer.WriteValue("height", this.Size.Height);
    writer.WriteValue("WidthScale", this.m_widthScale);
    writer.WriteValue("HeightScale", this.m_heightScale);
    writer.WriteValue("IsMetafile", this.ImageRecord.IsMetafile);
    if (this.m_wrappingStyle == TextWrappingStyle.Inline)
      return;
    writer.WriteValue("HorizontalOrigin", (Enum) this.m_horizontalOrigin);
    writer.WriteValue("VerticalOrigin", (Enum) this.m_verticalOrigin);
    writer.WriteValue("VerticalPosition", this.m_vertPosition);
    writer.WriteValue("HorizontalPosition", this.m_horizPosition);
    writer.WriteValue("WrappingStyle", (Enum) this.m_wrappingStyle);
    writer.WriteValue("WrappingType", (Enum) this.m_wrappingType);
    writer.WriteValue("IsBelowText", this.IsBelowText);
    writer.WriteValue("HorizontalAlignment", (Enum) this.m_horAlignment);
    writer.WriteValue("VerticalAlignment", (Enum) this.m_vertAlignment);
    writer.WriteValue("ShapeID", this.m_spid);
    if (!this.IsHeaderPicture)
      return;
    writer.WriteValue("IsHeader", this.IsHeaderPicture);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_size.Width = reader.ReadFloat("width");
    this.m_size.Height = reader.ReadFloat("height");
    this.m_widthScale = reader.ReadFloat("WidthScale");
    this.m_heightScale = reader.ReadFloat("HeightScale");
    if (reader.HasAttribute("HorizontalOrigin"))
      this.m_horizontalOrigin = (HorizontalOrigin) reader.ReadEnum("HorizontalOrigin", typeof (HorizontalOrigin));
    if (reader.HasAttribute("VerticalOrigin"))
      this.m_verticalOrigin = (VerticalOrigin) reader.ReadEnum("VerticalOrigin", typeof (VerticalOrigin));
    if (reader.HasAttribute("VerticalPosition"))
      this.m_vertPosition = reader.ReadFloat("VerticalPosition");
    if (reader.HasAttribute("HorizontalPosition"))
      this.m_horizPosition = reader.ReadFloat("HorizontalPosition");
    if (reader.HasAttribute("WrappingStyle"))
      this.m_wrappingStyle = (TextWrappingStyle) reader.ReadEnum("WrappingStyle", typeof (TextWrappingStyle));
    if (reader.HasAttribute("WrappingType"))
      this.m_wrappingType = (TextWrappingType) reader.ReadEnum("WrappingType", typeof (TextWrappingType));
    if (reader.HasAttribute("IsBelowText"))
      this.IsBelowText = reader.ReadBoolean("IsBelowText");
    if (reader.HasAttribute("HorizontalAlignment"))
      this.m_horAlignment = (ShapeHorizontalAlignment) reader.ReadEnum("HorizontalAlignment", typeof (ShapeHorizontalAlignment));
    if (reader.HasAttribute("VerticalAlignment"))
      this.m_vertAlignment = (ShapeVerticalAlignment) reader.ReadEnum("VerticalAlignment", typeof (ShapeVerticalAlignment));
    if (reader.HasAttribute("ShapeID"))
      this.m_spid = reader.ReadInt("ShapeID");
    if (!reader.HasAttribute("IsHeader"))
      return;
    this.IsHeaderPicture = reader.ReadBoolean("IsHeader");
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.m_imageRecord == null)
      return;
    writer.WriteChildBinaryElement("image", this.m_imageRecord.ImageBytes);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    base.ReadXmlContent(reader);
    if (!(reader.TagName == "image"))
      return false;
    this.LoadImage(this.GetImage(reader.ReadChildBinaryElement(), this.Document != null && !this.Document.IsOpening));
    return true;
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("character-format", (object) this.m_charFormat);
    this.XDLSHolder.AddElement("shape-format", (object) this.m_inlinePictureShape);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_imageRecord != null && !this.DeepDetached)
    {
      --this.m_imageRecord.OccurenceCount;
      this.m_imageRecord = (ImageRecord) null;
    }
    if (this.m_embedBody != null)
    {
      this.m_embedBody.Close();
      this.m_embedBody = (WTextBody) null;
    }
    if (this.m_inlinePictureShape != null)
    {
      this.m_inlinePictureShape.Close();
      this.m_inlinePictureShape = (InlineShapeObject) null;
    }
    if (this.m_wrapPolygon != null)
    {
      this.m_wrapPolygon.Close();
      this.m_wrapPolygon = (WrapPolygon) null;
    }
    if (this.m_docxProps != null)
    {
      foreach (Stream docxProp in this.m_docxProps)
        docxProp.Close();
      this.m_docxProps.Clear();
      this.m_docxProps = (List<Stream>) null;
    }
    if (this.m_docxVisualShapeProps != null)
    {
      foreach (Stream docxVisualShapeProp in this.m_docxVisualShapeProps)
        docxVisualShapeProp.Close();
      this.m_docxVisualShapeProps.Clear();
      this.m_docxVisualShapeProps = (List<Stream>) null;
    }
    if (this.m_signatureLineElements != null)
    {
      foreach (Stream signatureLineElement in this.m_signatureLineElements)
        signatureLineElement.Close();
      this.m_signatureLineElements.Clear();
      this.m_signatureLineElements = (List<Stream>) null;
    }
    if (this.m_fillFormat != null)
    {
      this.m_fillFormat.Close();
      this.m_fillFormat = (FillFormat) null;
    }
    if (this.m_svgImageData == null)
      return;
    this.m_svgImageData = (byte[]) null;
  }

  internal void SetWidthScaleValue(float value) => this.m_widthScale = value;

  internal void SetHeightScaleValue(float value) => this.m_heightScale = value;

  internal void SetTextWrappingStyleValue(TextWrappingStyle textWrappingStyle)
  {
    this.m_wrappingStyle = textWrappingStyle;
  }

  internal override void Detach()
  {
    if (this.m_imageRecord != null)
      this.m_imageRecord.Detach();
    this.RemoveImageInCollection();
  }

  internal void RemoveImageInCollection()
  {
    if (this.Document == null)
      return;
    if (this.Document.Escher != null && this.Document.Escher.Containers != null && this.Document.Escher.Containers.ContainsKey(this.ShapeId))
    {
      if (this.Document.Escher.Containers[this.ShapeId] is MsofbtSpContainer container && container.Bse != null)
      {
        container.Bse.Blip = (_Blip) null;
        container.Bse = (MsofbtBSE) null;
      }
      this.Document.Escher.Containers.Remove(this.ShapeId);
      foreach (MsofbtDgContainer dgContainer in (List<object>) this.Document.Escher.m_dgContainers)
      {
        for (int index = 0; index < dgContainer.PatriarchGroupContainer.Children.Count; ++index)
        {
          if (dgContainer.PatriarchGroupContainer.Children[index] is MsofbtSpContainer child && child.Shape.ShapeId == this.ShapeId)
          {
            dgContainer.PatriarchGroupContainer.Children.Remove((object) child);
            break;
          }
        }
      }
    }
    this.Document.FloatingItems.Remove((Entity) this);
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    if (this.m_imageRecord != null)
      this.m_imageRecord.Attach();
    base.AttachToParagraph(owner, itemPos);
    if (this.IsPreviousItemIsOleObject() || this.TextWrappingStyle == TextWrappingStyle.Inline && this.Document.ActualFormatType == FormatType.Docx)
      return;
    this.Document.FloatingItems.Add((Entity) this);
  }

  private bool IsPreviousItemIsOleObject()
  {
    return this.PreviousSibling is WFieldMark && (this.PreviousSibling as WFieldMark).Type == FieldMarkType.FieldSeparator && this.PreviousSibling.PreviousSibling is WOleObject;
  }

  internal void LoadImage(byte[] imageBytes, bool isMetafile)
  {
    if (imageBytes == null)
      throw new ArgumentNullException("image");
    this.m_imageRecord = !isMetafile ? this.Document.Images.LoadImage(imageBytes) : this.Document.Images.LoadMetaFileImage(imageBytes, false);
    imageBytes = (byte[]) null;
    this.m_size = new SizeF(float.MinValue, float.MinValue);
  }

  internal void LoadImage(ImageRecord imageRecord)
  {
    this.m_imageRecord = imageRecord;
    if (WordDocument.EnablePartialTrustCode)
      this.m_size = this.ConvertSizeForPartialTrustMode(this.GetImageForPartialTrustMode(imageRecord.ImageBytes));
    else
      this.m_size = this.ConvertSize(this.GetImage(imageRecord.ImageBytes, this.Document != null && !this.Document.IsOpening));
  }

  private Syncfusion.DocIO.DLS.Entities.Image GetImageForPartialTrustMode(byte[] imageBytes)
  {
    Syncfusion.DocIO.DLS.Entities.Image partialTrustMode = (Syncfusion.DocIO.DLS.Entities.Image) null;
    if (imageBytes != null)
    {
      try
      {
        partialTrustMode = Syncfusion.DocIO.DLS.Entities.Image.FromStream(new MemoryStream(imageBytes));
        imageBytes = (byte[]) null;
      }
      catch
      {
        throw new ArgumentException("Argument is not image byte array");
      }
    }
    return partialTrustMode;
  }

  internal SizeF ConvertSizeForPartialTrustMode(Syncfusion.DocIO.DLS.Entities.Image image)
  {
    this.m_imageRecord.Size = image != null ? image.Size : throw new ArgumentNullException(nameof (image));
    this.m_imageRecord.ImageFormatForPartialTrustMode = image.RawFormat;
    UnitsConvertor instance = UnitsConvertor.Instance;
    return !image.IsMetafile || image.HorizontalDpi == 0L ? (image.HorizontalDpi != 0L ? instance.ConvertFromPixels(new SizeF((float) image.Size.Width, (float) image.Size.Height), PrintUnits.Point, (float) image.HorizontalDpi) : instance.ConvertFromPixels(image.Size, PrintUnits.Point)) : instance.ConvertFromPixels(new SizeF((float) image.Size.Width, (float) image.Size.Height), PrintUnits.Point, (float) image.HorizontalDpi);
  }

  internal System.Drawing.Image GetDefaultImage()
  {
    return System.Drawing.Image.FromStream(WPicture.GetManifestResourceStream("ImageNotFound.jpg"), true, false);
  }

  internal System.Drawing.Image GetImage(byte[] imageBytes, bool isImageFromScratch)
  {
    System.Drawing.Image image = (System.Drawing.Image) null;
    if (imageBytes != null)
    {
      try
      {
        image = System.Drawing.Image.FromStream((Stream) new MemoryStream(imageBytes), true, false);
        imageBytes = (byte[]) null;
      }
      catch
      {
        if (isImageFromScratch)
          throw new ArgumentException("Argument is not image byte array");
        image = System.Drawing.Image.FromStream(WPicture.GetManifestResourceStream("ImageNotFound.jpg"), true, false);
        imageBytes = (byte[]) null;
      }
    }
    return image;
  }

  internal static Stream GetManifestResourceStream(string fileName)
  {
    Assembly executingAssembly = Assembly.GetExecutingAssembly();
    foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
    {
      if (manifestResourceName.EndsWith("." + fileName))
      {
        fileName = manifestResourceName;
        break;
      }
    }
    return executingAssembly.GetManifestResourceStream(fileName);
  }

  [DllImport("gdi32.dll")]
  public static extern int GetEnhMetaFileBits(int hemf, int cbBuffer, byte[] lpbBuffer);

  [DllImport("gdi32.dll")]
  public static extern bool DeleteEnhMetaFile(IntPtr hemf);

  internal static byte[] LoadMetafile(System.Drawing.Imaging.Metafile metaFile)
  {
    System.Drawing.Imaging.Metafile metafile1 = metaFile.Clone() as System.Drawing.Imaging.Metafile;
    IntPtr henhmetafile = metafile1.GetHenhmetafile();
    int int32 = henhmetafile.ToInt32();
    int enhMetaFileBits = WPicture.GetEnhMetaFileBits(int32, 0, (byte[]) null);
    byte[] lpbBuffer = new byte[enhMetaFileBits];
    if (WPicture.GetEnhMetaFileBits(int32, enhMetaFileBits, lpbBuffer) <= 0)
    {
      try
      {
        WPicture.DeleteEnhMetaFile(henhmetafile);
        metafile1.Dispose();
        Rectangle bounds = metaFile.GetMetafileHeader().Bounds;
        Graphics graphics1 = Graphics.FromImage((System.Drawing.Image) new Bitmap(bounds.Width, bounds.Height, metaFile.PixelFormat));
        IntPtr hdc = graphics1.GetHdc();
        MemoryStream memoryStream = new MemoryStream();
        System.Drawing.Imaging.Metafile metafile2 = new System.Drawing.Imaging.Metafile((Stream) memoryStream, hdc, EmfType.EmfPlusOnly);
        graphics1.ReleaseHdc(hdc);
        Graphics graphics2 = Graphics.FromImage((System.Drawing.Image) metafile2);
        graphics2.DrawImageUnscaled((System.Drawing.Image) metaFile, bounds);
        graphics2.Dispose();
        metafile2.Dispose();
        lpbBuffer = memoryStream.ToArray();
        memoryStream.Close();
      }
      catch
      {
        throw new ArgumentException("Invalid metafile format ");
      }
    }
    else
    {
      WPicture.DeleteEnhMetaFile(henhmetafile);
      metafile1.Dispose();
    }
    return lpbBuffer;
  }

  internal static byte[] LoadBitmap(System.Drawing.Image image)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      try
      {
        if (image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Tiff) || image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Bmp))
          image.Save((Stream) memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        else
          image.Save((Stream) memoryStream, image.RawFormat);
      }
      catch
      {
        image.Save((Stream) memoryStream, System.Drawing.Imaging.ImageFormat.Png);
      }
      return memoryStream.ToArray();
    }
  }

  private void ConvertBitmap(System.Drawing.Image image)
  {
    if (this.m_imageRecord != null)
    {
      --this.m_imageRecord.OccurenceCount;
      this.m_imageRecord = (ImageRecord) null;
    }
    this.m_imageRecord = this.Document.Images.LoadImage(WPicture.LoadBitmap(image));
  }

  private void ResetImageData()
  {
    if (this.m_inlinePictureShape.ShapeContainer != null && this.m_inlinePictureShape.ShapeContainer.Shape != null)
      this.m_inlinePictureShape.ShapeContainer = (MsofbtSpContainer) null;
    if (this.m_imageRecord != null)
    {
      --this.m_imageRecord.OccurenceCount;
      this.m_imageRecord = (ImageRecord) null;
    }
    this.m_size = new SizeF(float.MinValue, float.MinValue);
  }

  void IWidget.InitLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    this.IsWrappingBoundsAdded = false;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => dc.MeasureImage(this);

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    WParagraph wparagraph = this.OwnerParagraph;
    if (this.Owner is InlineContentControl || this.Owner is XmlParagraphItem || this.Owner is GroupShape || this.Owner is ChildGroupShape)
      wparagraph = this.GetOwnerParagraphValue();
    Entity ownerEntity = wparagraph.GetOwnerEntity();
    if (!wparagraph.IsInCell || !((IWidget) wparagraph).LayoutInfo.IsClipped)
    {
      switch (ownerEntity)
      {
        case Shape _:
        case WTextBox _:
        case ChildShape _:
          break;
        default:
          goto label_5;
      }
    }
    this.m_layoutInfo.IsClipped = true;
label_5:
    this.m_layoutInfo.IsVerticalText = ((IWidget) wparagraph).LayoutInfo.IsVerticalText;
    if (this.TextWrappingStyle != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkipBottomAlign = true;
    if (this.CharacterFormat.Hidden || !this.Visible && this.GetTextWrappingStyle() != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkip = true;
    if (!this.IsDeleteRevision || this.Document.RevisionOptions.ShowDeletedText)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_embedBody != null)
    {
      this.m_embedBody.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal SizeF ConvertSize(System.Drawing.Image image)
  {
    SizeF sizeF = image != null ? (SizeF) image.Size : throw new ArgumentNullException(nameof (image));
    this.m_imageRecord.Size = image.Size;
    this.m_imageRecord.ImageFormat = image.RawFormat;
    if (image.PixelFormat == PixelFormat.Format8bppIndexed || image.PixelFormat == PixelFormat.Format4bppIndexed || image.PixelFormat == PixelFormat.Format1bppIndexed || image is System.Drawing.Imaging.Metafile && image.PixelFormat == PixelFormat.Format32bppRgb || !Enum.IsDefined(typeof (PixelFormat), (object) image.PixelFormat))
      return UnitsConvertor.Instance.ConvertFromPixels((SizeF) image.Size, PrintUnits.Point, image.HorizontalResolution);
    using (Graphics g = Graphics.FromImage(image))
      return new UnitsConvertor(g).ConvertFromPixels(image.Size, PrintUnits.Point);
  }

  private void CheckPicSizeForPartialTrustMode(Syncfusion.DocIO.DLS.Entities.Image image)
  {
    if (image == null)
      return;
    this.m_size = this.ConvertSizeForPartialTrustMode(image);
  }

  private void CheckPicSize(System.Drawing.Image image)
  {
    if (image == null)
      return;
    this.m_size = this.ConvertSize(image);
  }
}
