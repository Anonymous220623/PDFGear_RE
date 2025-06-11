// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfSearch
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.EventArguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Provides methods to search the entire document asynchronously.
/// </summary>
public class PdfSearch
{
  private PdfDocument _document;
  private BackgroundWorker _searchWorker;
  private object _syncFoundText = new object();
  private List<PdfSearch.FoundText> _foundText = new List<PdfSearch.FoundText>();

  /// <summary>
  /// Occurs when the search operation has completed, has been canceled, or has raised an exception.
  /// </summary>
  /// <remarks>
  /// <para>If the operation completed successfully, you can access the result through the Result property which represents the array of <see cref="T:Patagames.Pdf.Net.PdfSearch.FoundText" /> objects</para>
  /// <para>The Error property of <see cref="T:System.ComponentModel.RunWorkerCompletedEventArgs" /> indicates that an exception was thrown by the operation.</para>
  /// <para>The Cancelled property of <see cref="T:System.ComponentModel.RunWorkerCompletedEventArgs" /> indicates whether a cancellation request was processed by the background operation.</para>
  /// </remarks>
  public event EventHandler<RunWorkerCompletedEventArgs> SearchCompleted;

  /// <summary>Occurs when progress is changed.</summary>
  public event EventHandler<ProgressChangedEventArgs> SearchProgressChanged;

  /// <summary>Occurs when the found text added into search results</summary>
  public event EventHandler<FoundTextAddedEventArgs> FoundTextAdded;

  /// <summary>
  /// Gets a value indicating whether the search operation is running.
  /// </summary>
  public bool IsBusy => this._searchWorker.IsBusy;

  /// <summary>Gets a copy of search results.</summary>
  public PdfSearch.FoundText[] FoundResults
  {
    get
    {
      lock (this._syncFoundText)
        return this._foundText.ToArray();
    }
  }

  /// <summary>
  /// Raises the <see cref="E:Patagames.Pdf.Net.PdfSearch.SearchCompleted" /> event.
  /// </summary>
  /// <param name="e">An RunWorkerCompletedEventArgs that contains the event data.</param>
  protected virtual void OnSearchCompleted(RunWorkerCompletedEventArgs e)
  {
    if (this.SearchCompleted == null)
      return;
    this.SearchCompleted((object) this, e);
  }

  /// <summary>
  ///  Raises the <see cref="E:Patagames.Pdf.Net.PdfSearch.SearchProgressChanged" /> event.
  /// </summary>
  /// <param name="e">An ProgressChangedEventArgs that contains the event data.</param>
  protected virtual void OnSearchProgressChanged(ProgressChangedEventArgs e)
  {
    if (this.SearchProgressChanged == null)
      return;
    this.SearchProgressChanged((object) this, e);
  }

  private void OnFoundTextAdded(FoundTextAddedEventArgs e)
  {
    if (this.FoundTextAdded == null)
      return;
    this.FoundTextAdded((object) this, e);
  }

  /// <summary>Initializes a new instance of the PdfSearch class.</summary>
  /// <param name="document">The instance of <see cref="T:Patagames.Pdf.Net.PdfDocument" /> class</param>
  public PdfSearch(PdfDocument document)
  {
    this._document = document;
    this._searchWorker = new BackgroundWorker();
    this._searchWorker.WorkerSupportsCancellation = true;
    this._searchWorker.WorkerReportsProgress = true;
    this._searchWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._searchWorker_RunWorkerCompleted);
    this._searchWorker.ProgressChanged += new ProgressChangedEventHandler(this._searchWorker_ProgressChanged);
    this._searchWorker.DoWork += new DoWorkEventHandler(this._searchWorker_DoWork);
  }

  /// <summary>Starts asynchronous execution of a search operation.</summary>
  /// <param name="findWhat">A string match pattern</param>
  /// <param name="flags">Option flags. See <see cref="T:Patagames.Pdf.Enums.FindFlags" /> for details.</param>
  /// <remarks>This method breaks the previous search operation, clears its results and then executes a new search operation.</remarks>
  public void Start(string findWhat, FindFlags flags)
  {
    this.End();
    do
      ;
    while (this.IsBusy);
    this._foundText.Clear();
    this._searchWorker.RunWorkerAsync((object) new object[2]
    {
      (object) findWhat,
      (object) flags
    });
  }

  /// <summary>Requests cancellation of a pending search operation.</summary>
  public void End() => this._searchWorker.CancelAsync();

  private void _searchWorker_DoWork(object sender, DoWorkEventArgs e)
  {
    string findWhat = (e.Argument as object[])[0] as string;
    FindFlags flags = (FindFlags) (e.Argument as object[])[1];
    int count = this._document.Pages.Count;
    for (int index = 0; index < count && !this._searchWorker.CancellationPending; ++index)
    {
      this._searchWorker.ReportProgress(index * 100 / count);
      using (PdfPage pdfPage = new PdfPage(this._document, index))
      {
        using (PdfFind find = pdfPage.Text.Find(findWhat, flags, 0))
        {
          if (find != null)
          {
            this.AddFound(index, find);
            while (find.FindNext())
            {
              if (!this._searchWorker.CancellationPending)
                this.AddFound(index, find);
              else
                break;
            }
          }
        }
      }
    }
    if (this._searchWorker.CancellationPending)
    {
      e.Cancel = true;
    }
    else
    {
      this._searchWorker.ReportProgress(100);
      lock (this._syncFoundText)
        e.Result = (object) this._foundText.ToArray();
    }
  }

  private void _searchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
    this.OnSearchCompleted(e);
  }

  private void _searchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
  {
    this.OnSearchProgressChanged(e);
  }

  private void AddFound(int page, PdfFind find)
  {
    PdfSearch.FoundText foundText = new PdfSearch.FoundText()
    {
      PageIndex = page,
      CharIndex = find.CharIndex,
      CharsCount = find.CharsCount
    };
    lock (this._syncFoundText)
      this._foundText.Add(foundText);
    this.OnFoundTextAdded(new FoundTextAddedEventArgs(foundText));
  }

  /// <summary>Represents a result of the search operation</summary>
  public struct FoundText
  {
    /// <summary>The page index of the search result.</summary>
    public int PageIndex;
    /// <summary>The starting character index of the search result.</summary>
    public int CharIndex;
    /// <summary>
    /// The number of matched characters in the search result.
    /// </summary>
    public int CharsCount;
  }
}
