namespace DbLogger
{
    public sealed class DbLoggerOptions
    {
        public int EventId { get; set; }
        public String ConnectionString { get; init; }
        public String ConnectionStringName { get; init; }
        public String[] LogFields { get; init; }
        public String LogTable { get; init; }
    }   
}