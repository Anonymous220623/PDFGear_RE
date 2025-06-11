// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.BstoreContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class BstoreContainer : BaseWordRecord
{
  private FBSE m_fbse;
  private BitmapBLIP m_bitmapBlip;
  private Image m_bitmap;
  private Blip m_blip;

  public BstoreContainer()
  {
    this.m_fbse = new FBSE();
    this.m_bitmapBlip = new BitmapBLIP();
  }

  internal FBSE Fbse
  {
    get => this.m_fbse;
    set => this.m_fbse = value;
  }

  public Image Bitmap => this.m_bitmap;

  public void Read(Stream stream)
  {
    this.m_fbse.Read(stream);
    MSOFBH msofbh = new MSOFBH();
    msofbh.Read(stream);
    bool chr = false;
    switch (msofbh.Msofbt - 61464)
    {
      case (MSOFBT) 2:
        if (((int) msofbh.Inst ^ 980) == 1)
          chr = true;
        this.m_blip = (Blip) new MetafileBlip();
        break;
      case (MSOFBT) 3:
        if (((int) msofbh.Inst ^ 534) == 1)
          chr = true;
        this.m_blip = (Blip) new MetafileBlip();
        break;
      case (MSOFBT) 4:
        if (((int) msofbh.Inst ^ 1346) == 1)
          chr = true;
        this.m_blip = (Blip) new MetafileBlip();
        break;
      case (MSOFBT) 5:
        if (((int) msofbh.Inst ^ 1130) == 1)
          chr = true;
        this.m_blip = (Blip) new BitmapBLIP();
        break;
      case (MSOFBT) 6:
        if (((int) msofbh.Inst ^ 1760) == 1)
          chr = true;
        this.m_blip = (Blip) new BitmapBLIP();
        break;
      case (MSOFBT) 7:
        if (((int) msofbh.Inst ^ 1960) == 1)
          chr = true;
        this.m_blip = (Blip) new BitmapBLIP();
        break;
    }
    this.m_bitmap = this.m_blip.Read(stream, (int) msofbh.Length, chr);
  }

  internal void Write(Stream stream, MemoryStream imageStream, byte[] id, Image image)
  {
    if (image is Metafile)
    {
      this.m_blip = (Blip) new MetafileBlip();
      (this.m_blip as MetafileBlip).Metafile = image as Metafile;
      this.m_blip.Write(stream, imageStream, MSOBlipType.msoblipEMF, id);
    }
    else
    {
      this.m_blip = (Blip) new BitmapBLIP();
      this.m_blip.Write(stream, imageStream, MSOBlipType.msoblipPNG, id);
    }
  }

  internal override void Close()
  {
    if (this.m_bitmap != null)
    {
      this.m_bitmap.Dispose();
      this.m_bitmap = (Image) null;
    }
    if (this.m_blip == null)
      return;
    this.m_blip.Close();
  }
}
