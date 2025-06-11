// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.RegistryWOW6432
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf;
using System;
using System.Text;

#nullable disable
namespace Patagames.Activation;

internal static class RegistryWOW6432
{
  public static T GetRegKey64<T>(UIntPtr inHive, string inKeyName, string inPropertyName)
  {
    return RegistryWOW6432.GetRegKey<T>(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName);
  }

  public static T GetRegKey32<T>(UIntPtr inHive, string inKeyName, string inPropertyName)
  {
    return RegistryWOW6432.GetRegKey<T>(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName);
  }

  private static T GetRegKey<T>(
    UIntPtr inHive,
    string inKeyName,
    RegSAM in32or64key,
    string inPropertyName)
  {
    int phkResult = 0;
    try
    {
      if (Platform.RegOpenKeyEx(inHive, inKeyName, 0U, (int) (RegSAM.QueryValue | in32or64key), out phkResult) != 0U)
        return default (T);
      uint lpType = 0;
      uint lpcbData = 0;
      Platform.RegQueryValueEx(phkResult, inPropertyName, 0, ref lpType, (byte[]) null, ref lpcbData);
      if (typeof (T) == typeof (string))
      {
        StringBuilder lpData = new StringBuilder((int) lpcbData);
        Platform.RegQueryValueEx(phkResult, inPropertyName, 0, ref lpType, lpData, ref lpcbData);
        return (T) Convert.ChangeType((object) lpData.ToString(), typeof (T));
      }
      if (!(typeof (T) == typeof (byte[])))
        throw new NotSupportedException();
      byte[] lpData1 = new byte[(int) lpcbData];
      Platform.RegQueryValueEx(phkResult, inPropertyName, 0, ref lpType, lpData1, ref lpcbData);
      return (T) Convert.ChangeType((object) lpData1, typeof (T));
    }
    finally
    {
      if (phkResult != 0)
      {
        int num = (int) Platform.RegCloseKey(phkResult);
      }
    }
  }
}
