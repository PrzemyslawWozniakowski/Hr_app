using System.Collections.Generic;

namespace hr_app.Models
{
    public class JobOfferCreateView : JobOffer
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
