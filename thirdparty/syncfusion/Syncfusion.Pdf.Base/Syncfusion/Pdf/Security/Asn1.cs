// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class Asn1 : Syncfusion.Pdf.Security.Asn1Encode
{
  private Asn1UniversalTags m_tag;
  private MemoryStream m_stream;

  internal Asn1()
  {
  }

  internal Asn1(Asn1UniversalTags tag) => this.m_tag = tag;

  internal static Asn1 FromByteArray(byte[] data)
  {
    try
    {
      return new Asn1Stream(data).ReadAsn1();
    }
    catch (InvalidCastException ex)
    {
      throw new IOException("Invalid entry");
    }
  }

  internal static Asn1 FromStream(Stream stream)
  {
    try
    {
      return new Asn1Stream(stream).ReadAsn1();
    }
    catch (InvalidCastException ex)
    {
      throw new IOException("Invalid entry");
    }
  }

  public sealed override Asn1 GetAsn1() => this;

  internal bool Asn1Equals(Asn1 obj) => this.IsEquals(obj);

  internal int GetAsn1Hash() => this.GetHashCode();

  internal byte[] Asn1Encode(byte[] bytes)
  {
    this.m_stream = new MemoryStream();
    this.m_stream.WriteByte((byte) this.m_tag);
    this.Write(bytes.Length);
    this.m_stream.Write(bytes, 0, bytes.Length);
    this.m_stream.Close();
    return this.m_stream.ToArray();
  }

  internal new byte[] GetDerEncoded()
  {
    try
    {
      MemoryStream memoryStream = new MemoryStream();
      new DerStream((Stream) memoryStream).WriteObject((object) this);
      return memoryStream.ToArray();
    }
    catch (Exception ex)
    {
      return (byte[]) null;
    }
  }

  internal abstract void Encode(DerStream derOut);

  protected abstract bool IsEquals(Asn1 asn1Object);

  public new abstract int GetHashCode();

  private void Write(int length)
  {
    if (length > (int) sbyte.MaxValue)
    {
      int num1 = 1;
      uint num2 = (uint) length;
      while ((num2 >>= 8) != 0U)
        ++num1;
      this.m_stream.WriteByte((byte) (num1 | 128 /*0x80*/));
      for (int index = (num1 - 1) * 8; index >= 0; index -= 8)
        this.m_stream.WriteByte((byte) (length >> index));
    }
    else
      this.m_stream.WriteByte((byte) length);
  }
}
