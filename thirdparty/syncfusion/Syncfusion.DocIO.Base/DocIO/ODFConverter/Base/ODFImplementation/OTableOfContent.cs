// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.OTableOfContent
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODFConverter.Base.ODFImplementation;

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
