// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Email.MapiRecipDesc
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Utils.Email;

[StructLayout(LayoutKind.Sequential)]
public class MapiRecipDesc
{
  public int reserved;
  public int recipClass;
  public string name;
  public string address;
  public int eIDSize;
  public IntPtr entryID;
}
