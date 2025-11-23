namespace culinary_project_coursework.Classes
{
    public class CookingStep
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
    }
}