
namespace Application.Constants
{
    public static class CountriesNames
    {
        public static Dictionary<string, string> CountryNames()
        {
            var countryNames = new Dictionary<string, string>();
            countryNames.Add("RS", "Serbia");
            countryNames.Add("US", "USA");

            return countryNames;
        }
    }
}
