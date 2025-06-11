// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IWMergeField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IWMergeField : IWField, IWTextRange, IParagraphItem, IEntity
{
  string FieldName { get; set; }

  string TextBefore { get; set; }

  string TextAfter { get; set; }

  string Prefix { get; }

  string NumberFormat { get; }

  string DateFormat { get; }
}
