// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.PasswordRequiredEventArgs
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

internal class PasswordRequiredEventArgs : EventArgs
{
  private bool m_bStopParsing;
  private string m_strNewPassword;

  public bool StopParsing
  {
    get => this.m_bStopParsing;
    set => this.m_bStopParsing = value;
  }

  public string NewPassword
  {
    get => this.m_strNewPassword;
    set => this.m_strNewPassword = value;
  }
}
