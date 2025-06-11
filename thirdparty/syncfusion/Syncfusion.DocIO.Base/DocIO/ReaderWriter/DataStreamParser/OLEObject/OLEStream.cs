// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEStream
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class OLEStream
{
  private const int DEF_VERSION_CONSTANT = 33554433 /*0x02000001*/;
  private const int DEF_RESERVED_VALUE = 0;
  private const int DEF_EMBEDDED_SIZE = 20;
  private const int DEF_CLSID_INDICATOR = -1;
  private const int DEF_EMBED_FLAG = 8;
  private const int DEF_LINK_FLAG = 1;
  private int m_streamLeng;
  private int m_oleVersion;
  private int m_flags;
  private int m_linkUpdateOption;
  private int m_reserved1;
  private int m_reservedMonikerStreamSize;
  private OLEStream.MonokerStream m_reservedMonikerStream;
  private int m_relativeSourceMonikerStreamSize;
  private OLEStream.MonokerStream m_relativeSourceMonikerStream;
  private int m_absoluteSourceMonikerStreamSize;
  private OLEStream.MonokerStream m_absoluteSourceMonikerStream;
  private int m_clsidIndicator;
  private OLEStream.CLSID m_clsid;
  private int m_reservedDisplayName;
  private int m_reserved2;
  private int m_localUpdateTime;
  private int m_localCheckUpdateTime;
  private int m_remoteUpdateTime;
  private OleLinkType m_linkType;
  private string m_filePath = string.Empty;

  internal int Length
  {
    get
    {
      if (this.m_streamLeng == 0)
        this.m_streamLeng = 20;
      return this.m_streamLeng;
    }
  }

  internal OLEStream(Stream stream) => this.Parse((stream as MemoryStream).ToArray(), 0);

  internal OLEStream(OleLinkType type, string filePath)
  {
    this.m_linkType = type;
    this.m_oleVersion = 33554433 /*0x02000001*/;
    this.m_reserved1 = 0;
    this.m_reservedMonikerStreamSize = 0;
    if (type == OleLinkType.Embed)
    {
      this.m_flags = 8;
    }
    else
    {
      this.m_flags = 1;
      this.m_filePath = filePath;
      this.m_linkUpdateOption = 3;
    }
  }

  internal void Parse(byte[] arrData, int iOffset)
  {
    this.m_streamLeng = arrData.Length;
    this.m_oleVersion = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (this.m_oleVersion != 33554433 /*0x02000001*/)
      throw new InvalidDataException("OLE stream is not valid");
    this.m_flags = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.m_linkUpdateOption = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.m_reserved1 = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (this.m_reserved1 != 0)
      throw new InvalidDataException("OLE stream is not valid");
    if (this.m_flags == 0 || this.m_flags == 8)
      return;
    this.m_reservedMonikerStreamSize = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (this.m_reservedMonikerStreamSize != 0)
    {
      byte[] arrData1 = ByteConverter.ReadBytes(arrData, this.m_reservedMonikerStreamSize, ref iOffset);
      this.m_reservedMonikerStream = new OLEStream.MonokerStream(this.m_filePath);
      this.m_reservedMonikerStream.Parse(arrData1, 0);
    }
    this.m_relativeSourceMonikerStreamSize = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (this.m_relativeSourceMonikerStreamSize != 0)
    {
      byte[] arrData2 = ByteConverter.ReadBytes(arrData, this.m_relativeSourceMonikerStreamSize, ref iOffset);
      this.m_relativeSourceMonikerStream = new OLEStream.MonokerStream(this.m_filePath);
      this.m_relativeSourceMonikerStream.Parse(arrData2, 0);
    }
    this.m_absoluteSourceMonikerStreamSize = ByteConverter.ReadInt32(arrData, ref iOffset);
    byte[] arrData3 = ByteConverter.ReadBytes(arrData, this.m_absoluteSourceMonikerStreamSize, ref iOffset);
    this.m_absoluteSourceMonikerStream = new OLEStream.MonokerStream(this.m_filePath);
    this.m_absoluteSourceMonikerStream.Parse(arrData3, 0);
    this.m_clsidIndicator = ByteConverter.ReadInt32(arrData, ref iOffset);
    if (this.m_clsidIndicator == -1)
      throw new InvalidDataException("OLE stream is not valid");
    byte[] arrData4 = ByteConverter.ReadBytes(arrData, 16 /*0x10*/, ref iOffset);
    this.m_clsid = new OLEStream.CLSID();
    this.m_clsid.Parse(arrData4, 0);
    this.m_reservedDisplayName = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.m_reserved2 = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.m_localUpdateTime = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.m_localCheckUpdateTime = ByteConverter.ReadInt32(arrData, ref iOffset);
    this.m_remoteUpdateTime = ByteConverter.ReadInt32(arrData, ref iOffset);
  }

  internal int Save(byte[] arrData, int iOffset)
  {
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_oleVersion);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_flags);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_linkUpdateOption);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_reserved1);
    if (this.m_flags == 0)
      return arrData.Length;
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_reservedMonikerStreamSize);
    if (this.m_reservedMonikerStreamSize != 0)
    {
      this.m_reservedMonikerStream.Save(arrData, iOffset);
      iOffset += this.m_reservedMonikerStream.Length;
    }
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_relativeSourceMonikerStreamSize);
    if (this.m_relativeSourceMonikerStreamSize != 0)
    {
      this.m_relativeSourceMonikerStream.Save(arrData, iOffset);
      iOffset += this.m_relativeSourceMonikerStream.Length;
    }
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_absoluteSourceMonikerStreamSize);
    this.m_absoluteSourceMonikerStream.Save(arrData, iOffset);
    iOffset += this.m_absoluteSourceMonikerStream.Length;
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_clsidIndicator);
    this.m_clsid.Save(arrData, iOffset);
    iOffset += this.m_clsid.Length;
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_reservedDisplayName);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_reserved2);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_localUpdateTime);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_localCheckUpdateTime);
    ByteConverter.WriteInt32(arrData, ref iOffset, this.m_remoteUpdateTime);
    return arrData.Length;
  }

  internal void SaveTo(Stream stream)
  {
    int iOffset = 0;
    byte[] numArray = new byte[20];
    ByteConverter.WriteInt32(numArray, ref iOffset, this.m_oleVersion);
    ByteConverter.WriteInt32(numArray, ref iOffset, this.m_flags);
    ByteConverter.WriteInt32(numArray, ref iOffset, this.m_linkUpdateOption);
    ByteConverter.WriteInt32(numArray, ref iOffset, this.m_reserved1);
    if (this.m_linkType != OleLinkType.Embed)
      return;
    ByteConverter.WriteInt32(numArray, ref iOffset, this.m_reservedMonikerStreamSize);
    stream.Write(numArray, 0, numArray.Length);
  }

  private class MonokerStream
  {
    private const int DEF_STRUCT_LEN = 0;
    private const int DEF_UNICODE_MARKER = -559022081;
    private const int DEF_UNICODE_MARKER_SIZE = 4;
    internal OLEStream.CLSID m_Clsid;
    internal byte[] m_streamData;
    internal string m_stringData;

    internal int Length => this.m_streamData != null ? this.m_streamData.Length + 16 /*0x10*/ : 0;

    internal MonokerStream(string data) => this.m_stringData = data;

    internal void Parse(byte[] arrData, int iOffset)
    {
      this.m_Clsid = new OLEStream.CLSID();
      this.m_Clsid.Parse(arrData, iOffset);
      iOffset += this.m_Clsid.Length;
      int length = arrData.Length - this.m_Clsid.Length;
      this.m_streamData = ByteConverter.ReadBytes(arrData, length, ref iOffset);
    }

    internal int Save(byte[] arrData, int iOffset)
    {
      this.m_Clsid.Save(arrData, iOffset);
      iOffset += this.m_Clsid.Length;
      ByteConverter.WriteBytes(arrData, ref iOffset, this.m_streamData);
      return 0;
    }
  }

  private class CLSID
  {
    internal const int DEF_STRUCT_LEN = 16 /*0x10*/;
    internal int m_data1;
    internal short m_data2;
    internal short m_data3;
    internal long m_data4;

    internal int Length => 16 /*0x10*/;

    internal CLSID()
    {
    }

    internal void Parse(byte[] arrData, int iOffset)
    {
      this.m_data1 = ByteConverter.ReadInt32(arrData, ref iOffset);
      this.m_data2 = ByteConverter.ReadInt16(arrData, ref iOffset);
      this.m_data3 = ByteConverter.ReadInt16(arrData, ref iOffset);
      this.m_data4 = ByteConverter.ReadInt64(arrData, ref iOffset);
    }

    internal int Save(byte[] arrData, int iOffset)
    {
      ByteConverter.WriteInt32(arrData, ref iOffset, this.m_data1);
      ByteConverter.WriteInt16(arrData, ref iOffset, this.m_data2);
      ByteConverter.WriteInt16(arrData, ref iOffset, this.m_data3);
      ByteConverter.WriteInt64(arrData, ref iOffset, this.m_data4);
      return 16 /*0x10*/;
    }
  }
}
