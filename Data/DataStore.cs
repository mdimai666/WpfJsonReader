using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using ToastNotifications;
using ToastNotifications.Messages;

namespace JsonReaderDima
{

    public class DataStore
    {
        public List<Post> Posts { get; set; }

        public event Action OnPostsUpdate;

        public string FileName { get; set; } = "";

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
                            Notify($"on OpenFile error: {ex.Message}", NitifyType.Error);
                            System.Console.WriteLine(ex.Message);
                        }
                    }

                }
                else
                {

                    posts = JsonConvert.DeserializeObject<List<Post>>(json);
                }

                FileName = filename;

                //OnPostsUpdate?.Invoke();
                Notify($"OpenFile: {filename}", NitifyType.Success);

                return posts ?? new List<Post>();
            }
            else
            {
                return new List<Post>();
            }
        }

        internal void RemoveCats(List<string> requireRemove)
        {
            foreach (var item in Posts)
            {
                item.Cats = item.Cats.Where(s => !requireRemove.Contains(s)).ToObservableCollection();
            }

            OnPostsUpdate?.Invoke();

        }

        internal bool RemoveRelatedPosts(string value)
        {

            int willRemovedPostsCount = Posts.Where(s => s.Cats.Contains(value)).Count();

            Posts = Posts.Where(s => !s.Cats.Contains(value)).ToList();

            OnPostsUpdate?.Invoke();

            return willRemovedPostsCount > 0;
        }

        internal bool RemovePost(int id)
        {
            return RemovePost(new int[] { id });
        }

        public void SaveFile()
        {
            string json = JsonConvert.SerializeObject(Posts);
            File.WriteAllText(FileName, json, Encoding.UTF8);
        }

        internal bool RemovePost(IEnumerable<int> ids)
        {
            int willRemovedPostsCount = Posts.Where(s => ids.Contains(s.Id)).Count();
            Posts = Posts.Where(s => !ids.Contains(s.Id)).ToList();

            OnPostsUpdate?.Invoke();

            return willRemovedPostsCount > 0;
        }

        internal void UpdatePost(Post selectedPost)
        {
            int index = Posts.FindIndex(s => s.Id == selectedPost.Id);

            if (index > -1)
            {
                Posts[index] = selectedPost;
            }
        }

        void Notify(string message, NitifyType type = NitifyType.Information)
        {
            Notifier notifier = DependencyService.GetInstance<Notifier>();

            if (type == NitifyType.Information)
                notifier.ShowInformation(message);
            else if (type == NitifyType.Success)
                notifier.ShowSuccess(message);
            else if (type == NitifyType.Warning)
                notifier.ShowWarning(message);
            else if (type == NitifyType.Error)
                notifier.ShowError(message);
        }

        enum NitifyType
        {
            Success,
            Information,
            Warning,
            Error
        }
    }
}
