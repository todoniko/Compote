using FsCheck;

namespace Compote.Test.Generators;
public static class Maybe<T>
{
    private static Gen<Compote.Maybe<T>> GenMaybe =
        Gen.OneOf<Compote.Maybe<T>>(
            Arb.Generate<T>().Select(Compote.Maybe<T>.Just),
            Gen.Constant(Compote.Maybe<T>.None));

    public static Arbitrary<Compote.Maybe<T>> Maybes() => Arb.From(GenMaybe);
}