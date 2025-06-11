// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.HeaderedSelectableItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class HeaderedSelectableItem : SelectableItem
{
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (HeaderedSelectableItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (HeaderedSelectableItem), new PropertyMetadata((object) null));

  public object Header
  {
    get => this.GetValue(HeaderedSelectableItem.HeaderProperty);
    set => this.SetValue(HeaderedSelectableItem.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(HeaderedSelectableItem.HeaderTemplateProperty);
    set => this.SetValue(HeaderedSelectableItem.HeaderTemplateProperty, (object) value);
  }
}
