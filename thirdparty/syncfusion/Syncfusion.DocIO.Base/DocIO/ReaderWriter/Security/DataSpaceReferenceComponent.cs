// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.DataSpaceReferenceComponent
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class DataSpaceReferenceComponent
{
  private int m_iComponentType;
  private string m_strName;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal int ComponentType => this.m_iComponentType;

  internal string Name => this.m_strName;

  internal DataSpaceReferenceComponent(int type, string name)
  {
    this.m_iComponentType = type;
    this.m_strName = name;
  }

  internal DataSpaceReferenceComponent(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iComponentType = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_strName = this.m_securityHelper.ReadUnicodeString(stream);
  }

  internal void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_securityHelper.WriteInt32(stream, this.m_iComponentType);
    this.m_securityHelper.WriteUnicodeString(stream, this.m_strName);
  }
}
