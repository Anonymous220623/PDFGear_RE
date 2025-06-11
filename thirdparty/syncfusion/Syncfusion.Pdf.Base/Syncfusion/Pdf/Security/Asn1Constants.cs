// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Constants
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class Asn1Constants
{
  internal const string BerSequence = "BER Sequence";
  internal const string DerSequence = "DER Sequence";
  internal const string Sequence = "Sequence";
  internal const string Null = "NULL";
  internal const string Empty = "EMPTY";
  internal const string Der = "DER";
  internal const string Ber = "BER";
  internal const string DesEde = "DESede";
  internal const string Des = "DES";
  internal const string RC2 = "RC2";
  internal const string RSA = "RSA";
  internal const string PKCS7 = "PKCS7";

  internal static void UInt32ToBe(uint n, byte[] bs, int off)
  {
    bs[off] = (byte) (n >> 24);
    bs[off + 1] = (byte) (n >> 16 /*0x10*/);
    bs[off + 2] = (byte) (n >> 8);
    bs[off + 3] = (byte) n;
  }

  internal static uint BeToUInt32(byte[] bs, int off)
  {
    return (uint) ((int) bs[off] << 24 | (int) bs[off + 1] << 16 /*0x10*/ | (int) bs[off + 2] << 8) | (uint) bs[off + 3];
  }

  internal static byte[] UInt64ToBe(ulong n)
  {
    byte[] bs = new byte[8];
    Asn1Constants.UInt64ToBe(n, bs, 0);
    return bs;
  }

  internal static void UInt32ToBe(uint n, byte[] bs)
  {
    bs[0] = (byte) (n >> 24);
    bs[1] = (byte) (n >> 16 /*0x10*/);
    bs[2] = (byte) (n >> 8);
    bs[3] = (byte) n;
  }

  internal static void UInt64ToBe(ulong n, byte[] bs)
  {
    Asn1Constants.UInt32ToBe((uint) (n >> 32 /*0x20*/), bs);
    Asn1Constants.UInt32ToBe((uint) n, bs, 4);
  }

  internal static void UInt64ToBe(ulong n, byte[] bs, int off)
  {
    Asn1Constants.UInt32ToBe((uint) (n >> 32 /*0x20*/), bs, off);
    Asn1Constants.UInt32ToBe((uint) n, bs, off + 4);
  }

  internal static uint BeToUInt32(byte[] bs)
  {
    return (uint) ((int) bs[0] << 24 | (int) bs[1] << 16 /*0x10*/ | (int) bs[2] << 8) | (uint) bs[3];
  }

  internal static byte[] UInt32ToLe(uint n)
  {
    byte[] bs = new byte[4];
    Asn1Constants.UInt32ToLe(n, bs, 0);
    return bs;
  }

  internal static void UInt32ToLe(uint n, byte[] bs)
  {
    bs[0] = (byte) n;
    bs[1] = (byte) (n >> 8);
    bs[2] = (byte) (n >> 16 /*0x10*/);
    bs[3] = (byte) (n >> 24);
  }

  internal static void UInt32ToLe(uint n, byte[] bs, int off)
  {
    bs[off] = (byte) n;
    bs[off + 1] = (byte) (n >> 8);
    bs[off + 2] = (byte) (n >> 16 /*0x10*/);
    bs[off + 3] = (byte) (n >> 24);
  }

  internal static byte[] UInt32ToLe(uint[] ns)
  {
    byte[] bs = new byte[4 * ns.Length];
    Asn1Constants.UInt32ToLe(ns, bs, 0);
    return bs;
  }

  internal static void UInt32ToLe(uint[] ns, byte[] bs, int off)
  {
    for (int index = 0; index < ns.Length; ++index)
    {
      Asn1Constants.UInt32ToLe(ns[index], bs, off);
      off += 4;
    }
  }

  internal static ushort LeToUInt16(byte[] bs) => (ushort) ((uint) bs[0] | (uint) bs[1] << 8);

  internal static ushort LeToUInt16(byte[] bs, int off)
  {
    return (ushort) ((uint) bs[off] | (uint) bs[off + 1] << 8);
  }

  internal static uint LeToUInt32(byte[] bs)
  {
    return (uint) ((int) bs[0] | (int) bs[1] << 8 | (int) bs[2] << 16 /*0x10*/ | (int) bs[3] << 24);
  }

  internal static uint LeToUInt32(byte[] bs, int off)
  {
    return (uint) ((int) bs[off] | (int) bs[off + 1] << 8 | (int) bs[off + 2] << 16 /*0x10*/ | (int) bs[off + 3] << 24);
  }

  internal static void LeToUInt32(byte[] bs, int off, uint[] ns)
  {
    for (int index = 0; index < ns.Length; ++index)
    {
      ns[index] = Asn1Constants.LeToUInt32(bs, off);
      off += 4;
    }
  }

  internal static ulong BeToUInt64(byte[] bs)
  {
    return (ulong) Asn1Constants.BeToUInt32(bs) << 32 /*0x20*/ | (ulong) Asn1Constants.BeToUInt32(bs, 4);
  }

  internal static ulong BeToUInt64(byte[] bs, int off)
  {
    return (ulong) Asn1Constants.BeToUInt32(bs, off) << 32 /*0x20*/ | (ulong) Asn1Constants.BeToUInt32(bs, off + 4);
  }

  public static bool AreEqual(byte[] a, byte[] b)
  {
    if (a == b)
      return true;
    return a != null && b != null && Asn1Constants.HaveSameContents(a, b);
  }

  private static bool HaveSameContents(byte[] a, byte[] b)
  {
    int length = a.Length;
    if (length != b.Length)
      return false;
    while (length != 0)
    {
      --length;
      if ((int) a[length] != (int) b[length])
        return false;
    }
    return true;
  }

  public static int GetHashCode(byte[] data)
  {
    if (data == null)
      return 0;
    int length = data.Length;
    int hashCode = length + 1;
    while (--length >= 0)
      hashCode = hashCode * 257 ^ (int) data[length];
    return hashCode;
  }

  public static byte[] Clone(byte[] data) => data != null ? (byte[]) data.Clone() : (byte[]) null;

  public static int[] Clone(int[] data) => data != null ? (int[]) data.Clone() : (int[]) null;
}
