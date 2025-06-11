// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ErrorIndicatorsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class ErrorIndicatorsCollection(IApplication application, object parent) : 
  CollectionBaseEx<ErrorIndicatorImpl>(application, parent)
{
  private Dictionary<ExcelIgnoreError, ErrorIndicatorImpl> m_dicErrorIndicators = new Dictionary<ExcelIgnoreError, ErrorIndicatorImpl>();

  public ErrorIndicatorImpl Add(ErrorIndicatorImpl errorIndicator)
  {
    ExcelIgnoreError key = errorIndicator != null ? errorIndicator.IgnoreOptions : throw new ArgumentNullException(nameof (errorIndicator));
    this.Remove(errorIndicator.CellList.ToArray());
    ErrorIndicatorImpl errorIndicatorImpl;
    if (this.m_dicErrorIndicators.TryGetValue(errorIndicator.IgnoreOptions, out errorIndicatorImpl))
    {
      errorIndicatorImpl.AddCells(errorIndicator);
    }
    else
    {
      base.Add(errorIndicator);
      this.m_dicErrorIndicators.Add(key, errorIndicator);
    }
    return errorIndicatorImpl ?? errorIndicator;
  }

  public ErrorIndicatorImpl Find(Rectangle[] arrRanges)
  {
    if (arrRanges == null)
      return (ErrorIndicatorImpl) null;
    if (arrRanges.Length == 0)
      return (ErrorIndicatorImpl) null;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      ErrorIndicatorImpl errorIndicatorImpl = this[i];
      if (errorIndicatorImpl.Contains(arrRanges, 0))
        return errorIndicatorImpl;
    }
    return (ErrorIndicatorImpl) null;
  }

  public void Remove(Rectangle[] rect)
  {
    foreach (KeyValuePair<ExcelIgnoreError, ErrorIndicatorImpl> dicErrorIndicator in this.m_dicErrorIndicators)
      dicErrorIndicator.Value.Remove(rect);
  }

  public override object Clone(object parent)
  {
    ErrorIndicatorsCollection indicatorsCollection = (ErrorIndicatorsCollection) base.Clone(parent);
    Dictionary<ExcelIgnoreError, ErrorIndicatorImpl> dictionary = new Dictionary<ExcelIgnoreError, ErrorIndicatorImpl>();
    foreach (KeyValuePair<ExcelIgnoreError, ErrorIndicatorImpl> dicErrorIndicator in this.m_dicErrorIndicators)
      dictionary.Add(dicErrorIndicator.Key, dicErrorIndicator.Value);
    indicatorsCollection.m_dicErrorIndicators = dictionary;
    return (object) indicatorsCollection;
  }

  internal Dictionary<long, ErrorIndicatorImpl> GetErrorIndicators(Rectangle fromRect)
  {
    Dictionary<long, ErrorIndicatorImpl> errorIndicators = new Dictionary<long, ErrorIndicatorImpl>();
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      ErrorIndicatorImpl errorIndicatorImpl = this[i];
      foreach (Rectangle cell in errorIndicatorImpl.CellList)
      {
        for (int y = cell.Y; y <= cell.Bottom; ++y)
        {
          for (int x = cell.X; x <= cell.Right; ++x)
          {
            if (fromRect.Contains(x, y))
              errorIndicators.Add(RangeImpl.GetCellIndex(x + 1, y + 1), errorIndicatorImpl);
          }
        }
      }
    }
    return errorIndicators;
  }
}
