using BusinessObjects;
using DataObjects;
using DataObjects.IRepository;
using DataObjects.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActionServices
{
    public class Service : IService
    {
        private readonly ApplicationDBContext _context;
        private readonly UnitOfWork unitOfwork;
        private readonly IArticleRepository articleRepository ;
        private readonly IStatusRepository statusRepository ;

        public Service(ApplicationDBContext context)
        {
            _context = context;
            unitOfwork = new UnitOfWork(_context);
            articleRepository = unitOfwork.ArticleRepository;
            statusRepository = unitOfwork.StatusRepository;
        } 

        // Article Service implemetation
        public IEnumerable<Article> GetAllArticle()
        {
            IEnumerable<Article> list = articleRepository.GetAll();
            return list;
        }
        public void AddArticle(Article article)
        {
            articleRepository.Add(article);
        }
        public Article GetArticle(Guid id)
        {
            Article article = articleRepository.GetArticleInfo(id);
            return article;
        }

        // Status Service Implementation
        public Status GetStatus(string name)
        {
            Status status = statusRepository.GetStatus(name);
            return status;
        }

        public async Task<bool> Commit()
        {
            return await unitOfwork.Commit();
        }

        public IQueryable<Article> GetArticleWithUserID(ApplicationUser user)
        {
            return articleRepository.GetArticlesByID(user);
        }

        public bool UpdateArticle(Article article)
        {
            return articleRepository.UpdateArticle(article);
        }

        public IQueryable<Article> SearchArticle(string searchValue)
        {
            return articleRepository.SearchArticle(searchValue);
        }

        public IQueryable<Article> GetArticleByStatus(string status)
        {
            return articleRepository.GetArticleByStatus(status);
        }

        public void GetImage(out byte[] img, HttpRequest req)
        {
                foreach (var file in req.Form.Files)
                {
                    // TODO: Compress Image if have time
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img = ms.ToArray();
                    ms.Close();

                    ms.Dispose();

                    return;
                }
            img = null;

        }


        // Credited to COLIN FARR
        // ResizeImgageByWidth & CropImage derived from:
        // http://www.britishdeveloper.co.uk/2011/05/image-resizing-cropping-and-compression.html
        public byte[] ResizeImgageByWidth(byte[] originalBytes, int w, ImageFormat format)
        {
            using (var streamOriginal = new MemoryStream(originalBytes))
            using (var imgOriginal = Image.FromStream(streamOriginal))
            {
                //get original width and height of the incoming image
                var originalWidth = imgOriginal.Width; // 1000
                var originalHeight = imgOriginal.Height; // 800
                var ratio = (originalHeight * 1.0 / originalWidth * 1.0);

                //get the ideal width and height for the resize (to the next whole number)
                var width = w;
                var height = (int) Math.Floor(ratio * w);

                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Default;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }

                    Rectangle rectangle = new Rectangle(0,0, width, height);

                    using (var croppedBmp = resizedBmp.Clone(rectangle, resizedBmp.PixelFormat))
                    using (var ms = new MemoryStream())
                    {
                        //get the codec needed
                        var imgCodec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid);

                        //make a paramater to adjust quality
                        var codecParams = new EncoderParameters(1);

                        codecParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

                        //save to the memorystream - convert it to an array and send it back as a byte[]
                        croppedBmp.Save(ms, imgCodec, codecParams);
                        return ms.ToArray();
                    }
                }
            }
        }

        public byte[] CropImage(byte[] originalBytes, Rectangle crop, ImageFormat format)
        {
            using (var streamOriginal = new MemoryStream(originalBytes))
            using (var imgOriginal = Image.FromStream(streamOriginal))
            {
                //get original width and height of the incoming image
                var width = imgOriginal.Width; // 1000
                var height = imgOriginal.Height; // 800

                crop.Width = (width < crop.Width) ? width : crop.Width;
                crop.Height = (height < crop.Height) ? height : crop.Height;

                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Default;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }

                    Rectangle rectangle = crop;

                    using (var croppedBmp = resizedBmp.Clone(rectangle, resizedBmp.PixelFormat))
                    using (var ms = new MemoryStream())
                    {

                        //get the codec needed
                        var imgCodec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid);

                        //make a paramater to adjust quality
                        var codecParams = new EncoderParameters(1);

                        codecParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

                        //save to the memorystream - convert it to an array and send it back as a byte[]
                        croppedBmp.Save(ms, imgCodec, codecParams);
                        return ms.ToArray();
                    }
                }
            }
        }

        public IQueryable<Article> GetArticleForViewer(ApplicationUser user)
        {
            return articleRepository.GetArticlesForViewer(user);
        }
    }
}
