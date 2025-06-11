// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ShareUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.Utils.Email;
using System;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

public static class ShareUtils
{
  public static async Task ShowInFolderAsync(string file)
  {
    try
    {
      GAManager.SendEvent("FileShare", "ShareInFolder", "Count", 1L);
      FileInfo fileInfo = new FileInfo(file);
      if (!fileInfo.Exists)
        return;
      int num = await fileInfo.ShowInExplorerAsync() ? 1 : 0;
    }
    catch
    {
    }
  }

  public static async Task WindowsShareAsync(string file)
  {
  }

  public static async Task SendMailAsync(string file)
  {
    GAManager.SendEvent("FileShare", "SendMail", "Count", 1L);
    FileInfo fileInfo = new FileInfo(file);
    if (!fileInfo.Exists)
      return;
    await Task.Run((Action) (() =>
    {
      try
      {
        new EmailMessage()
        {
          Subject = (fileInfo.Name ?? ""),
          AttachmentFilePath = {
            fileInfo.FullName
          },
          Body = "Here is a document for you, please see the attachment for details.\r\n \r\nShared from PDFgear:\r\nhttps://www.pdfgear.com/"
        }.Send();
      }
      catch
      {
      }
    }));
  }
}
