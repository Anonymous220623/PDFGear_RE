// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.TextSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class TextSummary : SummaryBase
{
  internal string text;

  public override void Combine(object other) => this.text = $"{this.text}_{other.ToString()}";

  public override void CombineSummary(SummaryBase other)
  {
    this.text = $"{this.text}_{((TextSummary) other).text}";
  }

  public override void Reset() => this.text = "";

  public override object GetResult() => (object) this.text;

  public override SummaryBase GetInstance() => (SummaryBase) new TextSummary();
}
