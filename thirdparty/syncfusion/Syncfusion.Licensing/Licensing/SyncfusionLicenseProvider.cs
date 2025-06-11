// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.SyncfusionLicenseProvider
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing;

public class SyncfusionLicenseProvider : FusionLicenseProvider
{
  public static void RegisterLicense(string licenseKey)
  {
    if (string.IsNullOrEmpty(licenseKey))
      return;
    string[] separator = new string[2]{ ",", ";" };
    foreach (string str in licenseKey.Split(separator, StringSplitOptions.None))
    {
      if (!FusionLicenseProvider.registeredLicenses.Contains(str))
        FusionLicenseProvider.registeredLicenses.Add(str);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool ValidateLicense(Platform platform, out string message)
  {
    message = FusionLicenseProvider.GetLicenseType(platform);
    return string.IsNullOrEmpty(message);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool ValidateLicense(Platform platform)
  {
    return string.IsNullOrEmpty(FusionLicenseProvider.GetLicenseType(platform));
  }
}
