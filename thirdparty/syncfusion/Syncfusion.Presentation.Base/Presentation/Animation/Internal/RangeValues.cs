// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.RangeValues
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class RangeValues
{
  private uint start;
  private uint end;

  internal uint Start
  {
    get => this.start;
    set => this.start = value;
  }

  internal uint End
  {
    get => this.end;
    set => this.end = value;
  }

  internal RangeValues Clone() => (RangeValues) this.MemberwiseClone();
}
