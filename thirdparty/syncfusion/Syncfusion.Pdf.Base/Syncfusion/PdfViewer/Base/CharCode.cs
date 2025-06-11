// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.CharCode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal struct CharCode
{
  private readonly byte[] bytes;
  private int intValue;

  public int BytesCount => this.bytes == null ? 0 : this.bytes.Length;

  public byte[] Bytes => this.bytes;

  public int IntValue
  {
    get => this.intValue;
    set => this.intValue = value;
  }

  public bool IsEmpty => this.bytes == null;

  public CharCode(byte[] bytes)
  {
    this.bytes = bytes;
    this.intValue = BytesAssistant.GetInt(this.bytes);
  }

  public CharCode(byte b)
  {
    this.bytes = new byte[1];
    this.bytes[0] = b;
    this.intValue = (int) b;
  }

  public CharCode(ushort us)
  {
    byte[] bytes = BitConverter.GetBytes(us);
    this.bytes = new byte[bytes.Length];
    int num = bytes.Length - 1;
    for (int index = 0; index < bytes.Length; ++index)
      this.bytes[num - index] = bytes[index];
    this.intValue = (int) us;
  }

  public CharCode(int ii)
  {
    byte[] bytes = BitConverter.GetBytes(ii);
    this.bytes = new byte[bytes.Length];
    int num = bytes.Length - 1;
    for (int index = 0; index < bytes.Length; ++index)
      this.bytes[num - index] = bytes[index];
    this.intValue = ii;
  }

  private static void InsureCharCodes(CharCode left, CharCode right)
  {
    if (left.bytes == null || right.bytes == null)
      throw new ArgumentException("Bytes cannot be null.");
    if (left.bytes.Length != right.bytes.Length)
      throw new InvalidOperationException("Cannot compare CharCodes with different length.");
  }

  public static CharCode operator ++(CharCode cc)
  {
    byte[] bytes = (byte[]) cc.bytes.Clone();
    int num1 = 1;
    for (int index = bytes.Length - 1; index >= 0; --index)
    {
      int num2 = (int) bytes[index] + num1;
      num1 = num2 / 256 /*0x0100*/;
      bytes[index] = (byte) (num2 % 256 /*0x0100*/);
      if (num1 == 0)
        break;
    }
    if (num1 > 0)
      throw new OverflowException();
    return new CharCode(bytes);
  }

  public static bool operator <(CharCode left, CharCode right)
  {
    CharCode.InsureCharCodes(left, right);
    for (int index = 0; index < left.bytes.Length; ++index)
    {
      if ((int) left.bytes[index] > (int) right.bytes[index] || (int) left.bytes[index] == (int) right.bytes[index] && left.IntValue == right.IntValue)
        return false;
    }
    return true;
  }

  public static bool operator ==(CharCode left, CharCode right)
  {
    CharCode.InsureCharCodes(left, right);
    for (int index = 0; index < left.bytes.Length; ++index)
    {
      if ((int) left.bytes[index] != (int) right.bytes[index])
        return false;
    }
    return true;
  }

  public static bool operator >(CharCode left, CharCode right)
  {
    CharCode.InsureCharCodes(left, right);
    for (int index = 0; index < left.bytes.Length; ++index)
    {
      if ((int) left.bytes[index] <= (int) right.bytes[index])
        return false;
    }
    return true;
  }

  public static bool operator !=(CharCode left, CharCode right) => !(left == right);

  public static bool operator <=(CharCode left, CharCode right) => left < right || left == right;

  public static bool operator >=(CharCode left, CharCode right) => left > right || left == right;

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<");
    for (int index = 0; index < this.bytes.Length; ++index)
      stringBuilder.AppendFormat("{0:X2}", (object) this.bytes[index]);
    stringBuilder.Append("> ");
    stringBuilder.Append(this.GetHashCode());
    return stringBuilder.ToString();
  }

  public override bool Equals(object obj)
  {
    if (this.bytes == null)
      return false;
    if (!(obj is CharCode charCode))
      return base.Equals(obj);
    return this.BytesCount == charCode.BytesCount && this == charCode;
  }

  public override int GetHashCode() => this.IntValue;
}
