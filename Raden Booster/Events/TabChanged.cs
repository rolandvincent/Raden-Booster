using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace Raden_Booster
{
    public class TabChanged
    {
        [ContentProperty("Actions")]
        public class IsSelectedControlTrigger : FrameworkContentElement
        {
            public RoutedEvent RoutedEvent { get; set; }
            public List<TriggerAction> Actions { get; set; }

            // Condition
            public bool IsSelected { get { return (bool)GetValue(IsSelectedProperty); } set { SetValue(IsSelectedProperty, value); } }
            public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(IsSelectedControlTrigger));

            // "Triggers" attached property
            public static ConditionalEventTriggerCollection GetTriggers(DependencyObject obj) { return (ConditionalEventTriggerCollection)obj.GetValue(TriggersProperty); }
            public static void SetTriggers(DependencyObject obj, ConditionalEventTriggerCollection value) { obj.SetValue(TriggersProperty, value); }
            public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("Triggers", typeof(ConditionalEventTriggerCollection), typeof(IsSelectedControlTrigger), new PropertyMetadata
            {
                PropertyChangedCallback = (obj, e) =>
                {
                    // When "Triggers" is set, register handlers for each trigger in the list 
                    var element = (FrameworkElement)obj;
                    var triggers = (List<IsSelectedControlTrigger>)e.NewValue;
                    foreach (var trigger in triggers)
                        element.AddHandler(trigger.RoutedEvent, new RoutedEventHandler((obj2, e2) =>
                          trigger.OnRoutedEvent(element)));
                }
            });

            public IsSelectedControlTrigger()
            {
                Actions = new List<TriggerAction>();
            }

            // When an event fires, check the condition and if it is true fire the actions 
            void OnRoutedEvent(FrameworkElement element)
            {
                DataContext = element.DataContext;  // Allow data binding to access element properties
                if (IsSelected)
                {
                    // Construct an EventTrigger containing the actions, then trigger it 
                    var dummyTrigger = new EventTrigger { RoutedEvent = _triggerActionsEvent };
                    foreach (var action in Actions)
                        dummyTrigger.Actions.Add(action);

                    element.Triggers.Add(dummyTrigger);
                    try
                    {
                        element.RaiseEvent(new RoutedEventArgs(_triggerActionsEvent));
                    }
                    finally
                    {
                        element.Triggers.Remove(dummyTrigger);
                    }
                }
            }

            static RoutedEvent _triggerActionsEvent = EventManager.RegisterRoutedEvent("", RoutingStrategy.Direct, typeof(EventHandler), typeof(IsSelectedControlTrigger));

        }

        // Create collection type visible to XAML - since it is attached we cannot construct it in code 
        public class ConditionalEventTriggerCollection : List<IsSelectedControlTrigger> { }
    }
}
