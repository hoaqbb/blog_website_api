namespace blog_website_api.Helpers.Params
{
    public class PaginationParams
    {
        private int _index = 1;
        // set pageIndex = 1 if the value set < 0
        public int PageIndex { 
            get => _index; 
            set => _index = (value < 1) ? _index : value; 
        }
        public int PageSize { get; } = 3;
    }
}
