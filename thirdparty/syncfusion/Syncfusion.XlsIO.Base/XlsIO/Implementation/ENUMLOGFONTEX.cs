// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ENUMLOGFONTEX
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class ENUMLOGFONTEX
{
  public LOGFONT m_logFont;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 /*0x40*/)]
  private byte[] m_arrFullName;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 /*0x20*/)]
  private byte[] m_arrStyle;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 /*0x20*/)]
  public byte[] m_arrScript;

  public string FullName => this.GetZeroTerminatedString(this.m_arrFullName);

  public string Style => this.GetZeroTerminatedString(this.m_arrStyle);

  public LOGFONT LogFont => this.m_logFont;

  private string GetZeroTerminatedString(byte[] arrData)
  {
    if (arrData == null)
      return (string) null;
    int count = 0;
    int length = arrData.Length;
    while (count < length && arrData[count] != (byte) 0)
      ++count;
    return Encoding.Default.GetString(arrData, 0, count);
  }
}
