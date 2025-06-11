// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Security.DataSpaceMap
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Security;

[CLSCompliant(false)]
internal class DataSpaceMap
{
  private const int DefaultHeaderSize = 8;
  private int m_iHeaderSize = 8;
  private List<DataSpaceMapEntry> m_lstMapEntries = new List<DataSpaceMapEntry>();
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal List<DataSpaceMapEntry> MapEntries => this.m_lstMapEntries;

  internal DataSpaceMap()
  {
  }

  internal DataSpaceMap(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    this.m_iHeaderSize = this.m_securityHelper.ReadInt32(stream, buffer);
    int num = this.m_securityHelper.ReadInt32(stream, buffer);
    if (this.m_lstMapEntries.Capacity < num)
      this.m_lstMapEntries.Capacity = num;
    if (this.m_iHeaderSize != 8)
      stream.Position += (long) (this.m_iHeaderSize - 8);
    for (int index = 0; index < num; ++index)
      this.m_lstMapEntries.Add(new DataSpaceMapEntry(stream));
  }

  internal void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_securityHelper.WriteInt32(stream, this.m_iHeaderSize);
    int count = this.m_lstMapEntries.Count;
    this.m_securityHelper.WriteInt32(stream, count);
    for (int index = 0; index < count; ++index)
      this.m_lstMapEntries[index].Serialize(stream);
  }
}
