// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RelationCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class RelationCollection
{
  private Dictionary<string, Relation> _list;
  private List<string> _removeElementList;

  internal RelationCollection()
  {
    this._list = new Dictionary<string, Relation>();
    this._removeElementList = new List<string>();
  }

  internal int Count => this._list.Count;

  internal void Add(string relationId, Relation relation)
  {
    if ((this._list.ContainsKey(relationId) || this.CheckTarget(relation)) && !(relation.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/video") && !(relation.Type == "http://schemas.microsoft.com/office/2007/relationships/media"))
      return;
    this._list.Add(relationId, relation);
  }

  private bool CheckTarget(Relation target)
  {
    foreach (Relation relation in this.GetRelationList())
    {
      if (target.Target.Contains("comments/comment") || target.Target.Contains("commentAuthors.xml"))
      {
        if (relation.Target == target.Target)
          return true;
      }
      else if (relation.Target == target.Target && relation.Id == target.Id)
        return true;
    }
    return false;
  }

  internal void Clear() => this._list.Clear();

  internal bool Contains(string id) => this._list.ContainsKey(id);

  internal List<string> GetImageRemoveList() => this._removeElementList;

  internal string GetItemByContentType(string contentType)
  {
    foreach (KeyValuePair<string, Relation> keyValuePair in this._list)
    {
      if (keyValuePair.Value.Type == contentType)
        return keyValuePair.Value.Target;
    }
    return (string) null;
  }

  internal Relation GetRelationByContentType(string contentType)
  {
    foreach (KeyValuePair<string, Relation> keyValuePair in this._list)
    {
      if (keyValuePair.Value.Type == contentType)
        return keyValuePair.Value;
    }
    return (Relation) null;
  }

  internal string GetItemPathByRelation(string slideRelationId)
  {
    return this._list[slideRelationId].Target;
  }

  internal List<Relation> GetRelationList()
  {
    Relation[] relationArray = new Relation[this._list.Count];
    this._list.Values.CopyTo(relationArray, 0);
    return new List<Relation>((IEnumerable<Relation>) relationArray);
  }

  internal void RemoveRelation(string relationKey)
  {
    if (!this._list.ContainsKey(relationKey))
      return;
    this._list.Remove(relationKey);
  }

  internal string GetIdByTarget(string target)
  {
    foreach (Relation relation in this._list.Values)
    {
      if (relation.Target == target)
        return relation.Id;
    }
    return (string) null;
  }

  internal string GetTarget(string value)
  {
    foreach (Relation relation in this.GetRelationList())
    {
      if (relation.Target.Contains(value))
        return relation.Target;
    }
    return (string) null;
  }

  internal string GetTargetByRelationId(string relationId)
  {
    return relationId != null ? this._list[relationId].Target : (string) null;
  }

  internal int RemoveLayoutRelation()
  {
    int num = 0;
    foreach (Relation relation in this.GetRelationList())
    {
      if (relation.Type.EndsWith("slideLayout"))
      {
        this._list.Remove(relation.Id);
        ++num;
      }
    }
    return num;
  }

  internal string GetItemPathByKeyword(string keyword)
  {
    foreach (Relation relation in this.GetRelationList())
    {
      if (relation.Type.EndsWith(keyword))
        return relation.Target;
    }
    return (string) null;
  }

  internal void Update(Relation relation)
  {
    Relation relation1 = this._list[relation.Id];
    relation1.Target = relation.Target;
    relation1.Type = relation.Type;
  }

  internal void RemoveRelationByKeword(string keyword)
  {
    foreach (Relation relation in this.GetRelationList())
    {
      if (relation.Type.EndsWith(keyword))
        this._list.Remove(relation.Id);
    }
  }

  internal List<string> GetImagePathList()
  {
    List<string> imagePathList = new List<string>();
    foreach (Relation relation in this.GetRelationList())
    {
      if (relation.Target.Contains("media"))
      {
        string str = "ppt" + relation.Target.Remove(0, 2);
        imagePathList.Add(str);
      }
    }
    return imagePathList;
  }

  internal void Close()
  {
    if (this._list != null)
    {
      this._list.Clear();
      this._list = (Dictionary<string, Relation>) null;
    }
    if (this._removeElementList == null)
      return;
    this._removeElementList.Clear();
    this._removeElementList = (List<string>) null;
  }

  public RelationCollection Clone()
  {
    RelationCollection relationCollection = (RelationCollection) this.MemberwiseClone();
    relationCollection._list = this.CloneRelationDictionary();
    relationCollection._removeElementList = Helper.CloneList(this._removeElementList);
    return relationCollection;
  }

  private Dictionary<string, Relation> CloneRelationDictionary()
  {
    Dictionary<string, Relation> dictionary = new Dictionary<string, Relation>();
    foreach (KeyValuePair<string, Relation> keyValuePair in this._list)
    {
      Relation relation = keyValuePair.Value.Clone();
      dictionary.Add(keyValuePair.Key, relation);
    }
    return dictionary;
  }

  internal string RemoveRelationByTypeContains(string containsString)
  {
    string str = (string) null;
    foreach (Relation relation in this.GetRelationList())
    {
      if (relation.Type.Contains(containsString))
      {
        this._list.Remove(relation.Id);
        str = relation.Target;
      }
    }
    return str;
  }
}
