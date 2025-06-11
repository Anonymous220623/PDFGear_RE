// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorScaleImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ColorScaleImpl : IColorScale
{
  private static readonly Color[] DefaultColors2 = new Color[2]
  {
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 113, 40),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 239, 156)
  };
  private static readonly Color[] DefaultColors3 = new Color[3]
  {
    Color.FromArgb((int) byte.MaxValue, 248, 105, 107),
    Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 235, 132),
    Color.FromArgb((int) byte.MaxValue, 99, 190, 123)
  };
  private IList<IColorConditionValue> m_arrCriteria = (IList<IColorConditionValue>) new List<IColorConditionValue>(3);

  public IList<IColorConditionValue> Criteria => this.m_arrCriteria;

  public void SetConditionCount(int count)
  {
    if (count < 2 || count > 3)
      throw new ArgumentOutOfRangeException(nameof (count), "Only 2 or 3 can be used as color count.");
    this.UpdateCount(count);
  }

  public ColorScaleImpl() => this.SetConditionCount(2);

  private void UpdateCount(int count)
  {
    this.m_arrCriteria.Clear();
    Color[] colorArray1 = count == 2 ? ColorScaleImpl.DefaultColors2 : ColorScaleImpl.DefaultColors3;
    int num1 = 0;
    IList<IColorConditionValue> arrCriteria1 = this.m_arrCriteria;
    Color[] colorArray2 = colorArray1;
    int index1 = num1;
    int num2 = index1 + 1;
    ColorConditionValue colorConditionValue1 = new ColorConditionValue(ConditionValueType.LowestValue, "0", colorArray2[index1]);
    arrCriteria1.Add((IColorConditionValue) colorConditionValue1);
    if (count == 3)
      this.m_arrCriteria.Add((IColorConditionValue) new ColorConditionValue(ConditionValueType.Percentile, "50", colorArray1[num2++]));
    IList<IColorConditionValue> arrCriteria2 = this.m_arrCriteria;
    Color[] colorArray3 = colorArray1;
    int index2 = num2;
    int num3 = index2 + 1;
    ColorConditionValue colorConditionValue2 = new ColorConditionValue(ConditionValueType.HighestValue, "0", colorArray3[index2]);
    arrCriteria2.Add((IColorConditionValue) colorConditionValue2);
  }
}
