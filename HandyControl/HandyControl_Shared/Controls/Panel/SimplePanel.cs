// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SimplePanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class SimplePanel : Panel
{
  protected override Size MeasureOverride(Size constraint)
  {
    Size size = new Size();
    foreach (UIElement internalChild in this.InternalChildren)
    {
      if (internalChild != null)
      {
        internalChild.Measure(constraint);
        size.Width = Math.Max(size.Width, internalChild.DesiredSize.Width);
        size.Height = Math.Max(size.Height, internalChild.DesiredSize.Height);
      }
    }
    return size;
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    foreach (UIElement internalChild in this.InternalChildren)
      internalChild?.Arrange(new Rect(arrangeSize));
    return arrangeSize;
  }
}
