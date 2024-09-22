namespace TestTask.Context.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string status { get; set; }
        public int? ParentId { get; set; } // Связь с родительским подразделением
    }
}
