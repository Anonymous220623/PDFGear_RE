// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.BitmapBLIP
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class BitmapBLIP : Blip
{
  private byte[] m_rgbUid;
  private byte[] m_rgbUidPrimary;
  private byte m_tag;
  private MemoryStream m_pvImageBytes;

  public BitmapBLIP()
  {
    this.m_rgbUid = new byte[16 /*0x10*/];
    this.m_rgbUidPrimary = new byte[16 /*0x10*/];
  }

  public byte[] RgbUid
  {
    get => this.m_rgbUid;
    set => this.m_rgbUid = value;
  }

  public byte[] RgbUidPrimary
  {
    get => this.m_rgbUidPrimary;
    set => this.m_rgbUidPrimary = value;
  }

  public byte Tag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public MemoryStream ImageBytes
  {
    get => this.m_pvImageBytes;
    set => this.m_pvImageBytes = value;
  }

  public override Image Read(Stream stream, int length, bool hasPrimaryUid)
  {
    stream.Read(this.RgbUid, 0, this.RgbUid.Length);
    int num1 = 16 /*0x10*/;
    if (hasPrimaryUid)
    {
      stream.Read(this.RgbUidPrimary, 0, this.RgbUidPrimary.Length);
      num1 += 16 /*0x10*/;
    }
    this.Tag = (byte) stream.ReadByte();
    this.ImageBytes = (MemoryStream) null;
    int num2 = num1 + 1;
    byte[] buffer = new byte[length - num2];
    stream.Read(buffer, 0, buffer.Length);
    this.ImageBytes = new MemoryStream(buffer, 0, buffer.Length);
    return (Image) new Bitmap((Stream) this.ImageBytes);
  }

  internal override void Write(
    Stream stream,
    MemoryStream image,
    MSOBlipType imageFormat,
    byte[] id)
  {
    this.WriteDefaults(stream, image.Length, imageFormat, id);
    byte[] buffer = new byte[image.Length];
    image.Position = 0L;
    image.Read(buffer, 0, buffer.Length);
    stream.Write(buffer, 0, buffer.Length);
  }

  private void WriteDefaults(Stream stream, long size, MSOBlipType type, byte[] id)
  {
    new MSOFBH()
    {
      Msofbt = MSOFBT.msofbtBSE,
      Inst = 6U,
      Version = 2U,
      Length = ((uint) ((ulong) size + 61UL))
    }.Write(stream);
    FBSE fbse = new FBSE();
    fbse.Win32 = type;
    fbse.MacOS = type;
    id.CopyTo((Array) fbse.Uid, 0);
    fbse.Usage = MSOBlipUsage.msoblipUsageDefault;
    fbse.Name = (byte) 0;
    fbse.Size = (uint) ((ulong) size + 25UL);
    fbse.Delay = 68U;
    fbse.Ref = 1U;
    fbse.Tag = (ushort) byte.MaxValue;
    fbse.Unused2 = (byte) 0;
    fbse.Unused3 = (byte) 0;
    fbse.Write(stream);
    new MSOFBH()
    {
      Length = ((uint) size + 17U),
      Msofbt = ((MSOFBT) (61464 + type)),
      Inst = 1760U,
      Version = 0U
    }.Write(stream);
    stream.Write(id, 0, id.Length);
    stream.WriteByte(byte.MaxValue);
  }

  internal override void Close()
  {
    base.Close();
    this.m_rgbUid = (byte[]) null;
    this.m_rgbUidPrimary = (byte[]) null;
    this.m_pvImageBytes.Close();
    this.m_pvImageBytes = (MemoryStream) null;
  }
}
