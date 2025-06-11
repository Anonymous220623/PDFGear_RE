// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartCategories
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartCategories : 
  IParentApplication,
  ICollection<IChartCategory>,
  IEnumerable<IChartCategory>,
  IEnumerable
{
  new int Count { get; }

  IChartCategory this[int index] { get; }

  IChartCategory this[string name] { get; }
}
