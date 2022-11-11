namespace Soundbox.Reloaded.Ui.Web.Controllers.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.AspNet.SignalR;

    using Soundbox.Reloaded.Ui.Presentation.Dto.Sounds;
    using Soundbox.Reloaded.Ui.Presentation.ViewServices;
    using Soundbox.Reloaded.Ui.Web.Controllers.Hubs;
    using System.Configuration;

    public class SoundController : ApiController
    {
        private readonly SoundViewService soundViewService;
        private readonly PlaybackViewService playbackViewService;
        private readonly IHubContext soundHub;

        public SoundController()
        {
            this.soundViewService = new SoundViewService();
            this.playbackViewService = new PlaybackViewService(this.RefreshQueueAndCurrentSound);
            this.soundHub = GlobalHost.ConnectionManager.GetHubContext<SoundHub>();
        }

        [HttpGet, ActionName("GetAllCategoriesWithSounds")]
        public IEnumerable<SoundCategoryDto> GetAllCategoriesWithSounds()
        {
            return this.soundViewService.GetAllCategoriesWithSounds();
        }

        [HttpGet, ActionName("GetQueue")]
        public IEnumerable<SoundDto> GetQueue()
        {
            return this.playbackViewService.GetQueue();
        }

        [HttpGet, ActionName("GetCurrentSound")]
        public PlayableSoundDto GetCurrentSound()
        {
            return this.playbackViewService.GetCurrentSound();
        }

        [HttpGet, ActionName("IsLooping")]
        public bool IsLooping()
        {
            return this.playbackViewService.IsLooping();
        }

        [HttpPost, ActionName("SetLooping")]
        public void SetLooping(bool loop)
        {
            this.playbackViewService.SetLooping(loop);
            this.RefreshIsLooping();
        }

        [HttpPost, ActionName("PlaySoundFromQueue")]
        public void PlaySoundFromQueue()
        {
            this.playbackViewService.PlaySoundFromQueue();
            this.RefreshQueueAndCurrentSound();
        }

        [HttpPost, ActionName("PlayPause")]
        public void PlayPause()
        {
            var isPlaying = this.playbackViewService.PlayPause();
            if (isPlaying)
            {
                this.RefreshCurrentSound();
            }
            else
            {
                this.Pause();
            }
        }

        [HttpPost, ActionName("Seek")]
        public void Seek(int seconds)
        {
            this.playbackViewService.Seek(seconds);
            this.RefreshCurrentSound();
        }

        [HttpPost, ActionName("AddToQueue")]
        public void AddToQueue(Guid soundId)
        {
            var isNoSoundPlaying = this.playbackViewService.GetCurrentSound() == null;
            this.playbackViewService.AddToQueue(soundId);

            if (isNoSoundPlaying)
            {
                this.RefreshCurrentSound();
            }

            this.RefreshQueue();
        }

        [HttpPost, ActionName("RemoveFromQueue")]
        public void RemoveFromQueue(int index)
        {
            this.playbackViewService.RemoveFromQueue(index);
            this.RefreshQueue();
        }

        [HttpPost, ActionName("AddNewCategoryWithSound")]
        public async Task<HttpResponseMessage> AddNewCategoryWithSound()
        {
            try
            {
                var uploadResult = await this.UploadFiles<SoundCategoryDto>();

                var soundCategory = uploadResult.Item1;
                soundCategory.Sounds.First().SoundFileName = uploadResult.Item2;
                soundCategory.Sounds.First().ImageFileName = uploadResult.Item3;

                if (soundCategory.Sounds.Count != 1)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, "There should be exactly one sound on the category");
                }

                var validationErrors = this.soundViewService.AddNewCategoryWithSound(soundCategory);

                if (validationErrors.Any())
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, validationErrors.ToArray());
                }
            }
            catch (Exception e)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            this.RefreshAvailableSounds();
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, ActionName("AddNewSoundToCategory")]
        public async Task<HttpResponseMessage> AddNewSoundToCategory()
        {
            try
            {
                var uploadResult = await this.UploadFiles<SoundDto>();

                var sound = uploadResult.Item1;
                sound.SoundFileName = uploadResult.Item2;
                sound.ImageFileName = uploadResult.Item3;

                var validationErrors = this.soundViewService.AddNewSoundToCategory(sound);

                if (validationErrors.Any())
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, validationErrors.ToArray());
                }
            }
            catch (Exception e)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            this.RefreshAvailableSounds();
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, ActionName("GetSoundFile")]
        public HttpResponseMessage GetSoundFile(string filename)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = Path.Combine(rootPath, ConfigurationManager.AppSettings["SoundFolder"], filename);

            byte[] file = File.ReadAllBytes(fullPath);
            var mediaType = MediaTypeHeaderValue.Parse("audio/mpeg");

            var memoryStream = new MemoryStream(file);

            if (this.Request.Headers.Range != null)
            {
                try
                {
                    var partialResponse = this.Request.CreateResponse(HttpStatusCode.PartialContent);
                    partialResponse.Content = new ByteRangeStreamContent(memoryStream, Request.Headers.Range, mediaType);
                    return partialResponse;
                }
                catch (InvalidByteRangeException invalidByteRangeException)
                {
                    return this.Request.CreateErrorResponse(invalidByteRangeException);
                }
            }
            else
            {
                var fullResponse = this.Request.CreateResponse(HttpStatusCode.OK);
                fullResponse.Content = new StreamContent(memoryStream);
                fullResponse.Content.Headers.ContentType = mediaType;
                return fullResponse;
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.soundViewService.Dispose();
            this.playbackViewService.Dispose();
            base.Dispose(disposing);
        }

        private void RefreshQueue()
        {
            this.soundHub.Clients.All.refreshQueue(this.GetQueue());
        }

        private void RefreshIsLooping()
        {
            this.soundHub.Clients.All.refreshIsLooping(this.IsLooping());
        }

        private void RefreshCurrentSound()
        {
            this.soundHub.Clients.All.refreshCurrentSound(this.GetCurrentSound());
        }

        private void RefreshQueueAndCurrentSound()
        {
            this.soundHub.Clients.All.refreshQueue(this.GetQueue());
            this.soundHub.Clients.All.refreshCurrentSound(this.GetCurrentSound());
        }

        private void Pause()
        {
            this.soundHub.Clients.All.pause();
        }

        private void RefreshAvailableSounds()
        {
            this.soundHub.Clients.All.refreshAvailableSounds(this.GetAllCategoriesWithSounds());
        }

        private async Task<Tuple<T, string, string>> UploadFiles<T>()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new Exception("Unsupported media type");
            }

            var provider = FileUploadHelper.GetMultipartProvider();
            var result = await this.Request.Content.ReadAsMultipartAsync(provider);

            if (result.FileData.Count == 0)
            {
                throw new Exception("Upload error");
            }

            var storedSoundFilename = StoreFile(result.FileData[0], ConfigurationManager.AppSettings["SoundFolder"], ".mp3");
            string storedImageFilename = null;

            if (result.FileData.Count > 1 && result.FileData[1] != null)
            {
                storedImageFilename = StoreFile(result.FileData[1], ConfigurationManager.AppSettings["ImageFolder"], ".png");
            }

            var fileDescription = FileUploadHelper.GetFormData<T>(result);

            return Tuple.Create(fileDescription, storedSoundFilename, storedImageFilename);
        }

        private static string StoreFile(MultipartFileData file, string folderName, string supportedExtension)
        {
            var uploadedFileInfo = new FileInfo(file.LocalFileName);

            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var targetDirectory = new DirectoryInfo(Path.Combine(rootPath, folderName));

            if (!targetDirectory.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            var originalExtension = Path.GetExtension(FileUploadHelper.GetDeserializedFileName(file));
            var generatedFileName = Guid.NewGuid().ToString();

            if (!StringComparer.InvariantCultureIgnoreCase.Equals(originalExtension, supportedExtension))
            {
                uploadedFileInfo.Delete();
                return null;
            }

            var fileFullname = generatedFileName + originalExtension;
            var newPath = Path.Combine(targetDirectory.FullName, fileFullname);
            
            uploadedFileInfo.MoveTo(newPath);

            return fileFullname;
        }
    }
}
