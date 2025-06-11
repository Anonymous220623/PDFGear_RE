// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AnnotationAuthorUtil
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;

#nullable disable
namespace pdfeditor.Utils;

public static class AnnotationAuthorUtil
{
  public static string GetAuthorName()
  {
    string annotationAuthorName = ConfigManager.GetAnnotationAuthorName();
    return !string.IsNullOrEmpty(annotationAuthorName) ? annotationAuthorName : Environment.UserName;
  }

  public static void SetAuthorName(string authorName)
  {
    ConfigManager.SetAnnotationAuthorName(authorName ?? string.Empty);
  }
}
