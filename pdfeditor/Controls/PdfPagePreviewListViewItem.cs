// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfPagePreviewListViewItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls;

public class PdfPagePreviewListViewItem : ListBoxItem
{
  static PdfPagePreviewListViewItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfPagePreviewListViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfPagePreviewListViewItem)));
  }
}
