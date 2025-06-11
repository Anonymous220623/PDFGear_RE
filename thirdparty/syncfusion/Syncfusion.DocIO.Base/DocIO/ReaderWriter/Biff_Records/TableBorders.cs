// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class TableBorders
{
  private BorderCode[] m_brcArr = new BorderCode[6];

  internal TableBorders() => this.Init();

  internal BorderCode this[int index]
  {
    get => this.m_brcArr[index];
    set => this.m_brcArr[index] = value;
  }

  internal BorderCode LeftBorder
  {
    get => this.m_brcArr[1];
    set => this.m_brcArr[1] = value;
  }

  internal BorderCode RightBorder
  {
    get => this.m_brcArr[3];
    set => this.m_brcArr[3] = value;
  }

  internal BorderCode TopBorder
  {
    get => this.m_brcArr[0];
    set => this.m_brcArr[0] = value;
  }

  internal BorderCode BottomBorder
  {
    get => this.m_brcArr[2];
    set => this.m_brcArr[2] = value;
  }

  internal BorderCode HorizontalBorder
  {
    get => this.m_brcArr[4];
    set => this.m_brcArr[4] = value;
  }

  internal BorderCode VerticalBorder
  {
    get => this.m_brcArr[5];
    set => this.m_brcArr[5] = value;
  }

  private void Init()
  {
    for (int index = 0; index < 6; ++index)
      this.m_brcArr[index] = new BorderCode();
  }
}
