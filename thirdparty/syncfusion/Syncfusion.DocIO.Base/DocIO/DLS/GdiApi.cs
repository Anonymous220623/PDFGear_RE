// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.GdiApi
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal sealed class GdiApi
{
  private GdiApi() => throw new NotImplementedException();

  [DllImport("user32.dll")]
  internal static extern IntPtr GetDC(IntPtr hWnd);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

  [DllImport("gdi32.dll")]
  internal static extern int DeleteObject(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern uint GetFontData(
    IntPtr hdc,
    uint dwTable,
    uint dwOffset,
    [In, Out] byte[] lpvBuffer,
    uint cbData);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateDC(
    string lpszDriver,
    string lpszDevice,
    string lpszOutput,
    IntPtr lpInitData);

  [DllImport("gdi32.dll")]
  internal static extern bool DeleteDC(IntPtr hdc);
}
