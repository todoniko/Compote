using Compote;
using FsCheck;
using FsCheck.Xunit;
using Gen = Compote.Test.Generators;
namespace MaybeTest;


public class FunctorLaws
{
    protected T Identity<T>(T x) => x;

    [Property(Arbitrary = new[] { typeof(Gen.Maybe<string>) })]
    public Property SelectIdentity(Maybe<string> x)
        => x.Equals(x.Select(Identity)).ToProperty();

    [Property(Arbitrary = new[] { typeof(Gen.Maybe<int>) })]
    public Property SelectComposition(Maybe<int> x, Func<int, string> f, Func<string, byte> g)
        => x.Select(c => (g(f(c)))).Equals(x.Select(f).Select(g)).ToProperty();
}

public class MonadLaws
{
    private Maybe<T> Wrap<T>(T x) => x;

    private Maybe<int> F(string x)
        => String.IsNullOrWhiteSpace(x)
            ? Maybe<int>.None
            : x.Max(c => (int)c);

    private Maybe<byte> G(int number)
        => number % 10 == 0
            ? Maybe<byte>.None
            : (byte)(number % 10);

    private Maybe<DateOnly> H(byte d)
        => d <= 7
            ? DateOnly.FromDayNumber(d)
            : Maybe<DateOnly>.None;

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