// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation.OTableOfContent
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation;

internal class OTableOfContent
{
  private string m_name;
  private bool m_isProtected;
  private string m_protectionKey;
  private string m_protectionKeyDigestAlgorithm;

  internal string ProtectionKeyDigestAlgorithm
  {
    get => this.m_protectionKeyDigestAlgorithm;
    set => this.m_protectionKeyDigestAlgorithm = value;
  }

  internal string ProtectionKey
  {
    get => this.m_protectionKey;
    set => this.m_protectionKey = value;
  }

  internal bool Isprotected
  {
    get => this.m_isProtected;
    set => this.m_isProtected = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }
}
