// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GPOSRecordsCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class GPOSRecordsCollection
{
  internal IDictionary<int, GPOSRecord> Records;
  internal IDictionary<int, GPOSValueRecord[]> Collection;
  internal IDictionary<int, IList<GPOSValueRecord[]>> Ligatures;

  internal GPOSRecordsCollection()
  {
    this.Records = (IDictionary<int, GPOSRecord>) new Dictionary<int, GPOSRecord>();
    this.Collection = (IDictionary<int, GPOSValueRecord[]>) new Dictionary<int, GPOSValueRecord[]>();
    this.Ligatures = (IDictionary<int, IList<GPOSValueRecord[]>>) new Dictionary<int, IList<GPOSValueRecord[]>>();
  }
}
