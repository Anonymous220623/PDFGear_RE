// Decompiled with JetBrains decompiler
// Type: FileWatcher.Program
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using CommomLib.Commom;
using CommomLib.Config;
using System;

#nullable disable
namespace FileWatcher;

public class Program
{
  [STAThread]
  public static void Main()
  {
    if (!SingleInstance.IsMainInstance || !SettingsHelper.IsEnabled)
      return;
    SqliteUtils.InitializeDatabase().GetAwaiter().GetResult();
    CultureInfoUtils.Initialize();
    App app = new App();
    app.InitializeComponent();
    app.Run();
  }
}
