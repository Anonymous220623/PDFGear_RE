// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PrintVisualContainer
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

internal class PrintVisualContainer : FrameworkElement
{
  private readonly VisualCollection children;

  public PrintVisualContainer() => this.children = new VisualCollection((Visual) this);

  public void AddVisual(Visual v) => this.children.Add(v);

  protected override Visual GetVisualChild(int index) => this.children[index];

  protected override int VisualChildrenCount => this.children.Count;
}
