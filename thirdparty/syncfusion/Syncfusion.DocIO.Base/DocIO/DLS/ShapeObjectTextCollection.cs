// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ShapeObjectTextCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

[CLSCompliant(false)]
public class ShapeObjectTextCollection
{
  private Dictionary<int, WTextBox> m_textTable = new Dictionary<int, WTextBox>();

  public void AddTextBox(int shapeId, WTextBox textBox)
  {
    if (this.m_textTable == null)
      this.m_textTable = new Dictionary<int, WTextBox>();
    this.m_textTable.Add(shapeId, textBox);
  }

  public WTextBox GetTextBox(int shapeId)
  {
    WTextBox textBox = (WTextBox) null;
    if (this.m_textTable.ContainsKey(shapeId))
    {
      textBox = this.m_textTable[shapeId];
      this.m_textTable.Remove(shapeId);
    }
    return textBox;
  }

  internal void Close()
  {
    if (this.m_textTable == null)
      return;
    this.m_textTable.Clear();
    this.m_textTable = (Dictionary<int, WTextBox>) null;
  }
}
