using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazored.Modal.Services
{
    /// <summary>
    /// An instance of a modal dialog
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IModalDialogInteraction<TComponent> where TComponent : IComponent
    {
        /// <summary>
        /// Executed when the modial dialog has closed
        /// </summary>
        event Action<ModalResult> OnClose;

        /// <summary>
        /// Closes the modal dialog
        /// </summary>
        /// <param name="modalResult"></param>
        void Close(ModalResult modalResult);

        /// <summary>
        /// Shows the modal dialog
        /// </summary>
        /// <returns>itself, for chained method calls</returns>
        IModalDialogInteraction<TComponent> Show();

        /// <summary>
        /// Adds a parameter value to the wrapped component
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="memberExpr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IModalDialogInteraction<TComponent> AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> memberExpr, TProperty value);

        /// <summary>
        /// Adds a callback to the wrapped component
        /// </summary>
        /// <typeparam name="TCallbackType"></typeparam>
        /// <param name="expr"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                            EventCallback<TCallbackType> callback);

        /// <summary>
        /// Adds a callback to the wrapped component
        /// </summary>
        /// <typeparam name="TCallbackType"></typeparam>
        /// <param name="expr"></param>
        /// <param name="component"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IModalDialogInteraction<TComponent> AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                                            IComponent component,
                                                                            Action<TCallbackType> action);

        /// <summary>
        /// Adds a callback to the wrapped component
        /// </summary>
        /// <typeparam name="TCallbackType"></typeparam>
        /// <param name="expr"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        IModalDialogInteraction<TComponent> AddEventCallback(Expression<Func<TComponent, EventCallback>> expr,
                                                             IComponent component,
                                                             Action action);
    }
}
