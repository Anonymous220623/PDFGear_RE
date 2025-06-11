// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IBehavior
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface IBehavior
{
  bool? Accumulate { get; set; }

  BehaviorAdditiveType Additive { get; set; }

  IBehaviorProperties Properties { get; }

  ITiming Timing { get; set; }
}
