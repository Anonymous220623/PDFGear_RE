// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.DebugHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Concurrent;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom;

public class DebugHelper
{
  private ConcurrentQueue<Key> lastPressedKeys;

  public DebugHelper(string preset) => this.Preset = preset?.Trim() ?? string.Empty;

  public string Preset { get; }

  public void ProcessKeyDown(Key key)
  {
    if (this.lastPressedKeys == null)
      this.lastPressedKeys = new ConcurrentQueue<Key>();
    try
    {
      this.lastPressedKeys.Enqueue(key);
      while (this.lastPressedKeys.Count > this.Preset.Length)
        this.lastPressedKeys.TryDequeue(out Key _);
      if (this.lastPressedKeys.Count != this.Preset.Length)
        return;
      Key[] array = this.lastPressedKeys.ToArray();
      int index = 0;
      while (index < array.Length && array[index] - 44 + 97 == (Key) this.Preset[index])
        ++index;
      if (index != this.Preset.Length)
        return;
      this.lastPressedKeys = new ConcurrentQueue<Key>();
      EventHandler processed = this.Processed;
      if (processed == null)
        return;
      processed((object) this, EventArgs.Empty);
    }
    catch
    {
    }
  }

  public event EventHandler Processed;
}
