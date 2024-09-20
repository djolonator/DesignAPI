using Infrastructure.Abstractions.Errors;

namespace Infrastracture.Abstractions.Errors
{
    public static class DesignErrors
    {
        public static readonly Error NotFound = new Error(
                 "Design.NotFound",
                 "Dizajn nije pronađen");
    }
}
