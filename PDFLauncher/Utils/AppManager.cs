// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Utils.AppManager
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System.Windows;

#nullable disable
namespace PDFLauncher.Utils;

public static class AppManager
{
  public static void ShowRecoverWindows()
  {
    RecoverWindow recoverWindow = new RecoverWindow();
    if (recoverWindow.VM.RecoverFileList.Count == 0)
      return;
    recoverWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    recoverWindow.ShowDialog();
  }
}
