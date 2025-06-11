// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.UnknownFunctionEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

public class UnknownFunctionEventArgs : EventArgs
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
