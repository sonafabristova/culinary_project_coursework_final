namespace culinary_project_coursework
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; } // гр, мл, шт и т.д.
        public int RecipeId { get; set; }
    }
}