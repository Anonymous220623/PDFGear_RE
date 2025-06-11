// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.MessageDigestAlgorithms
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class MessageDigestAlgorithms
{
  internal const string SHA1 = "SHA-1";
  internal const string SHA256 = "SHA-256";
  internal const string SHA384 = "SHA-384";
  internal const string SHA512 = "SHA-512";
  internal const string RIPEMD160 = "RIPEMD160";
  private readonly Dictionary<string, string> m_names = new Dictionary<string, string>();
  private readonly Dictionary<string, string> m_digests = new Dictionary<string, string>();
  private MessageDigestFinder m_finder = new MessageDigestFinder();

  internal MessageDigestAlgorithms()
  {
    this.m_names["1.2.840.113549.2.5"] = "MD5";
    this.m_names["1.3.14.3.2.26"] = nameof (SHA1);
    this.m_names["2.16.840.1.101.3.4.2.1"] = nameof (SHA256);
    this.m_names["2.16.840.1.101.3.4.2.2"] = nameof (SHA384);
    this.m_names["2.16.840.1.101.3.4.2.3"] = nameof (SHA512);
    this.m_names["1.3.36.3.2.1"] = nameof (RIPEMD160);
    this.m_names["1.2.840.113549.1.1.4"] = "MD5";
    this.m_names["1.2.840.113549.1.1.5"] = nameof (SHA1);
    this.m_names["1.2.840.113549.1.1.11"] = nameof (SHA256);
    this.m_names["1.2.840.113549.1.1.12"] = nameof (SHA384);
    this.m_names["1.2.840.113549.1.1.13"] = nameof (SHA512);
    this.m_names["1.2.840.113549.2.5"] = "MD5";
    this.m_names["1.2.840.10040.4.3"] = nameof (SHA1);
    this.m_names["2.16.840.1.101.3.4.3.2"] = nameof (SHA256);
    this.m_names["2.16.840.1.101.3.4.3.3"] = nameof (SHA384);
    this.m_names["2.16.840.1.101.3.4.3.4"] = nameof (SHA512);
    this.m_names["1.3.36.3.3.1.2"] = nameof (RIPEMD160);
    this.m_digests["MD5"] = "1.2.840.113549.2.5";
    this.m_digests["MD-5"] = "1.2.840.113549.2.5";
    this.m_digests[nameof (SHA1)] = "1.3.14.3.2.26";
    this.m_digests["SHA-1"] = "1.3.14.3.2.26";
    this.m_digests[nameof (SHA256)] = "2.16.840.1.101.3.4.2.1";
    this.m_digests["SHA-256"] = "2.16.840.1.101.3.4.2.1";
    this.m_digests[nameof (SHA384)] = "2.16.840.1.101.3.4.2.2";
    this.m_digests["SHA-384"] = "2.16.840.1.101.3.4.2.2";
    this.m_digests[nameof (SHA512)] = "2.16.840.1.101.3.4.2.3";
    this.m_digests["SHA-512"] = "2.16.840.1.101.3.4.2.3";
    this.m_digests[nameof (RIPEMD160)] = "1.3.36.3.2.1";
    this.m_digests["RIPEMD-160"] = "1.3.36.3.2.1";
  }

  internal IMessageDigest GetMessageDigest(string hashAlgorithm)
  {
    return this.m_finder.GetDigest(hashAlgorithm);
  }

  internal byte[] Digest(Stream data, string hashAlgorithm)
  {
    IMessageDigest messageDigest = this.GetMessageDigest(hashAlgorithm);
    return this.Digest(data, messageDigest);
  }

  internal byte[] Digest(Stream data, IMessageDigest messageDigest)
  {
    byte[] numArray = new byte[8192 /*0x2000*/];
    int length;
    while ((length = data.Read(numArray, 0, numArray.Length)) > 0)
      messageDigest.Update(numArray, 0, length);
    byte[] bytes = new byte[messageDigest.MessageDigestSize];
    messageDigest.DoFinal(bytes, 0);
    return bytes;
  }

  internal string GetDigest(string id)
  {
    string str;
    return this.m_names.TryGetValue(id, out str) ? str : id;
  }

  internal string GetAllowedDigests(string name)
  {
    string allowedDigests;
    this.m_digests.TryGetValue(name.ToUpperInvariant(), out allowedDigests);
    return allowedDigests;
  }

  internal byte[] Digest(string algorithm, byte[] bytes)
  {
    return this.Digest(this.m_finder.GetDigest(algorithm), bytes, 0, bytes.Length);
  }

  internal byte[] Digest(IMessageDigest digest, byte[] bytes, int offset, int length)
  {
    digest.Update(bytes, offset, length);
    byte[] bytes1 = new byte[digest.MessageDigestSize];
    digest.DoFinal(bytes1, 0);
    return bytes1;
  }
}
