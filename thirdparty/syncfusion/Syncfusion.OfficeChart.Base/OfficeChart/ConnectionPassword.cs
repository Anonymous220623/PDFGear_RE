// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ConnectionPassword
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

internal class ConnectionPassword : EventArgs
{
  private string m_connectionPassword;

  public string PasswordToConnectDB
  {
    get => this.m_connectionPassword;
    set => this.m_connectionPassword = value;
  }
}
