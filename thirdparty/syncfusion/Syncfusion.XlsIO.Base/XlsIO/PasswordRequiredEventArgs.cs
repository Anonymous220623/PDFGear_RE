// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.PasswordRequiredEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public class PasswordRequiredEventArgs : EventArgs
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
