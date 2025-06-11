// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IInternalFill
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Shapes;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IInternalFill : IFill
{
  ColorObject BackColorObject { get; }

  ColorObject ForeColorObject { get; }

  bool Tile { get; set; }

  GradientStops PreservedGradient { get; set; }

  bool IsGradientSupported { get; set; }

  new float TransparencyColor { get; set; }

  new float TextureVerticalScale { get; set; }

  new float TextureHorizontalScale { get; set; }

  new float TextureOffsetX { get; set; }

  new float TextureOffsetY { get; set; }

  string Alignment { get; set; }

  string TileFlipping { get; set; }
}
