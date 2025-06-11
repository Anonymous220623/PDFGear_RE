// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.AsnKeyBuilder
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

#nullable disable
namespace Patagames.Activation;

/// <summary>
/// A class file to aide in working with ASN.1 encoded
/// objects such as PKCS#8 PrivateKeyInfo messages and
/// X.509 PublicKeyInfo messages. Useful for exporting
/// a RSA or DSA key for use in Java and other non-XML
/// encoded key aware languages.
/// </summary>
/// <remarks>Jeffrey Walton</remarks>
internal class AsnKeyBuilder
{
  private static byte[] ZERO = new byte[1];
  private static byte[] EMPTY = new byte[0];

  /// <summary>
  /// Returns the AsnMessage representing the X.509 PublicKeyInfo.
  /// </summary>
  /// <param name="publicKey">The DSA key to be encoded.</param>
  /// <returns>Returns the AsnType representing the
  /// X.509 PublicKeyInfo.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PrivateKeyToPKCS8(System.Security.Cryptography.DSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PrivateKeyToPKCS8(System.Security.Cryptography.RSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PublicKeyToX509(System.Security.Cryptography.RSAParameters)" />
  internal static AsnKeyBuilder.AsnMessage PublicKeyToX509(DSAParameters publicKey)
  {
    AsnKeyBuilder.AsnType sequence = AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[3]
    {
      AsnKeyBuilder.CreateIntegerPos(publicKey.P),
      AsnKeyBuilder.CreateIntegerPos(publicKey.Q),
      AsnKeyBuilder.CreateIntegerPos(publicKey.G)
    });
    return new AsnKeyBuilder.AsnMessage(AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
    {
      AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
      {
        AsnKeyBuilder.CreateOid("1.2.840.10040.4.1"),
        sequence
      }),
      AsnKeyBuilder.CreateBitString(AsnKeyBuilder.CreateIntegerPos(publicKey.Y))
    }).GetBytes(), "X.509");
  }

  /// <summary>
  /// Returns the AsnMessage representing the X.509 PublicKeyInfo.
  /// </summary>
  /// <param name="publicKey">The RSA key to be encoded.</param>
  /// <returns>Returns the AsnType representing the
  /// X.509 PublicKeyInfo.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PrivateKeyToPKCS8(System.Security.Cryptography.DSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PrivateKeyToPKCS8(System.Security.Cryptography.RSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PublicKeyToX509(System.Security.Cryptography.DSAParameters)" />
  internal static AsnKeyBuilder.AsnMessage PublicKeyToX509(RSAParameters publicKey)
  {
    return new AsnKeyBuilder.AsnMessage(AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
    {
      AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
      {
        AsnKeyBuilder.CreateOid("1.2.840.113549.1.1.1"),
        AsnKeyBuilder.CreateNull()
      }),
      AsnKeyBuilder.CreateBitString(AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
      {
        AsnKeyBuilder.CreateIntegerPos(publicKey.Modulus),
        AsnKeyBuilder.CreateIntegerPos(publicKey.Exponent)
      }))
    }).GetBytes(), "X.509");
  }

  /// <summary>
  /// Returns AsnMessage representing the unencrypted
  /// PKCS #8 PrivateKeyInfo.
  /// </summary>
  /// <param name="privateKey">The DSA key to be encoded.</param>
  /// <returns>Returns the AsnType representing the unencrypted
  /// PKCS #8 PrivateKeyInfo.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PrivateKeyToPKCS8(System.Security.Cryptography.RSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PublicKeyToX509(System.Security.Cryptography.DSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PublicKeyToX509(System.Security.Cryptography.RSAParameters)" />
  internal static AsnKeyBuilder.AsnMessage PrivateKeyToPKCS8(DSAParameters privateKey)
  {
    AsnKeyBuilder.AsnType integer = AsnKeyBuilder.CreateInteger(AsnKeyBuilder.ZERO);
    AsnKeyBuilder.AsnType sequence1 = AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[3]
    {
      AsnKeyBuilder.CreateIntegerPos(privateKey.P),
      AsnKeyBuilder.CreateIntegerPos(privateKey.Q),
      AsnKeyBuilder.CreateIntegerPos(privateKey.G)
    });
    AsnKeyBuilder.AsnType sequence2 = AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
    {
      AsnKeyBuilder.CreateOid("1.2.840.10040.4.1"),
      sequence1
    });
    AsnKeyBuilder.AsnType octetString = AsnKeyBuilder.CreateOctetString(AsnKeyBuilder.CreateIntegerPos(privateKey.X));
    return new AsnKeyBuilder.AsnMessage(AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[3]
    {
      integer,
      sequence2,
      octetString
    }).GetBytes(), "PKCS#8");
  }

  /// <summary>
  /// Returns AsnMessage representing the unencrypted
  /// PKCS #8 PrivateKeyInfo.
  /// </summary>
  /// <param name="privateKey">The RSA key to be encoded.</param>
  /// <returns>Returns the AsnType representing the unencrypted
  /// PKCS #8 PrivateKeyInfo.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PrivateKeyToPKCS8(System.Security.Cryptography.DSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PublicKeyToX509(System.Security.Cryptography.DSAParameters)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.PublicKeyToX509(System.Security.Cryptography.RSAParameters)" />
  internal static AsnKeyBuilder.AsnMessage PrivateKeyToPKCS8(RSAParameters privateKey)
  {
    AsnKeyBuilder.AsnType integerPos1 = AsnKeyBuilder.CreateIntegerPos(privateKey.Modulus);
    AsnKeyBuilder.AsnType integerPos2 = AsnKeyBuilder.CreateIntegerPos(privateKey.Exponent);
    AsnKeyBuilder.AsnType integerPos3 = AsnKeyBuilder.CreateIntegerPos(privateKey.D);
    AsnKeyBuilder.AsnType integerPos4 = AsnKeyBuilder.CreateIntegerPos(privateKey.P);
    AsnKeyBuilder.AsnType integerPos5 = AsnKeyBuilder.CreateIntegerPos(privateKey.Q);
    AsnKeyBuilder.AsnType integerPos6 = AsnKeyBuilder.CreateIntegerPos(privateKey.DP);
    AsnKeyBuilder.AsnType integerPos7 = AsnKeyBuilder.CreateIntegerPos(privateKey.DQ);
    AsnKeyBuilder.AsnType integerPos8 = AsnKeyBuilder.CreateIntegerPos(privateKey.InverseQ);
    AsnKeyBuilder.AsnType integer = AsnKeyBuilder.CreateInteger(new byte[1]);
    AsnKeyBuilder.AsnType octetString = AsnKeyBuilder.CreateOctetString(AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[9]
    {
      integer,
      integerPos1,
      integerPos2,
      integerPos3,
      integerPos4,
      integerPos5,
      integerPos6,
      integerPos7,
      integerPos8
    }));
    AsnKeyBuilder.AsnType sequence = AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[2]
    {
      AsnKeyBuilder.CreateOid("1.2.840.113549.1.1.1"),
      AsnKeyBuilder.CreateNull()
    });
    return new AsnKeyBuilder.AsnMessage(AsnKeyBuilder.CreateSequence(new AsnKeyBuilder.AsnType[3]
    {
      integer,
      sequence,
      octetString
    }).GetBytes(), "PKCS#8");
  }

  /// <summary>
  /// <para>An ordered collection of one or more types.
  /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
  /// <para>If the AsnType is null, an empty sequence (length 0)
  /// is returned.</para>
  /// </summary>
  /// <param name="value">An AsnType consisting of
  /// a single value to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded sequence.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  internal static AsnKeyBuilder.AsnType CreateSequence(AsnKeyBuilder.AsnType value)
  {
    return !AsnKeyBuilder.IsEmpty(value) ? new AsnKeyBuilder.AsnType((byte) 48 /*0x30*/, value.GetBytes()) : throw new ArgumentException("A sequence requires at least one value.");
  }

  /// <summary>
  /// <para>An ordered collection of one or more types.
  /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
  /// <para>If the AsnType is null, an
  /// empty sequence (length 0) is returned.</para>
  /// </summary>
  /// <param name="values">An array of AsnType consisting of
  /// the values in the collection to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded Set.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  internal static AsnKeyBuilder.AsnType CreateSequence(AsnKeyBuilder.AsnType[] values)
  {
    return !AsnKeyBuilder.IsEmpty(values) ? new AsnKeyBuilder.AsnType((byte) 48 /*0x30*/, AsnKeyBuilder.Concatenate(values)) : throw new ArgumentException("A sequence requires at least one value.");
  }

  /// <summary>
  /// <para>An ordered collection zero, one or more types.
  /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
  /// <para>If the AsnType value is null,an
  /// empty sequence (length 0) is returned.</para>
  /// </summary>
  /// <param name="value">An AsnType consisting of
  /// a single value to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded sequence.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  internal static AsnKeyBuilder.AsnType CreateSequenceOf(AsnKeyBuilder.AsnType value)
  {
    return AsnKeyBuilder.IsEmpty(value) ? new AsnKeyBuilder.AsnType((byte) 48 /*0x30*/, AsnKeyBuilder.EMPTY) : new AsnKeyBuilder.AsnType((byte) 48 /*0x30*/, value.GetBytes());
  }

  /// <summary>
  /// <para>An ordered collection zero, one or more types.
  /// Returns the AsnType representing an ASN.1 encoded sequence.</para>
  /// <para>If the AsnType array is null or the array is 0 length,
  /// an empty sequence (length 0) is returned.</para>
  /// </summary>
  /// <param name="values">An AsnType consisting of
  /// the values in the collection to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded sequence.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequence(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateSequenceOf(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  internal static AsnKeyBuilder.AsnType CreateSequenceOf(AsnKeyBuilder.AsnType[] values)
  {
    return AsnKeyBuilder.IsEmpty(values) ? new AsnKeyBuilder.AsnType((byte) 48 /*0x30*/, AsnKeyBuilder.EMPTY) : new AsnKeyBuilder.AsnType((byte) 48 /*0x30*/, AsnKeyBuilder.Concatenate(values));
  }

  /// <summary>
  /// <para>An ordered sequence of zero, one or more bits. Returns
  /// the AsnType representing an ASN.1 encoded bit string.</para>
  /// <para>If octets is null or length is 0, an empty (0 length)
  /// bit string is returned.</para>
  /// </summary>
  /// <param name="octets">A MSB (big endian) byte[] representing the
  /// bit string to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded bit string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateBitString(byte[] octets)
  {
    return AsnKeyBuilder.CreateBitString(octets, 0U);
  }

  /// <summary>
  /// <para>An ordered sequence of zero, one or more bits. Returns
  /// the AsnType representing an ASN.1 encoded bit string.</para>
  /// <para>unusedBits is applied to the end of the bit string,
  /// not the start of the bit string. unusedBits must be less than 8
  /// (the size of an octet). Refer to ITU X.680, Section 32.</para>
  /// <para>If octets is null or length is 0, an empty (0 length)
  /// bit string is returned.</para>
  /// </summary>
  /// <param name="octets">A MSB (big endian) byte[] representing the
  /// bit string to be encoded.</param>
  /// <param name="unusedBits">The number of unused trailing binary
  /// digits in the bit string to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded bit string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateBitString(byte[] octets, uint unusedBits)
  {
    if (AsnKeyBuilder.IsEmpty(octets))
      return new AsnKeyBuilder.AsnType((byte) 3, AsnKeyBuilder.EMPTY);
    return unusedBits < 8U ? new AsnKeyBuilder.AsnType((byte) 3, AsnKeyBuilder.Concatenate(new byte[1]
    {
      (byte) unusedBits
    }, octets)) : throw new ArgumentException("Unused bits must be less than 8.");
  }

  /// <summary>
  /// An ordered sequence of zero, one or more bits. Returns
  /// the AsnType representing an ASN.1 encoded bit string.
  /// If value is null, an empty (0 length) bit string is
  /// returned.
  /// </summary>
  /// <param name="value">An AsnType object to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded bit string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateBitString(AsnKeyBuilder.AsnType value)
  {
    return AsnKeyBuilder.IsEmpty(value) ? new AsnKeyBuilder.AsnType((byte) 3, AsnKeyBuilder.EMPTY) : AsnKeyBuilder.CreateBitString(value.GetBytes(), 0U);
  }

  /// <summary>
  /// An ordered sequence of zero, one or more bits. Returns
  /// the AsnType representing an ASN.1 encoded bit string.
  /// If value is null, an empty (0 length) bit string is
  /// returned.
  /// </summary>
  /// <param name="values">An AsnType object to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded bit string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateBitString(AsnKeyBuilder.AsnType[] values)
  {
    return AsnKeyBuilder.IsEmpty(values) ? new AsnKeyBuilder.AsnType((byte) 3, AsnKeyBuilder.EMPTY) : AsnKeyBuilder.CreateBitString(AsnKeyBuilder.Concatenate(values), 0U);
  }

  /// <summary>
  /// <para>An ordered sequence of zero, one or more bits. Returns
  /// the AsnType representing an ASN.1 encoded bit string.</para>
  /// <para>If octets is null or length is 0, an empty (0 length)
  /// bit string is returned.</para>
  /// <para>If conversion fails, the bit string returned is a partial
  /// bit string. The partial bit string ends at the octet before the
  /// point of failure (it does not include the octet which could
  /// not be parsed, or subsequent octets).</para>
  /// </summary>
  /// <param name="value">A MSB (big endian) byte[] representing the
  /// bit string to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded bit string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateBitString(string value)
  {
    if (AsnKeyBuilder.IsEmpty(value))
      return AsnKeyBuilder.CreateBitString(AsnKeyBuilder.EMPTY);
    int length = value.Length;
    int unusedBits = 8 - length % 8;
    if (8 == unusedBits)
      unusedBits = 0;
    for (int index = 0; index < unusedBits; ++index)
      value += "0";
    int num1 = (length + 7) / 8;
    List<byte> byteList = new List<byte>();
    for (int index = 0; index < num1; ++index)
    {
      string str = value.Substring(index * 8, 8);
      byte num2;
      try
      {
        num2 = Convert.ToByte(str, 2);
      }
      catch (FormatException ex)
      {
        unusedBits = 0;
        break;
      }
      catch (OverflowException ex)
      {
        unusedBits = 0;
        break;
      }
      byteList.Add(num2);
    }
    return AsnKeyBuilder.CreateBitString(byteList.ToArray(), (uint) unusedBits);
  }

  /// <summary>
  /// An ordered sequence of zero, one or more octets. Returns
  /// the ASN.1 encoded octet string. If octets is null or length
  /// is 0, an empty (0 length) octet string is returned.
  /// </summary>
  /// <param name="value">A MSB (big endian) byte[] representing the
  /// octet string to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded octet string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateOctetString(byte[] value)
  {
    return AsnKeyBuilder.IsEmpty(value) ? new AsnKeyBuilder.AsnType((byte) 4, AsnKeyBuilder.EMPTY) : new AsnKeyBuilder.AsnType((byte) 4, value);
  }

  /// <summary>
  /// An ordered sequence of zero, one or more octets. Returns
  /// the byte[] representing an ASN.1 encoded octet string.
  /// If octets is null or length is 0, an empty (0 length)
  /// o ctet string is returned.
  /// </summary>
  /// <param name="value">An AsnType object to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded octet string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateOctetString(AsnKeyBuilder.AsnType value)
  {
    return AsnKeyBuilder.IsEmpty(value) ? new AsnKeyBuilder.AsnType((byte) 4, (byte) 0) : new AsnKeyBuilder.AsnType((byte) 4, value.GetBytes());
  }

  /// <summary>
  /// An ordered sequence of zero, one or more octets. Returns
  /// the byte[] representing an ASN.1 encoded octet string.
  /// If octets is null or length is 0, an empty (0 length)
  /// o ctet string is returned.
  /// </summary>
  /// <param name="values">An AsnType object to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded octet string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateOctetString(AsnKeyBuilder.AsnType[] values)
  {
    return AsnKeyBuilder.IsEmpty(values) ? new AsnKeyBuilder.AsnType((byte) 4, (byte) 0) : new AsnKeyBuilder.AsnType((byte) 4, AsnKeyBuilder.Concatenate(values));
  }

  /// <summary>
  /// <para>An ordered sequence of zero, one or more bits. Returns
  /// the AsnType representing an ASN.1 encoded octet string.</para>
  /// <para>If octets is null or length is 0, an empty (0 length)
  /// octet string is returned.</para>
  /// <para>If conversion fails, the bit string returned is a partial
  /// bit string. The partial octet string ends at the octet before the
  /// point of failure (it does not include the octet which could
  /// not be parsed, or subsequent octets).</para>
  /// </summary>
  /// <param name="value">A string representing the
  /// octet string to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded octet string.</returns>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.Byte[],System.UInt32)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(System.String)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateBitString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType)" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOctetString(Patagames.Activation.AsnKeyBuilder.AsnType[])" />
  internal static AsnKeyBuilder.AsnType CreateOctetString(string value)
  {
    if (AsnKeyBuilder.IsEmpty(value))
      return AsnKeyBuilder.CreateOctetString(AsnKeyBuilder.EMPTY);
    int num1 = (value.Length + (int) byte.MaxValue) / 256 /*0x0100*/;
    List<byte> byteList = new List<byte>();
    for (int index = 0; index < num1; ++index)
    {
      string str = value.Substring(index * 2, 2);
      byte num2;
      try
      {
        num2 = Convert.ToByte(str, 16 /*0x10*/);
      }
      catch (FormatException ex)
      {
        break;
      }
      catch (OverflowException ex)
      {
        break;
      }
      byteList.Add(num2);
    }
    return AsnKeyBuilder.CreateOctetString(byteList.ToArray());
  }

  /// <summary>
  /// <para>Returns the AsnType representing a ASN.1 encoded
  /// integer. The octets pass through this method are not modified.</para>
  /// <para>If octets is null or zero length, the method returns an
  /// AsnType equivalent to CreateInteger(byte[]{0})..</para>
  /// </summary>
  /// <param name="value">A MSB (big endian) byte[] representing the
  /// integer to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded integer.</returns>
  /// <example>
  /// ASN.1 encoded 0:
  /// <code>CreateInteger(null)</code>
  /// <code>CreateInteger(new byte[]{0x00})</code>
  /// <code>CreateInteger(new byte[]{0x00, 0x00})</code>
  /// </example>
  /// <example>
  /// ASN.1 encoded 1:
  /// <code>CreateInteger(new byte[]{0x01})</code>
  /// </example>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateIntegerPos(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateIntegerNeg(System.Byte[])" />
  internal static AsnKeyBuilder.AsnType CreateInteger(byte[] value)
  {
    return AsnKeyBuilder.IsEmpty(value) ? AsnKeyBuilder.CreateInteger(AsnKeyBuilder.ZERO) : new AsnKeyBuilder.AsnType((byte) 2, value);
  }

  /// <summary>
  /// <para>Returns the AsnType representing a positive ASN.1 encoded
  /// integer. If the high bit of most significant byte is set,
  /// the method prepends a 0x00 to octets before assigning the
  /// value to ensure the resulting integer is interpreted as
  /// positive in the application.</para>
  /// <para>If octets is null or zero length, the method returns an
  /// AsnType equivalent to CreateInteger(byte[]{0})..</para>
  /// </summary>
  /// <param name="value">A MSB (big endian) byte[] representing the
  /// integer to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded positive integer.</returns>
  /// <example>
  /// ASN.1 encoded 0:
  /// <code>CreateIntegerPos(null)</code>
  /// <code>CreateIntegerPos(new byte[]{0x00})</code>
  /// <code>CreateIntegerPos(new byte[]{0x00, 0x00})</code>
  /// </example>
  /// <example>
  /// ASN.1 encoded 1:
  /// <code>CreateInteger(new byte[]{0x01})</code>
  /// </example>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateInteger(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateIntegerNeg(System.Byte[])" />
  internal static AsnKeyBuilder.AsnType CreateIntegerPos(byte[] value)
  {
    byte[] numArray = AsnKeyBuilder.Duplicate(value);
    if (AsnKeyBuilder.IsEmpty(numArray))
      numArray = AsnKeyBuilder.ZERO;
    byte[] destinationArray;
    if (numArray.Length != 0 && numArray[0] > (byte) 127 /*0x7F*/)
    {
      destinationArray = new byte[numArray.Length + 1];
      destinationArray[0] = (byte) 0;
      Array.Copy((Array) numArray, 0, (Array) destinationArray, 1, value.Length);
    }
    else
      destinationArray = numArray;
    return AsnKeyBuilder.CreateInteger(destinationArray);
  }

  /// <summary>
  /// <para>Returns the negative ASN.1 encoded integer. If the high
  /// bit of most significant byte is set, the integer is already
  /// considered negative.</para>
  /// <para>If the high bit of most significant byte
  /// is <bold>not</bold> set, the integer will be 2's complimented
  /// to form a negative integer.</para>
  /// <para>If octets is null or zero length, the method returns an
  /// AsnType equivalent to CreateInteger(byte[]{0})..</para>
  /// </summary>
  /// <param name="value">A MSB (big endian) byte[] representing the
  /// integer to be encoded.</param>
  /// <returns>Returns the negative ASN.1 encoded integer.</returns>
  /// <example>
  /// ASN.1 encoded 0:
  /// <code>CreateIntegerNeg(null)</code>
  /// <code>CreateIntegerNeg(new byte[]{0x00})</code>
  /// <code>CreateIntegerNeg(new byte[]{0x00, 0x00})</code>
  /// </example>
  /// <example>
  /// ASN.1 encoded -1 (2's compliment 0xFF):
  /// <code>CreateIntegerNeg(new byte[]{0x01})</code>
  /// </example>
  /// <example>
  /// ASN.1 encoded -2 (2's compliment 0xFE):
  /// <code>CreateIntegerNeg(new byte[]{0x02})</code>
  /// </example>
  /// <example>
  /// ASN.1 encoded -1:
  /// <code>CreateIntegerNeg(new byte[]{0xFF})</code>
  /// <code>CreateIntegerNeg(new byte[]{0xFF,0xFF})</code>
  /// Note: already negative since the high bit is set.</example>
  /// <example>
  /// ASN.1 encoded -255 (2's compliment 0xFF, 0x01):
  /// <code>CreateIntegerNeg(new byte[]{0x00,0xFF})</code>
  /// </example>
  /// <example>
  /// ASN.1 encoded -255 (2's compliment 0xFF, 0xFF, 0x01):
  /// <code>CreateIntegerNeg(new byte[]{0x00,0x00,0xFF})</code>
  /// </example>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateInteger(System.Byte[])" />
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateIntegerPos(System.Byte[])" />
  internal static AsnKeyBuilder.AsnType CreateIntegerNeg(byte[] value)
  {
    if (AsnKeyBuilder.IsEmpty(value))
      return AsnKeyBuilder.CreateInteger(AsnKeyBuilder.ZERO);
    return AsnKeyBuilder.IsZero(value) || value[0] >= (byte) 128 /*0x80*/ ? AsnKeyBuilder.CreateInteger(value) : AsnKeyBuilder.CreateInteger(AsnKeyBuilder.Compliment2s(value));
  }

  /// <summary>
  /// Returns the AsnType representing an ASN.1 encoded null.
  /// </summary>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded null.</returns>
  internal static AsnKeyBuilder.AsnType CreateNull()
  {
    return new AsnKeyBuilder.AsnType((byte) 5, new byte[1]);
  }

  /// <summary>
  /// Removes leading 0x00 octets from the byte[] octets. This
  /// method may return an empty byte array (0 length).
  /// </summary>
  /// <param name="octets">An array of octets to trim.</param>
  /// <returns>A byte[] with leading 0x00 octets removed.</returns>
  internal static byte[] TrimStart(byte[] octets)
  {
    if (AsnKeyBuilder.IsEmpty(octets) || AsnKeyBuilder.IsZero(octets))
      return new byte[0];
    byte[] sourceArray = AsnKeyBuilder.Duplicate(octets);
    int sourceIndex = 0;
    byte[] numArray = sourceArray;
    for (int index = 0; index < numArray.Length && numArray[index] == (byte) 0; ++index)
      ++sourceIndex;
    if (sourceIndex == sourceArray.Length)
      return octets;
    byte[] destinationArray = new byte[sourceArray.Length - sourceIndex];
    Array.Copy((Array) sourceArray, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
    return destinationArray;
  }

  /// <summary>
  /// Removes trailing 0x00 octets from the byte[] octets. This
  /// method may return an empty byte array (0 length).
  /// </summary>
  /// <param name="octets">An array of octets to trim.</param>
  /// <returns>A byte[] with trailing 0x00 octets removed.</returns>
  internal static byte[] TrimEnd(byte[] octets)
  {
    if (AsnKeyBuilder.IsEmpty(octets) || AsnKeyBuilder.IsZero(octets))
      return AsnKeyBuilder.EMPTY;
    byte[] octets1 = AsnKeyBuilder.Duplicate(octets);
    Array.Reverse((Array) octets1);
    byte[] numArray = AsnKeyBuilder.TrimStart(octets1);
    Array.Reverse((Array) numArray);
    return numArray;
  }

  /// <summary>
  /// Returns the AsnType representing an ASN.1 encoded OID.
  /// If conversion fails, the result is a partial conversion
  /// up to the point of failure. If the oid string is null or
  /// not well formed, an empty byte[] is returned.
  /// </summary>
  /// <param name="value">The string representing the object
  /// identifier to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded object identifier.</returns>
  /// <example>The following assigns the encoded AsnType
  /// for a RSA key to oid:
  /// <code>AsnType oid = CreateOid("1.2.840.113549.1.1.1")</code>
  /// </example>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOid(System.Byte[])" />
  internal static AsnKeyBuilder.AsnType CreateOid(string value)
  {
    if (AsnKeyBuilder.IsEmpty(value))
      return (AsnKeyBuilder.AsnType) null;
    string[] strings = value.Split(' ', '.');
    if (AsnKeyBuilder.IsEmpty(strings))
      return (AsnKeyBuilder.AsnType) null;
    ulong num1 = 0;
    List<ulong> ulongList = new List<ulong>();
    foreach (string str in strings)
    {
      if (str.Length != 0)
      {
        try
        {
          num1 = Convert.ToUInt64(str, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
          break;
        }
        catch (OverflowException ex)
        {
          break;
        }
        ulongList.Add(num1);
      }
      else
        break;
    }
    if (ulongList.Count == 0)
      return (AsnKeyBuilder.AsnType) null;
    List<byte> byteList1 = new List<byte>();
    if (ulongList.Count >= 1)
      num1 = ulongList[0] * 40UL;
    if (ulongList.Count >= 2)
      num1 += ulongList[1];
    byteList1.Add((byte) num1);
    for (int index = 2; index < ulongList.Count; ++index)
    {
      List<byte> byteList2 = new List<byte>();
      ulong num2 = ulongList[index];
      do
      {
        byteList2.Add((byte) (128UL /*0x80*/ | num2 & (ulong) sbyte.MaxValue));
        num2 >>= 7;
      }
      while (num2 != 0UL);
      byte[] array = byteList2.ToArray();
      array[0] = (byte) ((uint) sbyte.MaxValue & (uint) array[0]);
      Array.Reverse((Array) array);
      foreach (byte num3 in array)
        byteList1.Add(num3);
    }
    return AsnKeyBuilder.CreateOid(byteList1.ToArray());
  }

  /// <summary>
  /// Returns the AsnType representing an ASN.1 encoded OID.
  /// If conversion fails, the result is a partial conversion
  /// (up to the point of failure). If octets is null, an
  /// empty byte[] is returned.
  /// </summary>
  /// <param name="value">The packed byte[] representing the object
  /// identifier to be encoded.</param>
  /// <returns>Returns the AsnType representing an ASN.1
  /// encoded object identifier.</returns>
  /// <example>The following assigns the encoded AsnType for a RSA
  /// key to oid:
  /// <code>// Packed 1.2.840.113549.1.1.1
  /// byte[] rsa = new byte[] { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };
  /// AsnType = CreateOid(rsa)</code>
  /// </example>
  /// <seealso cref="M:Patagames.Activation.AsnKeyBuilder.CreateOid(System.String)" />
  internal static AsnKeyBuilder.AsnType CreateOid(byte[] value)
  {
    return AsnKeyBuilder.IsEmpty(value) ? (AsnKeyBuilder.AsnType) null : new AsnKeyBuilder.AsnType((byte) 6, value);
  }

  private static byte[] Compliment1s(byte[] value)
  {
    if (AsnKeyBuilder.IsEmpty(value))
      return AsnKeyBuilder.EMPTY;
    byte[] numArray = AsnKeyBuilder.Duplicate(value);
    for (int index = numArray.Length - 1; index >= 0; --index)
      numArray[index] = ~numArray[index];
    return numArray;
  }

  private static byte[] Compliment2s(byte[] value)
  {
    if (AsnKeyBuilder.IsEmpty(value))
      return AsnKeyBuilder.EMPTY;
    if (AsnKeyBuilder.IsZero(value))
      return AsnKeyBuilder.Duplicate(value);
    byte[] sourceArray = AsnKeyBuilder.Duplicate(value);
    int num1 = 1;
    for (int index = sourceArray.Length - 1; index >= 0; --index)
    {
      sourceArray[index] = ~sourceArray[index];
      int num2 = (int) sourceArray[index] + num1;
      sourceArray[index] = (byte) (num2 & (int) byte.MaxValue);
      num1 = 256 /*0x0100*/ != (num2 & 256 /*0x0100*/) ? 0 : 1;
    }
    byte[] destinationArray;
    if (1 == num1)
    {
      destinationArray = new byte[sourceArray.Length + 1];
      destinationArray[0] = byte.MaxValue;
      Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 1, sourceArray.Length);
    }
    else
      destinationArray = sourceArray;
    return destinationArray;
  }

  private static byte[] Concatenate(AsnKeyBuilder.AsnType[] values)
  {
    if (AsnKeyBuilder.IsEmpty(values))
      return new byte[0];
    int length = 0;
    foreach (AsnKeyBuilder.AsnType asnType in values)
    {
      if (asnType != null)
        length += asnType.GetBytes().Length;
    }
    byte[] destinationArray = new byte[length];
    int destinationIndex = 0;
    foreach (AsnKeyBuilder.AsnType asnType in values)
    {
      if (asnType != null)
      {
        byte[] bytes = asnType.GetBytes();
        Array.Copy((Array) bytes, 0, (Array) destinationArray, destinationIndex, bytes.Length);
        destinationIndex += bytes.Length;
      }
    }
    return destinationArray;
  }

  private static byte[] Concatenate(byte[] first, byte[] second)
  {
    return AsnKeyBuilder.Concatenate(new byte[2][]
    {
      first,
      second
    });
  }

  private static byte[] Concatenate(byte[][] values)
  {
    if (AsnKeyBuilder.IsEmpty(values))
      return new byte[0];
    int length = 0;
    foreach (byte[] numArray in values)
    {
      if (numArray != null)
        length += numArray.Length;
    }
    byte[] destinationArray = new byte[length];
    int destinationIndex = 0;
    foreach (byte[] sourceArray in values)
    {
      if (sourceArray != null)
      {
        Array.Copy((Array) sourceArray, 0, (Array) destinationArray, destinationIndex, sourceArray.Length);
        destinationIndex += sourceArray.Length;
      }
    }
    return destinationArray;
  }

  private static byte[] Duplicate(byte[] b)
  {
    if (AsnKeyBuilder.IsEmpty(b))
      return AsnKeyBuilder.EMPTY;
    byte[] destinationArray = new byte[b.Length];
    Array.Copy((Array) b, (Array) destinationArray, b.Length);
    return destinationArray;
  }

  private static bool IsZero(byte[] octets)
  {
    if (AsnKeyBuilder.IsEmpty(octets))
      return false;
    bool flag = true;
    for (int index = 0; index < octets.Length; ++index)
    {
      if (octets[index] != (byte) 0)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private static bool IsEmpty(byte[] octets) => octets == null || octets.Length == 0;

  private static bool IsEmpty(string s) => s == null || s.Length == 0;

  private static bool IsEmpty(string[] strings) => strings == null || strings.Length == 0;

  private static bool IsEmpty(AsnKeyBuilder.AsnType value) => value == null;

  private static bool IsEmpty(AsnKeyBuilder.AsnType[] values)
  {
    return values == null || values.Length == 0;
  }

  private static bool IsEmpty(byte[][] arrays) => arrays == null || arrays.Length == 0;

  internal class AsnMessage
  {
    private byte[] m_octets;
    private string m_format;

    internal int Length => this.m_octets == null ? 0 : this.m_octets.Length;

    internal AsnMessage(byte[] octets, string format)
    {
      this.m_octets = octets;
      this.m_format = format;
    }

    internal byte[] GetBytes() => this.m_octets == null ? new byte[0] : this.m_octets;

    internal string GetFormat() => this.m_format;
  }

  internal class AsnType
  {
    private bool m_raw;
    private byte[] m_tag;
    private byte[] m_length;
    private byte[] m_octets;

    public AsnType(byte tag, byte octet)
    {
      this.m_raw = false;
      this.m_tag = new byte[1]{ tag };
      this.m_octets = new byte[1]{ octet };
    }

    public AsnType(byte tag, byte[] octets)
    {
      this.m_raw = false;
      this.m_tag = new byte[1]{ tag };
      this.m_octets = octets;
    }

    public AsnType(byte tag, byte[] length, byte[] octets)
    {
      this.m_raw = true;
      this.m_tag = new byte[1]{ tag };
      this.m_length = length;
      this.m_octets = octets;
    }

    private bool Raw
    {
      get => this.m_raw;
      set => this.m_raw = value;
    }

    public byte[] Tag => this.m_tag == null ? AsnKeyBuilder.EMPTY : this.m_tag;

    public byte[] Length => this.m_length == null ? AsnKeyBuilder.EMPTY : this.m_length;

    public byte[] Octets
    {
      get => this.m_octets == null ? AsnKeyBuilder.EMPTY : this.m_octets;
      set => this.m_octets = value;
    }

    internal byte[] GetBytes()
    {
      if (this.m_raw)
        return this.Concatenate(new byte[3][]
        {
          this.m_tag,
          this.m_length,
          this.m_octets
        });
      this.SetLength();
      return (byte) 5 == this.m_tag[0] ? this.Concatenate(new byte[2][]
      {
        this.m_tag,
        this.m_octets
      }) : this.Concatenate(new byte[3][]
      {
        this.m_tag,
        this.m_length,
        this.m_octets
      });
    }

    private void SetLength()
    {
      if (this.m_octets == null)
        this.m_length = AsnKeyBuilder.ZERO;
      else if ((byte) 5 == this.m_tag[0])
      {
        this.m_length = AsnKeyBuilder.EMPTY;
      }
      else
      {
        byte[] numArray;
        if (this.m_octets.Length < 128 /*0x80*/)
          numArray = new byte[1]
          {
            (byte) this.m_octets.Length
          };
        else if (this.m_octets.Length <= (int) byte.MaxValue)
          numArray = new byte[2]
          {
            (byte) 129,
            (byte) (this.m_octets.Length & (int) byte.MaxValue)
          };
        else if (this.m_octets.Length <= (int) ushort.MaxValue)
          numArray = new byte[3]
          {
            (byte) 130,
            (byte) ((this.m_octets.Length & 65280) >> 8),
            (byte) (this.m_octets.Length & (int) byte.MaxValue)
          };
        else if (this.m_octets.Length <= 16777215 /*0xFFFFFF*/)
          numArray = new byte[4]
          {
            (byte) 131,
            (byte) ((this.m_octets.Length & 16711680 /*0xFF0000*/) >> 16 /*0x10*/),
            (byte) ((this.m_octets.Length & 65280) >> 8),
            (byte) (this.m_octets.Length & (int) byte.MaxValue)
          };
        else
          numArray = new byte[5]
          {
            (byte) 132,
            (byte) (((long) this.m_octets.Length & 4278190080L /*0xFF000000*/) >> 24),
            (byte) ((this.m_octets.Length & 16711680 /*0xFF0000*/) >> 16 /*0x10*/),
            (byte) ((this.m_octets.Length & 65280) >> 8),
            (byte) (this.m_octets.Length & (int) byte.MaxValue)
          };
        this.m_length = numArray;
      }
    }

    private byte[] Concatenate(byte[][] values)
    {
      if (AsnKeyBuilder.IsEmpty(values))
        return new byte[0];
      int length = 0;
      foreach (byte[] numArray in values)
      {
        if (numArray != null)
          length += numArray.Length;
      }
      byte[] destinationArray = new byte[length];
      int destinationIndex = 0;
      foreach (byte[] sourceArray in values)
      {
        if (sourceArray != null)
        {
          Array.Copy((Array) sourceArray, 0, (Array) destinationArray, destinationIndex, sourceArray.Length);
          destinationIndex += sourceArray.Length;
        }
      }
      return destinationArray;
    }
  }
}
