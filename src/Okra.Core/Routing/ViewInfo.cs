namespace Okra.Routing
{
    public class ViewInfo
    {
        public ViewInfo(object view)
            : this(view, null)
        {
        }

        public ViewInfo(object view, object viewModel)
        {
            View = view;
            ViewModel = viewModel;
        }

        public object View
        {
            get;
            set;
        }

        public object ViewModel
        {
            get;
            set;
        }
    }
}