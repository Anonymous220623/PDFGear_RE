// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.DrawUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Properties;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Utils;

public static class DrawUtils
{
  private static bool CheckUIThread() => DispatcherHelper.UIDispatcher.CheckAccess();

  public static void ShowUnsupportedImageMessage()
  {
    int num = (int) ModernMessageBox.Show(Resources.UnsupportedImageMsg, UtilManager.GetProductName());
  }

  public static async Task<PdfBitmap> CloneAsync(
    this PdfBitmap pdfBitmap,
    FX_RECT clip,
    CancellationToken cancellationToken)
  {
    if (pdfBitmap == null)
      throw new ArgumentNullException(nameof (pdfBitmap));
    cancellationToken.ThrowIfCancellationRequested();
    return await Task.Run<PdfBitmap>(TaskExceptionHelper.ExceptionBoundary<PdfBitmap>((Func<PdfBitmap>) (() =>
    {
      cancellationToken.ThrowIfCancellationRequested();
      IntPtr num = Pdfium.FFPDFBitmap_Clone(pdfBitmap.Handle, clip);
      if (cancellationToken.IsCancellationRequested)
        Pdfium.FPDFBitmap_Destroy(num);
      cancellationToken.ThrowIfCancellationRequested();
      return new PdfBitmap(num);
    }))).ConfigureAwait(false);
  }

  public static async Task<WriteableBitmap> ToWriteableBitmapAsync(
    this PdfBitmap pdfBitmap,
    CancellationToken cancellationToken)
  {
    return await pdfBitmap.ToWriteableBitmapAsync(96U /*0x60*/, 96U /*0x60*/, cancellationToken).ConfigureAwait(false);
  }

  public static async Task<WriteableBitmap> ToWriteableBitmapAsync(
    this PdfBitmap pdfBitmap,
    uint dpiX,
    uint dpiY,
    CancellationToken cancellationToken)
  {
    if (pdfBitmap == null)
      throw new ArgumentNullException(nameof (pdfBitmap));
    cancellationToken.ThrowIfCancellationRequested();
    Func<Task<WriteableBitmap>> func = (Func<Task<WriteableBitmap>>) (async () =>
    {
      WriteableBitmap bitmap = new WriteableBitmap(pdfBitmap.Width, pdfBitmap.Height, (double) dpiX, (double) dpiY, PixelFormats.Bgra32, (BitmapPalette) null);
      await DrawUtils.DrawAsync(bitmap, pdfBitmap, cancellationToken);
      bitmap.Freeze();
      WriteableBitmap writeableBitmapAsync = bitmap;
      bitmap = (WriteableBitmap) null;
      return writeableBitmapAsync;
    });
    return DrawUtils.CheckUIThread() ? await func().ConfigureAwait(false) : await (await DispatcherHelper.UIDispatcher.InvokeAsync<Task<WriteableBitmap>>((Func<Task<WriteableBitmap>>) (async () => await func().ConfigureAwait(false)))).ConfigureAwait(false);
  }

  public static async Task DrawAsync(
    WriteableBitmap bitmap,
    PdfBitmap pdfBitmap,
    CancellationToken cancellationToken)
  {
    if (bitmap == null)
      throw new ArgumentNullException(nameof (bitmap));
    if (pdfBitmap == null)
      throw new ArgumentNullException(nameof (pdfBitmap));
    cancellationToken.ThrowIfCancellationRequested();
    await DrawUtils.DrawAsync(bitmap, pdfBitmap.Buffer, (uint) (pdfBitmap.Stride * pdfBitmap.Height), cancellationToken).ConfigureAwait(false);
  }

  public static async Task DrawAsync(
    WriteableBitmap bitmap,
    IntPtr pBuffer,
    uint length,
    CancellationToken cancellationToken)
  {
    if (bitmap == null)
      throw new ArgumentNullException(nameof (bitmap));
    cancellationToken.ThrowIfCancellationRequested();
    bool isUIThread = DrawUtils.CheckUIThread();
    if (isUIThread)
      bitmap.Lock();
    else
      await DispatcherHelper.UIDispatcher.InvokeAsync((Action) (() => bitmap.Lock()));
    try
    {
      IntPtr pBackBuffer = bitmap.BackBuffer;
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        uint Length;
        for (uint index = 0; index < length; index += Length)
        {
          Length = 4194304U /*0x400000*/;
          if (index + Length > length)
            Length = length - index;
          cancellationToken.ThrowIfCancellationRequested();
          DrawUtils.CopyMemory(pBackBuffer + (int) index, pBuffer + (int) index, Length);
          cancellationToken.ThrowIfCancellationRequested();
        }
      })), cancellationToken).ConfigureAwait(isUIThread);
      cancellationToken.ThrowIfCancellationRequested();
      Action callback = (Action) (() => bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight)));
      if (isUIThread)
        callback();
      else
        await DispatcherHelper.UIDispatcher.InvokeAsync(callback);
    }
    finally
    {
      if (isUIThread)
        bitmap.Unlock();
      else
        await DispatcherHelper.UIDispatcher.InvokeAsync((Action) (() => bitmap.Unlock()));
    }
  }

  [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
  public static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);

  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern bool DeleteObject(IntPtr hObject);
}
