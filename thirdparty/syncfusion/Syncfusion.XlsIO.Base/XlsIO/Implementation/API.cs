// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.API
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public sealed class API
{
  [DllImport("gdi32.dll")]
  public static extern int EnumFontFamiliesEx(
    IntPtr hdc,
    LOGFONT lpLogfont,
    EnumFontFamExProc lpEnumFontFamExProc,
    ref object objData,
    int dwFlags);
}
