// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsofbtBSE
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtBSE)]
public class MsofbtBSE : MsoBase
{
  private const int DEF_NAME_OFFSET = 36;
  private static readonly MsoBlipType[] DEF_BITMAP_BLIPS = new MsoBlipType[3]
  {
    MsoBlipType.msoblipDIB,
    MsoBlipType.msoblipPNG,
    MsoBlipType.msoblipJPEG
  };
  private static readonly MsoBlipType[] DEF_PICT_BLIPS = new MsoBlipType[3]
  {
    MsoBlipType.msoblipEMF,
    MsoBlipType.msoblipPICT,
    MsoBlipType.msoblipWMF
  };
  [BiffRecordPos(0, 1)]
  private byte m_btReqWin32;
  [BiffRecordPos(1, 1)]
  private byte m_btReqMac;
  [BiffRecordPos(20, 4)]
  private uint m_uiSize;
  [BiffRecordPos(24, 4)]
  private uint m_uiRefCount;
  [BiffRecordPos(28, 4)]
  private uint m_uiFileOffset;
  [BiffRecordPos(32 /*0x20*/, 1)]
  private byte m_btUsage;
  [BiffRecordPos(33, 1)]
  private byte m_btNameLength;
  [BiffRecordPos(34, 1)]
  private byte m_btUnused1;
  [BiffRecordPos(35, 1)]
  private byte m_btUnused2;
  private string m_strBlipName = string.Empty;
  private MsoBase m_msoPicture;
  private int m_iIndex;
  private string m_strPicturePath;

  public MsofbtBSE(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtBSE(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  protected override void OnDispose() => base.OnDispose();

  public new void Dispose()
  {
    if (this.m_msoPicture == null)
      return;
    this.m_msoPicture.Dispose();
  }

  public byte RequiredWin32
  {
    get => this.m_btReqWin32;
    set => this.m_btReqWin32 = value;
  }

  public byte RequiredMac
  {
    get => this.m_btReqMac;
    set => this.m_btReqMac = value;
  }

  public string BlipName
  {
    get => this.m_strBlipName;
    set
    {
      this.m_strBlipName = value;
      this.m_btNameLength = value == null ? (byte) 0 : (byte) this.m_strBlipName.Length;
    }
  }

  public uint SizeInStream
  {
    get => this.m_uiSize;
    set => this.m_uiSize = value;
  }

  public uint RefCount
  {
    get => this.m_uiRefCount;
    set => this.m_uiRefCount = value;
  }

  public uint FileOffset
  {
    get => this.m_uiFileOffset;
    set => this.m_uiFileOffset = value;
  }

  public MsoBlipUsage BlipUsage
  {
    get => (MsoBlipUsage) this.m_btUsage;
    set => this.m_btUsage = (byte) value;
  }

  public byte NameLength => this.m_btNameLength;

  public byte Unused1 => this.m_btUnused1;

  public byte Unused2 => this.m_btUnused2;

  public MsoBlipType BlipType
  {
    get => (MsoBlipType) this.Instance;
    set => this.Instance = (int) value;
  }

  public IPictureRecord PictureRecord
  {
    get => this.m_msoPicture as IPictureRecord;
    set => this.m_msoPicture = value as MsoBase;
  }

  public override bool NeedDataArray => true;

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  public string PicturePath
  {
    get => this.m_strPicturePath;
    set => this.m_strPicturePath = value;
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_uiSize = 0U;
    this.m_btNameLength = (byte) 0;
    this.m_iLength = 36;
    stream.WriteByte(this.m_btReqWin32);
    stream.WriteByte(this.m_btReqMac);
    stream.Position += 18L;
    long position1 = stream.Position;
    MsoBase.WriteUInt32(stream, this.m_uiSize);
    MsoBase.WriteUInt32(stream, this.m_uiRefCount);
    MsoBase.WriteUInt32(stream, this.m_uiFileOffset);
    stream.WriteByte(this.m_btUsage);
    stream.WriteByte(this.m_btNameLength);
    stream.WriteByte(this.m_btUnused1);
    stream.WriteByte(this.m_btUnused2);
    if (this.m_btNameLength > (byte) 0)
    {
      byte[] bytes = Encoding.Unicode.GetBytes(this.m_strBlipName);
      int length = bytes.Length;
      stream.Write(bytes, 0, length);
      this.m_iLength += length;
    }
    if (this.m_msoPicture == null)
      return;
    long position2 = stream.Position;
    this.m_msoPicture.FillArray(stream);
    this.m_uiSize = (uint) (stream.Position - position2);
    this.m_iLength += (int) this.m_uiSize;
    long position3 = stream.Position;
    stream.Position = position1;
    MsoBase.WriteUInt32(stream, this.m_uiSize);
    stream.Position = position3;
  }

  public override void ParseStructure(Stream stream)
  {
    this.m_btReqWin32 = (byte) stream.ReadByte();
    this.m_btReqMac = (byte) stream.ReadByte();
    stream.Position += 18L;
    this.m_uiSize = MsoBase.ReadUInt32(stream);
    this.m_uiRefCount = MsoBase.ReadUInt32(stream);
    this.m_uiFileOffset = MsoBase.ReadUInt32(stream);
    this.m_btUsage = (byte) stream.ReadByte();
    this.m_btNameLength = (byte) stream.ReadByte();
    this.m_btUnused1 = (byte) stream.ReadByte();
    this.m_btUnused2 = (byte) stream.ReadByte();
    int num = 36;
    if (this.m_btNameLength > (byte) 0)
    {
      byte[] numArray = new byte[(int) this.m_btNameLength];
      stream.Read(numArray, 0, (int) this.m_btNameLength);
      this.m_strBlipName = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
      num += (int) this.m_btNameLength;
    }
    if (num == this.m_iLength)
      this.m_msoPicture = (MsoBase) null;
    else if (Array.IndexOf<MsoBlipType>(MsofbtBSE.DEF_BITMAP_BLIPS, this.BlipType) != -1)
    {
      this.m_msoPicture = (MsoBase) new MsoBitmapPicture((MsoBase) this, stream);
    }
    else
    {
      if (Array.IndexOf<MsoBlipType>(MsofbtBSE.DEF_PICT_BLIPS, this.BlipType) == -1)
        return;
      this.m_msoPicture = (MsoBase) new MsoMetafilePicture((MsoBase) this, stream);
    }
  }

  protected override object InternalClone()
  {
    MsofbtBSE parent = (MsofbtBSE) base.InternalClone();
    if (this.m_msoPicture != null)
      parent.m_msoPicture = this.m_msoPicture.Clone((MsoBase) parent);
    return (object) parent;
  }

  public static ImageFormat ConvertToImageFormat(MsoBlipType blipType)
  {
    switch (blipType)
    {
      case MsoBlipType.msoblipEMF:
        return ImageFormat.Emf;
      case MsoBlipType.msoblipWMF:
        return ImageFormat.Wmf;
      case MsoBlipType.msoblipJPEG:
        return ImageFormat.Jpeg;
      case MsoBlipType.msoblipDIB:
        return ImageFormat.Bmp;
      default:
        return ImageFormat.Png;
    }
  }
}
