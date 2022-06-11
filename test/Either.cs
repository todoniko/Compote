using Compote;
using FsCheck;
using FsCheck.Xunit;
using Gen = Compote.Test.Generators;
namespace EitherTest;

public class FunctorLaws
{
    [Property(Arbitrary = new[] { typeof(Gen.Either<int, string>) })]
    public Property Identity(Either<int, string> x)
        => x.Equals(x.Select(x => x))
            .ToProperty();

    [Property(Arbitrary = new[] { typeof(Gen.Either<string, int>) })]
    public Property Composition(
        Either<string, int> x,
        Func<int, string> f,
        Func<string, byte> g)
        => x.Select(c => (g(f(c))))
            .Equals(x.Select(f).Select(g))
            .ToProperty();
}

public class MonadLaws
{
    private Either<string, int> F(string input)
        => !String.IsNullOrEmpty(input) && input.Length > 2
            ? input.Length
            : "drama";

    private Either<string, byte> G(int number)
        => number % 10 == 0 ? $"10 divides {number}" : (byte)(number % 10);

    private Either<string, DateOnly> H(byte d)
        => d <= 7 ? DateOnly.FromDayNumber(d) : $"no such day {d}";

    [Property]
    public Property LeftIdentity(string x)
        => Either<string, string>
            .Right(x).SelectMany(F)
            .Equals(F(x))
            .ToProperty();

    [Property]
    public Property RightIdentity(int x)
        => G(x).SelectMany(Either<string, byte>.Right)
            .Equals(G(x))
            .ToProperty();

    [Property]
    public Property Associativity(string x)
        => F(x).SelectMany(G).SelectMany(H)
               .Equals(
                   F(x).SelectMany(a => G(a).SelectMany(H)))
               .ToProperty();
}