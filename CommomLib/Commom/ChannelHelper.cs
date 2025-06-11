// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ChannelHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config;

#nullable disable
namespace CommomLib.Commom;

public static class ChannelHelper
{
  public static void SaveSetupName(string name)
  {
    if (string.IsNullOrEmpty(name))
      name = "";
    if (name.StartsWith("-name:"))
      name = name.Substring(6);
    name = name.Trim('"');
    ConfigUtils.TrySet<string>("setup-name", name);
  }

  public static string GetSetupName()
  {
    string str;
    return ConfigUtils.TryGet<string>("setup-name", out str) ? str : "";
  }

  public static string GetLinkArgs() => "";
}
