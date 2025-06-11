// Decompiled with JetBrains decompiler
// Type: Standard.MONITORINFO
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential)]
internal class MONITORINFO
{
  public int cbSize = Marshal.SizeOf(typeof (MONITORINFO));
  public RECT rcMonitor;
  public RECT rcWork;
  public int dwFlags;
}
