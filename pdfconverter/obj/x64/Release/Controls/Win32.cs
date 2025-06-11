// Decompiled with JetBrains decompiler
// Type: pdfconverter.Controls.Win32
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Runtime.InteropServices;

#nullable disable
namespace pdfconverter.Controls;

public static class Win32
{
  [DllImport("user32.dll")]
  public static extern bool GetCursorPos(ref Win32.POINT point);

  public struct POINT
  {
    public int X;
    public int Y;
  }
}
