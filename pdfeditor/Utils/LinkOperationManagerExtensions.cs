// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.LinkOperationManagerExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Models.Annotations;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public static class LinkOperationManagerExtensions
{
  public static async void LinkDeleteAllUndo(
    Dictionary<int, List<BaseAnnotation>> LinkDic,
    PdfDocument pdfDocument)
  {
    int startPage = 0;
    int pageCount = pdfDocument.Pages.Count;
    Dictionary<int, List<BaseAnnotation>> dictionary = new Dictionary<int, List<BaseAnnotation>>();
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(pdfDocument);
    (int num1, int num2) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    for (int i = 0; i < LinkDic.Count; ++i)
    {
      PdfPage page = (PdfPage) null;
      IntPtr pageHandle = IntPtr.Zero;
      try
      {
        if (i >= num1 && i <= num2)
        {
          page = pdfDocument.Pages[i];
        }
        else
        {
          pageHandle = Pdfium.FPDF_LoadPage(pdfDocument.Handle, i);
          if (pageHandle != IntPtr.Zero)
            page = PdfPage.FromHandle(pdfDocument, pageHandle, i);
        }
        int[] array1 = LinkDic.Keys.ToArray<int>();
        BaseAnnotation[] array2 = LinkDic[array1[i]].ToArray();
        if (array1[i] >= startPage)
        {
          if (array1[i] < pageCount)
          {
            page = pdfDocument.Pages[array1[i]];
            BaseAnnotation[] baseAnnotationArray = array2;
            for (int index = 0; index < baseAnnotationArray.Length; ++index)
            {
              PdfAnnotation pdfAnnotation = AnnotationFactory.Create(page, baseAnnotationArray[index]);
              if (pdfAnnotation is PdfLinkAnnotation)
              {
                page.Annots.Add(pdfAnnotation);
                await page.TryRedrawPageAsync();
              }
            }
            baseAnnotationArray = (BaseAnnotation[]) null;
          }
        }
      }
      finally
      {
        if (pageHandle != IntPtr.Zero)
        {
          PageDisposeHelper.DisposePage(page);
          Pdfium.FPDF_ClosePage(pageHandle);
        }
      }
      page = (PdfPage) null;
    }
  }

  public static Dictionary<int, List<BaseAnnotation>> LinkDeleteAllRedo(PdfDocument pdfDocument)
  {
    int num1 = 0;
    int count = pdfDocument.Pages.Count;
    Dictionary<int, List<BaseAnnotation>> dictionary = new Dictionary<int, List<BaseAnnotation>>();
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(pdfDocument);
    (int num2, int num3) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    for (int index1 = num1; index1 < num1 + count; ++index1)
    {
      PdfPage page = (PdfPage) null;
      IntPtr num4 = IntPtr.Zero;
      try
      {
        if (index1 >= num2 && index1 <= num3)
        {
          page = pdfDocument.Pages[index1];
        }
        else
        {
          num4 = Pdfium.FPDF_LoadPage(pdfDocument.Handle, index1);
          if (num4 != IntPtr.Zero)
            page = PdfPage.FromHandle(pdfDocument, num4, index1);
        }
        if (page?.Annots != null)
        {
          if (page.Annots.Count > 0)
          {
            for (int index2 = page.Annots.Count - 1; index2 >= 0; --index2)
            {
              if (page.Annots[index2] is PdfLinkAnnotation)
              {
                BaseAnnotation baseAnnotation = AnnotationFactory.Create(page.Annots[index2]);
                List<BaseAnnotation> baseAnnotationList;
                if (!dictionary.TryGetValue(index1, out baseAnnotationList))
                {
                  baseAnnotationList = new List<BaseAnnotation>();
                  dictionary[index1] = baseAnnotationList;
                }
                baseAnnotationList.Add(baseAnnotation);
                page.Annots.RemoveAt(index2);
              }
            }
          }
        }
      }
      finally
      {
        if (num4 != IntPtr.Zero)
        {
          PageDisposeHelper.DisposePage(page);
          Pdfium.FPDF_ClosePage(num4);
        }
      }
    }
    return dictionary;
  }
}
