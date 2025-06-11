// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.WorkbookObjectsCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class WorkbookObjectsCollection : CollectionBaseEx<object>, ITabSheets
{
  private Dictionary<string, int> m_hashNameToValue = new Dictionary<string, int>((IEqualityComparer<string>) System.StringComparer.CurrentCultureIgnoreCase);
  private WorkbookImpl m_book;

  public WorkbookObjectsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  [CLSCompliant(false)]
  public void Add(ISerializableNamedObject namedObject)
  {
    if (namedObject == null)
      throw new ArgumentNullException("workbookObject");
    if (namedObject.Name == null)
      throw new ArgumentNullException("Name can't be NULL.");
    if (this.m_hashNameToValue.ContainsKey(namedObject.Name))
      throw new ArgumentException("Sheet Name is already existed in workbook");
    int count = this.List.Count;
    namedObject.RealIndex = count;
    this.m_hashNameToValue.Add(namedObject.Name, count);
    this.InnerList.Add((object) namedObject);
    namedObject.NameChanged += new ValueChangedEventHandler(this.object_NameChanged);
  }

  public void Move(int iOldIndex, int iNewIndex)
  {
    if (iOldIndex == iNewIndex)
      return;
    ISerializableNamedObject serializableNamedObject = this[iOldIndex];
    this.InnerList.RemoveAt(iOldIndex);
    this.InnerList.Insert(iNewIndex, (object) serializableNamedObject);
    int num1 = Math.Min(iNewIndex, iOldIndex);
    int num2 = Math.Max(iNewIndex, iOldIndex);
    for (int index = num1; index <= num2; ++index)
      this[index].RealIndex = index;
    this.m_book.MoveSheetIndex(iOldIndex, iNewIndex);
    this.m_book.UpdateActiveSheetAfterMove(iOldIndex, iNewIndex);
    if (this.TabSheetMoved == null)
      return;
    this.TabSheetMoved((object) this, new TabSheetMovedEventArgs(iOldIndex, iNewIndex));
  }

  public void MoveBefore(ITabSheet sheetToMove, ITabSheet sheetForPlacement)
  {
    ISerializableNamedObject serializableNamedObject1 = (ISerializableNamedObject) sheetToMove;
    ISerializableNamedObject serializableNamedObject2 = (ISerializableNamedObject) sheetForPlacement;
    int realIndex1 = serializableNamedObject1.RealIndex;
    int realIndex2 = serializableNamedObject2.RealIndex;
    int iNewIndex = realIndex1 > realIndex2 ? realIndex2 : realIndex2 - 1;
    this.Move(realIndex1, iNewIndex);
  }

  public void MoveAfter(ITabSheet sheetToMove, ITabSheet sheetForPlacement)
  {
    ISerializableNamedObject serializableNamedObject1 = (ISerializableNamedObject) sheetToMove;
    ISerializableNamedObject serializableNamedObject2 = (ISerializableNamedObject) sheetForPlacement;
    int realIndex1 = serializableNamedObject1.RealIndex;
    int realIndex2 = serializableNamedObject2.RealIndex;
    int iNewIndex = realIndex1 > realIndex2 ? realIndex2 + 1 : realIndex2;
    this.Move(realIndex1, iNewIndex);
  }

  public void DisposeInternalData()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this.InnerList[index] is WorksheetBaseImpl inner)
        inner.Dispose();
    }
  }

  [CLSCompliant(false)]
  public ISerializableNamedObject this[int index]
  {
    get
    {
      if (index < 0 || index >= this.List.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      return this.List[index] as ISerializableNamedObject;
    }
  }

  public INamedObject this[string name]
  {
    get
    {
      int index;
      return !this.m_hashNameToValue.TryGetValue(name, out index) ? (INamedObject) null : (INamedObject) this[index];
    }
  }

  public IWorkbook Workbook => (IWorkbook) this.m_book;

  public override object Clone(object parent)
  {
    WorkbookObjectsCollection parent1 = new WorkbookObjectsCollection(this.Application, parent);
    System.Collections.Generic.List<object> innerList = this.InnerList;
    IList<object> list = parent1.List;
    parent1.m_book.Objects = parent1;
    int index1 = 0;
    for (int count = innerList.Count; index1 < count; ++index1)
    {
      object obj = (innerList[index1] as WorksheetBaseImpl).Clone((object) parent1, false);
      list.Add(obj);
    }
    int index2 = 0;
    for (int count = innerList.Count; index2 < count; ++index2)
      (innerList[index2] as WorksheetBaseImpl).CloneShapes(list[index2] as WorksheetBaseImpl);
    return (object) parent1;
  }

  ITabSheet ITabSheets.this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count - 1 ? (ITabSheet) this.InnerList[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1.");
    }
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  protected override void OnInsertComplete(int index, object value)
  {
    ISerializableNamedObject serializableNamedObject = (ISerializableNamedObject) value;
    serializableNamedObject.NameChanged += new ValueChangedEventHandler(this.object_NameChanged);
    this.m_hashNameToValue[serializableNamedObject.Name] = index;
    int num = index;
    for (int count = this.List.Count; num < count; ++num)
      this[index].RealIndex = num;
    this.m_book.IncreaseSheetIndex(index);
    base.OnInsertComplete(index, value);
  }

  protected override void OnSetComplete(int index, object oldValue, object newValue)
  {
    WorksheetImpl worksheetImpl1 = (WorksheetImpl) oldValue;
    WorksheetImpl worksheetImpl2 = (WorksheetImpl) newValue;
    worksheetImpl1.NameChanged -= new ValueChangedEventHandler(this.object_NameChanged);
    this.m_hashNameToValue.Remove(worksheetImpl1.Name);
    this.m_hashNameToValue[worksheetImpl2.Name] = index;
    base.OnSetComplete(index, oldValue, newValue);
  }

  protected override void OnRemoveComplete(int index, object value)
  {
    ISerializableNamedObject serializableNamedObject1 = (ISerializableNamedObject) value;
    serializableNamedObject1.NameChanged -= new ValueChangedEventHandler(this.object_NameChanged);
    this.m_hashNameToValue.Remove(serializableNamedObject1.Name);
    int count = this.List.Count;
    for (int index1 = index; index1 < count; ++index1)
    {
      ISerializableNamedObject serializableNamedObject2 = this[index1];
      serializableNamedObject2.RealIndex = index1;
      this.m_hashNameToValue[serializableNamedObject2.Name] = index1;
    }
    this.m_book.DecreaseSheetIndex(index);
    int activeSheetIndex = this.m_book.ActiveSheetIndex;
    if (index < this.m_book.ActiveSheetIndex || index == this.m_book.ActiveSheetIndex && index == count)
    {
      --activeSheetIndex;
      this.FindVisibleWorksheet(activeSheetIndex);
    }
    (this[activeSheetIndex] as ITabSheet).Activate();
    base.OnRemoveComplete(index, value);
  }

  private void FindVisibleWorksheet(int proposedIndex)
  {
    if ((this[proposedIndex] as ITabSheet).Visibility == OfficeWorksheetVisibility.Visible)
    {
      this.m_book.ActiveSheetIndex = proposedIndex;
    }
    else
    {
      int num = -1;
      for (int index = proposedIndex - 1; index >= 0; --index)
      {
        if ((this[index] as ITabSheet).Visibility == OfficeWorksheetVisibility.Visible)
        {
          num = index;
          break;
        }
      }
      if (num == -1)
      {
        int index = proposedIndex + 1;
        for (int count = this.Count; index < count; --index)
        {
          if ((this[index] as ITabSheet).Visibility == OfficeWorksheetVisibility.Visible)
          {
            num = index;
            break;
          }
        }
      }
      this.m_book.ActiveSheetIndex = num != -1 ? num : throw new Exception("A workbook must contain at least one visible worksheet. To hide, delete, or move the selected sheet(s), you must first insert a new sheet or unhide a sheet that is already hidden.");
    }
  }

  protected override void OnClearComplete()
  {
    base.OnClearComplete();
    this.m_hashNameToValue.Clear();
  }

  private void object_NameChanged(object sender, ValueChangedEventArgs e)
  {
    string newValue = (string) e.newValue;
    if (this.m_hashNameToValue.ContainsKey(newValue))
      throw new ArgumentException("Name of worksheet must be unique in a workbook.");
    string oldValue = (string) e.oldValue;
    int num = this.m_hashNameToValue[oldValue];
    this.m_hashNameToValue.Remove(oldValue);
    this.m_hashNameToValue[newValue] = num;
  }

  internal event TabSheetMovedEventHandler TabSheetMoved;
}
