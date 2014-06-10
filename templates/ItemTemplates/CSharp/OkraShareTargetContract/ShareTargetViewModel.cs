using Okra.Core;
using Okra.DataTransfer;
using Okra.Navigation;
using $safeprojectname$.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace $rootnamespace$
{
    // TODO: Add the following code to your AppBootstrapper,
    //
    //   [Import]
    //   public Okra.DataTransfer.IShareTargetManager ShareTargetManager { get; set; }
    //

    /// <summary>
    /// This view-model allows other applications to share content through this application.
    /// </summary>
    [ViewModelExport(SpecialPageNames.ShareTarget)]
    public class $fileinputname$ViewModel : ViewModelBase, IShareTarget
    {
        private IShareOperation shareOperation;

        private string title;
        private string description;
        private bool sharing;
        private bool showImage;
        private string comment;
        private bool supportsComment;
        private ImageSource image;

        public $fileinputname$ViewModel()
        {
            InitializeCommands();
        }

        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                SetProperty(ref comment, value);
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            protected set
            {
                SetProperty(ref description, value);
            }
        }

        public ImageSource Image
        {
            get
            {
                return image;
            }
            set
            {
                SetProperty(ref image, value);
            }
        }

        public bool Sharing
        {
            get
            {
                return sharing;
            }
            protected set
            {
                SetProperty(ref sharing, value);
                ((DelegateCommand)ShareCommand).NotifyCanExecuteChanged();
            }
        }

        public bool ShowImage
        {
            get
            {
                return showImage;
            }
            protected set
            {
                SetProperty(ref showImage, value);
            }
        }

        public bool SupportsComment
        {
            get
            {
                return supportsComment;
            }
            protected set
            {
                SetProperty(ref supportsComment, value);
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            protected set
            {
                SetProperty(ref title, value);
            }
        }

        public ICommand ShareCommand
        {
            get;
            private set;
        }

        public async void Activate(IShareOperation shareOperation)
        {
            this.shareOperation = shareOperation;

            // Communicate metadata about the shared content through the view model

            var shareProperties = this.shareOperation.Data.Properties;

            this.Title = shareProperties.Title;
            this.Description = shareProperties.Description;
            this.Sharing = false;
            this.Comment = String.Empty;
            this.SupportsComment = true;

            // Update the shared content's thumbnail image in the background

            BitmapImage thumbnailImage = new BitmapImage();
            this.Image = thumbnailImage;
            this.ShowImage = false;

            if (shareProperties.Thumbnail != null)
            {
                var stream = await shareProperties.Thumbnail.OpenReadAsync();
                thumbnailImage.SetSource(stream);
                this.ShowImage = true;
            }
        }

        public bool CanShare()
        {
            return !this.Sharing;
        }

        public void Share()
        {
            this.Sharing = true;

            shareOperation.ReportStarted();

            // TODO: Perform work appropriate to your sharing scenario using
            //       this.shareOperation.Data, typically with additional information captured
            //       through custom user interface elements added to this page such as 
            //       this.Comment

            shareOperation.ReportCompleted();
        }

        private void InitializeCommands()
        {
            this.ShareCommand = new DelegateCommand(Share, CanShare);
        }
    }
}
