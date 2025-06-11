// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.POINT
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Windows.Shared;

[StructLayout(LayoutKind.Sequential)]
public class POINT
{
  public int x;
  public int y;

  public POINT()
  {
  }

  public POINT(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public override string ToString() => $"{{{this.x.ToString()}; {this.y.ToString()}}}";
}
