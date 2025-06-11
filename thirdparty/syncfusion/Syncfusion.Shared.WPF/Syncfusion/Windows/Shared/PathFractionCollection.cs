// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PathFractionCollection
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class PathFractionCollection : ObservableCollection<FractionValue>
{
  internal void FindNearestPoints(
    double currentStopPointPathFraction,
    out FractionValue LeftNearestStopPint,
    out FractionValue RightNearestStopPint)
  {
    double num1 = -1.0;
    double num2 = -1.0;
    FractionValue fractionValue1 = new FractionValue();
    FractionValue fractionValue2 = new FractionValue();
    if (this.Items.Count <= 0)
      throw new NotImplementedException();
    foreach (FractionValue fractionValue3 in (IEnumerable<FractionValue>) this.Items)
    {
      if (fractionValue3.Fraction >= num1 && fractionValue3.Fraction <= currentStopPointPathFraction)
      {
        num1 = fractionValue3.Fraction;
        fractionValue1 = fractionValue3;
      }
      else
      {
        num2 = fractionValue3.Fraction;
        fractionValue2 = fractionValue3;
        break;
      }
    }
    LeftNearestStopPint = num1 != -1.0 ? fractionValue1 : (FractionValue) null;
    if (num2 == -1.0)
      RightNearestStopPint = (FractionValue) null;
    else
      RightNearestStopPint = fractionValue2;
  }
}
