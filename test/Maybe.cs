using Compote;
using FsCheck;
using FsCheck.Xunit;
namespace MaybeTest;
public static class MaybeGenerator<T>
{
    private static Gen<Maybe<T>> GenMaybe =
        Gen.OneOf<Maybe<T>>(
            Arb.Generate<T>().Select(Maybe.Just),
            Gen.Constant(Maybe.None<T>()));

    public static Arbitrary<Maybe<T>> Maybes() => Arb.From(GenMaybe);
}

public class FunctorLaws
{
    protected T Identity<T>(T x) => x;

    [Property(Arbitrary = new[] { typeof(MaybeGenerator<string>) })]
    public Property SelectIdentity(Maybe<string> x)
        => x.Equals(x.Select(Identity)).ToProperty();

    [Property(Arbitrary = new[] { typeof(MaybeGenerator<int>) })]
    public Property SelectComposition(Maybe<int> x, Func<int, string> f, Func<string, byte> g)
        => x.Select(c => (g(f(c)))).Equals(x.Select(f).Select(g)).ToProperty();
}

public class MonadLaws
{
    private Maybe<T> Wrap<T>(T x) => Maybe.Just(x);
    private Maybe<int> F(string x)
        => String.IsNullOrWhiteSpace(x)
            ? Maybe.None<int>()
            : Maybe.Just(x.Max(c => (int)c));

    private Maybe<byte> G(int number)
        => number % 10 == 0
            ? Maybe.None<byte>()
            : Maybe.Just((byte)(number % 10));

    private Maybe<DateOnly> H(byte d)
        => d <= 7
            ? Maybe.Just(DateOnly.FromDayNumber(d))
            : Maybe.None<DateOnly>();

    [Property]
    public Property LeftIdentity(string x)
        => Wrap(x).SelectMany(F)
            .Equals(F(x))
            .ToProperty();

    [Property]
    public Property RightIdentity(string x)
        => F(x).SelectMany(Wrap)
            .Equals(F(x))
            .ToProperty();

    [Property]
    public Property Associativity(string x)
        => F(x).SelectMany(G).SelectMany(H)
            .Equals(F(x).SelectMany(s => G(s).SelectMany(H)))
            .ToProperty();
}