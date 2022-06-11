namespace Compote;
/// <summary>
/// Represents value container in which only one of the values can be present. Either Left or Right.
/// </summary>
/// <typeparam name="L">The Left case value type</typeparam>
/// <typeparam name="R">The Right case value type</typeparam>
/// <remarks>
/// In practice Right represents the happy path and Left represents the error case.
/// </remarks>
public abstract class Either<L, R> : IEquatable<Either<L, R>>
{
    /// <summary>
    /// Constructs the Right variant of <see cref="Either{L,R}"/><br/>
    /// </summary>
    /// <returns>New <see cref="Either{L,R}"/> containing <typeparamref name="R"/> value</returns>
    public static Either<L, R> Right(R value) => new Right<L, R>(value);

    /// <summary>
    /// Constructs the Left variant of <see cref="Either{L,R}"/><br/>
    /// </summary>
    /// <returns>New <see cref="Either{L,R}"/> containing <typeparamref name="L"/> value</returns>
    public static Either<L, R> Left(L value) => new Left<L, R>(value);

    /// <summary>
    /// Applies <paramref name="onLeft"/> or <paramref name="onRight"/> depending on the variant of <see cref="Either{L,R}"/><br/>
    /// </summary>
    /// <typeparam name="T">The return value of value extracting functions <paramref name="onLeft"/> and <paramref name="onRight"/></typeparam>
    /// <param name="onLeft">Function to apply on the contained value <typeparamref name="L"/> when in Left variant</param>
    /// <param name="onRight">Function to apply on the contained value <typeparamref name="R"/> when in Right variant</param>
    /// <returns>The result of applying the <paramref name="onLeft"/> or <paramref name="onRight"/> function</returns>
    public abstract T Match<T>(Func<L, T> onLeft, Func<R, T> onRight);

    /// <summary>
    /// Applies <paramref name="onRight"/> on the value in Right case.
    /// </summary>
    /// <typeparam name="TR">The return value of value extracting function <paramref name="onRight"/></typeparam>
    /// <param name="onRight">Function to apply on the contained value <typeparamref name="R"/> when in Right variant</param>
    /// <returns>The resulting <typeparamref name="TR"/> of applying <paramref name="onRight"/> encapsulated in new <see cref="Either{L,TR}"/></returns>
    public abstract Either<L, TR> Select<TR>(Func<R, TR> onRight);

    /// <summary>
    /// Applies <paramref name="onRight"/> on the value in Right case.
    /// </summary>
    /// <typeparam name="TR">The Right case of <see cref="Either{L,TR}"/> returning function <paramref name="onRight"/></typeparam>
    /// <param name="onRight">Function to apply on the contained value <typeparamref name="R"/> when in Right case</param>
    /// <returns>New <see cref="Either{L,TR}"/></returns>
    public abstract Either<L, TR> SelectMany<TR>(Func<R, Either<L, TR>> onRight);

    /// <inheritdoc/>
    public Either<L, TR> SelectMany<TR, U>(Func<R, Either<L, U>> f, Func<R, U, TR> trans)
        => this.SelectMany(x => f(x).Select(y => trans(x, y)));

    public abstract bool Equals(Either<L, R>? either);

    public static implicit operator Either<L, R>(R value) =>
        new Right<L, R>(value);

    public static implicit operator Either<L, R>(L value) =>
        new Left<L, R>(value);
}

internal sealed class Left<L, R> : Either<L, R>
{
    private readonly L value;
    internal Left(L value) => this.value = value;

    public override T Match<T>(Func<L, T> onLeft, Func<R, T> _)
        => onLeft(value);

    public override Either<L, TR> Select<TR>(Func<R, TR> _)
        => new Left<L, TR>(value);

    public override Either<L, TR> SelectMany<TR>(Func<R, Either<L, TR>> _)
        => new Left<L, TR>(value);

    public override bool Equals(Either<L, R>? either)
    {
        L val = value;
        return either is not null
            && either.Match<bool>(
                v => (val is null && v is null)
                  || (v is not null && v.Equals(val)),
                _ => false);
    }
}

internal sealed class Right<L, R> : Either<L, R>
{
    private readonly R value;
    internal Right(R value) => this.value = value;

    public override T Match<T>(Func<L, T> _, Func<R, T> onRight)
        => onRight(value);

    public override Either<L, TR> Select<TR>(Func<R, TR> onRight)
        => new Right<L, TR>(onRight(value));

    public override Either<L, TR> SelectMany<TR>(Func<R, Either<L, TR>> onRight)
        => onRight(value);

    public override bool Equals(Either<L, R>? either)
    {
        R val = value;
        return either is not null
            && either.Match<bool>(
                _ => false,
                v => (val is null && v is null)
                  || (v is not null && v.Equals(val)));
    }
}