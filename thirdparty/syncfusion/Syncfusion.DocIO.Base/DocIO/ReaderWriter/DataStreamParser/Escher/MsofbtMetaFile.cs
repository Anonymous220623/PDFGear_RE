// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtMetaFile
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtMetaFile : _Blip
{
  internal int m_length;
  internal int m_rectLeft;
  internal int m_rectTop;
  internal int m_rectRight;
  internal int m_rectBottom;
  internal int m_rectWidth;
  internal int m_rectHeight;
  private byte m_fFilter;
  private ImageRecord m_imageRecord;

  internal CompressionMethod Compression => CompressionMethod.msocompressionZip;

  internal override byte[] ImageBytes
  {
    get => this.m_imageRecord == null ? (byte[]) null : this.m_imageRecord.ImageBytes;
    set
    {
      this.m_imageRecord = this.m_doc.Images.LoadMetaFileImage(value, false);
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

  internal MsofbtMetaFile(WordDocument doc)
    : base(doc)
  {
  }

  internal MsofbtMetaFile(ImageRecord imageRecord, WordDocument doc)
    : base(doc)
  {
    if (imageRecord == null)
      return;
    this.Header.Type = MSOFBT.msofbtBlipEMF;
    this.Header.Instance = 980;
    this.Uid = Guid.NewGuid();
    this.Uid2 = this.Uid;
    this.m_imageRecord = imageRecord;
    this.m_length = this.m_imageRecord.Length;
    this.m_fFilter = (byte) 254;
    this.m_rectRight = imageRecord.Size.Width;
    this.m_rectBottom = imageRecord.Size.Height;
    this.m_rectWidth = imageRecord.Size.Width * 12700 * 72 / 96 /*0x60*/;
    this.m_rectHeight = imageRecord.Size.Height * 12700 * 72 / 96 /*0x60*/;
  }

  internal MsofbtMetaFile(WPicture picture, WordDocument doc)
    : base(doc)
  {
    if (picture.ImageRecord == null)
      return;
    if (picture.ImageRecord.ImageFormat.Equals((object) ImageFormat.Emf))
    {
      this.Header.Type = MSOFBT.msofbtBlipEMF;
      this.Header.Instance = 980;
    }
    else if (picture.ImageRecord.ImageFormat.Equals((object) ImageFormat.Wmf))
    {
      this.Header.Type = MSOFBT.msofbtBlipWMF;
      this.Header.Instance = 534;
    }
    this.Uid = Guid.NewGuid();
    this.Uid2 = this.Uid;
    this.m_imageRecord = picture.ImageRecord;
    this.m_length = this.m_imageRecord.Length;
    this.m_fFilter = (byte) 254;
    this.m_rectRight = picture.ImageRecord.Size.Width;
    this.m_rectBottom = picture.ImageRecord.Size.Height;
    this.m_rectWidth = (int) ((double) ((long) picture.ImageRecord.Size.Width * 12700L * 72L) / (double) picture.Image.HorizontalResolution);
    this.m_rectHeight = (int) ((double) ((long) picture.ImageRecord.Size.Height * 12700L * 72L) / (double) picture.Image.VerticalResolution);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtMetaFile msofbtMetaFile = (MsofbtMetaFile) this.MemberwiseClone();
    if (this.m_imageRecord != null)
      msofbtMetaFile.m_imageRecord = new ImageRecord(this.m_doc, this.m_imageRecord);
    msofbtMetaFile.Header = this.Header.Clone();
    msofbtMetaFile.Uid = new Guid(this.Uid.ToByteArray());
    msofbtMetaFile.Uid2 = new Guid(this.Uid2.ToByteArray());
    msofbtMetaFile.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtMetaFile;
  }

  internal override void Close() => base.Close();

  protected override void ReadRecordData(Stream stream)
  {
    int position = (int) stream.Position;
    this.ReadGuid(stream);
    this.m_length = BaseWordRecord.ReadInt32(stream);
    this.m_rectLeft = BaseWordRecord.ReadInt32(stream);
    this.m_rectTop = BaseWordRecord.ReadInt32(stream);
    this.m_rectRight = BaseWordRecord.ReadInt32(stream);
    this.m_rectBottom = BaseWordRecord.ReadInt32(stream);
    this.m_rectWidth = BaseWordRecord.ReadInt32(stream);
    this.m_rectHeight = BaseWordRecord.ReadInt32(stream);
    int count = BaseWordRecord.ReadInt32(stream);
    CompressionMethod compressionMethod = (CompressionMethod) stream.ReadByte();
    this.m_fFilter = (byte) stream.ReadByte();
    byte[] numArray = new byte[count];
    stream.Read(numArray, 0, count);
    this.m_imageRecord = compressionMethod != CompressionMethod.msocompressionZip ? this.m_doc.Images.LoadMetaFileImage(numArray, false) : this.m_doc.Images.LoadMetaFileImage(numArray, true);
    stream.Position = (long) (position + this.Header.Length);
  }

  protected override void WriteRecordData(Stream stream)
  {
    if (this.ImageRecord.IsMetafileHeaderPresent(this.ImageRecord.ImageBytes))
    {
      byte[] numArray = new byte[this.ImageRecord.ImageBytes.Length - 22];
      Buffer.BlockCopy((Array) this.ImageRecord.ImageBytes, 22, (Array) numArray, 0, this.ImageRecord.ImageBytes.Length - 22);
      this.ImageRecord.m_imageBytes = this.m_doc.Images.CompressImageBytes(numArray);
    }
    byte[] byteArray1 = this.Uid.ToByteArray();
    stream.Write(byteArray1, 0, byteArray1.Length);
    if (this.HasUid2())
    {
      byte[] byteArray2 = this.Uid2.ToByteArray();
      stream.Write(byteArray2, 0, byteArray2.Length);
    }
    BaseWordRecord.WriteInt32(stream, this.m_length);
    BaseWordRecord.WriteInt32(stream, this.m_rectLeft);
    BaseWordRecord.WriteInt32(stream, this.m_rectTop);
    BaseWordRecord.WriteInt32(stream, this.m_rectRight);
    BaseWordRecord.WriteInt32(stream, this.m_rectBottom);
    BaseWordRecord.WriteInt32(stream, this.m_rectWidth);
    BaseWordRecord.WriteInt32(stream, this.m_rectHeight);
    BaseWordRecord.WriteInt32(stream, this.ImageRecord.m_imageBytes.Length);
    stream.WriteByte((byte) 0);
    stream.WriteByte(this.m_fFilter);
    stream.Write(this.ImageRecord.m_imageBytes, 0, this.ImageRecord.m_imageBytes.Length);
  }
}
