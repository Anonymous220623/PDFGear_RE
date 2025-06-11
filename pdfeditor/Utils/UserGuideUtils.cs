// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.UserGuideUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.IO;

#nullable disable
namespace pdfeditor.Utils;

public static class UserGuideUtils
{
  private static readonly string userGuidePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Doc", "User Guide.pdf");

  public static void OpenUserGuide()
  {
  }
}
