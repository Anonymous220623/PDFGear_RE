// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Email.MapiFileDesc
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Utils.Email;

[StructLayout(LayoutKind.Sequential)]
public class MapiFileDesc
{
  public int reserved;
  public int flags;
  public int position;
  public string path;
  public string name;
  public IntPtr type;
}
