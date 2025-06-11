// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.MetafileBlip
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class MetafileBlip : Blip
{
  private byte[] m_rgbUid;
  private byte[] m_rgbUidPrimary;
  private uint m_cbSave;
  private byte m_fCompression;
  private byte[] m_pvBits;
  private int m_length;
  private int m_rectLeft;
  private int m_rectTop;
  private int m_rectRight;
  private int m_rectBottom;
  private int m_rectWidth;
  private int m_rectHeight;
  private CompressionMethod m_compressionMethod;
  private byte m_fFilter;
  private byte[] m_compressedImage;
  private byte[] m_uncompressedImage;
  private Metafile m_srcMetafile;

  internal Metafile Metafile
  {
    get => this.m_srcMetafile;
    set => this.m_srcMetafile = value;
  }

  public MetafileBlip()
  {
    this.m_rgbUid = new byte[16 /*0x10*/];
    this.m_rgbUidPrimary = new byte[16 /*0x10*/];
  }

  public override Image Read(Stream stream, int length, bool hasPrimaryUid)
  {
    for (int index = 0; index < 16 /*0x10*/; ++index)
      this.m_rgbUid[index] = (byte) stream.ReadByte();
    this.m_length = BaseWordRecord.ReadInt32(stream);
    this.m_rectLeft = BaseWordRecord.ReadInt32(stream);
    this.m_rectTop = BaseWordRecord.ReadInt32(stream);
    this.m_rectRight = BaseWordRecord.ReadInt32(stream);
    this.m_rectBottom = BaseWordRecord.ReadInt32(stream);
    this.m_rectWidth = BaseWordRecord.ReadInt32(stream);
    this.m_rectHeight = BaseWordRecord.ReadInt32(stream);
    this.m_cbSave = BaseWordRecord.ReadUInt32(stream);
    this.m_fCompression = (byte) stream.ReadByte();
    this.m_fFilter = (byte) stream.ReadByte();
    this.m_pvBits = new byte[(IntPtr) this.m_cbSave];
    stream.Read(this.m_pvBits, 0, this.m_pvBits.Length);
    MemoryStream memoryStream1 = new MemoryStream(this.m_pvBits);
    if (this.m_fCompression != (byte) 0)
      return (Image) new Metafile((Stream) memoryStream1);
    CompressedStreamReader compressedStreamReader = new CompressedStreamReader((Stream) memoryStream1);
    MemoryStream memoryStream2 = new MemoryStream();
    byte[] buffer = new byte[4096 /*0x1000*/];
    while (true)
    {
      int count = compressedStreamReader.Read(buffer, 0, buffer.Length);
      if (count > 0)
        memoryStream2.Write(buffer, 0, count);
      else
        break;
    }
    memoryStream2.Position = 0L;
    return (Image) new Metafile((Stream) memoryStream2);
  }

  internal override void Write(
    Stream stream,
    MemoryStream image,
    MSOBlipType imageFormat,
    byte[] Uid)
  {
    this.m_uncompressedImage = image.ToArray();
    this.m_length = this.m_uncompressedImage.Length;
    this.m_compressedImage = this.m_uncompressedImage;
    Rectangle bounds = this.m_srcMetafile.GetMetafileHeader().Bounds;
    this.m_rectLeft = bounds.Left;
    this.m_rectTop = bounds.Top;
    this.m_rectRight = bounds.Right;
    this.m_rectBottom = bounds.Bottom;
    this.m_rectWidth = bounds.Width * 12700;
    this.m_rectHeight = bounds.Height * 12700;
    this.m_compressionMethod = CompressionMethod.msocompressionNone;
    this.m_fFilter = (byte) 254;
    this.WriteDefaults(stream, (long) this.m_length, Uid);
    stream.Write(Uid, 0, Uid.Length);
    BaseWordRecord.WriteInt32(stream, this.m_length);
    BaseWordRecord.WriteInt32(stream, this.m_rectLeft);
    BaseWordRecord.WriteInt32(stream, this.m_rectTop);
    BaseWordRecord.WriteInt32(stream, this.m_rectRight);
    BaseWordRecord.WriteInt32(stream, this.m_rectBottom);
    BaseWordRecord.WriteInt32(stream, this.m_rectWidth);
    BaseWordRecord.WriteInt32(stream, this.m_rectHeight);
    BaseWordRecord.WriteInt32(stream, this.m_compressedImage.Length);
    stream.WriteByte((byte) this.m_compressionMethod);
    stream.WriteByte(this.m_fFilter);
    stream.Write(this.m_compressedImage, 0, this.m_compressedImage.Length);
  }

  private void WriteDefaults(Stream stream, long size, byte[] Uid)
  {
    new MSOFBH()
    {
      Msofbt = MSOFBT.msofbtBSE,
      Inst = 2U,
      Version = 2U,
      Length = ((uint) ((ulong) size + 94UL))
    }.Write(stream);
    FBSE fbse = new FBSE();
    fbse.Win32 = MSOBlipType.msoblipEMF;
    fbse.MacOS = MSOBlipType.msoblipPICT;
    for (int index = 0; index < 16 /*0x10*/; ++index)
      fbse.Uid[index] = Uid[index];
    fbse.Usage = MSOBlipUsage.msoblipUsageDefault;
    fbse.Name = (byte) 0;
    fbse.Size = (uint) ((ulong) size + 58UL);
    fbse.Delay = 68U;
    fbse.Ref = 1U;
    fbse.Tag = (ushort) byte.MaxValue;
    fbse.Unused2 = (byte) 0;
    fbse.Unused3 = (byte) 0;
    fbse.Write(stream);
    new MSOFBH()
    {
      Length = ((uint) size + 50U),
      Msofbt = MSOFBT.msofbtBlipEMF,
      Inst = 980U,
      Version = 0U
    }.Write(stream);
  }

  internal override void Close()
  {
    base.Close();
    this.m_rgbUid = (byte[]) null;
    this.m_rgbUidPrimary = (byte[]) null;
    this.m_compressedImage = (byte[]) null;
    this.m_uncompressedImage = (byte[]) null;
    if (this.m_srcMetafile == null)
      return;
    this.m_srcMetafile.Dispose();
    this.m_srcMetafile = (Metafile) null;
  }
}
