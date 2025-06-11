// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

public class RevocationResult
{
  private bool m_isRevokedCRL;
  private RevocationStatus m_revocationStatus;

  public bool IsRevokedCRL
  {
    get => this.m_isRevokedCRL;
    internal set => this.m_isRevokedCRL = value;
  }

  public RevocationStatus OcspRevocationStatus
  {
    get => this.m_revocationStatus;
    internal set => this.m_revocationStatus = value;
  }
}
