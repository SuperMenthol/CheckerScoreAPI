namespace Domain.Data
{
    public class CheckerScoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string MatchResultsName { get; set; } = null!;
        public string PlayerDataName { get; set; } = null!;
    }
}
