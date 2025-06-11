// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerTeleText
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerTeleText : DerString
{
  private string m_value;

  internal DerTeleText(byte[] bytes)
    : this(DerTeleText.FromByteArray(bytes))
  {
  }

  public DerTeleText(string value)
  {
    this.m_value = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public override string GetString() => this.m_value;

  internal override void Encode(DerStream stream) => stream.WriteEncoded(20, this.GetBytes());

  public byte[] GetBytes() => this.ToByteArray(this.m_value);

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerTeleText derTeleText && this.m_value.Equals(derTeleText.m_value);
  }

  private byte[] ToByteArray(string value)
  {
    byte[] byteArray = new byte[value.Length];
    for (int index = 0; index < byteArray.Length; ++index)
      byteArray[index] = Convert.ToByte(value[index]);
    return byteArray;
  }

  internal static DerTeleText GetTeleText(object obj)
  {
    switch (obj)
    {
      case null:
      case DerTeleText _:
        return (DerTeleText) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal static DerTeleText GetTeleText(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is DerTeleText ? DerTeleText.GetTeleText((object) asn1) : new DerTeleText(Asn1Octet.GetOctetString((object) asn1).GetOctets());
  }

  internal static string FromByteArray(byte[] bytes)
  {
    char[] chArray = new char[bytes.Length];
    for (int index = 0; index < chArray.Length; ++index)
      chArray[index] = Convert.ToChar(bytes[index]);
    return new string(chArray);
  }
}
