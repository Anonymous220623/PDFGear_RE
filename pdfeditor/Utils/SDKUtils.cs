// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.SDKUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.IO;

#nullable disable
namespace pdfeditor.Utils;

public class SDKUtils
{
  internal static string GetLinceseKey()
  {
    return "EEF63308-0101E607-06160B50-44464955-4D5F434F-52501A00-62657474-65726170-70733230-3231406F-75746C6F-6F6B2E63-6F6D4000-4B8EEDCF-5B8A5E13-C956C329-692517EB-191FC5A4-52636192-14663A1E-D5F1505F-5AB670EE-75435E62-E5C9F073-8078F8AF-F5407B48-4EF2B17A-EEA5B257-F174E23D";
  }

  internal static string GetLibPath()
  {
    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfium.dll");
    return !string.IsNullOrWhiteSpace(path) && File.Exists(path) ? path : "";
  }
}
