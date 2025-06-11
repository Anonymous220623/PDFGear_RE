// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ColorExtension
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

internal static class ColorExtension
{
  public static IColor Empty = ColorObject.FromArgb(0, 0, 0, 0);

  public static IColor FromArgb(int value) => ColorObject.FromArgb(value);
}
