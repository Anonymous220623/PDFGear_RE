// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtSpgrContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtSpgrContainer : BaseContainer
{
  internal MsofbtSp Shape
  {
    get => this.Children[0] is MsofbtSpContainer child ? child.Shape : (MsofbtSp) null;
  }

  internal MsofbtSpgrContainer(WordDocument doc)
    : base(MSOFBT.msofbtSpgrContainer, doc)
  {
  }
}
