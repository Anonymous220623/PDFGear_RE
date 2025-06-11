// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewerDecorators.PdfViewerDecoratorCollection
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Collections;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace PDFKit.PdfViewerDecorators;

internal class PdfViewerDecoratorCollection : 
  DependencyObject,
  IList<IPdfViewerDecorator>,
  ICollection<IPdfViewerDecorator>,
  IEnumerable<IPdfViewerDecorator>,
  IEnumerable
{
  private readonly PdfViewer pdfViewer;
  private List<IPdfViewerDecorator> internalCollection;

  internal PdfViewerDecoratorCollection(PdfViewer pdfViewer)
  {
    this.internalCollection = new List<IPdfViewerDecorator>();
    this.pdfViewer = pdfViewer;
  }

  public IPdfViewerDecorator this[int index]
  {
    get => this.internalCollection[index];
    set
    {
      this.Dispatcher.VerifyAccess();
      this.internalCollection[index] = value;
      this.InvalidateViewerVisual();
    }
  }

  public int Count => this.internalCollection.Count;

  public bool IsReadOnly => ((ICollection<IPdfViewerDecorator>) this.internalCollection).IsReadOnly;

  public void Add(IPdfViewerDecorator item)
  {
    this.Dispatcher.VerifyAccess();
    this.internalCollection.Add(item);
    this.InvalidateViewerVisual();
  }

  public void Clear()
  {
    this.Dispatcher.VerifyAccess();
    this.internalCollection.Clear();
    this.InvalidateViewerVisual();
  }

  public bool Contains(IPdfViewerDecorator item) => this.internalCollection.Contains(item);

  public void CopyTo(IPdfViewerDecorator[] array, int arrayIndex)
  {
    this.internalCollection.CopyTo(array, arrayIndex);
  }

  public IEnumerator<IPdfViewerDecorator> GetEnumerator()
  {
    return ((IEnumerable<IPdfViewerDecorator>) this.internalCollection).GetEnumerator();
  }

  public int IndexOf(IPdfViewerDecorator item) => this.internalCollection.IndexOf(item);

  public void Insert(int index, IPdfViewerDecorator item)
  {
    this.Dispatcher.VerifyAccess();
    this.internalCollection.Insert(index, item);
    this.InvalidateViewerVisual();
  }

  public bool Remove(IPdfViewerDecorator item)
  {
    this.Dispatcher.VerifyAccess();
    bool flag = this.internalCollection.Remove(item);
    this.InvalidateViewerVisual();
    return flag;
  }

  public void RemoveAt(int index)
  {
    this.Dispatcher.VerifyAccess();
    this.internalCollection.RemoveAt(index);
    this.InvalidateViewerVisual();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return ((IEnumerable) this.internalCollection).GetEnumerator();
  }

  private void InvalidateViewerVisual() => this.pdfViewer.InvalidateVisual();
}
