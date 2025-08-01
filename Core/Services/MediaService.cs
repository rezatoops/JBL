using Core.Services.Interfaces;
using Datalayer.Context;
using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Core.Services
{
    public class MediaService : IMediaService
    {
        private ApplicationDbContext _context;

        public MediaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Media> GetFirstMedias()
        {
            var q = new List<Media>();
            q = (from a in _context.Medias orderby a.Id descending select a).Take(20).ToList();
            return q;
        }

        public bool IsNameExist(string mediaName)
        {
            var q = (from a in _context.Medias where a.Name == mediaName select a).ToList();
            if (q.Count() > 0)
                return true;
            else
                return false;
        }

        public void SaveMediaInDatabase(string name, string url, string type)
        {
            Media image = new Media() { Name = name, Url = url, Type = type };

            _context.Medias.Add(image);
            _context.SaveChanges();
        }

        public Media GetMediaByID(int id)
        {
            var q = (from a in _context.Medias where a.Id == id select a).FirstOrDefault();
            return q;
        }

        public bool DeleteMedia(int id)
        {
            var Image = (from a in _context.Medias where a.Id == id select a).FirstOrDefault();
            var ImageUrl = Image.Url;

            try
            {
                _context.Medias.Remove(Image);
                _context.SaveChanges();
                string ImageAddress = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + Image.Url);
                File.Delete(ImageAddress);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public List<Media> GetMoreMedias(int page)
        {
            var q = new List<Media>();
            q = (from a in _context.Medias orderby a.Id descending select a).Skip((page - 1) * 20).Take(20).ToList();
            return q;
        }

        public void UploadAparat(string UploadLink)
        {
            var media = new Media();
            media.Url = UploadLink;
            media.Type = "Aparat";
            media.Name = UploadLink;

            _context.Medias.Add(media);
            _context.SaveChanges();
        }

        public List<Slider> GetAllSliders(string Type)
        {
            return _context.Sliders.Include(s => s.Media).Include(s => s.MobileMedia).Where(s => s.Type == Type).OrderByDescending(p => p.Id).ToList();
        }

        public List<Slider> GetSpecificSliders(int page, int countPerPage, string Type)
        {
            return _context.Sliders.Include(p => p.Media).Include(s => s.MobileMedia).Where(s => s.Type == Type).OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public void SaveSlider(Slider slider)
        {
            _context.Sliders.Add(slider);
            _context.SaveChanges();
        }

        public Slider GetSlider(int id)
        {
            return _context.Sliders.Include(n => n.Media).Include(s => s.MobileMedia).FirstOrDefault(n => n.Id == id);
        }

        public void DeleteSlider(int id)
        {
            var slider = GetSlider(id);
            _context.Sliders.Remove(slider);

            _context.SaveChanges();
        }

        public void SaveDatabase()
        {
            _context.SaveChanges();
        }

        public bool DeleteManyImages(int[] AllImageId)
        {
            bool IsDeleted = true;

            foreach (var Id in AllImageId)
            {
                bool result = DeleteMedia(Id);
                if(result == false)
                {
                    IsDeleted = false;
                }
            }

            return IsDeleted;

        }
    }
}
