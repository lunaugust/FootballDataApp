using System;

namespace Football.Client.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CountryOfBirth { get; set; }

        public string Nationality { get; set; }
    }
}
