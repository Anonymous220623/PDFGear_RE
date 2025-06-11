// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.CreateNewFileUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.IO;

#nullable disable
namespace CommomLib.Commom;

public static class CreateNewFileUtils
{
  public static void CreateBlankPDFByEditor()
  {
    string str1 = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/', ' ');
    string fullName = Directory.GetParent(str1).FullName;
    string cmd = Path.Combine(str1, "pdfeditor.exe");
    string str2 = "CreateNewFile";
    string str3 = "new:CreatedFile";
    if (!string.IsNullOrEmpty(str3))
      str2 = $"{str2} -action {str3.Trim()}";
    string Parameters = str2;
    ProcessManager.RunProcess(cmd, Parameters);
  }
}
