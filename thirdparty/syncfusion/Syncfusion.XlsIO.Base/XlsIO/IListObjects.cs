// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IListObjects
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IListObjects : 
  IList<IListObject>,
  ICollection<IListObject>,
  IEnumerable<IListObject>,
  IEnumerable
{
  IListObject Create(string name, IRange range);

  IListObject AddEx(ExcelListObjectSourceType type, IConnection connection, IRange Destination);

  new bool Remove(IListObject listObject);

  new void RemoveAt(int index);
}
