// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.SummaryBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public abstract class SummaryBase
{
  private bool showNullAsBlank;

  public bool ShowNullAsBlank
  {
    get => this.showNullAsBlank;
    set => this.showNullAsBlank = value;
  }

  public abstract void Combine(object other);

  public abstract void Reset();

  public abstract object GetResult();

  public abstract SummaryBase GetInstance();

  public abstract void CombineSummary(SummaryBase other);
}
