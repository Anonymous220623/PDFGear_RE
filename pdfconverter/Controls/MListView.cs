// Decompiled with JetBrains decompiler
// Type: pdfconverter.Controls.MListView
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace pdfconverter.Controls;

public class MListView : ListView
{
  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    base.PrepareContainerForItemOverride(element, item);
    if (!(this.View is GridView))
      return;
    int num = this.ItemContainerGenerator.IndexFromContainer(element);
    ListViewItem listViewItem = element as ListViewItem;
    if (num % 2 == 0)
      listViewItem.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("White"));
    else
      listViewItem.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#F5F5F5"));
  }
}
