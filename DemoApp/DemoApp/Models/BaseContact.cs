using System.ComponentModel.DataAnnotations;

namespace Even.Models
{
    public class BaseContact
    {
        public int ID { get; set; }
        public string ?Email { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }

    }
}
