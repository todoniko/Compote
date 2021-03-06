namespace Compote;
public abstract class Maybe<T> : IEquatable<Maybe<T>>
{
    /// <summary>
    /// Applies function for both possible cases of <see cref="Maybe{T}"/>
    /// When <see cref="Just{T}"/> <paramref name="onJust"/> will be called.
    /// When <see cref="None{T}"/> <paramref name="onNone"/> will be called.
    /// </summary>
    /// <typeparam name="R">The return type</typeparam>
    /// <param name="onJust">Function passing the value of the container</param>
    /// <param name="onNone">Thunk when container is empty</param>
    /// <returns>The result of calling the <paramref name="onJust"/> or <paramref name="onNone"/> function</returns>
    public abstract R Match<R>(Func<T, R> onJust, Func<R> onNone);

    /// <summary>
    /// Apply <paramref name="f"/> on the value in Maybe of <typeparamref name="T"/> and wrap the result in Maybe of <typeparamref name="R"/>.
    /// </summary>
    /// <param name="f">A function to apply on the value <typeparamref name="T"/> in the container</param>
    /// <typeparam name="R">The return type of f and value type in the returned Maybe</typeparam>
    /// <returns>The result of applying <paramref name="f"/> to the contained value, wrapped in Maybe or None of <typeparamref name="R"/>.</returns>
    public abstract Maybe<R> Select<R>(Func<T, R> f);

    /// <summary>
    /// Apply <paramref name="f"/> on the value in Maybe of <typeparamref name="T"/>  and flattens the resulting value into Maybe of <typeparamref name="R"/>.
    /// </summary>
    /// <param name="f">A function to apply on the value <typeparamref name="T"/> in the container</param>
    /// <typeparam name="R">The value type in the resulting Maybe</typeparam>
    /// <returns>The result of <paramref name="f"/> or None of <typeparamref name="R"/> </returns>
    public abstract Maybe<R> SelectMany<R>(Func<T, Maybe<R>> f);

    public Maybe<R> SelectMany<R, U>(Func<T, Maybe<U>> f, Func<T, U, R> trans)
        => this.SelectMany<R>(x => f(x).Select(y => trans(x, y)));

    /// <inheritdoc/>
    public abstract bool Equals(Maybe<T>? maybe);

    public static implicit operator Maybe<T>(T value)
        => Maybe<T>.Just(value);

    public static Maybe<T> None = new None<T>();
    public static Maybe<T> Just(T value)
        => value is not null ? new Just<T>(value) : new None<T>();
}

/// <summary>
/// Empty container variant of <see cref="Maybe{T}"/>
/// </summary>
/// <typeparam name="T">The type of the absent value</typeparam>
internal sealed class None<T> : Maybe<T>
{
    public override R Match<R>(Func<T, R> _, Func<R> onNone) => onNone();
    public override Maybe<R> Select<R>(Func<T, R> _) => new None<R>();
    public override Maybe<R> SelectMany<R>(Func<T, Maybe<R>> _) => new None<R>();

    public override bool Equals(Maybe<T>? maybe)
        => maybe is not null
        && maybe.Match<bool>(_ => false, () => true);
}

/// <summary>
/// Container variant of <see cref="Maybe{T}"/> with value
/// </summary>
/// <typeparam name="T">The type of the contained value</typeparam>
internal sealed class Just<T> : Maybe<T>
{
    private readonly T value;
    internal Just(T value) { this.value = value; }

    public override R Match<R>(Func<T, R> onJust, Func<R> _) => onJust(value);
    public override Maybe<R> Select<R>(Func<T, R> f) => new Just<R>(f(value));
    public override Maybe<R> SelectMany<R>(Func<T, Maybe<R>> f) => f(value);

    public override bool Equals(Maybe<T>? maybe)
    {
        T val = value;
        return maybe is not null
            && maybe.Match<bool>(
                v => (val is null && v is null)
                  || (val is not null && val.Equals(v)),
                () => false);
    }
}