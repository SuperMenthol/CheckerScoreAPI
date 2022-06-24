using Infrastructure.Model;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers
{
    public static class Validators
    {
        private const string NAME_REGEX = @"[^a-zA-Z0-9]";

        private const int playerNameMaxLength = 20;

        public static BaseResponse<object> IsPlayerNameValid(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                return BaseResponse.GetResponse<object>(false, ResponseMessages.PLAYER_NAME_TOO_SHORT, new());
            }

            if (playerName.Length > playerNameMaxLength)
            {
                return BaseResponse.GetResponse<object>(false, string.Format(ResponseMessages.PLAYER_NAME_TOO_LONG, playerNameMaxLength), new());
            }

            if (Regex.Matches(playerName, NAME_REGEX).Count > 0)
            {
                return BaseResponse.GetResponse<object>(false, ResponseMessages.PLAYER_INVALID_CHARACTERS, new());
            }

            return BaseResponse.GetResponse<object>(true, ResponseMessages.PLAYER_NAME_SUCCESS_MESSAGE, new());
        }
    }
}
