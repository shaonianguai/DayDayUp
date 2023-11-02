using System.Collections.Generic;

namespace WpfApp1
{

    public class Filter
    {
        string name = "";
        List<string> param = new List<string>();
        string url = "";

        public Filter(string name, List<string> param, string url)
        {
            this.name = name;
            this.param = param;
            this.url = url;
        }

        public string Name { get => name; set => name = value; }
        public List<string> Param { get => param; set => param = value; }
        public string Url { get => url; set => url = value; }
    }
}
