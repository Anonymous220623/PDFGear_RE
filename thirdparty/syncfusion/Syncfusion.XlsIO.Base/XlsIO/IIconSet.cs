// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IIconSet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IIconSet
{
  IList<IConditionValue> IconCriteria { get; }

  ExcelIconSetType IconSet { get; set; }

  bool PercentileValues { get; set; }

  bool ReverseOrder { get; set; }

  bool ShowIconOnly { get; set; }
}
