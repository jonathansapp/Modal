using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazored.Modal.Services
{
    /// <summary>
    /// Utility class that handles the setting of sub-component parameters and event callbacks
    /// within a ModalDialogInteraction.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    internal class ComponentParameterSupport<TComponent> where TComponent : IComponent
    {
        private readonly List<Action<int, RenderTreeBuilder>> componentActions = new List<Action<int, RenderTreeBuilder>>();

        public void AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> expr, TProperty value)
        {
            var member = GetMemberInfo(expr);
            componentActions.Add((sequenceNumber, tree) => tree.AddAttribute(sequenceNumber, member.Name, value));
        }


        public void AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                    EventCallback<TCallbackType> callback)
        {
            var member = GetMemberInfo(expr);
            componentActions.Add((sequenceNumber, tree) => tree.AddAttribute(sequenceNumber, member.Name, callback));
        }

        public void AddEventCallback(Expression<Func<TComponent, EventCallback>> expr,
                                     EventCallback callback)
        {
            var member = GetMemberInfo(expr);
            componentActions.Add((sequenceNumber, tree) => tree.AddAttribute(sequenceNumber, member.Name, callback));
        }



        public void Execute(RenderTreeBuilder renderTreeBuilder, int startingSequenceNumber)
        {
            foreach ((var action, var i) in componentActions.Select((a, i) => (a, startingSequenceNumber + i)))
            {
                action(i, renderTreeBuilder);
            }
        }


        private MemberInfo GetMemberInfo<TProperty>(Expression<Func<TComponent, TProperty>> expr)
        {
            var memberExpr = expr.Body as MemberExpression;
            if (memberExpr == null) throw new InvalidOperationException("Not a member expression");

            var member = memberExpr.Member;
            if (!member.GetCustomAttributes(typeof(ParameterAttribute), true).Any())
                throw new InvalidOperationException($"Member '{member.Name}' is not a parameter");
            return member;
        }
    }
}
