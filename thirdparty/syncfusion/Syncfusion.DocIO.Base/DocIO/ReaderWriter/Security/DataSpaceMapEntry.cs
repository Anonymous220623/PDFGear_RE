// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.DataSpaceMapEntry
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class DataSpaceMapEntry
{
  private List<DataSpaceReferenceComponent> m_lstComponents = new List<DataSpaceReferenceComponent>();
  private string m_strDataSpaceName;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal List<DataSpaceReferenceComponent> Components => this.m_lstComponents;

  internal string DataSpaceName
  {
    get => this.m_strDataSpaceName;
    set => this.m_strDataSpaceName = value;
  }

  internal DataSpaceMapEntry()
  {
  }

  internal DataSpaceMapEntry(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_securityHelper.ReadInt32(stream, buffer);
    int num = this.m_securityHelper.ReadInt32(stream, buffer);
    for (int index = 0; index < num; ++index)
      this.m_lstComponents.Add(new DataSpaceReferenceComponent(stream));
    this.m_strDataSpaceName = this.m_securityHelper.ReadUnicodeString(stream);
  }

  internal void Serialize(Stream stream)
  {
    long num = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    stream.Position += 4L;
    int count = this.m_lstComponents.Count;
    this.m_securityHelper.WriteInt32(stream, count);
    for (int index = 0; index < count; ++index)
      this.m_lstComponents[index].Serialize(stream);
    this.m_securityHelper.WriteUnicodeString(stream, this.m_strDataSpaceName);
    long position = stream.Position;
    stream.Position = num;
    this.m_securityHelper.WriteInt32(stream, (int) (position - num));
    stream.Position = position;
  }
}
