// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotConditionalFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotConditionalFormat
{
  private int m_iPriority;
  private ConditionalFormatScope scope;
  private ConditionalTopNType m_formatType;
  private PivotAreaCollection m_pivotAreas;
}
