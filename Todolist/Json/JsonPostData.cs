using System.Collections.Generic;

namespace Todolist.Json
{
    public class JsonPostData
    {
        public int Start { get; set; }

        public int Length { get; set; }

        public IList<Column> Columns { get; set; }

        public IList<Order> Order { get; set; }

        public Search Search { get; set; }

        public JsonPostData()
        {
            Columns = new List<Column>();
            Order = new List<Order>();
        }
    }
}