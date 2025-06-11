// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ITextBodyItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface ITextBodyItem : IEntity
{
  int Replace(Regex pattern, string replace);

  int Replace(string given, string replace, bool caseSensitive, bool wholeWord);

  int Replace(Regex pattern, TextSelection textSelection);
}
