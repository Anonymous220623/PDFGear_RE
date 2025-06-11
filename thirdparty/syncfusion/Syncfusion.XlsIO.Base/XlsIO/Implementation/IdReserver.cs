// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.IdReserver
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class IdReserver
{
  public const int SegmentSize = 1024 /*0x0400*/;
  internal Dictionary<int, int> m_id = new Dictionary<int, int>();
  private Dictionary<int, int> m_idCount = new Dictionary<int, int>();
  private Dictionary<int, KeyValuePair<int, int>> m_collectionCount = new Dictionary<int, KeyValuePair<int, int>>();
  private int m_iMaximumId;
  private Dictionary<int, int> m_dictAdditionalShapes = new Dictionary<int, int>();

  public int MaximumId => this.m_iMaximumId;

  public static int GetSegmentStart(int id)
  {
    int num = id % 1024 /*0x0400*/;
    return id - num;
  }

  public bool CheckReserved(int id) => this.m_id.ContainsKey(IdReserver.GetSegmentStart(id));

  public bool CheckFree(int id, int count)
  {
    int segmentStart = IdReserver.GetSegmentStart(id);
    bool flag = true;
    int num = 0;
    while (num < count && flag)
    {
      flag = !this.m_id.ContainsKey(segmentStart);
      ++num;
      segmentStart += 1024 /*0x0400*/;
    }
    return flag;
  }

  public int ReservedBy(int id)
  {
    int num1 = id % 1024 /*0x0400*/;
    int num2;
    this.m_id.TryGetValue(id - num1, out num2);
    return num2;
  }

  public bool TryReserve(int id, int lastId, int collectionId)
  {
    int num1 = id;
    id = IdReserver.GetSegmentStart(id);
    this.m_iMaximumId = Math.Max(this.m_iMaximumId, lastId);
    int segmentStart = IdReserver.GetSegmentStart(lastId);
    int num2 = (segmentStart - id) / 1024 /*0x0400*/ + 1;
    bool flag;
    if (this.CheckFree(id, num2))
    {
      flag = true;
      KeyValuePair<int, int> keyValuePair1;
      if (!this.m_collectionCount.TryGetValue(collectionId, out keyValuePair1))
      {
        this.m_collectionCount.Add(collectionId, new KeyValuePair<int, int>(num2, id));
      }
      else
      {
        KeyValuePair<int, int> keyValuePair2 = new KeyValuePair<int, int>(keyValuePair1.Key + (int) Math.Ceiling((double) (segmentStart - id + 1) / 1024.0), keyValuePair1.Value);
        this.m_collectionCount[collectionId] = keyValuePair2;
      }
      for (int key = id; key <= lastId && flag; key += 1024 /*0x0400*/)
        this.m_id.Add(key, collectionId);
      for (int id1 = num1; id1 <= lastId; ++id1)
        this.IncreaseCount(id1);
    }
    else
      flag = this.IsReservedBy(id, segmentStart, collectionId);
    return flag;
  }

  private void IncreaseCount(int id)
  {
    int segmentStart = IdReserver.GetSegmentStart(id);
    int num1;
    int num2 = !this.m_idCount.TryGetValue(segmentStart, out num1) ? 1 : num1 + 1;
    this.m_idCount[segmentStart] = num2;
  }

  private bool IsReservedBy(int id, int lastId, int collectionId)
  {
    bool flag = true;
    for (int index = id; index <= lastId && flag; index += 1024 /*0x0400*/)
      flag = this.ReservedBy(id) == collectionId;
    return flag;
  }

  private void FreeSegment(int id) => this.m_id.Remove(IdReserver.GetSegmentStart(id));

  public void FreeSegmentsSequence(int id, int collectionId)
  {
    for (; this.ReservedBy(id) == collectionId; id += 1024 /*0x0400*/)
      this.FreeSegment(id);
  }

  public void FreeSequence(int collectionId)
  {
    KeyValuePair<int, int> keyValuePair;
    if (!this.m_collectionCount.TryGetValue(collectionId, out keyValuePair))
      return;
    this.FreeSegmentsSequence(keyValuePair.Value, collectionId);
  }

  public int Allocate(int idNumber, int collectionId, ShapeCollectionBase shapes)
  {
    int id = 1024 /*0x0400*/;
    int count = (int) Math.Ceiling((double) idNumber / 1024.0);
    while (!this.CheckFree(id, count) && (shapes[0] as ShapeImpl).ShapeId == 0)
      id += 1024 /*0x0400*/;
    int num = id;
    this.TryReserve(id, id + idNumber, collectionId);
    return num;
  }

  public int GetReservedCount(int collectionId)
  {
    KeyValuePair<int, int> keyValuePair;
    return !this.m_collectionCount.TryGetValue(collectionId, out keyValuePair) ? 0 : keyValuePair.Key * 1024 /*0x0400*/;
  }

  public int ReservedCount(int id)
  {
    id = IdReserver.GetSegmentStart(id);
    int num;
    this.m_idCount.TryGetValue(id, out num);
    return num;
  }

  public void AddAdditionalShapes(int collectionIndex, int shapesNumber)
  {
    int num;
    if (!this.m_dictAdditionalShapes.TryGetValue(collectionIndex, out num))
      num = 0;
    this.m_dictAdditionalShapes[collectionIndex] = num + shapesNumber;
  }

  public int GetAdditionalShapesNumber(int collectionIndex)
  {
    int additionalShapesNumber;
    this.m_dictAdditionalShapes.TryGetValue(collectionIndex, out additionalShapesNumber);
    return additionalShapesNumber;
  }
}
