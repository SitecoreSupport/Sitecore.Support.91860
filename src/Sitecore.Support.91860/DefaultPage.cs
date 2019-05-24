namespace Sitecore.Support
{
    using Sitecore.Security;
    using Sitecore.Web.Authentication;
    using System;
    using System.Web.UI;

    public class DefaultPage : Page
    {
        private void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentTicketId = TicketManager.GetCurrentTicketId();
            if ((string.IsNullOrEmpty(currentTicketId) || !TicketManager.IsTicketValid(currentTicketId)) || !TicketManager.Relogin(currentTicketId, true))
            {
                string startUrl = string.Empty;
                if (!string.IsNullOrEmpty(currentTicketId))
                {
                    Ticket ticket = TicketManager.GetTicket(currentTicketId);
                    if (ticket != null)
                    {
                        startUrl = ticket.StartUrl;
                    }
                }
                if (!string.IsNullOrEmpty(startUrl))
                {
                    base.Response.Redirect(startUrl);
                }
                base.Response.Redirect(SecurityHelper.CanRunApplication("Desktop") ? "/sitecore/shell" : Sitecore.Context.Site.LoginPage);
            }
        }
    }
}
