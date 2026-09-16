using System.Security.Claims;

namespace NotesApi.Common;

public static class UserExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userID = user.FindFirst("userId")?.Value;
        if (int.TryParse(userID, out var id))
        {
            return id;
        }

        throw new UnauthorizedAccessException("Valid User ID claim not found in JWT token.");
    }

    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst("username")?.Value ?? string.Empty;
    }
}
