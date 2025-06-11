// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageContents.PageObjectHitTestCacheItem
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace PDFKit.Utils.PageContents;

internal class PageObjectHitTestCacheItem
{
  private const int BlockWidthDivisor = 3;
  private const int BlockHeightDivisor = 4;
  private readonly PdfPage page;
  private readonly FS_RECTF othersRectKey = new FS_RECTF(0.0f, -1f, -1f, 0.0f);
  private Dictionary<FS_RECTF, List<PageObjectHitTestCacheItem.PageObjectValue>> rectPageObjectIndexDict;

  private PageObjectHitTestCacheItem(PdfPage page)
  {
    this.rectPageObjectIndexDict = new Dictionary<FS_RECTF, List<PageObjectHitTestCacheItem.PageObjectValue>>();
    this.page = page;
    this.InitBlocks();
    this.CalcBlocks();
  }

  private void InitBlocks()
  {
    FS_RECTF effectiveBox = this.page.GetEffectiveBox();
    float num1 = effectiveBox.Width / 3f;
    float num2 = effectiveBox.Height / 4f;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 3; ++index2)
        this.rectPageObjectIndexDict[new FS_RECTF(effectiveBox.left + (float) index2 * num1, effectiveBox.bottom + (float) (index1 + 1) * num2, effectiveBox.left + (float) (index2 + 1) * num1, effectiveBox.bottom + (float) index1 * num2)] = new List<PageObjectHitTestCacheItem.PageObjectValue>();
    }
    this.rectPageObjectIndexDict[this.othersRectKey] = new List<PageObjectHitTestCacheItem.PageObjectValue>();
  }

  private void CalcBlocks()
  {
    if (this.page.PageObjects == null || this.page.PageObjects.Count == 0)
      return;
    FS_RECTF[] array = this.rectPageObjectIndexDict.Keys.Where<FS_RECTF>((Func<FS_RECTF, bool>) (c => c != this.othersRectKey)).ToArray<FS_RECTF>();
    for (int index1 = 0; index1 < this.page.PageObjects.Count; ++index1)
    {
      PdfPageObject pageObject = this.page.PageObjects[index1];
      FS_RECTF objectBoundingBox = PageObjectHitTestHelper.GetPageObjectBoundingBox(pageObject);
      PageObjectHitTestCacheItem.PageObjectValue pageObjectValue = new PageObjectHitTestCacheItem.PageObjectValue(index1, pageObject.ObjectType, objectBoundingBox);
      bool flag = false;
      for (int index2 = 0; index2 < array.Length; ++index2)
      {
        FS_RECTF fsRectf = array[index2];
        if (PageObjectHitTestCacheItem.IntersectsWithCore(fsRectf, objectBoundingBox))
        {
          flag = true;
          this.rectPageObjectIndexDict[fsRectf].Add(pageObjectValue);
        }
      }
      if (!flag)
        this.rectPageObjectIndexDict[this.othersRectKey].Add(pageObjectValue);
    }
  }

  public int[] GetPointObject(double x, double y)
  {
    Point p = new Point(x, y);
    foreach (KeyValuePair<FS_RECTF, List<PageObjectHitTestCacheItem.PageObjectValue>> keyValuePair in this.rectPageObjectIndexDict)
    {
      if (keyValuePair.Key != this.othersRectKey && AnnotationHitTestHelper.Contains(keyValuePair.Key, p))
        return keyValuePair.Value.Where<PageObjectHitTestCacheItem.PageObjectValue>((Func<PageObjectHitTestCacheItem.PageObjectValue, bool>) (c => AnnotationHitTestHelper.Contains(c.BoundingBox, p))).Select<PageObjectHitTestCacheItem.PageObjectValue, int>((Func<PageObjectHitTestCacheItem.PageObjectValue, int>) (c => c.Index)).ToArray<int>();
    }
    return Array.Empty<int>();
  }

  public int[] GetIntersecteObject(FS_RECTF rect)
  {
    List<int> intList = new List<int>();
    foreach (KeyValuePair<FS_RECTF, List<PageObjectHitTestCacheItem.PageObjectValue>> keyValuePair in this.rectPageObjectIndexDict)
    {
      if (keyValuePair.Key != this.othersRectKey && AnnotationHitTestHelper.IntersecteWith(keyValuePair.Key, rect))
        intList.AddRange(keyValuePair.Value.Where<PageObjectHitTestCacheItem.PageObjectValue>((Func<PageObjectHitTestCacheItem.PageObjectValue, bool>) (c => AnnotationHitTestHelper.IntersecteWith(c.BoundingBox, rect))).Select<PageObjectHitTestCacheItem.PageObjectValue, int>((Func<PageObjectHitTestCacheItem.PageObjectValue, int>) (c => c.Index)));
    }
    return intList.ToArray();
  }

  private static bool IntersectsWithCore(FS_RECTF rect1, FS_RECTF rect2)
  {
    return (double) rect1.left <= (double) rect2.right && (double) rect1.right > (double) rect2.left && (double) rect1.top > (double) rect2.bottom && (double) rect1.bottom <= (double) rect2.top;
  }

  public static PageObjectHitTestCacheItem Create(PdfPage page)
  {
    return page?.Document == null || page.Document.Handle == IntPtr.Zero ? (PageObjectHitTestCacheItem) null : new PageObjectHitTestCacheItem(page);
  }

  private struct PageObjectValue(int index, PageObjectTypes type, FS_RECTF boundingBox)
  {
    public int Index { get; } = index;

    public PageObjectTypes Type { get; } = type;

    public FS_RECTF BoundingBox { get; } = boundingBox;

    public void Deconstruct(out int index, out PageObjectTypes type, out FS_RECTF boundingBox)
    {
      index = this.Index;
      type = this.Type;
      boundingBox = this.BoundingBox;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<int, PageObjectTypes, FS_RECTF>(this.Index, this.Type, this.BoundingBox);
    }

    public override bool Equals(object obj)
    {
      return obj is PageObjectHitTestCacheItem.PageObjectValue pageObjectValue && pageObjectValue.Index == this.Index && pageObjectValue.Type == this.Type && pageObjectValue.BoundingBox == this.BoundingBox;
    }

    public static bool operator ==(
      PageObjectHitTestCacheItem.PageObjectValue left,
      PageObjectHitTestCacheItem.PageObjectValue right)
    {
      return left.Equals((object) right);
    }

    public static bool operator !=(
      PageObjectHitTestCacheItem.PageObjectValue left,
      PageObjectHitTestCacheItem.PageObjectValue right)
    {
      return !left.Equals((object) right);
    }
  }
}
