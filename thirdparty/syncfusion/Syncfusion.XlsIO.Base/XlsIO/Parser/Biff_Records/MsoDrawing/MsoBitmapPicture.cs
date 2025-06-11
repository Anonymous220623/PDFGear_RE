// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoBitmapPicture
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
public class MsoBitmapPicture : MsoBase, IDisposable, IPictureRecord
{
  public const int DEF_DIB_HEADER_SIZE = 14;
  public const int DEF_COLOR_USED_OFFSET = 32 /*0x20*/;
  private const uint DEF_COLOR_SIZE = 4;
  internal const uint BlipDIBWithTwoUIDs = 1961;
  internal const uint BlipPNGWithTwoUIDs = 1761;
  internal const uint BlipJPEGWithTwoUIDs = 1347;
  private static readonly byte[] DEF_DIB_ID = new byte[2]
  {
    (byte) 66,
    (byte) 77
  };
  private static readonly byte[] DEF_RESERVED = new byte[4];
  private byte[] m_arrRgbUid = new byte[16 /*0x10*/];
  private byte[] m_arrRgbUidPrimary;
  private byte m_btTag = byte.MaxValue;
  private int m_iPictureDataOffset;
  private Image m_picture;
  private Stream m_pictStream;
  private MemoryStream m_stream;

  public MsoBitmapPicture(MsoBase parent)
    : base(parent)
  {
  }

  public MsoBitmapPicture(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsoBitmapPicture(MsoBase parent, Stream stream)
    : base(parent, stream, (GetNextMsoDrawingData) null)
  {
  }

  public Image Picture
  {
    get => this.m_picture;
    set
    {
      this.m_picture = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.EvaluateHash();
    }
  }

  public Stream PictureStream
  {
    get => this.m_pictStream;
    set
    {
      this.m_pictStream = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public byte[] RgbUid
  {
    get => this.m_arrRgbUid;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value.Length != this.m_arrRgbUid.Length)
        throw new ArgumentOutOfRangeException("value.Length");
      this.m_arrRgbUid = value;
    }
  }

  public byte Tag
  {
    get => this.m_btTag;
    set => this.m_btTag = value;
  }

  public int PictureDataOffset
  {
    get => this.m_iPictureDataOffset;
    set => this.m_iPictureDataOffset = value;
  }

  public bool IsDib => (this.Parent as MsofbtBSE).BlipType == MsoBlipType.msoblipDIB;

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    if (this.MsoRecordType == (MsoRecords) 0)
      this.MsoRecordType = (MsoRecords) 61470;
    this.m_iLength = 0;
    stream.Write(this.m_arrRgbUid, 0, 16 /*0x10*/);
    this.m_iLength += 16 /*0x10*/;
    if (this.HasTwoUIDs())
    {
      stream.Write(this.m_arrRgbUidPrimary, 0, 16 /*0x10*/);
      this.m_iLength += 16 /*0x10*/;
    }
    stream.WriteByte(this.m_btTag);
    ++this.m_iLength;
    int num = this.IsDib ? 14 : 0;
    MemoryStream memoryStream = new MemoryStream();
    this.m_picture.Save((Stream) memoryStream, MsofbtBSE.ConvertToImageFormat((this.Parent as MsofbtBSE).BlipType));
    memoryStream.Position = 0L;
    int length = (int) memoryStream.Length;
    byte[] buffer = new byte[10240];
    memoryStream.Position = (long) num;
    int count;
    for (iOffset = num; iOffset < length; iOffset += count)
    {
      count = memoryStream.Read(buffer, 0, 10240);
      stream.Write(buffer, 0, count);
    }
    this.m_iLength += length - num;
  }

  public override void ParseStructure(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    stream.Read(this.m_arrRgbUid, 0, 16 /*0x10*/);
    int num = this.LoadPrimaryUID(stream) + 16 /*0x10*/;
    this.m_btTag = (byte) stream.ReadByte();
    int iOffset = num + 1;
    this.m_iPictureDataOffset = iOffset;
    this.CreateImageStream(stream, iOffset);
    this.m_picture = ApplicationImpl.CreateImage((Stream) this.m_stream);
  }

  private int LoadPrimaryUID(Stream stream)
  {
    int num = 0;
    if (this.HasTwoUIDs())
    {
      this.m_arrRgbUidPrimary = new byte[16 /*0x10*/];
      stream.Read(this.m_arrRgbUidPrimary, 0, 16 /*0x10*/);
      num += 16 /*0x10*/;
    }
    return num;
  }

  protected override object InternalClone()
  {
    MsoBitmapPicture msoBitmapPicture = (MsoBitmapPicture) base.InternalClone();
    if (this.m_arrRgbUid != null)
      msoBitmapPicture.m_arrRgbUid = CloneUtils.CloneByteArray(this.m_arrRgbUid);
    if (this.m_stream != null)
      msoBitmapPicture.m_stream = UtilityMethods.CloneStream(this.m_stream);
    if (this.m_picture != null)
      msoBitmapPicture.m_picture = this.m_stream != null ? ApplicationImpl.CreateImage((Stream) msoBitmapPicture.m_stream) : (Image) this.m_picture.Clone();
    return (object) msoBitmapPicture;
  }

  private void CreateImageStream(Stream stream, int iOffset)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (this.m_stream != null)
      this.m_stream.Close();
    int num = this.m_iLength - iOffset;
    bool isDib = this.IsDib;
    int iFullSize = num + (isDib ? 14 : 0);
    this.m_stream = new MemoryStream(num + 14);
    int val2 = num;
    if (isDib)
    {
      uint uiSize = MsoBase.ReadUInt32(stream);
      stream.Position -= 4L;
      MsoBitmapPicture.AddBitMapHeaderToStream(this.m_stream, iFullSize, uiSize, this.GetDibColorsCount(stream, iOffset));
    }
    int count;
    for (byte[] buffer = new byte[10240]; (count = stream.Read(buffer, 0, Math.Min(10240, val2))) > 0 && val2 > 0; val2 -= count)
      this.m_stream.Write(buffer, 0, count);
    this.m_stream.Position = 0L;
  }

  private bool HasTwoUIDs()
  {
    int instance = this.Instance;
    switch (instance)
    {
      case 1761:
      case 1961:
        return true;
      default:
        return instance == 1347;
    }
  }

  private uint GetDibColorsCount(Stream stream, int iOffset)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    stream.Position += 32L /*0x20*/;
    stream.Read(buffer, 0, 4);
    stream.Position -= 36L;
    return BitConverter.ToUInt32(buffer, 0);
  }

  private void EvaluateHash()
  {
    MemoryStream inputStream = new MemoryStream();
    if (ApplicationImpl.EnablePartialTrustCodeStatic)
      new Bitmap(this.m_picture).Save((Stream) inputStream, MsofbtBSE.ConvertToImageFormat((this.Parent as MsofbtBSE).BlipType));
    else
      this.m_picture.Save((Stream) inputStream, MsofbtBSE.ConvertToImageFormat((this.Parent as MsofbtBSE).BlipType));
    inputStream.Position = 0L;
    try
    {
      Buffer.BlockCopy((Array) new SHA1Managed().ComputeHash((Stream) inputStream), 0, (Array) this.m_arrRgbUid, 0, this.m_arrRgbUid.Length);
    }
    catch (InvalidOperationException ex)
    {
      new MACTripleDES().ComputeHash((Stream) inputStream).CopyTo((Array) this.m_arrRgbUid, 0);
    }
  }

  public static void AddBitMapHeaderToStream(
    MemoryStream ms,
    int iFullSize,
    uint uiSize,
    uint dibColorCount)
  {
    byte[] bytes1 = BitConverter.GetBytes(iFullSize);
    ms.Write(MsoBitmapPicture.DEF_DIB_ID, 0, MsoBitmapPicture.DEF_DIB_ID.Length);
    ms.Write(bytes1, 0, bytes1.Length);
    ms.Write(MsoBitmapPicture.DEF_RESERVED, 0, MsoBitmapPicture.DEF_RESERVED.Length);
    byte[] bytes2 = BitConverter.GetBytes((uint) ((int) uiSize + 14 + (int) dibColorCount * 4));
    ms.Write(bytes2, 0, bytes2.Length);
  }

  protected override void OnDispose()
  {
    if (this.m_stream == null)
      return;
    this.m_stream.Close();
    this.m_stream.Dispose();
    this.m_stream = (MemoryStream) null;
  }
}
