// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordTextBoxReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordTextBoxReader : WordSubdocumentReader
{
  public WordTextBoxReader(WordReader mainReader)
    : base(mainReader)
  {
    this.m_type = WordSubdocument.TextBox;
  }

  protected override void CreateStatePositions()
  {
    this.m_statePositions = (StatePositionsBase) new TextBoxStatePositions(this.m_docInfo.FkpData);
    base.CreateStatePositions();
  }
}
