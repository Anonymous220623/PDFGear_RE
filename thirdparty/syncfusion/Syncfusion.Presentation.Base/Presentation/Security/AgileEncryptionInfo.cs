// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Security.AgileEncryptionInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Security;

[CLSCompliant(false)]
internal class AgileEncryptionInfo
{
  private int m_iVersionInfo;
  private int m_iReserved;
  private XmlEncryptionDescriptor m_xmlEncryptionDescriptor = new XmlEncryptionDescriptor();
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal int VersionInfo
  {
    get => this.m_iVersionInfo;
    set => this.m_iVersionInfo = value;
  }

  internal int Reserved
  {
    get => this.m_iReserved;
    set => this.m_iReserved = value;
  }

  internal XmlEncryptionDescriptor XmlEncryptionDescriptor => this.m_xmlEncryptionDescriptor;

  internal AgileEncryptionInfo()
  {
  }

  internal AgileEncryptionInfo(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iVersionInfo = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iReserved = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_xmlEncryptionDescriptor.Parse(stream);
  }

  internal void Serialize(Stream stream)
  {
    this.m_securityHelper.WriteInt32(stream, this.m_iVersionInfo);
    this.m_securityHelper.WriteInt32(stream, this.m_iReserved);
    this.m_xmlEncryptionDescriptor.Serialize(stream);
  }
}
