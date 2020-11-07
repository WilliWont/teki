﻿using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using BusinessObjects;
using DataObjects;
using DataObjects.IRepository;
using DataObjects.Repository;
using Microsoft.AspNetCore.Http;
using Smartcrop;
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
        private readonly IArticleRepository articleRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IBookmarkRepository bookmarkRepository;
        private readonly ITagRepository tagRepository;

        public Service(ApplicationDBContext context)
        {
            _context = context;
            unitOfwork = new UnitOfWork(_context);
            articleRepository = unitOfwork.ArticleRepository;
            statusRepository = unitOfwork.StatusRepository;
            bookmarkRepository = unitOfwork.BookmarkRepository;
            tagRepository = unitOfwork.TagRepository;
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

        public IQueryable<Article> GetArticleWithUser(ApplicationUser user)
        {
            return articleRepository.GetArticlesByUser(user);
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
                var height = (int)Math.Floor(ratio * w);

                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Default;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }

                    System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, width, height);

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

        public byte[] CropImage(byte[] originalBytes, System.Drawing.Rectangle crop, ImageFormat format)
        {
            using (var streamOriginal = new MemoryStream(originalBytes))
            using (var imgOriginal = Image.FromStream(streamOriginal))
            {
                //get original width and height of the incoming image
                var width = imgOriginal.Width; // 1000
                var height = imgOriginal.Height; // 800

                crop.Width = (width < crop.Width) ? width : crop.Width;
                crop.Height = (height < crop.Height) ? height : crop.Height;

                ///////////////////////////////////////////////////
                /// Determines the best crop coordinates
                /// Library cloned from here:
                // https://github.com/softawaregmbh/smartcrop.net
                var cropCoords = new ImageCrop(crop.Width, crop.Height).Crop(originalBytes);
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
                    cropCoords.Area.X,
                    cropCoords.Area.Y,
                    cropCoords.Area.Width,
                    cropCoords.Area.Height
                );
                ///////////////////////////////////////////////////


                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Default;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }


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

        public void AddBookmark(Bookmark bookmark)
        {
            bookmarkRepository.Add(bookmark);
        }

        public void RemoveBookmark(Bookmark bookmark)
        {
            bookmarkRepository.Remove(bookmark);
        }

        public IQueryable<Bookmark> GetBookmarks(Article article, ApplicationUser user)
        {
            return bookmarkRepository.GetBookmarks(article, user);
        }

        public IQueryable<Bookmark> GetBookmarks(Article article)
        {
            return bookmarkRepository.GetBookmarks(article);
        }

        public IQueryable<Bookmark> GetBookmarks(ApplicationUser user)
        {
            return bookmarkRepository.GetBookmarks(user);
        }

        public IQueryable<Article> GetArticleForAdmin()
        {
            return articleRepository.GetArticlesForAdmin();
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return tagRepository.GetAll();
        }

        public void CreateTag(Tag tag)
        {
            tagRepository.Add(tag);
        }

        public bool DeleteTag(int id)
        {
            Tag tag = tagRepository.GetByID(id);
            if (!tag.IsActive)
            {
                return false;
            }
            tag.IsActive = false;
            tagRepository.UpdateTag(tag);
            return true;
        }
        public IQueryable<Bookmark> GetBookmarks(ApplicationUser user, bool includeArticle)
        {
            return bookmarkRepository.GetBookmarks(user, true);
        }

        public async Task UploadToS3(string credAddr, string bucketName, byte[] file, string fileName)
        {
            List<string> keys = new List<string>();
            using (var reader = new StreamReader(credAddr))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('=');

                    keys.Add(values[1]);
                }
            }

            var credentials = new BasicAWSCredentials(keys[0], keys[1]);
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };

            using var client = new AmazonS3Client(credentials, config);

            await using var newMemoryStream = new MemoryStream(file);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = fileName,
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
        //public bool DeleteArticlesByAdmin(Guid id)
        //{
        //    Article article = articleRepository.GetArticleInfo(id);
        //    if (article != null)
        //    {
        //        Status deleteStatus = statusRepository.GetStatus("Deleted");
        //        article.Status = deleteStatus;
        //        articleRepository.UpdateArticle(article);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
