// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PanToolCursorHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.Utils;

internal class PanToolCursorHelper
{
  private const string PanToolHandUri = "pack://application:,,,/PDFKit;component/Resources/pantool-hand.png";
  private const string PanToolHandCloseUri = "pack://application:,,,/PDFKit;component/Resources/pantool-hand-close.png";
  private Cursor handCursor;
  private Cursor handCloseCursor;

  public Cursor Hand
  {
    get
    {
      if (this.handCursor == null)
      {
        lock (this)
        {
          if (this.handCursor == null)
            this.handCursor = PanToolCursorHelper.CreateCursor("pack://application:,,,/PDFKit;component/Resources/pantool-hand.png");
        }
      }
      return this.handCursor;
    }
  }

  public Cursor HandClose
  {
    get
    {
      if (this.handCloseCursor == null)
      {
        lock (this)
        {
          if (this.handCloseCursor == null)
            this.handCloseCursor = PanToolCursorHelper.CreateCursor("pack://application:,,,/PDFKit;component/Resources/pantool-hand-close.png");
        }
      }
      return this.handCloseCursor;
    }
  }

  private static Cursor CreateCursor(string uri)
  {
    return CursorHelper.CreateCursor((BitmapSource) PanToolCursorHelper.ResizeBitmap(new WriteableBitmap((BitmapSource) new BitmapImage(new Uri(uri))), 22, 23, 1f), 11U, 11U);
  }

  private static WriteableBitmap ResizeBitmap(
    WriteableBitmap _source,
    int _newWidth,
    int _newHeight,
    float _opacity)
  {
    if (_source.Width == (double) _newWidth && _source.Height == (double) _newHeight && (double) _opacity == 1.0)
      return _source;
    using (Bitmap bitmap1 = new Bitmap(_source.PixelWidth, _source.PixelHeight, PixelFormat.Format32bppArgb))
    {
      BitmapData bitmapdata = bitmap1.LockBits(new Rectangle(0, 0, _source.PixelWidth, _source.PixelHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
      _source.CopyPixels(new Int32Rect(0, 0, _source.PixelWidth, _source.PixelHeight), bitmapdata.Scan0, bitmapdata.Stride * bitmapdata.Height, bitmapdata.Stride);
      bitmap1.UnlockBits(bitmapdata);
      BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromWidthAndHeight(_newWidth, _newHeight);
      Int32Rect sourceRect = new Int32Rect(0, 0, bitmap1.Width, bitmap1.Height);
      BitmapSource source = (BitmapSource) null;
      if ((double) _opacity != 1.0)
      {
        using (Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, bitmap1.PixelFormat))
        {
          float num1 = Math.Max(0.0f, Math.Min(1f, _opacity));
          ColorMatrix newColorMatrix = new ColorMatrix();
          newColorMatrix.Matrix33 = num1;
          using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
          {
            using (ImageAttributes imageAttr = new ImageAttributes())
            {
              imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
              graphics.DrawImage((Image) bitmap1, new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), 0, 0, bitmap1.Width, bitmap1.Height, GraphicsUnit.Pixel, imageAttr);
            }
          }
          IntPtr num2 = IntPtr.Zero;
          try
          {
            num2 = bitmap2.GetHbitmap();
            source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(num2, IntPtr.Zero, sourceRect, sizeOptions);
          }
          finally
          {
            PanToolCursorHelper.DeleteObject(num2);
          }
        }
      }
      else
      {
        IntPtr num = IntPtr.Zero;
        try
        {
          num = bitmap1.GetHbitmap();
          source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(num, IntPtr.Zero, sourceRect, sizeOptions);
        }
        finally
        {
          PanToolCursorHelper.DeleteObject(num);
        }
      }
      return new WriteableBitmap(source);
    }
  }

  [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool DeleteObject(IntPtr hObject);
}
