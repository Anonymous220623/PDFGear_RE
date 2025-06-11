// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ParamUtility
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ParamUtility
{
  private readonly IDictionary m_algorithms = (IDictionary) new Hashtable();

  internal ParamUtility()
  {
    this.AddAlgorithm("DESEDE", (object) "DESEDEWRAP", (object) "TDEA", (object) new DerObjectID("1.3.14.3.2.17"), (object) PKCSOIDs.IdAlgCms3DesWrap);
    this.AddAlgorithm("DESEDE3", (object) PKCSOIDs.DesEde3Cbc);
    this.AddAlgorithm("RC2", (object) PKCSOIDs.RC2Cbc, (object) PKCSOIDs.IdAlgCmsRC2Wrap);
  }

  private void AddAlgorithm(string name, params object[] objects)
  {
    this.m_algorithms[(object) name] = (object) name;
    foreach (object obj in objects)
      this.m_algorithms[(object) obj.ToString()] = (object) name;
  }

  internal KeyParameter CreateKeyParameter(string algorithm, byte[] bytes, int offset, int length)
  {
    switch ((string) this.m_algorithms[(object) algorithm.ToUpperInvariant()])
    {
      case null:
        throw new Exception($"Invalid entry. Algorithm {algorithm} not recognised.");
      case "DES":
        return (KeyParameter) new DataEncryptionParameter(bytes, offset, length);
      case "DESEDE":
      case "DESEDE3":
        return (KeyParameter) new DESedeAlgorithmParameter(bytes, offset, length);
      default:
        return new KeyParameter(bytes, offset, length);
    }
  }
}
