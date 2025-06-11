// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.GroupPoint
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal struct GroupPoint
{
  private int m_x;
  private int m_y;
  private bool m_bIsparse;

  internal GroupPoint(int x, int y)
  {
    this.m_x = x;
    this.m_y = y;
    this.m_bIsparse = false;
  }

  internal int X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  internal int Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  internal bool IsParse
  {
    get => this.m_bIsparse;
    set => this.m_bIsparse = value;
  }
}
