// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RegularItemsControl
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class RegularItemsControl : SimpleItemsControl
{
  public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (RegularItemsControl), new PropertyMetadata(ValueBoxes.Double200Box));
  public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (RegularItemsControl), new PropertyMetadata(ValueBoxes.Double200Box));
  public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof (ItemMargin), typeof (Thickness), typeof (RegularItemsControl), new PropertyMetadata((object) new Thickness()));

  public double ItemWidth
  {
    get => (double) this.GetValue(RegularItemsControl.ItemWidthProperty);
    set => this.SetValue(RegularItemsControl.ItemWidthProperty, (object) value);
  }

  public double ItemHeight
  {
    get => (double) this.GetValue(RegularItemsControl.ItemHeightProperty);
    set => this.SetValue(RegularItemsControl.ItemHeightProperty, (object) value);
  }

  public Thickness ItemMargin
  {
    get => (Thickness) this.GetValue(RegularItemsControl.ItemMarginProperty);
    set => this.SetValue(RegularItemsControl.ItemMarginProperty, (object) value);
  }
}
