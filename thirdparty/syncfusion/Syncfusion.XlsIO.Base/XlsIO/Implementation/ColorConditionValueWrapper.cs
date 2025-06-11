// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorConditionValueWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ColorConditionValueWrapper(IConditionValue value, IOptimizedUpdate parent) : 
  ConditionValueWrapper(value, parent),
  IColorConditionValue,
  IConditionValue
{
  public Color FormatColorRGB
  {
    get => this.Wrapped.FormatColorRGB;
    set
    {
      this.BeginUpdate();
      this.Wrapped.FormatColorRGB = value;
      this.EndUpdate();
    }
  }

  public new ConditionValueType Type
  {
    get => this.Wrapped.Type;
    set
    {
      this.BeginUpdate();
      this.Wrapped.Type = value;
      this.EndUpdate();
    }
  }

  public new string Value
  {
    get => this.Wrapped.Value;
    set
    {
      this.BeginUpdate();
      this.Wrapped.Value = value;
      this.EndUpdate();
    }
  }

  public new ConditionalFormatOperator Operator
  {
    get => this.Wrapped.Operator;
    set
    {
      this.BeginUpdate();
      this.Wrapped.Operator = value;
      this.EndUpdate();
    }
  }

  private ColorConditionValue Wrapped => base.Wrapped as ColorConditionValue;
}
