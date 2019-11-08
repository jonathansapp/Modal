using System;
using System.Linq.Expressions;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Blazored.Modal.Services
{
    /// <summary>
    /// Represents a configuration of a modal dialog instance (title, display options, parameters, event callbacks).
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public class ModalDialogInteraction<TComponent> : IModalDialogInteraction<TComponent>
        where TComponent : IComponent
    {
        private readonly ModalService modalService;
        private readonly string title;
        private readonly ModalOptions options;
        private readonly ComponentParameterSupport<TComponent> componentParameterSupport = new ComponentParameterSupport<TComponent>();

        public ModalDialogInteraction(ModalService modalService, string title, ModalOptions options)
        {
            this.modalService = modalService ?? throw new ArgumentNullException(nameof(modalService));
            this.title = title ?? throw new ArgumentNullException(nameof(title));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Called when the modal dialog, belonging to the interaction, is closed.
        /// </summary>
        public event Action<ModalResult> OnClose;

        /// <summary>
        /// Shows the modal dialog.
        /// </summary>
        /// <returns>itself, for the purpose of method chaining</returns>
        public IModalDialogInteraction<TComponent> Show()
        {
            var content = new RenderFragment(renderTreeBuilder =>
            {
                renderTreeBuilder.OpenComponent(1, typeof(TComponent));
                componentParameterSupport.Execute(renderTreeBuilder, 2);
                renderTreeBuilder.CloseComponent();
            });

            modalService.TriggerShowEvent(title, content, new ModalParameters(), new ModalOptions());
            return this;
        }

        /// <summary>
        /// Closes the modal dialog interaction.
        /// </summary>
        /// <param name="modalResult"></param>
        public void Close(ModalResult modalResult)
        {
            modalService.Close(modalResult);
            OnClose?.Invoke(modalResult);
        }

        /// <summary>
        /// Adds a parameter to the wrapped component.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expr">a MemberExpression pointing to the property or field being set</param>
        /// <param name="value">the value to which the member will be set</param>
        /// <returns>itself, for the purpose of method chaining</returns>
        public IModalDialogInteraction<TComponent> AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> expr, TProperty value)
        {
            componentParameterSupport.AddParameter(expr, value);
            return this;
        }

        /// <summary>
        /// Binds an EventCallback to the wrapped component.
        /// </summary>
        /// <typeparam name="TCallbackType"></typeparam>
        /// <param name="expr">A MemberExpression that returns an EventCallback</param>
        /// <param name="callback">An EventCallback that is called when the child component's event is triggered</param>
        /// <returns></returns>
        public IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                                   EventCallback<TCallbackType> callback)
        {
            componentParameterSupport.AddEventCallback(expr, callback);
            return this;
        }

        /// <summary>
        /// Binds an Action to a wrapped component's EventCallback member
        /// </summary>
        /// <typeparam name="TCallbackType"></typeparam>
        /// <param name="expr">A MemberExpression that returns an EventCallback</param>
        /// <param name="component">A reference to the component that is handling the event</param>
        /// <param name="action">An Action that is called when the child component's event is triggered</param>
        /// <returns></returns>
        public IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                                   IComponent component,
                                                                                   Action<TCallbackType> action)
        {
            return AddEventCallback<TCallbackType>(expr, EventCallback.Factory.Create<TCallbackType>(component, action));
        }

        public IModalDialogInteraction<TComponent> AddEventCallback(Expression<Func<TComponent, EventCallback>> expr,
                                                                    IComponent component,
                                                                    Action action)
        {
            componentParameterSupport.AddEventCallback(expr, EventCallback.Factory.Create(component, action));
            return this;
        }
    }
}
