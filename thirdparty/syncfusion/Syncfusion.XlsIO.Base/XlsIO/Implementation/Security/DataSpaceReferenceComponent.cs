// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.DataSpaceReferenceComponent
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class DataSpaceReferenceComponent
{
  private int m_iComponentType;
  private string m_strName;

  public int ComponentType => this.m_iComponentType;

  public string Name => this.m_strName;

  public DataSpaceReferenceComponent(int type, string name)
  {
    this.m_iComponentType = type;
    this.m_strName = name;
  }

  public DataSpaceReferenceComponent(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iComponentType = SecurityHelper.ReadInt32(stream, buffer);
    this.m_strName = SecurityHelper.ReadUnicodeStringP4(stream);
  }

  public void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    SecurityHelper.WriteInt32(stream, this.m_iComponentType);
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strName);
  }
}
