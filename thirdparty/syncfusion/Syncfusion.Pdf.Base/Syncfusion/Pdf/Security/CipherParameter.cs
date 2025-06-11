// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CipherParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class CipherParameter : ICipherParam
{
  private readonly bool m_privateKey;

  protected CipherParameter(bool privateKey) => this.m_privateKey = privateKey;

  internal bool IsPrivate => this.m_privateKey;

  public override bool Equals(object obj) => obj is CipherParameter other && this.Equals(other);

  protected bool Equals(CipherParameter other) => this.m_privateKey == other.m_privateKey;

  public override int GetHashCode() => this.m_privateKey.GetHashCode();
}
