using System;

namespace StartOfNewPath.BusinessLayer.DTO
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset FinishDate { get; set; }

        public bool IsHaveCheckTestBefore { get; set; }

        public bool IsHaveCheckTestAfter { get; set; }

        public int MaxMentee { get; set; }

        public int Population { get; set; }

        public int Difficulty { get; set; }

        public string OwnerId { get; set; }
    }
}
