<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Payment: $<%= ViewData["Amount"] %></h1>

    <%= Html.ValidationSummary("Unable to process transaction.  Please correct the following errors and resubmit:") %>
    <form id="NewPaymentForm" action="<%= ViewData["TransparentRedirectURLForCreate"] %>" method="post" autocomplete="off">
    <div>
        <fieldset>
            <legend>Customer</legend>
            <div><label for="transaction[customer][first_name]">First Name</label></div>
            <div>
                <%= Html.TextBox("transaction[customer][first_name]") %>
                <%= Html.ValidationMessage("transaction[customer][first_name]")%>
            </div>
            <div><label for="transaction[customer][last_name]">Last Name</label></div>
            <div>
                <%= Html.TextBox("transaction[customer][last_name]") %>
                <%= Html.ValidationMessage("transaction[customer][last_name]")%>
            </div>
            <div><label for="transaction[customer][email]">Email</label></div>
            <div>
              <%= Html.TextBox("transaction[customer][email]") %>
              <%= Html.ValidationMessage("transaction[customer][email]")%>
            </div>
        </fieldset>
        <fieldset>
            <legend>Credit Card</legend>
            <div><label for="transaction[credit_card][number]">Number</label></div>
            <div>
                <%= Html.TextBox("transaction[credit_card][number]")%>
                <%= Html.ValidationMessage("transaction[credit_card][number]")%>
            </div>
            <div><label for="transaction[credit_card][expiration_date]">Expiration Date (MM/YY)</label></div>
            <div>
                <%= Html.TextBox("transaction[credit_card][expiration_date]")%>
                <%= Html.ValidationMessage("transaction[credit_card][expiration_date]")%>
            </div>
            <div><label for="transaction[credit_card][cvv]">CVV</label></div>
            <div>
                <%= Html.TextBox("transaction[credit_card][cvv]")%>
                <%= Html.ValidationMessage("transaction[credit_card][cvv]")%>
            </div>
        </fieldset>
        <input id="tr_data" name="tr_data" type="hidden" value="<%= ViewData["TrData"] %>" />
        <input id="transaction_submit" name="commit" type="submit" value="Submit" />
    </div>
    </form>
</asp:Content>
