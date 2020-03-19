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
                List<Post> posts;

                if (json.Length > 0 && json[0] != '[')
                {
                    posts = new List<Post>();

                    string[] list = json.Split('\n');

                    foreach (string a in list)
                    {
                        if (string.IsNullOrEmpty(a)) continue;
                        try
                        {
                            Post post = JsonConvert.DeserializeObject<Post>(a);

                            if (post != null)
                            {
                                posts.Add(post);
                            }
                            else
                            {
                                System.Console.WriteLine(a);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                        }
                    }

                }
                else
                {

                    posts = JsonConvert.DeserializeObject<List<Post>>(json);
                }

                return posts ?? new List<Post>();
            }
            else
            {
                return new List<Post>();
            }
        }

    }
}
