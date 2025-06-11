// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.SparklineGroups
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class SparklineGroups : 
  List<ISparklineGroup>,
  ISparklineGroups,
  IList<ISparklineGroup>,
  ICollection<ISparklineGroup>,
  IEnumerable<ISparklineGroup>,
  IEnumerable
{
  private WorkbookImpl m_book;

  public SparklineGroups(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
  }

  public void Clear(ISparklineGroup sparklineGroup)
  {
    if (sparklineGroup == null)
      throw new ArgumentNullException("SparklineGroup");
    this.Remove(sparklineGroup);
  }

  public ISparklineGroup Add()
  {
    SparklineGroup sparklineGroup = new SparklineGroup(this.m_book);
    this.Add((ISparklineGroup) sparklineGroup);
    return (ISparklineGroup) sparklineGroup;
  }
}
