// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Commets.PageCommetCollection
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Controls;
using pdfeditor.Models.Annotations;
using pdfeditor.Properties;
using pdfeditor.Utils;
using PDFKit.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Commets;

public class PageCommetCollection : 
  ObservableObject,
  System.Collections.Generic.IReadOnlyList<CommetModel>,
  IReadOnlyCollection<CommetModel>,
  IEnumerable<CommetModel>,
  IEnumerable,
  ITreeViewNode
{
  private readonly PdfDocument document;
  private readonly int pageIndex;
  private List<CommetModel> items;
  private bool isExpanded = true;

  private PageCommetCollection(PdfDocument document, int pageIndex)
  {
    this.document = document;
    this.pageIndex = pageIndex;
  }

  public PageCommetCollection(PdfDocument document, int pageIndex, List<CommetModel> models)
  {
    this.document = document;
    this.pageIndex = pageIndex;
    this.items = models;
  }

  public int PageIndex => this.pageIndex;

  public string DisplayPageIndex
  {
    get
    {
      return !Resources.LeftNavigationAnnotationPageLabelContent.Contains("XXX") ? $"{Resources.LeftNavigationAnnotationPageLabelContent} {this.pageIndex + 1}" : Resources.LeftNavigationAnnotationPageLabelContent.Replace("XXX", (this.pageIndex + 1).ToString()) + ":";
    }
  }

  public PdfDocument Document => this.document;

  public bool IsExpanded
  {
    get => this.isExpanded;
    set => this.SetProperty<bool>(ref this.isExpanded, value, nameof (IsExpanded));
  }

  public CommetModel this[int index] => this.items[index];

  public int Count => this.items.Count;

  public ITreeViewNode Parent => (ITreeViewNode) null;

  public IEnumerator<CommetModel> GetEnumerator()
  {
    return (IEnumerator<CommetModel>) this.items.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.items.GetEnumerator();

  public static PageCommetCollection Create(PdfDocument document, int pageIndex)
  {
    IEnumerable<CommetModel> core = PageCommetCollection.CreateCore(document, pageIndex);
    if (core == null)
      return (PageCommetCollection) null;
    PageCommetCollection commetCollection = new PageCommetCollection(document, pageIndex)
    {
      items = core.ToList<CommetModel>()
    };
    foreach (CommetModel commetModel in commetCollection.items)
      commetModel.Parent = (ITreeViewNode) commetCollection;
    return commetCollection;
  }

  private static IEnumerable<CommetModel> CreateCore(PdfDocument document, int pageIndex)
  {
    if (!document.IsDisposed && pageIndex < 0 || pageIndex >= document.Pages.Count)
      return (IEnumerable<CommetModel>) null;
    System.Collections.Generic.IReadOnlyList<BaseAnnotation> annots = (System.Collections.Generic.IReadOnlyList<BaseAnnotation>) null;
    PdfPage page1 = document.Pages[pageIndex];
    if (page1.IsLoaded)
    {
      lock (page1)
      {
        if (page1.IsLoaded)
        {
          if (page1.Annots != null)
          {
            if (page1.Annots.Count > 0)
              annots = AnnotationFactory.Create(page1);
          }
        }
      }
    }
    if (annots == null && !page1.IsLoaded)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        PageDisposeHelper.TryFixPageAnnotations(document, pageIndex);
        num = Pdfium.FPDF_LoadPage(document.Handle, pageIndex);
        PdfPage page2 = PdfPage.FromHandle(document, num, pageIndex);
        if (page2.Annots != null)
        {
          if (page2.Annots.Count > 0)
            annots = AnnotationFactory.Create(page2);
        }
      }
      finally
      {
        Pdfium.FPDF_ClosePage(num);
      }
    }
    if (annots != null && annots.Count > 0)
    {
      List<CommetModel> commets = PageCommetCollection.CreateCommets(annots);
      if (commets != null && commets.Count > 0)
        return (IEnumerable<CommetModel>) commets;
    }
    return (IEnumerable<CommetModel>) null;
  }

  private static List<CommetModel> CreateCommets(System.Collections.Generic.IReadOnlyList<BaseAnnotation> annots)
  {
    if (annots == null || annots.Count == 0)
      return new List<CommetModel>();
    IReadOnlyDictionary<BaseAnnotation, System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation>> annotationRepliesModel = CommetUtils.GetMarkupAnnotationRepliesModel(annots);
    List<CommetModel> commetModelList = new List<CommetModel>();
    foreach (BaseAnnotation annot in (IEnumerable<BaseAnnotation>) annots)
    {
      switch (annot)
      {
        case PopupAnnotation _:
        case WatermarkAnnotation _:
        case LinkAnnotation _:
        case NotImplementedAnnotation _:
          continue;
        default:
          System.Collections.Generic.IReadOnlyList<CommetModel> replies1 = (System.Collections.Generic.IReadOnlyList<CommetModel>) null;
          if (annot is BaseMarkupAnnotation key)
          {
            if (key.Relationship == RelationTypes.NonSpecified)
            {
              System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies2;
              if (annotationRepliesModel.TryGetValue((BaseAnnotation) key, out replies2))
                replies1 = PageCommetCollection.CreateReplies(replies2);
            }
            else
              continue;
          }
          CommetModel commetModel1 = CommetModel.TryCreate(annot, replies1);
          if (commetModel1 != null)
          {
            if (replies1 != null)
            {
              foreach (CommetModel commetModel2 in (IEnumerable<CommetModel>) replies1)
                commetModel2.Parent = (ITreeViewNode) commetModel1;
            }
            commetModelList.Add(commetModel1);
            continue;
          }
          continue;
      }
    }
    return commetModelList.Count <= 0 ? (List<CommetModel>) null : commetModelList;
  }

  private static System.Collections.Generic.IReadOnlyList<CommetModel> CreateReplies(
    System.Collections.Generic.IReadOnlyList<BaseMarkupAnnotation> replies)
  {
    if (replies == null || replies.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<CommetModel>) null;
    List<CommetModel> replies1 = new List<CommetModel>();
    for (int index = 0; index < replies.Count; ++index)
    {
      CommetModel commetModel = CommetModel.TryCreate((BaseAnnotation) replies[index], (System.Collections.Generic.IReadOnlyList<CommetModel>) null);
      if (commetModel != null)
        replies1.Add(commetModel);
    }
    return (System.Collections.Generic.IReadOnlyList<CommetModel>) replies1;
  }
}
