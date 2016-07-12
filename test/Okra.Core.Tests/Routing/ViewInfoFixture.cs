using Okra.Routing;
using Xunit;

namespace Okra.Tests.Routing
{
    public class ViewInfoFixture
    {
        [Fact]
        public void Constructor_SetsView()
        {
            var view = new object();
            var viewInfo = new ViewInfo(view);

            Assert.Equal(view, viewInfo.View);
            Assert.Equal(null, viewInfo.ViewModel);
        }

        [Fact]
        public void Constructor_SetsViewAndViewModel()
        {
            var view = new object();
            var viewModel = new object();
            var viewInfo = new ViewInfo(view, viewModel);

            Assert.Equal(view, viewInfo.View);
            Assert.Equal(viewModel, viewInfo.ViewModel);
        }

        [Fact]
        public void View_CanSetView()
        {
            var view = new object();
            var viewModel = new object();
            var viewInfo = new ViewInfo(null, viewModel);

            viewInfo.View = view;

            Assert.Equal(view, viewInfo.View);
            Assert.Equal(viewModel, viewInfo.ViewModel);
        }

        [Fact]
        public void ViewModel_CanSetViewModel()
        {
            var view = new object();
            var viewModel = new object();
            var viewInfo = new ViewInfo(view);

            viewInfo.ViewModel = viewModel;

            Assert.Equal(view, viewInfo.View);
            Assert.Equal(viewModel, viewInfo.ViewModel);
        }
    }
}