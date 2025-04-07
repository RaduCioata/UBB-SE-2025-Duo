using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuolingoNou.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public DateTime AwardedDate { get; set; }

        public string AwardedDateFormatted => AwardedDate.ToString("MM/dd/yyyy");
    }
}
