// Decompiled with JetBrains decompiler
// Type: Standard.NONCLIENTMETRICS
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

internal struct NONCLIENTMETRICS
{
  public int cbSize;
  public int iBorderWidth;
  public int iScrollWidth;
  public int iScrollHeight;
  public int iCaptionWidth;
  public int iCaptionHeight;
  public LOGFONT lfCaptionFont;
  public int iSmCaptionWidth;
  public int iSmCaptionHeight;
  public LOGFONT lfSmCaptionFont;
  public int iMenuWidth;
  public int iMenuHeight;
  public LOGFONT lfMenuFont;
  public LOGFONT lfStatusFont;
  public LOGFONT lfMessageFont;
  public int iPaddedBorderWidth;

  public static NONCLIENTMETRICS VistaMetricsStruct
  {
    get
    {
      return new NONCLIENTMETRICS()
      {
        cbSize = Marshal.SizeOf(typeof (NONCLIENTMETRICS))
      };
    }
  }

  public static NONCLIENTMETRICS XPMetricsStruct
  {
    get
    {
      return new NONCLIENTMETRICS()
      {
        cbSize = Marshal.SizeOf(typeof (NONCLIENTMETRICS)) - 4
      };
    }
  }
}
