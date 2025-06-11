// Decompiled with JetBrains decompiler
// Type: NLog.Config.INamedItemFactory`2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Config;

public interface INamedItemFactory<TInstanceType, TDefinitionType> where TInstanceType : class
{
  void RegisterDefinition(string itemName, TDefinitionType itemDefinition);

  bool TryGetDefinition(string itemName, out TDefinitionType result);

  TInstanceType CreateInstance(string itemName);

  bool TryCreateInstance(string itemName, out TInstanceType result);
}
