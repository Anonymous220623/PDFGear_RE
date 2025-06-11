// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Background
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Background : XDLSSerializableBase
{
  private BackgroundType m_effectType;
  private Color m_color = Color.White;
  private Color m_backColor = Color.White;
  private ImageRecord m_imageRecord;
  private BackgroundGradient m_gradient = new BackgroundGradient();
  private BackgroundFillType m_fillType;
  private EscherClass m_escher;
  private Stream m_patternFill;
  private byte[] m_patternImage;

  public BackgroundType Type
  {
    get => this.m_effectType;
    set
    {
      this.m_effectType = value;
      if (this.Document.IsOpening)
        return;
      this.Document.Settings.DisplayBackgrounds = true;
    }
  }

  public Image Picture
  {
    get => this.GetImageValue();
    set
    {
      if (this.m_imageRecord != null)
        --this.m_imageRecord.OccurenceCount;
      this.m_fillType = BackgroundFillType.msofillPicture;
      this.m_effectType = BackgroundType.Picture;
      this.LoadImage(value);
      if (this.Document.IsOpening)
        return;
      this.Document.Settings.DisplayBackgrounds = true;
    }
  }

  internal Image Image => this.GetImageValue();

  public Color Color
  {
    get => this.m_color;
    set
    {
      this.m_effectType = BackgroundType.Color;
      this.m_color = value;
      if (this.Document.IsOpening)
        return;
      this.Document.Settings.DisplayBackgrounds = true;
    }
  }

  public BackgroundGradient Gradient
  {
    get => this.m_gradient;
    set
    {
      this.m_gradient = value;
      this.m_effectType = BackgroundType.Gradient;
      this.m_gradient.SetOwner((OwnerHolder) this.Document);
      if (this.Document.IsOpening)
        return;
      this.Document.Settings.DisplayBackgrounds = true;
    }
  }

  internal ImageRecord ImageRecord
  {
    get => this.m_imageRecord;
    set
    {
      if (this.m_imageRecord != null)
        --this.m_imageRecord.OccurenceCount;
      this.m_imageRecord = value;
      ++this.m_imageRecord.OccurenceCount;
    }
  }

  internal byte[] ImageBytes
  {
    get => this.m_imageRecord == null ? (byte[]) null : this.m_imageRecord.ImageBytes;
    set
    {
      if (this.m_imageRecord != null)
        --this.m_imageRecord.OccurenceCount;
      this.LoadImage(this.GetImage(value));
    }
  }

  internal BackgroundFillType FillType
  {
    get => this.m_fillType;
    set => this.m_fillType = value;
  }

  internal Color PictureBackColor => this.m_backColor;

  internal Stream PatternFill
  {
    get => this.m_patternFill;
    set
    {
      this.m_effectType = BackgroundType.Texture;
      this.m_patternFill = value;
    }
  }

  internal byte[] PatternImageBytes
  {
    get => this.m_patternImage;
    set => this.m_patternImage = value;
  }

  internal Background(WordDocument doc, BackgroundType type)
    : base(doc, (Entity) null)
  {
    this.m_effectType = type;
  }

  internal Background(WordDocument doc)
    : base(doc, (Entity) null)
  {
    this.m_escher = doc.Escher;
    this.GetBackgroundData(this.m_escher.BackgroundContainer, true);
  }

  internal Background(WordDocument doc, MsofbtSpContainer container)
    : base(doc, (Entity) null)
  {
    this.m_escher = doc.Escher;
    this.GetBackgroundData(container, false);
  }

  internal Background Clone()
  {
    Background background = new Background(this.Document, this.Type);
    if (this.m_imageRecord != null)
      background.m_imageRecord = new ImageRecord(this.Document, this.m_imageRecord);
    if (this.m_gradient != null)
      background.Gradient = this.Gradient.Clone();
    background.Color = this.Color;
    return background;
  }

  internal void UpdateImageRecord(WordDocument doc)
  {
    if (this.m_imageRecord == null)
      return;
    ImageRecord imageRecord = this.m_imageRecord;
    this.m_imageRecord = !imageRecord.IsMetafile ? doc.Images.LoadImage(imageRecord.ImageBytes) : doc.Images.LoadMetaFileImage(imageRecord.m_imageBytes, true);
    this.m_imageRecord.Size = imageRecord.Size;
    this.m_imageRecord.ImageFormat = imageRecord.ImageFormat;
    this.m_imageRecord.Length = imageRecord.Length;
    imageRecord.Close();
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Type"))
      this.m_effectType = (BackgroundType) reader.ReadEnum("Type", typeof (BackgroundType));
    if (reader.HasAttribute("FillColor"))
      this.m_color = reader.ReadColor("FillColor");
    if (!reader.HasAttribute("FillBackgroundColor"))
      return;
    this.m_backColor = reader.ReadColor("FillBackgroundColor");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("Type", (Enum) this.m_effectType);
    if (this.m_imageRecord != null && this.m_imageRecord.IsMetafile)
      writer.WriteValue("IsMetafile", this.m_imageRecord.IsMetafile);
    if (this.m_color != Color.White)
      writer.WriteValue("FillColor", this.m_color);
    if (!(this.m_backColor != Color.White))
      return;
    writer.WriteValue("FillBackgroundColor", this.m_backColor);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.ImageBytes != null)
      writer.WriteChildBinaryElement("image", this.ImageBytes);
    else
      (writer as XDLSWriter).WriteImage(this.Image);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "image")
      this.LoadImage(this.GetImage(reader.ReadChildBinaryElement()));
    return flag;
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("gradient", (object) this.m_gradient);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_imageRecord != null)
    {
      this.m_imageRecord.Close();
      this.m_imageRecord = (ImageRecord) null;
    }
    if (this.m_gradient != null)
    {
      this.m_gradient.Close();
      this.m_gradient = (BackgroundGradient) null;
    }
    if (this.m_escher != null)
    {
      this.m_escher.Close();
      this.m_escher = (EscherClass) null;
    }
    if (this.m_patternFill != null)
    {
      this.m_patternFill.Close();
      this.m_patternFill = (Stream) null;
    }
    if (this.m_patternImage == null)
      return;
    this.m_patternImage = (byte[]) null;
  }

  internal void SetBackgroundColor(Color color) => this.m_color = color;

  internal void SetPatternFillValue(Stream stream) => this.m_patternFill = stream;

  private void GetBackgroundData(MsofbtSpContainer container, bool isDocBackground)
  {
    if (container == null || !container.HasFillEffect())
      return;
    this.m_fillType = container.GetBackgroundFillType();
    this.m_effectType = container.GetBackgroundType();
    if (this.m_effectType == BackgroundType.NoBackground && isDocBackground)
      this.m_effectType = BackgroundType.Color;
    if (this.m_effectType == BackgroundType.NoBackground)
      return;
    switch (this.m_effectType)
    {
      case BackgroundType.Gradient:
        this.m_gradient = new BackgroundGradient(this.Document, container);
        this.m_gradient.SetOwner((OwnerHolder) this.Document);
        break;
      case BackgroundType.Picture:
      case BackgroundType.Texture:
        this.m_imageRecord = container.GetBackgroundImage(this.m_escher);
        this.m_backColor = container.GetBackgroundColor(true);
        break;
      case BackgroundType.Color:
        this.m_color = container.GetBackgroundColor(false);
        if (this.m_effectType != BackgroundType.Color || !(this.m_color == Color.White))
          break;
        this.m_effectType = BackgroundType.NoBackground;
        break;
    }
  }

  private Image GetImageValue()
  {
    Image imageValue = (Image) null;
    if (this.ImageBytes != null)
    {
      try
      {
        imageValue = Image.FromStream((Stream) new MemoryStream(this.ImageBytes), true, false);
      }
      catch
      {
        throw new ArgumentException("Argument is not image byte array");
      }
    }
    return imageValue;
  }

  private Image GetImage(byte[] imageBytes)
  {
    Image image = (Image) null;
    if (imageBytes != null)
    {
      try
      {
        image = Image.FromStream((Stream) new MemoryStream(imageBytes), true, false);
        imageBytes = (byte[]) null;
      }
      catch
      {
        throw new ArgumentException("Argument is not image byte array");
      }
    }
    return image;
  }

  private void LoadImage(Image image)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    this.m_imageRecord = !(image is Metafile) ? this.Document.Images.LoadImage(WPicture.LoadBitmap(image)) : this.Document.Images.LoadMetaFileImage(WPicture.LoadMetafile(image as Metafile), false);
    this.m_imageRecord.UpdateImageSize(image);
  }
}
