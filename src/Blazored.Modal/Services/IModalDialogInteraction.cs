using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazored.Modal.Services
{
    public interface IModalDialogInteraction<TComponent> where TComponent : ComponentBase
    {
        event Action<ModalResult> OnClose;
        void Close(ModalResult modalResult);
        IModalDialogInteraction<TComponent> Show();
        IModalDialogInteraction<TComponent> AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> memberExpr, TProperty value);
        IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                            EventCallback<TCallbackType> callback);
        IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                            IComponent component,
                                                                            Action<TCallbackType> action);
    }
}
