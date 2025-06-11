// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CharacterProperties
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class CharacterProperties : ICharacterProperties
{
  private string _regExpression;
  private bool _isLiteral;
  private bool? _isUpper;
  private bool? _isPromptchar;
  private bool? _isOptional;

  public string RegExpression
  {
    get => this._regExpression;
    set => this._regExpression = value;
  }

  public bool IsLiteral
  {
    get => this._isLiteral;
    set => this._isLiteral = value;
  }

  public bool? IsUpper
  {
    get => this._isUpper;
    set => this._isUpper = value;
  }

  public bool? IsPromptCharacter
  {
    get => this._isPromptchar;
    set => this._isPromptchar = value;
  }

  public bool? IsOptional
  {
    get => this._isOptional;
    set => this._isOptional = value;
  }
}
