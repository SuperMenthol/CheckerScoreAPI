using CheckerScoreAPI.Model;
using System.Text.RegularExpressions;

namespace CheckerScoreAPI.Helpers
{
    public static class Validators
    {
        private const string NAME_REGEX = @"[^a-zA-Z0-9]";

        private const int playerNameMaxLength = 20;

        public static BaseResponse IsPlayerNameValid(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                return new BaseResponse(false, ResponseMessages.PLAYER_NAME_TOO_SHORT);
            }

            if (playerName.Length > playerNameMaxLength)
            {
                return new BaseResponse(false, string.Format(ResponseMessages.PLAYER_NAME_TOO_LONG, playerNameMaxLength));
            }

            if (Regex.Matches(playerName, NAME_REGEX).Count > 0)
            {
                return new BaseResponse(false, ResponseMessages.PLAYER_INVALID_CHARACTERS);
            }

            return new BaseResponse(true, ResponseMessages.PLAYER_NAME_SUCCESS_MESSAGE);
        }
    }
}
