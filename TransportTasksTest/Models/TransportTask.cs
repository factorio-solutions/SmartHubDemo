namespace TransportTasksTest.Models
{
    public class TransportTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsCompleted { get; set; }

        public TransportTask()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsCompleted = false;
        }
        public void MarkAsCompleted()
        {
            IsCompleted = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
