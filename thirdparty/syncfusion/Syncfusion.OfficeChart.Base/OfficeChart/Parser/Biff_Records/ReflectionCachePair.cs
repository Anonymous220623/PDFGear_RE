// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ReflectionCachePair
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Reflection;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class ReflectionCachePair
{
  private BiffRecordPosAttribute[] m_key;
  private FieldInfo[] m_tag;

  public event EventHandler KeyChanged;

  public event EventHandler TagChanged;

  public BiffRecordPosAttribute[] Key
  {
    get => this.m_key;
    set
    {
      if (value == this.m_key)
        return;
      this.m_key = value;
      this.OnKeyChanged();
    }
  }

  public FieldInfo[] Tag
  {
    get => this.m_tag;
    set
    {
      if (value == this.m_tag)
        return;
      this.m_tag = value;
      this.OnTagChanged();
    }
  }

  private void OnKeyChanged()
  {
    if (this.KeyChanged == null)
      return;
    this.KeyChanged((object) this, EventArgs.Empty);
  }

  private void OnTagChanged()
  {
    if (this.TagChanged == null)
      return;
    this.TagChanged((object) this, EventArgs.Empty);
  }

  private ReflectionCachePair()
  {
  }

  public ReflectionCachePair(BiffRecordPosAttribute[] key, FieldInfo[] tag)
  {
    this.m_key = key;
    this.m_tag = tag;
  }
}
