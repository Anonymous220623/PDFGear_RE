// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sparklines
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class Sparklines : 
  List<ISparkline>,
  ISparklines,
  IList<ISparkline>,
  ICollection<ISparkline>,
  IEnumerable<ISparkline>,
  IEnumerable
{
  internal SparklineGroup ParentGroup;

  public Sparkline Add()
  {
    Sparkline sparkline = new Sparkline();
    sparkline.Parent = (ISparklines) this;
    this.Add((ISparkline) sparkline);
    return sparkline;
  }

  public void Add(IRange dataRange, IRange referenceRange)
  {
    this.Add(dataRange, referenceRange, false);
  }

  public void RefreshRanges(IRange dataRange, IRange referenceRange)
  {
    this.RefreshRanges(dataRange, referenceRange, false);
  }

  public void Add(IRange dataRange, IRange referenceRange, bool isVertical)
  {
    if (isVertical)
    {
      if (referenceRange.Rows.Length != dataRange.Columns.Length)
        throw new ArgumentOutOfRangeException("range", "The reference for the location or data range is not valid.");
    }
    else if (referenceRange.Rows.Length > dataRange.Rows.Length || referenceRange.Columns.Length > dataRange.Columns.Length)
      throw new ArgumentOutOfRangeException("range", "The reference for the location or data range is not valid.");
    this.UpdateSparklines(dataRange, referenceRange, isVertical);
  }

  public void RefreshRanges(IRange dataRange, IRange referenceRange, bool isVertical)
  {
    if (isVertical)
    {
      if (referenceRange.Rows.Length != dataRange.Columns.Length)
        throw new ArgumentOutOfRangeException("range", "The reference for the location or data range is not valid.");
    }
    else if (referenceRange.Rows.Length > dataRange.Rows.Length || referenceRange.Columns.Length > dataRange.Columns.Length)
      throw new ArgumentOutOfRangeException("range", "The reference for the location or data range is not valid.");
    this.Clear();
    this.UpdateSparklines(dataRange, referenceRange, isVertical);
  }

  public void Clear(Sparkline sparkline) => this.Remove((ISparkline) sparkline);

  internal void UpdateSparklines(IRange dataRange, IRange referenceRange, bool isVertical)
  {
    if (isVertical)
    {
      if (referenceRange.Columns.Length > 1 && referenceRange.Rows.Length == 1)
      {
        int index1 = 0;
        for (int index2 = 0; index1 < dataRange.Columns.Length && index2 < referenceRange.Columns.Length; ++index2)
        {
          this.Add((ISparkline) new Sparkline()
          {
            Parent = (ISparklines) this,
            DataRange = (dataRange.AddressGlobal.Contains(",") || dataRange.Columns.Length != referenceRange.Columns.Length ? dataRange : dataRange.Columns[index1]),
            ReferenceRange = referenceRange.Columns[index2]
          });
          ++index1;
        }
      }
      else
      {
        int index3 = 0;
        for (int index4 = 0; index3 < dataRange.Columns.Length && index4 < referenceRange.Rows.Length; ++index4)
        {
          this.Add((ISparkline) new Sparkline()
          {
            Parent = (ISparklines) this,
            DataRange = (dataRange.AddressGlobal.Contains(",") || dataRange.Columns.Length != referenceRange.Rows.Length ? dataRange : dataRange.Columns[index3]),
            ReferenceRange = referenceRange.Rows[index4]
          });
          ++index3;
        }
      }
    }
    else if (referenceRange.Columns.Length > 1 && referenceRange.Rows.Length == 1)
    {
      int index5 = 0;
      for (int index6 = 0; index5 < dataRange.Rows.Length && index6 < referenceRange.Columns.Length; ++index6)
      {
        this.Add((ISparkline) new Sparkline()
        {
          Parent = (ISparklines) this,
          DataRange = (dataRange.AddressGlobal.Contains(",") || dataRange.Rows.Length != referenceRange.Columns.Length ? dataRange : dataRange.Rows[index5]),
          ReferenceRange = referenceRange.Columns[index6]
        });
        ++index5;
      }
    }
    else
    {
      int index7 = 0;
      for (int index8 = 0; index7 < dataRange.Rows.Length && index8 < referenceRange.Rows.Length; ++index8)
      {
        this.Add((ISparkline) new Sparkline()
        {
          Parent = (ISparklines) this,
          DataRange = (dataRange.AddressGlobal.Contains(",") || dataRange.Rows.Length != referenceRange.Rows.Length ? dataRange : dataRange.Rows[index7]),
          ReferenceRange = referenceRange.Rows[index8]
        });
        ++index7;
      }
    }
  }
}
