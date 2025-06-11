// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.BatchPrintDocumentModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Patagames.Pdf.Net;
using pdfeditor.Models;
using pdfeditor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Views;

public class BatchPrintDocumentModel : ObservableObject, IDisposable
{
  private string fileName;
  private bool isSelected;
  private int selectedPageCount;
  private string pageRange;
  private int currentPage;
  private BatchPrintDocumentModel.PageRangeEnum pageRangeMode;
  private BatchPrintDocumentModel.SubsetEnum subsetMode;
  private ICommand removeCmd;
  private bool disposedValue;

  public BatchPrintDocumentModel(string fileName, PdfDocument doc)
  {
    this.fileName = fileName;
    this.Document = doc;
    this.pageRangeMode = BatchPrintDocumentModel.PageRangeEnum.AllPages;
    this.subsetMode = BatchPrintDocumentModel.SubsetEnum.AllPages;
    this.pageRange = doc.Pages.Count == 1 ? "1" : $"1-{doc.Pages.Count}";
    this.DocumentTotalPageCount = doc.Pages.Count;
    this.UpdateSelectedPageCount();
  }

  public BatchPrintDocumentModel(DocumentWrapper wrapper)
    : this(Path.GetFileName(wrapper.DocumentPath), wrapper.Document)
  {
    this.DocumentWrapper = wrapper;
  }

  public PdfDocument Document { get; private set; }

  public DocumentWrapper DocumentWrapper { get; private set; }

  public string FileName
  {
    get => this.fileName;
    set => this.SetProperty<string>(ref this.fileName, value, nameof (FileName));
  }

  public bool IsSelected
  {
    get => this.isSelected;
    set => this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
  }

  public int DocumentTotalPageCount { get; }

  public int SelectedPageCount
  {
    get => this.selectedPageCount;
    set => this.SetProperty<int>(ref this.selectedPageCount, value, nameof (SelectedPageCount));
  }

  public string PageRange
  {
    get => this.pageRange;
    set
    {
      if (!this.SetProperty<string>(ref this.pageRange, value, nameof (PageRange)))
        return;
      this.UpdateSelectedPageCount();
    }
  }

  public int CurrentPage
  {
    get => this.currentPage;
    set
    {
      if (!this.SetProperty<int>(ref this.currentPage, value, nameof (CurrentPage)))
        return;
      this.UpdateSelectedPageCount();
    }
  }

  public BatchPrintDocumentModel.PageRangeEnum PageRangeMode
  {
    get => this.pageRangeMode;
    set
    {
      if (!this.SetProperty<BatchPrintDocumentModel.PageRangeEnum>(ref this.pageRangeMode, value, nameof (PageRangeMode)))
        return;
      this.UpdateSelectedPageCount();
    }
  }

  public BatchPrintDocumentModel.SubsetEnum SubsetMode
  {
    get => this.subsetMode;
    set
    {
      if (!this.SetProperty<BatchPrintDocumentModel.SubsetEnum>(ref this.subsetMode, value, nameof (SubsetMode)))
        return;
      this.UpdateSelectedPageCount();
    }
  }

  public ICommand RemoveCmd
  {
    get => this.removeCmd;
    set => this.SetProperty<ICommand>(ref this.removeCmd, value, nameof (RemoveCmd));
  }

  private void UpdateSelectedPageCount()
  {
    int pageCount = this.Document.Pages.Count;
    if (this.PageRangeMode == BatchPrintDocumentModel.PageRangeEnum.AllPages)
    {
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
        this.SelectedPageCount = pageCount;
      else if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Odd)
      {
        this.SelectedPageCount = pageCount / 2 + pageCount % 2;
      }
      else
      {
        if (this.SubsetMode != BatchPrintDocumentModel.SubsetEnum.Even)
          return;
        this.SelectedPageCount = pageCount / 2;
      }
    }
    else if (this.PageRangeMode == BatchPrintDocumentModel.PageRangeEnum.CurrentPage)
    {
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
        this.SelectedPageCount = 1;
      else if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Odd)
      {
        this.SelectedPageCount = this.CurrentPage % 2 == 1 ? 1 : 0;
      }
      else
      {
        if (this.SubsetMode != BatchPrintDocumentModel.SubsetEnum.Even)
          return;
        this.SelectedPageCount = this.CurrentPage % 2 == 0 ? 1 : 0;
      }
    }
    else
    {
      if (this.PageRangeMode != BatchPrintDocumentModel.PageRangeEnum.SelectedPages)
        return;
      int[] pageIndexes;
      if (PdfObjectExtensions.TryParsePageRange(this.PageRange, out pageIndexes, out int _) && ((IEnumerable<int>) pageIndexes).All<int>((Func<int, bool>) (c => c < pageCount)))
      {
        if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
          this.SelectedPageCount = ((IEnumerable<int>) pageIndexes).Count<int>();
        else if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Odd)
        {
          this.SelectedPageCount = ((IEnumerable<int>) pageIndexes).Count<int>((Func<int, bool>) (c => (c + 1) % 2 == 1));
        }
        else
        {
          if (this.SubsetMode != BatchPrintDocumentModel.SubsetEnum.Even)
            return;
          this.SelectedPageCount = ((IEnumerable<int>) pageIndexes).Count<int>((Func<int, bool>) (c => (c + 1) % 2 == 0));
        }
      }
      else
        this.SelectedPageCount = 0;
    }
  }

  public string GetActualPageRange(out int[] indexes)
  {
    int pageCount = this.Document.Pages.Count;
    indexes = (int[]) null;
    if (this.PageRangeMode == BatchPrintDocumentModel.PageRangeEnum.AllPages)
    {
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
      {
        indexes = Enumerable.Range(1, pageCount).ToArray<int>();
        return pageCount != 1 ? $"1-{pageCount}" : "1";
      }
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Odd)
      {
        indexes = Enumerable.Range(0, pageCount).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
        return ((IEnumerable<int>) indexes).ConvertToRange();
      }
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Even)
      {
        indexes = Enumerable.Range(0, pageCount).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
        return ((IEnumerable<int>) indexes).ConvertToRange();
      }
    }
    else if (this.PageRangeMode == BatchPrintDocumentModel.PageRangeEnum.CurrentPage)
    {
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
      {
        indexes = new int[1]{ this.CurrentPage };
        return $"{this.CurrentPage}";
      }
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Odd)
      {
        ref int[] local = ref indexes;
        int[] numArray;
        if (this.CurrentPage % 2 != 1)
          numArray = (int[]) null;
        else
          numArray = new int[1]{ this.CurrentPage };
        local = numArray;
        return this.CurrentPage % 2 != 1 ? string.Empty : $"{this.CurrentPage}";
      }
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Even)
      {
        ref int[] local = ref indexes;
        int[] numArray;
        if (this.CurrentPage % 2 != 0)
          numArray = (int[]) null;
        else
          numArray = new int[1]{ this.CurrentPage };
        local = numArray;
        return this.CurrentPage % 2 != 0 ? string.Empty : $"{this.CurrentPage}";
      }
    }
    else if (this.PageRangeMode == BatchPrintDocumentModel.PageRangeEnum.SelectedPages && PdfObjectExtensions.TryParsePageRange(this.PageRange, out indexes, out int _) && ((IEnumerable<int>) indexes).All<int>((Func<int, bool>) (c => c < pageCount)))
    {
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.AllPages)
        return ((IEnumerable<int>) indexes).ConvertToRange();
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Odd)
      {
        indexes = ((IEnumerable<int>) indexes).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
        return ((IEnumerable<int>) indexes).ConvertToRange();
      }
      if (this.SubsetMode == BatchPrintDocumentModel.SubsetEnum.Even)
      {
        indexes = ((IEnumerable<int>) indexes).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
        return ((IEnumerable<int>) indexes).ConvertToRange();
      }
    }
    return string.Empty;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      this.DocumentWrapper?.Dispose();
      this.DocumentWrapper = (DocumentWrapper) null;
      this.Document = (PdfDocument) null;
    }
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public enum PageRangeEnum
  {
    AllPages,
    CurrentPage,
    SelectedPages,
  }

  public enum SubsetEnum
  {
    AllPages,
    Odd,
    Even,
  }
}
