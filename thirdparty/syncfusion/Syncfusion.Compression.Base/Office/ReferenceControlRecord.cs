// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.ReferenceControlRecord
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal class ReferenceControlRecord : ReferenceRecord
{
  private ushort m_Id = 47;
  private string m_libTwiddled;
  private string m_extLibid;
  private Guid m_originalType;
  private uint m_cookie;
  private string m_name;
  private Encoding m_type;

  internal ushort Id => this.m_Id;

  internal string LibTwiddled
  {
    get => this.m_libTwiddled;
    set => this.m_libTwiddled = value;
  }

  internal string ExtLibId
  {
    get => this.m_extLibid;
    set => this.m_extLibid = value;
  }

  internal Guid OriginalType
  {
    get => this.m_originalType;
    set => this.m_originalType = value;
  }

  internal uint Cookie
  {
    get => this.m_cookie;
    set => this.m_cookie = value;
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
    int count1 = (int) ZipArchive.ReadUInt32(dirData);
    byte[] numArray1 = new byte[count1];
    dirData.Read(numArray1, 0, count1);
    this.LibTwiddled = this.EncodingType.GetString(numArray1, 0, numArray1.Length);
    dirData.Position += 6L;
    if (ZipArchive.ReadUInt16(dirData) == (ushort) 22)
    {
      int count2 = (int) ZipArchive.ReadUInt32(dirData);
      byte[] numArray2 = new byte[count2];
      dirData.Read(numArray2, 0, count2);
      this.Name = this.EncodingType.GetString(numArray2, 0, numArray2.Length);
      if (ZipArchive.ReadUInt16(dirData) == (ushort) 62)
      {
        int num = (int) ZipArchive.ReadUInt32(dirData);
        dirData.Position += (long) (num + 2);
      }
    }
    dirData.Position += 4L;
    int count3 = (int) ZipArchive.ReadUInt32(dirData);
    byte[] numArray3 = new byte[count3];
    dirData.Read(numArray3, 0, count3);
    this.m_extLibid = this.EncodingType.GetString(numArray3, 0, numArray3.Length);
    dirData.Position += 6L;
    byte[] numArray4 = new byte[16 /*0x10*/];
    dirData.Read(numArray4, 0, 16 /*0x10*/);
    this.OriginalType = new Guid(numArray4);
    this.Cookie = ZipArchive.ReadUInt32(dirData);
  }

  internal override void SerializeRecord(Stream dirData)
  {
    dirData.Write(BitConverter.GetBytes(this.m_Id), 0, 2);
    byte[] bytes1 = this.EncodingType.GetBytes(this.LibTwiddled);
    dirData.Write(BitConverter.GetBytes(bytes1.Length + 10), 0, 4);
    dirData.Write(BitConverter.GetBytes(bytes1.Length), 0, 4);
    dirData.Write(bytes1, 0, bytes1.Length);
    dirData.Write(BitConverter.GetBytes(0), 0, 4);
    dirData.Write(BitConverter.GetBytes(0), 0, 2);
    dirData.Write(BitConverter.GetBytes(48 /*0x30*/), 0, 2);
    byte[] bytes2 = this.EncodingType.GetBytes(this.ExtLibId);
    dirData.Write(BitConverter.GetBytes(bytes2.Length + 30), 0, 4);
    dirData.Write(BitConverter.GetBytes(bytes2.Length), 0, 4);
    dirData.Write(bytes2, 0, bytes2.Length);
    dirData.Write(BitConverter.GetBytes(0), 0, 4);
    dirData.Write(BitConverter.GetBytes(0), 0, 2);
    dirData.Write(this.OriginalType.ToByteArray(), 0, 16 /*0x10*/);
    dirData.Write(BitConverter.GetBytes(this.Cookie), 0, 4);
  }
}
