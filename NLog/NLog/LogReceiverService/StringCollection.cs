// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.StringCollection
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

#nullable disable
namespace NLog.LogReceiverService;

[CollectionDataContract(ItemName = "l", Namespace = "http://nlog-project.org/ws/")]
public class StringCollection : Collection<string>
{
}
