// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.CommetTreeView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls;

internal class CommetTreeView : TreeView
{
  static CommetTreeView()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CommetTreeView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CommetTreeView)));
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is CommetTreeViewItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new CommetTreeViewItem();
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is CommetTreeViewItem commetTreeViewItem && commetTreeViewItem.IsSelected)
      commetTreeViewItem.BringIntoView();
    base.PrepareContainerForItemOverride(element, item);
  }
}
