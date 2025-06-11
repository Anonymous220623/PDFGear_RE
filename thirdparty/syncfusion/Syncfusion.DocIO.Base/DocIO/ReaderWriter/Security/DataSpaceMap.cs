// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.DataSpaceMap
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

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
