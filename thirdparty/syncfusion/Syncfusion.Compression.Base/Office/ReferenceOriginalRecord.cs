// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.ReferenceOriginalRecord
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal class ReferenceOriginalRecord : ReferenceRecord
{
  private ushort m_Id = 51;
  private string m_Libid = string.Empty;
  private Encoding m_type;
  private string m_name;

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
    int count = (int) ZipArchive.ReadUInt32(dirData);
    byte[] numArray = new byte[count];
    dirData.Read(numArray, 0, count);
    this.Libid = this.EncodingType.GetString(numArray, 0, numArray.Length);
  }

  internal override void SerializeRecord(Stream dirData)
  {
    dirData.Write(BitConverter.GetBytes(this.Id), 0, 2);
    byte[] bytes = this.EncodingType.GetBytes(this.Libid);
    dirData.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
    dirData.Write(bytes, 0, bytes.Length);
  }
}
