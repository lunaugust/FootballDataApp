namespace Football.Client.Models
{
    public class CompetitionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public AreaModel Area { get; set; }
    }
}
