// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerBoolean
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerBoolean : Asn1
{
  private byte m_value;
  internal static readonly DerBoolean False = new DerBoolean(false);
  internal static readonly DerBoolean True = new DerBoolean(true);

  internal bool IsTrue => this.m_value != (byte) 0;

  internal DerBoolean(byte[] bytes)
  {
    this.m_value = bytes.Length == 1 ? bytes[0] : throw new ArgumentException("Invalid length in bytes");
  }

  private DerBoolean(bool value) => this.m_value = value ? byte.MaxValue : (byte) 0;

  internal override void Encode(DerStream stream)
  {
    stream.WriteEncoded(1, new byte[1]{ this.m_value });
  }

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerBoolean derBoolean && this.IsTrue == derBoolean.IsTrue;
  }

  public override int GetHashCode() => this.IsTrue.GetHashCode();

  public override string ToString() => !this.IsTrue ? "FALSE" : "TRUE";

  internal static DerBoolean GetBoolean(object obj)
  {
    switch (obj)
    {
      case null:
      case DerBoolean _:
        return (DerBoolean) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }
}
