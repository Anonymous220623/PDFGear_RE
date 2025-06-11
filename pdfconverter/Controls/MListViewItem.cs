// Decompiled with JetBrains decompiler
// Type: pdfconverter.Controls.MListViewItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfconverter.Controls;

internal class MListViewItem : ListViewItem
{
  static MListViewItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MListViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MListViewItem)));
  }

  protected override void OnMouseDown(MouseButtonEventArgs e) => base.OnMouseDown(e);
}
