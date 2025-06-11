// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtImage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtImage : _Blip
{
  private const int DEF_COLOR_USED_OFFSET = 32 /*0x20*/;
  private const int DEF_DIB_HEADER_SIZE = 14;
  private const uint DEF_COLOR_SIZE = 4;
  private static readonly byte[] DEF_SIGNATURE = new byte[2]
  {
    (byte) 66,
    (byte) 77
  };
  private static readonly byte[] DEF_RESERVED = new byte[4];
  private ImageRecord m_imageRecord;

  internal override byte[] ImageBytes
  {
    get => this.m_imageRecord == null ? (byte[]) null : this.m_imageRecord.ImageBytes;
    set
    {
      this.m_imageRecord = this.m_doc.Images.LoadImage(value);
      if (this.m_imageRecord == null)
        return;
      --this.m_imageRecord.OccurenceCount;
    }
  }

  internal override ImageRecord ImageRecord
  {
    get => this.m_imageRecord;
    set => this.m_imageRecord = value;
  }

  internal MsofbtImage(WordDocument doc)
    : base(doc)
  {
  }

  internal MsofbtImage(ImageRecord imageRecord, bool isBitmap, WordDocument doc)
    : base(doc)
  {
    if (isBitmap)
    {
      this.Header.Type = MSOFBT.msofbtBlipPNG;
      this.Header.Instance = 1760;
    }
    else
    {
      this.Header.Type = MSOFBT.msofbtBlipJPEG;
      this.Header.Instance = 1130;
    }
    this.Uid = Guid.NewGuid();
    this.Uid2 = this.Uid;
    this.m_imageRecord = imageRecord;
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.ReadGuid(stream);
    stream.ReadByte();
    int count = this.Header.Length - 16 /*0x10*/ - 1;
    byte[] numArray = new byte[count];
    stream.Read(numArray, 0, count);
    if (this.IsDib)
      numArray = this.ConvertDibToBmp(numArray);
    this.m_imageRecord = this.m_doc.Images.LoadImage(numArray);
  }

  protected override void WriteRecordData(Stream stream)
  {
    byte[] byteArray1 = this.Uid.ToByteArray();
    stream.Write(byteArray1, 0, byteArray1.Length);
    if (this.HasUid2())
    {
      byte[] byteArray2 = this.Uid2.ToByteArray();
      stream.Write(byteArray2, 0, byteArray2.Length);
    }
    stream.WriteByte(byte.MaxValue);
    stream.Write(this.ImageBytes, 0, this.ImageBytes.Length);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtImage msofbtImage = new MsofbtImage(this.m_doc);
    if (this.m_imageRecord != null)
      msofbtImage.m_imageRecord = new ImageRecord(this.m_doc, this.m_imageRecord);
    msofbtImage.Header = this.Header.Clone();
    msofbtImage.Uid = new Guid(this.Uid.ToByteArray());
    msofbtImage.Uid2 = new Guid(this.Uid2.ToByteArray());
    msofbtImage.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtImage;
  }

  internal override void Close() => base.Close();

  private byte[] ConvertDibToBmp(byte[] imageBytes)
  {
    uint uint32_1 = BitConverter.ToUInt32(imageBytes, 0);
    uint uint32_2 = BitConverter.ToUInt32(imageBytes, 32 /*0x20*/);
    int num = imageBytes.Length + 14;
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = BitConverter.GetBytes(num);
    memoryStream.Write(MsofbtImage.DEF_SIGNATURE, 0, MsofbtImage.DEF_SIGNATURE.Length);
    memoryStream.Write(bytes1, 0, bytes1.Length);
    memoryStream.Write(MsofbtImage.DEF_RESERVED, 0, MsofbtImage.DEF_RESERVED.Length);
    byte[] bytes2 = BitConverter.GetBytes((uint) ((int) uint32_1 + 14 + (int) uint32_2 * 4));
    memoryStream.Write(bytes2, 0, bytes2.Length);
    memoryStream.Write(imageBytes, 0, imageBytes.Length);
    imageBytes = memoryStream.ToArray();
    memoryStream.Close();
    return imageBytes;
  }
}
