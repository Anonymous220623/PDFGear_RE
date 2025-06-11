// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Print.PrintService
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Newtonsoft.Json;
using System;

#nullable disable
namespace pdfeditor.Models.Print;

public static class PrintService
{
  public static PrintSettings LoadSettings(string deviceId)
  {
    string key = "device-" + deviceId;
    try
    {
      string printSetting = ConfigManager.GetPrintSetting(key);
      if (!string.IsNullOrEmpty(printSetting))
        return JsonConvert.DeserializeObject<PrintSettings>(printSetting);
    }
    catch (Exception ex)
    {
    }
    return (PrintSettings) null;
  }

  public static void SaveSettings(PrintSettings settings)
  {
    string key = "device-" + settings.Device;
    try
    {
      string str = JsonConvert.SerializeObject((object) settings);
      if (string.IsNullOrEmpty(str))
        return;
      ConfigManager.SetPrintSetting(str, key);
    }
    catch (Exception ex)
    {
    }
  }
}
