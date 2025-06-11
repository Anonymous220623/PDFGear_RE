// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.CountableSortingHelper`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class CountableSortingHelper<TElement>
{
  internal abstract int CompareKeys(int index1, int index2);

  internal abstract void ComputeKeys(TElement[] elements, int count);

  private void QuickSort(int[] map, int left, int right)
  {
    do
    {
      int left1 = left;
      int right1 = right;
      int index1 = map[left1 + (right1 - left1 >> 1)];
      do
      {
        if (left1 < map.Length && this.CompareKeys(index1, map[left1]) > 0)
        {
          ++left1;
        }
        else
        {
          while (right1 >= 0 && this.CompareKeys(index1, map[right1]) < 0)
            --right1;
          if (left1 <= right1)
          {
            if (left1 < right1)
            {
              int num = map[left1];
              map[left1] = map[right1];
              map[right1] = num;
            }
            ++left1;
            --right1;
          }
          else
            break;
        }
      }
      while (left1 <= right1);
      if (right1 - left > right - left1)
      {
        if (left1 < right)
          this.QuickSort(map, left1, right);
        right = right1;
      }
      else
      {
        if (left < right1)
          this.QuickSort(map, left, right1);
        left = left1;
      }
    }
    while (left < right);
  }

  internal int[] Sort(TElement[] elements, int count)
  {
    this.ComputeKeys(elements, count);
    int[] map = new int[count];
    for (int index = 0; index < count; ++index)
      map[index] = index;
    this.QuickSort(map, 0, count - 1);
    return map;
  }
}
