namespace StockAdvisor.Core.Exceptions
{
    public static class ErrorCodes
    {
        public static string InvalidEmail => "invalid_email";
        public static string InvalidPassword => "invalid_password";
        public static string InvalidRole => "invalid_role";
        public static string InvalidUsername => "invalid_username";
        public static string InvalidFirstName => "invalid_first_name";
        public static string InvalidSurname => "invalid_surname";
        public static string ElementNotInSet => "element_not_included_in_set";
        public static string ElementInSet => "element_is_included_in_set";
    }
}