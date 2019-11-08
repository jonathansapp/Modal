﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Blazored.Modal.Services
{
    public class ModalService : IModalService
    {
        /// <summary>
        /// Invoked when the modal component closes.
        /// </summary>
        public event Action<ModalResult> OnClose;

        /// <summary>
        /// Internal event used to trigger the modal component to show.
        /// </summary>
        internal event Action<string, RenderFragment, ModalParameters, ModalOptions> OnShow;

        /// <summary>
        /// Shows the modal using the specified title and component type.
        /// </summary>
        /// <param name="title">Modal title.</param>
        /// <param name="componentType">Type of component to display.</param>
        public void Show(string title, Type componentType)
        {
            Show(title, componentType, new ModalParameters(), new ModalOptions());
        }

        /// <summary>
        /// Shows the modal using the specified title and component type.
        /// </summary>
        /// <param name="title">Modal title.</param>
        /// <param name="componentType">Type of component to display.</param>
        /// <param name="options">Options to configure the modal.</param>
        public void Show(string title, Type componentType, ModalOptions options)
        {
            Show(title, componentType, new ModalParameters(), options);
        }

        /// <summary>
        /// Shows the modal using the specified <paramref name="title"/> and <paramref name="componentType"/>, 
        /// passing the specified <paramref name="parameters"/>. 
        /// </summary>
        /// <param name="title">Modal title.</param>
        /// <param name="componentType">Type of component to display.</param>
        /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
        public void Show(string title, Type componentType, ModalParameters parameters)
        {
            Show(title, componentType, parameters, new ModalOptions());
        }

        /// <summary>
        /// Shows the modal using the specified <paramref name="title"/> and <paramref name="componentType"/>, 
        /// passing the specified <paramref name="parameters"/> and setting a custom CSS style. 
        /// </summary>
        /// <param name="title">Modal title.</param>
        /// <param name="componentType">Type of component to display.</param>
        /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
        /// <param name="options">Options to configure the modal.</param>
        public void Show(string title, Type componentType, ModalParameters parameters, ModalOptions options)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType))
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            var content = new RenderFragment(x => { x.OpenComponent(1, componentType); x.CloseComponent(); });

            TriggerShowEvent(title, content, parameters, options);
        }

        /// <summary>
        /// Closes the modal and invokes the <see cref="OnClose"/> event.
        /// </summary>
        public void Cancel()
        {
            OnClose?.Invoke(ModalResult.Cancel());
        }

        /// <summary>
        /// Closes the modal and invokes the <see cref="OnClose"/> event with the specified <paramref name="modalResult"/>.
        /// </summary>
        /// <param name="modalResult"></param>
        public void Close(ModalResult modalResult)
        {
            OnClose?.Invoke(modalResult);
        }

        /// <inheritdoc cref="IModalService.Show{T}(string, ModalParameters, ModalOptions)"/>
        public void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null) where T : ComponentBase
        {
            Show(title,
                 typeof(T),
                 parameters ?? new ModalParameters(),
                 options ?? new ModalOptions());
        }

        internal void TriggerShowEvent(string title, RenderFragment content, ModalParameters parameters, ModalOptions options)
        {
            OnShow?.Invoke(title, content, parameters, options);
        }

        /// <summary>
        /// Creates a new modal dialog interaction
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="title"></param>
        /// <returns></returns>
        public IModalDialogInteraction<TComponent> Create<TComponent>(string title) where TComponent : IComponent
        {
            return Create<TComponent>(title, new ModalOptions());
        }

        public IModalDialogInteraction<TComponent> Create<TComponent>(string title, ModalOptions options) where TComponent : IComponent
        {
            return new ModalDialogInteraction<TComponent>(this, title, options);
        }
    }
}
