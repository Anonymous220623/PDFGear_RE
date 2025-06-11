// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.CursorHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.Utils;

public class CursorHelper
{
  private static ThreadLocal<ConcurrentDictionary<(string path, uint xHotSpot, uint yHotSpot), Cursor>> cursorDict = new ThreadLocal<ConcurrentDictionary<(string, uint, uint), Cursor>>();

  public static Cursor CreateCursor(string filePath, uint xHotSpot = 0, uint yHotSpot = 0)
  {
    if (string.IsNullOrEmpty(filePath))
      return (Cursor) null;
    lock (CursorHelper.cursorDict)
    {
      if (!CursorHelper.cursorDict.IsValueCreated)
        CursorHelper.cursorDict.Value = new ConcurrentDictionary<(string, uint, uint), Cursor>();
      ConcurrentDictionary<(string, uint, uint), Cursor> concurrentDictionary = CursorHelper.cursorDict.Value;
      filePath = filePath.ToLowerInvariant();
      Cursor cursorCore;
      if (!concurrentDictionary.TryGetValue((filePath, xHotSpot, yHotSpot), out cursorCore))
      {
        cursorCore = CursorHelper.CreateCursorCore(filePath, xHotSpot, yHotSpot);
        concurrentDictionary[(filePath, xHotSpot, yHotSpot)] = cursorCore;
      }
      return cursorCore;
    }
  }

  private static Cursor CreateCursorCore(string filePath, uint xHotSpot = 0, uint yHotSpot = 0)
  {
    Cursor cursorCore = (Cursor) null;
    if (string.IsNullOrWhiteSpace(filePath) || Directory.Exists(filePath) || !File.Exists(filePath))
      return cursorCore;
    if (filePath.EndsWith(".ani") || filePath.EndsWith(".cur"))
    {
      try
      {
        cursorCore = new Cursor(filePath);
      }
      catch (Exception ex)
      {
        cursorCore = (Cursor) null;
      }
    }
    if (cursorCore == null)
    {
      try
      {
        if (Image.FromFile(filePath) is Bitmap bm)
          cursorCore = CursorHelper.CreateCursor(bm, xHotSpot, yHotSpot);
      }
      catch (Exception ex)
      {
        cursorCore = (Cursor) null;
      }
    }
    return cursorCore;
  }

  public static Cursor CreateCursor(BitmapSource bs, uint xHotSpot = 0, uint yHotSpot = 0)
  {
    Cursor cursor = (Cursor) null;
    Bitmap bitmap = CursorHelper.BitmapSource2Bitmap(bs);
    if (bitmap != null)
    {
      try
      {
        cursor = CursorHelper.InternalCreateCursor(bitmap, xHotSpot, yHotSpot);
      }
      catch (Exception ex)
      {
        cursor = (Cursor) null;
      }
    }
    return cursor;
  }

  private static Cursor CreateCursor(Bitmap bm, uint xHotSpot = 0, uint yHotSpot = 0)
  {
    Cursor cursor1 = (Cursor) null;
    if (bm == null)
      return cursor1;
    Cursor cursor2;
    try
    {
      cursor2 = CursorHelper.InternalCreateCursor(bm, xHotSpot, yHotSpot);
    }
    catch (Exception ex)
    {
      cursor2 = (Cursor) null;
    }
    return cursor2;
  }

  private static Bitmap BitmapSource2Bitmap2(BitmapSource bi)
  {
    Bitmap bitmap1 = (Bitmap) null;
    if (bi == null)
      return bitmap1;
    Bitmap bitmap2;
    try
    {
      MemoryStream memoryStream = new MemoryStream();
      BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
      bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bi));
      bmpBitmapEncoder.Save((Stream) memoryStream);
      bitmap2 = new Bitmap((Stream) memoryStream);
    }
    catch (Exception ex)
    {
      bitmap2 = (Bitmap) null;
    }
    return bitmap2;
  }

  private static Bitmap BitmapSource2Bitmap(BitmapSource bi)
  {
    Bitmap bitmap = new Bitmap(bi.PixelWidth, bi.PixelHeight, PixelFormat.Format32bppArgb);
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bi.PixelWidth, bi.PixelHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
    bi.CopyPixels(new Int32Rect(0, 0, bi.PixelWidth, bi.PixelHeight), bitmapdata.Scan0, bitmapdata.Stride * bitmapdata.Height, bitmapdata.Stride);
    bitmap.UnlockBits(bitmapdata);
    return bitmap;
  }

  private static Cursor InternalCreateCursor(Bitmap bitmap, uint xHotSpot, uint yHotSpot)
  {
    CursorHelper.NativeMethods.IconInfo iconInfo = new CursorHelper.NativeMethods.IconInfo();
    CursorHelper.NativeMethods.GetIconInfo(bitmap.GetHicon(), ref iconInfo);
    iconInfo.xHotspot = xHotSpot;
    iconInfo.yHotspot = yHotSpot;
    iconInfo.fIcon = false;
    return CursorInteropHelper.Create((SafeHandle) CursorHelper.NativeMethods.CreateIconIndirect(ref iconInfo));
  }

  public static void SetCursorPositon(int x, int y)
  {
    CursorHelper.NativeMethods.SetCursorPos(x, y);
  }

  protected static class NativeMethods
  {
    [DllImport("user32.dll")]
    public static extern CursorHelper.SafeIconHandle CreateIconIndirect(
      ref CursorHelper.NativeMethods.IconInfo icon);

    [DllImport("user32.dll")]
    public static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetIconInfo(
      IntPtr hIcon,
      ref CursorHelper.NativeMethods.IconInfo pIconInfo);

    [DllImport("User32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public struct IconInfo
    {
      public bool fIcon;
      public uint xHotspot;
      public uint yHotspot;
      public IntPtr hbmMask;
      public IntPtr hbmColor;
    }
  }

  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
  protected class SafeIconHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public SafeIconHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle() => CursorHelper.NativeMethods.DestroyIcon(this.handle);
  }
}
