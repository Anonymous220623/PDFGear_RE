// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveCapabilitiesHelpers
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using Microsoft.Win32;
using System;

#nullable disable
namespace NAudio.Wave;

public static class WaveCapabilitiesHelpers
{
  public static readonly Guid MicrosoftDefaultManufacturerId = new Guid("d5a47fa8-6d98-11d1-a21a-00a0c9223196");
  public static readonly Guid DefaultWaveOutGuid = new Guid("E36DC310-6D9A-11D1-A21A-00A0C9223196");
  public static readonly Guid DefaultWaveInGuid = new Guid("E36DC311-6D9A-11D1-A21A-00A0C9223196");

  public static string GetNameFromGuid(Guid guid)
  {
    string nameFromGuid = (string) null;
    using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\MediaCategories"))
    {
      using (RegistryKey registryKey2 = registryKey1.OpenSubKey(guid.ToString("B")))
      {
        if (registryKey2 != null)
          nameFromGuid = registryKey2.GetValue("Name") as string;
      }
    }
    return nameFromGuid;
  }
}
