// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.ReferenceProjectRecord
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal class ReferenceProjectRecord : ReferenceRecord
{
  private ushort m_Id = 14;
  private string m_LibAbsolute = string.Empty;
  private string m_LibRelative = string.Empty;
  private uint m_MajorVersion;
  private ushort m_MinorVersion = 1;
  private Encoding m_type;
  private string m_name;

  internal ushort Id => this.m_Id;

  internal string LibAbsolute
  {
    get => this.m_LibAbsolute;
    set => this.m_LibAbsolute = value;
  }

  internal string LibRelative
  {
    get => this.m_LibRelative;
    set => this.m_LibRelative = value;
  }

  internal uint MajorVersion
  {
    get => this.m_MajorVersion;
    set => this.m_MajorVersion = value;
  }

  internal ushort MinorVersion
  {
    get => this.m_MinorVersion;
    set => this.m_MinorVersion = value;
  }

  internal override string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal override Encoding EncodingType
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal override void ParseRecord(Stream dirData)
  {
    dirData.Position += 4L;
    Encoding encodingType = this.EncodingType;
    int count1 = (int) ZipArchive.ReadUInt32(dirData);
    byte[] numArray1 = new byte[count1];
    dirData.Read(numArray1, 0, count1);
    this.LibAbsolute = encodingType.GetString(numArray1, 0, numArray1.Length);
    int count2 = (int) ZipArchive.ReadUInt32(dirData);
    byte[] numArray2 = new byte[count2];
    dirData.Read(numArray2, 0, count2);
    this.LibRelative = encodingType.GetString(numArray2, 0, numArray2.Length);
    this.MajorVersion = ZipArchive.ReadUInt32(dirData);
    this.MinorVersion = ZipArchive.ReadUInt16(dirData);
  }

  internal override void SerializeRecord(Stream dirData)
  {
    if (!string.IsNullOrEmpty(this.Name))
    {
      byte[] bytes1 = this.EncodingType.GetBytes(this.Name);
      dirData.Write(BitConverter.GetBytes(22), 0, 2);
      dirData.Write(BitConverter.GetBytes(bytes1.Length), 0, 4);
      dirData.Write(bytes1, 0, bytes1.Length);
      byte[] bytes2 = Encoding.Unicode.GetBytes(this.Name);
      dirData.Write(BitConverter.GetBytes(62), 0, 2);
      dirData.Write(BitConverter.GetBytes(bytes2.Length), 0, 4);
      dirData.Write(bytes2, 0, bytes2.Length);
    }
    dirData.Write(BitConverter.GetBytes(14), 0, 2);
    byte[] bytes3 = this.EncodingType.GetBytes(this.LibAbsolute);
    byte[] bytes4 = this.EncodingType.GetBytes(this.LibRelative);
    dirData.Write(BitConverter.GetBytes(bytes3.Length + bytes4.Length + 16 /*0x10*/), 0, 4);
    dirData.Write(BitConverter.GetBytes(bytes3.Length), 0, 4);
    dirData.Write(bytes3, 0, bytes3.Length);
    dirData.Write(BitConverter.GetBytes(bytes4.Length), 0, 4);
    dirData.Write(bytes4, 0, bytes4.Length);
    dirData.Write(BitConverter.GetBytes(this.MajorVersion), 0, 4);
    dirData.Write(BitConverter.GetBytes(this.MinorVersion), 0, 2);
  }
}
