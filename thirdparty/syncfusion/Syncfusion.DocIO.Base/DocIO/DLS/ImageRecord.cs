// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ImageRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ImageRecord
{
  private int m_imageId;
  internal byte[] m_imageBytes;
  private int m_occurenceCount;
  private byte m_bFlags;
  private WordDocument m_doc;
  private Size m_size = new Size(int.MaxValue, int.MaxValue);
  private System.Drawing.Imaging.ImageFormat m_imageFormat;
  private Syncfusion.DocIO.DLS.Entities.ImageFormat m_imageFormatForPartialTrustMode;
  private int m_length = int.MinValue;

  internal int ImageId
  {
    get => this.m_imageId;
    set => this.m_imageId = value;
  }

  internal byte[] ImageBytes
  {
    get
    {
      return this.IsMetafile && this.m_doc != null ? this.m_doc.Images.DecompressImageBytes(this.m_imageBytes) : this.m_imageBytes;
    }
    set => this.m_imageBytes = this.m_doc.Images.CompressImageBytes(value);
  }

  internal byte[] ImageHash
  {
    get
    {
      return this.m_imageBytes != null ? ImageRecord.GetHMACSHA1().ComputeHash(this.m_imageBytes) : (byte[]) null;
    }
  }

  internal int OccurenceCount
  {
    get => this.m_occurenceCount;
    set
    {
      this.m_occurenceCount = value;
      if (this.m_occurenceCount != 0)
        return;
      if (this.m_doc != null)
        this.m_doc.Images.Remove(this.m_imageId);
      this.Close();
    }
  }

  internal bool IsMetafile
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsAdded
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal Size Size
  {
    get
    {
      if ((double) this.m_size.Width == -3.4028234663852886E+38 || (double) this.m_size.Height == -3.4028234663852886E+38)
        this.UpdateImageSize(this.GetImageInternal(this.ImageBytes));
      return this.m_size;
    }
    set => this.m_size = value;
  }

  internal Syncfusion.DocIO.DLS.Entities.ImageFormat ImageFormatForPartialTrustMode
  {
    get
    {
      this.UpdateImageSizeForPartialTrustMode(this.GetImageInternalForPartialTrustMode(this.ImageBytes));
      return this.m_imageFormatForPartialTrustMode;
    }
    set => this.m_imageFormatForPartialTrustMode = value;
  }

  internal System.Drawing.Imaging.ImageFormat ImageFormat
  {
    get
    {
      this.UpdateImageSize(this.GetImageInternal(this.ImageBytes));
      return this.m_imageFormat;
    }
    set => this.m_imageFormat = value;
  }

  internal int Length
  {
    get
    {
      if (this.m_length == int.MinValue)
        this.m_length = this.ImageBytes.Length;
      return this.m_length;
    }
    set => this.m_length = value;
  }

  internal ImageRecord(WordDocument doc, byte[] imageBytes)
  {
    this.m_doc = doc;
    this.m_imageBytes = imageBytes;
  }

  internal ImageRecord(WordDocument doc, ImageRecord imageRecord)
  {
    this.m_doc = doc;
    this.m_imageBytes = imageRecord.m_imageBytes;
    this.IsMetafile = imageRecord.IsMetafile;
    this.m_length = imageRecord.m_length;
    this.m_imageFormat = imageRecord.m_imageFormat;
    this.m_size = imageRecord.m_size;
    this.m_imageId = imageRecord.ImageId;
  }

  internal void Detach()
  {
    --this.m_occurenceCount;
    if (this.m_occurenceCount != 0)
      return;
    if (this.m_doc != null)
      this.m_doc.Images.Remove(this.m_imageId);
    this.m_imageId = 0;
    this.m_occurenceCount = 0;
  }

  internal bool IsMetafileHeaderPresent(byte[] imagebytes)
  {
    bool flag = false;
    byte[] numArray = new byte[4]
    {
      (byte) 215,
      (byte) 205,
      (byte) 198,
      (byte) 154
    };
    if (imagebytes != null && imagebytes.Length > 22)
    {
      flag = true;
      int index = 0;
      if (index < 4 && (int) numArray[index] != (int) imagebytes[index])
      {
        flag = false;
        return flag;
      }
    }
    return flag;
  }

  internal void Attach()
  {
    ++this.m_occurenceCount;
    this.m_doc.Images.Add(this);
  }

  internal void Close()
  {
    this.m_bFlags = (byte) 0;
    this.m_doc = (WordDocument) null;
    this.m_imageBytes = (byte[]) null;
    this.m_imageId = 0;
    this.m_occurenceCount = 0;
    this.m_imageFormat = (System.Drawing.Imaging.ImageFormat) null;
  }

  internal void UpdateImageSizeForPartialTrustMode(Syncfusion.DocIO.DLS.Entities.Image image)
  {
    if (image == null)
      return;
    this.m_size = image.Size;
    this.m_imageFormatForPartialTrustMode = image.RawFormat;
    image.Dispose();
  }

  internal void UpdateImageSize(System.Drawing.Image image)
  {
    if (image == null)
      return;
    this.m_size = image.Size;
    this.m_imageFormat = image.RawFormat;
    image.Dispose();
  }

  private Syncfusion.DocIO.DLS.Entities.Image GetImageInternalForPartialTrustMode(byte[] imageBytes)
  {
    if (imageBytes == null)
      return (Syncfusion.DocIO.DLS.Entities.Image) null;
    this.m_length = imageBytes.Length;
    return this.GetImageInternalForPartialTrustMode(imageBytes);
  }

  private System.Drawing.Image GetImageInternal(byte[] imageBytes)
  {
    if (imageBytes == null)
      return (System.Drawing.Image) null;
    this.m_length = imageBytes.Length;
    return ImageRecord.GetImage(imageBytes);
  }

  internal static System.Drawing.Image GetImage(byte[] imageBytes)
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
        image = System.Drawing.Image.FromStream(WPicture.GetManifestResourceStream("ImageNotFound.jpg"), true, false);
        imageBytes = (byte[]) null;
      }
    }
    return image;
  }

  internal static HMACSHA1 GetHMACSHA1()
  {
    HMACSHA1 hmacshA1 = new HMACSHA1();
    hmacshA1.Key = new byte[20]
    {
      (byte) 73,
      (byte) 0,
      (byte) 109,
      (byte) 0,
      (byte) 103,
      (byte) 0,
      (byte) 72,
      (byte) 0,
      (byte) 97,
      (byte) 0,
      (byte) 115,
      (byte) 0,
      (byte) 104,
      (byte) 0,
      (byte) 75,
      (byte) 0,
      (byte) 101,
      (byte) 0,
      (byte) 121,
      (byte) 0
    };
    return hmacshA1;
  }
}
