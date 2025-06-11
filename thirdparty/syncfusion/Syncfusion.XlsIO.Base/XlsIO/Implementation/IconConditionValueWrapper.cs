// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.IconConditionValueWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class IconConditionValueWrapper(IConditionValue value, IOptimizedUpdate parent) : 
  ConditionValueWrapper(value, parent),
  IIconConditionValue,
  IConditionValue
{
  public ExcelIconSetType IconSet
  {
    get => this.Wrapped.IconSet;
    set
    {
      this.BeginUpdate();
      this.Wrapped.IconSet = value;
      this.EndUpdate();
    }
  }

  public int Index
  {
    get => this.Wrapped.Index;
    set
    {
      this.BeginUpdate();
      this.Wrapped.Index = value;
      this.EndUpdate();
    }
  }

  private IconConditionValue Wrapped => base.Wrapped as IconConditionValue;
}
