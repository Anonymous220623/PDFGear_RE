// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.DataSpaceDefinition
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class DataSpaceDefinition
{
  private const int DefaultHeaderLength = 8;
  private int m_iHeaderLength = 8;
  private List<string> m_lstTransformRefs = new List<string>();

  public List<string> TransformRefs => this.m_lstTransformRefs;

  public DataSpaceDefinition()
  {
  }

  public DataSpaceDefinition(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iHeaderLength = SecurityHelper.ReadInt32(stream, buffer);
    int num = SecurityHelper.ReadInt32(stream, buffer);
    if (this.m_iHeaderLength != 8)
      stream.Position += (long) (this.m_iHeaderLength - 8);
    for (int index = 0; index < num; ++index)
      this.m_lstTransformRefs.Add(SecurityHelper.ReadUnicodeStringP4(stream));
  }

  public void Serialize(Stream stream)
  {
    SecurityHelper.WriteInt32(stream, this.m_iHeaderLength);
    int count = this.m_lstTransformRefs.Count;
    SecurityHelper.WriteInt32(stream, count);
    for (int index = 0; index < count; ++index)
    {
      string lstTransformRef = this.m_lstTransformRefs[index];
      SecurityHelper.WriteUnicodeStringP4(stream, lstTransformRef);
    }
  }
}
