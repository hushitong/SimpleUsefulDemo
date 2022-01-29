using System.Collections;
using WebApplication1.Interface;

namespace WebApplication1.Class
{
    public class Personal5cs : IGame, IEnumerable
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public string SayYouName()
        {
            throw new System.NotImplementedException();
        }
    }
}
