// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCacheCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfCacheCollection
{
  private List<List<object>> m_referenceObjects;
  private Dictionary<Font, int> m_fontOffsets;
  private Dictionary<Font, byte[]> m_fontData;
  private Dictionary<string, PdfFont> m_fontCollection;

  public PdfCacheCollection()
  {
    this.m_referenceObjects = new List<List<object>>();
    this.m_fontData = new Dictionary<Font, byte[]>();
    this.m_fontCollection = new Dictionary<string, PdfFont>();
  }

  private List<object> this[int index] => this.m_referenceObjects[index];

  internal Dictionary<Font, int> FontOffsetTable
  {
    get
    {
      if (this.m_fontOffsets == null)
        this.m_fontOffsets = new Dictionary<Font, int>();
      return this.m_fontOffsets;
    }
  }

  internal Dictionary<Font, byte[]> FontData => this.m_fontData;

  internal Dictionary<string, PdfFont> FontCollection
  {
    get
    {
      if (this.m_fontCollection == null)
        this.m_fontCollection = new Dictionary<string, PdfFont>();
      return this.m_fontCollection;
    }
  }

  public IPdfCache Search(IPdfCache obj)
  {
    IPdfCache pdfCache = (IPdfCache) null;
    List<object> objectList = this.GetGroup(obj);
    if (objectList == null)
      objectList = this.CreateNewGroup();
    else if (objectList.Count > 0)
      pdfCache = (IPdfCache) objectList[0];
    objectList.Add((object) obj);
    return pdfCache;
  }

  public bool Contains(IPdfCache obj)
  {
    bool flag = false;
    if (obj != null)
      flag = this.GetGroup(obj) != null;
    return flag;
  }

  public int GroupCount(IPdfCache obj)
  {
    int num = 0;
    if (obj != null)
    {
      List<object> group = this.GetGroup(obj);
      if (group != null)
        num = group.Count;
    }
    return num;
  }

  public void Remove(IPdfCache obj)
  {
    if (obj == null)
      return;
    List<object> group = this.GetGroup(obj);
    if (group == null)
      return;
    group.Remove((object) obj);
    if (group.Count != 0)
      return;
    this.RemoveGroup(group);
  }

  public void Clear()
  {
    if (this.m_referenceObjects != null)
    {
      int index = 0;
      for (int count = this.m_referenceObjects.Count; index < count; ++index)
        this.m_referenceObjects[index].Clear();
      this.m_referenceObjects.Clear();
    }
    if (this.m_fontOffsets != null)
      this.m_fontOffsets.Clear();
    if (this.m_fontData != null)
      this.m_fontData.Clear();
    this.m_fontCollection.Clear();
  }

  private List<object> CreateNewGroup()
  {
    List<object> newGroup = new List<object>();
    this.m_referenceObjects.Add(newGroup);
    return newGroup;
  }

  private List<object> GetGroup(IPdfCache result)
  {
    List<object> group = (List<object>) null;
    if (result != null)
    {
      int index = 0;
      for (int count = this.m_referenceObjects.Count; index < count; ++index)
      {
        if (this.m_referenceObjects.Count > 0)
        {
          List<object> referenceObject = this.m_referenceObjects[index];
          if (referenceObject.Count > 0)
          {
            IPdfCache pdfCache = (IPdfCache) referenceObject[0];
            if (result.EqualsTo(pdfCache))
            {
              group = referenceObject;
              break;
            }
          }
          else
            this.RemoveGroup(referenceObject);
        }
      }
    }
    return group;
  }

  private void RemoveGroup(List<object> group)
  {
    if (group == null)
      return;
    this.m_referenceObjects.Remove(group);
  }
}
