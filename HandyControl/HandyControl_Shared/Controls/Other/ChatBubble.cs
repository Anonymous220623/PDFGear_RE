// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ChatBubble
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class ChatBubble : SelectableItem
{
  public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(nameof (Role), typeof (ChatRoleType), typeof (ChatBubble), new PropertyMetadata((object) ChatRoleType.Sender));
  public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof (Type), typeof (ChatMessageType), typeof (ChatBubble), new PropertyMetadata((object) ChatMessageType.String));
  public static readonly DependencyProperty IsReadProperty = DependencyProperty.Register(nameof (IsRead), typeof (bool), typeof (ChatBubble), new PropertyMetadata(ValueBoxes.FalseBox));

  public ChatRoleType Role
  {
    get => (ChatRoleType) this.GetValue(ChatBubble.RoleProperty);
    set => this.SetValue(ChatBubble.RoleProperty, (object) value);
  }

  public ChatMessageType Type
  {
    get => (ChatMessageType) this.GetValue(ChatBubble.TypeProperty);
    set => this.SetValue(ChatBubble.TypeProperty, (object) value);
  }

  public bool IsRead
  {
    get => (bool) this.GetValue(ChatBubble.IsReadProperty);
    set => this.SetValue(ChatBubble.IsReadProperty, ValueBoxes.BooleanBox(value));
  }

  public Action<object> ReadAction { get; set; }

  protected override void OnSelected(RoutedEventArgs e)
  {
    base.OnSelected(e);
    this.IsRead = true;
    Action<object> readAction = this.ReadAction;
    if (readAction == null)
      return;
    readAction(this.Content);
  }
}
