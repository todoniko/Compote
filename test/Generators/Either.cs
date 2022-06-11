using FsCheck;
using FsCheck.Xunit;

namespace Compote.Test.Generators;
public static class Either<L, R>
{
    private static Gen<Compote.Either<L, R>> GenEither =
        Gen.OneOf<Compote.Either<L, R>>(
            Arb.Generate<L>().Select(Compote.Either<L, R>.Left),
            Arb.Generate<R>().Select(Compote.Either<L, R>.Right));

    public static Arbitrary<Compote.Either<L, R>> Choices => Arb.From(GenEither);
}