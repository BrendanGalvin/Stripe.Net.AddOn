namespace Stripe.Net.AddOn.Core
{
    public static class StripeAttributes
    {
        /// <summary>
        /// Used for a subscription attached to a customer, if the customer is still using trial time for the subscription - i.e. is not paying yet.
        /// </summary>
        public static string Trialing => "trialing";
        /// <summary>
        /// Used for creating, updating, or otherwise using a charge against a customer.
        /// </summary>
        public static string Succeeded => "succeeded";
        /// <summary>
        /// Used for creating, updating, or otherwise using a charge against a customer.
        /// </summary>
        public static string Currency => "USD";
        /// <summary>
        /// Used for subscriptions that are active and billing the user according to the billing plan's properties.
        /// </summary>
        public static string Active => "active";
        /// <summary>
        /// Used for a failed payment/charge. Also used for when a method in the service encounters an error.
        /// </summary>
        public static string Failed => "failed";
        /// <summary>
        /// Used for a canceled return.
        /// </summary>
        public static string Cancelled => "cancelled";
        /// <summary>
        /// Used for a dispute or list of disputes started by a customer, over a payment or invoice, that the business has not yet responded to.
        /// </summary>
        public static string NeedsResponse => "needs_response";
        /// <summary>
        /// Used for a return or dispute to indicate it is lost, i.e. the merchant lost, and money must/will be refunded to the customer.
        /// </summary>
        public static string Lost => "lost";
        /// <summary>
        /// Used for the Transfer API's, typically to transfer funds.
        /// </summary>
        public static string InTransit => "in_transit";
        /// <summary>
        /// Used when using a bank account API, or for new returns.
        /// </summary>
        public static string New => "new";
        /// <summary>
        /// Used for the Order API's.
        /// </summary>
        public static string Created => "created";
        /// <summary>
        /// Used for the Pay an Order API.
        /// </summary>
        public static string Paid => "paid";
        /// <summary>
        /// Used for the Account API's if the account is unverified.
        /// </summary>
        public static string Unverified => "unverified";
        /// <summary>
        /// Used for the Balance Transaction API's.
        /// </summary>
        public static string Pending => "pending";

    }
}
