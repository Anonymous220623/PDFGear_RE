// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.IWordImageReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

internal interface IWordImageReader
{
  Image Image { get; }

  int WidthScale { get; }

  int HeightScale { get; }

  short Width { get; }

  short Height { get; }
}
