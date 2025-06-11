// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ConditionValueWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ConditionValueWrapper : IConditionValue
{
  private IConditionValue m_wrapped;
  private IOptimizedUpdate m_parent;

  public ConditionValueType Type
  {
    get => this.m_wrapped.Type;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.Type = value;
      this.EndUpdate();
    }
  }

  public string Value
  {
    get => this.m_wrapped.Value;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.Value = value;
      this.EndUpdate();
    }
  }

  public ConditionalFormatOperator Operator
  {
    get => this.m_wrapped.Operator;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.Operator = value;
      this.EndUpdate();
    }
  }

  public void BeginUpdate() => this.m_parent.BeginUpdate();

  public void EndUpdate() => this.m_parent.EndUpdate();

  internal IConditionValue Wrapped
  {
    get => this.m_wrapped;
    set => this.m_wrapped = value;
  }

  public ConditionValueWrapper(IConditionValue value, IOptimizedUpdate parent)
  {
    this.m_wrapped = value;
    this.m_parent = parent;
  }
}
