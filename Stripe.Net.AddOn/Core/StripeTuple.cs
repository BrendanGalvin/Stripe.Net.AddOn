namespace Stripe.Net.AddOn.Core
{
    /// <summary>
    /// A documented Tuple lookalike object, that is used to handle the problem of multiple return values of asynchronous methods in this Stripe service add-on.
    /// This is as-of-yet not used, but may be used in future development of this Stripe service add-on.
    /// </summary>
    public class StripeTuple
    {
        public bool TryGet;
        public string MethodValue;
    }
}
