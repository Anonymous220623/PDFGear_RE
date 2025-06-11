// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EncryptionAlgorithms
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EncryptionAlgorithms
{
  private readonly Dictionary<string, string> algorithmNames = new Dictionary<string, string>();

  internal EncryptionAlgorithms()
  {
    this.algorithmNames["1.2.840.113549.1.1.1"] = "RSA";
    this.algorithmNames["1.2.840.10040.4.1"] = "DSA";
    this.algorithmNames["1.2.840.113549.1.1.2"] = "RSA";
    this.algorithmNames["1.2.840.113549.1.1.4"] = "RSA";
    this.algorithmNames["1.2.840.113549.1.1.5"] = "RSA";
    this.algorithmNames["1.2.840.113549.1.1.14"] = "RSA";
    this.algorithmNames["1.2.840.113549.1.1.11"] = "RSA";
    this.algorithmNames["1.2.840.113549.1.1.12"] = "RSA";
    this.algorithmNames["1.2.840.113549.1.1.13"] = "RSA";
    this.algorithmNames["1.2.840.10040.4.3"] = "DSA";
    this.algorithmNames["2.16.840.1.101.3.4.3.1"] = "DSA";
    this.algorithmNames["2.16.840.1.101.3.4.3.2"] = "DSA";
    this.algorithmNames["1.3.14.3.2.29"] = "RSA";
    this.algorithmNames["1.3.36.3.3.1.2"] = "RSA";
    this.algorithmNames["1.3.36.3.3.1.3"] = "RSA";
    this.algorithmNames["1.3.36.3.3.1.4"] = "RSA";
    this.algorithmNames["1.2.643.2.2.19"] = "ECGOST3410";
    this.algorithmNames["1.2.840.113549.1.1.10"] = "RSAandMGF1";
    this.algorithmNames["1.2.840.10045.2.1"] = "ECDSA";
    this.algorithmNames["1.2.840.10045.4.1"] = "ECDSA";
    this.algorithmNames["1.2.840.10045.4.3.1"] = "ECDSA";
    this.algorithmNames["1.2.840.10045.4.3.2"] = "ECDSA";
    this.algorithmNames["1.2.840.10045.4.3.3"] = "ECDSA";
    this.algorithmNames["1.2.840.10045.4.3.4"] = "ECDSA";
  }

  internal string GetAlgorithm(string oid)
  {
    string str;
    return this.algorithmNames.TryGetValue(oid, out str) ? str : oid;
  }
}
