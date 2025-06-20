﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonLoadSettings
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Linq;

public class JsonLoadSettings
{
  private CommentHandling _commentHandling;
  private LineInfoHandling _lineInfoHandling;
  private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;

  public JsonLoadSettings()
  {
    this._lineInfoHandling = LineInfoHandling.Load;
    this._commentHandling = CommentHandling.Ignore;
    this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
  }

  public CommentHandling CommentHandling
  {
    get => this._commentHandling;
    set
    {
      this._commentHandling = value >= CommentHandling.Ignore && value <= CommentHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public LineInfoHandling LineInfoHandling
  {
    get => this._lineInfoHandling;
    set
    {
      this._lineInfoHandling = value >= LineInfoHandling.Ignore && value <= LineInfoHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
  {
    get => this._duplicatePropertyNameHandling;
    set
    {
      this._duplicatePropertyNameHandling = value >= DuplicatePropertyNameHandling.Replace && value <= DuplicatePropertyNameHandling.Error ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
