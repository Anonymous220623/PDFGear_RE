// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PaperSize
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

internal class PaperSize
{
  public PaperSize(double x, double y)
  {
    this.Width = x;
    this.Height = y;
  }

  public double Width { get; set; }

  public double Height { get; set; }
}
