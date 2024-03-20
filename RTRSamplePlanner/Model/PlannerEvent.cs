using System;
using ReactorData;

namespace RTRSamplePlanner.Model
{
    [Model]
    partial class PlannerEvent
    {
        public PlannerEvent() { }

        public PlannerEvent(PlannerEvent newEvent) 
        {
            this.Id = newEvent.Id;
            this.Name = newEvent.Name;  
            this.Description = newEvent.Description;    
            this.Date = newEvent.Date;
            this.Location = newEvent.Location;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }
    }
}
