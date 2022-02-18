
using Microsoft.AspNetCore.Identity;

namespace TimeManager.API.JwtHelpers
{
    public static class StringHelper
    {

        public static string IdentityError(IEnumerable<IdentityError> strs)
        {
            string finalString = "";
            if (strs != null)
            {
                foreach (var str in strs)
                {
                    finalString += " \n" + str.Description;
                }
            }
            return finalString;
        }

    }
}
