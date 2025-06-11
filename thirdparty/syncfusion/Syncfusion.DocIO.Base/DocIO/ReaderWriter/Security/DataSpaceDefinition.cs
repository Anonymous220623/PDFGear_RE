// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.DataSpaceDefinition
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class DataSpaceDefinition
{
  private const int DefaultHeaderLength = 8;
  private int m_iHeaderLength = 8;
  private List<string> m_lstTransformRefs = new List<string>();
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal List<string> TransformRefs => this.m_lstTransformRefs;

  internal DataSpaceDefinition()
  {
  }

  internal DataSpaceDefinition(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iHeaderLength = this.m_securityHelper.ReadInt32(stream, buffer);
    int num = this.m_securityHelper.ReadInt32(stream, buffer);
    if (this.m_iHeaderLength != 8)
      stream.Position += (long) (this.m_iHeaderLength - 8);
    for (int index = 0; index < num; ++index)
      this.m_lstTransformRefs.Add(this.m_securityHelper.ReadUnicodeString(stream));
  }

  internal void Serialize(Stream stream)
  {
    this.m_securityHelper.WriteInt32(stream, this.m_iHeaderLength);
    int count = this.m_lstTransformRefs.Count;
    this.m_securityHelper.WriteInt32(stream, count);
    for (int index = 0; index < count; ++index)
    {
      string lstTransformRef = this.m_lstTransformRefs[index];
      this.m_securityHelper.WriteUnicodeString(stream, lstTransformRef);
    }
  }
}
