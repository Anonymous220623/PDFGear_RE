// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Utils
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class Utils
{
  private const int c_roundDecimals = 4;

  private Utils()
  {
  }

  public static string CheckFilePath(string path)
  {
    string path1 = path != null ? Path.GetFullPath(path) : throw new ArgumentNullException(nameof (path));
    return File.Exists(path1) ? path1 : throw new FileNotFoundException("File can't be found");
  }
}
