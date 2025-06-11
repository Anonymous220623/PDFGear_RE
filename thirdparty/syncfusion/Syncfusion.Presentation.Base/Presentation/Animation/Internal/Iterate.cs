// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.Iterate
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class Iterate
{
  private TimeAbsolute timeAbsolute;
  private TimePercentage timePercantage;
  private bool backwards;
  private IterateType type;

  internal TimeAbsolute TimeAbsolute
  {
    get => this.timeAbsolute;
    set => this.timeAbsolute = value;
  }

  internal TimePercentage TimePercantage
  {
    get => this.timePercantage;
    set => this.timePercantage = value;
  }

  internal bool Backwards
  {
    get => this.backwards;
    set => this.backwards = value;
  }

  internal IterateType Type
  {
    get => this.type;
    set => this.type = value;
  }

  internal Iterate Clone()
  {
    Iterate iterate = (Iterate) this.MemberwiseClone();
    if (this.timeAbsolute != null)
      iterate.timeAbsolute = this.timeAbsolute.Clone();
    if (this.timePercantage != null)
      iterate.timePercantage = this.timePercantage.Clone();
    return iterate;
  }
}
