using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "This field can't be left empty")]
        public string? Name { get; set; }
        public int StylistId { get; set; }
        public virtual Stylist Stylist { get; set; }
        public List<ClientStylist> JoinEntities { get;}

    }
}