// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.PresetStampTextModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using pdfeditor.Utils;
using System;

#nullable disable
namespace pdfeditor.ViewModels;

public class PresetStampTextModel : IStampTextModel
{
  public PresetStampTextModel(StampIconNames stampIconName, string fontColor, string groupId)
  {
    if (this.IconName == StampIconNames.Extended)
      throw new ArgumentException(nameof (stampIconName));
    this.IconName = stampIconName;
    this.TextContent = ToolbarContextMenuHelper.GetPresetStampTextContext(stampIconName);
    this.FontColor = fontColor;
    this.GroupId = groupId;
  }

  public StampIconNames IconName { get; }

  public string TextContent { get; }

  public string FontColor { get; }

  public string GroupId { get; }

  public bool IsPreset => true;
}
