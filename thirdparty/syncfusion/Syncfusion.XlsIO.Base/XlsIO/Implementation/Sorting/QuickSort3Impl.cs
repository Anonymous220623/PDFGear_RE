// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.QuickSort3Impl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class QuickSort3Impl(object[][] data, Type[] types, OrderBy[] orderBy, Color[] colors) : 
  SortingAlgorithm(data, types, orderBy, colors)
{
  private const int CUTOFF = 10;

  public new void SortInt(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    int num1 = (int) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while ((int) this.m_data[++index][iColumn] < num1);
        do
          ;
        while (num1 <= (int) this.m_data[--right1][iColumn] && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((int) this.m_data[index][iColumn] == num1)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (num1 != (int) this.m_data[right1][iColumn]);
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num2 = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num2);
      --left4;
      ++num2;
    }
    this.SortInt(left, right2, iColumn);
    this.SortInt(num2, right, iColumn);
  }

  public new void SortFloat(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    double num1 = (double) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while ((double) this.m_data[++index][iColumn] < num1);
        do
          ;
        while (num1 <= (double) this.m_data[--right1][iColumn] && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((double) this.m_data[index][iColumn] == num1)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (num1 != (double) this.m_data[right1][iColumn]);
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num2 = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num2);
      --left4;
      ++num2;
    }
    this.SortFloat(left, right2, iColumn);
    this.SortFloat(num2, right, iColumn);
  }

  public new void SortDate(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    DateTime dateTime = (DateTime) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while ((DateTime) this.m_data[++index][iColumn] < dateTime);
        do
          ;
        while (dateTime <= (DateTime) this.m_data[--right1][iColumn] && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((DateTime) this.m_data[index][iColumn] == dateTime)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (!(dateTime == (DateTime) this.m_data[right1][iColumn]));
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num);
      --left4;
      ++num;
    }
    this.SortDate(left, right2, iColumn);
    this.SortDate(num, right, iColumn);
  }

  public new void SortString(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    string strB = (string) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while (((string) this.m_data[++index][iColumn]).CompareTo(strB) < 0);
        do
          ;
        while (strB.CompareTo((string) this.m_data[--right1][iColumn]) <= 0 && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((string) this.m_data[index][iColumn] == strB)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (!(strB == (string) this.m_data[right1][iColumn]));
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num);
      --left4;
      ++num;
    }
    this.SortString(left, right2, iColumn);
    this.SortString(num, right, iColumn);
  }

  public new void SortIntDesc(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    int num1 = (int) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while ((int) this.m_data[++index][iColumn] > num1);
        do
          ;
        while (num1 >= (int) this.m_data[--right1][iColumn] && right1 != left);
        if (index > right1)
        {
          this.SwapRow(index, right1);
          if ((int) this.m_data[index][iColumn] == num1)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (num1 != (int) this.m_data[right1][iColumn]);
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num2 = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num2);
      --left4;
      ++num2;
    }
    this.SortIntDesc(left, right2, iColumn);
    this.SortIntDesc(num2, right, iColumn);
  }

  public new void SortFloatDesc(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    double num1 = (double) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while ((double) this.m_data[++index][iColumn] > num1);
        do
          ;
        while (num1 >= (double) this.m_data[--right1][iColumn] && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((double) this.m_data[index][iColumn] == num1)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (num1 != (double) this.m_data[right1][iColumn]);
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num2 = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num2);
      --left4;
      ++num2;
    }
    this.SortFloatDesc(left, right2, iColumn);
    this.SortFloatDesc(num2, right, iColumn);
  }

  public new void SortDateDesc(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    DateTime dateTime = (DateTime) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while ((DateTime) this.m_data[++index][iColumn] > dateTime);
        do
          ;
        while (dateTime >= (DateTime) this.m_data[--right1][iColumn] && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((DateTime) this.m_data[index][iColumn] == dateTime)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (!(dateTime == (DateTime) this.m_data[right1][iColumn]));
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num);
      --left4;
      ++num;
    }
    this.SortDateDesc(left, right2, iColumn);
    this.SortDateDesc(num, right, iColumn);
  }

  public new void SortStringDesc(int left, int right, int iColumn)
  {
    if (right <= left)
      return;
    string strB = (string) this.m_data[right][iColumn];
    int index = left - 1;
    int right1 = right;
    int left1 = left - 1;
    int left2 = right;
    while (true)
    {
      do
      {
        do
          ;
        while (((string) this.m_data[++index][iColumn]).CompareTo(strB) > 0);
        do
          ;
        while (strB.CompareTo((string) this.m_data[--right1][iColumn]) >= 0 && right1 != left);
        if (index < right1)
        {
          this.SwapRow(index, right1);
          if ((string) this.m_data[index][iColumn] == strB)
          {
            ++left1;
            this.SwapRow(left1, index);
          }
        }
        else
          goto label_10;
      }
      while (!(strB == (string) this.m_data[right1][iColumn]));
      --left2;
      this.SwapRow(left2, right1);
    }
label_10:
    this.SwapRow(index, right);
    int right2 = index - 1;
    int num = index + 1;
    int left3 = left;
    while (left3 < left1)
    {
      this.SwapRow(left3, right2);
      ++left3;
      --right2;
    }
    int left4 = right - 1;
    while (left4 > left2)
    {
      this.SwapRow(left4, num);
      --left4;
      ++num;
    }
    this.SortStringDesc(left, right2, iColumn);
    this.SortStringDesc(num, right, iColumn);
  }

  public override void Sort(int left, int right, int columnIndex)
  {
    this.SortOnTypes(left, right, columnIndex);
  }

  public new void SortOnTypes(int left, int right, int columnIndex)
  {
    this.QuickSort(this.m_data, 0, this.m_data.Length - 1);
  }

  public new IRange Range
  {
    get => throw new NotSupportedException(nameof (Range));
    set => throw new NotSupportedException(nameof (Range));
  }

  private void QuickSort(object[][] data, int low, int high)
  {
    if (low + 10 > high)
    {
      this.InsertionSort(data, low, high);
    }
    else
    {
      int index1 = (low + high) / 2;
      if (this.CompareRows<object>(data[index1], data[low]) < 0)
        this.SwapReferences(data, low, index1);
      if (this.CompareRows<object>(data[high], data[low]) < 0)
        this.SwapReferences(data, low, high);
      if (this.CompareRows<object>(data[high], data[index1]) < 0)
        this.SwapReferences(data, index1, high);
      this.SwapReferences(data, index1, high - 1);
      int index2 = high - 1;
      int firstIndex = low;
      int secondIndex = high - 1;
      while (true)
      {
        do
          ;
        while (this.CompareRows<object>(data[++firstIndex], data[index2]) < 0);
        do
          ;
        while (this.CompareRows<object>(data[index2], data[--secondIndex]) < 0);
        if (firstIndex < secondIndex)
          this.SwapReferences(data, firstIndex, secondIndex);
        else
          break;
      }
      this.SwapReferences(data, firstIndex, high - 1);
      this.QuickSort(data, low, firstIndex - 1);
      this.QuickSort(data, firstIndex + 1, high);
    }
  }

  public void SwapReferences(object[][] data, int firstIndex, int secondIndex)
  {
    object[] objArray = data[firstIndex];
    data[firstIndex] = data[secondIndex];
    data[secondIndex] = objArray;
  }

  private void InsertionSort(object[][] data, int low, int high)
  {
    for (int index1 = low + 1; index1 <= high; ++index1)
    {
      object[] currentValue = data[index1];
      int index2;
      for (index2 = index1; index2 > low && this.CompareNumberInStringFormat(currentValue, data[index2 - 1]); --index2)
        data[index2] = data[index2 - 1];
      data[index2] = currentValue;
    }
  }

  private bool CompareNumberInStringFormat(object[] currentValue, object[] compareValue)
  {
    if (this.types[0].Name == "String")
    {
      double[] firstObject = new double[currentValue.Length];
      double[] secondObject = new double[compareValue.Length];
      if (double.TryParse(currentValue[1] as string, out firstObject[1]) && double.TryParse(compareValue[1] as string, out secondObject[1]))
        return this.CompareRows<double>(firstObject, secondObject) < 0;
    }
    return this.CompareRows<object>(currentValue, compareValue) < 0;
  }

  private int CompareRows<T>(T[] firstObject, T[] secondObject)
  {
    int num1 = firstObject.Length > secondObject.Length ? firstObject.Length : secondObject.Length;
    IComparer<T> comparer = (IComparer<T>) Comparer<T>.Default;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string str = double.NaN.ToString();
    for (int index = 1; index < num1; ++index)
    {
      if ((object) firstObject[index] != null)
        empty1 = firstObject[index].ToString();
      if ((object) secondObject[index] != null)
        empty2 = secondObject[index].ToString();
      if (this.orderBy[index - 1] == OrderBy.Ascending)
      {
        int num2 = empty1 == str || empty1 == string.Empty ? 1 : (empty2 == str || empty2 == string.Empty ? -1 : comparer.Compare(firstObject[index], secondObject[index]));
        if (num2 != 0)
          return num2;
      }
      else
      {
        int num3 = empty1 == str || empty1 == string.Empty ? 1 : (empty2 == str || empty2 == string.Empty ? -1 : comparer.Compare(secondObject[index], firstObject[index]));
        if (num3 != 0)
          return num3;
      }
    }
    return firstObject.Length.CompareTo(secondObject.Length);
  }
}
