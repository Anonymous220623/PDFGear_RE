// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PictureWatermark
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class PictureWatermark : Watermark
{
  private WPicture m_picture;
  private ImageRecord m_imageRecord;
  private byte m_bFlags = 1;
  private int m_originalPib = -1;

  public float Scaling
  {
    get => this.m_picture.HeightScale;
    set => this.m_picture.HeightScale = this.m_picture.WidthScale = value;
  }

  public bool Washout
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public Image Picture
  {
    get
    {
      return this.m_picture.Document == null && this.m_imageRecord != null ? this.GetImage(this.m_imageRecord.ImageBytes) : this.m_picture.GetImage(this.m_picture.ImageBytes, false);
    }
    set
    {
      if (this.m_picture.Document == null || !this.m_picture.Document.IsOpening)
        this.m_picture.Title = string.Empty;
      this.m_originalPib = -1;
      if (this.m_picture.Document != null)
      {
        this.m_picture.LoadImage(value);
      }
      else
      {
        this.m_imageRecord = new ImageRecord((WordDocument) null, !(value is Metafile) ? WPicture.LoadBitmap(value) : WPicture.LoadMetafile(value as Metafile));
        if (!(value is Metafile))
          return;
        this.m_imageRecord.IsMetafile = true;
      }
    }
  }

  internal WPicture WordPicture
  {
    get => this.m_picture;
    set
    {
      if (this.m_picture != null)
        this.m_picture.SetOwner((WordDocument) null, (OwnerHolder) null);
      this.m_picture = value;
      if (this.m_picture == null)
        return;
      this.m_picture.SetOwner((OwnerHolder) this);
    }
  }

  internal int OriginalPib
  {
    get => this.m_originalPib;
    set => this.m_originalPib = value;
  }

  public PictureWatermark()
    : base(WatermarkType.PictureWatermark)
  {
    this.m_picture = new WPicture((IWordDocument) null);
    this.m_picture.SetOwner((OwnerHolder) this);
    this.m_picture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
    this.m_picture.VerticalAlignment = ShapeVerticalAlignment.Center;
    this.m_picture.SetTextWrappingStyleValue(TextWrappingStyle.Behind);
    this.m_picture.IsBelowText = true;
  }

  public PictureWatermark(Image image, bool washout)
    : this()
  {
    this.Picture = image;
    this.Washout = washout;
  }

  internal PictureWatermark(WordDocument doc)
    : base(doc, WatermarkType.PictureWatermark)
  {
    this.m_picture = new WPicture((IWordDocument) doc);
    this.m_picture.SetOwner((OwnerHolder) this);
  }

  internal override void Close()
  {
    if (this.m_picture != null)
    {
      this.m_picture.Close();
      this.m_picture = (WPicture) null;
    }
    if (this.m_imageRecord != null)
    {
      this.m_imageRecord.Close();
      this.m_imageRecord = (ImageRecord) null;
    }
    base.Close();
  }

  internal void UpdateImage()
  {
    if (this.m_imageRecord == null)
      return;
    this.m_picture.LoadImage(this.m_imageRecord.ImageBytes, this.m_imageRecord.IsMetafile);
    this.m_imageRecord.Close();
    this.m_imageRecord = (ImageRecord) null;
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

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("image", (object) this.m_picture);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (!this.Washout)
      writer.WriteValue("PictureWashout", this.Washout);
    if (this.m_originalPib == -1)
      return;
    writer.WriteValue("PicturePib", this.m_originalPib);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("PictureWashout"))
      this.Washout = reader.ReadBoolean("PictureWashout");
    if (!reader.HasAttribute("PicturePib"))
      return;
    this.m_originalPib = reader.ReadInt("PicturePib");
  }

  protected override object CloneImpl()
  {
    PictureWatermark owner = (PictureWatermark) base.CloneImpl();
    owner.WordPicture = (WPicture) this.WordPicture.Clone();
    owner.WordPicture.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }
}
