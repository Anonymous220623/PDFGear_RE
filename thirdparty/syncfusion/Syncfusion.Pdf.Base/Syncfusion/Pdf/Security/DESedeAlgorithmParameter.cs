// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DESedeAlgorithmParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DESedeAlgorithmParameter : DataEncryptionParameter
{
  internal const int DesEdeKeyLength = 24;

  internal DESedeAlgorithmParameter(byte[] key)
    : base(DESedeAlgorithmParameter.FixKey(key, 0, key.Length))
  {
  }

  internal DESedeAlgorithmParameter(byte[] key, int keyOff, int keyLen)
    : base(DESedeAlgorithmParameter.FixKey(key, keyOff, keyLen))
  {
  }

  private static byte[] FixKey(byte[] key, int keyOff, int keyLen)
  {
    byte[] numArray = new byte[24];
    switch (keyLen)
    {
      case 16 /*0x10*/:
        Array.Copy((Array) key, keyOff, (Array) numArray, 0, 16 /*0x10*/);
        Array.Copy((Array) key, keyOff, (Array) numArray, 16 /*0x10*/, 8);
        break;
      case 24:
        Array.Copy((Array) key, keyOff, (Array) numArray, 0, 24);
        break;
      default:
        throw new ArgumentException("Bad length for DESede key");
    }
    return !DESedeAlgorithmParameter.CheckKey(numArray, 0, numArray.Length) ? numArray : throw new ArgumentException("Attempt to create weak DESede key");
  }

  internal static bool CheckKey(byte[] key, int offset, int length)
  {
    for (int offset1 = offset; offset1 < length; offset1 += 8)
    {
      if (DataEncryptionParameter.CheckKey(key, offset1))
        return true;
    }
    return false;
  }
}
