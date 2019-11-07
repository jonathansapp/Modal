using System;

namespace Blazored.Modal.Services
{
    public class ModalResult
    {
        public object Data { get; }
        public Type DataType { get; }
        public bool Cancelled { get; }

        protected ModalResult(object data, Type resultType, bool cancelled)
        {
            Data = data;
            DataType = resultType;
            Cancelled = cancelled;
        }

        public static ModalResult<TResult> Ok<TResult>(TResult result) => new ModalResult<TResult>(result, false);

        public static ModalResult<object> Cancel() => new ModalResult<object>(default, true);
    }

    public class ModalResult<TResult> : ModalResult
    {
        private readonly TResult data;

        public new TResult Data
        {
            get => data;
        }

        protected internal ModalResult(TResult data, bool cancelled) : base(data, typeof(TResult), cancelled)
        {
            this.data = data;
        }
    }
}
