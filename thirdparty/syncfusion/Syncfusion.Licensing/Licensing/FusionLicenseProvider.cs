// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.FusionLicenseProvider
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public class FusionLicenseProvider
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool isDesignerModeValidated = false;
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool isAppDesignerMode = false;
  [EditorBrowsable(EditorBrowsableState.Never)]
  private static bool isBoldLicenseValidation = false;
  private static bool isLicenseExceptionShown = false;
  protected static List<string> registeredLicenses = new List<string>();
  private static bool isLicensed = false;
  private static bool isTrial = false;
  private static bool isDesigner = false;
  private static bool isSinglePlatform = true;
  private static string fileFormatExceptionMessage = string.Empty;
  private static bool isSyncfusionApp = false;
  private static string invalidVersion = string.Empty;
  [ThreadStatic]
  private static List<Platform> _Platforms;

  private static List<Platform> Platforms
  {
    get
    {
      if (FusionLicenseProvider._Platforms == null)
        FusionLicenseProvider._Platforms = new List<Platform>();
      return FusionLicenseProvider._Platforms;
    }
    set => FusionLicenseProvider._Platforms = value;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static string GetLicenseType(List<Platform> platformList)
  {
    FusionLicenseProvider.isSinglePlatform = false;
    if (platformList == null || platformList.Count <= 0)
      throw new ArgumentNullException("Platform", "Platform list should be passed.");
    FusionLicenseProvider.Platforms.AddRange((IEnumerable<Platform>) platformList);
    return FusionLicenseProvider.GetLicenseType(platformList[0]);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static string DesignMode => Process.GetCurrentProcess().ProcessName;

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool IsDesigner()
  {
    if (!FusionLicenseProvider.isDesigner && LicenseManager.UsageMode == LicenseUsageMode.Designtime)
      FusionLicenseProvider.isDesigner = true;
    return FusionLicenseProvider.isDesigner;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool IsDesignerMode()
  {
    return FusionLicenseProvider.DesignMode.ToLower() == "xdesproc" || FusionLicenseProvider.DesignMode.ToLower() == "devenv" || FusionLicenseProvider.DesignMode.ToLower() == "wpfsurface" || FusionLicenseProvider.DesignMode.ToLower().Contains("riderwpfpreviewerlauncher");
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static string GetLicenseType(Platform platform)
  {
    if (!FusionLicenseProvider.IsBoldLicenseValidation)
    {
      if (!FusionLicenseProvider.isDesignerModeValidated)
      {
        FusionLicenseProvider.isAppDesignerMode = FusionLicenseProvider.IsDesignerMode();
        FusionLicenseProvider.isDesignerModeValidated = true;
      }
      if (FusionLicenseProvider.IsDesigner() || FusionLicenseProvider.isAppDesignerMode || platform != Platform.ASPNET && FusionLicenseProvider.ValidateCallingFromSyncfusion())
        return (string) null;
      if (!FusionLicenseProvider.isSyncfusionApp && platform == Platform.Utility && FusionLicenseProvider.IsSyncfusionPFXSigned())
      {
        FusionLicenseProvider.isSyncfusionApp = true;
        return (string) null;
      }
      if (platform == Platform.FileFormats && !string.IsNullOrEmpty(FusionLicenseProvider.fileFormatExceptionMessage) && !FusionLicenseProvider.isSyncfusionApp)
      {
        if (FusionLicenseProvider.Platforms != null && FusionLicenseProvider.Platforms.Count > 0)
          FusionLicenseProvider.Platforms.Clear();
        FusionLicenseProvider.fileFormatExceptionMessage = Regex.Replace(FusionLicenseProvider.fileFormatExceptionMessage, "common/essential-studio", "file-formats", RegexOptions.IgnoreCase);
        return FusionLicenseProvider.fileFormatExceptionMessage;
      }
      if ((LicenseManager.UsageMode == LicenseUsageMode.Runtime && !FusionLicenseProvider.isLicenseExceptionShown || platform == Platform.FileFormats) && !FusionLicenseProvider.isSyncfusionApp)
      {
        if (FusionLicenseProvider.isSinglePlatform)
          FusionLicenseProvider.Platforms.Add(platform);
        FusionLicenseProvider.isSinglePlatform = true;
        if (!FusionLicenseProvider.isLicensed && !FusionLicenseProvider.isTrial || platform == Platform.FileFormats)
        {
          LicenseType modeOfLicense = FusionLicenseProvider.GetModeOfLicense(platform);
          FusionLicenseProvider.Platforms.Clear();
          if (modeOfLicense != LicenseType.Licensed && modeOfLicense != LicenseType.Trial)
          {
            DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) modeOfLicense.GetType().GetField(modeOfLicense.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
            if (customAttributes != null && customAttributes.Length > 0)
            {
              string input = customAttributes[0].Description.Replace("##LicenseVersion", FusionLicenseProvider.invalidVersion).Replace("##Requireversion", CoreAssembly.StudioCoreVersion);
              string replacement = platform.ToString().ToLower();
              switch (replacement)
              {
                case "fileformats":
                  replacement = "file-formats";
                  break;
                case "aspnetcore":
                  replacement = "aspnet-core";
                  break;
                case "universalwindows":
                  replacement = "uwp";
                  break;
              }
              string message = !replacement.Equals("blazor") ? Regex.Replace(input, "common/essential-studio", replacement, RegexOptions.IgnoreCase) : Regex.Replace(input, "help.syncfusion.com/common/essential-studio", "blazor.syncfusion.com/documentation", RegexOptions.IgnoreCase);
              if (platform != Platform.FileFormats)
                FusionLicenseProvider.isLicenseExceptionShown = true;
              if (platform == Platform.WPF || platform == Platform.WindowsForms)
              {
                int num = (int) new LicenseMessage().DisplayMessage("Syncfusion License", message, "help topic");
              }
              if (!string.IsNullOrEmpty(message) && string.IsNullOrEmpty(FusionLicenseProvider.fileFormatExceptionMessage))
                FusionLicenseProvider.fileFormatExceptionMessage = message;
              return message;
            }
          }
        }
        FusionLicenseProvider.Platforms.Clear();
      }
    }
    return (string) null;
  }

  private static bool ValidateCallingFromSyncfusion()
  {
    StackFrame[] frames = new StackTrace().GetFrames();
    try
    {
      string str = (string) null;
      for (int index = 0; index < frames.Length; ++index)
      {
        if (frames[index] != null && frames[index].GetMethod().DeclaringType.Assembly != (Assembly) null)
        {
          Assembly assembly = frames[index].GetMethod().DeclaringType.Assembly;
          if (!assembly.FullName.ToLower().Contains("syncfusion.licensing") && !string.IsNullOrEmpty(str) && str != assembly.FullName)
            return FusionLicenseProvider.IsAssemblySignedWithSfKey(assembly);
          if (!assembly.FullName.ToLower().Contains("syncfusion.licensing"))
            str = assembly.FullName;
        }
      }
    }
    catch (Exception ex)
    {
    }
    return false;
  }

  private static LicenseType GetModeOfLicense(Platform platform)
  {
    try
    {
      string studioCoreVersion = CoreAssembly.StudioCoreVersion;
      if (FusionLicenseProvider.registeredLicenses.Count == 0)
        return LicenseType.NoLicense;
      List<LicenseType> licenseTypeList = new List<LicenseType>();
      for (int index = 0; index < FusionLicenseProvider.registeredLicenses.Count; ++index)
        licenseTypeList.Add(FusionLicenseProvider.GetProjectKeyType(FusionLicenseProvider.registeredLicenses[index].Trim(), studioCoreVersion));
      if (licenseTypeList.Contains(LicenseType.Licensed))
      {
        FusionLicenseProvider.isLicensed = true;
        return LicenseType.Licensed;
      }
      if (licenseTypeList.Contains(LicenseType.Trial))
      {
        FusionLicenseProvider.isTrial = true;
        return LicenseType.Trial;
      }
      if (licenseTypeList.Contains(LicenseType.TrialExpired))
        return LicenseType.TrialExpired;
      if (licenseTypeList.Contains(LicenseType.PlatformMismatch))
        return LicenseType.PlatformMismatch;
      if (licenseTypeList.Contains(LicenseType.VersionMismatch))
        return LicenseType.VersionMismatch;
      if (licenseTypeList.Contains(LicenseType.WrongKey))
        return LicenseType.WrongKey;
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
      return LicenseType.WrongKey;
    }
    return LicenseType.Unlicensed;
  }

  private static bool IsSyncfusionPFXSigned()
  {
    try
    {
      X509Certificate fromSignedFile = X509Certificate.CreateFromSignedFile(Application.ExecutablePath);
      return fromSignedFile != null && fromSignedFile.Subject != null && fromSignedFile.Subject.Contains("CN=\"SYNCFUSION, INC.\"");
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  private static bool IsAssemblySignedWithSfKey(Assembly assembly)
  {
    try
    {
      byte[] publicKeyToken = assembly.GetName().GetPublicKeyToken();
      string empty = string.Empty;
      for (int index = 0; index < publicKeyToken.GetLength(0); ++index)
        empty += $"{publicKeyToken[index]:x2}";
      return empty == "3d67ed1f87d44c89" || empty == "632609b4d040f6b4";
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  private static bool IsSampleBrowser(Assembly assembly)
  {
    try
    {
      return new Regex("SampleBrowser.*,\\s*Version=[\\d]{1,3}.[\\d]{1}.[\\d]{1}.[\\d]{1,4},").IsMatch(assembly.FullName.ToString());
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  private static LicenseType GetProjectKeyType(string key, string version)
  {
    try
    {
      string key1 = "@" + FusionLicenseProvider.Base64Decode(key).Split('@')[1];
      bool flag1 = false;
      UnlockKeyInfo unlockKeyInfo = new UnlockKeyInfo(key1);
      bool flag2 = false;
      bool flag3 = false;
      for (int index1 = 0; index1 < FusionLicenseProvider.Platforms.Count; ++index1)
      {
        string str = FusionLicenseProvider.Platforms[index1].ToString().ToLower();
        if (FusionLicenseProvider.Platforms[index1] == Platform.UWP)
          str = "windowsappsplatform";
        if (FusionLicenseProvider.Platforms[index1] == Platform.FileFormats)
          flag2 = true;
        for (int index2 = 0; index2 < unlockKeyInfo.assemblies.Length; ++index2)
        {
          if (unlockKeyInfo.assemblies[index2].Contains("fileformats"))
            flag3 = true;
          if (Regex.IsMatch(unlockKeyInfo.assemblies[index2], $"\\b{str} \\b"))
          {
            flag1 = true;
            break;
          }
        }
      }
      if (unlockKeyInfo.encodedVersion != version.Remove(version.LastIndexOf('.') + 1, version.Length - version.LastIndexOf('.') - 1) + "*")
      {
        FusionLicenseProvider.invalidVersion = unlockKeyInfo.encodedVersion;
        return LicenseType.VersionMismatch;
      }
      if (!flag1 && !flag2 && !flag3)
        return LicenseType.PlatformMismatch;
      string keyInfo = unlockKeyInfo.ToString();
      return keyInfo.Contains("Evaluation Expiry Date") ? FusionLicenseProvider.ValidateExpiryDate(keyInfo) : LicenseType.Licensed;
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
      return LicenseType.WrongKey;
    }
  }

  private static LicenseType ValidateExpiryDate(string keyInfo)
  {
    string pattern = "Evaluation Expiry Date -.*?(\r\n|\n\n)";
    MatchCollection matchCollection = Regex.Matches(keyInfo, pattern);
    return matchCollection.Count > 0 && DateTime.Today > DateTime.Parse(matchCollection[0].Value.ToString().Replace("Evaluation Expiry Date - ", "")) ? LicenseType.TrialExpired : LicenseType.Trial;
  }

  private static string Base64Encode(string projectKey)
  {
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(projectKey));
  }

  private static string Base64Decode(string base64EncodedData)
  {
    return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public static bool IsBoldLicenseValidation
  {
    get => FusionLicenseProvider.isBoldLicenseValidation;
    set => FusionLicenseProvider.isBoldLicenseValidation = value;
  }
}
