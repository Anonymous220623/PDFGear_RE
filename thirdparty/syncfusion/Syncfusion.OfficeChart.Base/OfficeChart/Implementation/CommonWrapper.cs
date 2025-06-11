// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.CommonWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class CommonWrapper : IOptimizedUpdate, ICloneParent
{
  private int m_iBeginCount;

  protected int BeginCallsCount => this.m_iBeginCount;

  public virtual void BeginUpdate() => ++this.m_iBeginCount;

  public virtual void EndUpdate()
  {
    if (this.m_iBeginCount <= 0)
      return;
    --this.m_iBeginCount;
  }

  public virtual object Clone(object parent) => this.MemberwiseClone();
}
