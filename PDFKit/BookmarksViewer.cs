// Decompiled with JetBrains decompiler
// Type: PDFKit.BookmarksViewer
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace PDFKit;

public class BookmarksViewer : TreeView
{
  public static readonly DependencyProperty PdfViewerProperty = DependencyProperty.Register(nameof (PdfViewer), typeof (PdfViewer), typeof (BookmarksViewer), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) =>
  {
    BookmarksViewer bookmarksViewer = o as BookmarksViewer;
    PdfViewer oldValue = e.OldValue as PdfViewer;
    PdfViewer newValue = e.NewValue as PdfViewer;
    if (oldValue == newValue)
      return;
    bookmarksViewer.OnPdfViewerChanging(oldValue, newValue);
  })));

  public PdfViewer PdfViewer
  {
    get => (PdfViewer) this.GetValue(BookmarksViewer.PdfViewerProperty);
    set => this.SetValue(BookmarksViewer.PdfViewerProperty, (object) value);
  }

  public BookmarksViewer()
  {
    this.ItemTemplate = (DataTemplate) XamlReader.Parse("<HierarchicalDataTemplate ItemsSource=\"{Binding Path=Childs}\"><TextBlock Text=\"{Binding Title}\" /></HierarchicalDataTemplate>", new ParserContext()
    {
      XmlnsDictionary = {
        {
          "",
          "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        }
      }
    });
  }

  protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
  {
    if (!(e.NewValue is PdfBookmark newValue))
      return;
    if (newValue.Action != null)
      this.ProcessAction(newValue.Action);
    else if (newValue.Destination != null)
      this.ProcessDestination(newValue.Destination);
    base.OnSelectedItemChanged(e);
  }

  protected virtual void OnPdfViewerChanging(PdfViewer oldValue, PdfViewer newValue)
  {
    if (oldValue != null)
    {
      oldValue.AfterDocumentChanged -= new EventHandler(this.pdfViewer_DocumentChanged);
      oldValue.DocumentClosed -= new EventHandler(this.pdfViewer_DocumentClosed);
      oldValue.DocumentLoaded -= new EventHandler(this.pdfViewer_DocumentLoaded);
    }
    if (newValue != null)
    {
      newValue.AfterDocumentChanged += new EventHandler(this.pdfViewer_DocumentChanged);
      newValue.DocumentClosed += new EventHandler(this.pdfViewer_DocumentClosed);
      newValue.DocumentLoaded += new EventHandler(this.pdfViewer_DocumentLoaded);
    }
    this.RebuildTree();
  }

  protected virtual void ProcessAction(PdfAction pdfAction)
  {
    if (this.PdfViewer == null)
      return;
    this.PdfViewer.ProcessAction(pdfAction);
  }

  protected virtual void ProcessDestination(PdfDestination pdfDestination)
  {
    if (this.PdfViewer == null)
      return;
    this.PdfViewer.ProcessDestination(pdfDestination);
  }

  private void pdfViewer_DocumentChanged(object sender, EventArgs e) => this.RebuildTree();

  private void pdfViewer_DocumentLoaded(object sender, EventArgs e) => this.RebuildTree();

  private void pdfViewer_DocumentClosed(object sender, EventArgs e) => this.RebuildTree();

  public void RebuildTree()
  {
    if (this.PdfViewer == null || this.PdfViewer.Document == null || this.PdfViewer.Document.Bookmarks == null)
      this.ItemsSource = (IEnumerable) null;
    else
      this.ItemsSource = (IEnumerable) this.PdfViewer.Document.Bookmarks;
  }
}
