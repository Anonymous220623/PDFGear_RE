// Decompiled with JetBrains decompiler
// Type: Tesseract.PdfResultRenderer
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Runtime.InteropServices;
using Tesseract.Interop;

#nullable disable
namespace Tesseract;

public sealed class PdfResultRenderer : ResultRenderer
{
  private IntPtr _fontDirectoryHandle;

  public PdfResultRenderer(string outputFilename, string fontDirectory, bool textonly)
  {
    IntPtr hglobalAnsi = Marshal.StringToHGlobalAnsi(fontDirectory);
    this.Initialise(TessApi.Native.PDFRendererCreate(outputFilename, hglobalAnsi, textonly ? 1 : 0));
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (!(this._fontDirectoryHandle != IntPtr.Zero))
      return;
    Marshal.FreeHGlobal(this._fontDirectoryHandle);
    this._fontDirectoryHandle = IntPtr.Zero;
  }
}
