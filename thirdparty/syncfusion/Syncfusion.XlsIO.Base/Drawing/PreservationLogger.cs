// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.PreservationLogger
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.Drawing;

internal class PreservationLogger
{
  private PreservedFlag m_flag;

  internal PreservationLogger() => this.m_flag = (PreservedFlag) 0;

  internal bool CheckFlag(PreservedFlag flag) => (this.m_flag & flag) != (PreservedFlag) 0;

  internal void SetFlag(PreservedFlag flag) => this.m_flag |= flag;

  internal void ResetFlag() => this.m_flag = (PreservedFlag) 0;

  internal bool GetPreservedItem(PreservedFlag flag)
  {
    switch (flag)
    {
      case PreservedFlag.Fill:
        return this.CheckFlag(PreservedFlag.Fill);
      case PreservedFlag.Line:
        return this.CheckFlag(PreservedFlag.Line);
      case PreservedFlag.RichText:
        return this.CheckFlag(PreservedFlag.RichText);
      default:
        return false;
    }
  }
}
