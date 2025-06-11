// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ComboBoxItemCapsuleStyleSelector
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Tools;

public class ComboBoxItemCapsuleStyleSelector : StyleSelector
{
  public override Style SelectStyle(object item, DependencyObject container)
  {
    if (container is ComboBoxItem comboBoxItem)
    {
      ComboBox parent = VisualHelper.GetParent<ComboBox>((DependencyObject) comboBoxItem);
      if (parent != null)
      {
        int count = parent.Items.Count;
        if (count == 1)
          return ResourceHelper.GetResourceInternal<Style>("ComboBoxItemCapsuleSingle");
        int num = parent.ItemContainerGenerator.IndexFromContainer((DependencyObject) comboBoxItem);
        return num != 0 ? ResourceHelper.GetResourceInternal<Style>(num == count - 1 ? "ComboBoxItemCapsuleHorizontalLast" : "ComboBoxItemCapsuleDefault") : ResourceHelper.GetResourceInternal<Style>("ComboBoxItemCapsuleHorizontalFirst");
      }
    }
    return (Style) null;
  }
}
