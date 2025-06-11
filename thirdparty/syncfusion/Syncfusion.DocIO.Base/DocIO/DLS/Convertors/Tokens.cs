// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.Tokens
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class Tokens : Groups
{
  private string tokenName;
  private string tokenValue;

  internal string TokenName
  {
    get => this.tokenName;
    set => this.tokenName = value;
  }

  internal string TokenValue
  {
    get => this.tokenValue;
    set => this.tokenValue = value;
  }
}
