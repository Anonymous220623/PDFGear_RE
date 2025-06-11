// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerBmpString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerBmpString : DerString
{
  private string m_value;

  internal DerBmpString(byte[] bytes)
  {
    char[] chArray = bytes != null ? new char[bytes.Length / 2] : throw new ArgumentNullException(nameof (bytes));
    for (int index = 0; index != chArray.Length; ++index)
      chArray[index] = (char) ((int) bytes[2 * index] << 8 | (int) bytes[2 * index + 1] & (int) byte.MaxValue);
    this.m_value = new string(chArray);
  }

  public override string GetString() => this.m_value;

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerBmpString derBmpString && this.m_value.Equals(derBmpString.m_value);
  }

  internal override void Encode(DerStream stream)
  {
    char[] charArray = this.m_value.ToCharArray();
    byte[] bytes = new byte[charArray.Length * 2];
    for (int index = 0; index != charArray.Length; ++index)
    {
      bytes[2 * index] = (byte) ((uint) charArray[index] >> 8);
      bytes[2 * index + 1] = (byte) charArray[index];
    }
    stream.WriteEncoded(30, bytes);
  }
}
