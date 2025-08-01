using Datalayer.Entities;


namespace Core.Services.Interfaces
{
    public interface IMediaService
    {
        List<Media> GetFirstMedias();

        bool IsNameExist(string mediaName);

        void SaveMediaInDatabase(string name, string url,string type);

        Media GetMediaByID(int id);

        public bool DeleteMedia(int id);

        public List<Media> GetMoreMedias(int page);
        public void UploadAparat(string UploadLink);

        public List<Slider> GetAllSliders(string Type);

        public List<Slider> GetSpecificSliders(int page, int countPerPage,string Type);

        public void SaveSlider(Slider slider);

        public Slider GetSlider(int id);

        public void SaveDatabase();

        public void DeleteSlider(int id);

        public bool DeleteManyImages(int[] AllImageId);

    }
}
