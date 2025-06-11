// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PageObjectOperationManagerExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Models.Operations;
using pdfeditor.Models.PageContents;
using PDFKit.Utils.PageContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

public static class PageObjectOperationManagerExtensions
{
  public static async Task<PdfTextObject> MoveTextObjectAsync(
    this OperationManager manager,
    PdfPage page,
    PdfTextObject textObject,
    FS_POINTF destLocation,
    string tag = "")
  {
    if (manager == null || page == null || textObject == null)
      return (PdfTextObject) null;
    PageObjectOperationManagerExtensions.TextObjectParentAccessor parentAccessor = PageObjectOperationManagerExtensions.TextObjectParentAccessor.Create(page, textObject);
    if (parentAccessor == null)
      return (PdfTextObject) null;
    FS_POINTF sourceLocation = textObject.Location;
    int pageIdx = page.PageIndex;
    int objIdx = textObject.Container.IndexOf((PdfPageObject) textObject);
    textObject.Location = destLocation;
    page.GenerateContentAdvance();
    await manager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      PdfPage page1 = doc.Pages[pageIdx];
      PdfPageObjectsCollection parent = parentAccessor.GetParent(page1);
      PdfTextObject pdfTextObject = (PdfTextObject) null;
      if (parent.Count > objIdx)
        pdfTextObject = parent[objIdx] as PdfTextObject;
      if (pdfTextObject == null)
        return;
      pdfTextObject.Location = sourceLocation;
      page1.GenerateContentAdvance();
      await page1.TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      PdfPage page2 = doc.Pages[pageIdx];
      PdfPageObjectsCollection parent = parentAccessor.GetParent(page2);
      PdfTextObject pdfTextObject = (PdfTextObject) null;
      if (parent.Count > objIdx)
        pdfTextObject = parent[objIdx] as PdfTextObject;
      if (pdfTextObject == null)
        return;
      pdfTextObject.Location = destLocation;
      page2.GenerateContentAdvance();
      await page2.TryRedrawPageAsync();
    }), tag);
    await page.TryRedrawPageAsync();
    return parentAccessor.GetParent(page)?[objIdx] as PdfTextObject;
  }

  public static async Task<PdfTextObject[]> ModifyTextObjectAsync(
    this OperationManager manager,
    PdfPage page,
    PdfTextObject textObject,
    string newText,
    string tag = "")
  {
    if (manager == null || page == null || textObject == null || textObject.TextUnicode == newText)
      return (PdfTextObject[]) null;
    PageObjectOperationManagerExtensions.TextObjectParentAccessor parentAccessor = PageObjectOperationManagerExtensions.TextObjectParentAccessor.Create(page, textObject);
    if (parentAccessor == null)
      return (PdfTextObject[]) null;
    PdfPageObjectsCollection container = textObject.Container;
    int pageIdx = page.PageIndex;
    int objIdx = container.IndexOf((PdfPageObject) textObject);
    PageTextObject sourceModel = (PageTextObject) PageContentFactory.Create((PdfPageObject) textObject);
    PdfTextObject[] objs = PageContentUtils.UpdateTextObjectContent(page, textObject, newText);
    PageTextObject[] destModels = ((IEnumerable<PdfTextObject>) objs).Select<PdfTextObject, PageBaseObject>((Func<PdfTextObject, PageBaseObject>) (c => PageContentFactory.Create((PdfPageObject) c))).OfType<PageTextObject>().ToArray<PageTextObject>();
    container.RemoveAt(objIdx);
    for (int index = 0; index < objs.Length; ++index)
      container.Insert(index + objIdx, (PdfPageObject) objs[index]);
    page.GenerateContentAdvance();
    await manager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      PdfPage page1 = doc.Pages[pageIdx];
      PdfPageObjectsCollection parent = parentAccessor.GetParent(page1);
      for (int index = destModels.Length - 1; index >= 0; --index)
        parent.RemoveAt(objIdx + index);
      string textUnicode = sourceModel.TextUnicode;
      FS_POINTF location = sourceModel.Location;
      double x = (double) location.X;
      location = sourceModel.Location;
      double y = (double) location.Y;
      PdfFont font = sourceModel.Font;
      double fontSize = (double) sourceModel.FontSize;
      PdfTextObject pageObject = PdfTextObject.Create(textUnicode, (float) x, (float) y, font, (float) fontSize);
      sourceModel.Apply((PdfPageObject) pageObject);
      parent.Insert(objIdx, (PdfPageObject) pageObject);
      page1.GenerateContentAdvance();
      await page1.TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      PdfPage page2 = doc.Pages[pageIdx];
      PdfPageObjectsCollection parent = parentAccessor.GetParent(page2);
      parent.RemoveAt(objIdx);
      PdfTextObject[] array = ((IEnumerable<PageTextObject>) destModels).Select<PageTextObject, PdfTextObject>((Func<PageTextObject, PdfTextObject>) (c =>
      {
        string textUnicode = c.TextUnicode;
        FS_POINTF location = c.Location;
        double x = (double) location.X;
        location = c.Location;
        double y = (double) location.Y;
        PdfFont font = c.Font;
        double fontSize = (double) c.FontSize;
        PdfTextObject pageObject = PdfTextObject.Create(textUnicode, (float) x, (float) y, font, (float) fontSize);
        c.Apply((PdfPageObject) pageObject);
        return pageObject;
      })).ToArray<PdfTextObject>();
      for (int index = 0; index < array.Length; ++index)
        parent.Insert(index + objIdx, (PdfPageObject) array[index]);
      page2.GenerateContentAdvance();
      await page2.TryRedrawPageAsync();
    }), tag);
    await page.TryRedrawPageAsync();
    PdfPageObjectsCollection parent1 = parentAccessor.GetParent(page);
    return parent1 != null ? parent1.Skip<PdfPageObject>(objIdx).Take<PdfPageObject>(objs.Length).OfType<PdfTextObject>().ToArray<PdfTextObject>() : (PdfTextObject[]) null;
  }

  public static async Task<bool> DeleteTextObjectAsync(
    this OperationManager manager,
    PdfPage page,
    PdfTextObject textObject,
    string tag = "")
  {
    if (manager == null || page == null || textObject == null)
      return false;
    PageObjectOperationManagerExtensions.TextObjectParentAccessor parentAccessor = PageObjectOperationManagerExtensions.TextObjectParentAccessor.Create(page, textObject);
    if (parentAccessor == null)
      return false;
    PdfPageObjectsCollection container = textObject.Container;
    int pageIdx = page.PageIndex;
    int objIdx = container.IndexOf((PdfPageObject) textObject);
    PageTextObject sourceModel = (PageTextObject) PageContentFactory.Create((PdfPageObject) textObject);
    container.RemoveAt(objIdx);
    page.GenerateContentAdvance();
    await manager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      PdfPage page1 = doc.Pages[pageIdx];
      PdfPageObjectsCollection parent = parentAccessor.GetParent(page1);
      string textUnicode = sourceModel.TextUnicode;
      FS_POINTF location = sourceModel.Location;
      double x = (double) location.X;
      location = sourceModel.Location;
      double y = (double) location.Y;
      PdfFont font = sourceModel.Font;
      double fontSize = (double) sourceModel.FontSize;
      PdfTextObject pageObject = PdfTextObject.Create(textUnicode, (float) x, (float) y, font, (float) fontSize);
      sourceModel.Apply((PdfPageObject) pageObject);
      int index = objIdx;
      PdfTextObject pdfTextObject = pageObject;
      parent.Insert(index, (PdfPageObject) pdfTextObject);
      page1.GenerateContentAdvance();
      await page1.TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      PdfPage page2 = doc.Pages[pageIdx];
      parentAccessor.GetParent(page2).RemoveAt(objIdx);
      page2.GenerateContentAdvance();
      await page2.TryRedrawPageAsync();
    }), tag);
    await page.TryRedrawPageAsync();
    return true;
  }

  private class TextObjectParentAccessor
  {
    private System.Collections.Generic.IReadOnlyList<int> objectIndexes;

    private TextObjectParentAccessor(int pageIndex, System.Collections.Generic.IReadOnlyList<int> objectIndexes)
    {
      this.PageIndex = pageIndex;
      this.objectIndexes = objectIndexes;
    }

    public int PageIndex { get; }

    public PdfPageObjectsCollection GetParent(PdfPage page)
    {
      if (page == null)
        return (PdfPageObjectsCollection) null;
      PdfPageObjectsCollection pageObjects = page.PageObjects;
      for (int index = this.objectIndexes.Count - 1; index >= 0; --index)
      {
        int objectIndex = this.objectIndexes[index];
        if (!(pageObjects[objectIndex] is PdfFormObject pdfFormObject))
          throw new ArgumentException("formObj");
        pageObjects = pdfFormObject.PageObjects;
      }
      return pageObjects;
    }

    public static PageObjectOperationManagerExtensions.TextObjectParentAccessor Create(
      PdfPage page,
      PdfTextObject textObj)
    {
      if (page == null || textObj == null || textObj.Container == null)
        return (PageObjectOperationManagerExtensions.TextObjectParentAccessor) null;
      List<int> objectIndexes = new List<int>();
      PdfPageObjectsCollection container = textObj.Container;
      int count = container.Count;
      PdfPageObject textObj1 = (PdfPageObject) textObj;
      do
      {
        if (textObj1 != textObj)
        {
          int num = PageObjectOperationManagerExtensions.TextObjectParentAccessor.IndexOf(container, textObj1);
          if (num == -1)
            throw new ArgumentException("idx");
          objectIndexes.Add(num);
        }
        if (!(container.Handle == page.PageObjects.Handle))
        {
          textObj1 = (PdfPageObject) container.Form;
          container = textObj1?.Container;
        }
        else
          goto label_10;
      }
      while (container != null);
      return (PageObjectOperationManagerExtensions.TextObjectParentAccessor) null;
label_10:
      return new PageObjectOperationManagerExtensions.TextObjectParentAccessor(page.PageIndex, (System.Collections.Generic.IReadOnlyList<int>) objectIndexes);
    }

    private static int IndexOf(PdfPageObjectsCollection objCollection, PdfPageObject textObj)
    {
      if (objCollection == null || textObj == null)
        return -1;
      for (int index = 0; index < objCollection.Count; ++index)
      {
        if (textObj.Handle == objCollection[index].Handle)
          return index;
      }
      return -1;
    }
  }
}
