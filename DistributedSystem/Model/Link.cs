namespace DistributedSystem.Model
{
    public class Link : BaseModel
    {
        public string URL { get; set; }
        public bool status {  get; set; }
        public int statusCode { get; set; }

        public Link(string url)
        {
            this.URL = url;
            this.status = false;
            this.statusCode = 0;
        }

        public Link() { }
    }
}
