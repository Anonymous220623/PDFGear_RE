// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.SparklineVerticalAxis
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class SparklineVerticalAxis : ISparklineVerticalAxis
{
  private double m_customValue;
  private SparklineVerticalAxisOptions m_verticalAxisOptions;

  public double CustomValue
  {
    get => this.m_customValue;
    set
    {
      if (this.VerticalAxisOptions != SparklineVerticalAxisOptions.Custom)
        throw new NotSupportedException("It is not supported for this  Vertical Axis type");
      this.m_customValue = value;
    }
  }

  public SparklineVerticalAxisOptions VerticalAxisOptions
  {
    get => this.m_verticalAxisOptions;
    set => this.m_verticalAxisOptions = value;
  }
}
