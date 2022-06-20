namespace CheckerScoreAPI.Helpers
{
    public static class ResponseMessages // TO DO: EXPORT TO FILE
    {
        public const string CREATE_PLAYER_FAILED = "Creating new player was not possible";
        public const string CREATE_PLAYER_SUCCEEDED = "Player created successfully";

        public const string LOGIN_FAILED = "Login failed";

        public const string RENAME_PLAYER_SUCCEEDED = "Player successfully renamed";

        public const string MATCH_RESULT_POSTED = "Match result posted successfully";
        public const string MATCH_POST_FAILURE = "It was not possible to post match result";

        public const string PLAYER_ID_INVALID = "This player ID is not valid";

        public const string PLAYER_NAME_TOO_SHORT = "Player name cannot be empty";
        public const string PLAYER_NAME_TOO_LONG = "Player name cannot exceed {0} characters";
        public const string PLAYER_NAME_TAKEN = "This player name is already in use";
        public const string PLAYER_INVALID_CHARACTERS = "Player name contains invalid characters";
        public const string PLAYER_NAME_SUCCESS_MESSAGE = "Player name is valid";

        public const string PLAYER_RENAME_FAILURE = "Renaming was not successful.";

        public const string PLAYER_INFO_FAILURE = "Retrieving player information was not successful.";
        public const string SUMMARY_FAILURE = "Retrieving player summary did not succeed.";
        public const string RESULTS_FAILURE = "Retrieving match results did not succeed.";
    }
}
