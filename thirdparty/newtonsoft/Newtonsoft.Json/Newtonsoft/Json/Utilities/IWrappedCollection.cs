// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.IWrappedCollection
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal interface IWrappedCollection : IList, ICollection, IEnumerable
{
  object UnderlyingCollection { get; }
}
