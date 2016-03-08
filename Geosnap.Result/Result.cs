using System;
using System.Threading.Tasks;

namespace Geosnap.Result
{
    /// <summary>
    /// Modeled after https://github.com/kittinunf/Result
    /// </summary>
    public abstract class Result<TValue>
    {
        protected TValue V { get; }
        protected Exception E { get; }

        protected Result(TValue value)
        {
            V = value;
            E = default(Exception);
        }

        protected Result(Exception error)
        {
            V = default(TValue);
            E = error;
        }

        protected abstract TValue Component1();

        protected abstract Exception Component2();

        public abstract void Fold(Action<TValue> successAction, Action<Exception> failureAction);

        public abstract TReturn Fold<TReturn>(Func<TValue, TReturn> successFun, Func<Exception, TReturn> failureFun);

        public abstract TValue Get();

        public abstract TReturn GetAs<TReturn>();

        public abstract void OnSuccess(Action<TValue> action);

        public abstract Task OnSuccessAsync(Func<TValue, Task> action);

        public abstract void OnFailure(Action<Exception> action);

        public abstract Task OnFailureAsync(Func<Exception, Task> action);

        public abstract Result<TValue> Or(TValue fallback);

        public abstract TValue GetOrElse(TValue fallback);

        public abstract Result<TReturn> Map<TReturn>(Func<TValue, TReturn> fun);

        public abstract Task<Result<TReturn>> MapAsync<TReturn>(Func<TValue, Task<TReturn>> fun);

        public abstract Result<TReturn> FlatMap<TReturn>(Func<TValue, Result<TReturn>> fun);

        public abstract Task<Result<TReturn>> FlatMapAsync<TReturn>(Func<TValue, Task<Result<TReturn>>> fun);

        public abstract Result<TValue> MapError(Func<Exception, Exception> fun);

        public abstract Task<Result<TValue>> MapErrorAsync(Func<Exception, Task<Exception>> fun);

        public abstract Result<TValue> FlatMapError(Func<Exception, Result<TValue>> fun);

        public abstract Task<Result<TValue>> FlatMapErrorAsync(Func<Exception, Task<Result<TValue>>> fun);

        public abstract bool Any(Func<TValue, bool> fun);

        public static Result<TValue> Error(Exception error) => new Failure(error);

        public static Result<TValue> Of(TValue v) => Of(v, () => new Exception());

        public static Result<TValue> Of(TValue v, Func<Exception> fail) => v != null ? new Success(v) : Error(fail());

        public static Result<TValue> Of(Func<TValue> fun)
        {
            try
            {
                var value = fun();
                return new Success(value);
            }
            catch (Exception e)
            {
                return new Failure(e);
            }
        }

        public static async Task<Result<TValue>> OfAsync(Func<Task<TValue>> fun)
        {
            try
            {
                var value = await fun();
                return new Success(value);
            }
            catch (Exception e)
            {
                return new Failure(e);
            }
        }
        
        public static explicit operator Tuple<TValue, Exception>(Result<TValue> result) => new Tuple<TValue, Exception>(result.Component1(), result.Component2());

        public class Success : Result<TValue>
        {
            internal Success(TValue value) : base(value) { }

            protected override TValue Component1() => V;

            protected override Exception Component2() => default(Exception);

            public override void Fold(Action<TValue> successAction, Action<Exception> failureAction) => successAction(V);

            public override TReturn Fold<TReturn>(Func<TValue, TReturn> successFun, Func<Exception, TReturn> failureFun) => successFun(V);

            public override TValue Get() => V;

            public override string ToString() => $"Success: {V}";

            public override int GetHashCode() => V.GetHashCode();

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                var s = obj as Success;
                return s != null && s.V.Equals(V);
            }

            public override TReturn GetAs<TReturn>() => (TReturn)(object)V;

            public override void OnSuccess(Action<TValue> action) => action(V);

            public override Task OnSuccessAsync(Func<TValue, Task> action) => action(V);

            public override void OnFailure(Action<Exception> action) { }

            public override Task OnFailureAsync(Func<Exception, Task> action) => Task.FromResult<object>(null);

            public override Result<TValue> Or(TValue fallback) => new Success(V);

            public override TValue GetOrElse(TValue fallback) => V;

            public override Result<TReturn> Map<TReturn>(Func<TValue, TReturn> fun) => new Result<TReturn>.Success(fun(V));

            public override async Task<Result<TReturn>> MapAsync<TReturn>(Func<TValue, Task<TReturn>> fun) => new Result<TReturn>.Success(await fun(V));

            public override Result<TReturn> FlatMap<TReturn>(Func<TValue, Result<TReturn>> fun) => fun(V);

            public override Task<Result<TReturn>> FlatMapAsync<TReturn>(Func<TValue, Task<Result<TReturn>>> fun) => fun(V);

            public override Result<TValue> MapError(Func<Exception, Exception> fun) => new Success(V);

            public override Task<Result<TValue>> MapErrorAsync(Func<Exception, Task<Exception>> fun) => Task.FromResult<Result<TValue>>(new Success(V));

            public override Result<TValue> FlatMapError(Func<Exception, Result<TValue>> fun) => new Success(V);

            public override Task<Result<TValue>> FlatMapErrorAsync(Func<Exception, Task<Result<TValue>>> fun) => Task.FromResult<Result<TValue>>(new Success(V));

            public override bool Any(Func<TValue, bool> fun) => fun(V);
        }

        public class Failure : Result<TValue>
        {
            internal Failure(Exception error) : base(error) { }

            protected override TValue Component1() => default(TValue);

            protected override Exception Component2() => E;

            public override void Fold(Action<TValue> successAction, Action<Exception> failureAction) => failureAction(E);

            public override TReturn Fold<TReturn>(Func<TValue, TReturn> successFun, Func<Exception, TReturn> failureFun) => failureFun(E);

            public override TValue Get() { throw E; }

            public override string ToString() => $"Failure: {E}";

            public override int GetHashCode() => V.GetHashCode();

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                var f = obj as Failure;
                return f != null && f.E.Equals(E);
            }

            public override TReturn GetAs<TReturn>() => (TReturn)(object)E;

            public override void OnSuccess(Action<TValue> action) { }

            public override Task OnSuccessAsync(Func<TValue, Task> action) => Task.FromResult<object>(null);

            public override void OnFailure(Action<Exception> action) => action(E);

            public override Task OnFailureAsync(Func<Exception, Task> action) => action(E);

            public override Result<TValue> Or(TValue fallback) => new Success(fallback);

            public override TValue GetOrElse(TValue fallback) => fallback;

            public override Result<TReturn> Map<TReturn>(Func<TValue, TReturn> fun) => new Result<TReturn>.Failure(E);

            public override Task<Result<TReturn>> MapAsync<TReturn>(Func<TValue, Task<TReturn>> fun) => Task.FromResult<Result<TReturn>>(new Result<TReturn>.Failure(E));

            public override Result<TReturn> FlatMap<TReturn>(Func<TValue, Result<TReturn>> fun) => new Result<TReturn>.Failure(E);

            public override Task<Result<TReturn>> FlatMapAsync<TReturn>(Func<TValue, Task<Result<TReturn>>> fun) => Task.FromResult<Result<TReturn>>(new Result<TReturn>.Failure(E));

            public override Result<TValue> MapError(Func<Exception, Exception> fun) => new Failure(fun(E));

            public override async Task<Result<TValue>> MapErrorAsync(Func<Exception, Task<Exception>> fun) => new Failure(await fun(E));

            public override Result<TValue> FlatMapError(Func<Exception, Result<TValue>> fun) => fun(E);

            public override Task<Result<TValue>> FlatMapErrorAsync(Func<Exception, Task<Result<TValue>>> fun) => fun(E);

            public override bool Any(Func<TValue, bool> fun) => false;
        }
    }
}
