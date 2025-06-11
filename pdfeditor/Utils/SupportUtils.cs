// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.SupportUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Views;
using System;
using System.Diagnostics;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

internal class SupportUtils
{
  public static void ShowFeedbackWindow(string file = "")
  {
    FeedbackWindow feedbackWindow = new FeedbackWindow();
    feedbackWindow.HideFaq();
    feedbackWindow.Owner = Application.Current.MainWindow;
    feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    if (!string.IsNullOrWhiteSpace(file))
    {
      feedbackWindow.flist.Add(file);
      feedbackWindow.showAttachmentCB(true);
    }
    feedbackWindow.ShowDialog();
  }

  public static void SendEmailFeedback()
  {
    try
    {
      string str = $"mailto:{"pdfreadersustain@outlook.com"}?{$"subject=[{UtilManager.GetProductName()}] [{UtilManager.GetAppVersion()}] Support"}";
      Process.Start(new ProcessStartInfo()
      {
        FileName = str,
        UseShellExecute = true,
        CreateNoWindow = true
      });
    }
    catch (Exception ex)
    {
    }
  }
}
