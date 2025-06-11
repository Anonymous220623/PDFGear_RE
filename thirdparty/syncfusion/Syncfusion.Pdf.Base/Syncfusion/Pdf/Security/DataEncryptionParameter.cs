// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DataEncryptionParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DataEncryptionParameter : KeyParameter
{
  internal const int DataEncryptionKeyLength = 8;
  private const int DataEncryptionWeekKeysCount = 16 /*0x10*/;
  private static readonly byte[] DataEncryptionWeekKeys = new byte[128 /*0x80*/]
  {
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 31 /*0x1F*/,
    (byte) 31 /*0x1F*/,
    (byte) 31 /*0x1F*/,
    (byte) 31 /*0x1F*/,
    (byte) 14,
    (byte) 14,
    (byte) 14,
    (byte) 14,
    (byte) 224 /*0xE0*/,
    (byte) 224 /*0xE0*/,
    (byte) 224 /*0xE0*/,
    (byte) 224 /*0xE0*/,
    (byte) 241,
    (byte) 241,
    (byte) 241,
    (byte) 241,
    (byte) 254,
    (byte) 254,
    (byte) 254,
    (byte) 254,
    (byte) 254,
    (byte) 254,
    (byte) 254,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 31 /*0x1F*/,
    (byte) 224 /*0xE0*/,
    (byte) 31 /*0x1F*/,
    (byte) 224 /*0xE0*/,
    (byte) 14,
    (byte) 241,
    (byte) 14,
    (byte) 241,
    (byte) 1,
    (byte) 224 /*0xE0*/,
    (byte) 1,
    (byte) 224 /*0xE0*/,
    (byte) 1,
    (byte) 241,
    (byte) 1,
    (byte) 241,
    (byte) 31 /*0x1F*/,
    (byte) 254,
    (byte) 31 /*0x1F*/,
    (byte) 254,
    (byte) 14,
    (byte) 254,
    (byte) 14,
    (byte) 254,
    (byte) 1,
    (byte) 31 /*0x1F*/,
    (byte) 1,
    (byte) 31 /*0x1F*/,
    (byte) 1,
    (byte) 14,
    (byte) 1,
    (byte) 14,
    (byte) 224 /*0xE0*/,
    (byte) 254,
    (byte) 224 /*0xE0*/,
    (byte) 254,
    (byte) 241,
    (byte) 254,
    (byte) 241,
    (byte) 254,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 1,
    (byte) 254,
    (byte) 1,
    (byte) 224 /*0xE0*/,
    (byte) 31 /*0x1F*/,
    (byte) 224 /*0xE0*/,
    (byte) 31 /*0x1F*/,
    (byte) 241,
    (byte) 14,
    (byte) 241,
    (byte) 14,
    (byte) 224 /*0xE0*/,
    (byte) 1,
    (byte) 224 /*0xE0*/,
    (byte) 1,
    (byte) 241,
    (byte) 1,
    (byte) 241,
    (byte) 1,
    (byte) 254,
    (byte) 31 /*0x1F*/,
    (byte) 254,
    (byte) 31 /*0x1F*/,
    (byte) 254,
    (byte) 14,
    (byte) 254,
    (byte) 14,
    (byte) 31 /*0x1F*/,
    (byte) 1,
    (byte) 31 /*0x1F*/,
    (byte) 1,
    (byte) 14,
    (byte) 1,
    (byte) 14,
    (byte) 1,
    (byte) 254,
    (byte) 224 /*0xE0*/,
    (byte) 254,
    (byte) 224 /*0xE0*/,
    (byte) 254,
    (byte) 241,
    (byte) 254,
    (byte) 241
  };

  internal DataEncryptionParameter(byte[] keys)
    : base(keys)
  {
    if (DataEncryptionParameter.CheckKey(keys, 0))
      throw new ArgumentException("Invalid Data Encryption keys creation");
  }

  internal DataEncryptionParameter(byte[] keys, int offset, int length)
    : base(keys, offset, length)
  {
    if (DataEncryptionParameter.CheckKey(keys, offset))
      throw new ArgumentException("Invalid Data Encryption keys creation");
  }

  internal static bool CheckKey(byte[] bytes, int offset)
  {
    if (bytes.Length - offset < 8)
      throw new ArgumentException("Invalid length in bytes");
    for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
    {
      bool flag = false;
      for (int index2 = 0; index2 < 8; ++index2)
      {
        if ((int) bytes[index2 + offset] != (int) DataEncryptionParameter.DataEncryptionWeekKeys[index1 * 8 + index2])
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return true;
    }
    return false;
  }
}
