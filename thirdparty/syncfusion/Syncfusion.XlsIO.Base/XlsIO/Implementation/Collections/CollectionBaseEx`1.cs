// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CollectionBaseEx`1
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CollectionBaseEx<T> : 
  CollectionBase<T>,
  IList<T>,
  ICollection<T>,
  IEnumerable<T>,
  IEnumerable,
  IParentApplication,
  ICloneParent
{
  private IApplication m_appl;
  private object m_parent;
  private bool m_bSkipEvents;
  [ThreadStatic]
  private static Dictionary<string, int> m_dictCollectionsMaxValues;

  public IApplication Application
  {
    [DebuggerStepThrough] get => this.m_appl;
  }

  public object Parent
  {
    [DebuggerStepThrough] get => this.m_parent;
  }

  public bool QuietMode
  {
    get => this.m_bSkipEvents;
    set
    {
      if (value == this.m_bSkipEvents)
        return;
      this.m_bSkipEvents = value;
    }
  }

  protected ApplicationImpl AppImplementation
  {
    [DebuggerStepThrough] get => (ApplicationImpl) this.Application;
  }

  internal static Dictionary<string, int> DictCollectionsMaxValues
  {
    get
    {
      if (CollectionBaseEx<T>.m_dictCollectionsMaxValues == null)
        CollectionBaseEx<T>.m_dictCollectionsMaxValues = new Dictionary<string, int>();
      return CollectionBaseEx<T>.m_dictCollectionsMaxValues;
    }
  }

  public event EventHandler Changed;

  public event CollectionBaseEx<T>.CollectionClear Clearing;

  public event CollectionBaseEx<T>.CollectionClear Cleared;

  public event CollectionBaseEx<T>.CollectionChange Inserting;

  public event CollectionBaseEx<T>.CollectionChange Inserted;

  public event CollectionBaseEx<T>.CollectionChange Removing;

  public event CollectionBaseEx<T>.CollectionChange Removed;

  public event CollectionBaseEx<T>.CollectionSet Setting;

  public event CollectionBaseEx<T>.CollectionSet Set;

  private CollectionBaseEx()
  {
  }

  public CollectionBaseEx(IApplication application, object parent)
    : this()
  {
    this.m_appl = application;
    this.m_parent = parent;
  }

  private void RaiseChangedEvent()
  {
    if (this.Changed == null || this.m_bSkipEvents)
      return;
    this.Changed((object) this, EventArgs.Empty);
  }

  protected override void OnClear()
  {
    if (this.Clearing != null && !this.m_bSkipEvents)
      this.Clearing();
    CollectionBaseEx<T>.DictCollectionsMaxValues.Clear();
    base.OnClear();
  }

  protected override void OnClearComplete()
  {
    if (this.Cleared != null && !this.m_bSkipEvents)
      this.Cleared();
    base.OnClearComplete();
    this.RaiseChangedEvent();
  }

  protected override void OnInsert(int index, T value)
  {
    if (this.Inserting != null && !this.m_bSkipEvents)
      this.Inserting((object) this, new CollectionChangeEventArgs<T>(index, value));
    base.OnInsert(index, value);
  }

  protected override void OnInsertComplete(int index, T value)
  {
    if (this.Inserted != null && !this.m_bSkipEvents)
      this.Inserted((object) this, new CollectionChangeEventArgs<T>(index, value));
    base.OnInsertComplete(index, value);
    this.RaiseChangedEvent();
  }

  protected override void OnRemove(int index, T value)
  {
    if (this.Removing != null && !this.m_bSkipEvents)
      this.Removing((object) this, new CollectionChangeEventArgs<T>(index, value));
    base.OnRemove(index, value);
  }

  protected override void OnRemoveComplete(int index, T value)
  {
    if (this.Removed != null && !this.m_bSkipEvents)
      this.Removed((object) this, new CollectionChangeEventArgs<T>(index, value));
    base.OnRemoveComplete(index, value);
    this.RaiseChangedEvent();
  }

  protected override void OnSet(int index, T oldValue, T newValue)
  {
    if (this.Setting != null && !this.m_bSkipEvents)
      this.Setting(index, (object) oldValue, (object) newValue);
    base.OnSet(index, oldValue, newValue);
  }

  protected override void OnSetComplete(int index, T oldValue, T newValue)
  {
    if (this.Set != null && !this.m_bSkipEvents)
      this.Set(index, (object) oldValue, (object) newValue);
    base.OnSetComplete(index, oldValue, newValue);
    this.RaiseChangedEvent();
  }

  public object FindParent(Type parentType) => this.FindParent(parentType, false);

  public object FindParent(Type parentType, bool bCheckSubclasses)
  {
    int num = 0;
    IParentApplication parent = (IParentApplication) this.Parent;
    bool isInterface = parentType.IsInterface;
    while (num <= 100)
    {
      if (parent != null && parent.Parent != null)
      {
        Type type = parent.GetType();
        if (!isInterface)
        {
          if (!type.Equals(parentType))
          {
            if (bCheckSubclasses)
            {
              type.IsSubclassOf(parentType);
              goto label_10;
            }
          }
          else
            goto label_10;
        }
        else if (type.GetInterface(parentType.Name, false) != (Type) null)
          goto label_10;
        parent = (IParentApplication) parent.Parent;
        ++num;
        if (parent != null)
          continue;
      }
label_10:
      return (object) parent;
    }
    throw new ArgumentException("links Cycle in object tree detected!");
  }

  public void SetParent(object parent) => this.m_parent = parent;

  public virtual object Clone(object parent)
  {
    ConstructorInfo constructor = this.GetType().GetConstructor(new Type[2]
    {
      typeof (IApplication),
      typeof (object)
    });
    if (constructor == (ConstructorInfo) null)
      throw new ApplicationException("Cannot find required constructor.");
    CollectionBaseEx<T> parent1 = constructor.Invoke(new object[2]
    {
      (object) this.Application,
      parent
    }) as CollectionBaseEx<T>;
    System.Collections.Generic.List<T> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      T obj = innerList[index];
      if ((object) obj is ICloneParent)
        obj = (T) ((ICloneParent) (object) obj).Clone((object) parent1);
      else if ((object) obj is ICloneable)
        obj = (T) ((ICloneable) (object) obj).Clone();
      parent1.Add(obj);
    }
    return (object) parent1;
  }

  public void EnsureCapacity(int size)
  {
    if (this.InnerList.Capacity >= size)
      return;
    this.InnerList.Capacity = size;
  }

  protected void ClearMaxValues() => CollectionBaseEx<T>.DictCollectionsMaxValues.Clear();

  public static string GenerateDefaultName(ICollection<T> namesCollection, string strStart)
  {
    int val2 = 1;
    int length = strStart.Length;
    if (CollectionBaseEx<T>.DictCollectionsMaxValues.ContainsKey(strStart))
    {
      val2 = CollectionBaseEx<T>.DictCollectionsMaxValues[strStart] + 1;
      CollectionBaseEx<T>.DictCollectionsMaxValues[strStart] = val2;
    }
    else
    {
      foreach (T names in (IEnumerable<T>) namesCollection)
      {
        string name = ((INamedObject) (object) names).Name;
        double result;
        if (name != null && name.StartsWith(strStart) && double.TryParse(name.Substring(length, name.Length - length), NumberStyles.Integer, (IFormatProvider) null, out result))
          val2 = Math.Max((int) result + 1, val2);
      }
      CollectionBaseEx<T>.DictCollectionsMaxValues.Add(strStart, val2);
    }
    return strStart + val2.ToString();
  }

  public static string GenerateDefaultName(ICollection namesCollection, string strStart)
  {
    int val2 = 1;
    int length = strStart.Length;
    foreach (INamedObject names in (IEnumerable) namesCollection)
    {
      string name = names.Name;
      double result;
      if (name != null && name.StartsWith(strStart) && double.TryParse(name.Substring(length, name.Length - length), NumberStyles.Integer, (IFormatProvider) null, out result))
        val2 = Math.Max((int) result + 1, val2);
    }
    return strStart + val2.ToString();
  }

  internal static int GenerateID(ICollection<T> shapeCollection)
  {
    int num = 1;
    foreach (T shape1 in (IEnumerable<T>) shapeCollection)
    {
      IShape shape2 = (IShape) (object) shape1;
      if (num < shape2.Id)
        num = shape2.Id;
    }
    return num + 1;
  }

  public static string GenerateDefaultName(string strStart, params ICollection[] arrCollections)
  {
    int val2 = 1;
    int length1 = strStart.Length;
    int index = 0;
    for (int length2 = arrCollections.Length; index < length2; ++index)
    {
      foreach (object obj in (IEnumerable) arrCollections[index])
      {
        string str = !(obj is INamedObject) ? obj.ToString() : (obj as INamedObject).Name;
        if (str.StartsWith(strStart))
        {
          string s = str.Substring(length1, str.Length - length1);
          double result;
          if (double.TryParse(s, NumberStyles.Integer, (IFormatProvider) null, out result))
            val2 = Math.Max((int) result + 1, val2);
          else if (s == "")
            ++val2;
        }
      }
    }
    return $"{strStart} {val2.ToString()}";
  }

  public static void ChangeName(IDictionary hashNames, ValueChangedEventArgs e)
  {
    string oldValue = (string) e.oldValue;
    string newValue = (string) e.newValue;
    if (!hashNames.Contains((object) oldValue))
      throw new ArgumentOutOfRangeException("Collection does not contain object named " + oldValue);
    object obj = !hashNames.Contains((object) newValue) ? hashNames[(object) oldValue] : throw new ArgumentOutOfRangeException("Collection already contains object named " + newValue);
    hashNames.Remove((object) oldValue);
    hashNames.Add((object) newValue, obj);
  }

  public delegate void CollectionClear();

  public delegate void CollectionChange(object sender, CollectionChangeEventArgs<T> args);

  public delegate void CollectionSet(int index, object old, object value);
}
