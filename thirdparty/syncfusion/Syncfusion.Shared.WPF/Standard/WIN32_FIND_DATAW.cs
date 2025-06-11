// Decompiled with JetBrains decompiler
// Type: Standard.WIN32_FIND_DATAW
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[BestFitMapping(false)]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal class WIN32_FIND_DATAW
{
  public FileAttributes dwFileAttributes;
  public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
  public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
  public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
  public int nFileSizeHigh;
  public int nFileSizeLow;
  public int dwReserved0;
  public int dwReserved1;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
  public string cFileName;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
  public string cAlternateFileName;
}
