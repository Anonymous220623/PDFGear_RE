// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ColorOffset
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

internal class ColorOffset : IColorOffset
{
  private float value0 = float.NaN;
  private float value1 = float.NaN;
  private float value2 = float.NaN;

  public float Value0
  {
    get => this.value0;
    set => this.value0 = value;
  }

  public float Value1
  {
    get => this.value1;
    set => this.value1 = value;
  }

  public float Value2
  {
    get => this.value2;
    set => this.value2 = value;
  }

  internal ColorOffset Clone() => (ColorOffset) this.MemberwiseClone();
}
