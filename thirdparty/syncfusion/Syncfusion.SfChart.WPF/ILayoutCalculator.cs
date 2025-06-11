// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ILayoutCalculator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public interface ILayoutCalculator
{
  List<UIElement> Children { get; }

  Panel Panel { get; }

  double Left { get; set; }

  double Top { get; set; }

  Size DesiredSize { get; }

  Size Measure(Size availableSize);

  Size Arrange(Size finalSize);

  void UpdateElements();

  void DetachElements();
}
