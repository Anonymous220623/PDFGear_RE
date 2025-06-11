// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.DataSpaceMapEntry
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class DataSpaceMapEntry
{
  private List<DataSpaceReferenceComponent> m_lstComponents = new List<DataSpaceReferenceComponent>();
  private string m_strDataSpaceName;

  public List<DataSpaceReferenceComponent> Components => this.m_lstComponents;

  public string DataSpaceName
  {
    get => this.m_strDataSpaceName;
    set => this.m_strDataSpaceName = value;
  }

  public DataSpaceMapEntry()
  {
  }

  public DataSpaceMapEntry(Stream stream)
  {
    byte[] buffer = new byte[4];
    SecurityHelper.ReadInt32(stream, buffer);
    int num = SecurityHelper.ReadInt32(stream, buffer);
    for (int index = 0; index < num; ++index)
      this.m_lstComponents.Add(new DataSpaceReferenceComponent(stream));
    this.m_strDataSpaceName = SecurityHelper.ReadUnicodeStringP4(stream);
  }

  public void Serialize(Stream stream)
  {
    long num = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    stream.Position += 4L;
    int count = this.m_lstComponents.Count;
    SecurityHelper.WriteInt32(stream, count);
    for (int index = 0; index < count; ++index)
      this.m_lstComponents[index].Serialize(stream);
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strDataSpaceName);
    long position = stream.Position;
    stream.Position = num;
    SecurityHelper.WriteInt32(stream, (int) (position - num));
    stream.Position = position;
  }
}
