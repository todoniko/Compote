namespace Compote;
public interface Maybe<T> : IEquatable<Maybe<T>>
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
    public R Match<R>(Func<T, R> onJust, Func<R> onNone);

    /// <summary>
    /// Apply <paramref name="f"/> on the value in Maybe of <typeparamref name="T"/> and wrap the result in Maybe of <typeparamref name="R"/>.
    /// </summary>
    /// <param name="f">A function to apply on the value <typeparamref name="T"/> in the container</param>
    /// <typeparam name="R">The return type of f and value type in the returned Maybe</typeparam>
    /// <returns>The result of applying <paramref name="f"/> to the contained value, wrapped in Maybe or None of <typeparamref name="R"/>.</returns>
    public Maybe<R> Select<R>(Func<T, R> f);

    /// <summary>
    /// Apply <paramref name="f"/> on the value in Maybe of <typeparamref name="T"/>  and flattens the resulting value into Maybe of <typeparamref name="R"/>.
    /// </summary>
    /// <param name="f">A function to apply on the value <typeparamref name="T"/> in the container</param>
    /// <typeparam name="R">The value type in the resulting Maybe</typeparam>
    /// <returns>The result of <paramref name="f"/> or None of <typeparamref name="R"/> </returns>
    public Maybe<R> SelectMany<R>(Func<T, Maybe<R>> f);
}

/// <summary>
/// Empty container variant of <see cref="Maybe{T}"/>
/// </summary>
/// <typeparam name="T">The type of the absent value</typeparam>
internal readonly struct None<T> : Maybe<T>
{
    public R Match<R>(Func<T, R> _, Func<R> onNone) => onNone();
    public Maybe<R> Select<R>(Func<T, R> _) => new None<R>();
    public Maybe<R> SelectMany<R>(Func<T, Maybe<R>> _) => new None<R>();

    /// <inheritdoc/>
    public bool Equals(Maybe<T>? maybe)
        => this.Match<bool>(_ => false, () => true);
}

/// <summary>
/// Container variant of <see cref="Maybe{T}"/> with value
/// </summary>
/// <typeparam name="T">The type of the contained value</typeparam>
internal readonly struct Just<T> : Maybe<T>
{
    private T Value { get; init; }
    internal Just(T value) { Value = value; }

    public R Match<R>(Func<T, R> onJust, Func<R> _) => onJust(Value);
    public Maybe<R> Select<R>(Func<T, R> f) => new Just<R>(f(Value));
    public Maybe<R> SelectMany<R>(Func<T, Maybe<R>> f) => f(Value);

    /// <inheritdoc/>
    public bool Equals(Maybe<T>? maybe)
    {
        T val = Value;
        return this.Match<bool>(v => val.Equals(v), () => false);
    }
}

/// <summary>
/// Constructors and extension methods for working with <see cref="Maybe{T}"/>
/// </summary>
public static class Maybe
{
    /// <summary>
    /// Creates a <see cref="Maybe{T}"/> containing a value
    /// </summary>
    /// <param name="value">The value within <see cref="Maybe{T}"/></param>
    /// <typeparam name="T">The type of the contained value</typeparam>
    /// <returns>A <see cref="Maybe{T}"/> container with the specified value</returns>
    public static Maybe<T> Just<T>(T value)
    {
        if (value is null) return new None<T>();
        return new Just<T>(value);
    }

    /// <summary>
    /// Creates a <see cref="Maybe{T}"/> without value  
    /// </summary>
    /// <typeparam name="T">The type if there was value</typeparam>
    /// <returns>A <see cref="Maybe{T}"/> container without value</returns>
    public static Maybe<T> None<T>() => new None<T>();

    public static Maybe<R> SelectMany<T, U, R>
    (this Maybe<T> maybe, Func<T, Maybe<U>> f, Func<T, U, R> trans)
        => maybe.SelectMany<R>(x => f(x).Select(y => trans(x, y)));
}