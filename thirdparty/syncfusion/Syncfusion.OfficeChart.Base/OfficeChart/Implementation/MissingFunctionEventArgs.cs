// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.MissingFunctionEventArgs
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class MissingFunctionEventArgs : EventArgs
{
  private string m_missingFunctionName;
  private string m_cellLocation;

  public string MissingFunctionName
  {
    get => this.m_missingFunctionName;
    internal set => this.m_missingFunctionName = value;
  }

  public string CellLocation
  {
    get => this.m_cellLocation;
    internal set => this.m_cellLocation = value;
  }
}
