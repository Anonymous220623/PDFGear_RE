// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DisplayIfDiscreteValuesEqual
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DisplayIfDiscreteValuesEqual : SummaryBase
{
  private object commonValue;
  private bool isCommon;
  private string padString = "*";

  public DisplayIfDiscreteValuesEqual()
  {
    this.commonValue = (object) null;
    this.isCommon = false;
  }

  public string PadString
  {
    get => this.padString;
    set => this.padString = value;
  }

  public override void Combine(object other)
  {
    if (other != null)
    {
      if (this.commonValue == null)
      {
        this.commonValue = other;
        this.isCommon = this.commonValue.Equals(other);
      }
      else
        this.isCommon &= this.commonValue.Equals(other);
    }
    else
    {
      if (other != null)
        return;
      this.isCommon = true;
    }
  }

  public override void Reset()
  {
    this.commonValue = (object) null;
    this.isCommon = false;
  }

  public override object GetResult() => !this.isCommon ? (object) this.PadString : this.commonValue;

  public override SummaryBase GetInstance()
  {
    return (SummaryBase) new DisplayIfDiscreteValuesEqual()
    {
      PadString = this.PadString
    };
  }

  public override void CombineSummary(SummaryBase other)
  {
    if (!(other is DisplayIfDiscreteValuesEqual discreteValuesEqual))
      return;
    if (this.commonValue == null && discreteValuesEqual.commonValue != null)
    {
      this.commonValue = discreteValuesEqual.commonValue;
      this.isCommon = discreteValuesEqual.isCommon;
    }
    else
      this.isCommon = discreteValuesEqual.commonValue == null && this.commonValue == null || this.commonValue.Equals(discreteValuesEqual.commonValue) && discreteValuesEqual.isCommon && this.isCommon;
  }
}
