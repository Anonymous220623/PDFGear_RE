// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ISortField
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ISortField
{
  int Key { get; set; }

  SortOn SortOn { get; set; }

  OrderBy Order { get; set; }

  Color Color { get; set; }

  void SetPriority(int priority);
}
