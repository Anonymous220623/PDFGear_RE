// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerPrintableString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerPrintableString : DerString
{
  private string m_value;

  internal DerPrintableString(byte[] bytes)
    : this(Encoding.ASCII.GetString(bytes, 0, bytes.Length))
  {
  }

  internal DerPrintableString(string value)
  {
    this.m_value = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  internal byte[] Asn1Encode() => this.Asn1Encode(this.GetBytes());

  public override string GetString() => this.m_value;

  public byte[] GetBytes() => Encoding.ASCII.GetBytes(this.m_value);

  internal override void Encode(DerStream stream) => stream.WriteEncoded(19, this.GetBytes());

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerPrintableString derPrintableString && this.m_value.Equals(derPrintableString.m_value);
  }
}
