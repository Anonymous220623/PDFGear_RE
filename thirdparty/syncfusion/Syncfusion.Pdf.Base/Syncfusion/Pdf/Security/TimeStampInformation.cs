// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class TimeStampInformation
{
  private bool m_isValid = true;
  private X509Certificate2 m_certificate;
  private string m_messageImprintAlgorithmId;
  private string m_timeStampPolicyId;
  private DateTime m_time;
  private object m_signerInformation;

  internal string MessageImprintAlgorithmId
  {
    get => this.m_messageImprintAlgorithmId;
    set => this.m_messageImprintAlgorithmId = value;
  }

  public string TimeStampPolicyId
  {
    get => this.m_timeStampPolicyId;
    internal set => this.m_timeStampPolicyId = value;
  }

  public DateTime Time
  {
    get => this.m_time;
    internal set => this.m_time = value;
  }

  internal object SignerInformation
  {
    get => this.m_signerInformation;
    set => this.m_signerInformation = value;
  }

  public bool IsValid
  {
    get => this.m_isValid;
    internal set => this.m_isValid = value;
  }

  internal X509Certificate2 Certificate
  {
    get => this.m_certificate;
    set => this.m_certificate = value;
  }
}
