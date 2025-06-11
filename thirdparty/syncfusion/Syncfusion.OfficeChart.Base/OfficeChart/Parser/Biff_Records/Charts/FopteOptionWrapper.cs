// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.FopteOptionWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

internal class FopteOptionWrapper : IFopteOptionWrapper
{
  private List<MsofbtOPT.FOPTE> m_list;

  public FopteOptionWrapper() => this.m_list = new List<MsofbtOPT.FOPTE>();

  [CLSCompliant(false)]
  public FopteOptionWrapper(List<MsofbtOPT.FOPTE> list)
  {
    this.m_list = list != null ? list : throw new ArgumentNullException(nameof (list));
  }

  [CLSCompliant(false)]
  public List<MsofbtOPT.FOPTE> OptionList => this.m_list;

  [CLSCompliant(false)]
  public void AddOptionSorted(MsofbtOPT.FOPTE option)
  {
    int index = 0;
    int count = this.m_list.Count;
    int num = count;
    while (index < num && this.m_list[index].Id < option.Id)
      ++index;
    if (index < count)
    {
      if (this.m_list[index].Id == option.Id)
        this.m_list[index] = option;
      else
        this.m_list.Insert(index, option);
    }
    else
      this.m_list.Add(option);
  }

  public void RemoveOption(int index)
  {
    int index1 = 0;
    for (int count = this.m_list.Count; index1 < count; ++index1)
    {
      if (this.m_list[index1].Id == (MsoOptions) index)
      {
        this.m_list.RemoveAt(index1);
        break;
      }
    }
  }
}
