// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.ReferenceRegisterRecord
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal class ReferenceRegisterRecord : ReferenceRecord
{
  private ushort m_Id = 13;
  private string m_Libid;
  private string m_name;
  private Encoding m_type;

  internal ushort Id => this.m_Id;

  internal string Libid
  {
    get => this.m_Libid;
    set => this.m_Libid = value;
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
    int count = (int) ZipArchive.ReadUInt32(dirData);
    byte[] numArray = new byte[count];
    dirData.Read(numArray, 0, count);
    this.Libid = encodingType.GetString(numArray, 0, numArray.Length);
    dirData.Position += 6L;
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
    dirData.Write(BitConverter.GetBytes(this.m_Id), 0, 2);
    byte[] bytes = this.EncodingType.GetBytes(this.Libid);
    dirData.Write(BitConverter.GetBytes(bytes.Length + 10), 0, 4);
    dirData.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
    dirData.Write(bytes, 0, bytes.Length);
    dirData.Write(BitConverter.GetBytes(0L), 0, 6);
  }
}
