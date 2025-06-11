// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.CreateFileHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace pdfeditor.Utils;

public static class CreateFileHelper
{
  public static string CreateBlankPageAsync()
  {
    SizeF sizeF = new SizeF(595f, 842f);
    using (PdfDocument pdfDocument = PdfDocument.CreateNew())
    {
      pdfDocument.Pages.InsertPageAt(0, sizeF.Width, sizeF.Height);
      pdfDocument.Pages[0].GenerateContent();
      pdfDocument.Producer = "PDF Gear";
      string str = Path.Combine(UtilManager.GetTemporaryPath(), "Documents");
      if (!Directory.Exists(str))
        Directory.CreateDirectory(str);
      int num = 0;
      string path;
      do
      {
        ++num;
        string path2 = $"Untitled{num}" + ".pdf";
        path = Path.Combine(str, path2);
      }
      while (File.Exists(path));
      if (Directory.Exists(str))
      {
        try
        {
          using (FileStream fileStream = File.OpenWrite(path))
          {
            fileStream.Seek(0L, SeekOrigin.Begin);
            pdfDocument.Save((Stream) fileStream, SaveFlags.NoIncremental);
            fileStream.SetLength(fileStream.Position);
          }
          GAManager.SendEvent("PageView", "PageEditorCreateBlankCmd", "Success", 1L);
          return path;
        }
        catch
        {
          return "";
        }
      }
    }
    return "";
  }

  public static void OpenPDFFile(string file, string action = null)
  {
    string str1 = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/', ' ');
    string fullName = Directory.GetParent(str1).FullName;
    string cmd = Path.Combine(str1, "pdfeditor.exe");
    string str2 = $"\"{file}\"";
    if (!string.IsNullOrEmpty(action))
      str2 = $"{str2} -action {action.Trim()}";
    string Parameters = str2;
    ProcessManager.RunProcess(cmd, Parameters);
  }
}
