// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorConditionValue
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ColorConditionValue : ConditionValue, IColorConditionValue, IConditionValue
{
  private Color m_color;

  public Color FormatColorRGB
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public ColorConditionValue(ConditionValueType type, string value, Color color)
    : base(type, value)
  {
    this.m_color = color;
  }

  internal ColorConditionValue()
  {
  }
}
