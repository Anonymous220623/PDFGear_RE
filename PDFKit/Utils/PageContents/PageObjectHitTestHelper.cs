// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PageObjectHitTestHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.PageContents;

public static class PageObjectHitTestHelper
{
  private static PageObjectTypes[] allTypes = new PageObjectTypes[4]
  {
    PageObjectTypes.PDFPAGE_FORM,
    PageObjectTypes.PDFPAGE_IMAGE,
    PageObjectTypes.PDFPAGE_PATH,
    PageObjectTypes.PDFPAGE_TEXT
  };
  private static HashSet<PageObjectTypes> allTypeHash = new HashSet<PageObjectTypes>(((IEnumerable<PageObjectTypes>) PageObjectHitTestHelper.allTypes).Distinct<PageObjectTypes>());
  private static Dictionary<WeakReference<PdfDocument>, PageObjectHitTestCache> docHitTestCacheDict = new Dictionary<WeakReference<PdfDocument>, PageObjectHitTestCache>();

  public static PdfPageObject[] GetPointObjects(
    PdfPage page,
    Point pagePoint,
    PageObjectTypes[] supportedTypes)
  {
    return page == null ? (PdfPageObject[]) null : PageObjectHitTestHelper.GetPointObjects(page.Document, page.PageIndex, pagePoint, supportedTypes);
  }

  public static PdfPageObject[] GetPointObjects(
    PdfDocument doc,
    int pageIndex,
    Point pagePoint,
    PageObjectTypes[] supportedTypes)
  {
    return PageObjectHitTestHelper.GetObjectsCore(doc, pageIndex, (Func<PageObjectHitTestCache, int[]>) (_cache => _cache.GetPointObjectIndexes(pageIndex, pagePoint.X, pagePoint.Y)), (Func<PdfPageObject, bool>) (_obj => AnnotationHitTestHelper.Contains(PageObjectHitTestHelper.GetPageObjectBoundingBox(_obj), pagePoint)), supportedTypes);
  }

  public static PdfPageObject[] GetIntersectingObjects(
    PdfDocument doc,
    int pageIndex,
    FS_RECTF rect,
    PageObjectTypes[] supportedTypes)
  {
    return PageObjectHitTestHelper.GetObjectsCore(doc, pageIndex, (Func<PageObjectHitTestCache, int[]>) (_cache => _cache.GetIntersectingObjectIndexes(pageIndex, rect)), (Func<PdfPageObject, bool>) (_obj => AnnotationHitTestHelper.IntersecteWith(PageObjectHitTestHelper.GetPageObjectBoundingBox(_obj), rect)), supportedTypes);
  }

  private static PdfPageObject[] GetObjectsCore(
    PdfDocument doc,
    int pageIndex,
    Func<PageObjectHitTestCache, int[]> indexGetter,
    Func<PdfPageObject, bool> predicate,
    PageObjectTypes[] supportedTypes)
  {
    if (doc == null || doc.IsDisposed || doc.Handle == IntPtr.Zero)
      return (PdfPageObject[]) null;
    PageObjectHitTestCache hitTestCacheCore = PageObjectHitTestHelper.GetHitTestCacheCore(doc);
    if (supportedTypes == null || supportedTypes.Length == 0)
      supportedTypes = PageObjectHitTestHelper.allTypes;
    HashSet<PageObjectTypes> supportedTypes1 = new HashSet<PageObjectTypes>(((IEnumerable<PageObjectTypes>) supportedTypes).Distinct<PageObjectTypes>());
    int[] numArray = indexGetter(hitTestCacheCore);
    List<PdfPageObject> source = new List<PdfPageObject>();
    if (numArray != null && numArray.Length != 0)
    {
      PdfPage page = doc.Pages[pageIndex];
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        int index2 = numArray[index1];
        System.Collections.Generic.IReadOnlyList<PdfPageObject> actualObjs;
        if (index2 >= 0 && index2 < page.PageObjects.Count && PageObjectHitTestHelper.ProcessPageObject(page.PageObjects[index2], predicate, supportedTypes1, out actualObjs) && actualObjs != null)
          source.AddRange((IEnumerable<PdfPageObject>) actualObjs);
      }
    }
    return source.Distinct<PdfPageObject>().ToArray<PdfPageObject>();
  }

  internal static PageObjectHitTestCache GetHitTestCacheCore(PdfDocument doc)
  {
    if (doc == null || doc.IsDisposed || doc.Handle == IntPtr.Zero)
      return (PageObjectHitTestCache) null;
    PageObjectHitTestCache hitTestCacheCore = (PageObjectHitTestCache) null;
    lock (PageObjectHitTestHelper.docHitTestCacheDict)
    {
      List<WeakReference<PdfDocument>> weakReferenceList = new List<WeakReference<PdfDocument>>();
      foreach (KeyValuePair<WeakReference<PdfDocument>, PageObjectHitTestCache> keyValuePair in PageObjectHitTestHelper.docHitTestCacheDict)
      {
        PdfDocument target;
        if (keyValuePair.Key.TryGetTarget(out target) && target != null && !target.IsDisposed && target.Handle == doc.Handle)
          hitTestCacheCore = keyValuePair.Value;
        else
          weakReferenceList.Add(keyValuePair.Key);
      }
      foreach (WeakReference<PdfDocument> key in weakReferenceList)
        PageObjectHitTestHelper.docHitTestCacheDict.Remove(key);
      if (hitTestCacheCore == null)
      {
        hitTestCacheCore = new PageObjectHitTestCache(doc);
        PageObjectHitTestHelper.docHitTestCacheDict[new WeakReference<PdfDocument>(doc)] = hitTestCacheCore;
      }
    }
    return hitTestCacheCore;
  }

  public static bool HitTest(
    PdfPage page,
    PdfPageObject pageObj,
    Point devicePoint,
    PageObjectTypes[] supportedTypes,
    out System.Collections.Generic.IReadOnlyList<PdfPageObject> actualObjs)
  {
    actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) null;
    if (page == null || pageObj == null)
      return false;
    if (supportedTypes == null)
      supportedTypes = PageObjectHitTestHelper.allTypes;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(page.Document);
    if (pdfControl != null)
    {
      Point pp;
      if (pdfControl.TryGetPagePoint(page.PageIndex, devicePoint, out pp))
      {
        HashSet<PageObjectTypes> supportedTypes1 = new HashSet<PageObjectTypes>(((IEnumerable<PageObjectTypes>) supportedTypes).Distinct<PageObjectTypes>());
        return PageObjectHitTestHelper.ProcessPageObject(pageObj, (Func<PdfPageObject, bool>) (_obj => AnnotationHitTestHelper.Contains(PageObjectHitTestHelper.GetPageObjectBoundingBox(_obj), pp)), supportedTypes1, out actualObjs);
      }
    }
    return false;
  }

  private static bool ProcessPageObject(
    PdfPageObject pageObj,
    Func<PdfPageObject, bool> predicate,
    HashSet<PageObjectTypes> supportedTypes,
    out System.Collections.Generic.IReadOnlyList<PdfPageObject> actualObjs)
  {
    actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) null;
    if (pageObj == null)
      return false;
    bool flag = false;
    if (supportedTypes == null)
      supportedTypes = PageObjectHitTestHelper.allTypeHash;
    if (pageObj.ObjectType == PageObjectTypes.PDFPAGE_FORM)
      flag = PageObjectHitTestHelper.ProcessFormObject(pageObj as PdfFormObject, predicate, supportedTypes, out actualObjs);
    else if (supportedTypes.Contains(PageObjectTypes.PDFPAGE_TEXT) && pageObj.ObjectType == PageObjectTypes.PDFPAGE_TEXT)
    {
      flag = PageObjectHitTestHelper.ProcessNormalObject(pageObj, predicate);
      if (flag)
        actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) new PdfPageObject[1]
        {
          pageObj
        };
    }
    else if (supportedTypes.Contains(PageObjectTypes.PDFPAGE_IMAGE) && pageObj.ObjectType == PageObjectTypes.PDFPAGE_IMAGE)
    {
      flag = PageObjectHitTestHelper.ProcessNormalObject(pageObj, predicate);
      if (flag)
        actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) new PdfPageObject[1]
        {
          pageObj
        };
    }
    else if (supportedTypes.Contains(PageObjectTypes.PDFPAGE_PATH) && pageObj.ObjectType == PageObjectTypes.PDFPAGE_PATH)
    {
      flag = PageObjectHitTestHelper.ProcessNormalObject(pageObj, predicate);
      if (flag)
        actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) new PdfPageObject[1]
        {
          pageObj
        };
    }
    return flag;
  }

  private static bool ProcessFormObject(
    PdfFormObject pageObj,
    Func<PdfPageObject, bool> predicate,
    HashSet<PageObjectTypes> supportedTypes,
    out System.Collections.Generic.IReadOnlyList<PdfPageObject> actualObjs)
  {
    actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) null;
    if (pageObj == null || !predicate((PdfPageObject) pageObj))
      return false;
    if (pageObj.PageObjects == null || pageObj.PageObjects.Count == 0)
    {
      if (!supportedTypes.Contains(PageObjectTypes.PDFPAGE_FORM))
        return false;
      actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) new PdfFormObject[1]
      {
        pageObj
      };
      return true;
    }
    List<PdfPageObject> pdfPageObjectList = new List<PdfPageObject>();
    foreach (PdfPageObject pageObject in pageObj.PageObjects)
    {
      if (PageObjectHitTestHelper.ProcessPageObject(pageObject, predicate, supportedTypes, out actualObjs))
        pdfPageObjectList.AddRange((IEnumerable<PdfPageObject>) actualObjs);
    }
    if (supportedTypes.Contains(PageObjectTypes.PDFPAGE_FORM))
      pdfPageObjectList.Add((PdfPageObject) pageObj);
    actualObjs = (System.Collections.Generic.IReadOnlyList<PdfPageObject>) pdfPageObjectList;
    return pdfPageObjectList.Count > 0;
  }

  private static bool ProcessNormalObject(
    PdfPageObject pageObj,
    Func<PdfPageObject, bool> predicate)
  {
    return pageObj != null && predicate(pageObj);
  }

  internal static FS_RECTF GetPageObjectBoundingBox(PdfPageObject pageObj)
  {
    Matrix matrix = Matrix.Identity;
    if (pageObj.Container?.Form != null)
      matrix = pageObj.Container.Form.MatrixFromPage2();
    float left;
    float top;
    float right;
    float bottom;
    Pdfium.FPDFPageObj_GetBBox(pageObj.Handle, (FS_MATRIX) null, out left, out top, out right, out bottom);
    Point point1 = matrix.Transform(new Point((double) left, (double) top));
    Point point2 = matrix.Transform(new Point((double) right, (double) bottom));
    return new FS_RECTF(Math.Min(point1.X, point2.X), Math.Max(point1.Y, point2.Y), Math.Max(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
  }
}
