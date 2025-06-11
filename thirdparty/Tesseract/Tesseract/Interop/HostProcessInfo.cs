// Decompiled with JetBrains decompiler
// Type: Tesseract.Interop.HostProcessInfo
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;

#nullable disable
namespace Tesseract.Interop;

internal static class HostProcessInfo
{
  public static readonly bool Is64Bit = IntPtr.Size == 8;
}
