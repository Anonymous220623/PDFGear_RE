// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaImage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaImage : PdfXfaField
{
  private PdfStream m_imageStream;
  private SizeF m_size = SizeF.Empty;
  private RectangleF m_bounds;
  private string m_subFormName;
  private PdfXfaRotateAngle m_rotate;
  internal PdfXfaForm parent;
  private PdfBitmap m_image;
  internal string imageBase64String = string.Empty;
  internal string imageFormat = string.Empty;
  internal bool isBase64Type;

  internal PdfStream ImageStream
  {
    get => this.m_imageStream;
    set
    {
      this.m_imageStream = value;
      List<PdfName> pdfNameList = new List<PdfName>();
      foreach (PdfName key in this.m_imageStream.Keys)
        pdfNameList.Add(key);
      foreach (PdfName key in pdfNameList)
        this.m_imageStream.Remove(key);
    }
  }

  public SizeF Size
  {
    get => this.m_size;
    set => this.m_size = value;
  }

  internal RectangleF ImageBounds
  {
    set
    {
      this.m_bounds = value;
      this.m_size = this.m_bounds.Size;
    }
    get => this.m_bounds;
  }

  internal string SubFormName
  {
    set => this.m_subFormName = value;
    get => this.m_subFormName;
  }

  public PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  public PdfXfaImage(string name, string path)
  {
    this.m_image = new PdfBitmap(path);
    this.Name = name;
    if (this.m_image.InternalImage != null)
    {
      this.isBase64Type = true;
      this.imageFormat = this.GetImageType(this.m_image.InternalImage.RawFormat);
      this.imageBase64String = Convert.ToBase64String(File.ReadAllBytes(path));
    }
    else
    {
      this.m_image.Save();
      this.ImageStream = this.m_image.Stream;
    }
    this.ImageBounds = new RectangleF(new PointF(), new SizeF((float) this.m_image.Width, (float) this.m_image.Height));
  }

  public PdfXfaImage(string name, string path, SizeF size)
  {
    this.Name = name;
    this.isBase64Type = true;
    Image image = Image.FromFile(path);
    this.imageFormat = this.GetImageType(image.RawFormat);
    image.Dispose();
    this.imageBase64String = Convert.ToBase64String(File.ReadAllBytes(path));
    this.ImageBounds = new RectangleF(new PointF(), size);
  }

  public PdfXfaImage(string name, string path, float width, float height)
  {
    this.Name = name;
    this.isBase64Type = true;
    Image image = Image.FromFile(path);
    this.imageFormat = this.GetImageType(image.RawFormat);
    image.Dispose();
    this.imageBase64String = Convert.ToBase64String(File.ReadAllBytes(path));
    this.ImageBounds = new RectangleF(new PointF(), new SizeF(width, height));
  }

  public PdfXfaImage(string name, Image image)
  {
    this.Name = name;
    this.isBase64Type = true;
    this.m_image = new PdfBitmap(image);
    this.imageFormat = this.GetImageType(image.RawFormat);
    MemoryStream memoryStream = new MemoryStream();
    image.Save((Stream) memoryStream, image.RawFormat);
    this.imageFormat = this.GetImageType(image.RawFormat);
    this.imageBase64String = Convert.ToBase64String(memoryStream.ToArray());
    this.ImageBounds = new RectangleF(new PointF(), new SizeF((float) this.m_image.Width, (float) this.m_image.Height));
  }

  public PdfXfaImage(string name, Image image, float width, float height)
  {
    this.Name = name;
    this.isBase64Type = true;
    MemoryStream memoryStream = new MemoryStream();
    image.Save((Stream) memoryStream, image.RawFormat);
    this.imageFormat = this.GetImageType(image.RawFormat);
    this.imageBase64String = Convert.ToBase64String(memoryStream.ToArray());
    this.ImageBounds = new RectangleF(new PointF(), new SizeF(width, height));
  }

  public PdfXfaImage(string name, Image image, SizeF size)
  {
    this.Name = name;
    this.isBase64Type = true;
    MemoryStream memoryStream = new MemoryStream();
    image.Save((Stream) memoryStream, image.RawFormat);
    this.imageFormat = this.GetImageType(image.RawFormat);
    this.imageBase64String = Convert.ToBase64String(memoryStream.ToArray());
    this.ImageBounds = new RectangleF(new PointF(), size);
  }

  public PdfXfaImage(string name, PdfBitmap image)
  {
    this.m_image = image;
    this.Name = name;
    if (this.m_image.InternalImage != null)
    {
      this.isBase64Type = true;
      this.imageFormat = this.GetImageType(this.m_image.InternalImage.RawFormat);
      MemoryStream memoryStream = new MemoryStream();
      image.InternalImage.Save((Stream) memoryStream, image.InternalImage.RawFormat);
      this.imageBase64String = Convert.ToBase64String(memoryStream.ToArray());
    }
    else
    {
      this.m_image.Save();
      this.ImageStream = this.m_image.Stream;
    }
    this.ImageBounds = new RectangleF(new PointF(), new SizeF((float) image.Width, (float) image.Height));
  }

  public PdfXfaImage(string name, Stream stream)
  {
    this.imageBase64String = Convert.ToBase64String(this.StreamToByteArray(stream));
    this.isBase64Type = true;
    this.Name = name;
    this.m_image = new PdfBitmap(stream);
    if (this.m_image.InternalImage != null)
      this.imageFormat = this.GetImageType(this.m_image.InternalImage.RawFormat);
    this.ImageBounds = new RectangleF(new PointF(), new SizeF((float) this.m_image.Width, (float) this.m_image.Height));
  }

  public PdfXfaImage(string name, Stream stream, SizeF size)
  {
    this.imageBase64String = Convert.ToBase64String(this.StreamToByteArray(stream));
    this.isBase64Type = true;
    this.Name = name;
    this.imageFormat = this.GetImageType(Image.FromStream(stream).RawFormat);
    this.ImageBounds = new RectangleF(new PointF(), size);
  }

  public PdfXfaImage(string name, Stream stream, float width, float height)
  {
    this.imageBase64String = Convert.ToBase64String(this.StreamToByteArray(stream));
    this.isBase64Type = true;
    this.Name = name;
    this.imageFormat = this.GetImageType(Image.FromStream(stream).RawFormat);
    this.ImageBounds = new RectangleF(new PointF(), new SizeF(width, height));
  }

  internal PdfXfaImage(PdfBitmap image, RectangleF bounds)
  {
    this.m_image = image;
    image.Save();
    this.ImageStream = image.Stream;
    this.ImageBounds = bounds;
  }

  private byte[] StreamToByteArray(Stream stream)
  {
    stream.Position = 0L;
    byte[] buffer = new byte[stream.Length];
    stream.Read(buffer, 0, buffer.Length);
    return buffer;
  }

  private string GetImageType(ImageFormat imf)
  {
    string imageType = string.Empty;
    if (imf.Equals((object) ImageFormat.Jpeg))
      imageType = "jpg";
    else if (imf.Equals((object) ImageFormat.Png))
      imageType = "png";
    else if (imf.Equals((object) ImageFormat.Tiff))
      imageType = "tiff";
    else if (imf.Equals((object) ImageFormat.Bmp))
      imageType = "bmp";
    else if (imf.Equals((object) ImageFormat.Gif))
      imageType = "gif";
    return imageType;
  }

  internal void Save(int fieldCount, string imageName, XfaWriter xfaWriter)
  {
    xfaWriter.Write.WriteStartElement("draw");
    if (this.Name != string.Empty && this.Name != null)
      xfaWriter.Write.WriteAttributeString("name", this.Name);
    else
      xfaWriter.Write.WriteAttributeString("name", "image" + fieldCount.ToString());
    xfaWriter.SetRPR(this.Rotate, this.Visibility, false);
    if (this.Size != SizeF.Empty)
      xfaWriter.SetSize(this.Size.Height, this.Size.Width, 0.0f, 0.0f);
    else
      xfaWriter.SetSize(this.ImageBounds.Height, this.ImageBounds.Width, 0.0f, 0.0f);
    xfaWriter.WriteUI("imageEdit", (Dictionary<string, string>) null, (PdfXfaBorder) null);
    xfaWriter.WriteMargins(this.Margins);
    xfaWriter.Write.WriteStartElement("value");
    xfaWriter.Write.WriteStartElement("image");
    if (!this.isBase64Type)
    {
      xfaWriter.Write.WriteAttributeString("href", imageName);
      xfaWriter.Write.WriteAttributeString("aspect", "none");
    }
    else
    {
      string str = "image";
      if (this.imageFormat != string.Empty)
        str = $"{str}/{this.imageFormat}";
      xfaWriter.Write.WriteAttributeString("contentType", str);
      xfaWriter.Write.WriteAttributeString("aspect", "none");
      xfaWriter.Write.WriteString(this.imageBase64String);
    }
    xfaWriter.Write.WriteEndElement();
    xfaWriter.Write.WriteEndElement();
    xfaWriter.Write.WriteEndElement();
  }

  internal void SaveAcroForm(PdfPage page, RectangleF bounds)
  {
    RectangleF rectangleF = new RectangleF();
    SizeF size = this.GetSize();
    rectangleF = new RectangleF(new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top), new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom)));
    PdfImage image = !this.isBase64Type ? PdfImage.FromStream((Stream) this.m_imageStream.InternalStream) : (PdfImage) new PdfBitmap((Stream) new MemoryStream(Convert.FromBase64String(this.imageBase64String)));
    PdfGraphics graphics = page.Graphics;
    graphics.Save();
    graphics.TranslateTransform(rectangleF.X, rectangleF.Y);
    graphics.RotateTransform((float) -this.GetRotationAngle());
    RectangleF rectangle = RectangleF.Empty;
    switch (this.GetRotationAngle())
    {
      case 0:
        rectangle = new RectangleF(0.0f, 0.0f, rectangleF.Width, rectangleF.Height);
        break;
      case 90:
        rectangle = new RectangleF(-rectangleF.Height, 0.0f, rectangleF.Height, rectangleF.Width);
        break;
      case 180:
        rectangle = new RectangleF(-rectangleF.Width, -rectangleF.Height, rectangleF.Width, rectangleF.Height);
        break;
      case 270:
        rectangle = new RectangleF(0.0f, -rectangleF.Width, rectangleF.Height, rectangleF.Width);
        break;
    }
    graphics.DrawImage(image, rectangle);
    graphics.Restore();
  }

  internal SizeF GetSize()
  {
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(this.Size.Height, this.Size.Width) : this.Size;
  }

  private int GetRotationAngle()
  {
    int rotationAngle = 0;
    if (this.Rotate != PdfXfaRotateAngle.RotateAngle0)
    {
      switch (this.Rotate)
      {
        case PdfXfaRotateAngle.RotateAngle90:
          rotationAngle = 90;
          break;
        case PdfXfaRotateAngle.RotateAngle180:
          rotationAngle = 180;
          break;
        case PdfXfaRotateAngle.RotateAngle270:
          rotationAngle = 270;
          break;
      }
    }
    return rotationAngle;
  }

  internal object Clone() => this.MemberwiseClone();
}
