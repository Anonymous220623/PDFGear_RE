// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OleStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class OleStream : DataStructure
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
  private OleStream.MonokerStream m_reservedMonikerStream;
  private int m_relativeSourceMonikerStreamSize;
  private OleStream.MonokerStream m_relativeSourceMonikerStream;
  private int m_absoluteSourceMonikerStreamSize;
  private OleStream.MonokerStream m_absoluteSourceMonikerStream;
  private int m_clsidIndicator;
  private OleStream.CLSID m_clsid;
  private int m_reservedDisplayName;
  private int m_reserved2;
  private int m_localUpdateTime;
  private int m_localCheckUpdateTime;
  private int m_remoteUpdateTime;
  private OleLinkType m_linkType;
  private string m_filePath = string.Empty;

  internal override int Length
  {
    get
    {
      if (this.m_streamLeng == 0)
        this.m_streamLeng = 20;
      return this.m_streamLeng;
    }
  }

  internal OleStream(CompoundStream compStream)
  {
    byte[] numArray = new byte[compStream.Length];
    compStream.Read(numArray, 0, numArray.Length);
    this.Parse(numArray, 0);
  }

  internal OleStream(string filePath)
  {
    this.m_oleVersion = 33554433 /*0x02000001*/;
    this.m_reserved1 = 0;
    this.m_reservedMonikerStreamSize = 0;
    this.m_filePath = filePath;
    this.m_linkUpdateOption = 3;
  }

  internal override void Parse(byte[] arrData, int iOffset)
  {
    this.m_streamLeng = arrData.Length;
    this.m_oleVersion = DataStructure.ReadInt32(arrData, ref iOffset);
    if (this.m_oleVersion != 33554433 /*0x02000001*/)
      throw new Exception("OLE stream in not valid");
    this.m_flags = DataStructure.ReadInt32(arrData, ref iOffset);
    this.m_linkUpdateOption = DataStructure.ReadInt32(arrData, ref iOffset);
    this.m_reserved1 = DataStructure.ReadInt32(arrData, ref iOffset);
    if (this.m_reserved1 != 0)
      throw new InvalidDataException("OLE stream in not valid");
    if (this.m_flags == 0 || this.m_flags == 8)
      return;
    this.m_reservedMonikerStreamSize = DataStructure.ReadInt32(arrData, ref iOffset);
    if (this.m_reservedMonikerStreamSize != 0)
    {
      byte[] arrData1 = DataStructure.ReadBytes(arrData, this.m_reservedMonikerStreamSize, ref iOffset);
      this.m_reservedMonikerStream = new OleStream.MonokerStream(this.m_filePath);
      this.m_reservedMonikerStream.Parse(arrData1, 0);
    }
    this.m_relativeSourceMonikerStreamSize = DataStructure.ReadInt32(arrData, ref iOffset);
    if (this.m_relativeSourceMonikerStreamSize != 0)
    {
      byte[] arrData2 = DataStructure.ReadBytes(arrData, this.m_relativeSourceMonikerStreamSize, ref iOffset);
      this.m_relativeSourceMonikerStream = new OleStream.MonokerStream(this.m_filePath);
      this.m_relativeSourceMonikerStream.Parse(arrData2, 0);
    }
    this.m_absoluteSourceMonikerStreamSize = DataStructure.ReadInt32(arrData, ref iOffset);
    byte[] arrData3 = DataStructure.ReadBytes(arrData, this.m_absoluteSourceMonikerStreamSize, ref iOffset);
    this.m_absoluteSourceMonikerStream = new OleStream.MonokerStream(this.m_filePath);
    this.m_absoluteSourceMonikerStream.Parse(arrData3, 0);
    this.m_clsidIndicator = DataStructure.ReadInt32(arrData, ref iOffset);
    if (this.m_clsidIndicator == -1)
      throw new InvalidDataException("OLE stream in not valid");
    byte[] arrData4 = DataStructure.ReadBytes(arrData, 16 /*0x10*/, ref iOffset);
    this.m_clsid = new OleStream.CLSID();
    this.m_clsid.Parse(arrData4, 0);
    this.m_reservedDisplayName = DataStructure.ReadInt32(arrData, ref iOffset);
    this.m_reserved2 = DataStructure.ReadInt32(arrData, ref iOffset);
    this.m_localUpdateTime = DataStructure.ReadInt32(arrData, ref iOffset);
    this.m_localCheckUpdateTime = DataStructure.ReadInt32(arrData, ref iOffset);
    this.m_remoteUpdateTime = DataStructure.ReadInt32(arrData, ref iOffset);
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_oleVersion);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_flags);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_linkUpdateOption);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_reserved1);
    if (this.m_flags == 0)
      return arrData.Length;
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_reservedMonikerStreamSize);
    if (this.m_reservedMonikerStreamSize != 0)
    {
      this.m_reservedMonikerStream.Save(arrData, iOffset);
      iOffset += this.m_reservedMonikerStream.Length;
    }
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_relativeSourceMonikerStreamSize);
    if (this.m_relativeSourceMonikerStreamSize != 0)
    {
      this.m_relativeSourceMonikerStream.Save(arrData, iOffset);
      iOffset += this.m_relativeSourceMonikerStream.Length;
    }
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_absoluteSourceMonikerStreamSize);
    this.m_absoluteSourceMonikerStream.Save(arrData, iOffset);
    iOffset += this.m_absoluteSourceMonikerStream.Length;
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_clsidIndicator);
    this.m_clsid.Save(arrData, iOffset);
    iOffset += this.m_clsid.Length;
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_reservedDisplayName);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_reserved2);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_localUpdateTime);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_localCheckUpdateTime);
    DataStructure.WriteInt32(arrData, ref iOffset, this.m_remoteUpdateTime);
    return arrData.Length;
  }

  internal void SaveTo(CompoundStream stream)
  {
    int iOffset = 0;
    byte[] numArray = new byte[20];
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_oleVersion);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_flags);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_linkUpdateOption);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_reserved1);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_reservedMonikerStreamSize);
    stream.Write(numArray, 0, numArray.Length);
  }

  internal void SaveTo(StgStream stgStream)
  {
    int iOffset = 0;
    byte[] numArray = new byte[20];
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_oleVersion);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_flags);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_linkUpdateOption);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_reserved1);
    DataStructure.WriteInt32(numArray, ref iOffset, this.m_reservedMonikerStreamSize);
    stgStream.Write(numArray, 0, numArray.Length);
  }

  private void WriteZeroByteArr(StgStream stgStream, int byteLength)
  {
    byte[] buffer = new byte[byteLength];
    stgStream.Write(buffer, 0, byteLength);
  }

  private void WriteLengthPrefixedString(StgStream stgStream, string data)
  {
    byte[] numArray = new byte[4];
    ASCIIEncoding asciiEncoding = new ASCIIEncoding();
    int iOffset = 0;
    byte[] bytes = asciiEncoding.GetBytes(data);
    DataStructure.WriteInt32(numArray, ref iOffset, bytes.Length);
    stgStream.Write(numArray, 0, numArray.Length);
    if (bytes.Length <= 0)
      return;
    stgStream.Write(bytes, 0, bytes.Length);
  }

  private class MonokerStream : DataStructure
  {
    private const int DEF_STRUCT_LEN = 0;
    private const int DEF_UNICODE_MARKER = -559022081;
    private const int DEF_UNICODE_MARKER_SIZE = 4;
    internal OleStream.CLSID m_Clsid;
    internal byte[] m_streamData;
    internal string m_stringData;

    internal override int Length
    {
      get => this.m_streamData != null ? this.m_streamData.Length + 16 /*0x10*/ : 0;
    }

    internal MonokerStream(string data) => this.m_stringData = data;

    internal override void Parse(byte[] arrData, int iOffset)
    {
      this.m_Clsid = new OleStream.CLSID();
      this.m_Clsid.Parse(arrData, iOffset);
      iOffset += this.m_Clsid.Length;
      int length = arrData.Length - this.m_Clsid.Length;
      this.m_streamData = DataStructure.ReadBytes(arrData, length, ref iOffset);
    }

    internal override int Save(byte[] arrData, int iOffset)
    {
      this.m_Clsid.Save(arrData, iOffset);
      iOffset += this.m_Clsid.Length;
      DataStructure.WriteBytes(arrData, ref iOffset, this.m_streamData);
      return 0;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetShortPathName(
      [MarshalAs(UnmanagedType.LPTStr)] string path,
      [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
      int shortPathLength);

    internal void SaveTo(StgStream stgStream, bool isAbsolute)
    {
      this.m_Clsid = new OleStream.CLSID();
      byte[] numArray = new byte[16 /*0x10*/];
      this.m_Clsid.Save(numArray, 0);
      ASCIIEncoding asciiEncoding = new ASCIIEncoding();
      UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string s1;
      string s2;
      if (isAbsolute)
      {
        StringBuilder shortPath = new StringBuilder((int) byte.MaxValue);
        OleStream.MonokerStream.GetShortPathName(this.m_stringData, shortPath, shortPath.Capacity);
        string str = shortPath.ToString();
        s1 = str;
        s2 = str;
      }
      else
      {
        s1 = this.m_stringData;
        s2 = this.m_stringData;
      }
      byte[] bytes1 = asciiEncoding.GetBytes(s1);
      byte[] bytes2 = BitConverter.GetBytes(-559022081);
      byte[] bytes3 = unicodeEncoding.GetBytes(s2);
      int num = numArray.Length + bytes1.Length + bytes2.Length + bytes3.Length;
      int count = 4;
      stgStream.Write(BitConverter.GetBytes(num), 0, count);
      stgStream.Write(numArray, 0, numArray.Length);
      stgStream.Write(bytes1, 0, bytes1.Length);
      stgStream.Write(bytes2, 0, bytes2.Length);
      stgStream.Write(bytes3, 0, bytes3.Length);
    }
  }

  private class CLSID : DataStructure
  {
    internal const int DEF_STRUCT_LEN = 16 /*0x10*/;
    internal int m_data1;
    internal short m_data2;
    internal short m_data3;
    internal long m_data4;

    internal override int Length => 16 /*0x10*/;

    internal CLSID()
    {
    }

    internal override void Parse(byte[] arrData, int iOffset)
    {
      this.m_data1 = DataStructure.ReadInt32(arrData, ref iOffset);
      this.m_data2 = DataStructure.ReadInt16(arrData, ref iOffset);
      this.m_data3 = DataStructure.ReadInt16(arrData, ref iOffset);
      this.m_data4 = DataStructure.ReadInt64(arrData, ref iOffset);
    }

    internal override int Save(byte[] arrData, int iOffset)
    {
      DataStructure.WriteInt32(arrData, ref iOffset, this.m_data1);
      DataStructure.WriteInt16(arrData, ref iOffset, this.m_data2);
      DataStructure.WriteInt16(arrData, ref iOffset, this.m_data3);
      DataStructure.WriteInt64(arrData, ref iOffset, this.m_data4);
      return 16 /*0x10*/;
    }
  }
}
