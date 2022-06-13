namespace CheckerScoreAPI.Helpers
{
    public static class ResponseMessages
    {
        public const string INSERT_PLAYER_FAILED = "Creating new player was not possible";
        public const string INSERT_PLAYER_SUCCEEDED = "Player created successfully";

        public const string MATCH_RESULT_POSTED = "Match result posted successfully";

        public const string PLAYER_ID_INVALID = "This player ID is not valid";

        public const string PLAYER_NAME_TOO_SHORT = "Player name cannot be empty";
        public const string PLAYER_NAME_TOO_LONG = "Player name cannot exceed {0} characters";
        public const string PLAYER_NAME_TAKEN = "This player name is already in use";
        public const string PLAYER_INVALID_CHARACTERS = "Player name contains invalid characters";
        public const string PLAYER_NAME_SUCCESS_MESSAGE = "Player name is valid";
    }
}
