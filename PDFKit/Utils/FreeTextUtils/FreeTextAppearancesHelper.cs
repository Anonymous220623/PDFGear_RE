// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.FreeTextUtils.FreeTextAppearancesHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace PDFKit.Utils.FreeTextUtils;

internal class FreeTextAppearancesHelper
{
  private static FreeTextAppearancesHelper.CtsCollection ctsCollection = new FreeTextAppearancesHelper.CtsCollection();

  internal static FreeTextAppearancesHelper.CtsCollection CancellationTokenSources
  {
    get => FreeTextAppearancesHelper.ctsCollection;
  }

  public static async Task WaitForAnnotationGenerateAsync()
  {
    while (true)
    {
      if (FreeTextAppearancesHelper.ctsCollection.Count != 0)
        await Task.Delay(10);
      else
        break;
    }
  }

  internal class CtsCollection
  {
    private ConcurrentDictionary<WeakReference<PdfFreeTextAnnotation>, CancellationTokenSource> freeTextRegenCtsDict;

    public CtsCollection()
    {
      this.freeTextRegenCtsDict = new ConcurrentDictionary<WeakReference<PdfFreeTextAnnotation>, CancellationTokenSource>();
    }

    public int Count => this.freeTextRegenCtsDict.Count;

    public CancellationTokenSource GetOrCreateCts(PdfFreeTextAnnotation annot)
    {
      lock (this.freeTextRegenCtsDict)
      {
        if ((PdfWrapper) annot == (PdfWrapper) null)
          return (CancellationTokenSource) null;
        foreach (KeyValuePair<WeakReference<PdfFreeTextAnnotation>, CancellationTokenSource> keyValuePair in this.freeTextRegenCtsDict)
        {
          PdfFreeTextAnnotation target;
          if (keyValuePair.Key.TryGetTarget(out target) && (PdfWrapper) target == (PdfWrapper) annot)
            return keyValuePair.Value;
        }
        CancellationTokenSource cts = new CancellationTokenSource();
        this.freeTextRegenCtsDict[new WeakReference<PdfFreeTextAnnotation>(annot)] = cts;
        return cts;
      }
    }

    public bool IsFree(PdfFreeTextAnnotation annot)
    {
      foreach (WeakReference<PdfFreeTextAnnotation> key in (IEnumerable<WeakReference<PdfFreeTextAnnotation>>) this.freeTextRegenCtsDict.Keys)
      {
        PdfFreeTextAnnotation target;
        if (key.TryGetTarget(out target) && (PdfWrapper) target == (PdfWrapper) annot)
          return this.freeTextRegenCtsDict[key].IsCancellationRequested;
      }
      return true;
    }

    public void CancelCts(PdfFreeTextAnnotation annot)
    {
      lock (this.freeTextRegenCtsDict)
      {
        if ((PdfWrapper) annot == (PdfWrapper) null)
          return;
        WeakReference<PdfFreeTextAnnotation> key1 = (WeakReference<PdfFreeTextAnnotation>) null;
        foreach (WeakReference<PdfFreeTextAnnotation> key2 in (IEnumerable<WeakReference<PdfFreeTextAnnotation>>) this.freeTextRegenCtsDict.Keys)
        {
          PdfFreeTextAnnotation target;
          if (key2.TryGetTarget(out target) && (PdfWrapper) target == (PdfWrapper) annot)
            key1 = key2;
        }
        CancellationTokenSource cancellationTokenSource;
        if (key1 == null || !this.freeTextRegenCtsDict.TryRemove(key1, out cancellationTokenSource) || cancellationTokenSource.IsCancellationRequested)
          return;
        cancellationTokenSource.Cancel();
      }
    }
  }
}
