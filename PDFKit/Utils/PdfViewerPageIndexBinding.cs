// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfViewerPageIndexBinding
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.EventArguments;
using System;
using System.Windows;

#nullable disable
namespace PDFKit.Utils;

internal class PdfViewerPageIndexBinding : PdfControlPageIndexBinding
{
  private readonly PdfViewer viewer;

  public PdfViewerPageIndexBinding(PdfViewer viewer)
  {
    this.viewer = viewer;
    viewer.Loaded += (RoutedEventHandler) ((s, a) => this.OnLoaded());
    viewer.Unloaded += (RoutedEventHandler) ((s, a) => this.OnUnloaded());
    if (!viewer.IsLoaded)
      return;
    this.OnLoaded();
  }

  protected override IPdfScrollInfo ScrollInfo => (IPdfScrollInfo) this.viewer;

  protected override int PageIndexInternal
  {
    get => this.viewer.CurrentIndex;
    set => this.viewer.CurrentIndex = value;
  }

  protected override void OnLoaded()
  {
    base.OnLoaded();
    this.viewer.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.BeforeDocumentChanged);
    this.viewer.CurrentPageChanged -= new EventHandler(this.CurrentPageChanged);
    this.viewer.ScrollOwnerChanged -= new EventHandler(this.ScrollOwnerChanged);
    this.viewer.BeforeDocumentChanged += new EventHandler<DocumentClosingEventArgs>(this.BeforeDocumentChanged);
    this.viewer.CurrentPageChanged += new EventHandler(this.CurrentPageChanged);
    this.viewer.ScrollOwnerChanged += new EventHandler(this.ScrollOwnerChanged);
  }

  protected override void OnUnloaded()
  {
    base.OnUnloaded();
    this.viewer.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.BeforeDocumentChanged);
    this.viewer.CurrentPageChanged -= new EventHandler(this.CurrentPageChanged);
    this.viewer.ScrollOwnerChanged -= new EventHandler(this.ScrollOwnerChanged);
  }

  private void BeforeDocumentChanged(object sender, DocumentClosingEventArgs e)
  {
    this.OnBeforeDocumentChanged();
  }

  private void CurrentPageChanged(object sender, EventArgs e) => this.OnCurrentPageChanged();

  private void ScrollOwnerChanged(object sender, EventArgs e) => this.OnScrollOwnerChanged();
}
