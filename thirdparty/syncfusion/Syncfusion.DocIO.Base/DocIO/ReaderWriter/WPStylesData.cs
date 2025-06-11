// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WPStylesData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

internal class WPStylesData
{
  private WPTablesData m_tablesData;

  [CLSCompliant(false)]
  internal WPStylesData(WPTablesData tables) => this.m_tablesData = tables;

  [CLSCompliant(false)]
  internal StyleSheetInfoRecord StyleSheetInfo => this.m_tablesData.StyleSheetInfo;

  [CLSCompliant(false)]
  internal StyleDefinitionRecord[] StyleDefinitions => this.m_tablesData.StyleDefinitions;

  [CLSCompliant(false)]
  internal StyleDefinitionRecord GetStyleRecordByID(int styleID)
  {
    for (int index = 0; index < this.StyleDefinitions.Length; ++index)
    {
      StyleDefinitionRecord styleDefinition = this.StyleDefinitions[index];
      if ((int) styleDefinition.StyleId == styleID)
        return styleDefinition;
    }
    return (StyleDefinitionRecord) null;
  }
}
