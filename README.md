# Stripe.Net.AddOn
Added functionality (with testing) for the Stripe.Net library by Jayme Davis, for C# and .NET.

In particular, this library handles higher level functionality for Stripe, allowing a developer to make simpler, more abstract method calls to integrate Stripe's services with their application.

The core functionality allows the developer to:

- Create customers from credit card information
- Charge customers a custom amount, with a custom description appearing on the invoice
- Sign customers up for billing plans
- Unsubscribe customers from billing plans
- Easily create or delete payment plans from within your application
- Add items to a customer's billing invoice, if they are on a payment plan (use this for metered billing or pay-as-you-go plans!)
- Much more!

The service is properly documented and has preliminary unit and integration testing, and more functionality will come later.

To run the tests in the test project, you merely need to add a Stripe test-key to the App.config file in the test project.

Similarly, to use Stripe, you must add a key that accesses your Stripe account, into your App.config file. This is how the application will connect to Stripe.


# Feedback
Please send all feedback, whether positive or negative, or inquiries, to codefun64@gmail.com.
