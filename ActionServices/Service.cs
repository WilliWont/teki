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

        //public Bitmap ByteToImage(byte[] blob)
        //{
        //    using (MemoryStream mStream = new MemoryStream())
        //    {
        //        mStream.Write(blob, 0, blob.Length);
        //        mStream.Seek(0, SeekOrigin.Begin);

        //        Bitmap bm = new Bitmap(mStream);
        //        return bm;
        //    }
        //}

        //public void CropImage(ref byte[] img, int startX , int startY, int width, int height)
        //{
        //    Bitmap bmpImage = ByteToImage(img);
        //    Rectangle cropArea = new Rectangle(startX, startY, width, height);
        //    ImageConverter converter = new ImageConverter();
        //    var cropped = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        //    img = (byte[])converter.ConvertTo(cropped, typeof(byte[]));
        //}


        // Credited to COLIN FARR
        // function retrieved from:
        // http://www.britishdeveloper.co.uk/2011/05/image-resizing-cropping-and-compression.html
        public void ProcessImage(ref byte[] originalBytes, Size size, Rectangle crop, ImageFormat format)
        {
            using (var streamOriginal = new MemoryStream(originalBytes))
            using (var imgOriginal = Image.FromStream(streamOriginal))
            {
                //get original width and height of the incoming image
                var originalWidth = imgOriginal.Width; // 1000
                var originalHeight = imgOriginal.Height; // 800

                //get the percentage difference in size of the dimension that will change the least
                var percWidth = ( (float)size.Width / (float)originalWidth ); // 0.2
                var percHeight = ( (float)size.Height / (float)originalHeight ); // 0.25
                var percentage = Math.Max(percHeight, percWidth); // 0.25

                //get the ideal width and height for the resize (to the next whole number)
                var width = (int)Math.Max(originalWidth * percentage, size.Width); // 250
                var height = (int)Math.Max(originalHeight * percentage, size.Height); // 200

                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Default;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }

                    //work out the coordinates of the top left pixel for cropping
                    var x = ( width - size.Width ) / 2; // 25
                    var y = ( height - size.Height ) / 2; // 0

                    //create the cropping rectangle
                    var rectangle = crop; // 25, 0, 200, 200

                    //crop
                    using (var croppedBmp = resizedBmp.Clone(rectangle, resizedBmp.PixelFormat))
                    using (var ms = new MemoryStream())
                    {
                        //get the codec needed
                        var imgCodec = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid);

                        //make a paramater to adjust quality
                        var codecParams = new EncoderParameters(1);

                        //reduce to quality of 80 (from range of 0 (max compression) to 100 (no compression))
                        codecParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);

                        //save to the memorystream - convert it to an array and send it back as a byte[]
                        croppedBmp.Save(ms, imgCodec, codecParams);
                        originalBytes =  ms.ToArray();
                    }
                }
            }

        }

        public IQueryable<Article> SearchArticle(string searchValue)
        {
            return articleRepository.SearchArticle(searchValue);
        }
    }
}
