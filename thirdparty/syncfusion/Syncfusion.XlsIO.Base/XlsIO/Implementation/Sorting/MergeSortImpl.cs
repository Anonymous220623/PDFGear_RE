// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.MergeSortImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class MergeSortImpl(object[][] data, Type[] types, OrderBy[] orderBy, Color[] colors) : 
  SortingAlgorithm(data, types, orderBy, colors)
{
  public object[][] SortOnTypes(object[][] arrValues, int columnIndex)
  {
    if (this.types[columnIndex - 1] == typeof (int))
      return this.orderBy[columnIndex - 1] == OrderBy.Ascending ? this.SortInt(arrValues, columnIndex) : this.SortIntDesc(arrValues, columnIndex);
    if (this.types[columnIndex - 1] == typeof (double))
      return this.orderBy[columnIndex - 1] == OrderBy.Ascending ? this.SortFloat(arrValues, columnIndex) : this.SortFloatDesc(arrValues, columnIndex);
    if (this.types[columnIndex - 1] == typeof (string))
      return this.orderBy[columnIndex - 1] == OrderBy.Ascending ? this.SortString(arrValues, columnIndex) : this.SortStringDesc(arrValues, columnIndex);
    if (!(this.types[columnIndex - 1] == typeof (DateTime)))
      return new object[0][];
    return this.orderBy[columnIndex - 1] == OrderBy.Ascending ? this.SortDate(arrValues, columnIndex) : this.SortDateDesc(arrValues, columnIndex);
  }

  public object[][] GetRange(object[][] arrData, int startIndex, int endIndex)
  {
    object[][] range = new object[endIndex - startIndex][];
    int index1 = 0;
    for (int index2 = startIndex; index2 < endIndex; ++index2)
    {
      range[index1] = new object[arrData[0].Length];
      range[index1++] = arrData[index2];
    }
    return range;
  }

  public void AddRange(object[][] destArray, object[][] srcArray, int startIndex)
  {
    for (int index = 0; index < srcArray.Length; ++index)
      destArray[startIndex++] = srcArray[index];
  }

  public object[][] SortInt(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int num1 = arrData.Length / 2;
    int startIndex = 0;
    object[][] range1 = this.GetRange(arrData, 0, num1);
    object[][] range2 = this.GetRange(arrData, num1, arrData.Length);
    object[][] objArray1 = this.SortInt(range1, columnIndex);
    object[][] objArray2 = this.SortInt(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else
      {
        int num2 = (int) objArray1[index1][columnIndex];
        int num3 = (int) objArray2[index2][columnIndex];
        if (num2 < num3)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && num2 == num3)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, ++columnIndex);
          this.AddRange(destArray, srcArray, startIndex);
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortString(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int startIndex = 0;
    int num = arrData.Length / 2;
    object[][] range1 = this.GetRange(arrData, 0, num);
    object[][] range2 = this.GetRange(arrData, num, arrData.Length);
    object[][] objArray1 = this.SortString(range1, columnIndex);
    object[][] objArray2 = this.SortString(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else
      {
        string str = (string) objArray1[index1][columnIndex];
        string strB = (string) objArray2[index2][columnIndex];
        if (str.CompareTo(strB) < 0)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && str == strB)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, ++columnIndex);
          this.AddRange(destArray, srcArray, startIndex);
          startIndex += 2;
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortFloat(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int startIndex = 0;
    int num1 = arrData.Length / 2;
    object[][] range1 = this.GetRange(arrData, 0, num1);
    object[][] range2 = this.GetRange(arrData, num1, arrData.Length);
    object[][] objArray1 = this.SortFloat(range1, columnIndex);
    object[][] objArray2 = this.SortFloat(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else
      {
        double num2 = (double) objArray1[index1][columnIndex];
        double num3 = (double) objArray2[index2][columnIndex];
        if (num2 < num3)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && num2 == num3)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, columnIndex + 1);
          this.AddRange(destArray, srcArray, startIndex);
          startIndex += 2;
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortDate(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int num = arrData.Length / 2;
    int startIndex = 0;
    object[][] range1 = this.GetRange(arrData, 0, num);
    object[][] range2 = this.GetRange(arrData, num, arrData.Length);
    object[][] objArray1 = this.SortDate(range1, columnIndex);
    object[][] objArray2 = this.SortDate(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else if (objArray1[index1] != null && objArray2[index2] != null)
      {
        DateTime dateTime1 = (DateTime) objArray1[index1][columnIndex];
        DateTime dateTime2 = (DateTime) objArray2[index2][columnIndex];
        if (dateTime1 < dateTime2)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && dateTime1 == dateTime2)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, columnIndex + 1);
          this.AddRange(destArray, srcArray, startIndex);
          ++startIndex;
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortIntDesc(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int num1 = arrData.Length / 2;
    int startIndex = 0;
    object[][] range1 = this.GetRange(arrData, 0, num1);
    object[][] range2 = this.GetRange(arrData, num1, arrData.Length);
    object[][] objArray1 = this.SortIntDesc(range1, columnIndex);
    object[][] objArray2 = this.SortIntDesc(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else
      {
        int num2 = (int) objArray1[index1][columnIndex];
        int num3 = (int) objArray2[index2][columnIndex];
        if (num2 > num3)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && num2 == num3)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, columnIndex + 1);
          this.AddRange(destArray, srcArray, startIndex);
          startIndex += 2;
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortStringDesc(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int num = arrData.Length / 2;
    int startIndex = 0;
    object[][] range1 = this.GetRange(arrData, 0, num);
    object[][] range2 = this.GetRange(arrData, num, arrData.Length);
    object[][] objArray1 = this.SortStringDesc(range1, columnIndex);
    object[][] objArray2 = this.SortStringDesc(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else
      {
        string str = (string) objArray1[index1][columnIndex];
        string strB = (string) objArray2[index2][columnIndex];
        if (str.CompareTo(strB) > 0)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && str == strB)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, ++columnIndex);
          this.AddRange(destArray, srcArray, startIndex);
          startIndex += 2;
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortFloatDesc(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int num1 = arrData.Length / 2;
    int startIndex = 0;
    object[][] range1 = this.GetRange(arrData, 0, num1);
    object[][] range2 = this.GetRange(arrData, num1, arrData.Length);
    object[][] objArray1 = this.SortFloatDesc(range1, columnIndex);
    object[][] objArray2 = this.SortFloatDesc(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else
      {
        double num2 = (double) objArray1[index1][columnIndex];
        double num3 = (double) objArray2[index2][columnIndex];
        if (num2 > num3)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && num2 == num3)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, columnIndex + 1);
          this.AddRange(destArray, srcArray, startIndex);
          startIndex += 2;
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public object[][] SortDateDesc(object[][] arrData, int columnIndex)
  {
    if (arrData.Length == 1)
      return arrData;
    object[][] destArray = new object[arrData.Length][];
    int num = arrData.Length / 2;
    int startIndex = 0;
    object[][] range1 = this.GetRange(arrData, 0, num);
    object[][] range2 = this.GetRange(arrData, num, arrData.Length);
    object[][] objArray1 = this.SortDateDesc(range1, columnIndex);
    object[][] objArray2 = this.SortDateDesc(range2, columnIndex);
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < objArray1.Length + objArray2.Length; ++index3)
    {
      if (index1 == objArray1.Length)
      {
        destArray[startIndex++] = objArray2[index2];
        ++index2;
      }
      else if (index2 == objArray2.Length)
      {
        destArray[startIndex++] = objArray1[index1];
        ++index1;
      }
      else if (objArray1[index1] != null && objArray2[index2] != null)
      {
        DateTime dateTime1 = (DateTime) objArray1[index1][columnIndex];
        DateTime dateTime2 = (DateTime) objArray2[index2][columnIndex];
        if (dateTime1 > dateTime2)
        {
          destArray[startIndex++] = objArray1[index1];
          ++index1;
        }
        else if (columnIndex + 1 <= this.count && dateTime1 == dateTime2)
        {
          object[][] srcArray = this.SortOnTypes(new object[2][]
          {
            objArray1[index1],
            objArray2[index2]
          }, columnIndex + 1);
          this.AddRange(destArray, srcArray, startIndex);
          ++index1;
          ++index2;
          ++index3;
        }
        else
        {
          destArray[startIndex++] = objArray2[index2];
          ++index2;
        }
      }
    }
    return destArray;
  }

  public override void Sort(int left, int right, int columnIndex)
  {
    this.m_data = this.SortOnTypes(this.m_data, columnIndex);
  }

  public new void SortInt(int left, int right, int columnIndex)
  {
    this.m_data = this.SortInt(this.m_data, columnIndex);
  }

  public new void SortFloat(int left, int right, int columnIndex)
  {
    this.m_data = this.SortFloat(this.m_data, columnIndex);
  }

  public new void SortDate(int left, int right, int columnIndex)
  {
    this.m_data = this.SortDate(this.m_data, columnIndex);
  }

  public new void SortString(int left, int right, int columnIndex)
  {
    this.m_data = this.SortString(this.m_data, columnIndex);
  }

  public new void SortOnTypes(int left, int right, int columnIndex)
  {
    this.m_data = this.SortOnTypes(this.m_data, columnIndex);
  }

  public new void SortIntDesc(int left, int right, int columnIndex)
  {
    this.m_data = this.SortIntDesc(this.m_data, columnIndex);
  }

  public new void SortFloatDesc(int left, int right, int columnIndex)
  {
    this.m_data = this.SortFloatDesc(this.m_data, columnIndex);
  }

  public new void SortDateDesc(int left, int right, int columnIndex)
  {
    this.m_data = this.SortDateDesc(this.m_data, columnIndex);
  }

  public new void SortStringDesc(int left, int right, int columnIndex)
  {
    this.m_data = this.SortStringDesc(this.m_data, columnIndex);
  }

  public new IRange Range
  {
    get => throw new NotSupportedException(nameof (Range));
    set => throw new NotSupportedException(nameof (Range));
  }
}
