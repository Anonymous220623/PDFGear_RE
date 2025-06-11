// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.BitmapCursor
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Utils;

internal class BitmapCursor : SafeHandle
{
  public override bool IsInvalid => this.handle == (IntPtr) -1;

  public static Cursor CreateBmpCursor(Bitmap cursorBitmap)
  {
    return CursorInteropHelper.Create((SafeHandle) new BitmapCursor(cursorBitmap));
  }

  protected BitmapCursor(Bitmap cursorBitmap)
    : base((IntPtr) -1, true)
  {
    this.handle = cursorBitmap.GetHicon();
  }

  protected override bool ReleaseHandle()
  {
    int num = BitmapCursor.DestroyIcon(this.handle) ? 1 : 0;
    this.handle = (IntPtr) -1;
    return num != 0;
  }

  [DllImport("user32")]
  private static extern bool DestroyIcon(IntPtr hIcon);

  public static Cursor CreateCustomCursor(
    ToolbarSettingInkEraserModel inkEraserModel,
    MainViewModel VM,
    int cursorSize)
  {
    EllipseGeometry ellipseGeometry = new EllipseGeometry(new System.Windows.Point((double) (cursorSize / 2), (double) (cursorSize / 2)), (double) (cursorSize / 2), (double) (cursorSize / 2));
    DrawingVisual drawingVisual = new DrawingVisual();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.White);
      drawingContext.DrawGeometry((System.Windows.Media.Brush) solidColorBrush, (System.Windows.Media.Pen) null, (Geometry) ellipseGeometry);
    }
    RenderTargetBitmap source = new RenderTargetBitmap(cursorSize, cursorSize, 96.0, 96.0, PixelFormats.Default);
    source.Render((Visual) drawingVisual);
    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
    pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
    using (MemoryStream memoryStream = new MemoryStream())
    {
      pngBitmapEncoder.Save((Stream) memoryStream);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      using (Bitmap cursorBitmap = new Bitmap((Stream) memoryStream))
        return BitmapCursor.CreateBmpCursor(cursorBitmap);
    }
  }
}
