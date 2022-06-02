namespace Compote;
/// <summary>
/// Extension methods for Func types <br/>
/// <b>Currying</b> - <see cref="Unary"/>.<br/>
/// <b>Partial Application</b> - <see cref="With"/>.
/// </summary>
/// <remarks>
/// Extension methods are up to arity of four, because functions with more arguments are considered code smell.
/// You probably need to refactor not an Extension method.
/// </remarks>
public static class Function
{
    #region Arity 2
    /// <summary>
    /// Transforms two argument function to single argument one.
    /// The rest of the arguments are embeded as unary functions as well.
    /// </summary>
    /// <param name="f">Binary function</param>
    /// <returns>Unary function</returns>
    public static Func<T1, Func<T2, R>> Unary<T1, T2, R>(this Func<T1, T2, R> f)
    => arg1 => arg2 => f(arg1, arg2);

    /// <summary>
    /// Transforms two argument function to single argument one by fixing one of the arguments.
    /// </summary>
    /// <param name="f">Binary function</param>
    /// <returns>Unary function</returns>
    public static Func<T2, R> With<T1, T2, R>(
        this Func<T1, T2, R> f, T1 arg1) => arg2 => f(arg1, arg2);

    public static Func<T1, R> With<T1, T2, R>(
        this Func<T1, T2, R> f, T2 arg2) => arg1 => f(arg1, arg2);
    #endregion

    #region Arity 3
    /// <summary>
    /// Transforms three argument function to single argument one.
    /// The rest of the arguments are embeded as unary functions as well.
    /// </summary>
    /// <param name="f">Ternary function</param>
    /// <returns>Unary function</returns>
    public static Func<T1, Func<T2, Func<T3, R>>> Unary<T1, T2, T3, R>(this Func<T1, T2, T3, R> f)
    => arg1 => arg2 => arg3 => f(arg1, arg2, arg3);

    /// <summary>
    /// Transforms three argument function to two argument one by fixing one of the arguments.
    /// </summary>
    /// <param name="f">Ternary function</param>
    /// <returns>Binary function</returns>
    public static Func<T2, T3, R> With<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> f, T1 arg1) => (arg2, arg3) => f(arg1, arg2, arg3);
    public static Func<T1, T3, R> With<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> f, T2 arg2) => (arg1, arg3) => f(arg1, arg2, arg3);
    public static Func<T1, T2, R> With<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> f, T3 arg3) => (arg1, arg2) => f(arg1, arg2, arg3);

    /// <summary>
    /// Transforms three argument function to one argument one by fixing two of the arguments.
    /// </summary>
    /// <param name="f">Ternary function</param>
    /// <returns>Unary function</returns>
    public static Func<T3, R> With<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> f, T1 arg1, T2 arg2) => (arg3) => f(arg1, arg2, arg3);
    public static Func<T2, R> With<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> f, T1 arg1, T3 arg3) => (arg2) => f(arg1, arg2, arg3);
    public static Func<T1, R> With<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> f, T2 arg2, T3 arg3) => (arg1) => f(arg1, arg2, arg3);
    #endregion

    #region Arity 4
    /// <summary>
    /// Transforms four argument function to single argument one.
    /// The rest of the arguments are embeded as unary functions as well.
    /// </summary>
    /// <param name="f">Quaternary function</param>
    /// <returns>Unary function</returns>
    public static Func<T1, Func<T2, Func<T3, Func<T4, R>>>> Unary<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> f)
    => arg1 => arg2 => arg3 => arg4 => f(arg1, arg2, arg3, arg4);


    /// <summary>
    /// Transforms four argument function to three argument one by fixing one of the arguments.
    /// </summary>
    /// <param name="f">Quaternary function</param>
    /// <returns>Ternary function</returns>
    public static Func<T2, T3, T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1) => (arg2, arg3, arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, T3, T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T2 arg2) => (arg1, arg3, arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, T2, T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T3 arg3) => (arg1, arg2, arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, T2, T3, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T4 arg4) => (arg1, arg2, arg3) => f(arg1, arg2, arg3, arg4);

    /// <summary>
    /// Transforms four argument function to two argument one by fixing two of the arguments.
    /// </summary>
    /// <param name="f">Quaternary function</param>
    /// <returns>Binary function</returns>
    public static Func<T3, T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1, T2 arg2) => (arg3, arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T2, T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1, T3 arg3) => (arg2, arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T2, T3, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1, T4 arg4) => (arg2, arg3) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T2 arg2, T3 arg3) => (arg1, arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, T3, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T2 arg2, T4 arg4) => (arg1, arg3) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, T2, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T3 arg3, T4 arg4) => (arg1, arg2) => f(arg1, arg2, arg3, arg4);

    /// <summary>
    /// Transforms four argument function to single argument one by fixing three of the arguments.
    /// </summary>
    /// <param name="f">Quaternary function</param>
    /// <returns>Unary function</returns>
    public static Func<T4, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1, T2 arg2, T3 arg3) => (arg4) => f(arg1, arg2, arg3, arg4);
    public static Func<T3, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1, T2 arg2, T4 arg4) => (arg3) => f(arg1, arg2, arg3, arg4);
    public static Func<T2, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T1 arg1, T3 arg3, T4 arg4) => (arg2) => f(arg1, arg2, arg3, arg4);
    public static Func<T1, R> With<T1, T2, T3, T4, R>(
        this Func<T1, T2, T3, T4, R> f, T2 arg2, T3 arg3, T4 arg4) => (arg1) => f(arg1, arg2, arg3, arg4);
    #endregion
}