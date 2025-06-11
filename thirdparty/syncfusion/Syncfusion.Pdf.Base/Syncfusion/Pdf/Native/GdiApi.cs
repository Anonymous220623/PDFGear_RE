// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.GdiApi
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal sealed class GdiApi
{
  private GdiApi() => throw new NotImplementedException();

  [DllImport("gdi32.dll")]
  internal static extern int AddFontResource(string lpszFilename);

  [DllImport("gdi32.dll")]
  internal static extern bool RemoveFontResource(string lpFileName);

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

  [DllImport("gdi32.dll", EntryPoint = "GetCharWidth32")]
  internal static extern bool GetCharWidth(
    IntPtr hdc,
    int iFirstChar,
    int iLastChar,
    int[] lpBuffer);

  [DllImport("gdi32.dll", EntryPoint = "GetTextExtentPoint32")]
  internal static extern bool GetTextExtentPoint(
    IntPtr hdc,
    string lpString,
    int cbString,
    ref Size lpSize);

  [DllImport("gdi32.dll")]
  internal static extern bool GetCharABCWidths(
    IntPtr hdc,
    int uFirstChar,
    int uLastChar,
    ref ABC lpabc);

  [DllImport("gdi32.dll")]
  internal static extern int SetTextColor(IntPtr hdc, int crColor);

  [DllImport("gdi32.dll")]
  internal static extern int SetTextAlign(IntPtr hdc, int fMode);

  [DllImport("gdi32.dll")]
  internal static extern int SetBkColor(IntPtr hdc, int crColor);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr SaveDC(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int GetTextColor(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int GetBkColor(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int GetTextAlign(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr RestoreDC(IntPtr hdc, int nSavedDC);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr LPtoDP(IntPtr hdc, [In, Out] POINT[] lpPoints, int nCount);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr DPtoLP(IntPtr hdc, [In, Out] POINT[] lpPoints, int nCount);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateIC(
    string lpszDriver,
    string lpszDevice,
    string lpszOutput,
    IntPtr lpdvmInit);

  [DllImport("gdi32.dll")]
  internal static extern bool SetWindowExtEx(
    IntPtr hdc,
    int nXExtent,
    int nYExtent,
    ref SIZE lpSize);

  [DllImport("gdi32.dll")]
  internal static extern bool SetWindowOrgEx(IntPtr hdc, int X, int Y, ref POINT lpPoint);

  [DllImport("gdi32.dll")]
  internal static extern bool SetViewportExtEx(
    IntPtr hdc,
    int nXExtent,
    int nYExtent,
    ref SIZE lpSize);

  [DllImport("gdi32.dll")]
  internal static extern bool GetViewportExtEx(IntPtr hdc, ref SIZE lpSize);

  [DllImport("gdi32.dll")]
  internal static extern bool SetViewportOrgEx(IntPtr hdc, int X, int Y, ref POINT lpPoint);

  [DllImport("gdi32.dll")]
  internal static extern bool ScaleWindowExtEx(
    IntPtr hdc,
    int Xnum,
    int Xdenom,
    int Ynum,
    int Ydenom,
    ref SIZE lpSize);

  [DllImport("gdi32.dll")]
  internal static extern bool ScaleViewportExtEx(
    IntPtr hdc,
    int Xnum,
    int Xdenom,
    int Ynum,
    int Ydenom,
    ref SIZE lpSize);

  [DllImport("gdi32.dll")]
  internal static extern int SetMapMode(IntPtr hdc, int fnMapMode);

  [DllImport("gdi32.dll")]
  internal static extern int GetGraphicsMode(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

  [DllImport("user32.dll")]
  internal static extern IntPtr GetDC(IntPtr hWnd);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateDC(
    string lpszDriver,
    string lpszDevice,
    string lpszOutput,
    IntPtr lpInitData);

  [DllImport("gdi32.dll")]
  internal static extern bool DeleteDC(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool ModifyWorldTransform(IntPtr hdc, ref XFORM lpXform, int iMode);

  [DllImport("gdi32.dll")]
  internal static extern bool SetWorldTransform(IntPtr hdc, ref XFORM lpXform);

  [DllImport("gdi32.dll")]
  internal static extern int GetMapMode(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool SetMiterLimit(IntPtr hdc, float eNewLimit, out float peOldLimit);

  [DllImport("gdi32.dll")]
  internal static extern bool GetMiterLimit(IntPtr hdc, out float peLimit);

  [DllImport("gdi32.dll")]
  internal static extern int SetPolyFillMode(IntPtr hdc, int iPolyFillMode);

  [DllImport("gdi32.dll")]
  internal static extern int GetPolyFillMode(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int SetGraphicsMode(IntPtr hdc, int iMode);

  [DllImport("gdi32.dll")]
  internal static extern bool BeginPath(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool MoveToEx(IntPtr hdc, int X, int Y, ref POINT lpPoint);

  [DllImport("gdi32.dll")]
  internal static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

  [DllImport("gdi32.dll")]
  internal static extern bool EndPath(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool AbortPath(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool SelectClipPath(IntPtr hdc, int iMode);

  [DllImport("gdi32.dll")]
  internal static extern int SetICMMode(IntPtr hDC, int iEnableICM);

  [DllImport("gdi32.dll")]
  internal static extern int GetMetaFileBitsEx(IntPtr hmf, int nSize, byte[] lpvData);

  [DllImport("gdi32.dll")]
  internal static extern int GetEnhMetaFileBits(IntPtr hmf, int nSize, byte[] lpvData);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr SetMetaFileBitsEx(uint nSize, byte[] lpData);

  [DllImport("gdi32.dll")]
  internal static extern uint GetWinMetaFileBits(
    IntPtr hemf,
    uint cbBuffer,
    byte[] lpbBuffer,
    int fnMapMode,
    IntPtr hdcRef);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CopyMetaFile(IntPtr hmfSrc, string lpszFile);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr SetWinMetaFileBits(
    int cbBuffer,
    byte[] lpbBuffer,
    IntPtr hdcRef,
    ref METAFILEPICT lpmfp);

  [DllImport("gdi32.dll")]
  internal static extern bool DeleteEnhMetaFile(IntPtr hemf);

  [DllImport("user32.dll")]
  internal static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

  [DllImport("gdi32.dll")]
  internal static extern int SetArcDirection(IntPtr hdc, int ArcDirection);

  [DllImport("gdi32.dll")]
  internal static extern int GetArcDirection(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int SetBkMode(IntPtr hdc, int iBkMode);

  [DllImport("gdi32.dll")]
  internal static extern bool AngleArc(
    IntPtr hdc,
    int X,
    int Y,
    int dwRadius,
    float eStartAngle,
    float eSweepAngle);

  [DllImport("gdi32.dll")]
  internal static extern bool Chord(
    IntPtr hdc,
    int nLeftRect,
    int nTopRect,
    int nRightRect,
    int nBottomRect,
    int nXRadial1,
    int nYRadial1,
    int nXRadial2,
    int nYRadial2);

  [DllImport("gdi32.dll")]
  internal static extern bool ArcTo(
    IntPtr hdc,
    int nLeftRect,
    int nTopRect,
    int nRightRect,
    int nBottomRect,
    int nXRadial1,
    int nYRadial1,
    int nXRadial2,
    int nYRadial2);

  [DllImport("gdi32.dll")]
  internal static extern bool Arc(
    IntPtr hdc,
    int nLeftRect,
    int nTopRect,
    int nRightRect,
    int nBottomRect,
    int nXRadial1,
    int nYRadial1,
    int nXRadial2,
    int nYRadial2);

  [DllImport("gdi32.dll")]
  internal static extern bool CloseFigure(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool FillPath(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool StrokeAndFillPath(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern bool StrokePath(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int StretchDIBits(
    IntPtr hdc,
    int XDest,
    int YDest,
    int nDestWidth,
    int nDestHeight,
    int XSrc,
    int YSrc,
    int nSrcWidth,
    int nSrcHeight,
    byte[] lpBits,
    [In] ref BITMAPINFO lpBitsInfo,
    int iUsage,
    uint dwRop);

  [DllImport("gdi32.dll")]
  internal static extern bool BitBlt(
    IntPtr hdc,
    int nXDest,
    int nYDest,
    int nWidth,
    int nHeight,
    IntPtr hdcSrc,
    int nXSrc,
    int nYSrc,
    uint dwRop);

  [DllImport("gdi32.dll")]
  internal static extern int GetDCBrushColor(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern int GetDCPenColor(IntPtr hdc);

  [DllImport("gdi32.dll", EntryPoint = "GetOutlineTextMetrics")]
  internal static extern int GetOutlineTextMetricsEx(IntPtr hdc, int cbData, IntPtr lpOTM);

  [DllImport("gdi32.dll")]
  internal static extern int GetOutlineTextMetrics(
    IntPtr hdc,
    int cbData,
    ref OUTLINETEXTMETRIC lpOTM);

  [DllImport("gdi32.dll")]
  internal static extern bool GetTextExtentPoint32(
    IntPtr hdc,
    string lpString,
    int cbString,
    out SIZE lpSize);

  [DllImport("gdi32.dll")]
  internal static extern bool PolyBezierTo(IntPtr hdc, POINT[] lppt, uint cCount);

  [DllImport("gdi32.dll")]
  internal static extern bool PolylineTo(IntPtr hdc, POINT[] lppt, uint cCount);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateFontIndirect(ref LOGFONT lplf);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

  [DllImport("gdi32.dll")]
  internal static extern int SetStretchBltMode(IntPtr hdc, int iStretchMode);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateBitmapIndirect(ref BITMAP lpbm);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateDIBitmap(
    IntPtr hdc,
    ref BITMAPINFOHEADER lpbmih,
    uint fdwInit,
    byte[] lpbInit,
    ref BITMAPINFO lpbmi,
    uint fuUsage);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateDIBitmap(
    IntPtr hdc,
    IntPtr lpbmih,
    uint fdwInit,
    byte[] lpbInit,
    IntPtr lpbmi,
    uint fuUsage);

  [DllImport("gdi32.dll")]
  internal static extern bool ExtTextOut(
    IntPtr hdc,
    int X,
    int Y,
    int fuOptions,
    ref RECT lprc,
    string lpString,
    int cbCount,
    IntPtr lpDx);

  [DllImport("gdi32.dll")]
  internal static extern bool MaskBlt(
    IntPtr hdcDest,
    int nXDest,
    int nYDest,
    int nWidth,
    int nHeight,
    IntPtr hdcSrc,
    int nXSrc,
    int nYSrc,
    IntPtr hbmMask,
    int xMask,
    int yMask,
    uint dwRop);

  [DllImport("gdi32.dll")]
  internal static extern int SetLayout(IntPtr hdc, int dwLayout);

  [DllImport("gdi32.dll")]
  internal static extern int SetMetaRgn(IntPtr hdc);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr AddFontMemResourceEx(
    IntPtr pbFont,
    uint cbFont,
    IntPtr pdv,
    [In] ref uint pcFonts);
}
