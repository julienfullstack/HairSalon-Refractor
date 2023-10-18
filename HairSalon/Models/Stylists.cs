using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Models
{
    public class Stylist
    {
        public int StylistId { get; set; }
        
        [Required(ErrorMessage = "This field can't be left empty")]
        public string? Name { get; set; }
        
         public List<Client> Clients{ get; set; }
        
        public List<ClientStylist> JoinEntities { get;}
        

    }
}

