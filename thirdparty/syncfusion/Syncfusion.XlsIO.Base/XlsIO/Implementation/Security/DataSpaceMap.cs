// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.DataSpaceMap
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class DataSpaceMap
{
  private const int DefaultHeaderSize = 8;
  private int m_iHeaderSize = 8;
  private List<DataSpaceMapEntry> m_lstMapEntries = new List<DataSpaceMapEntry>();

  public List<DataSpaceMapEntry> MapEntries => this.m_lstMapEntries;

  public DataSpaceMap()
  {
  }

  public DataSpaceMap(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    this.m_iHeaderSize = SecurityHelper.ReadInt32(stream, buffer);
    int num = SecurityHelper.ReadInt32(stream, buffer);
    if (this.m_lstMapEntries.Capacity < num)
      this.m_lstMapEntries.Capacity = num;
    if (this.m_iHeaderSize != 8)
      stream.Position += (long) (this.m_iHeaderSize - 8);
    for (int index = 0; index < num; ++index)
      this.m_lstMapEntries.Add(new DataSpaceMapEntry(stream));
  }

  public void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    SecurityHelper.WriteInt32(stream, this.m_iHeaderSize);
    int count = this.m_lstMapEntries.Count;
    SecurityHelper.WriteInt32(stream, count);
    for (int index = 0; index < count; ++index)
      this.m_lstMapEntries[index].Serialize(stream);
  }
}
