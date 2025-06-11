// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IColor
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

public interface IColor
{
  Color SystemColor { get; set; }

  byte A { get; }

  byte B { get; }

  byte G { get; }

  bool IsEmpty { get; }

  bool IsKnownColor { get; }

  bool IsNamedColor { get; }

  bool IsSystemColor { get; }

  byte R { get; }

  int ToArgb();
}
