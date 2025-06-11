// Decompiled with JetBrains decompiler
// Type: Standard.WINDOWPLACEMENT
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential)]
internal class WINDOWPLACEMENT
{
  public int length = Marshal.SizeOf(typeof (WINDOWPLACEMENT));
  public int flags;
  public SW showCmd;
  public POINT ptMinPosition;
  public POINT ptMaxPosition;
  public RECT rcNormalPosition;
}
