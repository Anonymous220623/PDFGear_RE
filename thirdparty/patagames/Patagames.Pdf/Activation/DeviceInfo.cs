// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.DeviceInfo
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf;
using System;

#nullable disable
namespace Patagames.Activation;

internal sealed class DeviceInfo
{
  private static DeviceInfo _Instance;

  public static DeviceInfo Instance
  {
    get
    {
      if (DeviceInfo._Instance == null)
        DeviceInfo._Instance = new DeviceInfo();
      return DeviceInfo._Instance;
    }
  }

  public string Id
  {
    get
    {
      uint volumeSerialNumber = 0;
      uint num1 = 0;
      uint num2 = 0;
      uint num3 = 0;
      uint num4 = 0;
      string systemDirectory = Environment.SystemDirectory;
      if (systemDirectory != null && systemDirectory.Length > 0)
        Platform.GetVolumeInformation($"{systemDirectory[0]}:\\", (string) null, 0, out volumeSerialNumber, out uint _, out FileSystemFeature _, (string) null, 0);
      string g = RegistryWOW6432.GetRegKey32<string>(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Cryptography", "MachineGuid");
      if (g == null || g.Trim() == "")
        g = RegistryWOW6432.GetRegKey64<string>(RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Cryptography", "MachineGuid");
      if (g != null)
      {
        byte[] byteArray = new Guid(g).ToByteArray();
        if (byteArray != null)
        {
          num1 = BitConverter.ToUInt32(byteArray, 0);
          num2 = BitConverter.ToUInt32(byteArray, 4);
          num3 = BitConverter.ToUInt32(byteArray, 8);
          num4 = BitConverter.ToUInt32(byteArray, 12);
        }
      }
      return $"{volumeSerialNumber}-{num1}-{num2}-{num3}-{num4}";
    }
  }
}
