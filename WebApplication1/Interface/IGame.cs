namespace WebApplication1.Interface
{
    public interface IGame
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string ToString() => $"Name;{Name}, Description: {Description}";

        //public IGame();  //不能又构造函数

        public abstract string SayYouName();
    }
}
