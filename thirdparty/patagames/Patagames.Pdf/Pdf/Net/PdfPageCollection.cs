// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfPageCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the collection of <see cref="T:Patagames.Pdf.Net.PdfPage" /> objects
/// </summary>
public class PdfPageCollection : ReadOnlyList<PdfPage>, IDisposable
{
  private PdfDocument _doc;
  private int _currentIndex;

  /// <summary>Occurs when the CurrentPage has changed.</summary>
  public event EventHandler CurrentPageChanged;

  /// <summary>Occurs when the new page was added into collection.</summary>
  public event EventHandler<PageCollectionChangedEventArgs> PageInserted;

  /// <summary>Occurs when the page was removed from collection.</summary>
  public event EventHandler<PageCollectionChangedEventArgs> PageDeleted;

  /// <summary>Occurs when the page is loaded into memory.</summary>
  public event EventHandler<PdfPageEventArgs> PageLoaded;

  /// <summary>Occurs when the page is disposed.</summary>
  public event EventHandler<PdfPageEventArgs> PageDisposed;

  /// <summary>
  /// Occurs during page rendering via progressive rendering.
  /// </summary>
  public event EventHandler<ProgressiveRenderEventArgs> ProgressiveRender;

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPageCollection.CurrentPageChanged" /> event.
  /// </summary>
  /// <param name="e">An System.EventArgs that contains the event data.</param>
  protected virtual void OnCurrentPageChanged(EventArgs e)
  {
    if (this.CurrentPageChanged == null)
      return;
    this.CurrentPageChanged((object) this, e);
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPageCollection.PageInserted" /> event.
  /// </summary>
  /// <param name="e">An <see cref="T:Patagames.Pdf.Net.EventArguments.PageCollectionChangedEventArgs" /> that contains the event data.</param>
  protected virtual void OnPageInserted(PageCollectionChangedEventArgs e)
  {
    if (this.PageInserted == null)
      return;
    this.PageInserted((object) this, e);
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPageCollection.PageDeleted" /> event.
  /// </summary>
  /// <param name="e">An <see cref="T:Patagames.Pdf.Net.EventArguments.PageCollectionChangedEventArgs" /> that contains the event data.</param>
  protected virtual void OnPageDeleted(PageCollectionChangedEventArgs e)
  {
    if (this.PageDeleted == null)
      return;
    this.PageDeleted((object) this, e);
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPageCollection.PageLoaded" /> event.
  /// </summary>
  /// <param name="page">The page which was loaded</param>
  protected virtual void OnPageLoaded(PdfPage page)
  {
    if (this.PageLoaded == null)
      return;
    this.PageLoaded((object) this, new PdfPageEventArgs(page));
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPageCollection.PageDisposed" /> event.
  /// </summary>
  /// <param name="page">The page which was disposed</param>
  protected virtual void OnPageDisposed(PdfPage page)
  {
    if (this.PageDisposed == null)
      return;
    this.PageDisposed((object) this, new PdfPageEventArgs(page));
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfPageCollection.ProgressiveRender" /> event.
  /// </summary>
  /// <param name="page">The page which was rendered</param>
  /// <param name="e">An <see cref="T:Patagames.Pdf.Net.EventArguments.ProgressiveRenderEventArgs" /> that contains the event data.</param>
  protected virtual void OnProgressiveRender(PdfPage page, ProgressiveRenderEventArgs e)
  {
    if (this.ProgressiveRender == null)
      return;
    this.ProgressiveRender((object) page, e);
  }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets or sets the current index of a page in PdfPageCollection
  /// </summary>
  public int CurrentIndex
  {
    get
    {
      int count = this.Count;
      if (this._currentIndex < 0)
        this._currentIndex = 0;
      else if (this._currentIndex >= count)
        this._currentIndex = count - 1;
      return this._currentIndex;
    }
    set
    {
      if (this._currentIndex == value)
        return;
      this._currentIndex = value;
      int count = this.Count;
      if (this._currentIndex < 0)
        this._currentIndex = 0;
      else if (this._currentIndex >= count)
        this._currentIndex = count - 1;
      this.OnCurrentPageChanged(EventArgs.Empty);
    }
  }

  /// <summary>
  ///  Gets the current PdfPage item by <see cref="P:Patagames.Pdf.Net.PdfPageCollection.CurrentIndex" />
  /// </summary>
  public PdfPage CurrentPage => this[this.CurrentIndex];

  /// <summary>Gets total number of pages in a PDF document</summary>
  public new int Count => Pdfium.FPDF_GetPageCount(this._doc.Handle);

  /// <summary>Gets the page at the specified index.</summary>
  public new PdfPage this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      if (base.Count == 0)
        this.OndocumentLoaded();
      return base[index];
    }
    internal set => base[index] = value;
  }

  /// <summary>
  /// Initializes a new instance of the PdfPageCollection class.
  /// </summary>
  /// <param name="document">Document which contains this collection of destinations.</param>
  public PdfPageCollection(PdfDocument document)
  {
    this._doc = document;
    this.OndocumentLoaded();
  }

  private void OndocumentLoaded()
  {
    int count = this.Count;
    if (base.Count != 0 || count == 0)
      return;
    for (int page_index = 0; page_index < count; ++page_index)
    {
      PdfPage pdfPage = new PdfPage(this._doc, page_index);
      pdfPage.Loaded += new EventHandler(this.Page_Loaded);
      pdfPage.Disposed += new EventHandler(this.Page_Disposed);
      pdfPage.ProgressiveRender += new EventHandler<ProgressiveRenderEventArgs>(this.Page_ProgressiveRender);
      this.Add(pdfPage);
    }
  }

  /// <summary>Releases all resources used by the PdfPageCollection.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    for (int index = base.Count - 1; index >= 0; --index)
    {
      if (this[index].IsLoaded)
        this[index].Dispose();
      this[index].Loaded -= new EventHandler(this.Page_Loaded);
      this[index].Disposed -= new EventHandler(this.Page_Disposed);
      this[index].ProgressiveRender -= new EventHandler<ProgressiveRenderEventArgs>(this.Page_ProgressiveRender);
    }
    this.Clear();
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>
  /// Inserts a new page into the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which page should be inserted.</param>
  /// <param name="width">The page width.</param>
  /// <param name="height">The page height.</param>
  /// <returns>A PdfPage that has been inserted into a document.</returns>
  public PdfPage InsertPageAt(int index, float width, float height)
  {
    int count = this.Count;
    if (index < 0 || index > count)
      throw new IndexOutOfRangeException(string.Format(Error.err0006, (object) 0, (object) count));
    if (Pdfium.FPDFPage_New(this._doc.Handle, index, (double) width, (double) height) == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    PdfPage pdfPage = new PdfPage(this._doc, index);
    pdfPage.Loaded += new EventHandler(this.Page_Loaded);
    pdfPage.Disposed += new EventHandler(this.Page_Disposed);
    pdfPage.ProgressiveRender += new EventHandler<ProgressiveRenderEventArgs>(this.Page_ProgressiveRender);
    this.Insert(index, pdfPage);
    for (int index1 = index; index1 < count + 1; ++index1)
      this[index1].PageIndex = index1;
    this.OnPageInserted(new PageCollectionChangedEventArgs(index));
    return pdfPage;
  }

  /// <summary>
  /// Inserts a new page into the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which page should be inserted.</param>
  /// <param name="size">The page size</param>
  /// <returns>A PdfPage that has been inserted into a document.</returns>
  public PdfPage InsertPageAt(int index, FS_SIZEF size)
  {
    return this.InsertPageAt(index, size.Width, size.Height);
  }

  /// <summary>Import some pages to a PDF document.</summary>
  /// <param name="sourceDoc">A document to be imported.</param>
  /// <param name="pagerange">A page range string, Such as "1,3,5-7". If this parameter is NULL, it would import all pages in sourceDoc.</param>
  /// <param name="index">The page index wanted to insert from.</param>
  public void ImportPages(PdfDocument sourceDoc, string pagerange, int index)
  {
    int count = this.Count;
    if (index < 0 || index > count)
      throw new IndexOutOfRangeException(string.Format(Error.err0006, (object) 0, (object) count));
    if (!Pdfium.FPDF_ImportPages(this._doc.Handle, sourceDoc.Handle, pagerange, index))
      throw Pdfium.ProcessLastError();
    int num = this.Count - count;
    for (int index1 = index; index1 < count + num; ++index1)
    {
      if (index1 < index + num)
      {
        PdfPage pdfPage = new PdfPage(this._doc, index1);
        pdfPage.Loaded += new EventHandler(this.Page_Loaded);
        pdfPage.Disposed += new EventHandler(this.Page_Disposed);
        pdfPage.ProgressiveRender += new EventHandler<ProgressiveRenderEventArgs>(this.Page_ProgressiveRender);
        this.Insert(index1, pdfPage);
      }
      this[index1].PageIndex = index1;
    }
    this.OnPageInserted(new PageCollectionChangedEventArgs(index));
  }

  /// <summary>Delete a PDF page.</summary>
  /// <param name="index">The index of a page.</param>
  public void DeleteAt(int index)
  {
    int count = this.Count;
    if (index < 0 || index > count - 1)
      throw new IndexOutOfRangeException(string.Format(Error.err0006, (object) 0, (object) (count - 1)));
    if (this[index].IsLoaded)
      this[index].Dispose();
    this[index].Loaded -= new EventHandler(this.Page_Loaded);
    this[index].Disposed -= new EventHandler(this.Page_Disposed);
    this[index].ProgressiveRender -= new EventHandler<ProgressiveRenderEventArgs>(this.Page_ProgressiveRender);
    Pdfium.FPDFPage_Delete(this._doc.Handle, index);
    this.RemoveAt(index);
    for (int index1 = index; index1 < count - 1; ++index1)
      --this[index1].PageIndex;
    this.OnPageDeleted(new PageCollectionChangedEventArgs(index));
  }

  private void Page_Disposed(object sender, EventArgs e) => this.OnPageDisposed(sender as PdfPage);

  private void Page_Loaded(object sender, EventArgs e) => this.OnPageLoaded(sender as PdfPage);

  private void Page_ProgressiveRender(object sender, ProgressiveRenderEventArgs e)
  {
    this.OnProgressiveRender(sender as PdfPage, e);
  }

  /// <summary>Gets page index in PdfPageCollection</summary>
  /// <param name="page">PdfPage object</param>
  /// <returns>Page index in current collection or -1 if nothing found</returns>
  public int GetPageIndex(PdfPage page)
  {
    int count = this.Count;
    for (int index = 0; index < count; ++index)
    {
      if (page == this[index])
        return index;
    }
    return -1;
  }

  /// <summary>Gets page by specified handle</summary>
  /// <param name="page">Handle to the page object</param>
  /// <returns>Page in current collection or null if nothing found</returns>
  public PdfPage GetByHandle(IntPtr page)
  {
    return this.Find((Predicate<PdfPage>) (item => item.IsLoaded && item.Handle == page));
  }
}
