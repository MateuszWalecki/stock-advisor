namespace StockAdvisor.Infrastructure.Exceptions
{
    public static class ServiceErrorCodes
    {
        public static string UserNotFound => "user_not_found";
        public static string InvestorNotFound => "investor_not_found";
        public static string CompanyNotFound => "company_not_found";
        public static string EmailInUse => "email_is_in_use";
        public static string InvestorExists => "investor_exists";
    }
}