using System;
using System.Linq.Expressions;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Blazored.Modal.Services
{
    public class ModalDialogInteraction<TComponent> : IModalDialogInteraction<TComponent>
        where TComponent : ComponentBase
    {
        private readonly ModalService modalService;
        private readonly string title;
        private readonly ComponentParameterSupport<TComponent> componentParameterSupport = new ComponentParameterSupport<TComponent>();

        public ModalDialogInteraction(ModalService modalService, string title)
        {
            this.modalService = modalService;
            this.title = title;
        }

        public event Action<ModalResult> OnClose;

        public IModalDialogInteraction<TComponent> Show()
        {
            var content = new RenderFragment(renderTreeBuilder =>
            {
                renderTreeBuilder.OpenComponent(1, typeof(TComponent));
                componentParameterSupport.Execute(renderTreeBuilder, 2);
                renderTreeBuilder.CloseComponent();
            });

            modalService.TriggerShow(title, content, new ModalParameters(), new ModalOptions());
            return this;
        }

        public void Close(ModalResult modalResult)
        {
            modalService.Close(modalResult);
            OnClose?.Invoke(modalResult);
        }

        public IModalDialogInteraction<TComponent> AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> expr, TProperty value)
        {
            componentParameterSupport.AddParameter(expr, value);
            return this;
        }

        public IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                                   EventCallback<TCallbackType> callback)
        {
            componentParameterSupport.AddEventCallback(expr, callback);
            return this;
        }

        public IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                                   IComponent component,
                                                                                   Action<TCallbackType> action)
        {
            return AddEventCallback<TCallbackType>(expr, EventCallback.Factory.Create<TCallbackType>(component, action));
        }
    }
}
