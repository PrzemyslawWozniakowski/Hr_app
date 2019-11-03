using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_app.Models
{
    public class Job_OfferContext 
    {
        public Job_OfferContext()
        {
            job_Offers = new List<Job_Offer>(new Job_Offer[] { new Job_Offer(1,"CEO", new decimal(1000), new decimal(15020), new DateTime(2019, 10, 24), "Warsaw", "Some job",new DateTime(2019, 10, 27), new Company(1,"Daftcode"),1),
                                                                         new Job_Offer(2,"DevOps", new decimal(15000), new decimal(35020), new DateTime(2019, 10, 24), "Warsaw", "Some job",new DateTime(2019, 10, 27),new Company(1,"Daftcode"),1),
                                                                           new Job_Offer(3,"Junior .Net Programmer", new decimal(15000), new decimal(15020), new DateTime(2019, 10, 24), "Warsaw", "Some job",new DateTime(2019, 10, 29),new Company(1,"Daftcode"),1) });
        }
        public List<Job_Offer> job_Offers { get; set; }
    }
}
