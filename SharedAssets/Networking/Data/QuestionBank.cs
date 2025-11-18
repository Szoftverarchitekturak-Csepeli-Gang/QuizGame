public class QuestionBank
{
    public int Id { get; set; }
    public string Name { get; set; }

    public QuestionBank(int id, string name)
    {
        Id = id;
        Name = name;
    }
}