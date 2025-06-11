// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.WTFilterSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class WTFilterSpec
{
  public const byte FILTER_SPEC_MAIN_DEF = 0;
  public const byte FILTER_SPEC_COMP_DEF = 1;
  public const byte FILTER_SPEC_TILE_DEF = 2;
  public const byte FILTER_SPEC_TILE_COMP = 3;
  internal byte[] specValType;

  public abstract int WTDataType { get; }

  internal WTFilterSpec(int nc) => this.specValType = new byte[nc];

  public virtual byte getKerSpecType(int n) => this.specValType[n];
}
