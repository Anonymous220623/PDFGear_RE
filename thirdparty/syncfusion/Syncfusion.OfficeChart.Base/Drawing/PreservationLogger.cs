// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.PreservationLogger
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

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
