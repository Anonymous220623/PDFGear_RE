// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.EventTriggerBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public abstract class EventTriggerBase : TriggerBase
{
  public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register(nameof (SourceName), typeof (string), typeof (EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceNameChanged)));
  public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register(nameof (SourceObject), typeof (object), typeof (EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceObjectChanged)));
  private MethodInfo _eventHandlerMethodInfo;

  internal EventTriggerBase(Type sourceTypeConstraint)
    : base(typeof (DependencyObject))
  {
    this.SourceTypeConstraint = sourceTypeConstraint;
    this.SourceNameResolver = new NameResolver();
    this.RegisterSourceChanged();
  }

  protected sealed override Type AssociatedObjectTypeConstraint
  {
    get
    {
      return TypeDescriptor.GetAttributes(this.GetType())[typeof (TypeConstraintAttribute)] is TypeConstraintAttribute attribute ? attribute.Constraint : typeof (DependencyObject);
    }
  }

  private bool IsLoadedRegistered { get; set; }

  private bool IsSourceChangedRegistered { get; set; }

  private bool IsSourceNameSet
  {
    get
    {
      return !string.IsNullOrEmpty(this.SourceName) || this.ReadLocalValue(EventTriggerBase.SourceNameProperty) != DependencyProperty.UnsetValue;
    }
  }

  public object Source
  {
    get
    {
      object associatedObject = (object) this.AssociatedObject;
      if (this.SourceObject != null)
        return this.SourceObject;
      if (this.IsSourceNameSet)
      {
        associatedObject = (object) this.SourceNameResolver.Object;
        if (associatedObject != null && !this.SourceTypeConstraint.IsInstanceOfType(associatedObject))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, (object) this.GetType().Name, (object) associatedObject.GetType(), (object) this.SourceTypeConstraint, (object) nameof (Source)));
      }
      return associatedObject;
    }
  }

  public string SourceName
  {
    get => (string) this.GetValue(EventTriggerBase.SourceNameProperty);
    set => this.SetValue(EventTriggerBase.SourceNameProperty, (object) value);
  }

  private NameResolver SourceNameResolver { get; }

  public object SourceObject
  {
    get => this.GetValue(EventTriggerBase.SourceObjectProperty);
    set => this.SetValue(EventTriggerBase.SourceObjectProperty, value);
  }

  protected Type SourceTypeConstraint { get; }

  protected abstract string GetEventName();

  private static bool IsValidEvent(EventInfo eventInfo)
  {
    Type eventHandlerType = eventInfo.EventHandlerType;
    if (!typeof (Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
      return false;
    ParameterInfo[] parameters = eventHandlerType.GetMethod("Invoke")?.GetParameters();
    return parameters != null && parameters.Length == 2 && typeof (object).IsAssignableFrom(parameters[0].ParameterType) && typeof (EventArgs).IsAssignableFrom(parameters[1].ParameterType);
  }

  protected override void OnAttached()
  {
    base.OnAttached();
    DependencyObject associatedObject = this.AssociatedObject;
    Behavior behavior = associatedObject as Behavior;
    FrameworkElement frameworkElement = associatedObject as FrameworkElement;
    this.RegisterSourceChanged();
    if (behavior != null)
      behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
    else if (this.SourceObject != null || frameworkElement == null)
    {
      try
      {
        this.OnSourceChanged((object) null, this.Source);
      }
      catch (InvalidOperationException ex)
      {
      }
    }
    else
      this.SourceNameResolver.NameScopeReferenceElement = frameworkElement;
    if (string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) != 0 || frameworkElement == null || Interaction.IsElementLoaded(frameworkElement))
      return;
    this.RegisterLoaded(frameworkElement);
  }

  private void OnBehaviorHostChanged(object sender, EventArgs e)
  {
    this.SourceNameResolver.NameScopeReferenceElement = ((IAttachedObject) sender).AssociatedObject as FrameworkElement;
  }

  protected override void OnDetaching()
  {
    base.OnDetaching();
    Behavior associatedObject1 = this.AssociatedObject as Behavior;
    FrameworkElement associatedObject2 = this.AssociatedObject as FrameworkElement;
    try
    {
      this.OnSourceChanged(this.Source, (object) null);
    }
    catch (InvalidOperationException ex)
    {
    }
    this.UnregisterSourceChanged();
    if (associatedObject1 != null)
      associatedObject1.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
    this.SourceNameResolver.NameScopeReferenceElement = (FrameworkElement) null;
    if (string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) != 0 || associatedObject2 == null)
      return;
    this.UnregisterLoaded(associatedObject2);
  }

  protected virtual void OnEvent(EventArgs eventArgs) => this.InvokeActions((object) eventArgs);

  private void OnEventImpl(object sender, EventArgs eventArgs) => this.OnEvent(eventArgs);

  internal void OnEventNameChanged(string oldEventName, string newEventName)
  {
    if (this.AssociatedObject == null)
      return;
    if (this.Source is FrameworkElement source && string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0)
      this.UnregisterLoaded(source);
    else if (!string.IsNullOrEmpty(oldEventName))
      this.UnregisterEvent(this.Source, oldEventName);
    if (source != null && string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0)
    {
      this.RegisterLoaded(source);
    }
    else
    {
      if (string.IsNullOrEmpty(newEventName))
        return;
      this.RegisterEvent(this.Source, newEventName);
    }
  }

  private void OnSourceChanged(object oldSource, object newSource)
  {
    if (this.AssociatedObject == null)
      return;
    this.OnSourceChangedImpl(oldSource, newSource);
  }

  internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
  {
    if (string.IsNullOrEmpty(this.GetEventName()) || string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) == 0)
      return;
    if (oldSource != null && this.SourceTypeConstraint.IsInstanceOfType(oldSource))
      this.UnregisterEvent(oldSource, this.GetEventName());
    if (newSource == null || !this.SourceTypeConstraint.IsInstanceOfType(newSource))
      return;
    this.RegisterEvent(newSource, this.GetEventName());
  }

  private static void OnSourceNameChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((EventTriggerBase) obj).SourceNameResolver.Name = (string) args.NewValue;
  }

  private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
  {
    if (this.SourceObject != null)
      return;
    this.OnSourceChanged(e.OldObject, e.NewObject);
  }

  private static void OnSourceObjectChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    EventTriggerBase eventTriggerBase = (EventTriggerBase) obj;
    object newSource = (object) eventTriggerBase.SourceNameResolver.Object;
    if (args.NewValue == null)
    {
      eventTriggerBase.OnSourceChanged(args.OldValue, newSource);
    }
    else
    {
      if (args.OldValue == null && newSource != null)
        eventTriggerBase.UnregisterEvent(newSource, eventTriggerBase.GetEventName());
      eventTriggerBase.OnSourceChanged(args.OldValue, args.NewValue);
    }
  }

  private void RegisterEvent(object obj, string eventName)
  {
    EventInfo eventInfo1 = obj.GetType().GetEvent(eventName);
    if (eventInfo1 == (EventInfo) null)
    {
      if (this.SourceObject != null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.EventTriggerCannotFindEventNameExceptionMessage, new object[2]
        {
          (object) eventName,
          (object) obj.GetType().Name
        }));
    }
    else if (!EventTriggerBase.IsValidEvent(eventInfo1))
    {
      if (this.SourceObject != null)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.EventTriggerBaseInvalidEventExceptionMessage, new object[2]
        {
          (object) eventName,
          (object) obj.GetType().Name
        }));
    }
    else
    {
      this._eventHandlerMethodInfo = typeof (EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.Instance | BindingFlags.NonPublic);
      EventInfo eventInfo2 = eventInfo1;
      object target = obj;
      Type eventHandlerType = eventInfo1.EventHandlerType;
      Delegate handler = Delegate.CreateDelegate(eventHandlerType, (object) this, this._eventHandlerMethodInfo ?? throw new InvalidOperationException());
      eventInfo2.AddEventHandler(target, handler);
    }
  }

  private void RegisterLoaded(FrameworkElement associatedElement)
  {
    if (this.IsLoadedRegistered || associatedElement == null)
      return;
    associatedElement.Loaded += new RoutedEventHandler(this.OnEventImpl);
    this.IsLoadedRegistered = true;
  }

  private void RegisterSourceChanged()
  {
    if (this.IsSourceChangedRegistered)
      return;
    this.SourceNameResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
    this.IsSourceChangedRegistered = true;
  }

  private void UnregisterEvent(object obj, string eventName)
  {
    if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
    {
      if (!(obj is FrameworkElement associatedElement))
        return;
      this.UnregisterLoaded(associatedElement);
    }
    else
      this.UnregisterEventImpl(obj, eventName);
  }

  private void UnregisterEventImpl(object obj, string eventName)
  {
    Type type = obj.GetType();
    if (!(this._eventHandlerMethodInfo != (MethodInfo) null))
      return;
    EventInfo eventInfo = type.GetEvent(eventName);
    eventInfo.RemoveEventHandler(obj, Delegate.CreateDelegate(eventInfo.EventHandlerType, (object) this, this._eventHandlerMethodInfo));
    this._eventHandlerMethodInfo = (MethodInfo) null;
  }

  private void UnregisterLoaded(FrameworkElement associatedElement)
  {
    if (!this.IsLoadedRegistered || associatedElement == null)
      return;
    associatedElement.Loaded -= new RoutedEventHandler(this.OnEventImpl);
    this.IsLoadedRegistered = false;
  }

  private void UnregisterSourceChanged()
  {
    if (!this.IsSourceChangedRegistered)
      return;
    this.SourceNameResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
    this.IsSourceChangedRegistered = false;
  }
}
