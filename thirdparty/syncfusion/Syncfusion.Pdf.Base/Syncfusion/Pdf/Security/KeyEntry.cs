// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyEntry
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class KeyEntry
{
  private readonly CipherParameter m_key;
  private readonly IDictionary m_attributes;

  internal CipherParameter Key => this.m_key;

  internal KeyEntry(CipherParameter key, IDictionary attributes)
  {
    this.m_key = key;
    this.m_attributes = attributes;
  }

  public override bool Equals(object obj)
  {
    return obj is KeyEntry keyEntry && this.m_key.Equals((object) keyEntry.m_key);
  }

  public override int GetHashCode() => ~this.m_key.GetHashCode();
}
