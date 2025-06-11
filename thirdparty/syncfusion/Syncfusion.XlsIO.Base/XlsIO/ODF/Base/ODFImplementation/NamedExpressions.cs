// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.NamedExpressions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class NamedExpressions
{
  private List<NamedExpression> m_namedExps;
  private List<NamedRange> m_namedRanges;

  internal List<NamedExpression> NamedExps
  {
    get
    {
      if (this.m_namedExps == null)
        this.m_namedExps = new List<NamedExpression>();
      return this.m_namedExps;
    }
    set => this.m_namedExps = value;
  }

  internal List<NamedRange> NamedRanges
  {
    get
    {
      if (this.m_namedRanges == null)
        this.m_namedRanges = new List<NamedRange>();
      return this.m_namedRanges;
    }
    set => this.m_namedRanges = value;
  }
}
