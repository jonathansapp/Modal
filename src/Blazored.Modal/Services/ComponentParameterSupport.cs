using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazored.Modal.Services
{
    internal class ComponentParameterSupport<TComponent> where TComponent : ComponentBase
    {
        private readonly List<Action<int, RenderTreeBuilder>> componentActions = new List<Action<int, RenderTreeBuilder>>();

        public void AddParameter<TProperty>(Expression<Func<TComponent, TProperty>> expr, TProperty value)
        {
            var memberExpr = expr.Body as MemberExpression;
            if (memberExpr == null) throw new InvalidOperationException("Not a member expression");

            var member = memberExpr.Member as PropertyInfo;
            if (!member.GetCustomAttributes(typeof(ParameterAttribute), true).Any())
                throw new InvalidOperationException($"Member '{member.Name}' is not a parameter");

            componentActions.Add((i, tree) => tree.AddAttribute(i, member.Name, value));
        }

        public void AddEventCallback<TCallbackType>(Expression<Func<TComponent, EventCallback<TCallbackType>>> expr,
                                                    EventCallback<TCallbackType> callback)
        {
            var memberExpr = expr.Body as MemberExpression;
            if (memberExpr == null) throw new InvalidOperationException("Not a member expression");

            var member = memberExpr.Member as PropertyInfo;
            if (!member.GetCustomAttributes(typeof(ParameterAttribute), true).Any())
                throw new InvalidOperationException($"Member '{member.Name}' is not a parameter");

            if (member.PropertyType != typeof(EventCallback<TCallbackType>))
                throw new InvalidOperationException("Property was not an EventCallback of the property generic type");

            componentActions.Add((i, tree) => tree.AddAttribute(i, member.Name, callback));
        }

        public void Execute(RenderTreeBuilder renderTreeBuilder, int startingSequenceNumber)
        {
            foreach ((var action, var i) in componentActions.Select((a, i) => (a, startingSequenceNumber + i)))
            {
                action(i, renderTreeBuilder);
            }
        }
    }
}
