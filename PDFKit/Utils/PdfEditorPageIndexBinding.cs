// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfEditorPageIndexBinding
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.EventArguments;
using System;
using System.Windows;

#nullable disable
namespace PDFKit.Utils;

internal class PdfEditorPageIndexBinding : PdfControlPageIndexBinding
{
  private readonly PdfEditor editor;

  public PdfEditorPageIndexBinding(PdfEditor editor)
  {
    this.editor = editor;
    editor.Loaded += (RoutedEventHandler) ((s, a) => this.OnLoaded());
    editor.Unloaded += (RoutedEventHandler) ((s, a) => this.OnUnloaded());
    if (!editor.IsLoaded)
      return;
    this.OnLoaded();
  }

  protected override IPdfScrollInfo ScrollInfo => (IPdfScrollInfo) this.editor;

  protected override int PageIndexInternal
  {
    get => this.editor.CurrentIndex;
    set => this.editor.CurrentIndex = value;
  }

  protected override void OnLoaded()
  {
    base.OnLoaded();
    this.editor.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.BeforeDocumentChanged);
    this.editor.CurrentPageChanged -= new EventHandler(this.CurrentPageChanged);
    this.editor.ScrollOwnerChanged -= new EventHandler(this.ScrollOwnerChanged);
    this.editor.BeforeDocumentChanged += new EventHandler<DocumentClosingEventArgs>(this.BeforeDocumentChanged);
    this.editor.CurrentPageChanged += new EventHandler(this.CurrentPageChanged);
    this.editor.ScrollOwnerChanged += new EventHandler(this.ScrollOwnerChanged);
  }

  protected override void OnUnloaded()
  {
    base.OnUnloaded();
    this.editor.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.BeforeDocumentChanged);
    this.editor.CurrentPageChanged -= new EventHandler(this.CurrentPageChanged);
    this.editor.ScrollOwnerChanged -= new EventHandler(this.ScrollOwnerChanged);
  }

  private void BeforeDocumentChanged(object sender, DocumentClosingEventArgs e)
  {
    this.OnBeforeDocumentChanged();
  }

  private void CurrentPageChanged(object sender, EventArgs e) => this.OnCurrentPageChanged();

  private void ScrollOwnerChanged(object sender, EventArgs e) => this.OnScrollOwnerChanged();
}
