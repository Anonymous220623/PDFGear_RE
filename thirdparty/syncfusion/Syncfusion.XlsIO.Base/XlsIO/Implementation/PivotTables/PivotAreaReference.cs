// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotAreaReference
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotAreaReference
{
  private PivotSubtotalTypes m_subtotal;
  private bool m_bIsReferByPosition;
  private int m_iCount;
  private int m_iFieldIndex;
  private bool m_bIsRelativeReference;
  private bool m_bIsSelected;
  private List<int> m_iIndexes;
  private bool m_bIsDefaultSubtotal;

  public PivotSubtotalTypes Subtotal
  {
    get => this.m_subtotal;
    set => this.m_subtotal = value;
  }

  public bool IsReferByPosition
  {
    get => this.m_bIsReferByPosition;
    set => this.m_bIsReferByPosition = value;
  }

  public int Count
  {
    get => this.m_iCount;
    set => this.m_iCount = value;
  }

  public int FieldIndex
  {
    get => this.m_iFieldIndex;
    set => this.m_iFieldIndex = value;
  }

  public bool IsRelativeReference
  {
    get => this.m_bIsRelativeReference;
    set => this.m_bIsRelativeReference = value;
  }

  public bool IsSelected
  {
    get => this.m_bIsSelected;
    set => this.m_bIsSelected = value;
  }

  public List<int> Indexes
  {
    get
    {
      if (this.m_iIndexes == null)
        this.m_iIndexes = new List<int>();
      return this.m_iIndexes;
    }
  }

  public int FirstIndex => this.m_iIndexes == null ? -1 : this.m_iIndexes[0];

  internal bool IsDefaultSubTotal
  {
    get => this.m_bIsDefaultSubtotal;
    set => this.m_bIsDefaultSubtotal = value;
  }

  public PivotAreaReference() => this.Subtotal = PivotSubtotalTypes.Default;

  internal object Clone()
  {
    PivotAreaReference pivotAreaReference = (PivotAreaReference) this.MemberwiseClone();
    if (this.m_iIndexes != null)
    {
      pivotAreaReference.m_iIndexes = new List<int>();
      for (int index = 0; index < this.m_iIndexes.Count; ++index)
        pivotAreaReference.m_iIndexes.Add(this.m_iIndexes[index]);
    }
    return (object) pivotAreaReference;
  }

  internal new bool Equals(object obj)
  {
    return obj is PivotAreaReference pivotAreaReference && this.m_subtotal == pivotAreaReference.m_subtotal && this.m_bIsReferByPosition == pivotAreaReference.m_bIsReferByPosition && this.m_iCount == pivotAreaReference.m_iCount && this.m_iFieldIndex == pivotAreaReference.m_iFieldIndex && this.m_bIsRelativeReference == pivotAreaReference.m_bIsRelativeReference && this.m_bIsSelected == pivotAreaReference.m_bIsSelected && this.m_iIndexes.Count == pivotAreaReference.m_iIndexes.Count && this.CheckOrderedEqual<int>((ICollection<int>) this.m_iIndexes, (ICollection<int>) pivotAreaReference.m_iIndexes);
  }

  private bool CheckOrderedEqual<T>(ICollection<T> first, ICollection<T> second)
  {
    if (first.Count != second.Count)
      return false;
    Dictionary<T, int> dictionary = new Dictionary<T, int>();
    foreach (T key in (IEnumerable<T>) first)
    {
      int num;
      if (dictionary.TryGetValue(key, out num))
        dictionary[key] = num + 1;
      else
        dictionary.Add(key, 1);
    }
    foreach (T key in (IEnumerable<T>) second)
    {
      int num;
      if (!dictionary.TryGetValue(key, out num) || num == 0)
        return false;
      dictionary[key] = num - 1;
    }
    foreach (int num in dictionary.Values)
    {
      if (num != 0)
        return false;
    }
    return true;
  }
}
