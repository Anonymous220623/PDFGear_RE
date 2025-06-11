// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Interfaces.ISerializable
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Interfaces;

[CLSCompliant(false)]
internal interface ISerializable
{
  void Serialize(IList<IBiffStorage> records);
}
