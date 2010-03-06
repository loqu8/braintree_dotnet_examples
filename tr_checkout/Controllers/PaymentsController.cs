using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Braintree;

namespace BraintreeExamples.Controllers
{
    [HandleError]
    public class PaymentsController : Controller
    {
        private BraintreeGateway gateway;
        private const Decimal AMOUNT = 100M;

        public PaymentsController()
        {
            // the gateway must be configured before use, such as below
            gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "your_merchant_id",
                PublicKey = "your_public_key",
                PrivateKey = "your_private_key"
            };
        }

        public ActionResult New()
        {
            //  in a real app, this would be calculated from a shopping cart, determined by the product, etc.
            ViewData["Amount"] = AMOUNT;

            // used as the action on the form, and a key component of transparent redirect
            ViewData["TransparentRedirectURLForCreate"] = gateway.Transaction.TransparentRedirectURLForCreate();

            // another key component of transparent redirect, submitted along with the form to the gateway, and serves
            // as a signature for the request, as well as specifying the redirect destination. Depending on the request
            // type, specific fields must be passed in the trData rather than the form.
            ViewData["TrData"] = gateway.TrData(new TransactionRequest { Amount = AMOUNT, Type = TransactionType.SALE }, // key information about the transaction
                                                Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action("Confirm"));  // the redirect destination, in this case, the Confirm action below

            return View();
        }

        public ActionResult Confirm()
        {
            // after the form is posted to the gateway, we're redirected back here, per the redirect URL component of TrData 
            // in the New action
            
            // confirm the transaction we just posted via transparent redirect and receive the results of the transaction request
            Result<Transaction> result = gateway.Transaction.ConfirmTransparentRedirect(Request.Url.Query);

            if (result.IsSuccess())
            {
                // success indicates that the input parameters were valid and a transaction was successfully created
                // however, the transaction may be declined or rejected
                Transaction t = result.Target;
                ViewData["Amount"] = t.Amount;
                ViewData["TransactionId"] = t.Id;
                ViewData["FirstName"] = t.Customer.FirstName;
                ViewData["LastName"] = t.Customer.LastName;
                ViewData["Email"] = t.Customer.Email;
                ViewData["MaskedNumber"] = t.CreditCard.MaskedNumber;
                ViewData["CardType"] = t.CreditCard.CardType;

                return View();
            }
            else
            {
                // we're going to redisplay the form to the user, but we'll need to repopulate the fields with the values they posted to the gateway.
                RepopulateForm(result);

                // the gateway result also provides errors messages which we'll use to help the user fix errors in their submission
                PopulateErrors(result);

                New();

                return View("New");
            }
        }

        protected void RepopulateForm(Result<Transaction> result)
        {
            foreach (KeyValuePair<String, String> pair in result.Parameters)
            {
                ModelState[pair.Key] = new ModelState { Value = new ValueProviderResult(pair.Value, pair.Value, System.Globalization.CultureInfo.CurrentCulture) };
            }

            // sensitive fields won't be returned with the result, and with ASP.NET MVC, we have to set blank values on these fields so we can associate errors with them
            ModelState["transaction[credit_card][number]"] = new ModelState { Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentCulture) };
            ModelState["transaction[credit_card][cvv]"] = new ModelState { Value = new ValueProviderResult("", "", System.Globalization.CultureInfo.CurrentCulture) };
        }

        protected void PopulateErrors(Result<Transaction> result)
        {
            foreach (var pair in result.Errors.ByFormField())
            {
                foreach (var errorMessage in pair.Value)
                {
                    ModelState.AddModelError(pair.Key, errorMessage);
                }
            }
        }
    }
}
