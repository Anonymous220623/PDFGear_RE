// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.StartupTaskHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Microsoft.Win32;

#nullable disable
namespace CommomLib.Commom;

public class StartupTaskHelper
{
  private bool isStartupTaskEnabled;

  public StartupTaskHelper() => this.Refresh();

  public bool IsStartupTaskEnabled
  {
    get => this.isStartupTaskEnabled;
    private set => this.isStartupTaskEnabled = value;
  }

  public void Enable()
  {
    try
    {
      string rootDirectoryFile = AppManager.GetRootDirectoryFile("FileWatcher.exe");
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        registryKey?.SetValue("PDFgear", (object) $"\"{rootDirectoryFile}\"");
      this.IsStartupTaskEnabled = true;
    }
    catch
    {
    }
  }

  public void Disable()
  {
    try
    {
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        registryKey.DeleteValue("PDFgear", false);
      this.IsStartupTaskEnabled = false;
    }
    catch
    {
    }
  }

  public void Toggle()
  {
    if (this.IsStartupTaskEnabled)
      this.Disable();
    else
      this.Enable();
  }

  public void Refresh()
  {
    try
    {
      string rootDirectoryFile = AppManager.GetRootDirectoryFile("FileWatcher.exe");
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
      {
        if (registryKey.GetValue("PDFgear", (object) null) is string str)
        {
          if (rootDirectoryFile != str)
            registryKey.SetValue("PDFgear", (object) $"\"{rootDirectoryFile}\"");
          this.IsStartupTaskEnabled = true;
        }
        else
          this.IsStartupTaskEnabled = false;
      }
    }
    catch
    {
    }
  }
}
