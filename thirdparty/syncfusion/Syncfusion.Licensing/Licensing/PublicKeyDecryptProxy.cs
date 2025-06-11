// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.PublicKeyDecryptProxy
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;

#nullable disable
namespace Syncfusion.Licensing;

internal class PublicKeyDecryptProxy
{
  private static Type typeOfPublicKeyDecrypt;

  public static byte[] SyncfusionDecode(string key) => PublicKeyDecrypt.SyncfusionDecode(key);
}
