using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazored.Modal.Services
{
    public interface IModalDialogInteraction<TComponent> : IModalDialogInteraction<TComponent, Object> where TComponent : ComponentBase
    {

    }

    public interface IModalDialogInteraction<TComponent, TResult> where TComponent : ComponentBase
    {
        event Action<ModalResult<TResult>> OnClose;
        void Close(ModalResult<TResult> modalResult);
        IModalDialogInteraction<TComponent, TResult> Show();
        IModalDialogInteraction<TComponent, TResult> AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> memberExpr, TProperty value);
        IModalDialogInteraction<TComponent, TResult> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr, EventCallback<TCallbackType> callback);
        IModalDialogInteraction<TComponent, TResult> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr, IComponent component, Action<TCallbackType> action);

    }
}
