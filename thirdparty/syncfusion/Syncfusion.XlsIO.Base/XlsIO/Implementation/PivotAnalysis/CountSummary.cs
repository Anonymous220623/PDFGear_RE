// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.CountSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class CountSummary : SummaryBase, IAdjustable
{
  internal int? count = new int?();

  public override string ToString() => "Count";

  public override void Combine(object other)
  {
    this.count = new int?(this.count ?? 0);
    switch (other)
    {
      case DBNull _:
      case null:
        if (!this.ShowNullAsBlank)
          break;
        int? count1 = this.count;
        if ((count1.GetValueOrDefault() != 0 ? 0 : (count1.HasValue ? 1 : 0)) == 0)
          break;
        this.count = new int?();
        break;
      default:
        CountSummary countSummary = this;
        int? count2 = countSummary.count;
        countSummary.count = count2.HasValue ? new int?(count2.GetValueOrDefault() + 1) : new int?();
        break;
    }
  }

  public override void CombineSummary(SummaryBase other)
  {
    if (other != null && other.GetResult() == null)
    {
      CountSummary countSummary = this;
      int? count = countSummary.count;
      countSummary.count = count.HasValue ? new int?(count.GetValueOrDefault()) : new int?();
    }
    else
    {
      this.count = new int?(this.count ?? 0);
      CountSummary countSummary = this;
      int? count1 = countSummary.count;
      int? count2 = ((CountSummary) other).count;
      countSummary.count = count1.HasValue & count2.HasValue ? new int?(count1.GetValueOrDefault() + count2.GetValueOrDefault()) : new int?();
    }
  }

  public override void Reset() => this.count = new int?();

  public override object GetResult() => (object) this.count;

  public override SummaryBase GetInstance() => (SummaryBase) new CountSummary();

  public void AdjustForNewContribution(object newContribution)
  {
    this.count = new int?(this.count ?? 0);
    CountSummary countSummary = this;
    int? count = countSummary.count;
    countSummary.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
  }

  public void AdjustForOldContribution(object oldContribution)
  {
    this.count = new int?(this.count ?? 0);
    CountSummary countSummary = this;
    int? count = countSummary.count;
    countSummary.count = count.HasValue ? new int?(count.GetValueOrDefault() - 1) : new int?();
  }
}
