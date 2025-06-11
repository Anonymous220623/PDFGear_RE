// Decompiled with JetBrains decompiler
// Type: RegExt.Program
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using CommomLib.Commom;
using CommomLib.Config;
using RegExt.FileAssociations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace RegExt;

internal class Program
{
  private static void Main(string[] args)
  {
    bool flag1 = !((IEnumerable<string>) args).Contains<string>("-uninstall", (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    bool setAsDefault = ((IEnumerable<string>) args).Contains<string>("-setAsDefault", (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    bool flag2 = ((IEnumerable<string>) args).Contains<string>("-A54C4E30729A469DA8F0864FDB400881", (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    int num = ((IEnumerable<string>) args).Contains<string>("-stopwatcher", (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) ? 1 : 0;
    bool resetDefault = ((IEnumerable<string>) args).Contains<string>("-resetDefault", (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    bool flag3 = ((IEnumerable<string>) args).Contains<string>("-pinTaskBand", (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    SqliteUtils.InitializeDatabase().GetAwaiter().GetResult();
    if (num != 0)
      return;
    if (flag1)
    {
      if (flag2)
      {
        if (flag3)
          Program.PinAppToTaskbar(true);
        try
        {
          if ((DateTime.UtcNow - File.GetCreationTimeUtc(Path.Combine(AppDataHelper.LocalFolder, "pdfdata.db"))).TotalMinutes < 1.0)
            ConfigUtils.TrySet<Guid>("ID_A54C4E30729A469DA8F0864FDB400881", new Guid(3146596894U, (ushort) 32475, (ushort) 18483, (byte) 180, (byte) 111, (byte) 226, (byte) 20, (byte) 55, (byte) 169, (byte) 187, (byte) 135));
        }
        catch
        {
        }
        foreach (string name in args)
        {
          if (name.StartsWith("-name:"))
            ChannelHelper.SaveSetupName(name);
        }
      }
      Program.CreateProgId();
      Program.CreateFileExts(setAsDefault, resetDefault);
      DefaultAppHelper.Refresh();
    }
    else
    {
      if (flag3)
        Program.PinAppToTaskbar(false);
      FileWatcherHelper.Instance.IsEnabled = false;
      FileWatcherHelper.Instance.UpdateState(0);
      Program.RemoveProgId();
      SqliteUtils.InitializeDatabase().GetAwaiter().GetResult();
      ConfigManager.SetDefaultAppActionAsync((string) null);
      Program.PinAppToTaskbar(false);
    }
  }

  private static void PinAppToTaskbar(bool pinning)
  {
    string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDFLauncher.exe");
    ShellHelper.UpdateFilePinState(pinning, fileName, "PDFgear", "578ab678-3bcf-4410-8b82-675d5d214865");
  }

  private static void CreateProgId()
  {
    string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDFLauncher.exe");
    RegisterProgram registerProgram = new RegisterProgram("PdfGear.App.1")
    {
      Operation = "open",
      Command = $"\"{str}\" \"%1\"",
      FriendlyAppName = "PDFgear",
      AppUserModelID = "578ab678-3bcf-4410-8b82-675d5d214865",
      DefaultIcon = $"\"{str}\",1"
    };
    if (AppIdHelper.IsAdmin)
    {
      try
      {
        registerProgram.WriteToAllUser();
      }
      catch
      {
      }
    }
    try
    {
      registerProgram.WriteToCurrentUser();
    }
    catch
    {
    }
  }

  private static void RemoveProgId()
  {
    RegisterProgram registerProgram = new RegisterProgram("PdfGear.App.1");
    if (AppIdHelper.IsAdmin)
    {
      try
      {
        registerProgram.RemoveFromAllUser();
      }
      catch
      {
      }
    }
    try
    {
      registerProgram.RemoveFromCurrentUser();
    }
    catch
    {
    }
    DefaultAppHelper.Refresh();
  }

  private static void CreateFileExts(bool setAsDefault, bool resetDefault)
  {
    string[] defaultFileExts = AppManager.GetDefaultFileExts();
    if (defaultFileExts == null || defaultFileExts.Length == 0)
      return;
    RegisterFileExtension[] array = ((IEnumerable<string>) defaultFileExts).Select<string, RegisterFileExtension>((Func<string, RegisterFileExtension>) (c =>
    {
      return new RegisterFileExtension(c)
      {
        DefaultProgramId = "PdfGear.App.1",
        OpenWithProgramIds = {
          "PdfGear.App.1"
        }
      };
    })).ToArray<RegisterFileExtension>();
    string str;
    if (AppIdHelper.IsAdmin)
    {
      foreach (RegisterFileExtension registerFileExtension in array)
      {
        try
        {
          registerFileExtension.WriteToAllUser();
          if (setAsDefault)
          {
            if (resetDefault)
              DefaultAppHelper.ResetDefaultApp(registerFileExtension.FileExtension);
            else if (!SqliteUtils.TryGet("OriginVersion", out str))
              DefaultAppHelper.SetDefaultApp("PdfGear.App.1", registerFileExtension.FileExtension);
            else if (DefaultAppHelper.GetDefaultAppProgId(registerFileExtension.FileExtension) != "PdfGear.App.1")
            {
              if (AppIdHelper.HasUserChoiceLatest)
                AppIdHelper.RemoveOpenWithListAppFlag(registerFileExtension.FileExtension);
              else
                DefaultAppHelper.SetDefaultApp("PdfGear.App.1", registerFileExtension.FileExtension);
            }
          }
        }
        catch
        {
        }
      }
    }
    foreach (RegisterFileExtension registerFileExtension in array)
    {
      try
      {
        registerFileExtension.WriteToCurrentUser();
        if (setAsDefault)
        {
          if (resetDefault)
            DefaultAppHelper.ResetDefaultApp(registerFileExtension.FileExtension);
          else if (!SqliteUtils.TryGet("OriginVersion", out str))
            DefaultAppHelper.SetDefaultApp("PdfGear.App.1", registerFileExtension.FileExtension);
          else if (DefaultAppHelper.GetDefaultAppProgId(registerFileExtension.FileExtension) != "PdfGear.App.1")
          {
            if (AppIdHelper.HasUserChoiceLatest)
              AppIdHelper.RemoveOpenWithListAppFlag(registerFileExtension.FileExtension);
            else
              DefaultAppHelper.SetDefaultApp("PdfGear.App.1", registerFileExtension.FileExtension);
          }
        }
      }
      catch
      {
      }
    }
  }
}
