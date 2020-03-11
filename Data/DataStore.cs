using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonReaderDima
{

    public class DataStore
    {
        public List<Post> Posts { get; set; }

        public DataStore(string filename = null)
        {
            Posts = Load(filename);


        }

        List<Post> Load(string filename = null)
        {
            //string filename = @"C:\Users\D\Desktop\XPoems\parse\v1\db.json";

            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {

                var json = File.ReadAllText(filename);
                var posts = JsonConvert.DeserializeObject<List<Post>>(json);

                return posts;
            } else
            {
                return new List<Post>();
            }
        }

    }
}
