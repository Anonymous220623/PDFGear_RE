// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.PrepareTableInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal struct PrepareTableInfo
{
  internal bool InTable;
  internal int Level;
  internal int PrevLevel;
  internal PrepareTableState State;

  internal PrepareTableInfo(bool inTable, int currLevel, int prevLevel)
  {
    this.InTable = inTable;
    this.PrevLevel = prevLevel;
    this.Level = currLevel;
    if (this.Level > this.PrevLevel)
      this.State = PrepareTableState.EnterTable;
    else if (this.Level < this.PrevLevel)
      this.State = PrepareTableState.LeaveTable;
    else
      this.State = PrepareTableState.NoChange;
  }
}
