// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCacheCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotCacheCollection : 
  ICloneParent,
  IPivotCaches,
  IEnumerable<PivotCacheImpl>,
  IEnumerable,
  IParentApplication
{
  public const string DEF_PIVOT_CACHE_STORAGE = "_SX_DB_CUR";
  private WorkbookImpl m_book;
  private Dictionary<int, PivotCacheImpl> m_dictCaches = new Dictionary<int, PivotCacheImpl>();
  private List<int> m_arrOrder = new List<int>();

  IPivotCache IPivotCaches.this[int id] => (IPivotCache) this.m_dictCaches[id];

  public PivotCacheImpl this[int id]
  {
    get => this.m_dictCaches.ContainsKey(id) ? this.m_dictCaches[id] : (PivotCacheImpl) null;
  }

  public int Count => this.m_dictCaches.Count;

  public IApplication Application => this.m_book.Application;

  public object Parent => (object) this.m_book;

  public List<int> Order => this.m_arrOrder;

  public PivotCacheCollection(IApplication application, object parent)
  {
    this.m_book = this.FindParent(parent);
  }

  public PivotCacheCollection(
    IApplication application,
    object parent,
    ICompoundStorage storage,
    IDecryptor decryptor)
    : this(application, parent)
  {
    this.Parse(storage, decryptor);
  }

  public void Parse(ICompoundStorage storage, IDecryptor decryptor)
  {
    if (storage == null)
      throw new ArgumentNullException(nameof (storage));
    this.Clear();
    if (!storage.ContainsStorage("_SX_DB_CUR"))
      return;
    ICompoundStorage compoundStorage;
    using (compoundStorage = storage.OpenStorage("_SX_DB_CUR"))
    {
      string[] streams = compoundStorage.Streams;
      int index = 0;
      for (int length = streams.Length; index < length; ++index)
      {
        string streamName = streams[index];
        using (CompoundStream compoundStream = compoundStorage.OpenStream(streamName))
        {
          using (BiffReader reader = new BiffReader((Stream) compoundStream))
          {
            PivotCacheImpl cache = new PivotCacheImpl(this.Application, (object) this, reader, decryptor, streamName);
            this.Add(streamName, cache);
          }
        }
      }
    }
  }

  public void Clear() => this.m_dictCaches.Clear();

  public void Serialize(ICompoundStorage storage, IEncryptor encryptor)
  {
    if (storage == null)
      throw new ArgumentNullException(nameof (storage));
    if (this.Count == 0)
      return;
    using (ICompoundStorage storage1 = storage.CreateStorage("_SX_DB_CUR"))
    {
      foreach (PivotCacheImpl pivotCacheImpl in this.m_dictCaches.Values)
      {
        string streamName = pivotCacheImpl.StreamId.ToString("X4");
        using (CompoundStream stream = storage1.CreateStream(streamName))
        {
          OffsetArrayList records = new OffsetArrayList();
          pivotCacheImpl.Serialize(records);
          using (BiffWriter biffWriter = new BiffWriter((Stream) stream))
            biffWriter.WriteRecord(records, encryptor);
        }
      }
    }
  }

  public void Add(PivotCacheImpl cache)
  {
    this.m_dictCaches.Add(cache.Index = this.GetFreeIndex(cache), cache);
  }

  public void Add(int index, PivotCacheImpl cache)
  {
    this.m_dictCaches.Add(cache.Index = index, cache);
  }

  private int GetFreeIndex(PivotCacheImpl cache)
  {
    int streamId = (int) cache.StreamId;
    FileDataHolder dataHolder = this.m_book.DataHolder;
    Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
    if (dataHolder != null)
      dictionary = dataHolder.PreservedCaches;
    while (this.m_dictCaches.ContainsKey(streamId) || dictionary != null && dictionary.ContainsKey((streamId + 1).ToString()))
      ++streamId;
    cache.StreamId = (ushort) streamId;
    return streamId;
  }

  public IPivotCache Add(IRange range)
  {
    PivotCacheImpl cache = range != null ? new PivotCacheImpl(this.Application, (object) this, range) : throw new ArgumentNullException(nameof (range));
    this.Add(cache);
    this.m_arrOrder.Add(cache.Index);
    return (IPivotCache) cache;
  }

  private void Add(string streamName, PivotCacheImpl cache) => this.Add(cache);

  internal int CheckAndAddCache(PivotCacheImpl cache, Dictionary<string, string> hashWorksheetNames)
  {
    IRange sourceRange = cache.SourceRange;
    PivotCacheImpl cache1 = (PivotCacheImpl) null;
    if (sourceRange != null)
    {
      string str = sourceRange.Worksheet.Name;
      if (hashWorksheetNames != null && hashWorksheetNames.ContainsKey(str))
        str = hashWorksheetNames[str];
      IRange range = this.m_book.Worksheets[str][sourceRange.AddressLocal];
      int id = 0;
      for (int count = this.Count; id < count; ++id)
      {
        PivotCacheImpl pivotCacheImpl = this[id];
        if (pivotCacheImpl.SourceRange.AddressGlobal == range.AddressGlobal)
        {
          cache1 = pivotCacheImpl;
          break;
        }
      }
      if (cache1 == null)
        cache1 = (PivotCacheImpl) this.Add(range);
    }
    else
    {
      int id = 0;
      for (int count = this.Count; id < count; ++id)
      {
        PivotCacheImpl pivotCacheImpl = this[id];
        if (pivotCacheImpl.ComparePreservedData(cache))
        {
          cache1 = pivotCacheImpl;
          break;
        }
      }
      if (cache1 == null)
      {
        cache1 = (PivotCacheImpl) cache.Clone((object) this);
        this.Add(cache1);
      }
    }
    return cache1.Index;
  }

  public void RemoveAt(int index) => this.m_dictCaches.Remove(index);

  public int[] GetIndexes()
  {
    int[] array = new int[this.m_dictCaches.Count];
    this.m_dictCaches.Keys.CopyTo(array, 0);
    return array;
  }

  public object Clone(object parent)
  {
    PivotCacheCollection parent1 = new PivotCacheCollection(this.m_book.Application, (object) this.m_book);
    List<PivotCacheImpl> pivotCacheImplList = new List<PivotCacheImpl>();
    parent1.m_book = this.FindParent(parent);
    foreach (PivotCacheImpl pivotCacheImpl1 in this.m_dictCaches.Values)
    {
      PivotCacheImpl pivotCacheImpl2 = (PivotCacheImpl) pivotCacheImpl1.Clone((object) parent1);
      pivotCacheImplList.Add(pivotCacheImpl2);
    }
    foreach (PivotCacheImpl cache in pivotCacheImplList)
      parent1.Add(cache.Index, cache);
    return (object) parent1;
  }

  private WorkbookImpl FindParent(object parent)
  {
    return (WorkbookImpl) CommonObject.FindParent(parent, typeof (WorkbookImpl)) ?? throw new ArgumentOutOfRangeException("Cannot find parent workbook");
  }

  public IEnumerator<PivotCacheImpl> GetEnumerator()
  {
    foreach (PivotCacheImpl cache in this.m_dictCaches.Values)
      yield return cache;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    foreach (PivotCacheImpl cache in this.m_dictCaches.Values)
      yield return (object) cache;
  }
}
