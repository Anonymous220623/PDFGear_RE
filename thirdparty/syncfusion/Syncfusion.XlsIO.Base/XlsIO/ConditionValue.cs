// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ConditionValue
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.Formula;

#nullable disable
namespace Syncfusion.XlsIO;

public class ConditionValue : IConditionValue
{
  private ConditionValueType m_type;
  private string m_value;
  private ConditionalFormatOperator m_condition = ConditionalFormatOperator.GreaterThanorEqualTo;
  internal Ptg[] ref3Dptg;

  public ConditionValueType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public string Value
  {
    get => this.m_value;
    set
    {
      if (!(this.m_value != value))
        return;
      this.RefPtg = (Ptg[]) null;
      this.m_value = value;
    }
  }

  public ConditionalFormatOperator Operator
  {
    get => this.m_condition;
    set => this.m_condition = value;
  }

  internal Ptg[] RefPtg
  {
    get => this.ref3Dptg;
    set => this.ref3Dptg = value;
  }

  public static bool operator ==(ConditionValue first, ConditionValue second)
  {
    return first.m_type == second.m_type && first.m_condition == second.m_condition && first.m_value == second.m_value;
  }

  public static bool operator !=(ConditionValue first, ConditionValue second) => !(first == second);

  public ConditionValue(ConditionValueType type, string value)
  {
    this.m_type = type;
    this.m_value = value;
  }

  internal ConditionValue()
  {
  }

  internal ConditionValue Clone() => (ConditionValue) this.MemberwiseClone();

  public override bool Equals(object obj)
  {
    ConditionValue conditionValue = obj as ConditionValue;
    return obj != null && conditionValue == this;
  }

  public override int GetHashCode() => this.m_type.GetHashCode() ^ this.m_value.GetHashCode();
}
