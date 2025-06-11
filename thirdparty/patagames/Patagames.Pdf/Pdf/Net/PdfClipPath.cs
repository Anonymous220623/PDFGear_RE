// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfClipPath
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the clip path inside a page.</summary>
public class PdfClipPath : IDisposable
{
  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the Pdfium SDK handle that the clip path is bound to
  /// </summary>
  public IntPtr Handle { get; set; }

  /// <summary>
  /// Create a new instance of <see cref="T:Patagames.Pdf.Net.PdfClipPath" /> with a rectangle inserted.
  /// </summary>
  /// <param name="left">The left of the clip box.</param>
  /// <param name="bottom">The bottom of the clip box.</param>
  /// <param name="right">The right of the clip box.</param>
  /// <param name="top">The top of the clip box.</param>
  public PdfClipPath(float left, float top, float right, float bottom)
  {
    this.Handle = Pdfium.FPDF_CreateClipPath(left, bottom, right, top);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>Destroy the clip path.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDF_DestroyClipPath(this.Handle);
    this.Handle = IntPtr.Zero;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfClipPath()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfClipPath));
  }
}
